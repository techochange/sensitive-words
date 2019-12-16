using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using Badmati.BinarySpace.Infrastructure.OrmDapper;
using Badmati.BinarySpace.Services.AppService;
using Badmati.BinarySpace.Services.IAppService;
using Badmati.BinarySpace.WebApi.Middleware;
using StackExchange.Exceptional.Stores;
using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Infrastructure.AutoFac;
using Badmati.BinarySpace.WebApi.HostService;
using Badmati.BinarySpace.WebApi.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Badmati.BinarySpace.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Dapper;
using Badmati.BinarySpace.Services.IRefresh;
using Badmati.BinarySpace.Infrastructure.Quartz;
using Badmati.BinarySpace.WebApi.TaskJob;

namespace Badmati.BinarySpace.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer AppContainer { get; private set; }
        private IServiceProvider serviceProvider = null;
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ;
            //services.AddWebSocketManager();
            services.AddMvc(option=> {
                    option.Filters.Add<BmdExceptionFilter>();
                })                
                .AddJsonOptions(option => {
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            /*
            services.AddExceptional(setting =>
            { 
                //setting.DefaultStore = new SQLErrorStore(Configuration["ExceptionalStr"], Configuration["ExceptionalAppName"]);
            });
            */
            services.AddCors(options =>
            {
                options.AddPolicy("any", corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin() //允许任何来源的主机访问                    
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });

            //services.AddHostedService<CacheRefreshService>();
            //初始化后台调度线程
            #region Time Task
#if !DEBUG
            services.AddHostedService<QuartzHostedService>()
                .InitTask()
                .AddQuartzHostedService();
#endif
            #endregion

            //Redis缓存，如果没有redis，可以注释掉，把相应调用的地方拿掉
            //初始化redis链接字符串,如果不起用redis缓存可以注释掉
            //BadmatiRedisHelper.Initialization(Configuration["RedisConStr"],Configuration["prefix"]);
            //添加swagger json 支持
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "二进制世界", Version = "v2" });
            });
#endif
            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterIOC(builder);

            this.AppContainer = builder.Build();
            serviceProvider = new AutofacServiceProvider(this.AppContainer);
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {

            loggerFactory.RegisterLoggerFactory();//使用log4net
            FrameworkUtilsConfigure.Configure(serviceProvider, Configuration, loggerFactory);
            //ConstStr.TOTAL_FEE = int.Parse(Configuration["total_fee"]);

            //加载数据库连接字符串
            SystemConfig.DB_CONNECTION_STRTING = Configuration["ConnectionStr"];

            //四大名著id
            SystemConfig.FOUR_BOOKIDS = Configuration["FourBookIds"];
            SystemConfig.SUGGEST_COUNT = int.Parse(Configuration["SuggestCount"]);

            //注册数据库为mysql
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);

            //app.UseWebSockets();

            //app.MapWebSocketManager("/ws", serviceProvider.GetService<ChartHandler>());

            app.UseCors("any");
#if DEBUG
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V1");
            });
#endif
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            app.UseMvc();
        }

        /// <summary>
        /// 使用autofac自动注入程序集的功能
        /// 自动注入不同生命周期的类和接口
        /// </summary>
        /// <param name="containerBuilder"></param>
        private void RegisterIOC(ContainerBuilder containerBuilder)
        {
            //扫描程序集
            var allAssemblies = new List<Assembly>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
            foreach (var item in Directory.GetFiles(path, "Badmati*.dll"))
            {
                FileInfo fi = new FileInfo(item);
                var temp = Assembly.LoadFrom(item);//loadfrom 相比load 多一次文件打开
                allAssemblies.Add(temp);
            }

            var iServices = allAssemblies.ToArray(); 
            //注册泛型增删改查
            containerBuilder.RegisterGeneric(typeof(BaseDapperRepository<>)).As(typeof(IDapperRepository<>))
                .InstancePerLifetimeScope();

            Type baseTransientType = typeof(ITransient); 
            containerBuilder.RegisterAssemblyTypes(iServices)
                .Where(type => baseTransientType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerDependency();

            Type baseScopedType = typeof(IScoped); 
            containerBuilder.RegisterAssemblyTypes(iServices)
                .Where(type => baseScopedType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            Type baseSingletonType = typeof(ISingleton); 
            containerBuilder.RegisterAssemblyTypes(iServices)
                .Where(type => baseSingletonType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().SingleInstance(); 
        }
    }

}
