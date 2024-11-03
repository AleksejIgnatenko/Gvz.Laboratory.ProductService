using Confluent.Kafka;
using Gvz.Laboratory.ProductService;
using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Kafka;
using Gvz.Laboratory.ProductService.Middleware;
using Gvz.Laboratory.ProductService.Repositories;
using Gvz.Laboratory.ProductService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

builder.Services.AddDbContext<GvzLaboratoryProductServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

var producerConfig = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());
builder.Services.AddScoped<IProductKafkaProducer, ProductKafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "supplier-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddSupplierKafkaConsumer>());

builder.Services.AddSingleton<UpdateSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateSupplierKafkaConsumer>());

builder.Services.AddSingleton<DeleteSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteSupplierKafkaConsumer>());

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
