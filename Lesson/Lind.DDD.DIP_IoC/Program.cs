using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Builder;
using Autofac;
using Autofac.Configuration;
namespace Lind.DDD.DIP_IoC
{
    #region DIP（解决问题的概念）
    /*
   　*　 高级实现－> 高级接口　->　底层实现１
     * 　　　　　　　　　　　  ->  底层实现２
     * 　　　　　　　　　　　　->  氏层实现３
     */
    #endregion

    #region IoC（解决问题的方法）
    public interface IATM
    {
        /// <summary>
        /// 取钱
        /// </summary>
        /// <returns></returns>
        decimal Pull(decimal money);

    }

    public class BankChina : IATM
    {

        public decimal Pull(decimal money)
        {
            Console.WriteLine("中国银行取钱:" + money);
            return money;
        }

    }

    public class BankXingYe : IATM
    {
        public decimal Pull(decimal money)
        {
            Console.WriteLine("兴业银行取钱:" + money);
            return money;
        }
    }
    public interface IDI_Atm
    {

        /// <summary>
        /// 接口依赖注入
        /// </summary>
        /// <param name="iAtm"></param>
        void SetATM(IATM iAtm);
    }
    public class User
    {
        //没有告诉我们如何去实现这种免依赖
        IATM atm = new BankChina();
        public void PullMoney()
        {
            atm.Pull(1000);
        }
    }
    #endregion

    #region DI（对IoC具体的实现，分为构造函数注入，属性注入和接口注入等）
    public class UserDI : IDI_Atm
    {
        #region 构造方法注入（autofac）
        IATM _atm;
        public UserDI(IATM atm)
        {
            _atm = atm;
        }
        public UserDI()
            : this(new BankChina())
        {

        }
        #endregion

        #region 属性注入
        private IATM _pAtm;//定义一个私有变量保存抽象

        //属性，接受依赖
        public IATM PAtm
        {
            set { _pAtm = value; }
            get { return _pAtm; }
        }
        #endregion

        #region 接口注入
        IATM _iAtm;
        public void SetATM(IATM iAtm)
        {
            _iAtm = iAtm;
        }
        #endregion

        public void PullMoney()
        {
            _atm.Pull(1000);
        }


    }
    #endregion

    class Program
    {

        static void Main(string[] args)
        {
            #region Autofac动态注入
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            using (var container1 = builder.Build())
            {
                container1.Resolve<IATM>().Pull(10);
            }
            #endregion
            #region DIP依赖倒置，低层模块依赖于高层模块的接口实现（DIP是软件设计的一种思想）
            //一种原则，一种解决问题的方法
            #endregion

            #region IoC，它是对DIP原则的一种具体实现（IoC则是基于DIP衍生出的一种软件设计模式）
            new User().PullMoney();
            #endregion

            #region DI，它是IoC的具体实现，将低层模块的引用注入到高级模块（DI是IoC的具体实现方式之一）
            new UserDI().PullMoney();
            #endregion

            #region IoC容器，依赖注入的框架，用来映射依赖，管理对象创建和生存周期（DI框架）  var builder = new ContainerBuilder();
            builder.RegisterType(typeof(BankChina)).As(typeof(IATM));
            var container = builder.Build();

            var di = container.Resolve<IATM>();
            di.Pull(100);
            #endregion


        }
    }
}
