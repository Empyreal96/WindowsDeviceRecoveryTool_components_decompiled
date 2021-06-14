using System;

namespace System.Windows.Data
{
	/// <summary>Describes the direction of the data flow in a binding.</summary>
	// Token: 0x0200019B RID: 411
	public enum BindingMode
	{
		/// <summary>Causes changes to either the source property or the target property to automatically update the other. This type of binding is appropriate for editable forms or other fully-interactive UI scenarios.</summary>
		// Token: 0x040012F7 RID: 4855
		TwoWay,
		/// <summary>Updates the binding target (target) property when the binding source (source) changes. This type of binding is appropriate if the control being bound is implicitly read-only. For instance, you may bind to a source such as a stock ticker. Or perhaps your target property has no control interface provided for making changes, such as a data-bound background color of a table. If there is no need to monitor the changes of the target property, using the <see cref="F:System.Windows.Data.BindingMode.OneWay" /> binding mode avoids the overhead of the <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> binding mode.</summary>
		// Token: 0x040012F8 RID: 4856
		OneWay,
		/// <summary>Updates the binding target when the application starts or when the data context changes. This type of binding is appropriate if you are using data where either a snapshot of the current state is appropriate to use or the data is truly static. This type of binding is also useful if you want to initialize your target property with some value from a source property and the data context is not known in advance. This is essentially a simpler form of <see cref="F:System.Windows.Data.BindingMode.OneWay" /> binding that provides better performance in cases where the source value does not change.</summary>
		// Token: 0x040012F9 RID: 4857
		OneTime,
		/// <summary>Updates the source property when the target property changes.</summary>
		// Token: 0x040012FA RID: 4858
		OneWayToSource,
		/// <summary>Uses the default <see cref="P:System.Windows.Data.Binding.Mode" /> value of the binding target. The default value varies for each dependency property. In general, user-editable control properties, such as those of text boxes and check boxes, default to two-way bindings, whereas most other properties default to one-way bindings. A programmatic way to determine whether a dependency property binds one-way or two-way by default is to get the property metadata of the property using <see cref="M:System.Windows.DependencyProperty.GetMetadata(System.Type)" /> and then check the Boolean value of the <see cref="P:System.Windows.FrameworkPropertyMetadata.BindsTwoWayByDefault" /> property.</summary>
		// Token: 0x040012FB RID: 4859
		Default
	}
}
