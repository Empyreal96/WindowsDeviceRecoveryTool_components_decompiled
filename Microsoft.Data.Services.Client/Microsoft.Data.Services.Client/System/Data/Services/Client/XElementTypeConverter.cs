using System;
using System.Xml.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x020000A2 RID: 162
	internal sealed class XElementTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x00014F0F File Offset: 0x0001310F
		internal override object Parse(string text)
		{
			return XElement.Parse(text);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00014F17 File Offset: 0x00013117
		internal override string ToString(object instance)
		{
			return instance.ToString();
		}
	}
}
