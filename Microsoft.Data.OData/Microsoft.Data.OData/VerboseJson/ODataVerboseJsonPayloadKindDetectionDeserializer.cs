using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001B8 RID: 440
	internal sealed class ODataVerboseJsonPayloadKindDetectionDeserializer : ODataVerboseJsonPropertyAndValueDeserializer
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002F8A3 File Offset: 0x0002DAA3
		internal ODataVerboseJsonPayloadKindDetectionDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002F8BC File Offset: 0x0002DABC
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind()
		{
			this.detectedPayloadKinds.Clear();
			base.JsonReader.DisableInStreamErrorDetection = true;
			IEnumerable<ODataPayloadKind> result;
			try
			{
				base.ReadPayloadStart(false);
				JsonNodeType nodeType = base.JsonReader.NodeType;
				if (nodeType == JsonNodeType.StartObject)
				{
					base.JsonReader.ReadStartObject();
					int num = 0;
					while (base.JsonReader.NodeType == JsonNodeType.Property)
					{
						string strB = base.JsonReader.ReadPropertyName();
						num++;
						if (string.CompareOrdinal("__metadata", strB) == 0)
						{
							this.ProcessMetadataPropertyValue();
							break;
						}
						if (num == 1)
						{
							this.AddPayloadKinds(new ODataPayloadKind[]
							{
								ODataPayloadKind.Property,
								ODataPayloadKind.Entry,
								ODataPayloadKind.Parameter
							});
							ODataError odataError;
							if (string.CompareOrdinal("uri", strB) == 0 && base.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
							{
								this.AddPayloadKinds(new ODataPayloadKind[]
								{
									ODataPayloadKind.EntityReferenceLink
								});
							}
							else if (string.CompareOrdinal("error", strB) == 0 && base.JsonReader.StartBufferingAndTryToReadInStreamErrorPropertyValue(out odataError))
							{
								this.AddPayloadKinds(new ODataPayloadKind[]
								{
									ODataPayloadKind.Error
								});
							}
						}
						else if (num == 2)
						{
							this.RemovePayloadKinds(new ODataPayloadKind[]
							{
								ODataPayloadKind.Property,
								ODataPayloadKind.EntityReferenceLink,
								ODataPayloadKind.Error
							});
						}
						if (string.CompareOrdinal("results", strB) == 0 && base.JsonReader.NodeType == JsonNodeType.StartArray)
						{
							this.DetectStartArrayPayloadKind(false);
						}
						else if (base.ReadingResponse && string.CompareOrdinal("EntitySets", strB) == 0 && base.JsonReader.NodeType == JsonNodeType.StartArray)
						{
							this.ProcessEntitySetsArray();
						}
						base.JsonReader.SkipValue();
					}
					if (num == 0)
					{
						this.AddPayloadKinds(new ODataPayloadKind[]
						{
							ODataPayloadKind.Entry,
							ODataPayloadKind.Parameter
						});
					}
				}
				else if (nodeType == JsonNodeType.StartArray)
				{
					this.DetectStartArrayPayloadKind(true);
				}
				result = this.detectedPayloadKinds;
			}
			catch (ODataException)
			{
				result = Enumerable.Empty<ODataPayloadKind>();
			}
			finally
			{
				base.JsonReader.DisableInStreamErrorDetection = false;
			}
			return result;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002FAD0 File Offset: 0x0002DCD0
		private void DetectStartArrayPayloadKind(bool isTopLevel)
		{
			if (!isTopLevel)
			{
				this.AddPayloadKinds(new ODataPayloadKind[]
				{
					ODataPayloadKind.Property
				});
			}
			base.JsonReader.StartBuffering();
			try
			{
				base.JsonReader.ReadStartArray();
				JsonNodeType nodeType = base.JsonReader.NodeType;
				if (nodeType != JsonNodeType.StartObject)
				{
					switch (nodeType)
					{
					case JsonNodeType.EndArray:
						this.AddPayloadKinds(new ODataPayloadKind[]
						{
							ODataPayloadKind.Feed,
							ODataPayloadKind.Collection,
							ODataPayloadKind.EntityReferenceLinks
						});
						break;
					case JsonNodeType.PrimitiveValue:
						this.AddPayloadKinds(new ODataPayloadKind[]
						{
							ODataPayloadKind.Collection
						});
						break;
					}
				}
				else
				{
					base.JsonReader.ReadStartObject();
					bool flag = false;
					int num = 0;
					while (base.JsonReader.NodeType == JsonNodeType.Property)
					{
						string strB = base.JsonReader.ReadPropertyName();
						num++;
						if (num > 1)
						{
							break;
						}
						if (string.CompareOrdinal("uri", strB) == 0 && base.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
						{
							flag = true;
						}
						base.JsonReader.SkipValue();
					}
					this.AddPayloadKinds(new ODataPayloadKind[]
					{
						ODataPayloadKind.Feed,
						ODataPayloadKind.Collection
					});
					if (flag && num == 1)
					{
						this.AddPayloadKinds(new ODataPayloadKind[]
						{
							ODataPayloadKind.EntityReferenceLinks
						});
					}
				}
			}
			finally
			{
				base.JsonReader.StopBuffering();
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002FC28 File Offset: 0x0002DE28
		private void ProcessMetadataPropertyValue()
		{
			this.detectedPayloadKinds.Clear();
			string text = base.ReadTypeNameFromMetadataPropertyValue();
			EdmTypeKind edmTypeKind = EdmTypeKind.None;
			if (text != null)
			{
				MetadataUtils.ResolveTypeNameForRead(EdmCoreModel.Instance, null, text, base.MessageReaderSettings.ReaderBehavior, base.Version, out edmTypeKind);
			}
			if (edmTypeKind == EdmTypeKind.Primitive || edmTypeKind == EdmTypeKind.Collection)
			{
				return;
			}
			this.detectedPayloadKinds.Add(ODataPayloadKind.Entry);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002FC84 File Offset: 0x0002DE84
		private void ProcessEntitySetsArray()
		{
			base.JsonReader.StartBuffering();
			try
			{
				base.JsonReader.ReadStartArray();
				if (base.JsonReader.NodeType == JsonNodeType.EndArray || base.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
				{
					this.AddPayloadKinds(new ODataPayloadKind[]
					{
						ODataPayloadKind.ServiceDocument
					});
				}
			}
			finally
			{
				base.JsonReader.StopBuffering();
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002FCF4 File Offset: 0x0002DEF4
		private void AddPayloadKinds(params ODataPayloadKind[] payloadKinds)
		{
			this.AddOrRemovePayloadKinds(new Func<ODataPayloadKind, bool>(this.detectedPayloadKinds.Add), payloadKinds);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002FD0E File Offset: 0x0002DF0E
		private void RemovePayloadKinds(params ODataPayloadKind[] payloadKinds)
		{
			this.AddOrRemovePayloadKinds(new Func<ODataPayloadKind, bool>(this.detectedPayloadKinds.Remove), payloadKinds);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002FD28 File Offset: 0x0002DF28
		private void AddOrRemovePayloadKinds(Func<ODataPayloadKind, bool> addOrRemoveAction, params ODataPayloadKind[] payloadKinds)
		{
			foreach (ODataPayloadKind odataPayloadKind in payloadKinds)
			{
				if (ODataUtilsInternal.IsPayloadKindSupported(odataPayloadKind, !base.ReadingResponse))
				{
					addOrRemoveAction(odataPayloadKind);
				}
			}
		}

		// Token: 0x04000491 RID: 1169
		private readonly HashSet<ODataPayloadKind> detectedPayloadKinds = new HashSet<ODataPayloadKind>(EqualityComparer<ODataPayloadKind>.Default);
	}
}
