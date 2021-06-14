using System;

namespace System.Windows.Documents
{
	// Token: 0x02000329 RID: 809
	internal class NonLogicalAdornerDecorator : AdornerDecorator
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x000C2CB2 File Offset: 0x000C0EB2
		// (set) Token: 0x06002A9F RID: 10911 RVA: 0x000C2CBC File Offset: 0x000C0EBC
		public override UIElement Child
		{
			get
			{
				return base.IntChild;
			}
			set
			{
				if (base.IntChild != value)
				{
					base.RemoveVisualChild(base.IntChild);
					base.RemoveVisualChild(base.AdornerLayer);
					base.IntChild = value;
					if (value != null)
					{
						base.AddVisualChild(value);
						base.AddVisualChild(base.AdornerLayer);
					}
					base.InvalidateMeasure();
				}
			}
		}
	}
}
