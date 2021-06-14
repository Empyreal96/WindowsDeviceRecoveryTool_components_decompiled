using System;
using System.ComponentModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.StrokeErasing" /> event. </summary>
	// Token: 0x0200056D RID: 1389
	public class InkCanvasStrokeErasingEventArgs : CancelEventArgs
	{
		// Token: 0x06005B86 RID: 23430 RVA: 0x0019C5D7 File Offset: 0x0019A7D7
		internal InkCanvasStrokeErasingEventArgs(Stroke stroke)
		{
			if (stroke == null)
			{
				throw new ArgumentNullException("stroke");
			}
			this._stroke = stroke;
		}

		/// <summary>Gets the stroke that is about to be erased.</summary>
		/// <returns>The stroke that is about to be erased.</returns>
		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x0019C5F4 File Offset: 0x0019A7F4
		public Stroke Stroke
		{
			get
			{
				return this._stroke;
			}
		}

		// Token: 0x04002F89 RID: 12169
		private Stroke _stroke;
	}
}
