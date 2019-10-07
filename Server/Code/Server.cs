/* Server.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Net;

namespace EmuTarkovNXT.Server
{
	public class Server
	{
		private RequestHandler requestHandler;
		private ResponseHandler responseHandler;
		private HttpListener listener;
		private readonly string prefix;

		public Server(string address)
		{
			// address cannot be empty
			if (string.IsNullOrEmpty(address))
			{
				return;
			}

			// address cannot already exist
			if (listener.Prefixes.Contains(address))
			{
				return;
			}

			// address must be a valid http url
			Uri uriResult;
			bool validUrl = Uri.TryCreate(address, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;

			if (!validUrl)
			{
				return;
			}

			requestHandler = new RequestHandler();
			responseHandler = new ResponseHandler();
			listener = new HttpListener();
			listener.Prefixes.Add(address);
			prefix = address;
		}

		~Server()
		{
			if (listener.Prefixes.Contains(prefix))
			{
				listener.Prefixes.Remove(prefix);
			}
		}

		public void Start()
		{
			listener.Start();
		}

		public void Stop()
		{
			listener.Stop();
		}

		public void Update()
		{
			HttpListenerContext context = listener.GetContext();

			responseHandler.url = requestHandler.GetUrl(context.Request.Url.Segments);
			responseHandler.body = requestHandler.GetBody(context.Request.InputStream);
			responseHandler.response = context.Response;
			responseHandler.SendResponse();
		}
	}
}
