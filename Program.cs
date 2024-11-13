using Confluent.Kafka;
using Gvz.Laboratory.ProductService;
using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Helpers;
using Gvz.Laboratory.ProductService.Kafka;
using Gvz.Laboratory.ProductService.Middleware;
using Gvz.Laboratory.ProductService.Repositories;
using Gvz.Laboratory.ProductService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT bearer authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["test-cookies"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<GvzLaboratoryProductServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISuppliersService, SuppliersService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

var producerConfig = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());
builder.Services.AddScoped<IProductKafkaProducer, ProductKafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "product-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddSupplierKafkaConsumer>());

builder.Services.AddSingleton<UpdateSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateSupplierKafkaConsumer>());

builder.Services.AddSingleton<DeleteSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteSupplierKafkaConsumer>());

builder.Services.AddSingleton<AddPartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddPartyKafkaConsumer>());

builder.Services.AddSingleton<UpdatePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdatePartyKafkaConsumer>());

builder.Services.AddSingleton<DeletePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeletePartyKafkaConsumer>());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();
