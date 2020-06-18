using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;

namespace Xamarin.RisePlugin.Floatingactionbutton
{
    public interface IFloatActionButton
    {
        void Open();
        void Close();
        int CircleAngle { get; set; }
        int SubViewSpacing { get; set; }
        StackActionOrientation ActionOrientation { get; set; }
        ActionOpeningType OpeningType { get; set; }
        ActionButtonView MainButtonView { get; set; }
        bool IsSubShowing { get; set; }
        bool IsShowing { get; set; }
        IList<ActionButtonView> SubViews { get; set; }
        Task<bool> HideSubView(int Duration = 150);
        Task<bool> ShowSubView(int Duration = 150);
    }
}
