using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020004C6 RID: 1222
	internal class DeferredRunTextReference : DeferredReference
	{
		// Token: 0x06004A4D RID: 19021 RVA: 0x0014FB39 File Offset: 0x0014DD39
		internal DeferredRunTextReference(Run run)
		{
			this._run = run;
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x0014FB48 File Offset: 0x0014DD48
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return TextRangeBase.GetTextInternal(this._run.ContentStart, this._run.ContentEnd);
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x000B087A File Offset: 0x000AEA7A
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04002A52 RID: 10834
		private readonly Run _run;
	}
}
