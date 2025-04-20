
//uygulamay� olu�turur
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// yap�land�rmmalar�n oldu�u yerdir. ba��ml�l�k eklenebilir

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Http isteklerini controller'a y�nlendirir.
app.MapControllers();

app.Run();



