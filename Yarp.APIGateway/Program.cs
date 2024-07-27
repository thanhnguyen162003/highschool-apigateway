using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add config for yarp reverse proxy
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//add authentication/authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicyAuthen", policy =>
        policy.RequireAuthenticatedUser());
});
//add rate limit
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
app.Run();