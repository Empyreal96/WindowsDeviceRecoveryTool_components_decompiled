using System;
using System.Globalization;

namespace System.Windows.Controls
{
	/// <summary>Specifies a range of pages.</summary>
	// Token: 0x0200050C RID: 1292
	public struct PageRange
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PageRange" /> class that includes only the single specified page.</summary>
		/// <param name="page">The page that is printed or processed.</param>
		// Token: 0x060052D4 RID: 21204 RVA: 0x001714C7 File Offset: 0x0016F6C7
		public PageRange(int page)
		{
			this._pageFrom = page;
			this._pageTo = page;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PageRange" /> class with the specified first and last pages.</summary>
		/// <param name="pageFrom">The first page of the range.</param>
		/// <param name="pageTo">The last page of the range.</param>
		// Token: 0x060052D5 RID: 21205 RVA: 0x001714D7 File Offset: 0x0016F6D7
		public PageRange(int pageFrom, int pageTo)
		{
			this._pageFrom = pageFrom;
			this._pageTo = pageTo;
		}

		/// <summary>Gets or sets the page number of the first page in the range.</summary>
		/// <returns>The 1-based page number of the first page in the range.</returns>
		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x060052D6 RID: 21206 RVA: 0x001714E7 File Offset: 0x0016F6E7
		// (set) Token: 0x060052D7 RID: 21207 RVA: 0x001714EF File Offset: 0x0016F6EF
		public int PageFrom
		{
			get
			{
				return this._pageFrom;
			}
			set
			{
				this._pageFrom = value;
			}
		}

		/// <summary>Gets or sets the page number of the last page in the range.</summary>
		/// <returns>The 1-based page number of the last page in the range.</returns>
		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x060052D8 RID: 21208 RVA: 0x001714F8 File Offset: 0x0016F6F8
		// (set) Token: 0x060052D9 RID: 21209 RVA: 0x00171500 File Offset: 0x0016F700
		public int PageTo
		{
			get
			{
				return this._pageTo;
			}
			set
			{
				this._pageTo = value;
			}
		}

		/// <summary>Gets a string representation of the range.</summary>
		/// <returns>A string that represents the range of pages in the format "<see cref="P:System.Windows.Controls.PageRange.PageFrom" />-<see cref="P:System.Windows.Controls.PageRange.PageTo" />".</returns>
		// Token: 0x060052DA RID: 21210 RVA: 0x0017150C File Offset: 0x0016F70C
		public override string ToString()
		{
			string result;
			if (this._pageTo != this._pageFrom)
			{
				result = string.Format(CultureInfo.InvariantCulture, SR.Get("PrintDialogPageRange"), new object[]
				{
					this._pageFrom,
					this._pageTo
				});
			}
			else
			{
				result = this._pageFrom.ToString(CultureInfo.InvariantCulture);
			}
			return result;
		}

		/// <summary>Tests whether an object of unknown type is equal to this <see cref="T:System.Windows.Controls.PageRange" />. </summary>
		/// <param name="obj">The object tested.</param>
		/// <returns>
		///     <see langword="true" /> if the object is of type <see cref="T:System.Windows.Controls.PageRange" /> and is equal to this <see cref="T:System.Windows.Controls.PageRange" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052DB RID: 21211 RVA: 0x00171572 File Offset: 0x0016F772
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != typeof(PageRange)) && this.Equals((PageRange)obj);
		}

		/// <summary>Tests whether a <see cref="T:System.Windows.Controls.PageRange" /> is equal to this <see cref="T:System.Windows.Controls.PageRange" />. </summary>
		/// <param name="pageRange">The <see cref="T:System.Windows.Controls.PageRange" /> tested.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Controls.PageRange" /> objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052DC RID: 21212 RVA: 0x0017159C File Offset: 0x0016F79C
		public bool Equals(PageRange pageRange)
		{
			return pageRange.PageFrom == this.PageFrom && pageRange.PageTo == this.PageTo;
		}

		/// <summary>Gets the hash code value for the <see cref="T:System.Windows.Controls.PageRange" />.</summary>
		/// <returns>The hash code value for the <see cref="T:System.Windows.Controls.PageRange" />.</returns>
		// Token: 0x060052DD RID: 21213 RVA: 0x001715BE File Offset: 0x0016F7BE
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Defines the "==" operator for testing whether two specified <see cref="T:System.Windows.Controls.PageRange" /> objects are equal.</summary>
		/// <param name="pr1">The first <see cref="T:System.Windows.Controls.PageRange" />.</param>
		/// <param name="pr2">The second <see cref="T:System.Windows.Controls.PageRange" />.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Controls.PageRange" /> objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052DE RID: 21214 RVA: 0x001715D0 File Offset: 0x0016F7D0
		public static bool operator ==(PageRange pr1, PageRange pr2)
		{
			return pr1.Equals(pr2);
		}

		/// <summary>Defines the "!=" operator for testing whether two specified <see cref="T:System.Windows.Controls.PageRange" /> objects are not equal.</summary>
		/// <param name="pr1">The first <see cref="T:System.Windows.Controls.PageRange" />.</param>
		/// <param name="pr2">The second <see cref="T:System.Windows.Controls.PageRange" />.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Controls.PageRange" /> objects are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060052DF RID: 21215 RVA: 0x001715DA File Offset: 0x0016F7DA
		public static bool operator !=(PageRange pr1, PageRange pr2)
		{
			return !pr1.Equals(pr2);
		}

		// Token: 0x04002CD8 RID: 11480
		private int _pageFrom;

		// Token: 0x04002CD9 RID: 11481
		private int _pageTo;
	}
}
