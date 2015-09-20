using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OctopusStore
{
	public class PlainTextMediaTypeFormatter : MediaTypeFormatter
	{
		public PlainTextMediaTypeFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
		}

		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			var taskCompletionSource = new TaskCompletionSource<object>();

			try
			{
				using (var memoryStream = new MemoryStream())
				{
					readStream.CopyTo(memoryStream);
					var text = Encoding.UTF8.GetString(memoryStream.ToArray());
					taskCompletionSource.SetResult(text);
				}
			}
			catch (Exception e)
			{
				taskCompletionSource.SetException(e);
			}

			return taskCompletionSource.Task;
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
		{
			var bytes = Encoding.UTF8.GetBytes(value.ToString());
			return writeStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
		}

		public override bool CanReadType(Type type)
		{
			return type == typeof(string);
		}

		public override bool CanWriteType(Type type)
		{
			return type == typeof(string);
		}
	}
}
