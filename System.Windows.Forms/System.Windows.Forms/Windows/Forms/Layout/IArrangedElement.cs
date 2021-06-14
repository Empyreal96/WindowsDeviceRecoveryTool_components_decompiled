using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004DA RID: 1242
	internal interface IArrangedElement : IComponent, IDisposable
	{
		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x0600527E RID: 21118
		Rectangle Bounds { get; }

		// Token: 0x0600527F RID: 21119
		void SetBounds(Rectangle bounds, BoundsSpecified specified);

		// Token: 0x06005280 RID: 21120
		Size GetPreferredSize(Size proposedSize);

		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x06005281 RID: 21121
		Rectangle DisplayRectangle { get; }

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x06005282 RID: 21122
		bool ParticipatesInLayout { get; }

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x06005283 RID: 21123
		PropertyStore Properties { get; }

		// Token: 0x06005284 RID: 21124
		void PerformLayout(IArrangedElement affectedElement, string propertyName);

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06005285 RID: 21125
		IArrangedElement Container { get; }

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06005286 RID: 21126
		ArrangedElementCollection Children { get; }
	}
}
