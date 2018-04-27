﻿using System;
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
    public class Conditional : BoundConditional
    {
        public Conditional(Context context) : base(context) { }
        public Conditional(Context context, IAttributeSet attrs) : base(context, attrs) { }
    }
}