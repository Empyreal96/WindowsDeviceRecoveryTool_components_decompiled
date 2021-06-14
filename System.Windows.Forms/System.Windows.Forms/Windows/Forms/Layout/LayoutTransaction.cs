using System;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004DD RID: 1245
	internal sealed class LayoutTransaction : IDisposable
	{
		// Token: 0x060052BC RID: 21180 RVA: 0x0015A187 File Offset: 0x00158387
		public LayoutTransaction(Control controlToLayout, IArrangedElement controlCausingLayout, string property) : this(controlToLayout, controlCausingLayout, property, true)
		{
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0015A194 File Offset: 0x00158394
		public LayoutTransaction(Control controlToLayout, IArrangedElement controlCausingLayout, string property, bool resumeLayout)
		{
			CommonProperties.xClearPreferredSizeCache(controlCausingLayout);
			this._controlToLayout = controlToLayout;
			this._resumeLayout = resumeLayout;
			if (this._controlToLayout != null)
			{
				this._controlToLayout.SuspendLayout();
				CommonProperties.xClearPreferredSizeCache(this._controlToLayout);
				if (resumeLayout)
				{
					this._controlToLayout.PerformLayout(new LayoutEventArgs(controlCausingLayout, property));
				}
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0015A1F0 File Offset: 0x001583F0
		public void Dispose()
		{
			if (this._controlToLayout != null)
			{
				this._controlToLayout.ResumeLayout(this._resumeLayout);
			}
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0015A20C File Offset: 0x0015840C
		public static IDisposable CreateTransactionIf(bool condition, Control controlToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (condition)
			{
				return new LayoutTransaction(controlToLayout, elementCausingLayout, property);
			}
			CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
			return default(NullLayoutTransaction);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0015A239 File Offset: 0x00158439
		public static void DoLayout(IArrangedElement elementToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (elementCausingLayout != null)
			{
				CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
				if (elementToLayout != null)
				{
					CommonProperties.xClearPreferredSizeCache(elementToLayout);
					elementToLayout.PerformLayout(elementCausingLayout, property);
				}
			}
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0015A255 File Offset: 0x00158455
		public static void DoLayoutIf(bool condition, IArrangedElement elementToLayout, IArrangedElement elementCausingLayout, string property)
		{
			if (!condition)
			{
				if (elementCausingLayout != null)
				{
					CommonProperties.xClearPreferredSizeCache(elementCausingLayout);
					return;
				}
			}
			else
			{
				LayoutTransaction.DoLayout(elementToLayout, elementCausingLayout, property);
			}
		}

		// Token: 0x040034AA RID: 13482
		private Control _controlToLayout;

		// Token: 0x040034AB RID: 13483
		private bool _resumeLayout;
	}
}
