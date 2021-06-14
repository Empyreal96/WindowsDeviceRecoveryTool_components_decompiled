using System;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200001D RID: 29
	public class PropsFile
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000749C File Offset: 0x0000569C
		public override string ToString()
		{
			return this.Include;
		}

		// Token: 0x040000A8 RID: 168
		[XmlAttribute("Include")]
		public string Include;

		// Token: 0x040000A9 RID: 169
		public string InstallPath;

		// Token: 0x040000AA RID: 170
		public string MC_ARM_FRE;

		// Token: 0x040000AB RID: 171
		public string MC_ARM_CHK;

		// Token: 0x040000AC RID: 172
		public string MC_ARM64_FRE;

		// Token: 0x040000AD RID: 173
		public string MC_ARM64_CHK;

		// Token: 0x040000AE RID: 174
		public string MC_X86_FRE;

		// Token: 0x040000AF RID: 175
		public string MC_X86_CHK;

		// Token: 0x040000B0 RID: 176
		public string MC_AMD64_FRE;

		// Token: 0x040000B1 RID: 177
		public string MC_AMD64_CHK;

		// Token: 0x040000B2 RID: 178
		public string Feature;

		// Token: 0x040000B3 RID: 179
		public string Owner;

		// Token: 0x040000B4 RID: 180
		public string BusinessReason;
	}
}
