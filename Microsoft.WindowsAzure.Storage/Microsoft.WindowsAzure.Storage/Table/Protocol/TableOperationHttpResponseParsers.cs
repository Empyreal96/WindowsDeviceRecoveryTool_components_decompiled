using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.Data.OData;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004E RID: 78
	internal static class TableOperationHttpResponseParsers
	{
		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
		internal static TableResult TableOperationPreProcess(TableResult result, TableOperation operation, HttpWebResponse resp, Exception ex)
		{
			result.HttpStatusCode = (int)resp.StatusCode;
			if (operation.OperationType == TableOperationType.Retrieve)
			{
				if (resp.StatusCode != HttpStatusCode.OK && resp.StatusCode != HttpStatusCode.NotFound)
				{
					CommonUtility.AssertNotNull("ex", ex);
					throw ex;
				}
			}
			else
			{
				if (ex != null)
				{
					throw ex;
				}
				if (operation.OperationType == TableOperationType.Insert)
				{
					if (operation.EchoContent)
					{
						if (resp.StatusCode != HttpStatusCode.Created)
						{
							throw ex;
						}
					}
					else if (resp.StatusCode != HttpStatusCode.NoContent)
					{
						throw ex;
					}
				}
				else if (resp.StatusCode != HttpStatusCode.NoContent)
				{
					throw ex;
				}
			}
			string etag = HttpResponseParsers.GetETag(resp);
			if (etag != null)
			{
				result.Etag = etag;
				if (operation.Entity != null)
				{
					operation.Entity.ETag = result.Etag;
				}
			}
			return result;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0002E1A8 File Offset: 0x0002C3A8
		internal static TableResult TableOperationPostProcess(TableResult result, TableOperation operation, RESTCommand<TableResult> cmd, HttpWebResponse resp, OperationContext ctx, TableRequestOptions options, string accountName)
		{
			if (operation.OperationType != TableOperationType.Retrieve && operation.OperationType != TableOperationType.Insert)
			{
				result.Etag = HttpResponseParsers.GetETag(resp);
				operation.Entity.ETag = result.Etag;
			}
			else if (operation.OperationType == TableOperationType.Insert && !operation.EchoContent)
			{
				result.Etag = HttpResponseParsers.GetETag(resp);
				operation.Entity.ETag = result.Etag;
				operation.Entity.Timestamp = TableOperationHttpResponseParsers.ParseETagForTimestamp(result.Etag);
			}
			else
			{
				ODataMessageReaderSettings odataMessageReaderSettings = new ODataMessageReaderSettings();
				odataMessageReaderSettings.MessageQuotas = new ODataMessageQuotas
				{
					MaxPartsPerBatch = 1000,
					MaxReceivedMessageSize = 20971520L
				};
				if (resp.ContentType.Contains("application/json;odata=nometadata"))
				{
					result.Etag = resp.Headers["ETag"];
					TableOperationHttpResponseParsers.ReadEntityUsingJsonParser(result, operation, cmd.ResponseStream, ctx, options);
				}
				else
				{
					TableOperationHttpResponseParsers.ReadOdataEntity(result, operation, new HttpResponseAdapterMessage(resp, cmd.ResponseStream), ctx, odataMessageReaderSettings, accountName, options);
				}
			}
			return result;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0002E2B0 File Offset: 0x0002C4B0
		internal static IList<TableResult> TableBatchOperationPostProcess(IList<TableResult> result, TableBatchOperation batch, RESTCommand<IList<TableResult>> cmd, HttpWebResponse resp, OperationContext ctx, TableRequestOptions options, string accountName)
		{
			ODataMessageReaderSettings odataMessageReaderSettings = new ODataMessageReaderSettings();
			odataMessageReaderSettings.MessageQuotas = new ODataMessageQuotas
			{
				MaxPartsPerBatch = 1000,
				MaxReceivedMessageSize = 20971520L
			};
			using (ODataMessageReader odataMessageReader = new ODataMessageReader(new HttpResponseAdapterMessage(resp, cmd.ResponseStream), odataMessageReaderSettings))
			{
				ODataBatchReader odataBatchReader = odataMessageReader.CreateODataBatchReader();
				if (odataBatchReader.State == ODataBatchReaderState.Initial)
				{
					odataBatchReader.Read();
				}
				if (odataBatchReader.State == ODataBatchReaderState.ChangesetStart)
				{
					odataBatchReader.Read();
				}
				int num = 0;
				bool flag = false;
				while (odataBatchReader.State == ODataBatchReaderState.Operation)
				{
					TableOperation tableOperation = batch[num];
					TableResult tableResult = new TableResult
					{
						Result = tableOperation.Entity
					};
					result.Add(tableResult);
					ODataBatchOperationResponseMessage odataBatchOperationResponseMessage = odataBatchReader.CreateOperationResponseMessage();
					string header = odataBatchOperationResponseMessage.GetHeader("Content-Type");
					tableResult.HttpStatusCode = odataBatchOperationResponseMessage.StatusCode;
					bool flag2;
					if (tableOperation.OperationType == TableOperationType.Insert)
					{
						flag = (odataBatchOperationResponseMessage.StatusCode == 409);
						if (tableOperation.EchoContent)
						{
							flag2 = (odataBatchOperationResponseMessage.StatusCode != 201);
						}
						else
						{
							flag2 = (odataBatchOperationResponseMessage.StatusCode != 204);
						}
					}
					else if (tableOperation.OperationType == TableOperationType.Retrieve)
					{
						if (odataBatchOperationResponseMessage.StatusCode == 404)
						{
							num++;
							odataBatchReader.Read();
							continue;
						}
						flag2 = (odataBatchOperationResponseMessage.StatusCode != 200);
					}
					else
					{
						flag = (odataBatchOperationResponseMessage.StatusCode == 404);
						flag2 = (odataBatchOperationResponseMessage.StatusCode != 204);
					}
					if (flag)
					{
						if (cmd.ParseError != null)
						{
							cmd.CurrentResult.ExtendedErrorInformation = cmd.ParseError(odataBatchOperationResponseMessage.GetStream(), resp, header);
						}
						cmd.CurrentResult.HttpStatusCode = odataBatchOperationResponseMessage.StatusCode;
						throw new StorageException(cmd.CurrentResult, (cmd.CurrentResult.ExtendedErrorInformation != null) ? cmd.CurrentResult.ExtendedErrorInformation.ErrorMessage : "An unknown error has occurred, extended error information not available.", null)
						{
							IsRetryable = false
						};
					}
					if (flag2)
					{
						if (cmd.ParseError != null)
						{
							cmd.CurrentResult.ExtendedErrorInformation = cmd.ParseError(odataBatchOperationResponseMessage.GetStream(), resp, header);
						}
						cmd.CurrentResult.HttpStatusCode = odataBatchOperationResponseMessage.StatusCode;
						string str = Convert.ToString(num, CultureInfo.InvariantCulture);
						if (cmd.CurrentResult.ExtendedErrorInformation != null && !string.IsNullOrEmpty(cmd.CurrentResult.ExtendedErrorInformation.ErrorMessage))
						{
							string text = TableRequest.ExtractEntityIndexFromExtendedErrorInformation(cmd.CurrentResult);
							if (!string.IsNullOrEmpty(text))
							{
								str = text;
							}
						}
						throw new StorageException(cmd.CurrentResult, "Unexpected response code for operation : " + str, null)
						{
							IsRetryable = true
						};
					}
					if (!string.IsNullOrEmpty(odataBatchOperationResponseMessage.GetHeader("ETag")))
					{
						tableResult.Etag = odataBatchOperationResponseMessage.GetHeader("ETag");
						if (tableOperation.Entity != null)
						{
							tableOperation.Entity.ETag = tableResult.Etag;
						}
					}
					if (tableOperation.OperationType == TableOperationType.Retrieve || (tableOperation.OperationType == TableOperationType.Insert && tableOperation.EchoContent))
					{
						if (odataBatchOperationResponseMessage.GetHeader("Content-Type").Contains("application/json;odata=nometadata"))
						{
							TableOperationHttpResponseParsers.ReadEntityUsingJsonParser(tableResult, tableOperation, odataBatchOperationResponseMessage.GetStream(), ctx, options);
						}
						else
						{
							TableOperationHttpResponseParsers.ReadOdataEntity(tableResult, tableOperation, odataBatchOperationResponseMessage, ctx, odataMessageReaderSettings, accountName, options);
						}
					}
					else if (tableOperation.OperationType == TableOperationType.Insert)
					{
						tableOperation.Entity.Timestamp = TableOperationHttpResponseParsers.ParseETagForTimestamp(tableResult.Etag);
					}
					num++;
					odataBatchReader.Read();
				}
			}
			return result;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0002E658 File Offset: 0x0002C858
		internal static ResultSegment<TElement> TableQueryPostProcessGeneric<TElement, TQueryType>(Stream responseStream, Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, TElement> resolver, HttpWebResponse resp, TableRequestOptions options, OperationContext ctx, string accountName)
		{
			ResultSegment<TElement> resultSegment = new ResultSegment<TElement>(new List<TElement>());
			resultSegment.ContinuationToken = TableOperationHttpResponseParsers.ContinuationFromResponse(resp);
			if (resp.ContentType.Contains("application/json;odata=nometadata"))
			{
				TableOperationHttpResponseParsers.ReadQueryResponseUsingJsonParser<TElement>(resultSegment, responseStream, resp.Headers["ETag"], resolver, options.PropertyResolver, typeof(TQueryType), null, options);
			}
			else
			{
				ODataMessageReaderSettings odataMessageReaderSettings = new ODataMessageReaderSettings();
				odataMessageReaderSettings.MessageQuotas = new ODataMessageQuotas
				{
					MaxPartsPerBatch = 1000,
					MaxReceivedMessageSize = 20971520L
				};
				using (ODataMessageReader odataMessageReader = new ODataMessageReader(new HttpResponseAdapterMessage(resp, responseStream), odataMessageReaderSettings, new TableStorageModel(accountName)))
				{
					ODataReader odataReader = odataMessageReader.CreateODataFeedReader();
					if (odataReader.State == ODataReaderState.Start)
					{
						odataReader.Read();
					}
					if (odataReader.State == ODataReaderState.FeedStart)
					{
						odataReader.Read();
					}
					while (odataReader.State == ODataReaderState.EntryStart)
					{
						odataReader.Read();
						ODataEntry entry = (ODataEntry)odataReader.Item;
						resultSegment.Results.Add(TableOperationHttpResponseParsers.ReadAndResolve<TElement>(entry, resolver, options));
						odataReader.Read();
					}
					TableOperationHttpResponseParsers.DrainODataReader(odataReader);
				}
			}
			Logger.LogInformational(ctx, "Retrieved '{0}' results with continuation token '{1}'.", new object[]
			{
				resultSegment.Results.Count,
				resultSegment.ContinuationToken
			});
			return resultSegment;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0002E7B8 File Offset: 0x0002C9B8
		private static void DrainODataReader(ODataReader reader)
		{
			if (reader.State == ODataReaderState.FeedEnd)
			{
				reader.Read();
			}
			if (reader.State != ODataReaderState.Completed)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "OData Reader state expected to be Completed state. Actual state: {0}.", new object[]
				{
					reader.State
				}));
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002E80C File Offset: 0x0002CA0C
		private static void ReadQueryResponseUsingJsonParser<TElement>(ResultSegment<TElement> retSeg, Stream responseStream, string etag, Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, TElement> resolver, Func<string, string, string, string, EdmType> propertyResolver, Type type, OperationContext ctx, TableRequestOptions options)
		{
			StreamReader reader = new StreamReader(responseStream);
			bool disablePropertyResolverCache = false;
			if (TableEntity.DisablePropertyResolverCache)
			{
				disablePropertyResolverCache = TableEntity.DisablePropertyResolverCache;
				Logger.LogVerbose(ctx, "Property resolver cache is disabled.", new object[0]);
			}
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				jsonReader.DateParseHandling = DateParseHandling.None;
				JObject jobject = JObject.Load(jsonReader);
				JToken jtoken = jobject["value"];
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jtoken))
				{
					Dictionary<string, string> entityAttributes = jtoken2.ToObject<Dictionary<string, string>>();
					retSeg.Results.Add(TableOperationHttpResponseParsers.ReadAndResolveWithEdmTypeResolver<TElement>(entityAttributes, resolver, propertyResolver, etag, type, ctx, disablePropertyResolverCache, options));
				}
				if (jsonReader.Read())
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The JSON reader has not yet reached the completed state.", new object[0]));
				}
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0002E900 File Offset: 0x0002CB00
		internal static TableContinuationToken ContinuationFromResponse(HttpWebResponse response)
		{
			string text = response.Headers["x-ms-continuation-NextPartitionKey"];
			string text2 = response.Headers["x-ms-continuation-NextRowKey"];
			string text3 = response.Headers["x-ms-continuation-NextTableName"];
			text = (string.IsNullOrEmpty(text) ? null : text);
			text2 = (string.IsNullOrEmpty(text2) ? null : text2);
			text3 = (string.IsNullOrEmpty(text3) ? null : text3);
			if (text == null && text2 == null && text3 == null)
			{
				return null;
			}
			return new TableContinuationToken
			{
				NextPartitionKey = text,
				NextRowKey = text2,
				NextTableName = text3
			};
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0002E998 File Offset: 0x0002CB98
		private static void ReadOdataEntity(TableResult result, TableOperation operation, IODataResponseMessage respMsg, OperationContext ctx, ODataMessageReaderSettings readerSettings, string accountName, TableRequestOptions options)
		{
			using (ODataMessageReader odataMessageReader = new ODataMessageReader(respMsg, readerSettings, new TableStorageModel(accountName)))
			{
				ODataReader odataReader = odataMessageReader.CreateODataEntryReader();
				while (odataReader.Read())
				{
					if (odataReader.State == ODataReaderState.EntryEnd)
					{
						ODataEntry odataEntry = (ODataEntry)odataReader.Item;
						if (operation.OperationType == TableOperationType.Retrieve)
						{
							result.Result = TableOperationHttpResponseParsers.ReadAndResolve<object>(odataEntry, operation.RetrieveResolver, options);
							result.Etag = odataEntry.ETag;
						}
						else
						{
							result.Etag = TableOperationHttpResponseParsers.ReadAndUpdateTableEntity(operation.Entity, odataEntry, EntityReadFlags.Timestamp | EntityReadFlags.Etag, ctx);
						}
					}
				}
				TableOperationHttpResponseParsers.DrainODataReader(odataReader);
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0002EA40 File Offset: 0x0002CC40
		private static void ReadEntityUsingJsonParser(TableResult result, TableOperation operation, Stream stream, OperationContext ctx, TableRequestOptions options)
		{
			StreamReader reader = new StreamReader(stream);
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				JsonSerializer jsonSerializer = new JsonSerializer();
				Dictionary<string, string> entityAttributes = jsonSerializer.Deserialize<Dictionary<string, string>>(jsonReader);
				if (operation.OperationType == TableOperationType.Retrieve)
				{
					result.Result = TableOperationHttpResponseParsers.ReadAndResolveWithEdmTypeResolver<object>(entityAttributes, operation.RetrieveResolver, options.PropertyResolver, result.Etag, operation.PropertyResolverType, ctx, TableEntity.DisablePropertyResolverCache, options);
				}
				else
				{
					TableOperationHttpResponseParsers.ReadAndUpdateTableEntityWithEdmTypeResolver(operation.Entity, entityAttributes, EntityReadFlags.Timestamp | EntityReadFlags.Etag, options.PropertyResolver, ctx);
				}
				if (jsonReader.Read())
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The JSON reader has not yet reached the completed state.", new object[0]));
				}
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0002EAF8 File Offset: 0x0002CCF8
		private static T ReadAndResolve<T>(ODataEntry entry, Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, T> resolver, TableRequestOptions options)
		{
			string text = null;
			string text2 = null;
			DateTimeOffset arg = default(DateTimeOffset);
			Dictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();
			foreach (ODataProperty odataProperty in entry.Properties)
			{
				string name = odataProperty.Name;
				if (name == "PartitionKey")
				{
					text = (string)odataProperty.Value;
				}
				else if (name == "RowKey")
				{
					text2 = (string)odataProperty.Value;
				}
				else if (name == "Timestamp")
				{
					arg = new DateTimeOffset((DateTime)odataProperty.Value);
				}
				else
				{
					dictionary.Add(name, EntityProperty.CreateEntityPropertyFromObject(odataProperty.Value));
				}
			}
			if (options.EncryptionPolicy != null)
			{
				EntityProperty entityProperty;
				EntityProperty encryptionKeyProperty;
				if (dictionary.TryGetValue("_ClientEncryptionMetadata2", out entityProperty) && dictionary.TryGetValue("_ClientEncryptionMetadata1", out encryptionKeyProperty))
				{
					EncryptionData encryptionData = null;
					byte[] contentEncryptionKey = options.EncryptionPolicy.DecryptMetadataAndReturnCEK(text, text2, encryptionKeyProperty, entityProperty, out encryptionData);
					byte[] binaryValue = entityProperty.BinaryValue;
					HashSet<string> encryptedPropertyDetailsSet = JsonConvert.DeserializeObject<HashSet<string>>(Encoding.UTF8.GetString(binaryValue, 0, binaryValue.Length));
					dictionary = options.EncryptionPolicy.DecryptEntity(dictionary, encryptedPropertyDetailsSet, text, text2, contentEncryptionKey, encryptionData);
				}
				else if (options.RequireEncryption != null && options.RequireEncryption.Value)
				{
					throw new StorageException("Encryption data does not exist. If you do not want to decrypt the data, please do not set the RequireEncryption flag on request options.", null)
					{
						IsRetryable = false
					};
				}
			}
			return resolver(text, text2, arg, dictionary, entry.ETag);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0002ECA0 File Offset: 0x0002CEA0
		private static T ReadAndResolveWithEdmTypeResolver<T>(Dictionary<string, string> entityAttributes, Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, T> resolver, Func<string, string, string, string, EdmType> propertyResolver, string etag, Type type, OperationContext ctx, bool disablePropertyResolverCache, TableRequestOptions options)
		{
			string text = null;
			string text2 = null;
			byte[] contentEncryptionKey = null;
			EncryptionData encryptionData = null;
			DateTimeOffset arg = default(DateTimeOffset);
			Dictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();
			Dictionary<string, EdmType> dictionary2 = null;
			HashSet<string> encryptedPropertyDetailsSet = null;
			if (type != null)
			{
				if (!disablePropertyResolverCache)
				{
					dictionary2 = TableEntity.PropertyResolverCache.GetOrAdd(type, new Func<Type, Dictionary<string, EdmType>>(TableOperationHttpResponseParsers.CreatePropertyResolverDictionary));
				}
				else
				{
					dictionary2 = TableOperationHttpResponseParsers.CreatePropertyResolverDictionary(type);
				}
			}
			if (options.EncryptionPolicy != null)
			{
				string value = null;
				string value2 = null;
				if (entityAttributes.TryGetValue("_ClientEncryptionMetadata2", out value) && entityAttributes.TryGetValue("_ClientEncryptionMetadata1", out value2))
				{
					EntityProperty entityProperty = EntityProperty.CreateEntityPropertyFromObject(value, EdmType.Binary);
					EntityProperty encryptionKeyProperty = EntityProperty.CreateEntityPropertyFromObject(value2, EdmType.String);
					entityAttributes.TryGetValue("PartitionKey", out text);
					entityAttributes.TryGetValue("RowKey", out text2);
					contentEncryptionKey = options.EncryptionPolicy.DecryptMetadataAndReturnCEK(text, text2, encryptionKeyProperty, entityProperty, out encryptionData);
					dictionary.Add("_ClientEncryptionMetadata2", entityProperty);
					byte[] binaryValue = entityProperty.BinaryValue;
					encryptedPropertyDetailsSet = JsonConvert.DeserializeObject<HashSet<string>>(Encoding.UTF8.GetString(binaryValue, 0, binaryValue.Length));
				}
				else if (options.RequireEncryption != null && options.RequireEncryption.Value)
				{
					throw new StorageException("Encryption data does not exist. If you do not want to decrypt the data, please do not set the RequireEncryption flag on request options.", null)
					{
						IsRetryable = false
					};
				}
			}
			foreach (KeyValuePair<string, string> prop in entityAttributes)
			{
				if (prop.Key == "PartitionKey")
				{
					text = prop.Value;
				}
				else if (prop.Key == "RowKey")
				{
					text2 = prop.Value;
				}
				else if (prop.Key == "Timestamp")
				{
					arg = DateTimeOffset.Parse(prop.Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					if (etag == null)
					{
						etag = TableOperationHttpResponseParsers.GetETagFromTimestamp(prop.Value);
					}
				}
				else if (prop.Key == "_ClientEncryptionMetadata1")
				{
					dictionary.Add(prop.Key, EntityProperty.CreateEntityPropertyFromObject(prop.Value, EdmType.String));
				}
				else if (prop.Key == "_ClientEncryptionMetadata2")
				{
					if (!dictionary.ContainsKey("_ClientEncryptionMetadata2"))
					{
						dictionary.Add(prop.Key, EntityProperty.CreateEntityPropertyFromObject(prop.Value, EdmType.Binary));
					}
				}
				else
				{
					if (propertyResolver != null)
					{
						Logger.LogVerbose(ctx, "Using the property resolver provided via TableRequestOptions to deserialize the entity.", new object[0]);
						try
						{
							EdmType edmType = propertyResolver(text, text2, prop.Key, prop.Value);
							Logger.LogVerbose(ctx, "Attempting to deserialize '{0}' as type '{1}'", new object[]
							{
								prop.Key,
								edmType
							});
							try
							{
								TableOperationHttpResponseParsers.CreateEntityPropertyFromObject(dictionary, encryptedPropertyDetailsSet, prop, edmType);
							}
							catch (FormatException innerException)
							{
								throw new StorageException(string.Format(CultureInfo.InvariantCulture, "Failed to parse property '{0}' with value '{1}' as type '{2}'", new object[]
								{
									prop.Key,
									prop.Value,
									edmType
								}), innerException)
								{
									IsRetryable = false
								};
							}
							continue;
						}
						catch (StorageException)
						{
							throw;
						}
						catch (Exception innerException2)
						{
							throw new StorageException("The custom property resolver delegate threw an exception. Check the inner exception for more details", innerException2)
							{
								IsRetryable = false
							};
						}
					}
					if (type != null)
					{
						Logger.LogVerbose(ctx, "Using the default property resolver to deserialize the entity.", new object[0]);
						if (dictionary2 != null)
						{
							EdmType edmType2;
							dictionary2.TryGetValue(prop.Key, out edmType2);
							Logger.LogVerbose(ctx, "Attempting to deserialize '{0}' as type '{1}'", new object[]
							{
								prop.Key,
								edmType2
							});
							TableOperationHttpResponseParsers.CreateEntityPropertyFromObject(dictionary, encryptedPropertyDetailsSet, prop, edmType2);
						}
					}
					else
					{
						Logger.LogVerbose(ctx, "No property resolver available. Deserializing the entity properties as strings.", new object[0]);
						TableOperationHttpResponseParsers.CreateEntityPropertyFromObject(dictionary, encryptedPropertyDetailsSet, prop, EdmType.String);
					}
				}
			}
			if (options.EncryptionPolicy != null && encryptionData != null)
			{
				dictionary = options.EncryptionPolicy.DecryptEntity(dictionary, encryptedPropertyDetailsSet, text, text2, contentEncryptionKey, encryptionData);
			}
			return resolver(text, text2, arg, dictionary, etag);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0002F108 File Offset: 0x0002D308
		private static void CreateEntityPropertyFromObject(Dictionary<string, EntityProperty> properties, HashSet<string> encryptedPropertyDetailsSet, KeyValuePair<string, string> prop, EdmType edmType)
		{
			if (encryptedPropertyDetailsSet != null && encryptedPropertyDetailsSet.Contains(prop.Key))
			{
				properties.Add(prop.Key, EntityProperty.CreateEntityPropertyFromObject(prop.Value, EdmType.Binary));
				return;
			}
			properties.Add(prop.Key, EntityProperty.CreateEntityPropertyFromObject(prop.Value, edmType));
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002F15C File Offset: 0x0002D35C
		internal static string ReadAndUpdateTableEntity(ITableEntity entity, ODataEntry entry, EntityReadFlags flags, OperationContext ctx)
		{
			if ((flags & EntityReadFlags.Etag) > (EntityReadFlags)0)
			{
				entity.ETag = entry.ETag;
			}
			Dictionary<string, EntityProperty> dictionary = ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0) ? new Dictionary<string, EntityProperty>() : null;
			if (flags > (EntityReadFlags)0)
			{
				foreach (ODataProperty odataProperty in entry.Properties)
				{
					if (odataProperty.Name == "PartitionKey")
					{
						if ((flags & EntityReadFlags.PartitionKey) != (EntityReadFlags)0)
						{
							entity.PartitionKey = (string)odataProperty.Value;
						}
					}
					else if (odataProperty.Name == "RowKey")
					{
						if ((flags & EntityReadFlags.RowKey) != (EntityReadFlags)0)
						{
							entity.RowKey = (string)odataProperty.Value;
						}
					}
					else if (odataProperty.Name == "Timestamp")
					{
						if ((flags & EntityReadFlags.Timestamp) != (EntityReadFlags)0)
						{
							entity.Timestamp = (DateTime)odataProperty.Value;
						}
					}
					else if ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0)
					{
						dictionary.Add(odataProperty.Name, EntityProperty.CreateEntityPropertyFromObject(odataProperty.Value));
					}
				}
				if ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0)
				{
					entity.ReadEntity(dictionary, ctx);
				}
			}
			return entry.ETag;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002F28C File Offset: 0x0002D48C
		internal static void ReadAndUpdateTableEntityWithEdmTypeResolver(ITableEntity entity, Dictionary<string, string> entityAttributes, EntityReadFlags flags, Func<string, string, string, string, EdmType> propertyResolver, OperationContext ctx)
		{
			Dictionary<string, EntityProperty> dictionary = ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0) ? new Dictionary<string, EntityProperty>() : null;
			Dictionary<string, EdmType> dictionary2 = null;
			if (entity.GetType() != typeof(DynamicTableEntity))
			{
				if (!TableEntity.DisablePropertyResolverCache)
				{
					dictionary2 = TableEntity.PropertyResolverCache.GetOrAdd(entity.GetType(), new Func<Type, Dictionary<string, EdmType>>(TableOperationHttpResponseParsers.CreatePropertyResolverDictionary));
				}
				else
				{
					Logger.LogVerbose(ctx, "Property resolver cache is disabled.", new object[0]);
					dictionary2 = TableOperationHttpResponseParsers.CreatePropertyResolverDictionary(entity.GetType());
				}
			}
			if (flags > (EntityReadFlags)0)
			{
				foreach (KeyValuePair<string, string> keyValuePair in entityAttributes)
				{
					if (keyValuePair.Key == "PartitionKey")
					{
						entity.PartitionKey = keyValuePair.Value;
					}
					else if (keyValuePair.Key == "RowKey")
					{
						entity.RowKey = keyValuePair.Value;
					}
					else if (keyValuePair.Key == "Timestamp")
					{
						if ((flags & EntityReadFlags.Timestamp) != (EntityReadFlags)0)
						{
							entity.Timestamp = DateTime.Parse(keyValuePair.Value, CultureInfo.InvariantCulture);
						}
					}
					else if ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0)
					{
						if (propertyResolver != null)
						{
							Logger.LogVerbose(ctx, "Using the property resolver provided via TableRequestOptions to deserialize the entity.", new object[0]);
							try
							{
								EdmType edmType = propertyResolver(entity.PartitionKey, entity.RowKey, keyValuePair.Key, keyValuePair.Value);
								Logger.LogVerbose(ctx, "Attempting to deserialize '{0}' as type '{1}'", new object[]
								{
									keyValuePair.Key,
									edmType.GetType().ToString()
								});
								try
								{
									dictionary.Add(keyValuePair.Key, EntityProperty.CreateEntityPropertyFromObject(keyValuePair.Value, edmType.GetType()));
								}
								catch (FormatException innerException)
								{
									throw new StorageException(string.Format(CultureInfo.InvariantCulture, "Failed to parse property '{0}' with value '{1}' as type '{2}'", new object[]
									{
										keyValuePair.Key,
										keyValuePair.Value,
										edmType.ToString()
									}), innerException)
									{
										IsRetryable = false
									};
								}
								continue;
							}
							catch (StorageException)
							{
								throw;
							}
							catch (Exception innerException2)
							{
								throw new StorageException("The custom property resolver delegate threw an exception. Check the inner exception for more details", innerException2)
								{
									IsRetryable = false
								};
							}
						}
						if (entity.GetType() != typeof(DynamicTableEntity))
						{
							Logger.LogVerbose(ctx, "Using the default property resolver to deserialize the entity.", new object[0]);
							if (dictionary2 != null)
							{
								EdmType edmType2;
								dictionary2.TryGetValue(keyValuePair.Key, out edmType2);
								Logger.LogVerbose(ctx, "Attempting to deserialize '{0}' as type '{1}'", new object[]
								{
									keyValuePair.Key,
									edmType2
								});
								dictionary.Add(keyValuePair.Key, EntityProperty.CreateEntityPropertyFromObject(keyValuePair.Value, edmType2));
							}
						}
						else
						{
							Logger.LogVerbose(ctx, "No property resolver available. Deserializing the entity properties as strings.", new object[0]);
							dictionary.Add(keyValuePair.Key, EntityProperty.CreateEntityPropertyFromObject(keyValuePair.Value, typeof(string)));
						}
					}
				}
				if ((flags & EntityReadFlags.Properties) > (EntityReadFlags)0)
				{
					entity.ReadEntity(dictionary, ctx);
				}
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002F60C File Offset: 0x0002D80C
		private static DateTimeOffset ParseETagForTimestamp(string etag)
		{
			if (etag.StartsWith("W/", StringComparison.Ordinal))
			{
				etag = etag.Substring(2);
			}
			etag = etag.Substring("\"datetime'".Length, etag.Length - 2 - "\"datetime'".Length);
			return DateTimeOffset.Parse(Uri.UnescapeDataString(etag), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002F667 File Offset: 0x0002D867
		private static string GetETagFromTimestamp(string timeStampString)
		{
			timeStampString = Uri.EscapeDataString(timeStampString);
			return "W/\"datetime'" + timeStampString + "'\"";
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002F684 File Offset: 0x0002D884
		private static Dictionary<string, EdmType> CreatePropertyResolverDictionary(Type type)
		{
			Dictionary<string, EdmType> dictionary = new Dictionary<string, EdmType>();
			IEnumerable<PropertyInfo> properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.PropertyType == typeof(byte[]))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Binary);
				}
				else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Boolean);
				}
				else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTimeOffset) || propertyInfo.PropertyType == typeof(DateTimeOffset?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.DateTime);
				}
				else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Double);
				}
				else if (propertyInfo.PropertyType == typeof(Guid) || propertyInfo.PropertyType == typeof(Guid?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Guid);
				}
				else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Int32);
				}
				else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
				{
					dictionary.Add(propertyInfo.Name, EdmType.Int64);
				}
				else
				{
					dictionary.Add(propertyInfo.Name, EdmType.String);
				}
			}
			return dictionary;
		}
	}
}
