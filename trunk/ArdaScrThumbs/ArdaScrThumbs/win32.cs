using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

/*
 * 
 * Base Class 
 * There is nothing to change here
 * Taken from MSDN mag.
 * 
 * Endless thanks to Ben Ryves' Cogwheel. 
 * 
 */

namespace ZXThumbnailer 
{
        public enum WTS_ALPHATYPE 
        {
                WTSAT_UNKNOWN = 0,
                WTSAT_RGB = 1,
                WTSAT_ARGB = 2,
        }

        [ComVisible(true), Guid("e357fccd-a995-4576-b01f-234630154e96"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IThumbnailProvider 
        {
                void GetThumbnail(int cx, out IntPtr hBitmap, out WTS_ALPHATYPE bitmapType);
        }

        [ComVisible(true), Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithStream 
        {
                void Initialize(IStream stream, int grfMode);
        }
}

