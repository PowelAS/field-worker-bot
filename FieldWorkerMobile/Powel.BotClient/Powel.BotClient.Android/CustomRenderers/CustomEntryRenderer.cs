
using System;
using Android.Runtime;
using Android.Widget;
using Powel.BotClient.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Powel.BotClient.Droid.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
            ChangeCursorColorToDefault();
        }

        private void ChangeCursorColorToDefault()
        {
            IntPtr intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrtextViewClass, "mCursorDrawableRes", "I");
            JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, 0); // replace 0 with a Resource.Drawable.my_cursor
        }
    }
}