
using static Izuto.Controls.CustomWindow;

namespace Izuto.DockManager
{
    public class CustomWindowOptions
    {
        public WindowTypes WindowType { get; set; } = WindowTypes.Resizable;
        public bool ShowGripperWhenResizable { get; set; } = true;
    }
}
