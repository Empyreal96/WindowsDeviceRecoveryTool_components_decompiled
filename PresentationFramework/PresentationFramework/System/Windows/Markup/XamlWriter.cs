using System;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Markup.Primitives;
using System.Xml;
using MS.Internal.PresentationFramework;

namespace System.Windows.Markup
{
	/// <summary>Provides a single static <see cref="Overload:System.Windows.Markup.XamlWriter.Save" /> method (multiple overloads) that can be used for limited XAML serialization of provided run-time objects into XAML markup.</summary>
	// Token: 0x02000270 RID: 624
	public static class XamlWriter
	{
		/// <summary>Returns a XAML string that serializes the specified object and its properties.</summary>
		/// <param name="obj">The element to be serialized. Typically, this is the root element of a page or application.</param>
		/// <returns>A XAML string that can be written to a stream or file. The logical tree of all elements that fall under the provided <paramref name="obj" /> element will be serialized.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="obj" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The application is not running in full trust.</exception>
		// Token: 0x060023BC RID: 9148 RVA: 0x000AE8DC File Offset: 0x000ACADC
		[SecuritySafeCritical]
		public static string Save(object obj)
		{
			SecurityHelper.DemandUnmanagedCode();
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			StringBuilder stringBuilder = new StringBuilder();
			TextWriter textWriter = new StringWriter(stringBuilder, TypeConverterHelper.InvariantEnglishUS);
			try
			{
				XamlWriter.Save(obj, textWriter);
			}
			finally
			{
				textWriter.Close();
			}
			return stringBuilder.ToString();
		}

		/// <summary>Saves XAML information as the source for a provided <see cref="T:System.IO.TextWriter" /> object. The output of the <see cref="T:System.IO.TextWriter" /> can then be used to serialize the provided object and its properties.</summary>
		/// <param name="obj">The element to be serialized. Typically, this is the root element of a page or application.</param>
		/// <param name="writer">A <see cref="T:System.IO.TextWriter" /> instance as the destination where the serialized XAML information is written.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="obj" /> or <paramref name="writer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The application is not running in full trust.</exception>
		// Token: 0x060023BD RID: 9149 RVA: 0x000AE934 File Offset: 0x000ACB34
		[SecuritySafeCritical]
		public static void Save(object obj, TextWriter writer)
		{
			SecurityHelper.DemandUnmanagedCode();
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			XmlTextWriter writer2 = new XmlTextWriter(writer);
			MarkupWriter.SaveAsXml(writer2, obj);
		}

		/// <summary>Saves XAML information into a specified stream to serialize the specified object and its properties.</summary>
		/// <param name="obj">The element to be serialized. Typically, this is the root element of a page or application.</param>
		/// <param name="stream">Destination stream for the serialized XAML information.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="obj" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The application is not running in full trust.</exception>
		// Token: 0x060023BE RID: 9150 RVA: 0x000AE970 File Offset: 0x000ACB70
		[SecuritySafeCritical]
		public static void Save(object obj, Stream stream)
		{
			SecurityHelper.DemandUnmanagedCode();
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			XmlTextWriter writer = new XmlTextWriter(stream, null);
			MarkupWriter.SaveAsXml(writer, obj);
		}

		/// <summary>Saves XAML information as the source for a provided <see cref="T:System.Xml.XmlWriter" /> object. The output of the <see cref="T:System.Xml.XmlWriter" /> can then be used to serialize the provided object and its properties.</summary>
		/// <param name="obj">The element to be serialized. Typically, this is the root element of a page or application.</param>
		/// <param name="xmlWriter">Writer to use to write the serialized XAML information.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="obj" /> or <paramref name="xmlWriter" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The application is not running in full trust.</exception>
		// Token: 0x060023BF RID: 9151 RVA: 0x000AE9B0 File Offset: 0x000ACBB0
		[SecuritySafeCritical]
		public static void Save(object obj, XmlWriter xmlWriter)
		{
			SecurityHelper.DemandUnmanagedCode();
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (xmlWriter == null)
			{
				throw new ArgumentNullException("xmlWriter");
			}
			try
			{
				MarkupWriter.SaveAsXml(xmlWriter, obj);
			}
			finally
			{
				xmlWriter.Flush();
			}
		}

		/// <summary>Saves XAML information into a custom serializer. The output of the serializer can then be used to serialize the provided object and its properties.</summary>
		/// <param name="obj">The element to be serialized. Typically, this is the root element of a page or application.</param>
		/// <param name="manager">A custom serialization implementation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="obj" /> or <paramref name="manager" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The application is not running in full trust.</exception>
		// Token: 0x060023C0 RID: 9152 RVA: 0x000AEA00 File Offset: 0x000ACC00
		[SecuritySafeCritical]
		public static void Save(object obj, XamlDesignerSerializationManager manager)
		{
			SecurityHelper.DemandUnmanagedCode();
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			MarkupWriter.SaveAsXml(manager.XmlWriter, obj, manager);
		}
	}
}
