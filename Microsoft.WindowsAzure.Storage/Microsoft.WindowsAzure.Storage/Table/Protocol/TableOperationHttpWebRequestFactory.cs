using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Data.OData;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004D RID: 77
	internal static class TableOperationHttpWebRequestFactory
	{
		// Token: 0x06000CCD RID: 3277 RVA: 0x0002D710 File Offset: 0x0002B910
		internal static HttpWebRequest BuildRequestCore(Uri uri, UriQueryBuilder builder, string method, int? timeout, bool useVersionHeader, OperationContext ctx)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest(method, uri, timeout, builder, useVersionHeader, ctx);
			httpWebRequest.Headers.Add("Accept-Charset", "UTF-8");
			httpWebRequest.Headers.Add("MaxDataServiceVersion", "3.0;NetFx");
			return httpWebRequest;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0002D758 File Offset: 0x0002B958
		internal static HttpWebRequest BuildRequestForTableQuery(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx, TablePayloadFormat payloadFormat)
		{
			HttpWebRequest httpWebRequest = TableOperationHttpWebRequestFactory.BuildRequestCore(uri, builder, "GET", timeout, useVersionHeader, ctx);
			TableOperationHttpWebRequestFactory.SetAcceptHeaderForHttpWebRequest(httpWebRequest, payloadFormat);
			Logger.LogInformational(ctx, "Setting payload format for the request to '{0}'.", new object[]
			{
				payloadFormat
			});
			return httpWebRequest;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0002D7A0 File Offset: 0x0002B9A0
		internal static Tuple<HttpWebRequest, Stream> BuildRequestForTableOperation(Uri uri, UriQueryBuilder builder, IBufferManager bufferManager, int? timeout, TableOperation operation, bool useVersionHeader, OperationContext ctx, TableRequestOptions options, string accountName)
		{
			HttpWebRequest httpWebRequest = TableOperationHttpWebRequestFactory.BuildRequestCore(uri, builder, operation.HttpMethod, timeout, useVersionHeader, ctx);
			TablePayloadFormat value = options.PayloadFormat.Value;
			TableOperationHttpWebRequestFactory.SetAcceptHeaderForHttpWebRequest(httpWebRequest, value);
			Logger.LogInformational(ctx, "Setting payload format for the request to '{0}'.", new object[]
			{
				value
			});
			if (operation.HttpMethod != "HEAD" && operation.HttpMethod != "GET")
			{
				TableOperationHttpWebRequestFactory.SetContentTypeForHttpWebRequest(httpWebRequest, value);
			}
			if (operation.OperationType == TableOperationType.InsertOrMerge || operation.OperationType == TableOperationType.Merge)
			{
				options.AssertNoEncryptionPolicyOrStrictMode();
				httpWebRequest.Headers.Add("X-HTTP-Method", "MERGE");
			}
			if ((operation.OperationType == TableOperationType.Delete || operation.OperationType == TableOperationType.Replace || operation.OperationType == TableOperationType.Merge) && operation.Entity.ETag != null)
			{
				httpWebRequest.Headers.Add("If-Match", operation.Entity.ETag);
			}
			if (operation.OperationType == TableOperationType.Insert)
			{
				httpWebRequest.Headers.Add("Prefer", operation.EchoContent ? "return-content" : "return-no-content");
			}
			if (operation.OperationType == TableOperationType.Insert || operation.OperationType == TableOperationType.Merge || operation.OperationType == TableOperationType.InsertOrMerge || operation.OperationType == TableOperationType.InsertOrReplace || operation.OperationType == TableOperationType.Replace)
			{
				ODataMessageWriterSettings settings = new ODataMessageWriterSettings
				{
					CheckCharacters = false,
					Version = new ODataVersion?(TableConstants.ODataProtocolVersion)
				};
				HttpWebRequestAdapterMessage httpWebRequestAdapterMessage = new HttpWebRequestAdapterMessage(httpWebRequest, bufferManager);
				if (operation.HttpMethod != "HEAD" && operation.HttpMethod != "GET")
				{
					TableOperationHttpWebRequestFactory.SetContentTypeForAdapterMessage(httpWebRequestAdapterMessage, value);
				}
				ODataMessageWriter odataMessageWriter = new ODataMessageWriter(httpWebRequestAdapterMessage, settings, new TableStorageModel(accountName));
				ODataWriter writer = odataMessageWriter.CreateODataEntryWriter();
				TableOperationHttpWebRequestFactory.WriteOdataEntity(operation.Entity, operation.OperationType, ctx, writer, options);
				return new Tuple<HttpWebRequest, Stream>(httpWebRequestAdapterMessage.GetPopulatedMessage(), httpWebRequestAdapterMessage.GetStream());
			}
			return new Tuple<HttpWebRequest, Stream>(httpWebRequest, null);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
		internal static Tuple<HttpWebRequest, Stream> BuildRequestForTableBatchOperation(Uri uri, UriQueryBuilder builder, IBufferManager bufferManager, int? timeout, string tableName, TableBatchOperation batch, bool useVersionHeader, OperationContext ctx, TableRequestOptions options, string accountName)
		{
			HttpWebRequest msg = TableOperationHttpWebRequestFactory.BuildRequestCore(NavigationHelper.AppendPathToSingleUri(uri, "$batch"), builder, "POST", timeout, useVersionHeader, ctx);
			TablePayloadFormat value = options.PayloadFormat.Value;
			Logger.LogInformational(ctx, "Setting payload format for the request to '{0}'.", new object[]
			{
				value
			});
			ODataMessageWriterSettings settings = new ODataMessageWriterSettings
			{
				CheckCharacters = false,
				Version = new ODataVersion?(TableConstants.ODataProtocolVersion)
			};
			HttpWebRequestAdapterMessage httpWebRequestAdapterMessage = new HttpWebRequestAdapterMessage(msg, bufferManager);
			ODataMessageWriter odataMessageWriter = new ODataMessageWriter(httpWebRequestAdapterMessage, settings);
			ODataBatchWriter odataBatchWriter = odataMessageWriter.CreateODataBatchWriter();
			odataBatchWriter.WriteStartBatch();
			bool flag = batch.Count == 1 && batch[0].OperationType == TableOperationType.Retrieve;
			if (!flag)
			{
				odataBatchWriter.WriteStartChangeset();
				odataBatchWriter.Flush();
			}
			foreach (TableOperation tableOperation in batch)
			{
				string method = tableOperation.HttpMethod;
				if (tableOperation.OperationType == TableOperationType.Merge || tableOperation.OperationType == TableOperationType.InsertOrMerge)
				{
					options.AssertNoEncryptionPolicyOrStrictMode();
					method = "MERGE";
				}
				ODataBatchOperationRequestMessage odataBatchOperationRequestMessage = odataBatchWriter.CreateOperationRequestMessage(method, tableOperation.GenerateRequestURI(uri, tableName));
				TableOperationHttpWebRequestFactory.SetAcceptAndContentTypeForODataBatchMessage(odataBatchOperationRequestMessage, value);
				if (tableOperation.OperationType == TableOperationType.Delete || tableOperation.OperationType == TableOperationType.Replace || tableOperation.OperationType == TableOperationType.Merge)
				{
					odataBatchOperationRequestMessage.SetHeader("If-Match", tableOperation.Entity.ETag);
				}
				if (tableOperation.OperationType == TableOperationType.Insert)
				{
					odataBatchOperationRequestMessage.SetHeader("Prefer", tableOperation.EchoContent ? "return-content" : "return-no-content");
				}
				if (tableOperation.OperationType != TableOperationType.Delete && tableOperation.OperationType != TableOperationType.Retrieve)
				{
					using (ODataMessageWriter odataMessageWriter2 = new ODataMessageWriter(odataBatchOperationRequestMessage, settings, new TableStorageModel(accountName)))
					{
						ODataWriter writer = odataMessageWriter2.CreateODataEntryWriter();
						TableOperationHttpWebRequestFactory.WriteOdataEntity(tableOperation.Entity, tableOperation.OperationType, ctx, writer, options);
					}
				}
			}
			if (!flag)
			{
				odataBatchWriter.WriteEndChangeset();
			}
			odataBatchWriter.WriteEndBatch();
			odataBatchWriter.Flush();
			return new Tuple<HttpWebRequest, Stream>(httpWebRequestAdapterMessage.GetPopulatedMessage(), httpWebRequestAdapterMessage.GetStream());
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0002DC00 File Offset: 0x0002BE00
		private static void WriteOdataEntity(ITableEntity entity, TableOperationType operationType, OperationContext ctx, ODataWriter writer, TableRequestOptions options)
		{
			ODataEntry odataEntry = new ODataEntry
			{
				Properties = TableOperationHttpWebRequestFactory.GetPropertiesWithKeys(entity, ctx, operationType, options),
				TypeName = "account.sometype"
			};
			odataEntry.SetAnnotation<SerializationTypeNameAnnotation>(new SerializationTypeNameAnnotation
			{
				TypeName = null
			});
			writer.WriteStart(odataEntry);
			writer.WriteEnd();
			writer.Flush();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0002DC8C File Offset: 0x0002BE8C
		internal static IEnumerable<ODataProperty> GetPropertiesFromDictionary(IDictionary<string, EntityProperty> properties, TableRequestOptions options, string partitionKey, string rowKey)
		{
			if (options != null)
			{
				options.AssertPolicyIfRequired();
				if (options.EncryptionPolicy != null)
				{
					properties = options.EncryptionPolicy.EncryptEntity(properties, partitionKey, rowKey, options.EncryptionResolver);
				}
			}
			return from kvp in properties
			select new ODataProperty
			{
				Name = kvp.Key,
				Value = kvp.Value.PropertyAsObject
			};
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002DFA0 File Offset: 0x0002C1A0
		internal static IEnumerable<ODataProperty> GetPropertiesWithKeys(ITableEntity entity, OperationContext operationContext, TableOperationType operationType, TableRequestOptions options)
		{
			if (operationType == TableOperationType.Insert)
			{
				if (entity.PartitionKey != null)
				{
					yield return new ODataProperty
					{
						Name = "PartitionKey",
						Value = entity.PartitionKey
					};
				}
				if (entity.RowKey != null)
				{
					yield return new ODataProperty
					{
						Name = "RowKey",
						Value = entity.RowKey
					};
				}
			}
			foreach (ODataProperty property in TableOperationHttpWebRequestFactory.GetPropertiesFromDictionary(entity.WriteEntity(operationContext), options, entity.PartitionKey, entity.RowKey))
			{
				yield return property;
			}
			yield break;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002DFD2 File Offset: 0x0002C1D2
		private static void SetAcceptHeaderForHttpWebRequest(HttpWebRequest msg, TablePayloadFormat payloadFormat)
		{
			if (payloadFormat == TablePayloadFormat.AtomPub)
			{
				msg.Accept = "application/atom+xml,application/atomsvc+xml,application/xml";
				return;
			}
			if (payloadFormat == TablePayloadFormat.JsonFullMetadata)
			{
				msg.Accept = "application/json;odata=fullmetadata";
				return;
			}
			if (payloadFormat == TablePayloadFormat.Json)
			{
				msg.Accept = "application/json;odata=minimalmetadata";
				return;
			}
			if (payloadFormat == TablePayloadFormat.JsonNoMetadata)
			{
				msg.Accept = "application/json;odata=nometadata";
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002E012 File Offset: 0x0002C212
		private static void SetContentTypeForHttpWebRequest(HttpWebRequest msg, TablePayloadFormat payloadFormat)
		{
			if (payloadFormat == TablePayloadFormat.AtomPub)
			{
				msg.ContentType = "application/atom+xml";
				return;
			}
			msg.ContentType = "application/json";
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0002E02E File Offset: 0x0002C22E
		private static void SetContentTypeForAdapterMessage(HttpWebRequestAdapterMessage adapterMsg, TablePayloadFormat payloadFormat)
		{
			if (payloadFormat == TablePayloadFormat.AtomPub)
			{
				adapterMsg.SetHeader("Content-Type", "application/atom+xml");
				return;
			}
			adapterMsg.SetHeader("Content-Type", "application/json");
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0002E054 File Offset: 0x0002C254
		private static void SetAcceptAndContentTypeForODataBatchMessage(ODataBatchOperationRequestMessage mimePartMsg, TablePayloadFormat payloadFormat)
		{
			if (payloadFormat == TablePayloadFormat.AtomPub)
			{
				mimePartMsg.SetHeader("Accept", "application/atom+xml,application/atomsvc+xml,application/xml");
				mimePartMsg.SetHeader("Content-Type", "application/atom+xml");
				return;
			}
			if (payloadFormat == TablePayloadFormat.JsonFullMetadata)
			{
				mimePartMsg.SetHeader("Accept", "application/json;odata=fullmetadata");
				mimePartMsg.SetHeader("Content-Type", "application/json");
				return;
			}
			if (payloadFormat == TablePayloadFormat.Json)
			{
				mimePartMsg.SetHeader("Accept", "application/json;odata=minimalmetadata");
				mimePartMsg.SetHeader("Content-Type", "application/json");
				return;
			}
			mimePartMsg.SetHeader("Accept", "application/json;odata=nometadata");
			mimePartMsg.SetHeader("Content-Type", "application/json");
		}
	}
}
