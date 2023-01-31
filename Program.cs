using System.Text;
using BankAPI.Data; //INDICAMOS DONDE ESTA ALMACENADO EL PROYECTO
using BankAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountTypeService>();
builder.Services.AddScoped<TransactionTypeService>();//para bankTransaction
builder.Services.AddScoped<BankTransactionService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<LoginClientService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
//definimos politica de autorizacion
builder.Services.AddAuthorization(options => {
    options.AddPolicy("SuperAdmin", policy => policy.RequireClaim("AdminType", "Super"));
});

//app = middleware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//llamar a la autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
