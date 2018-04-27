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

namespace LogicReinc.Android.Binding
{
    public interface IListBinding
    {
        List<object> Model { get; set; }
        bool IsOrdered { get; set; }
        void Update();
    }
}