using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MarkovAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nifcloud;

namespace MarkovAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MarkovOnceController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<string[]> Get()
		{
			var markovData = new MarkovService().GetFirstWord();
			return new string[] { markovData.Key1, markovData.Key2, markovData.Key3, markovData.Key4, markovData.Value };
		}

		// POST: api/Default
		[HttpPost]
		public ActionResult<string> Post([FromBody] string[] values)
		{
			QueryData querydata = new MarkovService().CreateQueryData(values[0], values[1], values[2], values[3]);
			var markovdata = new MarkovService().GetContinuedWord(querydata);
			return string.Join(',', new string[] { markovdata.Key1, markovdata.Key2, markovdata.Key3, markovdata.Key4, markovdata.Value });
		}
	}
	public class MyClass
	{
		public string Name;
	}
}