using System;
using System.ComponentModel;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Collects the characteristics associated with flow layouts.</summary>
	// Token: 0x02000249 RID: 585
	[DefaultProperty("FlowDirection")]
	public class FlowLayoutSettings : LayoutSettings
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x000A7773 File Offset: 0x000A5973
		internal FlowLayoutSettings(IArrangedElement owner) : base(owner)
		{
		}

		/// <summary>Gets the current flow layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> currently being used. </returns>
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x000A76F4 File Offset: 0x000A58F4
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		/// <summary>Gets or sets a value indicating the flow direction of consecutive controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FlowDirection" /> indicating the flow direction of consecutive controls in the container. The default is <see cref="F:System.Windows.Forms.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x000A777C File Offset: 0x000A597C
		// (set) Token: 0x06002272 RID: 8818 RVA: 0x000A7789 File Offset: 0x000A5989
		[SRDescription("FlowPanelFlowDirectionDescr")]
		[DefaultValue(FlowDirection.LeftToRight)]
		[SRCategory("CatLayout")]
		public FlowDirection FlowDirection
		{
			get
			{
				return FlowLayout.GetFlowDirection(base.Owner);
			}
			set
			{
				FlowLayout.SetFlowDirection(base.Owner, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the contents should be wrapped or clipped when they exceed the original boundaries of their container.</summary>
		/// <returns>
		///     <see langword="true" /> if the contents should be wrapped; otherwise, <see langword="false" /> if the contents should be clipped. The default is <see langword="true" />.</returns>
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x000A7797 File Offset: 0x000A5997
		// (set) Token: 0x06002274 RID: 8820 RVA: 0x000A77A4 File Offset: 0x000A59A4
		[SRDescription("FlowPanelWrapContentsDescr")]
		[DefaultValue(true)]
		[SRCategory("CatLayout")]
		public bool WrapContents
		{
			get
			{
				return FlowLayout.GetWrapContents(base.Owner);
			}
			set
			{
				FlowLayout.SetWrapContents(base.Owner, value);
			}
		}

		/// <summary>Sets the value that represents the flow break setting of the control.</summary>
		/// <param name="child">The child control.</param>
		/// <param name="value">The flow break value to set.</param>
		// Token: 0x06002275 RID: 8821 RVA: 0x000A77B4 File Offset: 0x000A59B4
		public void SetFlowBreak(object child, bool value)
		{
			IArrangedElement element = FlowLayout.Instance.CastToArrangedElement(child);
			if (this.GetFlowBreak(child) != value)
			{
				CommonProperties.SetFlowBreak(element, value);
			}
		}

		/// <summary>Returns a value that represents the flow break setting of the control.</summary>
		/// <param name="child">The child control.</param>
		/// <returns>
		///     <see langword="true" /> if the flow break is set; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002276 RID: 8822 RVA: 0x000A77E0 File Offset: 0x000A59E0
		public bool GetFlowBreak(object child)
		{
			IArrangedElement element = FlowLayout.Instance.CastToArrangedElement(child);
			return CommonProperties.GetFlowBreak(element);
		}
	}
}
