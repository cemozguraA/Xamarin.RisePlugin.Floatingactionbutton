using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.RisePlugin.Droid.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton;
using Xamarin.RisePlugin.Floatingactionbutton.Enums;
using Orientation = Android.Widget.Orientation;

[assembly: Xamarin.Forms.DependencyAttribute(
          typeof(Xamarin.RisePlugin.Droid.Floatingactionbutton.COAFloatingactionbutton))]
namespace Xamarin.RisePlugin.Droid.Floatingactionbutton
{
    public class COAFloatingactionbutton : IFloatActionButton
    {
        Context context;
        FloatingActionButton fltbutton;
        StackActionOrientation _stackActionOrientation;
        public StackActionOrientation ActionOrientation
        {
            get { return _stackActionOrientation; }
            set
            {
                if (_stackActionOrientation == value)
                    return;
                _stackActionOrientation = value;
                if (linearLayout != null)
                {
                    RelativeLayout.LayoutParams lrparam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    lrparam.AddRule(LayoutRules.AlignParentBottom);
                    if (ActionOrientation == StackActionOrientation.Right)
                        lrparam.AddRule(LayoutRules.AlignParentEnd);
                    else if (ActionOrientation == StackActionOrientation.Center)
                        lrparam.AddRule(LayoutRules.CenterHorizontal);
                    else if (ActionOrientation == StackActionOrientation.Left)
                        lrparam.AddRule(LayoutRules.AlignParentStart);
                    lrparam.SetMargins((int)MainButtonView.Margin.Left, (int)MainButtonView.Margin.Top, (int)MainButtonView.Margin.Right, (int)MainButtonView.Margin.Bottom);
                    linearLayout.LayoutParameters = lrparam;
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
                if (linearLayout != null)
                {
                    if (value == ActionOpeningType.VerticalTop || value == ActionOpeningType.VerticalBottom)
                        linearLayout.Orientation = Orientation.Vertical;
                    else
                        linearLayout.Orientation = Orientation.Horizontal;
                }

            }
        }
        public ActionButtonView MainButtonView { get; set; }
        public IList<ActionButtonView> SubViews { get; set; }
        public bool IsSubShowing { get; set; }
        bool isShowing;
        bool isProgress;
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
                    context = Application.Context;
                    context.SetTheme(Resource.Style.MainTheme);

                    linearLayout = new LinearLayout(context);

                    if (OpeningType == ActionOpeningType.VerticalTop || OpeningType == ActionOpeningType.VerticalBottom)
                        linearLayout.Orientation = Orientation.Vertical;
                    else
                        linearLayout.Orientation = Orientation.Horizontal;


                    fltbutton = new CustomFloatingactionbutton(context, MainButtonView, SubViewSpacing);
                    MainButtonView.PropertyChanged += MainButtonView_PropertyChanged;
                    linearLayout.AddView(fltbutton);
                    linearLayout.LayoutParameters = SetMainButtonLayout();
                    RootView.View.AddView(linearLayout);
                    fltbutton.Click += Fltbutton_Click;
                }
                else
                {
                    HideSubView(10);
                    for (int i = 0; i < linearLayout.ChildCount; i++)
                    {
                        linearLayout.RemoveViewAt(0);
                    }
                    ((RelativeLayout)linearLayout.Parent).RemoveView(linearLayout);
                    fltbutton = null;
                    linearLayout = null;
                    context = null;
                    //SubViews = null;
                }
                isShowing = value;
            }
        }

        private void Fltbutton_Click(object sender, EventArgs e)
        {
            MainButtonView.ClickAction();
        }

        public int CircleAngle { get; set; }
        public int SubViewSpacing { get; set; }
        public COAFloatingactionbutton()
        {
            if (SubViews == null)
                SubViews = new List<ActionButtonView>();
            CircleAngle = 300;
            SubViewSpacing = 10;
        }
        public async Task<bool> ShowSubView(int Duration = 150)
        {

            try
            {
                if (SubViews.Count > 0)
                {
                    if (!IsSubShowing && IsShowing && !isProgress)
                    {
                        isProgress = true;
                        if (OpeningType != ActionOpeningType.Circle)
                            foreach (var item in SubViews)
                            {

                                var Btnn = new CustomFloatingactionbutton(context, item, SubViewSpacing);

                                if (OpeningType == ActionOpeningType.VerticalTop || OpeningType == ActionOpeningType.HorizontalLeft)
                                    linearLayout.AddView(Btnn, 0);
                                else
                                    linearLayout.AddView(Btnn);

                                //Animation animate = AnimationUtils.LoadAnimation(context, Resource.Animation.EnterFromLeft);
                                //Btnn.StartAnimation(animate);

                                switch (OpeningType)
                                {
                                    case ActionOpeningType.HorizontalLeft:
                                        Btnn.Animate().TranslationX(-SubViewSpacing).SetDuration(Duration);
                                        break;
                                    case ActionOpeningType.HorizontalRight:
                                        Btnn.Animate().TranslationX(+SubViewSpacing).SetDuration(Duration);
                                        break;
                                    case ActionOpeningType.VerticalTop:
                                        Btnn.Animate().TranslationY(-SubViewSpacing).SetDuration(Duration);
                                        break;
                                    case ActionOpeningType.VerticalBottom:
                                        Btnn.Animate().TranslationY(+SubViewSpacing).SetDuration(Duration);
                                        break;
                                    default:
                                        break;
                                }


                                await Task.Delay(Duration);
                                if (OpeningType == ActionOpeningType.HorizontalLeft || OpeningType == ActionOpeningType.HorizontalRight)
                                    Btnn.Animate().TranslationX(0).SetDuration(Duration);
                                else
                                    Btnn.Animate().TranslationY(0).SetDuration(Duration);

                                Btnn.Click += (sender, e) => { item.ClickAction(); };
                            }
                        else
                        {

                            var RL = new FrameLayout(context);
                            var metrics = Resources.System.DisplayMetrics;
                            var ly = new RelativeLayout.LayoutParams(metrics.WidthPixels, metrics.HeightPixels);
                            ly.AddRule(LayoutRules.AlignParentBottom);
                            if (ActionOrientation == StackActionOrientation.Right)
                                ly.AddRule(LayoutRules.AlignParentEnd);
                            else if (ActionOrientation == StackActionOrientation.Center)
                            {
                                ly.AddRule(LayoutRules.CenterHorizontal);
                                ly.Width *= 2;
                            }
                            else if (ActionOrientation == StackActionOrientation.Right)
                                ly.AddRule(LayoutRules.AlignParentStart);


                            RL.LayoutParameters = ly;


                            RootView.View.AddView(RL, 1);
                            float AndroidCircle = (float)(CircleAngle * 2.5);
                            if (ActionOrientation == StackActionOrientation.Center)
                            {
                                if (SubViews.Count == 5)
                                    SetAngel(SubViews, AndroidCircle, RL, 225, Duration);
                                else if (SubViews.Count == 4)
                                    SetAngel(SubViews, AndroidCircle, RL, 240, Duration);
                                else if (SubViews.Count == 6)
                                    SetAngel(SubViews, AndroidCircle, RL, 220, Duration);
                                else if (SubViews.Count == 3)
                                    SetAngel(SubViews, AndroidCircle, RL, 270, Duration);
                                else if (SubViews.Count == 2)
                                    SetAngel(SubViews, AndroidCircle, RL, 360, Duration);
                                else if (SubViews.Count >= 7)
                                    SetAngel(SubViews, AndroidCircle, RL, 215, Duration);


                            }
                            else
                                SetAngel(SubViews, AndroidCircle, RL, 110, Duration);






                        }
                        isProgress = false;
                        IsSubShowing = true;
                        lastOpeningType = OpeningType;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("SubViews Null", ex);
            }

        }
        public void SetAngel(IList<ActionButtonView> Lst, float radius, FrameLayout rl, int Degress, int Duration)
        {

            var degrees = Degress / Lst.Count;
            int i = 0;
            foreach (var item in Lst)
            {

                var hOffset = radius * Math.Cos(i * degrees * 3.14 / 180);
                var vOffset = radius * Math.Sin(i * degrees * 3.14 / 180);
                var btn = new CustomFloatingactionbutton(context, item, SubViewSpacing);
                btn.Alpha = 0;
                var ly = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                btn.CustomSize = (int)(item.HeightRequest * 2.5);

                if (ActionOrientation == StackActionOrientation.Right)
                    ly.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                else if (ActionOrientation == StackActionOrientation.Center)
                    ly.Gravity = GravityFlags.Center | GravityFlags.Bottom;
                else if (ActionOrientation == StackActionOrientation.Left)
                    ly.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                var mainbuttonmargins = (LinearLayout.LayoutParams)linearLayout.GetChildAt(0).LayoutParameters;
                var LinearLayoutprm = (RelativeLayout.LayoutParams)linearLayout.LayoutParameters;
                ly.SetMargins(mainbuttonmargins.LeftMargin, 0, mainbuttonmargins.RightMargin, LinearLayoutprm.BottomMargin == 0 ? 30 : LinearLayoutprm.BottomMargin);

                //ly.AddRule(LayoutRules.AlignParentBottom);
                btn.LayoutParameters = ly;
                if (ActionOrientation == StackActionOrientation.Left)
                    btn.Animate().TranslationX(-(float)(-1 * hOffset)).SetDuration(Duration);
                else
                    btn.Animate().TranslationX((float)(-1 * hOffset)).SetDuration(Duration);
                btn.Animate().TranslationY(((float)(-1 * vOffset))).SetDuration(Duration);

                btn.Animate().Alpha(1).SetDuration(Duration);
                btn.Click += (sender, e) =>
                {
                    item.ClickAction();
                };
                //btn.SetIcon(item.Icon);
                rl.AddView(btn);
                i++;
            }


        }
        ActionOpeningType lastOpeningType;
        public async Task<bool> HideSubView(int Duration = 150)
        {

            try
            {
                if (IsSubShowing && IsShowing && !isProgress)
                {
                    isProgress = true;
                    var count = linearLayout.ChildCount - 1;
                    for (int i = 0; i < count; i++)
                    {

                        if (lastOpeningType == ActionOpeningType.VerticalTop || lastOpeningType == ActionOpeningType.HorizontalLeft)
                        {
                            linearLayout.GetChildAt(0).Animate().Alpha(0).SetDuration(Duration);
                            await Task.Delay(Duration);
                            linearLayout.RemoveViewAt(0);
                        }
                        else
                        {
                            linearLayout.GetChildAt(1).Animate().Alpha(0).SetDuration(Duration);
                            await Task.Delay(Duration);
                            linearLayout.RemoveViewAt(1);
                        }
                        IsSubShowing = false;
                    }
                    if (count == 0 && lastOpeningType == ActionOpeningType.Circle)
                    {
                        var RL = (FrameLayout)RootView.View.GetChildAt(1);
                        for (int i = 0; i < RL.ChildCount; i++)
                        {
                            var btn = (FloatingActionButton)RL.GetChildAt(i);
                            btn.Animate().TranslationX(0).SetDuration(Duration);
                            btn.Animate().TranslationY(0).SetDuration(Duration);
                            btn.Animate().Alpha(0).SetDuration(Duration);
                        }
                        await Task.Delay(Duration);
                        for (int i = 1; i < RootView.View.ChildCount; i++)
                        {
                            if (RootView.View.GetChildAt(i) is FrameLayout)
                                RootView.View.RemoveViewAt(i);
                        }

                        IsSubShowing = false;
                    }
                    isProgress = false;
                }

                return true;

            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Some wrong prog.", ex);

            }

        }
        LinearLayout linearLayout;
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
        RelativeLayout.LayoutParams SetMainButtonLayout()
        {
            RelativeLayout.LayoutParams lrparam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lrparam.AddRule(LayoutRules.AlignParentBottom);
            if (ActionOrientation == StackActionOrientation.Right)
                lrparam.AddRule(LayoutRules.AlignParentEnd);
            else if (ActionOrientation == StackActionOrientation.Center)
                lrparam.AddRule(LayoutRules.CenterHorizontal);
            else if (ActionOrientation == StackActionOrientation.Left)
                lrparam.AddRule(LayoutRules.AlignParentStart);
            lrparam.SetMargins((int)MainButtonView.Margin.Left, (int)MainButtonView.Margin.Top, (int)MainButtonView.Margin.Right, (int)MainButtonView.Margin.Bottom);
            return lrparam;
        }
        private void MainButtonView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainButtonView.Margin))
            {
                linearLayout.LayoutParameters = SetMainButtonLayout();
                HideSubView(0);
                ShowSubView(0);
            }
        }
    }
}
