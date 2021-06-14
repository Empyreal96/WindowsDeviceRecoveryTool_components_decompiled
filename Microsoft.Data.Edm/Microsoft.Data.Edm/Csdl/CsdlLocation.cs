using System;
using System.Globalization;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x02000003 RID: 3
	public class CsdlLocation : EdmLocation
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020D8 File Offset: 0x000002D8
		internal CsdlLocation(int number, int position)
		{
			this.LineNumber = number;
			this.LinePosition = position;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020EE File Offset: 0x000002EE
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020F6 File Offset: 0x000002F6
		public int LineNumber { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020FF File Offset: 0x000002FF
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002107 File Offset: 0x00000307
		public int LinePosition { get; private set; }

		// Token: 0x06000008 RID: 8 RVA: 0x00002110 File Offset: 0x00000310
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				Convert.ToString(this.LineNumber, CultureInfo.InvariantCulture),
				", ",
				Convert.ToString(this.LinePosition, CultureInfo.InvariantCulture),
				")"
			});
		}
	}
}
