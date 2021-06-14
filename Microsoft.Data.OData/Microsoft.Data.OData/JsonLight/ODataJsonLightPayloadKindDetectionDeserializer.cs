using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000199 RID: 409
	internal sealed class ODataJsonLightPayloadKindDetectionDeserializer : ODataJsonLightPropertyAndValueDeserializer
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x0002A56B File Offset: 0x0002876B
		internal ODataJsonLightPayloadKindDetectionDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0002A574 File Offset: 0x00028774
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind(ODataPayloadKindDetectionInfo detectionInfo)
		{
			base.JsonReader.DisableInStreamErrorDetection = true;
			IEnumerable<ODataPayloadKind> result;
			try
			{
				base.ReadPayloadStart(ODataPayloadKind.Unsupported, null, false, false);
				result = this.DetectPayloadKindImplementation(detectionInfo);
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

		// Token: 0x06000C5D RID: 3165 RVA: 0x0002A608 File Offset: 0x00028808
		internal Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(ODataPayloadKindDetectionInfo detectionInfo)
		{
			base.JsonReader.DisableInStreamErrorDetection = true;
			return base.ReadPayloadStartAsync(ODataPayloadKind.Unsupported, null, false, false).FollowOnSuccessWith((Task t) => this.DetectPayloadKindImplementation(detectionInfo)).FollowOnFaultAndCatchExceptionWith((ODataException t) => Enumerable.Empty<ODataPayloadKind>()).FollowAlwaysWith(delegate(Task<IEnumerable<ODataPayloadKind>> t)
			{
				base.JsonReader.DisableInStreamErrorDetection = false;
			});
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002A688 File Offset: 0x00028888
		private IEnumerable<ODataPayloadKind> DetectPayloadKindImplementation(ODataPayloadKindDetectionInfo detectionInfo)
		{
			if (base.MetadataUriParseResult != null)
			{
				detectionInfo.SetPayloadKindDetectionFormatState(new ODataJsonLightPayloadKindDetectionState(base.MetadataUriParseResult));
				return base.MetadataUriParseResult.DetectedPayloadKinds;
			}
			ODataError odataError = null;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				if (!ODataJsonLightReaderUtils.IsAnnotationProperty(text))
				{
					return Enumerable.Empty<ODataPayloadKind>();
				}
				string text2;
				string text3;
				if (ODataJsonLightDeserializer.TryParsePropertyAnnotation(text, out text2, out text3))
				{
					return Enumerable.Empty<ODataPayloadKind>();
				}
				if (ODataJsonLightReaderUtils.IsODataAnnotationName(text))
				{
					if (string.CompareOrdinal("odata.error", text) != 0)
					{
						return Enumerable.Empty<ODataPayloadKind>();
					}
					if (odataError != null || !base.JsonReader.StartBufferingAndTryToReadInStreamErrorPropertyValue(out odataError))
					{
						return Enumerable.Empty<ODataPayloadKind>();
					}
					base.JsonReader.SkipValue();
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			if (odataError == null)
			{
				return Enumerable.Empty<ODataPayloadKind>();
			}
			return new ODataPayloadKind[]
			{
				ODataPayloadKind.Error
			};
		}
	}
}
