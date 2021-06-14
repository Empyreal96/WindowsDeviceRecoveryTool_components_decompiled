using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Internal;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Implements a basic data transfer mechanism.</summary>
	// Token: 0x02000213 RID: 531
	[ClassInterface(ClassInterfaceType.None)]
	public class DataObject : IDataObject, IDataObject
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x000A13B1 File Offset: 0x0009F5B1
		// (set) Token: 0x0600202B RID: 8235 RVA: 0x000A13B9 File Offset: 0x0009F5B9
		internal bool RestrictedFormats { get; set; }

		// Token: 0x0600202C RID: 8236 RVA: 0x000A13C2 File Offset: 0x0009F5C2
		internal DataObject(IDataObject data)
		{
			this.innerData = data;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000A13D1 File Offset: 0x0009F5D1
		internal DataObject(IDataObject data)
		{
			if (data is DataObject)
			{
				this.innerData = (data as IDataObject);
				return;
			}
			this.innerData = new DataObject.OleConverter(data);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataObject" /> class.</summary>
		// Token: 0x0600202E RID: 8238 RVA: 0x000A13FA File Offset: 0x0009F5FA
		public DataObject()
		{
			this.innerData = new DataObject.DataStore();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataObject" /> class and adds the specified object to it.</summary>
		/// <param name="data">The data to store. </param>
		// Token: 0x0600202F RID: 8239 RVA: 0x000A1410 File Offset: 0x0009F610
		public DataObject(object data)
		{
			if (data is IDataObject && !Marshal.IsComObject(data))
			{
				this.innerData = (IDataObject)data;
				return;
			}
			if (data is IDataObject)
			{
				this.innerData = new DataObject.OleConverter((IDataObject)data);
				return;
			}
			this.innerData = new DataObject.DataStore();
			this.SetData(data);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataObject" /> class and adds the specified object in the specified format.</summary>
		/// <param name="format">The format of the specified data. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats.</param>
		/// <param name="data">The data to store. </param>
		// Token: 0x06002030 RID: 8240 RVA: 0x000A146C File Offset: 0x0009F66C
		public DataObject(string format, object data) : this()
		{
			this.SetData(format, data);
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x000A147C File Offset: 0x0009F67C
		private IntPtr GetCompatibleBitmap(Bitmap bm)
		{
			IntPtr hbitmap = bm.GetHbitmap();
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			IntPtr handle = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			IntPtr handle2 = SafeNativeMethods.SelectObject(new HandleRef(null, handle), new HandleRef(bm, hbitmap));
			IntPtr handle3 = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			IntPtr intPtr = SafeNativeMethods.CreateCompatibleBitmap(new HandleRef(null, dc), bm.Size.Width, bm.Size.Height);
			IntPtr handle4 = SafeNativeMethods.SelectObject(new HandleRef(null, handle3), new HandleRef(null, intPtr));
			SafeNativeMethods.BitBlt(new HandleRef(null, handle3), 0, 0, bm.Size.Width, bm.Size.Height, new HandleRef(null, handle), 0, 0, 13369376);
			SafeNativeMethods.SelectObject(new HandleRef(null, handle), new HandleRef(null, handle2));
			SafeNativeMethods.SelectObject(new HandleRef(null, handle3), new HandleRef(null, handle4));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, handle));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, handle3));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			SafeNativeMethods.DeleteObject(new HandleRef(bm, hbitmap));
			return intPtr;
		}

		/// <summary>Returns the data associated with the specified data format, using an automated conversion parameter to determine whether to convert the data to the format.</summary>
		/// <param name="format">The format of the data to retrieve. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <param name="autoConvert">
		///       <see langword="true" /> to the convert data to the specified format; otherwise, <see langword="false" />. </param>
		/// <returns>The data associated with the specified format, or <see langword="null" />.</returns>
		// Token: 0x06002032 RID: 8242 RVA: 0x000A15AF File Offset: 0x0009F7AF
		public virtual object GetData(string format, bool autoConvert)
		{
			return this.innerData.GetData(format, autoConvert);
		}

		/// <summary>Returns the data associated with the specified data format.</summary>
		/// <param name="format">The format of the data to retrieve. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <returns>The data associated with the specified format, or <see langword="null" />.</returns>
		// Token: 0x06002033 RID: 8243 RVA: 0x000A15BE File Offset: 0x0009F7BE
		public virtual object GetData(string format)
		{
			return this.GetData(format, true);
		}

		/// <summary>Returns the data associated with the specified class type format.</summary>
		/// <param name="format">A <see cref="T:System.Type" /> representing the format of the data to retrieve. </param>
		/// <returns>The data associated with the specified format, or <see langword="null" />.</returns>
		// Token: 0x06002034 RID: 8244 RVA: 0x000A15C8 File Offset: 0x0009F7C8
		public virtual object GetData(Type format)
		{
			if (format == null)
			{
				return null;
			}
			return this.GetData(format.FullName);
		}

		/// <summary>Determines whether data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with, or can be converted to, the specified format.</summary>
		/// <param name="format">A <see cref="T:System.Type" /> representing the format to check for. </param>
		/// <returns>
		///     <see langword="true" /> if data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with, or can be converted to, the specified format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002035 RID: 8245 RVA: 0x000A15E4 File Offset: 0x0009F7E4
		public virtual bool GetDataPresent(Type format)
		{
			return !(format == null) && this.GetDataPresent(format.FullName);
		}

		/// <summary>Determines whether this <see cref="T:System.Windows.Forms.DataObject" /> contains data in the specified format or, optionally, contains data that can be converted to the specified format.</summary>
		/// <param name="format">The format to check for. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <param name="autoConvert">
		///       <see langword="true" /> to determine whether data stored in this <see cref="T:System.Windows.Forms.DataObject" /> can be converted to the specified format; <see langword="false" /> to check whether the data is in the specified format. </param>
		/// <returns>
		///     <see langword="true" /> if the data is in, or can be converted to, the specified format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002036 RID: 8246 RVA: 0x000A160C File Offset: 0x0009F80C
		public virtual bool GetDataPresent(string format, bool autoConvert)
		{
			return this.innerData.GetDataPresent(format, autoConvert);
		}

		/// <summary>Determines whether data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with, or can be converted to, the specified format.</summary>
		/// <param name="format">The format to check for. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <returns>
		///     <see langword="true" /> if data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with, or can be converted to, the specified format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002037 RID: 8247 RVA: 0x000A1628 File Offset: 0x0009F828
		public virtual bool GetDataPresent(string format)
		{
			return this.GetDataPresent(format, true);
		}

		/// <summary>Returns a list of all formats that data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with or can be converted to, using an automatic conversion parameter to determine whether to retrieve only native data formats or all formats that the data can be converted to.</summary>
		/// <param name="autoConvert">
		///       <see langword="true" /> to retrieve all formats that data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with, or can be converted to; <see langword="false" /> to retrieve only native data formats. </param>
		/// <returns>An array of type <see cref="T:System.String" />, containing a list of all formats that are supported by the data stored in this object.</returns>
		// Token: 0x06002038 RID: 8248 RVA: 0x000A163F File Offset: 0x0009F83F
		public virtual string[] GetFormats(bool autoConvert)
		{
			return this.innerData.GetFormats(autoConvert);
		}

		/// <summary>Returns a list of all formats that data stored in this <see cref="T:System.Windows.Forms.DataObject" /> is associated with or can be converted to.</summary>
		/// <returns>An array of type <see cref="T:System.String" />, containing a list of all formats that are supported by the data stored in this object.</returns>
		// Token: 0x06002039 RID: 8249 RVA: 0x000A164D File Offset: 0x0009F84D
		public virtual string[] GetFormats()
		{
			return this.GetFormats(true);
		}

		/// <summary>Indicates whether the data object contains data in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</summary>
		/// <returns>
		///     <see langword="true" /> if the data object contains audio data; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600203A RID: 8250 RVA: 0x000A1656 File Offset: 0x0009F856
		public virtual bool ContainsAudio()
		{
			return this.GetDataPresent(DataFormats.WaveAudio, false);
		}

		/// <summary>Indicates whether the data object contains data that is in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format or can be converted to that format.</summary>
		/// <returns>
		///     <see langword="true" /> if the data object contains a file drop list; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600203B RID: 8251 RVA: 0x000A1664 File Offset: 0x0009F864
		public virtual bool ContainsFileDropList()
		{
			return this.GetDataPresent(DataFormats.FileDrop, true);
		}

		/// <summary>Indicates whether the data object contains data that is in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format or can be converted to that format.</summary>
		/// <returns>
		///     <see langword="true" /> if the data object contains image data; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600203C RID: 8252 RVA: 0x000A1672 File Offset: 0x0009F872
		public virtual bool ContainsImage()
		{
			return this.GetDataPresent(DataFormats.Bitmap, true);
		}

		/// <summary>Indicates whether the data object contains data in the <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format.</summary>
		/// <returns>
		///     <see langword="true" /> if the data object contains text data; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600203D RID: 8253 RVA: 0x000A1680 File Offset: 0x0009F880
		public virtual bool ContainsText()
		{
			return this.ContainsText(TextDataFormat.UnicodeText);
		}

		/// <summary>Indicates whether the data object contains text data in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if the data object contains text data in the specified format; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x0600203E RID: 8254 RVA: 0x000A1689 File Offset: 0x0009F889
		public virtual bool ContainsText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			return this.GetDataPresent(DataObject.ConvertToDataFormats(format), false);
		}

		/// <summary>Retrieves an audio stream from the data object.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> containing audio data or <see langword="null" /> if the data object does not contain any data in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</returns>
		// Token: 0x0600203F RID: 8255 RVA: 0x000A16BE File Offset: 0x0009F8BE
		public virtual Stream GetAudioStream()
		{
			return this.GetData(DataFormats.WaveAudio, false) as Stream;
		}

		/// <summary>Retrieves a collection of file names from the data object. </summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> containing file names or <see langword="null" /> if the data object does not contain any data that is in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format or can be converted to that format.</returns>
		// Token: 0x06002040 RID: 8256 RVA: 0x000A16D4 File Offset: 0x0009F8D4
		public virtual StringCollection GetFileDropList()
		{
			StringCollection stringCollection = new StringCollection();
			string[] array = this.GetData(DataFormats.FileDrop, true) as string[];
			if (array != null)
			{
				stringCollection.AddRange(array);
			}
			return stringCollection;
		}

		/// <summary>Retrieves an image from the data object.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> representing the image data in the data object or <see langword="null" /> if the data object does not contain any data that is in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format or can be converted to that format.</returns>
		// Token: 0x06002041 RID: 8257 RVA: 0x000A1704 File Offset: 0x0009F904
		public virtual Image GetImage()
		{
			return this.GetData(DataFormats.Bitmap, true) as Image;
		}

		/// <summary>Retrieves text data from the data object in the <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format.</summary>
		/// <returns>The text data in the data object or <see cref="F:System.String.Empty" /> if the data object does not contain data in the <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format.</returns>
		// Token: 0x06002042 RID: 8258 RVA: 0x000A1717 File Offset: 0x0009F917
		public virtual string GetText()
		{
			return this.GetText(TextDataFormat.UnicodeText);
		}

		/// <summary>Retrieves text data from the data object in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <returns>The text data in the data object or <see cref="F:System.String.Empty" /> if the data object does not contain data in the specified format.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x06002043 RID: 8259 RVA: 0x000A1720 File Offset: 0x0009F920
		public virtual string GetText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			string text = this.GetData(DataObject.ConvertToDataFormats(format), false) as string;
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		/// <summary>Adds a <see cref="T:System.Byte" /> array to the data object in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format after converting it to a <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="audioBytes">A <see cref="T:System.Byte" /> array containing the audio data.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="audioBytes" /> is <see langword="null" />.</exception>
		// Token: 0x06002044 RID: 8260 RVA: 0x000A1770 File Offset: 0x0009F970
		public virtual void SetAudio(byte[] audioBytes)
		{
			if (audioBytes == null)
			{
				throw new ArgumentNullException("audioBytes");
			}
			this.SetAudio(new MemoryStream(audioBytes));
		}

		/// <summary>Adds a <see cref="T:System.IO.Stream" /> to the data object in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</summary>
		/// <param name="audioStream">A <see cref="T:System.IO.Stream" /> containing the audio data.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="audioStream" /> is <see langword="null" />.</exception>
		// Token: 0x06002045 RID: 8261 RVA: 0x000A178C File Offset: 0x0009F98C
		public virtual void SetAudio(Stream audioStream)
		{
			if (audioStream == null)
			{
				throw new ArgumentNullException("audioStream");
			}
			this.SetData(DataFormats.WaveAudio, false, audioStream);
		}

		/// <summary>Adds a collection of file names to the data object in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format.</summary>
		/// <param name="filePaths">A <see cref="T:System.Collections.Specialized.StringCollection" /> containing the file names.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="filePaths" /> is <see langword="null" />.</exception>
		// Token: 0x06002046 RID: 8262 RVA: 0x000A17AC File Offset: 0x0009F9AC
		public virtual void SetFileDropList(StringCollection filePaths)
		{
			if (filePaths == null)
			{
				throw new ArgumentNullException("filePaths");
			}
			string[] array = new string[filePaths.Count];
			filePaths.CopyTo(array, 0);
			this.SetData(DataFormats.FileDrop, true, array);
		}

		/// <summary>Adds an <see cref="T:System.Drawing.Image" /> to the data object in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to add to the data object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06002047 RID: 8263 RVA: 0x000A17E8 File Offset: 0x0009F9E8
		public virtual void SetImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			this.SetData(DataFormats.Bitmap, true, image);
		}

		/// <summary>Adds text data to the data object in the <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format.</summary>
		/// <param name="textData">The text to add to the data object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="textData" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		// Token: 0x06002048 RID: 8264 RVA: 0x000A1805 File Offset: 0x0009FA05
		public virtual void SetText(string textData)
		{
			this.SetText(textData, TextDataFormat.UnicodeText);
		}

		/// <summary>Adds text data to the data object in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="textData">The text to add to the data object.</param>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="textData" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x06002049 RID: 8265 RVA: 0x000A1810 File Offset: 0x0009FA10
		public virtual void SetText(string textData, TextDataFormat format)
		{
			if (string.IsNullOrEmpty(textData))
			{
				throw new ArgumentNullException("textData");
			}
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			this.SetData(DataObject.ConvertToDataFormats(format), false, textData);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000A1864 File Offset: 0x0009FA64
		private static string ConvertToDataFormats(TextDataFormat format)
		{
			switch (format)
			{
			case TextDataFormat.UnicodeText:
				return DataFormats.UnicodeText;
			case TextDataFormat.Rtf:
				return DataFormats.Rtf;
			case TextDataFormat.Html:
				return DataFormats.Html;
			case TextDataFormat.CommaSeparatedValue:
				return DataFormats.CommaSeparatedValue;
			default:
				return DataFormats.UnicodeText;
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000A18A0 File Offset: 0x0009FAA0
		private static string[] GetDistinctStrings(string[] formats)
		{
			ArrayList arrayList = new ArrayList();
			foreach (string text in formats)
			{
				if (!arrayList.Contains(text))
				{
					arrayList.Add(text);
				}
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000A18EC File Offset: 0x0009FAEC
		private static string[] GetMappedFormats(string format)
		{
			if (format == null)
			{
				return null;
			}
			if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.UnicodeText) || format.Equals(DataFormats.StringFormat))
			{
				return new string[]
				{
					DataFormats.StringFormat,
					DataFormats.UnicodeText,
					DataFormats.Text
				};
			}
			if (format.Equals(DataFormats.FileDrop) || format.Equals(DataObject.CF_DEPRECATED_FILENAME) || format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
			{
				return new string[]
				{
					DataFormats.FileDrop,
					DataObject.CF_DEPRECATED_FILENAMEW,
					DataObject.CF_DEPRECATED_FILENAME
				};
			}
			if (format.Equals(DataFormats.Bitmap) || format.Equals(typeof(Bitmap).FullName))
			{
				return new string[]
				{
					typeof(Bitmap).FullName,
					DataFormats.Bitmap
				};
			}
			return new string[]
			{
				format
			};
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000A19DC File Offset: 0x0009FBDC
		private bool GetTymedUseable(TYMED tymed)
		{
			for (int i = 0; i < DataObject.ALLOWED_TYMEDS.Length; i++)
			{
				if ((tymed & DataObject.ALLOWED_TYMEDS[i]) != TYMED.TYMED_NULL)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000A1A0C File Offset: 0x0009FC0C
		private void GetDataIntoOleStructs(ref FORMATETC formatetc, ref STGMEDIUM medium)
		{
			if (this.GetTymedUseable(formatetc.tymed) && this.GetTymedUseable(medium.tymed))
			{
				string name = DataFormats.GetFormat((int)formatetc.cfFormat).Name;
				if (!this.GetDataPresent(name))
				{
					Marshal.ThrowExceptionForHR(-2147221404);
					return;
				}
				object data = this.GetData(name);
				if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL)
				{
					int num = this.SaveDataToHandle(data, name, ref medium);
					if (NativeMethods.Failed(num))
					{
						Marshal.ThrowExceptionForHR(num);
						return;
					}
				}
				else
				{
					if ((formatetc.tymed & TYMED.TYMED_GDI) == TYMED.TYMED_NULL)
					{
						Marshal.ThrowExceptionForHR(-2147221399);
						return;
					}
					if (name.Equals(DataFormats.Bitmap) && data is Bitmap)
					{
						Bitmap bitmap = (Bitmap)data;
						if (bitmap != null)
						{
							medium.unionmember = this.GetCompatibleBitmap(bitmap);
							return;
						}
					}
				}
			}
			else
			{
				Marshal.ThrowExceptionForHR(-2147221399);
			}
		}

		/// <summary>Creates a connection between a data object and an advisory sink. This method is called by an object that supports an advisory sink and enables the advisory sink to be notified of changes in the object's data.</summary>
		/// <param name="pFormatetc"> A <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format, target device, aspect, and medium that will be used for future notifications.</param>
		/// <param name="advf">One of the <see cref="T:System.Runtime.InteropServices.ComTypes.ADVF" /> values that specifies a group of flags for controlling the advisory connection.</param>
		/// <param name="pAdvSink">A pointer to the <see cref="T:System.Runtime.InteropServices.ComTypes.IAdviseSink" /> interface on the advisory sink that will receive the change notification.</param>
		/// <param name="pdwConnection">When this method returns, contains a pointer to a DWORD token that identifies this connection. You can use this token later to delete the advisory connection by passing it to <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.DUnadvise(System.Int32)" />. If this value is zero, the connection was not established. This parameter is passed uninitialized.</param>
		/// <returns>This method supports the standard return values E_INVALIDARG, E_UNEXPECTED, and E_OUTOFMEMORY, as well as the following: ValueDescriptionS_OKThe advisory connection was created.E_NOTIMPLThis method is not implemented on the data object.DV_E_LINDEXThere is an invalid value for <see cref="F:System.Runtime.InteropServices.ComTypes.FORMATETC.lindex" />; currently, only -1 is supported.DV_E_FORMATETCThere is an invalid value for the <paramref name="pFormatetc" /> parameter.OLE_E_ADVISENOTSUPPORTEDThe data object does not support change notification.</returns>
		// Token: 0x0600204F RID: 8271 RVA: 0x000A1AD8 File Offset: 0x0009FCD8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink pAdvSink, out int pdwConnection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.DAdvise(ref pFormatetc, advf, pAdvSink, out pdwConnection);
			}
			pdwConnection = 0;
			return -2147467263;
		}

		/// <summary>Destroys a notification connection that had been previously established.</summary>
		/// <param name="dwConnection">A DWORD token that specifies the connection to remove. Use the value returned by <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.DAdvise(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.ADVF,System.Runtime.InteropServices.ComTypes.IAdviseSink,System.Int32@)" /> when the connection was originally established.</param>
		// Token: 0x06002050 RID: 8272 RVA: 0x000A1B0B File Offset: 0x0009FD0B
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.DUnadvise(int dwConnection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.DUnadvise(dwConnection);
				return;
			}
			Marshal.ThrowExceptionForHR(-2147467263);
		}

		/// <summary>Creates an object that can be used to enumerate the current advisory connections.</summary>
		/// <param name="enumAdvise">When this method returns, contains an <see cref="T:System.Runtime.InteropServices.ComTypes.IEnumSTATDATA" /> that receives the interface pointer to the new enumerator object. If the implementation sets <paramref name="enumAdvise" /> to <see langword="null" />, there are no connections to advisory sinks at this time. This parameter is passed uninitialized.</param>
		/// <returns>This method supports the standard return value E_OUTOFMEMORY, as well as the following:ValueDescriptionS_OKThe enumerator object is successfully instantiated or there are no connections.OLE_E_ADVISENOTSUPPORTEDThis object does not support advisory notifications.</returns>
		// Token: 0x06002051 RID: 8273 RVA: 0x000A1B3B File Offset: 0x0009FD3B
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.EnumDAdvise(out IEnumSTATDATA enumAdvise)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.EnumDAdvise(out enumAdvise);
			}
			enumAdvise = null;
			return -2147221501;
		}

		/// <summary>Creates an object for enumerating the <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structures for a data object. These structures are used in calls to <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" /> or <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.SetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@,System.Boolean)" />. </summary>
		/// <param name="dwDirection">One of the <see cref="T:System.Runtime.InteropServices.ComTypes.DATADIR" /> values that specifies the direction of the data.</param>
		/// <returns>This method supports the standard return values E_INVALIDARG and E_OUTOFMEMORY, as well as the following:ValueDescriptionS_OKThe enumerator object was successfully created.E_NOTIMPLThe direction specified by the <paramref name="direction" /> parameter is not supported.OLE_S_USEREGRequests that OLE enumerate the formats from the registry.</returns>
		// Token: 0x06002052 RID: 8274 RVA: 0x000A1B6C File Offset: 0x0009FD6C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		IEnumFORMATETC IDataObject.EnumFormatEtc(DATADIR dwDirection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.EnumFormatEtc(dwDirection);
			}
			if (dwDirection == DATADIR.DATADIR_GET)
			{
				return new DataObject.FormatEnumerator(this);
			}
			throw new ExternalException(SR.GetString("ExternalException"), -2147467263);
		}

		/// <summary>Provides a standard <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure that is logically equivalent to a more complex structure. Use this method to determine whether two different <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structures would return the same data, removing the need for duplicate rendering.</summary>
		/// <param name="pformatetcIn">A pointer to a <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format, medium, and target device that the caller would like to use to retrieve data in a subsequent call such as <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" />. The <see cref="T:System.Runtime.InteropServices.ComTypes.TYMED" /> member is not significant in this case and should be ignored.</param>
		/// <param name="pformatetcOut">When this method returns, contains a pointer to a <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure that contains the most general information possible for a specific rendering, making it canonically equivalent to <paramref name="formatetIn" />. The caller must allocate this structure and the <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetCanonicalFormatEtc(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.FORMATETC@)" /> method must fill in the data. To retrieve data in a subsequent call such as <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" />, the caller uses the supplied value of <paramref name="formatOut" />, unless the value supplied is <see langword="null" />. This value is <see langword="null" /> if the method returns <see langword="DATA_S_SAMEFORMATETC" />. The <see cref="T:System.Runtime.InteropServices.ComTypes.TYMED" /> member is not significant in this case and should be ignored. This parameter is passed uninitialized.</param>
		/// <returns>This method supports the standard return values E_INVALIDARG, E_UNEXPECTED, and E_OUTOFMEMORY, as well as the following: ValueDescriptionS_OKThe returned <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure is different from the one that was passed.DATA_S_SAMEFORMATETCThe <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structures are the same and <see langword="null" /> is returned in the <paramref name="formatOut" /> parameter.DV_E_LINDEXThere is an invalid value for <see cref="F:System.Runtime.InteropServices.ComTypes.FORMATETC.lindex" />; currently, only -1 is supported.DV_E_FORMATETCThere is an invalid value for the <paramref name="pFormatetc" /> parameter.OLE_E_NOTRUNNINGThe application is not running.</returns>
		// Token: 0x06002053 RID: 8275 RVA: 0x000A1BBC File Offset: 0x0009FDBC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.GetCanonicalFormatEtc(ref FORMATETC pformatetcIn, out FORMATETC pformatetcOut)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.GetCanonicalFormatEtc(ref pformatetcIn, out pformatetcOut);
			}
			pformatetcOut = default(FORMATETC);
			return 262448;
		}

		/// <summary>Obtains data from a source data object. The <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" /> method, which is called by a data consumer, renders the data described in the specified <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure and transfers it through the specified <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" /> structure. The caller then assumes responsibility for releasing the <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" /> structure.</summary>
		/// <param name="formatetc">A pointer to a <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format, medium, and target device to use when passing the data. It is possible to specify more than one medium by using the Boolean OR operator, allowing the method to choose the best medium among those specified.</param>
		/// <param name="medium">When this method returns, contains a pointer to the <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" /> structure that indicates the storage medium containing the returned data through its <see cref="F:System.Runtime.InteropServices.ComTypes.STGMEDIUM.tymed" /> member, and the responsibility for releasing the medium through the value of its <see cref="F:System.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease" /> member. If <see cref="F:System.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease" /> is <see langword="null" />, the receiver of the medium is responsible for releasing it; otherwise, <see cref="F:System.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease" /> points to the <see langword="IUnknown" /> interface on the appropriate object so its <see langword="Release" /> method can be called. The medium must be allocated and filled in by <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" />. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough memory to perform this operation.</exception>
		// Token: 0x06002054 RID: 8276 RVA: 0x000A1BF0 File Offset: 0x0009FDF0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.GetData(ref FORMATETC formatetc, out STGMEDIUM medium)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.GetData(ref formatetc, out medium);
				return;
			}
			medium = default(STGMEDIUM);
			if (this.GetTymedUseable(formatetc.tymed))
			{
				if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL)
				{
					medium.tymed = TYMED.TYMED_HGLOBAL;
					medium.unionmember = UnsafeNativeMethods.GlobalAlloc(8258, 1);
					if (medium.unionmember == IntPtr.Zero)
					{
						throw new OutOfMemoryException();
					}
					try
					{
						((IDataObject)this).GetDataHere(ref formatetc, ref medium);
						return;
					}
					catch
					{
						UnsafeNativeMethods.GlobalFree(new HandleRef(medium, medium.unionmember));
						medium.unionmember = IntPtr.Zero;
						throw;
					}
				}
				medium.tymed = formatetc.tymed;
				((IDataObject)this).GetDataHere(ref formatetc, ref medium);
				return;
			}
			Marshal.ThrowExceptionForHR(-2147221399);
		}

		/// <summary>Obtains data from a source data object. This method, which is called by a data consumer, differs from the <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" /> method in that the caller must allocate and free the specified storage medium.</summary>
		/// <param name="formatetc">A pointer to a <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format, medium, and target device to use when passing the data. Only one medium can be specified in <see cref="T:System.Runtime.InteropServices.ComTypes.TYMED" />, and only the following <see cref="T:System.Runtime.InteropServices.ComTypes.TYMED" /> values are valid: <see cref="F:System.Runtime.InteropServices.ComTypes.TYMED.TYMED_ISTORAGE" />, <see cref="F:System.Runtime.InteropServices.ComTypes.TYMED.TYMED_ISTREAM" />, <see cref="F:System.Runtime.InteropServices.ComTypes.TYMED.TYMED_HGLOBAL" />, or <see cref="F:System.Runtime.InteropServices.ComTypes.TYMED.TYMED_FILE" />.</param>
		/// <param name="medium">A <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" />, passed by reference, that defines the storage medium containing the data being transferred. The medium must be allocated by the caller and filled in by <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetDataHere(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" />. The caller must also free the medium. The implementation of this method must always supply a value of <see langword="null" /> for the <see cref="F:System.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease" /> member of the <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" /> structure that this parameter points to.</param>
		// Token: 0x06002055 RID: 8277 RVA: 0x000A1CD8 File Offset: 0x0009FED8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.GetDataHere(ref FORMATETC formatetc, ref STGMEDIUM medium)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.GetDataHere(ref formatetc, ref medium);
				return;
			}
			this.GetDataIntoOleStructs(ref formatetc, ref medium);
		}

		/// <summary>Determines whether the data object is capable of rendering the data described in the <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure. Objects attempting a paste or drop operation can call this method before calling <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" /> to get an indication of whether the operation may be successful.</summary>
		/// <param name="formatetc">A pointer to a <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format, medium, and target device to use for the query.</param>
		/// <returns>This method supports the standard return values E_INVALIDARG, E_UNEXPECTED, and E_OUTOFMEMORY, as well as the following: ValueDescriptionS_OKA subsequent call to <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.GetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@)" /> would probably be successful.DV_E_LINDEXAn invalid value for <see cref="F:System.Runtime.InteropServices.ComTypes.FORMATETC.lindex" />; currently, only -1 is supported.DV_E_FORMATETCAn invalid value for the <paramref name="pFormatetc" /> parameter.DV_E_TYMEDAn invalid <see cref="F:System.Runtime.InteropServices.ComTypes.FORMATETC.tymed" /> value.DV_E_DVASPECTAn invalid <see cref="F:System.Runtime.InteropServices.ComTypes.FORMATETC.dwAspect" /> value.OLE_E_NOTRUNNINGThe application is not running.</returns>
		// Token: 0x06002056 RID: 8278 RVA: 0x000A1D08 File Offset: 0x0009FF08
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.QueryGetData(ref FORMATETC formatetc)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.QueryGetData(ref formatetc);
			}
			if (formatetc.dwAspect != DVASPECT.DVASPECT_CONTENT)
			{
				return -2147221397;
			}
			if (!this.GetTymedUseable(formatetc.tymed))
			{
				return -2147221399;
			}
			if (formatetc.cfFormat == 0)
			{
				return 1;
			}
			if (!this.GetDataPresent(DataFormats.GetFormat((int)formatetc.cfFormat).Name))
			{
				return -2147221404;
			}
			return 0;
		}

		/// <summary>Transfers data to the object that implements this method. This method is called by an object that contains a data source.</summary>
		/// <param name="pFormatetcIn">A <see cref="T:System.Runtime.InteropServices.ComTypes.FORMATETC" /> structure, passed by reference, that defines the format used by the data object when interpreting the data contained in the storage medium.</param>
		/// <param name="pmedium">A <see cref="T:System.Runtime.InteropServices.ComTypes.STGMEDIUM" /> structure, passed by reference, that defines the storage medium in which the data is being passed.</param>
		/// <param name="fRelease">
		///       <see langword="true" /> to specify that the data object called, which implements <see cref="M:System.Runtime.InteropServices.ComTypes.IDataObject.SetData(System.Runtime.InteropServices.ComTypes.FORMATETC@,System.Runtime.InteropServices.ComTypes.STGMEDIUM@,System.Boolean)" />, owns the storage medium after the call returns. This means that the data object must free the medium after it has been used by calling the <see langword="ReleaseStgMedium" /> function. <see langword="false" /> to specify that the caller retains ownership of the storage medium, and the data object called uses the storage medium for the duration of the call only.</param>
		/// <exception cref="T:System.NotImplementedException">This method does not support the type of the underlying data object.</exception>
		// Token: 0x06002057 RID: 8279 RVA: 0x000A1D85 File Offset: 0x0009FF85
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.SetData(ref FORMATETC pFormatetcIn, ref STGMEDIUM pmedium, bool fRelease)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.SetData(ref pFormatetcIn, ref pmedium, fRelease);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000A1DB4 File Offset: 0x0009FFB4
		private int SaveDataToHandle(object data, string format, ref STGMEDIUM medium)
		{
			int result = -2147467259;
			if (data is Stream)
			{
				result = this.SaveStreamToHandle(ref medium.unionmember, (Stream)data);
			}
			else if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.Rtf) || format.Equals(DataFormats.OemText))
			{
				result = this.SaveStringToHandle(medium.unionmember, data.ToString(), false);
			}
			else if (format.Equals(DataFormats.Html))
			{
				if (WindowsFormsUtils.TargetsAtLeast_v4_5)
				{
					result = this.SaveHtmlToHandle(medium.unionmember, data.ToString());
				}
				else
				{
					result = this.SaveStringToHandle(medium.unionmember, data.ToString(), false);
				}
			}
			else if (format.Equals(DataFormats.UnicodeText))
			{
				result = this.SaveStringToHandle(medium.unionmember, data.ToString(), true);
			}
			else if (format.Equals(DataFormats.FileDrop))
			{
				result = this.SaveFileListToHandle(medium.unionmember, (string[])data);
			}
			else if (format.Equals(DataObject.CF_DEPRECATED_FILENAME))
			{
				string[] array = (string[])data;
				result = this.SaveStringToHandle(medium.unionmember, array[0], false);
			}
			else if (format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
			{
				string[] array2 = (string[])data;
				result = this.SaveStringToHandle(medium.unionmember, array2[0], true);
			}
			else if (format.Equals(DataFormats.Dib) && data is Image)
			{
				result = -2147221399;
			}
			else if (format.Equals(DataFormats.Serializable) || data is ISerializable || (data != null && data.GetType().IsSerializable))
			{
				result = this.SaveObjectToHandle(ref medium.unionmember, data);
			}
			return result;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000A1F58 File Offset: 0x000A0158
		private int SaveObjectToHandle(ref IntPtr handle, object data)
		{
			Stream stream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(DataObject.serializedObjectID);
			DataObject.SaveObjectToHandleSerializer(stream, data);
			return this.SaveStreamToHandle(ref handle, stream);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000A1F8C File Offset: 0x000A018C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private static void SaveObjectToHandleSerializer(Stream stream, object data)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(stream, data);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000A1FA8 File Offset: 0x000A01A8
		private int SaveStreamToHandle(ref IntPtr handle, Stream stream)
		{
			if (handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.GlobalFree(new HandleRef(null, handle));
			}
			int num = (int)stream.Length;
			handle = UnsafeNativeMethods.GlobalAlloc(8194, num);
			if (handle == IntPtr.Zero)
			{
				return -2147024882;
			}
			IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
			if (intPtr == IntPtr.Zero)
			{
				return -2147024882;
			}
			try
			{
				byte[] array = new byte[num];
				stream.Position = 0L;
				stream.Read(array, 0, num);
				Marshal.Copy(array, 0, intPtr, num);
			}
			finally
			{
				UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
			}
			return 0;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000A2060 File Offset: 0x000A0260
		private int SaveFileListToHandle(IntPtr handle, string[] files)
		{
			if (files == null)
			{
				return 0;
			}
			if (files.Length < 1)
			{
				return 0;
			}
			if (handle == IntPtr.Zero)
			{
				return -2147024809;
			}
			bool flag = Marshal.SystemDefaultCharSize != 1;
			IntPtr intPtr = IntPtr.Zero;
			int num = 20;
			int num2 = num;
			if (flag)
			{
				for (int i = 0; i < files.Length; i++)
				{
					num2 += (files[i].Length + 1) * 2;
				}
				num2 += 2;
			}
			else
			{
				for (int j = 0; j < files.Length; j++)
				{
					num2 += NativeMethods.Util.GetPInvokeStringLength(files[j]) + 1;
				}
				num2++;
			}
			IntPtr intPtr2 = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), num2, 8194);
			if (intPtr2 == IntPtr.Zero)
			{
				return -2147024882;
			}
			IntPtr intPtr3 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr2));
			if (intPtr3 == IntPtr.Zero)
			{
				return -2147024882;
			}
			intPtr = intPtr3;
			int[] array = new int[5];
			array[0] = num;
			int[] array2 = array;
			if (flag)
			{
				array2[4] = -1;
			}
			Marshal.Copy(array2, 0, intPtr, array2.Length);
			intPtr = (IntPtr)((long)intPtr + (long)num);
			for (int k = 0; k < files.Length; k++)
			{
				if (flag)
				{
					UnsafeNativeMethods.CopyMemoryW(intPtr, files[k], files[k].Length * 2);
					intPtr = (IntPtr)((long)intPtr + (long)(files[k].Length * 2));
					Marshal.Copy(new byte[2], 0, intPtr, 2);
					intPtr = (IntPtr)((long)intPtr + 2L);
				}
				else
				{
					int pinvokeStringLength = NativeMethods.Util.GetPInvokeStringLength(files[k]);
					UnsafeNativeMethods.CopyMemoryA(intPtr, files[k], pinvokeStringLength);
					intPtr = (IntPtr)((long)intPtr + (long)pinvokeStringLength);
					Marshal.Copy(new byte[1], 0, intPtr, 1);
					intPtr = (IntPtr)((long)intPtr + 1L);
				}
			}
			if (flag)
			{
				Marshal.Copy(new char[1], 0, intPtr, 1);
				intPtr = (IntPtr)((long)intPtr + 2L);
			}
			else
			{
				Marshal.Copy(new byte[1], 0, intPtr, 1);
				intPtr = (IntPtr)((long)intPtr + 1L);
			}
			UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr2));
			return 0;
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000A226C File Offset: 0x000A046C
		private int SaveStringToHandle(IntPtr handle, string str, bool unicode)
		{
			if (handle == IntPtr.Zero)
			{
				return -2147024809;
			}
			IntPtr intPtr = IntPtr.Zero;
			if (unicode)
			{
				int bytes = str.Length * 2 + 2;
				intPtr = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), bytes, 8258);
				if (intPtr == IntPtr.Zero)
				{
					return -2147024882;
				}
				IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
				if (intPtr2 == IntPtr.Zero)
				{
					return -2147024882;
				}
				char[] array = str.ToCharArray(0, str.Length);
				UnsafeNativeMethods.CopyMemoryW(intPtr2, array, array.Length * 2);
			}
			else
			{
				int num = UnsafeNativeMethods.WideCharToMultiByte(0, 0, str, str.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array2 = new byte[num];
				UnsafeNativeMethods.WideCharToMultiByte(0, 0, str, str.Length, array2, array2.Length, IntPtr.Zero, IntPtr.Zero);
				intPtr = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), num + 1, 8258);
				if (intPtr == IntPtr.Zero)
				{
					return -2147024882;
				}
				IntPtr intPtr3 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
				if (intPtr3 == IntPtr.Zero)
				{
					return -2147024882;
				}
				UnsafeNativeMethods.CopyMemory(intPtr3, array2, num);
				Marshal.Copy(new byte[1], 0, (IntPtr)((long)intPtr3 + (long)num), 1);
			}
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
			}
			return 0;
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000A23D8 File Offset: 0x000A05D8
		private int SaveHtmlToHandle(IntPtr handle, string str)
		{
			if (handle == IntPtr.Zero)
			{
				return -2147024809;
			}
			IntPtr intPtr = IntPtr.Zero;
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] bytes = utf8Encoding.GetBytes(str);
			intPtr = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), bytes.Length + 1, 8258);
			if (intPtr == IntPtr.Zero)
			{
				return -2147024882;
			}
			IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
			if (intPtr2 == IntPtr.Zero)
			{
				return -2147024882;
			}
			try
			{
				UnsafeNativeMethods.CopyMemory(intPtr2, bytes, bytes.Length);
				Marshal.Copy(new byte[1], 0, (IntPtr)((long)intPtr2 + (long)bytes.Length), 1);
			}
			finally
			{
				UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
			}
			return 0;
		}

		/// <summary>Adds the specified object to the <see cref="T:System.Windows.Forms.DataObject" /> using the specified format and indicating whether the data can be converted to another format.</summary>
		/// <param name="format">The format associated with the data. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <param name="autoConvert">
		///       <see langword="true" /> to allow the data to be converted to another format; otherwise, <see langword="false" />. </param>
		/// <param name="data">The data to store. </param>
		// Token: 0x0600205F RID: 8287 RVA: 0x000A24A0 File Offset: 0x000A06A0
		public virtual void SetData(string format, bool autoConvert, object data)
		{
			this.innerData.SetData(format, autoConvert, data);
		}

		/// <summary>Adds the specified object to the <see cref="T:System.Windows.Forms.DataObject" /> using the specified format.</summary>
		/// <param name="format">The format associated with the data. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats. </param>
		/// <param name="data">The data to store. </param>
		// Token: 0x06002060 RID: 8288 RVA: 0x000A24B0 File Offset: 0x000A06B0
		public virtual void SetData(string format, object data)
		{
			this.innerData.SetData(format, data);
		}

		/// <summary>Adds the specified object to the <see cref="T:System.Windows.Forms.DataObject" /> using the specified type as the format.</summary>
		/// <param name="format">A <see cref="T:System.Type" /> representing the format associated with the data. </param>
		/// <param name="data">The data to store. </param>
		// Token: 0x06002061 RID: 8289 RVA: 0x000A24BF File Offset: 0x000A06BF
		public virtual void SetData(Type format, object data)
		{
			this.innerData.SetData(format, data);
		}

		/// <summary>Adds the specified object to the <see cref="T:System.Windows.Forms.DataObject" /> using the object type as the data format.</summary>
		/// <param name="data">The data to store. </param>
		// Token: 0x06002062 RID: 8290 RVA: 0x000A24CE File Offset: 0x000A06CE
		public virtual void SetData(object data)
		{
			this.innerData.SetData(data);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000A24DC File Offset: 0x000A06DC
		// Note: this type is marked as 'beforefieldinit'.
		static DataObject()
		{
			TYMED[] array = new TYMED[5];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.5349A08D8A4FDDAE8155740A69720675F65FC008).FieldHandle);
			DataObject.ALLOWED_TYMEDS = array;
			DataObject.serializedObjectID = new Guid("FD9EA796-3B13-4370-A679-56106BB288FB").ToByteArray();
		}

		// Token: 0x04000DEA RID: 3562
		private static readonly string CF_DEPRECATED_FILENAME = "FileName";

		// Token: 0x04000DEB RID: 3563
		private static readonly string CF_DEPRECATED_FILENAMEW = "FileNameW";

		// Token: 0x04000DEC RID: 3564
		private const int DV_E_FORMATETC = -2147221404;

		// Token: 0x04000DED RID: 3565
		private const int DV_E_LINDEX = -2147221400;

		// Token: 0x04000DEE RID: 3566
		private const int DV_E_TYMED = -2147221399;

		// Token: 0x04000DEF RID: 3567
		private const int DV_E_DVASPECT = -2147221397;

		// Token: 0x04000DF0 RID: 3568
		private const int OLE_E_NOTRUNNING = -2147221499;

		// Token: 0x04000DF1 RID: 3569
		private const int OLE_E_ADVISENOTSUPPORTED = -2147221501;

		// Token: 0x04000DF2 RID: 3570
		private const int DATA_S_SAMEFORMATETC = 262448;

		// Token: 0x04000DF3 RID: 3571
		private static readonly TYMED[] ALLOWED_TYMEDS;

		// Token: 0x04000DF4 RID: 3572
		private IDataObject innerData;

		// Token: 0x04000DF6 RID: 3574
		private static readonly byte[] serializedObjectID;

		// Token: 0x020005C3 RID: 1475
		private class FormatEnumerator : IEnumFORMATETC
		{
			// Token: 0x060059EA RID: 23018 RVA: 0x0017ABE9 File Offset: 0x00178DE9
			public FormatEnumerator(IDataObject parent) : this(parent, parent.GetFormats())
			{
			}

			// Token: 0x060059EB RID: 23019 RVA: 0x0017ABF8 File Offset: 0x00178DF8
			public FormatEnumerator(IDataObject parent, FORMATETC[] formats)
			{
				this.formats = new ArrayList();
				base..ctor();
				this.formats.Clear();
				this.parent = parent;
				this.current = 0;
				if (formats != null)
				{
					DataObject dataObject = parent as DataObject;
					if (dataObject != null && dataObject.RestrictedFormats && !Clipboard.IsFormatValid(formats))
					{
						throw new SecurityException(SR.GetString("ClipboardSecurityException"));
					}
					foreach (FORMATETC formatetc in formats)
					{
						FORMATETC formatetc2 = default(FORMATETC);
						formatetc2.cfFormat = formatetc.cfFormat;
						formatetc2.dwAspect = formatetc.dwAspect;
						formatetc2.ptd = formatetc.ptd;
						formatetc2.lindex = formatetc.lindex;
						formatetc2.tymed = formatetc.tymed;
						this.formats.Add(formatetc2);
					}
				}
			}

			// Token: 0x060059EC RID: 23020 RVA: 0x0017ACD0 File Offset: 0x00178ED0
			public FormatEnumerator(IDataObject parent, string[] formats)
			{
				this.formats = new ArrayList();
				base..ctor();
				this.parent = parent;
				this.formats.Clear();
				string bitmap = DataFormats.Bitmap;
				string enhancedMetafile = DataFormats.EnhancedMetafile;
				string text = DataFormats.Text;
				string unicodeText = DataFormats.UnicodeText;
				string stringFormat = DataFormats.StringFormat;
				string stringFormat2 = DataFormats.StringFormat;
				if (formats != null)
				{
					DataObject dataObject = parent as DataObject;
					if (dataObject != null && dataObject.RestrictedFormats && !Clipboard.IsFormatValid(formats))
					{
						throw new SecurityException(SR.GetString("ClipboardSecurityException"));
					}
					foreach (string text2 in formats)
					{
						FORMATETC formatetc = default(FORMATETC);
						formatetc.cfFormat = (short)((ushort)DataFormats.GetFormat(text2).Id);
						formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
						formatetc.ptd = IntPtr.Zero;
						formatetc.lindex = -1;
						if (text2.Equals(bitmap))
						{
							formatetc.tymed = TYMED.TYMED_GDI;
						}
						else if (text2.Equals(enhancedMetafile))
						{
							formatetc.tymed = TYMED.TYMED_ENHMF;
						}
						else if (text2.Equals(text) || text2.Equals(unicodeText) || text2.Equals(stringFormat) || text2.Equals(stringFormat2) || text2.Equals(DataFormats.Rtf) || text2.Equals(DataFormats.CommaSeparatedValue) || text2.Equals(DataFormats.FileDrop) || text2.Equals(DataObject.CF_DEPRECATED_FILENAME) || text2.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
						{
							formatetc.tymed = TYMED.TYMED_HGLOBAL;
						}
						else
						{
							formatetc.tymed = TYMED.TYMED_HGLOBAL;
						}
						if (formatetc.tymed != TYMED.TYMED_NULL)
						{
							this.formats.Add(formatetc);
						}
					}
				}
			}

			// Token: 0x060059ED RID: 23021 RVA: 0x0017AE80 File Offset: 0x00179080
			public int Next(int celt, FORMATETC[] rgelt, int[] pceltFetched)
			{
				if (this.current < this.formats.Count && celt > 0)
				{
					FORMATETC formatetc = (FORMATETC)this.formats[this.current];
					rgelt[0].cfFormat = formatetc.cfFormat;
					rgelt[0].tymed = formatetc.tymed;
					rgelt[0].dwAspect = DVASPECT.DVASPECT_CONTENT;
					rgelt[0].ptd = IntPtr.Zero;
					rgelt[0].lindex = -1;
					if (pceltFetched != null)
					{
						pceltFetched[0] = 1;
					}
					this.current++;
					return 0;
				}
				if (pceltFetched != null)
				{
					pceltFetched[0] = 0;
				}
				return 1;
			}

			// Token: 0x060059EE RID: 23022 RVA: 0x0017AF2E File Offset: 0x0017912E
			public int Skip(int celt)
			{
				if (this.current + celt >= this.formats.Count)
				{
					return 1;
				}
				this.current += celt;
				return 0;
			}

			// Token: 0x060059EF RID: 23023 RVA: 0x0017AF56 File Offset: 0x00179156
			public int Reset()
			{
				this.current = 0;
				return 0;
			}

			// Token: 0x060059F0 RID: 23024 RVA: 0x0017AF60 File Offset: 0x00179160
			public void Clone(out IEnumFORMATETC ppenum)
			{
				FORMATETC[] array = new FORMATETC[this.formats.Count];
				this.formats.CopyTo(array, 0);
				ppenum = new DataObject.FormatEnumerator(this.parent, array);
			}

			// Token: 0x0400394B RID: 14667
			internal IDataObject parent;

			// Token: 0x0400394C RID: 14668
			internal ArrayList formats;

			// Token: 0x0400394D RID: 14669
			internal int current;
		}

		// Token: 0x020005C4 RID: 1476
		private class OleConverter : IDataObject
		{
			// Token: 0x060059F1 RID: 23025 RVA: 0x0017AF99 File Offset: 0x00179199
			public OleConverter(IDataObject data)
			{
				this.innerData = data;
			}

			// Token: 0x170015C8 RID: 5576
			// (get) Token: 0x060059F2 RID: 23026 RVA: 0x0017AFA8 File Offset: 0x001791A8
			public IDataObject OleDataObject
			{
				get
				{
					return this.innerData;
				}
			}

			// Token: 0x060059F3 RID: 23027 RVA: 0x0017AFB0 File Offset: 0x001791B0
			private object GetDataFromOleIStream(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				formatetc.cfFormat = (short)((ushort)DataFormats.GetFormat(format).Id);
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = TYMED.TYMED_ISTREAM;
				stgmedium.tymed = TYMED.TYMED_ISTREAM;
				if (this.QueryGetDataUnsafe(ref formatetc) != 0)
				{
					return null;
				}
				try
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						this.innerData.GetData(ref formatetc, out stgmedium);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				catch
				{
					return null;
				}
				if (stgmedium.unionmember != IntPtr.Zero)
				{
					UnsafeNativeMethods.IStream stream = (UnsafeNativeMethods.IStream)Marshal.GetObjectForIUnknown(stgmedium.unionmember);
					Marshal.Release(stgmedium.unionmember);
					NativeMethods.STATSTG statstg = new NativeMethods.STATSTG();
					stream.Stat(statstg, 0);
					int num = (int)statstg.cbSize;
					IntPtr intPtr = UnsafeNativeMethods.GlobalAlloc(8258, num);
					IntPtr buf = UnsafeNativeMethods.GlobalLock(new HandleRef(this.innerData, intPtr));
					stream.Read(buf, num);
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(this.innerData, intPtr));
					return this.GetDataFromHGLOBLAL(format, intPtr);
				}
				return null;
			}

			// Token: 0x060059F4 RID: 23028 RVA: 0x0017B0F0 File Offset: 0x001792F0
			private object GetDataFromHGLOBLAL(string format, IntPtr hglobal)
			{
				object result = null;
				if (hglobal != IntPtr.Zero)
				{
					if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.Rtf) || format.Equals(DataFormats.OemText))
					{
						result = this.ReadStringFromHandle(hglobal, false);
					}
					else if (format.Equals(DataFormats.Html))
					{
						if (WindowsFormsUtils.TargetsAtLeast_v4_5)
						{
							result = this.ReadHtmlFromHandle(hglobal);
						}
						else
						{
							result = this.ReadStringFromHandle(hglobal, false);
						}
					}
					else if (format.Equals(DataFormats.UnicodeText))
					{
						result = this.ReadStringFromHandle(hglobal, true);
					}
					else if (format.Equals(DataFormats.FileDrop))
					{
						result = this.ReadFileListFromHandle(hglobal);
					}
					else if (format.Equals(DataObject.CF_DEPRECATED_FILENAME))
					{
						result = new string[]
						{
							this.ReadStringFromHandle(hglobal, false)
						};
					}
					else if (format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
					{
						result = new string[]
						{
							this.ReadStringFromHandle(hglobal, true)
						};
					}
					else if (!LocalAppContextSwitches.EnableLegacyDangerousClipboardDeserializationMode)
					{
						bool restrictDeserialization = format.Equals(DataFormats.StringFormat) || format.Equals(typeof(Bitmap).FullName) || format.Equals(DataFormats.CommaSeparatedValue) || format.Equals(DataFormats.Dib) || format.Equals(DataFormats.Dif) || format.Equals(DataFormats.Locale) || format.Equals(DataFormats.PenData) || format.Equals(DataFormats.Riff) || format.Equals(DataFormats.SymbolicLink) || format.Equals(DataFormats.Tiff) || format.Equals(DataFormats.WaveAudio) || format.Equals(DataFormats.Bitmap) || format.Equals(DataFormats.EnhancedMetafile) || format.Equals(DataFormats.Palette) || format.Equals(DataFormats.MetafilePict);
						result = this.ReadObjectFromHandle(hglobal, restrictDeserialization);
					}
					else
					{
						result = this.ReadObjectFromHandle(hglobal, false);
					}
					UnsafeNativeMethods.GlobalFree(new HandleRef(null, hglobal));
				}
				return result;
			}

			// Token: 0x060059F5 RID: 23029 RVA: 0x0017B2FC File Offset: 0x001794FC
			private object GetDataFromOleHGLOBAL(string format, out bool done)
			{
				done = false;
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				formatetc.cfFormat = (short)((ushort)DataFormats.GetFormat(format).Id);
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = TYMED.TYMED_HGLOBAL;
				stgmedium.tymed = TYMED.TYMED_HGLOBAL;
				object result = null;
				if (this.QueryGetDataUnsafe(ref formatetc) == 0)
				{
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							this.innerData.GetData(ref formatetc, out stgmedium);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (stgmedium.unionmember != IntPtr.Zero)
						{
							result = this.GetDataFromHGLOBLAL(format, stgmedium.unionmember);
						}
					}
					catch (DataObject.OleConverter.RestrictedTypeDeserializationException)
					{
						done = true;
					}
					catch
					{
					}
				}
				return result;
			}

			// Token: 0x060059F6 RID: 23030 RVA: 0x0017B3D4 File Offset: 0x001795D4
			private object GetDataFromOleOther(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				TYMED tymed = TYMED.TYMED_NULL;
				if (format.Equals(DataFormats.Bitmap))
				{
					tymed = TYMED.TYMED_GDI;
				}
				else if (format.Equals(DataFormats.EnhancedMetafile))
				{
					tymed = TYMED.TYMED_ENHMF;
				}
				if (tymed == TYMED.TYMED_NULL)
				{
					return null;
				}
				formatetc.cfFormat = (short)((ushort)DataFormats.GetFormat(format).Id);
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = tymed;
				stgmedium.tymed = tymed;
				object result = null;
				if (this.QueryGetDataUnsafe(ref formatetc) == 0)
				{
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							this.innerData.GetData(ref formatetc, out stgmedium);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					catch
					{
					}
				}
				if (stgmedium.unionmember != IntPtr.Zero && format.Equals(DataFormats.Bitmap))
				{
					System.Internal.HandleCollector.Add(stgmedium.unionmember, NativeMethods.CommonHandles.GDI);
					Image image = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						image = Image.FromHbitmap(stgmedium.unionmember);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (image != null)
					{
						Image image2 = image;
						image = (Image)image.Clone();
						SafeNativeMethods.DeleteObject(new HandleRef(null, stgmedium.unionmember));
						image2.Dispose();
					}
					result = image;
				}
				return result;
			}

			// Token: 0x060059F7 RID: 23031 RVA: 0x0017B530 File Offset: 0x00179730
			private object GetDataFromBoundOleDataObject(string format, out bool done)
			{
				object obj = null;
				done = false;
				try
				{
					obj = this.GetDataFromOleOther(format);
					if (obj == null)
					{
						obj = this.GetDataFromOleHGLOBAL(format, out done);
					}
					if (obj == null && !done)
					{
						obj = this.GetDataFromOleIStream(format);
					}
				}
				catch (Exception ex)
				{
				}
				return obj;
			}

			// Token: 0x060059F8 RID: 23032 RVA: 0x0017B57C File Offset: 0x0017977C
			private Stream ReadByteStreamFromHandle(IntPtr handle, out bool isSerializedObject)
			{
				IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					throw new ExternalException(SR.GetString("ExternalException"), -2147024882);
				}
				Stream result;
				try
				{
					int num = UnsafeNativeMethods.GlobalSize(new HandleRef(null, handle));
					byte[] array = new byte[num];
					Marshal.Copy(intPtr, array, 0, num);
					int num2 = 0;
					if (num > DataObject.serializedObjectID.Length)
					{
						isSerializedObject = true;
						for (int i = 0; i < DataObject.serializedObjectID.Length; i++)
						{
							if (DataObject.serializedObjectID[i] != array[i])
							{
								isSerializedObject = false;
								break;
							}
						}
						if (isSerializedObject)
						{
							num2 = DataObject.serializedObjectID.Length;
						}
					}
					else
					{
						isSerializedObject = false;
					}
					result = new MemoryStream(array, num2, array.Length - num2);
				}
				finally
				{
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
				}
				return result;
			}

			// Token: 0x060059F9 RID: 23033 RVA: 0x0017B650 File Offset: 0x00179850
			private object ReadObjectFromHandle(IntPtr handle, bool restrictDeserialization)
			{
				bool flag;
				Stream stream = this.ReadByteStreamFromHandle(handle, out flag);
				object result;
				if (flag)
				{
					result = DataObject.OleConverter.ReadObjectFromHandleDeserializer(stream, restrictDeserialization);
				}
				else
				{
					result = stream;
				}
				return result;
			}

			// Token: 0x060059FA RID: 23034 RVA: 0x0017B67C File Offset: 0x0017987C
			private static object ReadObjectFromHandleDeserializer(Stream stream, bool restrictDeserialization)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				if (restrictDeserialization)
				{
					binaryFormatter.Binder = new DataObject.OleConverter.RestrictiveBinder();
				}
				binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				return binaryFormatter.Deserialize(stream);
			}

			// Token: 0x060059FB RID: 23035 RVA: 0x0017B6AC File Offset: 0x001798AC
			private string[] ReadFileListFromHandle(IntPtr hdrop)
			{
				string[] array = null;
				StringBuilder stringBuilder = new StringBuilder(260);
				int num = UnsafeNativeMethods.DragQueryFile(new HandleRef(null, hdrop), -1, null, 0);
				if (num > 0)
				{
					array = new string[num];
					for (int i = 0; i < num; i++)
					{
						int num2 = UnsafeNativeMethods.DragQueryFileLongPath(new HandleRef(null, hdrop), i, stringBuilder);
						if (num2 != 0)
						{
							string text = stringBuilder.ToString(0, num2);
							string fullPath = Path.GetFullPath(text);
							new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullPath).Demand();
							array[i] = text;
						}
					}
				}
				return array;
			}

			// Token: 0x060059FC RID: 23036 RVA: 0x0017B728 File Offset: 0x00179928
			private unsafe string ReadStringFromHandle(IntPtr handle, bool unicode)
			{
				string result = null;
				IntPtr value = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
				try
				{
					if (unicode)
					{
						result = new string((char*)((void*)value));
					}
					else
					{
						result = new string((sbyte*)((void*)value));
					}
				}
				finally
				{
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
				}
				return result;
			}

			// Token: 0x060059FD RID: 23037 RVA: 0x0017B784 File Offset: 0x00179984
			private string ReadHtmlFromHandle(IntPtr handle)
			{
				string result = null;
				IntPtr source = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
				try
				{
					int num = UnsafeNativeMethods.GlobalSize(new HandleRef(null, handle));
					byte[] array = new byte[num];
					Marshal.Copy(source, array, 0, num);
					result = Encoding.UTF8.GetString(array);
				}
				finally
				{
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
				}
				return result;
			}

			// Token: 0x060059FE RID: 23038 RVA: 0x0017B7EC File Offset: 0x001799EC
			public virtual object GetData(string format, bool autoConvert)
			{
				bool flag = false;
				object dataFromBoundOleDataObject = this.GetDataFromBoundOleDataObject(format, out flag);
				object obj = dataFromBoundOleDataObject;
				if (!flag && autoConvert && (dataFromBoundOleDataObject == null || dataFromBoundOleDataObject is MemoryStream))
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						int num = 0;
						while (!flag && num < mappedFormats.Length)
						{
							if (!format.Equals(mappedFormats[num]))
							{
								dataFromBoundOleDataObject = this.GetDataFromBoundOleDataObject(mappedFormats[num], out flag);
								if (!flag && dataFromBoundOleDataObject != null && !(dataFromBoundOleDataObject is MemoryStream))
								{
									obj = null;
									break;
								}
							}
							num++;
						}
					}
				}
				if (obj != null)
				{
					return obj;
				}
				return dataFromBoundOleDataObject;
			}

			// Token: 0x060059FF RID: 23039 RVA: 0x0017B86A File Offset: 0x00179A6A
			public virtual object GetData(string format)
			{
				return this.GetData(format, true);
			}

			// Token: 0x06005A00 RID: 23040 RVA: 0x0017B874 File Offset: 0x00179A74
			public virtual object GetData(Type format)
			{
				return this.GetData(format.FullName);
			}

			// Token: 0x06005A01 RID: 23041 RVA: 0x0000701A File Offset: 0x0000521A
			public virtual void SetData(string format, bool autoConvert, object data)
			{
			}

			// Token: 0x06005A02 RID: 23042 RVA: 0x0017B882 File Offset: 0x00179A82
			public virtual void SetData(string format, object data)
			{
				this.SetData(format, true, data);
			}

			// Token: 0x06005A03 RID: 23043 RVA: 0x0017B88D File Offset: 0x00179A8D
			public virtual void SetData(Type format, object data)
			{
				this.SetData(format.FullName, data);
			}

			// Token: 0x06005A04 RID: 23044 RVA: 0x0017B89C File Offset: 0x00179A9C
			public virtual void SetData(object data)
			{
				if (data is ISerializable)
				{
					this.SetData(DataFormats.Serializable, data);
					return;
				}
				this.SetData(data.GetType(), data);
			}

			// Token: 0x06005A05 RID: 23045 RVA: 0x0017B8C0 File Offset: 0x00179AC0
			[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
			private int QueryGetDataUnsafe(ref FORMATETC formatetc)
			{
				return this.innerData.QueryGetData(ref formatetc);
			}

			// Token: 0x06005A06 RID: 23046 RVA: 0x0017B8C0 File Offset: 0x00179AC0
			private int QueryGetDataInner(ref FORMATETC formatetc)
			{
				return this.innerData.QueryGetData(ref formatetc);
			}

			// Token: 0x06005A07 RID: 23047 RVA: 0x0017B8CE File Offset: 0x00179ACE
			public virtual bool GetDataPresent(Type format)
			{
				return this.GetDataPresent(format.FullName);
			}

			// Token: 0x06005A08 RID: 23048 RVA: 0x0017B8DC File Offset: 0x00179ADC
			private bool GetDataPresentInner(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				formatetc.cfFormat = (short)((ushort)DataFormats.GetFormat(format).Id);
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				for (int i = 0; i < DataObject.ALLOWED_TYMEDS.Length; i++)
				{
					formatetc.tymed |= DataObject.ALLOWED_TYMEDS[i];
				}
				int num = this.QueryGetDataUnsafe(ref formatetc);
				return num == 0;
			}

			// Token: 0x06005A09 RID: 23049 RVA: 0x0017B948 File Offset: 0x00179B48
			public virtual bool GetDataPresent(string format, bool autoConvert)
			{
				IntSecurity.ClipboardRead.Demand();
				bool flag = false;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					flag = this.GetDataPresentInner(format);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (!flag && autoConvert)
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						for (int i = 0; i < mappedFormats.Length; i++)
						{
							if (!format.Equals(mappedFormats[i]))
							{
								IntSecurity.UnmanagedCode.Assert();
								try
								{
									flag = this.GetDataPresentInner(mappedFormats[i]);
								}
								finally
								{
									CodeAccessPermission.RevertAssert();
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
				return flag;
			}

			// Token: 0x06005A0A RID: 23050 RVA: 0x0017B9E4 File Offset: 0x00179BE4
			public virtual bool GetDataPresent(string format)
			{
				return this.GetDataPresent(format, true);
			}

			// Token: 0x06005A0B RID: 23051 RVA: 0x0017B9F0 File Offset: 0x00179BF0
			public virtual string[] GetFormats(bool autoConvert)
			{
				IEnumFORMATETC enumFORMATETC = null;
				ArrayList arrayList = new ArrayList();
				try
				{
					enumFORMATETC = this.innerData.EnumFormatEtc(DATADIR.DATADIR_GET);
				}
				catch
				{
				}
				if (enumFORMATETC != null)
				{
					enumFORMATETC.Reset();
					FORMATETC[] array = new FORMATETC[1];
					int[] array2 = new int[]
					{
						1
					};
					while (array2[0] > 0)
					{
						array2[0] = 0;
						try
						{
							enumFORMATETC.Next(1, array, array2);
						}
						catch
						{
						}
						if (array2[0] > 0)
						{
							string name = DataFormats.GetFormat((int)array[0].cfFormat).Name;
							if (autoConvert)
							{
								string[] mappedFormats = DataObject.GetMappedFormats(name);
								for (int i = 0; i < mappedFormats.Length; i++)
								{
									arrayList.Add(mappedFormats[i]);
								}
							}
							else
							{
								arrayList.Add(name);
							}
						}
					}
				}
				string[] array3 = new string[arrayList.Count];
				arrayList.CopyTo(array3, 0);
				return DataObject.GetDistinctStrings(array3);
			}

			// Token: 0x06005A0C RID: 23052 RVA: 0x0017BAE0 File Offset: 0x00179CE0
			public virtual string[] GetFormats()
			{
				return this.GetFormats(true);
			}

			// Token: 0x0400394E RID: 14670
			internal IDataObject innerData;

			// Token: 0x02000890 RID: 2192
			private class RestrictiveBinder : SerializationBinder
			{
				// Token: 0x060070A6 RID: 28838 RVA: 0x0019BD9C File Offset: 0x00199F9C
				static RestrictiveBinder()
				{
					AssemblyName assemblyName = new AssemblyName(typeof(Bitmap).Assembly.FullName);
					if (assemblyName != null)
					{
						DataObject.OleConverter.RestrictiveBinder.s_allowedAssemblyName = assemblyName.Name;
						DataObject.OleConverter.RestrictiveBinder.s_allowedToken = assemblyName.GetPublicKeyToken();
					}
				}

				// Token: 0x060070A7 RID: 28839 RVA: 0x0019BDF0 File Offset: 0x00199FF0
				public override Type BindToType(string assemblyName, string typeName)
				{
					if (string.CompareOrdinal(typeName, DataObject.OleConverter.RestrictiveBinder.s_allowedTypeName) == 0)
					{
						AssemblyName assemblyName2 = null;
						try
						{
							assemblyName2 = new AssemblyName(assemblyName);
						}
						catch
						{
						}
						if (assemblyName2 != null && string.CompareOrdinal(assemblyName2.Name, DataObject.OleConverter.RestrictiveBinder.s_allowedAssemblyName) == 0)
						{
							byte[] publicKeyToken = assemblyName2.GetPublicKeyToken();
							if (publicKeyToken != null && DataObject.OleConverter.RestrictiveBinder.s_allowedToken != null && publicKeyToken.Length == DataObject.OleConverter.RestrictiveBinder.s_allowedToken.Length)
							{
								bool flag = false;
								for (int i = 0; i < DataObject.OleConverter.RestrictiveBinder.s_allowedToken.Length; i++)
								{
									if (DataObject.OleConverter.RestrictiveBinder.s_allowedToken[i] != publicKeyToken[i])
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									return null;
								}
							}
						}
					}
					throw new DataObject.OleConverter.RestrictedTypeDeserializationException();
				}

				// Token: 0x040043EA RID: 17386
				private static string s_allowedTypeName = typeof(Bitmap).FullName;

				// Token: 0x040043EB RID: 17387
				private static string s_allowedAssemblyName;

				// Token: 0x040043EC RID: 17388
				private static byte[] s_allowedToken;
			}

			// Token: 0x02000891 RID: 2193
			private class RestrictedTypeDeserializationException : Exception
			{
			}
		}

		// Token: 0x020005C5 RID: 1477
		private class DataStore : IDataObject
		{
			// Token: 0x06005A0E RID: 23054 RVA: 0x0017BB04 File Offset: 0x00179D04
			public virtual object GetData(string format, bool autoConvert)
			{
				DataObject.DataStore.DataStoreEntry dataStoreEntry = (DataObject.DataStore.DataStoreEntry)this.data[format];
				object obj = null;
				if (dataStoreEntry != null)
				{
					obj = dataStoreEntry.data;
				}
				object obj2 = obj;
				if (autoConvert && (dataStoreEntry == null || dataStoreEntry.autoConvert) && (obj == null || obj is MemoryStream))
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						for (int i = 0; i < mappedFormats.Length; i++)
						{
							if (!format.Equals(mappedFormats[i]))
							{
								DataObject.DataStore.DataStoreEntry dataStoreEntry2 = (DataObject.DataStore.DataStoreEntry)this.data[mappedFormats[i]];
								if (dataStoreEntry2 != null)
								{
									obj = dataStoreEntry2.data;
								}
								if (obj != null && !(obj is MemoryStream))
								{
									obj2 = null;
									break;
								}
							}
						}
					}
				}
				if (obj2 != null)
				{
					return obj2;
				}
				return obj;
			}

			// Token: 0x06005A0F RID: 23055 RVA: 0x0017BBA9 File Offset: 0x00179DA9
			public virtual object GetData(string format)
			{
				return this.GetData(format, true);
			}

			// Token: 0x06005A10 RID: 23056 RVA: 0x0017BBB3 File Offset: 0x00179DB3
			public virtual object GetData(Type format)
			{
				return this.GetData(format.FullName);
			}

			// Token: 0x06005A11 RID: 23057 RVA: 0x0017BBC4 File Offset: 0x00179DC4
			public virtual void SetData(string format, bool autoConvert, object data)
			{
				if (data is Bitmap && format.Equals(DataFormats.Dib))
				{
					if (!autoConvert)
					{
						throw new NotSupportedException(SR.GetString("DataObjectDibNotSupported"));
					}
					format = DataFormats.Bitmap;
				}
				this.data[format] = new DataObject.DataStore.DataStoreEntry(data, autoConvert);
			}

			// Token: 0x06005A12 RID: 23058 RVA: 0x0017BC15 File Offset: 0x00179E15
			public virtual void SetData(string format, object data)
			{
				this.SetData(format, true, data);
			}

			// Token: 0x06005A13 RID: 23059 RVA: 0x0017BC20 File Offset: 0x00179E20
			public virtual void SetData(Type format, object data)
			{
				this.SetData(format.FullName, data);
			}

			// Token: 0x06005A14 RID: 23060 RVA: 0x0017BC2F File Offset: 0x00179E2F
			public virtual void SetData(object data)
			{
				if (data is ISerializable && !this.data.ContainsKey(DataFormats.Serializable))
				{
					this.SetData(DataFormats.Serializable, data);
				}
				this.SetData(data.GetType(), data);
			}

			// Token: 0x06005A15 RID: 23061 RVA: 0x0017BC64 File Offset: 0x00179E64
			public virtual bool GetDataPresent(Type format)
			{
				return this.GetDataPresent(format.FullName);
			}

			// Token: 0x06005A16 RID: 23062 RVA: 0x0017BC74 File Offset: 0x00179E74
			public virtual bool GetDataPresent(string format, bool autoConvert)
			{
				if (!autoConvert)
				{
					return this.data.ContainsKey(format);
				}
				string[] formats = this.GetFormats(autoConvert);
				for (int i = 0; i < formats.Length; i++)
				{
					if (format.Equals(formats[i]))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06005A17 RID: 23063 RVA: 0x0017BCB5 File Offset: 0x00179EB5
			public virtual bool GetDataPresent(string format)
			{
				return this.GetDataPresent(format, true);
			}

			// Token: 0x06005A18 RID: 23064 RVA: 0x0017BCC0 File Offset: 0x00179EC0
			public virtual string[] GetFormats(bool autoConvert)
			{
				string[] array = new string[this.data.Keys.Count];
				this.data.Keys.CopyTo(array, 0);
				if (autoConvert)
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < array.Length; i++)
					{
						if (((DataObject.DataStore.DataStoreEntry)this.data[array[i]]).autoConvert)
						{
							string[] mappedFormats = DataObject.GetMappedFormats(array[i]);
							for (int j = 0; j < mappedFormats.Length; j++)
							{
								arrayList.Add(mappedFormats[j]);
							}
						}
						else
						{
							arrayList.Add(array[i]);
						}
					}
					string[] array2 = new string[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					array = DataObject.GetDistinctStrings(array2);
				}
				return array;
			}

			// Token: 0x06005A19 RID: 23065 RVA: 0x0017BD77 File Offset: 0x00179F77
			public virtual string[] GetFormats()
			{
				return this.GetFormats(true);
			}

			// Token: 0x0400394F RID: 14671
			private Hashtable data = new Hashtable(BackCompatibleStringComparer.Default);

			// Token: 0x02000892 RID: 2194
			private class DataStoreEntry
			{
				// Token: 0x060070AA RID: 28842 RVA: 0x0019BE90 File Offset: 0x0019A090
				public DataStoreEntry(object data, bool autoConvert)
				{
					this.data = data;
					this.autoConvert = autoConvert;
				}

				// Token: 0x040043ED RID: 17389
				public object data;

				// Token: 0x040043EE RID: 17390
				public bool autoConvert;
			}
		}
	}
}
