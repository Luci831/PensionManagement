using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserDetailsMicroservice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UserDetailsMicroservice
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

            //services.AddControllers();
            //new CamelCasePropertyNamesContractResolver();
            services.AddControllers().AddNewtonsoftJson(options =>
             {
                 //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 //Bedefault Pascal format
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             });

            services.AddDbContextPool<AppDBContext>(

            options => options.UseSqlServer(Configuration.GetConnectionString("MyDBConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User and Pension MicroService", Version = "v1" });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Details Microservice v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
