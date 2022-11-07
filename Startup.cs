using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace mailer
{
    public class Startup
    {
        IConfiguration _configuration;
        readonly string SpecificAllowedOrigins = "SpecificAllowedOrigins";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: SpecificAllowedOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(SpecificAllowedOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost(
                    "/sendEmail/",
                    async context =>
                    {
                        MailResponse mailResponse;
                        string body = "";
                        string jsonMailResponse;
                        using (StreamReader stream = new StreamReader(context.Request.Body))
                        {
                            body = await stream.ReadToEndAsync();
                        }
                        if (body == "")
                        {
                            mailResponse = new MailResponse(
                                "Email Configuration is required",
                                "fail"
                            );
                            jsonMailResponse = JsonConvert.SerializeObject(mailResponse);
                            await context.Response.WriteAsync(jsonMailResponse);
                        }
                        MailSettings mail = JsonConvert.DeserializeObject<MailSettings>(body);

                        MailResponse message = await MailUtils.MailUtils.SendSmtpMail(
                            mail.From,
                            mail.To,
                            "test gui mail tu Letuanbao",
                            "xin chao Le tuan bao",
                            mail.Email,
                            mail.Password,
                            mail.Host,
                            (int)mail.Port,
                            mail.IsSecurity
                        );

                        jsonMailResponse = JsonConvert.SerializeObject(message);
                        await context.Response.WriteAsync(jsonMailResponse);
                    }
                );
            });
        }
    }
}
