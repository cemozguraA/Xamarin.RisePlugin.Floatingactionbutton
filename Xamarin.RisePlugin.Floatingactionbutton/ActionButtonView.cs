using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.RisePlugin.Floatingactionbutton
{
    public class ActionButtonView : View
    {
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(ActionButtonView), Color.Transparent, BindingMode.OneWay, null, null);

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(string), typeof(ActionButtonView), "", BindingMode.OneWay, null, null);

        public event EventHandler<EventArgs> ClickActionButton;
        public void ClickAction()
        {
            this.ClickActionButton?.Invoke(this, null);
        }
        public ActionButtonView()
        {

            HeightRequest = 70;
        }
    }
}
