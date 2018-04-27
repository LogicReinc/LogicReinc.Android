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
using Android.Content.Res;
using Android.Util;
using System.Collections;
using System.Reflection;

namespace LogicReinc.Android.Binding
{
    public class BoundScrollView : ScrollView, IBoundView
    {
        private Context _context = null;

        public bool AlwaysUpdate { get; } = true;
        public string Binding { get; set; }
        public string VisibilityBinding { get; set; }
        
        

        public BoundScrollView(Context context) : base(context)
        {
            _context = context;
        }

        public BoundScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;

            ViewBinding.FillBoundView(this, context, attrs);
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {
        }

        public void Apply(object data)
        {
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }
}