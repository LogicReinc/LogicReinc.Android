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
using System.Collections.Concurrent;
using Android.Graphics;
using System.Net;
using System.IO;
using System.Threading;
using LogicReinc.Threading;

namespace LogicReinc.Android.Images
{
    public class ImageCache
    {
        public bool UseCache { get; private set; }
        public int ThreadCount { get; private set; }
        private object lck = new object();
        private int _threadCount = 0;

        private ConcurrentQueue<DownloadTask> _tasks = new ConcurrentQueue<DownloadTask>();

        public ImageCache(int threadCount, bool cache)
        {
            UseCache = cache;
            ThreadCount = threadCount;
        }

        public void LoadInto(Handler handler, ImageView view, string url, bool conditionVisible)
        {
            DownloadTask task = new DownloadTask(handler, view, url, conditionVisible);
            _tasks.Enqueue(task);
            lock (lck)
            {
                if(_threadCount < ThreadCount)
                    StartThreading();
            }
        }

        public void StartThreading()
        {
            _threadCount++;
            Thread thread = new Thread(() =>
            {
                DownloadTask task = null;
                do
                {
                    bool didFind = _tasks.TryDequeue(out task);
                    if (!didFind)
                        break;

                    Bitmap bitmap = task.Download();
                    if(bitmap != null)
                    {
                        if (UseCache)
                            Cache(task.Url, bitmap);
                        task.Load(bitmap);
                    }
                }
                while (task != null);
                lock(lck)
                {
                    _threadCount--;
                }
            });
            thread.Start();
        }

        public void Cache(string url, Bitmap bitmap)
        {

        }

        private class DownloadTask
        {
            public bool ConditionVisible { get; private set; }
            public ImageView View { get; private set; }
            public Handler Handler { get; private set; }
            public string Url { get; private set; }

            public DownloadTask(Handler handler, ImageView view, string url, bool condition)
            {
                Handler = handler;
                View = view;
                ConditionVisible = condition;
                Url = url;
            }

            public Bitmap Download()
            {
                if (!ConditionVisible || (ConditionVisible && View.IsShown))
                {
                    using (WebClient client = new WebClient())
                    using (Stream str = client.OpenRead(Url))
                        return BitmapFactory.DecodeStream(str);
                }
                return null;
            }

            public void Load(Bitmap bitmap)
            {
                if (!ConditionVisible || (ConditionVisible && View.IsShown))
                    Handler.Post(() => View.SetImageBitmap(bitmap));
            }
        }
    }
}