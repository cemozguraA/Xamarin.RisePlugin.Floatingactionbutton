using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.RisePlugin.Floatingactionbutton;

namespace Xamarin.RisePlugin.IOS.Floatingactionbutton
{
    public class CustomFloatingactionbutton : UIButton
    {
        ActionButtonView _view;
        NSLayoutConstraint heightConstraint;
        NSLayoutConstraint widthConstraint;
        public CustomFloatingactionbutton(ActionButtonView View)
        {
            _view = View;
            SetIcon();
            Layer.CornerRadius = (nfloat)_view.HeightRequest / 2;
            BackgroundColor = _view.BackgroundColor.ToUIColor();
            heightConstraint = HeightAnchor.ConstraintEqualTo((nfloat)View.HeightRequest);
            heightConstraint.Active = true;
            widthConstraint = WidthAnchor.ConstraintEqualTo((nfloat)View.HeightRequest);
            widthConstraint.Active = true;
            TouchDown += delegate (object sender, EventArgs e)
            {
                UIView.Animate(0.1, 0, UIViewAnimationOptions.Autoreverse,
                  () => { BackgroundColor = View.SelectedColor.ToUIColor(); },
                  () => { BackgroundColor = View.BackgroundColor.ToUIColor(); });
                View.ClickAction();
            };
            _view.PropertyChanged += _view_PropertyChanged;

        }

        private void _view_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_view.Icon))
                SetIcon();
            else if (e.PropertyName == nameof(_view.HeightRequest))
            {
                heightConstraint.Constant = (nfloat)_view.HeightRequest;
                widthConstraint.Constant = (nfloat)_view.HeightRequest;
            }
        }

        void SetIcon()
        {
            if (string.IsNullOrEmpty(_view.Icon))
                return;
            var image = File.Exists(_view.Icon) ? new UIImage(_view.Icon) : UIImage.FromBundle(_view.Icon);
            if (image != null)
                SetImage(ResizeImage(image, _view.HeightRequest / 2, _view.HeightRequest / 2), UIControlState.Normal);


        }
        UIImage ResizeImage(UIImage sourceImage, double Width, double Height)
        {
            UIGraphics.BeginImageContext(new CGSize(Width, Height));
            sourceImage.Draw(new CGRect(0, 0, Width, Height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}