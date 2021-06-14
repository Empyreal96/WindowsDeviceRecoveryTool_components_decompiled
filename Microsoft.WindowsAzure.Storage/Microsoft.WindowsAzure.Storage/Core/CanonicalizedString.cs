using System;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000085 RID: 133
	internal class CanonicalizedString
	{
		// Token: 0x06000F3A RID: 3898 RVA: 0x0003A098 File Offset: 0x00038298
		public CanonicalizedString(string initialElement) : this(initialElement, 300)
		{
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0003A0A6 File Offset: 0x000382A6
		public CanonicalizedString(string initialElement, int capacity)
		{
			this.canonicalizedString = new StringBuilder(initialElement, capacity);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0003A0BB File Offset: 0x000382BB
		public void AppendCanonicalizedElement(string element)
		{
			this.canonicalizedString.Append('\n');
			this.canonicalizedString.Append(element);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0003A0D8 File Offset: 0x000382D8
		public override string ToString()
		{
			return this.canonicalizedString.ToString();
		}

		// Token: 0x04000280 RID: 640
		private const int DefaultCapacity = 300;

		// Token: 0x04000281 RID: 641
		private const char ElementDelimiter = '\n';

		// Token: 0x04000282 RID: 642
		private readonly StringBuilder canonicalizedString;
	}
}
