using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ZXThumbnailer
{

    [ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    [ProgId("APM.SCRThumbs"), Guid("836d9b4f-9333-4d5e-a1bf-149b3741c163")]


    public class ThumbnailProvider : IThumbnailProvider, IInitializeWithStream
    {

        #region init

        protected IStream Stream { get; set; }

        public void Initialize(IStream stream, int grfMode)
        {
            this.Stream = stream;
        }

        protected byte[] GetStreamContents()
        {

            if (this.Stream == null) return null;

            System.Runtime.InteropServices.ComTypes.STATSTG statData;
            this.Stream.Stat(out statData, 1);

            byte[] Result = new byte[statData.cbSize];

            IntPtr P = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(UInt64)));
            try
            {
                this.Stream.Read(Result, Result.Length, P);
            }
            finally
            {
                Marshal.FreeCoTaskMem(P);
            }
            return Result;
        }

        #endregion

        #region getthumb

        public void GetThumbnail(int squareLength, out IntPtr hBitmap, out WTS_ALPHATYPE bitmapType)
        {
            
            hBitmap = IntPtr.Zero;
            bitmapType = WTS_ALPHATYPE.WTSAT_UNKNOWN;

            #region cols
            Color[] pixel = new Color[16];
             
            pixel[0] = Color.FromArgb(255, 0, 0, 0);
            
            pixel[1] = Color.FromArgb(255, 0, 0, 192);
            pixel[2] = Color.FromArgb(255, 192, 0, 0);
            pixel[3] = Color.FromArgb(255, 192, 0, 192);
            pixel[4] = Color.FromArgb(255, 0, 192, 0);
            pixel[5] = Color.FromArgb(255, 0, 192, 192);
            pixel[6] = Color.FromArgb(255, 192, 192, 0);
            pixel[7] = Color.FromArgb(255, 192, 192, 192);
            
            pixel[8] = Color.FromArgb(255, 0, 0, 0);
            pixel[9] = Color.FromArgb(255, 0, 0, 255);
            pixel[10] = Color.FromArgb(255, 255, 0, 0);
            pixel[11] = Color.FromArgb(255, 255, 0, 255);
            pixel[12] = Color.FromArgb(255, 0, 255, 0);
            pixel[13] = Color.FromArgb(255, 0, 255, 255);
            pixel[14] = Color.FromArgb(255, 255, 255, 0);
            pixel[15] = Color.FromArgb(255, 255, 255, 255);
            #endregion

            try
            {
                //Do something with zx images..

                using (var Thumbnail = new Bitmap(256, 192)) //create surface and dispose
                {

                        #region init!?
                    byte[] scrn = new byte [6912];
                        byte[] s = new byte [6912];
                        scrn=this.GetStreamContents();
                    #endregion

                        #region initagain
                         #region diz

                        int addr = 0;

                        
                         for (int l = 0; l <= 2; l++)
                         {
                            for (int j=0; j<=7; j++)
                            {
                                for (int x = (2048*l)+(j*32); x <= (2048 * (l + 1)) - 1; x += 256) 
                                { 
                                    for ( int z=0; z<=31;z++)
                                    {
                                        s[addr] = scrn [x+z];
                                        addr++;
                                    }
                                }
                            }
                         }



                        #endregion

                         #region ciz
                         addr = 0;
                         int lc = 0;
                         int ink = 0;
                         int paper =0;

                         
                    

                         for (int y = 0; y <= 191; y++)
                         {
                             for (int x = 0; x <= 31; x++)
                             {
                                 int bri = 0;
                                 int rng = scrn[6144 + ((int)(y / 8) * 32) + x];
                                 if ((rng & 128) != 0) {rng -=  128;}
                                 if ((rng & 64) != 0) { rng -= 64; bri = 8; }

                                 paper = ((int)(rng / 8));
                                 ink = (rng - (paper * 8)) + bri;
                                 paper += bri;
                                 
                                 lc = x * 8;

                                 switch (s[addr])
                                 {
                                    case 0:
                                         for (int i = 0; i <= 7; i++)
                                         {
                                             Thumbnail.SetPixel(lc + i, y, pixel[paper]);
                                         }
                                         break;
                                     case 255:
                                         for (int i = 0; i <= 7; i++)
                                         {
                                             Thumbnail.SetPixel(lc + i, y, pixel[ink]);
                                         }
                                         break; 
                                     default:

                                          for ( byte z=0; z<=7 ; z++)
                                              {
                                                      byte r = (byte)Math.Pow(2, z);
                                                
                                                      if ((s[addr] & r) == r)
                                                      {  
                                                          Thumbnail.SetPixel(lc + (7 - z), y, pixel[ink]);
                                                      }
                                                      else
                                                      {
                                                          Thumbnail.SetPixel(lc + (7 - z), y, pixel[paper]);
                                                      }
                                                
                                               }
                                           
                                         break;
                                 }
                                 addr++;
                             }

                         }

                         #endregion
#endregion

                        #region draw
                         hBitmap = Thumbnail.GetHbitmap();
                         #endregion

                }
                

            }
            catch 
            { 
                //do nothing. 
            }
        }

        #endregion
    }
}
