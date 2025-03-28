using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Interfaces;
using OnlineNews.Services;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity;
using OnlineNews.Middleware;
using OnlineNews.Models.Helper;

namespace OnlineNews
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration for connection string
            var connectionString = builder.Configuration.GetConnectionString("LexiconConnection")
                ?? throw new InvalidOperationException("Connection string 'LexiconConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Add Identity Services for User and Role management
            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Session Services
            builder.Services.AddDistributedMemoryCache();  // Required for sessions
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);  // Set the session timeout
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });

            // Register services
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICartService,CartService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IEditorService, EditorService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddHttpClient<RequestService>();  // Injecting HttpClient for services needing HTTP calls
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();  // Allow access to HttpContext
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Add MVC (Controllers + Views)
            builder.Services.AddControllersWithViews();

            // Build app and configure
            var app = builder.Build();

            // Session Middleware should come before routing
            app.UseSession();  // This ensures the session is available throughout the request pipeline

            // Application-specific middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<SessionInitializationMiddleware>();
            app.UseRouting();
            app.UseAuthorization();

            // Routing setup
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Editor", "Subscriber","Writer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = "admin@admin.com";
                string password = "Admin_password_1";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = "madhurimandalapu82473@gmail.com";
                string password = "H@mms1992";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Writer");
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = "editor@editor.com";
                string password = "Editor_password_1";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Editor");
                }


            }

            app.Run();
        }
    }
}
