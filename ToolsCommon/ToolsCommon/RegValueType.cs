using System;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005B RID: 91
	public enum RegValueType
	{
		// Token: 0x04000160 RID: 352
		[XmlEnum(Name = "REG_SZ")]
		String,
		// Token: 0x04000161 RID: 353
		[XmlEnum(Name = "REG_EXPAND_SZ")]
		ExpandString,
		// Token: 0x04000162 RID: 354
		[XmlEnum(Name = "REG_BINARY")]
		Binary,
		// Token: 0x04000163 RID: 355
		[XmlEnum(Name = "REG_DWORD")]
		DWord,
		// Token: 0x04000164 RID: 356
		[XmlEnum(Name = "REG_MULTI_SZ")]
		MultiString,
		// Token: 0x04000165 RID: 357
		[XmlEnum(Name = "REG_QWORD")]
		QWord,
		// Token: 0x04000166 RID: 358
		[XmlEnum(Name = "REG_HEX")]
		Hex
	}
}
