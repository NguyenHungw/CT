using CT.MOD.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Mail;

using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using CT.Controllers.PhanQuyenVaTaiKhoan;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CT.DAL.ScheduledTask;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<RefreshTokenStore>();
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

//config stmp gửi mail
/*builder.Services.AddTransient<SmtpClient>(provider =>
{
    var smtpClient = new SmtpClient("smtp.gmail.com")
    {
        Port = 587,
        Credentials = new NetworkCredential("vuacomputer.vjppro@gmail.com", "ebuvtbajmhgldomi"),
        EnableSsl = true,
    };
    return smtpClient;
});*/
// ... (Các phần code khác)

// Thêm dịch vụ SmtpClient vào DI container

//config stmp gửi mail

// Cấu hình Facebook OAuth để đăng nhập bằng fb / mới cấu hình app id và pass chưa làm
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Facebook";
})
.AddCookie()

.AddOAuth("Facebook", options =>
{
    options.ClientId = "275201555380486";//app id
    options.ClientSecret = "a4e19ccf209202913a46fbe84457c1cd";//appsecret
    options.CallbackPath = "/signin-facebook"; // Đường dẫn callback tùy chọn
    options.AuthorizationEndpoint = "https://www.facebook.com/v13.0/dialog/oauth";
    options.TokenEndpoint = "https://graph.facebook.com/v13.0/oauth/access_token";
    options.UserInformationEndpoint = "https://graph.facebook.com/v13.0/me";

    //Được sử dụng để ánh xạ các thuộc tính từ thông tin người dùng được trả về từ Facebook API thành các Claims trong chuỗi xác thực (token).
    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = context =>
        {
            // Customize the claims mapping here if needed
            return Task.CompletedTask;
        }
    };
});

//send mail
builder.Services.AddTransient<SmtpClient>(provider =>
{
   
    return new SmtpClient("smtp.gmail.com", 587, "vuacomputer.vjppro@gmail.com", "ebuvtbajmhgldomi");
});


builder.Services.AddMemoryCache(); // Thêm dịch vụ IMemoryCache
builder.Services.AddTransient<TKController>(); // Đảm bảo đăng ký controller của bạn

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials(); // Bổ sung AllowCredentials nếu bạn cần chấp nhận cookie từ client
                      });
});
// JWT Authentication Configuration
var jwtKey = Encoding.UTF8.GetBytes("YourSuperSecretKey");
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

//check quyền nhưng chưa làm dc
// Other Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewQLSP", policy => policy.RequireClaim("CN", "QLSP:Xem"));
    options.AddPolicy("CanAddQLSP", policy => policy.RequireClaim("CN", "QLSP:Them"));
    options.AddPolicy("CanViewTK", policy =>
    {
        policy.RequireClaim("CN", "QLTK");
        policy.RequireClaim("QLTK", "Xem");
    });
    // Add other policies as needed
});

// Configure CORS
builder.Services.AddCors();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configure JWT authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            new string[] { }
        }
    });
});
builder.Services.AddDistributedMemoryCache(); // Thêm cache cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian chờ trước khi Session hết hạn
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
///
//config scheduled task xóa data mỗi ngày của phần đăng ký nếu ko active tài khoản
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
var scheTask = new ScheTask(app.Configuration);
scheTask.Start();

CT.ULT.SQLHelper.appConnectionStrings = app.Configuration.GetConnectionString("DefaultConnection");
app.UseSession();
// Configure middleware
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.OAuthClientId("your-client-id"); // Replace with your OAuth2 client ID
        c.OAuthAppName("My API");
    });
}

app.Run();
