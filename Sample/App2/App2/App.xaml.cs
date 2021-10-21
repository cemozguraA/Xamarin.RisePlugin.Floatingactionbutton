using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;

namespace App2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        public static void OnAppearing()
        {
            if (COAFloatingactionbutton.Current.MainButtonView != null)
                return;
            var btn = new ActionButtonView() { BackgroundColor = Color.Aqua, SelectedColor = Color.Brown, HeightRequest = 50, Icon = "logo.png" };

            btn.Margin = new Thickness(20, 10, 10, Device.RuntimePlatform == Device.Android ? 160 : 10);
            var btnSub1 = new ActionButtonView() { BackgroundColor = Color.IndianRed, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "tab_feed.png" };
            var btnSub2 = new ActionButtonView() { BackgroundColor = Color.Orange, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub3 = new ActionButtonView() { BackgroundColor = Color.OrangeRed, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub4 = new ActionButtonView() { BackgroundColor = Color.Maroon, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub5 = new ActionButtonView() { BackgroundColor = Color.Lime, HeightRequest = 50, SelectedColor = Color.Pink, Icon = "logo.png" };
            var btnSub6 = new ActionButtonView() { BackgroundColor = Color.LightGreen, SelectedColor = Color.Pink, Icon = "logo.png" };
            btnSub1.Click += BtnSub1_ClickActionButton;
            btnSub2.Click += BtnSub2_ClickActionButton;
            btnSub3.Click += BtnSub3_ClickActionButton;
            btnSub4.Click += BtnSub4_ClickActionButton;

            COAFloatingactionbutton.Current.SubViews.Add(btnSub1);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub2);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub3);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub4);
            COAFloatingactionbutton.Current.SubViews.Add(btnSub5);

            btn.Click += Btn_ClickActionButton;
            btn.LongClick += Btn_LongClick;
            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Center;
            COAFloatingactionbutton.Current.OpeningType = ActionOpeningType.Circle;
            COAFloatingactionbutton.Current.CircleAngle = 150;
            COAFloatingactionbutton.Current.MainButtonView = btn;
            COAFloatingactionbutton.Current.Open();
        }

        private static async void Btn_LongClick(object sender, EventArgs e)
        {

            await Application.Current.MainPage.DisplayAlert("test", "LongCLick", "ok");
        }

        private static void BtnSub1_ClickActionButton(object sender, EventArgs e)
        {

            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Center;
        }
        private static void BtnSub2_ClickActionButton(object sender, EventArgs e)
        {

            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Right;
        }
        private static void BtnSub3_ClickActionButton(object sender, EventArgs e)
        {
            COAFloatingactionbutton.Current.ActionOrientation = StackActionOrientation.Left;
        }
        private static void BtnSub4_ClickActionButton(object sender, EventArgs e)
        {
           Current.MainPage.Navigation.PushModalAsync(new Page1());
        }
        private static async void Btn_ClickActionButton(object sender, EventArgs e)
        {
            if (!COAFloatingactionbutton.Current.IsSubShowing)
                await COAFloatingactionbutton.Current.ShowSubView();
            else
                await COAFloatingactionbutton.Current.HideSubView();


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
