### AutoCompleteTextView


#### Description
- A button that will work on 'Rootview'. If you want, you can add sub-elements and use it in 5 different ways


![](https://github.com/cemozguraA/Xamarin.RisePlugin.Floatingactionbutton/blob/master/Images/CircleDroidGroup.gif?raw=true)
![](https://github.com/cemozguraA/Xamarin.RisePlugin.Floatingactionbutton/blob/master/Images/CircleIOSGroup.gif?raw=true)
![](https://github.com/cemozguraA/Xamarin.RisePlugin.Floatingactionbutton/blob/master/Images/VerticalHorizDroid.gif?raw=true)
![](https://github.com/cemozguraA/Xamarin.RisePlugin.Floatingactionbutton/blob/master/Images/VerticalHorizIOS.gif?raw=true)


- **Step1**

Add the following NuGet package to your solution.
- **Step2 (IOS)**

You must add this line to your AppDelegate.cs before you use AutoCompleteTextView
 ```csharp
COAFloatingactionbutton.Init();
```
- **Step2 (ANDROID)**

You must add this code to your MainActivity.cs before you use AutoCompleteTextView
 ```csharp
public override void SetContentView(Android.Views.View view)
        {
            RootView.View = (Android.Widget.RelativeLayout)view;
            base.SetContentView(view);
        }
```


| Platforms  | 
| ------------- | 
| IOS  | 
| Android  | 

## Propertys
| Property  | What it does |
| ------------- | ------------|
| Open  | Adding 'Mainbutton' to the rootview. |
| Close  | Deletes 'Mainbutton' to the rootview. |
| CircleAngle  | Set the distance between the circle. |
| SubViewSpacing  | Set spacing between subviews |
| ActionOrientation  | MainButton position (left, center, right)|
| OpeningType  | Subviews opening type (Circle, VerticalTop, VerticalBottom, HorizontalLeft, HorizontalRight)|
| MainButtonView  | Set Mainbutton(Use ActionButtonView)|
| IsSubShowing  | Check SubViews|
| IsShowing  | Check Floatingactionbutton|
| SubViews  | List of subview |
| ShowSubView  | With animation |
| HideSubView  | With animation |




