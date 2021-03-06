using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Comments.Data;
using App.Comments.Data.Repositories;
using App.Comments.Common.Interfaces.Repositories;
using App.Comments.Common.Interfaces.Services;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using App.Comments.Web;
using Microsoft.AspNetCore.SpaServices.Webpack;
using App.Comments.Services;

namespace NewAngularCommentsApplication
{
    public class Startup
    {
		private IHostingEnvironment CurrentEnvironment { get; set; }
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			CurrentEnvironment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile("appsettings-Shared.json", optional: false)
				.AddJsonFile("secrets.json", optional: false)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

        public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy("AllowAny", x => {
				x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
			}));

			services.AddDbContext<CommentsContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddTransient(typeof(ICommentRepository), typeof(CommentRepository));
			services.AddTransient(typeof(IApplicationUserRepository), typeof(UserRepository));

			services.AddTransient(typeof(ICommentsService), typeof(CommentsService));
			services.AddTransient(typeof(IAuthenticationService), typeof(AuthenticationService));
			services.AddTransient(typeof(ITestsService), typeof(TestsService));

			services.AddAuthentication().AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
				facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
			});

			services.AddMvc().AddJsonOptions(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});

			services.AddSignalR();
			services.AddAutoMapper();
		}


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true
				});
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseCors("AllowAny");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseWebSockets();

			app.UseMvcWithDefaultRoute();
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "DefaultApi",
					template: "api/{controller}/{action}");
			});

			app.UseSignalR(routes =>
			{
				routes.MapHub<CommentsPublisher>("commentsPublisher");
			});

			RouteBuilder routeBuilder = new RouteBuilder(app);
        }
    }
}
