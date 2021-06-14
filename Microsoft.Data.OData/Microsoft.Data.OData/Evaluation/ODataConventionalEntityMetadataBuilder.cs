using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000136 RID: 310
	internal sealed class ODataConventionalEntityMetadataBuilder : ODataEntityMetadataBuilder
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x0001AD08 File Offset: 0x00018F08
		internal ODataConventionalEntityMetadataBuilder(IODataEntryMetadataContext entryMetadataContext, IODataMetadataContext metadataContext, ODataUriBuilder uriBuilder)
		{
			this.entryMetadataContext = entryMetadataContext;
			this.uriBuilder = uriBuilder;
			this.metadataContext = metadataContext;
			this.processedNavigationLinks = new HashSet<string>(StringComparer.Ordinal);
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001AD35 File Offset: 0x00018F35
		private string ComputedId
		{
			get
			{
				this.ComputeAndCacheId();
				return this.computedId;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001AD43 File Offset: 0x00018F43
		private Uri ComputedEntityInstanceUri
		{
			get
			{
				this.ComputeAndCacheId();
				return this.computedEntityInstanceUri;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x0001AD54 File Offset: 0x00018F54
		private ODataMissingOperationGenerator MissingOperationGenerator
		{
			get
			{
				ODataMissingOperationGenerator result;
				if ((result = this.missingOperationGenerator) == null)
				{
					result = (this.missingOperationGenerator = new ODataMissingOperationGenerator(this.entryMetadataContext, this.metadataContext));
				}
				return result;
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001AD88 File Offset: 0x00018F88
		internal override Uri GetEditLink()
		{
			Uri nonComputedEditLink;
			if (!this.entryMetadataContext.Entry.HasNonComputedEditLink)
			{
				if ((nonComputedEditLink = this.computedEditLink) == null)
				{
					return this.computedEditLink = this.ComputeEditLink();
				}
			}
			else
			{
				nonComputedEditLink = this.entryMetadataContext.Entry.NonComputedEditLink;
			}
			return nonComputedEditLink;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001ADD4 File Offset: 0x00018FD4
		internal override Uri GetReadLink()
		{
			Uri nonComputedReadLink;
			if (!this.entryMetadataContext.Entry.HasNonComputedReadLink)
			{
				if ((nonComputedReadLink = this.computedReadLink) == null)
				{
					return this.computedReadLink = this.GetEditLink();
				}
			}
			else
			{
				nonComputedReadLink = this.entryMetadataContext.Entry.NonComputedReadLink;
			}
			return nonComputedReadLink;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001AE20 File Offset: 0x00019020
		internal override string GetId()
		{
			if (this.entryMetadataContext.Entry.HasNonComputedId)
			{
				return this.entryMetadataContext.Entry.NonComputedId;
			}
			if (this.entryMetadataContext.Entry.HasNonComputedReadLink)
			{
				return UriUtilsCommon.UriToString(this.entryMetadataContext.Entry.NonComputedReadLink);
			}
			if (this.entryMetadataContext.Entry.NonComputedEditLink != null)
			{
				return UriUtilsCommon.UriToString(this.entryMetadataContext.Entry.NonComputedEditLink);
			}
			return this.ComputedId;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001AEAC File Offset: 0x000190AC
		internal override string GetETag()
		{
			if (this.entryMetadataContext.Entry.HasNonComputedETag)
			{
				return this.entryMetadataContext.Entry.NonComputedETag;
			}
			if (!this.etagComputed)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<string, object> keyValuePair in this.entryMetadataContext.ETagProperties)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(',');
					}
					else
					{
						stringBuilder.Append("W/\"");
					}
					string value;
					if (keyValuePair.Value == null)
					{
						value = "null";
					}
					else
					{
						value = LiteralFormatter.ForConstants.Format(keyValuePair.Value);
					}
					stringBuilder.Append(value);
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('"');
					this.computedETag = stringBuilder.ToString();
				}
				this.etagComputed = true;
			}
			return this.computedETag;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001AFA4 File Offset: 0x000191A4
		internal override ODataStreamReferenceValue GetMediaResource()
		{
			if (this.entryMetadataContext.Entry.NonComputedMediaResource != null)
			{
				return this.entryMetadataContext.Entry.NonComputedMediaResource;
			}
			if (this.computedMediaResource == null && this.entryMetadataContext.TypeContext.IsMediaLinkEntry)
			{
				this.computedMediaResource = new ODataStreamReferenceValue();
				this.computedMediaResource.SetMetadataBuilder(this, null);
			}
			return this.computedMediaResource;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001B00C File Offset: 0x0001920C
		internal override IEnumerable<ODataProperty> GetProperties(IEnumerable<ODataProperty> nonComputedProperties)
		{
			return ODataUtilsInternal.ConcatEnumerables<ODataProperty>(nonComputedProperties, this.GetComputedStreamProperties(nonComputedProperties));
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001B01B File Offset: 0x0001921B
		internal override IEnumerable<ODataAction> GetActions()
		{
			return ODataUtilsInternal.ConcatEnumerables<ODataAction>(this.entryMetadataContext.Entry.NonComputedActions, this.MissingOperationGenerator.GetComputedActions());
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001B03D File Offset: 0x0001923D
		internal override IEnumerable<ODataFunction> GetFunctions()
		{
			return ODataUtilsInternal.ConcatEnumerables<ODataFunction>(this.entryMetadataContext.Entry.NonComputedFunctions, this.MissingOperationGenerator.GetComputedFunctions());
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001B05F File Offset: 0x0001925F
		internal override void MarkNavigationLinkProcessed(string navigationPropertyName)
		{
			this.processedNavigationLinks.Add(navigationPropertyName);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001B084 File Offset: 0x00019284
		internal override ODataJsonLightReaderNavigationLinkInfo GetNextUnprocessedNavigationLink()
		{
			if (this.unprocessedNavigationLinks == null)
			{
				this.unprocessedNavigationLinks = (from p in this.entryMetadataContext.SelectedNavigationProperties
				where !this.processedNavigationLinks.Contains(p.Name)
				select p).Select(new Func<IEdmNavigationProperty, ODataJsonLightReaderNavigationLinkInfo>(ODataJsonLightReaderNavigationLinkInfo.CreateProjectedNavigationLinkInfo)).GetEnumerator();
			}
			if (this.unprocessedNavigationLinks.MoveNext())
			{
				return this.unprocessedNavigationLinks.Current;
			}
			return null;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001B0F2 File Offset: 0x000192F2
		internal override Uri GetStreamEditLink(string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return this.uriBuilder.BuildStreamEditLinkUri(this.GetEditLink(), streamPropertyName);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001B111 File Offset: 0x00019311
		internal override Uri GetStreamReadLink(string streamPropertyName)
		{
			ExceptionUtils.CheckArgumentStringNotEmpty(streamPropertyName, "streamPropertyName");
			return this.uriBuilder.BuildStreamReadLinkUri(this.GetReadLink(), streamPropertyName);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001B130 File Offset: 0x00019330
		internal override Uri GetNavigationLinkUri(string navigationPropertyName, Uri navigationLinkUrl, bool hasNavigationLinkUrl)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			if (!hasNavigationLinkUrl)
			{
				return this.uriBuilder.BuildNavigationLinkUri(this.GetEditLink(), navigationPropertyName);
			}
			return navigationLinkUrl;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001B154 File Offset: 0x00019354
		internal override Uri GetAssociationLinkUri(string navigationPropertyName, Uri associationLinkUrl, bool hasAssociationLinkUrl)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(navigationPropertyName, "navigationPropertyName");
			if (!hasAssociationLinkUrl)
			{
				return this.uriBuilder.BuildAssociationLinkUri(this.GetEditLink(), navigationPropertyName);
			}
			return associationLinkUrl;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001B178 File Offset: 0x00019378
		internal override Uri GetOperationTargetUri(string operationName, string bindingParameterTypeName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			Uri editLink;
			if (string.IsNullOrEmpty(bindingParameterTypeName) || this.entryMetadataContext.Entry.NonComputedEditLink != null)
			{
				editLink = this.GetEditLink();
			}
			else
			{
				editLink = this.ComputedEntityInstanceUri;
			}
			return this.uriBuilder.BuildOperationTargetUri(editLink, operationName, bindingParameterTypeName);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001B1CE File Offset: 0x000193CE
		internal override string GetOperationTitle(string operationName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(operationName, "operationName");
			return operationName;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001B1DC File Offset: 0x000193DC
		private Uri ComputeEditLink()
		{
			Uri uri = this.ComputedEntityInstanceUri;
			if (this.entryMetadataContext.ActualEntityTypeName != this.entryMetadataContext.TypeContext.EntitySetElementTypeName)
			{
				uri = this.uriBuilder.AppendTypeSegment(uri, this.entryMetadataContext.ActualEntityTypeName);
			}
			return uri;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001B22C File Offset: 0x0001942C
		private void ComputeAndCacheId()
		{
			if (this.computedEntityInstanceUri == null)
			{
				Uri uri = this.uriBuilder.BuildBaseUri();
				uri = this.uriBuilder.BuildEntitySetUri(uri, this.entryMetadataContext.TypeContext.EntitySetName);
				uri = this.uriBuilder.BuildEntityInstanceUri(uri, this.entryMetadataContext.KeyProperties, this.entryMetadataContext.ActualEntityTypeName);
				this.computedEntityInstanceUri = uri;
				this.computedId = UriUtilsCommon.UriToString(uri);
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001B2A8 File Offset: 0x000194A8
		private IEnumerable<ODataProperty> GetComputedStreamProperties(IEnumerable<ODataProperty> nonComputedProperties)
		{
			if (this.computedStreamProperties == null)
			{
				IDictionary<string, IEdmStructuralProperty> selectedStreamProperties = this.entryMetadataContext.SelectedStreamProperties;
				if (nonComputedProperties != null)
				{
					foreach (ODataProperty odataProperty in nonComputedProperties)
					{
						selectedStreamProperties.Remove(odataProperty.Name);
					}
				}
				this.computedStreamProperties = new List<ODataProperty>();
				if (selectedStreamProperties.Count > 0)
				{
					foreach (string text in selectedStreamProperties.Keys)
					{
						ODataStreamReferenceValue odataStreamReferenceValue = new ODataStreamReferenceValue();
						odataStreamReferenceValue.SetMetadataBuilder(this, text);
						this.computedStreamProperties.Add(new ODataProperty
						{
							Name = text,
							Value = odataStreamReferenceValue
						});
					}
				}
			}
			return this.computedStreamProperties;
		}

		// Token: 0x0400031E RID: 798
		private readonly ODataUriBuilder uriBuilder;

		// Token: 0x0400031F RID: 799
		private readonly IODataEntryMetadataContext entryMetadataContext;

		// Token: 0x04000320 RID: 800
		private readonly IODataMetadataContext metadataContext;

		// Token: 0x04000321 RID: 801
		private readonly HashSet<string> processedNavigationLinks;

		// Token: 0x04000322 RID: 802
		private Uri computedEditLink;

		// Token: 0x04000323 RID: 803
		private Uri computedReadLink;

		// Token: 0x04000324 RID: 804
		private string computedETag;

		// Token: 0x04000325 RID: 805
		private bool etagComputed;

		// Token: 0x04000326 RID: 806
		private string computedId;

		// Token: 0x04000327 RID: 807
		private Uri computedEntityInstanceUri;

		// Token: 0x04000328 RID: 808
		private ODataStreamReferenceValue computedMediaResource;

		// Token: 0x04000329 RID: 809
		private List<ODataProperty> computedStreamProperties;

		// Token: 0x0400032A RID: 810
		private IEnumerator<ODataJsonLightReaderNavigationLinkInfo> unprocessedNavigationLinks;

		// Token: 0x0400032B RID: 811
		private ODataMissingOperationGenerator missingOperationGenerator;
	}
}
