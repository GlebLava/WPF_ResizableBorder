# WPF_ResizableBorder

## Warning
Don't use this! While it may seem to be a simple task to make a border resize by mouse it is not trivial in wpf. Using a grid splitter is the better alternative.
Problems with this approach at the moment:
- Resizing the border does not want to trigger the resize of it's parent when wanted
- Constraining the border is not simple with my approach. I am using margin to anchor the element when its width or height is increased, so it looks like it's staying in the same spot.
  This makes it hard to get the following scenario working: 1. Window is maximized 2. Border's width is increased 3. Window is minimized -> Result: The border does not constrain itself to    the available space given by the parent.  

## What it is
An attempt at making a border that is resizable by mouse for wpf. 

## How to use
Add the files [ResizableBorder.cs](https://github.com/GlebLava/WPF_ResizableBorder/blob/main/ResizableBorder/ResizableBorder.cs) and [ResizableBorderStyle.xaml](https://github.com/GlebLava/WPF_ResizableBorder/blob/main/ResizableBorder/ResizableBorderStyle.xaml) to your project (typically inside a components folder).

Add the ResourceDictionary wherever the MergedDictionary for the whole project is. Typically it is in App.xaml or in Themes/Generic.xaml. Alternatively, the ResourceDictionary can be added to the Resources of the window using this component:
```
    <Window.Resources>
	    <ResourceDictionary Source=" PathToFolderResizableBorderWasPutIn/ResizableBorderStyle.xaml"/>
    </Window.Resources>
```
Add the namespace to use the component to the window:
```
<Window x:Class="TestingWPFSuite.MainWindow"
	...
	xmlns:component="clr-namespace:GlibGrozin.Components"
	...
>
```
Now the component is usable:
```
<component:ResizableBorder 	BorderBrush="Black"
				BorderThickness="2"
				CornerRadius="2"
				MinHeight="100" MinWidth="100">
            <TextBlock Text="Hello" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Background="Gray"/>
</component:ResizableBorder>
