using ImageClassification.Api.Interface;
using ImageClassification.Api.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MLModel.zip");

builder.Services.AddSingleton<IImageClassificationService>(sp =>
    new ImageClassificationService(modelPath));

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
