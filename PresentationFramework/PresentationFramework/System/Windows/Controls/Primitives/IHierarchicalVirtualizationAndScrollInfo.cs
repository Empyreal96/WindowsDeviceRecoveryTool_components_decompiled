using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides properties through which a control that displays hierarchical data communicates with a <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</summary>
	// Token: 0x02000596 RID: 1430
	public interface IHierarchicalVirtualizationAndScrollInfo
	{
		/// <summary>Gets or sets an object that represents the sizes of the control's viewport and cache.</summary>
		/// <returns>An object that represents the sizes of the control's viewport and cache.</returns>
		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x06005E5F RID: 24159
		// (set) Token: 0x06005E60 RID: 24160
		HierarchicalVirtualizationConstraints Constraints { get; set; }

		/// <summary>Gets an object that represents the desired size of the control's header, in device-independent units (1/96th inch per unit) and in logical units.</summary>
		/// <returns>An object that represents the desired size of the control's header, in device-independent units (1/96th inch per unit) and in logical units.</returns>
		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06005E61 RID: 24161
		HierarchicalVirtualizationHeaderDesiredSizes HeaderDesiredSizes { get; }

		/// <summary>Gets or sets an object that represents the desired size of the control's items, in device-independent units (1/96th inch per unit) and in logical units.</summary>
		/// <returns>An object that represents the desired size of the control's items, in device-independent units (1/96th inch per unit) and in logical units.</returns>
		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06005E62 RID: 24162
		// (set) Token: 0x06005E63 RID: 24163
		HierarchicalVirtualizationItemDesiredSizes ItemDesiredSizes { get; set; }

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Panel" /> that displays the items of the control.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Panel" /> that displays the items of the control.</returns>
		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06005E64 RID: 24164
		Panel ItemsHost { get; }

		/// <summary>Gets or sets a value that indicates whether the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items.</summary>
		/// <returns>
		///     <see langword="true" /> if the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06005E65 RID: 24165
		// (set) Token: 0x06005E66 RID: 24166
		bool MustDisableVirtualization { get; set; }

		/// <summary>Gets a value that indicates whether the control's layout pass occurs at a lower priority.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's layout pass occurs at a lower priority; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x06005E67 RID: 24167
		// (set) Token: 0x06005E68 RID: 24168
		bool InBackgroundLayout { get; set; }
	}
}
