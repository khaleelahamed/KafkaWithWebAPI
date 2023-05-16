using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ProducerConfig { BootstrapServers = "kafka:29092" });
builder.Services.AddSingleton<IProducer<string, string>>(x => new ProducerBuilder<string, string>(x.GetRequiredService<ProducerConfig>()).Build());

builder.Services.AddSingleton(new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "my-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
});
builder.Services.AddScoped<IConsumer<string, string>>(x =>
    new ConsumerBuilder<string, string>(x.GetRequiredService<ConsumerConfig>())
        .Build()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
