using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Cryptography;
using TTSS.Game.Analysis.Api.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

string publicKeyPem = builder.Configuration.GetSection("Authen:VerifyKey").Value 
    ?? throw new Exception("Authentication Key is Require in Configuration");
RSA rsa = RSA.Create();
rsa.ImportFromPem(publicKeyPem);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };
        });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicyConstant.Admin, x => x.RequireRole(AuthPolicyConstant.Admin));
    options.AddPolicy(AuthPolicyConstant.Player, x => x.RequireRole(AuthPolicyConstant.Player));
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.MapGet("/test", () =>"hello") .WithName("GetWeatherForecast");

app.Run();
