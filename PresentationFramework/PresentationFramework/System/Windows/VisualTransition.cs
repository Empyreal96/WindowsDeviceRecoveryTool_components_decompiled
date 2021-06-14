using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	/// <summary>Represents the visual behavior that occurs when a control transitions from one state to another.</summary>
	// Token: 0x0200013A RID: 314
	[ContentProperty("Storyboard")]
	public class VisualTransition : DependencyObject
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.VisualTransition" /> class. </summary>
		// Token: 0x06000D0B RID: 3339 RVA: 0x00030ADC File Offset: 0x0002ECDC
		public VisualTransition()
		{
			this.DynamicStoryboardCompleted = true;
			this.ExplicitStoryboardCompleted = true;
		}

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.VisualState" /> to transition from.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.VisualState" /> to transition from.</returns>
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00030B11 File Offset: 0x0002ED11
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x00030B19 File Offset: 0x0002ED19
		public string From { get; set; }

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.VisualState" /> to transition to.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.VisualState" /> to transition to.</returns>
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00030B22 File Offset: 0x0002ED22
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x00030B2A File Offset: 0x0002ED2A
		public string To { get; set; }

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Animation.Storyboard" /> that occurs when the transition occurs.</summary>
		/// <returns>The <see cref="T:System.Windows.Media.Animation.Storyboard" /> that occurs when the transition occurs.</returns>
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00030B33 File Offset: 0x0002ED33
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00030B3B File Offset: 0x0002ED3B
		public Storyboard Storyboard { get; set; }

		/// <summary>Gets or sets the time that it takes to move from one state to another.</summary>
		/// <returns>The time that it takes to move from one state to another.</returns>
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00030B44 File Offset: 0x0002ED44
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00030B4C File Offset: 0x0002ED4C
		[TypeConverter(typeof(DurationConverter))]
		public Duration GeneratedDuration
		{
			get
			{
				return this._generatedDuration;
			}
			set
			{
				this._generatedDuration = value;
			}
		}

		/// <summary>Gets or sets a custom mathematical formula that is used to transition between states.</summary>
		/// <returns>A custom mathematical formula that is used to transition between states.</returns>
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00030B55 File Offset: 0x0002ED55
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00030B5D File Offset: 0x0002ED5D
		public IEasingFunction GeneratedEasingFunction { get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00030B66 File Offset: 0x0002ED66
		internal bool IsDefault
		{
			get
			{
				return this.From == null && this.To == null;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00030B7B File Offset: 0x0002ED7B
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x00030B83 File Offset: 0x0002ED83
		internal bool DynamicStoryboardCompleted { get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00030B8C File Offset: 0x0002ED8C
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00030B94 File Offset: 0x0002ED94
		internal bool ExplicitStoryboardCompleted { get; set; }

		// Token: 0x04000B3B RID: 2875
		private Duration _generatedDuration = new Duration(default(TimeSpan));
	}
}
