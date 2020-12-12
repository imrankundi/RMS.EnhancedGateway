using System;
using System.Drawing;
using System.Windows.Forms;

namespace RMS
{
    public class CrossThreadControlOperations
    {
        public static void EnableButton(Control control, Button button, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, Button, bool>(EnableButton),
                    new object[] { control, button, enabled });
                return;

            }

            button.Enabled = enabled;
        }

        public static void ChangeButtonText(Control control, Button button, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, Button, string>(ChangeButtonText),
                    new object[] { control, button, text });
                return;

            }

            button.Text = text;
        }

        public static void UpdatePictureBox(Control control, PictureBox pictureBox, Image image)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, PictureBox, Image>(UpdatePictureBox),
                    new object[] { control, pictureBox, image });
                return;

            }

            pictureBox.Image = image;
        }

        public static void ChangeSelectedIndex(Control control, ComboBox comboBox, int index)
        {

            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, ComboBox, int>(ChangeSelectedIndex),
                    new object[] { control, comboBox, index });
                return;

            }

            comboBox.SelectedIndex = index;
        }

        public static void EnableComboBox(Control control, ComboBox comboBox, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, ComboBox, bool>(EnableComboBox),
                    new object[] { control, comboBox, enabled });
                return;
            }

            comboBox.Enabled = enabled;
        }

        public static void MakeTextBoxReadOnly(Control control, TextBox textbox, bool readOnly)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, TextBox, bool>(MakeTextBoxReadOnly),
                    new object[] { control, textbox, readOnly });
                return;

            }

            textbox.ReadOnly = readOnly;
        }

        public static void UpdateTextBox(Control control, TextBox textbox, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, TextBox, string>(UpdateTextBox),
                    new object[] { control, textbox, text });
                return;

            }

            textbox.Text = text;
        }

        public static void UpdateLabel(Control control, Label label, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, Label, string>(UpdateLabel),
                    new object[] { control, label, text });
                return;

            }

            label.ForeColor = Color.FromArgb(15, 39, 93);
            label.Text = text;
        }

        public static void UpdateLabel(Control control, Label label, string text, Color color)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, Label, string, Color>(UpdateLabel),
                    new object[] { control, label, text, color });
                return;

            }
            label.ForeColor = color;
            label.Text = text;
        }

        public static void UpdateTextBox(Control control, RichTextBox textbox, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, RichTextBox, string>(UpdateTextBox),
                    new object[] { control, textbox, text });
                return;

            }

            textbox.Text = text;
        }
    }
}
