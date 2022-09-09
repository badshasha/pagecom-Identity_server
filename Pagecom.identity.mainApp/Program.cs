using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pagecom.identity.data.configinformation;
using pagecom.identity.data.data;
using pagecom.identity.data.data.DbInitializer;
using pagecom.identity.data.data.user;
using Pagecom.identity.mainApp.ProfileChange;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(); // [+] adding controller views to the site


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); // todo : add default token provide missing


//builder.Services.AddScoped<Initializer, IdentityDbInitializer>();





builder.Services.AddIdentityServer()
    //.AddProfileService<ProfileServiceclass>()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.Scopes)
    .AddInMemoryIdentityResources(Config.Resources)
    .AddAspNetIdentity<ApplicationUser>()
    //.AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();


builder.Services.AddScoped<IProfileService, ProfileServiceclass>();

var app = builder.Build();
preperationDb.DatabaseCreate(app);

app.UseRouting(); // allow routing
app.UseStaticFiles(); // activate static wwwroot
app.UseIdentityServer();

// Initializer value = app.Services.GetRequiredService<Initializer>();
// value.Initilizer();

app.UseAuthorization();

app.MapDefaultControllerRoute(); // controller activate
app.Run();