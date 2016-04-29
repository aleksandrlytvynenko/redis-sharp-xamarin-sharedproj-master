using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RedisSharp;

namespace RedisSharpDroid
{
	[Activity (Label = "RedisSharpDroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};

			// replace with Redis server IP address.  I found simulators
			// don't like localhost
			string host = "192.168.1.111";
			Redis r = new Redis (host);

			// set some values in redis
			//r.Set ("foo", "droid was here");

			// subscribe to channel foo
			RedisSub rs = new RedisSub(r.Host, r.Port);

			RedisSubEventHandler eventHandler = new RedisSubEventHandler (MessageReceived);
			rs.MessageReceived += eventHandler;
			rs.SubscribeReceived += eventHandler;
			rs.UnsubscribeReceived += eventHandler;

			// subscribe to channel foo
			rs.Subscribe ("foo");


		}

		static void MessageReceived (object sender, RedisSubEventArgs e)
		{
			switch (e.kind) {
			case "psubscribe":
			case "punsubscribe":
				Console.WriteLine ("Received {0}: {1}", e.kind, e.message);
				break;
			default:
				Console.WriteLine ("Received {0}: {1}", e.kind, e.message);
				break;
			}
		}
	}
}


