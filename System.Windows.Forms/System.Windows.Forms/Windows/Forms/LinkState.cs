using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants that define the state of the link.</summary>
	// Token: 0x020002B8 RID: 696
	public enum LinkState
	{
		/// <summary>The state of a link in its normal state (none of the other states apply).</summary>
		// Token: 0x04001191 RID: 4497
		Normal,
		/// <summary>The state of a link over which a mouse pointer is resting.</summary>
		// Token: 0x04001192 RID: 4498
		Hover,
		/// <summary>The state of a link that has been clicked.</summary>
		// Token: 0x04001193 RID: 4499
		Active,
		/// <summary>The state of a link that has been visited.</summary>
		// Token: 0x04001194 RID: 4500
		Visited = 4
	}
}
