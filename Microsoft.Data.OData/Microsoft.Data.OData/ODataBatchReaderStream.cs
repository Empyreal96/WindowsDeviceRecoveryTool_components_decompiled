using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D9 RID: 473
	internal sealed class ODataBatchReaderStream
	{
		// Token: 0x06000EA4 RID: 3748 RVA: 0x000333A7 File Offset: 0x000315A7
		internal ODataBatchReaderStream(ODataRawInputContext inputContext, string batchBoundary, Encoding batchEncoding)
		{
			this.inputContext = inputContext;
			this.batchBoundary = batchBoundary;
			this.batchEncoding = batchEncoding;
			this.batchBuffer = new ODataBatchReaderStreamBuffer();
			this.lineBuffer = new byte[2000];
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x000333DF File Offset: 0x000315DF
		internal string BatchBoundary
		{
			get
			{
				return this.batchBoundary;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000333E7 File Offset: 0x000315E7
		internal string ChangeSetBoundary
		{
			get
			{
				return this.changesetBoundary;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x000334F8 File Offset: 0x000316F8
		private IEnumerable<string> CurrentBoundaries
		{
			get
			{
				if (this.changesetBoundary != null)
				{
					yield return this.changesetBoundary;
				}
				yield return this.batchBoundary;
				yield break;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00033515 File Offset: 0x00031715
		private Encoding CurrentEncoding
		{
			get
			{
				return this.changesetEncoding ?? this.batchEncoding;
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00033527 File Offset: 0x00031727
		internal void ResetChangeSetBoundary()
		{
			this.changesetBoundary = null;
			this.changesetEncoding = null;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00033538 File Offset: 0x00031738
		internal bool SkipToBoundary(out bool isEndBoundary, out bool isParentBoundary)
		{
			this.EnsureBatchEncoding();
			ODataBatchReaderStreamScanResult odataBatchReaderStreamScanResult = ODataBatchReaderStreamScanResult.NoMatch;
			while (odataBatchReaderStreamScanResult != ODataBatchReaderStreamScanResult.Match)
			{
				int num;
				int num2;
				odataBatchReaderStreamScanResult = this.batchBuffer.ScanForBoundary(this.CurrentBoundaries, int.MaxValue, out num, out num2, out isEndBoundary, out isParentBoundary);
				switch (odataBatchReaderStreamScanResult)
				{
				case ODataBatchReaderStreamScanResult.NoMatch:
					if (this.underlyingStreamExhausted)
					{
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + this.batchBuffer.NumberOfBytesInBuffer);
						return false;
					}
					this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, 8000);
					break;
				case ODataBatchReaderStreamScanResult.PartialMatch:
					if (this.underlyingStreamExhausted)
					{
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + this.batchBuffer.NumberOfBytesInBuffer);
						return false;
					}
					this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, num);
					break;
				case ODataBatchReaderStreamScanResult.Match:
					this.batchBuffer.SkipTo(isParentBoundary ? num : (num2 + 1));
					return true;
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStream_SkipToBoundary));
				}
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStream_SkipToBoundary));
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00033664 File Offset: 0x00031864
		internal int ReadWithDelimiter(byte[] userBuffer, int userBufferOffset, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			int num = count;
			ODataBatchReaderStreamScanResult odataBatchReaderStreamScanResult = ODataBatchReaderStreamScanResult.NoMatch;
			while (num > 0 && odataBatchReaderStreamScanResult != ODataBatchReaderStreamScanResult.Match)
			{
				int num2;
				int num3;
				bool flag;
				bool flag2;
				odataBatchReaderStreamScanResult = this.batchBuffer.ScanForBoundary(this.CurrentBoundaries, num, out num2, out num3, out flag, out flag2);
				switch (odataBatchReaderStreamScanResult)
				{
				case ODataBatchReaderStreamScanResult.NoMatch:
				{
					if (this.batchBuffer.NumberOfBytesInBuffer >= num)
					{
						Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, num);
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + num);
						return count;
					}
					int numberOfBytesInBuffer = this.batchBuffer.NumberOfBytesInBuffer;
					Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, numberOfBytesInBuffer);
					num -= numberOfBytesInBuffer;
					userBufferOffset += numberOfBytesInBuffer;
					if (this.underlyingStreamExhausted)
					{
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + numberOfBytesInBuffer);
						return count - num;
					}
					this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, 8000);
					break;
				}
				case ODataBatchReaderStreamScanResult.PartialMatch:
				{
					if (this.underlyingStreamExhausted)
					{
						int num4 = Math.Min(this.batchBuffer.NumberOfBytesInBuffer, num);
						Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, num4);
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + num4);
						num -= num4;
						return count - num;
					}
					int num5 = num2 - this.batchBuffer.CurrentReadPosition;
					Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, num5);
					num -= num5;
					userBufferOffset += num5;
					this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, num2);
					break;
				}
				case ODataBatchReaderStreamScanResult.Match:
				{
					int num5 = num2 - this.batchBuffer.CurrentReadPosition;
					Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, num5);
					num -= num5;
					userBufferOffset += num5;
					this.batchBuffer.SkipTo(num2);
					return count - num;
				}
				}
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStream_ReadWithDelimiter));
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00033898 File Offset: 0x00031A98
		internal int ReadWithLength(byte[] userBuffer, int userBufferOffset, int count)
		{
			int i = count;
			while (i > 0)
			{
				if (this.batchBuffer.NumberOfBytesInBuffer >= i)
				{
					Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, i);
					this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + i);
					i = 0;
				}
				else
				{
					int numberOfBytesInBuffer = this.batchBuffer.NumberOfBytesInBuffer;
					Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, userBuffer, userBufferOffset, numberOfBytesInBuffer);
					i -= numberOfBytesInBuffer;
					userBufferOffset += numberOfBytesInBuffer;
					if (this.underlyingStreamExhausted)
					{
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStreamBuffer_ReadWithLength));
					}
					this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, 8000);
				}
			}
			return count - i;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0003396C File Offset: 0x00031B6C
		internal bool ProcessPartHeader()
		{
			bool flag;
			ODataBatchOperationHeaders odataBatchOperationHeaders = this.ReadPartHeaders(out flag);
			if (flag)
			{
				this.DetermineChangesetBoundaryAndEncoding(odataBatchOperationHeaders["Content-Type"]);
				if (this.changesetEncoding == null)
				{
					this.changesetEncoding = this.DetectEncoding();
				}
				ReaderValidationUtils.ValidateEncodingSupportedInBatch(this.changesetEncoding);
			}
			return flag;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000339B8 File Offset: 0x00031BB8
		internal ODataBatchOperationHeaders ReadHeaders()
		{
			ODataBatchOperationHeaders odataBatchOperationHeaders = new ODataBatchOperationHeaders();
			string text = this.ReadLine();
			while (text != null && text.Length > 0)
			{
				string text2;
				string value;
				ODataBatchReaderStream.ValidateHeaderLine(text, out text2, out value);
				if (odataBatchOperationHeaders.ContainsKeyOrdinal(text2))
				{
					throw new ODataException(Strings.ODataBatchReaderStream_DuplicateHeaderFound(text2));
				}
				odataBatchOperationHeaders.Add(text2, value);
				text = this.ReadLine();
			}
			return odataBatchOperationHeaders;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00033A10 File Offset: 0x00031C10
		internal string ReadFirstNonEmptyLine()
		{
			for (;;)
			{
				string text = this.ReadLine();
				if (text == null)
				{
					break;
				}
				if (text.Length != 0)
				{
					return text;
				}
			}
			throw new ODataException(Strings.ODataBatchReaderStream_UnexpectedEndOfInput);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00033A3C File Offset: 0x00031C3C
		private static void ValidateHeaderLine(string headerLine, out string headerName, out string headerValue)
		{
			int num = headerLine.IndexOf(':');
			if (num <= 0)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidHeaderSpecified(headerLine));
			}
			headerName = headerLine.Substring(0, num).Trim();
			headerValue = headerLine.Substring(num + 1).Trim();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00033A84 File Offset: 0x00031C84
		private string ReadLine()
		{
			int num = 0;
			byte[] array = this.lineBuffer;
			ODataBatchReaderStreamScanResult odataBatchReaderStreamScanResult = ODataBatchReaderStreamScanResult.NoMatch;
			while (odataBatchReaderStreamScanResult != ODataBatchReaderStreamScanResult.Match)
			{
				int num2;
				int num3;
				odataBatchReaderStreamScanResult = this.batchBuffer.ScanForLineEnd(out num2, out num3);
				switch (odataBatchReaderStreamScanResult)
				{
				case ODataBatchReaderStreamScanResult.NoMatch:
				{
					int num4 = this.batchBuffer.NumberOfBytesInBuffer;
					if (num4 > 0)
					{
						ODataBatchUtils.EnsureArraySize(ref array, num, num4);
						Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, array, num, num4);
						num += num4;
					}
					if (this.underlyingStreamExhausted)
					{
						if (num == 0)
						{
							return null;
						}
						odataBatchReaderStreamScanResult = ODataBatchReaderStreamScanResult.Match;
						this.batchBuffer.SkipTo(this.batchBuffer.CurrentReadPosition + num4);
					}
					else
					{
						this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, 8000);
					}
					break;
				}
				case ODataBatchReaderStreamScanResult.PartialMatch:
				{
					int num4 = num2 - this.batchBuffer.CurrentReadPosition;
					if (num4 > 0)
					{
						ODataBatchUtils.EnsureArraySize(ref array, num, num4);
						Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, array, num, num4);
						num += num4;
					}
					if (this.underlyingStreamExhausted)
					{
						odataBatchReaderStreamScanResult = ODataBatchReaderStreamScanResult.Match;
						this.batchBuffer.SkipTo(num2 + 1);
					}
					else
					{
						this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, num2);
					}
					break;
				}
				case ODataBatchReaderStreamScanResult.Match:
				{
					int num4 = num2 - this.batchBuffer.CurrentReadPosition;
					if (num4 > 0)
					{
						ODataBatchUtils.EnsureArraySize(ref array, num, num4);
						Buffer.BlockCopy(this.batchBuffer.Bytes, this.batchBuffer.CurrentReadPosition, array, num, num4);
						num += num4;
					}
					this.batchBuffer.SkipTo(num3 + 1);
					break;
				}
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStream_ReadLine));
				}
			}
			return this.CurrentEncoding.GetString(array, 0, num);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00033C4B File Offset: 0x00031E4B
		private void EnsureBatchEncoding()
		{
			if (this.batchEncoding == null)
			{
				this.batchEncoding = this.DetectEncoding();
			}
			ReaderValidationUtils.ValidateEncodingSupportedInBatch(this.batchEncoding);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00033C6C File Offset: 0x00031E6C
		private Encoding DetectEncoding()
		{
			while (!this.underlyingStreamExhausted && this.batchBuffer.NumberOfBytesInBuffer < 4)
			{
				this.underlyingStreamExhausted = this.batchBuffer.RefillFrom(this.inputContext.Stream, this.batchBuffer.CurrentReadPosition);
			}
			int numberOfBytesInBuffer = this.batchBuffer.NumberOfBytesInBuffer;
			if (numberOfBytesInBuffer < 2)
			{
				return Encoding.ASCII;
			}
			if (this.batchBuffer[this.batchBuffer.CurrentReadPosition] == 254 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 1] == 255)
			{
				return new UnicodeEncoding(true, true);
			}
			if (this.batchBuffer[this.batchBuffer.CurrentReadPosition] == 255 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 1] == 254)
			{
				if (numberOfBytesInBuffer >= 4 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 2] == 0 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 3] == 0)
				{
					return new UTF32Encoding(false, true);
				}
				return new UnicodeEncoding(false, true);
			}
			else
			{
				if (numberOfBytesInBuffer >= 3 && this.batchBuffer[this.batchBuffer.CurrentReadPosition] == 239 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 1] == 187 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 2] == 191)
				{
					return Encoding.UTF8;
				}
				if (numberOfBytesInBuffer >= 4 && this.batchBuffer[this.batchBuffer.CurrentReadPosition] == 0 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 1] == 0 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 2] == 254 && this.batchBuffer[this.batchBuffer.CurrentReadPosition + 3] == 255)
				{
					return new UTF32Encoding(true, true);
				}
				return Encoding.ASCII;
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00033E7C File Offset: 0x0003207C
		private ODataBatchOperationHeaders ReadPartHeaders(out bool isChangeSetPart)
		{
			ODataBatchOperationHeaders headers = this.ReadHeaders();
			return this.ValidatePartHeaders(headers, out isChangeSetPart);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00033E98 File Offset: 0x00032098
		private ODataBatchOperationHeaders ValidatePartHeaders(ODataBatchOperationHeaders headers, out bool isChangeSetPart)
		{
			string text;
			if (!headers.TryGetValue("Content-Type", out text))
			{
				throw new ODataException(Strings.ODataBatchReaderStream_MissingContentTypeHeader);
			}
			if (MediaTypeUtils.MediaTypeAndSubtypeAreEqual(text, "application/http"))
			{
				isChangeSetPart = false;
				string strA;
				if (!headers.TryGetValue("Content-Transfer-Encoding", out strA) || string.Compare(strA, "binary", StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ODataException(Strings.ODataBatchReaderStream_MissingOrInvalidContentEncodingHeader("Content-Transfer-Encoding", "binary"));
				}
			}
			else
			{
				if (!MediaTypeUtils.MediaTypeStartsWithTypeAndSubtype(text, "multipart/mixed"))
				{
					throw new ODataException(Strings.ODataBatchReaderStream_InvalidContentTypeSpecified("Content-Type", text, "multipart/mixed", "application/http"));
				}
				isChangeSetPart = true;
				if (this.changesetBoundary != null)
				{
					throw new ODataException(Strings.ODataBatchReaderStream_NestedChangesetsAreNotSupported);
				}
			}
			return headers;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00033F40 File Offset: 0x00032140
		private void DetermineChangesetBoundaryAndEncoding(string contentType)
		{
			MediaType mediaType;
			ODataPayloadKind odataPayloadKind;
			MediaTypeUtils.GetFormatFromContentType(contentType, new ODataPayloadKind[]
			{
				ODataPayloadKind.Batch
			}, MediaTypeResolver.DefaultMediaTypeResolver, out mediaType, out this.changesetEncoding, out odataPayloadKind, out this.changesetBoundary);
		}

		// Token: 0x04000509 RID: 1289
		private const int LineBufferLength = 2000;

		// Token: 0x0400050A RID: 1290
		private readonly byte[] lineBuffer;

		// Token: 0x0400050B RID: 1291
		private readonly ODataRawInputContext inputContext;

		// Token: 0x0400050C RID: 1292
		private readonly string batchBoundary;

		// Token: 0x0400050D RID: 1293
		private readonly ODataBatchReaderStreamBuffer batchBuffer;

		// Token: 0x0400050E RID: 1294
		private Encoding batchEncoding;

		// Token: 0x0400050F RID: 1295
		private string changesetBoundary;

		// Token: 0x04000510 RID: 1296
		private Encoding changesetEncoding;

		// Token: 0x04000511 RID: 1297
		private bool underlyingStreamExhausted;
	}
}
