using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020004C8 RID: 1224
	internal class DeferredTextReference : DeferredReference
	{
		// Token: 0x06004A53 RID: 19027 RVA: 0x0014FB92 File Offset: 0x0014DD92
		internal DeferredTextReference(ITextContainer textContainer)
		{
			this._textContainer = textContainer;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0014FBA4 File Offset: 0x0014DDA4
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			string textInternal = TextRangeBase.GetTextInternal(this._textContainer.Start, this._textContainer.End);
			TextBox textBox = this._textContainer.Parent as TextBox;
			if (textBox != null)
			{
				textBox.OnDeferredTextReferenceResolved(this, textInternal);
			}
			return textInternal;
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x000B087A File Offset: 0x000AEA7A
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04002A54 RID: 10836
		private readonly ITextContainer _textContainer;
	}
}
