using ToDoList.Data;
using ToDoList.Services;
using GraphQL.AspNet.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ApplicationXMLContext>();
builder.Services.AddScoped<INoteService, NoteServiceXML>();
builder.Services.AddScoped<ICategoryService, CategoryServiceXML>();

builder.Services.AddSingleton<ApplicationSQLContext>();
builder.Services.AddScoped<INoteService, NoteServiceSQL>();
builder.Services.AddScoped<ICategoryService, CategoryServiceSQL>();

builder.Services.AddGraphQL();

builder.Services.AddScoped<ServiceFactory>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.UseGraphQL();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
