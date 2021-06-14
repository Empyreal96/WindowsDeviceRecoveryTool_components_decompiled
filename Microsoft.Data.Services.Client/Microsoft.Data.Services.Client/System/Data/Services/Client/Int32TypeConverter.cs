using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000098 RID: 152
	internal sealed class Int32TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x00014DD3 File Offset: 0x00012FD3
		internal override object Parse(string text)
		{
			return XmlConvert.ToInt32(text);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014DE0 File Offset: 0x00012FE0
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((int)instance);
		}
	}
}
