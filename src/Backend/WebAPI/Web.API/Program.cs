using Application.Common.Exceptions;
using Application.Extensions;
using Infrastructure.Extensions;
using Persistence.Extensions;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection - Application - Infrastructure - Persistence
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
#endregion


#region CORS Settings
var allowedResources = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedResources,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
#endregion

builder.Services.AddDistributedMemoryCache();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(allowedResources);
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

//Custom Exception Handler
app.UseMiddleware<UseCustomExceptionHandler>();

//Auto migrate
app.Services.ApplyMigrations(logger: app.Services.GetRequiredService<ILogger<Program>>());

app.Run();