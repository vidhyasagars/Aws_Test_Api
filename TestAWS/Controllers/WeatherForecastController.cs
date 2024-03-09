using Microsoft.AspNetCore.Mvc;
using Amazon.SecretsManager;
using Newtonsoft.Json;
using Amazon.Runtime;
using Amazon.SecretsManager.Model;
using Amazon;

namespace TestAWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet]
        public IEnumerable<string> MyData()
        {
            // read parameter stores
            var connectionString = _configuration.GetValue<string>("MyTestKey");
            var key1 = _configuration.GetValue<string>("key1");
            var key2 = _configuration.GetValue<string>("key2");
            return new string[] {
                connectionString,
                key1,
                key2
            };
        }

        [HttpGet]
        public async Task<string> MyData2()
        {
            string arn = "arn:aws:secretsmanager:eu-north-1:905418151097:secret:prod-CKpfuu";
            string key = "";

            // -- passing aws credentials directly --
            //var Credentials = new BasicAWSCredentials("AKIA5FTZASS43SEHVEWI", "C27WbttizPKbUaoEWut4qsO1BVO9quq/pG5wOQX0");
            //var awsClient = new AmazonSecretsManagerClient(Credentials, Amazon.RegionEndpoint.EUNorth1);

            //-- using credentials from aws profiles - by passing profile name (if many profiles are presents) --
            //var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();
            //AWSCredentials awsCredentials;
            //if (chain.TryGetAWSCredentials("default", out awsCredentials))
            //{
            //    var a = awsCredentials;
            //}
            //var awsClient = new AmazonSecretsManagerClient(awsCredentials, RegionEndpoint.EUNorth1);

            //-- using credentials from aws profiles - by taking default Profile --
            var awsClient = new AmazonSecretsManagerClient(RegionEndpoint.EUNorth1);


            var response = await awsClient.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = arn
            });

            var secretStringss = response.SecretString;
            //if (!string.IsNullOrEmpty(response.SecretString))
            //{
            //    var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString);
            //    if (keyValuePairs.TryGetValue(key, out var value))
            //    {
            //        var a = value;
            //    }
            //}

            return "";
        }


        //[HttpGet(Name = "WeatherForecast")]
        //public IEnumerable<WeatherForecast> WeatherForecast()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}