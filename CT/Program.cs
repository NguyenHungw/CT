using CT.MOD;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;


using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
                          builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});*/

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Them", policy => policy.RequireClaim("QLSP", "Them"));
});*/

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

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("QLSP", policy =>
    {
        policy.RequireClaim("NhomNguoiDung", "Admin");
        policy.RequireClaim("ChucNang", "QLSP");
        policy.RequireClaim("Quyen", "Them");
    });

    options.AddPolicy("UserOnly", policy =>
    {
        policy.RequireClaim("NhomNguoiDung", "User");
    });
});

*/
// Thêm services cho phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewQLSP", policy => policy.RequireClaim("CN", "QLSP:Xem"));
    options.AddPolicy("CanAddQLSP", policy => policy.RequireClaim("CN", "QLSP:Them"));
    options.AddPolicy("CanViewTK", policy =>
    {
        policy.RequireClaim("CN", "QLTK");
        
        policy.RequireClaim("QLTK", "Xem");
    });


    // Thêm các policy khác tương tự
});






// Cors
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

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
builder.Services.AddAuthentication();
app.Use(async (context, next) =>
{
    if (context.Response.StatusCode == 403)
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";

        var result = new BaseResultMOD
        {
            Status = -99,
            Message = "Bạn không có quyền truy cập"
        };

        await context.Response.WriteAsJsonAsync(result);
    }
    else if (context.Response.StatusCode == 401)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var result = new BaseResultMOD
        {
            Status = 0,
            Message = "Chưa đăng nhập"
        };

        await context.Response.WriteAsJsonAsync(result);
    }
    else
    {
        await next();
    }
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

        // Add "Authorize" button to Swagger UI
        c.OAuthClientId("your-client-id"); // Replace with your OAuth2 client ID
        c.OAuthAppName("My API");
    });
}


app.UseHttpsRedirection();

app.UseStaticFiles();



// Enable CORS

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
