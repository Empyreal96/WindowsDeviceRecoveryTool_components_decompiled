using System;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that advances a <see cref="T:System.Windows.Media.Animation.Storyboard" /> to the end of its fill period. </summary>
	// Token: 0x02000187 RID: 391
	public sealed class SkipStoryboardToFill : ControllableStoryboardAction
	{
		// Token: 0x060016A9 RID: 5801 RVA: 0x000709A2 File Offset: 0x0006EBA2
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.SkipToFill(containingFE);
				return;
			}
			storyboard.SkipToFill(containingFCE);
		}
	}
}
