using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core.Auth;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200009A RID: 154
	internal static class NavigationHelper
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x0003CDC0 File Offset: 0x0003AFC0
		internal static string GetContainerName(Uri blobAddress, bool? usePathStyleUris)
		{
			string result;
			string text;
			NavigationHelper.GetContainerNameAndBlobName(blobAddress, usePathStyleUris, out result, out text);
			return result;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0003CDDC File Offset: 0x0003AFDC
		internal static string GetBlobName(Uri blobAddress, bool? usePathStyleUris)
		{
			string text;
			string stringToUnescape;
			NavigationHelper.GetContainerNameAndBlobName(blobAddress, usePathStyleUris, out text, out stringToUnescape);
			return Uri.UnescapeDataString(stringToUnescape);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0003CDFC File Offset: 0x0003AFFC
		internal static string GetShareName(Uri fileAddress, bool? usePathStyleUris)
		{
			string result;
			string text;
			NavigationHelper.GetShareNameAndFileName(fileAddress, usePathStyleUris, out result, out text);
			return result;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003CE18 File Offset: 0x0003B018
		internal static string GetFileName(Uri fileAddress, bool? usePathStyleUris)
		{
			string text;
			string stringToUnescape;
			NavigationHelper.GetShareNameAndFileName(fileAddress, usePathStyleUris, out text, out stringToUnescape);
			return Uri.UnescapeDataString(stringToUnescape);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003CE38 File Offset: 0x0003B038
		internal static string GetFileAndDirectoryName(Uri fileAddress, bool? usePathStyleUris)
		{
			CommonUtility.AssertNotNull("fileAddress", fileAddress);
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(fileAddress));
			}
			string[] segments = fileAddress.Segments;
			int num = usePathStyleUris.Value ? 2 : 1;
			if (segments.Length - 1 < num)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Invalid file address '{0}', missing share information", new object[]
				{
					fileAddress
				});
				throw new ArgumentException(message, "fileAddress");
			}
			if (segments.Length - 1 == num)
			{
				return string.Empty;
			}
			return Uri.UnescapeDataString(string.Concat(segments.Skip(num + 1)));
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0003CECC File Offset: 0x0003B0CC
		internal static bool GetBlobParentNameAndAddress(StorageUri blobAddress, string delimiter, bool? usePathStyleUris, out string parentName, out StorageUri parentAddress)
		{
			CommonUtility.AssertNotNull("blobAbsoluteUriString", blobAddress);
			CommonUtility.AssertNotNullOrEmpty("delimiter", delimiter);
			parentName = null;
			parentAddress = null;
			string text;
			StorageUri storageUri;
			if (!NavigationHelper.GetContainerNameAndAddress(blobAddress, usePathStyleUris, out text, out storageUri))
			{
				return false;
			}
			Uri uri = storageUri.PrimaryUri.MakeRelativeUri(blobAddress.PrimaryUri);
			string text2 = uri.OriginalString;
			delimiter = Uri.EscapeUriString(delimiter);
			if (text2.EndsWith(delimiter, StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - delimiter.Length);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				int num = text2.LastIndexOf(delimiter, StringComparison.Ordinal);
				if (num <= 0)
				{
					parentName = string.Empty;
					parentAddress = storageUri;
				}
				else
				{
					parentName = Uri.UnescapeDataString(text2.Substring(0, num + delimiter.Length)).Substring(text.Length + 1);
					parentAddress = NavigationHelper.AppendPathToUri(storageUri, parentName);
				}
			}
			return parentName != null;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0003CFA0 File Offset: 0x0003B1A0
		internal static bool GetFileParentNameAndAddress(StorageUri fileAddress, bool? usePathStyleUris, out string parentName, out StorageUri parentAddress)
		{
			CommonUtility.AssertNotNull("fileAbsoluteUriString", fileAddress);
			parentName = null;
			parentAddress = null;
			string text;
			StorageUri storageUri;
			NavigationHelper.GetShareNameAndAddress(fileAddress, usePathStyleUris, out text, out storageUri);
			Uri uri = storageUri.PrimaryUri.MakeRelativeUri(fileAddress.PrimaryUri);
			string text2 = uri.OriginalString;
			if (text2.Length > 0 && text2[text2.Length - 1] == '/')
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				int num = text2.LastIndexOf('/');
				if (num <= text.Length)
				{
					parentName = string.Empty;
					parentAddress = storageUri;
				}
				else
				{
					parentName = Uri.UnescapeDataString(text2.Substring(0, num)).Substring(text.Length + 1);
					parentAddress = NavigationHelper.AppendPathToUri(storageUri, parentName);
					num = parentName.LastIndexOf('/');
					if (num >= 0)
					{
						parentName = parentName.Substring(num + 1);
					}
				}
			}
			return parentName != null;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0003D081 File Offset: 0x0003B281
		internal static StorageUri GetServiceClientBaseAddress(StorageUri addressUri, bool? usePathStyleUris)
		{
			return new StorageUri(NavigationHelper.GetServiceClientBaseAddress(addressUri.PrimaryUri, usePathStyleUris), NavigationHelper.GetServiceClientBaseAddress(addressUri.SecondaryUri, usePathStyleUris));
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003D0A0 File Offset: 0x0003B2A0
		internal static Uri GetServiceClientBaseAddress(Uri addressUri, bool? usePathStyleUris)
		{
			if (addressUri == null)
			{
				return null;
			}
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(addressUri));
			}
			Uri uri = new Uri(addressUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped));
			if (!usePathStyleUris.Value)
			{
				return uri;
			}
			string[] segments = addressUri.Segments;
			if (segments.Length < 2)
			{
				string paramName = string.Format(CultureInfo.CurrentCulture, "Missing account name information inside path style uri. Path style uris should be of the form http://<IPAddressPlusPort>/<accountName>", new object[0]);
				throw new ArgumentException("address", paramName);
			}
			return new Uri(uri, segments[1]);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003D120 File Offset: 0x0003B320
		internal static StorageUri AppendPathToUri(StorageUri uriList, string relativeUri)
		{
			return NavigationHelper.AppendPathToUri(uriList, relativeUri, "/");
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003D12E File Offset: 0x0003B32E
		internal static StorageUri AppendPathToUri(StorageUri uriList, string relativeUri, string sep)
		{
			return new StorageUri(NavigationHelper.AppendPathToSingleUri(uriList.PrimaryUri, relativeUri, sep), NavigationHelper.AppendPathToSingleUri(uriList.SecondaryUri, relativeUri, sep));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003D14F File Offset: 0x0003B34F
		internal static Uri AppendPathToSingleUri(Uri uri, string relativeUri)
		{
			return NavigationHelper.AppendPathToSingleUri(uri, relativeUri, "/");
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003D160 File Offset: 0x0003B360
		internal static Uri AppendPathToSingleUri(Uri uri, string relativeUri, string sep)
		{
			if (uri == null || relativeUri.Length == 0)
			{
				return uri;
			}
			sep = Uri.EscapeUriString(sep);
			relativeUri = Uri.EscapeUriString(relativeUri);
			UriBuilder uriBuilder = new UriBuilder(uri);
			string str;
			if (uriBuilder.Path.EndsWith(sep, StringComparison.Ordinal))
			{
				str = relativeUri;
			}
			else
			{
				str = sep + relativeUri;
			}
			UriBuilder uriBuilder2 = uriBuilder;
			uriBuilder2.Path += str;
			return uriBuilder.Uri;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003D1CC File Offset: 0x0003B3CC
		internal static string GetContainerNameFromContainerAddress(Uri uri, bool? usePathStyleUris)
		{
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(uri));
			}
			if (!usePathStyleUris.Value)
			{
				return uri.AbsolutePath.Substring(1);
			}
			string[] array = uri.AbsolutePath.Split(NavigationHelper.SlashAsSplitOptions);
			if (array.Length < 3)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot find account information inside Uri '{0}'", new object[]
				{
					uri.AbsoluteUri
				});
				throw new InvalidOperationException(message);
			}
			return array[2];
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003D247 File Offset: 0x0003B447
		internal static string GetQueueNameFromUri(Uri uri, bool? usePathStyleUris)
		{
			return NavigationHelper.GetContainerNameFromContainerAddress(uri, usePathStyleUris);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003D250 File Offset: 0x0003B450
		internal static string GetTableNameFromUri(Uri uri, bool? usePathStyleUris)
		{
			return NavigationHelper.GetContainerNameFromContainerAddress(uri, usePathStyleUris);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003D259 File Offset: 0x0003B459
		internal static string GetShareNameFromShareAddress(Uri uri, bool? usePathStyleUris)
		{
			return NavigationHelper.GetContainerNameFromContainerAddress(uri, usePathStyleUris);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003D264 File Offset: 0x0003B464
		private static bool GetContainerNameAndAddress(StorageUri blobAddress, bool? usePathStyleUris, out string containerName, out StorageUri containerUri)
		{
			string text;
			bool containerNameAndBlobName = NavigationHelper.GetContainerNameAndBlobName(blobAddress.PrimaryUri, usePathStyleUris, out containerName, out text);
			containerUri = NavigationHelper.AppendPathToUri(NavigationHelper.GetServiceClientBaseAddress(blobAddress, usePathStyleUris), containerName);
			return containerNameAndBlobName;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003D294 File Offset: 0x0003B494
		private static void GetShareNameAndAddress(StorageUri fileAddress, bool? usePathStyleUris, out string shareName, out StorageUri shareUri)
		{
			string text;
			NavigationHelper.GetShareNameAndFileName(fileAddress.PrimaryUri, usePathStyleUris, out shareName, out text);
			shareUri = NavigationHelper.AppendPathToUri(NavigationHelper.GetServiceClientBaseAddress(fileAddress, usePathStyleUris), shareName);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003D2C0 File Offset: 0x0003B4C0
		private static bool GetContainerNameAndBlobName(Uri blobAddress, bool? usePathStyleUris, out string containerName, out string blobName)
		{
			CommonUtility.AssertNotNull("blobAddress", blobAddress);
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(blobAddress));
			}
			string[] segments = blobAddress.Segments;
			int num = usePathStyleUris.Value ? 2 : 1;
			int num2 = num + 1;
			if (segments.Length - 1 < num)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Invalid blob address '{0}', missing container information", new object[]
				{
					blobAddress
				});
				throw new ArgumentException(message, "blobAddress");
			}
			if (segments.Length - 1 == num)
			{
				string text = segments[num];
				if (text[text.Length - 1] == '/')
				{
					containerName = text.Trim(new char[]
					{
						'/'
					});
					blobName = string.Empty;
				}
				else
				{
					containerName = "$root";
					blobName = text;
				}
				return false;
			}
			containerName = segments[num].Trim(new char[]
			{
				'/'
			});
			string[] array = new string[segments.Length - num2];
			Array.Copy(segments, num2, array, 0, array.Length);
			blobName = string.Concat(array);
			return true;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003D3CC File Offset: 0x0003B5CC
		private static void GetShareNameAndFileName(Uri fileAddress, bool? usePathStyleUris, out string shareName, out string fileName)
		{
			CommonUtility.AssertNotNull("fileAddress", fileAddress);
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(fileAddress));
			}
			string[] segments = fileAddress.Segments;
			int num = usePathStyleUris.Value ? 2 : 1;
			if (segments.Length - 1 < num)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Invalid file address '{0}', missing share information", new object[]
				{
					fileAddress
				});
				throw new ArgumentException(message, "fileAddress");
			}
			if (segments.Length - 1 == num)
			{
				string text = segments[num];
				shareName = text.Trim(new char[]
				{
					'/'
				});
				fileName = string.Empty;
				return;
			}
			shareName = segments[num].Trim(new char[]
			{
				'/'
			});
			fileName = segments[segments.Length - 1];
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0003D494 File Offset: 0x0003B694
		internal static DateTimeOffset ParseSnapshotTime(string snapshot)
		{
			DateTimeOffset result;
			if (!DateTimeOffset.TryParse(snapshot, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
			{
				CommonUtility.ArgumentOutOfRange("snapshot", snapshot);
			}
			return result;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003D4C0 File Offset: 0x0003B6C0
		internal static StorageUri ParseBlobQueryAndVerify(StorageUri address, out StorageCredentials parsedCredentials, out DateTimeOffset? parsedSnapshot)
		{
			StorageCredentials storageCredentials;
			DateTimeOffset? dateTimeOffset;
			return new StorageUri(NavigationHelper.ParseBlobQueryAndVerify(address.PrimaryUri, out parsedCredentials, out parsedSnapshot), NavigationHelper.ParseBlobQueryAndVerify(address.SecondaryUri, out storageCredentials, out dateTimeOffset));
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003D4F0 File Offset: 0x0003B6F0
		private static Uri ParseBlobQueryAndVerify(Uri address, out StorageCredentials parsedCredentials, out DateTimeOffset? parsedSnapshot)
		{
			parsedCredentials = null;
			parsedSnapshot = null;
			if (address == null)
			{
				return null;
			}
			if (!address.IsAbsoluteUri)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Address '{0}' is a relative address. Only absolute addresses are permitted.", new object[]
				{
					address.ToString()
				});
				throw new ArgumentException(message, "address");
			}
			IDictionary<string, string> dictionary = HttpWebUtility.ParseQueryString(address.Query);
			string text;
			if (dictionary.TryGetValue("snapshot", out text) && !string.IsNullOrEmpty(text))
			{
				parsedSnapshot = new DateTimeOffset?(NavigationHelper.ParseSnapshotTime(text));
			}
			parsedCredentials = SharedAccessSignatureHelper.ParseQuery(dictionary);
			if (parsedCredentials != null)
			{
				string value;
				dictionary.TryGetValue("sr", out value);
				if (string.IsNullOrEmpty(value))
				{
					string message2 = string.Format(CultureInfo.CurrentCulture, "Missing mandatory parameters for valid Shared Access Signature", new object[0]);
					throw new ArgumentException(message2);
				}
			}
			return new Uri(address.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped));
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003D5D0 File Offset: 0x0003B7D0
		internal static StorageUri ParseFileQueryAndVerify(StorageUri address, out StorageCredentials parsedCredentials)
		{
			StorageCredentials storageCredentials;
			return new StorageUri(NavigationHelper.ParseFileQueryAndVerify(address.PrimaryUri, out parsedCredentials), NavigationHelper.ParseFileQueryAndVerify(address.SecondaryUri, out storageCredentials));
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0003D5FC File Offset: 0x0003B7FC
		private static Uri ParseFileQueryAndVerify(Uri address, out StorageCredentials parsedCredentials)
		{
			parsedCredentials = null;
			if (address == null)
			{
				return null;
			}
			if (!address.IsAbsoluteUri)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Address '{0}' is a relative address. Only absolute addresses are permitted.", new object[]
				{
					address.ToString()
				});
				throw new ArgumentException(message, "address");
			}
			IDictionary<string, string> dictionary = HttpWebUtility.ParseQueryString(address.Query);
			parsedCredentials = SharedAccessSignatureHelper.ParseQuery(dictionary);
			if (parsedCredentials != null)
			{
				string value;
				dictionary.TryGetValue("sr", out value);
				if (string.IsNullOrEmpty(value))
				{
					string message2 = string.Format(CultureInfo.CurrentCulture, "Missing mandatory parameters for valid Shared Access Signature", new object[0]);
					throw new ArgumentException(message2);
				}
			}
			return new Uri(address.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped));
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003D6A8 File Offset: 0x0003B8A8
		internal static StorageUri ParseQueueTableQueryAndVerify(StorageUri address, out StorageCredentials parsedCredentials)
		{
			StorageCredentials storageCredentials;
			return new StorageUri(NavigationHelper.ParseQueueTableQueryAndVerify(address.PrimaryUri, out parsedCredentials), NavigationHelper.ParseQueueTableQueryAndVerify(address.SecondaryUri, out storageCredentials));
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0003D6D4 File Offset: 0x0003B8D4
		private static Uri ParseQueueTableQueryAndVerify(Uri address, out StorageCredentials parsedCredentials)
		{
			parsedCredentials = null;
			if (address == null)
			{
				return null;
			}
			if (!address.IsAbsoluteUri)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Address '{0}' is a relative address. Only absolute addresses are permitted.", new object[]
				{
					address.ToString()
				});
				throw new ArgumentException(message, "address");
			}
			IDictionary<string, string> queryParameters = HttpWebUtility.ParseQueryString(address.Query);
			parsedCredentials = SharedAccessSignatureHelper.ParseQuery(queryParameters);
			return new Uri(address.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped));
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0003D748 File Offset: 0x0003B948
		internal static string GetAccountNameFromUri(Uri clientUri, bool? usePathStyleUris)
		{
			if (usePathStyleUris == null)
			{
				usePathStyleUris = new bool?(CommonUtility.UsePathStyleAddressing(clientUri));
			}
			string[] array = clientUri.AbsoluteUri.Split(NavigationHelper.SlashAsSplitOptions, StringSplitOptions.RemoveEmptyEntries);
			if (usePathStyleUris.Value)
			{
				if (array.Length < 3)
				{
					string message = string.Format(CultureInfo.InvariantCulture, "Cannot find account information inside Uri '{0}'", new object[]
					{
						clientUri.AbsoluteUri
					});
					throw new InvalidOperationException(message);
				}
				return array[2];
			}
			else
			{
				if (array.Length < 2)
				{
					string message2 = string.Format(CultureInfo.InvariantCulture, "Cannot find account information inside Uri '{0}'", new object[]
					{
						clientUri.AbsoluteUri
					});
					throw new InvalidOperationException(message2);
				}
				int length = array[1].IndexOf(".", StringComparison.Ordinal);
				return array[1].Substring(0, length);
			}
		}

		// Token: 0x040003B9 RID: 953
		public const string RootContainerName = "$root";

		// Token: 0x040003BA RID: 954
		public const string Slash = "/";

		// Token: 0x040003BB RID: 955
		public const string Dot = ".";

		// Token: 0x040003BC RID: 956
		public const char SlashChar = '/';

		// Token: 0x040003BD RID: 957
		public static readonly char[] SlashAsSplitOptions = "/".ToCharArray();

		// Token: 0x040003BE RID: 958
		public static readonly char[] DotAsSplitOptions = ".".ToCharArray();
	}
}
