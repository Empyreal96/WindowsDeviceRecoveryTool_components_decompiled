using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000014 RID: 20
	[ToolboxItem(false)]
	public abstract class ArchiverForgeBase : Component, IEnumerator, IEnumerable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600006E RID: 110 RVA: 0x0000BC68 File Offset: 0x0000AC68
		// (remove) Token: 0x0600006F RID: 111 RVA: 0x0000BC81 File Offset: 0x0000AC81
		[Description("Occurs after an application completes opening the archive file.")]
		public event ArchiverForgeBase.OnAfterOpenDelegate OnAfterOpen;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000070 RID: 112 RVA: 0x0000BC9A File Offset: 0x0000AC9A
		// (remove) Token: 0x06000071 RID: 113 RVA: 0x0000BCB3 File Offset: 0x0000ACB3
		[Description("Occurs before application will overwrite existing file.")]
		public event ArchiverForgeBase.OnConfirmOverwriteDelegate OnConfirmOverwrite;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000072 RID: 114 RVA: 0x0000BCCC File Offset: 0x0000ACCC
		// (remove) Token: 0x06000073 RID: 115 RVA: 0x0000BCE5 File Offset: 0x0000ACE5
		[Description("Occurs before archive group operations.")]
		public event ArchiverForgeBase.OnConfirmProcessFileDelegate OnConfirmProcessFile;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000074 RID: 116 RVA: 0x0000BCFE File Offset: 0x0000ACFE
		// (remove) Token: 0x06000075 RID: 117 RVA: 0x0000BD17 File Offset: 0x0000AD17
		[Description("Occurs when archive operation with a file updates a progress indication value.")]
		public event ArchiverForgeBase.OnFileProgressDelegate OnFileProgress;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000076 RID: 118 RVA: 0x0000BD30 File Offset: 0x0000AD30
		// (remove) Token: 0x06000077 RID: 119 RVA: 0x0000BD49 File Offset: 0x0000AD49
		[Description("Occurs when archive operation with a group of files updates a progress indication value.")]
		public event ArchiverForgeBase.OnOverallProgressDelegate OnOverallProgress;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000078 RID: 120 RVA: 0x0000BD62 File Offset: 0x0000AD62
		// (remove) Token: 0x06000079 RID: 121 RVA: 0x0000BD7B File Offset: 0x0000AD7B
		[Description("Occurs in case of failure of the current operation.")]
		public event ArchiverForgeBase.OnProcessFileFailureDelegate OnProcessFileFailure;

		// Token: 0x0600007A RID: 122 RVA: 0x0000BD94 File Offset: 0x0000AD94
		protected ArchiverForgeBase()
		{
			this._oemCodePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._exclusionMasks = new StringCollection();
			this._fileMasks = new StringCollection();
			this._fileName = string.Empty;
			this._archiverOptions = new ArchiverOptions();
			this._isOpenCorruptedArchives = true;
			this._skipFile = false;
			this._baseDir = string.Empty;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000BE01 File Offset: 0x0000AE01
		[Browsable(false)]
		public virtual long FileCount
		{
			get
			{
				return (long)this.GetFileCount();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000BE0A File Offset: 0x0000AE0A
		[Browsable(false)]
		public long Size
		{
			get
			{
				return this.GetArchiveSize();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000BE12 File Offset: 0x0000AE12
		[Browsable(false)]
		public bool InUpdate
		{
			get
			{
				return this.GetInUpdate();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007E RID: 126 RVA: 0x0000BE1A File Offset: 0x0000AE1A
		[Browsable(false)]
		public bool Exists
		{
			get
			{
				return this.GetExists();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000BE22 File Offset: 0x0000AE22
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000BE2A File Offset: 0x0000AE2A
		[Browsable(false)]
		public virtual bool Active
		{
			get
			{
				return this._isOpened;
			}
			set
			{
				this.SetActive(value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000BE33 File Offset: 0x0000AE33
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000BE3B File Offset: 0x0000AE3B
		[Description("Specifies the default path for archive operations.")]
		public string BaseDir
		{
			get
			{
				return this._baseDir;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._baseDir = ((value.IndexOf("..") > -1) ? Path.GetFullPath(value) : value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000BE64 File Offset: 0x0000AE64
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
		[Description("Specifies files to be excluded from archive operations.")]
		public StringCollection ExclusionMasks
		{
			get
			{
				return this._exclusionMasks;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000BE6C File Offset: 0x0000AE6C
		[Description("Specifies files or wildcards for archive operations.")]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public StringCollection FileMasks
		{
			get
			{
				return this._fileMasks;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000BE74 File Offset: 0x0000AE74
		// (set) Token: 0x06000086 RID: 134 RVA: 0x0000BE7C File Offset: 0x0000AE7C
		[Description("Specifies the archive file name.")]
		public virtual string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._fileName = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000BE8F File Offset: 0x0000AE8F
		// (set) Token: 0x06000088 RID: 136 RVA: 0x0000BE97 File Offset: 0x0000AE97
		[Description("Indicates if an archive file is stored in memory.")]
		public bool InMemory
		{
			get
			{
				return this._isInMemory;
			}
			set
			{
				this.SetInMemory(value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000BEA0 File Offset: 0x0000AEA0
		[Description("Gets the number of the OEM code page used to store and restore archive.")]
		public int OemCodePage
		{
			get
			{
				return this._oemCodePage;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000BEA8 File Offset: 0x0000AEA8
		[Description("Specifies the options for archiving operations.")]
		public ArchiverOptions Options
		{
			get
			{
				return this._archiverOptions;
			}
		}

		// Token: 0x0600008B RID: 139
		protected abstract BaseArchiveItem CreateNewArchiveItem();

		// Token: 0x0600008C RID: 140 RVA: 0x0000BEB0 File Offset: 0x0000AEB0
		protected internal int GetFileCount()
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this.CheckInactive();
			int num = 0;
			bool recurse = this._archiverOptions.Recurse;
			try
			{
				this._archiverOptions.Recurse = true;
				if (this.FindFirst("*.*", ref baseArchiveItem, FileAttributes.Directory))
				{
					if ((baseArchiveItem.ExternalFileAttributes & FileAttributes.Directory) != (FileAttributes)0)
					{
						num++;
					}
					while (this.FindNext(ref baseArchiveItem))
					{
						if ((baseArchiveItem.ExternalFileAttributes & FileAttributes.Directory) != (FileAttributes)0)
						{
							num++;
						}
					}
				}
				num = this._itemsHandler.ItemsArray.Count - num;
			}
			finally
			{
				this._archiverOptions.Recurse = recurse;
			}
			return num;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000BF54 File Offset: 0x0000AF54
		public virtual void ChangeFilesAttr(string fileMask, FileAttributes newAttributes)
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this.CheckInactive();
			if (this.FindFirst(fileMask, ref baseArchiveItem))
			{
				this.BeginUpdate();
				try
				{
					do
					{
						this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].ExternalAttributes = newAttributes;
						if (!this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified)
						{
							this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified = true;
							this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation = ProcessOperation.ChangeAttr;
						}
					}
					while (this.FindNext(ref baseArchiveItem));
					this.EndUpdate();
					return;
				}
				catch
				{
					this.CancelUpdate();
					throw;
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileMask
			});
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000C048 File Offset: 0x0000B048
		public virtual void AddFiles()
		{
			this.CheckInactive();
			this.BeginUpdate();
			try
			{
				this.ExclusionMasks.Add(CompressionUtils.GetFullFileName(string.Empty, this._fileName));
				try
				{
					this.InternalAddFiles(this.FileMasks, (uint)this._archiverOptions.SearchAttr, this.ExclusionMasks, false, this._archiverOptions.Recurse);
				}
				finally
				{
					this.ExclusionMasks.Remove(CompressionUtils.GetFullFileName(string.Empty, this._fileName));
				}
				this.EndUpdate();
			}
			catch (Exception)
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000C0F4 File Offset: 0x0000B0F4
		public virtual void AddFiles(string fileMask)
		{
			this.AddFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000C107 File Offset: 0x0000B107
		public virtual void AddFiles(string fileMask, FileAttributes searchAttr)
		{
			this.AddFiles(fileMask, searchAttr, string.Empty);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000C118 File Offset: 0x0000B118
		public virtual void AddFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			this.CheckInactive();
			StringCollection stringCollection = new StringCollection();
			StringCollection stringCollection2 = new StringCollection();
			stringCollection.Add(fileMask);
			if (exclusionMask != null && exclusionMask != string.Empty)
			{
				stringCollection2.Add(exclusionMask);
			}
			stringCollection2.Add(CompressionUtils.GetFullFileName(string.Empty, this._fileName));
			try
			{
				this.BeginUpdate();
				try
				{
					this.InternalAddFiles(stringCollection, (uint)searchAttr, stringCollection2, false, this._archiverOptions.Recurse);
					this.EndUpdate();
				}
				catch
				{
					this.CancelUpdate();
					throw;
				}
			}
			finally
			{
				stringCollection.Clear();
				stringCollection2.Clear();
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000C1C8 File Offset: 0x0000B1C8
		public virtual void AddFromBuffer(string fileName, byte[] buffer, int count)
		{
			this.CheckInactive();
			this._progressEnabled = false;
			this.BeginUpdate();
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			BaseArchiveItem baseArchiveItem2 = baseArchiveItem;
			baseArchiveItem.FileName = fileName;
			baseArchiveItem2.SrcFileName = fileName;
			baseArchiveItem._uncompressedSize = (long)count;
			int num = this.InitializeNewItem(baseArchiveItem);
			if (num >= 0)
			{
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					memoryStream.Write(buffer, 0, count);
					this._itemsHandler.ItemsArray[num].Stream = memoryStream;
					this._itemsHandler.ItemsArray[num].StreamPosition = 0;
					this._itemsHandler.ItemsArray[num].UncompressedSize = memoryStream.Length;
					this._itemsHandler.ItemsArray[num].NeedDestroyStream = true;
					this.EndUpdate();
					return;
				}
				catch
				{
					if (num >= 0 && num < this._itemsHandler.ItemsArray.Count && this._itemsHandler.ItemsArray[num].Stream != null)
					{
						this._itemsHandler.ItemsArray[num].Stream.Close();
					}
					this.CancelUpdate();
					throw;
				}
			}
			this.EndUpdate();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000C2F8 File Offset: 0x0000B2F8
		public virtual void AddFromStream(string fileName, Stream stream)
		{
			this.AddFromStream(fileName, stream, 0L, 0L);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000C306 File Offset: 0x0000B306
		public virtual void AddFromStream(string fileName, Stream stream, long position)
		{
			this.AddFromStream(fileName, stream, position, stream.Length - position);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000C31C File Offset: 0x0000B31C
		public virtual void AddFromStream(string fileName, Stream stream, long position, long count)
		{
			this.CheckInactive();
			this._progressEnabled = false;
			this.BeginUpdate();
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			BaseArchiveItem baseArchiveItem2 = baseArchiveItem;
			baseArchiveItem.FileName = fileName;
			baseArchiveItem2.SrcFileName = fileName;
			baseArchiveItem._uncompressedSize = ((count == 0L) ? stream.Length : count);
			int num = this.InitializeNewItem(baseArchiveItem);
			if (num >= 0)
			{
				if (count == 0L)
				{
					this._itemsHandler.ItemsArray[num].Stream = stream;
					this._itemsHandler.ItemsArray[num].StreamPosition = 0;
					this._itemsHandler.ItemsArray[num].UncompressedSize = stream.Length;
					this._itemsHandler.ItemsArray[num].NeedDestroyStream = false;
					this.EndUpdate();
					return;
				}
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					long position2 = stream.Position;
					stream.Position = position;
					CompressionUtils.CopyStream(stream, memoryStream, count);
					this._itemsHandler.ItemsArray[num].Stream = memoryStream;
					this._itemsHandler.ItemsArray[num].StreamPosition = 0;
					this._itemsHandler.ItemsArray[num].NeedDestroyStream = true;
					this.EndUpdate();
					stream.Position = position2;
					return;
				}
				catch
				{
					if (num >= 0 && num < this._itemsHandler.ItemsArray.Count && this._itemsHandler.ItemsArray[num].Stream != null)
					{
						this._itemsHandler.ItemsArray[num].Stream.Close();
					}
					this.CancelUpdate();
					throw;
				}
			}
			this.EndUpdate();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000C4C4 File Offset: 0x0000B4C4
		public virtual void AddFromString(string fileName, string text)
		{
			this.CheckInactive();
			this._progressEnabled = false;
			this.BeginUpdate();
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			BaseArchiveItem baseArchiveItem2 = baseArchiveItem;
			baseArchiveItem.SrcFileName = fileName;
			baseArchiveItem2.FileName = fileName;
			baseArchiveItem._uncompressedSize = (long)text.Length;
			int num = this.InitializeNewItem(baseArchiveItem);
			if (num >= 0)
			{
				MemoryStream memoryStream = new MemoryStream(CompressionUtils.StringToByteArray(text, this._oemCodePage));
				try
				{
					this._itemsHandler.ItemsArray[num].Stream = memoryStream;
					this._itemsHandler.ItemsArray[num].StreamPosition = 0;
					this._itemsHandler.ItemsArray[num].UncompressedSize = memoryStream.Length;
					this._itemsHandler.ItemsArray[num].NeedDestroyStream = true;
					this.EndUpdate();
					return;
				}
				catch
				{
					if (num >= 0 && num < this._itemsHandler.ItemsArray.Count && this._itemsHandler.ItemsArray[num].Stream != null)
					{
						this._itemsHandler.ItemsArray[num].Stream.Close();
					}
					this.CancelUpdate();
					throw;
				}
			}
			this.EndUpdate();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000C5FC File Offset: 0x0000B5FC
		public virtual void BeginUpdate()
		{
			if (this._updateCount == 0)
			{
				this.Active = true;
				if (this._fileOpenAccess == FileAccess.Read && this._fileOpenShare == FileShare.ReadWrite && !this._isCustomStream)
				{
					throw ExceptionBuilder.Exception(ErrorCode.FileIsInReadonlyMode);
				}
				for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
				{
					this._itemsHandler.ItemsArray[i].IsModified = false;
				}
				this._itemsHandler.ItemsArrayBackup = (IItemsArray)this._itemsHandler.ItemsArray.Clone();
			}
			this._updateCount++;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000C69A File Offset: 0x0000B69A
		public virtual void CancelUpdate()
		{
			if (this._updateCount > 0)
			{
				this._updateCount = 0;
				this._itemsHandler.ItemsArray = (IItemsArray)this._itemsHandler.ItemsArrayBackup.Clone();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000C6CC File Offset: 0x0000B6CC
		public virtual void CloseArchive()
		{
			if (!this._isOpened)
			{
				if (this._compressedStream != null)
				{
					this._compressedStream.Close();
				}
				this._compressedStream = null;
				this._itemsHandler = null;
			}
			else
			{
				this.ForceUpdate();
				if (this._compressedStream != null && this._compressedStream.CanWrite && this._compressedStream.Length == 0L)
				{
					this._itemsHandler.SaveItemsArray();
				}
				this._isOpened = false;
			}
			if (!this._isCustomStream && this._compressedStream != null)
			{
				this._compressedStream.Close();
			}
			this._compressedStream = null;
			this._itemsHandler = null;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000C76C File Offset: 0x0000B76C
		public virtual void DeleteFiles()
		{
			this.CheckInactive();
			this.TagFiles();
			this.BeginUpdate();
			try
			{
				this.ProcessTaggedFiles(ProcessOperation.Delete);
				this.EndUpdate();
			}
			catch
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000C7B4 File Offset: 0x0000B7B4
		public virtual void DeleteFiles(string fileMask)
		{
			this.DeleteFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000C7C7 File Offset: 0x0000B7C7
		protected internal long GetArchiveSize()
		{
			this.CheckInactive();
			return this._compressedStream.Length;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000C7DA File Offset: 0x0000B7DA
		public virtual void DeleteFiles(string fileMask, FileAttributes searchAttributes)
		{
			this.DeleteFiles(fileMask, searchAttributes, string.Empty);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000C7EC File Offset: 0x0000B7EC
		public virtual void DeleteFiles(string fileMask, FileAttributes searchAttributes, string exclusionMask)
		{
			this.CheckInactive();
			this.TagFiles(fileMask, searchAttributes, exclusionMask);
			this.BeginUpdate();
			try
			{
				this.ProcessTaggedFiles(ProcessOperation.Delete);
				this.EndUpdate();
			}
			catch
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000C838 File Offset: 0x0000B838
		public virtual void EndUpdate()
		{
			if (this._updateCount == 1)
			{
				this.ForceUpdate();
				return;
			}
			if (this._updateCount > 1)
			{
				this._updateCount--;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000C861 File Offset: 0x0000B861
		public virtual void ExtractFiles()
		{
			this.CheckInactive();
			this.CheckInUpdate();
			this.TagFiles();
			this.ProcessTaggedFiles(ProcessOperation.Extract);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000C87C File Offset: 0x0000B87C
		public virtual void ExtractFiles(string fileMask)
		{
			this.ExtractFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000C88F File Offset: 0x0000B88F
		public virtual void ExtractFiles(string fileMask, FileAttributes searchAttr)
		{
			this.ExtractFiles(fileMask, searchAttr, string.Empty);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000C8A0 File Offset: 0x0000B8A0
		public virtual void ExtractFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			this.CheckInactive();
			this.CheckInUpdate();
			bool flag = fileMask.IndexOf('*') == -1 && fileMask.IndexOf('?') == -1;
			bool flag2 = CompressionUtils.IsNullOrEmpty(exclusionMask) && searchAttr == (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted);
			if (flag && flag2 && !this.Options.Recurse)
			{
				BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
				baseArchiveItem.Reset();
				if (this.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask))
				{
					this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsTagged = true;
					this.ProcessTaggedFile(ProcessOperation.Extract, baseArchiveItem.Handle.ItemNo);
					return;
				}
			}
			else
			{
				this.TagFiles(fileMask, searchAttr, exclusionMask);
				this.ProcessTaggedFiles(ProcessOperation.Extract);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000C958 File Offset: 0x0000B958
		public virtual long ExtractToBuffer(string fileName, out byte[] buffer)
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this.CheckInactive();
			this.CheckInUpdate();
			this._progressEnabled = false;
			if (this.FindFirst(fileName, ref baseArchiveItem))
			{
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					memoryStream.Position = 0L;
					this.ExtractItem(baseArchiveItem.Handle.ItemNo, memoryStream);
					memoryStream.Position = 0L;
					long length = memoryStream.Length;
					buffer = new byte[length];
					buffer = memoryStream.ToArray();
					return length;
				}
				finally
				{
					memoryStream.Close();
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileName
			});
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000C9F8 File Offset: 0x0000B9F8
		public virtual void ExtractToStream(string fileName, Stream stream)
		{
			this.CheckInactive();
			this.CheckInUpdate();
			this._progressEnabled = false;
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			if (this.FindFirst(fileName, ref baseArchiveItem))
			{
				this.ExtractItem(baseArchiveItem.Handle.ItemNo, stream);
				return;
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileName
			});
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000CA50 File Offset: 0x0000BA50
		public virtual void ExtractToString(string fileName, out string text)
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this.CheckInactive();
			this.CheckInUpdate();
			this._progressEnabled = false;
			if (this.FindFirst(fileName, ref baseArchiveItem))
			{
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					this.ExtractItem(baseArchiveItem.Handle.ItemNo, memoryStream);
					StreamReader streamReader = new StreamReader(memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					text = streamReader.ReadToEnd();
					return;
				}
				finally
				{
					memoryStream.Close();
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileName
			});
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000CAE0 File Offset: 0x0000BAE0
		protected virtual bool FindFirst(ref BaseArchiveItem archiveItem)
		{
			return CentralDirectorySearcher.FindFirst(ref archiveItem, this._archiverOptions.SearchAttr, this._itemsHandler.ItemsArray, this._archiverOptions.Recurse, this.FileMasks, this.ExclusionMasks);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000CB15 File Offset: 0x0000BB15
		protected virtual bool FindFirst(string fileMask, ref BaseArchiveItem archiveItem)
		{
			return CentralDirectorySearcher.FindFirst(fileMask, ref archiveItem, this._itemsHandler.ItemsArray, this._archiverOptions.Recurse);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000CB34 File Offset: 0x0000BB34
		protected virtual bool FindFirst(string fileMask, ref BaseArchiveItem archiveItem, FileAttributes searchAttr)
		{
			return CentralDirectorySearcher.FindFirst(fileMask, ref archiveItem, searchAttr, this._itemsHandler.ItemsArray, this._archiverOptions.Recurse);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000CB54 File Offset: 0x0000BB54
		protected virtual bool FindFirst(string fileMask, ref BaseArchiveItem archiveItem, FileAttributes searchAttr, string exclusionMask)
		{
			return CentralDirectorySearcher.FindFirst(fileMask, ref archiveItem, searchAttr, exclusionMask, this._itemsHandler.ItemsArray, this._archiverOptions.Recurse);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000CB76 File Offset: 0x0000BB76
		protected virtual bool FindNext(ref BaseArchiveItem archiveItem)
		{
			return CentralDirectorySearcher.FindNext(ref archiveItem, this._itemsHandler.ItemsArray, this._archiverOptions.Recurse, this.FileMasks, this.ExclusionMasks);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000CBA0 File Offset: 0x0000BBA0
		public virtual void MoveFiles()
		{
			this.CheckInactive();
			this.BeginUpdate();
			try
			{
				this.InternalAddFiles(this.FileMasks, (uint)this._archiverOptions.SearchAttr, this.ExclusionMasks, true, this._archiverOptions.Recurse);
				this.EndUpdate();
			}
			catch
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000CC04 File Offset: 0x0000BC04
		public virtual void MoveFiles(string fileMask)
		{
			this.MoveFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000CC17 File Offset: 0x0000BC17
		public virtual void MoveFiles(string fileMask, FileAttributes searchAttr)
		{
			this.MoveFiles(fileMask, searchAttr, string.Empty);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000CC28 File Offset: 0x0000BC28
		public virtual void MoveFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			this.CheckInactive();
			StringCollection stringCollection = new StringCollection();
			StringCollection stringCollection2 = new StringCollection();
			stringCollection.Add(fileMask);
			if (exclusionMask != string.Empty)
			{
				stringCollection2.Add(exclusionMask);
			}
			stringCollection2.Add(CompressionUtils.GetFullFileName(string.Empty, this._fileName));
			try
			{
				this.BeginUpdate();
				try
				{
					this.InternalAddFiles(stringCollection, (uint)searchAttr, stringCollection2, true, this._archiverOptions.Recurse);
					this.EndUpdate();
				}
				catch
				{
					this.CancelUpdate();
					throw;
				}
			}
			finally
			{
				stringCollection.Clear();
				stringCollection2.Clear();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000CCD4 File Offset: 0x0000BCD4
		public virtual void OpenArchive()
		{
			if (!this._isInMemory && Path.GetFileName(this._fileName) == string.Empty)
			{
				throw ExceptionBuilder.Exception(ErrorCode.BlankFileName);
			}
			if (this._isInMemory || !File.Exists(this._fileName))
			{
				this._fileOpenMode = FileMode.Create;
			}
			else
			{
				FileAttributes attributes = FileUtils.GetAttributes(this._fileName);
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					this._fileOpenAccess = FileAccess.Read;
					this._fileOpenShare = FileShare.None;
					this._fileOpenMode = FileMode.Open;
				}
				else
				{
					this._fileOpenAccess = FileAccess.ReadWrite;
					this._fileOpenShare = FileShare.Write;
					this._fileOpenMode = FileMode.Open;
				}
			}
			this.OpenArchive(this._fileOpenMode);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000CD6F File Offset: 0x0000BD6F
		public virtual void OpenArchive(FileMode fileMode)
		{
			this.OpenArchive(fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000CD7A File Offset: 0x0000BD7A
		public virtual void OpenArchive(FileMode fileMode, FileAccess fileAccess)
		{
			this.OpenArchive(fileMode, fileAccess, FileShare.ReadWrite);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000CD88 File Offset: 0x0000BD88
		public virtual void OpenArchive(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
		{
			if (!this._isInMemory && Path.GetFileName(this._fileName) == string.Empty)
			{
				throw ExceptionBuilder.Exception(ErrorCode.BlankFileName);
			}
			if (fileMode != FileMode.Create && fileMode != FileMode.OpenOrCreate && fileMode != FileMode.CreateNew && !File.Exists(this._fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
				{
					this._fileName
				});
			}
			if (fileMode != FileMode.Create && this._isInMemory)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InMemoryArchiveCanBeCreatedOnly, new object[]
				{
					this._fileName
				});
			}
			if (fileMode == FileMode.CreateNew && File.Exists(this._fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.BlankFileName, new object[]
				{
					this._fileName
				});
			}
			this.CloseArchive();
			this._isCustomStream = false;
			this._fileOpenMode = fileMode;
			this._fileOpenAccess = fileAccess;
			this._fileOpenShare = fileShare;
			this.InternalOpenArchive();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000CE64 File Offset: 0x0000BE64
		public virtual void OpenArchive(Stream stream, bool create)
		{
			this.CloseArchive();
			this._isCustomStream = true;
			if (create)
			{
				stream.SetLength(0L);
			}
			this._itemsHandler = this.CreateNewItemsHandler(stream, create);
			this._compressedStream = stream;
			if (create)
			{
				this._itemsHandler.SaveItemsArray();
			}
			else
			{
				this._itemsHandler.LoadItemsArray();
			}
			this._isOpened = true;
			this.DoAfterOpen();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000CEC6 File Offset: 0x0000BEC6
		public virtual void TestFiles()
		{
			this.CheckInactive();
			this.CheckInUpdate();
			this.TagFiles();
			this.ProcessTaggedFiles(ProcessOperation.Test);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000CEE1 File Offset: 0x0000BEE1
		public virtual void TestFiles(string fileMask)
		{
			this.TestFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000CEF4 File Offset: 0x0000BEF4
		public virtual void TestFiles(string fileMask, FileAttributes searchAttr)
		{
			this.TestFiles(fileMask, searchAttr, string.Empty);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000CF03 File Offset: 0x0000BF03
		public virtual void TestFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			this.CheckInactive();
			this.CheckInUpdate();
			this.TagFiles(fileMask, searchAttr, exclusionMask);
			this.ProcessTaggedFiles(ProcessOperation.Test);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000CF24 File Offset: 0x0000BF24
		public virtual void UpdateFiles()
		{
			this.CheckInactive();
			this.TagFiles();
			this.BeginUpdate();
			try
			{
				this.ProcessTaggedFiles(ProcessOperation.Update);
				this.EndUpdate();
			}
			catch
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000CF6C File Offset: 0x0000BF6C
		public virtual void UpdateFiles(string fileMask)
		{
			this.UpdateFiles(fileMask, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000CF7F File Offset: 0x0000BF7F
		public virtual void UpdateFiles(string fileMask, FileAttributes searchAttr)
		{
			this.UpdateFiles(fileMask, searchAttr, string.Empty);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000CF90 File Offset: 0x0000BF90
		public virtual void UpdateFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			this.CheckInactive();
			this.TagFiles(fileMask, searchAttr, exclusionMask);
			this.BeginUpdate();
			try
			{
				this.ProcessTaggedFiles(ProcessOperation.Update);
				this.EndUpdate();
			}
			catch
			{
				this.CancelUpdate();
				throw;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000CFDC File Offset: 0x0000BFDC
		protected internal virtual void AddFromFileRest(int itemNo, Stream backupFileStream, long backupOffset, Stream compressedStream)
		{
			int itemNoInBackupArray = this.GetItemNoInBackupArray(itemNo);
			if (itemNoInBackupArray == -1)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			if (this._itemsHandler.ItemsArray[itemNo].Operation == ProcessOperation.Update)
			{
				this._itemsHandler.ItemsArray[itemNo] = this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray];
			}
			long offset = this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray].RelativeLocalHeaderOffset - backupOffset;
			this._itemsHandler.ItemsArray[itemNo].RelativeLocalHeaderOffset = compressedStream.Position;
			backupFileStream.Seek(offset, SeekOrigin.Begin);
			if (this._itemsHandler.ItemsArray[itemNo].IsModified && (this._itemsHandler.ItemsArray[itemNo].Operation == ProcessOperation.Rename || this._itemsHandler.ItemsArray[itemNo].Operation == ProcessOperation.ChangeComment))
			{
				backupFileStream.Seek((long)this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray].GetDataOffset(), SeekOrigin.Current);
				this.SaveRenamedItemToArchive(backupFileStream, compressedStream, itemNo, itemNoInBackupArray);
				return;
			}
			long count;
			if (itemNoInBackupArray < this._itemsHandler.ItemsArrayBackup.Count - 1)
			{
				count = this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray + 1].RelativeLocalHeaderOffset - backupFileStream.Position - backupOffset;
			}
			else
			{
				count = this.GetEndOfTheDataStreamPosition(itemNo) - backupFileStream.Position - backupOffset;
			}
			CompressionUtils.CopyStream(backupFileStream, compressedStream, count);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000D140 File Offset: 0x0000C140
		protected internal int GetItemNoInBackupArray(int itemNo)
		{
			int result = -1;
			int num = 0;
			while (num < this._itemsHandler.ItemsArrayBackup.Count && itemNo < this._itemsHandler.ItemsArray.Count)
			{
				if (this._itemsHandler.ItemsArrayBackup[num].Name == this._itemsHandler.ItemsArray[itemNo].Name || (this._itemsHandler.ItemsArray[itemNo].Operation == ProcessOperation.Rename && this._itemsHandler.ItemsArrayBackup[num].Name == this._itemsHandler.ItemsArray[itemNo].OldName))
				{
					result = num;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x060000BF RID: 191
		protected abstract void SaveRenamedItemToArchive(Stream backupFileStream, Stream compressedStream, int itemNo, int itemNoInBackupArray);

		// Token: 0x060000C0 RID: 192
		protected abstract long GetEndOfTheDataStreamPosition(int itemNo);

		// Token: 0x060000C1 RID: 193 RVA: 0x0000D208 File Offset: 0x0000C208
		protected internal virtual bool AddFromNewSource(int itemNo, Stream compressedStream, ref FailureAction action)
		{
			Stream stream = null;
			long oldPosition = 0L;
			string empty = string.Empty;
			bool result = false;
			IItem item = this._itemsHandler.ItemsArray[itemNo];
			this._currentItemOperationStartTime = DateTime.Now;
			if (item.Operation == ProcessOperation.Add || item.Operation == ProcessOperation.Move)
			{
				this.DoOnFileProgress(item.SrcFileName, 0.0, new TimeSpan(0L), new TimeSpan(0L), item.Operation, ProgressPhase.Start, ref this._progressCancel);
			}
			else
			{
				this.DoOnFileProgress(item.Name, 0.0, new TimeSpan(0L), new TimeSpan(0L), item.Operation, ProgressPhase.Start, ref this._progressCancel);
			}
			if (this._progressCancel)
			{
				return false;
			}
			try
			{
				oldPosition = 0L;
				if (item.Stream == null)
				{
					if ((item.ExternalAttributes & FileAttributes.Directory) != (FileAttributes)0)
					{
						goto IL_133;
					}
					try
					{
						stream = new FileStream(item.SrcFileName, FileMode.Open, FileAccess.Read, this._archiverOptions.ShareMode);
						goto IL_133;
					}
					catch (Exception innerException)
					{
						throw ExceptionBuilder.Exception(ErrorCode.CannotOpenFile, new object[]
						{
							this._itemsHandler.ItemsArray[itemNo].SrcFileName
						}, innerException);
					}
				}
				stream = item.Stream;
				oldPosition = stream.Position;
				stream.Position = (long)item.StreamPosition;
				IL_133:
				item.RelativeLocalHeaderOffset = compressedStream.Position;
				item.UncompressedSize = (long)((ulong)((stream != null) ? ((uint)stream.Length) : 0U));
				try
				{
					compressedStream.Seek((long)item.GetDataOffset(), SeekOrigin.Current);
					if (stream != null)
					{
						this.InternalCompressFile(stream, compressedStream, item);
					}
					compressedStream.Position = item.RelativeLocalHeaderOffset;
					item.WriteLocalHeaderToStream(compressedStream, 0);
					if (this._progressCancel)
					{
						this.CloseStream(itemNo, ref stream, oldPosition);
						return false;
					}
					compressedStream.Seek(0L, SeekOrigin.End);
				}
				finally
				{
					if (!CompressionUtils.IsNullOrEmpty(empty))
					{
						File.Delete(empty);
					}
				}
				this.CloseStream(itemNo, ref stream, oldPosition);
				if (item.Operation == ProcessOperation.Add || item.Operation == ProcessOperation.Move)
				{
					this.DoOnFileProgress(item.SrcFileName, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), item.Operation, ProgressPhase.End, ref this._progressCancel);
				}
				else
				{
					this.DoOnFileProgress(item.Name, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), item.Operation, ProgressPhase.End, ref this._progressCancel);
				}
				result = true;
			}
			catch (ArchiverException ex)
			{
				this.CloseStream(itemNo, ref stream, oldPosition);
				if (this._compressedStream == null)
				{
					action = FailureAction.Abort;
				}
				else if (item.Operation == ProcessOperation.Update)
				{
					this.DoOnProcessFileFailure(item.Name, item.Operation, ex.ErrorCode, ex.Args, ex.Message, ex.InnerException, ref action);
				}
				else
				{
					this.DoOnProcessFileFailure(item.SrcFileName, item.Operation, ex.ErrorCode, ex.Args, ex.Message, ex, ref action);
				}
			}
			catch
			{
				this.CloseStream(itemNo, ref stream, oldPosition);
				throw;
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000D56C File Offset: 0x0000C56C
		protected internal virtual void BackupFileRest(int itemNo, ref string tempFileName, ref Stream backupFileStream)
		{
			tempFileName = this.GetTempFileName();
			backupFileStream = new FileStream(tempFileName, FileMode.Create);
			long relativeLocalHeaderOffset = this._itemsHandler.ItemsArrayBackup[itemNo].RelativeLocalHeaderOffset;
			this._compressedStream.Seek(relativeLocalHeaderOffset, SeekOrigin.Begin);
			CompressionUtils.CopyStream(this._compressedStream, backupFileStream, this._compressedStream.Length - relativeLocalHeaderOffset);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000D5CC File Offset: 0x0000C5CC
		protected internal void CloseStream(int itemNo, ref Stream currentItemStream, long oldPosition)
		{
			if (this._itemsHandler.ItemsArray[itemNo].Stream == null)
			{
				if (currentItemStream != null)
				{
					currentItemStream.Close();
					currentItemStream = null;
					return;
				}
			}
			else
			{
				if (this._itemsHandler.ItemsArray[itemNo].NeedDestroyStream)
				{
					this._itemsHandler.ItemsArray[itemNo].Stream.Close();
					this._itemsHandler.ItemsArray[itemNo].Stream = null;
					return;
				}
				currentItemStream.Position = oldPosition;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000D653 File Offset: 0x0000C653
		protected internal void DeleteFileRest(ref Stream backupFileStream, string tempFileName)
		{
			if (backupFileStream != null)
			{
				backupFileStream.Close();
				backupFileStream = null;
			}
			if (File.Exists(tempFileName))
			{
				File.Delete(tempFileName);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000D674 File Offset: 0x0000C674
		protected internal bool DeleteFilesAfterMove()
		{
			for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
			{
				if (this._itemsHandler.ItemsArray[i].IsModified && this._itemsHandler.ItemsArray[i].Operation == ProcessOperation.Move)
				{
					FailureAction failureAction;
					do
					{
						failureAction = FailureAction.Ignore;
						try
						{
							if ((this._itemsHandler.ItemsArray[i].ExternalAttributes & FileAttributes.Directory) == (FileAttributes)0)
							{
								try
								{
									File.Delete(this._itemsHandler.ItemsArray[i].SrcFileName);
								}
								catch (Exception innerException)
								{
									throw ExceptionBuilder.Exception(ErrorCode.CannotDeleteFile, new object[]
									{
										this._itemsHandler.ItemsArray[i].SrcFileName
									}, innerException);
								}
							}
						}
						catch (Exception ex)
						{
							if (ex is ArchiverException)
							{
								this.DoOnProcessFileFailure(this._itemsHandler.ItemsArray[i].SrcFileName, this._itemsHandler.ItemsArray[i].Operation, ((ArchiverException)ex).ErrorCode, ((ArchiverException)ex).Args, ex.Message, ex.InnerException, ref failureAction);
							}
							else
							{
								this.DoOnProcessFileFailure(this._itemsHandler.ItemsArray[i].SrcFileName, this._itemsHandler.ItemsArray[i].Operation, ErrorCode.UnknownError, null, ex.Message, ex, ref failureAction);
							}
						}
					}
					while (failureAction == FailureAction.Retry);
					if (failureAction == FailureAction.Abort)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060000C6 RID: 198
		internal abstract void DoCompress(bool encrypted, IItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long count, ref long compSize, ref uint fcrc32);

		// Token: 0x060000C7 RID: 199 RVA: 0x0000D810 File Offset: 0x0000C810
		protected internal int GetIndexOfFirstChange()
		{
			int num = (this._itemsHandler.ItemsArrayBackup.Count < this._itemsHandler.ItemsArray.Count) ? this._itemsHandler.ItemsArrayBackup.Count : this._itemsHandler.ItemsArray.Count;
			int result = num;
			for (int i = 0; i < num; i++)
			{
				if (this._itemsHandler.ItemsArrayBackup[i].Name != this._itemsHandler.ItemsArray[i].Name || this._itemsHandler.ItemsArray[i].IsModified)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000D8C0 File Offset: 0x0000C8C0
		protected internal virtual bool InternalCompressFile(Stream currentItemStream, Stream compressedStream, IItem item)
		{
			long length = currentItemStream.Length;
			currentItemStream.Position = 0L;
			long num = 0L;
			long num2 = 0L;
			uint maxValue = uint.MaxValue;
			int blockSize = (int)this.GetBlockSize(item);
			this.DoCompress(false, item, blockSize, currentItemStream, compressedStream, ref num, ref num2, ref maxValue);
			bool result;
			if (num == length)
			{
				result = true;
			}
			else
			{
				if (!this._progressCancel)
				{
					throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat, new object[]
					{
						currentItemStream.Length,
						num
					});
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000D940 File Offset: 0x0000C940
		protected internal void RestoreFileRest(int itemNo, ref Stream backupFileStream, long backupOffset, string tempFileName)
		{
			if (backupFileStream == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNull, new ArgumentNullException("backupFileStream"));
			}
			this._compressedStream.Position = backupOffset;
			this._compressedStream.SetLength(this._compressedStream.Position);
			backupFileStream.Position = 0L;
			CompressionUtils.CopyStream(backupFileStream, this._compressedStream);
			this.DeleteFileRest(ref backupFileStream, tempFileName);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000D9A4 File Offset: 0x0000C9A4
		protected internal void Rollback(ref Stream backupFileStream, int chgItemNo, long backupOffset, string tempFileName)
		{
			this._itemsHandler.ItemsArray = (IItemsArray)this._itemsHandler.ItemsArrayBackup.Clone();
			if (this._compressedStream != null)
			{
				if (backupFileStream != null)
				{
					this.RestoreFileRest(chgItemNo, ref backupFileStream, backupOffset, tempFileName);
					return;
				}
				this._compressedStream.SetLength(backupOffset);
				this._itemsHandler.SaveItemsArray();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000DA00 File Offset: 0x0000CA00
		protected internal virtual int AddNewItemToArchive(BaseArchiveItem item, bool move)
		{
			string fullName = item.FullName;
			ProcessOperation operation = move ? ProcessOperation.Move : ProcessOperation.Add;
			bool flag = true;
			this.DoOnConfirmProcessFile(fullName, operation, ref flag);
			if (!flag)
			{
				return -1;
			}
			int num = this.InitializeNewItem(item);
			if (num < 0)
			{
				return num;
			}
			if (move)
			{
				this._itemsHandler.ItemsArray[num].Operation = ProcessOperation.Move;
			}
			return num;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000DA58 File Offset: 0x0000CA58
		protected internal virtual int InitializeNewItem(BaseArchiveItem item)
		{
			int num = 0;
			bool flag = false;
			string fullName = item.FullName;
			item.SrcFileName = CompressionUtils.GetFullFileName(this._baseDir, item.SrcFileName);
			this.DoOnStoreFile(ref item);
			int num2;
			if (this._itemsHandler.ItemsArray.FileExists(item.FullName, ref num))
			{
				IItem item2 = this._itemsHandler.ItemsArray[num];
				bool flag2;
				switch (this._archiverOptions.Overwrite)
				{
				case OverwriteMode.Prompt:
					flag2 = false;
					this.DoOnConfirmOverwrite(item.FullName, ref fullName, ref flag2, ref flag);
					if (fullName != item.FullName)
					{
						item.FileName = Path.GetFileName(fullName);
						item.StoredPath = Path.GetDirectoryName(fullName);
					}
					break;
				case OverwriteMode.Always:
					flag2 = true;
					break;
				case OverwriteMode.Never:
					flag2 = false;
					break;
				case OverwriteMode.IfNewer:
				case OverwriteMode.IfOlder:
				{
					DateTime lastModificationDateTime = this.GetLastModificationDateTime(item2);
					DateTime fileModificationDateTime = item.FileModificationDateTime;
					flag2 = ((!(lastModificationDateTime >= fileModificationDateTime) || this._archiverOptions.Overwrite != OverwriteMode.IfNewer) && (!(lastModificationDateTime <= fileModificationDateTime) || this._archiverOptions.Overwrite != OverwriteMode.IfOlder));
					break;
				}
				default:
					flag2 = false;
					break;
				}
				if (flag2 && !flag)
				{
					num2 = num;
					this.InitDirItem(num2, item.FileName == string.Empty);
					this.CopyItem(item, this._itemsHandler.ItemsArray[num2]);
					item2.IsModified = true;
					item2.Operation = ProcessOperation.Update;
				}
				else
				{
					num2 = -1;
				}
			}
			else
			{
				this.AddNewItemToItemsHandler();
				num2 = this._itemsHandler.ItemsArray.Count - 1;
				this.InitDirItem(num2, item.FileName == string.Empty);
				this.CopyItem(item, this._itemsHandler.ItemsArray[num2]);
				this._itemsHandler.ItemsArray[num2].Operation = ProcessOperation.Add;
			}
			return num2;
		}

		// Token: 0x060000CD RID: 205
		protected abstract void AddNewItemToItemsHandler();

		// Token: 0x060000CE RID: 206 RVA: 0x0000DC38 File Offset: 0x0000CC38
		protected virtual void CopyItem(BaseArchiveItem source, IItem destination)
		{
			destination.CopyFrom(source);
		}

		// Token: 0x060000CF RID: 207
		protected abstract DateTime GetLastModificationDateTime(IItem item);

		// Token: 0x060000D0 RID: 208 RVA: 0x0000DC41 File Offset: 0x0000CC41
		protected internal void CheckInactive()
		{
			if (!this._isOpened)
			{
				throw ExceptionBuilder.Exception(ErrorCode.ArchiveIsNotOpen);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000DC52 File Offset: 0x0000CC52
		protected internal void CheckInUpdate()
		{
			if (this.InUpdate)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InUpdate);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000DC64 File Offset: 0x0000CC64
		protected internal bool CheckNameMatchInMaskList(string name, StringCollection maskList, bool isDir)
		{
			foreach (string fileMask in maskList)
			{
				if (this.IsExternalFileMatchMask(name, fileMask, isDir))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000D3 RID: 211
		protected internal abstract BaseArchiveItem CreateNewArchiveItem(string fileName, string baseDir, StorePathMode storePathMode);

		// Token: 0x060000D4 RID: 212
		protected internal abstract IItemsHandler CreateNewItemsHandler(Stream stream, bool create);

		// Token: 0x060000D5 RID: 213 RVA: 0x0000DCC0 File Offset: 0x0000CCC0
		protected internal virtual void DeleteItem(int itemNo)
		{
			if (itemNo < 0 || itemNo >= this._itemsHandler.ItemsArray.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds, new ArgumentOutOfRangeException("ItemNo"));
			}
			if (!this.InUpdate)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NotInUpdate);
			}
			this._itemsHandler.ItemsArray.DeleteItem(itemNo);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000DD16 File Offset: 0x0000CD16
		protected internal void DeleteTaggedFile(int itemNo)
		{
			this.DeleteItem(itemNo);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000DD1F File Offset: 0x0000CD1F
		protected internal void DoAfterOpen()
		{
			if (this.OnAfterOpen != null)
			{
				this.OnAfterOpen(this, this._fileName);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000DD3C File Offset: 0x0000CD3C
		protected internal virtual void DoOnConfirmOverwrite(string sourceFileName, ref string destFileName, ref bool confirm, ref bool cancel)
		{
			if (this.OnConfirmOverwrite != null)
			{
				this.OnConfirmOverwrite(this, sourceFileName, ref destFileName, ref confirm, ref cancel);
				return;
			}
			cancel = false;
			switch (new ConfirmOverwriteDialog
			{
				DialogText = "The folder already contains this file: " + sourceFileName + "\n Would you like to replace the existing file?"
			}.ShowDialog())
			{
			case DialogResult.OK:
				this._archiverOptions.Overwrite = OverwriteMode.Always;
				confirm = true;
				return;
			case DialogResult.Cancel:
				confirm = false;
				cancel = true;
				break;
			case DialogResult.Abort:
			case DialogResult.Retry:
			case DialogResult.Ignore:
				break;
			case DialogResult.Yes:
				confirm = true;
				return;
			case DialogResult.No:
				confirm = false;
				return;
			default:
				return;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000DDD0 File Offset: 0x0000CDD0
		protected internal virtual void DoOnConfirmProcessFile(string fileName, ProcessOperation operation, ref bool confirm)
		{
			if (this.OnConfirmProcessFile != null)
			{
				this.OnConfirmProcessFile(this, fileName, operation, ref confirm);
				return;
			}
			confirm = true;
		}

		// Token: 0x060000DA RID: 218
		protected internal abstract void DoOnExtractFile(ref BaseArchiveItem item);

		// Token: 0x060000DB RID: 219 RVA: 0x0000DDF0 File Offset: 0x0000CDF0
		protected internal virtual void DoOnFileProgress(string fileName, double progress, TimeSpan timeElapsed, TimeSpan timeLeft, ProcessOperation operation, ProgressPhase progressPhase, ref bool cancel)
		{
			if (this.OnFileProgress != null)
			{
				string fileName2 = fileName.Replace('/', '\\');
				this.OnFileProgress(this, fileName2, progress, timeElapsed, timeLeft, operation, progressPhase, ref cancel);
				return;
			}
			cancel = false;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000DE2C File Offset: 0x0000CE2C
		protected internal virtual void DoOnOverallProgress(double progress, TimeSpan timeElapsed, TimeSpan timeLeft, ProcessOperation operation, ProgressPhase progressPhase, ref bool cancel)
		{
			if (this.OnOverallProgress != null)
			{
				this.OnOverallProgress(this, progress, timeElapsed, timeLeft, operation, progressPhase, ref cancel);
				return;
			}
			cancel = false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000DE50 File Offset: 0x0000CE50
		protected internal virtual void DoOnProcessFileFailure(string fileName, ProcessOperation operation, ErrorCode errorCode, object[] args, string errorMessage, Exception innerException, ref FailureAction action)
		{
			action = FailureAction.Ignore;
			if (this.OnProcessFileFailure != null)
			{
				this.OnProcessFileFailure(this, fileName, operation, errorCode, errorMessage, innerException, ref action);
				return;
			}
			if (args != null)
			{
				throw ExceptionBuilder.Exception(errorCode, args, innerException);
			}
			throw ExceptionBuilder.Exception(errorCode, innerException);
		}

		// Token: 0x060000DE RID: 222
		protected internal abstract void DoOnStoreFile(ref BaseArchiveItem item);

		// Token: 0x060000DF RID: 223
		protected internal abstract void ExtractItem(int itemNo, Stream destStream);

		// Token: 0x060000E0 RID: 224 RVA: 0x0000DE8C File Offset: 0x0000CE8C
		protected internal void ExtractTaggedFile(int itemNo, string fileName, string path, ref bool cancel)
		{
			bool flag = true;
			cancel = false;
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this._itemsHandler.ItemsArray[itemNo].GetArchiveItem(ref baseArchiveItem);
			this.DoOnExtractFile(ref baseArchiveItem);
			string text = baseArchiveItem.FullName;
			if (text.IndexOf(":") > 0 || text.IndexOf("\\\\") > 0)
			{
				path = string.Empty;
			}
			if (!this._archiverOptions.CreateDirs)
			{
				text = Path.GetFileName(text);
			}
			else if (this._archiverOptions.CreateDirs)
			{
				try
				{
					string path2 = path + Path.GetDirectoryName(text);
					if (!FileUtils.DirectotyExists(path2))
					{
						Directory.CreateDirectory(path2);
					}
				}
				catch (Exception innerException)
				{
					throw ExceptionBuilder.Exception(ErrorCode.CannotCreateDir, new object[]
					{
						path + Path.GetDirectoryName(text)
					}, innerException);
				}
			}
			string text2 = (path + text).Replace("/", "\\");
			if (this._itemsHandler.ItemsArray[itemNo].IsDirectory())
			{
				if (this._archiverOptions.SetAttributes)
				{
					CompressionUtils.FileSetAttr(text2, baseArchiveItem.ExternalFileAttributes);
				}
				return;
			}
			if (File.Exists(text2))
			{
				if (!this._archiverOptions.ReplaceReadOnly)
				{
					FileAttributes attributes = FileUtils.GetAttributes(text2);
					if ((attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
					{
						return;
					}
				}
				switch (this._archiverOptions.Overwrite)
				{
				case OverwriteMode.Prompt:
					this.DoOnConfirmOverwrite(text, ref text2, ref flag, ref cancel);
					if (flag && !cancel)
					{
						goto IL_1CA;
					}
					break;
				case OverwriteMode.Always:
					goto IL_1CA;
				case OverwriteMode.Never:
					break;
				case OverwriteMode.IfNewer:
				case OverwriteMode.IfOlder:
				{
					DateTime lastWriteTime = File.GetLastWriteTime(text2);
					DateTime lastModificationDateTime = this.GetLastModificationDateTime(this._itemsHandler.ItemsArray[itemNo]);
					if ((lastWriteTime >= lastModificationDateTime && this._archiverOptions.Overwrite == OverwriteMode.IfNewer) || (lastWriteTime <= lastModificationDateTime && this._archiverOptions.Overwrite == OverwriteMode.IfOlder))
					{
						return;
					}
					goto IL_1CA;
				}
				default:
					goto IL_1CA;
				}
				return;
			}
			FileStream fileStream;
			try
			{
				IL_1CA:
				if (File.Exists(text2))
				{
					CompressionUtils.FileSetAttr(text2, (FileAttributes)0);
					File.Delete(text2);
				}
				fileStream = new FileStream(text2, FileMode.Create);
			}
			catch (Exception innerException2)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotCreateFile, new object[]
				{
					text2
				}, innerException2);
			}
			try
			{
				this.ExtractItem(itemNo, fileStream);
				if (this._archiverOptions.FlushBuffers)
				{
					fileStream.Flush();
				}
				fileStream.Close();
				this.SetFileLastWriteTime(itemNo, fileStream);
				if (this._skipFile || this._progressCancel)
				{
					File.Delete(text2);
				}
				else if (this._archiverOptions.SetAttributes)
				{
					CompressionUtils.FileSetAttr(text2, baseArchiveItem.ExternalFileAttributes);
				}
			}
			catch (Exception)
			{
				fileStream.Close();
				File.Delete(text2);
				throw;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000E138 File Offset: 0x0000D138
		protected virtual void SetFileLastWriteTime(int itemNo, FileStream fileStream)
		{
			DateTime lastModificationDateTime = this.GetLastModificationDateTime(this._itemsHandler.ItemsArray[itemNo]);
			FileUtils.SetFileLastWriteTime(fileStream.Name, lastModificationDateTime);
		}

		// Token: 0x060000E2 RID: 226
		protected internal abstract void FillDirItem(int itemNo, string fileName);

		// Token: 0x060000E3 RID: 227 RVA: 0x0000E16C File Offset: 0x0000D16C
		protected internal void ForceUpdate()
		{
			if (this._updateCount > 0)
			{
				Stream backupFileStream = null;
				this._processedFileNo = 0;
				this._processedFileCount = 0;
				this._progressCancel = false;
				this._progressEnabled = true;
				int num = -1;
				this.CalculateTotalProcessFilesSize(ref num);
				int indexOfFirstChange = this.GetIndexOfFirstChange();
				ProcessOperation operation = (num >= 0) ? this._itemsHandler.ItemsArray[num].Operation : ProcessOperation.Delete;
				this._operationStartTime = DateTime.Now;
				if (num >= 0 || indexOfFirstChange < this._itemsHandler.ItemsArrayBackup.Count)
				{
					this.DoOnOverallProgress(0.0, new TimeSpan(0L), new TimeSpan(0L), operation, ProgressPhase.Start, ref this._progressCancel);
				}
				if (this._progressCancel)
				{
					return;
				}
				string empty = string.Empty;
				if (indexOfFirstChange < this._itemsHandler.ItemsArrayBackup.Count)
				{
					this.BackupFileRest(indexOfFirstChange, ref empty, ref backupFileStream);
				}
				long num2;
				this.CalculateBackupOffset(indexOfFirstChange, out num2);
				this._compressedStream.Seek(num2, SeekOrigin.Begin);
				this._compressedStream.SetLength(num2);
				FailureAction failureAction = FailureAction.Ignore;
				try
				{
					int num3 = indexOfFirstChange;
					while (num3 < this._itemsHandler.ItemsArray.Count && !this._progressCancel)
					{
						bool flag;
						do
						{
							failureAction = FailureAction.Ignore;
							flag = true;
							IItem item = this._itemsHandler.ItemsArray[num3];
							if (!item.IsModified || item.Operation == ProcessOperation.Rename || item.Operation == ProcessOperation.ChangeComment || item.Operation == ProcessOperation.ChangeAttr)
							{
								this.AddFromFileRest(num3, backupFileStream, num2, this._compressedStream);
							}
							else
							{
								flag = this.AddFromNewSource(num3, this._compressedStream, ref failureAction);
							}
						}
						while (failureAction == FailureAction.Retry);
						if (failureAction == FailureAction.Abort || this._progressCancel)
						{
							break;
						}
						if (flag)
						{
							this._processedFileNo++;
							num3++;
						}
						else if (this._itemsHandler.ItemsArray[num3].Operation == ProcessOperation.Add)
						{
							this._itemsHandler.ItemsArray.DeleteItem(num3);
						}
						else
						{
							this.AddFromFileRest(num3, backupFileStream, num2, this._compressedStream);
							num3++;
						}
					}
					if (!this._progressCancel && failureAction != FailureAction.Abort)
					{
						this._itemsHandler.SaveItemsArray(this._compressedStream);
						this.DeleteFileRest(ref backupFileStream, empty);
					}
				}
				catch (ArchiverException ex)
				{
					if (ex.ErrorCode == ErrorCode.ShouldCreateSeparateArchivers)
					{
						throw;
					}
					this.Rollback(ref backupFileStream, indexOfFirstChange, num2, empty);
					throw;
				}
				catch
				{
					this.Rollback(ref backupFileStream, indexOfFirstChange, num2, empty);
					throw;
				}
				if (!this._progressCancel && failureAction != FailureAction.Abort && !this.DeleteFilesAfterMove())
				{
					failureAction = FailureAction.Abort;
				}
				if ((num >= 0 || indexOfFirstChange < this._itemsHandler.ItemsArrayBackup.Count) && !this._progressCancel && failureAction != FailureAction.Abort)
				{
					this.DoOnOverallProgress(100.0, DateTime.Now - this._operationStartTime, new TimeSpan(0L), operation, ProgressPhase.End, ref this._progressCancel);
				}
				if (this._progressCancel || failureAction == FailureAction.Abort)
				{
					this.Rollback(ref backupFileStream, indexOfFirstChange, num2, empty);
				}
				else if (this._archiverOptions.FlushBuffers && this._compressedStream != null && this._compressedStream is FileStream)
				{
					this._compressedStream.Flush();
				}
				this._updateCount = 0;
				this._toProcessFilesTotalSize = 0L;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000E4BC File Offset: 0x0000D4BC
		protected internal virtual void CalculateBackupOffset(int chgItemNo, out long backupOffset)
		{
			if (chgItemNo < this._itemsHandler.ItemsArrayBackup.Count)
			{
				backupOffset = this._itemsHandler.ItemsArrayBackup[chgItemNo].RelativeLocalHeaderOffset;
				return;
			}
			if (this._itemsHandler.ItemsArrayBackup.Count > 0)
			{
				backupOffset = this.GetEndOfTheDataStreamPosition(chgItemNo);
				return;
			}
			backupOffset = 0L;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000E518 File Offset: 0x0000D518
		protected internal virtual void CalculateTotalProcessFilesSize(ref int firstItemNo)
		{
			for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
			{
				IItem item = this._itemsHandler.ItemsArray[i];
				if (item.IsModified)
				{
					this._processedFileCount++;
					if (item.Operation != ProcessOperation.Rename && item.Operation != ProcessOperation.ChangeComment && item.Operation != ProcessOperation.ChangeAttr)
					{
						this._toProcessFilesTotalSize += item.UncompressedSize;
					}
					if (firstItemNo == -1)
					{
						firstItemNo = i;
					}
				}
			}
		}

		// Token: 0x060000E6 RID: 230
		protected internal abstract long GetBlockSize(IItem item);

		// Token: 0x060000E7 RID: 231 RVA: 0x0000E5A0 File Offset: 0x0000D5A0
		protected internal string GetFullMask(string mask, string baseDir, ref bool recurse)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (mask.Length > 0)
			{
				if (baseDir == "\\" && mask[0] == '\\')
				{
					return mask;
				}
				if (mask[0] == '\\' || mask[0] == '/')
				{
					recurse = false;
					if (mask.Length > 1)
					{
						if (mask[1] != '\\' && mask[1] != '/')
						{
							stringBuilder.Append(CompressionUtils.GetFullFileName(baseDir, string.Empty) + mask);
						}
						else
						{
							stringBuilder.Append(mask);
						}
					}
					else
					{
						stringBuilder.Append(CompressionUtils.GetFullFileName(baseDir, string.Empty));
					}
				}
				else if (mask.IndexOf(':') >= 0 || mask.IndexOf("..\\") >= 0 || mask.IndexOf("../") >= 0)
				{
					recurse = false;
					stringBuilder.Append(mask);
				}
				else
				{
					recurse = true;
					stringBuilder.Append(CompressionUtils.GetFullFileName(baseDir, string.Empty));
					if (stringBuilder[stringBuilder.Length - 1] != '\\')
					{
						stringBuilder.Append('\\');
					}
					stringBuilder.Append(mask);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000E6C1 File Offset: 0x0000D6C1
		protected internal bool GetInUpdate()
		{
			return this._updateCount > 0;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000E6CC File Offset: 0x0000D6CC
		protected internal string GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000E6D4 File Offset: 0x0000D6D4
		protected internal virtual void InitDirItem(int itemNo, bool isDirectory)
		{
			IItem item = this._itemsHandler.ItemsArray[itemNo];
			item.Reset();
			item.IsModified = true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000E700 File Offset: 0x0000D700
		protected internal void InternalAddFiles(StringCollection fileMasks, uint searchAttr, StringCollection exclusionMasks, bool move, bool recurse)
		{
			this._totalProcessedFilesSize = 0L;
			if (fileMasks.Count == 0)
			{
				fileMasks.Add("*.*");
			}
			lock (typeof(FileUtils))
			{
				string currentDirectory = FileUtils.GetCurrentDirectory();
				try
				{
					string fullFileName = CompressionUtils.GetFullFileName(this._baseDir, string.Empty);
					FileUtils.SetCurrentDirectory(fullFileName);
					for (int i = 0; i < fileMasks.Count; i++)
					{
						this.RecursiveProcess(fullFileName, fullFileName, fileMasks[i], exclusionMasks, searchAttr, recurse, move, true, true);
					}
				}
				finally
				{
					FileUtils.SetCurrentDirectory(currentDirectory);
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000E7B0 File Offset: 0x0000D7B0
		public virtual void RenameFile(string oldName, string newName)
		{
			this.CheckInactive();
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			if (this.FindFirst(oldName, ref baseArchiveItem))
			{
				this.BeginUpdate();
				try
				{
					string text = this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Name;
					string oldValue = oldName.Replace('\\', '/');
					string newValue = newName.Replace('\\', '/');
					text = text.Replace(oldValue, newValue);
					this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Name = text;
					if (!this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified || this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation == ProcessOperation.ChangeAttr || this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation == ProcessOperation.ChangeComment)
					{
						this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified = true;
						this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation = ProcessOperation.Rename;
					}
					this.EndUpdate();
					return;
				}
				catch
				{
					this.CancelUpdate();
					throw;
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				oldName
			});
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000E92C File Offset: 0x0000D92C
		protected internal virtual void InternalCreateArchive()
		{
			if (this._isInMemory)
			{
				this._compressedStream = new MemoryStream();
				this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, true);
				return;
			}
			if (this._compressedStream == null)
			{
				this._compressedStream = new FileStream(this._fileName, this._fileOpenMode, this._fileOpenAccess, this._fileOpenShare);
			}
			this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, true);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000E9A0 File Offset: 0x0000D9A0
		protected internal virtual void InternalOpenArchive()
		{
			if (this._fileOpenMode == FileMode.Create || (this._fileOpenMode == FileMode.OpenOrCreate && !File.Exists(this._fileName)) || this._isInMemory)
			{
				this.InternalCreateArchive();
			}
			else
			{
				this.OpenFileStream();
				this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, false);
				this._itemsHandler.LoadItemsArray();
			}
			this._isOpened = true;
			this.DoAfterOpen();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000EA10 File Offset: 0x0000DA10
		protected internal bool IsExternalFileMatchMask(string fileName, string fileMask, bool isDir)
		{
			bool flag = false;
			string text = fileName;
			string text2 = fileMask;
			string text3 = fileMask;
			if (text3 == "*.*")
			{
				return true;
			}
			if (isDir)
			{
				text = CompressionUtils.GetSlashedDir(fileName);
				text2 = CompressionUtils.GetSlashedDir(text2);
				if (text3 != string.Empty && !text3.EndsWith("*"))
				{
					text3 = CompressionUtils.GetSlashedDir(text3);
				}
			}
			string fullMask = this.GetFullMask(text3, FileUtils.GetCurrentDirectory(), ref flag);
			if (flag)
			{
				text = CompressionUtils.ExtractRelativePath(CompressionUtils.GetSlashedDir(FileUtils.GetCurrentDirectory()), text);
				if (text2.IndexOf('/') == -1 && text2.IndexOf('\\') == -1)
				{
					text = Path.GetFileName(text);
				}
				text3 = ((!isDir) ? Path.GetFileName(fullMask) : CompressionUtils.ExtractRelativePath(CompressionUtils.GetSlashedDir(FileUtils.GetCurrentDirectory()), fullMask));
				return CompressionUtils.MatchesMask(text, text3) || CompressionUtils.MatchesMask(text, CompressionUtils.ExtractRelativePath(CompressionUtils.GetSlashedDir(this._baseDir), text3));
			}
			text3 = fullMask;
			if (Path.GetDirectoryName(text).ToLower() == Path.GetDirectoryName(text3).ToLower())
			{
				return CompressionUtils.MatchesMask(text, text3);
			}
			return Path.GetFileName(text3) == "*.*" && Path.GetDirectoryName(text).ToLower().Substring(0, text3.Length - 3) == Path.GetDirectoryName(text3).ToLower();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000EB50 File Offset: 0x0000DB50
		protected internal void OpenFileStream()
		{
			if (this._compressedStream == null)
			{
				this._compressedStream = new FileStream(this._fileName, this._fileOpenMode, this._fileOpenAccess, this._fileOpenShare);
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000EB80 File Offset: 0x0000DB80
		protected internal bool ProcessFile(string paramFileName, StringCollection exclusionMasks, string startDir, string baseDir, bool move, bool retrieveFileDate, bool retrieveAttributes)
		{
			string fullFileName = CompressionUtils.GetFullFileName(startDir, string.Empty);
			string text;
			if (FileUtils.DirectotyExists(paramFileName))
			{
				text = CompressionUtils.GetSlashedDir(fullFileName) + CompressionUtils.GetSlashedDir(Path.GetFileName(paramFileName));
			}
			else
			{
				text = CompressionUtils.GetSlashedDir(fullFileName) + Path.GetFileName(paramFileName);
			}
			FileAttributes attributes = FileUtils.GetAttributes(text);
			bool flag;
			if (exclusionMasks.Count > 1)
			{
				flag = !this.CheckNameMatchInMaskList(text, exclusionMasks, (attributes & FileAttributes.Directory) != (FileAttributes)0);
			}
			else
			{
				flag = (text != exclusionMasks[0]);
			}
			if (flag)
			{
				BaseArchiveItem item = this.CreateNewArchiveItem(text, baseDir, this.Options.StorePath);
				int num = this.AddNewItemToArchive(item, move);
				if (num >= 0 && File.Exists(this._itemsHandler.ItemsArray[num].SrcFileName))
				{
					this._itemsHandler.ItemsArray[num].UncompressedSize = new FileInfo(this._itemsHandler.ItemsArray[num].SrcFileName).Length;
				}
			}
			return flag;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000EC84 File Offset: 0x0000DC84
		protected internal void ProcessTaggedFile(ProcessOperation operation, int itemNo)
		{
			bool flag = true;
			lock (this)
			{
				this._processedFileNo = 0;
				this._processedFileCount = 0;
				this._progressCancel = false;
				this._progressEnabled = true;
				this._operationStartTime = DateTime.Now;
				this._toProcessFilesTotalSize = 0L;
				this._totalProcessedFilesSize = 0L;
				if (this._itemsHandler.ItemsArray[itemNo].IsTagged)
				{
					this._processedFileCount++;
					this._toProcessFilesTotalSize += this._itemsHandler.ItemsArray[itemNo].UncompressedSize;
				}
				if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
				{
					this.DoOnOverallProgress(0.0, new TimeSpan(0L), new TimeSpan(0L), operation, ProgressPhase.Start, ref this._progressCancel);
				}
				FailureAction failureAction = FailureAction.Ignore;
				bool progressCancel;
				if (this._itemsHandler.ItemsArray[itemNo].IsTagged)
				{
					failureAction = FailureAction.Ignore;
					do
					{
						string text = string.Empty;
						try
						{
							text = this._itemsHandler.ItemsArray[itemNo].Name;
							text = text.Replace('/', '\\');
							this.DoOnConfirmProcessFile(text, operation, ref flag);
							if (flag)
							{
								string slashedDir = CompressionUtils.GetSlashedDir(this.BaseDir);
								if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
								{
									this.DoOnFileProgress(this._itemsHandler.ItemsArray[itemNo].Name, 0.0, DateTime.Now - this._operationStartTime, new TimeSpan(0L), operation, ProgressPhase.Start, ref this._progressCancel);
								}
								if (this._progressCancel)
								{
									break;
								}
								switch (operation)
								{
								case ProcessOperation.Delete:
									this.DeleteTaggedFile(itemNo);
									break;
								case ProcessOperation.Update:
									this.UpdateTaggedFile(itemNo, text, slashedDir);
									break;
								case ProcessOperation.Extract:
									this.ExtractTaggedFile(itemNo, text, slashedDir, ref this._progressCancel);
									break;
								case ProcessOperation.Test:
									this.TestTaggedFile(itemNo, text);
									break;
								}
								progressCancel = this._progressCancel;
								if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
								{
									this.DoOnFileProgress(this._itemsHandler.ItemsArray[itemNo].Name, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), operation, ProgressPhase.End, ref this._progressCancel);
								}
								this._progressCancel = (this._progressCancel || progressCancel);
								if (this._progressCancel)
								{
									break;
								}
								this._processedFileNo++;
							}
						}
						catch (Exception ex)
						{
							if (ex is ArchiverException)
							{
								this.DoOnProcessFileFailure(text, operation, (ex as ArchiverException).ErrorCode, (ex as ArchiverException).Args, ex.Message, ex.InnerException, ref failureAction);
							}
							else
							{
								this.DoOnProcessFileFailure(text, operation, ErrorCode.UnknownError, null, ex.Message, ex, ref failureAction);
							}
						}
					}
					while (failureAction == FailureAction.Retry);
				}
				progressCancel = this._progressCancel;
				if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
				{
					this.DoOnOverallProgress(100.0, DateTime.Now - this._operationStartTime, new TimeSpan(0L), operation, ProgressPhase.End, ref this._progressCancel);
				}
				this._progressCancel = (this._progressCancel || progressCancel);
				if (this._progressCancel || failureAction == FailureAction.Abort)
				{
					this.CancelUpdate();
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000EFD0 File Offset: 0x0000DFD0
		protected internal void ProcessTaggedFiles(ProcessOperation operation)
		{
			bool flag = true;
			lock (this)
			{
				this._processedFileNo = 0;
				this._processedFileCount = 0;
				this._progressCancel = false;
				this._progressEnabled = true;
				this._operationStartTime = DateTime.Now;
				this._toProcessFilesTotalSize = 0L;
				this._totalProcessedFilesSize = 0L;
				int i;
				for (i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
				{
					if (this._itemsHandler.ItemsArray[i].IsTagged)
					{
						this._processedFileCount++;
						this._toProcessFilesTotalSize += this._itemsHandler.ItemsArray[i].UncompressedSize;
					}
				}
				if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
				{
					this.DoOnOverallProgress(0.0, new TimeSpan(0L), new TimeSpan(0L), operation, ProgressPhase.Start, ref this._progressCancel);
				}
				FailureAction failureAction = FailureAction.Ignore;
				i = 0;
				bool progressCancel;
				while (i < this._itemsHandler.ItemsArray.Count && !this._progressCancel)
				{
					if (this._itemsHandler.ItemsArray[i].IsTagged)
					{
						failureAction = FailureAction.Ignore;
						do
						{
							string text = string.Empty;
							try
							{
								text = this._itemsHandler.ItemsArray[i].Name;
								text = text.Replace('/', '\\');
								this.DoOnConfirmProcessFile(text, operation, ref flag);
								if (flag)
								{
									string slashedDir = CompressionUtils.GetSlashedDir(this.BaseDir);
									if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
									{
										this.DoOnFileProgress(this._itemsHandler.ItemsArray[i].Name, 0.0, DateTime.Now - this._operationStartTime, new TimeSpan(0L), operation, ProgressPhase.Start, ref this._progressCancel);
									}
									if (this._progressCancel)
									{
										break;
									}
									switch (operation)
									{
									case ProcessOperation.Delete:
										this.DeleteTaggedFile(i);
										break;
									case ProcessOperation.Update:
										this.UpdateTaggedFile(i, text, slashedDir);
										break;
									case ProcessOperation.Extract:
										this.ExtractTaggedFile(i, text, slashedDir, ref this._progressCancel);
										break;
									case ProcessOperation.Test:
										this.TestTaggedFile(i, text);
										break;
									}
									progressCancel = this._progressCancel;
									if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
									{
										this.DoOnFileProgress(this._itemsHandler.ItemsArray[i].Name, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), operation, ProgressPhase.End, ref this._progressCancel);
									}
									this._progressCancel = (this._progressCancel || progressCancel);
									if (this._progressCancel)
									{
										break;
									}
									this._processedFileNo++;
								}
							}
							catch (Exception ex)
							{
								if (ex is ArchiverException)
								{
									this.DoOnProcessFileFailure(text, operation, (ex as ArchiverException).ErrorCode, (ex as ArchiverException).Args, ex.Message, ex.InnerException, ref failureAction);
								}
								else
								{
									this.DoOnProcessFileFailure(text, operation, ErrorCode.UnknownError, null, ex.Message, ex, ref failureAction);
								}
							}
						}
						while (failureAction == FailureAction.Retry);
						if (failureAction == FailureAction.Abort || this._progressCancel)
						{
							break;
						}
						if (operation != ProcessOperation.Delete)
						{
							i++;
						}
					}
					else
					{
						i++;
					}
				}
				progressCancel = this._progressCancel;
				if (operation == ProcessOperation.Extract || operation == ProcessOperation.Test)
				{
					this.DoOnOverallProgress(100.0, DateTime.Now - this._operationStartTime, new TimeSpan(0L), operation, ProgressPhase.End, ref this._progressCancel);
				}
				this._progressCancel = (this._progressCancel || progressCancel);
				if (this._progressCancel || failureAction == FailureAction.Abort)
				{
					this.CancelUpdate();
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000F378 File Offset: 0x0000E378
		protected virtual void AddItem(BaseArchiveItem item)
		{
			this.CheckInactive();
			this._progressEnabled = false;
			this.BeginUpdate();
			string archiveFileName = CompressionUtils.GetArchiveFileName(item.SrcFileName, this._baseDir, this.Options.StorePath);
			item.FileName = ((Path.GetFileName(archiveFileName) != item.FileName) ? item.FileName : Path.GetFileName(archiveFileName));
			item.storedPath = ((!item.IsCustomPath) ? Path.GetDirectoryName(archiveFileName).Replace('\\', '/') : CompressionUtils.ExcludeTrailingBackslash(item.storedPath));
			int num = this.AddNewItemToArchive(item, false);
			if (num >= 0 && File.Exists(this._itemsHandler.ItemsArray[num].SrcFileName))
			{
				this._itemsHandler.ItemsArray[num].UncompressedSize = new FileInfo(this._itemsHandler.ItemsArray[num].SrcFileName).Length;
			}
			this.EndUpdate();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000F46C File Offset: 0x0000E46C
		protected internal void RecursiveProcess(string startDir, string baseDir, string mask, StringCollection exclusionMasks, uint searchAttr, bool recurse, bool move, bool retrieveFileDate, bool retrieveAttributes)
		{
			bool flag = false;
			string fullMask = this.GetFullMask(mask, startDir, ref flag);
			string fileName = Path.GetFileName(fullMask);
			string text = (fullMask.IndexOf("..\\") < 0) ? Path.GetDirectoryName(fullMask) : (this.BaseDir + "\\" + Path.GetDirectoryName(fullMask) + "\\");
			if (mask == fullMask && mask.IndexOf('*') == -1 && mask.IndexOf('?') == -1 && File.Exists(fullMask))
			{
				this.ProcessFile(fileName, exclusionMasks, text, baseDir, move, retrieveFileDate, retrieveAttributes);
				return;
			}
			bool flag2 = searchAttr == 32759U;
			uint num;
			if ((searchAttr & 16U) != 0U)
			{
				num = searchAttr - 16U;
			}
			else
			{
				num = searchAttr;
			}
			string[] array;
			try
			{
				array = Directory.GetFiles(text, fileName);
			}
			catch
			{
				array = new string[0];
			}
			if (array.Length != 0)
			{
				foreach (string text2 in array)
				{
					if (!flag2)
					{
						FileAttributes attributes = FileUtils.GetAttributes(text2);
						if ((attributes & (FileAttributes)num) != (FileAttributes)0)
						{
							this.ProcessFile(Path.GetFileName(text2), exclusionMasks, text, baseDir, move, retrieveFileDate, retrieveAttributes);
						}
					}
					else
					{
						this.ProcessFile(Path.GetFileName(text2), exclusionMasks, text, baseDir, move, retrieveFileDate, retrieveAttributes);
					}
				}
			}
			if ((searchAttr & 16U) != 0U || recurse)
			{
				string text3 = Path.GetDirectoryName(fullMask);
				if (CompressionUtils.GetFullFileName(text3, string.Empty).IndexOf(startDir) == -1 && flag)
				{
					text3 = CompressionUtils.IncludeTrailingBackslash(startDir);
				}
				string[] array3;
				try
				{
					array3 = Directory.GetDirectories(text3, "*");
				}
				catch
				{
					array3 = new string[0];
				}
				if (array3.Length != 0)
				{
					foreach (string text4 in array3)
					{
						if (!(text4 == ".") && !(text4 == ".."))
						{
							bool flag3 = !this.CheckNameMatchInMaskList(text4, exclusionMasks, true);
							if (flag3)
							{
								if ((searchAttr & 16U) != 0U && (CompressionUtils.MatchesMask(text4, fullMask) || CompressionUtils.MatchesMask(text4 + '.', fullMask)))
								{
									if (this.ProcessFile(text4, exclusionMasks, text3, baseDir, move, retrieveFileDate, retrieveAttributes))
									{
										this.RecursiveProcess(text4, baseDir, "*.*", exclusionMasks, searchAttr, true, move, retrieveFileDate, retrieveAttributes);
									}
								}
								else if (recurse)
								{
									if (mask.IndexOf('/') >= 0 || mask.IndexOf('\\') >= 0)
									{
										if (mask.IndexOf('*') >= 0 || mask.IndexOf('?') >= 0)
										{
											this.RecursiveProcess(text4, baseDir, Path.GetFileName(fullMask), exclusionMasks, searchAttr, true, move, retrieveFileDate, retrieveAttributes);
										}
									}
									else
									{
										this.RecursiveProcess(text4, baseDir, mask, exclusionMasks, searchAttr, true, move, retrieveFileDate, retrieveAttributes);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000F724 File Offset: 0x0000E724
		protected internal void SetActive(bool value)
		{
			if (value != this._isOpened)
			{
				if (value)
				{
					this.OpenArchive();
					return;
				}
				this.CloseArchive();
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000F740 File Offset: 0x0000E740
		protected internal void SetInMemory(bool value)
		{
			if (this._isInMemory != value && this._isOpened)
			{
				if (this._isInMemory)
				{
					if (this._fileName == string.Empty)
					{
						throw ExceptionBuilder.Exception(ErrorCode.BlankFileName);
					}
					FileStream fileStream = new FileStream(this._fileName, FileMode.Create);
					try
					{
						this._compressedStream.Position = 0L;
						CompressionUtils.CopyStream(this._compressedStream, fileStream);
					}
					finally
					{
						fileStream.Close();
					}
					this.CloseArchive();
					this._isInMemory = false;
					this.OpenArchive();
				}
				else
				{
					this.CloseArchive();
					this._isInMemory = true;
					this.OpenArchive();
				}
			}
			this._isInMemory = value;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000F7F0 File Offset: 0x0000E7F0
		protected internal virtual void TagFiles()
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			this._itemsHandler.ItemsArray.ClearTags();
			if (this.FindFirst(ref baseArchiveItem))
			{
				do
				{
					this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsTagged = true;
				}
				while (this.FindNext(ref baseArchiveItem));
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000F84C File Offset: 0x0000E84C
		protected internal virtual void TagFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			BaseArchiveItem baseArchiveItem = this.CreateNewArchiveItem();
			baseArchiveItem.Reset();
			this._itemsHandler.ItemsArray.ClearTags();
			if (this.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask))
			{
				do
				{
					this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsTagged = true;
				}
				while (this.FindNext(ref baseArchiveItem));
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000F8B0 File Offset: 0x0000E8B0
		protected internal void TestTaggedFile(int itemNo, string fileName)
		{
			if (this._itemsHandler.ItemsArray[itemNo].IsDirectory())
			{
				return;
			}
			string tempFileName = this.GetTempFileName();
			FileStream fileStream;
			try
			{
				fileStream = new FileStream(tempFileName, FileMode.Open);
			}
			catch (Exception innerException)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotCreateFile, new object[]
				{
					tempFileName
				}, innerException);
			}
			try
			{
				this.ExtractItem(itemNo, fileStream);
			}
			finally
			{
				fileStream.Close();
				File.Delete(tempFileName);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000F930 File Offset: 0x0000E930
		protected internal void UpdateItem(int itemNo, string srcFileName)
		{
			if (itemNo < 0 || itemNo >= this._itemsHandler.ItemsArray.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds, new ArgumentOutOfRangeException("ItemNo"));
			}
			if (!this.InUpdate)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NotInUpdate);
			}
			this.FillDirItem(itemNo, srcFileName);
			this._itemsHandler.ItemsArray[itemNo].Operation = ProcessOperation.Update;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000F994 File Offset: 0x0000E994
		protected internal void UpdateTaggedFile(int itemNo, string fileName, string path)
		{
			bool flag = false;
			if (this._itemsHandler.ItemsArray[itemNo].IsDirectory())
			{
				return;
			}
			string fullFileName = CompressionUtils.GetFullFileName(path, fileName);
			if (File.Exists(fullFileName))
			{
				if (!this._archiverOptions.ReplaceReadOnly && (this._itemsHandler.ItemsArray[itemNo].ExternalAttributes & FileAttributes.ReadOnly) != (FileAttributes)0)
				{
					return;
				}
				switch (this._archiverOptions.Overwrite)
				{
				case OverwriteMode.Prompt:
				{
					string text = fileName;
					bool flag2 = false;
					this.DoOnConfirmOverwrite(fullFileName, ref text, ref flag2, ref flag);
					if (flag2 && !flag)
					{
						goto IL_EC;
					}
					break;
				}
				case OverwriteMode.Always:
					goto IL_EC;
				case OverwriteMode.Never:
					break;
				case OverwriteMode.IfNewer:
				case OverwriteMode.IfOlder:
				{
					DateTime lastModificationDateTime = this.GetLastModificationDateTime(this._itemsHandler.ItemsArray[itemNo]);
					DateTime lastWriteTime = File.GetLastWriteTime(fullFileName);
					if ((lastModificationDateTime >= lastWriteTime && this._archiverOptions.Overwrite == OverwriteMode.IfNewer) || (lastModificationDateTime <= lastWriteTime && this._archiverOptions.Overwrite == OverwriteMode.IfOlder))
					{
						return;
					}
					goto IL_EC;
				}
				default:
					goto IL_EC;
				}
				return;
				IL_EC:
				lock (typeof(FileUtils))
				{
					string currentDirectory = FileUtils.GetCurrentDirectory();
					try
					{
						FileUtils.SetCurrentDirectory(CompressionUtils.GetFullFileName(this._baseDir, string.Empty));
						this.UpdateItem(itemNo, fullFileName);
					}
					finally
					{
						FileUtils.SetCurrentDirectory(currentDirectory);
					}
				}
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000FD RID: 253 RVA: 0x0000FAF4 File Offset: 0x0000EAF4
		// (remove) Token: 0x060000FE RID: 254 RVA: 0x0000FB0D File Offset: 0x0000EB0D
		[Description("Occurs when an I/O error occurs.")]
		public event ArchiverForgeBase.OnIOErrorDelegate OnIOError;

		// Token: 0x060000FF RID: 255 RVA: 0x0000FB26 File Offset: 0x0000EB26
		protected void DoOnWriteToStreamFailure(Exception innerException, ref bool cancel)
		{
			if (this.OnIOError != null)
			{
				this.OnIOError(this, innerException, ref cancel);
				return;
			}
			throw innerException;
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000100 RID: 256 RVA: 0x0000FB40 File Offset: 0x0000EB40
		// (remove) Token: 0x06000101 RID: 257 RVA: 0x0000FB59 File Offset: 0x0000EB59
		internal event ArchiverForgeBase.OnReadFromStreamFailureDelegate OnReadFromStreamFailure;

		// Token: 0x06000102 RID: 258 RVA: 0x0000FB72 File Offset: 0x0000EB72
		protected internal void DoOnReadFromStreamFailure(Exception innerException, ref bool cancel)
		{
			if (this.OnReadFromStreamFailure != null)
			{
				this.OnReadFromStreamFailure(this, innerException, ref cancel);
				return;
			}
			throw innerException;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000FB8C File Offset: 0x0000EB8C
		protected internal bool GetExists()
		{
			return File.Exists(this.FileName);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000FB99 File Offset: 0x0000EB99
		public IEnumerator GetEnumerator()
		{
			this._currentItemIndex = -1;
			return this;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000FBA4 File Offset: 0x0000EBA4
		[Browsable(false)]
		public object Current
		{
			get
			{
				BaseArchiveItem result = this.CreateNewArchiveItem();
				this._itemsHandler.ItemsArray[this._currentItemIndex].GetArchiveItem(ref result);
				return result;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000FBD6 File Offset: 0x0000EBD6
		public bool MoveNext()
		{
			if (this._currentItemIndex < this._itemsHandler.ItemsArray.Count - 1)
			{
				this._currentItemIndex++;
				return true;
			}
			return false;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000FC03 File Offset: 0x0000EC03
		public void Reset()
		{
			this._currentItemIndex = 0;
		}

		// Token: 0x04000096 RID: 150
		protected readonly int _oemCodePage;

		// Token: 0x04000097 RID: 151
		protected string _baseDir;

		// Token: 0x04000098 RID: 152
		internal Stream _compressedStream;

		// Token: 0x04000099 RID: 153
		protected DateTime _currentItemOperationStartTime;

		// Token: 0x0400009A RID: 154
		protected string _fileName;

		// Token: 0x0400009B RID: 155
		internal FileAccess _fileOpenAccess;

		// Token: 0x0400009C RID: 156
		internal FileMode _fileOpenMode;

		// Token: 0x0400009D RID: 157
		internal FileShare _fileOpenShare;

		// Token: 0x0400009E RID: 158
		protected bool _isCustomStream;

		// Token: 0x0400009F RID: 159
		protected bool _isInMemory;

		// Token: 0x040000A0 RID: 160
		internal bool _isOpenCorruptedArchives;

		// Token: 0x040000A1 RID: 161
		protected bool _isOpened;

		// Token: 0x040000A2 RID: 162
		protected IItemsHandler _itemsHandler;

		// Token: 0x040000A3 RID: 163
		protected DateTime _operationStartTime;

		// Token: 0x040000A4 RID: 164
		protected int _processedFileCount;

		// Token: 0x040000A5 RID: 165
		protected int _processedFileNo;

		// Token: 0x040000A6 RID: 166
		protected bool _progressCancel;

		// Token: 0x040000A7 RID: 167
		protected bool _progressEnabled;

		// Token: 0x040000A8 RID: 168
		protected bool _skipFile;

		// Token: 0x040000A9 RID: 169
		protected ArchiverOptions _archiverOptions;

		// Token: 0x040000AA RID: 170
		protected long _toProcessFilesTotalSize;

		// Token: 0x040000AB RID: 171
		protected long _totalProcessedFilesSize;

		// Token: 0x040000AC RID: 172
		internal int _updateCount;

		// Token: 0x040000B3 RID: 179
		private StringCollection _exclusionMasks;

		// Token: 0x040000B4 RID: 180
		private StringCollection _fileMasks;

		// Token: 0x040000B7 RID: 183
		private int _currentItemIndex;

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x06000109 RID: 265
		public delegate void OnAfterOpenDelegate(object sender, string fileName);

		// Token: 0x02000016 RID: 22
		// (Invoke) Token: 0x0600010D RID: 269
		public delegate void OnConfirmOverwriteDelegate(object sender, string sourceFileName, ref string destFileName, ref bool confirm, ref bool cancel);

		// Token: 0x02000017 RID: 23
		// (Invoke) Token: 0x06000111 RID: 273
		public delegate void OnConfirmProcessFileDelegate(object sender, string fileName, ProcessOperation operation, ref bool confirm);

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x06000115 RID: 277
		public delegate void OnFileProgressDelegate(object sender, string fileName, double progress, TimeSpan timeElapsed, TimeSpan timeLeft, ProcessOperation operation, ProgressPhase progressPhase, ref bool cancel);

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x06000119 RID: 281
		public delegate void OnOverallProgressDelegate(object sender, double progress, TimeSpan timeElapsed, TimeSpan timeLeft, ProcessOperation operation, ProgressPhase progressPhase, ref bool cancel);

		// Token: 0x0200001A RID: 26
		// (Invoke) Token: 0x0600011D RID: 285
		public delegate void OnProcessFileFailureDelegate(object sender, string fileName, ProcessOperation operation, ErrorCode errorCode, string errorMessage, Exception exception, ref FailureAction action);

		// Token: 0x0200001B RID: 27
		// (Invoke) Token: 0x06000121 RID: 289
		public delegate void OnIOErrorDelegate(object sender, Exception innerException, ref bool cancel);

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x06000125 RID: 293
		internal delegate void OnReadFromStreamFailureDelegate(object sender, Exception innerException, ref bool cancel);
	}
}
