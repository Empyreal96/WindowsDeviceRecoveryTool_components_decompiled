using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x0200008F RID: 143
	internal static class SharedAccessSignatureHelper
	{
		// Token: 0x06000F9F RID: 3999 RVA: 0x0003B0BC File Offset: 0x000392BC
		internal static UriQueryBuilder GetSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string accessPolicyIdentifier, string resourceType, string signature, string accountKeyName, string sasVersion)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceType", resourceType);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sv", sasVersion);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sr", resourceType);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "si", accessPolicyIdentifier);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sk", accountKeyName);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sig", signature);
			if (policy != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "st", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessStartTime));
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "se", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessExpiryTime));
				string value = SharedAccessBlobPolicy.PermissionsToString(policy.Permissions);
				if (!string.IsNullOrEmpty(value))
				{
					SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sp", value);
				}
			}
			if (headers != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscc", headers.CacheControl);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rsct", headers.ContentType);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rsce", headers.ContentEncoding);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscl", headers.ContentLanguage);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscd", headers.ContentDisposition);
			}
			return uriQueryBuilder;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0003B1C4 File Offset: 0x000393C4
		internal static UriQueryBuilder GetSignature(SharedAccessFilePolicy policy, SharedAccessFileHeaders headers, string accessPolicyIdentifier, string resourceType, string signature, string accountKeyName, string sasVersion)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceType", resourceType);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sv", sasVersion);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sr", resourceType);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "si", accessPolicyIdentifier);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sk", accountKeyName);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sig", signature);
			if (policy != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "st", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessStartTime));
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "se", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessExpiryTime));
				string value = SharedAccessFilePolicy.PermissionsToString(policy.Permissions);
				if (!string.IsNullOrEmpty(value))
				{
					SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sp", value);
				}
			}
			if (headers != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscc", headers.CacheControl);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rsct", headers.ContentType);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rsce", headers.ContentEncoding);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscl", headers.ContentLanguage);
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "rscd", headers.ContentDisposition);
			}
			return uriQueryBuilder;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003B2CC File Offset: 0x000394CC
		internal static UriQueryBuilder GetSignature(SharedAccessQueuePolicy policy, string accessPolicyIdentifier, string signature, string accountKeyName, string sasVersion)
		{
			CommonUtility.AssertNotNull("signature", signature);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sv", sasVersion);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "si", accessPolicyIdentifier);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sk", accountKeyName);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sig", signature);
			if (policy != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "st", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessStartTime));
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "se", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessExpiryTime));
				string value = SharedAccessQueuePolicy.PermissionsToString(policy.Permissions);
				if (!string.IsNullOrEmpty(value))
				{
					SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sp", value);
				}
			}
			return uriQueryBuilder;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003B36C File Offset: 0x0003956C
		internal static UriQueryBuilder GetSignature(SharedAccessTablePolicy policy, string tableName, string accessPolicyIdentifier, string startPartitionKey, string startRowKey, string endPartitionKey, string endRowKey, string signature, string accountKeyName, string sasVersion)
		{
			CommonUtility.AssertNotNull("signature", signature);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sv", sasVersion);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "tn", tableName);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "spk", startPartitionKey);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "srk", startRowKey);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "epk", endPartitionKey);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "erk", endRowKey);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "si", accessPolicyIdentifier);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sk", accountKeyName);
			SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sig", signature);
			if (policy != null)
			{
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "st", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessStartTime));
				SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "se", SharedAccessSignatureHelper.GetDateTimeOrNull(policy.SharedAccessExpiryTime));
				string value = SharedAccessTablePolicy.PermissionsToString(policy.Permissions);
				if (!string.IsNullOrEmpty(value))
				{
					SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, "sp", value);
				}
			}
			return uriQueryBuilder;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003B450 File Offset: 0x00039650
		internal static string GetDateTimeOrEmpty(DateTimeOffset? value)
		{
			return SharedAccessSignatureHelper.GetDateTimeOrNull(value) ?? string.Empty;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003B470 File Offset: 0x00039670
		internal static string GetDateTimeOrNull(DateTimeOffset? value)
		{
			return (value != null) ? value.Value.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) : null;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0003B4AC File Offset: 0x000396AC
		internal static void AddEscapedIfNotNull(UriQueryBuilder builder, string name, string value)
		{
			if (value != null)
			{
				builder.Add(name, value);
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0003B4BC File Offset: 0x000396BC
		internal static StorageCredentials ParseQuery(IDictionary<string, string> queryParameters)
		{
			bool flag = false;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in queryParameters)
			{
				string a;
				if ((a = keyValuePair.Key.ToLower()) != null)
				{
					if (!(a == "sig"))
					{
						if (a == "restype" || a == "comp" || a == "snapshot" || a == "api-version")
						{
							list.Add(keyValuePair.Key);
						}
					}
					else
					{
						flag = true;
					}
				}
			}
			foreach (string key in list)
			{
				queryParameters.Remove(key);
			}
			if (flag)
			{
				UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
				foreach (KeyValuePair<string, string> keyValuePair2 in queryParameters)
				{
					SharedAccessSignatureHelper.AddEscapedIfNotNull(uriQueryBuilder, keyValuePair2.Key.ToLower(), keyValuePair2.Value);
				}
				return new StorageCredentials(uriQueryBuilder.ToString());
			}
			return null;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003B61C File Offset: 0x0003981C
		internal static string GetHash(SharedAccessQueuePolicy policy, string accessPolicyIdentifier, string resourceName, string sasVersion, byte[] keyValue)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceName", resourceName);
			CommonUtility.AssertNotNull("keyValue", keyValue);
			CommonUtility.AssertNotNullOrEmpty("sasVersion", sasVersion);
			string text = null;
			DateTimeOffset? value = null;
			DateTimeOffset? value2 = null;
			if (policy != null)
			{
				text = SharedAccessQueuePolicy.PermissionsToString(policy.Permissions);
				value = policy.SharedAccessStartTime;
				value2 = policy.SharedAccessExpiryTime;
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n{5}", new object[]
			{
				text,
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value),
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value2),
				resourceName,
				accessPolicyIdentifier,
				sasVersion
			});
			Logger.LogVerbose(null, "StringToSign = {0}.", new object[]
			{
				text2
			});
			return CryptoUtility.ComputeHmac256(keyValue, text2);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003B6E0 File Offset: 0x000398E0
		internal static string GetHash(SharedAccessTablePolicy policy, string accessPolicyIdentifier, string startPartitionKey, string startRowKey, string endPartitionKey, string endRowKey, string resourceName, string sasVersion, byte[] keyValue)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceName", resourceName);
			CommonUtility.AssertNotNull("keyValue", keyValue);
			CommonUtility.AssertNotNullOrEmpty("sasVersion", sasVersion);
			string text = null;
			DateTimeOffset? value = null;
			DateTimeOffset? value2 = null;
			if (policy != null)
			{
				text = SharedAccessTablePolicy.PermissionsToString(policy.Permissions);
				value = policy.SharedAccessStartTime;
				value2 = policy.SharedAccessExpiryTime;
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}", new object[]
			{
				text,
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value),
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value2),
				resourceName,
				accessPolicyIdentifier,
				sasVersion,
				startPartitionKey,
				startRowKey,
				endPartitionKey,
				endRowKey
			});
			Logger.LogVerbose(null, "StringToSign = {0}.", new object[]
			{
				text2
			});
			return CryptoUtility.ComputeHmac256(keyValue, text2);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0003B7C0 File Offset: 0x000399C0
		internal static string GetHash(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string accessPolicyIdentifier, string resourceName, string sasVersion, byte[] keyValue)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceName", resourceName);
			CommonUtility.AssertNotNull("keyValue", keyValue);
			CommonUtility.AssertNotNullOrEmpty("sasVersion", sasVersion);
			string text = null;
			DateTimeOffset? value = null;
			DateTimeOffset? value2 = null;
			if (policy != null)
			{
				text = SharedAccessBlobPolicy.PermissionsToString(policy.Permissions);
				value = policy.SharedAccessStartTime;
				value2 = policy.SharedAccessExpiryTime;
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n{5}", new object[]
			{
				text,
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value),
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value2),
				resourceName,
				accessPolicyIdentifier,
				sasVersion
			});
			if (string.Equals(sasVersion, "2012-02-12"))
			{
				if (headers != null)
				{
					string message = string.Format(CultureInfo.CurrentCulture, "Headers are not supported in the 2012-02-12 version.", new object[0]);
					throw new ArgumentException(message);
				}
			}
			else
			{
				string text3 = null;
				string text4 = null;
				string text5 = null;
				string text6 = null;
				string text7 = null;
				if (headers != null)
				{
					text3 = headers.CacheControl;
					text4 = headers.ContentDisposition;
					text5 = headers.ContentEncoding;
					text6 = headers.ContentLanguage;
					text7 = headers.ContentType;
				}
				text2 += string.Format(CultureInfo.InvariantCulture, "\n{0}\n{1}\n{2}\n{3}\n{4}", new object[]
				{
					text3,
					text4,
					text5,
					text6,
					text7
				});
			}
			Logger.LogVerbose(null, "StringToSign = {0}.", new object[]
			{
				text2
			});
			return CryptoUtility.ComputeHmac256(keyValue, text2);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0003B930 File Offset: 0x00039B30
		internal static string GetHash(SharedAccessFilePolicy policy, SharedAccessFileHeaders headers, string accessPolicyIdentifier, string resourceName, string sasVersion, byte[] keyValue)
		{
			CommonUtility.AssertNotNullOrEmpty("resourceName", resourceName);
			CommonUtility.AssertNotNull("keyValue", keyValue);
			CommonUtility.AssertNotNullOrEmpty("sasVersion", sasVersion);
			string text = null;
			DateTimeOffset? value = null;
			DateTimeOffset? value2 = null;
			if (policy != null)
			{
				text = SharedAccessFilePolicy.PermissionsToString(policy.Permissions);
				value = policy.SharedAccessStartTime;
				value2 = policy.SharedAccessExpiryTime;
			}
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			if (headers != null)
			{
				text2 = headers.CacheControl;
				text3 = headers.ContentDisposition;
				text4 = headers.ContentEncoding;
				text5 = headers.ContentLanguage;
				text6 = headers.ContentType;
			}
			string text7 = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}", new object[]
			{
				text,
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value),
				SharedAccessSignatureHelper.GetDateTimeOrEmpty(value2),
				resourceName,
				accessPolicyIdentifier,
				sasVersion,
				text2,
				text3,
				text4,
				text5,
				text6
			});
			Logger.LogVerbose(null, "StringToSign = {0}.", new object[]
			{
				text7
			});
			return CryptoUtility.ComputeHmac256(keyValue, text7);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0003BA50 File Offset: 0x00039C50
		internal static string ValidateSASVersionString(string sasVersion)
		{
			if (sasVersion == null)
			{
				return "2015-02-21";
			}
			if (string.Equals(sasVersion, "2013-08-15"))
			{
				return "2013-08-15";
			}
			if (string.Equals(sasVersion, "2012-02-12"))
			{
				return "2012-02-12";
			}
			string message = string.Format(CultureInfo.CurrentCulture, "SAS Version invalid. Valid versions include 2012-02-12 and 2013-08-15.", new object[0]);
			throw new ArgumentException(message);
		}
	}
}
