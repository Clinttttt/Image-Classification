using ImageClassification.Api;
using ImageClassification.Api.Interface;
using ImageClassification.Api.Models;
using ImageClassification.Api.Services;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
