using System;

namespace System.Windows
{
	/// <summary>Identifies the property system source of a particular dependency property value.</summary>
	// Token: 0x020000AE RID: 174
	public enum BaseValueSource
	{
		/// <summary>Source is not known. This is the default value.</summary>
		// Token: 0x040005FB RID: 1531
		Unknown,
		/// <summary>Source is the default value, as defined by property metadata. </summary>
		// Token: 0x040005FC RID: 1532
		Default,
		/// <summary>Source is a value through property value inheritance.</summary>
		// Token: 0x040005FD RID: 1533
		Inherited,
		/// <summary>Source is from a setter in the default style. The default style comes from the current theme. </summary>
		// Token: 0x040005FE RID: 1534
		DefaultStyle,
		/// <summary>Source is from a trigger in the default style. The default style comes from the current theme.</summary>
		// Token: 0x040005FF RID: 1535
		DefaultStyleTrigger,
		/// <summary>Source is from a style setter of a non-theme style.</summary>
		// Token: 0x04000600 RID: 1536
		Style,
		/// <summary>Source is a trigger-based value in a template that is from a non-theme style. </summary>
		// Token: 0x04000601 RID: 1537
		TemplateTrigger,
		/// <summary>Source is a trigger-based value of a non-theme style.</summary>
		// Token: 0x04000602 RID: 1538
		StyleTrigger,
		/// <summary>Source is an implicit style reference (style was based on detected type or based type). This value is only returned for the Style property itself, not for properties that are set through setters or triggers of such a style.</summary>
		// Token: 0x04000603 RID: 1539
		ImplicitStyleReference,
		/// <summary>Source is based on a parent template being used by an element.</summary>
		// Token: 0x04000604 RID: 1540
		ParentTemplate,
		/// <summary>Source is a trigger-based value from a parent template that created the element.</summary>
		// Token: 0x04000605 RID: 1541
		ParentTemplateTrigger,
		/// <summary>Source is a locally set value.</summary>
		// Token: 0x04000606 RID: 1542
		Local
	}
}
