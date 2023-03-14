using System.Text.Json;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using HackathonSuperRetro.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new List<User>());
builder.Services.AddSingleton(new List<RetrospectiveData>());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .WithOrigins("https://localhost:3000")
);

app.Run();