using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.WindowsAzure.Storage.Analytics
{
	// Token: 0x02000006 RID: 6
	public sealed class CloudAnalyticsClient
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003A4D File Offset: 0x00001C4D
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003A55 File Offset: 0x00001C55
		internal string LogContainer { get; set; }

		// Token: 0x0600005A RID: 90 RVA: 0x00003A60 File Offset: 0x00001C60
		public CloudAnalyticsClient(StorageUri blobStorageUri, StorageUri tableStorageUri, StorageCredentials credentials)
		{
			CommonUtility.AssertNotNull("blobStorageUri", blobStorageUri);
			CommonUtility.AssertNotNull("tableStorageUri", tableStorageUri);
			this.blobClient = new CloudBlobClient(blobStorageUri, credentials);
			this.tableClient = new CloudTableClient(tableStorageUri, credentials);
			this.LogContainer = "$logs";
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003AAE File Offset: 0x00001CAE
		public CloudBlobDirectory GetLogDirectory(StorageService service)
		{
			return this.blobClient.GetContainerReference(this.LogContainer).GetDirectoryReference(service.ToString().ToLowerInvariant());
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003AD6 File Offset: 0x00001CD6
		public CloudTable GetHourMetricsTable(StorageService service)
		{
			return this.GetHourMetricsTable(service, StorageLocation.Primary);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003AE0 File Offset: 0x00001CE0
		public CloudTable GetHourMetricsTable(StorageService service, StorageLocation location)
		{
			switch (service)
			{
			case StorageService.Blob:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsHourPrimaryTransactionsBlob");
				}
				return this.tableClient.GetTableReference("$MetricsHourSecondaryTransactionsBlob");
			case StorageService.Queue:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsHourPrimaryTransactionsQueue");
				}
				return this.tableClient.GetTableReference("$MetricsHourSecondaryTransactionsQueue");
			case StorageService.Table:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsHourPrimaryTransactionsTable");
				}
				return this.tableClient.GetTableReference("$MetricsHourSecondaryTransactionsTable");
			default:
				throw new ArgumentException("Invalid storage service specified.");
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003B7C File Offset: 0x00001D7C
		public CloudTable GetMinuteMetricsTable(StorageService service)
		{
			return this.GetMinuteMetricsTable(service, StorageLocation.Primary);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003B88 File Offset: 0x00001D88
		public CloudTable GetMinuteMetricsTable(StorageService service, StorageLocation location)
		{
			switch (service)
			{
			case StorageService.Blob:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsMinutePrimaryTransactionsBlob");
				}
				return this.tableClient.GetTableReference("$MetricsMinuteSecondaryTransactionsBlob");
			case StorageService.Queue:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsMinutePrimaryTransactionsQueue");
				}
				return this.tableClient.GetTableReference("$MetricsMinuteSecondaryTransactionsQueue");
			case StorageService.Table:
				if (location == StorageLocation.Primary)
				{
					return this.tableClient.GetTableReference("$MetricsMinutePrimaryTransactionsTable");
				}
				return this.tableClient.GetTableReference("$MetricsMinuteSecondaryTransactionsTable");
			default:
				throw new ArgumentException("Invalid storage service specified.");
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003C24 File Offset: 0x00001E24
		public CloudTable GetCapacityTable()
		{
			return this.tableClient.GetTableReference("$MetricsCapacityBlob");
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003C36 File Offset: 0x00001E36
		public IEnumerable<ICloudBlob> ListLogs(StorageService service)
		{
			return this.ListLogs(service, LoggingOperations.All, BlobListingDetails.None, null, null);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003C64 File Offset: 0x00001E64
		public IEnumerable<ICloudBlob> ListLogs(StorageService service, LoggingOperations operations, BlobListingDetails details, BlobRequestOptions options, OperationContext operationContext)
		{
			BlobListingDetails blobListingDetails = BlobListingDetails.None;
			if (details.HasFlag(BlobListingDetails.Copy) || details.HasFlag(BlobListingDetails.Snapshots) || details.HasFlag(BlobListingDetails.UncommittedBlobs))
			{
				throw new ArgumentException("Invalid blob listing details specified.");
			}
			if (operations == LoggingOperations.None)
			{
				throw new ArgumentException("Invalid logging operations specified.");
			}
			if (details.HasFlag(BlobListingDetails.Metadata) || !operations.HasFlag(LoggingOperations.All))
			{
				blobListingDetails = BlobListingDetails.Metadata;
			}
			IEnumerable<IListBlobItem> source = this.GetLogDirectory(service).ListBlobs(true, blobListingDetails, options, operationContext);
			return from log in source
			select (ICloudBlob)log into log
			where CloudAnalyticsClient.IsCorrectLogType(log, operations)
			select log;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003D4B File Offset: 0x00001F4B
		public IEnumerable<ICloudBlob> ListLogs(StorageService service, DateTimeOffset startTime, DateTimeOffset? endTime)
		{
			return this.ListLogs(service, startTime, endTime, LoggingOperations.All, BlobListingDetails.None, null, null);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004738 File Offset: 0x00002938
		public IEnumerable<ICloudBlob> ListLogs(StorageService service, DateTimeOffset startTime, DateTimeOffset? endTime, LoggingOperations operations, BlobListingDetails details, BlobRequestOptions options, OperationContext operationContext)
		{
			CloudBlobDirectory logDirectory = this.GetLogDirectory(service);
			BlobListingDetails metadataDetails = details;
			DateTimeOffset utcStartTime = startTime.ToUniversalTime();
			DateTimeOffset dateCounter = new DateTimeOffset(utcStartTime.Ticks - utcStartTime.Ticks % 36000000000L, utcStartTime.Offset);
			DateTimeOffset? utcEndTime = null;
			string endPrefix = null;
			if (endTime != null)
			{
				utcEndTime = new DateTimeOffset?(endTime.Value.ToUniversalTime());
				endPrefix = logDirectory.Prefix + utcEndTime.Value.ToString("yyyy/MM/dd/HH", CultureInfo.InvariantCulture);
				if (utcStartTime > utcEndTime.Value)
				{
					string message = string.Format(CultureInfo.InvariantCulture, "StartTime invalid. The start time '{0}' occurs after the end time '{1}'.", new object[]
					{
						startTime,
						endTime.Value
					});
					throw new ArgumentException(message);
				}
			}
			if (details.HasFlag(BlobListingDetails.Copy) || details.HasFlag(BlobListingDetails.Snapshots) || details.HasFlag(BlobListingDetails.UncommittedBlobs))
			{
				throw new ArgumentException("Invalid blob listing details specified.");
			}
			if (operations == LoggingOperations.None)
			{
				throw new ArgumentException("Invalid logging operations specified.");
			}
			if (details.HasFlag(BlobListingDetails.Metadata) || !operations.HasFlag(LoggingOperations.All))
			{
				metadataDetails = BlobListingDetails.Metadata;
			}
			while (dateCounter.Hour > 0)
			{
				string currentPrefix = logDirectory.Prefix + dateCounter.ToString("yyyy/MM/dd/HH", CultureInfo.InvariantCulture);
				IEnumerable<IListBlobItem> currentLogs = logDirectory.Container.ListBlobs(currentPrefix, true, metadataDetails, options, operationContext);
				foreach (IListBlobItem listBlobItem in currentLogs)
				{
					ICloudBlob log = (ICloudBlob)listBlobItem;
					if (utcEndTime != null && string.Compare(log.Parent.Prefix, endPrefix) > 0)
					{
						yield break;
					}
					if (CloudAnalyticsClient.IsCorrectLogType(log, operations))
					{
						yield return log;
					}
				}
				dateCounter = dateCounter.AddHours(1.0);
				if (dateCounter > DateTimeOffset.UtcNow.AddHours(1.0))
				{
					IL_78C:
					yield break;
				}
			}
			while (dateCounter.Day > 1)
			{
				string currentPrefix2 = logDirectory.Prefix + dateCounter.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
				IEnumerable<IListBlobItem> currentLogs2 = logDirectory.Container.ListBlobs(currentPrefix2, true, metadataDetails, options, operationContext);
				foreach (IListBlobItem listBlobItem2 in currentLogs2)
				{
					ICloudBlob log2 = (ICloudBlob)listBlobItem2;
					if (utcEndTime != null && string.Compare(log2.Parent.Prefix, endPrefix) > 0)
					{
						yield break;
					}
					if (CloudAnalyticsClient.IsCorrectLogType(log2, operations))
					{
						yield return log2;
					}
				}
				dateCounter = dateCounter.AddDays(1.0);
				if (dateCounter > DateTimeOffset.UtcNow.AddHours(1.0))
				{
					goto IL_78C;
				}
			}
			while (dateCounter.Month > 1)
			{
				string currentPrefix3 = logDirectory.Prefix + dateCounter.ToString("yyyy/MM", CultureInfo.InvariantCulture);
				IEnumerable<IListBlobItem> currentLogs3 = logDirectory.Container.ListBlobs(currentPrefix3, true, metadataDetails, options, operationContext);
				foreach (IListBlobItem listBlobItem3 in currentLogs3)
				{
					ICloudBlob log3 = (ICloudBlob)listBlobItem3;
					if (utcEndTime != null && string.Compare(log3.Parent.Prefix, endPrefix) > 0)
					{
						yield break;
					}
					if (CloudAnalyticsClient.IsCorrectLogType(log3, operations))
					{
						yield return log3;
					}
				}
				dateCounter = dateCounter.AddMonths(1);
				if (dateCounter > DateTimeOffset.UtcNow.AddHours(1.0))
				{
					goto IL_78C;
				}
			}
			for (;;)
			{
				string currentPrefix4 = logDirectory.Prefix + dateCounter.ToString("yyyy", CultureInfo.InvariantCulture);
				IEnumerable<IListBlobItem> currentLogs4 = logDirectory.Container.ListBlobs(currentPrefix4, true, metadataDetails, options, operationContext);
				foreach (IListBlobItem listBlobItem4 in currentLogs4)
				{
					ICloudBlob log4 = (ICloudBlob)listBlobItem4;
					if (utcEndTime != null && string.Compare(log4.Parent.Prefix, endPrefix) > 0)
					{
						yield break;
					}
					if (CloudAnalyticsClient.IsCorrectLogType(log4, operations))
					{
						yield return log4;
					}
				}
				dateCounter = dateCounter.AddYears(1);
				if (dateCounter > DateTimeOffset.UtcNow.AddHours(1.0))
				{
					goto IL_78C;
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000478A File Offset: 0x0000298A
		public IEnumerable<LogRecord> ListLogRecords(StorageService service)
		{
			return this.ListLogRecords(service, null, null);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004795 File Offset: 0x00002995
		public IEnumerable<LogRecord> ListLogRecords(StorageService service, BlobRequestOptions options, OperationContext operationContext)
		{
			return CloudAnalyticsClient.ParseLogBlobs(this.ListLogs(service, LoggingOperations.All, BlobListingDetails.None, options, operationContext));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000047A7 File Offset: 0x000029A7
		public IEnumerable<LogRecord> ListLogRecords(StorageService service, DateTimeOffset startTime, DateTimeOffset? endTime)
		{
			return this.ListLogRecords(service, startTime, endTime, null, null);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000047B4 File Offset: 0x000029B4
		public IEnumerable<LogRecord> ListLogRecords(StorageService service, DateTimeOffset startTime, DateTimeOffset? endTime, BlobRequestOptions options, OperationContext operationContext)
		{
			return CloudAnalyticsClient.ParseLogBlobs(this.ListLogs(service, startTime, endTime, LoggingOperations.All, BlobListingDetails.None, options, operationContext));
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000047CA File Offset: 0x000029CA
		public static IEnumerable<LogRecord> ParseLogBlobs(IEnumerable<ICloudBlob> logBlobs)
		{
			return logBlobs.SelectMany(new Func<ICloudBlob, IEnumerable<LogRecord>>(CloudAnalyticsClient.ParseLogBlob));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000049E4 File Offset: 0x00002BE4
		public static IEnumerable<LogRecord> ParseLogBlob(ICloudBlob logBlob)
		{
			using (Stream stream = ((CloudBlockBlob)logBlob).OpenRead(null, null, null))
			{
				using (LogRecordStreamReader reader = new LogRecordStreamReader(stream, (int)stream.Length))
				{
					while (!reader.IsEndOfFile)
					{
						LogRecord log = new LogRecord(reader);
						yield return log;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004B14 File Offset: 0x00002D14
		public static IEnumerable<LogRecord> ParseLogStream(Stream stream)
		{
			LogRecordStreamReader reader = new LogRecordStreamReader(stream, (int)stream.Length);
			while (!reader.IsEndOfFile)
			{
				LogRecord log = new LogRecord(reader);
				yield return log;
			}
			yield break;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004B34 File Offset: 0x00002D34
		public TableQuery<CapacityEntity> CreateCapacityQuery()
		{
			CloudTable capacityTable = this.GetCapacityTable();
			return capacityTable.CreateQuery<CapacityEntity>();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B50 File Offset: 0x00002D50
		public TableQuery<MetricsEntity> CreateHourMetricsQuery(StorageService service, StorageLocation location)
		{
			CloudTable hourMetricsTable = this.GetHourMetricsTable(service, location);
			return hourMetricsTable.CreateQuery<MetricsEntity>();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004B6C File Offset: 0x00002D6C
		public TableQuery<MetricsEntity> CreateMinuteMetricsQuery(StorageService service, StorageLocation location)
		{
			CloudTable minuteMetricsTable = this.GetMinuteMetricsTable(service, location);
			return minuteMetricsTable.CreateQuery<MetricsEntity>();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004B88 File Offset: 0x00002D88
		internal static bool IsCorrectLogType(ICloudBlob logBlob, LoggingOperations operations)
		{
			IDictionary<string, string> metadata = logBlob.Metadata;
			string text;
			return !metadata.TryGetValue("LogType", out text) || (operations.HasFlag(LoggingOperations.Read) && text.Contains("read")) || (operations.HasFlag(LoggingOperations.Write) && text.Contains("write")) || (operations.HasFlag(LoggingOperations.Delete) && text.Contains("delete"));
		}

		// Token: 0x04000022 RID: 34
		private CloudBlobClient blobClient;

		// Token: 0x04000023 RID: 35
		private CloudTableClient tableClient;
	}
}
