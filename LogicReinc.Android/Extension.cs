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
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Android.Graphics;
using System.Threading;
using Android.Util;
using Android.Content.Res;

namespace LogicReinc.Android
{
    public static class Extension
    {

        //UI
        public static List<View> GetDescendantViews(this ViewGroup group, List<View> views = null)
        {
            if (views == null)
                views = new List<View>();

            List<View> newViews = group.GetAllViews();
            views.AddRange(newViews);

            foreach (View groupView in newViews.Where(x => x is ViewGroup))
                GetDescendantViews(((ViewGroup)groupView), views);
            return views;
        }
        public static List<View> GetAllViews(this ViewGroup group)
        {
            HashSet<View> hasViews = new HashSet<View>();
            List<View> views = new List<View>();

            int initCount = group.ChildCount;
            for (int i = 0; i < initCount; i++)
            {
                if (i < group.ChildCount)
                {
                    View v = group.GetChildAt(i);
                    if (initCount == group.ChildCount || !hasViews.Contains(v))
                    {
                        hasViews.Add(v);
                        views.Add(v);
                    }
                }
            }

            return views;
        }

        public static async Task Alert(this Context context, string title, string msg, string button = "Ok")
        {
            await UI.Alert(context, title, msg, button);
        }


        public static string GetAttributeProperty(this IAttributeSet attrs, Context context, int[] group, int attr)
        {
            TypedArray attrArr = context.ObtainStyledAttributes(attrs, group);
            return attrArr.GetString(attr);
        }

        public static Task LoadImagesAsync(List<KeyValuePair<ImageView, string>> sets)
        {
            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    foreach (var set in sets)
                        using (Stream stream = client.OpenRead(set.Value))
                        {
                            try
                            {
                                Handler handler = new Handler(set.Key.Context.MainLooper);
                                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                                handler.Post(() => set.Key.SetImageBitmap(bitmap));
                                Thread.Sleep(500);
                                handler.Dispose();
                            }
                            catch
                            {

                            }
                        }
                }
            });
        }
        public static Task LoadImageAsync(this ImageView view, string url)
        {
            return Task.Run(() =>
            {
                    using (WebClient client = new WebClient())
                    {
                        using (Stream stream = client.OpenRead(url))
                        {
                            try
                            {
                                Handler handler = new Handler(view.Context.MainLooper);
                                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                                handler.Post(() => view.SetImageBitmap(bitmap));
                                Thread.Sleep(500);
                                handler.Dispose();
                            }
                            catch
                            {

                            }
                        }
                    }
            });
        }


    }
}