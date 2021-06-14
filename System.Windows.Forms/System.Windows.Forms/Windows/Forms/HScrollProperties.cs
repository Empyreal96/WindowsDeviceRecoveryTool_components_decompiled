using System;

namespace System.Windows.Forms
{
	/// <summary>Provides basic properties for the <see cref="T:System.Windows.Forms.HScrollBar" /></summary>
	// Token: 0x02000268 RID: 616
	public class HScrollProperties : ScrollProperties
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.HScrollProperties" /> class. </summary>
		/// <param name="container">A <see cref="T:System.Windows.Forms.ScrollableControl" /> that contains the scroll bar.</param>
		// Token: 0x060024E1 RID: 9441 RVA: 0x000B2A49 File Offset: 0x000B0C49
		public HScrollProperties(ScrollableControl container) : base(container)
		{
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000B2A54 File Offset: 0x000B0C54
		internal override int PageSize
		{
			get
			{
				return base.ParentControl.ClientRectangle.Width;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override int Orientation
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x000B2A74 File Offset: 0x000B0C74
		internal override int HorizontalDisplayPosition
		{
			get
			{
				return -this.value;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000B2A80 File Offset: 0x000B0C80
		internal override int VerticalDisplayPosition
		{
			get
			{
				return base.ParentControl.DisplayRectangle.Y;
			}
		}
	}
}
