using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides information about the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event that occurs one or more times when a user drags a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control with the mouse.</summary>
	// Token: 0x02000587 RID: 1415
	public class DragDeltaEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DragDeltaEventArgs" /> class.</summary>
		/// <param name="horizontalChange">The horizontal change in the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> position since the last <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event.</param>
		/// <param name="verticalChange">The vertical change in the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> position since the last <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event.</param>
		// Token: 0x06005DF0 RID: 24048 RVA: 0x001A7080 File Offset: 0x001A5280
		public DragDeltaEventArgs(double horizontalChange, double verticalChange)
		{
			this._horizontalChange = horizontalChange;
			this._verticalChange = verticalChange;
			base.RoutedEvent = Thumb.DragDeltaEvent;
		}

		/// <summary>Gets the horizontal distance that the mouse has moved since the previous <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control with the mouse.</summary>
		/// <returns>A horizontal change in position of the mouse during a drag operation. There is no default value.</returns>
		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06005DF1 RID: 24049 RVA: 0x001A70A1 File Offset: 0x001A52A1
		public double HorizontalChange
		{
			get
			{
				return this._horizontalChange;
			}
		}

		/// <summary>Gets the vertical distance that the mouse has moved since the previous <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> with the mouse.</summary>
		/// <returns>A vertical change in position of the mouse during a drag operation. There is no default value.</returns>
		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06005DF2 RID: 24050 RVA: 0x001A70A9 File Offset: 0x001A52A9
		public double VerticalChange
		{
			get
			{
				return this._verticalChange;
			}
		}

		/// <summary>Converts a method that handles the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event to the <see cref="T:System.Windows.Controls.Primitives.DragDeltaEventHandler" /> type.</summary>
		/// <param name="genericHandler">The event handler delegate.</param>
		/// <param name="genericTarget">The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that uses the handler.</param>
		// Token: 0x06005DF3 RID: 24051 RVA: 0x001A70B4 File Offset: 0x001A52B4
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			DragDeltaEventHandler dragDeltaEventHandler = (DragDeltaEventHandler)genericHandler;
			dragDeltaEventHandler(genericTarget, this);
		}

		// Token: 0x0400303F RID: 12351
		private double _horizontalChange;

		// Token: 0x04003040 RID: 12352
		private double _verticalChange;
	}
}
