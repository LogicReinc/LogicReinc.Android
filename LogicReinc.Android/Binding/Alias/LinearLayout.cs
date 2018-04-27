using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LogicReinc.Android.Binding;

namespace Bound
{
    public class LinearLayout : BoundLinearLayout
    {
        public LinearLayout(Context context) : base(context) { }
        public LinearLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }
    }
}