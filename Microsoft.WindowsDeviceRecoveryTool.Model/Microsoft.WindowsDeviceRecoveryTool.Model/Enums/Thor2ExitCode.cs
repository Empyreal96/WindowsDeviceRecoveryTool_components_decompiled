using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Enums
{
	// Token: 0x0200002A RID: 42
	public enum Thor2ExitCode : uint
	{
		// Token: 0x040000FF RID: 255
		Thor2AllOk,
		// Token: 0x04000100 RID: 256
		Thor2UnexpectedExit,
		// Token: 0x04000101 RID: 257
		Thor2NotResponding = 103U,
		// Token: 0x04000102 RID: 258
		Thor2ErrorConnectionNotFound = 84000U,
		// Token: 0x04000103 RID: 259
		Thor2ErrorConnectionOpenFailed,
		// Token: 0x04000104 RID: 260
		Thor2ErrorBootToFlashAppFailed,
		// Token: 0x04000105 RID: 261
		Thor2ErrorNoDeviceWithinTimeout,
		// Token: 0x04000106 RID: 262
		Thor2ErrorUefiFlasherInitFailed,
		// Token: 0x04000107 RID: 263
		Thor2ErrorConnectionCloseFailed,
		// Token: 0x04000108 RID: 264
		Thor2ErrorInvalidHandle,
		// Token: 0x04000109 RID: 265
		Thor2ErrorMessageSendFailed,
		// Token: 0x0400010A RID: 266
		Thor2ErrorNoSaharaHandshake,
		// Token: 0x0400010B RID: 267
		Thor2ErrorInvalidArgumets,
		// Token: 0x0400010C RID: 268
		Thor2ErrorBootToNbmFailed,
		// Token: 0x0400010D RID: 269
		Thor2ErrorConnectionChangeFailed,
		// Token: 0x0400010E RID: 270
		Thor2ErrorRebootOfDeviceFailed,
		// Token: 0x0400010F RID: 271
		Thor2ErrorBootToPhoneInfoAppFailed,
		// Token: 0x04000110 RID: 272
		Thor2ErrorVplParseFailed = 84100U,
		// Token: 0x04000111 RID: 273
		Thor2ErrorNoFfuEntryInVpl,
		// Token: 0x04000112 RID: 274
		Thor2ErrorToCommunicateWithDevice,
		// Token: 0x04000113 RID: 275
		Thor2ErrorToCommunicateWithUefiInDevice,
		// Token: 0x04000114 RID: 276
		Thor2ErrorSecureFfuNotSupported,
		// Token: 0x04000115 RID: 277
		Thor2ErrorUefiFileProgrammingFailed,
		// Token: 0x04000116 RID: 278
		Thor2ErrorFileDoesNotFitIntoPartition,
		// Token: 0x04000117 RID: 279
		Thor2ErrorOutOfMemory,
		// Token: 0x04000118 RID: 280
		Thor2ErrorProgrammingFailed,
		// Token: 0x04000119 RID: 281
		Thor2ErrorUnsecureFfuNotSupported,
		// Token: 0x0400011A RID: 282
		Thor2ErrorInvalidGpt,
		// Token: 0x0400011B RID: 283
		Thor2ErrorUnexpectedResult,
		// Token: 0x0400011C RID: 284
		Thor2ErrorInvalidRawMsgReq,
		// Token: 0x0400011D RID: 285
		Thor2ErrorInvalidRawMsgResp,
		// Token: 0x0400011E RID: 286
		Thor2ErrorUnknownMessage,
		// Token: 0x0400011F RID: 287
		Thor2ErrorInvalidSaharaMsgResp,
		// Token: 0x04000120 RID: 288
		Thor2ErrorUnknownDeviceProtocol,
		// Token: 0x04000121 RID: 289
		Thor2ErrorFactoryResetFailed,
		// Token: 0x04000122 RID: 290
		Thor2ErrorRkhNotFoundFromBootImage,
		// Token: 0x04000123 RID: 291
		Thor2ErrorRkhMismatchBetweenBootImageAndDevice,
		// Token: 0x04000124 RID: 292
		Thor2ErrorUefiDoesNotSupportFullNviUpdate,
		// Token: 0x04000125 RID: 293
		Thor2ErrorUefiDoesNotSupportProductCodeUpdate,
		// Token: 0x04000126 RID: 294
		Thor2ErrorUefiCannotFindProductDatFile,
		// Token: 0x04000127 RID: 295
		Thor2ErrorBatteryLevelTooLow,
		// Token: 0x04000128 RID: 296
		Thor2ErrorFfuReadNotFfuFile = 84201U,
		// Token: 0x04000129 RID: 297
		Thor2ErrorFfuReadFileOpenFailed,
		// Token: 0x0400012A RID: 298
		Thor2ErrorFfuReadWrongVersion,
		// Token: 0x0400012B RID: 299
		Thor2ErrorFfuReadCorruptedFfu,
		// Token: 0x0400012C RID: 300
		Thor2ErrorFfuReadAssertionFailed,
		// Token: 0x0400012D RID: 301
		Thor2ErrorFfuReadTooSmallBlockSize,
		// Token: 0x0400012E RID: 302
		Thor2ErrorFfuReadUpdateModeNotSupported,
		// Token: 0x0400012F RID: 303
		Thor2ErrorFfuReadGptHeaderCrcMismatch,
		// Token: 0x04000130 RID: 304
		Thor2ErrorFfuReadGptPartitionEntryArrayCrcMismatch,
		// Token: 0x04000131 RID: 305
		Thor2ErrorFfuReadFileOperationFailed,
		// Token: 0x04000132 RID: 306
		Thor2ErrorFfuReadTerminatedByUser,
		// Token: 0x04000133 RID: 307
		Thor2ErrorFfuReadNotBootFile,
		// Token: 0x04000134 RID: 308
		Thor2ErrorUefiNotSupported,
		// Token: 0x04000135 RID: 309
		Thor2ErrorUefiRdcOrAuthenticationRequired,
		// Token: 0x04000136 RID: 310
		Thor2ErrorUefiInvalidParameter,
		// Token: 0x04000137 RID: 311
		Thor2ErrorUefiProtocolNotFound,
		// Token: 0x04000138 RID: 312
		Thor2ErrorUefiPartNotFound,
		// Token: 0x04000139 RID: 313
		Thor2ErrorUefiEraseFail,
		// Token: 0x0400013A RID: 314
		Thor2ErrorUefiOutOfMemory,
		// Token: 0x0400013B RID: 315
		Thor2ErrorUefiReadFail,
		// Token: 0x0400013C RID: 316
		Thor2ErrorUefiVerifyFail,
		// Token: 0x0400013D RID: 317
		Thor2ErrorInvalidNviFile,
		// Token: 0x0400013E RID: 318
		Thor2ErrorNviWriteError,
		// Token: 0x0400013F RID: 319
		Thor2ErrorJsonOperationError,
		// Token: 0x04000140 RID: 320
		Thor2ErrorInvalidJsonFile,
		// Token: 0x04000141 RID: 321
		Thor2ErrorCertificateOperationFailed,
		// Token: 0x04000142 RID: 322
		Thor2ErrorFileOpenFailed,
		// Token: 0x04000143 RID: 323
		Thor2ErrorWinUsbInvalidParameter,
		// Token: 0x04000144 RID: 324
		Thor2ErrorWinUsbInvalidMutex,
		// Token: 0x04000145 RID: 325
		Thor2ErrorWinUsbStartNotificationFailed,
		// Token: 0x04000146 RID: 326
		Thor2ErrorWinUsbEventCreationFailed,
		// Token: 0x04000147 RID: 327
		Thor2ErrorWinUsbHandleCreationFailed,
		// Token: 0x04000148 RID: 328
		Thor2ErrorWinUsbPreparationFailed,
		// Token: 0x04000149 RID: 329
		Thor2ErrorWinUsbNotInitialized,
		// Token: 0x0400014A RID: 330
		Thor2ErrorWinUsbInvalidPointer,
		// Token: 0x0400014B RID: 331
		Thor2ErrorWinUsbWaitForConnection,
		// Token: 0x0400014C RID: 332
		Thor2ErrorWinUsbForConnect,
		// Token: 0x0400014D RID: 333
		Thor2ErrorWinUsbConnectionLost,
		// Token: 0x0400014E RID: 334
		Thor2ErrorWinUsbMessageSendFailedNotSupported = 84240U,
		// Token: 0x0400014F RID: 335
		Thor2ErrorWinUsbMessageSendFailed,
		// Token: 0x04000150 RID: 336
		Thor2ErrorWinUsbNoMessage,
		// Token: 0x04000151 RID: 337
		Thor2ErrorWinUsbRxThreadCreationFailed,
		// Token: 0x04000152 RID: 338
		Thor2ErrorWinUsbArrivedDeviceThreadCreationFailed,
		// Token: 0x04000153 RID: 339
		Thor2ErrorWinUsbDeviceScannerThreadCreationFailed,
		// Token: 0x04000154 RID: 340
		Thor2ErrorWinUsHighSpeedUsbRequired,
		// Token: 0x04000155 RID: 341
		Thor2ErrorUefiTestsFailed = 84950U,
		// Token: 0x04000156 RID: 342
		Thor2ErrorNotImplemented = 84999U,
		// Token: 0x04000157 RID: 343
		OutOfMemory = 65537U,
		// Token: 0x04000158 RID: 344
		FileErrorInvalidFilePath = 131073U,
		// Token: 0x04000159 RID: 345
		FileSeekError,
		// Token: 0x0400015A RID: 346
		FileReadError,
		// Token: 0x0400015B RID: 347
		DevInvalidMode = 196609U,
		// Token: 0x0400015C RID: 348
		DevTooSmallTransferBuffer,
		// Token: 0x0400015D RID: 349
		DevReportedErrorDuringProgramming,
		// Token: 0x0400015E RID: 350
		DevReturnedResponseWithInvalidId,
		// Token: 0x0400015F RID: 351
		DevNoMemoryCardAvailable,
		// Token: 0x04000160 RID: 352
		DevMemoryCardFileSizeError,
		// Token: 0x04000161 RID: 353
		DevRkhMismatchError,
		// Token: 0x04000162 RID: 354
		DevSbl1NotSigned,
		// Token: 0x04000163 RID: 355
		DevReportedErrorResendForbidden,
		// Token: 0x04000164 RID: 356
		DevUefiNotSigned,
		// Token: 0x04000165 RID: 357
		MsgNoBufferInfoProvided = 262146U,
		// Token: 0x04000166 RID: 358
		MsgUnknownSubBlokInfoQueryResp,
		// Token: 0x04000167 RID: 359
		MsgInvalidSizeInfoQueryResp,
		// Token: 0x04000168 RID: 360
		MsgInvalidSizeFlashResp,
		// Token: 0x04000169 RID: 361
		MsgInvalidSizeSecureFlashResp,
		// Token: 0x0400016A RID: 362
		MsgSecureFlashRespMissingReqSbl,
		// Token: 0x0400016B RID: 363
		MsgInvalidSizeFfuPayloadStatusSbl,
		// Token: 0x0400016C RID: 364
		MsgSecureFlashRespMissingPayloadStatusSbl,
		// Token: 0x0400016D RID: 365
		MsgInvalidSizeAsyncFlashResp,
		// Token: 0x0400016E RID: 366
		MsgInvalidSizeEraseWpDataPartitionResp,
		// Token: 0x0400016F RID: 367
		MsgInvalidReadRkhResp,
		// Token: 0x04000170 RID: 368
		MsgFfuHeaderMessageDoesNotFitIntoMessageBuffer,
		// Token: 0x04000171 RID: 369
		MsgInvalidSizeBackupResp,
		// Token: 0x04000172 RID: 370
		MsgInvalidFlashAppWriteParamResp,
		// Token: 0x04000173 RID: 371
		MsgUnableToSendOrReceiveMessageDuringSffuProgramming,
		// Token: 0x04000174 RID: 372
		UnexpectedExceptionDuringSffuProgramming,
		// Token: 0x04000175 RID: 373
		MsgUnableToSendMessage = 327681U,
		// Token: 0x04000176 RID: 374
		DevReportedErrorDuringSffuProgramming = 393216U,
		// Token: 0x04000177 RID: 375
		DevReportedErrorResendSffuForbidden = 458752U,
		// Token: 0x04000178 RID: 376
		FfuParsingError = 2228224U,
		// Token: 0x04000179 RID: 377
		InvalidFfuReaderVersion,
		// Token: 0x0400017A RID: 378
		DppPartitionHasDataInFfu,
		// Token: 0x0400017B RID: 379
		FfuFileTooBigForDevice,
		// Token: 0x0400017C RID: 380
		FaOk = 4194304000U,
		// Token: 0x0400017D RID: 381
		FaErrOutOfMemory,
		// Token: 0x0400017E RID: 382
		FaErrReadFail,
		// Token: 0x0400017F RID: 383
		FaErrBuEmpty,
		// Token: 0x04000180 RID: 384
		FaErrWriteFail,
		// Token: 0x04000181 RID: 385
		FaErrEraseFail,
		// Token: 0x04000182 RID: 386
		FaErrPartNotFound,
		// Token: 0x04000183 RID: 387
		FaErrInvalidPart,
		// Token: 0x04000184 RID: 388
		FaErrInvalidParameter,
		// Token: 0x04000185 RID: 389
		FaErrProtocolNotFound,
		// Token: 0x04000186 RID: 390
		FaErrNotFound,
		// Token: 0x04000187 RID: 391
		FaErrNotSupported,
		// Token: 0x04000188 RID: 392
		FaErrLoadFail,
		// Token: 0x04000189 RID: 393
		FaErrVerifyFail,
		// Token: 0x0400018A RID: 394
		FaErrInvalidSbId,
		// Token: 0x0400018B RID: 395
		FaErrInvalidSbCount,
		// Token: 0x0400018C RID: 396
		FaErrInvalidSbLength,
		// Token: 0x0400018D RID: 397
		FaErrNssFail,
		// Token: 0x0400018E RID: 398
		FaErrAuthenticationRequired,
		// Token: 0x0400018F RID: 399
		FaErrAsyncMsgSendingFailed,
		// Token: 0x04000190 RID: 400
		FaErrInvalidMsgLength,
		// Token: 0x04000191 RID: 401
		FaErrFileNotFound,
		// Token: 0x04000192 RID: 402
		FaErrFfuInvalidHeaderType = 4194308096U,
		// Token: 0x04000193 RID: 403
		FaErrFfuInvalidHeaderSize,
		// Token: 0x04000194 RID: 404
		FaErrFfuHeaderImportFail,
		// Token: 0x04000195 RID: 405
		FaErrFfuHashFail,
		// Token: 0x04000196 RID: 406
		FaErrFfuHeaderMissing,
		// Token: 0x04000197 RID: 407
		FaErrFfuWrongChunkSize,
		// Token: 0x04000198 RID: 408
		FaErrFfuHashNotFound,
		// Token: 0x04000199 RID: 409
		FaErrFfuPartialPayloadData,
		// Token: 0x0400019A RID: 410
		FaErrFfuNullBlockDataEntry,
		// Token: 0x0400019B RID: 411
		FaErrFfuNullLocationEntry,
		// Token: 0x0400019C RID: 412
		FaErrFfuFlashingNotCompleted,
		// Token: 0x0400019D RID: 413
		FaErrFfuTooMuchPayloadData,
		// Token: 0x0400019E RID: 414
		FaErrFfuSecHdrInvalidSignature = 4194308352U,
		// Token: 0x0400019F RID: 415
		FaErrFfuSecHdrInvalidStrSize,
		// Token: 0x040001A0 RID: 416
		FaErrFfuSecHdrInvalidAlgorithm,
		// Token: 0x040001A1 RID: 417
		FaErrFfuSecHdrInvalidChunkSize,
		// Token: 0x040001A2 RID: 418
		FaErrFfuSecHdrInvalidCatalogSize,
		// Token: 0x040001A3 RID: 419
		FaErrFfuSecHdrInvalidSecHashTableSize,
		// Token: 0x040001A4 RID: 420
		FaErrFfuSecHdrValidationFail,
		// Token: 0x040001A5 RID: 421
		FaErrFfuImgHdrInvalidSignature = 4194308609U,
		// Token: 0x040001A6 RID: 422
		FaErrFfuImgHdrInvalidStrSize,
		// Token: 0x040001A7 RID: 423
		FaErrFfuImgHdrInvalidManifestSize,
		// Token: 0x040001A8 RID: 424
		FaErrFfuImgHdrInvalidChunkSize,
		// Token: 0x040001A9 RID: 425
		FaErrFfuStrHdrInvalidUpdateType = 4194308865U,
		// Token: 0x040001AA RID: 426
		FaErrFfuStrHdrInvalidStrVersion,
		// Token: 0x040001AB RID: 427
		FaErrFfuStrHdrInvalidFfuVersion,
		// Token: 0x040001AC RID: 428
		FaErrFfuStrHdrInvalidPlatformId,
		// Token: 0x040001AD RID: 429
		FaErrFfuStrHdrInvalidBlockSize,
		// Token: 0x040001AE RID: 430
		FaErrFfuStrHdrInvalidWriteDescriptor,
		// Token: 0x040001AF RID: 431
		FaErrFfuStrHdrInvalidValidateDescriptorInfo,
		// Token: 0x040001B0 RID: 432
		FaErrNssAuthInitFail = 4194309121U,
		// Token: 0x040001B1 RID: 433
		FaErrNssAuthFail,
		// Token: 0x040001B2 RID: 434
		FaErrNssAuthVerifyFail,
		// Token: 0x040001B3 RID: 435
		FaErrNssAuthSdTypeFail,
		// Token: 0x040001B4 RID: 436
		ResetProtectionEnabledOnDeviceNotFoundInFfu = 4194308613U,
		// Token: 0x040001B5 RID: 437
		ResetProtectionEnabledOnDeviceLowerInFfu,
		// Token: 0x040001B6 RID: 438
		ApplicationForcedToClose = 4294967295U,
		// Token: 0x040001B7 RID: 439
		UnknownError = 255U,
		// Token: 0x040001B8 RID: 440
		UnknownError2 = 3221225477U,
		// Token: 0x040001B9 RID: 441
		WriteErrorFromFlashApp = 393220U,
		// Token: 0x040001BA RID: 442
		UnknownMessageResponseError = 1313819477U,
		// Token: 0x040001BB RID: 443
		WindowsOsError = 3221225477U,
		// Token: 0x040001BC RID: 444
		WindowsOsError2 = 3221226356U
	}
}
