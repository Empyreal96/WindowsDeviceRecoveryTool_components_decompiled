using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000079 RID: 121
	public static class NameValidator
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x00038175 File Offset: 0x00036375
		public static void ValidateContainerName(string containerName)
		{
			if (!"$root".Equals(containerName, StringComparison.Ordinal) && !"$logs".Equals(containerName, StringComparison.Ordinal))
			{
				NameValidator.ValidateShareContainerQueueHelper(containerName, "container");
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0003819E File Offset: 0x0003639E
		public static void ValidateQueueName(string queueName)
		{
			NameValidator.ValidateShareContainerQueueHelper(queueName, "queue");
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000381AB File Offset: 0x000363AB
		public static void ValidateShareName(string shareName)
		{
			NameValidator.ValidateShareContainerQueueHelper(shareName, "share");
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000381B8 File Offset: 0x000363B8
		private static void ValidateShareContainerQueueHelper(string resourceName, string resourceType)
		{
			if (string.IsNullOrWhiteSpace(resourceName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. The {0} name may not be null, empty, or whitespace only.", new object[]
				{
					resourceType
				}));
			}
			if (resourceName.Length < 3 || resourceName.Length > 63)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name length. The {0} name must be between {1} and {2} characters long.", new object[]
				{
					resourceType,
					3,
					63
				}));
			}
			if (!NameValidator.ShareContainerQueueRegex.IsMatch(resourceName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. Check MSDN for more information about valid {0} naming.", new object[]
				{
					resourceType
				}));
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00038264 File Offset: 0x00036464
		public static void ValidateBlobName(string blobName)
		{
			if (string.IsNullOrWhiteSpace(blobName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. The {0} name may not be null, empty, or whitespace only.", new object[]
				{
					"blob"
				}));
			}
			if (blobName.Length < 1 || blobName.Length > 1024)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name length. The {0} name must be between {1} and {2} characters long.", new object[]
				{
					"blob",
					1,
					1024
				}));
			}
			int num = 0;
			foreach (char c in blobName)
			{
				if (c == '/')
				{
					num++;
				}
			}
			if (num >= 254)
			{
				throw new ArgumentException("The count of URL path segments (strings between '/' characters) as part of the blob name cannot exceed 254.");
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00038330 File Offset: 0x00036530
		public static void ValidateFileName(string fileName)
		{
			NameValidator.ValidateFileDirectoryHelper(fileName, "file");
			if (fileName.EndsWith("/", StringComparison.Ordinal))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. Check MSDN for more information about valid {0} naming.", new object[]
				{
					"file"
				}));
			}
			foreach (string text in NameValidator.ReservedFileNames)
			{
				if (text.Equals(fileName, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. This {0} name is reserved.", new object[]
					{
						"file"
					}));
				}
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000383C5 File Offset: 0x000365C5
		public static void ValidateDirectoryName(string directoryName)
		{
			NameValidator.ValidateFileDirectoryHelper(directoryName, "directory");
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000383D4 File Offset: 0x000365D4
		private static void ValidateFileDirectoryHelper(string resourceName, string resourceType)
		{
			if (string.IsNullOrWhiteSpace(resourceName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. The {0} name may not be null, empty, or whitespace only.", new object[]
				{
					resourceType
				}));
			}
			if (resourceName.Length < 1 || resourceName.Length > 255)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name length. The {0} name must be between {1} and {2} characters long.", new object[]
				{
					resourceType,
					1,
					255
				}));
			}
			if (!NameValidator.FileDirectoryRegex.IsMatch(resourceName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. Check MSDN for more information about valid {0} naming.", new object[]
				{
					resourceType
				}));
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00038488 File Offset: 0x00036688
		public static void ValidateTableName(string tableName)
		{
			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. The {0} name may not be null, empty, or whitespace only.", new object[]
				{
					"table"
				}));
			}
			if (tableName.Length < 3 || tableName.Length > 63)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name length. The {0} name must be between {1} and {2} characters long.", new object[]
				{
					"table",
					3,
					63
				}));
			}
			if (!NameValidator.TableRegex.IsMatch(tableName) && !NameValidator.MetricsTableRegex.IsMatch(tableName) && !tableName.Equals("$MetricsCapacityBlob", StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0} name. Check MSDN for more information about valid {0} naming.", new object[]
				{
					"table"
				}));
			}
		}

		// Token: 0x04000241 RID: 577
		private const int BlobFileDirectoryMinLength = 1;

		// Token: 0x04000242 RID: 578
		private const int ContainerShareQueueTableMinLength = 3;

		// Token: 0x04000243 RID: 579
		private const int ContainerShareQueueTableMaxLength = 63;

		// Token: 0x04000244 RID: 580
		private const int FileDirectoryMaxLength = 255;

		// Token: 0x04000245 RID: 581
		private const int BlobMaxLength = 1024;

		// Token: 0x04000246 RID: 582
		private static readonly string[] ReservedFileNames = new string[]
		{
			".",
			"..",
			"LPT1",
			"LPT2",
			"LPT3",
			"LPT4",
			"LPT5",
			"LPT6",
			"LPT7",
			"LPT8",
			"LPT9",
			"COM1",
			"COM2",
			"COM3",
			"COM4",
			"COM5",
			"COM6",
			"COM7",
			"COM8",
			"COM9",
			"PRN",
			"AUX",
			"NUL",
			"CON",
			"CLOCK$"
		};

		// Token: 0x04000247 RID: 583
		private static readonly RegexOptions RegexOptions = RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.CultureInvariant;

		// Token: 0x04000248 RID: 584
		private static readonly Regex FileDirectoryRegex = new Regex("^[^\"\\\\/:|<>*?]*\\/{0,1}$", NameValidator.RegexOptions);

		// Token: 0x04000249 RID: 585
		private static readonly Regex ShareContainerQueueRegex = new Regex("^[a-z0-9]+(-[a-z0-9]+)*$", NameValidator.RegexOptions);

		// Token: 0x0400024A RID: 586
		private static readonly Regex TableRegex = new Regex("^[A-Za-z][A-Za-z0-9]*$", NameValidator.RegexOptions);

		// Token: 0x0400024B RID: 587
		private static readonly Regex MetricsTableRegex = new Regex("^\\$Metrics(HourPrimary|MinutePrimary|HourSecondary|MinuteSecondary)?(Transactions)(Blob|Queue|Table)$", NameValidator.RegexOptions);
	}
}
