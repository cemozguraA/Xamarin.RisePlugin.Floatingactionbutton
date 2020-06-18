using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;
using Xamarin.RisePlugin.IOS.Floatingactionbutton;

[assembly: Xamarin.Forms.DependencyAttribute(
          typeof(Xamarin.RisePlugin.IOS.Floatingactionbutton.COAFloatingactionbutton))]
namespace Xamarin.RisePlugin.IOS.Floatingactionbutton
{
    public class COAFloatingactionbutton : IFloatActionButton
    {
        public int CircleAngle { get; set; }
        StackActionOrientation _stackActionOrientation;
        public StackActionOrientation ActionOrientation
        {
            get { return _stackActionOrientation; }
            set
            {
                if (_stackActionOrientation == value)
                    return;
                _stackActionOrientation = value;
                if (MainView != null && !isProgress)
                {

                    if (ActionOrientation == StackActionOrientation.Left)
                        LeftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                    else if (ActionOrientation == StackActionOrientation.Center)
                        LeftConstraint.Constant = (viewController.Frame.Width / 2) - ((nfloat)MainButtonView.HeightRequest / 2);
                    else if (ActionOrientation == StackActionOrientation.Right)
                        LeftConstraint.Constant = viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest - (nfloat)MainButtonView.Margin.Right;


                    HideSubView(0);
                    ShowSubView(0);
                }

            }
        }
        ActionOpeningType _openingType;
        public ActionOpeningType OpeningType
        {
            get
            {
                return _openingType;
            }
            set
            {
                _openingType = value;
                if (MainView != null && !isProgress)
                {
                    HideSubView(0);

                    if (value == ActionOpeningType.VerticalTop || value == ActionOpeningType.VerticalBottom)
                        MainView.Axis = UILayoutConstraintAxis.Vertical;
                    else
                        MainView.Axis = UILayoutConstraintAxis.Horizontal;

                    ShowSubView(0);
                }

            }
        }
        public ActionButtonView MainButtonView { get; set; }
        public bool IsSubShowing { get; set; }
        public IList<ActionButtonView> SubViews { get; set; }
        int subViewSpacing;
        public int SubViewSpacing { get { return subViewSpacing; } set { subViewSpacing = value; if (MainView != null) MainView.Spacing = (nfloat)value; } }
        bool isShowing;
        public static void Init()
        {

        }
        public bool IsShowing
        {
            get
            {
                return isShowing;
            }
            set
            {
                if (value)
                {
                    viewController = UIApplication.SharedApplication.Windows.FirstOrDefault(w => w.RootViewController != null);
                    if (viewController == null)
                        return;
                    MainView = new UIStackView();
                    if (OpeningType == ActionOpeningType.VerticalTop || OpeningType == ActionOpeningType.VerticalBottom)
                        MainView.Axis = UILayoutConstraintAxis.Vertical;
                    else
                        MainView.Axis = UILayoutConstraintAxis.Horizontal;

                    MainView.Spacing = subViewSpacing;
                    MainView.TranslatesAutoresizingMaskIntoConstraints = false;
                    viewController.AddSubview(MainView);
                    LeftConstraint = MainView.LeftAnchor.ConstraintGreaterThanOrEqualTo(viewController.LeftAnchor, 0);
                    LeftConstraint.Active = true;
                    MainButtonView.PropertyChanged += MainButtonView_PropertyChanged;
                    if (ActionOrientation == StackActionOrientation.Left)
                        LeftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                    else if (ActionOrientation == StackActionOrientation.Center)
                        LeftConstraint.Constant = (viewController.Frame.Width / 2) - ((nfloat)MainButtonView.HeightRequest / 2);
                    else if (ActionOrientation == StackActionOrientation.Right)
                        LeftConstraint.Constant = viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest - (nfloat)MainButtonView.Margin.Right;

                    bottomtConstraint = MainView.BottomAnchor.ConstraintEqualTo(viewController.BottomAnchor, 0);
                    bottomtConstraint.Active = true;
                    bottomtConstraint.Constant = -(nfloat)(MainButtonView.HeightRequest + MainButtonView.Margin.Bottom);


                    MainBtn = new CustomFloatingactionbutton(MainButtonView);
                    //btn.Frame = new CoreGraphics.CGRect((viewController.Frame.Width / 2) - (MainButtonView.HeightRequest / 2), (viewController.Frame.Height / 2) - (MainButtonView.HeightRequest / 2), MainButtonView.HeightRequest, MainButtonView.HeightRequest);
                    MainView.AddArrangedSubview(MainBtn);
                }
                else
                {
                    HideSubView(10);
                    MainBtn.RemoveFromSuperview();
                    MainView.RemoveFromSuperview();
                    MainView = null;
                    MainBtn = null;
                    viewController = null;
                }
                isShowing = value;
            }
        }
        public COAFloatingactionbutton()
        {
            if (SubViews == null)
                SubViews = new List<ActionButtonView>();
            SubViewSpacing = 10;
            CircleAngle = 300;
        }
        bool isProgress;
        ActionOpeningType lastOpeningType;
        public async Task<bool> HideSubView(int Duration = 150)
        {
            if (IsSubShowing && !isProgress && IsShowing)
                if (MainView.Subviews.Count() > 0)
                {
                    isProgress = true;
                    var mainviewwidh = MainView.Frame.Width;
                    var count = MainView.Subviews.Count();
                    if (lastOpeningType != ActionOpeningType.Circle)
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            var view = MainView.Subviews[1];
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionFlipFromBottom,
                       () =>
                       {
                           view.Alpha = 0;
                       },
                       () => { });
                            await Task.Delay(Duration);
                            if (lastOpeningType == ActionOpeningType.HorizontalLeft || lastOpeningType == ActionOpeningType.HorizontalRight)
                            {
                                mainviewwidh -= MainView.Spacing + view.Frame.Width;
                                if (ActionOrientation == StackActionOrientation.Center)
                                    LeftConstraint.Constant = (viewController.Frame.Width / 2) - (mainviewwidh / 2);
                                else if (ActionOrientation == StackActionOrientation.Right)
                                    LeftConstraint.Constant = viewController.Frame.Width - mainviewwidh;
                                else if (ActionOrientation == StackActionOrientation.Right)
                                    LeftConstraint.Constant = mainviewwidh;
                            }



                            view.RemoveFromSuperview();
                        }
                    }
                    else
                    {
                        List<CustomFloatingactionbutton> lst = new List<CustomFloatingactionbutton>();
                        foreach (var item in viewController.Subviews)
                        {
                            if (item is CustomFloatingactionbutton)
                            {
                                lst.Add((CustomFloatingactionbutton)item);
                                UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionFlipFromBottom,
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
                    isProgress = false;
                }




            return true;
        }
        NSLayoutConstraint LeftConstraint;
        NSLayoutConstraint bottomtConstraint;
        CustomFloatingactionbutton MainBtn;
        UIStackView MainView;
        UIWindow viewController;
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
                    LeftConstraint.Constant = (nfloat)MainButtonView.Margin.Left;
                else if (ActionOrientation == StackActionOrientation.Center)
                    LeftConstraint.Constant = (viewController.Frame.Width / 2) - ((nfloat)MainButtonView.HeightRequest / 2);
                else if (ActionOrientation == StackActionOrientation.Right)
                    LeftConstraint.Constant = viewController.Frame.Width - (nfloat)MainButtonView.HeightRequest - (nfloat)MainButtonView.Margin.Right;

                bottomtConstraint.Constant = -(nfloat)(MainButtonView.HeightRequest + MainButtonView.Margin.Bottom);
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
                viewController.InsertSubview(btn, 1);
                btn.CenterXAnchor.ConstraintGreaterThanOrEqualTo(MainView.CenterXAnchor, 0).Active = true;
                btn.CenterYAnchor.ConstraintGreaterThanOrEqualTo(MainView.CenterYAnchor, 0).Active = true;
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
            if (!IsSubShowing && !isProgress && IsShowing)
                if (SubViews.Count > 0)
                {

                    IsSubShowing = true;
                    isProgress = true;
                    lastOpeningType = OpeningType;
                    if (OpeningType != ActionOpeningType.Circle)
                        foreach (var item in SubViews)
                        {
                            if (OpeningType == ActionOpeningType.HorizontalLeft || OpeningType == ActionOpeningType.HorizontalRight)
                            {
                                if (ActionOrientation == StackActionOrientation.Right)
                                    LeftConstraint.Constant -= MainView.Spacing + (nfloat)MainButtonView.HeightRequest;
                                else if (ActionOrientation == StackActionOrientation.Center)
                                    LeftConstraint.Constant -= (MainView.Spacing + (nfloat)MainButtonView.HeightRequest) / 2;
                            }

                            var btn = new CustomFloatingactionbutton(item);


                            if (OpeningType == ActionOpeningType.VerticalTop || OpeningType == ActionOpeningType.HorizontalLeft)
                                MainView.InsertArrangedSubview(btn, 0);
                            else
                                MainView.AddArrangedSubview(btn);

                            var point = btn.Layer.AnchorPoint;
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionNone,
                                 () =>
                                 {
                                     switch (OpeningType)
                                     {
                                         case ActionOpeningType.HorizontalLeft:
                                             btn.Transform = CGAffineTransform.MakeTranslation(-subViewSpacing, 0);
                                             break;
                                         case ActionOpeningType.HorizontalRight:
                                             btn.Transform = CGAffineTransform.MakeTranslation(+subViewSpacing, 0);
                                             break;
                                         case ActionOpeningType.VerticalTop:
                                             btn.Transform = CGAffineTransform.MakeTranslation(0, -subViewSpacing);
                                             break;
                                         case ActionOpeningType.VerticalBottom:
                                             btn.Transform = CGAffineTransform.MakeTranslation(0, +subViewSpacing);
                                             break;
                                         default:
                                             break;
                                     }

                                 },
                                () => { });
                            await Task.Delay(Duration);
                            UIView.Animate((float)Duration / 1000, 0, UIViewAnimationOptions.TransitionNone,
                                 () =>
                                 {
                                     btn.Transform = CGAffineTransform.MakeTranslation(0, 0);
                                 },
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

                    isProgress = false;
                }


            return true;
        }
    }
}