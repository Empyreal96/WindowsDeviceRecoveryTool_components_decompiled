using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.PresentationFramework.Markup
{
	// Token: 0x02000803 RID: 2051
	internal class XamlReaderProxy
	{
		// Token: 0x06007DEC RID: 32236 RVA: 0x00234DF4 File Offset: 0x00232FF4
		static XamlReaderProxy()
		{
			MethodInfo method = XamlReaderProxy._xamlReaderType.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(Stream),
				typeof(ParserContext),
				typeof(bool)
			}, null);
			if (method != null)
			{
				XamlReaderProxy._xamlLoad3 = (XamlReaderProxy.XamlLoadDelegate3)method.CreateDelegate(typeof(XamlReaderProxy.XamlLoadDelegate3));
			}
			method = XamlReaderProxy._xamlReaderType.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(XmlReader),
				typeof(bool)
			}, null);
			if (method != null)
			{
				XamlReaderProxy._xamlLoad2 = (XamlReaderProxy.XamlLoadDelegate2)method.CreateDelegate(typeof(XamlReaderProxy.XamlLoadDelegate2));
			}
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x00234ECB File Offset: 0x002330CB
		public static object Load(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (XamlReaderProxy._xamlLoad3 != null)
			{
				return XamlReaderProxy._xamlLoad3(stream, parserContext, XamlReaderProxy.DisableLegacyDangerousXamlDeserializationMode && useRestrictiveXamlReader);
			}
			return XamlReader.Load(stream, parserContext);
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x00234EEF File Offset: 0x002330EF
		public static object Load(XmlReader reader, bool useRestrictiveXamlReader)
		{
			if (XamlReaderProxy._xamlLoad2 != null)
			{
				return XamlReaderProxy._xamlLoad2(reader, XamlReaderProxy.DisableLegacyDangerousXamlDeserializationMode && useRestrictiveXamlReader);
			}
			return XamlReader.Load(reader);
		}

		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x06007DEF RID: 32239 RVA: 0x00234F11 File Offset: 0x00233111
		private static bool DisableLegacyDangerousXamlDeserializationMode
		{
			get
			{
				return FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode;
			}
		}

		// Token: 0x04003B72 RID: 15218
		private static XamlReaderProxy.XamlLoadDelegate3 _xamlLoad3;

		// Token: 0x04003B73 RID: 15219
		private static XamlReaderProxy.XamlLoadDelegate2 _xamlLoad2;

		// Token: 0x04003B74 RID: 15220
		private static readonly Type _xamlReaderType = typeof(XamlReader);

		// Token: 0x04003B75 RID: 15221
		private const string XamlLoadMethodName = "Load";

		// Token: 0x02000B8D RID: 2957
		// (Invoke) Token: 0x06008E7F RID: 36479
		private delegate object XamlLoadDelegate3(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader);

		// Token: 0x02000B8E RID: 2958
		// (Invoke) Token: 0x06008E83 RID: 36483
		private delegate object XamlLoadDelegate2(XmlReader reader, bool useRestrictiveXamlReader);
	}
}
