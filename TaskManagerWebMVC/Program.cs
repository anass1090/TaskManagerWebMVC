using TaskManager.DAL.Repositories;
using TaskManager.Logic.Managers;
using TaskManager.Logic.Services;
using TaskManager.Logic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Register repositories and services
builder.Services.AddScoped<ITaskRepository, TaskRepository>(provider =>
{
    if (builder.Configuration.GetConnectionString("DefaultConnection") == null)
    {
        throw new InvalidOperationException("Connection string is not configured.");
    }
    else
    {
        return new TaskRepository(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});
builder.Services.AddScoped<IProjectRepository, ProjectRepository>(provider =>
{
    if (builder.Configuration.GetConnectionString("DefaultConnection") == null)
    {
        throw new InvalidOperationException("Connection string is not configured.");
    }
    else
    {
        return new ProjectRepository(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});
builder.Services.AddScoped<IUserRepository, UserRepository>(provider =>
{
    if (builder.Configuration.GetConnectionString("DefaultConnection") == null)
    {
        throw new InvalidOperationException("Connection string is not configured.");
    } else
    {
        return new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ProjectService>();

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
