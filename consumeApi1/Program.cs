using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//var tokenValidationParameters = new TokenValidationParameters()
//{
//    ValidIssuer = Configuration["Jwt:JwtIssuer"],
//    ValidAudience = Configuration["Jwt:JwtIssuer"],
//    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtKey"])),
//    ValidateIssuer = true,
//    ValidateAudience = true,
//    ValidateIssuerSigningKey = true,
//};
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = tokenValidationParameters;
//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                var token = context.HttpContext.Request.Cookies["access_token"];
//                if (!string.IsNullOrEmpty(token))
//                {
//                    context.Token = token;
//                    return Task.CompletedTask;
//                }
//                return Task.CompletedTask;
//            }
//        };
//    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(50);
});

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

app.Run();
