using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000081 RID: 129
	public sealed class StorageUri : IEquatable<StorageUri>
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x00039944 File Offset: 0x00037B44
		// (set) Token: 0x06000F07 RID: 3847 RVA: 0x0003994C File Offset: 0x00037B4C
		public Uri PrimaryUri
		{
			get
			{
				return this.primaryUri;
			}
			private set
			{
				StorageUri.AssertAbsoluteUri(value);
				this.primaryUri = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0003995B File Offset: 0x00037B5B
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x00039963 File Offset: 0x00037B63
		public Uri SecondaryUri
		{
			get
			{
				return this.secondaryUri;
			}
			private set
			{
				StorageUri.AssertAbsoluteUri(value);
				this.secondaryUri = value;
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00039972 File Offset: 0x00037B72
		public StorageUri(Uri primaryUri) : this(primaryUri, null)
		{
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003997C File Offset: 0x00037B7C
		public StorageUri(Uri primaryUri, Uri secondaryUri)
		{
			if (primaryUri != null && secondaryUri != null)
			{
				bool flag = CommonUtility.UsePathStyleAddressing(primaryUri);
				bool flag2 = CommonUtility.UsePathStyleAddressing(secondaryUri);
				if (!flag && !flag2)
				{
					if (primaryUri.PathAndQuery != secondaryUri.PathAndQuery)
					{
						throw new ArgumentException("Primary and secondary location URIs in a StorageUri must point to the same resource.", "secondaryUri");
					}
				}
				else
				{
					IEnumerable<string> first = primaryUri.Segments.Skip(flag ? 2 : 0);
					IEnumerable<string> second = secondaryUri.Segments.Skip(flag2 ? 2 : 0);
					if (!first.SequenceEqual(second) || primaryUri.Query != secondaryUri.Query)
					{
						throw new ArgumentException("Primary and secondary location URIs in a StorageUri must point to the same resource.", "secondaryUri");
					}
				}
			}
			this.PrimaryUri = primaryUri;
			this.SecondaryUri = secondaryUri;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00039A40 File Offset: 0x00037C40
		public Uri GetUri(StorageLocation location)
		{
			switch (location)
			{
			case StorageLocation.Primary:
				return this.PrimaryUri;
			case StorageLocation.Secondary:
				return this.SecondaryUri;
			default:
				CommonUtility.ArgumentOutOfRange("location", location);
				return null;
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00039A80 File Offset: 0x00037C80
		internal bool ValidateLocationMode(LocationMode mode)
		{
			switch (mode)
			{
			case LocationMode.PrimaryOnly:
				return this.PrimaryUri != null;
			case LocationMode.SecondaryOnly:
				return this.SecondaryUri != null;
			}
			return this.PrimaryUri != null && this.SecondaryUri != null;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00039ADC File Offset: 0x00037CDC
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "Primary = '{0}'; Secondary = '{1}'", new object[]
			{
				this.PrimaryUri,
				this.SecondaryUri
			});
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00039B14 File Offset: 0x00037D14
		public override int GetHashCode()
		{
			int num = (this.PrimaryUri != null) ? this.PrimaryUri.GetHashCode() : 0;
			int num2 = (this.SecondaryUri != null) ? this.SecondaryUri.GetHashCode() : 0;
			return num ^ num2;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00039B5E File Offset: 0x00037D5E
		public override bool Equals(object obj)
		{
			return this.Equals(obj as StorageUri);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00039B6C File Offset: 0x00037D6C
		public bool Equals(StorageUri other)
		{
			return other != null && this.PrimaryUri == other.PrimaryUri && this.SecondaryUri == other.SecondaryUri;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00039B9D File Offset: 0x00037D9D
		public static bool operator ==(StorageUri uri1, StorageUri uri2)
		{
			return object.ReferenceEquals(uri1, uri2) || (!object.ReferenceEquals(uri1, null) && uri1.Equals(uri2));
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00039BBC File Offset: 0x00037DBC
		public static bool operator !=(StorageUri uri1, StorageUri uri2)
		{
			return !(uri1 == uri2);
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00039BC8 File Offset: 0x00037DC8
		private static void AssertAbsoluteUri(Uri uri)
		{
			if (uri != null && !uri.IsAbsoluteUri)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Address '{0}' is a relative address. Only absolute addresses are permitted.", new object[]
				{
					uri.ToString()
				});
				throw new ArgumentException(message, "uri");
			}
		}

		// Token: 0x04000275 RID: 629
		private Uri primaryUri;

		// Token: 0x04000276 RID: 630
		private Uri secondaryUri;
	}
}
