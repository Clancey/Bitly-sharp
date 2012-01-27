// 
//  Copyright 2011 Xamarin Inc  (http://www.xamarin.com)
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;

namespace Bitly
{
	public class Api
	{
		private string username;
		private string apiKey;
		private string baseUrl = @"http://api.bit.ly/v3/{0}?login=&apiKey={1}&longUrl={2}&format={3}";
		
		public Api (string username, string apikey)
		{
			if(string.IsNullOrEmpty(apikey) || string.IsNullOrWhiteSpace(apikey))
			throw new ArgumentNullException("key","A valid Bit.ly API key is required");
			
			if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
			throw new ArgumentNullException("login", "A valid Bit.ly login is requied");
			
			this.username = username;
			this.apiKey = apikey;
		}
		
		public string Shorten (string url)
		{
			if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException("url", "A valid url must be provided");
			return getUrl(url,"shorten");
		}

		public string Expand (string url)
		{
			if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException("url", "A valid url must be provided");
			
			return getUrl(url,"expand");
		}
		
		private string getUrl(string url,string type)
		{
			string output = string.Empty;
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format(baseUrl,type,username,apiKey, HttpUtility.UrlEncode(url),"txt"));
			
			using (WebResponse webResponse = webRequest.GetResponse())
			{
				using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
				{
					output = reader.ReadToEnd();
				}
			}
			return output;
		}
	}
}

