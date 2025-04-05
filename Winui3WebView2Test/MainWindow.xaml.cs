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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Winui3WebView2Test
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
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

            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var files = await folder.GetFilesAsync();

            foreach (var file in files)
            {
                Debug.WriteLine(file.Name);
            }
            
            await MyWebView0.EnsureCoreWebView2Async();
            await MyWebView1.EnsureCoreWebView2Async();
            await MyWebView2.EnsureCoreWebView2Async();
            await MyWebView3.EnsureCoreWebView2Async();
            //MyWebView.NavigateToString("SAMPLE TEXT");

            // Asset
            {
                // var assetFile = new Uri("ms-appx:///Assets/"); // StorageFile.GetFileFromApplicationUriAsync() throws: System.Runtime.InteropServices.COMException: ''
                // var assetFile = new Uri("ms-appx-web:///Assets/index.html"); // StoreageFile.GetFileFromApplicationUriAsync() throws : System.ArgumentException: 'Value does not fall within the expected range.'
                // var sourceUri = new Uri("ms-appdata:///Assets/index.html"); // Exception
                var sourceUri = new Uri("ms-appx:///Assets/index.html"); // works
                var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
                MyWebView0.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
            }

            // Local
            {
                using var resource = GetType().Assembly.GetManifestResourceStream("Winui3WebView2Test.Resources.index.html");
                var destinationFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
                using var destinationStream = File.OpenWrite(destinationFilePath);
                await resource.CopyToAsync(destinationStream);

                var sourceUri = new Uri("ms-appdata:///Local/index.html");
                var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
                MyWebView1.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
            }

            // LocalCache
            {
                using var resource = GetType().Assembly.GetManifestResourceStream("Winui3WebView2Test.Resources.index.html");
                var destinationFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder;
                var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
                using var destinationStream = File.OpenWrite(destinationFilePath);
                await resource.CopyToAsync(destinationStream);

                var sourceUri = new Uri("ms-appdata:///LocalCache/index.html");
                var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
                MyWebView2.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
            }

            // Roaming
            {
                using var resource = GetType().Assembly.GetManifestResourceStream("Winui3WebView2Test.Resources.index.html");
                var destinationFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
                var destinationFilePath = Path.Combine(destinationFolder.Path, "index.html");
                using var destinationStream = File.OpenWrite(destinationFilePath);
                await resource.CopyToAsync(destinationStream);

                var sourceUri = new Uri("ms-appdata:///Roaming/index.html");
                var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(sourceUri);
                MyWebView3.Source = UseSourceUri ? sourceUri : new Uri(storageFile.Path);
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
            MapTextBlock(sender).Text += $"""
            MyWebView_WebMessageReceived
                source: [{args.Source}]

            """;
        }

        private void MyWebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            MapTextBlock(sender).Text += $"""
            CORE WEB VIEW 2 INITIALIZED
                exception: [{args.Exception}]

            """;
        }

        private void MyWebView_CoreProcessFailed(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2ProcessFailedEventArgs args)
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_CoreProcessFailed: 
                kind:[{args.ProcessFailedKind}] 
                reason:[{args.Reason}] 
                source:[{args.FailureSourceModulePath}]
                process:[{args.ProcessDescription}]
                frameInfo:[{args.FrameInfosForFailedProcess}]
                exit code:[{args.ExitCode}]

            """;
        }

        private void MyWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_NavigationCompleted
                success: [{args.IsSuccess}]
                status:[{args.HttpStatusCode}]
                errorStatus:[{args.WebErrorStatus}]
                id:[{args.NavigationId}]

            """;
        }

        private void MyWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            MapTextBlock(sender).Text += $"""
            MyWebView_NavigationStarting
                cancel: [{args.Cancel}]
                kind:[{args.NavigationKind}]
                url:[{args.Uri}]
                id:[{args.NavigationId}]
                userInitiated:[{args.IsUserInitiated}]

            """;
        }

        private void useSourceUriToggle_Toggled(object sender, RoutedEventArgs e)
        {
            UseSourceUri = useSourceUriToggle.IsOn;
        }
    }
}
