using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.Gesture" /> event. </summary>
	// Token: 0x0200056F RID: 1391
	public class InkCanvasGestureEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InkCanvasGestureEventArgs" /> class. </summary>
		/// <param name="strokes">The strokes that represent the possible gesture.</param>
		/// <param name="gestureRecognitionResults">The results from the gesture recognizer.</param>
		// Token: 0x06005B8C RID: 23436 RVA: 0x0019C5FC File Offset: 0x0019A7FC
		public InkCanvasGestureEventArgs(StrokeCollection strokes, IEnumerable<GestureRecognitionResult> gestureRecognitionResults) : base(InkCanvas.GestureEvent)
		{
			if (strokes == null)
			{
				throw new ArgumentNullException("strokes");
			}
			if (strokes.Count < 1)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyStrokeCollection"), "strokes");
			}
			if (gestureRecognitionResults == null)
			{
				throw new ArgumentNullException("strokes");
			}
			List<GestureRecognitionResult> list = new List<GestureRecognitionResult>(gestureRecognitionResults);
			if (list.Count == 0)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyArray"), "gestureRecognitionResults");
			}
			this._strokes = strokes;
			this._gestureRecognitionResults = list;
		}

		/// <summary>Gets the strokes that represent the possible gesture.</summary>
		/// <returns>The strokes that represent the possible gesture.</returns>
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06005B8D RID: 23437 RVA: 0x0019C680 File Offset: 0x0019A880
		public StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		/// <summary>Returns results from the gesture recognizer.</summary>
		/// <returns>A collection of possible application gestures that the <see cref="P:System.Windows.Controls.InkCanvasGestureEventArgs.Strokes" /> might be.</returns>
		// Token: 0x06005B8E RID: 23438 RVA: 0x0019C688 File Offset: 0x0019A888
		public ReadOnlyCollection<GestureRecognitionResult> GetGestureRecognitionResults()
		{
			return new ReadOnlyCollection<GestureRecognitionResult>(this._gestureRecognitionResults);
		}

		/// <summary>Gets or sets a Boolean value that indicates whether strokes should be considered a gesture.</summary>
		/// <returns>
		///     <see langword="true" /> if the strokes are ink; <see langword="false" /> if the strokes are a gesture.</returns>
		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06005B8F RID: 23439 RVA: 0x0019C695 File Offset: 0x0019A895
		// (set) Token: 0x06005B90 RID: 23440 RVA: 0x0019C69D File Offset: 0x0019A89D
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		/// <summary>When overridden in a derived class, provides a way to invoke event handlers in a type-specific way, which can increase efficiency over the base implementation.</summary>
		/// <param name="genericHandler">The generic handler / delegate implementation to be invoked.</param>
		/// <param name="genericTarget">The target on which the provided handler should be invoked.</param>
		// Token: 0x06005B91 RID: 23441 RVA: 0x0019C6A8 File Offset: 0x0019A8A8
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			InkCanvasGestureEventHandler inkCanvasGestureEventHandler = (InkCanvasGestureEventHandler)genericHandler;
			inkCanvasGestureEventHandler(genericTarget, this);
		}

		// Token: 0x04002F8A RID: 12170
		private StrokeCollection _strokes;

		// Token: 0x04002F8B RID: 12171
		private List<GestureRecognitionResult> _gestureRecognitionResults;

		// Token: 0x04002F8C RID: 12172
		private bool _cancel;
	}
}
