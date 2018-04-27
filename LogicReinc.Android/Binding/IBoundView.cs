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
using System.Reflection;

namespace LogicReinc.Android.Binding
{
    public interface IBoundView
    {
        bool AlwaysUpdate { get; }
        string Binding { get; set; }
        string VisibilityBinding { get; set; }

        void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context);
        void Apply(object data);
        void SetVisibility(ViewStates visibility);
    }
}