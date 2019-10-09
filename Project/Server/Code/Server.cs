/* Server.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Net;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Server
{
	public class Server
	{
		private AccountHandler accountHandler;
		private RequestHandler requestHandler;
		private ResponseHandler responseHandler;
		private HttpListener listener;
		private readonly string prefix;

		public Server(string filepath, string address)
		{
			listener = new HttpListener();
			requestHandler = new RequestHandler();
			responseHandler = new ResponseHandler();
			accountHandler = new AccountHandler(filepath, requestHandler);

			// address cannot be empty
			if (string.IsNullOrEmpty(address))
			{
				Log.Error("Address is empty");
				return;
			}

			// address cannot already exist
			if (listener.Prefixes.Contains(address))
			{
				Log.Error("Port is already bound");
				return;
			}

			// address must be a valid http url
			Uri uriResult;
			bool validUrl = Uri.TryCreate(address, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;

			if (!validUrl)
			{
				Log.Error("");
				return;
			}
			
			listener.Prefixes.Add(address);
			prefix = address;

			SetupResponses();
			Log.Info("Initialized server");
		}

		~Server()
		{
			if (listener.Prefixes.Contains(prefix))
			{
				listener.Prefixes.Remove(prefix);
			}

			Log.Info("Destroyed server");
		}

		public void Start()
		{
			listener.Start();
			Log.Info("Started server");
		}

		public void Stop()
		{
			listener.Stop();
			Log.Info("Stopped server");
		}

		public void Update()
		{
			HttpListenerContext context = listener.GetContext();

			requestHandler.SetIp(context.Request);
			requestHandler.SetSid(context.Request);
			requestHandler.SetUrl(context.Request);
			requestHandler.SetBody(context.Request);
			requestHandler.ShowRequestInfo();

			responseHandler.SendResponse(context.Response, requestHandler);			
		}

		private string GetVersion(string body)
		{
			return @"EmuTarkov-NXT | v0.0.1a";
		}

		private void SetupResponses()
		{
			ResponseFactory.AddResponse("/", GetVersion);
			ResponseFactory.AddResponse("/launcher/account/create", accountHandler.CreateAccount);
			ResponseFactory.AddResponse("/launcher/account/delete", accountHandler.DeleteAccount);
			ResponseFactory.AddResponse("/launcher/account/login", accountHandler.LoginAccount);
		}
	}
}
