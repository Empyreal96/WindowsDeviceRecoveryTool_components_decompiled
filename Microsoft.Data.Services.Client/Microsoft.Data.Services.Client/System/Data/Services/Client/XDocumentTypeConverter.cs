using System;
using System.Xml.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x020000A1 RID: 161
	internal sealed class XDocumentTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x00014EE8 File Offset: 0x000130E8
		internal override object Parse(string text)
		{
			if (text.Length <= 0)
			{
				return new XDocument();
			}
			return XDocument.Parse(text);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00014EFF File Offset: 0x000130FF
		internal override string ToString(object instance)
		{
			return instance.ToString();
		}
	}
}
