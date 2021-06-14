using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a button control with or without visual styles. This class cannot be inherited.</summary>
	// Token: 0x02000135 RID: 309
	public sealed class ButtonRenderer
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x000027DB File Offset: 0x000009DB
		private ButtonRenderer()
		{
		}

		/// <summary>Gets or sets a value indicating whether the renderer uses the application state to determine rendering style.</summary>
		/// <returns>
		///     <see langword="true" /> if the application state is used to determine rendering style; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0001C994 File Offset: 0x0001AB94
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0001C99B File Offset: 0x0001AB9B
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return ButtonRenderer.renderMatchingApplicationState;
			}
			set
			{
				ButtonRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001C9A3 File Offset: 0x0001ABA3
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !ButtonRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		/// <summary>Indicates whether the background of the button has semitransparent or alpha-blended pieces.</summary>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		/// <returns>
		///     <see langword="true" /> if the background of the button has semitransparent or alpha-blended pieces; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600097A RID: 2426 RVA: 0x0001C9B3 File Offset: 0x0001ABB3
		public static bool IsBackgroundPartiallyTransparent(PushButtonState state)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				return ButtonRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		/// <summary>Draws the background of a control's parent in the specified area.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the background of the parent of <paramref name="childControl" />.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which to draw the parent control's background. This rectangle should be inside the child control’s bounds.</param>
		/// <param name="childControl">The control whose parent's background will be drawn.</param>
		// Token: 0x0600097B RID: 2427 RVA: 0x0001C9CE File Offset: 0x0001ABCE
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer(0);
				ButtonRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		/// <summary>Draws a button control in the specified state and bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x0600097C RID: 2428 RVA: 0x0001C9EA File Offset: 0x0001ABEA
		public static void DrawButton(Graphics g, Rectangle bounds, PushButtonState state)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				return;
			}
			ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001CA14 File Offset: 0x0001AC14
		internal static void DrawButtonForHandle(Graphics g, Rectangle bounds, bool focused, PushButtonState state, IntPtr handle)
		{
			Rectangle rectangle;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds, handle);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				rectangle = Rectangle.Inflate(bounds, -3, -3);
			}
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a button control in the specified state and bounds, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x0600097E RID: 2430 RVA: 0x0001CA6D File Offset: 0x0001AC6D
		public static void DrawButton(Graphics g, Rectangle bounds, bool focused, PushButtonState state)
		{
			ButtonRenderer.DrawButtonForHandle(g, bounds, focused, state, IntPtr.Zero);
		}

		/// <summary>Draws a button control in the specified state and bounds, with the specified text, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="buttonText">The <see cref="T:System.String" /> to draw on the button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="buttonText" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x0600097F RID: 2431 RVA: 0x0001CA7D File Offset: 0x0001AC7D
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, bool focused, PushButtonState state)
		{
			ButtonRenderer.DrawButton(g, bounds, buttonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		/// <summary>Draws a button control in the specified state and bounds, with the specified text and text formatting, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="buttonText">The <see cref="T:System.String" /> to draw on the button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="buttonText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values to apply to <paramref name="buttonText" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x06000980 RID: 2432 RVA: 0x0001CA90 File Offset: 0x0001AC90
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, TextFormatFlags flags, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			Color foreColor;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
				foreColor = ButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				rectangle = Rectangle.Inflate(bounds, -3, -3);
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, buttonText, font, rectangle, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a button control in the specified state and bounds, with the specified image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw on the button.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the dimensions of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x06000981 RID: 2433 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		public static void DrawButton(Graphics g, Rectangle bounds, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				ButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				g.DrawImage(image, imageBounds);
				rectangle = Rectangle.Inflate(bounds, -3, -3);
			}
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a button control in the specified state and bounds, with the specified text and image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="buttonText">The <see cref="T:System.String" /> to draw on the button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="buttonText" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw on the button.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the dimensions of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x06000982 RID: 2434 RVA: 0x0001CB7C File Offset: 0x0001AD7C
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			ButtonRenderer.DrawButton(g, bounds, buttonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		/// <summary>Draws a button control in the specified state and bounds; with the specified text, text formatting, and image; and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the button.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the button.</param>
		/// <param name="buttonText">The <see cref="T:System.String" /> to draw on the button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="buttonText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values to apply to <paramref name="buttonText" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw on the button.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the dimensions of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle on the button; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.PushButtonState" /> values that specifies the visual state of the button.</param>
		// Token: 0x06000983 RID: 2435 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			Color foreColor;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				ButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
				foreColor = ButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				g.DrawImage(image, imageBounds);
				rectangle = Rectangle.Inflate(bounds, -3, -3);
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, buttonText, font, rectangle, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001CC31 File Offset: 0x0001AE31
		internal static ButtonState ConvertToButtonState(PushButtonState state)
		{
			if (state == PushButtonState.Pressed)
			{
				return ButtonState.Pushed;
			}
			if (state != PushButtonState.Disabled)
			{
				return ButtonState.Normal;
			}
			return ButtonState.Inactive;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		private static void InitializeRenderer(int state)
		{
			if (ButtonRenderer.visualStyleRenderer == null)
			{
				ButtonRenderer.visualStyleRenderer = new VisualStyleRenderer(ButtonRenderer.ButtonElement.ClassName, ButtonRenderer.ButtonElement.Part, state);
				return;
			}
			ButtonRenderer.visualStyleRenderer.SetParameters(ButtonRenderer.ButtonElement.ClassName, ButtonRenderer.ButtonElement.Part, state);
		}

		// Token: 0x04000694 RID: 1684
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04000695 RID: 1685
		private static readonly VisualStyleElement ButtonElement = VisualStyleElement.Button.PushButton.Normal;

		// Token: 0x04000696 RID: 1686
		private static bool renderMatchingApplicationState = true;
	}
}
