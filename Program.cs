using BankAPI.Data; //INDICAMOS DONDE ESTA ALMACENADO EL PROYECTO
using BankAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Añadimos servicio como referencia a la base de datos.
//DB CONTEXT
builder.Services.AddSqlServer<BankDbContext>(builder.Configuration.GetConnectionString("BankConnection"));

//Service Layer
//inyectamos servicio a la aplicacion
builder.Services.AddScoped<ClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
