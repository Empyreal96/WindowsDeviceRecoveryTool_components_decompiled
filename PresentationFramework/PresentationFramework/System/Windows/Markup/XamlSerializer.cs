using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000266 RID: 614
	internal class XamlSerializer
	{
		// Token: 0x06002335 RID: 9013 RVA: 0x000ACE52 File Offset: 0x000AB052
		internal virtual void ConvertXamlToBaml(XamlReaderHelper tokenReader, ParserContext context, XamlNode xamlNode, BamlRecordWriter bamlWriter)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000ACE52 File Offset: 0x000AB052
		internal virtual void ConvertXamlToObject(XamlReaderHelper tokenReader, ReadWriteStreamManager streamManager, ParserContext context, XamlNode xamlNode, BamlRecordReader reader)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000ACE52 File Offset: 0x000AB052
		internal virtual void ConvertBamlToObject(BamlRecordReader reader, BamlRecord bamlRecord, ParserContext context)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000ACE63 File Offset: 0x000AB063
		public virtual bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			throw new InvalidOperationException(SR.Get("InvalidCustomSerialize"));
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000ACE63 File Offset: 0x000AB063
		public virtual object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			throw new InvalidOperationException(SR.Get("InvalidCustomSerialize"));
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object GetDictionaryKey(BamlRecord bamlRecord, ParserContext parserContext)
		{
			return null;
		}

		// Token: 0x04001A76 RID: 6774
		internal const string DefNamespacePrefix = "x";

		// Token: 0x04001A77 RID: 6775
		internal const string DefNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x04001A78 RID: 6776
		internal const string ArrayTag = "Array";

		// Token: 0x04001A79 RID: 6777
		internal const string ArrayTagTypeAttribute = "Type";
	}
}
