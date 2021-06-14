using System;

namespace System.Windows
{
	/// <summary>Specifies the types of framework-level property behavior that pertain to a particular dependency property in the Windows Presentation Foundation (WPF) property system.</summary>
	// Token: 0x020000C8 RID: 200
	[Flags]
	public enum FrameworkPropertyMetadataOptions
	{
		/// <summary>No options are specified; the dependency property uses the default behavior of the Windows Presentation Foundation (WPF) property system.</summary>
		// Token: 0x040006E9 RID: 1769
		None = 0,
		/// <summary>The measure pass of layout compositions is affected by value changes to this dependency property. </summary>
		// Token: 0x040006EA RID: 1770
		AffectsMeasure = 1,
		/// <summary>The arrange pass of layout composition is affected by value changes to this dependency property. </summary>
		// Token: 0x040006EB RID: 1771
		AffectsArrange = 2,
		/// <summary>The measure pass on the parent element is affected by value changes to this dependency property.</summary>
		// Token: 0x040006EC RID: 1772
		AffectsParentMeasure = 4,
		/// <summary>The arrange pass on the parent element is affected by value changes to this dependency property.</summary>
		// Token: 0x040006ED RID: 1773
		AffectsParentArrange = 8,
		/// <summary>Some aspect of rendering or layout composition (other than measure or arrange) is affected by value changes to this dependency property.</summary>
		// Token: 0x040006EE RID: 1774
		AffectsRender = 16,
		/// <summary>The values of this dependency property are inherited by child elements.</summary>
		// Token: 0x040006EF RID: 1775
		Inherits = 32,
		/// <summary>The values of this dependency property span separated trees for purposes of property value inheritance. </summary>
		// Token: 0x040006F0 RID: 1776
		OverridesInheritanceBehavior = 64,
		/// <summary>Data binding to this dependency property is not allowed.</summary>
		// Token: 0x040006F1 RID: 1777
		NotDataBindable = 128,
		/// <summary>The <see cref="T:System.Windows.Data.BindingMode" /> for data bindings on this dependency property defaults to <see cref="F:System.Windows.Data.BindingMode.TwoWay" />.</summary>
		// Token: 0x040006F2 RID: 1778
		BindsTwoWayByDefault = 256,
		/// <summary>The values of this dependency property should be saved or restored by journaling processes, or when navigating by Uniform resource identifiers (URIs). </summary>
		// Token: 0x040006F3 RID: 1779
		Journal = 1024,
		/// <summary>The subproperties on the value of this dependency property do not affect any aspect of rendering.</summary>
		// Token: 0x040006F4 RID: 1780
		SubPropertiesDoNotAffectRender = 2048
	}
}
