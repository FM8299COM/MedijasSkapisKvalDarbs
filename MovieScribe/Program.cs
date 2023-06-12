using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;

var builder = WebApplication.CreateBuilder(args);

// Get the SendGrid API key from the configuration
var sendGridApiKey = builder.Configuration.GetValue<string>("SendGrid:ApiKey");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IActorsService, ActorsService>();
builder.Services.AddScoped<IProducerService, ProducerService>();
builder.Services.AddScoped<IWritersService, WritersService>();
builder.Services.AddScoped<IDistributorService, DistributorService>();
builder.Services.AddScoped<IStudiosService, StudiosService>();
builder.Services.AddScoped<IMediaService, MediaService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;

    options.SignIn.RequireConfirmedAccount = true; 

    options.Tokens.ProviderMap.Add("Default",
        new TokenProviderDescriptor(typeof(DataProtectorTokenProvider<AppUser>)));
})
.AddEntityFrameworkStores<DBContext>()
.AddDefaultTokenProviders(); 

builder.Services.AddSingleton<IEmailSender>(i =>
    new SendGridEmailSender(sendGridApiKey, i.GetRequiredService<ILogger<SendGridEmailSender>>()));

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DBFill.Initialize(app);
DBFill.SeedUsersAsync(app).Wait();

app.Run();
