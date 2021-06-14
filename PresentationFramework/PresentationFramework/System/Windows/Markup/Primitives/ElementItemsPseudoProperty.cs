using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000280 RID: 640
	internal class ElementItemsPseudoProperty : ElementPseudoPropertyBase
	{
		// Token: 0x06002449 RID: 9289 RVA: 0x000B097B File Offset: 0x000AEB7B
		internal ElementItemsPseudoProperty(IEnumerable value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
			this._value = value;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000B098D File Offset: 0x000AEB8D
		public override string Name
		{
			get
			{
				return "Items";
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x000B0994 File Offset: 0x000AEB94
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (object instance in this._value)
				{
					yield return new ElementMarkupObject(instance, base.Manager);
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x04001B2E RID: 6958
		private IEnumerable _value;
	}
}
