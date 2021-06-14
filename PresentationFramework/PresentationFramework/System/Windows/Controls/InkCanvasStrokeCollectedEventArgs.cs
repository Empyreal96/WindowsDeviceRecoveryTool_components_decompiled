using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.StrokeCollected" /> event. </summary>
	// Token: 0x02000565 RID: 1381
	public class InkCanvasStrokeCollectedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InkCanvasStrokeCollectedEventArgs" /> class.</summary>
		/// <param name="stroke">The collected <see cref="T:System.Windows.Ink.Stroke" /> object.</param>
		// Token: 0x06005B65 RID: 23397 RVA: 0x0019C43C File Offset: 0x0019A63C
		public InkCanvasStrokeCollectedEventArgs(Stroke stroke) : base(InkCanvas.StrokeCollectedEvent)
		{
			if (stroke == null)
			{
				throw new ArgumentNullException("stroke");
			}
			this._stroke = stroke;
		}

		/// <summary>Gets the stroke that was added to the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The stroke that was added to the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06005B66 RID: 23398 RVA: 0x0019C45E File Offset: 0x0019A65E
		public Stroke Stroke
		{
			get
			{
				return this._stroke;
			}
		}

		/// <summary>Provides a way to invoke event handlers in a type-specific way.</summary>
		/// <param name="genericHandler">The event handler.</param>
		/// <param name="genericTarget">The event target.</param>
		// Token: 0x06005B67 RID: 23399 RVA: 0x0019C468 File Offset: 0x0019A668
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			InkCanvasStrokeCollectedEventHandler inkCanvasStrokeCollectedEventHandler = (InkCanvasStrokeCollectedEventHandler)genericHandler;
			inkCanvasStrokeCollectedEventHandler(genericTarget, this);
		}

		// Token: 0x04002F80 RID: 12160
		private Stroke _stroke;
	}
}
