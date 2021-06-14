using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	/// <summary>Represents the visual appearance of the control when it is in a specific state.</summary>
	// Token: 0x02000136 RID: 310
	[ContentProperty("Storyboard")]
	[RuntimeNameProperty("Name")]
	public class VisualState : DependencyObject
	{
		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.VisualState" />.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.VisualState" />.</returns>
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0002FC1E File Offset: 0x0002DE1E
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x0002FC26 File Offset: 0x0002DE26
		public string Name { get; set; }

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Animation.Storyboard" /> that defines the appearance of the control when it is in the state that is represented by the <see cref="T:System.Windows.VisualState" />. </summary>
		/// <returns>A storyboard that defines the appearance of the control when it is in the state that is represented by the <see cref="T:System.Windows.VisualState" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0002FC2F File Offset: 0x0002DE2F
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0002FC41 File Offset: 0x0002DE41
		public Storyboard Storyboard
		{
			get
			{
				return (Storyboard)base.GetValue(VisualState.StoryboardProperty);
			}
			set
			{
				base.SetValue(VisualState.StoryboardProperty, value);
			}
		}

		// Token: 0x04000B25 RID: 2853
		private static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(VisualState));
	}
}
