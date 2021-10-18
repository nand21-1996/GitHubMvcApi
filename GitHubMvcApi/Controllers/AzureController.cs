using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubMvcApi.Controllers
{
    public class AzureController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            return Ok("Works");
        }
    }
}
