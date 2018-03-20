using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExpenseTracker.ContentViews.AdControlView), typeof(AdView))]
namespace ExpenseTracker.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer<ContentViews.AdControlView, AdView>
    {
        string adUnitId = string.Empty;
        AdSize adSize = AdSize.SmartBanner;
        AdView adView;

        AdView CreateNativeAdControl()
        {
            if (adView != null)
                return adView;

            adUnitId = "ca-app-pub-0100865074267126/9917922436";
            adView = new AdView(Forms.Context);
            adView.AdSize = adSize;
            adView.AdUnitId = adUnitId;

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            adView.LayoutParameters = adParams;

            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ContentViews.AdControlView> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null)
            {
                CreateNativeAdControl();
                SetNativeControl(adView);
            }
        }
    }
}