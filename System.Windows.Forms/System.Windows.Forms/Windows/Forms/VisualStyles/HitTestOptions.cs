using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies the options that can be used when performing a hit test on the background specified by a visual style.</summary>
	// Token: 0x02000475 RID: 1141
	[Flags]
	public enum HitTestOptions
	{
		/// <summary>The hit test option for the background segment.</summary>
		// Token: 0x040032C1 RID: 12993
		BackgroundSegment = 0,
		/// <summary>The hit test option for the fixed border.</summary>
		// Token: 0x040032C2 RID: 12994
		FixedBorder = 2,
		/// <summary>The hit test option for the caption.</summary>
		// Token: 0x040032C3 RID: 12995
		Caption = 4,
		/// <summary>The hit test option for the left resizing border.</summary>
		// Token: 0x040032C4 RID: 12996
		ResizingBorderLeft = 16,
		/// <summary>The hit test option for the top resizing border.</summary>
		// Token: 0x040032C5 RID: 12997
		ResizingBorderTop = 32,
		/// <summary>The hit test option for the right resizing border.</summary>
		// Token: 0x040032C6 RID: 12998
		ResizingBorderRight = 64,
		/// <summary>The hit test option for the bottom resizing border.</summary>
		// Token: 0x040032C7 RID: 12999
		ResizingBorderBottom = 128,
		/// <summary>The hit test option for the resizing border.</summary>
		// Token: 0x040032C8 RID: 13000
		ResizingBorder = 240,
		/// <summary>The resizing border is specified as a template, not just window edges. This option is mutually exclusive with <see cref="F:System.Windows.Forms.VisualStyles.HitTestOptions.SystemSizingMargins" />; <see cref="F:System.Windows.Forms.VisualStyles.HitTestOptions.SizingTemplate" /> takes precedence.</summary>
		// Token: 0x040032C9 RID: 13001
		SizingTemplate = 256,
		/// <summary>The system resizing border width is used instead of visual style content margins. This option is mutually exclusive with <see cref="F:System.Windows.Forms.VisualStyles.HitTestOptions.SizingTemplate" />; <see cref="F:System.Windows.Forms.VisualStyles.HitTestOptions.SizingTemplate" /> takes precedence.</summary>
		// Token: 0x040032CA RID: 13002
		SystemSizingMargins = 512
	}
}
