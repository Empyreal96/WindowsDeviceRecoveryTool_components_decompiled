using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Spatial
{
	// Token: 0x02000013 RID: 19
	public static class FormatterExtensions
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00003644 File Offset: 0x00001844
		public static string Write(this SpatialFormatter<TextReader, TextWriter> formatter, ISpatial spatial)
		{
			Util.CheckArgumentNull(formatter, "formatter");
			StringBuilder stringBuilder = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				formatter.Write(spatial, textWriter);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003698 File Offset: 0x00001898
		public static string Write(this SpatialFormatter<XmlReader, XmlWriter> formatter, ISpatial spatial)
		{
			Util.CheckArgumentNull(formatter, "formatter");
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
			{
				formatter.Write(spatial, xmlWriter);
			}
			return stringBuilder.ToString();
		}
	}
}
