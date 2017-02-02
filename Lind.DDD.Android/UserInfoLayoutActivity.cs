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
    [Activity(Label = "UserInfoLayoutActivity")]
    public class UserInfoLayoutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInfoLayout);//指定要去渲染的视图     
            var listView = FindViewById<ListView>(Resource.Id.userInfoViewMain);
            listView.Adapter = new UserAdapter(this, new UserInfo
            {
                Title = Intent.GetStringExtra("Title"),
                Desc = Intent.GetStringExtra("Desc"),
                AssistsCount = Intent.GetIntExtra("AssistsCount", 0),
                Level = Intent.GetIntExtra("Level", 0),
                Fails = Intent.GetIntExtra("Fails", 0),
                Image = Intent.GetIntExtra("Image", 0),
                Score = Intent.GetIntExtra("Score", 0),
            });
        }
    }
}