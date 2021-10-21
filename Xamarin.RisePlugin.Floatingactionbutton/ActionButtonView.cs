using System;
using Xamarin.Forms;

namespace Xamarin.RisePlugin.Floatingactionbutton
{
    public class ActionButtonView : View
    {
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(ActionButtonView), Color.Transparent,
                BindingMode.OneWay, null, null);

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(string), typeof(ActionButtonView), "", BindingMode.OneWay,
                null, null);

        public event EventHandler<EventArgs> Click;

        public void ClickAction()
        {
            Click?.Invoke(this, null);
        }
        public event EventHandler<EventArgs> LongClick;

        public void LongClickAction()
        {
            LongClick?.Invoke(this, null);
        }



        public ActionButtonView()
        {
            HeightRequest = 70;
        }
    }
}