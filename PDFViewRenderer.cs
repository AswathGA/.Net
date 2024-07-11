using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Draftsy.dependency;
using Draftsy.Droid.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PDFView), typeof(PDFViewRenderer))]
namespace Draftsy.Droid.Dependency
{
    public class PDFViewRenderer : WebViewRenderer
    {
        public PDFViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {


                var pdfView = Element as PDFView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                if (pdfView.IsPDF)
                {
                    var url = "https://drive.google.com/viewerng/viewer?embedded=true&url=" + pdfView.Uri;

                    Control.LoadUrl(url);
                }
                else
                {
                    Control.LoadUrl(pdfView.Uri);
                }

            }
        }
    }
}