using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027E RID: 638
	internal class ElementKey : ElementPseudoPropertyBase
	{
		// Token: 0x06002443 RID: 9283 RVA: 0x000B0962 File Offset: 0x000AEB62
		internal ElementKey(object value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000B096D File Offset: 0x000AEB6D
		public override string Name
		{
			get
			{
				return "Key";
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsKey
		{
			get
			{
				return true;
			}
		}
	}
}
