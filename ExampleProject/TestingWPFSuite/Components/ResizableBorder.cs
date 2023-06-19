using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GlibGrozin.Components;

public class ResizableBorder : UserControl // For direct content this needs to derive from UserControl
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ResizableBorder), new PropertyMetadata(new CornerRadius(0)));
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty GrabPaddingProperty =
    DependencyProperty.Register(nameof(GrabPadding), typeof(double), typeof(ResizableBorder), new PropertyMetadata(3.0));
    public double GrabPadding
    {
        get { return (double)GetValue(GrabPaddingProperty); }
        set { SetValue(GrabPaddingProperty, value); }
    }

    public static readonly DependencyProperty ResizableTopProperty =
    DependencyProperty.Register(nameof(ResizableTop), typeof(bool), typeof(ResizableBorder), new PropertyMetadata(true));
    public bool ResizableTop
    {
        get { return (bool)GetValue(ResizableTopProperty); }
        set { SetValue(ResizableTopProperty, value); }
    }

    public static readonly DependencyProperty ResizableRightProperty =
    DependencyProperty.Register(nameof(ResizableRight), typeof(bool), typeof(ResizableBorder), new PropertyMetadata(true));
    public bool ResizableRight
    {
        get { return (bool)GetValue(ResizableRightProperty); }
        set { SetValue(ResizableRightProperty, value); }
    }

    public static readonly DependencyProperty ResizableBotProperty =
    DependencyProperty.Register(nameof(ResizableBot), typeof(bool), typeof(ResizableBorder), new PropertyMetadata(true));
    public bool ResizableBot
    {
        get { return (bool)GetValue(ResizableBotProperty); }
        set { SetValue(ResizableBotProperty, value); }
    }

    public static readonly DependencyProperty ResizableLeftProperty =
    DependencyProperty.Register(nameof(ResizableLeft), typeof(bool), typeof(ResizableBorder), new PropertyMetadata(true));
    public bool ResizableLeft
    {
        get { return (bool)GetValue(ResizableLeftProperty); }
        set { SetValue(ResizableLeftProperty, value); }
    }

    private Border? border;
    private Point mouseStartDragPos;

    private bool isStretchingLeft = false;
    private bool isStretchingTop = false;
    private bool isStretchingRight = false;
    private bool isStretchingBot = false;

    private bool onBorderLeft = false;
    private bool onBorderTop = false;
    private bool onBorderRight = false;
    private bool onBorderBot = false;

    static ResizableBorder()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizableBorder), new FrameworkPropertyMetadata(typeof(ResizableBorder)));
    }


    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (this.GetTemplateChild("PART_Border") is Border border)
        {
            this.border = border;
        }

        Window.GetWindow(this).MouseLeftButtonDown += OnMouseLeftButtonDownParentWindow;
        Window.GetWindow(this).MouseLeftButtonUp += OnMouseLeftButtonUpParentWindow;
        Window.GetWindow(this).MouseMove += OnMouseMoveParentWindow;
    }

    protected void OnMouseLeftButtonDownParentWindow(object o, MouseButtonEventArgs e)
    {

        //Check if mouse is on bordeŕ
        mouseStartDragPos = Mouse.GetPosition(Window.GetWindow(this));

        if (onBorderLeft) isStretchingLeft = true;
        if (onBorderTop) isStretchingTop = true;
        if (onBorderRight) isStretchingRight = true;
        if (onBorderBot) isStretchingBot = true;
    }

    protected void OnMouseLeftButtonUpParentWindow(object o, MouseButtonEventArgs e)
    {
        DisableStretching();
    }

    private void DisableStretching()
    {
        isStretchingLeft = false;
        isStretchingTop = false;
        isStretchingRight = false;
        isStretchingBot = false;
    }

    protected void OnMouseMoveParentWindow(object o, MouseEventArgs e)
    {
        onBorderLeft = false;
        onBorderTop = false;
        onBorderRight = false;
        onBorderBot = false;


        Point mousePosRelativeToBorder = Mouse.GetPosition(this);

        if (ResizableLeft && InRangePadding(GrabPadding, 0, mousePosRelativeToBorder.X) && InRange(0 - GrabPadding, border!.ActualHeight + GrabPadding, mousePosRelativeToBorder.Y))
        {
            onBorderLeft = true;
        }

        if (ResizableTop && InRangePadding(GrabPadding, 0, mousePosRelativeToBorder.Y) && InRange(0 - GrabPadding, border!.ActualWidth + GrabPadding, mousePosRelativeToBorder.X))
        {
            onBorderTop = true;
        }

        if (ResizableRight && InRangePadding(GrabPadding, border!.ActualWidth, mousePosRelativeToBorder.X) && InRange(0 - GrabPadding, border!.ActualHeight + GrabPadding, mousePosRelativeToBorder.Y))
        {
            onBorderRight = true;
        }

        if (ResizableBot && InRangePadding(GrabPadding, border!.ActualHeight, mousePosRelativeToBorder.Y) && InRange(0 - GrabPadding, border!.ActualWidth + GrabPadding, mousePosRelativeToBorder.X))
        {
            onBorderBot = true;
        }


        Point currentMousePos = Mouse.GetPosition(Window.GetWindow(this));
        Point difference = (Point)(currentMousePos - mouseStartDragPos);
        difference.X /= 2;
        difference.Y /= 2;


        if (isStretchingLeft)
        {
            double newWidth = border!.ActualWidth - difference.X;
            if (!(newWidth < border.MinWidth))
            {
                border.Width = newWidth;
                AddToMargin(difference.X, 0, 0, 0);
            }
        }

        if (isStretchingTop)
        {
            double newHeight = border!.ActualHeight - difference.Y;
            if (!(newHeight < border.MinHeight))
            {
                border.Height = newHeight;
                AddToMargin(0, difference.Y, 0, 0);
            }
        }

        if (isStretchingRight)
        {
            double newWidth = border!.ActualWidth + difference.X;
            if (!(newWidth < border.MinWidth))
            {
                border.Width = newWidth;
                AddToMargin(0, 0, -difference.X, 0);
            }
        }

        if (isStretchingBot)
        {
            double newHeight = border!.ActualHeight + difference.Y;
            if (!(newHeight < border.MinHeight))
            {
                border.Height = newHeight;
                AddToMargin(0, 0, 0, -difference.Y);
            }
        }

        mouseStartDragPos = currentMousePos;

        bool l = onBorderLeft || isStretchingLeft;
        bool t = onBorderTop || isStretchingTop;
        bool r = onBorderRight || isStretchingRight;
        bool b = onBorderBot || isStretchingBot;


        if (l) Mouse.SetCursor(Cursors.SizeWE);
        if (t) Mouse.SetCursor(Cursors.SizeNS);
        if (r) Mouse.SetCursor(Cursors.SizeWE);
        if (b) Mouse.SetCursor(Cursors.SizeNS);

        if (l && t || r && b) Mouse.SetCursor(Cursors.SizeNWSE);
        if (l && b || r && t) Mouse.SetCursor(Cursors.SizeNESW);

        // Need to check if the mouse button is not up anymore explicitly because
        // window.MouseButtonUp does not trigger if the mouse button was released 
        // above another component
        if (Mouse.LeftButton != MouseButtonState.Pressed) DisableStretching();
    }

    private bool InRangePadding(double padding, double middle, double value)
    {
        return (middle - padding) <= value && (middle + padding) >= value;
    }

    private bool InRange(double min, double max, double value)
    {
        return value >= min && value <= max;
    }

    private void AddToMargin(double l, double t, double r, double b)
    {
        Thickness margin = this.Margin;
        margin.Left += l;
        margin.Top += t;
        margin.Right += r;
        margin.Bottom += b;

        this.Margin = margin;
    }
}

