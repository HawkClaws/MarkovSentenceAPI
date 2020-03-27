using MarkovAPI.Model;
using Newtonsoft.Json;
using Nifcloud;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MarkovAPI.Service
{
	public class MarkovService
	{

		private const string START_WORD = "__NULL_OF_SENTENCE__";
		private const string END_WORD = "__END_OF_SENTENCE__";

		/// <summary>
		/// DBに入ってるデータの数
		/// </summary>
		private const int RowMaxCount = 260000;

		/// <summary>
		/// DBから取得した結果(Analyze)
		/// </summary>
		public List<IEnumerable<string>> DBDataList = new List<IEnumerable<string>>();

		public string[] GetSentence(int? wordCount = null)
		{
			List<string> result = new List<string>();

			var firstData = GetFirstWord();
			result.Add(firstData.Value);

			while (true)
			{
				QueryData queryData = CreateQueryData(firstData.Key2, firstData.Key3, firstData.Key4, firstData.Value);
				firstData = GetContinuedWord(queryData);

				if (firstData == null || firstData.Value == END_WORD) break;

				result.Add(firstData.Value);

				if (wordCount != null && result.Count == wordCount) break;
			}

			return result.ToArray();
		}

		/// <summary>
		/// 開始ワードを取得します。
		/// </summary>
		/// <returns></returns>
		public MarkovData GetFirstWord()
		{
			var firstList = NCMBQuery.Query<MarkovResult>(CreateFirstQueryData()).results.OrderBy(i => Guid.NewGuid());
			this.DBDataList.Add(firstList.Take(10)?.Select(p => p.Value));

			return firstList.FirstOrDefault();
		}

		/// <summary>
		/// 継続ワードを取得します。
		/// </summary>
		/// <param name="queryData"></param>
		/// <returns></returns>
		public MarkovData GetContinuedWord(QueryData queryData)
		{
			var queryDatas = NCMBQuery.Query<MarkovResult>(queryData).results.OrderBy(i => Guid.NewGuid());
			this.DBDataList.Add(queryDatas.Take(10)?.Select(p => p.Value));

			return queryDatas.FirstOrDefault();
		}

		/// <summary>
		/// QueryDataを作成します。
		/// </summary>
		/// <param name="key1"></param>
		/// <param name="key2"></param>
		/// <param name="key3"></param>
		/// <param name="key4"></param>
		/// <returns></returns>
		public QueryData CreateQueryData(string key1, string key2, string key3, string key4)
		{
			var data = NCMBQuery.CreateDicData(new MarkovData
			{
				Key1 = key1,
				Key2 = key2,
				Key3 = key3,
				Key4 = key4,
			});

			return new QueryData
			{
				className = "Markov",
				where = data,
			};
		}

		private QueryData CreateFirstQueryData()
		{
			int random = new Random(Convert.ToInt32(Guid.NewGuid().ToString("N").Substring(0, 8), 16)).Next(1, RowMaxCount);

			var data = NCMBQuery.CreateDicData(new MarkovData
			{
				Key1 = START_WORD,
				Key2 = START_WORD,
				Key3 = START_WORD,
				Key4 = START_WORD,
			});

			return new QueryData
			{
				className = "Markov",
				where = data,
				skip = random,
				limit = 10,
			};
		}
	}
}
