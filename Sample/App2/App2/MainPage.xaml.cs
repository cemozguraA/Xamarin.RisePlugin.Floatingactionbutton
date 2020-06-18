using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;

namespace App2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            var btn = new ActionButtonView() { BackgroundColor = Color.Aqua, SelectedColor = Color.Brown, HeightRequest = 50, Icon = "logo.png" };

            btn.Margin = new Thickness(20, 10, 10, Device.RuntimePlatform == Device.Android ? 160 : 10);
            var btnSub1 = new ActionButtonView() { BackgroundColor = Color.IndianRed, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "tab_feed.png" };
            var btnSub2 = new ActionButtonView() { BackgroundColor = Color.Orange, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub3 = new ActionButtonView() { BackgroundColor = Color.OrangeRed, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub4 = new ActionButtonView() { BackgroundColor = Color.Maroon, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub5 = new ActionButtonView() { BackgroundColor = Color.Lime, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub6 = new ActionButtonView() { BackgroundColor = Color.LightGreen, SelectedColor = Color.Pink, Icon = "logo.png" };
            btnSub1.ClickActionButton += BtnSub1_ClickActionButton;
            btnSub2.ClickActionButton += BtnSub2_ClickActionButton;
            btnSub3.ClickActionButton += BtnSub3_ClickActionButton;
            btnSub4.ClickActionButton += BtnSub4_ClickActionButton;

            COAFloatingactionbutton.Current.SubViews.Add(btnSub1);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub2);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub3);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub4);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub5);

            btn.ClickActionButton += Btn_ClickActionButton;
            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Center;
            COAFloatingactionbutton.Current.OpeningType = ActionOpeningType.Circle;
            COAFloatingactionbutton.Current.CircleAngle = 150;
            COAFloatingactionbutton.Current.MainButtonView = btn;
            COAFloatingactionbutton.Current.Open();
        }
        private void BtnSub1_ClickActionButton(object sender, EventArgs e)
        {

            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Center;
        }
        private void BtnSub2_ClickActionButton(object sender, EventArgs e)
        {

            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Right;
        }
        private void BtnSub3_ClickActionButton(object sender, EventArgs e)
        {
            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Left;
        }
        private void BtnSub4_ClickActionButton(object sender, EventArgs e)
        {

        }
        private async void Btn_ClickActionButton(object sender, EventArgs e)
        {
            if (!COAFloatingactionbutton.Current.IsSubShowing)
                await COAFloatingactionbutton.Current.ShowSubView();
            else
                await COAFloatingactionbutton.Current.HideSubView();


        }
    }
}
