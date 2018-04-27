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
    public class BoundActivity : Activity
    {
        protected ViewBinding Binding { get; private set; }

        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewGroup root = (ViewGroup)Window.DecorView.RootView;
            ViewGroup root2 = (ViewGroup)root.GetChildAt(0);
            ViewGroup root3 = (ViewGroup)root2.GetChildAt(0);
            Binding = new ViewBinding(root3, this, this);
        }

        public void Update()
        {
            Binding.Update();
        }
        public void UpdateProperties(params string[] props)
        {
            Binding.UpdateProperties(props);
        }

    }
}