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
using System.Reflection;

namespace LogicReinc.Android.Binding
{
    public class BoundEditText : EditText, IBoundView
    {
        public bool AlwaysUpdate { get; } = false;
        public string Binding { get; set; }
        public string VisibilityBinding { get; set; }

        Context _context;

        public BoundEditText(Context context) : base(context)
        {
            _context = context;
        }

        public BoundEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            ViewBinding.FillBoundView(this, context, attrs);
        }

        public void Apply(object data)
        {
            this.Text = (string)data;
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {
            //TODO: Replace with emit calls for set

            if (Binding != null)
            {
                this.TextChanged += (a, b) =>
                {
                    prop.SetValue(model, (string)this.Text);
                };
            }
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }
}