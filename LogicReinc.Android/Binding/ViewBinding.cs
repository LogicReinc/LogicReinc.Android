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
using System.Reflection;
using Android.Content.Res;
using Android.Util;

namespace LogicReinc.Android.Binding
{
    public class ViewBinding
    {
        private List<IBoundView> _views = new List<IBoundView>();
        private Dictionary<string, PropertyInfo> _props = new Dictionary<string, PropertyInfo>();
        private object _model = null;
        private Type _modelType = null;
        private Dictionary<string, object> _lastUpdate = null;

        public ViewBinding(ViewGroup view, object obj, Context context = null)
        {
            _model = obj;
            _modelType = obj.GetType();
            List<View> views = view.GetDescendantViews();
            _views = views.Where(x => x is IBoundView).Select(y => (IBoundView)y).ToList();

            List<string> careProps = _views
                .Select(x => x.Binding).Where(x=>x != null)
                .Union(_views.Where(x=>x.VisibilityBinding != null)
                        .Select(x=> (x.VisibilityBinding.StartsWith("!")) ? x.VisibilityBinding.Substring(1) : x.VisibilityBinding))
                .Distinct().ToList();
            Type type = obj.GetType();
            foreach(string prop in careProps)
            {
                if (prop == null)
                    continue;

                string aProp = (prop.StartsWith("!")) ? prop.Substring(1) : prop;

                PropertyInfo propObj = type.GetProperty(aProp);
                if (propObj == null)
                    throw new ArgumentException("Object doesn't have property: " + aProp);
                _props.Add(aProp, propObj);
            }
            foreach(IBoundView v in _views)
            {
                v.InitializeBind(this, _modelType, (v.Binding != null) ? _props[v.Binding] : null, obj, context);
            }
        }

        public List<IBoundView> GetViewByBinding(string binding)
        {
            return _views.Where(x => x.Binding == binding).ToList();
        }

        public void Update()
        {
            Dictionary<string, object> cache = GetCacheOrNew();
            Dictionary<string, bool> didChange = new Dictionary<string, bool>();
            List<string> didUpdate = new List<string>();
            foreach (IBoundView view in _views)
            {
                UpdateProperty(view, cache, didChange, didUpdate);
                UpdateVisibility(view, cache, didChange, didUpdate);
            }
        }
        public void UpdateProperties(params string[] items)
        {
            List<string> didUpdate = new List<string>();
            Dictionary<string, bool> didChange = new Dictionary<string, bool>();
            Dictionary<string, object> cache = GetCacheOrNew();
            foreach (IBoundView view in _views)
            {
                if (view.Binding != null && items.Contains(view.Binding))
                    UpdateProperty(view, cache, didChange, didUpdate);
                
                if (view.VisibilityBinding != null && items.Contains((!view.VisibilityBinding.StartsWith("!")) ? view.VisibilityBinding : view.VisibilityBinding.Substring(1)))
                    UpdateVisibility(view, cache, didChange, didUpdate);
            }
            _lastUpdate = cache;
        }

        protected Dictionary<string, object> GetCacheOrNew()
        {
            Dictionary<string, object> cache = null;
            if (_lastUpdate != null)
                cache = new Dictionary<string, object>(_lastUpdate);
            else
                cache = new Dictionary<string, object>();
            return cache;
        }
        protected void UpdateProperty(IBoundView view, Dictionary<string,object> cache, Dictionary<string,bool> didChange, List<string> didUpdate)
        {
            if (view.Binding == null)
                return;
            
            if (!didUpdate.Contains(view.Binding))
            {
                object val = Expressions.Property.Get(_model, _props[view.Binding].Name);//_props[view.Binding].GetValue(_model);
                if (cache.ContainsKey(view.Binding))
                {
                    if (cache[view.Binding] != val)
                    {
                        cache[view.Binding] = val;
                        didChange.Add(view.Binding, true);
                    }
                    else
                        didChange.Add(view.Binding, false);
                }
                else
                {
                    cache.Add(view.Binding, val);
                    didChange.Add(view.Binding, true);
                }
                didUpdate.Add(view.Binding);
            }

            if (view.AlwaysUpdate || didChange.ContainsKey(view.Binding))
                view.Apply(cache[view.Binding]);
        }
        protected void UpdateVisibility(IBoundView view, Dictionary<string, object> cache, Dictionary<string, bool> didChange, List<string> didUpdate)
        {
            if (view.VisibilityBinding == null)
                return;

            bool inverted = view.VisibilityBinding.StartsWith("!");
            string bind = view.VisibilityBinding;
            if (inverted)
                bind = bind.Substring(1);

            if (!didUpdate.Contains(bind))
            {
                object val = Expressions.Property.Get(_model, _props[bind].Name);//_props[bind].GetValue(_model);
                if (cache.ContainsKey(bind))
                {
                    if (cache[bind] != val)
                    {
                        cache[bind] = val;
                        didChange.Add(bind, true);
                    }
                    else
                        didChange.Add(bind, false);
                }
                else
                {
                    cache.Add(bind, val);
                    didChange.Add(bind, true);
                }
                didUpdate.Add(bind);
            }

            if (didChange.ContainsKey(bind))
            {
                bool val = (bool)cache[bind];
                if(inverted)
                {
                    if (!val)
                        view.SetVisibility(ViewStates.Visible);
                    else
                        view.SetVisibility(ViewStates.Gone);
                }
                else
                {
                    if (val)
                        view.SetVisibility(ViewStates.Visible);
                    else
                        view.SetVisibility(ViewStates.Gone);
                }
            }   
        }

        internal static void FillBoundView(IBoundView view, Context context, IAttributeSet attrs)
        {
            TypedArray attrArr = context.ObtainStyledAttributes(attrs, Resource.Styleable.BoundItem);
            view.Binding = attrArr.GetString(Resource.Styleable.BoundItem_binding);
            view.VisibilityBinding = attrArr.GetString(Resource.Styleable.BoundItem_visibility_binding);
        }
    }
}