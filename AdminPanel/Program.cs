using AdminPanel.CommandSide.Infra;
using AdminPanel.QuerySide;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMenuQuery, MenuQuery>();


builder.Services.AddDbContext<PanelDbContextt>(options => options
               .UseSqlServer(builder.Configuration.GetConnectionString("panelConn"),
               b => b.MigrationsAssembly(typeof(PanelDbContextt).Assembly.FullName)), ServiceLifetime.Scoped);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
         //builder.WithOrigins("https://localhost:5057")
         builder.AllowAnyOrigin()
             .AllowAnyHeader()
             .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAllOrigins");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
