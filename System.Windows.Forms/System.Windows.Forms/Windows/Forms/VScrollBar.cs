using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a standard Windows vertical scroll bar.</summary>
	// Token: 0x02000421 RID: 1057
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionVScrollBar")]
	public class VScrollBar : ScrollBar
	{
		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0013256C File Offset: 0x0013076C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1;
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x0013258F File Offset: 0x0013078F
		protected override Size DefaultSize
		{
			get
			{
				if (DpiHelper.EnableDpiChangedHighDpiImprovements)
				{
					return new Size(SystemInformation.GetVerticalScrollBarWidthForDpi(this.deviceDpi), base.LogicalToDeviceUnits(80));
				}
				return new Size(SystemInformation.VerticalScrollBarWidth, 80);
			}
		}

		/// <summary>Gets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.</summary>
		/// <returns>The <see cref="F:System.Windows.Forms.RightToLeft.No" /> value.</returns>
		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06004960 RID: 18784 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06004961 RID: 18785 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
			set
			{
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.VScrollBar.RightToLeft" /> property changes.</summary>
		// Token: 0x140003B7 RID: 951
		// (add) Token: 0x06004962 RID: 18786 RVA: 0x000DAB83 File Offset: 0x000D8D83
		// (remove) Token: 0x06004963 RID: 18787 RVA: 0x000DAB8C File Offset: 0x000D8D8C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		// Token: 0x040026EC RID: 9964
		private const int VERTICAL_SCROLLBAR_HEIGHT = 80;
	}
}
