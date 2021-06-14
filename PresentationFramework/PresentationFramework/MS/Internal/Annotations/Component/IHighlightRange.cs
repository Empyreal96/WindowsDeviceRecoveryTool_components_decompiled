using System;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E1 RID: 2017
	internal interface IHighlightRange
	{
		// Token: 0x06007CA6 RID: 31910
		void AddChild(Shape child);

		// Token: 0x06007CA7 RID: 31911
		void RemoveChild(Shape child);

		// Token: 0x17001CF2 RID: 7410
		// (get) Token: 0x06007CA8 RID: 31912
		Color Background { get; }

		// Token: 0x17001CF3 RID: 7411
		// (get) Token: 0x06007CA9 RID: 31913
		Color SelectedBackground { get; }

		// Token: 0x17001CF4 RID: 7412
		// (get) Token: 0x06007CAA RID: 31914
		TextAnchor Range { get; }

		// Token: 0x17001CF5 RID: 7413
		// (get) Token: 0x06007CAB RID: 31915
		int Priority { get; }

		// Token: 0x17001CF6 RID: 7414
		// (get) Token: 0x06007CAC RID: 31916
		bool HighlightContent { get; }
	}
}
