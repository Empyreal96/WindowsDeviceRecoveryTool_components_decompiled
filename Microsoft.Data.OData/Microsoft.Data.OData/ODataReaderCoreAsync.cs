using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200015A RID: 346
	internal abstract class ODataReaderCoreAsync : ODataReaderCore
	{
		// Token: 0x0600097C RID: 2428 RVA: 0x0001D8CD File Offset: 0x0001BACD
		protected ODataReaderCoreAsync(ODataInputContext inputContext, bool readingFeed, IODataReaderWriterListener listener) : base(inputContext, readingFeed, listener)
		{
		}

		// Token: 0x0600097D RID: 2429
		protected abstract Task<bool> ReadAtStartImplementationAsync();

		// Token: 0x0600097E RID: 2430
		protected abstract Task<bool> ReadAtFeedStartImplementationAsync();

		// Token: 0x0600097F RID: 2431
		protected abstract Task<bool> ReadAtFeedEndImplementationAsync();

		// Token: 0x06000980 RID: 2432
		protected abstract Task<bool> ReadAtEntryStartImplementationAsync();

		// Token: 0x06000981 RID: 2433
		protected abstract Task<bool> ReadAtEntryEndImplementationAsync();

		// Token: 0x06000982 RID: 2434
		protected abstract Task<bool> ReadAtNavigationLinkStartImplementationAsync();

		// Token: 0x06000983 RID: 2435
		protected abstract Task<bool> ReadAtNavigationLinkEndImplementationAsync();

		// Token: 0x06000984 RID: 2436
		protected abstract Task<bool> ReadAtEntityReferenceLinkAsync();

		// Token: 0x06000985 RID: 2437 RVA: 0x0001D928 File Offset: 0x0001BB28
		protected override Task<bool> ReadAsynchronously()
		{
			Task<bool> antecedentTask;
			switch (this.State)
			{
			case ODataReaderState.Start:
				antecedentTask = this.ReadAtStartImplementationAsync();
				break;
			case ODataReaderState.FeedStart:
				antecedentTask = this.ReadAtFeedStartImplementationAsync();
				break;
			case ODataReaderState.FeedEnd:
				antecedentTask = this.ReadAtFeedEndImplementationAsync();
				break;
			case ODataReaderState.EntryStart:
				antecedentTask = TaskUtils.GetTaskForSynchronousOperation(delegate()
				{
					base.IncreaseEntryDepth();
				}).FollowOnSuccessWithTask((Task t) => this.ReadAtEntryStartImplementationAsync());
				break;
			case ODataReaderState.EntryEnd:
				antecedentTask = TaskUtils.GetTaskForSynchronousOperation(delegate()
				{
					base.DecreaseEntryDepth();
				}).FollowOnSuccessWithTask((Task t) => this.ReadAtEntryEndImplementationAsync());
				break;
			case ODataReaderState.NavigationLinkStart:
				antecedentTask = this.ReadAtNavigationLinkStartImplementationAsync();
				break;
			case ODataReaderState.NavigationLinkEnd:
				antecedentTask = this.ReadAtNavigationLinkEndImplementationAsync();
				break;
			case ODataReaderState.EntityReferenceLink:
				antecedentTask = this.ReadAtEntityReferenceLinkAsync();
				break;
			case ODataReaderState.Exception:
			case ODataReaderState.Completed:
				antecedentTask = TaskUtils.GetFaultedTask<bool>(new ODataException(Strings.ODataReaderCore_NoReadCallsAllowed(this.State)));
				break;
			default:
				antecedentTask = TaskUtils.GetFaultedTask<bool>(new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataReaderCoreAsync_ReadAsynchronously)));
				break;
			}
			return antecedentTask.FollowOnSuccessWith(delegate(Task<bool> t)
			{
				if ((this.State == ODataReaderState.EntryStart || this.State == ODataReaderState.EntryEnd) && this.Item != null)
				{
					ReaderValidationUtils.ValidateEntry(base.CurrentEntry);
				}
				return t.Result;
			});
		}
	}
}
