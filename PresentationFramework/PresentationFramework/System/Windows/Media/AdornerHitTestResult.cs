using System;
using System.Windows.Documents;

namespace System.Windows.Media
{
	/// <summary>Represents data returned from calling the <see cref="M:System.Windows.Documents.AdornerLayer.AdornerHitTest(System.Windows.Point)" /> method.</summary>
	// Token: 0x0200017E RID: 382
	public class AdornerHitTestResult : PointHitTestResult
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x0007039B File Offset: 0x0006E59B
		internal AdornerHitTestResult(Visual visual, Point pt, Adorner adorner) : base(visual, pt)
		{
			this._adorner = adorner;
		}

		/// <summary> Gets the visual that was hit. </summary>
		/// <returns>The visual that was hit.</returns>
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x000703AC File Offset: 0x0006E5AC
		public Adorner Adorner
		{
			get
			{
				return this._adorner;
			}
		}

		// Token: 0x0400129F RID: 4767
		private readonly Adorner _adorner;
	}
}
