using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Control.ListBox
{
    public class ImageListBoxItem
    {
        public string Index { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Bitmap Icon { get; set; }
        public Bitmap Thumnail { get; set; }
    }

    public partial class ImageListBox : UserControl
    {
        private Size m_size;
        private int m_nPadding = 10;
        private Font m_fontIndex;
        private Font m_fontTitle;
        private Font m_fontSubTitle;
        private Point m_ptIndexPosition;
        private Point m_ptTitlePosition;
        private Point m_ptSubTitlePosition;
        private int m_nCol = 2;
        private int m_nRow = 2;
        private int m_nImageWidth;
        private int m_nImageHeight;

        private Dictionary<int, List<ImageListBoxItem>> m_dicItemGroup = new Dictionary<int, List<ImageListBoxItem>>();
        private List<ImageListBoxItem> m_listItem = new List<ImageListBoxItem>();

        public ImageListBox()
        {
            InitializeComponent();

            CalcItemSize();
        }

        private void CalcItemSize()
        {
            m_nImageWidth = (this.Width - (m_nPadding * (m_nCol - 1))) / m_nCol;
            m_nImageHeight = (this.Height - (m_nPadding * (m_nRow - 1))) / m_nRow;
        }

        public void AddItem(ImageListBoxItem item)
        {
            if (0 == m_listItem.Count)
            {
                m_dicItemGroup[0] = new List<ImageListBoxItem>();
                m_dicItemGroup[0].Add(item);
                m_listItem.Add(item);
            }
            else
            {
                int nPageTotalCount = m_nCol * m_nRow;
                int nPageNo = m_listItem.Count / nPageTotalCount;
                int nItemNo = m_listItem.Count % nPageTotalCount;

                if (0 == nItemNo)
                {
                    m_dicItemGroup[nPageNo] = new List<ImageListBoxItem>();
                    m_dicItemGroup[nPageNo].Add(item);
                    m_listItem.Add(item);
                }
            }
        }

        private void m_buttonPreviously_Click(object sender, EventArgs e)
        {
            
        }
    }
}
