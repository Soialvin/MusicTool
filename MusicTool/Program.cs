using Microsoft.EntityFrameworkCore;
using MusicTool.DataAccess.Data;
using MusicTool.DataAccess.Repositories.AccountRepositories;
using MusicTool.DataAccess.Repositories.CategoryRepositories;
using MusicTool.DataAccess.Repositories.FileUploadRepositories;
using MusicTool.DataAccess.Repositories.SingerRepositories;
using MusicTool.DataAccess.Repositories.Song_CategoryRepositories;
using MusicTool.DataAccess.Repositories.Song_SingerRepositories;
using MusicTool.DataAccess.Repositories.SongRepositories;
using MusicTool.DataAccess.Repositories.VipRepositories;
using MusicTool.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<MusicToolDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ISingerRepository, SingerRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<ISong_SingerRepository, Song_SingerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISong_CategoryRepository, Song_CategoryRepository>();
builder.Services.AddScoped<IVipRepository, VipRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MTHome}/{action=Index}/{id?}");

app.Run();
