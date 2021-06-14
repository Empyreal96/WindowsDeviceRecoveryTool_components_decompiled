using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000099 RID: 153
	internal sealed class Int64TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x00014DF5 File Offset: 0x00012FF5
		internal override object Parse(string text)
		{
			return XmlConvert.ToInt64(text);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00014E02 File Offset: 0x00013002
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((long)instance);
		}
	}
}
