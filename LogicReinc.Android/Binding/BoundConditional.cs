using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace LogicReinc.Android.Binding
{
    public class BoundConditional : LinearLayout, IBoundView
    {
        Context _context;

        public bool AlwaysUpdate => false;

        public string Binding { get; set; }
        public string BindValue { get; private set; }
        public bool Inverted { get; private set; }

        public string VisibilityBinding { get; set; }


        public BoundConditional(Context context) : base(context)
        {
            _context = context;
        }

        public BoundConditional(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            ViewBinding.FillBoundView(this, context, attrs);
            if (Binding != null && Binding.StartsWith("!"))
            {
                Binding = Binding.Substring(1);
                Inverted = true;
            }
            BindValue = attrs.GetAttributeProperty(context, Resource.Styleable.BoundConditional, Resource.Styleable.BoundConditional_bindvalue);
        }

        public void Apply(object data)
        {
            if (Binding != null && BindValue != null)
            {
                if ((!Inverted && data.ToString() == BindValue) || (Inverted && data.ToString() != BindValue))
                    this.Visibility = ViewStates.Visible;
                else
                    this.Visibility = ViewStates.Gone;
            }
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }
}