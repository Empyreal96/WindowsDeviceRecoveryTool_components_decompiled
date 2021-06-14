using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a panel that dynamically lays out its contents horizontally or vertically.</summary>
	// Token: 0x02000248 RID: 584
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ProvideProperty("FlowBreak", typeof(Control))]
	[DefaultProperty("FlowDirection")]
	[Designer("System.Windows.Forms.Design.FlowLayoutPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Docking(DockingBehavior.Ask)]
	[SRDescription("DescriptionFlowLayoutPanel")]
	public class FlowLayoutPanel : Panel, IExtenderProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FlowLayoutPanel" /> class.</summary>
		// Token: 0x06002266 RID: 8806 RVA: 0x000A76E0 File Offset: 0x000A58E0
		public FlowLayoutPanel()
		{
			this._flowLayoutSettings = FlowLayout.CreateSettings(this);
		}

		/// <summary>Gets a cached instance of the panel's layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> for the panel's contents.</returns>
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x000A76F4 File Offset: 0x000A58F4
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		/// <summary>Gets or sets a value indicating the flow direction of the <see cref="T:System.Windows.Forms.FlowLayoutPanel" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlowDirection" /> values indicating the direction of consecutive placement of controls in the panel. The default is <see cref="F:System.Windows.Forms.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x000A76FB File Offset: 0x000A58FB
		// (set) Token: 0x06002269 RID: 8809 RVA: 0x000A7708 File Offset: 0x000A5908
		[SRDescription("FlowPanelFlowDirectionDescr")]
		[DefaultValue(FlowDirection.LeftToRight)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		public FlowDirection FlowDirection
		{
			get
			{
				return this._flowLayoutSettings.FlowDirection;
			}
			set
			{
				this._flowLayoutSettings.FlowDirection = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.FlowLayoutPanel" /> control should wrap its contents or let the contents be clipped.</summary>
		/// <returns>
		///     <see langword="true" /> if the contents should be wrapped; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x000A7716 File Offset: 0x000A5916
		// (set) Token: 0x0600226B RID: 8811 RVA: 0x000A7723 File Offset: 0x000A5923
		[SRDescription("FlowPanelWrapContentsDescr")]
		[DefaultValue(true)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		public bool WrapContents
		{
			get
			{
				return this._flowLayoutSettings.WrapContents;
			}
			set
			{
				this._flowLayoutSettings.WrapContents = value;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.ComponentModel.IExtenderProvider.CanExtend(System.Object)" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to receive the extender properties.</param>
		/// <returns>
		///     <see langword="true" /> if this object can provide extender properties to the specified object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600226C RID: 8812 RVA: 0x000A7734 File Offset: 0x000A5934
		bool IExtenderProvider.CanExtend(object obj)
		{
			Control control = obj as Control;
			return control != null && control.Parent == this;
		}

		/// <summary>Returns a value that represents the flow-break setting of the <see cref="T:System.Windows.Forms.FlowLayoutPanel" /> control.</summary>
		/// <param name="control">The child control.</param>
		/// <returns>
		///     <see langword="true" /> if the flow break is set; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600226D RID: 8813 RVA: 0x000A7756 File Offset: 0x000A5956
		[DefaultValue(false)]
		[DisplayName("FlowBreak")]
		public bool GetFlowBreak(Control control)
		{
			return this._flowLayoutSettings.GetFlowBreak(control);
		}

		/// <summary>Sets the value that represents the flow-break setting of the <see cref="T:System.Windows.Forms.FlowLayoutPanel" /> control.</summary>
		/// <param name="control">The child control.</param>
		/// <param name="value">The flow-break value to set.</param>
		// Token: 0x0600226E RID: 8814 RVA: 0x000A7764 File Offset: 0x000A5964
		[DisplayName("FlowBreak")]
		public void SetFlowBreak(Control control, bool value)
		{
			this._flowLayoutSettings.SetFlowBreak(control, value);
		}

		// Token: 0x04000EFD RID: 3837
		private FlowLayoutSettings _flowLayoutSettings;
	}
}
