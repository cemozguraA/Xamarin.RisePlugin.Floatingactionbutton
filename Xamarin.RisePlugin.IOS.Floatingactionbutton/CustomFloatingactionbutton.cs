using System;
using System.IO;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.RisePlugin.Floatingactionbutton;

namespace Xamarin.RisePlugin.IOS.Floatingactionbutton
{
    public sealed class CustomFloatingactionbutton : UIButton
    {
        private readonly ActionButtonView _view;
        private readonly NSLayoutConstraint _heightConstraint;
        private readonly NSLayoutConstraint _widthConstraint;

        public CustomFloatingactionbutton(ActionButtonView View)
        {
            _view = View;
            SetIcon();
            Layer.CornerRadius = (nfloat)_view.HeightRequest / 2;
            BackgroundColor = _view.BackgroundColor.ToUIColor();
            _heightConstraint = HeightAnchor.ConstraintEqualTo((nfloat)View.HeightRequest);
            _heightConstraint.Active = true;
            _widthConstraint = WidthAnchor.ConstraintEqualTo((nfloat)View.HeightRequest);
            _widthConstraint.Active = true;
            TouchDown += CustomFloatingactionbutton_TouchDown;
            _view.PropertyChanged += PropertyChanged;
           var _longPressGestureRecognizer = new UILongPressGestureRecognizer(HandleLongPress);
            AddGestureRecognizer(_longPressGestureRecognizer);
        }
        private void HandleLongPress(UILongPressGestureRecognizer o)
        {
            switch (o.State)
            {
                case UIGestureRecognizerState.Began:
                    _view.LongClickAction();
                    System.Diagnostics.Debug.WriteLine("LONG PRESS BEGAN");
                    break;

                case UIGestureRecognizerState.Ended:
                    System.Diagnostics.Debug.WriteLine("LONG PRESS ENDED");
                    break;
            }
        }
        private void CustomFloatingactionbutton_TouchDown(object sender, EventArgs e)
        {
            Animate(0.1, 0, UIViewAnimationOptions.Autoreverse,
                () => { BackgroundColor = _view.SelectedColor.ToUIColor(); },
                () => { BackgroundColor = _view.BackgroundColor.ToUIColor(); });
            _view.ClickAction();
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_view.Icon))
                SetIcon();
            else if (e.PropertyName == nameof(_view.HeightRequest))
            {
                _heightConstraint.Constant = (nfloat)_view.HeightRequest;
                _widthConstraint.Constant = (nfloat)_view.HeightRequest;
            }
        }

        private void SetIcon()
        {
            if (string.IsNullOrEmpty(_view.Icon))
                return;
            var image = File.Exists(_view.Icon) ? new UIImage(_view.Icon) : UIImage.FromBundle(_view.Icon);
            if (image != null)
                SetImage(ResizeImage(image, _view.HeightRequest / 2, _view.HeightRequest / 2), UIControlState.Normal);
        }

        private static UIImage ResizeImage(UIImage sourceImage, double Width, double Height)
        {
            UIGraphics.BeginImageContext(new CGSize(Width, Height));
            sourceImage.Draw(new CGRect(0, 0, Width, Height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            TouchDown -= CustomFloatingactionbutton_TouchDown;
            _view.PropertyChanged -= PropertyChanged;
        }
    }
}