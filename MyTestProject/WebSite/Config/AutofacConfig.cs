using Autofac;
using Autofac.Integration.Mvc;
using Dal;
using IDal;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using Utlis.Autofac;

namespace WebSite.Config
    public class AutofacConfig
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialise()
        {
            var builder = RegisterService();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

        public static ContainerBuilder RegisterService()
        {
            var builder = new ContainerBuilder();

            var baseType = typeof(IAutofac);
            //var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();

            builder.RegisterControllers(assemblys.ToArray());

            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerRequest();

            return builder;
        }
    }
}