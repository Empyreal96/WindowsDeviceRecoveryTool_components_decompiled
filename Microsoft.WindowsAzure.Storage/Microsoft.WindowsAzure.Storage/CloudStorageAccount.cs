using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Analytics;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000070 RID: 112
	public sealed class CloudStorageAccount
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x00036BCD File Offset: 0x00034DCD
		// (set) Token: 0x06000E32 RID: 3634 RVA: 0x00036BD4 File Offset: 0x00034DD4
		public static bool UseV1MD5
		{
			get
			{
				return CloudStorageAccount.version1MD5;
			}
			set
			{
				CloudStorageAccount.version1MD5 = value;
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00036BDC File Offset: 0x00034DDC
		public CloudStorageAccount(StorageCredentials storageCredentials, Uri blobEndpoint, Uri queueEndpoint, Uri tableEndpoint, Uri fileEndpoint) : this(storageCredentials, new StorageUri(blobEndpoint), new StorageUri(queueEndpoint), new StorageUri(tableEndpoint), new StorageUri(fileEndpoint))
		{
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00036BFF File Offset: 0x00034DFF
		public CloudStorageAccount(StorageCredentials storageCredentials, StorageUri blobStorageUri, StorageUri queueStorageUri, StorageUri tableStorageUri, StorageUri fileStorageUri)
		{
			this.Credentials = storageCredentials;
			this.BlobStorageUri = blobStorageUri;
			this.QueueStorageUri = queueStorageUri;
			this.TableStorageUri = tableStorageUri;
			this.FileStorageUri = fileStorageUri;
			this.DefaultEndpoints = false;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00036C33 File Offset: 0x00034E33
		public CloudStorageAccount(StorageCredentials storageCredentials, bool useHttps) : this(storageCredentials, null, useHttps)
		{
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00036C40 File Offset: 0x00034E40
		public CloudStorageAccount(StorageCredentials storageCredentials, string endpointSuffix, bool useHttps)
		{
			CommonUtility.AssertNotNull("storageCredentials", storageCredentials);
			string scheme = useHttps ? "https" : "http";
			this.BlobStorageUri = CloudStorageAccount.ConstructBlobEndpoint(scheme, storageCredentials.AccountName, endpointSuffix);
			this.QueueStorageUri = CloudStorageAccount.ConstructQueueEndpoint(scheme, storageCredentials.AccountName, endpointSuffix);
			this.TableStorageUri = CloudStorageAccount.ConstructTableEndpoint(scheme, storageCredentials.AccountName, endpointSuffix);
			this.FileStorageUri = CloudStorageAccount.ConstructFileEndpoint(scheme, storageCredentials.AccountName, endpointSuffix);
			this.Credentials = storageCredentials;
			this.EndpointSuffix = endpointSuffix;
			this.DefaultEndpoints = true;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00036CCF File Offset: 0x00034ECF
		public static CloudStorageAccount DevelopmentStorageAccount
		{
			get
			{
				if (CloudStorageAccount.devStoreAccount == null)
				{
					CloudStorageAccount.devStoreAccount = CloudStorageAccount.GetDevelopmentStorageAccount(null);
				}
				return CloudStorageAccount.devStoreAccount;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00036CE8 File Offset: 0x00034EE8
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00036CF0 File Offset: 0x00034EF0
		private bool IsDevStoreAccount { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00036CF9 File Offset: 0x00034EF9
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00036D01 File Offset: 0x00034F01
		private string EndpointSuffix { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00036D0A File Offset: 0x00034F0A
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00036D12 File Offset: 0x00034F12
		private IDictionary<string, string> Settings { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00036D1B File Offset: 0x00034F1B
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00036D23 File Offset: 0x00034F23
		private bool DefaultEndpoints { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00036D2C File Offset: 0x00034F2C
		public Uri BlobEndpoint
		{
			get
			{
				if (this.BlobStorageUri == null)
				{
					return null;
				}
				return this.BlobStorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x00036D49 File Offset: 0x00034F49
		public Uri QueueEndpoint
		{
			get
			{
				if (this.QueueStorageUri == null)
				{
					return null;
				}
				return this.QueueStorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00036D66 File Offset: 0x00034F66
		public Uri TableEndpoint
		{
			get
			{
				if (this.TableStorageUri == null)
				{
					return null;
				}
				return this.TableStorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x00036D83 File Offset: 0x00034F83
		public Uri FileEndpoint
		{
			get
			{
				if (this.FileStorageUri == null)
				{
					return null;
				}
				return this.FileStorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00036DA0 File Offset: 0x00034FA0
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00036DA8 File Offset: 0x00034FA8
		public StorageUri BlobStorageUri { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00036DB1 File Offset: 0x00034FB1
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00036DB9 File Offset: 0x00034FB9
		public StorageUri QueueStorageUri { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00036DC2 File Offset: 0x00034FC2
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x00036DCA File Offset: 0x00034FCA
		public StorageUri TableStorageUri { get; private set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00036DD3 File Offset: 0x00034FD3
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00036DDB File Offset: 0x00034FDB
		public StorageUri FileStorageUri { get; private set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00036DE4 File Offset: 0x00034FE4
		// (set) Token: 0x06000E4D RID: 3661 RVA: 0x00036DEC File Offset: 0x00034FEC
		public StorageCredentials Credentials { get; private set; }

		// Token: 0x06000E4E RID: 3662 RVA: 0x00036E00 File Offset: 0x00035000
		public static CloudStorageAccount Parse(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("connectionString");
			}
			CloudStorageAccount result;
			if (CloudStorageAccount.ParseImpl(connectionString, out result, delegate(string err)
			{
				throw new FormatException(err);
			}))
			{
				return result;
			}
			throw new ArgumentException("Error parsing value");
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00036E58 File Offset: 0x00035058
		public static bool TryParse(string connectionString, out CloudStorageAccount account)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				account = null;
				return false;
			}
			bool result;
			try
			{
				result = CloudStorageAccount.ParseImpl(connectionString, out account, delegate(string err)
				{
				});
			}
			catch (Exception)
			{
				account = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00036EB4 File Offset: 0x000350B4
		public CloudTableClient CreateCloudTableClient()
		{
			if (this.TableEndpoint == null)
			{
				throw new InvalidOperationException("No table endpoint configured.");
			}
			if (this.Credentials == null)
			{
				throw new InvalidOperationException("No credentials provided.");
			}
			return new CloudTableClient(this.TableStorageUri, this.Credentials);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00036EF3 File Offset: 0x000350F3
		public CloudQueueClient CreateCloudQueueClient()
		{
			if (this.QueueEndpoint == null)
			{
				throw new InvalidOperationException("No queue endpoint configured.");
			}
			if (this.Credentials == null)
			{
				throw new InvalidOperationException("No credentials provided.");
			}
			return new CloudQueueClient(this.QueueStorageUri, this.Credentials);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00036F32 File Offset: 0x00035132
		public CloudBlobClient CreateCloudBlobClient()
		{
			if (this.BlobEndpoint == null)
			{
				throw new InvalidOperationException("No blob endpoint configured.");
			}
			return new CloudBlobClient(this.BlobStorageUri, this.Credentials);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00036F60 File Offset: 0x00035160
		public CloudAnalyticsClient CreateCloudAnalyticsClient()
		{
			if (this.BlobEndpoint == null)
			{
				throw new InvalidOperationException("No blob endpoint configured.");
			}
			if (this.TableEndpoint == null)
			{
				throw new InvalidOperationException("No table endpoint configured.");
			}
			if (this.Credentials == null)
			{
				throw new InvalidOperationException("No credentials provided.");
			}
			return new CloudAnalyticsClient(this.BlobStorageUri, this.TableStorageUri, this.Credentials);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00036FC9 File Offset: 0x000351C9
		public CloudFileClient CreateCloudFileClient()
		{
			if (this.FileEndpoint == null)
			{
				throw new InvalidOperationException("No file endpoint configured.");
			}
			if (this.Credentials == null)
			{
				throw new InvalidOperationException("No credentials provided.");
			}
			return new CloudFileClient(this.FileStorageUri, this.Credentials);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00037008 File Offset: 0x00035208
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0003704C File Offset: 0x0003524C
		public string ToString(bool exportSecrets)
		{
			if (this.Settings == null)
			{
				this.Settings = new Dictionary<string, string>();
				if (this.DefaultEndpoints)
				{
					this.Settings.Add("DefaultEndpointsProtocol", this.BlobEndpoint.Scheme);
					if (this.EndpointSuffix != null)
					{
						this.Settings.Add("EndpointSuffix", this.EndpointSuffix);
					}
				}
				else
				{
					if (this.BlobEndpoint != null)
					{
						this.Settings.Add("BlobEndpoint", this.BlobEndpoint.ToString());
					}
					if (this.QueueEndpoint != null)
					{
						this.Settings.Add("QueueEndpoint", this.QueueEndpoint.ToString());
					}
					if (this.TableEndpoint != null)
					{
						this.Settings.Add("TableEndpoint", this.TableEndpoint.ToString());
					}
					if (this.FileEndpoint != null)
					{
						this.Settings.Add("FileEndpoint", this.FileEndpoint.ToString());
					}
				}
			}
			List<string> list = (from pair in this.Settings
			select string.Format(CultureInfo.InvariantCulture, "{0}={1}", new object[]
			{
				pair.Key,
				pair.Value
			})).ToList<string>();
			if (this.Credentials != null && !this.IsDevStoreAccount)
			{
				list.Add(this.Credentials.ToString(exportSecrets));
			}
			return string.Join(";", list);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000371B8 File Offset: 0x000353B8
		private static CloudStorageAccount GetDevelopmentStorageAccount(Uri proxyUri)
		{
			UriBuilder uriBuilder = (proxyUri != null) ? new UriBuilder(proxyUri.Scheme, proxyUri.Host) : new UriBuilder("http", "127.0.0.1");
			uriBuilder.Path = "devstoreaccount1";
			uriBuilder.Port = 10000;
			Uri uri = uriBuilder.Uri;
			uriBuilder.Port = 10001;
			Uri uri2 = uriBuilder.Uri;
			uriBuilder.Port = 10002;
			Uri uri3 = uriBuilder.Uri;
			uriBuilder.Path = "devstoreaccount1-secondary";
			uriBuilder.Port = 10000;
			Uri uri4 = uriBuilder.Uri;
			uriBuilder.Port = 10001;
			Uri uri5 = uriBuilder.Uri;
			uriBuilder.Port = 10002;
			Uri uri6 = uriBuilder.Uri;
			StorageCredentials storageCredentials = new StorageCredentials("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");
			CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, new StorageUri(uri, uri4), new StorageUri(uri2, uri5), new StorageUri(uri3, uri6), null);
			cloudStorageAccount.Settings = new Dictionary<string, string>();
			cloudStorageAccount.Settings.Add("UseDevelopmentStorage", "true");
			if (proxyUri != null)
			{
				cloudStorageAccount.Settings.Add("DevelopmentStorageProxyUri", proxyUri.ToString());
			}
			cloudStorageAccount.IsDevStoreAccount = true;
			return cloudStorageAccount;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000372F8 File Offset: 0x000354F8
		internal static bool ParseImpl(string connectionString, out CloudStorageAccount accountInformation, Action<string> error)
		{
			IDictionary<string, string> dictionary = CloudStorageAccount.ParseStringIntoSettings(connectionString, error);
			if (dictionary == null)
			{
				accountInformation = null;
				return false;
			}
			if (CloudStorageAccount.MatchesSpecification(dictionary, new Func<IDictionary<string, string>, IDictionary<string, string>>[]
			{
				CloudStorageAccount.AllRequired(new KeyValuePair<string, Func<string, bool>>[]
				{
					CloudStorageAccount.UseDevelopmentStorageSetting
				}),
				CloudStorageAccount.Optional(new KeyValuePair<string, Func<string, bool>>[]
				{
					CloudStorageAccount.DevelopmentStorageProxyUriSetting
				})
			}))
			{
				string uriString = null;
				if (dictionary.TryGetValue("DevelopmentStorageProxyUri", out uriString))
				{
					accountInformation = CloudStorageAccount.GetDevelopmentStorageAccount(new Uri(uriString));
				}
				else
				{
					accountInformation = CloudStorageAccount.DevelopmentStorageAccount;
				}
				accountInformation.Settings = CloudStorageAccount.ValidCredentials(dictionary);
				return true;
			}
			if (CloudStorageAccount.MatchesSpecification(dictionary, new Func<IDictionary<string, string>, IDictionary<string, string>>[]
			{
				CloudStorageAccount.AllRequired(new KeyValuePair<string, Func<string, bool>>[]
				{
					CloudStorageAccount.DefaultEndpointsProtocolSetting,
					CloudStorageAccount.AccountNameSetting,
					CloudStorageAccount.AccountKeySetting
				}),
				CloudStorageAccount.Optional(new KeyValuePair<string, Func<string, bool>>[]
				{
					CloudStorageAccount.BlobEndpointSetting,
					CloudStorageAccount.QueueEndpointSetting,
					CloudStorageAccount.TableEndpointSetting,
					CloudStorageAccount.FileEndpointSetting,
					CloudStorageAccount.AccountKeyNameSetting,
					CloudStorageAccount.EndpointSuffixSetting
				})
			}))
			{
				string text = null;
				dictionary.TryGetValue("BlobEndpoint", out text);
				string text2 = null;
				dictionary.TryGetValue("QueueEndpoint", out text2);
				string text3 = null;
				dictionary.TryGetValue("TableEndpoint", out text3);
				string text4 = null;
				dictionary.TryGetValue("FileEndpoint", out text4);
				accountInformation = new CloudStorageAccount(CloudStorageAccount.GetCredentials(dictionary), (text != null) ? new StorageUri(new Uri(text)) : CloudStorageAccount.ConstructBlobEndpoint(dictionary), (text2 != null) ? new StorageUri(new Uri(text2)) : CloudStorageAccount.ConstructQueueEndpoint(dictionary), (text3 != null) ? new StorageUri(new Uri(text3)) : CloudStorageAccount.ConstructTableEndpoint(dictionary), (text4 != null) ? new StorageUri(new Uri(text4)) : CloudStorageAccount.ConstructFileEndpoint(dictionary));
				string endpointSuffix = null;
				if (dictionary.TryGetValue("EndpointSuffix", out endpointSuffix))
				{
					accountInformation.EndpointSuffix = endpointSuffix;
				}
				accountInformation.Settings = CloudStorageAccount.ValidCredentials(dictionary);
				return true;
			}
			if (CloudStorageAccount.MatchesSpecification(dictionary, new Func<IDictionary<string, string>, IDictionary<string, string>>[]
			{
				CloudStorageAccount.AtLeastOne(new KeyValuePair<string, Func<string, bool>>[]
				{
					CloudStorageAccount.BlobEndpointSetting,
					CloudStorageAccount.QueueEndpointSetting,
					CloudStorageAccount.TableEndpointSetting,
					CloudStorageAccount.FileEndpointSetting
				}),
				new Func<IDictionary<string, string>, IDictionary<string, string>>(CloudStorageAccount.ValidCredentials)
			}))
			{
				Uri blobEndpoint = (!dictionary.ContainsKey("BlobEndpoint") || dictionary["BlobEndpoint"] == null) ? null : new Uri(dictionary["BlobEndpoint"]);
				Uri queueEndpoint = (!dictionary.ContainsKey("QueueEndpoint") || dictionary["QueueEndpoint"] == null) ? null : new Uri(dictionary["QueueEndpoint"]);
				Uri tableEndpoint = (!dictionary.ContainsKey("TableEndpoint") || dictionary["TableEndpoint"] == null) ? null : new Uri(dictionary["TableEndpoint"]);
				Uri fileEndpoint = (!dictionary.ContainsKey("FileEndpoint") || dictionary["FileEndpoint"] == null) ? null : new Uri(dictionary["FileEndpoint"]);
				accountInformation = new CloudStorageAccount(CloudStorageAccount.GetCredentials(dictionary), blobEndpoint, queueEndpoint, tableEndpoint, fileEndpoint);
				accountInformation.Settings = CloudStorageAccount.ValidCredentials(dictionary);
				return true;
			}
			accountInformation = null;
			error("No valid combination of account information found.");
			return false;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000376C8 File Offset: 0x000358C8
		private static IDictionary<string, string> ParseStringIntoSettings(string connectionString, Action<string> error)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = connectionString.Split(new char[]
			{
				';'
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					'='
				}, 2);
				IDictionary<string, string> result;
				if (array3.Length != 2)
				{
					error("Settings must be of the form \"name=value\".");
					result = null;
				}
				else
				{
					if (!dictionary.ContainsKey(array3[0]))
					{
						dictionary.Add(array3[0], array3[1]);
						i++;
						continue;
					}
					error(string.Format(CultureInfo.InvariantCulture, "Duplicate setting '{0}' found.", new object[]
					{
						array3[0]
					}));
					result = null;
				}
				return result;
			}
			return dictionary;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000377A8 File Offset: 0x000359A8
		private static KeyValuePair<string, Func<string, bool>> Setting(string name, params string[] validValues)
		{
			return new KeyValuePair<string, Func<string, bool>>(name, (string settingValue) => validValues.Length == 0 || validValues.Contains(settingValue));
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000377D4 File Offset: 0x000359D4
		private static KeyValuePair<string, Func<string, bool>> Setting(string name, Func<string, bool> isValid)
		{
			return new KeyValuePair<string, Func<string, bool>>(name, isValid);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000377E0 File Offset: 0x000359E0
		private static bool IsValidBase64String(string settingValue)
		{
			bool result;
			try
			{
				Convert.FromBase64String(settingValue);
				result = true;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00037810 File Offset: 0x00035A10
		private static bool IsValidUri(string settingValue)
		{
			return Uri.IsWellFormedUriString(settingValue, UriKind.Absolute);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00037819 File Offset: 0x00035A19
		private static bool IsValidDomain(string settingValue)
		{
			return Uri.CheckHostName(settingValue).Equals(UriHostNameType.Dns);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000378B4 File Offset: 0x00035AB4
		private static Func<IDictionary<string, string>, IDictionary<string, string>> AllRequired(params KeyValuePair<string, Func<string, bool>>[] requiredSettings)
		{
			return delegate(IDictionary<string, string> settings)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>(settings);
				foreach (KeyValuePair<string, Func<string, bool>> keyValuePair in requiredSettings)
				{
					string arg;
					if (!dictionary.TryGetValue(keyValuePair.Key, out arg) || !keyValuePair.Value(arg))
					{
						return null;
					}
					dictionary.Remove(keyValuePair.Key);
				}
				return dictionary;
			};
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00037950 File Offset: 0x00035B50
		private static Func<IDictionary<string, string>, IDictionary<string, string>> Optional(params KeyValuePair<string, Func<string, bool>>[] optionalSettings)
		{
			return delegate(IDictionary<string, string> settings)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>(settings);
				foreach (KeyValuePair<string, Func<string, bool>> keyValuePair in optionalSettings)
				{
					string arg;
					if (dictionary.TryGetValue(keyValuePair.Key, out arg) && keyValuePair.Value(arg))
					{
						dictionary.Remove(keyValuePair.Key);
					}
				}
				return dictionary;
			};
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000379F8 File Offset: 0x00035BF8
		private static Func<IDictionary<string, string>, IDictionary<string, string>> AtLeastOne(params KeyValuePair<string, Func<string, bool>>[] atLeastOneSettings)
		{
			return delegate(IDictionary<string, string> settings)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>(settings);
				bool flag = false;
				foreach (KeyValuePair<string, Func<string, bool>> keyValuePair in atLeastOneSettings)
				{
					string arg;
					if (dictionary.TryGetValue(keyValuePair.Key, out arg) && keyValuePair.Value(arg))
					{
						dictionary.Remove(keyValuePair.Key);
						flag = true;
					}
				}
				if (!flag)
				{
					return null;
				}
				return dictionary;
			};
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00037A20 File Offset: 0x00035C20
		private static IDictionary<string, string> ValidCredentials(IDictionary<string, string> settings)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>(settings);
			string text;
			if (settings.TryGetValue("AccountName", out text) && !CloudStorageAccount.AccountNameSetting.Value(text))
			{
				return null;
			}
			string text2;
			if (settings.TryGetValue("AccountKey", out text2) && !CloudStorageAccount.AccountKeySetting.Value(text2))
			{
				return null;
			}
			string text3;
			if (settings.TryGetValue("AccountKeyName", out text3) && !CloudStorageAccount.AccountKeyNameSetting.Value(text3))
			{
				return null;
			}
			string text4;
			if (settings.TryGetValue("SharedAccessSignature", out text4) && !CloudStorageAccount.SharedAccessSignatureSetting.Value(text4))
			{
				return null;
			}
			dictionary.Remove("AccountName");
			dictionary.Remove("AccountKey");
			dictionary.Remove("AccountKeyName");
			dictionary.Remove("SharedAccessSignature");
			if (text != null && text2 != null && text4 == null)
			{
				return dictionary;
			}
			if (text == null && text2 == null && text3 == null && text4 != null)
			{
				return dictionary;
			}
			if (text == null && text2 == null && text3 == null && text4 == null)
			{
				return dictionary;
			}
			return null;
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00037B30 File Offset: 0x00035D30
		private static bool MatchesSpecification(IDictionary<string, string> settings, params Func<IDictionary<string, string>, IDictionary<string, string>>[] constraints)
		{
			foreach (Func<IDictionary<string, string>, IDictionary<string, string>> func in constraints)
			{
				IDictionary<string, string> dictionary = func(settings);
				if (dictionary == null)
				{
					return false;
				}
				settings = dictionary;
			}
			return settings.Count == 0;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00037B78 File Offset: 0x00035D78
		private static StorageCredentials GetCredentials(IDictionary<string, string> settings)
		{
			string text;
			settings.TryGetValue("AccountName", out text);
			string text2;
			settings.TryGetValue("AccountKey", out text2);
			string text3;
			settings.TryGetValue("AccountKeyName", out text3);
			string text4;
			settings.TryGetValue("SharedAccessSignature", out text4);
			if (text != null && text2 != null && text4 == null)
			{
				return new StorageCredentials(text, text2, text3);
			}
			if (text == null && text2 == null && text3 == null && text4 != null)
			{
				return new StorageCredentials(text4);
			}
			return null;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00037BE3 File Offset: 0x00035DE3
		private static StorageUri ConstructBlobEndpoint(IDictionary<string, string> settings)
		{
			return CloudStorageAccount.ConstructBlobEndpoint(settings["DefaultEndpointsProtocol"], settings["AccountName"], settings.ContainsKey("EndpointSuffix") ? settings["EndpointSuffix"] : null);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00037C1C File Offset: 0x00035E1C
		private static StorageUri ConstructBlobEndpoint(string scheme, string accountName, string endpointSuffix)
		{
			if (string.IsNullOrEmpty(scheme))
			{
				throw new ArgumentNullException("scheme");
			}
			if (string.IsNullOrEmpty(accountName))
			{
				throw new ArgumentNullException("accountName");
			}
			if (string.IsNullOrEmpty(endpointSuffix))
			{
				endpointSuffix = "core.windows.net";
			}
			string uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}.{2}.{3}/", new object[]
			{
				scheme,
				accountName,
				"blob",
				endpointSuffix
			});
			string uriString2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}.{3}.{4}", new object[]
			{
				scheme,
				accountName,
				"-secondary",
				"blob",
				endpointSuffix
			});
			return new StorageUri(new Uri(uriString), new Uri(uriString2));
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00037CCF File Offset: 0x00035ECF
		private static StorageUri ConstructFileEndpoint(IDictionary<string, string> settings)
		{
			return CloudStorageAccount.ConstructFileEndpoint(settings["DefaultEndpointsProtocol"], settings["AccountName"], settings.ContainsKey("EndpointSuffix") ? settings["EndpointSuffix"] : null);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00037D08 File Offset: 0x00035F08
		private static StorageUri ConstructFileEndpoint(string scheme, string accountName, string endpointSuffix)
		{
			if (string.IsNullOrEmpty(scheme))
			{
				throw new ArgumentNullException("scheme");
			}
			if (string.IsNullOrEmpty(accountName))
			{
				throw new ArgumentNullException("accountName");
			}
			if (string.IsNullOrEmpty(endpointSuffix))
			{
				endpointSuffix = "core.windows.net";
			}
			string uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}.{2}.{3}/", new object[]
			{
				scheme,
				accountName,
				"file",
				endpointSuffix
			});
			string uriString2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}.{3}.{4}", new object[]
			{
				scheme,
				accountName,
				"-secondary",
				"file",
				endpointSuffix
			});
			return new StorageUri(new Uri(uriString), new Uri(uriString2));
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00037DBB File Offset: 0x00035FBB
		private static StorageUri ConstructQueueEndpoint(IDictionary<string, string> settings)
		{
			return CloudStorageAccount.ConstructQueueEndpoint(settings["DefaultEndpointsProtocol"], settings["AccountName"], settings.ContainsKey("EndpointSuffix") ? settings["EndpointSuffix"] : null);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00037DF4 File Offset: 0x00035FF4
		private static StorageUri ConstructQueueEndpoint(string scheme, string accountName, string endpointSuffix)
		{
			if (string.IsNullOrEmpty(scheme))
			{
				throw new ArgumentNullException("scheme");
			}
			if (string.IsNullOrEmpty(accountName))
			{
				throw new ArgumentNullException("accountName");
			}
			if (string.IsNullOrEmpty(endpointSuffix))
			{
				endpointSuffix = "core.windows.net";
			}
			string uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}.{2}.{3}/", new object[]
			{
				scheme,
				accountName,
				"queue",
				endpointSuffix
			});
			string uriString2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}.{3}.{4}", new object[]
			{
				scheme,
				accountName,
				"-secondary",
				"queue",
				endpointSuffix
			});
			return new StorageUri(new Uri(uriString), new Uri(uriString2));
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00037EA7 File Offset: 0x000360A7
		private static StorageUri ConstructTableEndpoint(IDictionary<string, string> settings)
		{
			return CloudStorageAccount.ConstructTableEndpoint(settings["DefaultEndpointsProtocol"], settings["AccountName"], settings.ContainsKey("EndpointSuffix") ? settings["EndpointSuffix"] : null);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00037EE0 File Offset: 0x000360E0
		private static StorageUri ConstructTableEndpoint(string scheme, string accountName, string endpointSuffix)
		{
			if (string.IsNullOrEmpty(scheme))
			{
				throw new ArgumentNullException("scheme");
			}
			if (string.IsNullOrEmpty(accountName))
			{
				throw new ArgumentNullException("accountName");
			}
			if (string.IsNullOrEmpty(endpointSuffix))
			{
				endpointSuffix = "core.windows.net";
			}
			string uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}.{2}.{3}/", new object[]
			{
				scheme,
				accountName,
				"table",
				endpointSuffix
			});
			string uriString2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}.{3}.{4}", new object[]
			{
				scheme,
				accountName,
				"-secondary",
				"table",
				endpointSuffix
			});
			return new StorageUri(new Uri(uriString), new Uri(uriString2));
		}

		// Token: 0x04000205 RID: 517
		internal const string UseDevelopmentStorageSettingString = "UseDevelopmentStorage";

		// Token: 0x04000206 RID: 518
		internal const string DevelopmentStorageProxyUriSettingString = "DevelopmentStorageProxyUri";

		// Token: 0x04000207 RID: 519
		internal const string DefaultEndpointsProtocolSettingString = "DefaultEndpointsProtocol";

		// Token: 0x04000208 RID: 520
		internal const string AccountNameSettingString = "AccountName";

		// Token: 0x04000209 RID: 521
		internal const string AccountKeyNameSettingString = "AccountKeyName";

		// Token: 0x0400020A RID: 522
		internal const string AccountKeySettingString = "AccountKey";

		// Token: 0x0400020B RID: 523
		internal const string BlobEndpointSettingString = "BlobEndpoint";

		// Token: 0x0400020C RID: 524
		internal const string QueueEndpointSettingString = "QueueEndpoint";

		// Token: 0x0400020D RID: 525
		internal const string TableEndpointSettingString = "TableEndpoint";

		// Token: 0x0400020E RID: 526
		internal const string FileEndpointSettingString = "FileEndpoint";

		// Token: 0x0400020F RID: 527
		internal const string EndpointSuffixSettingString = "EndpointSuffix";

		// Token: 0x04000210 RID: 528
		internal const string SharedAccessSignatureSettingString = "SharedAccessSignature";

		// Token: 0x04000211 RID: 529
		private const string DevstoreAccountName = "devstoreaccount1";

		// Token: 0x04000212 RID: 530
		private const string DevstoreAccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

		// Token: 0x04000213 RID: 531
		internal const string SecondaryLocationAccountSuffix = "-secondary";

		// Token: 0x04000214 RID: 532
		private const string DefaultEndpointSuffix = "core.windows.net";

		// Token: 0x04000215 RID: 533
		private const string DefaultBlobHostnamePrefix = "blob";

		// Token: 0x04000216 RID: 534
		private const string DefaultQueueHostnamePrefix = "queue";

		// Token: 0x04000217 RID: 535
		private const string DefaultTableHostnamePrefix = "table";

		// Token: 0x04000218 RID: 536
		private const string DefaultFileHostnamePrefix = "file";

		// Token: 0x04000219 RID: 537
		private static bool version1MD5 = true;

		// Token: 0x0400021A RID: 538
		private static readonly KeyValuePair<string, Func<string, bool>> UseDevelopmentStorageSetting = CloudStorageAccount.Setting("UseDevelopmentStorage", new string[]
		{
			"true"
		});

		// Token: 0x0400021B RID: 539
		private static readonly KeyValuePair<string, Func<string, bool>> DevelopmentStorageProxyUriSetting = CloudStorageAccount.Setting("DevelopmentStorageProxyUri", new Func<string, bool>(CloudStorageAccount.IsValidUri));

		// Token: 0x0400021C RID: 540
		private static readonly KeyValuePair<string, Func<string, bool>> DefaultEndpointsProtocolSetting = CloudStorageAccount.Setting("DefaultEndpointsProtocol", new string[]
		{
			"http",
			"https"
		});

		// Token: 0x0400021D RID: 541
		private static readonly KeyValuePair<string, Func<string, bool>> AccountNameSetting = CloudStorageAccount.Setting("AccountName", new string[0]);

		// Token: 0x0400021E RID: 542
		private static readonly KeyValuePair<string, Func<string, bool>> AccountKeyNameSetting = CloudStorageAccount.Setting("AccountKeyName", new string[0]);

		// Token: 0x0400021F RID: 543
		private static readonly KeyValuePair<string, Func<string, bool>> AccountKeySetting = CloudStorageAccount.Setting("AccountKey", new Func<string, bool>(CloudStorageAccount.IsValidBase64String));

		// Token: 0x04000220 RID: 544
		private static readonly KeyValuePair<string, Func<string, bool>> BlobEndpointSetting = CloudStorageAccount.Setting("BlobEndpoint", new Func<string, bool>(CloudStorageAccount.IsValidUri));

		// Token: 0x04000221 RID: 545
		private static readonly KeyValuePair<string, Func<string, bool>> QueueEndpointSetting = CloudStorageAccount.Setting("QueueEndpoint", new Func<string, bool>(CloudStorageAccount.IsValidUri));

		// Token: 0x04000222 RID: 546
		private static readonly KeyValuePair<string, Func<string, bool>> TableEndpointSetting = CloudStorageAccount.Setting("TableEndpoint", new Func<string, bool>(CloudStorageAccount.IsValidUri));

		// Token: 0x04000223 RID: 547
		private static readonly KeyValuePair<string, Func<string, bool>> FileEndpointSetting = CloudStorageAccount.Setting("FileEndpoint", new Func<string, bool>(CloudStorageAccount.IsValidUri));

		// Token: 0x04000224 RID: 548
		private static readonly KeyValuePair<string, Func<string, bool>> EndpointSuffixSetting = CloudStorageAccount.Setting("EndpointSuffix", new Func<string, bool>(CloudStorageAccount.IsValidDomain));

		// Token: 0x04000225 RID: 549
		private static readonly KeyValuePair<string, Func<string, bool>> SharedAccessSignatureSetting = CloudStorageAccount.Setting("SharedAccessSignature", new string[0]);

		// Token: 0x04000226 RID: 550
		private static CloudStorageAccount devStoreAccount;
	}
}
