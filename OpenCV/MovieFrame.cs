using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenCvSharp;

namespace AKI.OpenCV
{
    public class MovieFrameOpenCV
    {
        private VideoCapture m_vcVideoCapture;
        private bool m_checkLoad = false;
        private int m_nSamplingCycle = 1000;
        private int m_nWidth;
        private int m_nHeight;
        private int m_nSourceWidth;
        private int m_nSourceHeight;
        private int m_nScaleWidth;
        private int m_nScaleHeight;
        private int m_nCurrentSamplingTime;
        private double m_dSeondPerFrame;
        private object m_objLock = new object();
        private OpenCvSharp.Size m_szSize = new OpenCvSharp.Size();
        private eFrameDisplay m_eFrameDisplay = eFrameDisplay.None;

        #region Get / Set
        public bool IsLoad { get { return m_checkLoad; } }
        public double FPS { get { return (null == m_vcVideoCapture) ? 0 : m_vcVideoCapture.Fps; } }
        public int FrameCount { get { return (null == m_vcVideoCapture) ? 0 : m_vcVideoCapture.FrameCount; } }
        public int Width { get { return m_nWidth; } }
        public int Height { get { return m_nHeight; } }
        public int SourceWidth
        {
            get
            {
                if (m_eFrameDisplay == eFrameDisplay.Left)
                    return m_nSourceWidth / 2;
                else if (m_eFrameDisplay == eFrameDisplay.Top)
                    return m_nSourceWidth;
                else if (m_eFrameDisplay == eFrameDisplay.Center)
                    return m_nSourceWidth / 2;
                else
                    return m_nSourceWidth;
            }
        }
        public int SourceHeight
        {
            get
            {
                if (m_eFrameDisplay == eFrameDisplay.Left)
                    return m_nSourceHeight;
                else if (m_eFrameDisplay == eFrameDisplay.Top)
                    return m_nSourceHeight / 2;
                else if (m_eFrameDisplay == eFrameDisplay.Center)
                    return m_nSourceHeight;
                else
                    return m_nSourceHeight;
            }
        }
        #endregion
        public eFrameDisplay FrameDisplay { get { return m_eFrameDisplay; } set { m_eFrameDisplay = value; } }

        public string Load(string strFileName)
        {
            try
            {
                m_vcVideoCapture = new VideoCapture(strFileName);
                m_checkLoad = true;
            }
            catch (Exception ex)
            {
                m_vcVideoCapture = null;
                m_checkLoad = false;
                return ex.Message;
            }

            if (null == m_vcVideoCapture)
            {
                m_checkLoad = false;
                return "Failed to load file.";
            }

            Mat image = new Mat();
            m_vcVideoCapture.Read(image); // same as cvQueryFrame
            if (image.Empty())
            {
                m_checkLoad = false;
                return "Failed to read image.";
            }

            m_nSourceWidth = image.Width;
            m_nSourceHeight = image.Height;

            return string.Empty;
        }
        
        public Bitmap GetFrame(int nFrameIndex)
        {
            if (null == m_vcVideoCapture)
                return null;

            Bitmap bmpResult = null;
            lock(m_objLock)
            {
                Mat matImage = new Mat();
                Mat matImageTemp = new Mat();
                try
                {
                    m_vcVideoCapture.PosFrames = nFrameIndex;
                    m_vcVideoCapture.Read(matImage); // same as cvQueryFrame
                    if (matImage.Empty())
                        return null;
                    //Bitmap bmpTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImage);
                    //bmpResult = new Bitmap(bmpTemp, m_nWidth, m_nHeight);
                    //bmpTemp.Dispose();

                    if (m_eFrameDisplay == eFrameDisplay.Left)
                    {
                        matImageTemp = matImage.Resize(m_szSize);
                        Bitmap bmpResultTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImageTemp);
                        bmpResult = new Bitmap(m_nWidth / 2, m_nHeight);
                        Graphics grp = Graphics.FromImage(bmpResult);
                        grp.DrawImage(bmpResultTemp, 0, 0);
                        bmpResultTemp.Dispose();
                    }
                    else if (m_eFrameDisplay == eFrameDisplay.Top)
                    {
                        matImageTemp = matImage.Resize(m_szSize);
                        Bitmap bmpResultTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImageTemp);
                        bmpResult = new Bitmap(m_nWidth, m_nHeight / 2);
                        Graphics grp = Graphics.FromImage(bmpResult);
                        grp.DrawImage(bmpResultTemp, 0, 0);
                        bmpResultTemp.Dispose();
                    }
                    else if (m_eFrameDisplay == eFrameDisplay.Center)
                    {
                        matImageTemp = matImage.Resize(m_szSize);
                        Bitmap bmpResultTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImageTemp);
                        bmpResult = new Bitmap(m_nWidth / 2, m_nHeight);
                        Graphics grp = Graphics.FromImage(bmpResult);
                        grp.DrawImage(bmpResultTemp, -1 * (m_nWidth / 4), 0);
                        bmpResultTemp.Dispose();
                    }
                    else
                    {
                        matImageTemp = matImage.Resize(m_szSize);
                        bmpResult = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matImageTemp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    matImage.Dispose();

                    if (null != matImageTemp)
                        matImageTemp.Dispose();
                }
            }
            return bmpResult;
        }

        public void SetSamplingFrame(int nMillisecond, int nWidth = -1, int nHeight = -1)
        {
            m_nSamplingCycle = nMillisecond;
            m_nCurrentSamplingTime = 0;
            m_dSeondPerFrame = FPS / 1000;

            m_nScaleWidth = nWidth;
            m_nScaleHeight = nHeight;

            m_nWidth = (0 < nWidth) ? nWidth : m_nSourceWidth;
            m_nHeight = (0 < nHeight) ? nHeight : m_nSourceHeight;

            m_szSize.Width = m_nWidth;
            m_szSize.Height = m_nHeight;
        }

        public Bitmap NextSamplingFrame()
        {
            int nFrameIndex = (int)(m_nCurrentSamplingTime * m_dSeondPerFrame);
            if (nFrameIndex > FrameCount)
                return null;

            Bitmap bmp = GetFrame(nFrameIndex);
            if (null == bmp)
                return null;

            m_nCurrentSamplingTime += m_nSamplingCycle;
            return bmp;
        }

        public Bitmap GetSamplingFrame(int nIndex)
        {
            int nFrameIndex = (int)((nIndex * m_nSamplingCycle) * m_dSeondPerFrame);
            if (nFrameIndex > FrameCount)
                return null;

            Bitmap bmp = GetFrame(nFrameIndex);
            if (null == bmp)
                return null;

            return bmp;
        }

    }
}
