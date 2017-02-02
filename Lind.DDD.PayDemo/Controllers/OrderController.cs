using Lind.DDD.OnlinePay.Weixin;
using Lind.DDD.OnlinePay.WeixinJSApi;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.PayDemo.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        #region JSAPI直接支付
        public void GetOpenId()
        {
            JsApiImplement.GetOpenId();
        }
        #endregion

        #region JSAPI支付-代理
        /// <summary>
        /// 微信支付
        /// 从代理服务器获取openID
        /// 注意：代理服务器与本服务器需要有预定，双方要告诉对应的URL地址，用来进行二次跳转
        /// </summary>
        /// <returns></returns>
        public ActionResult Weixin()
        {
            string openId = Request.QueryString["openId"];
            if (string.IsNullOrWhiteSpace(openId))
            {
                //第一次加载页面，跳转到代理服务器获openid
                Logger.LoggerFactory.Instance.Logger_Info("openid为空");
                Response.Redirect("http://vyxa.haoban173.com/authorization/?url=" + Request.Url.AbsoluteUri);

            }
            else
            {
                //由代理服务器跳回来后，得到本用户的openId
                Logger.LoggerFactory.Instance.Logger_Info("从m158拿到openid:" + openId);
                ViewBag.openId = openId;
            }
            return View();
        }

        /// <summary>
        /// 微信支付的代理服务
        /// </summary>
        /// <returns></returns>
        public void WeixinProxy()
        {
            string openId = JsApiImplement.GetOpenId();//获取当前openId
            string bussinessUrl = "";//业务网站支付地址
            if (!string.IsNullOrWhiteSpace(openId))
                Redirect(bussinessUrl + "?openId=" + openId);
        }
        /// <summary>
        /// 获取支付JSON串
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="money"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string GetPayJson(string orderId, int money, string openId)
        {
            try
            {
                Logger.LoggerFactory.Instance.Logger_Info("发送订单号=" + orderId + ",openID=" + openId);
                //JsApiImplement.GetOpenId();加到主页面上
                return JsApiImplement.Send(money, orderId, openId);

            }
            catch (Exception ex)
            {

                Logger.LoggerFactory.Instance.Logger_Error(ex);
                return "出错了";
            }

        }

        //微信回调
        public void Notify()
        {
            JsApiImplement.Notify((model) =>
            {
                Logger.LoggerFactory.Instance.Logger_Info("回调订单号" + model.Out_Trade_No);
                if (model.IsSuccess)
                {
                    //微信回调成功
                    //更新领域订单状态
                }
                else
                {
                    //微信回调失败
                }


            });
        }

        #endregion


        #region 二维码支持
        WxPayImpl wxPayImpl = new WxPayImpl();
        /// <summary>
        /// 发送支付请求
        /// </summary>
        public void Send()
        {
            WxPayImpl wxPayImpl = new WxPayImpl();

            QRCodeHelper.OutPutQRCodeImage(wxPayImpl.RechargeTo("order001", 1, 1, "product"), Response);
        }

        #endregion



    }
}
