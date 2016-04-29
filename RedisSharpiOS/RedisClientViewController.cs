using System;

using UIKit;
using RedisSharp;

namespace RedisSharpiOS
{
	public partial class RedisClientViewController : UIViewController
	{
		public RedisClientViewController () : base ("RedisClientViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			outputLabel.Text = null;
			outputLabel.Lines = 0;
			outputLabel.SizeToFit ();
			ConnectToRedis ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		private void ConnectToRedis()
		{
			string host = "192.168.1.111";
			Redis r = new Redis (host);

			// set some values in redis
			//r.Set ("foo", "iOS was here");

			// subscribe to channel foo
			RedisSub rs = new RedisSub(r.Host, r.Port);

			RedisSubEventHandler eventHandler = new RedisSubEventHandler (MessageReceived);
			rs.MessageReceived += eventHandler;
			rs.SubscribeReceived += eventHandler;
			rs.UnsubscribeReceived += eventHandler;

			// subscribe to channel foo
			rs.Subscribe ("foo");
		}

		void MessageReceived (object sender, RedisSubEventArgs e)
		{
			switch (e.kind) {
			case "subscribe":
				WriteToLabel ("subscribed");
				break;
			case "unsubscribe":
				WriteToLabel ("unsubscribed");
				Console.WriteLine ("Received {0}: {1}", e.kind, e.message);
				break;
			default:
				WriteToLabel (e.message.ToString());
				Console.WriteLine ("Received {0}: {1}", e.kind, e.message);
				break;
			}
		}
		void WriteToLabel(string value)
		{
			InvokeOnMainThread(()=>
				outputLabel.Text += value + Environment.NewLine);
		}
	}
}


