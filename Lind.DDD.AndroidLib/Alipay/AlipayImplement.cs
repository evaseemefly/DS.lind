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
    /// ֧����ʵ����
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
        ///  �����̻�ID����ǩԼ֧�����˺ŵ�¼ms.alipay.com�����˻���Ϣҳ���ȡ
        /// </summary>
        string partner;
        /// <summary>
        /// �̻��տ��֧�����˺�
        /// </summary>
        string seller;
        /// <summary>
        /// �̻�˽Կ
        /// </summary>
        string rsa_private;
        /// <summary>
        /// ����˻ص���ַ����Ҫ����֧�������ʵ���
        /// </summary>
        string notify_url;
        /// <summary>
        /// ��ǰandroid������
        /// </summary>
        Activity context;
        #endregion

        /// <summary>
        /// ֧������
        /// </summary>
        public void Pay(String subject, String body, decimal money)
        {
            try
            {
                var con = GetOrderInfo(subject, body, money);
                // �ر�ע�⣬�����ǩ���߼���Ҫ���ڷ���ˣ�����˽Կй¶�ڴ����У�
                var sign = SignatureUtils.Sign(con, rsa_private);
                sign = URLEncoder.Encode(sign, "UTF-8");
                con += "&sign=\"" + sign + "\"&" + MySignType;
                Com.Alipay.Sdk.App.PayTask pa = new Com.Alipay.Sdk.App.PayTask(context);

                //  con = "partner=\"2088101568358171\"&seller_id=\"xxx@alipay.com\"&out_trade_no=\"YR2VGG3G1I31XDZ\"&subject=\"1\"&body=\"���ǲ�������\"&total_fee=\"0.01\"&notify_url=\"http://111.203.248.34:89/Order/AlipayNotify\"&service=\"mobile.securitypay.pay\"&payment_type=\"1\"&_input_charset=\"utf-8\"&it_b_pay=\"30m\"&sign=\"GsSZgPloF1vn52XAItRAldwQAbzIgkDyByCxMfTZG%2FMapRoyrNIJo4U1LUGjHp6gdBZ7U8jA1kljLPqkeGv8MZigd3kH25V0UK3Jc3C94Ngxm5S%2Fz5QsNr6wnqNY9sx%2Bw6DqNdEQnnks7PKvvU0zgsynip50lAhJmflmfHvp%2Bgk%3D\"&sign_type=\"RSA\"";
                Logger_Info(con);
                var result = pa.Pay(con,false);
                Logger_Info("֧����result:" + result);
                //���ý���鿴result���Ƿ񷵻���90000,����ǣ���ɹ�      
            }
            catch (Exception ex)
            {

                Logger_Info("2" + ex.Message + ex.StackTrace);
            }

        }
        #region Private Methods
        /// <summary>
        /// Alipay��־
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
        /// ��֧֯����������Ϣ
        /// </summary>
        /// <param name="notify_url">�ص���ַ������֧����skd��server-demo,��appһ���client-demo</param>
        /// <param name="subject">��Ʒ����</param>
        /// <param name="body">��Ʒ����</param>
        /// <param name="money">�۸�</param>
        /// <returns></returns>
        String GetOrderInfo(String subject, String body, decimal money)
        {
            // ǩԼ���������ID
            String orderInfo = "partner=" + "\"" + partner + "\"";
            // ǩԼ����֧�����˺�
            orderInfo += "&seller_id=" + "\"" + seller + "\"";
            // �̻���վΨһ������
            orderInfo += "&out_trade_no=" + "\"DJ" + DateTime.Now.ToString("yyyyMMddhhmmss") + "\"";
            // ��Ʒ����
            orderInfo += "&subject=" + "\"" + subject + "\"";
            // ��Ʒ����
            orderInfo += "&body=" + "\"" + body + "\"";
            // ��Ʒ���
            orderInfo += "&total_fee=" + "\"" + money + "\"";
            // �������첽֪ͨҳ��·��
            orderInfo += "&notify_url=\"" + notify_url + "\"";
            // ����ӿ����ƣ� �̶�ֵ
            orderInfo += "&payment_type=\"1\"";

            //    orderInfo += "&rsaPubkey =" + "\"" + alipayWAPPublicKey + "\"";

            // �������룬 �̶�ֵ

            orderInfo += "&_input_charset=\"utf-8\"";

            // ����δ����׵ĳ�ʱʱ��

            // Ĭ��30���ӣ�һ����ʱ���ñʽ��׾ͻ��Զ����رա�

            // ȡֵ��Χ��1m��15d��

            // m-���ӣ�h-Сʱ��d-�죬1c-���죨���۽��׺�ʱ����������0��رգ���

            // �ò�����ֵ������С���㣬��1.5h����ת��Ϊ90m��
            orderInfo += "&it_b_pay=\"30m\"";

            // extern_tokenΪ���������Ȩ��ȡ����alipay_open_id,���ϴ˲����û���ʹ����Ȩ���˻�����֧��

            // orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

            // ֧��������������󣬵�ǰҳ����ת���̻�ָ��ҳ���·�����ɿ�

            orderInfo += "&return_url=\"" + notify_url + "\"";

            // �������п�֧���������ô˲���������ǩ���� �̶�ֵ ����ҪǩԼ���������п����֧��������ʹ�ã�

            // orderInfo += "&paymethod=\"expressGateway\"";

            return orderInfo;

        }
        /// <summary>
        /// ǩ��
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