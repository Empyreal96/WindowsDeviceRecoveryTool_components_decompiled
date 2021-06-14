using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a date selection range in a month calendar control.</summary>
	// Token: 0x02000355 RID: 853
	[TypeConverter(typeof(SelectionRangeConverter))]
	public sealed class SelectionRange
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SelectionRange" /> class.</summary>
		// Token: 0x0600351F RID: 13599 RVA: 0x000F2154 File Offset: 0x000F0354
		public SelectionRange()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SelectionRange" /> class with the specified beginning and ending dates.</summary>
		/// <param name="lower">The starting date in the <see cref="T:System.Windows.Forms.SelectionRange" />. </param>
		/// <param name="upper">The ending date in the <see cref="T:System.Windows.Forms.SelectionRange" />. </param>
		// Token: 0x06003520 RID: 13600 RVA: 0x000F2190 File Offset: 0x000F0390
		public SelectionRange(DateTime lower, DateTime upper)
		{
			if (lower < upper)
			{
				this.start = lower.Date;
				this.end = upper.Date;
				return;
			}
			this.start = upper.Date;
			this.end = lower.Date;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SelectionRange" /> class with the specified selection range.</summary>
		/// <param name="range">The existing <see cref="T:System.Windows.Forms.SelectionRange" />. </param>
		// Token: 0x06003521 RID: 13601 RVA: 0x000F2208 File Offset: 0x000F0408
		public SelectionRange(SelectionRange range)
		{
			this.start = range.start;
			this.end = range.end;
		}

		/// <summary>Gets or sets the ending date and time of the selection range.</summary>
		/// <returns>The ending <see cref="T:System.DateTime" /> value of the range.</returns>
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x000F2259 File Offset: 0x000F0459
		// (set) Token: 0x06003523 RID: 13603 RVA: 0x000F2261 File Offset: 0x000F0461
		public DateTime End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value.Date;
			}
		}

		/// <summary>Gets or sets the starting date and time of the selection range.</summary>
		/// <returns>The starting <see cref="T:System.DateTime" /> value of the range.</returns>
		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003524 RID: 13604 RVA: 0x000F2270 File Offset: 0x000F0470
		// (set) Token: 0x06003525 RID: 13605 RVA: 0x000F2278 File Offset: 0x000F0478
		public DateTime Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value.Date;
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.SelectionRange" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.SelectionRange" />.</returns>
		// Token: 0x06003526 RID: 13606 RVA: 0x000F2287 File Offset: 0x000F0487
		public override string ToString()
		{
			return "SelectionRange: Start: " + this.start.ToString() + ", End: " + this.end.ToString();
		}

		// Token: 0x040020B1 RID: 8369
		private DateTime start = DateTime.MinValue.Date;

		// Token: 0x040020B2 RID: 8370
		private DateTime end = DateTime.MaxValue.Date;
	}
}
