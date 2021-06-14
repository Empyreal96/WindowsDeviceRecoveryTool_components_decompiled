using System;

namespace System.Drawing
{
	/// <summary>Specifies that, when interpreting <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> declarations, the assembly should look for the indicated resources in the same assembly, but with the <see cref="P:System.Drawing.Configuration.SystemDrawingSection.BitmapSuffix" /> configuration value appended to the declared file name.</summary>
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.Assembly)]
	public class BitmapSuffixInSameAssemblyAttribute : Attribute
	{
	}
}
