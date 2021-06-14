using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides information about the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragStarted" /> event that occurs when a user drags a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control with the mouse..</summary>
	// Token: 0x02000589 RID: 1417
	public class DragStartedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DragStartedEventArgs" /> class.</summary>
		/// <param name="horizontalOffset">The horizontal offset of the mouse click with respect to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</param>
		/// <param name="verticalOffset">The vertical offset of the mouse click with respect to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</param>
		// Token: 0x06005DF8 RID: 24056 RVA: 0x001A70D0 File Offset: 0x001A52D0
		public DragStartedEventArgs(double horizontalOffset, double verticalOffset)
		{
			this._horizontalOffset = horizontalOffset;
			this._verticalOffset = verticalOffset;
			base.RoutedEvent = Thumb.DragStartedEvent;
		}

		/// <summary>Gets the horizontal offset of the mouse click relative to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		/// <returns>The horizontal offset of the mouse click with respect to the upper-left corner of the bounding box of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. There is no default value.</returns>
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x001A70F1 File Offset: 0x001A52F1
		public double HorizontalOffset
		{
			get
			{
				return this._horizontalOffset;
			}
		}

		/// <summary>Gets the vertical offset of the mouse click relative to the screen coordinates of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		/// <returns>The horizontal offset of the mouse click with respect to the upper-left corner of the bounding box of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. There is no default value.</returns>
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06005DFA RID: 24058 RVA: 0x001A70F9 File Offset: 0x001A52F9
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
		}

		/// <summary>Converts a method that handles the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragStarted" /> event to the <see cref="T:System.Windows.Controls.Primitives.DragStartedEventHandler" /> type.</summary>
		/// <param name="genericHandler">The event handler delegate.</param>
		/// <param name="genericTarget">The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that uses the handler.</param>
		// Token: 0x06005DFB RID: 24059 RVA: 0x001A7104 File Offset: 0x001A5304
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			DragStartedEventHandler dragStartedEventHandler = (DragStartedEventHandler)genericHandler;
			dragStartedEventHandler(genericTarget, this);
		}

		// Token: 0x04003041 RID: 12353
		private double _horizontalOffset;

		// Token: 0x04003042 RID: 12354
		private double _verticalOffset;
	}
}
