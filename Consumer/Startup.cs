
using ConsumerAPI.Service.Interface;
using ConsumerAPI.Service;
using ConsumerAPI.Model.Mail;
namespace ConsumerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IGetQueueInfoService, GetQueueInfoService>();
            // Configure services, add middleware, etc.
            services.AddLogging();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, Service.MailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure request pipeline, add middleware, etc.
            if (env.IsDevelopment() | env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler("/Error");
                app.UseDeveloperExceptionPage();
            }
           

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.MapControllers();

            //app.Run();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
