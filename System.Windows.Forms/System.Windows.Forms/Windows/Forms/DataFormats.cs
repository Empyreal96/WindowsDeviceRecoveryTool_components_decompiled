using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Provides <see langword="static" />, predefined <see cref="T:System.Windows.Forms.Clipboard" /> format names. Use them to identify the format of data that you store in an <see cref="T:System.Windows.Forms.IDataObject" />.</summary>
	// Token: 0x02000169 RID: 361
	public class DataFormats
	{
		// Token: 0x060010D7 RID: 4311 RVA: 0x000027DB File Offset: 0x000009DB
		private DataFormats()
		{
		}

		/// <summary>Returns a <see cref="T:System.Windows.Forms.DataFormats.Format" /> with the Windows Clipboard numeric ID and name for the specified format.</summary>
		/// <param name="format">The format name. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataFormats.Format" /> that has the Windows Clipboard numeric ID and the name of the format.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">Registering a new <see cref="T:System.Windows.Forms.Clipboard" /> format failed. </exception>
		// Token: 0x060010D8 RID: 4312 RVA: 0x0003B9F0 File Offset: 0x00039BF0
		public static DataFormats.Format GetFormat(string format)
		{
			object obj = DataFormats.internalSyncObject;
			DataFormats.Format result;
			lock (obj)
			{
				DataFormats.EnsurePredefined();
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					if (DataFormats.formatList[i].Name.Equals(format))
					{
						return DataFormats.formatList[i];
					}
				}
				for (int j = 0; j < DataFormats.formatCount; j++)
				{
					if (string.Equals(DataFormats.formatList[j].Name, format, StringComparison.OrdinalIgnoreCase))
					{
						return DataFormats.formatList[j];
					}
				}
				int num = SafeNativeMethods.RegisterClipboardFormat(format);
				if (num == 0)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), SR.GetString("RegisterCFFailed"));
				}
				DataFormats.EnsureFormatSpace(1);
				DataFormats.formatList[DataFormats.formatCount] = new DataFormats.Format(format, num);
				result = DataFormats.formatList[DataFormats.formatCount++];
			}
			return result;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Forms.DataFormats.Format" /> with the Windows Clipboard numeric ID and name for the specified ID.</summary>
		/// <param name="id">The format ID. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataFormats.Format" /> that has the Windows Clipboard numeric ID and the name of the format.</returns>
		// Token: 0x060010D9 RID: 4313 RVA: 0x0003BAE4 File Offset: 0x00039CE4
		public static DataFormats.Format GetFormat(int id)
		{
			return DataFormats.InternalGetFormat(null, (ushort)(id & 65535));
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0003BAF4 File Offset: 0x00039CF4
		private static DataFormats.Format InternalGetFormat(string strName, ushort id)
		{
			object obj = DataFormats.internalSyncObject;
			DataFormats.Format result;
			lock (obj)
			{
				DataFormats.EnsurePredefined();
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					if (DataFormats.formatList[i].Id == (int)id)
					{
						return DataFormats.formatList[i];
					}
				}
				StringBuilder stringBuilder = new StringBuilder(128);
				if (SafeNativeMethods.GetClipboardFormatName((int)id, stringBuilder, stringBuilder.Capacity) == 0)
				{
					stringBuilder.Length = 0;
					if (strName == null)
					{
						stringBuilder.Append("Format").Append(id);
					}
					else
					{
						stringBuilder.Append(strName);
					}
				}
				DataFormats.EnsureFormatSpace(1);
				DataFormats.formatList[DataFormats.formatCount] = new DataFormats.Format(stringBuilder.ToString(), (int)id);
				result = DataFormats.formatList[DataFormats.formatCount++];
			}
			return result;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0003BBD4 File Offset: 0x00039DD4
		private static void EnsureFormatSpace(int size)
		{
			if (DataFormats.formatList == null || DataFormats.formatList.Length <= DataFormats.formatCount + size)
			{
				int num = DataFormats.formatCount + 20;
				DataFormats.Format[] array = new DataFormats.Format[num];
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					array[i] = DataFormats.formatList[i];
				}
				DataFormats.formatList = array;
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0003BC28 File Offset: 0x00039E28
		private static void EnsurePredefined()
		{
			if (DataFormats.formatCount == 0)
			{
				DataFormats.formatList = new DataFormats.Format[]
				{
					new DataFormats.Format(DataFormats.UnicodeText, 13),
					new DataFormats.Format(DataFormats.Text, 1),
					new DataFormats.Format(DataFormats.Bitmap, 2),
					new DataFormats.Format(DataFormats.MetafilePict, 3),
					new DataFormats.Format(DataFormats.EnhancedMetafile, 14),
					new DataFormats.Format(DataFormats.Dif, 5),
					new DataFormats.Format(DataFormats.Tiff, 6),
					new DataFormats.Format(DataFormats.OemText, 7),
					new DataFormats.Format(DataFormats.Dib, 8),
					new DataFormats.Format(DataFormats.Palette, 9),
					new DataFormats.Format(DataFormats.PenData, 10),
					new DataFormats.Format(DataFormats.Riff, 11),
					new DataFormats.Format(DataFormats.WaveAudio, 12),
					new DataFormats.Format(DataFormats.SymbolicLink, 4),
					new DataFormats.Format(DataFormats.FileDrop, 15),
					new DataFormats.Format(DataFormats.Locale, 16)
				};
				DataFormats.formatCount = DataFormats.formatList.Length;
			}
		}

		/// <summary>Specifies the standard ANSI text format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008BC RID: 2236
		public static readonly string Text = "Text";

		/// <summary>Specifies the standard Windows Unicode text format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008BD RID: 2237
		public static readonly string UnicodeText = "UnicodeText";

		/// <summary>Specifies the Windows device-independent bitmap (DIB) format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008BE RID: 2238
		public static readonly string Dib = "DeviceIndependentBitmap";

		/// <summary>Specifies a Windows bitmap format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008BF RID: 2239
		public static readonly string Bitmap = "Bitmap";

		/// <summary>Specifies the Windows enhanced metafile format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C0 RID: 2240
		public static readonly string EnhancedMetafile = "EnhancedMetafile";

		/// <summary>Specifies the Windows metafile format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C1 RID: 2241
		public static readonly string MetafilePict = "MetaFilePict";

		/// <summary>Specifies the Windows symbolic link format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C2 RID: 2242
		public static readonly string SymbolicLink = "SymbolicLink";

		/// <summary>Specifies the Windows Data Interchange Format (DIF), which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C3 RID: 2243
		public static readonly string Dif = "DataInterchangeFormat";

		/// <summary>Specifies the Tagged Image File Format (TIFF), which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C4 RID: 2244
		public static readonly string Tiff = "TaggedImageFileFormat";

		/// <summary>Specifies the standard Windows original equipment manufacturer (OEM) text format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C5 RID: 2245
		public static readonly string OemText = "OEMText";

		/// <summary>Specifies the Windows palette format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C6 RID: 2246
		public static readonly string Palette = "Palette";

		/// <summary>Specifies the Windows pen data format, which consists of pen strokes for handwriting software; Windows Forms does not use this format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C7 RID: 2247
		public static readonly string PenData = "PenData";

		/// <summary>Specifies the Resource Interchange File Format (RIFF) audio format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C8 RID: 2248
		public static readonly string Riff = "RiffAudio";

		/// <summary>Specifies the wave audio format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008C9 RID: 2249
		public static readonly string WaveAudio = "WaveAudio";

		/// <summary>Specifies the Windows file drop format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CA RID: 2250
		public static readonly string FileDrop = "FileDrop";

		/// <summary>Specifies the Windows culture format, which Windows Forms does not directly use. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CB RID: 2251
		public static readonly string Locale = "Locale";

		/// <summary>Specifies text in the HTML Clipboard format. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CC RID: 2252
		public static readonly string Html = "HTML Format";

		/// <summary>Specifies text consisting of Rich Text Format (RTF) data. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CD RID: 2253
		public static readonly string Rtf = "Rich Text Format";

		/// <summary>Specifies a comma-separated value (CSV) format, which is a common interchange format used by spreadsheets. This format is not used directly by Windows Forms. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CE RID: 2254
		public static readonly string CommaSeparatedValue = "Csv";

		/// <summary>Specifies the Windows Forms string class format, which Windows Forms uses to store string objects. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008CF RID: 2255
		public static readonly string StringFormat = typeof(string).FullName;

		/// <summary>Specifies a format that encapsulates any type of Windows Forms object. This <see langword="static" /> field is read-only.</summary>
		// Token: 0x040008D0 RID: 2256
		public static readonly string Serializable = Application.WindowsFormsVersion + "PersistentObject";

		// Token: 0x040008D1 RID: 2257
		private static DataFormats.Format[] formatList;

		// Token: 0x040008D2 RID: 2258
		private static int formatCount = 0;

		// Token: 0x040008D3 RID: 2259
		private static object internalSyncObject = new object();

		/// <summary>Represents a Clipboard format type.</summary>
		// Token: 0x02000589 RID: 1417
		public class Format
		{
			/// <summary>Gets the name of this format.</summary>
			/// <returns>The name of this format.</returns>
			// Token: 0x170014F3 RID: 5363
			// (get) Token: 0x060057D2 RID: 22482 RVA: 0x00171A32 File Offset: 0x0016FC32
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			/// <summary>Gets the ID number for this format.</summary>
			/// <returns>The ID number for this format.</returns>
			// Token: 0x170014F4 RID: 5364
			// (get) Token: 0x060057D3 RID: 22483 RVA: 0x00171A3A File Offset: 0x0016FC3A
			public int Id
			{
				get
				{
					return this.id;
				}
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataFormats.Format" /> class with a Boolean that indicates whether a <see langword="Win32" /> handle is expected.</summary>
			/// <param name="name">The name of this format. </param>
			/// <param name="id">The ID number for this format. </param>
			// Token: 0x060057D4 RID: 22484 RVA: 0x00171A42 File Offset: 0x0016FC42
			public Format(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			// Token: 0x04003877 RID: 14455
			private readonly string name;

			// Token: 0x04003878 RID: 14456
			private readonly int id;
		}
	}
}
