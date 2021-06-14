using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.ZipForge.Encryption;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000087 RID: 135
	internal class DirManager : IItemsHandler
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x000283C0 File Offset: 0x000273C0
		internal bool CheckCDirEnd(ZipCentralDirEnd dirEnd, long pos)
		{
			bool result = false;
			long position = this.FCompressedStream.Position;
			this.FCompressedStream.Position = pos;
			if (this.FCompressedStream.Length - this.FCompressedStream.Position >= (long)ZipCentralDirEnd.SizeOf())
			{
				byte[] array = new byte[ZipCentralDirEnd.SizeOf()];
				this.FCompressedStream.Read(array, 0, ZipCentralDirEnd.SizeOf());
				dirEnd.LoadFromByteArray(array, 0U);
				result = ((ulong)dirEnd.CommentLength <= (ulong)(this.FCompressedStream.Length - this.FCompressedStream.Position));
			}
			this.FCompressedStream.Position = position;
			return result;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00028464 File Offset: 0x00027464
		internal bool GetCentralDirEnd(ref ZipCentralDirEnd dirEnd, ref long position)
		{
			position = -1L;
			dirEnd.Reset();
			byte[] bytes = BitConverter.GetBytes(this.FArc.GetCentralDirEndSignature());
			long num = Math.Min(65600L, this.FCompressedStream.Length);
			int num2 = (int)num;
			if (num2 == 0)
			{
				return false;
			}
			if (this.FCompressedStream.Length < (long)num2)
			{
				return false;
			}
			byte[] array = new byte[num2];
			this.FCompressedStream.Seek(this.FCompressedStream.Length - (long)num2, SeekOrigin.Begin);
			this.FCompressedStream.Read(array, 0, num2);
			long num3 = (long)(num2 - 1);
			long num4 = -1L;
			while (num3 >= 0L)
			{
				if (array[(int)(checked((IntPtr)num3))] == bytes[3])
				{
					long num5 = 2L;
					while (num5 >= 0L && num3 - (3L - num5) >= 0L && checked(array[(int)((IntPtr)(unchecked(num3 - (3L - num5))))] == bytes[(int)((IntPtr)num5)]))
					{
						num5 -= 1L;
						if (num5 < 0L)
						{
							num4 = num3 - 3L;
						}
					}
					if (num5 < 0L && this.CheckCDirEnd(dirEnd, this.FCompressedStream.Length - (long)num2 + num4))
					{
						position = this.FCompressedStream.Length - (long)num2 + num4;
						break;
					}
				}
				num3 -= 1L;
			}
			if (num4 < 0L || position < 0L || this.FCompressedStream.Length - num4 < (long)ZipCentralDirEnd.SizeOf())
			{
				return false;
			}
			dirEnd.LoadFromByteArray(array, (uint)num4);
			if (dirEnd.CommentLength > 0)
			{
				this.FCompressedStream.Position = position + (long)ZipUtil.ZipCentralDirEndSize;
				byte[] array2 = new byte[(int)dirEnd.CommentLength];
				try
				{
					this.FCompressedStream.Read(array2, 0, (int)dirEnd.CommentLength);
					Encoding encoding = Encoding.GetEncoding(this.OemCodePage);
					this.ArchiveComment = encoding.GetString(array2, 0, array2.Length);
				}
				finally
				{
				}
			}
			return true;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00028638 File Offset: 0x00027638
		private bool CheckFile(DirItem dir, ref string name, long headerPos)
		{
			ZipFileHeader header = default(ZipFileHeader);
			bool result = false;
			long position = this.FCompressedStream.Position;
			this.FCompressedStream.Position = headerPos;
			if (this.FCompressedStream.Length - this.FCompressedStream.Position >= (long)DirItem.LocalHeaderSize())
			{
				byte[] array = new byte[DirItem.LocalHeaderSize()];
				this.FCompressedStream.Read(array, 0, DirItem.LocalHeaderSize());
				header.LoadFromByteArray(array, 0U);
				byte[] array2 = new byte[(int)header.nameLength];
				this.FCompressedStream.Read(array2, 0, (int)header.nameLength);
				name = CompressionUtils.ByteArrayToString(array2, this.OemCodePage);
				result = dir.CheckLocalHeaderItem(header, name);
			}
			this.FCompressedStream.Position = position;
			return result;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000286F8 File Offset: 0x000276F8
		internal void CalculateStubSize(long DirEndPos)
		{
			string text = "";
			if (this.CentralDirEnd.EntriesCentralDir == 0)
			{
				this.StubSize = (int)DirEndPos;
				return;
			}
			byte[] bytes = BitConverter.GetBytes(this.FArc.GetFileHeaderSignature());
			string name = this.ItemsArray[0].Name;
			byte[] array = new byte[65535];
			long relativeLocalHeaderOffset = this.ItemsArray[0].RelativeLocalHeaderOffset;
			this.FCompressedStream.Position = relativeLocalHeaderOffset;
			while (this.FCompressedStream.Position < this.FCompressedStream.Length)
			{
				long position = this.FCompressedStream.Position;
				long num = Math.Min(65535L, this.FCompressedStream.Length - this.FCompressedStream.Position);
				if ((long)this.FCompressedStream.Read(array, 0, (int)num) < num)
				{
					break;
				}
				int num2 = 0;
				long num3 = -1L;
				while ((long)num2 < num)
				{
					num3 = -1L;
					if (array[num2] == bytes[0])
					{
						long num4 = 1L;
						while (num4 <= 3L && (long)num2 + num4 < num && checked(array[(int)((IntPtr)(unchecked((long)num2 + num4)))] == bytes[(int)((IntPtr)num4)]))
						{
							num4 += 1L;
							if (num4 > 3L)
							{
								num3 = (long)num2;
							}
						}
						if (num3 >= 0L)
						{
							if (this.CheckFile(this.ItemsArray[0] as DirItem, ref text, position + num3))
							{
								break;
							}
							num3 = -1L;
						}
					}
					num2++;
				}
				if (num3 >= 0L)
				{
					this.StubSize = (int)(num3 + position - relativeLocalHeaderOffset);
					break;
				}
				this.FCompressedStream.Position = position + num - 4L;
				if (this.FCompressedStream.Position <= position)
				{
					break;
				}
			}
			this.FCompressedStream.Position = relativeLocalHeaderOffset;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00028897 File Offset: 0x00027897
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0002889F File Offset: 0x0002789F
		public IItemsArray ItemsArrayBackup
		{
			get
			{
				return this._itemsArrayBackup;
			}
			set
			{
				this._itemsArrayBackup = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000288A8 File Offset: 0x000278A8
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x000288B0 File Offset: 0x000278B0
		public IItemsArray ItemsArray
		{
			get
			{
				return this._itemsArray;
			}
			set
			{
				this._itemsArray = value;
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000288BC File Offset: 0x000278BC
		public DirManager(Stream stream, bool bCreate, BaseArchiver arc)
		{
			this.FArc = arc;
			this.ItemsArray = new CompressionItemsArray();
			this.ItemsArrayBackup = new CompressionItemsArray();
			this.FCompressedStream = stream;
			this.FIsCustomStream = (this.FCompressedStream != this.FArc._compressedStream);
			this.FZip64 = false;
			this.StubSize = 0;
			this.ArchiveComment = "";
			this.InitCentralDirEnd();
			if (bCreate)
			{
				this.FArc._volumeNumber = 0;
				if (!CompressionUtils.IsNullOrEmpty(this.FArc.SFXStub))
				{
					this.SaveSFXStub();
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00028955 File Offset: 0x00027955
		public bool IsEncrypted()
		{
			return this.ItemsArray.Count > 0 && (this.ItemsArray[0] as DirItem).IsGeneralPurposeFlagBitSet(0);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00028980 File Offset: 0x00027980
		internal void SaveSFXStub()
		{
			FileStream fileStream = new FileStream(this.FArc.SFXStub, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				if (this.FArc.SpanningMode != SpanningMode.None && this.FArc.SpanningOptions.GetVolumeSize(0) != -1L && fileStream.Length > this.FArc.SpanningOptions.GetVolumeSize(0))
				{
					throw ExceptionBuilder.Exception(ErrorCode.CannotFitSFXStubOnVolume);
				}
				CompressionUtils.CopyStream(fileStream, this.FCompressedStream, fileStream.Length);
				this.StubSize = (int)fileStream.Length;
			}
			finally
			{
				fileStream.Close();
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00028A1C File Offset: 0x00027A1C
		internal void InitCentralDirEnd()
		{
			this.CentralDirEnd.Signature = this.FArc.GetCentralDirEndSignature();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00028A34 File Offset: 0x00027A34
		internal long FindLocalFileHeader(long startPosition)
		{
			byte[] array = new byte[4];
			long num = -1L;
			long position = this.FCompressedStream.Position;
			this.FCompressedStream.Position = startPosition;
			array = BitConverter.GetBytes(this.FArc.GetFileHeaderSignature());
			byte[] array2 = new byte[65535];
			while (this.FCompressedStream.Position < this.FCompressedStream.Length)
			{
				long position2 = this.FCompressedStream.Position;
				long num2 = Math.Min(65535L, this.FCompressedStream.Length - this.FCompressedStream.Position);
				this.FCompressedStream.Read(array2, 0, (int)num2);
				int num3 = 0;
				long num4 = -1L;
				while ((long)num3 < num2)
				{
					num4 = -1L;
					if (array2[num3] == array[0])
					{
						long num5 = 1L;
						while (num5 <= 3L && (long)num3 + num5 < num2 && checked(array2[(int)((IntPtr)(unchecked((long)num3 + num5)))] == array[(int)((IntPtr)num5)]))
						{
							num5 += 1L;
							if (num5 > 3L)
							{
								num4 = (long)num3;
							}
						}
						if (num4 >= 0L)
						{
							break;
						}
					}
					num3++;
				}
				if (num4 >= 0L)
				{
					num = num4 + position2;
					break;
				}
				this.FCompressedStream.Position = position2 + num2 - 4L;
				if (this.FCompressedStream.Position <= position2)
				{
					break;
				}
			}
			if (num >= 0L && this.FCompressedStream.Length - num < (long)DirItem.LocalHeaderSize())
			{
				num = -1L;
			}
			this.FCompressedStream.Position = position;
			return num;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00028BA0 File Offset: 0x00027BA0
		internal bool IsStubOffsetApplied()
		{
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				if ((this.ItemsArray[i] as DirItem).CompressedSize > 0L)
				{
					return this.ItemsArray[i].RelativeLocalHeaderOffset >= (long)this.StubSize;
				}
			}
			return true;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00028BFC File Offset: 0x00027BFC
		private void BuildCentralDirByLocalHeaders(ref byte[] buf)
		{
			long num = this.FindLocalFileHeader(0L);
			if (num < 0L)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat);
			}
			while (num >= 0L)
			{
				this.FCompressedStream.Position = num;
				byte[] array = new byte[DirItem.LocalHeaderSize()];
				this.FCompressedStream.Read(array, 0, DirItem.LocalHeaderSize());
				DirItem dirItem = new DirItem();
				dirItem.LoadLocalHeaderFromByteArray(array, 0U);
				dirItem.DiskStartNumber = 0U;
				dirItem.InternalAttributes = 0;
				if (dirItem.UncompressedSize == 0L)
				{
					dirItem.ExternalAttributes = FileAttributes.Directory;
				}
				else
				{
					dirItem.ExternalAttributes = FileAttributes.Archive;
				}
				dirItem.RelativeLocalHeaderOffset = (long)((ulong)((uint)num));
				ushort num2 = dirItem.NameLengthToRead;
				if (num2 > 0 && this.FCompressedStream.Length - this.FCompressedStream.Position >= (long)((ulong)num2))
				{
					array = new byte[(int)num2];
					this.FCompressedStream.Read(array, 0, (int)num2);
					Array.Copy(array, 0, buf, 0, array.Length);
					if (this.FArc.IsFlexCompress() && (this.CentralDirEnd.Signature == this.FArc.GetCentralDirEndSignature() || dirItem.ExtractVersion == 16660) && dirItem.IsGeneralPurposeFlagBitSet(0))
					{
						this.FArc.FXCDecryptFilename(ref buf, (int)num2, this.FArc.GetCentralDirEndSignature().ToString());
					}
					if (dirItem.IsGeneralPurposeFlagBitSet(11))
					{
						dirItem.Name = Encoding.UTF8.GetString(array);
					}
					else
					{
						dirItem.Name = Encoding.GetEncoding(this.OemCodePage).GetString(array);
					}
				}
				else
				{
					dirItem.Name = "";
				}
				dirItem.Name.Replace('\\', '/');
				num2 = dirItem.ExtraFieldsLenRead;
				dirItem.EncryptionAlgorithm = ZipForgeCryptoTransformFactory.GetEncryptionAlgorithm(dirItem);
				if (num2 > 0)
				{
					this.FCompressedStream.Seek((long)((ulong)num2), SeekOrigin.Current);
				}
				num = this.FindLocalFileHeader(num + 4L);
				this.ItemsArray.AddItem(dirItem);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00028DD0 File Offset: 0x00027DD0
		private bool GetZip64CentralDirEndLocator(ref long curPos)
		{
			bool result = false;
			if (curPos > (long)ZipUtil.Zip64CentralDirEndLocatorSize)
			{
				this.FCompressedStream.Position = curPos - (long)ZipUtil.Zip64CentralDirEndLocatorSize;
				byte[] array = new byte[ZipUtil.Zip64CentralDirEndLocatorSize];
				this.FCompressedStream.Read(array, 0, ZipUtil.Zip64CentralDirEndLocatorSize);
				this.Zip64CentralDirEndLocator.LoadFromByteArray(array, 0U);
				if (this.Zip64CentralDirEndLocator.Signature == ZipUtil.Zip64CentralDirEndLocatorSignature)
				{
					result = true;
					curPos -= (long)ZipUtil.Zip64CentralDirEndLocatorSize;
				}
			}
			return result;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00028E48 File Offset: 0x00027E48
		private bool GetZip64CentralDirEnd(ref long curPos)
		{
			bool result = false;
			if (curPos > (long)ZipUtil.Zip64CentralDirEndSize)
			{
				this.FCompressedStream.Position = this.Zip64CentralDirEndLocator.OffsetStartDirEnd;
				byte[] array = new byte[ZipUtil.Zip64CentralDirEndSize];
				this.FCompressedStream.Read(array, 0, ZipUtil.Zip64CentralDirEndSize);
				this.Zip64CentralDirEnd.LoadFromByteArray(array, 0U);
				long num = (long)((ulong)ZipUtil.Zip64CentralDirEndSignature);
				if (num == (long)((ulong)this.Zip64CentralDirEnd.Signature))
				{
					result = true;
					curPos -= (long)ZipUtil.Zip64CentralDirEndSize;
				}
			}
			return result;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00028EC8 File Offset: 0x00027EC8
		public void LoadItemsArray()
		{
			ZipCentralDirEnd centralDirEnd = default(ZipCentralDirEnd);
			long num = 0L;
			byte[] array = new byte[257];
			this.InitCentralDirEnd();
			this.StubSize = 0;
			if (!this.GetCentralDirEnd(ref centralDirEnd, ref num))
			{
				if (this.FArc.SpanningMode == SpanningMode.None)
				{
					this.BuildCentralDirByLocalHeaders(ref array);
					return;
				}
				throw ExceptionBuilder.Exception(ErrorCode.CannotOpenArchiveFile);
			}
			else
			{
				this.CentralDirEnd = centralDirEnd;
				this.FZip64 = this.GetZip64CentralDirEndLocator(ref num);
				if (this.FZip64 && !this.GetZip64CentralDirEnd(ref num))
				{
					throw ExceptionBuilder.Exception(ErrorCode.UnableToFindZip64DirEnd);
				}
				uint num2;
				uint num3;
				if (this.FZip64)
				{
					num2 = this.Zip64CentralDirEnd.StartDiskNumber;
					num3 = this.Zip64CentralDirEndLocator.TotalNumberOfDisks - 1U;
				}
				else
				{
					num2 = (uint)this.CentralDirEnd.StartDiskNumber;
					num3 = (uint)this.CentralDirEnd.DiskNumber;
				}
				if (this.FArc.SpanningMode != SpanningMode.None)
				{
					this.OpenVolume(false, false, (int)num2, (int)num3);
				}
				int num4;
				if (this.FZip64)
				{
					if (this.FArc.SpanningMode == SpanningMode.None)
					{
						this.FCompressedStream.Position = num - this.Zip64CentralDirEnd.CentralDirSize;
					}
					else
					{
						this.FCompressedStream.Position = this.Zip64CentralDirEnd.OffsetStartDir;
					}
					num4 = (int)this.Zip64CentralDirEnd.EntriesCentralDir;
				}
				else
				{
					if (this.FArc.SpanningMode == SpanningMode.None)
					{
						this.FCompressedStream.Position = num - (long)centralDirEnd.CentralDirSize;
					}
					else
					{
						this.FCompressedStream.Position = (long)((ulong)centralDirEnd.OffsetStartDir);
					}
					this.ItemsArray.Clear();
					num4 = (int)centralDirEnd.EntriesCentralDir;
				}
				new DirItem();
				for (int i = 0; i < num4; i++)
				{
					DirItem dirItem = new DirItem();
					if (this.FCompressedStream.Length - this.FCompressedStream.Position < (long)DirItem.CentralDirSize() && this.FArc.SpanningMode != SpanningMode.None)
					{
						num2 += 1U;
						this.OpenVolume(false, false, (int)num2, (int)num3);
					}
					dirItem.RelativeCentralDirectoryOffset = this.FCompressedStream.Position;
					byte[] array2 = new byte[DirItem.CentralDirSize()];
					this.FCompressedStream.Read(array2, 0, DirItem.CentralDirSize());
					dirItem.LoadCentralDirFromByteArray(array2, 0U);
					long num5 = (long)((ulong)dirItem.NameLengthToRead);
					if (num5 > 0L)
					{
						array2 = new byte[num5];
						this.FCompressedStream.Read(array2, 0, (int)num5);
						if (this.FArc.IsFlexCompress() && (centralDirEnd.Signature == this.FArc.GetCentralDirEndSignature() || dirItem.ExtractVersion == 16660) && dirItem.IsGeneralPurposeFlagBitSet(0))
						{
							this.FArc.FXCDecryptFilename(ref array2, array2.Length, this.FArc.GetCentralDirEndSignature().ToString());
						}
						if (dirItem.IsGeneralPurposeFlagBitSet(11))
						{
							dirItem.Name = CompressionUtils.RemoveEscapeSequences(Encoding.UTF8.GetString(array2, 0, array2.Length));
						}
						else
						{
							dirItem.Name = CompressionUtils.RemoveEscapeSequences(Encoding.GetEncoding(this.OemCodePage).GetString(array2, 0, array2.Length));
						}
						dirItem.SrcFileName = dirItem.Name;
					}
					else
					{
						dirItem.Name = "";
					}
					this.ItemsArray.AddItem(dirItem);
					num5 = (long)((ulong)(this._itemsArray[i] as DirItem).ExtraFieldsLenRead);
					(this._itemsArray[i] as DirItem).LoadExtraFieldsFromStream(this.FCompressedStream, (ushort)num5);
					num5 = (long)((ulong)(this._itemsArray[i] as DirItem).CommentLengthToRead);
					if (num5 > 0L)
					{
						array2 = new byte[num5];
						this.FCompressedStream.Read(array2, 0, (int)num5);
						if ((this._itemsArray[i] as DirItem).IsGeneralPurposeFlagBitSet(11))
						{
							(this._itemsArray[i] as DirItem).Comment = Encoding.UTF8.GetString(array2, 0, array2.Length);
						}
						else
						{
							(this._itemsArray[i] as DirItem).Comment = Encoding.GetEncoding(this.OemCodePage).GetString(array2, 0, array2.Length);
						}
					}
					else
					{
						(this._itemsArray[i] as DirItem).Comment = string.Empty;
					}
					(this._itemsArray[i] as DirItem).EncryptionAlgorithm = ZipForgeCryptoTransformFactory.GetEncryptionAlgorithm(this._itemsArray[i] as DirItem);
				}
				if (num3 == 0U)
				{
					this.CalculateStubSize(num);
				}
				if (!this.IsStubOffsetApplied())
				{
					this.ApplyStubOffset();
				}
				return;
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002933B File Offset: 0x0002833B
		internal void OpenVolume(bool FirstVolume, bool LastVolume, int VolumeNumber)
		{
			this.OpenVolume(FirstVolume, LastVolume, VolumeNumber, -1);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00029347 File Offset: 0x00028347
		internal void OpenVolume(bool FirstVolume, bool LastVolume, int VolumeNumber, int LastVolumeNumber)
		{
			if (!this.FIsCustomStream)
			{
				this.FArc.OpenVolume(FirstVolume, LastVolume, VolumeNumber, LastVolumeNumber);
				this.FCompressedStream = this.FArc._compressedStream;
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00029372 File Offset: 0x00028372
		public void SaveItemsArray()
		{
			this.SaveDir(true);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002937B File Offset: 0x0002837B
		public void SaveItemsArray(Stream stream)
		{
			this.SaveDir(true, stream);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00029385 File Offset: 0x00028385
		public void SaveDir(bool recreateCDirEnd)
		{
			this.SaveDir(recreateCDirEnd, null);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00029390 File Offset: 0x00028390
		internal void SaveZip64CentralDirEnd(long startCentralDir)
		{
			this.Zip64CentralDirEndLocator.OffsetStartDirEnd = this.FCompressedStream.Position;
			this.Zip64CentralDirEnd.Signature = ZipUtil.Zip64CentralDirEndSignature;
			this.Zip64CentralDirEnd.CentralDirEndSize = (long)(ZipUtil.Zip64CentralDirEndSize - 12);
			this.Zip64CentralDirEnd.VersionMadeBy = 45;
			this.Zip64CentralDirEnd.VersionNeededToExtract = 45;
			this.Zip64CentralDirEnd.DiskNumber = 0U;
			this.Zip64CentralDirEnd.StartDiskNumber = 0U;
			this.Zip64CentralDirEnd.EntriesOnDisk = (long)this.ItemsArray.Count;
			this.Zip64CentralDirEnd.EntriesCentralDir = (long)this.ItemsArray.Count;
			this.Zip64CentralDirEnd.CentralDirSize = this.FCompressedStream.Position - startCentralDir;
			this.Zip64CentralDirEnd.OffsetStartDir = startCentralDir;
			this.FCompressedStream.Write(this.Zip64CentralDirEnd.GetBytes(), 0, ZipUtil.Zip64CentralDirEndSize);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00029478 File Offset: 0x00028478
		internal void SaveZip64CentralDirEndLocator()
		{
			this.Zip64CentralDirEndLocator.Signature = ZipUtil.Zip64CentralDirEndLocatorSignature;
			this.Zip64CentralDirEndLocator.StartDiskNumber = 0U;
			this.Zip64CentralDirEndLocator.TotalNumberOfDisks = 1U;
			this.FCompressedStream.Write(this.Zip64CentralDirEndLocator.GetBytes(), 0, ZipUtil.Zip64CentralDirEndLocatorSize);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000294CC File Offset: 0x000284CC
		internal ushort GetCDirZip64ExtraFieldLength(int DirItemNo)
		{
			ushort num = 0;
			if (this.ItemsArray[DirItemNo].UncompressedSize == (long)((ulong)-1))
			{
				num += 8;
			}
			if ((this._itemsArray[DirItemNo] as DirItem).CompressedSize == (long)((ulong)-1))
			{
				num += 8;
			}
			if ((this._itemsArray[DirItemNo] as DirItem).RelativeLocalHeaderOffset == (long)((ulong)-1))
			{
				num += 8;
			}
			if ((this._itemsArray[DirItemNo] as DirItem).DiskStartNumber == 65535U)
			{
				num += 4;
			}
			if (num > 0)
			{
				num += 4;
			}
			return num;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002955F File Offset: 0x0002855F
		internal ushort GetCDirExtraFieldsTotalLength(int DirItemNo)
		{
			return (this._itemsArray[DirItemNo] as DirItem).ExtraFields.GetBytesLength(ExtraFieldsTarget.CentralDirectoryExtraFields);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002957D File Offset: 0x0002857D
		internal void SaveCDirExtraFields(int DirItemNo)
		{
			(this._itemsArray[DirItemNo] as DirItem).ExtraFields.WriteToStream(this.FCompressedStream, 0L, ExtraFieldsTarget.CentralDirectoryExtraFields);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000295A4 File Offset: 0x000285A4
		public void SaveDir(bool recreateCDirEnd, Stream stream)
		{
			long num = 0L;
			ZipCentralDirEnd zipCentralDirEnd = default(ZipCentralDirEnd);
			byte[] buffer = new byte[257];
			if (stream != null)
			{
				this.FCompressedStream = stream;
			}
			string archiveComment = this.ArchiveComment;
			bool flag = false;
			if (!recreateCDirEnd)
			{
				flag = this.GetCentralDirEnd(ref zipCentralDirEnd, ref num);
			}
			if (!flag)
			{
				this.FCompressedStream.Position = this.FCompressedStream.Length;
			}
			else
			{
				this.FCompressedStream.Position = num - (long)((ulong)this.CentralDirEnd.CentralDirSize);
			}
			this.ArchiveComment = archiveComment;
			this.FCompressedStream.SetLength(this.FCompressedStream.Position);
			num = this.FCompressedStream.Position;
			if (this.FArc._volumeNumber < 65535)
			{
				this.CentralDirEnd.StartDiskNumber = (ushort)this.FArc._volumeNumber;
			}
			else
			{
				this.CentralDirEnd.StartDiskNumber = ushort.MaxValue;
				this.Zip64CentralDirEnd.StartDiskNumber = (uint)this.FArc._volumeNumber;
			}
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				DirItem dirItem = this._itemsArray[i] as DirItem;
				int volumeNumber = this.FArc._volumeNumber;
				dirItem.RelativeCentralDirectoryOffset = this.FCompressedStream.Position;
				this.FArc.WriteBufferToStream(dirItem.GetCentralDirBytes(), (long)DirItem.CentralDirSize(), ref this.FCompressedStream, ref volumeNumber, (int)(dirItem.GetExtraFieldsLength(ExtraFieldsTarget.CentralDirectoryExtraFields) + dirItem.NameLength));
				this.FArc._volumeNumber = volumeNumber;
				if (dirItem.IsGeneralPurposeFlagBitSet(11))
				{
					buffer = Encoding.UTF8.GetBytes(dirItem.Name);
				}
				else
				{
					buffer = Encoding.GetEncoding(this.OemCodePage).GetBytes(dirItem.Name);
				}
				if (this.FArc.IsFlexCompress() && (this.CentralDirEnd.Signature == this.FArc.GetCentralDirEndSignature() || dirItem.ExtractVersion == 16660) && dirItem.IsGeneralPurposeFlagBitSet(0))
				{
					this.FArc.FXCEncryptFilename(ref buffer, dirItem.Name.Length, this.FArc.GetCentralDirEndSignature().ToString());
				}
				this.FArc.WriteBufferToStream(buffer, (long)((ulong)dirItem.NameLength), ref this.FCompressedStream, ref volumeNumber);
				this.FArc._volumeNumber = volumeNumber;
				this.SaveCDirExtraFields(i);
				if (dirItem.Comment != "")
				{
					byte[] bytes;
					if (dirItem.IsGeneralPurposeFlagBitSet(11))
					{
						bytes = Encoding.UTF8.GetBytes(dirItem.Comment);
					}
					else
					{
						bytes = Encoding.GetEncoding(this.OemCodePage).GetBytes((this._itemsArray[i] as DirItem).Comment);
					}
					this.FArc.WriteBufferToStream(bytes, (long)bytes.Length, ref this.FCompressedStream, ref volumeNumber);
					this.FArc._volumeNumber = volumeNumber;
				}
			}
			long num2 = this.FCompressedStream.Position - num;
			bool flag2 = this.FZip64;
			if (this.ItemsArray.Count < 65535)
			{
				this.CentralDirEnd.EntriesOnDisk = (ushort)this.ItemsArray.Count;
			}
			else
			{
				this.CentralDirEnd.EntriesOnDisk = ushort.MaxValue;
				flag2 = true;
			}
			if (this.ItemsArray.Count < 65535)
			{
				this.CentralDirEnd.EntriesCentralDir = (ushort)this.ItemsArray.Count;
			}
			else
			{
				this.CentralDirEnd.EntriesCentralDir = ushort.MaxValue;
				flag2 = true;
			}
			if (num2 < (long)((ulong)-1))
			{
				this.CentralDirEnd.CentralDirSize = (uint)num2;
			}
			else
			{
				this.CentralDirEnd.CentralDirSize = uint.MaxValue;
				flag2 = true;
			}
			if (num < (long)((ulong)-1))
			{
				this.CentralDirEnd.OffsetStartDir = (uint)num;
			}
			else
			{
				this.CentralDirEnd.OffsetStartDir = uint.MaxValue;
				flag2 = true;
			}
			if (this.ArchiveComment != null)
			{
				this.CentralDirEnd.CommentLength = (ushort)this.ArchiveComment.Length;
			}
			else
			{
				this.CentralDirEnd.CommentLength = 0;
			}
			if (this.FArc._volumeNumber < 65535)
			{
				this.CentralDirEnd.DiskNumber = (ushort)this.FArc._volumeNumber;
			}
			else
			{
				this.CentralDirEnd.DiskNumber = ushort.MaxValue;
				this.Zip64CentralDirEnd.DiskNumber = (uint)this.FArc._volumeNumber;
				flag2 = true;
			}
			if (this.FCompressedStream.Position >= (long)((ulong)-1) || flag2)
			{
				this.SaveZip64CentralDirEnd(num);
				this.SaveZip64CentralDirEndLocator();
			}
			this.CentralDirEnd.GetBytes();
			this.FCompressedStream.Write(this.CentralDirEnd.GetBytes(), 0, ZipUtil.ZipCentralDirEndSize);
			if (!CompressionUtils.IsNullOrEmpty(this.ArchiveComment))
			{
				byte[] array = new byte[(int)this.CentralDirEnd.CommentLength];
				try
				{
					Array.Copy(CompressionUtils.StringToByteArray(this.ArchiveComment, this.OemCodePage), 0, array, 0, this.ArchiveComment.Length);
					this.FCompressedStream.Write(array, 0, (int)this.CentralDirEnd.CommentLength);
				}
				finally
				{
				}
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00029A94 File Offset: 0x00028A94
		public void ApplyStubOffset()
		{
			for (int i = 0; i < this.ItemsArray.Count; i++)
			{
				if ((this.ItemsArray[i].ExternalAttributes & FileAttributes.Directory) == (FileAttributes)0)
				{
					this.ItemsArray[i].RelativeLocalHeaderOffset += (long)((ulong)this.StubSize);
				}
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00029AEC File Offset: 0x00028AEC
		public bool HasCentralDirEnd()
		{
			long num = 0L;
			if (!this.FArc._isOpenCorruptedArchives)
			{
				return this.GetCentralDirEnd(ref this.CentralDirEnd, ref num);
			}
			this.GetCentralDirEnd(ref this.CentralDirEnd, ref num);
			return true;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00029B28 File Offset: 0x00028B28
		public bool IsSFXArchive()
		{
			return this.ItemsArray.Count > 0 && this.ItemsArray[0].RelativeLocalHeaderOffset > 0L;
		}

		// Token: 0x04000356 RID: 854
		public const ushort Zip64ExtraFieldHeaderId = 1;

		// Token: 0x04000357 RID: 855
		public const ushort UnicodeExtraFieldHeaderId = 21838;

		// Token: 0x04000358 RID: 856
		public int OemCodePage;

		// Token: 0x04000359 RID: 857
		internal Stream FCompressedStream;

		// Token: 0x0400035A RID: 858
		internal bool FIsCustomStream;

		// Token: 0x0400035B RID: 859
		internal BaseArchiver FArc;

		// Token: 0x0400035C RID: 860
		private IItemsArray _itemsArrayBackup;

		// Token: 0x0400035D RID: 861
		private IItemsArray _itemsArray;

		// Token: 0x0400035E RID: 862
		public string ArchiveComment;

		// Token: 0x0400035F RID: 863
		public Zip64CentralDirEndLocator Zip64CentralDirEndLocator;

		// Token: 0x04000360 RID: 864
		public Zip64CentralDirEnd Zip64CentralDirEnd;

		// Token: 0x04000361 RID: 865
		public ZipCentralDirEnd CentralDirEnd;

		// Token: 0x04000362 RID: 866
		public int StubSize;

		// Token: 0x04000363 RID: 867
		public bool FZip64;
	}
}
