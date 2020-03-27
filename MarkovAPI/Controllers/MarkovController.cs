using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarkovAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarkovAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkovController : ControllerBase
    {
		// GET api/values
		[HttpGet]
		public ActionResult<string> Get()
		{
			return string.Join(string.Empty, new MarkovService().GetSentence());
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return string.Join(string.Empty, new MarkovService().GetSentence(id));
		}
	}
}