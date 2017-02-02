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
using System.Threading;
using Java.Net;
using Lind.DDD.AndroidLib.Alipay;
using Android;

namespace Lind.DDD.AndroidLib.Alipay
{
    /// <summary>
    /// 支付宝实现者
    /// </summary>
    public class AlipayImplement
    {
        public AlipayImplement(Activity activityContext)
        {
            context = activityContext;
            //partner = context.GetString(Resource.String.Partner).Trim();
            //seller = context.GetString(Resource.String.Seller).Trim();
            //rsa_private = context.GetString(Resource.String.Rsa_private).Trim();
            //notify_url = context.GetString(Resource.String.AlipayNotify).Trim();

        }
        #region Fields
        /// <summary>
        ///  合作商户ID。用签约支付宝账号登录ms.alipay.com后，在账户信息页面获取
        /// </summary>
        string partner;
        /// <summary>
        /// 商户收款的支付宝账号
        /// </summary>
        string seller;
        /// <summary>
        /// 商户私钥
        /// </summary>
        string rsa_private;
        /// <summary>
        /// 服务端回调地址，需要能让支付宝访问到的
        /// </summary>
        string notify_url;
        /// <summary>
        /// 当前android上下文
        /// </summary>
        Activity context;
        #endregion

        /// <summary>
        /// 支付动作
        /// </summary>
        public void Pay(String subject, String body, decimal money)
        {
            try
            {
                var con = GetOrderInfo(subject, body, money);
                // 特别注意，这里的签名逻辑需要放在服务端，切勿将私钥泄露在代码中！
                var sign = SignatureUtils.Sign(con, rsa_private);
                sign = URLEncoder.Encode(sign, "UTF-8");
                con += "&sign=\"" + sign + "\"&" + MySignType;
                Com.Alipay.Sdk.App.PayTask pa = new Com.Alipay.Sdk.App.PayTask(context);

                //  con = "partner=\"2088101568358171\"&seller_id=\"xxx@alipay.com\"&out_trade_no=\"YR2VGG3G1I31XDZ\"&subject=\"1\"&body=\"我是测试数据\"&total_fee=\"0.01\"&notify_url=\"http://111.203.248.34:89/Order/AlipayNotify\"&service=\"mobile.securitypay.pay\"&payment_type=\"1\"&_input_charset=\"utf-8\"&it_b_pay=\"30m\"&sign=\"GsSZgPloF1vn52XAItRAldwQAbzIgkDyByCxMfTZG%2FMapRoyrNIJo4U1LUGjHp6gdBZ7U8jA1kljLPqkeGv8MZigd3kH25V0UK3Jc3C94Ngxm5S%2Fz5QsNr6wnqNY9sx%2Bw6DqNdEQnnks7PKvvU0zgsynip50lAhJmflmfHvp%2Bgk%3D\"&sign_type=\"RSA\"";
                Logger_Info(con);
                var result = pa.Pay(con,false);
                Logger_Info("支付宝result:" + result);
                //调用结果查看result中是否返回是90000,如果是，则成功      
            }
            catch (Exception ex)
            {

                Logger_Info("2" + ex.Message + ex.StackTrace);
            }

        }
        #region Private Methods
        /// <summary>
        /// Alipay日志
        /// </summary>
        /// <param name="msg"></param>
        private void Logger_Info(string msg)
        {
            using (System.IO.StreamWriter srFile = new System.IO.StreamWriter("/sdcard/Alipay.txt", true))
            {
                srFile.WriteLine(string.Format("{0}{1}{2}"
                    , DateTime.Now.ToString().PadRight(20)
                    , ("[ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "]").PadRight(14)
                    , msg));
                srFile.Close();
                srFile.Dispose();
            }
        }
        /// <summary>
        /// 组织支付宝订单信息
        /// </summary>
        /// <param name="notify_url">回调地址，就是支付宝skd的server-demo,而app一般叫client-demo</param>
        /// <param name="subject">商品名称</param>
        /// <param name="body">商品详情</param>
        /// <param name="money">价格</param>
        /// <returns></returns>
        String GetOrderInfo(String subject, String body, decimal money)
        {
            // 签约合作者身份ID
            String orderInfo = "partner=" + "\"" + partner + "\"";
            // 签约卖家支付宝账号
            orderInfo += "&seller_id=" + "\"" + seller + "\"";
            // 商户网站唯一订单号
            orderInfo += "&out_trade_no=" + "\"DJ" + DateTime.Now.ToString("yyyyMMddhhmmss") + "\"";
            // 商品名称
            orderInfo += "&subject=" + "\"" + subject + "\"";
            // 商品详情
            orderInfo += "&body=" + "\"" + body + "\"";
            // 商品金额
            orderInfo += "&total_fee=" + "\"" + money + "\"";
            // 服务器异步通知页面路径
            orderInfo += "&notify_url=\"" + notify_url + "\"";
            // 服务接口名称， 固定值
            orderInfo += "&payment_type=\"1\"";

            //    orderInfo += "&rsaPubkey =" + "\"" + alipayWAPPublicKey + "\"";

            // 参数编码， 固定值

            orderInfo += "&_input_charset=\"utf-8\"";

            // 设置未付款交易的超时时间

            // 默认30分钟，一旦超时，该笔交易就会自动被关闭。

            // 取值范围：1m～15d。

            // m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。

            // 该参数数值不接受小数点，如1.5h，可转换为90m。
            orderInfo += "&it_b_pay=\"30m\"";

            // extern_token为经过快登授权获取到的alipay_open_id,带上此参数用户将使用授权的账户进行支付

            // orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

            // 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空

            orderInfo += "&return_url=\"" + notify_url + "\"";

            // 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能使用）

            // orderInfo += "&paymethod=\"expressGateway\"";

            return orderInfo;

        }
        /// <summary>
        /// 签名
        /// </summary>
        String MySignType
        {

            get
            {

                return "sign_type=\"RSA\"";

            }

        }

        #endregion

    }
}