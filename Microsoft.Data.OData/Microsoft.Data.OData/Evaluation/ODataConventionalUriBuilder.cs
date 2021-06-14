using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000138 RID: 312
	internal sealed class ODataConventionalUriBuilder : ODataUriBuilder
	{
		// Token: 0x06000851 RID: 2129 RVA: 0x0001B422 File Offset: 0x00019622
		internal ODataConventionalUriBuilder(Uri serviceBaseUri, UrlConvention urlConvention)
		{
			this.serviceBaseUri = serviceBaseUri;
			this.urlConvention = urlConvention;
			this.keySerializer = KeySerializer.Create(this.urlConvention);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001B449 File Offset: 0x00019649
		internal override Uri BuildBaseUri()
		{
			return this.serviceBaseUri;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001B451 File Offset: 0x00019651
		internal override Uri BuildEntitySetUri(Uri baseUri, string entitySetName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(entitySetName, "entitySetName");
			return ODataConventionalUriBuilder.AppendSegment(baseUri, entitySetName, true);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001B468 File Offset: 0x00019668
		internal override Uri BuildEntityInstanceUri(Uri baseUri, ICollection<KeyValuePair<string, object>> keyProperties, string entityTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder(UriUtilsCommon.UriToString(baseUri));
			this.AppendKeyExpression(stringBuilder, keyProperties, entityTypeName);
			return new Uri(stringBuilder.ToString(), UriKind.Absolute);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001B496 File Offset: 0x00019696
		internal override Uri BuildStreamEditLinkUri(Uri baseUri, string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			if (streamPropertyName == null)
			{
				return ODataConventionalUriBuilder.AppendSegment(baseUri, "$value", false);
			}
			return ODataConventionalUriBuilder.AppendSegment(baseUri, streamPropertyName, true);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001B4BB File Offset: 0x000196BB
		internal override Uri BuildStreamReadLinkUri(Uri baseUri, string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			if (streamPropertyName == null)
			{
				return ODataConventionalUriBuilder.AppendSegment(baseUri, "$value", false);
			}
			return ODataConventionalUriBuilder.AppendSegment(baseUri, streamPropertyName, true);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001B4E0 File Offset: 0x000196E0
		internal override Uri BuildNavigationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			return ODataConventionalUriBuilder.AppendSegment(baseUri, navigationPropertyName, true);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001B4F8 File Offset: 0x000196F8
		internal override Uri BuildAssociationLinkUri(Uri baseUri, string navigationPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			Uri baseUri2 = ODataConventionalUriBuilder.AppendSegment(baseUri, "$links/", false);
			return ODataConventionalUriBuilder.AppendSegment(baseUri2, navigationPropertyName, true);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001B528 File Offset: 0x00019728
		internal override Uri BuildOperationTargetUri(Uri baseUri, string operationName, string bindingParameterTypeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			if (!string.IsNullOrEmpty(bindingParameterTypeName))
			{
				Uri baseUri2 = ODataConventionalUriBuilder.AppendSegment(baseUri, bindingParameterTypeName, true);
				return ODataConventionalUriBuilder.AppendSegment(baseUri2, operationName, true);
			}
			return ODataConventionalUriBuilder.AppendSegment(baseUri, operationName, true);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001B562 File Offset: 0x00019762
		internal override Uri AppendTypeSegment(Uri baseUri, string typeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(typeName, "typeName");
			return ODataConventionalUriBuilder.AppendSegment(baseUri, typeName, true);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001B577 File Offset: 0x00019777
		[Conditional("DEBUG")]
		private static void ValidateBaseUri(Uri baseUri)
		{
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001B57C File Offset: 0x0001977C
		private static Uri AppendSegment(Uri baseUri, string segment, bool escapeSegment)
		{
			string text = UriUtilsCommon.UriToString(baseUri);
			if (escapeSegment)
			{
				segment = Uri.EscapeDataString(segment);
			}
			if (text[text.Length - 1] != '/')
			{
				return new Uri(text + "/" + segment, UriKind.Absolute);
			}
			return new Uri(baseUri, segment);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001B5C7 File Offset: 0x000197C7
		private static object ValidateKeyValue(string keyPropertyName, object keyPropertyValue, string entityTypeName)
		{
			if (keyPropertyValue == null)
			{
				throw new ODataException(Strings.ODataConventionalUriBuilder_NullKeyValue(keyPropertyName, entityTypeName));
			}
			return keyPropertyValue;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001B608 File Offset: 0x00019808
		private void AppendKeyExpression(StringBuilder builder, ICollection<KeyValuePair<string, object>> keyProperties, string entityTypeName)
		{
			if (!keyProperties.Any<KeyValuePair<string, object>>())
			{
				throw new ODataException(Strings.ODataConventionalUriBuilder_EntityTypeWithNoKeyProperties(entityTypeName));
			}
			this.keySerializer.AppendKeyExpression<KeyValuePair<string, object>>(builder, keyProperties, (KeyValuePair<string, object> p) => p.Key, (KeyValuePair<string, object> p) => ODataConventionalUriBuilder.ValidateKeyValue(p.Key, p.Value, entityTypeName));
		}

		// Token: 0x0400032C RID: 812
		private readonly Uri serviceBaseUri;

		// Token: 0x0400032D RID: 813
		private readonly UrlConvention urlConvention;

		// Token: 0x0400032E RID: 814
		private readonly KeySerializer keySerializer;
	}
}
