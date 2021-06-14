using System;
using System.Windows;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200061A RID: 1562
	internal sealed class InlineObject : EmbeddedObject
	{
		// Token: 0x060067C7 RID: 26567 RVA: 0x001D17F7 File Offset: 0x001CF9F7
		internal InlineObject(int dcp, UIElementIsland uiElementIsland, TextParagraph para) : base(dcp)
		{
			this._para = para;
			this._uiElementIsland = uiElementIsland;
			this._uiElementIsland.DesiredSizeChanged += this._para.OnUIElementDesiredSizeChanged;
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x001D182A File Offset: 0x001CFA2A
		internal override void Dispose()
		{
			if (this._uiElementIsland != null)
			{
				this._uiElementIsland.DesiredSizeChanged -= this._para.OnUIElementDesiredSizeChanged;
			}
			base.Dispose();
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x001D1858 File Offset: 0x001CFA58
		internal override void Update(EmbeddedObject newObject)
		{
			InlineObject inlineObject = newObject as InlineObject;
			ErrorHandler.Assert(inlineObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(inlineObject._uiElementIsland == this._uiElementIsland, ErrorHandler.EmbeddedObjectOwnerMismatch);
			inlineObject._uiElementIsland = null;
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x060067CA RID: 26570 RVA: 0x001D1899 File Offset: 0x001CFA99
		internal override DependencyObject Element
		{
			get
			{
				return this._uiElementIsland.Root;
			}
		}

		// Token: 0x04003389 RID: 13193
		private UIElementIsland _uiElementIsland;

		// Token: 0x0400338A RID: 13194
		private TextParagraph _para;
	}
}
