using System;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000234 RID: 564
	internal sealed class ODataVerboseJsonErrorDeserializer : ODataVerboseJsonDeserializer
	{
		// Token: 0x060011FF RID: 4607 RVA: 0x00043A41 File Offset: 0x00041C41
		internal ODataVerboseJsonErrorDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00043A4C File Offset: 0x00041C4C
		internal ODataError ReadTopLevelError()
		{
			base.JsonReader.DisableInStreamErrorDetection = true;
			ODataError odataError = new ODataError();
			try
			{
				base.ReadPayloadStart(false, false);
				base.JsonReader.ReadStartObject();
				ODataVerboseJsonReaderUtils.ErrorPropertyBitMask errorPropertyBitMask = ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string text = base.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal("error", text) != 0)
					{
						throw new ODataException(Strings.ODataJsonErrorDeserializer_TopLevelErrorWithInvalidProperty(text));
					}
					ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.Error, "error");
					base.JsonReader.ReadStartObject();
					while (base.JsonReader.NodeType == JsonNodeType.Property)
					{
						text = base.JsonReader.ReadPropertyName();
						string a;
						if ((a = text) != null)
						{
							if (a == "code")
							{
								ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.Code, "code");
								odataError.ErrorCode = base.JsonReader.ReadStringValue("code");
								continue;
							}
							if (a == "message")
							{
								ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.Message, "message");
								base.JsonReader.ReadStartObject();
								while (base.JsonReader.NodeType == JsonNodeType.Property)
								{
									text = base.JsonReader.ReadPropertyName();
									string a2;
									if ((a2 = text) != null)
									{
										if (a2 == "lang")
										{
											ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageLanguage, "lang");
											odataError.MessageLanguage = base.JsonReader.ReadStringValue("lang");
											continue;
										}
										if (a2 == "value")
										{
											ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageValue, "value");
											odataError.Message = base.JsonReader.ReadStringValue("value");
											continue;
										}
									}
									throw new ODataException(Strings.ODataJsonErrorDeserializer_TopLevelErrorMessageValueWithInvalidProperty(text));
								}
								base.JsonReader.ReadEndObject();
								continue;
							}
							if (a == "innererror")
							{
								ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.InnerError, "innererror");
								odataError.InnerError = this.ReadInnerError(0);
								continue;
							}
						}
						throw new ODataException(Strings.ODataVerboseJsonErrorDeserializer_TopLevelErrorValueWithInvalidProperty(text));
					}
					base.JsonReader.ReadEndObject();
				}
				base.JsonReader.ReadEndObject();
				base.ReadPayloadEnd(false, false);
			}
			finally
			{
				base.JsonReader.DisableInStreamErrorDetection = false;
			}
			return odataError;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00043C8C File Offset: 0x00041E8C
		private ODataInnerError ReadInnerError(int recursionDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
			base.JsonReader.ReadStartObject();
			ODataInnerError odataInnerError = new ODataInnerError();
			ODataVerboseJsonReaderUtils.ErrorPropertyBitMask errorPropertyBitMask = ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				string a;
				if ((a = text) != null)
				{
					if (a == "message")
					{
						ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageValue, "message");
						odataInnerError.Message = base.JsonReader.ReadStringValue("message");
						continue;
					}
					if (a == "type")
					{
						ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.TypeName, "type");
						odataInnerError.TypeName = base.JsonReader.ReadStringValue("type");
						continue;
					}
					if (a == "stacktrace")
					{
						ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.StackTrace, "stacktrace");
						odataInnerError.StackTrace = base.JsonReader.ReadStringValue("stacktrace");
						continue;
					}
					if (a == "internalexception")
					{
						ODataVerboseJsonReaderUtils.VerifyErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.InnerError, "internalexception");
						odataInnerError.InnerError = this.ReadInnerError(recursionDepth);
						continue;
					}
				}
				base.JsonReader.SkipValue();
			}
			base.JsonReader.ReadEndObject();
			return odataInnerError;
		}
	}
}
