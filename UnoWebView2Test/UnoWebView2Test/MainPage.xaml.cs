using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace UnoWebView2Test;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();

        MyWebView0.NavigationStarting += MyWebView_NavigationStarting;
        MyWebView0.NavigationCompleted += MyWebView_NavigationCompleted;
        MyWebView0.CoreProcessFailed += MyWebView_CoreProcessFailed;
        MyWebView0.CoreWebView2Initialized += MyWebView_CoreWebView2Initialized;
        MyWebView0.WebMessageReceived += MyWebView_WebMessageReceived;

        MyWebView1.NavigationStarting += MyWebView_NavigationStarting;
        MyWebView1.NavigationCompleted += MyWebView_NavigationCompleted;
        MyWebView1.CoreProcessFailed += MyWebView_CoreProcessFailed;
        MyWebView1.CoreWebView2Initialized += MyWebView_CoreWebView2Initialized;
        MyWebView1.WebMessageReceived += MyWebView_WebMessageReceived;

        MyWebView2.NavigationStarting += MyWebView_NavigationStarting;
        MyWebView2.NavigationCompleted += MyWebView_NavigationCompleted;
        MyWebView2.CoreProcessFailed += MyWebView_CoreProcessFailed;
        MyWebView2.CoreWebView2Initialized += MyWebView_CoreWebView2Initialized;
        MyWebView2.WebMessageReceived += MyWebView_WebMessageReceived;

        MyWebView3.NavigationStarting += MyWebView_NavigationStarting;
        MyWebView3.NavigationCompleted += MyWebView_NavigationCompleted;
        MyWebView3.CoreProcessFailed += MyWebView_CoreProcessFailed;
        MyWebView3.CoreWebView2Initialized += MyWebView_CoreWebView2Initialized;
        MyWebView3.WebMessageReceived += MyWebView_WebMessageReceived;

    }

    private bool UseSourceUri;

    private async void Button_Click(object sender, RoutedEventArgs e)
    {

        StatusTextBlock0.Text = StatusTextBlock1.Text = StatusTextBlock2.Text = StatusTextBlock3.Text = string.Empty;


        await MyWebView0.EnsureCoreWebView2Async();
        await MyWebView1.EnsureCoreWebView2Async();
        await MyWebView2.EnsureCoreWebView2Async();
        await MyWebView3.EnsureCoreWebView2Async();
        
        //MyWebView0.NavigateToString("SAMPLE TEXT");
        //return;

        try
        // Asset
        {
#if !__ANDROID__
            //var assetFile = new Uri("ms-appx:///Assets/"); // StorageFile.GetFileFromApplicationUriAsync() throws: System.Runtime.InteropServices.COMException: ''
           // var assetFile = new Uri("ms-appx-web:///Assets/index.html"); // StoreageFile.GetFileFromApplicationUriAsync() throws : System.ArgumentException: 'Value does not fall within the expected range.'
            //var sourceUri = new Uri("ms-appdata:///Assets/index.html"); // Exception
            var sourceUri = new Uri("ms-appx:///Assets/index.html"); // works
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            MyWebView0.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
            // MyWebView0.NavigateToString("TEST STRING");
#endif
        }
        catch (Exception ex)
        {
            StatusTextBlock0.Text = ex.Message;
        }


        using var resource = GetType().Assembly.GetManifestResourceStream("UnoWebView2Test.Resources.index.html");

        try
        {
            var destinationFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (!Directory.Exists(destinationFolder.Path))
                Directory.CreateDirectory(destinationFolder.Path);
            var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
            using var destinationStream = File.OpenWrite(destinationFilePath);
            await resource.CopyToAsync(destinationStream);

            var sourceUri = new Uri("ms-appdata:///Local/index.html");
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            
            #if __ANDROID__
            if (VisualTreeHelper.GetChildren<Android.Webkit.WebView>(MyWebView1).FirstOrDefault() is Android.Webkit.WebView webView)
            {
                webView.Settings.AllowContentAccess = true;
                webView.Settings.AllowFileAccessFromFileURLs = true;
                webView.Settings.AllowUniversalAccessFromFileURLs = true;
                webView.Settings.AllowFileAccess = true;
            }
            #endif
            
            MyWebView1.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
        }
        catch (Exception ex)
        {
            StatusTextBlock1.Text = ex.Message;
        }

        try
        // Temp
        {
            var destinationFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            if (!Directory.Exists(destinationFolder.Path))
                Directory.CreateDirectory(destinationFolder.Path);
            var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
            using var destinationStream = File.OpenWrite(destinationFilePath);
            await resource.CopyToAsync(destinationStream);

            var sourceUri = new Uri("ms-appdata:///Temp/index.html");
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            
#if __ANDROID__
            if (VisualTreeHelper.GetChildren<Android.Webkit.WebView>(MyWebView2).FirstOrDefault() is Android.Webkit.WebView webView)
            {
                webView.Settings.AllowContentAccess = true;
                webView.Settings.AllowFileAccessFromFileURLs = true;
                webView.Settings.AllowUniversalAccessFromFileURLs = true;
                webView.Settings.AllowFileAccess = true;
            }
#endif
            
            MyWebView2.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
        }
        catch (Exception ex)
        {
            StatusTextBlock2.Text = ex.Message;
        }

        try
        // Roaming
        {
            var destinationFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            if (!Directory.Exists(destinationFolder.Path))
                Directory.CreateDirectory(destinationFolder.Path);
            var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
            using var destinationStream = File.OpenWrite(destinationFilePath);
            await resource.CopyToAsync(destinationStream);

            var sourceUri = new Uri("ms-appdata:///Roaming/index.html");
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            
#if __ANDROID__
            if (VisualTreeHelper.GetChildren<Android.Webkit.WebView>(MyWebView3).FirstOrDefault() is Android.Webkit.WebView webView)
            {
                webView.Settings.AllowContentAccess = true;
                webView.Settings.AllowFileAccessFromFileURLs = true;
                webView.Settings.AllowUniversalAccessFromFileURLs = true;
                webView.Settings.AllowFileAccess = true;
            }
#endif
            
            MyWebView3.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
        }
        catch (Exception ex)
        {
            StatusTextBlock3.Text = ex.Message;
        }

    }

    private TextBlock MapTextBlock(WebView2 sender)
    {
        return sender switch
        {
            WebView2 o when o == MyWebView0 => StatusTextBlock0,
            WebView2 o when o == MyWebView1 => StatusTextBlock1,
            WebView2 o when o == MyWebView2 => StatusTextBlock2,
            WebView2 o when o == MyWebView3 => StatusTextBlock3,
            _ => throw new NotImplementedException()
        };
    }

    private void MyWebView_WebMessageReceived(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
    {
        try
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_WebMessageReceived
            """;
            MapTextBlock(sender).Text += $"""
                    source: [{args.Source}]                
                """;
        }
        catch (Exception ex)
        {
            MapTextBlock(sender).Text = ex.Message;
        }
    }

    private void MyWebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
    {
        try
        {
            MapTextBlock(sender).Text += $"""
            CORE WEB VIEW 2 INITIALIZED
                exception: [{args.Exception}]
            """;
        }
        catch (Exception ex)
        {
            MapTextBlock(sender).Text = ex.Message;
        }
    }

    private void MyWebView_CoreProcessFailed(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2ProcessFailedEventArgs args)
    {
        try
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_CoreProcessFailed: 
            """;
            MapTextBlock(sender).Text += $"""
                    kind:[{args.ProcessFailedKind}] 
                    reason:[{args.Reason}] 
                    source:[{args.FailureSourceModulePath}]
                    process:[{args.ProcessDescription}]
                    frameInfo:[{args.FrameInfosForFailedProcess}]
                    exit code:[{args.ExitCode}]                
                """;
        }
        catch (Exception ex)
        {
            MapTextBlock(sender).Text = ex.Message;
        }
    }

    private void MyWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
    {
        try
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_NavigationCompleted
                success: [{args.IsSuccess}]
                status:[{args.HttpStatusCode}]
                errorStatus:[{args.WebErrorStatus}]
                id:[{args.NavigationId}]

            """;
        }
        catch (Exception ex)
        {
            MapTextBlock(sender).Text = ex.Message;
        }
    }

    private void MyWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
    {
        try
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_NavigationStarting
                cancel: [{args.Cancel}]
                url:[{args.Uri}]
                id:[{args.NavigationId}]
                userInitiated:[{args.IsUserInitiated}]
            """;

            MapTextBlock(sender).Text += $"""
                    kind:[{args.NavigationKind}]                
                """;
        }
        catch (Exception ex)
        {
            MapTextBlock(sender).Text = ex.Message;
        }
    }

    private void useSourceUriToggle_Toggled(object sender, RoutedEventArgs e)
    {
        UseSourceUri = useSourceUriToggle.IsOn;
    }



}
