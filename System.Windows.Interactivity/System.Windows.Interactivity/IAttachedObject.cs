using System;

namespace System.Windows.Interactivity
{
	// Token: 0x02000002 RID: 2
	public interface IAttachedObject
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		DependencyObject AssociatedObject { get; }

		// Token: 0x06000002 RID: 2
		void Attach(DependencyObject dependencyObject);

		// Token: 0x06000003 RID: 3
		void Detach();
	}
}
