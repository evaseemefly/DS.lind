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

namespace Lind.DDD.Android
{
    [Activity(Label = "OrderActivity")]
    public class OrderActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            System.Threading.Thread the = new System.Threading.Thread(() =>
            {
                 
            });
        }
    }
}