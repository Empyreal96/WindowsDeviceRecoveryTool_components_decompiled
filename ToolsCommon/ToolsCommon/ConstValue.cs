using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000023 RID: 35
	public class ConstValue<T>
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00007C06 File Offset: 0x00005E06
		public ConstValue(T value)
		{
			this.Value = value;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007C1E File Offset: 0x00005E1E
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00007C15 File Offset: 0x00005E15
		public T Value { get; private set; }
	}
}
