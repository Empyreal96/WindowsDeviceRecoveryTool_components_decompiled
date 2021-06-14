using System;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066B RID: 1643
	[ComVisible(true)]
	[Guid("0B8732A6-AF74-498c-A251-9DC86B0538B0")]
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class XpsFilter : IFilter, IPersistFile, IPersistStream
	{
		// Token: 0x06006CA5 RID: 27813 RVA: 0x001F41FC File Offset: 0x001F23FC
		IFILTER_FLAGS IFilter.Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147467259);
			}
			if (cAttributes > 0U && aAttributes == null)
			{
				throw new COMException(SR.Get("FilterInitInvalidAttributes"), -2147024809);
			}
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x001F4250 File Offset: 0x001F2450
		STAT_CHUNK IFilter.GetChunk()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			STAT_CHUNK chunk;
			try
			{
				chunk = this._filter.GetChunk();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147215616)
				{
					this.ReleaseResources();
				}
				throw ex;
			}
			return chunk;
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x001F42B0 File Offset: 0x001F24B0
		[SecurityCritical]
		void IFilter.GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			if (pBuffer == IntPtr.Zero)
			{
				throw new NullReferenceException(SR.Get("FilterNullGetTextBufferPointer"));
			}
			if (bufCharacterCount == 0U)
			{
				return;
			}
			if (bufCharacterCount == 1U)
			{
				Marshal.WriteInt16(pBuffer, 0);
				return;
			}
			uint num = bufCharacterCount;
			if (bufCharacterCount > 4096U)
			{
				bufCharacterCount = 4096U;
			}
			uint num2 = bufCharacterCount - 1U;
			bufCharacterCount = num2;
			uint num3 = num2;
			this._filter.GetText(ref bufCharacterCount, pBuffer);
			if (bufCharacterCount > num3)
			{
				throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
			}
			if (num == 2U && Marshal.ReadInt16(pBuffer) == 0)
			{
				bufCharacterCount = 2U;
				this._filter.GetText(ref bufCharacterCount, pBuffer);
				if (bufCharacterCount > 2U)
				{
					throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
				}
				if (bufCharacterCount == 2U)
				{
					short num4 = Marshal.ReadInt16(pBuffer, 2);
					Invariant.Assert(num4 == 0);
					bufCharacterCount = 1U;
				}
			}
			Marshal.WriteInt16(pBuffer, (int)(bufCharacterCount * 2U), 0);
			bufCharacterCount += 1U;
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x001F43AF File Offset: 0x001F25AF
		IntPtr IFilter.GetValue()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			return this._filter.GetValue();
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x001F22DB File Offset: 0x001F04DB
		IntPtr IFilter.BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x001F43D9 File Offset: 0x001F25D9
		void IPersistFile.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06006CAB RID: 27819 RVA: 0x001F43E6 File Offset: 0x001F25E6
		[PreserveSig]
		int IPersistFile.GetCurFile(out string ppszFileName)
		{
			ppszFileName = null;
			if (this._filter == null || this._xpsFileName == null)
			{
				ppszFileName = "*.xps";
				return 1;
			}
			ppszFileName = this._xpsFileName;
			return 0;
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x00016748 File Offset: 0x00014948
		[PreserveSig]
		int IPersistFile.IsDirty()
		{
			return 1;
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x001F4410 File Offset: 0x001F2610
		void IPersistFile.Load(string pszFileName, int dwMode)
		{
			if (pszFileName == null || pszFileName == string.Empty)
			{
				throw new ArgumentException(SR.Get("FileNameNullOrEmpty"), "pszFileName");
			}
			STGM_FLAGS stgm_FLAGS = (STGM_FLAGS)(dwMode & 4096);
			if (stgm_FLAGS == STGM_FLAGS.CREATE)
			{
				throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
			}
			FileMode fileMode = FileMode.Open;
			stgm_FLAGS = (STGM_FLAGS)(dwMode & 3);
			if (stgm_FLAGS == STGM_FLAGS.READ || stgm_FLAGS == STGM_FLAGS.READWRITE)
			{
				FileAccess fileAccess = FileAccess.Read;
				FileShare fileSharing = FileShare.ReadWrite;
				Invariant.Assert(this._package == null || this._encryptedPackage == null);
				this.ReleaseResources();
				this._filter = null;
				this._xpsFileName = null;
				bool flag = EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(pszFileName);
				try
				{
					this._packageStream = XpsFilter.FileToStream(pszFileName, fileMode, fileAccess, fileSharing, 1048576L);
					if (flag)
					{
						this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
						this._filter = new EncryptedPackageFilter(this._encryptedPackage);
					}
					else
					{
						this._package = Package.Open(this._packageStream);
						this._filter = new PackageFilter(this._package);
					}
				}
				catch (IOException ex)
				{
					throw new COMException(ex.Message, -2147215613);
				}
				catch (FileFormatException ex2)
				{
					throw new COMException(ex2.Message, -2147215604);
				}
				finally
				{
					if (this._filter == null)
					{
						this.ReleaseResources();
					}
				}
				this._xpsFileName = pszFileName;
				return;
			}
			throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x001F4590 File Offset: 0x001F2790
		void IPersistFile.Save(string pszFileName, bool fRemember)
		{
			throw new COMException(SR.Get("FilterIPersistFileIsReadOnly"), -2147286781);
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x00002137 File Offset: 0x00000337
		void IPersistFile.SaveCompleted(string pszFileName)
		{
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x001F43D9 File Offset: 0x001F25D9
		void IPersistStream.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x00016748 File Offset: 0x00014948
		[PreserveSig]
		int IPersistStream.IsDirty()
		{
			return 1;
		}

		// Token: 0x06006CB2 RID: 27826 RVA: 0x001F45A8 File Offset: 0x001F27A8
		[SecurityCritical]
		void IPersistStream.Load(IStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Invariant.Assert(this._package == null || this._encryptedPackage == null);
			this.ReleaseResources();
			this._filter = null;
			this._xpsFileName = null;
			try
			{
				this._packageStream = new UnsafeIndexingFilterStream(stream);
				if (EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(this._packageStream))
				{
					this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
					this._filter = new EncryptedPackageFilter(this._encryptedPackage);
				}
				else
				{
					this._package = Package.Open(this._packageStream);
					this._filter = new PackageFilter(this._package);
				}
			}
			catch (IOException ex)
			{
				throw new COMException(ex.Message, -2147215613);
			}
			catch (Exception ex2)
			{
				throw new COMException(ex2.Message, -2147215604);
			}
			finally
			{
				if (this._filter == null)
				{
					this.ReleaseResources();
				}
			}
		}

		// Token: 0x06006CB3 RID: 27827 RVA: 0x001F46AC File Offset: 0x001F28AC
		void IPersistStream.Save(IStream stream, bool fClearDirty)
		{
			throw new COMException(SR.Get("FilterIPersistStreamIsReadOnly"), -2147286781);
		}

		// Token: 0x06006CB4 RID: 27828 RVA: 0x001F46C2 File Offset: 0x001F28C2
		void IPersistStream.GetSizeMax(out long pcbSize)
		{
			throw new NotSupportedException(SR.Get("FilterIPersistFileIsReadOnly"));
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x001F46D4 File Offset: 0x001F28D4
		private void ReleaseResources()
		{
			if (this._encryptedPackage != null)
			{
				this._encryptedPackage.Close();
				this._encryptedPackage = null;
			}
			else if (this._package != null)
			{
				this._package.Close();
				this._package = null;
			}
			if (this._packageStream != null)
			{
				this._packageStream.Close();
				this._packageStream = null;
			}
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x001F4734 File Offset: 0x001F2934
		private static Stream FileToStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileSharing, long maxMemoryStream)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			long length = fileInfo.Length;
			Stream stream = new FileStream(filePath, fileMode, fileAccess, fileSharing);
			if (length < maxMemoryStream)
			{
				MemoryStream memoryStream = new MemoryStream((int)length);
				using (stream)
				{
					PackagingUtilities.CopyStream(stream, memoryStream, length, 4096);
				}
				stream = memoryStream;
			}
			return stream;
		}

		// Token: 0x04003556 RID: 13654
		[ComVisible(false)]
		private static readonly Guid _filterClsid = new Guid(193409702U, 44916, 18828, 162, 81, 157, 200, 107, 5, 56, 176);

		// Token: 0x04003557 RID: 13655
		[ComVisible(false)]
		private IFilter _filter;

		// Token: 0x04003558 RID: 13656
		[ComVisible(false)]
		private Package _package;

		// Token: 0x04003559 RID: 13657
		[ComVisible(false)]
		private EncryptedPackageEnvelope _encryptedPackage;

		// Token: 0x0400355A RID: 13658
		[ComVisible(false)]
		private string _xpsFileName;

		// Token: 0x0400355B RID: 13659
		[ComVisible(false)]
		private Stream _packageStream;

		// Token: 0x0400355C RID: 13660
		[ComVisible(false)]
		private const int _int16Size = 2;

		// Token: 0x0400355D RID: 13661
		[ComVisible(false)]
		private const uint _maxTextBufferSizeInCharacters = 4096U;

		// Token: 0x0400355E RID: 13662
		[ComVisible(false)]
		private const int _maxMemoryStreamBuffer = 1048576;
	}
}
