using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200021F RID: 543
	public sealed class AtomTextConstruct : ODataAnnotatable
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0003F24E File Offset: 0x0003D44E
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0003F256 File Offset: 0x0003D456
		public AtomTextConstructKind Kind { get; set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0003F25F File Offset: 0x0003D45F
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0003F267 File Offset: 0x0003D467
		public string Text { get; set; }

		// Token: 0x060010E4 RID: 4324 RVA: 0x0003F270 File Offset: 0x0003D470
		public static AtomTextConstruct ToTextConstruct(string text)
		{
			return new AtomTextConstruct
			{
				Text = text
			};
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0003F28B File Offset: 0x0003D48B
		public static implicit operator AtomTextConstruct(string text)
		{
			return AtomTextConstruct.ToTextConstruct(text);
		}
	}
}
