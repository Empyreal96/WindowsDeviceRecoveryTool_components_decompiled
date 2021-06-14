using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.StrokesReplaced" /> event. </summary>
	// Token: 0x02000567 RID: 1383
	public class InkCanvasStrokesReplacedEventArgs : EventArgs
	{
		// Token: 0x06005B6C RID: 23404 RVA: 0x0019C484 File Offset: 0x0019A684
		internal InkCanvasStrokesReplacedEventArgs(StrokeCollection newStrokes, StrokeCollection previousStrokes)
		{
			if (newStrokes == null)
			{
				throw new ArgumentNullException("newStrokes");
			}
			if (previousStrokes == null)
			{
				throw new ArgumentNullException("previousStrokes");
			}
			this._newStrokes = newStrokes;
			this._previousStrokes = previousStrokes;
		}

		/// <summary>Gets the new strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The new strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06005B6D RID: 23405 RVA: 0x0019C4B6 File Offset: 0x0019A6B6
		public StrokeCollection NewStrokes
		{
			get
			{
				return this._newStrokes;
			}
		}

		/// <summary>Gets the previous strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The previous strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06005B6E RID: 23406 RVA: 0x0019C4BE File Offset: 0x0019A6BE
		public StrokeCollection PreviousStrokes
		{
			get
			{
				return this._previousStrokes;
			}
		}

		// Token: 0x04002F81 RID: 12161
		private StrokeCollection _newStrokes;

		// Token: 0x04002F82 RID: 12162
		private StrokeCollection _previousStrokes;
	}
}
