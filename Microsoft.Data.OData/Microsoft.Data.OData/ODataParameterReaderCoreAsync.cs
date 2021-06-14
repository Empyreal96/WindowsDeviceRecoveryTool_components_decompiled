using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x02000155 RID: 341
	internal abstract class ODataParameterReaderCoreAsync : ODataParameterReaderCore
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x0001D256 File Offset: 0x0001B456
		protected ODataParameterReaderCoreAsync(ODataInputContext inputContext, IEdmFunctionImport functionImport) : base(inputContext, functionImport)
		{
		}

		// Token: 0x0600093E RID: 2366
		protected abstract Task<bool> ReadAtStartImplementationAsync();

		// Token: 0x0600093F RID: 2367
		protected abstract Task<bool> ReadNextParameterImplementationAsync();

		// Token: 0x06000940 RID: 2368
		protected abstract Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference);

		// Token: 0x06000941 RID: 2369 RVA: 0x0001D260 File Offset: 0x0001B460
		protected override Task<bool> ReadAsynchronously()
		{
			switch (this.State)
			{
			case ODataParameterReaderState.Start:
				return this.ReadAtStartImplementationAsync();
			case ODataParameterReaderState.Value:
			case ODataParameterReaderState.Collection:
				base.OnParameterCompleted();
				return this.ReadNextParameterImplementationAsync();
			case ODataParameterReaderState.Exception:
			case ODataParameterReaderState.Completed:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterReaderCoreAsync_ReadAsynchronously));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterReaderCoreAsync_ReadAsynchronously));
			}
		}
	}
}
