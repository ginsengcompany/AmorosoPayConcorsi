using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static QuizAmoroso.Droid.MainActivity;
using Xfx;
using Android.Content;
using Plugin.InAppBilling;
using Plugin.MediaManager.Forms.Android;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(WebView), typeof(ScrollableWebViewRendererAndroid))]
namespace QuizAmoroso.Droid
{

    [Activity(Label = "Amoroso Concorsi", Icon = "@drawable/icon", Theme = "@style/MainTheme", /*ScreenOrientation = ScreenOrientation.Portrait,*/ ConfigurationChanges =  ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public TaskCompletionSource<string> PickImageTaskCompletionSource { get; internal set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            
            XfxControls.Init();
            VideoViewRenderer.Init();

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //Xamarin.FormsMaps.Init(this, bundle);
            // Serve per non far andare l'applicazione in onSleep automaticamente
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);

           /* if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    // Set the filename as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
                InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);

            }*/
        }
        public override void OnBackPressed()
        {
            
        }
        public class ScrollableWebViewRendererAndroid : WebViewRenderer
        {
            
            public ScrollableWebViewRendererAndroid()
            {
                System.Diagnostics.Debug.WriteLine("ScrollableWebViewRendererAndroid()");
            }

            protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
            {
                base.OnElementChanged(e);

                if (e.OldElement != null)
                {
                    Control.Touch -= Control_Touch;
                }

                if (e.NewElement != null)
                {
                    Control.Touch += Control_Touch;
                }
            }

            void Control_Touch(object sender, Android.Views.View.TouchEventArgs e)
            {
                // Executing this will prevent the Scrolling to be intercepted by parent views
                switch (e.Event.Action)
                {
                    case MotionEventActions.Down:
                        Control.Parent.RequestDisallowInterceptTouchEvent(true);
                        break;
                    case MotionEventActions.Up:
                        Control.Parent.RequestDisallowInterceptTouchEvent(false);
                        break;
                }
                // Calling this will allow the scrolling event to be executed in the WebView
                Control.OnTouchEvent(e.Event);
            }
        }
    }
}

