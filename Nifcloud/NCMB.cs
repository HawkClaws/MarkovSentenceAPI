using Nifcloud.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Nifcloud
{
	public class NCMB
	{
		public static void SetRequestNiftyCloud(WebRequest request)
		{
			string appKey = Resources.AppKey;
			string clientKey = Resources.ClientKey;
			var now = DateTime.Now;
			var query = "";

			var parameters = new List<string>() {
		   "SignatureMethod=HmacSHA256",
		   "SignatureVersion=2",
		   $"X-NCMB-Application-Key={appKey}",
		   $"X-NCMB-Timestamp={now.ToString( "yyyy-MM-ddTHH:mm:sszzzz" )}"
		};
			if (request.RequestUri.Query != null && request.RequestUri.Query.Length > 0)
				foreach (var item in request.RequestUri.Query.Substring(1).Split('&'))
					parameters.Add(item);

			var buf = new StringBuilder(64);
			foreach (var item in parameters.OrderBy(x => x.Split('=')[0], StringComparer.Ordinal))
			{
				if (buf.Length > 0)
					buf.Append('&');
				buf.Append(item);
			}
			query = buf.ToString();

			var signature = new StringBuilder(64)
			   .Append(request.Method).Append('\n')
			   .Append(request.RequestUri.Host).Append('\n')
			   .Append(request.RequestUri.AbsolutePath).Append('\n')
			   .Append(query)
			   .ToString();

			using (var hmacsha256 = new HMACSHA256(Encoding.ASCII.GetBytes(clientKey)))
			{
				request.Headers.Add("X-NCMB-Application-Key", appKey);
				request.Headers.Add("X-NCMB-Signature", Convert.ToBase64String(hmacsha256.ComputeHash(Encoding.ASCII.GetBytes(signature))));
				request.Headers.Add("X-NCMB-Timestamp", now.ToString("yyyy-MM-ddTHH:mm:sszzzz"));
			}
		}
	}
}
