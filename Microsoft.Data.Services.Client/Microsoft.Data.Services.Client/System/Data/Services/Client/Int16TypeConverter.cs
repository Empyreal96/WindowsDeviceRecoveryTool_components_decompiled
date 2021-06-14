using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000097 RID: 151
	internal sealed class Int16TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x00014DB1 File Offset: 0x00012FB1
		internal override object Parse(string text)
		{
			return XmlConvert.ToInt16(text);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00014DBE File Offset: 0x00012FBE
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((short)instance);
		}
	}
}
