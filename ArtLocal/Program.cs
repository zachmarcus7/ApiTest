using ArtLocal.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure the Dependency Injection Container
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDirectoryBrowser();

    // inject DbContext
    builder.Services.AddDbContext<ArtLocalDataContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")
    ));

    // allow CORS for Angular UI to communicate with the API
    builder.Services.AddCors((setup) =>
    {
        setup.AddPolicy("defaultCorsPolicy", (options) =>
        {
            options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        });
    });

    // allow larger file sizes for incoming form files
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MemoryBufferThreshold = Int32.MaxValue;
        options.MultipartBodyLengthLimit = long.MaxValue;
    });

    // add services for Jwts and authentication
    // old way
    /* builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
    });


    
    // failed attempt
    string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = domain;
            options.Audience = builder.Configuration["Auth0:Audience"];

            // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("read:artstyles", policy => policy.Requirements.Add(new HasScopeRequirement("read:artstyles", "dev-3dk6ltrw.us.auth0.com")));
    });
    builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    */


    // 1. Add Authentication Services
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = "https://dev-3dk6ltrw.us.auth0.com/";
        options.Audience = "https://identifier.com";
    });



}

var app = builder.Build();

// Configure the HTTP request processing pipeline
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // set CORs to the policy we created above
    app.UseCors("defaultCorsPolicy");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();

    // these are here so users can access the photos stored in the
    // UploadedFiles directory
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"UploadedFiles")),
        RequestPath = new PathString("/UploadedFiles")
    });
    app.UseDirectoryBrowser(new DirectoryBrowserOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"UploadedFiles")),
        RequestPath = new PathString("/UploadedFiles")
    });
    app.MapControllers();
}

app.Run();
