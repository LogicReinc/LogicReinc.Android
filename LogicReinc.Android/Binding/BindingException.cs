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
    public class BindingException : Exception
    {
        public BindingException(string msg) : base(msg)
        {

        }
        public BindingException(string msg, Exception ex) : base(msg, ex)
        {
        }
    }
}