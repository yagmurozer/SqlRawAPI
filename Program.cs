
//uygulamayý oluþturur
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// yapýlandýrmmalarýn olduðu yerdir. baðýmlýlýk eklenebilir

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Http isteklerini controller'a yönlendirir.
app.MapControllers();

app.Run();



