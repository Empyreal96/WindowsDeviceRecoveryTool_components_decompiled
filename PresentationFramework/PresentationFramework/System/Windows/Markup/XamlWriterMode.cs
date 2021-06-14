using System;

namespace System.Windows.Markup
{
	/// <summary>Specifies the XAML writer mode for serializing values that are expressions (such as binding declarations).</summary>
	// Token: 0x02000271 RID: 625
	public enum XamlWriterMode
	{
		/// <summary>The <see cref="T:System.Windows.Expression" /> is serialized.</summary>
		// Token: 0x04001AFE RID: 6910
		Expression,
		/// <summary>The evaluated value of the <see cref="T:System.Windows.Expression" /> is serialized.</summary>
		// Token: 0x04001AFF RID: 6911
		Value
	}
}
