using System;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that removes a <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000183 RID: 387
	public sealed class RemoveStoryboard : ControllableStoryboardAction
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x00070816 File Offset: 0x0006EA16
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Remove(containingFE);
				return;
			}
			storyboard.Remove(containingFCE);
		}
	}
}
