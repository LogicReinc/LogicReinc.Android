using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LogicReinc.Android
{
    public static class UI
    {
        public static async Task Alert(Context context, string title, string msg, string button = "Ok")
        {
            await Task.Run(() =>
            {
                ManualResetEvent even = new ManualResetEvent(false);
                AlertDialog.Builder builder = new AlertDialog.Builder(context)
                    .SetTitle(title)
                    .SetMessage(msg)
                    .SetPositiveButton(button, (s, e) =>
                    {
                        even.Set();
                    });
                builder.Show();
                even.WaitOne();
            });
        }
    }
}