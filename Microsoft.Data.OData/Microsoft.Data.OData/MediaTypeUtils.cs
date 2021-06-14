using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Data.OData
{
	// Token: 0x02000257 RID: 599
	internal static class MediaTypeUtils
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00049D95 File Offset: 0x00047F95
		internal static UTF8Encoding EncodingUtf8NoPreamble
		{
			get
			{
				return MediaTypeUtils.encodingUtf8NoPreamble;
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00049DBC File Offset: 0x00047FBC
		internal static ODataFormat GetContentTypeFromSettings(ODataMessageWriterSettings settings, ODataPayloadKind payloadKind, MediaTypeResolver mediaTypeResolver, out MediaType mediaType, out Encoding encoding)
		{
			IList<MediaTypeWithFormat> mediaTypesForPayloadKind = mediaTypeResolver.GetMediaTypesForPayloadKind(payloadKind);
			if (mediaTypesForPayloadKind == null || mediaTypesForPayloadKind.Count == 0)
			{
				throw new ODataContentTypeException(Strings.MediaTypeUtils_DidNotFindMatchingMediaType(null, settings.AcceptableMediaTypes));
			}
			ODataFormat format;
			if (settings.UseFormat == true)
			{
				mediaType = MediaTypeUtils.GetDefaultMediaType(mediaTypesForPayloadKind, settings.Format, out format);
				encoding = mediaType.SelectEncoding();
			}
			else
			{
				IList<KeyValuePair<MediaType, string>> list = HttpUtils.MediaTypesFromString(settings.AcceptableMediaTypes);
				if (settings.Version >= ODataVersion.V3)
				{
					MediaTypeUtils.ConvertApplicationJsonInAcceptableMediaTypes(list);
				}
				string text = null;
				MediaTypeWithFormat mediaTypeWithFormat;
				if (list == null || list.Count == 0)
				{
					mediaTypeWithFormat = mediaTypesForPayloadKind[0];
				}
				else
				{
					MediaTypeUtils.MediaTypeMatchInfo mediaTypeMatchInfo = MediaTypeUtils.MatchMediaTypes(from kvp in list
					select kvp.Key, mediaTypesForPayloadKind.Select((MediaTypeWithFormat smt) => smt.MediaType).ToArray<MediaType>());
					if (mediaTypeMatchInfo == null)
					{
						string p = string.Join(", ", (from mt in mediaTypesForPayloadKind
						select mt.MediaType.ToText()).ToArray<string>());
						throw new ODataContentTypeException(Strings.MediaTypeUtils_DidNotFindMatchingMediaType(p, settings.AcceptableMediaTypes));
					}
					mediaTypeWithFormat = mediaTypesForPayloadKind[mediaTypeMatchInfo.TargetTypeIndex];
					text = list[mediaTypeMatchInfo.SourceTypeIndex].Value;
				}
				format = mediaTypeWithFormat.Format;
				mediaType = mediaTypeWithFormat.MediaType;
				string text2 = settings.AcceptableCharsets;
				if (text != null)
				{
					text2 = ((text2 == null) ? text : (text + "," + text2));
				}
				encoding = MediaTypeUtils.GetEncoding(text2, payloadKind, mediaType, true);
			}
			return format;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00049F90 File Offset: 0x00048190
		internal static ODataFormat GetFormatFromContentType(string contentTypeHeader, ODataPayloadKind[] supportedPayloadKinds, MediaTypeResolver mediaTypeResolver, out MediaType mediaType, out Encoding encoding, out ODataPayloadKind selectedPayloadKind, out string batchBoundary)
		{
			ODataFormat formatFromContentType = MediaTypeUtils.GetFormatFromContentType(contentTypeHeader, supportedPayloadKinds, mediaTypeResolver, out mediaType, out encoding, out selectedPayloadKind);
			if (selectedPayloadKind == ODataPayloadKind.Batch)
			{
				KeyValuePair<string, string> keyValuePair = default(KeyValuePair<string, string>);
				IEnumerable<KeyValuePair<string, string>> parameters = mediaType.Parameters;
				if (parameters != null)
				{
					bool flag = false;
					foreach (KeyValuePair<string, string> keyValuePair2 in from p in parameters
					where HttpUtils.CompareMediaTypeParameterNames("boundary", p.Key)
					select p)
					{
						if (flag)
						{
							throw new ODataException(Strings.MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads(contentTypeHeader, "boundary"));
						}
						keyValuePair = keyValuePair2;
						flag = true;
					}
				}
				if (keyValuePair.Key == null)
				{
					throw new ODataException(Strings.MediaTypeUtils_BoundaryMustBeSpecifiedForBatchPayloads(contentTypeHeader, "boundary"));
				}
				batchBoundary = keyValuePair.Value;
				ValidationUtils.ValidateBoundaryString(batchBoundary);
			}
			else
			{
				batchBoundary = null;
			}
			return formatFromContentType;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0004A07C File Offset: 0x0004827C
		internal static IList<ODataPayloadKindDetectionResult> GetPayloadKindsForContentType(string contentTypeHeader, MediaTypeResolver mediaTypeResolver, out MediaType contentType, out Encoding encoding)
		{
			encoding = null;
			string text;
			contentType = MediaTypeUtils.ParseContentType(contentTypeHeader, out text);
			MediaType[] targetTypes = new MediaType[]
			{
				contentType
			};
			List<ODataPayloadKindDetectionResult> list = new List<ODataPayloadKindDetectionResult>();
			for (int i = 0; i < MediaTypeUtils.allSupportedPayloadKinds.Length; i++)
			{
				ODataPayloadKind payloadKind = MediaTypeUtils.allSupportedPayloadKinds[i];
				IList<MediaTypeWithFormat> mediaTypesForPayloadKind = mediaTypeResolver.GetMediaTypesForPayloadKind(payloadKind);
				MediaTypeUtils.MediaTypeMatchInfo mediaTypeMatchInfo = MediaTypeUtils.MatchMediaTypes(from smt in mediaTypesForPayloadKind
				select smt.MediaType, targetTypes);
				if (mediaTypeMatchInfo != null)
				{
					list.Add(new ODataPayloadKindDetectionResult(payloadKind, mediaTypesForPayloadKind[mediaTypeMatchInfo.SourceTypeIndex].Format));
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				encoding = HttpUtils.GetEncodingFromCharsetName(text);
			}
			return list;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0004A137 File Offset: 0x00048337
		internal static bool MediaTypeAndSubtypeAreEqual(string firstTypeAndSubtype, string secondTypeAndSubtype)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(firstTypeAndSubtype, "firstTypeAndSubtype");
			ExceptionUtils.CheckArgumentNotNull<string>(secondTypeAndSubtype, "secondTypeAndSubtype");
			return HttpUtils.CompareMediaTypeNames(firstTypeAndSubtype, secondTypeAndSubtype);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0004A156 File Offset: 0x00048356
		internal static bool MediaTypeStartsWithTypeAndSubtype(string mediaType, string typeAndSubtype)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(mediaType, "mediaType");
			ExceptionUtils.CheckArgumentNotNull<string>(typeAndSubtype, "typeAndSubtype");
			return mediaType.StartsWith(typeAndSubtype, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0004A1AC File Offset: 0x000483AC
		internal static bool MediaTypeHasParameterWithValue(this MediaType mediaType, string parameterName, string parameterValue)
		{
			return mediaType.Parameters != null && mediaType.Parameters.Any((KeyValuePair<string, string> p) => HttpUtils.CompareMediaTypeParameterNames(p.Key, parameterName) && string.Compare(p.Value, parameterValue, StringComparison.OrdinalIgnoreCase) == 0);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0004A1EE File Offset: 0x000483EE
		internal static bool HasStreamingSetToTrue(this MediaType mediaType)
		{
			return mediaType.MediaTypeHasParameterWithValue("streaming", "true");
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004A200 File Offset: 0x00048400
		internal static void CheckMediaTypeForWildCards(MediaType mediaType)
		{
			if (HttpUtils.CompareMediaTypeNames("*", mediaType.TypeName) || HttpUtils.CompareMediaTypeNames("*", mediaType.SubTypeName))
			{
				throw new ODataContentTypeException(Strings.ODataMessageReader_WildcardInContentType(mediaType.FullTypeName));
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004A238 File Offset: 0x00048438
		internal static string AlterContentTypeForJsonPadding(string contentType)
		{
			if (contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
			{
				return contentType.Remove(0, "application/json".Length).Insert(0, "text/javascript");
			}
			if (contentType.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase))
			{
				return contentType.Remove(0, "text/plain".Length).Insert(0, "text/javascript");
			}
			throw new ODataException(Strings.ODataMessageWriter_JsonPaddingOnInvalidContentType(contentType));
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0004A2F4 File Offset: 0x000484F4
		private static ODataFormat GetFormatFromContentType(string contentTypeName, ODataPayloadKind[] supportedPayloadKinds, MediaTypeResolver mediaTypeResolver, out MediaType mediaType, out Encoding encoding, out ODataPayloadKind selectedPayloadKind)
		{
			string acceptCharsetHeader;
			mediaType = MediaTypeUtils.ParseContentType(contentTypeName, out acceptCharsetHeader);
			if (!mediaTypeResolver.IsIllegalMediaType(mediaType))
			{
				foreach (ODataPayloadKind odataPayloadKind in supportedPayloadKinds)
				{
					IList<MediaTypeWithFormat> mediaTypesForPayloadKind = mediaTypeResolver.GetMediaTypesForPayloadKind(odataPayloadKind);
					MediaTypeUtils.MediaTypeMatchInfo mediaTypeMatchInfo = MediaTypeUtils.MatchMediaTypes(from smt in mediaTypesForPayloadKind
					select smt.MediaType, new MediaType[]
					{
						mediaType
					});
					if (mediaTypeMatchInfo != null)
					{
						selectedPayloadKind = odataPayloadKind;
						encoding = MediaTypeUtils.GetEncoding(acceptCharsetHeader, selectedPayloadKind, mediaType, false);
						return mediaTypesForPayloadKind[mediaTypeMatchInfo.SourceTypeIndex].Format;
					}
				}
			}
			string p = string.Join(", ", supportedPayloadKinds.SelectMany((ODataPayloadKind pk) => from mt in mediaTypeResolver.GetMediaTypesForPayloadKind(pk)
			select mt.MediaType.ToText()).ToArray<string>());
			throw new ODataContentTypeException(Strings.MediaTypeUtils_CannotDetermineFormatFromContentType(p, contentTypeName));
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0004A3E4 File Offset: 0x000485E4
		private static MediaType ParseContentType(string contentTypeHeader, out string charset)
		{
			IList<KeyValuePair<MediaType, string>> list = HttpUtils.MediaTypesFromString(contentTypeHeader);
			if (list.Count != 1)
			{
				throw new ODataContentTypeException(Strings.MediaTypeUtils_NoOrMoreThanOneContentTypeSpecified(contentTypeHeader));
			}
			MediaType key = list[0].Key;
			MediaTypeUtils.CheckMediaTypeForWildCards(key);
			charset = list[0].Value;
			return key;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004A438 File Offset: 0x00048638
		private static MediaType GetDefaultMediaType(IList<MediaTypeWithFormat> supportedMediaTypes, ODataFormat specifiedFormat, out ODataFormat actualFormat)
		{
			for (int i = 0; i < supportedMediaTypes.Count; i++)
			{
				MediaTypeWithFormat mediaTypeWithFormat = supportedMediaTypes[i];
				if (specifiedFormat == null || mediaTypeWithFormat.Format == specifiedFormat)
				{
					actualFormat = mediaTypeWithFormat.Format;
					return mediaTypeWithFormat.MediaType;
				}
			}
			throw new ODataException(Strings.ODataUtils_DidNotFindDefaultMediaType(specifiedFormat));
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0004A484 File Offset: 0x00048684
		private static Encoding GetEncoding(string acceptCharsetHeader, ODataPayloadKind payloadKind, MediaType mediaType, bool useDefaultEncoding)
		{
			if (payloadKind == ODataPayloadKind.BinaryValue)
			{
				return null;
			}
			return HttpUtils.EncodingFromAcceptableCharsets(acceptCharsetHeader, mediaType, MediaTypeUtils.encodingUtf8NoPreamble, useDefaultEncoding ? MediaTypeUtils.encodingUtf8NoPreamble : null);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004A4A4 File Offset: 0x000486A4
		private static MediaTypeUtils.MediaTypeMatchInfo MatchMediaTypes(IEnumerable<MediaType> sourceTypes, MediaType[] targetTypes)
		{
			MediaTypeUtils.MediaTypeMatchInfo mediaTypeMatchInfo = null;
			int num = 0;
			if (sourceTypes != null)
			{
				foreach (MediaType sourceType in sourceTypes)
				{
					int num2 = 0;
					foreach (MediaType targetType in targetTypes)
					{
						MediaTypeUtils.MediaTypeMatchInfo mediaTypeMatchInfo2 = new MediaTypeUtils.MediaTypeMatchInfo(sourceType, targetType, num, num2);
						if (!mediaTypeMatchInfo2.IsMatch)
						{
							num2++;
						}
						else
						{
							if (mediaTypeMatchInfo == null)
							{
								mediaTypeMatchInfo = mediaTypeMatchInfo2;
							}
							else
							{
								int num3 = mediaTypeMatchInfo.CompareTo(mediaTypeMatchInfo2);
								if (num3 < 0)
								{
									mediaTypeMatchInfo = mediaTypeMatchInfo2;
								}
							}
							num2++;
						}
					}
					num++;
				}
			}
			if (mediaTypeMatchInfo == null)
			{
				return null;
			}
			return mediaTypeMatchInfo;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0004A56C File Offset: 0x0004876C
		private static void ConvertApplicationJsonInAcceptableMediaTypes(IList<KeyValuePair<MediaType, string>> specifiedTypes)
		{
			if (specifiedTypes == null)
			{
				return;
			}
			for (int i = 0; i < specifiedTypes.Count; i++)
			{
				MediaType key = specifiedTypes[i].Key;
				if (HttpUtils.CompareMediaTypeNames(key.SubTypeName, "json") && HttpUtils.CompareMediaTypeNames(key.TypeName, "application"))
				{
					if (key.Parameters != null)
					{
						if (key.Parameters.Any((KeyValuePair<string, string> p) => HttpUtils.CompareMediaTypeParameterNames(p.Key, "odata")))
						{
							goto IL_E6;
						}
					}
					IList<KeyValuePair<string, string>> parameters = key.Parameters;
					int capacity = (parameters == null) ? 1 : (parameters.Count + 1);
					List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(capacity);
					list.Add(new KeyValuePair<string, string>("odata", "minimalmetadata"));
					if (parameters != null)
					{
						list.AddRange(parameters);
					}
					specifiedTypes[i] = new KeyValuePair<MediaType, string>(new MediaType(key.TypeName, key.SubTypeName, list), specifiedTypes[i].Value);
				}
				IL_E6:;
			}
		}

		// Token: 0x04000702 RID: 1794
		private static readonly ODataPayloadKind[] allSupportedPayloadKinds = new ODataPayloadKind[]
		{
			ODataPayloadKind.Feed,
			ODataPayloadKind.Entry,
			ODataPayloadKind.Property,
			ODataPayloadKind.MetadataDocument,
			ODataPayloadKind.ServiceDocument,
			ODataPayloadKind.Value,
			ODataPayloadKind.BinaryValue,
			ODataPayloadKind.Collection,
			ODataPayloadKind.EntityReferenceLinks,
			ODataPayloadKind.EntityReferenceLink,
			ODataPayloadKind.Batch,
			ODataPayloadKind.Error,
			ODataPayloadKind.Parameter
		};

		// Token: 0x04000703 RID: 1795
		private static readonly UTF8Encoding encodingUtf8NoPreamble = new UTF8Encoding(false, true);

		// Token: 0x02000258 RID: 600
		private sealed class MediaTypeMatchInfo : IComparable<MediaTypeUtils.MediaTypeMatchInfo>
		{
			// Token: 0x060013CD RID: 5069 RVA: 0x0004A6CF File Offset: 0x000488CF
			public MediaTypeMatchInfo(MediaType sourceType, MediaType targetType, int sourceIndex, int targetIndex)
			{
				this.sourceIndex = sourceIndex;
				this.targetIndex = targetIndex;
				this.MatchTypes(sourceType, targetType);
			}

			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x060013CE RID: 5070 RVA: 0x0004A6EE File Offset: 0x000488EE
			public int SourceTypeIndex
			{
				get
				{
					return this.sourceIndex;
				}
			}

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x060013CF RID: 5071 RVA: 0x0004A6F6 File Offset: 0x000488F6
			public int TargetTypeIndex
			{
				get
				{
					return this.targetIndex;
				}
			}

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x060013D0 RID: 5072 RVA: 0x0004A6FE File Offset: 0x000488FE
			// (set) Token: 0x060013D1 RID: 5073 RVA: 0x0004A706 File Offset: 0x00048906
			public int MatchingTypeNamePartCount { get; private set; }

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0004A70F File Offset: 0x0004890F
			// (set) Token: 0x060013D3 RID: 5075 RVA: 0x0004A717 File Offset: 0x00048917
			public int MatchingParameterCount { get; private set; }

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x060013D4 RID: 5076 RVA: 0x0004A720 File Offset: 0x00048920
			// (set) Token: 0x060013D5 RID: 5077 RVA: 0x0004A728 File Offset: 0x00048928
			public int QualityValue { get; private set; }

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0004A731 File Offset: 0x00048931
			// (set) Token: 0x060013D7 RID: 5079 RVA: 0x0004A739 File Offset: 0x00048939
			public int SourceTypeParameterCountForMatching { get; private set; }

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0004A742 File Offset: 0x00048942
			public bool IsMatch
			{
				get
				{
					return this.QualityValue != 0 && this.MatchingTypeNamePartCount >= 0 && (this.MatchingTypeNamePartCount <= 1 || this.MatchingParameterCount == -1 || this.MatchingParameterCount >= this.SourceTypeParameterCountForMatching);
				}
			}

			// Token: 0x060013D9 RID: 5081 RVA: 0x0004A77C File Offset: 0x0004897C
			public int CompareTo(MediaTypeUtils.MediaTypeMatchInfo other)
			{
				ExceptionUtils.CheckArgumentNotNull<MediaTypeUtils.MediaTypeMatchInfo>(other, "other");
				if (this.MatchingTypeNamePartCount > other.MatchingTypeNamePartCount)
				{
					return 1;
				}
				if (this.MatchingTypeNamePartCount == other.MatchingTypeNamePartCount)
				{
					if (this.MatchingParameterCount > other.MatchingParameterCount)
					{
						return 1;
					}
					if (this.MatchingParameterCount == other.MatchingParameterCount)
					{
						int num = this.QualityValue.CompareTo(other.QualityValue);
						if (num != 0)
						{
							return num;
						}
						if (other.TargetTypeIndex >= this.TargetTypeIndex)
						{
							return 1;
						}
						return -1;
					}
				}
				return -1;
			}

			// Token: 0x060013DA RID: 5082 RVA: 0x0004A800 File Offset: 0x00048A00
			private static int ParseQualityValue(string qualityValueText)
			{
				int result = 1000;
				if (qualityValueText.Length > 0)
				{
					int num = 0;
					HttpUtils.ReadQualityValue(qualityValueText, ref num, out result);
				}
				return result;
			}

			// Token: 0x060013DB RID: 5083 RVA: 0x0004A82C File Offset: 0x00048A2C
			private static bool TryFindMediaTypeParameter(IList<KeyValuePair<string, string>> parameters, string parameterName, out string parameterValue)
			{
				parameterValue = null;
				if (parameters != null)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						string key = parameters[i].Key;
						if (HttpUtils.CompareMediaTypeParameterNames(parameterName, key))
						{
							parameterValue = parameters[i].Value;
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x0004A87D File Offset: 0x00048A7D
			private static bool IsQualityValueParameter(string parameterName)
			{
				return HttpUtils.CompareMediaTypeParameterNames("q", parameterName);
			}

			// Token: 0x060013DD RID: 5085 RVA: 0x0004A88C File Offset: 0x00048A8C
			private void MatchTypes(MediaType sourceType, MediaType targetType)
			{
				this.MatchingTypeNamePartCount = -1;
				if (sourceType.TypeName == "*")
				{
					this.MatchingTypeNamePartCount = 0;
				}
				else if (HttpUtils.CompareMediaTypeNames(sourceType.TypeName, targetType.TypeName))
				{
					if (sourceType.SubTypeName == "*")
					{
						this.MatchingTypeNamePartCount = 1;
					}
					else if (HttpUtils.CompareMediaTypeNames(sourceType.SubTypeName, targetType.SubTypeName))
					{
						this.MatchingTypeNamePartCount = 2;
					}
				}
				this.QualityValue = 1000;
				this.SourceTypeParameterCountForMatching = 0;
				this.MatchingParameterCount = 0;
				IList<KeyValuePair<string, string>> parameters = sourceType.Parameters;
				IList<KeyValuePair<string, string>> parameters2 = targetType.Parameters;
				bool flag = parameters2 != null && parameters2.Count > 0;
				bool flag2 = parameters != null && parameters.Count > 0;
				if (flag2)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						string key = parameters[i].Key;
						if (MediaTypeUtils.MediaTypeMatchInfo.IsQualityValueParameter(key))
						{
							this.QualityValue = MediaTypeUtils.MediaTypeMatchInfo.ParseQualityValue(parameters[i].Value.Trim());
							break;
						}
						this.SourceTypeParameterCountForMatching = i + 1;
						string text;
						if (flag && MediaTypeUtils.MediaTypeMatchInfo.TryFindMediaTypeParameter(parameters2, key, out text) && string.Compare(parameters[i].Value.Trim(), text.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.MatchingParameterCount++;
						}
					}
				}
				if (!flag2 || this.SourceTypeParameterCountForMatching == 0 || this.MatchingParameterCount == this.SourceTypeParameterCountForMatching)
				{
					this.MatchingParameterCount = -1;
				}
			}

			// Token: 0x0400070B RID: 1803
			private const int DefaultQualityValue = 1000;

			// Token: 0x0400070C RID: 1804
			private readonly int sourceIndex;

			// Token: 0x0400070D RID: 1805
			private readonly int targetIndex;
		}
	}
}
