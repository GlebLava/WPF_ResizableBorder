# WPF_ResizableBorder

## How to use
Add the files [ResizableBorder.cs](https://github.com/GlebLava/WPF_ResizableBorder/blob/main/ResizableBorder/ResizableBorder.cs) and [ResizableBorderStyle.xaml](https://github.com/GlebLava/WPF_ResizableBorder/blob/main/ResizableBorder/ResizableBorderStyle.xaml) to your project (typically inside a components folder).

Add the ResourceDictionary wherever the MergedDictionary for the whole project is. Typically it is in App.xaml or in Themes/Generic.xaml. Alternatively the ResourceDictionary can be added to the Resources of the window using this component:
```
    <Window.Resources>
	    <ResourceDictionary Source="PathToFolderResizableBorderWasPutIn/ResizableBorderStyle.xaml"/>
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
<component:ResizableBorder 	VerticalAlignment="Center"
							HorizontalAlignment="Center"
                            BorderBrush="Black"
                            BorderThickness="2"
                            CornerRadius="2"
                            MinHeight="100" MinWidth="100">
            <TextBlock Text="Hello" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" Background="Gray"/>
</component:ResizableBorder>
