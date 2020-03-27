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
	public class MarkovAnalyzeController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			List<string> results = new List<string>();

			var markovService = new MarkovService();
			var sentence = markovService.GetSentence();

			results.Add(string.Join(":", sentence));
			results.AddRange(markovService.DBDataList.Select(p => string.Join(":", p)));

			return results;
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<string>> Get(int id)
		{
			List<string> results = new List<string>();

			var markovService = new MarkovService();
			var sentence = markovService.GetSentence(id);

			results.Add(string.Join(":", sentence));
			results.AddRange(markovService.DBDataList.Select(p => string.Join(":", p)));

			return results;
		}
	}
}