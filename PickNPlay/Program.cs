using CloudinaryDotNet;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PickNPlay.Controllers;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Services;
using PickNPlay.picknplay_dal.Data;
using Serilog;
using Serilog.Formatting.Json;
using Stripe;
using System.Text;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File("logs/logs.txt",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

        try
        {


            Log.Information("app began building");

            var builder = WebApplication.CreateBuilder(args);

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var key = Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Secret"]);

            builder.Services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 64;
            });



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Secret"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeAdmin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("userRole","3");
                });

                options.AddPolicy("MustBeRegistered", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("MustBeDealer", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("userRole", "2");
                });

                options.AddPolicy("MustBeModer", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("userRole", "4");
                });

            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowForAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                });
            });

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pick&Play API",
                    Version = "v1",
                });
                

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Give me the damn token, CJ",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",

                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                options.CustomSchemaIds(type=>type.FullName.Replace('.','_'));

                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "PickNPlay.xml");
                options.IncludeXmlComments(xmlPath);
                options.SchemaFilter<EnumTypesSchemaFilter>(xmlPath);

            });


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);


            builder.Services.AddDbContext<picknplayContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CategoryService>();
            // builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<ListingService>();
            builder.Services.AddScoped<MessageService>();
            builder.Services.AddScoped<PickNPlay.picknplay_bll.Services.ReviewService>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<UserService>();
            // builder.Services.AddScoped<ProviderService>();
            builder.Services.AddScoped<FavouriteService>();
            builder.Services.AddScoped<DepositService>();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(); 

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run("https://localhost:5001/");
            Log.Information("app started succesfully");

        }
        catch (Exception e)
        {
            Log.Fatal(e, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}