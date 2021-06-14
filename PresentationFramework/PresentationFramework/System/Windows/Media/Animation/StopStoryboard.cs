using System;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that stops a <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000188 RID: 392
	public sealed class StopStoryboard : ControllableStoryboardAction
	{
		// Token: 0x060016AB RID: 5803 RVA: 0x000709B6 File Offset: 0x0006EBB6
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Stop(containingFE);
				return;
			}
			storyboard.Stop(containingFCE);
		}
	}
}
