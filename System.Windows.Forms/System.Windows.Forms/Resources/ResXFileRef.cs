using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Resources
{
	/// <summary>Represents a link to an external resource.</summary>
	// Token: 0x020000ED RID: 237
	[TypeConverter(typeof(ResXFileRef.Converter))]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[Serializable]
	public class ResXFileRef
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Resources.ResXFileRef" /> class that references the specified file.</summary>
		/// <param name="fileName">The file to reference. </param>
		/// <param name="typeName">The type of the resource that is referenced. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="fileName" /> or <paramref name="typeName " />is <see langword="null" />.</exception>
		// Token: 0x0600035C RID: 860 RVA: 0x0000A2CD File Offset: 0x000084CD
		public ResXFileRef(string fileName, string typeName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this.fileName = fileName;
			this.typeName = typeName;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000A2FF File Offset: 0x000084FF
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.textFileEncoding = null;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000701A File Offset: 0x0000521A
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXFileRef" /> class that references the specified file. </summary>
		/// <param name="fileName">The file to reference. </param>
		/// <param name="typeName">The type name of the resource that is referenced. </param>
		/// <param name="textFileEncoding">The encoding used in the referenced file.</param>
		// Token: 0x0600035F RID: 863 RVA: 0x0000A308 File Offset: 0x00008508
		public ResXFileRef(string fileName, string typeName, Encoding textFileEncoding) : this(fileName, typeName)
		{
			this.textFileEncoding = textFileEncoding;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000A319 File Offset: 0x00008519
		internal ResXFileRef Clone()
		{
			return new ResXFileRef(this.fileName, this.typeName, this.textFileEncoding);
		}

		/// <summary>Gets the file name specified in the current <see cref="Overload:System.Resources.ResXFileRef.#ctor" /> constructor.</summary>
		/// <returns>The name of the referenced file.</returns>
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000A332 File Offset: 0x00008532
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		/// <summary>Gets the type name specified in the current <see cref="Overload:System.Resources.ResXFileRef.#ctor" /> constructor. </summary>
		/// <returns>The type name of the resource that is referenced. </returns>
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000A33A File Offset: 0x0000853A
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		/// <summary>Gets the encoding specified in the current <see cref="Overload:System.Resources.ResXFileRef.#ctor" /> constructor.</summary>
		/// <returns>The encoding used in the referenced file.</returns>
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000A342 File Offset: 0x00008542
		public Encoding TextFileEncoding
		{
			get
			{
				return this.textFileEncoding;
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000A34C File Offset: 0x0000854C
		private static string PathDifference(string path1, string path2, bool compareCase)
		{
			int num = -1;
			int i = 0;
			while (i < path1.Length && i < path2.Length && (path1[i] == path2[i] || (!compareCase && char.ToLower(path1[i], CultureInfo.InvariantCulture) == char.ToLower(path2[i], CultureInfo.InvariantCulture))))
			{
				if (path1[i] == Path.DirectorySeparatorChar)
				{
					num = i;
				}
				i++;
			}
			if (i == 0)
			{
				return path2;
			}
			if (i == path1.Length && i == path2.Length)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (i < path1.Length)
			{
				if (path1[i] == Path.DirectorySeparatorChar)
				{
					stringBuilder.Append(".." + Path.DirectorySeparatorChar.ToString());
				}
				i++;
			}
			return stringBuilder.ToString() + path2.Substring(num + 1);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000A42E File Offset: 0x0000862E
		internal void MakeFilePathRelative(string basePath)
		{
			if (basePath == null || basePath.Length == 0)
			{
				return;
			}
			this.fileName = ResXFileRef.PathDifference(basePath, this.fileName, false);
		}

		/// <summary>Gets the text representation of the current <see cref="T:System.Resources.ResXFileRef" /> object.</summary>
		/// <returns>A string that consists of the concatenated text representations of the parameters specified in the current <see cref="Overload:System.Resources.ResXFileRef.#ctor" /> constructor.</returns>
		// Token: 0x06000366 RID: 870 RVA: 0x0000A450 File Offset: 0x00008650
		public override string ToString()
		{
			string text = "";
			if (this.fileName.IndexOf(";") != -1 || this.fileName.IndexOf("\"") != -1)
			{
				text = text + "\"" + this.fileName + "\";";
			}
			else
			{
				text = text + this.fileName + ";";
			}
			text += this.typeName;
			if (this.textFileEncoding != null)
			{
				text = text + ";" + this.textFileEncoding.WebName;
			}
			return text;
		}

		// Token: 0x040003C1 RID: 961
		private string fileName;

		// Token: 0x040003C2 RID: 962
		private string typeName;

		// Token: 0x040003C3 RID: 963
		[OptionalField(VersionAdded = 2)]
		private Encoding textFileEncoding;

		/// <summary>Provides a type converter to convert data for a <see cref="T:System.Resources.ResXFileRef" /> to and from a string.</summary>
		// Token: 0x02000539 RID: 1337
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class Converter : TypeConverter
		{
			/// <summary>Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
			/// <returns>
			///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
			// Token: 0x060054B0 RID: 21680 RVA: 0x001639F2 File Offset: 0x00161BF2
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}

			/// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
			/// <returns>
			///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
			// Token: 0x060054B1 RID: 21681 RVA: 0x001639F2 File Offset: 0x00161BF2
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}

			/// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed. </param>
			/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
			/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to. </param>
			/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
			// Token: 0x060054B2 RID: 21682 RVA: 0x00163A0C File Offset: 0x00161C0C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				object result = null;
				if (destinationType == typeof(string))
				{
					result = ((ResXFileRef)value).ToString();
				}
				return result;
			}

			// Token: 0x060054B3 RID: 21683 RVA: 0x00163A3C File Offset: 0x00161C3C
			internal static string[] ParseResxFileRefString(string stringValue)
			{
				string[] result = null;
				if (stringValue != null)
				{
					stringValue = stringValue.Trim();
					string text;
					string text2;
					if (stringValue.StartsWith("\""))
					{
						int num = stringValue.LastIndexOf("\"");
						if (num - 1 < 0)
						{
							throw new ArgumentException("value");
						}
						text = stringValue.Substring(1, num - 1);
						if (num + 2 > stringValue.Length)
						{
							throw new ArgumentException("value");
						}
						text2 = stringValue.Substring(num + 2);
					}
					else
					{
						int num2 = stringValue.IndexOf(";");
						if (num2 == -1)
						{
							throw new ArgumentException("value");
						}
						text = stringValue.Substring(0, num2);
						if (num2 + 1 > stringValue.Length)
						{
							throw new ArgumentException("value");
						}
						text2 = stringValue.Substring(num2 + 1);
					}
					string[] array = text2.Split(new char[]
					{
						';'
					});
					if (array.Length > 1)
					{
						result = new string[]
						{
							text,
							array[0],
							array[1]
						};
					}
					else if (array.Length != 0)
					{
						result = new string[]
						{
							text,
							array[0]
						};
					}
					else
					{
						result = new string[]
						{
							text
						};
					}
				}
				return result;
			}

			/// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
			/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
			/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
			// Token: 0x060054B4 RID: 21684 RVA: 0x00163B54 File Offset: 0x00161D54
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				object result = null;
				string text = value as string;
				if (text != null)
				{
					string[] array = ResXFileRef.Converter.ParseResxFileRefString(text);
					string text2 = array[0];
					Type type = Type.GetType(array[1], true);
					if (type.Equals(typeof(string)))
					{
						Encoding encoding = Encoding.Default;
						if (array.Length > 2)
						{
							encoding = Encoding.GetEncoding(array[2]);
						}
						using (StreamReader streamReader = new StreamReader(text2, encoding))
						{
							return streamReader.ReadToEnd();
						}
					}
					byte[] array2 = null;
					using (FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						array2 = new byte[fileStream.Length];
						fileStream.Read(array2, 0, (int)fileStream.Length);
					}
					if (type.Equals(typeof(byte[])))
					{
						result = array2;
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream(array2);
						if (type.Equals(typeof(MemoryStream)))
						{
							return memoryStream;
						}
						if (type.Equals(typeof(Bitmap)) && text2.EndsWith(".ico"))
						{
							Icon icon = new Icon(memoryStream);
							result = icon.ToBitmap();
						}
						else
						{
							result = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[]
							{
								memoryStream
							}, null);
						}
					}
				}
				return result;
			}
		}
	}
}
