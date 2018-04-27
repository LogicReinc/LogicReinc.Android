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
    public class BoundButton: Button, IBoundView
    {
        private Context _context;

        public bool AlwaysUpdate { get; } = false;
        public string Binding { get; set; }
        public string VisibilityBinding { get; set; }
        public string ClickBinding { get; private set; }


        public BoundButton(Context context): base(context)
        {
            _context = context;
        }
        public BoundButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;

            ViewBinding.FillBoundView(this, context, attrs);

            TypedArray attrArr2 = _context.ObtainStyledAttributes(attrs, Resource.Styleable.BoundButton);
            ClickBinding = attrArr2.GetString(Resource.Styleable.BoundButton_onclick);
        }


        public void Apply(object data)
        {
            Text = (string)data;
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {
            if(ClickBinding != null)
            {
                object[] paraVal = null;
                MethodInfo method = null;
                object callObj = null;

                if (ClickBinding.StartsWith("activity:"))
                {
                    callObj = context;
                    method = context.GetType().GetMethod(ClickBinding.Substring(ClickBinding.IndexOf(":") + 1));
                    if (method != null)
                    {
                        ParameterInfo[] paras = method.GetParameters();

                        paraVal = new object[paras.Length];
                        if (paras.Length > 0)
                        {
                            for (int i = 0; i < paras.Length; i++)
                            {
                                if (paras[i].ParameterType == typeof(Context))
                                    paraVal[i] = context;
                                else if (paras[i].ParameterType == typeof(ViewBinding))
                                    paraVal[i] = binding;
                                else if (paras[i].ParameterType == modelType)
                                    paraVal[i] = model;
                            }
                        }
                    }
                }
                else
                {
                    method = modelType.GetMethod(ClickBinding);
                    if (method != null)
                    {
                        ParameterInfo[] paras = method.GetParameters();
                        paraVal = new object[] { binding };
                    }
                }

                Click += (a, b) =>
                {
                    try
                    {
                        method.Invoke(callObj, paraVal);
                    }
                    catch (Exception ex)
                    {
                        throw new BindingException("Exception on image click: " + ex.InnerException?.Message, ex.InnerException);
                    }
                };
            }
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }
}