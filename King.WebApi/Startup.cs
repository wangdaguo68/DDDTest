using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace King.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #region API文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "King API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                //c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "OA.WebApi.xml");
                //c.IncludeXmlComments(xmlPath);
                //var xmlPath1 = Path.Combine(AppContext.BaseDirectory, "OA.Models.xml");
                //c.IncludeXmlComments(xmlPath1);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "King API V1");

                // Display
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                //c.DefaultModelsExpandDepth(-1);//不显示model
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                //c.EnableDeepLinking();
                //c.EnableFilter();

                // Other
                c.DocumentTitle = "King API文档";
                //css注入
                //c.InjectStylesheet("/swagger-common.css");//自定义样式
                //c.InjectStylesheet("/buzyload/app.min.css");//等待load遮罩层样式
            });
            app.UseMvc();
        }
    }
}
