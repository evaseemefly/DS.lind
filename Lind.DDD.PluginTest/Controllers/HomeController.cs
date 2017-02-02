using Lind.DDD.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Test.Controllers
{
    public class Info
    {
        public string Area { get; set; }
    }
    public interface DemoP : IPlugins
    {
        string Hello();
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

    public class PD : DemoP
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
    }

    public class PD2 : DemoP
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
    }

    public class PD4 : DemoP
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
    }


    public class HomeController : Controller
    {

        static List<Lind.DDD.Plugins.PluginModel> PluginModelList = new List<Lind.DDD.Plugins.PluginModel>();
        static HomeController()
        {
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型1",
                TypeFullName = "Test.Controllers.PD"
            });
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型2",
                TypeFullName = "Test.Controllers.PD2"
            });
            PluginModelList.Add(new PluginModel
            {
                ModuleName = "DEMOP",
                TypeName = "模型4",
                TypeFullName = "Test.Controllers.PD4"
            });

        }
        public ActionResult Index(string typeName = "Test.Controllers.PD")
        {
            var c = Lind.DDD.Plugins.PluginManager.Resolve<DemoP>(typeName);
            ViewBag.Type = PluginModelList;
            return View(c);
        }

        //
        // GET: /Default1/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Default1/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
