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
using Android.Util;
using Android.Content.Res;
using System.Reflection;

namespace LogicReinc.Android.Binding
{
    public class BoundTextView : TextView, IBoundView
    {
        private Context _context = null;

        public bool AlwaysUpdate { get; } = false;
        public string Binding { get; set; }
        public string VisibilityBinding { get; set; }

        public BoundTextView(Context context) : base(context)
        {
            _context = context;
        }

        public BoundTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;

            ViewBinding.FillBoundView(this, context, attrs);
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {

        }

        public void Apply(object data)
        {
            Text = (string)data;
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }
}