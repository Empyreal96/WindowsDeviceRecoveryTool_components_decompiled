using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a control should be docked by default when added through a designer.</summary>
	// Token: 0x02000220 RID: 544
	public enum DockingBehavior
	{
		/// <summary>Do not prompt the user for the desired docking behavior.</summary>
		// Token: 0x04000E45 RID: 3653
		Never,
		/// <summary>Prompt the user for the desired docking behavior.</summary>
		// Token: 0x04000E46 RID: 3654
		Ask,
		/// <summary>Set the control's <see cref="P:System.Windows.Forms.Control.Dock" /> property to <see cref="F:System.Windows.Forms.DockStyle.Fill" />  when it is dropped into a container with no other child controls.</summary>
		// Token: 0x04000E47 RID: 3655
		AutoDock
	}
}
