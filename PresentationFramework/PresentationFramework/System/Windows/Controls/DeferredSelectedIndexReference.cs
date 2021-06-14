using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x020004C7 RID: 1223
	internal class DeferredSelectedIndexReference : DeferredReference
	{
		// Token: 0x06004A50 RID: 19024 RVA: 0x0014FB65 File Offset: 0x0014DD65
		internal DeferredSelectedIndexReference(Selector selector)
		{
			this._selector = selector;
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0014FB74 File Offset: 0x0014DD74
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return this._selector.InternalSelectedIndex;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0014FB86 File Offset: 0x0014DD86
		internal override Type GetValueType()
		{
			return typeof(int);
		}

		// Token: 0x04002A53 RID: 10835
		private readonly Selector _selector;
	}
}
