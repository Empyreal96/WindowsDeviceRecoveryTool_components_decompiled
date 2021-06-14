using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x0200014C RID: 332
	internal abstract class ODataCollectionReaderCoreAsync : ODataCollectionReaderCore
	{
		// Token: 0x06000904 RID: 2308 RVA: 0x0001CB65 File Offset: 0x0001AD65
		protected ODataCollectionReaderCoreAsync(ODataInputContext inputContext, IEdmTypeReference expectedItemTypeReference, IODataReaderWriterListener listener) : base(inputContext, expectedItemTypeReference, listener)
		{
		}

		// Token: 0x06000905 RID: 2309
		protected abstract Task<bool> ReadAtStartImplementationAsync();

		// Token: 0x06000906 RID: 2310
		protected abstract Task<bool> ReadAtCollectionStartImplementationAsync();

		// Token: 0x06000907 RID: 2311
		protected abstract Task<bool> ReadAtValueImplementationAsync();

		// Token: 0x06000908 RID: 2312
		protected abstract Task<bool> ReadAtCollectionEndImplementationAsync();

		// Token: 0x06000909 RID: 2313 RVA: 0x0001CB70 File Offset: 0x0001AD70
		protected override Task<bool> ReadAsynchronously()
		{
			switch (this.State)
			{
			case ODataCollectionReaderState.Start:
				return this.ReadAtStartImplementationAsync();
			case ODataCollectionReaderState.CollectionStart:
				return this.ReadAtCollectionStartImplementationAsync();
			case ODataCollectionReaderState.Value:
				return this.ReadAtValueImplementationAsync();
			case ODataCollectionReaderState.CollectionEnd:
				return this.ReadAtCollectionEndImplementationAsync();
			case ODataCollectionReaderState.Exception:
			case ODataCollectionReaderState.Completed:
				return TaskUtils.GetFaultedTask<bool>(new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionReaderCoreAsync_ReadAsynchronously)));
			default:
				return TaskUtils.GetFaultedTask<bool>(new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionReaderCoreAsync_ReadAsynchronously)));
			}
		}
	}
}
