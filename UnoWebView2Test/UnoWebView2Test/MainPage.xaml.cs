namespace UnoWebView2Test;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await MyWebView.EnsureCoreWebView2Async();
        MyWebView.NavigateToString("SAMPLE TEXT");
    }
}
