using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Office.Work.Platform.Api.AppCodes;
using Office.Work.Platform.Api.DataService;

namespace Office.Work.Platform.Api
{
    /// <summary>
    /// 启动类：
    /// 此类将IdentityServer4 验证授权服务 与 保护的API 放在同一个项目中了。
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<GHDbContext>(option =>
            {
                option.UseMySQL(Configuration["DbConnString"]);
            });
            //允许跨域请求
            services.AddCors(option => option.AddPolicy("cors", 
                policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(new[] { "http://xxx.xxx.com" })));

            //注册IS4服务
            var is4Buider = services.AddIdentityServer()
                  .AddDeveloperSigningCredential()
                  .AddInMemoryApiResources(IS4Config.GetApiResources()) //IS4 导入定义的应用资源API
                  .AddInMemoryIdentityResources(IS4Config.GetIdentityResources())  //IS4 自身API
                  .AddInMemoryClients(IS4Config.GetClients())  //IS4 导入定义的客户端
                  .AddResourceOwnerValidator<ResourceOwnerValidator>();//IS4 自数据库验证用户类


            //注册验证（*用于保护API资源，与IS4无关* ） 
            services.AddAuthentication("Bearer").AddJwtBearer(r =>
            {
                //认证服务地址
                r.Authority = "http://localhost:5000";
                //权限标识
                r.Audience = "Apis";
                //是否必需HTTPS
                r.RequireHttpsMetadata = false;
            });

            services.AddControllers();
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Configuration["StaticFileDir"]),
                RequestPath = @"/GHStaticFiles"
            });
            app.UseCors("cors");
            app.UseRouting();

            //IS4Server
            app.UseIdentityServer();

            //添加验证（*用于保护API资源，与IS4无关* ） 注意，验证中间件必须放在授权之前。
            app.UseAuthentication();
            //添加授权（*用于保护API资源，与IS4无关* ）
            app.UseAuthorization();
            //上两要放在app.UseRouting、app.UseCors("cors")之后，并且在app.UseEndpoints之前

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
