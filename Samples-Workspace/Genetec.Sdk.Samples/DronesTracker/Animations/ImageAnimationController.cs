// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DronesTracker.Animations
{
    /// <summary>
    /// Provides a way to pause, resume or seek a GIF animation.
    /// </summary>
    public class ImageAnimationController : IDisposable
    {
        #region Private Fields

        private static readonly DependencyPropertyDescriptor SourceDescriptor;

        private readonly ObjectAnimationUsingKeyFrames m_animation;

        private readonly AnimationClock m_clock;

        private readonly ClockController m_clockController;

        private readonly Image m_image;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Raised when the current frame changes.
        /// </summary>
        public event EventHandler CurrentFrameChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Returns the current frame index.
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                var time = m_clock.CurrentTime;
                var frameAndIndex =
                    m_animation.KeyFrames
                              .Cast<ObjectKeyFrame>()
                              .Select((f, i) => new { Time = f.KeyTime.TimeSpan, Index = i })
                              .FirstOrDefault(fi => fi.Time >= time);
                if (frameAndIndex != null)
                    return frameAndIndex.Index;
                return -1;
            }
        }

        /// <summary>
        /// Returns the number of frames in the image.
        /// </summary>
        public int FrameCount => m_animation.KeyFrames.Count;

        /// <summary>
        /// Returns a value that indicates whether the animation is complete.
        /// </summary>
        public bool IsComplete => m_clock.CurrentState == ClockState.Filling;

        /// <summary>
        /// Returns a value that indicates whether the animation is paused.
        /// </summary>
        public bool IsPaused => m_clock.IsPaused;

        #endregion Public Properties

        #region Public Constructors

        static ImageAnimationController()
        {
            SourceDescriptor = DependencyPropertyDescriptor.FromProperty(Image.SourceProperty, typeof(Image));
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal ImageAnimationController(Image image, ObjectAnimationUsingKeyFrames animation, bool autoStart)
        {
            m_image = image;
            m_animation = animation;
            m_animation.Completed += AnimationCompleted;
            m_clock = m_animation.CreateClock();
            m_clockController = m_clock.Controller;
            SourceDescriptor.AddValueChanged(image, ImageSourceChanged);

            // ReSharper disable once PossibleNullReferenceException
            m_clockController.Pause();

            m_image.ApplyAnimationClock(Image.SourceProperty, m_clock);

            if (autoStart)
                m_clockController.Resume();
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Disposes the current object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Seeks the animation to the specified frame index.
        /// </summary>
        /// <param name="index">The index of the frame to seek to</param>
        public void GotoFrame(int index)
        {
            var frame = m_animation.KeyFrames[index];
            m_clockController.Seek(frame.KeyTime.TimeSpan, TimeSeekOrigin.BeginTime);
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause()
        {
            m_clockController.Pause();
        }

        /// <summary>
        /// Starts or resumes the animation. If the animation is complete, it restarts from the beginning.
        /// </summary>
        public void Play()
        {
            m_clockController.Resume();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the current object
        /// </summary>
        /// <param name="disposing">true to dispose both managed an unmanaged resources, false to dispose only managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            m_image.BeginAnimation(Image.SourceProperty, null);
            m_animation.Completed -= AnimationCompleted;
            SourceDescriptor.RemoveValueChanged(m_image, ImageSourceChanged);
            m_image.Source = null;
        }

        #endregion Protected Methods

        #region Private Methods

        private void AnimationCompleted(object sender, EventArgs e)
        {
            m_image.RaiseEvent(new System.Windows.RoutedEventArgs(ImageBehavior.AnimationCompletedEvent, m_image));
        }

        private void ImageSourceChanged(object sender, EventArgs e)
        {
            OnCurrentFrameChanged();
        }
        private void OnCurrentFrameChanged()
        {
            var handler = CurrentFrameChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion Private Methods

        #region Private Destructors

        /// <summary>
        /// Finalizes the current object.
        /// </summary>
        ~ImageAnimationController()
        {
            Dispose(false);
        }

        #endregion Private Destructors
    }
}