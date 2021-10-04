using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(Xamarin.RisePlugin.IOS.Floatingactionbutton.COAFloatingactionbutton))]

namespace Xamarin.RisePlugin.IOS.Floatingactionbutton
{
    public class COAFloatingactionbutton : IFloatActionButton
    {
        public int CircleAngle { get; set; }
        private StackActionOrientation _stackActionOrientation;

        public StackActionOrientation ActionOrientation
        {
            get => _stackActionOrientation;
            set
            {
                if (_stackActionOrientation == value)
                    return;
                _stackActionOrientation = value;
                if (_mainView != null && !_isProgress)
                {

                    if (ActionOrientation == StackActionOrientation.Left)
                        _leftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                    else if (ActionOrientation == StackActionOrientation.Center)
                        _leftConstraint.Constant = (_viewController.Frame.Width / 2) -
                                                   ((nfloat)MainButtonView.HeightRequest / 2);
                    else if (ActionOrientation == StackActionOrientation.Right)
                        _leftConstraint.Constant = _viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest -
                                                   (nfloat)MainButtonView.Margin.Right;


                    HideSubView(0);
                    ShowSubView(0);
                }
            }
        }

        private ActionOpeningType _openingType;

        public ActionOpeningType OpeningType
        {
            get => _openingType;
            set
            {
                _openingType = value;
                if (_mainView != null && !_isProgress)
                {
                    _ = HideSubView(0);

                    if (value == ActionOpeningType.VerticalTop || value == ActionOpeningType.VerticalBottom)
                        _mainView.Axis = UILayoutConstraintAxis.Vertical;
                    else
                        _mainView.Axis = UILayoutConstraintAxis.Horizontal;

                    _ = ShowSubView(0);
                }
            }
        }

        public ActionButtonView MainButtonView { get; set; }
        public bool IsSubShowing { get; set; }
        public IList<ActionButtonView> SubViews { get; set; }
        private int _subViewSpacing;

        public int SubViewSpacing
        {
            get => _subViewSpacing;
            set
            {
                _subViewSpacing = value;
                if (_mainView != null) _mainView.Spacing = (nfloat)value;
            }
        }

        private bool _isShowing;

        public static void Init()
        {
        }

        public bool IsShowing
        {
            get => _isShowing;
            set
            {
                if (value)
                {
                    _viewController =
                        UIApplication.SharedApplication.Windows.FirstOrDefault(w => w.RootViewController != null);
                    if (_viewController == null)
                        return;
                    _mainView = new UIStackView();
                    if (OpeningType == ActionOpeningType.VerticalTop || OpeningType == ActionOpeningType.VerticalBottom)
                        _mainView.Axis = UILayoutConstraintAxis.Vertical;
                    else
                        _mainView.Axis = UILayoutConstraintAxis.Horizontal;

                    _mainView.Spacing = _subViewSpacing;
                    _mainView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _viewController.AddSubview(_mainView);
                    _leftConstraint =
                        _mainView.LeftAnchor.ConstraintGreaterThanOrEqualTo(_viewController.LeftAnchor, 0);
                    _leftConstraint.Active = true;
                    MainButtonView.PropertyChanged += MainButtonView_PropertyChanged;
                    if (ActionOrientation == StackActionOrientation.Left)
                        _leftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                    else if (ActionOrientation == StackActionOrientation.Center)
                        _leftConstraint.Constant = (_viewController.Frame.Width / 2) -
                                                   ((nfloat)MainButtonView.HeightRequest / 2);
                    else if (ActionOrientation == StackActionOrientation.Right)
                        _leftConstraint.Constant = _viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest -
                                                   (nfloat)MainButtonView.Margin.Right;

                    _bottomtConstraint = _mainView.BottomAnchor.ConstraintEqualTo(_viewController.BottomAnchor, 0);
                    _bottomtConstraint.Active = true;
                    _bottomtConstraint.Constant =
                        -(nfloat)(MainButtonView.HeightRequest + MainButtonView.Margin.Bottom);


                    _mainBtn = new CustomFloatingactionbutton(MainButtonView);
                    //btn.Frame = new CoreGraphics.CGRect((viewController.Frame.Width / 2) - (MainButtonView.HeightRequest / 2), (viewController.Frame.Height / 2) - (MainButtonView.HeightRequest / 2), MainButtonView.HeightRequest, MainButtonView.HeightRequest);
                    _mainView.AddArrangedSubview(_mainBtn);
                }
                else
                {
                    _ = HideSubView(10);
                    _mainBtn.RemoveFromSuperview();
                    _mainView.RemoveFromSuperview();
                    _mainView = null;
                    _mainBtn = null;
                    _viewController = null;
                }

                _isShowing = value;
            }
        }

        public COAFloatingactionbutton()
        {
            SubViews = new List<ActionButtonView>();
            SubViewSpacing = 10;
            CircleAngle = 300;
        }

        private bool _isProgress;
        private ActionOpeningType _lastOpeningType;

        public async Task<bool> HideSubView(int Duration = 150)
        {
            if (IsSubShowing && !_isProgress && IsShowing)
                if (_mainView.Subviews.Length > 0)
                {
                    _isProgress = true;
                    var mainviewwidh = _mainView.Frame.Width;
                    var count = _mainView.Subviews.Count();
                    if (_lastOpeningType != ActionOpeningType.Circle)
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            var view = _mainView.Subviews[1];
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionFlipFromBottom,
                                () => { view.Alpha = 0; },
                                () => { });
                            await Task.Delay(Duration);
                            if (_lastOpeningType == ActionOpeningType.HorizontalLeft ||
                                _lastOpeningType == ActionOpeningType.HorizontalRight)
                            {
                                mainviewwidh -= _mainView.Spacing + view.Frame.Width;
                                if (ActionOrientation == StackActionOrientation.Center)
                                    _leftConstraint.Constant = (_viewController.Frame.Width / 2) - (mainviewwidh / 2);
                                else if (ActionOrientation == StackActionOrientation.Right)
                                    _leftConstraint.Constant = _viewController.Frame.Width - mainviewwidh;
                                else if (ActionOrientation == StackActionOrientation.Right)
                                    _leftConstraint.Constant = mainviewwidh;
                            }

                            view.RemoveFromSuperview();
                        }
                    }
                    else
                    {
                        List<CustomFloatingactionbutton> lst = new List<CustomFloatingactionbutton>();
                        foreach (var item in _viewController.Subviews)
                        {
                            if (item is CustomFloatingactionbutton)
                            {
                                lst.Add((CustomFloatingactionbutton)item);
                                UIView.Animate((float)Duration / 1000, 0,
                                    UIViewAnimationOptions.TransitionFlipFromBottom,
                                    () =>
                                    {
                                        item.Transform = CGAffineTransform.MakeTranslation(0, 0);
                                        item.Alpha = 0;
                                    },
                                    () => { });


                            }

                        }

                        await Task.Delay(Duration);
                        foreach (var item in lst)
                        {
                            item.RemoveFromSuperview();
                        }

                        lst = null;
                    }

                    IsSubShowing = false;
                    _isProgress = false;
                }

            return true;
        }

        private NSLayoutConstraint _leftConstraint;
        private NSLayoutConstraint _bottomtConstraint;
        private CustomFloatingactionbutton _mainBtn;
        private UIStackView _mainView;
        private UIWindow _viewController;

        public void Open()
        {
            if (!IsShowing)
                IsShowing = true;
        }

        public void Close()
        {
            if (IsShowing)
                IsShowing = false;
        }

        private void MainButtonView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainButtonView.Margin))
            {
                if (ActionOrientation == StackActionOrientation.Left)
                    _leftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                else if (ActionOrientation == StackActionOrientation.Center)
                    _leftConstraint.Constant =
                        (_viewController.Frame.Width / 2) - ((nfloat)MainButtonView.HeightRequest / 2);
                else if (ActionOrientation == StackActionOrientation.Right)
                    _leftConstraint.Constant = _viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest -
                                               (nfloat)MainButtonView.Margin.Right;

                _bottomtConstraint.Constant = -(nfloat)(MainButtonView.HeightRequest + MainButtonView.Margin.Bottom);
            }
        }

        public void SetAngel(IList<ActionButtonView> Lst, float radius, int Degress, int Duration)
        {

            var degrees = Degress / Lst.Count;
            int i = 0;
            foreach (var item in Lst)
            {

                var hOffset = radius * Math.Cos(i * degrees * 3.14 / 180);
                var vOffset = radius * Math.Sin(i * degrees * 3.14 / 180);
                var btn = new CustomFloatingactionbutton(item);
                btn.TranslatesAutoresizingMaskIntoConstraints = false;
                btn.Alpha = 0;
                _viewController.InsertSubview(btn, 1);
                btn.CenterXAnchor.ConstraintGreaterThanOrEqualTo(_mainView.CenterXAnchor, 0).Active = true;
                btn.CenterYAnchor.ConstraintGreaterThanOrEqualTo(_mainView.CenterYAnchor, 0).Active = true;
                nfloat xpos = 0;

                if (ActionOrientation == StackActionOrientation.Left)
                    xpos = (nfloat)hOffset;
                else
                    xpos = -(nfloat)hOffset;

                UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionFlipFromBottom,
                    () =>
                    {
                        btn.Transform = CGAffineTransform.MakeTranslation(xpos, -1 * (nfloat)vOffset);
                        btn.Alpha = 1;
                    },
                    () => { });
                i++;
            }
        }

        public async Task<bool> ShowSubView(int Duration = 150)
        {
            if (!IsSubShowing && !_isProgress && IsShowing)
                if (SubViews.Count > 0)
                {

                    IsSubShowing = true;
                    _isProgress = true;
                    _lastOpeningType = OpeningType;
                    if (OpeningType != ActionOpeningType.Circle)
                        foreach (var item in SubViews)
                        {
                            if (OpeningType == ActionOpeningType.HorizontalLeft ||
                                OpeningType == ActionOpeningType.HorizontalRight)
                            {
                                if (ActionOrientation == StackActionOrientation.Right)
                                    _leftConstraint.Constant -=
                                        _mainView.Spacing + (nfloat)MainButtonView.HeightRequest;
                                else if (ActionOrientation == StackActionOrientation.Center)
                                    _leftConstraint.Constant -=
                                        (_mainView.Spacing + (nfloat)MainButtonView.HeightRequest) / 2;
                            }

                            var btn = new CustomFloatingactionbutton(item);


                            if (OpeningType == ActionOpeningType.VerticalTop ||
                                OpeningType == ActionOpeningType.HorizontalLeft)
                                _mainView.InsertArrangedSubview(btn, 0);
                            else
                                _mainView.AddArrangedSubview(btn);

                            var point = btn.Layer.AnchorPoint;
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionNone,
                                () =>
                                {
                                    switch (OpeningType)
                                    {
                                        case ActionOpeningType.HorizontalLeft:
                                            btn.Transform = CGAffineTransform.MakeTranslation(-_subViewSpacing, 0);
                                            break;
                                        case ActionOpeningType.HorizontalRight:
                                            btn.Transform = CGAffineTransform.MakeTranslation(+_subViewSpacing, 0);
                                            break;
                                        case ActionOpeningType.VerticalTop:
                                            btn.Transform = CGAffineTransform.MakeTranslation(0, -_subViewSpacing);
                                            break;
                                        case ActionOpeningType.VerticalBottom:
                                            btn.Transform = CGAffineTransform.MakeTranslation(0, +_subViewSpacing);
                                            break;
                                    }
                                },
                                () => { });
                            await Task.Delay(Duration);
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionNone,
                                () => { btn.Transform = CGAffineTransform.MakeTranslation(0, 0); },
                                () => { });
                            await Task.Delay(Duration);
                        }
                    else
                    {

                        if (ActionOrientation == StackActionOrientation.Center)
                        {
                            if (SubViews.Count == 5)
                                SetAngel(SubViews, CircleAngle, 225, Duration);
                            else if (SubViews.Count == 4)
                                SetAngel(SubViews, CircleAngle, 240, Duration);
                            else if (SubViews.Count == 6)
                                SetAngel(SubViews, CircleAngle, 220, Duration);
                            else if (SubViews.Count == 3)
                                SetAngel(SubViews, CircleAngle, 270, Duration);
                            else if (SubViews.Count == 2)
                                SetAngel(SubViews, CircleAngle, 360, Duration);
                            else if (SubViews.Count >= 7)
                                SetAngel(SubViews, CircleAngle, 215, Duration);
                        }
                        else
                            SetAngel(SubViews, CircleAngle, 110, Duration);
                    }

                    _isProgress = false;
                }

            return true;
        }
    }
}