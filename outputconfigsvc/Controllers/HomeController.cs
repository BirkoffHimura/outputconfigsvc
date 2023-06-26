using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace outputconfigsvc.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<MyConfig> _options;
        public HomeController(IConfiguration configuration, IOptions<MyConfig> options)
        {
            this._configuration = configuration;
            _options = options;
        }

        public string kv_secret { get { return this._configuration["kv_secret"]; } }
        public string Sampleval { get { return this._configuration[kv_secret]; } }


        public string Index()
        {
            return Sampleval;
            /*return "it works";*/
        }
    }
}
