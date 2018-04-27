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
    public class ListBinding : IListBinding
    {
        private LinearLayout _list = null;
        private Context _context = null;

        public List<object> Model { get; set; } = new List<object>();
        private Dictionary<object, BoundViewContainer> _present = new Dictionary<object, BoundViewContainer>();

        private Type _viewType = null;
        private int _layout = -1;

        public bool IsOrdered { get; set; } = false;

        public ListBinding(Context context, Type type, LinearLayout list, List<object> model)
        {
            _viewType = type;
            _list = list;
            _context = context;
            Model = model;
        }
        public ListBinding(Context context, int layout, LinearLayout list, List<object> model)
        {
            _layout = layout;
            _list = list;
            _context = context;
            Model = model;
        }


        public void Update()
        {
            if (!IsOrdered)
            {
                ModelChanges changes = GetModelChanges();
                foreach (object mod in changes.New)
                {
                    BoundViewContainer view = CreateView();
                    _list.AddView(view);
                    _present.Add(mod, view);
                    view.InitializeBinding(mod);
                }
                foreach(object mod in changes.Removed)
                {
                    _list.RemoveView(_present[mod]);
                    _present.Remove(mod);
                }
                foreach (BoundViewContainer view in _present.Values)
                    view.Update();
            }
            else
            {
                if (Model.Count == _present.Count)
                    if (Model.All(x => _present.ContainsKey(x)))
                    {
                        foreach (BoundViewContainer view in _present.Values)
                            view.Update();
                        return;
                    }
                _list.RemoveAllViews();
                _present.Clear();
                foreach (object mod in Model)
                {
                    BoundViewContainer view = CreateView();
                    _list.AddView(view);
                    _present.Add(mod, view);
                    view.InitializeBinding(mod);
                    view.Update();
                }
            }
        }

        private BoundViewContainer CreateView()
        {
            if (_viewType != null)
                return (BoundViewContainer)Activator.CreateInstance(_viewType, _context);
            else
            {
                if (_layout == -1)
                    throw new Exception("No view type or layout was given to ListBinding");
                return new BoundViewContainer(_context, _layout);
            }
        }

        private ModelChanges GetModelChanges()
        {
            ModelChanges mods = new ModelChanges();

            HashSet<object> checkd = new HashSet<object>(_present.Keys);
            foreach(object type in Model)
            {
                if(!_present.ContainsKey(type))
                    mods.New.Add(type);
                else
                    checkd.Remove(type);
            }

            foreach(object type in checkd)
                mods.Removed.Add(type);

            return mods;
        }

        private class ModelChanges
        {
            public List<object> New { get; } = new List<object>();
            public List<object> Removed { get; } = new List<object>();
        }
    }
}