﻿using Autofac;
using Autofac.Integration.Mvc;
using KiaserMid;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KiaserWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
      

        protected void Application_Start()
        {
            LogHelper.WriteInfo(" 开始..");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //注册EF 追踪
            MiniProfilerEF6.Initialize();

            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll").Select(Assembly.LoadFrom).ToArray();
            //创建autofac管理注册类的容器实例
            var builder = new ContainerBuilder();
            //注册所有实现了 IDependency 接口的类型
            Type baseType = typeof(IDependency);
            builder.RegisterAssemblyTypes(assemblies)
            //.Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract || type.GetCustomAttribute(typeof(ServiceAttribute)) != null)
            .Where(t => baseType.IsAssignableFrom(t) || baseType.IsAssignableFrom(t) && !t.IsAbstract)
            .AsSelf().AsImplementedInterfaces()
            .PropertiesAutowired().InstancePerLifetimeScope();

            //使用Autofac提供的RegisterControllers扩展方法来对程序集中所有的Controller一次性的完成注册
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterFilterProvider();
            //生成具体的实例
            var container = builder.Build();
            //下面就是使用MVC的扩展 更改了MVC中的注入方式.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        protected void Application_BeginRequest()
        {
            MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}
