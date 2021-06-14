using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000287 RID: 647
	internal static class ODataJsonWriterUtils
	{
		// Token: 0x0600157D RID: 5501 RVA: 0x0004E3B8 File Offset: 0x0004C5B8
		internal static void WriteError(IJsonWriter jsonWriter, Action<IEnumerable<ODataInstanceAnnotation>> writeInstanceAnnotationsDelegate, ODataError error, bool includeDebugInformation, int maxInnerErrorDepth, bool writingJsonLight)
		{
			string code;
			string message;
			string messageLanguage;
			ErrorUtils.GetErrorDetails(error, out code, out message, out messageLanguage);
			ODataInnerError innerError = includeDebugInformation ? error.InnerError : null;
			IEnumerable<ODataInstanceAnnotation> instanceAnnotationsForWriting = error.GetInstanceAnnotationsForWriting();
			ODataJsonWriterUtils.WriteError(jsonWriter, code, message, messageLanguage, innerError, instanceAnnotationsForWriting, writeInstanceAnnotationsDelegate, maxInnerErrorDepth, writingJsonLight);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0004E3F7 File Offset: 0x0004C5F7
		internal static void WriteMetadataWithTypeName(IJsonWriter jsonWriter, string typeName)
		{
			jsonWriter.WriteName("__metadata");
			jsonWriter.StartObjectScope();
			jsonWriter.WriteName("type");
			jsonWriter.WriteValue(typeName);
			jsonWriter.EndObjectScope();
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0004E422 File Offset: 0x0004C622
		internal static void StartJsonPaddingIfRequired(IJsonWriter jsonWriter, ODataMessageWriterSettings settings)
		{
			if (settings.HasJsonPaddingFunction())
			{
				jsonWriter.WritePaddingFunctionName(settings.JsonPCallback);
				jsonWriter.StartPaddingFunctionScope();
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0004E43E File Offset: 0x0004C63E
		internal static void EndJsonPaddingIfRequired(IJsonWriter jsonWriter, ODataMessageWriterSettings settings)
		{
			if (settings.HasJsonPaddingFunction())
			{
				jsonWriter.EndPaddingFunctionScope();
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0004E450 File Offset: 0x0004C650
		internal static string UriToUriString(ODataOutputContext outputContext, Uri uri, bool makeAbsolute)
		{
			Uri uri2;
			if (outputContext.UrlResolver != null)
			{
				uri2 = outputContext.UrlResolver.ResolveUrl(outputContext.MessageWriterSettings.BaseUri, uri);
				if (uri2 != null)
				{
					return UriUtilsCommon.UriToString(uri2);
				}
			}
			uri2 = uri;
			if (!uri2.IsAbsoluteUri)
			{
				if (makeAbsolute)
				{
					if (outputContext.MessageWriterSettings.BaseUri == null)
					{
						throw new ODataException(Strings.ODataWriter_RelativeUriUsedWithoutBaseUriSpecified(UriUtilsCommon.UriToString(uri)));
					}
					uri2 = UriUtils.UriToAbsoluteUri(outputContext.MessageWriterSettings.BaseUri, uri);
				}
				else
				{
					uri2 = UriUtils.EnsureEscapedRelativeUri(uri2);
				}
			}
			return UriUtilsCommon.UriToString(uri2);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0004E4E0 File Offset: 0x0004C6E0
		private static void WriteError(IJsonWriter jsonWriter, string code, string message, string messageLanguage, ODataInnerError innerError, IEnumerable<ODataInstanceAnnotation> instanceAnnotations, Action<IEnumerable<ODataInstanceAnnotation>> writeInstanceAnnotationsDelegate, int maxInnerErrorDepth, bool writingJsonLight)
		{
			jsonWriter.StartObjectScope();
			jsonWriter.WriteName(writingJsonLight ? "odata.error" : "error");
			jsonWriter.StartObjectScope();
			jsonWriter.WriteName("code");
			jsonWriter.WriteValue(code);
			jsonWriter.WriteName("message");
			jsonWriter.StartObjectScope();
			jsonWriter.WriteName("lang");
			jsonWriter.WriteValue(messageLanguage);
			jsonWriter.WriteName("value");
			jsonWriter.WriteValue(message);
			jsonWriter.EndObjectScope();
			if (innerError != null)
			{
				ODataJsonWriterUtils.WriteInnerError(jsonWriter, innerError, "innererror", 0, maxInnerErrorDepth);
			}
			if (writingJsonLight)
			{
				writeInstanceAnnotationsDelegate(instanceAnnotations);
			}
			jsonWriter.EndObjectScope();
			jsonWriter.EndObjectScope();
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0004E58C File Offset: 0x0004C78C
		private static void WriteInnerError(IJsonWriter jsonWriter, ODataInnerError innerError, string innerErrorPropertyName, int recursionDepth, int maxInnerErrorDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, maxInnerErrorDepth);
			jsonWriter.WriteName(innerErrorPropertyName);
			jsonWriter.StartObjectScope();
			jsonWriter.WriteName("message");
			jsonWriter.WriteValue(innerError.Message ?? string.Empty);
			jsonWriter.WriteName("type");
			jsonWriter.WriteValue(innerError.TypeName ?? string.Empty);
			jsonWriter.WriteName("stacktrace");
			jsonWriter.WriteValue(innerError.StackTrace ?? string.Empty);
			if (innerError.InnerError != null)
			{
				ODataJsonWriterUtils.WriteInnerError(jsonWriter, innerError.InnerError, "internalexception", recursionDepth, maxInnerErrorDepth);
			}
			jsonWriter.EndObjectScope();
		}
	}
}
