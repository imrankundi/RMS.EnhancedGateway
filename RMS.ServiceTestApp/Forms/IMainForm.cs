using RMS.UserControls;

namespace RMS.Models
{
    public interface IMainForm
    {
        void LoadChildControl(UcBase sender, UcBase control);
        void ResetChildControl();
        void ShowLoginControl(UcBase control);
    }
}
