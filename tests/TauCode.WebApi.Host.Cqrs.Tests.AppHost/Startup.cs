using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TauCode.Domain.NHibernate.Types;
using TauCode.WebApi.Host.Cqrs.Tests.Core;

namespace TauCode.WebApi.Host.Cqrs.Tests.AppHost
{
    public class Startup : IStartupHelperHost
    {
        private IStartupHelper _startupHelper;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var cqrsAssembly = typeof(CoreBeacon).Assembly;

            services.AddControllers(options => options.Filters.Add(new ValidationFilterAttribute(cqrsAssembly)));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Demo Server RESTful Service",
                        Version = "v1"
                    });
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
            });

            var startupHelper = this.GetStartupHelper();
            startupHelper.Init(services);

            this.SQLiteTestConfigurationBuilder = new TestSQLiteTestConfigurationBuilder();

            startupHelper
                .AddCqrs(
                    cqrsAssembly,
                    typeof(TestTransactionalCommandHandlerDecorator<>))
                .AddNHibernate(
                    this.SQLiteTestConfigurationBuilder.Configuration,
                    typeof(Startup).Assembly,
                    typeof(SQLiteIdUserType<>));

            // finalize
            _startupHelper.Accomplish();
            var container = _startupHelper.GetContainer();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Server RESTful Service"); });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public IStartupHelper GetStartupHelper() => _startupHelper ??= new StartupHelper();

        public TestSQLiteTestConfigurationBuilder SQLiteTestConfigurationBuilder { get; private set; }
    }
}
