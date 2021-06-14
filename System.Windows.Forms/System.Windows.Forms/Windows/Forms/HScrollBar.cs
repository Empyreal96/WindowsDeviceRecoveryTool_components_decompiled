using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a standard Windows horizontal scroll bar.</summary>
	// Token: 0x02000267 RID: 615
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionHScrollBar")]
	public class HScrollBar : ScrollBar
	{
		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000B2A10 File Offset: 0x000B0C10
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 0;
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x000B2A33 File Offset: 0x000B0C33
		protected override Size DefaultSize
		{
			get
			{
				return new Size(80, SystemInformation.HorizontalScrollBarHeight);
			}
		}
	}
}
