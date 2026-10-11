using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MiniGame.Core;

public static class UtilsPing
{
	public class PingResult
	{
		private object _lock = new object();

		private long _httpRtt = 460L;

		private string _httpError = string.Empty;

		private long _udpRtt = 460L;

		private string _udpError = string.Empty;

		public PingRequest Request { get; }

		public string Url
		{
			get
			{
				lock (_lock)
				{
					return (RttMs == UdpRtt) ? Request.UdpUrl : Request.HttpUrl;
				}
			}
		}

		public long RttMs
		{
			get
			{
				lock (_lock)
				{
					return Math.Min(HttpRtt, UdpRtt);
				}
			}
		}

		public string Error
		{
			get
			{
				lock (_lock)
				{
					return (RttMs < 460) ? "" : ("http:" + HttpError + " udp:" + UdpError);
				}
			}
		}

		public long HttpRtt
		{
			get
			{
				lock (_lock)
				{
					return _httpRtt;
				}
			}
			set
			{
				lock (_lock)
				{
					_httpRtt = value;
				}
			}
		}

		public string HttpError
		{
			get
			{
				lock (_lock)
				{
					return _httpError;
				}
			}
			set
			{
				lock (_lock)
				{
					_httpError = value;
				}
			}
		}

		public long UdpRtt
		{
			get
			{
				lock (_lock)
				{
					return _udpRtt;
				}
			}
			set
			{
				lock (_lock)
				{
					_udpRtt = value;
				}
			}
		}

		public string UdpError
		{
			get
			{
				lock (_lock)
				{
					return _udpError;
				}
			}
			set
			{
				lock (_lock)
				{
					_udpError = value;
				}
			}
		}

		public PingResult(PingRequest request)
		{
			Request = request ?? throw new ArgumentNullException("request");
		}
	}

	public class PingRequest
	{
		public string Name { get; set; }

		public string UdpUrl { get; set; }

		public string HttpUrl { get; set; }

		public PingResult Result { get; set; }

		public PingRequest()
		{
			Result = new PingResult(this);
		}
	}

	public const long kInvalidRttMs = 460L;

	private static readonly HttpClient _sharedHttpClient = new HttpClient();

	private static readonly SemaphoreSlim _httpConcurrencySemaphore = new SemaphoreSlim(4, 4);

	private static Action<Action> _sharedPostMainThread;

	private static ArraySegment<byte> _udpSendBuffer = new ArraySegment<byte>(new byte[4] { 112, 105, 110, 103 });

	private static ArraySegment<byte> _udpRecvBuffer = new ArraySegment<byte>(new byte[1024]);

	public static void SetPostMainThreadDelegate(Action<Action> postToMainThread)
	{
		_sharedPostMainThread = postToMainThread ?? throw new ArgumentNullException("postToMainThread");
	}

	public static CancellationTokenSource PingAll(IReadOnlyList<PingRequest> requests, Action<List<PingResult>> callback, int timeoutMs = 3000, int sampleCount = 2, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		if (_sharedPostMainThread == null)
		{
			throw new InvalidOperationException("Must call SetPostMainThreadDelegate first.");
		}
		CancellationTokenSource combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		System.Threading.Tasks.Task.Run(async delegate
		{
			try
			{
				List<PingResult> results = await PingAllAsync(requests, sampleCount, timeoutMs, combinedCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				_sharedPostMainThread(delegate
				{
					callback?.Invoke(results);
				});
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				List<PingResult> results2 = requests.Select(delegate(PingRequest r)
				{
					r.Result.HttpError = ex3.Message;
					return r.Result;
				}).ToList();
				_sharedPostMainThread(delegate
				{
					callback?.Invoke(results2);
				});
			}
		}, CancellationToken.None);
		return combinedCts;
	}

	public static async Task<List<PingResult>> PingAllAsync(IReadOnlyList<PingRequest> requests, int sampleCount = 2, int timeoutMs = 3000, CancellationToken ct = default(CancellationToken))
	{
		if (requests == null)
		{
			throw new ArgumentNullException("requests");
		}
		List<PingResult> results = new List<PingResult>();
		Task<PingResult>[] httpTasks = new Task<PingResult>[requests.Count];
		Task<PingResult>[] udpTasks = new Task<PingResult>[requests.Count];
		for (int i = 0; i < requests.Count; i++)
		{
			PingResult result = requests[i].Result;
			results.Add(result);
		}
		List<System.Threading.Tasks.Task> list = new List<System.Threading.Tasks.Task>();
		for (int j = 0; j < requests.Count; j++)
		{
			PingRequest pingRequest = requests[j];
			if (!string.IsNullOrEmpty(pingRequest.HttpUrl))
			{
				httpTasks[j] = HttpPingAsync(pingRequest, sampleCount, ct);
				list.Add(httpTasks[j]);
			}
			if (!string.IsNullOrEmpty(pingRequest.UdpUrl))
			{
				udpTasks[j] = UdpPingAsync(pingRequest, sampleCount, ct);
				list.Add(udpTasks[j]);
			}
		}
		if (list.Count == 0)
		{
			return results;
		}
		using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
		{
			System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Delay(timeoutMs, cts.Token);
			await System.Threading.Tasks.Task.WhenAny(System.Threading.Tasks.Task.WhenAll(list), task).ConfigureAwait(continueOnCapturedContext: false);
			cts.Cancel();
		}
		for (int k = 0; k < requests.Count; k++)
		{
			if (httpTasks[k] != null && httpTasks[k].IsCompleted && !httpTasks[k].IsFaulted && !httpTasks[k].IsCanceled)
			{
				await httpTasks[k].ConfigureAwait(continueOnCapturedContext: false);
			}
			if (udpTasks[k] != null && udpTasks[k].IsCompleted && !udpTasks[k].IsFaulted && !udpTasks[k].IsCanceled)
			{
				await udpTasks[k].ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return results;
	}

	public static async Task<PingResult> PingAsync(PingRequest req, int sampleCount, CancellationToken ct = default(CancellationToken))
	{
		bool hasHttp = !string.IsNullOrEmpty(req.HttpUrl);
		bool hasUdp = !string.IsNullOrEmpty(req.UdpUrl);
		PingResult result = req.Result;
		if (!hasHttp && !hasUdp)
		{
			result.HttpRtt = 460L;
			result.HttpError = "No URL provided";
			result.UdpRtt = 460L;
			result.UdpError = "No URL provided";
			return result;
		}
		Task<PingResult> httpTask = (hasHttp ? HttpPingAsync(req, sampleCount, ct) : System.Threading.Tasks.Task.FromResult<PingResult>(null));
		Task<PingResult> udpTask = (hasUdp ? UdpPingAsync(req, sampleCount, ct) : System.Threading.Tasks.Task.FromResult<PingResult>(null));
		await System.Threading.Tasks.Task.WhenAll(hasHttp ? httpTask : System.Threading.Tasks.Task.CompletedTask, hasUdp ? udpTask : System.Threading.Tasks.Task.CompletedTask).ConfigureAwait(continueOnCapturedContext: false);
		if (hasHttp)
		{
			await httpTask.ConfigureAwait(continueOnCapturedContext: false);
		}
		if (hasUdp)
		{
			await udpTask.ConfigureAwait(continueOnCapturedContext: false);
		}
		return result;
	}

	public static async Task<PingResult> HttpPingAsync(string url, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		return await HttpPingAsync(new PingRequest
		{
			HttpUrl = url
		}, sampleCount, ct);
	}

	public static async Task<PingResult> HttpPingAsync(PingRequest request, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		PingResult result = request.Result;
		string url = request.HttpUrl;
		if (string.IsNullOrWhiteSpace(url))
		{
			result.HttpRtt = 460L;
			result.HttpError = "URL is null or empty";
			return result;
		}
		sampleCount = Math.Max(1, sampleCount);
		await _httpConcurrencySemaphore.WaitAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
		CancellationTokenSource ctSend = null;
		try
		{
			ctSend = new CancellationTokenSource();
			ctSend.CancelAfter(TimeSpan.FromSeconds(5.0));
			for (int i = 0; i < sampleCount && !ct.IsCancellationRequested; i++)
			{
				HttpResponseMessage response = null;
				try
				{
					HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Head, url);
					Stopwatch sw = Stopwatch.StartNew();
					Task<HttpResponseMessage> sendTask = _sharedHttpClient.SendAsync(request2, ctSend.Token);
					System.Threading.Tasks.Task timeoutTask = System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2.0), ct);
					System.Threading.Tasks.Task obj = await System.Threading.Tasks.Task.WhenAny(sendTask, timeoutTask).ConfigureAwait(continueOnCapturedContext: false);
					sw.Stop();
					if (obj == timeoutTask)
					{
						result.HttpError = "HTTP request timed out (hard limit)";
						continue;
					}
					response = await sendTask.ConfigureAwait(continueOnCapturedContext: false);
					long elapsedMilliseconds = sw.ElapsedMilliseconds;
					if (elapsedMilliseconds >= 0 && elapsedMilliseconds < result.HttpRtt)
					{
						result.HttpRtt = elapsedMilliseconds;
					}
				}
				catch (OperationCanceledException) when (ct.IsCancellationRequested)
				{
					result.HttpError = "Cancelled";
					break;
				}
				catch (OperationCanceledException)
				{
					result.HttpError = "HTTP request cancelled or internal timeout";
					continue;
				}
				catch (HttpRequestException ex3)
				{
					result.HttpError = "Network Error: " + ex3.Message;
				}
				catch (Exception ex4)
				{
					result.HttpError = "Unexpected Error: " + ex4.Message;
				}
				finally
				{
					response?.Dispose();
				}
				if (i < sampleCount - 1)
				{
					if (ct.IsCancellationRequested)
					{
						break;
					}
					await System.Threading.Tasks.Task.Delay(30, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		finally
		{
			ctSend?.Dispose();
			_httpConcurrencySemaphore.Release();
		}
		return result;
	}

	public static async Task<PingResult> UdpPingAsync(string url, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		return await UdpPingAsync(new PingRequest
		{
			UdpUrl = url
		}, sampleCount, ct);
	}

	public static async Task<PingResult> UdpPingAsync(PingRequest request, int sampleCount = 2, CancellationToken ct = default(CancellationToken))
	{
		PingResult result = request.Result;
		string udpUrl = result.Request.UdpUrl;
		if (string.IsNullOrWhiteSpace(udpUrl))
		{
			result.UdpRtt = 460L;
			result.UdpError = "URL is null or empty";
			return result;
		}
		IPEndPoint endpoint = await TryParseUdpUrlAsync(udpUrl, ct).ConfigureAwait(continueOnCapturedContext: false);
		if (endpoint == null)
		{
			result.UdpRtt = 460L;
			result.UdpError = "Invalid UDP URL or DNS failed";
			return result;
		}
		sampleCount = Math.Max(1, sampleCount);
		byte[] sendBuffer = new byte[1] { 1 };
		byte[] recvBuffer = new byte[64];
		for (int i = 0; i < sampleCount; i++)
		{
			if (ct.IsCancellationRequested)
			{
				break;
			}
			Socket socket = null;
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
				{
					Blocking = false
				};
				try
				{
					socket.SendTo(sendBuffer, endpoint);
				}
				catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock)
				{
				}
				Stopwatch sw = Stopwatch.StartNew();
				EndPoint remoteEp = endpoint;
				bool received = false;
				while (sw.ElapsedMilliseconds < 2000 && !ct.IsCancellationRequested)
				{
					if (socket.Poll(0, SelectMode.SelectRead))
					{
						try
						{
							if (socket.ReceiveFrom(recvBuffer, ref remoteEp) > 0)
							{
								received = true;
								sw.Stop();
								break;
							}
						}
						catch (SocketException ex2)
						{
							result.UdpError = $"Socket error: {ex2.SocketErrorCode}";
							break;
						}
					}
					await System.Threading.Tasks.Task.Delay(10, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (received && !ct.IsCancellationRequested)
				{
					long elapsedMilliseconds = sw.ElapsedMilliseconds;
					if (elapsedMilliseconds < result.UdpRtt)
					{
						result.UdpRtt = elapsedMilliseconds;
					}
				}
				else
				{
					result.UdpError = "UDP Timeout";
				}
			}
			catch (OperationCanceledException)
			{
				result.UdpError = "Cancelled";
				break;
			}
			catch (SocketException ex4)
			{
				result.UdpError = $"SocketError: {ex4.SocketErrorCode}";
			}
			catch (Exception ex5)
			{
				result.UdpError = "Unexpected: " + ex5.Message;
			}
			finally
			{
				socket?.Dispose();
			}
			if (i < sampleCount - 1 && !ct.IsCancellationRequested)
			{
				await System.Threading.Tasks.Task.Delay(10, ct).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return result;
	}

	private static async Task<IPEndPoint> TryParseUdpUrlAsync(string url, CancellationToken ct = default(CancellationToken), int dnsTimeoutMs = 2000)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return null;
		}
		string[] array = url.Split(new char[1] { ':' });
		if (array.Length != 2)
		{
			return null;
		}
		string text = array[0];
		if (!int.TryParse(array[1], out var port) || port <= 0 || port > 65535)
		{
			return null;
		}
		if (IPAddress.TryParse(text, out var address))
		{
			return new IPEndPoint(address, port);
		}
		try
		{
			Task<IPAddress[]> dnsTask = Dns.GetHostAddressesAsync(text);
			System.Threading.Tasks.Task delayTask = System.Threading.Tasks.Task.Delay(dnsTimeoutMs, ct);
			if (await System.Threading.Tasks.Task.WhenAny(dnsTask, delayTask).ConfigureAwait(continueOnCapturedContext: false) == delayTask)
			{
				return null;
			}
			IPAddress[] array2 = await dnsTask.ConfigureAwait(continueOnCapturedContext: false);
			if (array2.Length != 0)
			{
				return new IPEndPoint(array2[0], port);
			}
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (TimeoutException)
		{
			return null;
		}
		catch
		{
		}
		return null;
	}
}
