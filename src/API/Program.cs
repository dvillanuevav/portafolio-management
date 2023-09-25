global using FastEndpoints;
using API.Startup;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();
app.AddApplication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();