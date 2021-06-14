using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000090 RID: 144
	internal sealed class ByteTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x00014C6C File Offset: 0x00012E6C
		internal override object Parse(string text)
		{
			return XmlConvert.ToByte(text);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00014C79 File Offset: 0x00012E79
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((byte)instance);
		}
	}
}
