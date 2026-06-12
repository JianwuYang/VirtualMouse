using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VirtualMouse;

public partial class MainWindow : Window
{
    // ── P/Invoke ──────────────────────────────────────────────

    const int LEFTDOWN = 0x0002, LEFTUP = 0x0004;
    const int RIGHTDOWN = 0x0008, RIGHTUP = 0x0010;
    const int MIDDLEDOWN = 0x0020, MIDDLEUP = 0x0040;

    [DllImport("user32.dll")]
    static extern void mouse_event(uint f, uint dx, uint dy, uint data, UIntPtr extra);

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    struct POINT { public int X, Y; }

    [DllImport("user32.dll")]
    static extern int GetWindowLong(nint hWnd, int nIndex);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(nint hWnd, nint hAfter, int X, int Y, int cx, int cy, uint flags);

    const int GWL_EXSTYLE = -20, WS_EX_TRANSPARENT = 0x20;
    const uint SWP_FRAMECHANGED = 0x0020, SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001, SWP_NOZORDER = 0x0004, SWP_NOACTIVATE = 0x0010;

    static void DoClick(uint down, uint up)
    {
        mouse_event(down, 0, 0, 0, UIntPtr.Zero);
        mouse_event(up, 0, 0, 0, UIntPtr.Zero);
    }
    static void ClickLeft()   => DoClick(LEFTDOWN,   LEFTUP);
    static void ClickRight()  => DoClick(RIGHTDOWN,  RIGHTUP);
    static void ClickMiddle() => DoClick(MIDDLEDOWN, MIDDLEUP);

    // ── 状态 ──────────────────────────────────────────────────

    private bool _clicking;
    private nint _hwnd;

    // ── 颜色 ──────────────────────────────────────────────────

    private static readonly Color C_LEFT  = Color.FromRgb(80, 80, 80);
    private static readonly Color C_RIGHT = Color.FromRgb(72, 72, 72);
    private static readonly Color C_MID   = Color.FromRgb(69, 69, 69);
    private static readonly SolidColorBrush HOVER_LEFT  = new(Color.FromRgb(100, 140, 220));
    private static readonly SolidColorBrush HOVER_RIGHT = new(Color.FromRgb(220, 100, 90));
    private static readonly SolidColorBrush HOVER_MID   = new(Color.FromRgb(180, 150, 80));

    public MainWindow()
    {
        InitializeComponent();

        var wa = System.Windows.SystemParameters.WorkArea;
        Left = wa.Width - 300;
        Top = wa.Height - 320;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
    }

    // ── 红点固定位置 ──────────────────────────────────────────

    Point GetDotScreenPos()
    {
        return PointToScreen(new Point(Width / 2, 65));
    }

    // ── 按钮事件 ──────────────────────────────────────────────

    private async void BtnLeft_Click(object s, MouseButtonEventArgs e)
    { e.Handled = true; await ClickAtPtr(ClickLeft); }

    private async void BtnMiddle_Click(object s, MouseButtonEventArgs e)
    { e.Handled = true; await ClickAtPtr(ClickMiddle); }

    private async void BtnRight_Click(object s, MouseButtonEventArgs e)
    { e.Handled = true; await ClickAtPtr(ClickRight); }

    private void BtnLeft_MouseEnter(object s, MouseEventArgs e)   => BtnLeft.Background = HOVER_LEFT;
    private void BtnLeft_MouseLeave(object s, MouseEventArgs e)   => BtnLeft.Background = new SolidColorBrush(C_LEFT);
    private void BtnMiddle_MouseEnter(object s, MouseEventArgs e) => BtnMiddle.Background = HOVER_MID;
    private void BtnMiddle_MouseLeave(object s, MouseEventArgs e) => BtnMiddle.Background = new SolidColorBrush(C_MID);
    private void BtnRight_MouseEnter(object s, MouseEventArgs e)  => BtnRight.Background = HOVER_RIGHT;
    private void BtnRight_MouseLeave(object s, MouseEventArgs e)  => BtnRight.Background = new SolidColorBrush(C_RIGHT);

    private void ResetHover(object sender, MouseEventArgs e)
    {
        if (BtnLeft != null) BtnLeft.Background = new SolidColorBrush(C_LEFT);
        if (BtnMiddle != null) BtnMiddle.Background = new SolidColorBrush(C_MID);
        if (BtnRight != null) BtnRight.Background = new SolidColorBrush(C_RIGHT);
    }

    // ── 点击执行 ──────────────────────────────────────────────

    async Task ClickAtPtr(Action clickAction)
    {
        if (_clicking) return;
        _clicking = true;

        var target = GetDotScreenPos();
        GetCursorPos(out var saved);

        try
        {
            var ex = GetWindowLong(_hwnd, GWL_EXSTYLE);
            SetWindowLong(_hwnd, GWL_EXSTYLE, ex | WS_EX_TRANSPARENT);
            SetWindowPos(_hwnd, 0, 0, 0, 0, 0,
                SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);

            SetCursorPos((int)target.X, (int)target.Y);
            await Task.Delay(40);
            clickAction();
            await Task.Delay(20);

            SetWindowLong(_hwnd, GWL_EXSTYLE, ex);
            SetWindowPos(_hwnd, 0, 0, 0, 0, 0,
                SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
            SetCursorPos(saved.X, saved.Y);
        }
        finally
        {
            _clicking = false;
        }
    }

    // ── 拖拽 / 关闭 / 退出 ────────────────────────────────────

    private void Body_Drag(object sender, MouseButtonEventArgs e)
    { if (!_clicking) try { DragMove(); } catch { } }

    private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
    { e.Handled = true; Close(); }

    private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
    { if (sender is TextBlock tb) tb.Foreground = new SolidColorBrush(Color.FromRgb(255, 80, 80)); }

    private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
    { if (sender is TextBlock tb) tb.Foreground = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255)); }

    private void Canvas_ContextMenu(object sender, MouseButtonEventArgs e)
    {
        var menu = new ContextMenu();
        var exit = new MenuItem { Header = "退出" };
        exit.Click += (_, _) => Close();
        menu.Items.Add(exit);
        menu.IsOpen = true;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    { if (e.Key == Key.Escape) Close(); }
}
