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

        Window.GetWindow(this).MouseLeftButtonDown += OnMouseLeftButtonDownParentWindow;
        Window.GetWindow(this).MouseLeftButtonUp += OnMouseLeftButtonUpParentWindow;
        Window.GetWindow(this).MouseMove += OnMouseMoveParentWindow;

        this.LayoutUpdated += ResizableBorder_LayoutUpdated;
    }

    private void ResizableBorder_LayoutUpdated(object? sender, System.EventArgs e)
    {

    }

    protected void OnMouseLeftButtonDownParentWindow(object o, MouseButtonEventArgs e)
    {
        mouseStartDragPos = Mouse.GetPosition(Window.GetWindow(this));
        SetStretching();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        SetStretching();
    }

    protected void SetStretching()
    {
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

        if (ResizableLeft && InRangePadding(GrabPadding, 0, mousePosRelativeToBorder.X) && InRange(0 - GrabPadding, ActualHeight + GrabPadding, mousePosRelativeToBorder.Y))
        {
            onBorderLeft = true;
        }

        if (ResizableTop && InRangePadding(GrabPadding, 0, mousePosRelativeToBorder.Y) && InRange(0 - GrabPadding, ActualWidth + GrabPadding, mousePosRelativeToBorder.X))
        {
            onBorderTop = true;
        }

        if (ResizableRight && InRangePadding(GrabPadding, ActualWidth, mousePosRelativeToBorder.X) && InRange(0 - GrabPadding, ActualHeight + GrabPadding, mousePosRelativeToBorder.Y))
        {
            onBorderRight = true;
        }

        if (ResizableBot && InRangePadding(GrabPadding, ActualHeight, mousePosRelativeToBorder.Y) && InRange(0 - GrabPadding, ActualWidth + GrabPadding, mousePosRelativeToBorder.X))
        {
            onBorderBot = true;
        }


        Point currentMousePos = Mouse.GetPosition(Window.GetWindow(this));
        Point difference = (Point)(currentMousePos - mouseStartDragPos);
        difference.X /= 2;
        difference.Y /= 2;


        if (isStretchingLeft)
        {
            double newWidth = ActualWidth - difference.X;
            if (IsInConstrains(newWidth, ActualHeight))
            {
                Width = newWidth;
                AddToMargin(difference.X, 0, 0, 0);
            }
        }

        if (isStretchingTop)
        {
            double newHeight = ActualHeight - difference.Y;
            if (IsInConstrains(ActualWidth, newHeight))
            {
                Height = newHeight;
                AddToMargin(0, difference.Y, 0, 0);
            }
        }

        if (isStretchingRight)
        {
            double newWidth = ActualWidth + difference.X;
            if (IsInConstrains(newWidth, ActualHeight))
            {
                Width = newWidth;
                AddToMargin(0, 0, -difference.X, 0);
            }
        }

        if (isStretchingBot)
        {
            double newHeight = ActualHeight + difference.Y;
            if (IsInConstrains(ActualWidth, newHeight))
            {
                Height = newHeight;
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

    protected bool IsInConstrains(double newWidth, double newHeight)
    {
        return !(newWidth < MinWidth) && !(newWidth > MaxWidth) && !(newHeight < MinHeight) && !(newHeight > MaxHeight);
    }

}

