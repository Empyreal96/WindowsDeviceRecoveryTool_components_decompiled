using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.File;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000094 RID: 148
	internal static class CommonUtility
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x0003C420 File Offset: 0x0003A620
		internal static CommandLocationMode GetListingLocationMode(IContinuationToken token)
		{
			if (token != null && token.TargetLocation != null)
			{
				switch (token.TargetLocation.Value)
				{
				case StorageLocation.Primary:
					return CommandLocationMode.PrimaryOnly;
				case StorageLocation.Secondary:
					return CommandLocationMode.SecondaryOnly;
				default:
					CommonUtility.ArgumentOutOfRange("TargetLocation", token.TargetLocation.Value);
					break;
				}
			}
			return CommandLocationMode.PrimaryOrSecondary;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003C484 File Offset: 0x0003A684
		internal static ExecutionState<NullType> CreateTemporaryExecutionState(BlobRequestOptions options)
		{
			RESTCommand<NullType> cmd = new RESTCommand<NullType>(new StorageCredentials(), null);
			if (options != null)
			{
				options.ApplyToStorageCommand<NullType>(cmd);
			}
			return new ExecutionState<NullType>(cmd, (options != null) ? options.RetryPolicy : null, new OperationContext());
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003C4C0 File Offset: 0x0003A6C0
		internal static ExecutionState<NullType> CreateTemporaryExecutionState(FileRequestOptions options)
		{
			RESTCommand<NullType> cmd = new RESTCommand<NullType>(new StorageCredentials(), null);
			if (options != null)
			{
				options.ApplyToStorageCommand<NullType>(cmd);
			}
			return new ExecutionState<NullType>(cmd, (options != null) ? options.RetryPolicy : null, new OperationContext());
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003C4FA File Offset: 0x0003A6FA
		public static TimeSpan MaxTimeSpan(TimeSpan val1, TimeSpan val2)
		{
			if (!(val1 > val2))
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003C508 File Offset: 0x0003A708
		public static string GetFirstHeaderValue<T>(IEnumerable<T> headerValues) where T : class
		{
			if (headerValues != null)
			{
				T t = headerValues.FirstOrDefault<T>();
				if (t != null)
				{
					return t.ToString().TrimStart(new char[0]);
				}
			}
			return null;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003C541 File Offset: 0x0003A741
		internal static void AssertNotNullOrEmpty(string paramName, string value)
		{
			CommonUtility.AssertNotNull(paramName, value);
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("The argument must not be empty string.", paramName);
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003C55E File Offset: 0x0003A75E
		internal static void AssertNotNull(string paramName, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0003C56C File Offset: 0x0003A76C
		internal static void ArgumentOutOfRange(string paramName, object value)
		{
			throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.InvariantCulture, "The argument is out of range. Value passed: {0}", new object[]
			{
				value
			}));
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0003C59C File Offset: 0x0003A79C
		internal static void AssertInBounds<T>(string paramName, T val, T min, T max) where T : IComparable
		{
			if (val.CompareTo(min) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.InvariantCulture, "The argument '{0}' is smaller than minimum of '{1}'", new object[]
				{
					paramName,
					min
				}));
			}
			if (val.CompareTo(max) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.InvariantCulture, "The argument '{0}' is larger than maximum of '{1}'", new object[]
				{
					paramName,
					max
				}));
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0003C62C File Offset: 0x0003A82C
		internal static void AssertInBounds<T>(string paramName, T val, T min) where T : IComparable
		{
			if (val.CompareTo(min) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.InvariantCulture, "The argument '{0}' is smaller than minimum of '{1}'", new object[]
				{
					paramName,
					min
				}));
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0003C67A File Offset: 0x0003A87A
		internal static void CheckStringParameter(string paramName, bool canBeNullOrEmpty, string value, int maxSize)
		{
			if (!canBeNullOrEmpty)
			{
				CommonUtility.AssertNotNullOrEmpty(value, paramName);
			}
			CommonUtility.AssertInBounds<int>(value, paramName.Length, 0, maxSize);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003C694 File Offset: 0x0003A894
		internal static int RoundUpToSeconds(this TimeSpan timeSpan)
		{
			return (int)Math.Ceiling(timeSpan.TotalSeconds);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003C6A4 File Offset: 0x0003A8A4
		internal static byte[] BinaryAppend(byte[] arr1, byte[] arr2)
		{
			int num = arr1.Length + arr2.Length;
			byte[] array = new byte[num];
			Array.Copy(arr1, array, arr1.Length);
			Array.Copy(arr2, 0, array, arr1.Length, arr2.Length);
			return array;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003C6D9 File Offset: 0x0003A8D9
		internal static bool UsePathStyleAddressing(Uri uri)
		{
			CommonUtility.AssertNotNull("uri", uri);
			return uri.HostNameType != UriHostNameType.Dns || CommonUtility.PathStylePorts.Contains(uri.Port);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003C704 File Offset: 0x0003A904
		internal static string ReadElementAsString(string elementName, XmlReader reader)
		{
			string result = null;
			if (reader.IsStartElement(elementName))
			{
				if (reader.IsEmptyElement)
				{
					reader.Skip();
				}
				else
				{
					result = reader.ReadElementContentAsString();
				}
				reader.MoveToContent();
				return result;
			}
			throw new XmlException(elementName);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003C95C File Offset: 0x0003AB5C
		internal static IEnumerable<T> LazyEnumerable<T>(Func<IContinuationToken, ResultSegment<T>> segmentGenerator, long maxResults)
		{
			ResultSegment<T> currentSeg = segmentGenerator(null);
			long count = 0L;
			for (;;)
			{
				foreach (T result in currentSeg.Results)
				{
					yield return result;
					count += 1L;
					if (count >= maxResults)
					{
						break;
					}
				}
				if (count >= maxResults || currentSeg.ContinuationToken == null)
				{
					break;
				}
				currentSeg = segmentGenerator(currentSeg.ContinuationToken);
			}
			yield break;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003C980 File Offset: 0x0003AB80
		internal static void ApplyRequestOptimizations(HttpWebRequest request, long length)
		{
			if (length >= 65536L)
			{
				request.AllowWriteStreamBuffering = false;
			}
			if (length >= 0L)
			{
				request.ContentLength = length;
			}
			request.ServicePoint.Expect100Continue = false;
		}

		// Token: 0x040003B3 RID: 947
		private static readonly int[] PathStylePorts = new int[]
		{
			10000,
			10001,
			10002,
			10003,
			10004,
			10100,
			10101,
			10102,
			10103,
			10104,
			11000,
			11001,
			11002,
			11003,
			11004,
			11100,
			11101,
			11102,
			11103,
			11104
		};
	}
}
