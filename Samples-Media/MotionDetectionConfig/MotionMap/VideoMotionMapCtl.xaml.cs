using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Genetec.Sdk.Entities.Video.MotionDetection;
using Genetec.Sdk.Media;
using MediaPlayer = Genetec.Sdk.Media.MediaPlayer;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 7:
//  1907 – Delta Sigma Pi is founded at New York University.
//  1996 – NASA launches the Mars Global Surveyor.
//  2002 – Iran bans advertising of United States products.
// ==========================================================================
namespace MotionDetectionConfig.MotionMap
{
    #region Enumerations

    public enum VideoMotionMapDrawingMode
    {
        /// <summary>
        /// Pen is used to draw a motion mask points by points on top of video
        /// </summary>
        Pen = 0,

        /// <summary>
        /// Rectangle is used to draw a motion mask rectangles on top of video
        /// </summary>
        Rectangle,

        /// <summary>
        /// Eraser is used to erase a motion mask already on top of video
        /// </summary>
        Eraser,

        /// <summary>
        /// None is used when we do not want to draw the motion mask
        /// </summary>
        None,
    }

    #endregion

    #region Classes

    public sealed class MotionMap
    {
        #region Properties

        /// <summary>
        /// Gets or Sets a value representing the height of the map in motion blocks
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets a value representing the total block count of the map (Height x Width)
        /// </summary>
        public int TotalBlockCount
        {
            get { return Values.Length; }
        }

        /// <summary>
        /// Gets a value representing the total count of block that are set/have motion on the map
        /// </summary>
        public int TotalMotionBlockCount
        {
            get
            {
                int blockCount = 0;
                for (int i = 0; i < Values.Length; i++)
                {
                    if (Values.Get(i))
                    {
                        blockCount++;
                    }
                }
                return blockCount;
            }
        }

        /// <summary>
        /// Gets an array of 0 and 1 representing the values of the map
        /// </summary>
        public BitArray Values { get; private set; }

        /// <summary>
        /// Gets or Sets a value representing the width of the map in motion blocks
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Constructors

        public MotionMap(int width, int height)
            : this(width, height, new BitArray(width * height))
        {
        }

        public MotionMap(int width, int height, BitArray values)
        {
            Width = width;
            Height = height;
            Values = values;
        }

        public MotionMap(MotionMap otherMotionMap)
        {
            Width = otherMotionMap.Width;
            Height = otherMotionMap.Height;
            Values = otherMotionMap.Values;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies a source map to a new target map. If the maps are not the same size, the best possible equivalent is created
        /// </summary>
        public static bool ConverMotionMap(MotionMap sourceMap, ref MotionMap targetMap)
        {
            bool success;

            if ((sourceMap.Height == 0) || (sourceMap.Width == 0) || (sourceMap.TotalBlockCount == 0))
            {
                // Source is empty so we will empty our mask
                targetMap.Reset();
                return true;
            }

            if ((targetMap.Height == 0) || (targetMap.Width == 0))
            {
                targetMap = sourceMap;
                return false;
            }

            int deltaHeight = targetMap.Height - sourceMap.Height;
            int deltaWidth = targetMap.Width - sourceMap.Width;

            // Check if they are the same size
            if ((deltaHeight == 0) && (deltaWidth == 0))
            {
                targetMap = sourceMap;
                success = true;
            }
            // Check if the target is bigger than the source
            else if ((deltaHeight >= 0) && (deltaWidth >= 0))
            {
                targetMap.Reset();

                double widthRatio = (double)targetMap.Width / sourceMap.Width;
                double heightRatio = (double)targetMap.Height / sourceMap.Height;

                for (int x = 0; x < sourceMap.Width; x++)
                {
                    for (int y = 0; y < sourceMap.Height; y++)
                    {
                        if (sourceMap.IsMotion(x, y))
                        {
                            int xPosNormalize = (int)(widthRatio * x);
                            int yPosNormalize = (int)(heightRatio * y);
                            int maxXPos = (int)((widthRatio * (x + 1)) - 1);
                            int maxYPos = (int)((heightRatio * (y + 1)) - 1);

                            for (int xOut = xPosNormalize; xOut <= maxXPos; xOut++)
                            {
                                for (int yOut = yPosNormalize; yOut <= maxYPos; yOut++)
                                {
                                    targetMap.SetMotion(xOut, yOut, true);
                                }
                            }
                        }
                    }
                }

                success = true;
            }
            // Check if the target is smaller than source
            else if ((deltaHeight <= 0) && (deltaWidth <= 0))
            {
                targetMap.Reset();

                double widthRatio = (double)sourceMap.Width / targetMap.Width;
                double heightRatio = (double)sourceMap.Height / targetMap.Height;

                for (int x = 0; x < targetMap.Width; x++)
                {
                    for (int y = 0; y < targetMap.Height; y++)
                    {
                        bool isMotion = false;
                        int xPosNormalize = (int)(widthRatio * x);
                        int yPosNormalize = (int)(heightRatio * y);
                        int maxXPos = (int)((widthRatio * (x + 1)) - 1);
                        int maxYPos = (int)((heightRatio * (y + 1)) - 1);

                        // Go throught all blocks in the source that represent one block in the target 
                        // and as soon as we have a motion in one them, we set the motion on in the target
                        for (int xSource = xPosNormalize; xSource <= maxXPos; xSource++)
                        {
                            for (int ySource = yPosNormalize; ySource <= maxYPos; ySource++)
                            {
                                if (sourceMap.IsMotion(xSource, ySource))
                                {
                                    targetMap.SetMotion(x, y, true);
                                    isMotion = true;
                                    break;
                                }
                            }
                            if (isMotion)
                                break;
                        }
                    }
                }

                success = true;
            }
            else
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Checks if the block is set at the provided coordinate
        /// </summary>
        public bool IsMotion(int x, int y)
        {
            return Values.Get((x + y * Width));
        }

        /// <summary>
        /// Reset all the values to 0
        /// </summary>
        public void Reset()
        {
            Values.SetAll(false);
        }

        /// <summary>
        /// Sets all the values to the provided value
        /// </summary>
        public void SetAll(bool value)
        {
            Values.SetAll(value);
        }

        /// <summary>
        /// Sets the block at the provided index with the provided value
        /// </summary>
        public void SetMotion(int index, bool isMotionOn)
        {
            Values.Set(index, isMotionOn);
        }

        /// <summary>
        /// Sets the block at the provided coordinate with the provided value
        /// </summary>
        public void SetMotion(int x, int y, bool isMotionOn)
        {
            Values.Set((Width * y + x), isMotionOn);
        }

        #endregion
    }

    public sealed class MotionMaskChangedEventArgs : EventArgs
    {
        #region Constants

        private readonly int m_nBlocks;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of blocks of the motion mask
        /// </summary>
        public int Blocks
        {
            get { return m_nBlocks; }
        }

        #endregion

        #region Constructors

        public MotionMaskChangedEventArgs(int nBlocks)
        {
            m_nBlocks = nBlocks;
        }

        #endregion
    }

    /// <summary>
    /// Interaction logic for VideoMotionMapCtl.xaml
    /// </summary>
    public partial class VideoMotionMapCtl : UserControl
    {
        #region Constants

        public static readonly DependencyProperty AreIrregularZoneShapeSupportedProperty =
            DependencyProperty.Register
            ("AreIrregularZoneShapeSupported", typeof(bool), typeof(VideoMotionMapCtl),
            new PropertyMetadata(false));

        #endregion

        #region Fields

        /// <summary>
        /// Mask values set externally
        /// </summary>
        private MotionMap m_externalMask;

        /// <summary>
        /// The index of the first block where mouse interaction occurred
        /// </summary>
        private int m_firstIndexSelected;

        /// <summary>
        /// Mask values used internally
        /// </summary>
        private MotionMap m_internalMask;

        /// <summary>
        /// Flag indicating if the mouse is being dragged
        /// </summary>
        private bool m_isDragging;

        /// <summary>
        /// Flag indicating if operations are being inverted (using right mouse button)
        /// </summary>
        private bool m_isInverted;

        /// <summary>
        /// The index of the last block where mouse interaction occurred
        /// </summary>
        private int m_lastIndexSelected;

        /// <summary>
        /// The instance of the media player used to display the video on which the map is drawn
        /// </summary>
        private MediaPlayer m_mediaPlayer;

        /// <summary>
        /// Values representing the rectangle that is being drawn over the video
        /// </summary>
        private BitArray m_rectangleDrawing;

        #endregion

        #region Properties

        /// <summary>
        /// Value representing the heigh of the map in pixels
        /// </summary>
        public double ActualMaskHeight { get; private set; }

        /// <summary>
        /// Value representing the width of the map in pixels
        /// </summary>
        public double ActualMaskWidth { get; private set; }

        /// <summary>
        /// Flag indicating if only rectangular zones are supported
        /// </summary>
        public bool AreIrregularZoneShapeSupported
        {
            get { return (bool)GetValue(AreIrregularZoneShapeSupportedProperty); }
            set { SetValue(AreIrregularZoneShapeSupportedProperty, value); }
        }

        /// <summary>
        /// Value representing the current drawing mode
        /// </summary>
        public VideoMotionMapDrawingMode DrawingMode { get; set; }

        /// <summary>
        /// Value representing the motion map drawn over the video
        /// </summary>
        public MotionMap Mask
        {
            get
            {
                if (m_internalMask != null)
                {
                    return new MotionMap(m_internalMask);
                }

                return null;
            }
        }

        #endregion

        #region Events and Delegates

        public event EventHandler<MotionMaskChangedEventArgs> MotionMaskChanged;

        #endregion

        #region Constructors

        public VideoMotionMapCtl()
        {
            InitializeComponent();
            SubscribeVisualMotionMap();
        }

        #endregion

        #region Event Handlers

        private void OnMediaPlayerVideoDimensionRefreshed(object sender, VideoDimensionRefreshedEventArgs e)
        {
            //Set the mask size to be the same as the stream resolution
            ActualMaskHeight = m_mediaPlayer.StreamDimension.Height;
            ActualMaskWidth = m_mediaPlayer.StreamDimension.Width;
            UpdateMotionMask();
        }

        private void OnMotionMaskChanged(int blockCount)
        {
            if (m_visualMotionMap != null)
            {
                //Draw the new changes
                m_visualMotionMap.DrawMotion(m_internalMask.Values);
            }

            if (MotionMaskChanged != null)
            {
                MotionMaskChanged(this, new MotionMaskChangedEventArgs(blockCount));
            }
        }

        private void OnVideoHostSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateMotionMask();
        }

        private void OnVisualMotionMapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DrawingMode == VideoMotionMapDrawingMode.None)
            {
                return;
            }

            if (!AreIrregularZoneShapeSupported)
            {
                //If only rectangular zones are supported, always clear the mask when the user begins drawing
                ClearMask();
            }

            m_isDragging = true;
            m_isInverted = false;

            int index = m_visualMotionMap.GetIndex(e);

            StartDrawing(index);
        }

        private void OnVisualMotionMapMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StopDrawing();
        }

        private void OnVisualMotionMapMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isDragging)
            {
                Point position = e.GetPosition(m_visualMotionMap);
                if ((position.X <= 0) || (position.X >= m_visualMotionMap.ActualWidth) ||
                   (position.Y <= 0) || (position.Y >= m_visualMotionMap.ActualHeight))
                {
                    //If for some reason the mouse is outside the map, ignore it
                    return;
                }

                int index = m_visualMotionMap.GetIndex(e);
                if ((m_lastIndexSelected == index) || (index < 0))
                {
                    //If the cursor is still pointing at the same index than when it started, ignore it
                    return;
                }

                m_lastIndexSelected = index;
                switch (DrawingMode)
                {
                    case VideoMotionMapDrawingMode.Rectangle:
                        SelectMotionBlocksBetweenPoints();
                        break;

                    case VideoMotionMapDrawingMode.Pen:
                        SetMotionBlock(index);
                        break;

                    case VideoMotionMapDrawingMode.Eraser:
                        ResetMotionBlock(index);
                        break;
                }
            }
        }

        private void OnVisualMotionMapMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DrawingMode == VideoMotionMapDrawingMode.None)
            {
                return;
            }

            if (!AreIrregularZoneShapeSupported)
            {
                //If only rectangular zones are supported, always clear the mask when the user begins drawing
                ClearMask();
            }

            m_isDragging = true;
            m_isInverted = true;

            int index = m_visualMotionMap.GetIndex(e);

            StartDrawing(index);
        }

        private void OnVisualMotionMapMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            StopDrawing();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets all the values of the mask to 0
        /// </summary>
        public void ClearMask()
        {
            if (m_internalMask == null)
            {
                //If mask is null, no need to clear it
                return;
            }
            m_internalMask.SetAll(false);
            OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
        }

        /// <summary>
        /// Sets all the values of the mask to 1
        /// </summary>
        public void FillMask()
        {
            m_internalMask.SetAll(true);
            OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
        }

        /// <summary>
        /// Hide the video from the control
        /// </summary>
        public void HideVideo()
        {
            m_videoHost.Content = null;
        }

        /// <summary>
        /// Initializes the control with the instance of the media player used to display the video
        /// </summary>
        /// <param name="mediaPlayer">The instance of the media player</param>
        public void Initialize(MediaPlayer mediaPlayer)
        {
            m_mediaPlayer = mediaPlayer;
            m_mediaPlayer.HardwareAccelerationEnabled = true;
            //Set the mask size to be the same as the stream resolution
            ActualMaskHeight = mediaPlayer.StreamDimension.Height;
            ActualMaskWidth = mediaPlayer.StreamDimension.Width;

            //Subscribe to the events of the media player
            SubscribeMediaPlayer();
        }

        /// <summary>
        /// Inverts the values of the mask, 0s becoming 1s and 1s becoming 0s
        /// </summary>
        public void InvertMask()
        {
            for (int i = 0; i < m_internalMask.TotalBlockCount; ++i)
            {
                m_internalMask.SetMotion(i, !m_internalMask.Values.Get(i));
            }

            OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
        }

        /// <summary>
        /// Sets the values of the mask to be the same as the ones from the provided motion detectiion zone configuration
        /// </summary>
        /// <param name="zone">The motion detection zone configuration to be copied</param>
        public void SetMask(IMotionDetectionZoneConfiguration zone)
        {
            m_externalMask = new MotionMap(zone.MapWidth, zone.MapHeight, zone.Map);

            UpdateMotionMask();
        }

        /// <summary>
        /// Sets the values of mask according to the provided parameters
        /// </summary>
        /// <param name="width">The width of the mask in motion blocks</param>
        /// <param name="height">The height of the mask in motion blocks</param>
        /// <param name="values">The values of the mask</param>
        public void SetMask(int width, int height, BitArray values)
        {
            if (values.Count != width * height)
            {
                values = new BitArray(width * height);
            }

            m_externalMask = new MotionMap(width, height, values);
            UpdateMotionMask();
        }

        /// <summary>
        /// Displays the video in the control
        /// </summary>
        public void ShowVideo()
        {
            m_videoHost.Content = m_mediaPlayer;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the internal mask to fit the provide width and height
        /// </summary>
        private void InitializeInternalMask(int nbBlocksWidth, int nbBlocksHeight)
        {
            // Current Mask is not valid or does not exist
            if (m_internalMask == null || m_internalMask.Height != nbBlocksHeight || m_internalMask.Width != nbBlocksWidth)
            {
                //Set new mask
                m_internalMask = new MotionMap(nbBlocksWidth, nbBlocksHeight);
                m_visualMotionMap.DrawMotion(m_internalMask.Values);
            }
        }

        /// <summary>
        /// Initializes the motion mask with the provided width and height values
        /// </summary>
        private void InitializeMotionMask(int nbBlocksWidth, int nbBlocksHeight)
        {
            // Initialize our internal mask
            InitializeInternalMask(nbBlocksWidth, nbBlocksHeight);

            // Use the mask set from external if it has the same size
            if (m_externalMask != null)
            {
                if (m_externalMask.Height == m_internalMask.Height && m_externalMask.Width == m_internalMask.Width)
                {
                    m_internalMask = m_externalMask;
                    m_externalMask = null;

                    // Allow value update for listeners.
                    OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
                }
                else
                {
                    MotionMap map = new MotionMap(m_internalMask.Width, m_internalMask.Height);

                    if (MotionMap.ConverMotionMap(m_externalMask, ref map))
                    {
                        m_internalMask = map;
                        m_externalMask = null;

                        // Allow value update for listeners.
                        OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the motion block at the provided index
        /// </summary>
        private void ResetMotionBlock(int index)
        {
            if (!m_isInverted)
                TurnOffMotionBlock(index);
            else
                TurnOnMotionBlock(index);
        }

        /// <summary>
        /// Selects all the motion blocks in between the starting and ending indexes. This is used to delimeter a rectangle
        /// </summary>
        private void SelectMotionBlocksBetweenPoints()
        {
            if (m_firstIndexSelected > -1 && m_lastIndexSelected > -1)
            {
                int startPosX = m_firstIndexSelected % m_internalMask.Width;
                int stopPosX = m_lastIndexSelected % m_internalMask.Width;
                int startPosY = m_firstIndexSelected / m_internalMask.Width;
                int stopPosY = m_lastIndexSelected / m_internalMask.Width;

                // Flip X points if they are inversed
                if (stopPosX < startPosX)
                {
                    int temp = startPosX;
                    startPosX = stopPosX;
                    stopPosX = temp;
                }

                // Flip Y points if they are inversed
                if (stopPosY < startPosY)
                {
                    int temp = startPosY;
                    startPosY = stopPosY;
                    stopPosY = temp;
                }

                bool isDirty = false;
                m_rectangleDrawing = new BitArray(m_internalMask.Values);

                //Go through all the values of the mask to see if it changed
                for (int i = 0; i < m_rectangleDrawing.Length; ++i)
                {
                    int xPos = i % m_internalMask.Width;
                    int yPos = i / m_internalMask.Width;

                    if ((xPos >= startPosX) && (xPos <= stopPosX) && (yPos >= startPosY) && (yPos <= stopPosY))
                    {
                        if (!isDirty)
                        {
                            if (m_rectangleDrawing.Get(i) != !m_isInverted)
                            {
                                isDirty = true;
                            }
                        }
                        m_rectangleDrawing.Set(i, !m_isInverted);
                    }
                }

                if (isDirty)
                {
                    m_visualMotionMap.DrawMotion(m_rectangleDrawing);
                }
            }
        }

        /// <summary>
        /// Sets the motion block at the provided index
        /// </summary>
        private void SetMotionBlock(int index)
        {
            if (m_isInverted)
                TurnOffMotionBlock(index);
            else
                TurnOnMotionBlock(index);
        }

        /// <summary>
        /// Starts drawing at the provided index
        /// </summary>
        private void StartDrawing(int index)
        {
            m_visualMotionMap.CaptureMouse();
            switch (DrawingMode)
            {
                case VideoMotionMapDrawingMode.Rectangle:
                    m_firstIndexSelected = index;
                    break;

                case VideoMotionMapDrawingMode.Pen:
                    SetMotionBlock(index);
                    break;

                case VideoMotionMapDrawingMode.Eraser:
                    ResetMotionBlock(index);
                    break;
            }
        }

        /// <summary>
        /// Stops drawing since mouse was released
        /// </summary>
        private void StopDrawing()
        {
            if (m_rectangleDrawing != null)
            {
                bool isDirty = false;
                //Go through all the values of the mask to see if it changed
                for (int i = 0; i < m_internalMask.TotalBlockCount; ++i)
                {
                    if (!isDirty)
                    {
                        if (m_rectangleDrawing.Get(i) != m_internalMask.Values.Get(i))
                        {
                            isDirty = true;
                        }
                    }

                    m_internalMask.SetMotion(i, m_rectangleDrawing.Get(i));
                }

                if (isDirty)
                {
                    OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
                }
            }

            m_rectangleDrawing = null;
            m_isDragging = false;
            m_isInverted = false;

            m_firstIndexSelected = -1;
            m_lastIndexSelected = -1;

            m_visualMotionMap.ReleaseMouseCapture();
        }

        /// <summary>
        /// Subscribes to the media player events
        /// </summary>
        private void SubscribeMediaPlayer()
        {
            if (m_mediaPlayer != null)
            {
                m_mediaPlayer.VideoDimensionRefreshed += OnMediaPlayerVideoDimensionRefreshed;
            }
        }

        /// <summary>
        /// Subscribes to the visual motion map events used to draw zones
        /// </summary>
        private void SubscribeVisualMotionMap()
        {
            if (m_visualMotionMap != null)
            {
                m_visualMotionMap.MouseMove += OnVisualMotionMapMouseMove;
                m_visualMotionMap.MouseLeftButtonDown += OnVisualMotionMapMouseLeftButtonDown;
                m_visualMotionMap.MouseLeftButtonUp += OnVisualMotionMapMouseLeftButtonUp;
                m_visualMotionMap.MouseRightButtonDown += OnVisualMotionMapMouseRightButtonDown;
                m_visualMotionMap.MouseRightButtonUp += OnVisualMotionMapMouseRightButtonUp;
            }
        }

        /// <summary>
        /// Sets the block at the provided index to 0
        /// </summary>
        private void TurnOffMotionBlock(int index)
        {
            if (m_internalMask.Values.Get(index))
            {
                m_internalMask.SetMotion(index, false);

                OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
            }
        }

        /// <summary>
        /// Sets the block at the provided index to 1
        /// </summary>
        private void TurnOnMotionBlock(int index)
        {
            if (!m_internalMask.Values.Get(index))
            {
                m_internalMask.SetMotion(index, true);

                OnMotionMaskChanged(m_internalMask.TotalMotionBlockCount);
            }
        }

        /// <summary>
        /// Updates the motion mask according to its new size and values
        /// </summary>
        private void UpdateMotionMask()
        {
            if (m_mediaPlayer != null)
            {
                double width = m_mediaPlayer.RenderingDimension.Width;
                double height = m_mediaPlayer.RenderingDimension.Height;

                if ((width > 0) && (height > 0))
                {
                    double ratio = width / height;

                    //Respect the stream's ratio when setting the size of the visualMotionMap
                    if (ratio > (m_videoHost.ActualWidth / m_videoHost.ActualHeight))
                    {
                        m_visualMotionMap.Width = m_videoHost.ActualWidth;
                        m_visualMotionMap.Height = m_visualMotionMap.Width / ratio;
                    }
                    else if (ratio < (m_videoHost.ActualWidth / m_videoHost.ActualHeight))
                    {
                        m_visualMotionMap.Height = m_videoHost.ActualHeight;
                        m_visualMotionMap.Width = m_visualMotionMap.Height * ratio;
                    }
                    else
                    {
                        m_visualMotionMap.Width = m_videoHost.ActualWidth;
                        m_visualMotionMap.Height = m_videoHost.ActualHeight;
                    }

                    int macroBlockWidth, macroBlockHeight;

                    //PAL
                    if (ratio <= (176d / 144d))
                    {
                        macroBlockWidth = 44;
                        macroBlockHeight = 36;
                    }
                    else
                    {
                        macroBlockWidth = 44;
                        macroBlockHeight = 30;
                    }

                    m_visualMotionMap.Initialize(macroBlockWidth, macroBlockHeight);
                    InitializeMotionMask(macroBlockWidth, macroBlockHeight);
                }
            }
        }

        #endregion
    }

    #endregion
}

