// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DronesTracker.Animations
{
    internal static class AnimationCache
    {
        #region Private Fields

        private static readonly Dictionary<CacheKey, ObjectAnimationUsingKeyFrames> _animationCache = new Dictionary<CacheKey, ObjectAnimationUsingKeyFrames>();

        private static readonly Dictionary<CacheKey, int> ReferenceCount = new Dictionary<CacheKey, int>();

        #endregion Private Fields

        #region Public Methods

        public static void AddAnimation(ImageSource source, RepeatBehavior repeatBehavior, ObjectAnimationUsingKeyFrames animation)
        {
            var key = new CacheKey(source, repeatBehavior);
            _animationCache[key] = animation;
        }

        public static void DecrementReferenceCount(ImageSource source, RepeatBehavior repeatBehavior)
        {
            var cacheKey = new CacheKey(source, repeatBehavior);
            ReferenceCount.TryGetValue(cacheKey, out var count);
            if (count > 0)
            {
                count--;
                ReferenceCount[cacheKey] = count;
            }

            if (count != 0) return;
            _animationCache.Remove(cacheKey);
            ReferenceCount.Remove(cacheKey);
        }

        public static ObjectAnimationUsingKeyFrames GetAnimation(ImageSource source, RepeatBehavior repeatBehavior)
        {
            var key = new CacheKey(source, repeatBehavior);
            _animationCache.TryGetValue(key, out var animation);
            return animation;
        }

        public static void IncrementReferenceCount(ImageSource source, RepeatBehavior repeatBehavior)
        {
            var cacheKey = new CacheKey(source, repeatBehavior);
            ReferenceCount.TryGetValue(cacheKey, out var count);
            count++;
            ReferenceCount[cacheKey] = count;
        }

        public static void RemoveAnimation(ImageSource source, RepeatBehavior repeatBehavior, ObjectAnimationUsingKeyFrames animation)
        {
            var key = new CacheKey(source, repeatBehavior);
            _animationCache.Remove(key);
        }

        #endregion Public Methods

        #region Private Classes

        private class CacheKey
        {
            #region Private Fields

            private readonly RepeatBehavior m_repeatBehavior;
            private readonly ImageSource m_source;

            #endregion Private Fields

            #region Public Constructors

            public CacheKey(ImageSource source, RepeatBehavior repeatBehavior)
            {
                m_source = source;
                m_repeatBehavior = repeatBehavior;
            }

            #endregion Public Constructors

            #region Public Methods

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((CacheKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (ImageGetHashCode(m_source) * 397) ^ m_repeatBehavior.GetHashCode();
                }
            }

            #endregion Public Methods

            #region Private Methods

            private static Uri GetUri(ImageSource image)
            {
                if (image is BitmapImage bmp && bmp.UriSource != null)
                {
                    if (bmp.UriSource.IsAbsoluteUri)
                        return bmp.UriSource;
                    if (bmp.BaseUri != null)
                        return new Uri(bmp.BaseUri, bmp.UriSource);
                }

                if (!(image is BitmapFrame frame)) return null;
                var s = frame.ToString();
                if (s == frame.GetType().FullName) return null;
                if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out var fUri)) return null;
                if (fUri.IsAbsoluteUri)
                    return fUri;
                return frame.BaseUri != null ? new Uri(frame.BaseUri, fUri) : null;
            }

            private static bool ImageEquals(ImageSource x, ImageSource y)
            {
                if (Equals(x, y))
                    return true;
                if ((x == null) != (y == null))
                    return false;
                // They can't both be null or Equals would have returned true
                // and if any is null, the previous would have detected it
                // ReSharper disable PossibleNullReferenceException
                if (x.GetType() != y.GetType())
                    return false;
                // ReSharper restore PossibleNullReferenceException
                var xUri = GetUri(x);
                var yUri = GetUri(y);
                return xUri != null && xUri == yUri;
            }

            private static int ImageGetHashCode(ImageSource image)
            {
                if (image == null) return 0;
                var uri = GetUri(image);
                return uri != null ? uri.GetHashCode() : 0;
            }

            private bool Equals(CacheKey other)
            {
                return ImageEquals(m_source, other.m_source)
                    && Equals(m_repeatBehavior, other.m_repeatBehavior);
            }

            #endregion Private Methods
        }

        #endregion Private Classes
    }
}