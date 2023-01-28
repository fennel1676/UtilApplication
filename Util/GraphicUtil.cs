using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AKI.Util
{
    public class GraphicUtil
    {
        static public Bitmap rotateImage(Bitmap returnBitmap, float fAngle)
        {
            if (null == returnBitmap)
                return null;

            Bitmap bmp = new Bitmap(returnBitmap.Size.Width, returnBitmap.Size.Height);
            Graphics GFX2 = Graphics.FromImage(bmp);
            GFX2.Clear(Color.Transparent);

            float fWidth = (float)returnBitmap.Width / 2;
            float fHeight = (float)returnBitmap.Height / 2;
            GFX2.TranslateTransform(fWidth, fHeight);
            GFX2.RotateTransform(fAngle);
            GFX2.TranslateTransform(-1 * fWidth, -1 * fHeight);
            GFX2.DrawImage(returnBitmap, 0, 0, returnBitmap.Size.Width, returnBitmap.Size.Height);
            return bmp;
        }

        static public Bitmap rotateImage3(Bitmap returnBitmap, float fAngle)
        {
            if (null == returnBitmap)
                return null;

            //create a new empty bitmap to hold rotated image
            Bitmap bmp = new Bitmap(returnBitmap.Size.Width, returnBitmap.Size.Height);
            Graphics GFX2 = Graphics.FromImage(bmp);
            GFX2.Clear(Color.Transparent);

            GFX2.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
            GFX2.RotateTransform(fAngle);
            GFX2.TranslateTransform(-(float)returnBitmap.Width / 2, -(float)returnBitmap.Height / 2);
            GFX2.DrawImage(returnBitmap, new Point(0, 0));

            return bmp;
        }

        static public Bitmap rotateImage2(Bitmap returnBitmap, float fAngle)
        {
            if (null == returnBitmap)
                return null;

            Bitmap bmp = new Bitmap(returnBitmap.Size.Width, returnBitmap.Size.Height);
            Graphics GFX2 = Graphics.FromImage(bmp);
            Matrix matrix = new Matrix();
            Point center = new Point((int)bmp.Width / 2, (int)bmp.Height / 2);
            matrix.RotateAt(fAngle, center);
            GFX2.Transform = matrix;
            GFX2.Clear(Color.Transparent);
            GFX2.DrawImage(returnBitmap, new PointF(0, 0));

            return bmp;
        }
        
        public static void CaptureImage(Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle, string FilePath, string extension)
        {
            using (Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);
                }

                Image img = (Image)bitmap;
                Clipboard.SetImage(img);

                switch (extension)
                {
                    case ".emf": bitmap.Save(FilePath, ImageFormat.Emf); break;
                    case ".bmp": bitmap.Save(FilePath, ImageFormat.Bmp); break;
                    case ".jpg": bitmap.Save(FilePath, ImageFormat.Jpeg); break;
                    case ".gif": bitmap.Save(FilePath, ImageFormat.Gif); break;
                    case ".tiff": bitmap.Save(FilePath, ImageFormat.Tiff); break;
                    case ".png": bitmap.Save(FilePath, ImageFormat.Png); break;
                    default: bitmap.Save(FilePath, ImageFormat.Jpeg); break;
                }
            }
        }

        public static bool CaptureWindow(System.Windows.Forms.Control control, ref System.Drawing.Bitmap bitmap)
        {
            Graphics g2 = Graphics.FromImage(bitmap);

            int meint = (int)(Win32.PRF.CLIENT | Win32.PRF.ERASEBKGND);
            System.IntPtr meptr = new System.IntPtr(meint);

            System.IntPtr hdc = g2.GetHdc();
            Win32.API.User32.SendMessage(control.Handle, Win32.WM.PRINT, hdc, meptr);

            g2.ReleaseHdc(hdc);
            g2.Dispose();

            return true;
        }
    }
}
