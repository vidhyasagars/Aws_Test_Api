using Amazon.Runtime;
using Amazon.SecretsManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Configuration.AddJsonFile("appsettings.json");

//AWS Parameter Store
builder.Configuration.AddSystemsManager(source =>
    {
        source.Path = "/myapp/dev/";
        source.Optional = true;
        source.AwsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions()
        {
            // pass IAM user secret keys
            Credentials = new BasicAWSCredentials("AKIA5FTZASS43SEHVEWI", "C27WbttizPKbUaoEWut4qsO1BVO9quq/pG5wOQX0"),
            Region = Amazon.RegionEndpoint.EUNorth1
        };
    }
);

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
