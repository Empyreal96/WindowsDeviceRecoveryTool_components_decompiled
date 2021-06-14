using System;
using System.Data.Services.Common;
using System.Text;
using Microsoft.Data.Edm.Values;

namespace System.Data.Services.Client
{
	// Token: 0x02000034 RID: 52
	internal sealed class ConventionalODataEntityMetadataBuilder : ODataEntityMetadataBuilder
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00008EE4 File Offset: 0x000070E4
		internal ConventionalODataEntityMetadataBuilder(Uri baseUri, string entitySetName, IEdmStructuredValue entityInstance, DataServiceUrlConventions conventions) : this(UriResolver.CreateFromBaseUri(baseUri, "baseUri"), entitySetName, entityInstance, conventions)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008EFC File Offset: 0x000070FC
		internal ConventionalODataEntityMetadataBuilder(UriResolver resolver, string entitySetName, IEdmStructuredValue entityInstance, DataServiceUrlConventions conventions)
		{
			Util.CheckArgumentNullAndEmpty(entitySetName, "entitySetName");
			Util.CheckArgumentNull<IEdmStructuredValue>(entityInstance, "entityInstance");
			Util.CheckArgumentNull<DataServiceUrlConventions>(conventions, "conventions");
			this.entitySetName = entitySetName;
			this.entityInstance = entityInstance;
			this.uriBuilder = new ConventionalODataEntityMetadataBuilder.ConventionalODataUriBuilder(resolver, conventions);
			this.baseUri = resolver.BaseUriOrNull;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008F5C File Offset: 0x0000715C
		internal override Uri GetEditLink()
		{
			Uri uri = this.uriBuilder.BuildEntitySetUri(this.baseUri, this.entitySetName);
			return this.uriBuilder.BuildEntityInstanceUri(uri, this.entityInstance);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008F95 File Offset: 0x00007195
		internal override string GetId()
		{
			return this.GetEditLink().AbsoluteUri;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008FA2 File Offset: 0x000071A2
		internal override string GetETag()
		{
			return null;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008FA5 File Offset: 0x000071A5
		internal override Uri GetReadLink()
		{
			return null;
		}

		// Token: 0x040001F8 RID: 504
		private readonly IEdmStructuredValue entityInstance;

		// Token: 0x040001F9 RID: 505
		private readonly string entitySetName;

		// Token: 0x040001FA RID: 506
		private readonly Uri baseUri;

		// Token: 0x040001FB RID: 507
		private readonly ConventionalODataEntityMetadataBuilder.ConventionalODataUriBuilder uriBuilder;

		// Token: 0x02000035 RID: 53
		private class ConventionalODataUriBuilder : ODataUriBuilder
		{
			// Token: 0x0600018E RID: 398 RVA: 0x00008FA8 File Offset: 0x000071A8
			internal ConventionalODataUriBuilder(UriResolver resolver, DataServiceUrlConventions conventions)
			{
				this.resolver = resolver;
				this.conventions = conventions;
			}

			// Token: 0x0600018F RID: 399 RVA: 0x00008FBE File Offset: 0x000071BE
			internal override Uri BuildEntitySetUri(Uri baseUri, string entitySetName)
			{
				return this.resolver.GetEntitySetUri(entitySetName);
			}

			// Token: 0x06000190 RID: 400 RVA: 0x00008FCC File Offset: 0x000071CC
			internal override Uri BuildEntityInstanceUri(Uri baseUri, IEdmStructuredValue entityInstance)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (baseUri != null)
				{
					stringBuilder.Append(UriUtil.UriToString(baseUri));
				}
				this.conventions.AppendKeyExpression(entityInstance, stringBuilder);
				return UriUtil.CreateUri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
			}

			// Token: 0x040001FC RID: 508
			private readonly UriResolver resolver;

			// Token: 0x040001FD RID: 509
			private readonly DataServiceUrlConventions conventions;
		}
	}
}
