using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Describes the location of a point in the background specified by a visual style.</summary>
	// Token: 0x02000476 RID: 1142
	public enum HitTestCode
	{
		/// <summary>The hit test succeeded outside the control or on a transparent area.</summary>
		// Token: 0x040032CC RID: 13004
		Nowhere,
		/// <summary>The hit test succeeded in the middle background segment.</summary>
		// Token: 0x040032CD RID: 13005
		Client,
		/// <summary>The hit test succeeded in the left border segment.</summary>
		// Token: 0x040032CE RID: 13006
		Left = 10,
		/// <summary>The hit test succeeded in the right border segment.</summary>
		// Token: 0x040032CF RID: 13007
		Right,
		/// <summary>The hit test succeeded in the top border segment.</summary>
		// Token: 0x040032D0 RID: 13008
		Top,
		/// <summary>The hit test succeeded in the bottom border segment.</summary>
		// Token: 0x040032D1 RID: 13009
		Bottom = 15,
		/// <summary>The hit test succeeded in the top and left border intersection.</summary>
		// Token: 0x040032D2 RID: 13010
		TopLeft = 13,
		/// <summary>The hit test succeeded in the top and right border intersection.</summary>
		// Token: 0x040032D3 RID: 13011
		TopRight,
		/// <summary>The hit test succeeded in the bottom and left border intersection.</summary>
		// Token: 0x040032D4 RID: 13012
		BottomLeft = 16,
		/// <summary>The hit test succeeded in the bottom and right border intersection.</summary>
		// Token: 0x040032D5 RID: 13013
		BottomRight
	}
}
