using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RMS.Component.Common
{
    public enum MouseSimulatorClickType
    {
        Left = 1,
        Right = 2
    }
    public class MouseSimulator
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private const int leftClick = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
        private const int rightClick = MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP;

        private uint GetClickType(MouseSimulatorClickType clickType)
        {
            switch (clickType)
            {
                case MouseSimulatorClickType.Left:
                    return leftClick;
                case MouseSimulatorClickType.Right:
                    return rightClick;
                default:
                    return leftClick;
            }
        }

        public void PerformClick(MouseSimulatorClickType clickType)
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            PerformClick(clickType, X, Y);
        }
        public void PerformClick(MouseSimulatorClickType clickType, uint x, uint y)
        {
            uint click = GetClickType(clickType);
            mouse_event(click, x, y, 0, 0);
        }


    }
}
