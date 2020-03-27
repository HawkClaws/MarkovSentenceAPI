using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nifcloud
{
	public class NCMBQuery
	{
		private const string WHERE_URL = "?";
		public static T Query<T>(QueryData queryData)
		{
			string url = "https://mbaas.api.nifcloud.com/2013-09-01/classes/";

			url += queryData.className;

			url += WHERE_URL;//「?」を末尾に追加する。

			url = NCMBQuery.MakeWhereUrl(url, queryData);

			var req = WebRequest.Create(url);
			req.Method = "GET";
			req.ContentType = "application/json";

			string result = string.Empty;

			NCMB.SetRequestNiftyCloud(req); // シグネチャを付加する

			using (var webRes = (HttpWebResponse)req.GetResponse())
				if (webRes.StatusCode == HttpStatusCode.OK)
					using (var sr = new StreamReader(webRes.GetResponseStream()))
					{
						result = sr.ReadToEnd();
					}

			return JsonConvert.DeserializeObject<T>(result);
		}

		public static string MakeWhereUrl(string url, QueryData queryData)
		{
			Dictionary<string, object> beforejsonData = CreateDicData(queryData);

			return MakeWhereUrl(url, beforejsonData);
		}

		public static Dictionary<string, object> CreateDicData<T>(T data)
		{
			Dictionary<string, object> result = new Dictionary<string, object>();

			var types = typeof(T).GetProperties();
			foreach (PropertyInfo prop in types)
			{
				var value = prop.GetValue(data);

				if (value != null)
					result.Add(prop.Name, value);
			}
			return result;
		}

		//beforejsonDataの各値をJSON化→エンコードしurlに結合する
		private static string MakeWhereUrl(string url, Dictionary<string, object> beforejsonData)
		{

			StringBuilder sb = new StringBuilder();
			sb.Append(url);
			foreach (string key in beforejsonData.Keys)
			{
				if (key.Equals("className"))
				{
					continue;
				}

				Dictionary<string, object> whereDic;
				int intValue;//Json化前のkeyの値【limit】
				string jsonValue;//Json化後のKeyの値
								 //where の valueはDictionary型　limit の　valueはint型　
				if (beforejsonData[key] is IDictionary)
				{
					//whre
					whereDic = (Dictionary<string, object>)beforejsonData[key];
					jsonValue =JsonConvert.SerializeObject(whereDic);
				}
				else if (beforejsonData[key] is int)
				{
					//limit
					intValue = (int)beforejsonData[key];
					jsonValue = JsonConvert.SerializeObject(intValue);
				}
				else
				{
					//その他
					jsonValue = (string)beforejsonData[key];
				}
				string encodeJsonValue = Uri.EscapeUriString(jsonValue);//JSON化された値をエンコードされた文字列
				encodeJsonValue = encodeJsonValue.Replace(":", "%3A");

				//結合
				sb.Append(key).Append("=").Append(encodeJsonValue).Append("&");
			}

			if (beforejsonData.Count > 0)
			{
				// 最後に追加した&を削除
				sb.Remove(sb.Length - 1, 1);
			}

			return sb.ToString();
		}

	}
	public class QueryData
	{
		public object className { get; set; }
		public Dictionary<string, object> where { get; set; }
		public object limit { get; set; }
		public object skip { get; set; }
		public object order { get; set; }
	}
}
