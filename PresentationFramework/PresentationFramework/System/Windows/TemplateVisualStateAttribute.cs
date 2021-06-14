using System;

namespace System.Windows
{
	/// <summary>Specifies that a control can be in a certain state and that a <see cref="T:System.Windows.VisualState" /> is expected in the control's <see cref="T:System.Windows.Controls.ControlTemplate" />.</summary>
	// Token: 0x02000125 RID: 293
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class TemplateVisualStateAttribute : Attribute
	{
		/// <summary>Gets or sets the name of the state that the control can be in.</summary>
		/// <returns>The name of the state that the control can be in.</returns>
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0002D504 File Offset: 0x0002B704
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0002D50C File Offset: 0x0002B70C
		public string Name { get; set; }

		/// <summary>Gets or sets the name of the group that the state belongs to.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.VisualStateGroup" /> that the state belongs to.</returns>
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0002D515 File Offset: 0x0002B715
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x0002D51D File Offset: 0x0002B71D
		public string GroupName { get; set; }
	}
}
