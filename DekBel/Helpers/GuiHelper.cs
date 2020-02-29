using System.Windows.Forms;

namespace Dek.Bel.Helpers
{
    public class GuiHelper
    {
        public int Width { get; }
        public int Height { get; }
        public int Margin { get; }
        public float BorderThickness { get; }
        public string Font { get; }
        public float FontSize { get; }
        public bool RightMargin { get; }
        public string DisplayMode { get; }

        public static void LoadAComboBoxWithPdfFonts(ComboBox theComboBox)
        {
            foreach (string item in Constants.PdfFonts)
            {
                theComboBox.Items.Add(item);
            }
        }

        public static void LoadAComboBoxWithDisplayModes(ComboBox theComboBox)
        {
            foreach (string item in Constants.MarginBoxVisualModes)
            {
                theComboBox.Items.Add(item);
            }
        }
    }
}
