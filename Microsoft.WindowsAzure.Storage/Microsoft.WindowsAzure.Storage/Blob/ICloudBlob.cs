using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000017 RID: 23
	public interface ICloudBlob : IListBlobItem
	{
		// Token: 0x0600026B RID: 619
		Stream OpenRead(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x0600026C RID: 620
		ICancellableAsyncResult BeginOpenRead(AsyncCallback callback, object state);

		// Token: 0x0600026D RID: 621
		ICancellableAsyncResult BeginOpenRead(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x0600026E RID: 622
		Stream EndOpenRead(IAsyncResult asyncResult);

		// Token: 0x0600026F RID: 623
		Task<Stream> OpenReadAsync();

		// Token: 0x06000270 RID: 624
		Task<Stream> OpenReadAsync(CancellationToken cancellationToken);

		// Token: 0x06000271 RID: 625
		Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000272 RID: 626
		Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x06000273 RID: 627
		void UploadFromStream(Stream source, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000274 RID: 628
		void UploadFromStream(Stream source, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000275 RID: 629
		ICancellableAsyncResult BeginUploadFromStream(Stream source, AsyncCallback callback, object state);

		// Token: 0x06000276 RID: 630
		ICancellableAsyncResult BeginUploadFromStream(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000277 RID: 631
		ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AsyncCallback callback, object state);

		// Token: 0x06000278 RID: 632
		ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000279 RID: 633
		void EndUploadFromStream(IAsyncResult asyncResult);

		// Token: 0x0600027A RID: 634
		Task UploadFromStreamAsync(Stream source);

		// Token: 0x0600027B RID: 635
		Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken);

		// Token: 0x0600027C RID: 636
		Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x0600027D RID: 637
		Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x0600027E RID: 638
		Task UploadFromStreamAsync(Stream source, long length);

		// Token: 0x0600027F RID: 639
		Task UploadFromStreamAsync(Stream source, long length, CancellationToken cancellationToken);

		// Token: 0x06000280 RID: 640
		Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000281 RID: 641
		Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x06000282 RID: 642
		void UploadFromFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000283 RID: 643
		ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AsyncCallback callback, object state);

		// Token: 0x06000284 RID: 644
		ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000285 RID: 645
		void EndUploadFromFile(IAsyncResult asyncResult);

		// Token: 0x06000286 RID: 646
		Task UploadFromFileAsync(string path, FileMode mode);

		// Token: 0x06000287 RID: 647
		Task UploadFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken);

		// Token: 0x06000288 RID: 648
		Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000289 RID: 649
		Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x0600028A RID: 650
		void UploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x0600028B RID: 651
		ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state);

		// Token: 0x0600028C RID: 652
		ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x0600028D RID: 653
		void EndUploadFromByteArray(IAsyncResult asyncResult);

		// Token: 0x0600028E RID: 654
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count);

		// Token: 0x0600028F RID: 655
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken);

		// Token: 0x06000290 RID: 656
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000291 RID: 657
		Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x06000292 RID: 658
		void DownloadToStream(Stream target, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000293 RID: 659
		ICancellableAsyncResult BeginDownloadToStream(Stream target, AsyncCallback callback, object state);

		// Token: 0x06000294 RID: 660
		ICancellableAsyncResult BeginDownloadToStream(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000295 RID: 661
		void EndDownloadToStream(IAsyncResult asyncResult);

		// Token: 0x06000296 RID: 662
		Task DownloadToStreamAsync(Stream target);

		// Token: 0x06000297 RID: 663
		Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken);

		// Token: 0x06000298 RID: 664
		Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000299 RID: 665
		Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x0600029A RID: 666
		void DownloadToFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x0600029B RID: 667
		ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AsyncCallback callback, object state);

		// Token: 0x0600029C RID: 668
		ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x0600029D RID: 669
		void EndDownloadToFile(IAsyncResult asyncResult);

		// Token: 0x0600029E RID: 670
		Task DownloadToFileAsync(string path, FileMode mode);

		// Token: 0x0600029F RID: 671
		Task DownloadToFileAsync(string path, FileMode mode, CancellationToken cancellationToken);

		// Token: 0x060002A0 RID: 672
		Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002A1 RID: 673
		Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002A2 RID: 674
		int DownloadToByteArray(byte[] target, int index, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002A3 RID: 675
		ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AsyncCallback callback, object state);

		// Token: 0x060002A4 RID: 676
		ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002A5 RID: 677
		int EndDownloadToByteArray(IAsyncResult asyncResult);

		// Token: 0x060002A6 RID: 678
		Task<int> DownloadToByteArrayAsync(byte[] target, int index);

		// Token: 0x060002A7 RID: 679
		Task<int> DownloadToByteArrayAsync(byte[] target, int index, CancellationToken cancellationToken);

		// Token: 0x060002A8 RID: 680
		Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002A9 RID: 681
		Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002AA RID: 682
		void DownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002AB RID: 683
		ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AsyncCallback callback, object state);

		// Token: 0x060002AC RID: 684
		ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002AD RID: 685
		void EndDownloadRangeToStream(IAsyncResult asyncResult);

		// Token: 0x060002AE RID: 686
		Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length);

		// Token: 0x060002AF RID: 687
		Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, CancellationToken cancellationToken);

		// Token: 0x060002B0 RID: 688
		Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002B1 RID: 689
		Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002B2 RID: 690
		int DownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002B3 RID: 691
		ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AsyncCallback callback, object state);

		// Token: 0x060002B4 RID: 692
		ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002B5 RID: 693
		int EndDownloadRangeToByteArray(IAsyncResult asyncResult);

		// Token: 0x060002B6 RID: 694
		Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length);

		// Token: 0x060002B7 RID: 695
		Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, CancellationToken cancellationToken);

		// Token: 0x060002B8 RID: 696
		Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002B9 RID: 697
		Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002BA RID: 698
		bool Exists(BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002BB RID: 699
		ICancellableAsyncResult BeginExists(AsyncCallback callback, object state);

		// Token: 0x060002BC RID: 700
		ICancellableAsyncResult BeginExists(BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002BD RID: 701
		bool EndExists(IAsyncResult asyncResult);

		// Token: 0x060002BE RID: 702
		Task<bool> ExistsAsync();

		// Token: 0x060002BF RID: 703
		Task<bool> ExistsAsync(CancellationToken cancellationToken);

		// Token: 0x060002C0 RID: 704
		Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002C1 RID: 705
		Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002C2 RID: 706
		void FetchAttributes(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002C3 RID: 707
		ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state);

		// Token: 0x060002C4 RID: 708
		ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002C5 RID: 709
		void EndFetchAttributes(IAsyncResult asyncResult);

		// Token: 0x060002C6 RID: 710
		Task FetchAttributesAsync();

		// Token: 0x060002C7 RID: 711
		Task FetchAttributesAsync(CancellationToken cancellationToken);

		// Token: 0x060002C8 RID: 712
		Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002C9 RID: 713
		Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002CA RID: 714
		void SetMetadata(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002CB RID: 715
		ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state);

		// Token: 0x060002CC RID: 716
		ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002CD RID: 717
		void EndSetMetadata(IAsyncResult asyncResult);

		// Token: 0x060002CE RID: 718
		Task SetMetadataAsync();

		// Token: 0x060002CF RID: 719
		Task SetMetadataAsync(CancellationToken cancellationToken);

		// Token: 0x060002D0 RID: 720
		Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002D1 RID: 721
		Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002D2 RID: 722
		void SetProperties(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002D3 RID: 723
		ICancellableAsyncResult BeginSetProperties(AsyncCallback callback, object state);

		// Token: 0x060002D4 RID: 724
		ICancellableAsyncResult BeginSetProperties(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002D5 RID: 725
		void EndSetProperties(IAsyncResult asyncResult);

		// Token: 0x060002D6 RID: 726
		Task SetPropertiesAsync();

		// Token: 0x060002D7 RID: 727
		Task SetPropertiesAsync(CancellationToken cancellationToken);

		// Token: 0x060002D8 RID: 728
		Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002D9 RID: 729
		Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002DA RID: 730
		void Delete(DeleteSnapshotsOption deleteSnapshotsOption = DeleteSnapshotsOption.None, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002DB RID: 731
		ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state);

		// Token: 0x060002DC RID: 732
		ICancellableAsyncResult BeginDelete(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002DD RID: 733
		void EndDelete(IAsyncResult asyncResult);

		// Token: 0x060002DE RID: 734
		Task DeleteAsync();

		// Token: 0x060002DF RID: 735
		Task DeleteAsync(CancellationToken cancellationToken);

		// Token: 0x060002E0 RID: 736
		Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002E1 RID: 737
		Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002E2 RID: 738
		bool DeleteIfExists(DeleteSnapshotsOption deleteSnapshotsOption = DeleteSnapshotsOption.None, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002E3 RID: 739
		ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state);

		// Token: 0x060002E4 RID: 740
		ICancellableAsyncResult BeginDeleteIfExists(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002E5 RID: 741
		bool EndDeleteIfExists(IAsyncResult asyncResult);

		// Token: 0x060002E6 RID: 742
		Task<bool> DeleteIfExistsAsync();

		// Token: 0x060002E7 RID: 743
		Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken);

		// Token: 0x060002E8 RID: 744
		Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002E9 RID: 745
		Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002EA RID: 746
		string AcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002EB RID: 747
		ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AsyncCallback callback, object state);

		// Token: 0x060002EC RID: 748
		ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002ED RID: 749
		string EndAcquireLease(IAsyncResult asyncResult);

		// Token: 0x060002EE RID: 750
		Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId);

		// Token: 0x060002EF RID: 751
		Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, CancellationToken cancellationToken);

		// Token: 0x060002F0 RID: 752
		Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002F1 RID: 753
		Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002F2 RID: 754
		void RenewLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002F3 RID: 755
		ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, AsyncCallback callback, object state);

		// Token: 0x060002F4 RID: 756
		ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002F5 RID: 757
		void EndRenewLease(IAsyncResult asyncResult);

		// Token: 0x060002F6 RID: 758
		Task RenewLeaseAsync(AccessCondition accessCondition);

		// Token: 0x060002F7 RID: 759
		Task RenewLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken);

		// Token: 0x060002F8 RID: 760
		Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x060002F9 RID: 761
		Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x060002FA RID: 762
		string ChangeLease(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x060002FB RID: 763
		ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, AsyncCallback callback, object state);

		// Token: 0x060002FC RID: 764
		ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x060002FD RID: 765
		string EndChangeLease(IAsyncResult asyncResult);

		// Token: 0x060002FE RID: 766
		Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition);

		// Token: 0x060002FF RID: 767
		Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, CancellationToken cancellationToken);

		// Token: 0x06000300 RID: 768
		Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000301 RID: 769
		Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x06000302 RID: 770
		void ReleaseLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000303 RID: 771
		ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, AsyncCallback callback, object state);

		// Token: 0x06000304 RID: 772
		ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000305 RID: 773
		void EndReleaseLease(IAsyncResult asyncResult);

		// Token: 0x06000306 RID: 774
		Task ReleaseLeaseAsync(AccessCondition accessCondition);

		// Token: 0x06000307 RID: 775
		Task ReleaseLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken);

		// Token: 0x06000308 RID: 776
		Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000309 RID: 777
		Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x0600030A RID: 778
		TimeSpan BreakLease(TimeSpan? breakPeriod = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x0600030B RID: 779
		ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AsyncCallback callback, object state);

		// Token: 0x0600030C RID: 780
		ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x0600030D RID: 781
		TimeSpan EndBreakLease(IAsyncResult asyncResult);

		// Token: 0x0600030E RID: 782
		Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod);

		// Token: 0x0600030F RID: 783
		Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, CancellationToken cancellationToken);

		// Token: 0x06000310 RID: 784
		Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000311 RID: 785
		Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x06000312 RID: 786
		string StartCopyFromBlob(Uri source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x06000313 RID: 787
		ICancellableAsyncResult BeginStartCopyFromBlob(Uri source, AsyncCallback callback, object state);

		// Token: 0x06000314 RID: 788
		ICancellableAsyncResult BeginStartCopyFromBlob(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x06000315 RID: 789
		string EndStartCopyFromBlob(IAsyncResult asyncResult);

		// Token: 0x06000316 RID: 790
		Task<string> StartCopyFromBlobAsync(Uri source);

		// Token: 0x06000317 RID: 791
		Task<string> StartCopyFromBlobAsync(Uri source, CancellationToken cancellationToken);

		// Token: 0x06000318 RID: 792
		Task<string> StartCopyFromBlobAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000319 RID: 793
		Task<string> StartCopyFromBlobAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x0600031A RID: 794
		void AbortCopy(string copyId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null);

		// Token: 0x0600031B RID: 795
		ICancellableAsyncResult BeginAbortCopy(string copyId, AsyncCallback callback, object state);

		// Token: 0x0600031C RID: 796
		ICancellableAsyncResult BeginAbortCopy(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state);

		// Token: 0x0600031D RID: 797
		void EndAbortCopy(IAsyncResult asyncResult);

		// Token: 0x0600031E RID: 798
		Task AbortCopyAsync(string copyId);

		// Token: 0x0600031F RID: 799
		Task AbortCopyAsync(string copyId, CancellationToken cancellationToken);

		// Token: 0x06000320 RID: 800
		Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext);

		// Token: 0x06000321 RID: 801
		Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken);

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000322 RID: 802
		string Name { get; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000323 RID: 803
		CloudBlobClient ServiceClient { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000324 RID: 804
		// (set) Token: 0x06000325 RID: 805
		int StreamWriteSizeInBytes { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000326 RID: 806
		// (set) Token: 0x06000327 RID: 807
		int StreamMinimumReadSizeInBytes { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000328 RID: 808
		BlobProperties Properties { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000329 RID: 809
		IDictionary<string, string> Metadata { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600032A RID: 810
		DateTimeOffset? SnapshotTime { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600032B RID: 811
		bool IsSnapshot { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600032C RID: 812
		Uri SnapshotQualifiedUri { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600032D RID: 813
		StorageUri SnapshotQualifiedStorageUri { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600032E RID: 814
		CopyState CopyState { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600032F RID: 815
		BlobType BlobType { get; }

		// Token: 0x06000330 RID: 816
		string GetSharedAccessSignature(SharedAccessBlobPolicy policy);

		// Token: 0x06000331 RID: 817
		string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier);

		// Token: 0x06000332 RID: 818
		string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers);

		// Token: 0x06000333 RID: 819
		string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string groupPolicyIdentifier);

		// Token: 0x06000334 RID: 820
		string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string groupPolicyIdentifier, string sasVersion);
	}
}
