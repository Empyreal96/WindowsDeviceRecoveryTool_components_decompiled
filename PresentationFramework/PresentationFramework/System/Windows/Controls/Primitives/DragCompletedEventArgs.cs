using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides information about the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragCompleted" /> event that occurs when a user completes a drag operation with the mouse of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control. </summary>
	// Token: 0x02000585 RID: 1413
	public class DragCompletedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DragCompletedEventArgs" /> class. </summary>
		/// <param name="horizontalChange">The horizontal change in position of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control, resulting from the drag operation.</param>
		/// <param name="verticalChange">The vertical change in position of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control, resulting from the drag operation.</param>
		/// <param name="canceled">A Boolean value that indicates whether the drag operation was canceled by a call to the <see cref="M:System.Windows.Controls.Primitives.Thumb.CancelDrag" /> method.</param>
		// Token: 0x06005DE7 RID: 24039 RVA: 0x001A7022 File Offset: 0x001A5222
		public DragCompletedEventArgs(double horizontalChange, double verticalChange, bool canceled)
		{
			this._horizontalChange = horizontalChange;
			this._verticalChange = verticalChange;
			this._wasCanceled = canceled;
			base.RoutedEvent = Thumb.DragCompletedEvent;
		}

		/// <summary>Gets the horizontal change in position of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> after the user drags the control with the mouse. </summary>
		/// <returns>The horizontal difference between the point at which the user pressed the left mouse button and the point at which the user released the button during a drag operation of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control. There is no default value.</returns>
		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x06005DE8 RID: 24040 RVA: 0x001A704A File Offset: 0x001A524A
		public double HorizontalChange
		{
			get
			{
				return this._horizontalChange;
			}
		}

		/// <summary>Gets the vertical change in position of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> after the user drags the control with the mouse.</summary>
		/// <returns>The vertical difference between the point at which the user pressed the left mouse button and the point at which the user released the button during a drag operation of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control. There is no default value.</returns>
		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06005DE9 RID: 24041 RVA: 0x001A7052 File Offset: 0x001A5252
		public double VerticalChange
		{
			get
			{
				return this._verticalChange;
			}
		}

		/// <summary>Gets whether the drag operation for a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> was canceled by a call to the <see cref="M:System.Windows.Controls.Primitives.Thumb.CancelDrag" /> method. </summary>
		/// <returns>
		///     <see langword="true" /> if a drag operation was canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06005DEA RID: 24042 RVA: 0x001A705A File Offset: 0x001A525A
		public bool Canceled
		{
			get
			{
				return this._wasCanceled;
			}
		}

		/// <summary>Converts a method that handles the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragCompleted" /> event to the <see cref="T:System.Windows.Controls.Primitives.DragCompletedEventHandler" /> type.</summary>
		/// <param name="genericHandler">The event handler delegate.</param>
		/// <param name="genericTarget">The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that uses the handler.</param>
		// Token: 0x06005DEB RID: 24043 RVA: 0x001A7064 File Offset: 0x001A5264
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			DragCompletedEventHandler dragCompletedEventHandler = (DragCompletedEventHandler)genericHandler;
			dragCompletedEventHandler(genericTarget, this);
		}

		// Token: 0x0400303C RID: 12348
		private double _horizontalChange;

		// Token: 0x0400303D RID: 12349
		private double _verticalChange;

		// Token: 0x0400303E RID: 12350
		private bool _wasCanceled;
	}
}
