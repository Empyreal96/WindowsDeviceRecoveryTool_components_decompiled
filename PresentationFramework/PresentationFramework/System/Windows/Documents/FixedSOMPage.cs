using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x0200035E RID: 862
	internal sealed class FixedSOMPage : FixedSOMContainer
	{
		// Token: 0x06002DDF RID: 11743 RVA: 0x000CE54C File Offset: 0x000CC74C
		public void AddFixedBlock(FixedSOMFixedBlock fixedBlock)
		{
			base.Add(fixedBlock);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000CE54C File Offset: 0x000CC74C
		public void AddTable(FixedSOMTable table)
		{
			base.Add(table);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000CE555 File Offset: 0x000CC755
		public override void SetRTFProperties(FixedElement element)
		{
			if (this._cultureInfo != null)
			{
				element.SetValue(FrameworkContentElement.LanguageProperty, XmlLanguage.GetLanguage(this._cultureInfo.IetfLanguageTag));
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000CE57A File Offset: 0x000CC77A
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.Section
				};
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000CE587 File Offset: 0x000CC787
		// (set) Token: 0x06002DE4 RID: 11748 RVA: 0x000CE58F File Offset: 0x000CC78F
		internal List<FixedNode> MarkupOrder
		{
			get
			{
				return this._markupOrder;
			}
			set
			{
				this._markupOrder = value;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (set) Token: 0x06002DE5 RID: 11749 RVA: 0x000CE598 File Offset: 0x000CC798
		internal CultureInfo CultureInfo
		{
			set
			{
				this._cultureInfo = value;
			}
		}

		// Token: 0x04001DD1 RID: 7633
		private List<FixedNode> _markupOrder;

		// Token: 0x04001DD2 RID: 7634
		private CultureInfo _cultureInfo;
	}
}
