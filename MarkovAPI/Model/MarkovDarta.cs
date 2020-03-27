using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkovAPI.Model
{
	public class MarkovResult
	{
		public MarkovData[] results { get; set; }
	}

	public class MarkovData
	{
		public string objectId { get; set; }
		public DateTime? createDate { get; set; } = null;
		public DateTime? updateDate { get; set; } = null;
		public long? Id { get; set; } = null;
		public string Key1 { get; set; }
		public string Key2 { get; set; }
		public string Key3 { get; set; }
		public string Key4 { get; set; }
		public string Value { get; set; }
	}
}
