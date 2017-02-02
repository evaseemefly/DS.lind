using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace Lind.DDD.Android
{

    [Activity(Label = "请选择用户")]
    public class ChooseUserActivity : Activity
    {
        private List<UserInfo> datas;
        ListView listView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ChooseUserLayout);
            listView = FindViewById<ListView>(Resource.Id.listViewMain);
            //加载假数据
            datas = new List<UserInfo>();
            #region 一级
            datas.Add(new UserInfo
           {
               Title = "刘淼（中脑袋）",
               Desc = "擅长连续用头攻击对方",
               Image = Resource.Drawable.miao,
               AddTime = DateTime.Now,
               AssistsCount = 0,
               Fails = 10,
               Score = 200,
               Level = 1,
           });
            datas.Add(new UserInfo
            {
                Title = "安子",
                Desc = "擅长倒勾",
                Image = Resource.Drawable.anliyu,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 10,
                Score = 300,
                Level = 1,
            });
            datas.Add(new UserInfo
            {
                Title = "宋師傅",
                Desc = "擅长快速移动",
                Image = Resource.Drawable.song,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 20,
                Score = 300,
                Level = 1,
            });
            datas.Add(new UserInfo
            {
                Title = "张明",
                Desc = "擅长后场暴杆",
                Image = Resource.Drawable.ming,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 20,
                Score = 200,
                Level = 1,
            });

            #endregion

            #region 二级
            datas.Add(new UserInfo
            {
                Title = "邹老板",
                Desc = "擅长用计",
                Image = Resource.Drawable.zou,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 50,
                Score = 150,
                Level = 2,
            });
            datas.Add(new UserInfo
            {
                Title = "臧师傅（五金鞋，毽杀手）",
                Desc = "擅长脚工",
                Image = Resource.Drawable.zang,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 50,
                Score = 100,
                Level = 2,
            });
            datas.Add(new UserInfo
            {
                Title = "朱月龙（西毒，攻击力）",
                Desc = "擅长发球",
                Image = Resource.Drawable.zhu,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 50,
                Score = 80,
                Level = 2,
            });
            datas.Add(new UserInfo
            {
                Title = "宁哥（脚工暴推）",
                Desc = "擅长发球",
                Image = Resource.Drawable.ning,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 50,
                Score = 80,
                Level = 2,
            });
            #endregion

            #region 三级
            datas.Add(new UserInfo
            {
                Title = "占岭（北腿）",
                Desc = "擅长用左腿和右腿",
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 100,
                Score = 50,
                Level = 3,
                Image = Resource.Drawable.ling,
            });
            datas.Add(new UserInfo
            {
                Title = "李超（裁判）",
                Desc = "擅长用非正常动作",
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 100,
                Score = 50,
                Level = 3,
                Image = Resource.Drawable.li,
            });
            datas.Add(new UserInfo
            {
                Title = "达哥（南拳）",
                Desc = "擅长用双手太极",
                Image = Resource.Drawable.da,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 10,
                Score = 100,
                Level = 3,
            });
            datas.Add(new UserInfo
            {
                Title = "小处",
                Desc = "擅长用脚工",
                Image = Resource.Drawable.chu,
                AddTime = DateTime.Now,
                AssistsCount = 0,
                Fails = 10,
                Score = 50,
                Level = 3,
            });
            #endregion
            listView.Adapter = new UserListAdapter(this, datas);
            listView.ItemClick += listView_ItemClick;
        }

        /// <summary>
        /// 点选item 后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "你选择了 " + datas[e.Position].Title, ToastLength.Short).Show();

            Intent intent = new Intent(this, typeof(UserInfoLayoutActivity));
            /* 通过Bundle对象存储需要传递的数据 */
            Bundle bundle = new Bundle();
            /*字符、字符串、布尔、字节数组、浮点数等等，都可以传*/
            intent.PutExtra("Title", datas[e.Position].Title);
            intent.PutExtra("Desc", datas[e.Position].Desc);
            intent.PutExtra("AssistsCount", datas[e.Position].AssistsCount);
            intent.PutExtra("Fails", datas[e.Position].Fails);
            intent.PutExtra("Score", datas[e.Position].Score);
            intent.PutExtra("Level", datas[e.Position].Level);
            intent.PutExtra("Image", datas[e.Position].Image);
            /*把bundle对象assign给Intent*/

            intent.PutExtras(bundle);
            StartActivity(intent);
        }
    }
}

