using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000194 RID: 404
	internal sealed class ODataJsonLightParameterReader : ODataParameterReaderCoreAsync
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x00029929 File Offset: 0x00027B29
		internal ODataJsonLightParameterReader(ODataJsonLightInputContext jsonLightInputContext, IEdmFunctionImport functionImport) : base(jsonLightInputContext, functionImport)
		{
			this.jsonLightInputContext = jsonLightInputContext;
			this.jsonLightParameterDeserializer = new ODataJsonLightParameterDeserializer(this, jsonLightInputContext);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00029947 File Offset: 0x00027B47
		protected override bool ReadAtStartImplementation()
		{
			this.duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			this.jsonLightParameterDeserializer.ReadPayloadStart(ODataPayloadKind.Parameter, this.duplicatePropertyNamesChecker, false, true);
			return this.ReadAtStartImplementationSynchronously();
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002997D File Offset: 0x00027B7D
		protected override Task<bool> ReadAtStartImplementationAsync()
		{
			this.duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			return this.jsonLightParameterDeserializer.ReadPayloadStartAsync(ODataPayloadKind.Parameter, this.duplicatePropertyNamesChecker, false, true).FollowOnSuccessWith((Task t) => this.ReadAtStartImplementationSynchronously());
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000299B6 File Offset: 0x00027BB6
		protected override bool ReadNextParameterImplementation()
		{
			return this.ReadNextParameterImplementationSynchronously();
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000299BE File Offset: 0x00027BBE
		protected override Task<bool> ReadNextParameterImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadNextParameterImplementationSynchronously));
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000299D1 File Offset: 0x00027BD1
		protected override ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			return this.CreateCollectionReaderSynchronously(expectedItemTypeReference);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000299F8 File Offset: 0x00027BF8
		protected override Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionReader>(() => this.CreateCollectionReaderSynchronously(expectedItemTypeReference));
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00029A2A File Offset: 0x00027C2A
		private bool ReadAtStartImplementationSynchronously()
		{
			if (this.jsonLightInputContext.JsonReader.NodeType == JsonNodeType.EndOfInput)
			{
				base.PopScope(ODataParameterReaderState.Start);
				base.EnterScope(ODataParameterReaderState.Completed, null, null);
				return false;
			}
			return this.jsonLightParameterDeserializer.ReadNextParameter(this.duplicatePropertyNamesChecker);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00029A62 File Offset: 0x00027C62
		private bool ReadNextParameterImplementationSynchronously()
		{
			base.PopScope(this.State);
			return this.jsonLightParameterDeserializer.ReadNextParameter(this.duplicatePropertyNamesChecker);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00029A81 File Offset: 0x00027C81
		private ODataCollectionReader CreateCollectionReaderSynchronously(IEdmTypeReference expectedItemTypeReference)
		{
			return new ODataJsonLightCollectionReader(this.jsonLightInputContext, expectedItemTypeReference, this);
		}

		// Token: 0x0400042A RID: 1066
		private readonly ODataJsonLightInputContext jsonLightInputContext;

		// Token: 0x0400042B RID: 1067
		private readonly ODataJsonLightParameterDeserializer jsonLightParameterDeserializer;

		// Token: 0x0400042C RID: 1068
		private DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;
	}
}
