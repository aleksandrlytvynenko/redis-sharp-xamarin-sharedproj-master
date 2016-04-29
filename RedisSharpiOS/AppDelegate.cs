﻿using Foundation;
using UIKit;
using RedisSharp;
using System;

namespace RedisSharpiOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// create a new window instance based on the screen size
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			// If you have defined a root view controller, set it here:
			Window.RootViewController = new UIViewController();

			// make the window visible
			Window.MakeKeyAndVisible ();

			// replace with Redis server IP address.  I found simulators
			// don't like localhost
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

			

			return true;
		}

		// log out messages received
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

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		
		}
	}
}

