using Lind.DDD.LindPlugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Lind.DDD.Plugins.Controllers
{
    public class Info
    {
        public string Area { get; set; }
    }
    public interface IDemoP : IPlugins
    {
        string Hello();
        void AddNews();
        void EditNews();
    }
    public enum Style
    {
        [Description("普通")]
        Normal,
        [Description("线")]
        Line,
        [Description("圆")]
        Circle
    }


    public class Address
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
    }

    public class PD : IDemoP
    {
        #region DemoP 成员

        public string Hello()
        {
            return "枣";
        }
        public PD()
        {
            Type = Controllers.Style.Circle;
            Type2 = Controllers.Style.Circle;
            Birthday = DateTime.Now;
            var city = new List<SelectListItem>();
            city.Add(new SelectListItem { Text = "华北", Value = "1" });
            city.Add(new SelectListItem { Text = "东北", Value = "2" });
            city.Add(new SelectListItem { Text = "华南", Value = "3" });

            this.City = city;
        }

        [DisplayName("标题")]
        public string Name { get; set; }
        [DisplayName("年纪")]
        public int Age { get; set; }
        [DisplayName("Email")]
        [UIHint("MultilineText")]
        public string Email { get; set; }
        [DisplayName("类型_EnumRadio"), EnumDataType(typeof(Style))]
        [UIHint("_EnumRadio")]
        public Style Type { get; set; }
        [DisplayName("类型_EnumCheckbox"), EnumDataType(typeof(Style))]
        [UIHint("_EnumCheckbox")]
        public Style Type2 { get; set; }
        [DisplayName("类型_EnumDropdownList"), EnumDataType(typeof(Style))]
        [UIHint("_EnumDropdownList")]
        public Style Type3 { get; set; }
        [DisplayName("出生日期")]
        [UIHint("_DateTime")]
        public DateTime Birthday { get; set; }
        [DisplayName("性别")]
        [UIHint("Bool")]
        public bool Sex { get; set; }
        [DisplayName("大区")]
        [UIHint("_DropDownList")]
        public IList<SelectListItem> City { get; set; }
        [DisplayName("详细地址")]
        [UIHint("_EntityDisplay")]
        public Info Info { get; set; }

        public Address Address { get; set; }
        #endregion

        #region IDemoP 成员


        public void AddNews()
        {
            System.Web.HttpContext.Current.Response.Write("添加新闻PD");
        }

        public void EditNews()
        {
            System.Web.HttpContext.Current.Response.Write("编辑新闻PD");

        }

        #endregion
    }

    public class PD2 : IDemoP
    {
        #region DemoP 成员

        public string Hello()
        {
            return "草莓";
        }
        [DisplayName("标题")]
        public string Name { get; set; }
        [DisplayName("年纪")]
        public int Age { get; set; }
        #endregion

        #region IDemoP 成员


        public void AddNews()
        {
            System.Web.HttpContext.Current.Response.Write("添加新闻PD2");

        }

        public void EditNews()
        {
            System.Web.HttpContext.Current.Response.Write("编辑新闻PD2");

        }

        #endregion
    }

    public class PD4 : IDemoP
    {
        #region DemoP 成员

        public string Hello()
        {
            return "苹果";
        }
        [DisplayName("标题")]
        public string Name { get; set; }
        [DisplayName("年纪")]
        public int Age { get; set; }
        #endregion

        #region IDemoP 成员


        public void AddNews()
        {
            System.Web.HttpContext.Current.Response.Write("添加新闻PD4");

        }

        public void EditNews()
        {
            System.Web.HttpContext.Current.Response.Write("编辑新闻PD4");

        }

        #endregion
    }


    public class HomeController : Controller
    {

        static List<PluginModel> PluginModelList = new List<PluginModel>();
        static IDemoP behavor;

        static HomeController()
        {
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型1",
                TypeFullName = "Lind.DDD.Plugins.Controllers.PD"
            });
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型2",
                TypeFullName = "Lind.DDD.Plugins.Controllers.PD2"
            });
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型4",
                TypeFullName = "Lind.DDD.Plugins.Controllers.PD4"
            });

        }
        public ActionResult Index(string typeName = "Lind.DDD.Plugins.Controllers.PD")
        {
            behavor = PluginManager.Resolve<IDemoP>(typeName);

            ViewBag.Type = PluginModelList;
            return View(behavor);
        }

    }
}
