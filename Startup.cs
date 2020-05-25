using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Api.IdentityUser;
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
                option.UseMySql(Configuration["DbConnString"]);
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
                  .AddResourceOwnerValidator<IS4UserValidator>() //IS4 自数据库验证用户类
                  .AddProfileService<IS4ProfileService>();//IS4 自数据库验证用户类


            //注册验证（*用于被保护的API资源，与IS4无关* ） 
            string ProtectApiUrl = Configuration["ProtectApiUrl"];
            services.AddAuthentication("Bearer").AddJwtBearer(r =>
            {
                //是否必需HTTPS
                r.RequireHttpsMetadata = false;
                //认证服务地址(由于本项目APi资源与IS4服务器均在一起，故地址相同)
                r.Authority = ProtectApiUrl;
                //权限标识
                r.Audience = "PlatformApis";
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不更改元数据的key的大小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }); 

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
            //不启用静态文件，以防止用户直接访问文件而绕过授权。
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Configuration["StaticFileDir"]),
            //    RequestPath = @"/GHStaticFiles"
            //});

            //允许跨域请求
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
