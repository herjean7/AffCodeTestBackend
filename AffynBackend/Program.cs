using AffynBackend.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication()
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JwtSecret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddIdentity<IdentityUser, IdentityRole>();
//builder.Services.AddAuthentication(
//    options =>
//    {
//        options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//    })
//    .AddGoogle(options =>
//    {
//        options.ClientId = "976950391231-e52pgouph58apvjljntnmns8ge2f6b9f.apps.googleusercontent.com";
//        options.ClientSecret = "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";
//    }
//    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
