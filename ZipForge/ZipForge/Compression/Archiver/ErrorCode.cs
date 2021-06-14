using System;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000034 RID: 52
	public enum ErrorCode
	{
		// Token: 0x04000151 RID: 337
		UnknownError,
		// Token: 0x04000152 RID: 338
		IndexOutOfBounds,
		// Token: 0x04000153 RID: 339
		InvalidCompressionMode,
		// Token: 0x04000154 RID: 340
		UnexpectedNull,
		// Token: 0x04000155 RID: 341
		InvalidCheckSum,
		// Token: 0x04000156 RID: 342
		BlankFileName,
		// Token: 0x04000157 RID: 343
		FileNotFound,
		// Token: 0x04000158 RID: 344
		ArchiveIsNotOpen,
		// Token: 0x04000159 RID: 345
		StubNotSpecified,
		// Token: 0x0400015A RID: 346
		CannotCreateFile,
		// Token: 0x0400015B RID: 347
		CannotCreateDir,
		// Token: 0x0400015C RID: 348
		NotInUpdate,
		// Token: 0x0400015D RID: 349
		CannotOpenFile,
		// Token: 0x0400015E RID: 350
		InUpdate,
		// Token: 0x0400015F RID: 351
		CannotDeleteFile,
		// Token: 0x04000160 RID: 352
		InMemoryArchiveCanBeCreatedOnly,
		// Token: 0x04000161 RID: 353
		FileIsInReadonlyMode,
		// Token: 0x04000162 RID: 354
		InvalidCompressedSize,
		// Token: 0x04000163 RID: 355
		InvalidFormat,
		// Token: 0x04000164 RID: 356
		CannotCreateOutputFile,
		// Token: 0x04000165 RID: 357
		ArchiveIsOpen,
		// Token: 0x04000166 RID: 358
		UnableToCreateDirectory,
		// Token: 0x04000167 RID: 359
		UnableToFindZip64DirEnd,
		// Token: 0x04000168 RID: 360
		HugeFileModeIsNotEnabled,
		// Token: 0x04000169 RID: 361
		CannotOpenArchiveFile,
		// Token: 0x0400016A RID: 362
		CannotWriteToStream,
		// Token: 0x0400016B RID: 363
		CannotFitSFXStubOnVolume,
		// Token: 0x0400016C RID: 364
		DamagedArchive,
		// Token: 0x0400016D RID: 365
		MakeSFXIsNotAllowed,
		// Token: 0x0400016E RID: 366
		ArchiveAlreadyHasSFXStub,
		// Token: 0x0400016F RID: 367
		MultiVolumeArchiveIsNotAllowed,
		// Token: 0x04000170 RID: 368
		SpanningModificationIsNotAllowed,
		// Token: 0x04000171 RID: 369
		IncorrectPassword,
		// Token: 0x04000172 RID: 370
		InvalidVolumeName,
		// Token: 0x04000173 RID: 371
		InvalidUnicodeExtraFieldSignature,
		// Token: 0x04000174 RID: 372
		InvalidExtraFieldId,
		// Token: 0x04000175 RID: 373
		NoOnRequestBlankVolumeHandler,
		// Token: 0x04000176 RID: 374
		NoOnRequestFirstVolumeHandler,
		// Token: 0x04000177 RID: 375
		NoOnRequestMiddleVolumeHandler,
		// Token: 0x04000178 RID: 376
		NoOnRequestLastVolumeHandler,
		// Token: 0x04000179 RID: 377
		DiskIsFull,
		// Token: 0x0400017A RID: 378
		CompressionEngineIsNotInitialized,
		// Token: 0x0400017B RID: 379
		UnknownCompressionMethod,
		// Token: 0x0400017C RID: 380
		UnknownEncryptionMethod,
		// Token: 0x0400017D RID: 381
		InvalidFileHeaderObject,
		// Token: 0x0400017E RID: 382
		StreamDoesNotSupportWriting,
		// Token: 0x0400017F RID: 383
		StreamDoesNotSupportReading,
		// Token: 0x04000180 RID: 384
		FileNameAlreadyExists,
		// Token: 0x04000181 RID: 385
		ErrorOccursDuringSavingFileStream,
		// Token: 0x04000182 RID: 386
		CompressionFailed,
		// Token: 0x04000183 RID: 387
		UnexpectedNullPointer,
		// Token: 0x04000184 RID: 388
		IncorrectSignatureFound,
		// Token: 0x04000185 RID: 389
		InvalidReadedBytesCount,
		// Token: 0x04000186 RID: 390
		ReadFromStreamFailed,
		// Token: 0x04000187 RID: 391
		WriteToTheStreamFailed,
		// Token: 0x04000188 RID: 392
		ShouldCreateSeparateArchivers,
		// Token: 0x04000189 RID: 393
		CanNotWriteToTheClosedWriter,
		// Token: 0x0400018A RID: 394
		NotAllPrevisiousDataWasRead,
		// Token: 0x0400018B RID: 395
		NotAllHeaderBytesWasRead,
		// Token: 0x0400018C RID: 396
		InvalidTarArchive,
		// Token: 0x0400018D RID: 397
		NameTooLong,
		// Token: 0x0400018E RID: 398
		FileNameWasNotSpecified,
		// Token: 0x0400018F RID: 399
		SpecifiedFileNameIsNullOrEmpty,
		// Token: 0x04000190 RID: 400
		FileNameEmpty,
		// Token: 0x04000191 RID: 401
		CompressedStreamNotSpecified,
		// Token: 0x04000192 RID: 402
		OutputFilesDirNotSpecified
	}
}
