using GraphQL;
using GraphQL.Types;
using ToDoList.Data;
using ToDoList.GraphQL;
using ToDoList.Services;
using ToDoList.GraphQL.Queries;
using static ToDoList.Services.ServiceFactory;
using ToDoList.GraphQL.Mutations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ApplicationXMLContext>();
builder.Services.AddSingleton<INoteService, NoteServiceXML>();
builder.Services.AddSingleton<ICategoryService, CategoryServiceXML>();

builder.Services.AddSingleton<ApplicationSQLContext>();
builder.Services.AddSingleton<INoteService, NoteServiceSQL>();
builder.Services.AddSingleton<ICategoryService, CategoryServiceSQL>();

builder.Services.AddSingleton<CategoryType>();
builder.Services.AddSingleton<CategoryQueries>();
builder.Services.AddSingleton<CategoryMutations>();

builder.Services.AddSingleton<NoteType>();
builder.Services.AddSingleton<NoteQueries>();
builder.Services.AddSingleton<NoteMutations>();

builder.Services.AddSingleton<GraphQueries>();
builder.Services.AddSingleton<GraphMutations>();

builder.Services.AddSingleton<ISchema, GraphSchema>();

builder.Services.AddGraphQL(x => x
    .AddAutoSchema<GraphQueries>()
    .AddAutoSchema<GraphMutations>()
    .AddSystemTextJson());

builder.Services.AddSingleton<ServiceFactory>();

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

app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground(
    "/playground",
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",
        SubscriptionsEndPoint = "/graphql",
        Headers = new Dictionary<string, object> {
            { "CurrentDatabase", CurrentDatabase.SQL.ToString() }
        }
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
