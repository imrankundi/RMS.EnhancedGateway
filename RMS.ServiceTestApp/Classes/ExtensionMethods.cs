using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RMS
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Control> GetAllControls(this Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
    }
}
