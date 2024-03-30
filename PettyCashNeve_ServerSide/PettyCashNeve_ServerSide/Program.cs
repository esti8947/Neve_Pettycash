using AutoMapper;
using BL.NdbManager;
using BL.Repositories.AnnualBudgetRepository;
using BL.Repositories.BudgetTypeRepository;
using BL.Repositories.BuyerRepository;
using BL.Repositories.EventCategoryRepository;
using BL.Repositories.EventRepository;
using BL.Repositories.ExpenseCategoryRepository;
//using BL.Repositories.ExpenseOfEvent;
using BL.Repositories.ExpenseRepository;
using BL.Repositories.MonthlyBudgetRepository;
using BL.Repositories.RefundBudgetRepository;
using BL.Repositories.UserRepository;
using BL.Services.AnnualBudgetService;
using BL.Services.BudgetTypeService;
using BL.Services.EmailService;
using BL.Services.EventCategoryService;
using BL.Services.EventService;
using BL.Services.ExpenseCategoryService;
using BL.Services.ExpenseService;
using BL.Services.MonthlyBudgetService;
using BL.Services.RefundBudgetService;
using BL.Services.UserService;
using DAL.Data;
using DAL.Models;
using Entities.Models_Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PettyCashNeve_ServerSide;
using PettyCashNeve_ServerSide.Repositories;
using PettyCashNeve_ServerSide.Repositories.DepartmentRepository;
using PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository;
using PettyCashNeve_ServerSide.Services;
using PettyCashNeve_ServerSide.Services.DepartmentService;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        });
});



builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<PettyCashNeveDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PettyCashNeveDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddIdentity<NdbUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = false; })
        .AddEntityFrameworkStores<PettyCashNeveDbContext>()
        .AddDefaultTokenProviders();

//builder.Services.AddScoped<ActiveDirectoryService>();
//builder.Services.AddScoped<NdbSignInManager>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Default Password setting
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Your API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});



builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IMonthlyCashRegisterRepository, MonthlyCashRegisterRepository>();
builder.Services.AddScoped<IMonthlyCashRegisterService, MonthlyCashRegisterService>();
builder.Services.AddScoped<IAnnualBudgetRepository, AnnualBudgetRepository>();
builder.Services.AddScoped<IAnnualBudgetService, AnnualBudgetService>();
builder.Services.AddScoped<IMonthlyBudgetRepository, MonthlyBudgetRepository>();
builder.Services.AddScoped<IMonthlyBudgetService, MonthlyBudgetService>();
builder.Services.AddScoped<IRefundBudgetRepository, RefundBudgetRepository>();
builder.Services.AddScoped<IRefundBudgetService, RefundBudgetService>();
//builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
builder.Services.AddScoped<IExpenseMoreInfoService, ExpenseMoreInfoService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
builder.Services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
builder.Services.AddScoped<IBudgetTypeRepository, BudgetTypeRepository>();
builder.Services.AddScoped<IBudgetTypeService, BudgetTypeService>();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

//app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseRouting();


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
