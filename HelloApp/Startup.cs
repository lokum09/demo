using HelloApp.Model;
using HelloApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace WebApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			#if DEBUG
				Console.WriteLine($"Running at pid {System.Diagnostics.Process.GetCurrentProcess().Id}");
			#endif

			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			//app.UseMvc(routes =>
			//{
			//	routes
			//		.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}")
			//		.MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
			//});

			services.AddDbContext<HelloAppContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("HelloApp"))
			);

			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new Info { Title = "HelloApp API", Version = "V1" });
			});

			//services.AddScoped<IBlogService, BlogService>();
			//services.AddTransient<BlogService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc(routes =>
			{
				//routes
					//.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}")
					//.MapRoute(name: "api", template: "api/{controller}/{action=Get}/{id?}");
			});

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "post API V1");
			});
		}

		//public static void Main(string[] args)
		//{

		//}
	}
}
