using Android.App;
using Android.Widget;
using Android.OS;
using LogicReinc.Android.Binding;
using Android.Views;
using System.Collections.Generic;
using System;

namespace LogicReinc.Android.TestApp
{
    [Activity(Label = "LogicReinc.Android.TestApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BoundActivity
    {
        public string Name { get; set; } = "YoloName";
        public string Description { get; set; } = "Lorem Ipsum dolor whatever";

        [BoundViewItem(Resource.Layout.TestView)]
        public List<TestModel> Items { get; set; } = new List<TestModel>();

        public string ButtonText { get; set; } = "TestButton";

        public string InputSearch { get; set; } = "TestSearch";

        public bool IsEven => counter % 2 == 0;

        protected override void OnCreate(Bundle bundle)
        {
            SetContentView(Resource.Layout.Main);
            base.OnCreate(bundle);
            
            ChangeData();
        }

        public void OnClick()
        {
            Toast.MakeText(this, "Button pressed: " + counter.ToString(), ToastLength.Short).Show();
        }
        public void OnSetDescription()
        {
            Description = InputSearch;
            UpdateProperties(nameof(Description));
        }

        int counter = 0;
        public void OnCountClick()
        {
            counter++;
            Description = counter.ToString();
            Update();
        }

        public void ChangeData()
        {
            //Changing list items
            Items.Add(new TestModel()
            {
                Title = "Item 1",
                Description = "This is item 1",
                ImageUrl = "" //testurl
            });
            Items.Add(new TestModel()
            {
                Title = "Item 2",
                Description = "This is item 2",
                ImageUrl = "" //testurl2
            });
            Items.Add(new TestModel()
            {
                Title = "Item 3",
                Description = "This is item 3",
                ImageUrl = "" //testurl3
            });

            //Update view
            Update();
        }

        public class TestModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}

