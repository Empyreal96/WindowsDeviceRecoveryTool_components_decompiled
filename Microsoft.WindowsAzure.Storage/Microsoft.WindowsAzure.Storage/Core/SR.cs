using System;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x0200008B RID: 139
	internal class SR
	{
		// Token: 0x04000293 RID: 659
		public const string AbsoluteAddressNotPermitted = "Address '{0}' is an absolute address. Only relative addresses are permitted.";

		// Token: 0x04000294 RID: 660
		public const string ArgumentEmptyError = "The argument must not be empty string.";

		// Token: 0x04000295 RID: 661
		public const string ArgumentOutOfRangeError = "The argument is out of range. Value passed: {0}";

		// Token: 0x04000296 RID: 662
		public const string ArgumentTooLargeError = "The argument '{0}' is larger than maximum of '{1}'";

		// Token: 0x04000297 RID: 663
		public const string ArgumentTooSmallError = "The argument '{0}' is smaller than minimum of '{1}'";

		// Token: 0x04000298 RID: 664
		public const string AttemptedEdmTypeForTheProperty = "Attempting to deserialize '{0}' as type '{1}'";

		// Token: 0x04000299 RID: 665
		public const string BatchWithRetreiveContainsOtherOperations = "A batch transaction with a retrieve operation cannot contain any other operations.";

		// Token: 0x0400029A RID: 666
		public const string BinaryMessageShouldUseBase64Encoding = "EncodeMessage should be true for binary message.";

		// Token: 0x0400029B RID: 667
		public const string Blob = "blob";

		// Token: 0x0400029C RID: 668
		public const string BlobDataCorrupted = "Blob data corrupted (integrity check failed), Expected value is '{0}', retrieved '{1}'";

		// Token: 0x0400029D RID: 669
		public const string BlobEndPointNotConfigured = "No blob endpoint configured.";

		// Token: 0x0400029E RID: 670
		public const string BlobInvalidSequenceNumber = "The sequence number may not be specified for an increment operation.";

		// Token: 0x0400029F RID: 671
		public const string BlobStreamAlreadyCommitted = "Blob stream has already been committed once.";

		// Token: 0x040002A0 RID: 672
		public const string BlobStreamFlushPending = "Blob stream has a pending flush operation. Please call EndFlush first.";

		// Token: 0x040002A1 RID: 673
		public const string BlobStreamReadPending = "Blob stream has a pending read operation. Please call EndRead first.";

		// Token: 0x040002A2 RID: 674
		public const string BlobTypeMismatch = "Blob type of the blob reference doesn't match blob type of the blob.";

		// Token: 0x040002A3 RID: 675
		public const string BufferTooSmall = "The provided buffer is too small to fit in the blob data given the offset.";

		// Token: 0x040002A4 RID: 676
		public const string BufferManagerProvidedIncorrectLengthBuffer = "The IBufferManager provided an incorrect length buffer to the stream, Expected {0}, received {1}. Buffer length should equal the value returned by IBufferManager.GetDefaultBufferSize().";

		// Token: 0x040002A5 RID: 677
		public const string CannotCreateSASSignatureForGivenCred = "Cannot create Shared Access Signature as the credentials does not have account name information. Please check that the credentials used support creating Shared Access Signature.";

		// Token: 0x040002A6 RID: 678
		public const string CannotCreateSASWithoutAccountKey = "Cannot create Shared Access Signature unless Account Key credentials are used.";

		// Token: 0x040002A7 RID: 679
		public const string CannotModifySnapshot = "Cannot perform this operation on a blob representing a snapshot.";

		// Token: 0x040002A8 RID: 680
		public const string CannotUpdateKeyWithoutAccountKeyCreds = "Cannot update key unless Account Key credentials are used.";

		// Token: 0x040002A9 RID: 681
		public const string CannotUpdateSasWithoutSasCreds = "Cannot update Shared Access Signature unless Sas credentials are used.";

		// Token: 0x040002AA RID: 682
		public const string ConcurrentOperationsNotSupported = "Could not acquire exclusive use of the TableServiceContext, Concurrent operations are not supported.";

		// Token: 0x040002AB RID: 683
		public const string Container = "container";

		// Token: 0x040002AC RID: 684
		public const string ContentMD5NotCalculated = "The operation requires a response body but no data was copied to the destination buffer.";

		// Token: 0x040002AD RID: 685
		public const string CopyAborted = "The copy operation has been aborted by the user.";

		// Token: 0x040002AE RID: 686
		public const string CopyFailed = "The copy operation failed with the following error message: {0}";

		// Token: 0x040002AF RID: 687
		public const string CryptoError = "Cryptographic error occurred. Please check the inner exception for more details.";

		// Token: 0x040002B0 RID: 688
		public const string CryptoFunctionFailed = "Crypto function failed with error code '{0}'";

		// Token: 0x040002B1 RID: 689
		public const string DecryptionLogicError = "Decryption logic threw error. Please check the inner exception for more details.";

		// Token: 0x040002B2 RID: 690
		public const string DeleteSnapshotsNotValidError = "The option '{0}' must be 'None' to delete a specific snapshot specified by '{1}'";

		// Token: 0x040002B3 RID: 691
		public const string Directory = "directory";

		// Token: 0x040002B4 RID: 692
		public const string EmptyBatchOperation = "Cannot execute an empty batch operation";

		// Token: 0x040002B5 RID: 693
		public const string EncryptedMessageTooLarge = "Encrypted Messages cannot be larger than {0} bytes. Please note that encrypting queue messages can increase their size.";

		// Token: 0x040002B6 RID: 694
		public const string EncryptionDataNotPresentError = "Encryption data does not exist. If you do not want to decrypt the data, please do not set the RequireEncryption flag on request options.";

		// Token: 0x040002B7 RID: 695
		public const string EncryptionLogicError = "Encryption logic threw error. Please check the inner exception for more details.";

		// Token: 0x040002B8 RID: 696
		public const string EncryptedMessageDeserializingError = "Error while de-serializing the encrypted queue message string from the wire. Please check inner exception for more details.";

		// Token: 0x040002B9 RID: 697
		public const string EncryptionNotSupportedForOperation = "Encryption is not supported for the current operation. Please ensure that EncryptionPolicy is not set on RequestOptions.";

		// Token: 0x040002BA RID: 698
		public const string EncryptingNullPropertiesNotAllowed = "Null properties cannot be encrypted. Please assign a default value to the property if you wish to encrypt it.";

		// Token: 0x040002BB RID: 699
		public const string EncryptionMetadataError = "Error while de-serializing the encryption metadata string from the wire.";

		// Token: 0x040002BC RID: 700
		public const string EncryptionNotSupportedForExistingBlobs = "Encryption is not supported for a blob that already exists. Please do not specify an encryption policy.";

		// Token: 0x040002BD RID: 701
		public const string EncryptionNotSupportedForFiles = "Encryption is not supported for files.";

		// Token: 0x040002BE RID: 702
		public const string EncryptionNotSupportedForPageBlobsOnPhone = "Encryption is not supported for PageBlobs on Windows Phone.";

		// Token: 0x040002BF RID: 703
		public const string EncryptionPolicyMissingInStrictMode = "Encryption Policy is mandatory when RequireEncryption is set to true. If you do not want to encrypt/decrypt data, please set RequireEncryption to false in request options.";

		// Token: 0x040002C0 RID: 704
		public const string EncryptionProtocolVersionInvalid = "Invalid Encryption Agent. This version of the client library does not understand the Encryption Agent set on the blob.";

		// Token: 0x040002C1 RID: 705
		public const string ETagMissingForDelete = "Delete requires an ETag (which may be the '*' wildcard).";

		// Token: 0x040002C2 RID: 706
		public const string ETagMissingForMerge = "Merge requires an ETag (which may be the '*' wildcard).";

		// Token: 0x040002C3 RID: 707
		public const string ETagMissingForReplace = "Replace requires an ETag (which may be the '*' wildcard).";

		// Token: 0x040002C4 RID: 708
		public const string ExceptionOccurred = "An exception has occurred. For more information please deserialize this message via RequestResult.TranslateFromExceptionMessage.";

		// Token: 0x040002C5 RID: 709
		public const string ExtendedErrorUnavailable = "An unknown error has occurred, extended error information not available.";

		// Token: 0x040002C6 RID: 710
		public const string File = "file";

		// Token: 0x040002C7 RID: 711
		public const string FileDataCorrupted = "File data corrupted (integrity check failed), Expected value is '{0}', retrieved '{1}'";

		// Token: 0x040002C8 RID: 712
		public const string FileEndPointNotConfigured = "No file endpoint configured.";

		// Token: 0x040002C9 RID: 713
		public const string FileStreamAlreadyCommitted = "File stream has already been committed once.";

		// Token: 0x040002CA RID: 714
		public const string FileStreamFlushPending = "File stream has a pending flush operation. Please call EndFlush first.";

		// Token: 0x040002CB RID: 715
		public const string FileStreamReadPending = "File stream has a pending read operation. Please call EndRead first.";

		// Token: 0x040002CC RID: 716
		public const string FailParseProperty = "Failed to parse property '{0}' with value '{1}' as type '{2}'";

		// Token: 0x040002CD RID: 717
		public const string IncorrectNumberOfBytes = "Incorrect number of bytes received. Expected '{0}', received '{1}'";

		// Token: 0x040002CE RID: 718
		public const string InternalStorageError = "Unexpected internal storage client error.";

		// Token: 0x040002CF RID: 719
		public const string InvalidAclType = "Invalid acl public access type returned '{0}'. Expected blob or container.";

		// Token: 0x040002D0 RID: 720
		public const string InvalidBlobListItem = "Invalid blob list item returned";

		// Token: 0x040002D1 RID: 721
		public const string InvalidCorsRule = "A CORS rule must contain at least one allowed origin and allowed method, and MaxAgeInSeconds cannot have a value less than zero.";

		// Token: 0x040002D2 RID: 722
		public const string InvalidDelimiter = "\\ is an invalid delimiter.";

		// Token: 0x040002D3 RID: 723
		public const string InvalidFileAclType = "Invalid acl public access type returned '{0}'. Expected file or share.";

		// Token: 0x040002D4 RID: 724
		public const string InvalidEncryptionAlgorithm = "Invalid Encryption Algorithm found on the resource. This version of the client library does not support the specified encryption algorithm.";

		// Token: 0x040002D5 RID: 725
		public const string InvalidEncryptionMode = "Invalid BlobEncryptionMode set on the policy. Please set it to FullBlob when the policy is used with UploadFromStream.";

		// Token: 0x040002D6 RID: 726
		public const string InvalidFileListItem = "Invalid file list item returned";

		// Token: 0x040002D7 RID: 727
		public const string InvalidGeoReplicationStatus = "Invalid geo-replication status in response: '{0}'";

		// Token: 0x040002D8 RID: 728
		public const string InvalidHeaders = "Headers are not supported in the 2012-02-12 version.";

		// Token: 0x040002D9 RID: 729
		public const string InvalidLeaseStatus = "Invalid lease status in response: '{0}'";

		// Token: 0x040002DA RID: 730
		public const string InvalidLeaseState = "Invalid lease state in response: '{0}'";

		// Token: 0x040002DB RID: 731
		public const string InvalidLeaseDuration = "Invalid lease duration in response: '{0}'";

		// Token: 0x040002DC RID: 732
		public const string InvalidListingDetails = "Invalid blob listing details specified.";

		// Token: 0x040002DD RID: 733
		public const string InvalidLoggingLevel = "Invalid logging operations specified.";

		// Token: 0x040002DE RID: 734
		public const string InvalidMetricsLevel = "Invalid metrics level specified.";

		// Token: 0x040002DF RID: 735
		public const string InvalidBlockSize = "Append block data should not exceed the maximum blob size condition value.";

		// Token: 0x040002E0 RID: 736
		public const string InvalidPageSize = "Page data must be a multiple of 512 bytes.";

		// Token: 0x040002E1 RID: 737
		public const string InvalidResourceName = "Invalid {0} name. Check MSDN for more information about valid {0} naming.";

		// Token: 0x040002E2 RID: 738
		public const string InvalidResourceNameLength = "Invalid {0} name length. The {0} name must be between {1} and {2} characters long.";

		// Token: 0x040002E3 RID: 739
		public const string InvalidResourceReservedName = "Invalid {0} name. This {0} name is reserved.";

		// Token: 0x040002E4 RID: 740
		public const string InvalidSASVersion = "SAS Version invalid. Valid versions include 2012-02-12 and 2013-08-15.";

		// Token: 0x040002E5 RID: 741
		public const string InvalidStorageService = "Invalid storage service specified.";

		// Token: 0x040002E6 RID: 742
		public const string IQueryableExtensionObjectMustBeTableQuery = "Query must be a TableQuery<T>";

		// Token: 0x040002E7 RID: 743
		public const string JsonNotSupportedOnRT = "JSON payloads are not supported in Windows Runtime.";

		// Token: 0x040002E8 RID: 744
		public const string JsonReaderNotInCompletedState = "The JSON reader has not yet reached the completed state.";

		// Token: 0x040002E9 RID: 745
		public const string KeyMismatch = "Key mismatch. The key id stored on the service does not match the specified key.";

		// Token: 0x040002EA RID: 746
		public const string KeyMissingError = "Key is not initialized. Encryption requires it to be initialized.";

		// Token: 0x040002EB RID: 747
		public const string KeyAndResolverMissingError = "Key and Resolver are not initialized. Decryption requires either of them to be initialized.";

		// Token: 0x040002EC RID: 748
		public const string LeaseConditionOnSource = "A lease condition cannot be specified on the source of a copy.";

		// Token: 0x040002ED RID: 749
		public const string LeaseTimeNotReceived = "Valid lease time expected but not received from the service.";

		// Token: 0x040002EE RID: 750
		public const string LengthNotInRange = "The length provided is out of range. The range must be between 0 and the length of the byte array.";

		// Token: 0x040002EF RID: 751
		public const string ListSnapshotsWithDelimiterError = "Listing snapshots is only supported in flat mode (no delimiter). Consider setting the useFlatBlobListing parameter to true.";

		// Token: 0x040002F0 RID: 752
		public const string LogStreamEndError = "Error parsing log record: unexpected end of stream at position '{0}'.";

		// Token: 0x040002F1 RID: 753
		public const string LogStreamDelimiterError = "Error parsing log record: expected the delimiter '{0}', but read '{1}' at position '{2}'.";

		// Token: 0x040002F2 RID: 754
		public const string LogStreamParseError = "Error parsing log record: could not parse '{0}' using format: {1}";

		// Token: 0x040002F3 RID: 755
		public const string LogStreamQuoteError = "Error parsing log record: unexpected quote character found. String so far: '{0}'. Character read: '{1}'";

		// Token: 0x040002F4 RID: 756
		public const string LogVersionUnsupported = "A storage log version of {0} is unsupported.";

		// Token: 0x040002F5 RID: 757
		public const string LoggingVersionNull = "The logging version is null or empty.";

		// Token: 0x040002F6 RID: 758
		public const string MD5MismatchError = "Calculated MD5 does not match existing property";

		// Token: 0x040002F7 RID: 759
		public const string MD5NotPossible = "MD5 cannot be calculated for an existing blob because it would require reading the existing data. Please disable StoreBlobContentMD5.";

		// Token: 0x040002F8 RID: 760
		public const string MD5NotPresentError = "MD5 does not exist. If you do not want to force validation, please disable UseTransactionalMD5.";

		// Token: 0x040002F9 RID: 761
		public const string MessageTooLarge = "Messages cannot be larger than {0} bytes.";

		// Token: 0x040002FA RID: 762
		public const string MetricVersionNull = "The metrics version is null or empty.";

		// Token: 0x040002FB RID: 763
		public const string MissingAccountInformationInUri = "Cannot find account information inside Uri '{0}'";

		// Token: 0x040002FC RID: 764
		public const string MissingContainerInformation = "Invalid blob address '{0}', missing container information";

		// Token: 0x040002FD RID: 765
		public const string MissingCredentials = "No credentials provided.";

		// Token: 0x040002FE RID: 766
		public const string MissingLeaseIDChanging = "A lease ID must be specified when changing a lease.";

		// Token: 0x040002FF RID: 767
		public const string MissingLeaseIDReleasing = "A lease ID must be specified when releasing a lease.";

		// Token: 0x04000300 RID: 768
		public const string MissingLeaseIDRenewing = "A lease ID must be specified when renewing a lease.";

		// Token: 0x04000301 RID: 769
		public const string MissingMandatoryParametersForSAS = "Missing mandatory parameters for valid Shared Access Signature";

		// Token: 0x04000302 RID: 770
		public const string MissingShareInformation = "Invalid file address '{0}', missing share information";

		// Token: 0x04000303 RID: 771
		public const string MissingWrappingIV = "A key wrapping IV must be present in the encryption metadata while decrypting.";

		// Token: 0x04000304 RID: 772
		public const string StorageUriMustMatch = "Primary and secondary location URIs in a StorageUri must point to the same resource.";

		// Token: 0x04000305 RID: 773
		public const string MultipleCredentialsProvided = "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.";

		// Token: 0x04000306 RID: 774
		public const string MultipleSnapshotTimesProvided = "Multiple different snapshot times provided as part of query '{0}' and as constructor parameter '{1}'.";

		// Token: 0x04000307 RID: 775
		public const string NoPropertyResolverAvailable = "No property resolver available. Deserializing the entity properties as strings.";

		// Token: 0x04000308 RID: 776
		public const string OffsetNotInRange = "The offset provided is out of range. The range must be between 0 and the length of the byte array.";

		// Token: 0x04000309 RID: 777
		public const string ODataReaderNotInCompletedState = "OData Reader state expected to be Completed state. Actual state: {0}.";

		// Token: 0x0400030A RID: 778
		public const string OperationCanceled = "Operation was canceled by user.";

		// Token: 0x0400030B RID: 779
		public const string ParseError = "Error parsing value";

		// Token: 0x0400030C RID: 780
		public const string PartitionKey = "All entities in a given batch must have the same partition key.";

		// Token: 0x0400030D RID: 781
		public const string PathStyleUriMissingAccountNameInformation = "Missing account name information inside path style uri. Path style uris should be of the form http://<IPAddressPlusPort>/<accountName>";

		// Token: 0x0400030E RID: 782
		public const string PayloadFormat = "Setting payload format for the request to '{0}'.";

		// Token: 0x0400030F RID: 783
		public const string PreconditionFailed = "The condition specified using HTTP conditional header(s) is not met.";

		// Token: 0x04000310 RID: 784
		public const string PreconditionFailureIgnored = "Pre-condition failure on a retry is being ignored since the request should have succeeded in the first attempt.";

		// Token: 0x04000311 RID: 785
		public const string PrimaryOnlyCommand = "This operation can only be executed against the primary storage location.";

		// Token: 0x04000312 RID: 786
		public const string PropertyResolverCacheDisabled = "Property resolver cache is disabled.";

		// Token: 0x04000313 RID: 787
		public const string PropertyResolverThrewError = "The custom property resolver delegate threw an exception. Check the inner exception for more details";

		// Token: 0x04000314 RID: 788
		public const string PutBlobNeedsStoreBlobContentMD5 = "When uploading a blob in a single request, StoreBlobContentMD5 must be set to true if UseTransactionalMD5 is true, because the MD5 calculated for the transaction will be stored in the blob.";

		// Token: 0x04000315 RID: 789
		public const string QueryBuilderKeyNotFound = "'{0}' key not found in the query builder.";

		// Token: 0x04000316 RID: 790
		public const string Queue = "queue";

		// Token: 0x04000317 RID: 791
		public const string QueueEndPointNotConfigured = "No queue endpoint configured.";

		// Token: 0x04000318 RID: 792
		public const string RangeDownloadNotPermittedOnPhone = "Windows Phone does not support downloading closed ranges from an encrypted blob. Please download the full blob or an open range (by specifying length as null)";

		// Token: 0x04000319 RID: 793
		public const string RelativeAddressNotPermitted = "Address '{0}' is a relative address. Only absolute addresses are permitted.";

		// Token: 0x0400031A RID: 794
		public const string ResourceConsumed = "Resource consumed";

		// Token: 0x0400031B RID: 795
		public const string ResourceNameEmpty = "Invalid {0} name. The {0} name may not be null, empty, or whitespace only.";

		// Token: 0x0400031C RID: 796
		public const string RetrieveWithContinuationToken = "Retrieved '{0}' results with continuation token '{1}'.";

		// Token: 0x0400031D RID: 797
		public const string SecondaryOnlyCommand = "This operation can only be executed against the secondary storage location.";

		// Token: 0x0400031E RID: 798
		public const string Share = "share";

		// Token: 0x0400031F RID: 799
		public const string StartTimeExceedsEndTime = "StartTime invalid. The start time '{0}' occurs after the end time '{1}'.";

		// Token: 0x04000320 RID: 800
		public const string StorageUriMissingLocation = "The Uri for the target storage location is not specified. Please consider changing the request's location mode.";

		// Token: 0x04000321 RID: 801
		public const string StreamLengthError = "The length of the stream exceeds the permitted length.";

		// Token: 0x04000322 RID: 802
		public const string StreamLengthMismatch = "Cannot specify both copyLength and maxLength.";

		// Token: 0x04000323 RID: 803
		public const string StreamLengthShortError = "The requested number of bytes exceeds the length of the stream remaining from the specified position.";

		// Token: 0x04000324 RID: 804
		public const string Table = "table";

		// Token: 0x04000325 RID: 805
		public const string TableEndPointNotConfigured = "No table endpoint configured.";

		// Token: 0x04000326 RID: 806
		public const string TableQueryDynamicPropertyAccess = "Accessing property dictionary of DynamicTableEntity requires a string constant for property name.";

		// Token: 0x04000327 RID: 807
		public const string TableQueryEntityPropertyInQueryNotSupported = "Referencing {0} on EntityProperty only supported with properties dictionary exposed via DynamicTableEntity.";

		// Token: 0x04000328 RID: 808
		public const string TableQueryFluentMethodNotAllowed = "Fluent methods may not be invoked on a Query created via CloudTable.CreateQuery<T>()";

		// Token: 0x04000329 RID: 809
		public const string TableQueryMustHaveQueryProvider = "Unknown Table. The TableQuery does not have an associated CloudTable Reference. Please execute the query via the CloudTable ExecuteQuery APIs.";

		// Token: 0x0400032A RID: 810
		public const string TableQueryTypeMustImplementITableEnitty = "TableQuery Generic Type must implement the ITableEntity Interface";

		// Token: 0x0400032B RID: 811
		public const string TableQueryTypeMustHaveDefaultParameterlessCtor = "TableQuery Generic Type must provide a default parameterless constructor.";

		// Token: 0x0400032C RID: 812
		public const string TakeCountNotPositive = "Take count must be positive and greater than 0.";

		// Token: 0x0400032D RID: 813
		public const string TimeoutExceptionMessage = "The client could not finish the operation within specified timeout.";

		// Token: 0x0400032E RID: 814
		public const string TooManyPolicyIdentifiers = "Too many '{0}' shared access policy identifiers provided. Server does not support setting more than '{1}' on a single container, queue, table, or share.";

		// Token: 0x0400032F RID: 815
		public const string TooManyPathSegments = "The count of URL path segments (strings between '/' characters) as part of the blob name cannot exceed 254.";

		// Token: 0x04000330 RID: 816
		public const string TraceAbort = "Aborting pending request due to timeout.";

		// Token: 0x04000331 RID: 817
		public const string TraceAbortError = "Could not abort pending request because of {0}.";

		// Token: 0x04000332 RID: 818
		public const string TraceAbortRetry = "Aborting pending retry due to user request.";

		// Token: 0x04000333 RID: 819
		public const string TraceDispose = "Disposing action invoked.";

		// Token: 0x04000334 RID: 820
		public const string TraceDisposeError = "Disposing action threw an exception : {0}.";

		// Token: 0x04000335 RID: 821
		public const string TraceDownload = "Downloading response body.";

		// Token: 0x04000336 RID: 822
		public const string TraceDownloadError = "Downloading error response body.";

		// Token: 0x04000337 RID: 823
		public const string TraceRetryInfo = "The extended retry policy set the next location to {0} and updated the location mode to {1}.";

		// Token: 0x04000338 RID: 824
		public const string TraceGenericError = "Exception thrown during the operation: {0}.";

		// Token: 0x04000339 RID: 825
		public const string TraceGetResponse = "Waiting for response.";

		// Token: 0x0400033A RID: 826
		public const string TraceGetResponseError = "Exception thrown while waiting for response: {0}.";

		// Token: 0x0400033B RID: 827
		public const string TraceIgnoreAttribute = "Omitting property '{0}' from serialization/de-serialization because IgnoreAttribute has been set on that property.";

		// Token: 0x0400033C RID: 828
		public const string TraceInitLocation = "Starting operation with location {0} per location mode {1}.";

		// Token: 0x0400033D RID: 829
		public const string TraceInitRequestError = "Exception thrown while initializing request: {0}.";

		// Token: 0x0400033E RID: 830
		public const string TraceMissingDictionaryEntry = "Omitting property '{0}' from de-serialization because there is no corresponding entry in the dictionary provided.";

		// Token: 0x0400033F RID: 831
		public const string TraceNextLocation = "The next location has been set to {0}, based on the location mode.";

		// Token: 0x04000340 RID: 832
		public const string TraceNonPublicGetSet = "Omitting property '{0}' from serialization/de-serialization because the property's getter/setter are not public.";

		// Token: 0x04000341 RID: 833
		public const string TracePrepareUpload = "Preparing to write request data.";

		// Token: 0x04000342 RID: 834
		public const string TracePrepareUploadError = "Exception thrown while preparing to write request data: {0}.";

		// Token: 0x04000343 RID: 835
		public const string TracePreProcessDone = "Response headers were processed successfully, proceeding with the rest of the operation.";

		// Token: 0x04000344 RID: 836
		public const string TracePreProcessError = "Exception thrown while processing response: {0}.";

		// Token: 0x04000345 RID: 837
		public const string TracePostProcess = "Processing response body.";

		// Token: 0x04000346 RID: 838
		public const string TracePostProcessError = "Exception thrown while ending operation: {0}.";

		// Token: 0x04000347 RID: 839
		public const string TraceResponse = "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.";

		// Token: 0x04000348 RID: 840
		public const string TraceRetry = "Retrying failed operation.";

		// Token: 0x04000349 RID: 841
		public const string TraceRetryCheck = "Checking if the operation should be retried. Retry count = {0}, HTTP status code = {1}, Retryable exception = {2}, Exception = {3}.";

		// Token: 0x0400034A RID: 842
		public const string TraceRetryDecisionPolicy = "Retry policy did not allow for a retry. Failing with {0}.";

		// Token: 0x0400034B RID: 843
		public const string TraceRetryDecisionTimeout = "Operation cannot be retried because the maximum execution time has been reached. Failing with {0}.";

		// Token: 0x0400034C RID: 844
		public const string TraceRetryDelay = "Operation will be retried after {0}ms.";

		// Token: 0x0400034D RID: 845
		public const string TraceRetryError = "Exception thrown while retrying operation: {0}.";

		// Token: 0x0400034E RID: 846
		public const string TraceStartRequestAsync = "Starting asynchronous request to {0}.";

		// Token: 0x0400034F RID: 847
		public const string TraceStartRequestSync = "Starting synchronous request to {0}.";

		// Token: 0x04000350 RID: 848
		public const string TraceStringToSign = "StringToSign = {0}.";

		// Token: 0x04000351 RID: 849
		public const string TraceSuccess = "Operation completed successfully.";

		// Token: 0x04000352 RID: 850
		public const string TraceUpload = "Writing request data.";

		// Token: 0x04000353 RID: 851
		public const string TraceUploadError = "Exception thrown while writing request data: {0}.";

		// Token: 0x04000354 RID: 852
		public const string UndefinedBlobType = "The blob type cannot be undefined.";

		// Token: 0x04000355 RID: 853
		public const string UnexpectedElement = "Unexpected Element '{0}'";

		// Token: 0x04000356 RID: 854
		public const string UnexpectedEmptyElement = "Unexpected Empty Element '{0}'";

		// Token: 0x04000357 RID: 855
		public const string UnexpectedContinuationType = "Unexpected Continuation Type";

		// Token: 0x04000358 RID: 856
		public const string UnexpectedLocation = "Unexpected Location '{0}'";

		// Token: 0x04000359 RID: 857
		public const string UnexpectedResponseCode = "Unexpected response code, Expected:{0}, Received:{1}";

		// Token: 0x0400035A RID: 858
		public const string UnexpectedResponseCodeForOperation = "Unexpected response code for operation : ";

		// Token: 0x0400035B RID: 859
		public const string UnsupportedPropertyTypeForEncryption = "Unsupported type : {0} encountered during encryption. Only string properties can be encrypted on the client side.";

		// Token: 0x0400035C RID: 860
		public const string UpdateMessageVisibilityRequired = "Calls to UpdateMessage must include the Visibility flag.";

		// Token: 0x0400035D RID: 861
		public const string UsingDefaultPropertyResolver = "Using the default property resolver to deserialize the entity.";

		// Token: 0x0400035E RID: 862
		public const string UsingUserProvidedPropertyResolver = "Using the property resolver provided via TableRequestOptions to deserialize the entity.";

		// Token: 0x0400035F RID: 863
		public const string ALinqCouldNotConvert = "Could not convert constant {0} expression to string.";

		// Token: 0x04000360 RID: 864
		public const string ALinqMethodNotSupported = "The method '{0}' is not supported.";

		// Token: 0x04000361 RID: 865
		public const string ALinqUnaryNotSupported = "The unary operator '{0}' is not supported.";

		// Token: 0x04000362 RID: 866
		public const string ALinqBinaryNotSupported = "The binary operator '{0}' is not supported.";

		// Token: 0x04000363 RID: 867
		public const string ALinqConstantNotSupported = "The constant for '{0}' is not supported.";

		// Token: 0x04000364 RID: 868
		public const string ALinqTypeBinaryNotSupported = "An operation between an expression and a type is not supported.";

		// Token: 0x04000365 RID: 869
		public const string ALinqConditionalNotSupported = "The conditional expression is not supported.";

		// Token: 0x04000366 RID: 870
		public const string ALinqParameterNotSupported = "The parameter expression is not supported.";

		// Token: 0x04000367 RID: 871
		public const string ALinqMemberAccessNotSupported = "The member access of '{0}' is not supported.";

		// Token: 0x04000368 RID: 872
		public const string ALinqLambdaNotSupported = "Lambda Expressions not supported.";

		// Token: 0x04000369 RID: 873
		public const string ALinqNewNotSupported = "New Expressions not supported.";

		// Token: 0x0400036A RID: 874
		public const string ALinqMemberInitNotSupported = "Member Init Expressions not supported.";

		// Token: 0x0400036B RID: 875
		public const string ALinqListInitNotSupported = "List Init Expressions not supported.";

		// Token: 0x0400036C RID: 876
		public const string ALinqNewArrayNotSupported = "New Array Expressions not supported.";

		// Token: 0x0400036D RID: 877
		public const string ALinqInvocationNotSupported = "Invocation Expressions not supported.";

		// Token: 0x0400036E RID: 878
		public const string ALinqUnsupportedExpression = "The expression type {0} is not supported.";

		// Token: 0x0400036F RID: 879
		public const string ALinqCanOnlyProjectTheLeaf = "Can only project the last entity type in the query being translated.";

		// Token: 0x04000370 RID: 880
		public const string ALinqCantCastToUnsupportedPrimitive = "Can't cast to unsupported type '{0}'";

		// Token: 0x04000371 RID: 881
		public const string ALinqCantTranslateExpression = "The expression {0} is not supported.";

		// Token: 0x04000372 RID: 882
		public const string ALinqCantNavigateWithoutKeyPredicate = "Navigation properties can only be selected from a single resource. Specify a key predicate to restrict the entity set to a single instance.";

		// Token: 0x04000373 RID: 883
		public const string ALinqCantReferToPublicField = "Referencing public field '{0}' not supported in query option expression.  Use public property instead.";

		// Token: 0x04000374 RID: 884
		public const string ALinqCannotConstructKnownEntityTypes = "Construction of entity type instances must use object initializer with default constructor.";

		// Token: 0x04000375 RID: 885
		public const string ALinqCannotCreateConstantEntity = "Referencing of local entity type instances not supported when projecting results.";

		// Token: 0x04000376 RID: 886
		public const string ALinqExpressionNotSupportedInProjectionToEntity = "Initializing instances of the entity type {0} with the expression {1} is not supported.";

		// Token: 0x04000377 RID: 887
		public const string ALinqExpressionNotSupportedInProjection = "Constructing or initializing instances of the type {0} with the expression {1} is not supported.";

		// Token: 0x04000378 RID: 888
		public const string ALinqProjectionMemberAssignmentMismatch = "Cannot initialize an instance of entity type '{0}' because '{1}' and '{2}' do not refer to the same source entity.";

		// Token: 0x04000379 RID: 889
		public const string ALinqPropertyNamesMustMatchInProjections = "Cannot assign the value from the {0} property to the {1} property.  When projecting results into a entity type, the property names of the source type and the target type must match for the properties being projected.";

		// Token: 0x0400037A RID: 890
		public const string ALinqQueryOptionOutOfOrder = "The {0} query option cannot be specified after the {1} query option.";

		// Token: 0x0400037B RID: 891
		public const string ALinqQueryOptionsOnlyAllowedOnLeafNodes = "Can only specify query options (orderby, where, take, skip) after last navigation.";
	}
}
