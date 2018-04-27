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
    public class BoundLinearLayout : LinearLayout, IBoundView
    {
        private Context _context = null;

        public bool AlwaysUpdate { get; } = true;
        public string Binding { get; set; }
        public string VisibilityBinding { get; set; }

        private IListBinding _binding = null;

        private Type _listType = null;
        private Type _itemType = null;

        public BoundLinearLayout(Context context) : base(context)
        {
            _context = context;
        }

        public BoundLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;

            ViewBinding.FillBoundView(this, context, attrs);
        }

        public void InitializeType(Type viewtype)
        {
            _binding = new ListBinding(_context, viewtype, this, new List<object>());
            _binding.IsOrdered = true;
        }
        public void InitializeLayout(int layout)
        {
            _binding = new ListBinding(_context, layout, this, new List<object>());
            _binding.IsOrdered = true;
        }

        public void InitializeBind(ViewBinding binding, Type modelType, PropertyInfo prop, object model, Context context)
        {
            BoundViewItemAttribute attr = prop.GetCustomAttribute<BoundViewItemAttribute>();
            if (attr != null)
            {
                if(attr.ViewType != null)
                    InitializeType(attr.ViewType);
                else
                {
                    if (attr.LayoutID == -1)
                        throw new Exception("No ViewType or layout given in ViewItemAttribute");
                    else
                        InitializeLayout(attr.LayoutID);
                }
            }
        }

        public void Apply(object data)
        {
            if (data == null)
            {
                if (_binding != null)
                {
                    _binding.Model = new List<object>();
                    _binding.Update();
                }
                return;
            }
            List<object> objs = new List<object>();

            if (_listType == null)
                _listType = data.GetType();

            if (typeof(IList).IsAssignableFrom(_listType))
            {
                if (!_listType.IsGenericType)
                    throw new Exception("Only support List<> or Arrays");
                if(_itemType == null)
                    _itemType = _listType.GetGenericArguments()[0];
                IList listType = (IList)data;
                for (int i = 0; i < listType.Count; i++)
                    objs.Add(listType[i]);
            }
            else if (_listType.IsArray)
            {
                if (_itemType == null)
                    _itemType = _listType.GetElementType();
                Array arr = (Array)data;
                for (int i = 0; i < arr.Length; i++)
                    objs.Add(arr.GetValue(i));
            }
            else
                throw new Exception("Only support List<> or Arrays");

            if (_binding == null)
                throw new Exception("No item view defined for binding: " + Binding);

            _binding.Model = objs;
            _binding.Update();
        }

        public void SetVisibility(ViewStates visibility) => Visibility = visibility;
    }

    public class BoundViewItemAttribute : Attribute
    {
        public Type ViewType { get; private set; }
        public int LayoutID { get; private set; }
        public BoundViewItemAttribute(Type viewType)
        {
            ViewType = viewType;
        }
        public BoundViewItemAttribute(int layoutId)
        {
            LayoutID = layoutId;
        }
    }
}