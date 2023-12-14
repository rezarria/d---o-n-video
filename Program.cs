using Dudoan.MML;
using Dudoan.Dbcontext;
using Dudoan.MML.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("sqlite"));
});
builder.Services.AddDbContext<IdentityDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("security"));
});
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<MLService, MLService>();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
app.MapIdentityApi<IdentityUser>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
