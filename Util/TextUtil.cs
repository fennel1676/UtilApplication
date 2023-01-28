using System.Drawing;

namespace AKI.Util
{
    public class TextUtil
    {
        public static Size GetTextSize(Graphics graphics, string strText, Font font, Size size)
        {
            if (0 == strText.Length)
                return Size.Empty;

            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.FitBlackBox; //MeasureTrailingSpaces;

            RectangleF layoutRect = new System.Drawing.RectangleF(0, 0, size.Width, size.Height);
            CharacterRange[] chRange = { new CharacterRange(0, strText.Length) };
            Region[] regs = new Region[1];

            format.SetMeasurableCharacterRanges(chRange);

            regs = graphics.MeasureCharacterRanges(strText, font, layoutRect, format);
            Rectangle rect = Rectangle.Round(regs[0].GetBounds(graphics));

            return new Size(rect.Width, rect.Height);
        }

    }
}
