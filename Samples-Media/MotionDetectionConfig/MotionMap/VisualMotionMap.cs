using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// ==========================================================================
// Copyright (C) 2011 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for March 15:
//  1564 – Mughal Emperor Akbar abolishes jizya (per capita tax) .
//  1672 – Charles II of England issues the Royal Declaration of Indulgence.
//  1990 – Iraq hangs British journalist Farzad Bazoft for spying.
// ==========================================================================
namespace MotionDetectionConfig.MotionMap
{
    #region Classes

    public class VisualMotionMap : FrameworkElement, IDisposable
    {
        #region Constants

        public static readonly Brush ClearColor = Brushes.Transparent;

        public static readonly Brush FillColor = Brushes.DodgerBlue;

        public static readonly Brush MotionColor = Brushes.Green;

        public static readonly Brush MotionOnColor = Brushes.Red;

        #endregion

        #region Fields

        /// <summary>
        /// The total blocks count
        /// </summary>
        private int m_blocksCount;

        /// <summary>
        /// The height of a block in pixels
        /// </summary>
        private double m_blocksHeight;

        /// <summary>
        /// An array of 0s and 1s representing the map
        /// </summary>
        private BitArray m_blocksMap;

        /// <summary>
        /// The width of a block in pixels
        /// </summary>
        private double m_blocksWidth;

        /// <summary>
        /// The collection of visual representing the map
        /// </summary>
        private VisualCollection m_map;

        /// <summary>
        /// An array of 0s and 1s representing the motion map
        /// </summary>
        private BitArray m_motionMap;

        /// <summary>
        /// An array of 0s and 1s representing the blocks of the map where motion is on
        /// </summary>
        private BitArray m_motionOnMap;

        /// <summary>
        /// The visual used to display the map
        /// </summary>
        private DrawingVisual m_visual;

        /// <summary>
        /// The width of the map in blocks
        /// </summary>
        private int m_xBlocksCount;

        /// <summary>
        /// The height of the map in blocks
        /// </summary>
        private int m_yBlocksCount;

        #endregion

        #region Properties

        /// <summary>
        /// Provides a required override for the VisualChildrenCount property.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return (m_map != null) ? m_map.Count : 0; }
        }

        #endregion

        #region Constructors

        public VisualMotionMap()
        {
            m_map = new VisualCollection(this);
            m_visual = new DrawingVisual { Opacity = 0.4 };
            m_map.Add(m_visual);

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Destructors and Dispose Methods

        public void Dispose()
        {
            SizeChanged -= OnSizeChanged;

            m_blocksMap = null;
            m_map = null;
            m_motionMap = null;
            m_motionOnMap = null;
            m_visual = null;
        }

        #endregion

        #region Event Handlers

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((m_xBlocksCount > 0) && (m_yBlocksCount > 0))
            {
                m_blocksWidth = (ActualWidth / m_xBlocksCount);
                m_blocksHeight = (ActualHeight / m_yBlocksCount);
            }

            //In case we had something painted, re paint it
            PaintVisualMap(m_blocksMap, m_motionMap, m_motionOnMap);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes all the painted blocks from the map
        /// </summary>
        public void Clear()
        {
            DrawMotion(new BitArray(m_blocksCount));
        }

        /// <summary>
        /// Draws motion block according to the provided array of blocks
        /// </summary>
        public void DrawMotion(BitArray blocks)
        {
            DrawMotion(blocks, null, null);
        }

        /// <summary>
        /// Draws motion according to the provided arrays
        /// </summary>
        /// <param name="blocks">Array representing the map of possible blocks</param>
        /// <param name="motionBlocks">Array representing the map of motion blocks of the mask</param>
        /// <param name="motionOnBlocks">Array representing the map of motion blocks that currently have motion</param>
        public void DrawMotion(BitArray blocks, BitArray motionBlocks, BitArray motionOnBlocks)
        {
            PaintVisualMap(blocks, motionBlocks, motionOnBlocks);
        }

        /// <summary>
        /// Draws motion according to the provided arrays
        /// </summary>
        /// <param name="blocks">Array representing the map of possible blocks</param>
        /// <param name="motionBlocks">Array representing the map of motion blocks of the mask</param>
        public void DrawMotion(BitArray blocks, BitArray motionBlocks)
        {
            DrawMotion(blocks, motionBlocks, null);
        }

        /// <summary>
        /// Fills the map with blocks
        /// </summary>
        public void Fill()
        {
            BitArray bitArray = new BitArray(m_blocksCount);
            bitArray.SetAll(true);
            DrawMotion(bitArray);
        }

        /// <summary>
        /// Gets the index of the block located at the provided position on the screen (related to this control)
        /// </summary>
        public int GetIndex(Point point)
        {
            int yIndex = (int)(point.Y / m_blocksHeight);
            int xIndex = (int)(point.X / m_blocksWidth);

            int index = (yIndex * m_xBlocksCount) + xIndex;

            return (index < m_blocksCount) ? index : -1;
        }

        /// <summary>
        /// Gets the index of the block located at the provided MouseEventArgs origin position
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public int GetIndex(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);

            return GetIndex(pt);
        }

        /// <summary>
        /// Gets the position on the screen (related to this control) of the block at the provided index
        /// </summary>
        public Point GetPosition(int index)
        {
            int xPos = (int)((index % m_xBlocksCount) * m_blocksWidth);
            int yPos = (int)((index / m_xBlocksCount) * m_blocksHeight);

            return new Point(xPos, yPos);
        }

        /// <summary>
        /// Initializes the control with the provided map size
        /// </summary>
        /// <param name="mapWidth">The width of the map in blocks</param>
        /// <param name="mapHeight">The height of the map in blocks</param>
        public void Initialize(int mapWidth, int mapHeight)
        {
            m_blocksWidth = (ActualWidth / mapWidth);
            m_blocksHeight = (ActualHeight / mapHeight);
            m_blocksCount = (mapWidth * mapHeight);
            m_xBlocksCount = mapWidth;
            m_yBlocksCount = mapHeight;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Provides a required override for the GetVisualChild method.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= m_map.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return m_map[index];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Paints the visual map with the right colors according the the provided arrays
        /// </summary>
        /// <param name="blocksMap">The array representing the possible blocks locations</param>
        /// <param name="motionMap">The array representing the motion blocks location</param>
        /// <param name="motionOnMap">The array representing the motion blocks location where motion is on</param>
        private void PaintVisualMap(BitArray blocksMap, BitArray motionMap, BitArray motionOnMap)
        {
            m_blocksMap = blocksMap;
            m_motionMap = motionMap;
            m_motionOnMap = motionOnMap;

            if ((blocksMap == null))
            {
                return;
            }

            if ((motionMap != null) && (blocksMap.Length != motionMap.Length) ||
                (motionOnMap != null) && (blocksMap.Length != motionOnMap.Length))
            {
                return;
            }

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = m_visual.RenderOpen();

            Rect rect = new Rect(new Size(m_blocksWidth, m_blocksHeight));
            for (int i = 0; i < blocksMap.Length; ++i)
            {
                int xIndex = i % m_xBlocksCount;
                int yIndex = i / m_xBlocksCount;

                double xPos = xIndex * m_blocksWidth;
                double yPos = yIndex * m_blocksHeight;
                rect.Location = new Point(xPos, yPos);

                if (blocksMap.Get(i))
                {
                    //If the block exists
                    if ((motionOnMap != null) && motionOnMap.Get(i))
                    {
                        //If the block is detecting real motion (respecting the motion configuration)
                        drawingContext.DrawRectangle(MotionOnColor, null, rect);
                    }
                    else if ((motionMap != null) && motionMap.Get(i))
                    {
                        //If the block is detecting motion
                        drawingContext.DrawRectangle(MotionColor, null, rect);
                    }
                    else
                    {
                        //If the block is part of the mask
                        drawingContext.DrawRectangle(FillColor, null, rect);
                    }
                }
                else
                {
                    //If the block doesn't exist
                    drawingContext.DrawRectangle(ClearColor, null, rect);
                }
            }

            drawingContext.Close();
        }

        #endregion
    }

    #endregion
}


