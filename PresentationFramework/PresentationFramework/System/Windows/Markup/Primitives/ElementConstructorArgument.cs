using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027F RID: 639
	internal class ElementConstructorArgument : ElementPseudoPropertyBase
	{
		// Token: 0x06002446 RID: 9286 RVA: 0x000B0962 File Offset: 0x000AEB62
		internal ElementConstructorArgument(object value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x000B0974 File Offset: 0x000AEB74
		public override string Name
		{
			get
			{
				return "Argument";
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsConstructorArgument
		{
			get
			{
				return true;
			}
		}
	}
}
