// 
//  Copyright 2012  Clancey
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
using System.Runtime.InteropServices;

namespace Bitly
{
	public class API
	{
		private string username;
		private string apiKey;
		
		public API (string username, string apikey)
		{
			this.username = username;
			this.apiKey = apiKey;
		}
		
		public string Shorten (string url)
		{
			return post ("shorten", "shortUrl", "longUrl", url);
		}
		
		public string GetMeta (string url)
		{
			return post ("info", "htmlMetaDescription", "shortUrl", url);
		}

		public string Expand (string url)
		{
			return post ("expand", "longUrl", "shortUrl", url);
		}

		public int ClickCount (string url)
		{
			return int.Parse (post ("stats", "clicks", "hash", url));
		}

		public string GetUser (string url)
		{
			return post ("info", "shortenedByUser", "shortUrl", url);
		}
		
		private string post (string Btype, string Xtype, string Itype, string input)
		{
			StringBuilder url = new StringBuilder ();  //Build a new string
			url.Append ("http://api.bit.ly/");   //Add base URL
			url.Append (Btype);
			url.Append ("?version=2.0.1");             //Add Version
			url.Append ("&format=xml");
			url.Append ("&");
			url.Append (Itype);
			url.Append ("=");
			url.Append (input);                         //Append longUrl from input
			url.Append ("&login=");                    //Add login "Key"
			url.Append (username);                     //Append login from input
			url.Append ("&apiKey=");                   //Add ApiKey "Key"
			url.Append (apiKey);                       //Append ApiKey from input
			WebRequest request = WebRequest.Create (url.ToString ()); //prepare web request
			StreamReader responseStream = new StreamReader (request.GetResponse ().GetResponseStream ()); //prepare responese holder
			String response = responseStream.ReadToEnd (); //fill up response
			responseStream.Close (); //Close stream

			string data = response.ToString (); //Turn it into a string
			string newdata = XmlParse_general (data, Xtype); //parse the XML
			if (newdata == "Error") {
				return "";
			} else {
				return newdata;
			}
		}

		private static string XmlParse_general (string Url, string type)    //XML parse Function
		{

			System.Xml.XmlTextReader xmlrt1 = new XmlTextReader (new StringReader (Url));
			while (xmlrt1.Read()) {
				string strNodeType = xmlrt1.NodeType.ToString ();
				string strName = xmlrt1.Name;

				if (strNodeType == "Element" && strName == type) { //get the clicks
					xmlrt1.Read ();
					return xmlrt1.Value; //Return output
				} // end if
			}
			return "";// end while
		}
	}
}

