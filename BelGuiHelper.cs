using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dek.Bel.Core.Cls;

namespace Dek.Bel
{
    public static class BelGuiHelper
    {
        public static void SetComboBoxSelectedIndex(ComboBox theComboBox, string text)
        {
            int idx = 0;
            foreach (string item in theComboBox.Items)
            {
                if (item.Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    theComboBox.SelectedIndex = idx;
                    return;
                }
                idx++;
            }
        }

        public static void TextChanged_ValidateTextBoxDate(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            if (tb.Text.IsValidSaneDate())
                tb.BackColor = SystemColors.Control;
            else
                tb.BackColor = Color.LightPink;
        }



    }
}
