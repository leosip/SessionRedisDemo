using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var appName = "s505";

var useRedis = builder.Configuration.GetValue<bool>("CacheSettings:UseRedis");

if (useRedis) 
{
    var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
    if (redisConnectionString == null) 
    {
        throw new ApplicationException("Redis connection string is missing.");
    }
    var redis = ConnectionMultiplexer.Connect(redisConnectionString);

    builder.Services.AddDataProtection()
        .SetApplicationName(appName)
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

    builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = appName;
        }
    );
}
else 
{
    if (builder.Environment.IsEnvironment("DockerMemory")) 
    {
        builder.Services.AddDataProtection()
            .SetApplicationName(appName)
            .PersistKeysToFileSystem(new DirectoryInfo(@"/root/.aspnet/DataProtection-Keys"));
    }

    builder.Services.AddDistributedMemoryCache();        
}

builder.Services.AddSession(options =>
    {
        options.Cookie.Name = $".{appName}.Session";
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
);

// Adiciona o Antiforgery
// builder.Services.AddAntiforgery(options =>
// {    
//     options.FormFieldName = "AntiforgeryFieldname";
//     options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
//     options.SuppressXFrameOptionsHeader = false;
//     options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;

//     options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
//     options.Cookie.HttpOnly = true;    
// });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

// Use o Antiforgery após o UseSession e antes de UseAuthorization
//app.UseAntiforgery();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
