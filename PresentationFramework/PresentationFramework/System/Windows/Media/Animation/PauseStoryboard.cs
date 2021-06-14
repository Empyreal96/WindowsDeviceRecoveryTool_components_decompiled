using System;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that pauses a <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000182 RID: 386
	public sealed class PauseStoryboard : ControllableStoryboardAction
	{
		// Token: 0x06001698 RID: 5784 RVA: 0x000707FA File Offset: 0x0006E9FA
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Pause(containingFE);
				return;
			}
			storyboard.Pause(containingFCE);
		}
	}
}
