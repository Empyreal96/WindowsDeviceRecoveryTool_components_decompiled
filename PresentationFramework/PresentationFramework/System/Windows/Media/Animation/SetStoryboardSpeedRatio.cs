using System;
using System.ComponentModel;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that changes the speed of a <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000186 RID: 390
	public sealed class SetStoryboardSpeedRatio : ControllableStoryboardAction
	{
		/// <summary>Gets or sets a new <see cref="T:System.Windows.Media.Animation.Storyboard" /> animation speed as a ratio of the old animation speed.</summary>
		/// <returns>The speed ratio for the <see cref="T:System.Windows.Media.Animation.Storyboard" />. The default value is 1.0. </returns>
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060016A5 RID: 5797 RVA: 0x00070934 File Offset: 0x0006EB34
		// (set) Token: 0x060016A6 RID: 5798 RVA: 0x0007093C File Offset: 0x0006EB3C
		[DefaultValue(1.0)]
		public double SpeedRatio
		{
			get
			{
				return this._speedRatio;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SetStoryboardSpeedRatio"
					}));
				}
				this._speedRatio = value;
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0007096B File Offset: 0x0006EB6B
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.SetSpeedRatio(containingFE, this.SpeedRatio);
				return;
			}
			storyboard.SetSpeedRatio(containingFCE, this.SpeedRatio);
		}

		// Token: 0x040012A9 RID: 4777
		private double _speedRatio = 1.0;
	}
}
