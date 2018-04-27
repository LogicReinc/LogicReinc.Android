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

namespace LogicReinc.Android.Binding
{
    public class BoundViewContainer : LinearLayout
    {
        private bool _initialized = false;
        private ViewBinding _binding = null;

        public BoundViewContainer(Context context, int layoutId) : base(context)
        {
            Inflate(context, layoutId, this);
        }

        public BoundViewContainer(Context context) : base(context)
        {}
        public BoundViewContainer(Context context, IAttributeSet attrs) : base(context, attrs)
        {}

        public void InitializeBinding(object model)
        {
            _binding = new ViewBinding(this, model);
            _initialized = true;
        }

        public void Update()
        {
            _binding.Update();
        }
    }
}