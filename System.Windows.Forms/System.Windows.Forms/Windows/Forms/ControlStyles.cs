using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Specifies the style and behavior of a control.</summary>
	// Token: 0x02000160 RID: 352
	[Flags]
	public enum ControlStyles
	{
		/// <summary>If <see langword="true" />, the control is a container-like control.</summary>
		// Token: 0x0400086A RID: 2154
		ContainerControl = 1,
		/// <summary>If <see langword="true" />, the control paints itself rather than the operating system doing so. If <see langword="false" />, the <see cref="E:System.Windows.Forms.Control.Paint" /> event is not raised. This style only applies to classes derived from <see cref="T:System.Windows.Forms.Control" />.</summary>
		// Token: 0x0400086B RID: 2155
		UserPaint = 2,
		/// <summary>If <see langword="true" />, the control is drawn opaque and the background is not painted.</summary>
		// Token: 0x0400086C RID: 2156
		Opaque = 4,
		/// <summary>If <see langword="true" />, the control is redrawn when it is resized.</summary>
		// Token: 0x0400086D RID: 2157
		ResizeRedraw = 16,
		/// <summary>If <see langword="true" />, the control has a fixed width when auto-scaled. For example, if a layout operation attempts to rescale the control to accommodate a new <see cref="T:System.Drawing.Font" />, the control's <see cref="P:System.Windows.Forms.Control.Width" /> remains unchanged.</summary>
		// Token: 0x0400086E RID: 2158
		FixedWidth = 32,
		/// <summary>If <see langword="true" />, the control has a fixed height when auto-scaled. For example, if a layout operation attempts to rescale the control to accommodate a new <see cref="T:System.Drawing.Font" />, the control's <see cref="P:System.Windows.Forms.Control.Height" /> remains unchanged.</summary>
		// Token: 0x0400086F RID: 2159
		FixedHeight = 64,
		/// <summary>If <see langword="true" />, the control implements the standard <see cref="E:System.Windows.Forms.Control.Click" /> behavior.</summary>
		// Token: 0x04000870 RID: 2160
		StandardClick = 256,
		/// <summary>If <see langword="true" />, the control can receive focus.</summary>
		// Token: 0x04000871 RID: 2161
		Selectable = 512,
		/// <summary>If <see langword="true" />, the control does its own mouse processing, and mouse events are not handled by the operating system.</summary>
		// Token: 0x04000872 RID: 2162
		UserMouse = 1024,
		/// <summary>If <see langword="true" />, the control accepts a <see cref="P:System.Windows.Forms.Control.BackColor" /> with an alpha component of less than 255 to simulate transparency. Transparency will be simulated only if the <see cref="F:System.Windows.Forms.ControlStyles.UserPaint" /> bit is set to <see langword="true" /> and the parent control is derived from <see cref="T:System.Windows.Forms.Control" />.</summary>
		// Token: 0x04000873 RID: 2163
		SupportsTransparentBackColor = 2048,
		/// <summary>If <see langword="true" />, the control implements the standard <see cref="E:System.Windows.Forms.Control.DoubleClick" /> behavior. This style is ignored if the <see cref="F:System.Windows.Forms.ControlStyles.StandardClick" /> bit is not set to <see langword="true" />.</summary>
		// Token: 0x04000874 RID: 2164
		StandardDoubleClick = 4096,
		/// <summary>If <see langword="true" />, the control ignores the window message WM_ERASEBKGND to reduce flicker. This style should only be applied if the <see cref="F:System.Windows.Forms.ControlStyles.UserPaint" /> bit is set to <see langword="true" />.</summary>
		// Token: 0x04000875 RID: 2165
		AllPaintingInWmPaint = 8192,
		/// <summary>If <see langword="true" />, the control keeps a copy of the text rather than getting it from the <see cref="P:System.Windows.Forms.Control.Handle" /> each time it is needed. This style defaults to <see langword="false" />. This behavior improves performance, but makes it difficult to keep the text synchronized.</summary>
		// Token: 0x04000876 RID: 2166
		CacheText = 16384,
		/// <summary>If <see langword="true" />, the <see cref="M:System.Windows.Forms.Control.OnNotifyMessage(System.Windows.Forms.Message)" /> method is called for every message sent to the control's <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />. This style defaults to <see langword="false" />. <see cref="F:System.Windows.Forms.ControlStyles.EnableNotifyMessage" /> does not work in partial trust.</summary>
		// Token: 0x04000877 RID: 2167
		EnableNotifyMessage = 32768,
		/// <summary>If <see langword="true" />, drawing is performed in a buffer, and after it completes, the result is output to the screen. Double-buffering prevents flicker caused by the redrawing of the control. If you set <see cref="F:System.Windows.Forms.ControlStyles.DoubleBuffer" /> to <see langword="true" />, you should also set <see cref="F:System.Windows.Forms.ControlStyles.UserPaint" /> and <see cref="F:System.Windows.Forms.ControlStyles.AllPaintingInWmPaint" /> to <see langword="true" />.</summary>
		// Token: 0x04000878 RID: 2168
		[EditorBrowsable(EditorBrowsableState.Never)]
		DoubleBuffer = 65536,
		/// <summary>If <see langword="true" />, the control is first drawn to a buffer rather than directly to the screen, which can reduce flicker. If you set this property to <see langword="true" />, you should also set the <see cref="F:System.Windows.Forms.ControlStyles.AllPaintingInWmPaint" /> to <see langword="true" />.</summary>
		// Token: 0x04000879 RID: 2169
		OptimizedDoubleBuffer = 131072,
		/// <summary>Specifies that the value of the control's Text property, if set, determines the control's default Active Accessibility name and shortcut key.</summary>
		// Token: 0x0400087A RID: 2170
		UseTextForAccessibility = 262144
	}
}
