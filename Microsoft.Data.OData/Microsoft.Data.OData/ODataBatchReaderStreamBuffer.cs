using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D8 RID: 472
	internal sealed class ODataBatchReaderStreamBuffer
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00032E6C File Offset: 0x0003106C
		internal byte[] Bytes
		{
			get
			{
				return this.bytes;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00032E74 File Offset: 0x00031074
		internal int CurrentReadPosition
		{
			get
			{
				return this.currentReadPosition;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00032E7C File Offset: 0x0003107C
		internal int NumberOfBytesInBuffer
		{
			get
			{
				return this.numberOfBytesInBuffer;
			}
		}

		// Token: 0x17000314 RID: 788
		internal byte this[int index]
		{
			get
			{
				return this.bytes[index];
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00032E90 File Offset: 0x00031090
		internal void SkipTo(int newPosition)
		{
			int num = newPosition - this.currentReadPosition;
			this.currentReadPosition = newPosition;
			this.numberOfBytesInBuffer -= num;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00032EBC File Offset: 0x000310BC
		internal bool RefillFrom(Stream stream, int preserveFrom)
		{
			this.ShiftToBeginning(preserveFrom);
			int count = 8000 - this.numberOfBytesInBuffer;
			int num = stream.Read(this.bytes, this.numberOfBytesInBuffer, count);
			this.numberOfBytesInBuffer += num;
			return num == 0;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00032F04 File Offset: 0x00031104
		internal ODataBatchReaderStreamScanResult ScanForLineEnd(out int lineEndStartPosition, out int lineEndEndPosition)
		{
			bool flag;
			return this.ScanForLineEnd(this.currentReadPosition, 8000, false, out lineEndStartPosition, out lineEndEndPosition, out flag);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00032F28 File Offset: 0x00031128
		internal ODataBatchReaderStreamScanResult ScanForBoundary(IEnumerable<string> boundaries, int maxDataBytesToScan, out int boundaryStartPosition, out int boundaryEndPosition, out bool isEndBoundary, out bool isParentBoundary)
		{
			boundaryStartPosition = -1;
			boundaryEndPosition = -1;
			isEndBoundary = false;
			isParentBoundary = false;
			int num = this.currentReadPosition;
			int num2;
			int num3;
			for (;;)
			{
				switch (this.ScanForBoundaryStart(num, maxDataBytesToScan, out num2, out num3))
				{
				case ODataBatchReaderStreamScanResult.NoMatch:
					return ODataBatchReaderStreamScanResult.NoMatch;
				case ODataBatchReaderStreamScanResult.PartialMatch:
					goto IL_40;
				case ODataBatchReaderStreamScanResult.Match:
					isParentBoundary = false;
					foreach (string boundary in boundaries)
					{
						switch (this.MatchBoundary(num2, num3, boundary, out boundaryStartPosition, out boundaryEndPosition, out isEndBoundary))
						{
						case ODataBatchReaderStreamScanResult.NoMatch:
							boundaryStartPosition = -1;
							boundaryEndPosition = -1;
							isEndBoundary = false;
							isParentBoundary = true;
							break;
						case ODataBatchReaderStreamScanResult.PartialMatch:
							boundaryEndPosition = -1;
							isEndBoundary = false;
							return ODataBatchReaderStreamScanResult.PartialMatch;
						case ODataBatchReaderStreamScanResult.Match:
							return ODataBatchReaderStreamScanResult.Match;
						default:
							throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStreamBuffer_ScanForBoundary));
						}
					}
					num = ((num == num3) ? (num3 + 1) : num3);
					continue;
				}
				break;
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStreamBuffer_ScanForBoundary));
			IL_40:
			boundaryStartPosition = ((num2 < 0) ? num3 : num2);
			return ODataBatchReaderStreamScanResult.PartialMatch;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00033044 File Offset: 0x00031244
		private ODataBatchReaderStreamScanResult ScanForBoundaryStart(int scanStartIx, int maxDataBytesToScan, out int lineEndStartPosition, out int boundaryDelimiterStartPosition)
		{
			int num = this.currentReadPosition + Math.Min(maxDataBytesToScan, this.numberOfBytesInBuffer) - 1;
			int i = scanStartIx;
			while (i <= num)
			{
				char c = (char)this.bytes[i];
				if (c == '\r' || c == '\n')
				{
					lineEndStartPosition = i;
					if (c == '\r' && i == num && maxDataBytesToScan >= this.numberOfBytesInBuffer)
					{
						boundaryDelimiterStartPosition = i;
						return ODataBatchReaderStreamScanResult.PartialMatch;
					}
					boundaryDelimiterStartPosition = ((c == '\r' && this.bytes[i + 1] == 10) ? (i + 2) : (i + 1));
					return ODataBatchReaderStreamScanResult.Match;
				}
				else
				{
					if (c == '-')
					{
						lineEndStartPosition = -1;
						if (i == num && maxDataBytesToScan >= this.numberOfBytesInBuffer)
						{
							boundaryDelimiterStartPosition = i;
							return ODataBatchReaderStreamScanResult.PartialMatch;
						}
						if (this.bytes[i + 1] == 45)
						{
							boundaryDelimiterStartPosition = i;
							return ODataBatchReaderStreamScanResult.Match;
						}
					}
					i++;
				}
			}
			lineEndStartPosition = -1;
			boundaryDelimiterStartPosition = -1;
			return ODataBatchReaderStreamScanResult.NoMatch;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00033100 File Offset: 0x00031300
		private ODataBatchReaderStreamScanResult ScanForLineEnd(int scanStartIx, int maxDataBytesToScan, bool allowLeadingWhitespaceOnly, out int lineEndStartPosition, out int lineEndEndPosition, out bool endOfBufferReached)
		{
			endOfBufferReached = false;
			int num = this.currentReadPosition + Math.Min(maxDataBytesToScan, this.numberOfBytesInBuffer) - 1;
			int i = scanStartIx;
			while (i <= num)
			{
				char c = (char)this.bytes[i];
				if (c == '\r' || c == '\n')
				{
					lineEndStartPosition = i;
					if (c == '\r' && i == num && maxDataBytesToScan >= this.numberOfBytesInBuffer)
					{
						lineEndEndPosition = -1;
						return ODataBatchReaderStreamScanResult.PartialMatch;
					}
					lineEndEndPosition = lineEndStartPosition;
					if (c == '\r' && this.bytes[i + 1] == 10)
					{
						lineEndEndPosition++;
					}
					return ODataBatchReaderStreamScanResult.Match;
				}
				else
				{
					if (allowLeadingWhitespaceOnly && !char.IsWhiteSpace(c))
					{
						lineEndStartPosition = -1;
						lineEndEndPosition = -1;
						return ODataBatchReaderStreamScanResult.NoMatch;
					}
					i++;
				}
			}
			endOfBufferReached = true;
			lineEndStartPosition = -1;
			lineEndEndPosition = -1;
			return ODataBatchReaderStreamScanResult.NoMatch;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x000331A8 File Offset: 0x000313A8
		private ODataBatchReaderStreamScanResult MatchBoundary(int lineEndStartPosition, int boundaryDelimiterStartPosition, string boundary, out int boundaryStartPosition, out int boundaryEndPosition, out bool isEndBoundary)
		{
			boundaryStartPosition = -1;
			boundaryEndPosition = -1;
			int num = this.currentReadPosition + this.numberOfBytesInBuffer - 1;
			int num2 = boundaryDelimiterStartPosition + 2 + boundary.Length + 2 - 1;
			bool flag;
			int matchLength;
			if (num < num2 + 2)
			{
				flag = true;
				matchLength = Math.Min(num, num2) - boundaryDelimiterStartPosition + 1;
			}
			else
			{
				flag = false;
				matchLength = num2 - boundaryDelimiterStartPosition + 1;
			}
			if (this.MatchBoundary(boundary, boundaryDelimiterStartPosition, matchLength, out isEndBoundary))
			{
				boundaryStartPosition = ((lineEndStartPosition < 0) ? boundaryDelimiterStartPosition : lineEndStartPosition);
				if (flag)
				{
					isEndBoundary = false;
					return ODataBatchReaderStreamScanResult.PartialMatch;
				}
				boundaryEndPosition = boundaryDelimiterStartPosition + 2 + boundary.Length - 1;
				if (isEndBoundary)
				{
					boundaryEndPosition += 2;
				}
				int num3;
				int num4;
				bool flag2;
				switch (this.ScanForLineEnd(boundaryEndPosition + 1, 2147483647, true, out num3, out num4, out flag2))
				{
				case ODataBatchReaderStreamScanResult.NoMatch:
					if (flag2)
					{
						if (boundaryStartPosition == 0)
						{
							throw new ODataException(Strings.ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached(8000));
						}
						isEndBoundary = false;
						return ODataBatchReaderStreamScanResult.PartialMatch;
					}
					break;
				case ODataBatchReaderStreamScanResult.PartialMatch:
					if (boundaryStartPosition == 0)
					{
						throw new ODataException(Strings.ODataBatchReaderStreamBuffer_BoundaryLineSecurityLimitReached(8000));
					}
					isEndBoundary = false;
					return ODataBatchReaderStreamScanResult.PartialMatch;
				case ODataBatchReaderStreamScanResult.Match:
					boundaryEndPosition = num4;
					return ODataBatchReaderStreamScanResult.Match;
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReaderStreamBuffer_ScanForBoundary));
				}
			}
			return ODataBatchReaderStreamScanResult.NoMatch;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x000332CC File Offset: 0x000314CC
		private bool MatchBoundary(string boundary, int startIx, int matchLength, out bool isEndBoundary)
		{
			isEndBoundary = false;
			if (matchLength == 0)
			{
				return true;
			}
			int num = 0;
			int num2 = startIx;
			for (int i = -2; i < matchLength - 2; i++)
			{
				if (i < 0)
				{
					if (this.bytes[num2] != 45)
					{
						return false;
					}
				}
				else if (i < boundary.Length)
				{
					if ((char)this.bytes[num2] != boundary[i])
					{
						return false;
					}
				}
				else
				{
					if (this.bytes[num2] != 45)
					{
						return true;
					}
					num++;
				}
				num2++;
			}
			isEndBoundary = (num == 2);
			return true;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00033344 File Offset: 0x00031544
		private void ShiftToBeginning(int startIndex)
		{
			int num = this.currentReadPosition + this.numberOfBytesInBuffer - startIndex;
			this.currentReadPosition = 0;
			if (num <= 0)
			{
				this.numberOfBytesInBuffer = 0;
				return;
			}
			this.numberOfBytesInBuffer = num;
			Buffer.BlockCopy(this.bytes, startIndex, this.bytes, 0, num);
		}

		// Token: 0x04000503 RID: 1283
		internal const int BufferLength = 8000;

		// Token: 0x04000504 RID: 1284
		private const int MaxLineFeedLength = 2;

		// Token: 0x04000505 RID: 1285
		private const int TwoDashesLength = 2;

		// Token: 0x04000506 RID: 1286
		private readonly byte[] bytes = new byte[8000];

		// Token: 0x04000507 RID: 1287
		private int currentReadPosition;

		// Token: 0x04000508 RID: 1288
		private int numberOfBytesInBuffer;
	}
}
