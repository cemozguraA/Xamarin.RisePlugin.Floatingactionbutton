using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.RisePlugin.Floatingactionbutton;

namespace Xamarin.RisePlugin.Droid.Floatingactionbutton
{
    class CustomFloatingactionbutton : FloatingActionButton
    {
        private ActionButtonView _renderer;
        int _spacing;
        public CustomFloatingactionbutton(Context context, ActionButtonView renderer, int spacing) : base(context)
        {
            _spacing = spacing;
            Setbtn(renderer);
            renderer.PropertyChanged += Renderer_PropertyChanged;


        }

        private void CustomFloatingActionButton_Click(object sender, EventArgs e)
        {
            _renderer.ClickAction();
        }

        public void Setbtn(ActionButtonView v)
        {
            _renderer = v;
            SetRippleColor(ColorStateList.ValueOf(v.SelectedColor.ToAndroid()));
            BackgroundTintList = ColorStateList.ValueOf(v.BackgroundColor.ToAndroid());
            SetSize();
            if (!string.IsNullOrWhiteSpace(v.Icon))
            {
                SetIcon(v.Icon);
            }
        }
        public void SetIcon(string Icon)
        {
            try
            {
                var drawableNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Icon).ToLower();
                var resources = Context.Resources;
                var imageResourceName = resources.GetIdentifier(drawableNameWithoutExtension, "drawable", Context.PackageName);
                var bitmapp = BitmapFactory.DecodeResource(Context.Resources, imageResourceName);
                var Height = (int)(_renderer.HeightRequest * 2.5) / 2;
                var result = Bitmap.CreateScaledBitmap(bitmapp, Height, Height, false);
                SetImageBitmap(result);
                SetScaleType(ImageView.ScaleType.Center);

            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("There was no Android Drawable by that name.", ex);
            }
        }
        void SetSize()
        {

            if (!(LayoutParameters is RelativeLayout.LayoutParams))
            {
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

                lp.SetMargins(_spacing, _spacing, _spacing, _spacing);


                lp.Gravity = GravityFlags.Center;

                CustomSize = (int)(_renderer.HeightRequest * 2.5);

                LayoutParameters = lp;
            }
        }
        private void Renderer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //var lp = (LinearLayout.LayoutParams)LayoutParameters;
            if (e.PropertyName == "SelectedColor")
                SetRippleColor(ColorStateList.ValueOf(_renderer.SelectedColor.ToAndroid()));
            else if (e.PropertyName == "BackgroundColor")
                BackgroundTintList = ColorStateList.ValueOf(_renderer.BackgroundColor.ToAndroid());
            else if (e.PropertyName == "HeightRequest")
                SetSize();
            else if (e.PropertyName == "Icon")
                SetIcon(_renderer.Icon);
        }


    }
}