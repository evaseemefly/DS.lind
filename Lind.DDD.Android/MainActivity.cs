using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Graphics;
using System.Net;
using Android.Locations;

namespace Lind.DDD.Android
{
    [Activity(Label = "Lind.DDD.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                StartActivity(typeof(ChooseUserActivity));

            };
            
            // gps位置信息
            LocationManager lm = (LocationManager)GetSystemService(LocationService);
            lm.RequestLocationUpdates(LocationManager.GpsProvider, 5000, 50f, this);//位置变化为50米时间为5s，更新一次gps
        }




        #region ILocationListener 成员

        public void OnLocationChanged(Location location)
        {
            String s = String.Format("{0}   {1}", location.Longitude, location.Latitude);
            Toast.MakeText(ApplicationContext, s, ToastLength.Short).Show();
        }
        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #region 列表标签

    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public int Image { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 总进球
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 直接失误数，乌龙
        /// </summary>
        public int Fails { get; set; }
        /// <summary>
        /// 帮助队友得分数，助攻
        /// </summary>
        public int AssistsCount { get; set; }
        /// <summary>
        /// 入会时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }

    /// <summary>
    /// 用户列表失陪器
    /// </summary>
    public class UserListAdapter : BaseAdapter<UserInfo>
    {
        /// <summary>
        /// 所有UserInof 的数据
        /// </summary>
        List<UserInfo> items;

        Activity context;

        public UserListAdapter(Activity context, List<UserInfo> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override UserInfo this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// 系统会呼叫 并且render.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            var view = convertView;
            if (view == null)
            {
                //使用自订的UserListItemLayout
                view = context.LayoutInflater.Inflate(Resource.Layout.UserListItemLayout, null);
            }

            view.FindViewById<TextView>(Resource.Id.tvName).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.tvDesc).Text = item.Desc;
            view.FindViewById<TextView>(Resource.Id.tvFails).Text = item.Fails.ToString();
            view.FindViewById<TextView>(Resource.Id.tvScore).Text = item.Score.ToString();
            view.FindViewById<TextView>(Resource.Id.tvLevel).Text = item.Level.ToString();
            view.FindViewById<TextView>(Resource.Id.tvAssistsCount).Text = item.AssistsCount.ToString();
            view.FindViewById<ImageView>(Resource.Id.imgUser).SetImageResource(item.Image);

            return view;
        }



    }
    public class UserAdapter : BaseAdapter<UserInfo>
    {
        /// <summary>
        /// 所有UserInof 的数据
        /// </summary>
        UserInfo item;

        Activity context;

        public override UserInfo this[int position]
        {
            get { return item; }
        }
        public override int Count
        {
            get { return 1; }
        }

        public UserAdapter(Activity context, UserInfo item)
            : base()
        {
            this.context = context;
            this.item = item;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// 系统会呼叫 并且render.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var view = convertView;
            if (view == null)
            {
                //使用自订的UserListItemLayout
                view = context.LayoutInflater.Inflate(Resource.Layout.UserInfoDetailLayout, null);
            }
            view.FindViewById<TextView>(Resource.Id.tvName).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.tvDesc).Text = item.Desc;
            view.FindViewById<TextView>(Resource.Id.tvFails).Text = item.Fails.ToString();
            view.FindViewById<TextView>(Resource.Id.tvScore).Text = item.Score.ToString();
            view.FindViewById<TextView>(Resource.Id.tvLevel).Text = item.Level.ToString();
            view.FindViewById<TextView>(Resource.Id.tvAssistsCount).Text = item.AssistsCount.ToString();
            view.FindViewById<ImageView>(Resource.Id.imgUser).SetImageResource(item.Image);
            return view;
        }

    }
    public class XamarinHelper
    {
        /// <summary>
        /// 因为图片是网址，所以将其图片download回来后转为bitmap
        /// Get IamgeBitmap form url.
        /// code reference : http://forums.xamarin.com/discussion/4323/image-from-url-in-imageview
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
    #endregion
}

