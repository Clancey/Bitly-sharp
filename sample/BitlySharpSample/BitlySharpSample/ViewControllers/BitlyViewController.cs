// 
//  Copyright 2012  abhatia
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
using MonoTouch.Dialog;
using Bitly;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BitlySharpSample
{
	public class BitlyViewController : DialogViewController
	{
		// Credentials go here.
		const string USER_NAME = "anujbaba";
		const string APIKEY = "R_b88b47858d0407a0505aa9854d5c203c";
		
		private static Api _BitlyApi;
		
		Action<string> ShortenUrlAction;
		Action<string> ExpandUrlAction;
		
		EntryElement ShortenUrlElement;
		EntryElement ExpandUrlElement;
		
		StringElement ResultElement;
		
		public BitlyViewController()
			: base(new RootElement("Shorten URL"), true)
		{
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		
		public override void LoadView()
		{
			base.LoadView();
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			_BitlyApi = new Api(USER_NAME, APIKEY);
			ShortenUrlAction = ShortenUrlActionHandler;
			ExpandUrlAction = ExpandUrlActionHandler;
			
			ShortenUrlElement = new EntryElement("Url to Shorten:", "", "");
			ExpandUrlElement = new EntryElement("Btly Url to Expand: ", "", "");
			
			var urlSection = new Section("URLs") {
				ShortenUrlElement,
				ExpandUrlElement,
			};
			
			var operationSection = new Section("") {
				new StringElement("Shorten URL", () => { 
					ShortenUrlAction.BeginInvoke(ShortenUrlElement.Value, (ar) => {}, null);
				}),
				new StringElement("Expand URL", () => {
					ExpandUrlAction.BeginInvoke(ExpandUrlElement.Value, (ar) => {}, null);
				}),
			};
			
			ResultElement = new StringElement("");
			
			var resultSection = new Section("Result") {
				ResultElement,
			};
			
			this.Root.Add(new Section[] { urlSection, operationSection, resultSection });
		}
		
		public void ShortenUrlActionHandler(string url)
		{
			if(string.IsNullOrWhiteSpace(url)) {
				NotifyInvalidUrl();
				return;
			}
			var result = _BitlyApi.Shorten(url);
			UpdateResultElement(result);
		}
		
		public void ExpandUrlActionHandler(string url)
		{
			if(string.IsNullOrWhiteSpace(url)) {
				NotifyInvalidUrl();
				return;
			}
			
			var result = _BitlyApi.Expand(url);
			UpdateResultElement(result);
		}
		
		private void UpdateResultElement(string result)
		{
			ResultElement.Value = result;
			
			using(var pool = new NSAutoreleasePool()) {
				pool.BeginInvokeOnMainThread(()=>{
					this.ReloadData();
				});
			}
		}
		
		public void NotifyInvalidUrl()
		{
			var alert = new UIAlertView("Invaid URL", "Invalid URL Specified. Try again...", null, "OK", null);
			alert.Show();
		}
		
		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		
	}
}

