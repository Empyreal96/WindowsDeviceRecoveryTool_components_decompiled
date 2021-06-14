using System;

namespace System.Data.Services.Common
{
	// Token: 0x020000FB RID: 251
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class NamedStreamAttribute : Attribute
	{
		// Token: 0x06000834 RID: 2100 RVA: 0x00022F71 File Offset: 0x00021171
		public NamedStreamAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00022F80 File Offset: 0x00021180
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x00022F88 File Offset: 0x00021188
		public string Name { get; private set; }
	}
}
