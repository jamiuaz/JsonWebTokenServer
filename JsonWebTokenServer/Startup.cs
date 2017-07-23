//using System;
//using System.IdentityModel.Tokens;
//using Microsoft.AspNet.Authorization;
//using Microsoft.AspNet.Builder;
//using Microsoft.AspNet.Diagnostics.Entity;
//using Microsoft.AspNet.Hosting;
//using Microsoft.Dnx.Runtime;
//using Microsoft.Framework.Configuration;
//using Microsoft.Framework.DependencyInjection;
//using Microsoft.Framework.Logging;
//using MB5.Model.Service.Appstart;
//using MB5.Model.Security.Token;
//using Microsoft.AspNet.Http;
//using System.Security.Cryptography;
//using Microsoft.AspNet.Authentication.JwtBearer;
//using Microsoft.AspNet.Diagnostics;
//using Newtonsoft.Json;

//namespace MB5
//{
//    public class Startup
//    {
//        const string TokenAudience = "ExampleAudience";
//        const string TokenIssuer = "ExampleIssuer";
//        private RsaSecurityKey key;
//        private TokenAuthOptions tokenOptions;

//        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
//        {
//            // Setup configuration sources.

//            var builder = new ConfigurationBuilder()
//                .SetBasePath(appEnv.ApplicationBasePath)
//                .AddJsonFile("appsettings.json")
//                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

//            if (env.IsDevelopment())
//            {
//                // This reads the configuration keys from the secret store.
//                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
//                builder.AddUserSecrets();
//            }
//            builder.AddEnvironmentVariables();
//            Configuration = builder.Build();
//        }

//        public IConfigurationRoot Configuration { get; set; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            string path = @"C:\website\Maintbook\MB5\src\MB5\MyTokenKey.json";
//            RSAParameters keyParams = RSAKeyUtility.GetKeyParameters(path);
//            key = new RsaSecurityKey(keyParams);

//            tokenOptions = new TokenAuthOptions();                                                                                 // 4. Create TokenAuthOptions using 3.
//            tokenOptions.Audience = TokenAudience;
//            tokenOptions.Issuer = TokenIssuer;
//            tokenOptions.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);

//            services.AddInstance<TokenAuthOptions>(tokenOptions);

//            services.AddAuthorization(auth =>
//            {
//                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
//                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
//                    .RequireAuthenticatedUser().Build());
//            });

//            // Add Entity Framework services to the services container.
//            //services.AddEntityFramework()
//            //    .AddSqlServer()
//            //    .AddDbContext<ApplicationDbContext>(options =>
//            //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

//            // Add Identity services to the services container.
//            //services.AddIdentity<ApplicationUser, IdentityRole>()
//            //    .AddEntityFrameworkStores<ApplicationDbContext>()
//            //    .AddDefaultTokenProviders();

//            // Add MVC services to the services container.
//            DependencyInjectionConfig.RegisterDI(services);
//            services.AddMvc();

//            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
//            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
//            // services.AddWebApiConventions();

//            // Register application services.
//            //services.AddTransient<IEmailSender, AuthMessageSender>();
//            //services.AddTransient<ISmsSender, AuthMessageSender>();
//        }

//        // Configure is called after ConfigureServices is called.
//        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
//        {
//            loggerFactory.MinimumLevel = LogLevel.Information;
//            loggerFactory.AddConsole();
//            loggerFactory.AddDebug();

//            // Configure the HTTP request pipeline.

//            // Add the following to the request pipeline only in development environment.
//            if (env.IsDevelopment())
//            {
//                app.UseBrowserLink();
//                app.UseDeveloperExceptionPage();
//                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
//            }
//            else
//            {
//                // Add Error handling middleware which catches all application specific errors and
//                // sends the request to the following path or controller action.
//                app.UseExceptionHandler("/Home/Error");
//            }

//            // Add the platform handler to the request pipeline.
//            app.UseIISPlatformHandler();

//            app.UseExceptionHandler(appBuilder =>
//            {
//                appBuilder.Use(async (context, next) =>
//                {
//                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
//                    // This should be much more intelligent - at the moment only expired 
//                    // security tokens are caught - might be worth checking other possible 
//                    // exceptions such as an invalid signature.
//                    if (error != null && error.Error is SecurityTokenExpiredException)
//                    {
//                        context.Response.StatusCode = 401;
//                        // What you choose to return here is up to you, in this case a simple 
//                        // bit of JSON to say you're no longer authenticated.
//                        context.Response.ContentType = "application/json";
//                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { authenticated = false, tokenExpired = true }));
//                    }
//                    else if (error != null && error.Error != null)
//                    {
//                        context.Response.StatusCode = 500;
//                        context.Response.ContentType = "application/json";
//                        // TODO: Shouldn't pass the exception message straight out, change this.
//                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { success = false, error = error.Error.Message }));
//                    }
//                    // We're not trying to handle anything else so just let the default 
//                    // handler handle.
//                    else await next();
//                });
//            });
//            // Add static files to the request pipeline.
//            app.UseStaticFiles();
            
//            app.UseJwtBearerAuthentication(options =>
//            {
//                //Validate with audience and issuer
//                options.TokenValidationParameters.IssuerSigningKey = key;
//                options.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
//                options.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;

//                //Check that we have sign the token
//                options.TokenValidationParameters.ValidateLifetime = true;

//                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(0);


//            });

//            //app.UseCookieAuthentication(options =>
//            //{
//            //    options.AuthenticationScheme = "Cookie";
//            //    options.LoginPath = new PathString("/Account/Unauthorized/");
//            //    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
//            //    options.AutomaticAuthentication = false;
//            //});

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=MB5}/{action=Index}/{id?}");

//                // Uncomment the following line to add a route for porting Web API 2 controllers.
//                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
//            });
//        }
//    }
//}
