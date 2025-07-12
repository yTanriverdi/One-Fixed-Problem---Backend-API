using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OFB_API.JWT;
using OFP_CORE.Entities;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_DAL.Repos.Repositories.EntityRepositories;
using OFP_SERVICE.Mapper;
using OFP_SERVICE.Services.Concrete;
using OFP_SERVICE.Services.Concrete.LikesServices;
using OFP_SERVICE.Services.Concrete.LogService;
using OFP_SERVICE.Services.Concrete.ReportsServices;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LikesServices;
using OFP_SERVICE.Services.Interfaces.LogService;
using OFP_SERVICE.Services.Interfaces.ReportsServices;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace OFB_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
          var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddIdentity<BaseUser, BaseUserRole>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false; 
                options.Password.RequireLowercase = false; 
                options.Password.RequireDigit = false; 
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<OneFixedProblemContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<OneFixedProblemContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            

            builder.Services.AddSingleton<JwtTokenService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Insert JWT Token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                opt.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()   // Herhangi bir kaynaða izin ver
                        .AllowAnyMethod()   // Herhangi bir HTTP metoduna izin ver
                        .AllowAnyHeader()); // Herhangi bir baþlýða izin ver
            });
            // --------------------------------------------------------------------------------------------

            // CONTEXT
            //builder.Services.AddScoped<OneFixedProblemContext>();


            // REPOS REPOS REPOS
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            // LIKES
            builder.Services.AddScoped<IAnswerLikeRepository, AnswerLikeRepository>();
            builder.Services.AddScoped<IProblemLikeRepository, ProblemLikeRepository>();
            builder.Services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();

            // SUP
            builder.Services.AddScoped<IMailRepository, MailRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<ISuggestRepository, SuggestRepository>();

            // REPORT
            builder.Services.AddScoped<IReportAnswerRepository, ReportAnswerRepository>();
            builder.Services.AddScoped<IReportCommentRepository, ReportCommentRepository>();


            // --------------------------------------------------------------------------------------------


            // SERVICES SERVICES SERVICES
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProblemService, ProblemService>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            // LIKES
            builder.Services.AddScoped<IAnswerLikeService, AnswerLikeService>();
            builder.Services.AddScoped<IProblemLikeService, ProblemLikeService>();
            builder.Services.AddScoped<ICommentLikeService, CommentLikeService>();

            // SUP
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<ISuggestService, SuggestService>();

            // REPORT
            builder.Services.AddScoped<IReportAnswerService, ReportAnswerService>();
            builder.Services.AddScoped<IReportCommentService, ReportCommentService>();

            // --------------------------------------------------------------------------------------------

            

            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
