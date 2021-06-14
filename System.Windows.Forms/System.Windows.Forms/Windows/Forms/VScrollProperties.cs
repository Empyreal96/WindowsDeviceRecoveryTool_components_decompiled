using System;

namespace System.Windows.Forms
{
	/// <summary>Provides basic properties for the <see cref="T:System.Windows.Forms.VScrollBar" /> class.</summary>
	// Token: 0x02000422 RID: 1058
	public class VScrollProperties : ScrollProperties
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.VScrollProperties" /> class. </summary>
		/// <param name="container">A <see cref="T:System.Windows.Forms.ScrollableControl" /> that contains the scroll bar.</param>
		// Token: 0x06004965 RID: 18789 RVA: 0x000B2A49 File Offset: 0x000B0C49
		public VScrollProperties(ScrollableControl container) : base(container)
		{
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06004966 RID: 18790 RVA: 0x001325C0 File Offset: 0x001307C0
		internal override int PageSize
		{
			get
			{
				return base.ParentControl.ClientRectangle.Height;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06004967 RID: 18791 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override int Orientation
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06004968 RID: 18792 RVA: 0x001325E0 File Offset: 0x001307E0
		internal override int HorizontalDisplayPosition
		{
			get
			{
				return base.ParentControl.DisplayRectangle.X;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06004969 RID: 18793 RVA: 0x000B2A74 File Offset: 0x000B0C74
		internal override int VerticalDisplayPosition
		{
			get
			{
				return -this.value;
			}
		}
	}
}
