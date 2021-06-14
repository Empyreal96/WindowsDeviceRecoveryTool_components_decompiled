using System;

namespace System.Windows
{
	// Token: 0x020000F5 RID: 245
	internal class SharedDp
	{
		// Token: 0x060008AD RID: 2221 RVA: 0x0001C2BD File Offset: 0x0001A4BD
		internal SharedDp(DependencyProperty dp, object value, string elementName)
		{
			this.Dp = dp;
			this.Value = value;
			this.ElementName = elementName;
		}

		// Token: 0x040007B2 RID: 1970
		internal DependencyProperty Dp;

		// Token: 0x040007B3 RID: 1971
		internal object Value;

		// Token: 0x040007B4 RID: 1972
		internal string ElementName;
	}
}
