using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Wraps ActiveX controls and exposes them as fully featured Windows Forms controls.</summary>
	// Token: 0x0200011D RID: 285
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultEvent("Enter")]
	[Designer("System.Windows.Forms.Design.AxHostDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class AxHost : Control, ISupportInitialize, ICustomTypeDescriptor
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost" /> class, wrapping the ActiveX control indicated by the specified CLSID. </summary>
		/// <param name="clsid">The CLSID of the ActiveX control to wrap.</param>
		// Token: 0x0600064A RID: 1610 RVA: 0x00011DD0 File Offset: 0x0000FFD0
		protected AxHost(string clsid) : this(clsid, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost" /> class, wrapping the ActiveX control indicated by the specified CLSID, and using the shortcut-menu behavior indicated by the specified <paramref name="flags" /> value.</summary>
		/// <param name="clsid">The CLSID of the ActiveX control to wrap.</param>
		/// <param name="flags">An <see cref="T:System.Int32" /> that modifies the shortcut-menu behavior for the control.</param>
		// Token: 0x0600064B RID: 1611 RVA: 0x00011DDC File Offset: 0x0000FFDC
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		protected AxHost(string clsid, int flags)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("AXMTAThread", new object[]
				{
					clsid
				}));
			}
			this.oleSite = new AxHost.OleInterfaces(this);
			this.selectionChangeHandler = new EventHandler(this.OnNewSelection);
			this.clsid = new Guid(clsid);
			this.flags = flags;
			this.axState[AxHost.assignUniqueID] = !base.GetType().GUID.Equals(AxHost.comctlImageCombo_Clsid);
			this.axState[AxHost.needLicenseKey] = true;
			this.axState[AxHost.rejectSelection] = true;
			this.isMaskEdit = this.clsid.Equals(AxHost.maskEdit_Clsid);
			this.onContainerVisibleChanged = new EventHandler(this.OnContainerVisibleChanged);
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00011EF8 File Offset: 0x000100F8
		private bool CanUIActivate
		{
			get
			{
				return this.IsUserMode() || this.editMode != 0;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00011F10 File Offset: 0x00010110
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (this.axState[AxHost.fOwnWindow] && this.IsUserMode())
				{
					createParams.Style &= -268435457;
				}
				return createParams;
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00011F51 File Offset: 0x00010151
		private bool GetAxState(int mask)
		{
			return this.axState[mask];
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00011F5F File Offset: 0x0001015F
		private void SetAxState(int mask, bool value)
		{
			this.axState[mask] = value;
		}

		/// <summary>When overridden in a derived class, attaches interfaces to the underlying ActiveX control.</summary>
		// Token: 0x06000650 RID: 1616 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void AttachInterfaces()
		{
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00011F70 File Offset: 0x00010170
		private void RealizeStyles()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			int num = 0;
			int miscStatus = this.GetOleObject().GetMiscStatus(1, out num);
			if (!NativeMethods.Failed(miscStatus))
			{
				this.miscStatusBits = num;
				this.ParseMiscBits(this.miscStatusBits);
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>A Color that represents the background color of the control.</returns>
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00011FB9 File Offset: 0x000101B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> value.</returns>
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImeMode" /> value.</returns>
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600065A RID: 1626 RVA: 0x00011FF5 File Offset: 0x000101F5
		// (remove) Token: 0x0600065B RID: 1627 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600065C RID: 1628 RVA: 0x00012014 File Offset: 0x00010214
		// (remove) Token: 0x0600065D RID: 1629 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseDoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDoubleClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x0001203B File Offset: 0x0001023B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The shortcut menu associated with the control.</returns>
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00012044 File Offset: 0x00010244
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0001204C File Offset: 0x0001024C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00012055 File Offset: 0x00010255
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value.</returns>
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00012060 File Offset: 0x00010260
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00012068 File Offset: 0x00010268
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new virtual bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The font of the text displayed by the control.</returns>
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00012079 File Offset: 0x00010279
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value.</returns>
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00012094 File Offset: 0x00010294
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x000120AC File Offset: 0x000102AC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public new virtual bool RightToLeft
		{
			get
			{
				RightToLeft rightToLeft = base.RightToLeft;
				return rightToLeft == System.Windows.Forms.RightToLeft.Yes;
			}
			set
			{
				base.RightToLeft = (value ? System.Windows.Forms.RightToLeft.Yes : System.Windows.Forms.RightToLeft.No);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x000120BB File Offset: 0x000102BB
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x000120C3 File Offset: 0x000102C3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000120CC File Offset: 0x000102CC
		internal override bool CanAccessProperties
		{
			get
			{
				int num = this.GetOcState();
				return (this.axState[AxHost.fOwnWindow] && (num > 2 || (this.IsUserMode() && num >= 2))) || num >= 4;
			}
		}

		/// <summary>Returns a value that indicates whether the hosted control is in a state in which its properties can be accessed.</summary>
		/// <returns>
		///     <see langword="true" /> if the properties of the hosted control can be accessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600066E RID: 1646 RVA: 0x0001210B File Offset: 0x0001030B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected bool PropsValid()
		{
			return this.CanAccessProperties;
		}

		/// <summary>Begins the initialization of the ActiveX control.</summary>
		// Token: 0x0600066F RID: 1647 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void BeginInit()
		{
		}

		/// <summary>Ends the initialization of an ActiveX control.</summary>
		// Token: 0x06000670 RID: 1648 RVA: 0x00012114 File Offset: 0x00010314
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void EndInit()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.CreateControl(true);
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl != null)
				{
					containerControl.VisibleChanged += this.onContainerVisibleChanged;
				}
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001214C File Offset: 0x0001034C
		private void OnContainerVisibleChanged(object sender, EventArgs e)
		{
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null)
			{
				if (containerControl.Visible && base.Visible && !this.axState[AxHost.fOwnWindow])
				{
					this.MakeVisibleWithShow();
					return;
				}
				if (!containerControl.Visible && base.Visible && base.IsHandleCreated && this.GetOcState() >= 4)
				{
					this.HideAxControl();
					return;
				}
				if (containerControl.Visible && !base.GetState(2) && base.IsHandleCreated && this.GetOcState() >= 4)
				{
					this.HideAxControl();
				}
			}
		}

		/// <summary>Returns a value that indicates whether the hosted control is in edit mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the hosted control is in edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x000121DC File Offset: 0x000103DC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool EditMode
		{
			get
			{
				return this.editMode != 0;
			}
		}

		/// <summary>Gets a value indicating whether the ActiveX control has an About dialog box.</summary>
		/// <returns>
		///     <see langword="true" /> if the ActiveX control has an About dialog box; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x000121E7 File Offset: 0x000103E7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HasAboutBox
		{
			get
			{
				return this.aboutBoxDelegate != null;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x000121F2 File Offset: 0x000103F2
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x000121FA File Offset: 0x000103FA
		private int NoComponentChangeEvents
		{
			get
			{
				return this.noComponentChange;
			}
			set
			{
				this.noComponentChange = value;
			}
		}

		/// <summary>Displays the ActiveX control's About dialog box.</summary>
		// Token: 0x06000676 RID: 1654 RVA: 0x00012203 File Offset: 0x00010403
		public void ShowAboutBox()
		{
			if (this.aboutBoxDelegate != null)
			{
				this.aboutBoxDelegate();
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.BackColorChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000677 RID: 1655 RVA: 0x00012218 File Offset: 0x00010418
		// (remove) Token: 0x06000678 RID: 1656 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackColorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.BackgroundImageChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000679 RID: 1657 RVA: 0x00012237 File Offset: 0x00010437
		// (remove) Token: 0x0600067A RID: 1658 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600067B RID: 1659 RVA: 0x00012256 File Offset: 0x00010456
		// (remove) Token: 0x0600067C RID: 1660 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageLayoutChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.BindingContextChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600067D RID: 1661 RVA: 0x00012275 File Offset: 0x00010475
		// (remove) Token: 0x0600067E RID: 1662 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BindingContextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BindingContextChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.ContextMenuChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600067F RID: 1663 RVA: 0x00012294 File Offset: 0x00010494
		// (remove) Token: 0x06000680 RID: 1664 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ContextMenuChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.CursorChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000681 RID: 1665 RVA: 0x000122B3 File Offset: 0x000104B3
		// (remove) Token: 0x06000682 RID: 1666 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"CursorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.EnabledChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000683 RID: 1667 RVA: 0x000122D2 File Offset: 0x000104D2
		// (remove) Token: 0x06000684 RID: 1668 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"EnabledChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.FontChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000685 RID: 1669 RVA: 0x000122F1 File Offset: 0x000104F1
		// (remove) Token: 0x06000686 RID: 1670 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"FontChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.ForeColorChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000687 RID: 1671 RVA: 0x00012310 File Offset: 0x00010510
		// (remove) Token: 0x06000688 RID: 1672 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ForeColorChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.RightToLeftChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000689 RID: 1673 RVA: 0x0001232F File Offset: 0x0001052F
		// (remove) Token: 0x0600068A RID: 1674 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"RightToLeftChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.TextChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600068B RID: 1675 RVA: 0x0001234E File Offset: 0x0001054E
		// (remove) Token: 0x0600068C RID: 1676 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"TextChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.Click" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600068D RID: 1677 RVA: 0x0001236D File Offset: 0x0001056D
		// (remove) Token: 0x0600068E RID: 1678 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Click
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Click"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.DragDrop" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600068F RID: 1679 RVA: 0x0001238C File Offset: 0x0001058C
		// (remove) Token: 0x06000690 RID: 1680 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragDrop"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.DragEnter" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000691 RID: 1681 RVA: 0x000123AB File Offset: 0x000105AB
		// (remove) Token: 0x06000692 RID: 1682 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragEnter"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.DragOver" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000693 RID: 1683 RVA: 0x000123CA File Offset: 0x000105CA
		// (remove) Token: 0x06000694 RID: 1684 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragOver
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragOver"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.DragLeave" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000695 RID: 1685 RVA: 0x000123E9 File Offset: 0x000105E9
		// (remove) Token: 0x06000696 RID: 1686 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DragLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragLeave"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.GiveFeedback" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000697 RID: 1687 RVA: 0x00012408 File Offset: 0x00010608
		// (remove) Token: 0x06000698 RID: 1688 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"GiveFeedback"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.HelpRequested" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000699 RID: 1689 RVA: 0x00012427 File Offset: 0x00010627
		// (remove) Token: 0x0600069A RID: 1690 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event HelpEventHandler HelpRequested
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"HelpRequested"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.Paint" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600069B RID: 1691 RVA: 0x00012446 File Offset: 0x00010646
		// (remove) Token: 0x0600069C RID: 1692 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Paint"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.QueryContinueDrag" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x0600069D RID: 1693 RVA: 0x00012465 File Offset: 0x00010665
		// (remove) Token: 0x0600069E RID: 1694 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryContinueDrag"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.QueryAccessibilityHelp" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x0600069F RID: 1695 RVA: 0x00012484 File Offset: 0x00010684
		// (remove) Token: 0x060006A0 RID: 1696 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryAccessibilityHelp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.DoubleClick" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060006A1 RID: 1697 RVA: 0x000124A3 File Offset: 0x000106A3
		// (remove) Token: 0x060006A2 RID: 1698 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DoubleClick"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.ImeModeChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060006A3 RID: 1699 RVA: 0x000124C2 File Offset: 0x000106C2
		// (remove) Token: 0x060006A4 RID: 1700 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ImeModeChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.KeyDown" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060006A5 RID: 1701 RVA: 0x000124E1 File Offset: 0x000106E1
		// (remove) Token: 0x060006A6 RID: 1702 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyDown"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.KeyPress" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060006A7 RID: 1703 RVA: 0x00012500 File Offset: 0x00010700
		// (remove) Token: 0x060006A8 RID: 1704 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyPress"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.KeyUp" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060006A9 RID: 1705 RVA: 0x0001251F File Offset: 0x0001071F
		// (remove) Token: 0x060006AA RID: 1706 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyUp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.Layout" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060006AB RID: 1707 RVA: 0x0001253E File Offset: 0x0001073E
		// (remove) Token: 0x060006AC RID: 1708 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event LayoutEventHandler Layout
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Layout"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseDown" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060006AD RID: 1709 RVA: 0x0001255D File Offset: 0x0001075D
		// (remove) Token: 0x060006AE RID: 1710 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDown"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseEnter" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060006AF RID: 1711 RVA: 0x0001257C File Offset: 0x0001077C
		// (remove) Token: 0x060006B0 RID: 1712 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseEnter"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseLeave" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060006B1 RID: 1713 RVA: 0x0001259B File Offset: 0x0001079B
		// (remove) Token: 0x060006B2 RID: 1714 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseLeave"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseHover" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060006B3 RID: 1715 RVA: 0x000125BA File Offset: 0x000107BA
		// (remove) Token: 0x060006B4 RID: 1716 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseHover
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseHover"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseMove" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060006B5 RID: 1717 RVA: 0x000125D9 File Offset: 0x000107D9
		// (remove) Token: 0x060006B6 RID: 1718 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseMove"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseUp" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060006B7 RID: 1719 RVA: 0x000125F8 File Offset: 0x000107F8
		// (remove) Token: 0x060006B8 RID: 1720 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseUp"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.MouseWheel" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060006B9 RID: 1721 RVA: 0x00012617 File Offset: 0x00010817
		// (remove) Token: 0x060006BA RID: 1722 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseWheel
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseWheel"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.ChangeUICues" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060006BB RID: 1723 RVA: 0x00012636 File Offset: 0x00010836
		// (remove) Token: 0x060006BC RID: 1724 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ChangeUICues"
				}));
			}
			remove
			{
			}
		}

		/// <summary>The <see cref="E:System.Windows.Forms.AxHost.StyleChanged" /> event is not supported by the <see cref="T:System.Windows.Forms.AxHost" /> class.</summary>
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060006BD RID: 1725 RVA: 0x00012655 File Offset: 0x00010855
		// (remove) Token: 0x060006BE RID: 1726 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler StyleChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"StyleChanged"
				}));
			}
			remove
			{
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060006BF RID: 1727 RVA: 0x00012674 File Offset: 0x00010874
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AmbientChanged(-703);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060006C0 RID: 1728 RVA: 0x00012688 File Offset: 0x00010888
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.AmbientChanged(-704);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060006C1 RID: 1729 RVA: 0x0001269C File Offset: 0x0001089C
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.AmbientChanged(-701);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000126B0 File Offset: 0x000108B0
		private void AmbientChanged(int dispid)
		{
			if (this.GetOcx() != null)
			{
				try
				{
					base.Invalidate();
					this.GetOleControl().OnAmbientPropertyChange(dispid);
				}
				catch (Exception ex)
				{
				}
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000126F0 File Offset: 0x000108F0
		private bool OwnWindow()
		{
			return this.axState[AxHost.fOwnWindow] || this.axState[AxHost.fFakingWindow];
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00012716 File Offset: 0x00010916
		private IntPtr GetHandleNoCreate()
		{
			if (base.IsHandleCreated)
			{
				return base.Handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001272C File Offset: 0x0001092C
		private ISelectionService GetSelectionService()
		{
			return AxHost.GetSelectionService(this);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00012734 File Offset: 0x00010934
		private static ISelectionService GetSelectionService(Control ctl)
		{
			ISite site = ctl.Site;
			if (site != null)
			{
				object service = site.GetService(typeof(ISelectionService));
				return service as ISelectionService;
			}
			return null;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00012764 File Offset: 0x00010964
		private void AddSelectionHandler()
		{
			if (this.axState[AxHost.addedSelectionHandler])
			{
				return;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				selectionService.SelectionChanging += this.selectionChangeHandler;
			}
			this.axState[AxHost.addedSelectionHandler] = true;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000127AC File Offset: 0x000109AC
		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (e.Component == this)
			{
				UnsafeNativeMethods.IOleControl oleControl = this.GetOcx() as UnsafeNativeMethods.IOleControl;
				if (oleControl != null)
				{
					oleControl.OnAmbientPropertyChange(-702);
				}
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000127E0 File Offset: 0x000109E0
		private bool RemoveSelectionHandler()
		{
			if (!this.axState[AxHost.addedSelectionHandler])
			{
				return false;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				selectionService.SelectionChanging -= this.selectionChangeHandler;
			}
			this.axState[AxHost.addedSelectionHandler] = false;
			return true;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001282C File Offset: 0x00010A2C
		private void SyncRenameNotification(bool hook)
		{
			if (base.DesignMode && hook != this.axState[AxHost.renameEventHooked])
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					if (hook)
					{
						componentChangeService.ComponentRename += this.OnComponentRename;
					}
					else
					{
						componentChangeService.ComponentRename -= this.OnComponentRename;
					}
					this.axState[AxHost.renameEventHooked] = hook;
				}
			}
		}

		/// <summary>Gets or sets the site of the control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the Control, if any.</returns>
		// Token: 0x1700022F RID: 559
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x000128A8 File Offset: 0x00010AA8
		public override ISite Site
		{
			set
			{
				if (this.axState[AxHost.disposed])
				{
					return;
				}
				bool flag = this.RemoveSelectionHandler();
				bool flag2 = this.IsUserMode();
				this.SyncRenameNotification(false);
				base.Site = value;
				bool flag3 = this.IsUserMode();
				if (!flag3)
				{
					this.GetOcxCreate();
				}
				if (flag)
				{
					this.AddSelectionHandler();
				}
				this.SyncRenameNotification(value != null);
				if (value != null && !flag3 && flag2 != flag3 && this.GetOcState() > 1)
				{
					this.TransitionDownTo(1);
					this.TransitionUpTo(4);
					ContainerControl containerControl = this.ContainingControl;
					if (containerControl != null && containerControl.Visible && base.Visible)
					{
						this.MakeVisibleWithShow();
					}
				}
				if (flag2 != flag3 && !base.IsHandleCreated && !this.axState[AxHost.disposed] && this.GetOcx() != null)
				{
					this.RealizeStyles();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060006CC RID: 1740 RVA: 0x00012978 File Offset: 0x00010B78
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLostFocus(EventArgs e)
		{
			bool flag = this.GetHandleNoCreate() != this.hwndFocus;
			if (flag && base.IsHandleCreated)
			{
				flag = !UnsafeNativeMethods.IsChild(new HandleRef(this, this.GetHandleNoCreate()), new HandleRef(null, this.hwndFocus));
			}
			base.OnLostFocus(e);
			if (flag)
			{
				this.UiDeactivate();
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000129D4 File Offset: 0x00010BD4
		private void OnNewSelection(object sender, EventArgs e)
		{
			if (this.IsUserMode())
			{
				return;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				if (this.GetOcState() >= 8 && !selectionService.GetComponentSelected(this))
				{
					int hr = this.UiDeactivate();
					NativeMethods.Failed(hr);
				}
				if (!selectionService.GetComponentSelected(this))
				{
					if (this.editMode != 0)
					{
						this.GetParentContainer().OnExitEditMode(this);
						this.editMode = 0;
					}
					this.SetSelectionStyle(1);
					this.RemoveSelectionHandler();
					return;
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
				{
					int num = (int)propertyDescriptor.GetValue(this);
					if (num != this.selectionStyle)
					{
						propertyDescriptor.SetValue(this, this.selectionStyle);
					}
				}
			}
		}

		/// <summary>This method is not supported by this control.</summary>
		/// <param name="bitmap">A <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="targetBounds">A <see cref="T:System.Drawing.Rectangle" />.</param>
		// Token: 0x060006CE RID: 1742 RVA: 0x00012A9D File Offset: 0x00010C9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x060006CF RID: 1743 RVA: 0x00012AA8 File Offset: 0x00010CA8
		protected override void CreateHandle()
		{
			if (!base.IsHandleCreated)
			{
				this.TransitionUpTo(2);
				if (!this.axState[AxHost.fOwnWindow])
				{
					if (this.axState[AxHost.fNeedOwnWindow])
					{
						this.axState[AxHost.fNeedOwnWindow] = false;
						this.axState[AxHost.fFakingWindow] = true;
						base.CreateHandle();
					}
					else
					{
						this.TransitionUpTo(4);
						if (this.axState[AxHost.fNeedOwnWindow])
						{
							this.CreateHandle();
							return;
						}
					}
				}
				else
				{
					base.SetState(2, false);
					base.CreateHandle();
				}
				this.GetParentContainer().ControlCreated(this);
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00012B4F File Offset: 0x00010D4F
		private NativeMethods.COMRECT GetClipRect(NativeMethods.COMRECT clipRect)
		{
			if (clipRect != null)
			{
				AxHost.FillInRect(clipRect, new Rectangle(0, 0, 32000, 32000));
			}
			return clipRect;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00012B70 File Offset: 0x00010D70
		private static int SetupLogPixels(bool force)
		{
			if (AxHost.logPixelsX == -1 || force)
			{
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				if (dc == IntPtr.Zero)
				{
					return -2147467259;
				}
				AxHost.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
				AxHost.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
			return 0;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00012BE0 File Offset: 0x00010DE0
		private void HiMetric2Pixel(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			pointl.x = sz.cx;
			pointl.y = sz.cy;
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			((UnsafeNativeMethods.IOleControlSite)this.oleSite).TransformCoords(pointl, tagPOINTF, 6);
			szout.cx = (int)tagPOINTF.x;
			szout.cy = (int)tagPOINTF.y;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00012C3C File Offset: 0x00010E3C
		private void Pixel2hiMetric(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			tagPOINTF.x = (float)sz.cx;
			tagPOINTF.y = (float)sz.cy;
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			((UnsafeNativeMethods.IOleControlSite)this.oleSite).TransformCoords(pointl, tagPOINTF, 10);
			szout.cx = pointl.x;
			szout.cy = pointl.y;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00012C98 File Offset: 0x00010E98
		private static int Pixel2Twip(int v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = xDirection ? AxHost.logPixelsX : AxHost.logPixelsY;
			return (int)((double)v / (double)num * 72.0 * 20.0);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00012CD8 File Offset: 0x00010ED8
		private static int Twip2Pixel(double v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = xDirection ? AxHost.logPixelsX : AxHost.logPixelsY;
			return (int)(v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00012D18 File Offset: 0x00010F18
		private static int Twip2Pixel(int v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = xDirection ? AxHost.logPixelsX : AxHost.logPixelsY;
			return (int)((double)v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00012D58 File Offset: 0x00010F58
		private Size SetExtent(int width, int height)
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			tagSIZEL.cx = width;
			tagSIZEL.cy = height;
			bool flag = !this.IsUserMode();
			try
			{
				this.Pixel2hiMetric(tagSIZEL, tagSIZEL);
				this.GetOleObject().SetExtent(1, tagSIZEL);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this.GetOleObject().GetExtent(1, tagSIZEL);
				try
				{
					this.GetOleObject().SetExtent(1, tagSIZEL);
				}
				catch (COMException ex)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00012DE8 File Offset: 0x00010FE8
		private Size GetExtent()
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			this.GetOleObject().GetExtent(1, tagSIZEL);
			this.HiMetric2Pixel(tagSIZEL, tagSIZEL);
			return new Size(tagSIZEL.cx, tagSIZEL.cy);
		}

		/// <summary>Called by the system to retrieve the current bounds of the ActiveX control.</summary>
		/// <param name="bounds">The original bounds of the ActiveX control.</param>
		/// <param name="factor">A scaling factor. </param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value.</param>
		/// <returns>The unmodified <paramref name="bounds" /> value.</returns>
		// Token: 0x060006D9 RID: 1753 RVA: 0x00012E22 File Offset: 0x00011022
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			return bounds;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00012E25 File Offset: 0x00011025
		private void SetObjectRects(Rectangle bounds)
		{
			if (this.GetOcState() < 4)
			{
				return;
			}
			this.GetInPlaceObject().SetObjectRects(AxHost.FillInRect(new NativeMethods.COMRECT(), bounds), this.GetClipRect(new NativeMethods.COMRECT()));
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control. 
		/// </param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x060006DB RID: 1755 RVA: 0x00012E54 File Offset: 0x00011054
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.GetAxState(AxHost.handlePosRectChanged))
			{
				return;
			}
			this.axState[AxHost.handlePosRectChanged] = true;
			Size size = base.ApplySizeConstraints(width, height);
			width = size.Width;
			height = size.Height;
			try
			{
				if (this.axState[AxHost.fFakingWindow])
				{
					base.SetBoundsCore(x, y, width, height, specified);
				}
				else
				{
					Rectangle bounds = base.Bounds;
					if (bounds.X != x || bounds.Y != y || bounds.Width != width || bounds.Height != height)
					{
						if (!base.IsHandleCreated)
						{
							base.UpdateBounds(x, y, width, height);
						}
						else
						{
							if (this.GetOcState() > 2)
							{
								this.CheckSubclassing();
								if (width != bounds.Width || height != bounds.Height)
								{
									Size size2 = this.SetExtent(width, height);
									width = size2.Width;
									height = size2.Height;
								}
							}
							if (this.axState[AxHost.manualUpdate])
							{
								this.SetObjectRects(new Rectangle(x, y, width, height));
								this.CheckSubclassing();
								base.UpdateBounds();
							}
							else
							{
								this.SetObjectRects(new Rectangle(x, y, width, height));
								base.SetBoundsCore(x, y, width, height, specified);
								base.Invalidate();
							}
						}
					}
				}
			}
			finally
			{
				this.axState[AxHost.handlePosRectChanged] = false;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00012FD4 File Offset: 0x000111D4
		private bool CheckSubclassing()
		{
			if (!base.IsHandleCreated || this.wndprocAddr == IntPtr.Zero)
			{
				return true;
			}
			IntPtr handle = base.Handle;
			IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
			if (windowLong == this.wndprocAddr)
			{
				return true;
			}
			if ((int)((long)base.SendMessage(this.REGMSG_MSG, 0, 0)) == 123)
			{
				this.wndprocAddr = windowLong;
				return true;
			}
			base.WindowReleaseHandle();
			UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -4, new HandleRef(this, windowLong));
			base.WindowAssignHandle(handle, this.axState[AxHost.assignUniqueID]);
			this.InformOfNewHandle();
			this.axState[AxHost.manualUpdate] = true;
			return false;
		}

		/// <summary>Destroys the handle associated with the control.</summary>
		// Token: 0x060006DD RID: 1757 RVA: 0x0001308F File Offset: 0x0001128F
		protected override void DestroyHandle()
		{
			if (this.axState[AxHost.fOwnWindow])
			{
				base.DestroyHandle();
				return;
			}
			if (base.IsHandleCreated)
			{
				this.TransitionDownTo(2);
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000130BC File Offset: 0x000112BC
		private void TransitionDownTo(int state)
		{
			if (this.axState[AxHost.inTransition])
			{
				return;
			}
			try
			{
				this.axState[AxHost.inTransition] = true;
				while (state < this.GetOcState())
				{
					int num = this.GetOcState();
					switch (num)
					{
					case 1:
						this.ReleaseAxControl();
						this.SetOcState(0);
						continue;
					case 2:
						this.StopEvents();
						this.DisposeAxControl();
						this.SetOcState(1);
						continue;
					case 3:
						break;
					case 4:
						if (this.axState[AxHost.fFakingWindow])
						{
							this.DestroyFakeWindow();
							this.SetOcState(2);
						}
						else
						{
							this.InPlaceDeactivate();
						}
						this.SetOcState(2);
						continue;
					default:
						if (num == 8)
						{
							int num2 = this.UiDeactivate();
							this.SetOcState(4);
							continue;
						}
						if (num == 16)
						{
							this.SetOcState(8);
							continue;
						}
						break;
					}
					this.SetOcState(this.GetOcState() - 1);
				}
			}
			finally
			{
				this.axState[AxHost.inTransition] = false;
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000131C8 File Offset: 0x000113C8
		private void TransitionUpTo(int state)
		{
			if (this.axState[AxHost.inTransition])
			{
				return;
			}
			try
			{
				this.axState[AxHost.inTransition] = true;
				while (state > this.GetOcState())
				{
					switch (this.GetOcState())
					{
					case 0:
						this.axState[AxHost.disposed] = false;
						this.GetOcxCreate();
						this.SetOcState(1);
						continue;
					case 1:
						this.ActivateAxControl();
						this.SetOcState(2);
						if (this.IsUserMode())
						{
							this.StartEvents();
							continue;
						}
						continue;
					case 2:
						this.axState[AxHost.ownDisposing] = false;
						if (!this.axState[AxHost.fOwnWindow])
						{
							this.InPlaceActivate();
							if (!base.Visible && this.ContainingControl != null && this.ContainingControl.Visible)
							{
								this.HideAxControl();
							}
							else
							{
								base.CreateControl(true);
								if (!this.IsUserMode() && !this.axState[AxHost.ocxStateSet])
								{
									Size extent = this.GetExtent();
									Rectangle bounds = base.Bounds;
									if (bounds.Size.Equals(this.DefaultSize) && !bounds.Size.Equals(extent))
									{
										bounds.Width = extent.Width;
										bounds.Height = extent.Height;
										base.Bounds = bounds;
									}
								}
							}
						}
						if (this.GetOcState() < 4)
						{
							this.SetOcState(4);
						}
						this.OnInPlaceActive();
						continue;
					case 4:
						this.DoVerb(-1);
						this.SetOcState(8);
						continue;
					}
					this.SetOcState(this.GetOcState() + 1);
				}
			}
			finally
			{
				this.axState[AxHost.inTransition] = false;
			}
		}

		/// <summary>Called when the control transitions to the in-place active state.</summary>
		// Token: 0x060006E0 RID: 1760 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnInPlaceActive()
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000133C8 File Offset: 0x000115C8
		private void InPlaceActivate()
		{
			try
			{
				this.DoVerb(-5);
			}
			catch (Exception inner)
			{
				throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
				{
					base.GetType().Name
				}), inner);
			}
			this.EnsureWindowPresent();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001341C File Offset: 0x0001161C
		private void InPlaceDeactivate()
		{
			this.axState[AxHost.ownDisposing] = true;
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null && containerControl.ActiveControl == this)
			{
				containerControl.ActiveControl = null;
			}
			try
			{
				this.GetInPlaceObject().InPlaceDeactivate();
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00013478 File Offset: 0x00011678
		private void UiActivate()
		{
			if (this.CanUIActivate)
			{
				this.DoVerb(-4);
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001348A File Offset: 0x0001168A
		private void DestroyFakeWindow()
		{
			this.axState[AxHost.fFakingWindow] = false;
			base.DestroyHandle();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000134A4 File Offset: 0x000116A4
		private void EnsureWindowPresent()
		{
			if (!base.IsHandleCreated)
			{
				try
				{
					((UnsafeNativeMethods.IOleClientSite)this.oleSite).ShowObject();
				}
				catch
				{
				}
			}
			if (base.IsHandleCreated)
			{
				return;
			}
			if (this.ParentInternal != null)
			{
				throw new NotSupportedException(SR.GetString("AXNohWnd", new object[]
				{
					base.GetType().Name
				}));
			}
		}

		/// <summary>Sets the control to the specified visible state.</summary>
		/// <param name="value">
		///       <see langword="true" /> to make the control visible; otherwise, <see langword="false" />.</param>
		// Token: 0x060006E6 RID: 1766 RVA: 0x00013510 File Offset: 0x00011710
		protected override void SetVisibleCore(bool value)
		{
			if (base.GetState(2) != value)
			{
				bool visible = base.Visible;
				if ((base.IsHandleCreated || value) && this.ParentInternal != null && this.ParentInternal.Created && !this.axState[AxHost.fOwnWindow])
				{
					this.TransitionUpTo(2);
					if (value)
					{
						if (this.axState[AxHost.fFakingWindow])
						{
							this.DestroyFakeWindow();
						}
						if (!base.IsHandleCreated)
						{
							try
							{
								this.SetExtent(base.Width, base.Height);
								this.InPlaceActivate();
								base.CreateControl(true);
								goto IL_AE;
							}
							catch
							{
								this.MakeVisibleWithShow();
								goto IL_AE;
							}
						}
						this.MakeVisibleWithShow();
					}
					else
					{
						this.HideAxControl();
					}
				}
				IL_AE:
				if (!value)
				{
					this.axState[AxHost.fNeedOwnWindow] = false;
				}
				if (!this.axState[AxHost.fOwnWindow])
				{
					base.SetState(2, value);
					if (base.Visible != visible)
					{
						this.OnVisibleChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00013620 File Offset: 0x00011820
		private void MakeVisibleWithShow()
		{
			ContainerControl containerControl = this.ContainingControl;
			Control control = (containerControl == null) ? null : containerControl.ActiveControl;
			try
			{
				this.DoVerb(-1);
			}
			catch (Exception inner)
			{
				throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
				{
					base.GetType().Name
				}), inner);
			}
			this.EnsureWindowPresent();
			base.CreateControl(true);
			if (containerControl != null && containerControl.ActiveControl != control)
			{
				containerControl.ActiveControl = control;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000136A0 File Offset: 0x000118A0
		private void HideAxControl()
		{
			this.DoVerb(-3);
			if (this.GetOcState() < 4)
			{
				this.axState[AxHost.fNeedOwnWindow] = true;
				this.SetOcState(4);
			}
		}

		/// <summary>Determines if a character is an input character that the ActiveX control recognizes.</summary>
		/// <param name="charCode">The character to test. </param>
		/// <returns>
		///     <see langword="true" /> if the character should be sent directly to the ActiveX control and not preprocessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006E9 RID: 1769 RVA: 0x0000E214 File Offset: 0x0000C414
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006EA RID: 1770 RVA: 0x000136CB File Offset: 0x000118CB
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return !this.ignoreDialogKeys && base.ProcessDialogKey(keyData);
		}

		/// <summary>Preprocesses keyboard or input messages within the message loop before they are dispatched.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR. </param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006EB RID: 1771 RVA: 0x000136E0 File Offset: 0x000118E0
		public override bool PreProcessMessage(ref Message msg)
		{
			if (this.IsUserMode())
			{
				if (this.axState[AxHost.siteProcessedInputKey])
				{
					return base.PreProcessMessage(ref msg);
				}
				NativeMethods.MSG msg2 = default(NativeMethods.MSG);
				msg2.message = msg.Msg;
				msg2.wParam = msg.WParam;
				msg2.lParam = msg.LParam;
				msg2.hwnd = msg.HWnd;
				this.axState[AxHost.siteProcessedInputKey] = false;
				try
				{
					UnsafeNativeMethods.IOleInPlaceActiveObject inPlaceActiveObject = this.GetInPlaceActiveObject();
					if (inPlaceActiveObject != null)
					{
						int num = inPlaceActiveObject.TranslateAccelerator(ref msg2);
						msg.Msg = msg2.message;
						msg.WParam = msg2.wParam;
						msg.LParam = msg2.lParam;
						msg.HWnd = msg2.hwnd;
						if (num == 0)
						{
							return true;
						}
						if (num == 1)
						{
							bool result = false;
							this.ignoreDialogKeys = true;
							try
							{
								result = base.PreProcessMessage(ref msg);
							}
							finally
							{
								this.ignoreDialogKeys = false;
							}
							return result;
						}
						if (this.axState[AxHost.siteProcessedInputKey])
						{
							return base.PreProcessMessage(ref msg);
						}
						return false;
					}
				}
				finally
				{
					this.axState[AxHost.siteProcessedInputKey] = false;
				}
				return false;
			}
			return false;
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006EC RID: 1772 RVA: 0x00013828 File Offset: 0x00011A28
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.CanSelect)
			{
				try
				{
					NativeMethods.tagCONTROLINFO tagCONTROLINFO = new NativeMethods.tagCONTROLINFO();
					int controlInfo = this.GetOleControl().GetControlInfo(tagCONTROLINFO);
					if (NativeMethods.Failed(controlInfo))
					{
						return false;
					}
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					msg.hwnd = ((this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle);
					msg.message = 260;
					msg.wParam = (IntPtr)((int)char.ToUpper(charCode, CultureInfo.CurrentCulture));
					msg.lParam = (IntPtr)538443777;
					msg.time = SafeNativeMethods.GetTickCount();
					NativeMethods.POINT point = new NativeMethods.POINT();
					UnsafeNativeMethods.GetCursorPos(point);
					msg.pt_x = point.x;
					msg.pt_y = point.y;
					if (SafeNativeMethods.IsAccelerator(new HandleRef(tagCONTROLINFO, tagCONTROLINFO.hAccel), (int)tagCONTROLINFO.cAccel, ref msg, null))
					{
						this.GetOleControl().OnMnemonic(ref msg);
						base.Focus();
						return true;
					}
				}
				catch (Exception ex)
				{
					return false;
				}
				return false;
			}
			return false;
		}

		/// <summary>Calls the <see cref="M:System.Windows.Forms.AxHost.ShowAboutBox" /> method to display the ActiveX control's About dialog box.</summary>
		/// <param name="d">The <see cref="T:System.Windows.Forms.AxHost.AboutBoxDelegate" /> to call. </param>
		// Token: 0x060006ED RID: 1773 RVA: 0x00013948 File Offset: 0x00011B48
		protected void SetAboutBoxDelegate(AxHost.AboutBoxDelegate d)
		{
			this.aboutBoxDelegate = (AxHost.AboutBoxDelegate)Delegate.Combine(this.aboutBoxDelegate, d);
		}

		/// <summary>Gets or sets the persisted state of the ActiveX control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.AxHost.State" /> that represents the persisted state of the ActiveX control.</returns>
		/// <exception cref="T:System.Exception">The ActiveX control is already loaded. </exception>
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00013961 File Offset: 0x00011B61
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001398C File Offset: 0x00011B8C
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public AxHost.State OcxState
		{
			get
			{
				if (this.IsDirty() || this.ocxState == null)
				{
					this.ocxState = this.CreateNewOcxState(this.ocxState);
				}
				return this.ocxState;
			}
			set
			{
				this.axState[AxHost.ocxStateSet] = true;
				if (value == null)
				{
					return;
				}
				if (this.storageType != -1 && this.storageType != value.type)
				{
					throw new InvalidOperationException(SR.GetString("AXOcxStateLoaded"));
				}
				if (this.ocxState == value)
				{
					return;
				}
				this.ocxState = value;
				if (this.ocxState != null)
				{
					this.axState[AxHost.manualUpdate] = this.ocxState._GetManualUpdate();
					this.licenseKey = this.ocxState._GetLicenseKey();
				}
				else
				{
					this.axState[AxHost.manualUpdate] = false;
					this.licenseKey = null;
				}
				if (this.ocxState != null && this.GetOcState() >= 2)
				{
					this.DepersistControl();
				}
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00013A4C File Offset: 0x00011C4C
		private AxHost.State CreateNewOcxState(AxHost.State oldOcxState)
		{
			int noComponentChangeEvents = this.NoComponentChangeEvents;
			this.NoComponentChangeEvents = noComponentChangeEvents + 1;
			try
			{
				if (this.GetOcState() < 2)
				{
					return null;
				}
				try
				{
					AxHost.PropertyBagStream propertyBagStream = null;
					if (this.iPersistPropBag != null)
					{
						propertyBagStream = new AxHost.PropertyBagStream();
						this.iPersistPropBag.Save(propertyBagStream, true, true);
					}
					noComponentChangeEvents = this.storageType;
					if (noComponentChangeEvents > 1)
					{
						if (noComponentChangeEvents != 2)
						{
							return null;
						}
						if (oldOcxState != null)
						{
							return oldOcxState.RefreshStorage(this.iPersistStorage);
						}
						return null;
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream();
						if (this.storageType == 0)
						{
							this.iPersistStream.Save(new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream), true);
						}
						else
						{
							this.iPersistStreamInit.Save(new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream), true);
						}
						if (memoryStream != null)
						{
							return new AxHost.State(memoryStream, this.storageType, this, propertyBagStream);
						}
						if (propertyBagStream != null)
						{
							return new AxHost.State(propertyBagStream);
						}
					}
				}
				catch (Exception ex)
				{
				}
			}
			finally
			{
				noComponentChangeEvents = this.NoComponentChangeEvents;
				this.NoComponentChangeEvents = noComponentChangeEvents - 1;
			}
			return null;
		}

		/// <summary>Gets or sets the control containing the ActiveX control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ContainerControl" /> that represents the control containing the ActiveX control.</returns>
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00013B54 File Offset: 0x00011D54
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x00013B7A File Offset: 0x00011D7A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ContainerControl ContainingControl
		{
			get
			{
				IntSecurity.GetParent.Demand();
				if (this.containingControl == null)
				{
					this.containingControl = this.FindContainerControlInternal();
				}
				return this.containingControl;
			}
			set
			{
				this.containingControl = value;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00013B84 File Offset: 0x00011D84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeText()
		{
			bool result = false;
			try
			{
				result = (this.Text.Length != 0);
			}
			catch (COMException)
			{
			}
			return result;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00013BB8 File Offset: 0x00011DB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeContainingControl()
		{
			return this.ContainingControl != this.ParentInternal;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00013BCC File Offset: 0x00011DCC
		private ContainerControl FindContainerControlInternal()
		{
			if (this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ContainerControl containerControl = designerHost.RootComponent as ContainerControl;
					if (containerControl != null)
					{
						return containerControl;
					}
				}
			}
			ContainerControl result = null;
			for (Control control = this; control != null; control = control.ParentInternal)
			{
				ContainerControl containerControl2 = control as ContainerControl;
				if (containerControl2 != null)
				{
					result = containerControl2;
					break;
				}
			}
			return result;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00013C34 File Offset: 0x00011E34
		private bool IsDirty()
		{
			if (this.GetOcState() < 2)
			{
				return false;
			}
			if (this.axState[AxHost.valueChanged])
			{
				this.axState[AxHost.valueChanged] = false;
				return true;
			}
			int num;
			switch (this.storageType)
			{
			case 0:
				num = this.iPersistStream.IsDirty();
				break;
			case 1:
				num = this.iPersistStreamInit.IsDirty();
				break;
			case 2:
				num = this.iPersistStorage.IsDirty();
				break;
			default:
				return true;
			}
			if (num == 1)
			{
				return false;
			}
			NativeMethods.Failed(num);
			return true;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00013CCC File Offset: 0x00011ECC
		internal bool IsUserMode()
		{
			ISite site = this.Site;
			return site == null || !site.DesignMode;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00013CF0 File Offset: 0x00011EF0
		private object GetAmbientProperty(int dispid)
		{
			Control parentInternal = this.ParentInternal;
			if (dispid != -732)
			{
				switch (dispid)
				{
				case -715:
					return true;
				case -713:
					return false;
				case -712:
					return false;
				case -711:
					return false;
				case -710:
					return false;
				case -709:
					return this.IsUserMode();
				case -706:
					return true;
				case -705:
					return Thread.CurrentThread.CurrentCulture.LCID;
				case -704:
					if (parentInternal != null)
					{
						return AxHost.GetOleColorFromColor(parentInternal.ForeColor);
					}
					return null;
				case -703:
					if (parentInternal != null)
					{
						return AxHost.GetIFontFromFont(parentInternal.Font);
					}
					return null;
				case -702:
				{
					string text = this.GetParentContainer().GetNameForControl(this);
					if (text == null)
					{
						text = "";
					}
					return text;
				}
				case -701:
					if (parentInternal != null)
					{
						return AxHost.GetOleColorFromColor(parentInternal.BackColor);
					}
					return null;
				}
				return null;
			}
			Control control = this;
			while (control != null)
			{
				if (control.RightToLeft == System.Windows.Forms.RightToLeft.No)
				{
					return false;
				}
				if (control.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
				{
					return true;
				}
				if (control.RightToLeft == System.Windows.Forms.RightToLeft.Inherit)
				{
					control = control.Parent;
				}
			}
			return null;
		}

		/// <summary>Requests that an object perform an action in response to an end-user's action.</summary>
		/// <param name="verb">One of the actions enumerated for the object in IOleObject::EnumVerbs.</param>
		// Token: 0x060006F9 RID: 1785 RVA: 0x00013E38 File Offset: 0x00012038
		public void DoVerb(int verb)
		{
			Control parentInternal = this.ParentInternal;
			this.GetOleObject().DoVerb(verb, IntPtr.Zero, this.oleSite, -1, (parentInternal != null) ? parentInternal.Handle : IntPtr.Zero, AxHost.FillInRect(new NativeMethods.COMRECT(), base.Bounds));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00013E85 File Offset: 0x00012085
		private bool AwaitingDefreezing()
		{
			return this.freezeCount > 0;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00013E90 File Offset: 0x00012090
		private void Freeze(bool v)
		{
			if (v)
			{
				try
				{
					this.GetOleControl().FreezeEvents(-1);
				}
				catch (COMException ex)
				{
				}
				this.freezeCount++;
				return;
			}
			try
			{
				this.GetOleControl().FreezeEvents(0);
			}
			catch (COMException ex2)
			{
			}
			this.freezeCount--;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00013F00 File Offset: 0x00012100
		private int UiDeactivate()
		{
			bool value = this.axState[AxHost.ownDisposing];
			this.axState[AxHost.ownDisposing] = true;
			int result = 0;
			try
			{
				result = this.GetInPlaceObject().UIDeactivate();
			}
			finally
			{
				this.axState[AxHost.ownDisposing] = value;
			}
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00013F64 File Offset: 0x00012164
		private int GetOcState()
		{
			return this.ocState;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00013F6C File Offset: 0x0001216C
		private void SetOcState(int nv)
		{
			this.ocState = nv;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00013F75 File Offset: 0x00012175
		private string GetLicenseKey()
		{
			return this.GetLicenseKey(this.clsid);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00013F84 File Offset: 0x00012184
		private string GetLicenseKey(Guid clsid)
		{
			if (this.licenseKey != null || !this.axState[AxHost.needLicenseKey])
			{
				return this.licenseKey;
			}
			try
			{
				UnsafeNativeMethods.IClassFactory2 classFactory = UnsafeNativeMethods.CoGetClassObject(ref clsid, 1, 0, ref AxHost.icf2_Guid);
				NativeMethods.tagLICINFO tagLICINFO = new NativeMethods.tagLICINFO();
				classFactory.GetLicInfo(tagLICINFO);
				if (tagLICINFO.fRuntimeAvailable != 0)
				{
					string[] array = new string[1];
					classFactory.RequestLicKey(0, array);
					this.licenseKey = array[0];
					return this.licenseKey;
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == AxHost.E_NOINTERFACE.ErrorCode)
				{
					return null;
				}
				this.axState[AxHost.needLicenseKey] = false;
			}
			catch (Exception ex2)
			{
				this.axState[AxHost.needLicenseKey] = false;
			}
			return null;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001405C File Offset: 0x0001225C
		private void CreateWithoutLicense(Guid clsid)
		{
			object obj = UnsafeNativeMethods.CoCreateInstance(ref clsid, null, 1, ref NativeMethods.ActiveX.IID_IUnknown);
			this.instance = obj;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00014084 File Offset: 0x00012284
		private void CreateWithLicense(string license, Guid clsid)
		{
			if (license != null)
			{
				try
				{
					UnsafeNativeMethods.IClassFactory2 classFactory = UnsafeNativeMethods.CoGetClassObject(ref clsid, 1, 0, ref AxHost.icf2_Guid);
					if (classFactory != null)
					{
						classFactory.CreateInstanceLic(null, null, ref NativeMethods.ActiveX.IID_IUnknown, license, out this.instance);
					}
				}
				catch (Exception ex)
				{
				}
			}
			if (this.instance == null)
			{
				this.CreateWithoutLicense(clsid);
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000140E0 File Offset: 0x000122E0
		private void CreateInstance()
		{
			try
			{
				this.instance = this.CreateInstanceCore(this.clsid);
			}
			catch (ExternalException ex)
			{
				if (ex.ErrorCode == -2147221230)
				{
					throw new LicenseException(base.GetType(), this, SR.GetString("AXNoLicenseToUse"));
				}
				throw;
			}
			this.SetOcState(1);
		}

		/// <summary>Called by the system to create the ActiveX control.</summary>
		/// <param name="clsid">The CLSID of the ActiveX control.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the ActiveX control. </returns>
		// Token: 0x06000704 RID: 1796 RVA: 0x00014140 File Offset: 0x00012340
		protected virtual object CreateInstanceCore(Guid clsid)
		{
			if (this.IsUserMode())
			{
				this.CreateWithLicense(this.licenseKey, clsid);
			}
			else
			{
				this.CreateWithoutLicense(clsid);
			}
			return this.instance;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00014168 File Offset: 0x00012368
		private CategoryAttribute GetCategoryForDispid(int dispid)
		{
			NativeMethods.ICategorizeProperties categorizeProperties = this.GetCategorizeProperties();
			if (categorizeProperties == null)
			{
				return null;
			}
			int num = 0;
			try
			{
				categorizeProperties.MapPropertyToCategory(dispid, ref num);
				if (num != 0)
				{
					int num2 = -num;
					if (num2 > 0 && num2 < AxHost.categoryNames.Length && AxHost.categoryNames[num2] != null)
					{
						return AxHost.categoryNames[num2];
					}
					num2 = -num2;
					int num3 = num2;
					if (this.objectDefinedCategoryNames != null)
					{
						CategoryAttribute categoryAttribute = (CategoryAttribute)this.objectDefinedCategoryNames[num3];
						if (categoryAttribute != null)
						{
							return categoryAttribute;
						}
					}
					string text = null;
					if (categorizeProperties.GetCategoryName(num2, CultureInfo.CurrentCulture.LCID, out text) == 0 && text != null)
					{
						CategoryAttribute categoryAttribute = new CategoryAttribute(text);
						if (this.objectDefinedCategoryNames == null)
						{
							this.objectDefinedCategoryNames = new Hashtable();
						}
						this.objectDefinedCategoryNames.Add(num3, categoryAttribute);
						return categoryAttribute;
					}
				}
			}
			catch (Exception ex)
			{
			}
			return null;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00014258 File Offset: 0x00012458
		private void SetSelectionStyle(int selectionStyle)
		{
			if (!this.IsUserMode())
			{
				ISelectionService selectionService = this.GetSelectionService();
				this.selectionStyle = selectionStyle;
				if (selectionService != null && selectionService.GetComponentSelected(this))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						propertyDescriptor.SetValue(this, selectionStyle);
					}
				}
			}
		}

		/// <summary>Attempts to activate the editing mode of the hosted control. </summary>
		// Token: 0x06000707 RID: 1799 RVA: 0x000142C0 File Offset: 0x000124C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void InvokeEditMode()
		{
			if (this.editMode != 0)
			{
				return;
			}
			this.AddSelectionHandler();
			this.editMode = 2;
			this.SetSelectionStyle(2);
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			try
			{
				this.UiActivate();
			}
			catch (Exception ex)
			{
			}
		}

		/// <summary>Returns a collection of type <see cref="T:System.Attribute" /> for the current object.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.AttributeCollection" /> with the attributes for the current object.</returns>
		// Token: 0x06000708 RID: 1800 RVA: 0x0001430C File Offset: 0x0001250C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			if (!this.axState[AxHost.editorRefresh] && this.HasPropertyPages())
			{
				this.axState[AxHost.editorRefresh] = true;
				TypeDescriptor.Refresh(base.GetType());
			}
			return TypeDescriptor.GetAttributes(this, true);
		}

		/// <summary>Returns the class name of the current object.</summary>
		/// <returns>Returns <see langword="null" /> in all cases.</returns>
		// Token: 0x06000709 RID: 1801 RVA: 0x0000DE5C File Offset: 0x0000C05C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		/// <summary>Returns the name of the current object.</summary>
		/// <returns>Returns <see langword="null" /> in all cases.</returns>
		// Token: 0x0600070A RID: 1802 RVA: 0x0000DE5C File Offset: 0x0000C05C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		/// <summary>Returns a type converter for the current object.</summary>
		/// <returns>Returns <see langword="null" /> in all cases.</returns>
		// Token: 0x0600070B RID: 1803 RVA: 0x0000DE5C File Offset: 0x0000C05C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		/// <summary>Returns the default event for the current object.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptor" /> that represents the default event for the current object, or <see langword="null" /> if the object does not have events.</returns>
		// Token: 0x0600070C RID: 1804 RVA: 0x0001434B File Offset: 0x0001254B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		/// <summary>Returns the default property for the current object.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property for the current object, or <see langword="null" /> if the object does not have properties.</returns>
		// Token: 0x0600070D RID: 1805 RVA: 0x00014354 File Offset: 0x00012554
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		/// <summary>Returns an editor of the specified type for the current object.</summary>
		/// <param name="editorBaseType">A <see cref="T:System.Type" /> that represents the editor for the current object.</param>
		/// <returns>An object of the specified type that is the editor for the current object, or <see langword="null" /> if the editor cannot be found.</returns>
		// Token: 0x0600070E RID: 1806 RVA: 0x00014360 File Offset: 0x00012560
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			if (editorBaseType != typeof(ComponentEditor))
			{
				return null;
			}
			if (this.editor != null)
			{
				return this.editor;
			}
			if (this.editor == null && this.HasPropertyPages())
			{
				this.editor = new AxHost.AxComponentEditor();
			}
			return this.editor;
		}

		/// <summary>Returns the events for the current object.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the events for the current object.</returns>
		// Token: 0x0600070F RID: 1807 RVA: 0x000143B1 File Offset: 0x000125B1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		/// <summary>Returns the events for the current object using the specified attribute array as a filter.</summary>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the events for the <see cref="T:System.Windows.Forms.AxHost" /> that match the given set of attributes.</returns>
		// Token: 0x06000710 RID: 1808 RVA: 0x000143BA File Offset: 0x000125BA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000143C4 File Offset: 0x000125C4
		private void OnIdle(object sender, EventArgs e)
		{
			if (this.axState[AxHost.refreshProperties])
			{
				TypeDescriptor.Refresh(base.GetType());
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x000143E3 File Offset: 0x000125E3
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x000143F8 File Offset: 0x000125F8
		private bool RefreshAllProperties
		{
			get
			{
				return this.axState[AxHost.refreshProperties];
			}
			set
			{
				this.axState[AxHost.refreshProperties] = value;
				if (value && !this.axState[AxHost.listeningToIdle])
				{
					Application.Idle += this.OnIdle;
					this.axState[AxHost.listeningToIdle] = true;
					return;
				}
				if (!value && this.axState[AxHost.listeningToIdle])
				{
					Application.Idle -= this.OnIdle;
					this.axState[AxHost.listeningToIdle] = false;
				}
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00014488 File Offset: 0x00012688
		private PropertyDescriptorCollection FillProperties(Attribute[] attributes)
		{
			if (this.RefreshAllProperties)
			{
				this.RefreshAllProperties = false;
				this.propsStash = null;
				this.attribsStash = null;
			}
			else if (this.propsStash != null)
			{
				if (attributes == null && this.attribsStash == null)
				{
					return this.propsStash;
				}
				if (attributes != null && this.attribsStash != null && attributes.Length == this.attribsStash.Length)
				{
					bool flag = true;
					int num = 0;
					foreach (Attribute attribute in attributes)
					{
						if (!attribute.Equals(this.attribsStash[num++]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return this.propsStash;
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			if (this.properties == null)
			{
				this.properties = new Hashtable();
			}
			if (this.propertyInfos == null)
			{
				this.propertyInfos = new Hashtable();
				PropertyInfo[] array = base.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in array)
				{
					this.propertyInfos.Add(propertyInfo.Name, propertyInfo);
				}
			}
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(this, null, true);
			if (propertyDescriptorCollection != null)
			{
				for (int k = 0; k < propertyDescriptorCollection.Count; k++)
				{
					if (propertyDescriptorCollection[k].DesignTimeOnly)
					{
						arrayList.Add(propertyDescriptorCollection[k]);
					}
					else
					{
						string name = propertyDescriptorCollection[k].Name;
						PropertyInfo propertyInfo2 = (PropertyInfo)this.propertyInfos[name];
						if (!(propertyInfo2 != null) || propertyInfo2.CanRead)
						{
							if (!this.properties.ContainsKey(name))
							{
								PropertyDescriptor propertyDescriptor;
								if (propertyInfo2 != null)
								{
									propertyDescriptor = new AxHost.AxPropertyDescriptor(propertyDescriptorCollection[k], this);
									((AxHost.AxPropertyDescriptor)propertyDescriptor).UpdateAttributes();
								}
								else
								{
									propertyDescriptor = propertyDescriptorCollection[k];
								}
								this.properties.Add(name, propertyDescriptor);
								arrayList.Add(propertyDescriptor);
							}
							else
							{
								PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)this.properties[name];
								AxHost.AxPropertyDescriptor axPropertyDescriptor = propertyDescriptor2 as AxHost.AxPropertyDescriptor;
								if ((!(propertyInfo2 == null) || axPropertyDescriptor == null) && (!(propertyInfo2 != null) || axPropertyDescriptor != null))
								{
									if (axPropertyDescriptor != null)
									{
										axPropertyDescriptor.UpdateAttributes();
									}
									arrayList.Add(propertyDescriptor2);
								}
							}
						}
					}
				}
				if (attributes != null)
				{
					Attribute attribute2 = null;
					foreach (Attribute attribute3 in attributes)
					{
						if (attribute3 is BrowsableAttribute)
						{
							attribute2 = attribute3;
						}
					}
					if (attribute2 != null)
					{
						ArrayList arrayList2 = null;
						foreach (object obj in arrayList)
						{
							PropertyDescriptor propertyDescriptor3 = (PropertyDescriptor)obj;
							if (propertyDescriptor3 is AxHost.AxPropertyDescriptor)
							{
								Attribute attribute4 = propertyDescriptor3.Attributes[typeof(BrowsableAttribute)];
								if (attribute4 != null && !attribute4.Equals(attribute2))
								{
									if (arrayList2 == null)
									{
										arrayList2 = new ArrayList();
									}
									arrayList2.Add(propertyDescriptor3);
								}
							}
						}
						if (arrayList2 != null)
						{
							foreach (object obj2 in arrayList2)
							{
								arrayList.Remove(obj2);
							}
						}
					}
				}
			}
			PropertyDescriptor[] array3 = new PropertyDescriptor[arrayList.Count];
			arrayList.CopyTo(array3, 0);
			this.propsStash = new PropertyDescriptorCollection(array3);
			this.attribsStash = attributes;
			return this.propsStash;
		}

		/// <summary>Returns the properties for the current object.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the events for the current object.</returns>
		// Token: 0x06000715 RID: 1813 RVA: 0x00014810 File Offset: 0x00012A10
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.FillProperties(null);
		}

		/// <summary>Returns the properties for the current object using the specified attribute array as a filter.</summary>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the events for the current <see cref="T:System.Windows.Forms.AxHost" /> that match the given set of attributes.</returns>
		// Token: 0x06000716 RID: 1814 RVA: 0x00014819 File Offset: 0x00012A19
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return this.FillProperties(attributes);
		}

		/// <summary>Returns the object that owns the specified value.</summary>
		/// <param name="pd">Not used.</param>
		/// <returns>The current object.</returns>
		// Token: 0x06000717 RID: 1815 RVA: 0x000069BD File Offset: 0x00004BBD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00014824 File Offset: 0x00012A24
		private AxHost.AxPropertyDescriptor GetPropertyDescriptorFromDispid(int dispid)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = this.FillProperties(null);
			foreach (object obj in propertyDescriptorCollection)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				AxHost.AxPropertyDescriptor axPropertyDescriptor = propertyDescriptor as AxHost.AxPropertyDescriptor;
				if (axPropertyDescriptor != null && axPropertyDescriptor.Dispid == dispid)
				{
					return axPropertyDescriptor;
				}
			}
			return null;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00014898 File Offset: 0x00012A98
		private void ActivateAxControl()
		{
			if (this.QuickActivate())
			{
				this.DepersistControl();
			}
			else
			{
				this.SlowActivate();
			}
			this.SetOcState(2);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000148B7 File Offset: 0x00012AB7
		private void DepersistFromIPropertyBag(UnsafeNativeMethods.IPropertyBag propBag)
		{
			this.iPersistPropBag.Load(propBag, null);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000148C6 File Offset: 0x00012AC6
		private void DepersistFromIStream(UnsafeNativeMethods.IStream istream)
		{
			this.storageType = 0;
			this.iPersistStream.Load(istream);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000148DB File Offset: 0x00012ADB
		private void DepersistFromIStreamInit(UnsafeNativeMethods.IStream istream)
		{
			this.storageType = 1;
			this.iPersistStreamInit.Load(istream);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000148F0 File Offset: 0x00012AF0
		private void DepersistFromIStorage(UnsafeNativeMethods.IStorage storage)
		{
			this.storageType = 2;
			if (storage != null)
			{
				int num = this.iPersistStorage.Load(storage);
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00014918 File Offset: 0x00012B18
		private void DepersistControl()
		{
			this.Freeze(true);
			if (this.ocxState != null)
			{
				switch (this.ocxState.Type)
				{
				case 0:
					try
					{
						this.iPersistStream = (UnsafeNativeMethods.IPersistStream)this.instance;
						this.DepersistFromIStream(this.ocxState.GetStream());
						goto IL_1D5;
					}
					catch (Exception ex)
					{
						goto IL_1D5;
					}
					break;
				case 1:
					break;
				case 2:
					try
					{
						this.iPersistStorage = (UnsafeNativeMethods.IPersistStorage)this.instance;
						this.DepersistFromIStorage(this.ocxState.GetStorage());
						goto IL_1D5;
					}
					catch (Exception ex2)
					{
						goto IL_1D5;
					}
					goto IL_1C5;
				default:
					goto IL_1C5;
				}
				if (this.instance is UnsafeNativeMethods.IPersistStreamInit)
				{
					try
					{
						this.iPersistStreamInit = (UnsafeNativeMethods.IPersistStreamInit)this.instance;
						this.DepersistFromIStreamInit(this.ocxState.GetStream());
					}
					catch (Exception ex3)
					{
					}
					this.GetControlEnabled();
					goto IL_1D5;
				}
				this.ocxState.Type = 0;
				this.DepersistControl();
				return;
				IL_1C5:
				throw new InvalidOperationException(SR.GetString("UnableToInitComponent"));
				IL_1D5:
				if (this.ocxState.GetPropBag() != null)
				{
					try
					{
						this.iPersistPropBag = (UnsafeNativeMethods.IPersistPropertyBag)this.instance;
						this.DepersistFromIPropertyBag(this.ocxState.GetPropBag());
					}
					catch (Exception ex4)
					{
					}
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStreamInit)
			{
				this.iPersistStreamInit = (UnsafeNativeMethods.IPersistStreamInit)this.instance;
				try
				{
					this.storageType = 1;
					this.iPersistStreamInit.InitNew();
				}
				catch (Exception ex5)
				{
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStream)
			{
				this.storageType = 0;
				this.iPersistStream = (UnsafeNativeMethods.IPersistStream)this.instance;
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStorage)
			{
				this.storageType = 2;
				this.ocxState = new AxHost.State(this);
				this.iPersistStorage = (UnsafeNativeMethods.IPersistStorage)this.instance;
				try
				{
					this.iPersistStorage.InitNew(this.ocxState.GetStorage());
				}
				catch (Exception ex6)
				{
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistPropertyBag)
			{
				this.iPersistPropBag = (UnsafeNativeMethods.IPersistPropertyBag)this.instance;
				try
				{
					this.iPersistPropBag.InitNew();
				}
				catch (Exception ex7)
				{
				}
			}
			throw new InvalidOperationException(SR.GetString("UnableToInitComponent"));
		}

		/// <summary>Retrieves a reference to the underlying ActiveX control.</summary>
		/// <returns>An object that represents the ActiveX control.</returns>
		// Token: 0x0600071F RID: 1823 RVA: 0x00014B88 File Offset: 0x00012D88
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object GetOcx()
		{
			return this.instance;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00014B90 File Offset: 0x00012D90
		private object GetOcxCreate()
		{
			if (this.instance == null)
			{
				this.CreateInstance();
				this.RealizeStyles();
				this.AttachInterfaces();
				this.oleSite.OnOcxCreate();
			}
			return this.instance;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00014BC0 File Offset: 0x00012DC0
		private void StartEvents()
		{
			if (!this.axState[AxHost.sinkAttached])
			{
				try
				{
					this.CreateSink();
					this.oleSite.StartEvents();
				}
				catch (Exception ex)
				{
				}
				this.axState[AxHost.sinkAttached] = true;
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00014C18 File Offset: 0x00012E18
		private void StopEvents()
		{
			if (this.axState[AxHost.sinkAttached])
			{
				try
				{
					this.DetachSink();
				}
				catch (Exception ex)
				{
				}
				this.axState[AxHost.sinkAttached] = false;
			}
			this.oleSite.StopEvents();
		}

		/// <summary>Called by the control to prepare it for listening to events.</summary>
		// Token: 0x06000723 RID: 1827 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void CreateSink()
		{
		}

		/// <summary>Called by the control when it stops listening to events.</summary>
		// Token: 0x06000724 RID: 1828 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void DetachSink()
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00014C70 File Offset: 0x00012E70
		private bool CanShowPropertyPages()
		{
			return this.GetOcState() >= 2 && this.GetOcx() is NativeMethods.ISpecifyPropertyPages;
		}

		/// <summary>Determines if the ActiveX control has a property page.</summary>
		/// <returns>
		///     <see langword="true" /> if the ActiveX control has a property page; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000726 RID: 1830 RVA: 0x00014C8C File Offset: 0x00012E8C
		public bool HasPropertyPages()
		{
			if (!this.CanShowPropertyPages())
			{
				return false;
			}
			NativeMethods.ISpecifyPropertyPages specifyPropertyPages = (NativeMethods.ISpecifyPropertyPages)this.GetOcx();
			try
			{
				NativeMethods.tagCAUUID tagCAUUID = new NativeMethods.tagCAUUID();
				try
				{
					specifyPropertyPages.GetPages(tagCAUUID);
					if (tagCAUUID.cElems > 0)
					{
						return true;
					}
				}
				finally
				{
					if (tagCAUUID.pElems != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(tagCAUUID.pElems);
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00014D10 File Offset: 0x00012F10
		private unsafe void ShowPropertyPageForDispid(int dispid, Guid guid)
		{
			try
			{
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this.GetOcx());
				UnsafeNativeMethods.OleCreatePropertyFrameIndirect(new NativeMethods.OCPFIPARAMS
				{
					hwndOwner = ((this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle),
					lpszCaption = base.Name,
					ppUnk = (IntPtr)(&iunknownForObject),
					uuid = (IntPtr)(&guid),
					dispidInitial = dispid
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>Announces to the component change service that the <see cref="T:System.Windows.Forms.AxHost" /> has changed.</summary>
		// Token: 0x06000728 RID: 1832 RVA: 0x00014D9C File Offset: 0x00012F9C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void MakeDirty()
		{
			ISite site = this.Site;
			if (site == null)
			{
				return;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
			if (componentChangeService == null)
			{
				return;
			}
			componentChangeService.OnComponentChanging(this, null);
			componentChangeService.OnComponentChanged(this, null, null, null);
		}

		/// <summary>Displays the property pages associated with the ActiveX control.</summary>
		// Token: 0x06000729 RID: 1833 RVA: 0x00014DE0 File Offset: 0x00012FE0
		public void ShowPropertyPages()
		{
			if (this.ParentInternal == null)
			{
				return;
			}
			if (!this.ParentInternal.IsHandleCreated)
			{
				return;
			}
			this.ShowPropertyPages(this.ParentInternal);
		}

		/// <summary>Displays the property pages associated with the ActiveX control assigned to the specified parent control.</summary>
		/// <param name="control">The parent <see cref="T:System.Windows.Forms.Control" /> of the ActiveX control. </param>
		// Token: 0x0600072A RID: 1834 RVA: 0x00014E08 File Offset: 0x00013008
		public void ShowPropertyPages(Control control)
		{
			try
			{
				if (this.CanShowPropertyPages())
				{
					NativeMethods.ISpecifyPropertyPages specifyPropertyPages = (NativeMethods.ISpecifyPropertyPages)this.GetOcx();
					NativeMethods.tagCAUUID tagCAUUID = new NativeMethods.tagCAUUID();
					try
					{
						specifyPropertyPages.GetPages(tagCAUUID);
						if (tagCAUUID.cElems <= 0)
						{
							return;
						}
					}
					catch
					{
						return;
					}
					IDesignerHost designerHost = null;
					if (this.Site != null)
					{
						designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
					}
					DesignerTransaction designerTransaction = null;
					try
					{
						if (designerHost != null)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("AXEditProperties"));
						}
						string caption = null;
						object ocx = this.GetOcx();
						IntPtr handle = (this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle;
						SafeNativeMethods.OleCreatePropertyFrame(new HandleRef(this, handle), 0, 0, caption, 1, ref ocx, tagCAUUID.cElems, new HandleRef(null, tagCAUUID.pElems), Application.CurrentCulture.LCID, 0, IntPtr.Zero);
					}
					finally
					{
						if (this.oleSite != null)
						{
							((UnsafeNativeMethods.IPropertyNotifySink)this.oleSite).OnChanged(-1);
						}
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
						}
						if (tagCAUUID.pElems != IntPtr.Zero)
						{
							Marshal.FreeCoTaskMem(tagCAUUID.pElems);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00014F7C File Offset: 0x0001317C
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (this.isMaskEdit)
			{
				return base.InitializeDCForWmCtlColor(dc, msg);
			}
			return IntPtr.Zero;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x0600072C RID: 1836 RVA: 0x00014F94 File Offset: 0x00013194
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 83)
			{
				if (msg <= 21)
				{
					if (msg != 2)
					{
						if (msg == 8)
						{
							this.hwndFocus = m.WParam;
							try
							{
								base.WndProc(ref m);
								return;
							}
							finally
							{
								this.hwndFocus = IntPtr.Zero;
							}
							goto IL_F9;
						}
						if (msg - 20 > 1)
						{
							goto IL_1D0;
						}
					}
					else
					{
						if (this.GetOcState() >= 4)
						{
							UnsafeNativeMethods.IOleInPlaceObject inPlaceObject = this.GetInPlaceObject();
							IntPtr handle;
							if (NativeMethods.Succeeded(inPlaceObject.GetWindow(out handle)))
							{
								Application.ParkHandle(new HandleRef(inPlaceObject, handle), DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED);
							}
						}
						bool state = base.GetState(2);
						this.TransitionDownTo(2);
						this.DetachAndForward(ref m);
						if (state != base.GetState(2))
						{
							base.SetState(2, state);
							return;
						}
						return;
					}
				}
				else if (msg != 32 && msg != 43)
				{
					if (msg != 83)
					{
						goto IL_1D0;
					}
					base.WndProc(ref m);
					this.DefWndProc(ref m);
					return;
				}
			}
			else if (msg <= 257)
			{
				if (msg != 123)
				{
					if (msg != 130)
					{
						if (msg != 257)
						{
							goto IL_1D0;
						}
						if (this.axState[AxHost.processingKeyUp])
						{
							return;
						}
						this.axState[AxHost.processingKeyUp] = true;
						try
						{
							if (base.PreProcessControlMessage(ref m) != PreProcessControlState.MessageProcessed)
							{
								this.DefWndProc(ref m);
							}
							return;
						}
						finally
						{
							this.axState[AxHost.processingKeyUp] = false;
						}
					}
					this.DetachAndForward(ref m);
					return;
				}
				this.DefWndProc(ref m);
				return;
			}
			else
			{
				if (msg == 273)
				{
					goto IL_F9;
				}
				switch (msg)
				{
				case 513:
				case 516:
				case 519:
					if (this.IsUserMode())
					{
						base.Focus();
					}
					this.DefWndProc(ref m);
					return;
				case 514:
				case 515:
				case 517:
				case 518:
				case 520:
				case 521:
					break;
				default:
					if (msg != 8277)
					{
						goto IL_1D0;
					}
					break;
				}
			}
			this.DefWndProc(ref m);
			return;
			IL_F9:
			if (!Control.ReflectMessageInternal(m.LParam, ref m))
			{
				this.DefWndProc(ref m);
				return;
			}
			return;
			IL_1D0:
			if (m.Msg == this.REGMSG_MSG)
			{
				m.Result = (IntPtr)123;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000151B0 File Offset: 0x000133B0
		private void DetachAndForward(ref Message m)
		{
			IntPtr handleNoCreate = this.GetHandleNoCreate();
			this.DetachWindow();
			if (handleNoCreate != IntPtr.Zero)
			{
				IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handleNoCreate), -4);
				m.Result = UnsafeNativeMethods.CallWindowProc(windowLong, handleNoCreate, m.Msg, m.WParam, m.LParam);
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00015208 File Offset: 0x00013408
		private void DetachWindow()
		{
			if (base.IsHandleCreated)
			{
				this.OnHandleDestroyed(EventArgs.Empty);
				for (Control control = this; control != null; control = control.ParentInternal)
				{
				}
				base.WindowReleaseHandle();
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001523C File Offset: 0x0001343C
		private void InformOfNewHandle()
		{
			for (Control control = this; control != null; control = control.ParentInternal)
			{
			}
			this.wndprocAddr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -4);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00015270 File Offset: 0x00013470
		private void AttachWindow(IntPtr hwnd)
		{
			if (!this.axState[AxHost.fFakingWindow])
			{
				base.WindowAssignHandle(hwnd, this.axState[AxHost.assignUniqueID]);
			}
			base.UpdateZOrder();
			Size size = base.Size;
			base.UpdateBounds();
			Size extent = this.GetExtent();
			Point location = base.Location;
			if (size.Width < extent.Width || size.Height < extent.Height)
			{
				base.Bounds = new Rectangle(location.X, location.Y, extent.Width, extent.Height);
			}
			else
			{
				Size size2 = this.SetExtent(size.Width, size.Height);
				if (!size2.Equals(size))
				{
					base.Bounds = new Rectangle(location.X, location.Y, size2.Width, size2.Height);
				}
			}
			this.OnHandleCreated(EventArgs.Empty);
			this.InformOfNewHandle();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000731 RID: 1841 RVA: 0x00015372 File Offset: 0x00013572
		protected override void OnHandleCreated(EventArgs e)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			base.SetAcceptDrops(this.AllowDrop);
			base.RaiseCreateHandleEvent(e);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001539E File Offset: 0x0001359E
		private int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x000153AD File Offset: 0x000135AD
		private int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000153C0 File Offset: 0x000135C0
		private bool QuickActivate()
		{
			if (!(this.instance is UnsafeNativeMethods.IQuickActivate))
			{
				return false;
			}
			UnsafeNativeMethods.IQuickActivate quickActivate = (UnsafeNativeMethods.IQuickActivate)this.instance;
			UnsafeNativeMethods.tagQACONTAINER tagQACONTAINER = new UnsafeNativeMethods.tagQACONTAINER();
			UnsafeNativeMethods.tagQACONTROL tagQACONTROL = new UnsafeNativeMethods.tagQACONTROL();
			tagQACONTAINER.pClientSite = this.oleSite;
			tagQACONTAINER.pPropertyNotifySink = this.oleSite;
			tagQACONTAINER.pFont = AxHost.GetIFontFromFont(this.GetParentContainer().parent.Font);
			tagQACONTAINER.dwAppearance = 0;
			tagQACONTAINER.lcid = Application.CurrentCulture.LCID;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				tagQACONTAINER.colorFore = AxHost.GetOleColorFromColor(parentInternal.ForeColor);
				tagQACONTAINER.colorBack = AxHost.GetOleColorFromColor(parentInternal.BackColor);
			}
			else
			{
				tagQACONTAINER.colorFore = AxHost.GetOleColorFromColor(SystemColors.WindowText);
				tagQACONTAINER.colorBack = AxHost.GetOleColorFromColor(SystemColors.Window);
			}
			tagQACONTAINER.dwAmbientFlags = 224;
			if (this.IsUserMode())
			{
				tagQACONTAINER.dwAmbientFlags |= 4;
			}
			try
			{
				quickActivate.QuickActivate(tagQACONTAINER, tagQACONTROL);
			}
			catch (Exception ex)
			{
				this.DisposeAxControl();
				return false;
			}
			this.miscStatusBits = tagQACONTROL.dwMiscStatus;
			this.ParseMiscBits(this.miscStatusBits);
			return true;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000154F4 File Offset: 0x000136F4
		internal override void DisposeAxControls()
		{
			this.axState[AxHost.rejectSelection] = true;
			base.DisposeAxControls();
			this.TransitionDownTo(0);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00015514 File Offset: 0x00013714
		private bool GetControlEnabled()
		{
			bool result;
			try
			{
				result = base.IsHandleCreated;
			}
			catch (Exception ex)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00015540 File Offset: 0x00013740
		internal override bool CanSelectCore()
		{
			return this.GetControlEnabled() && !this.axState[AxHost.rejectSelection] && base.CanSelectCore();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000738 RID: 1848 RVA: 0x00015564 File Offset: 0x00013764
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.TransitionDownTo(0);
				if (this.newParent != null)
				{
					this.newParent.Dispose();
				}
				if (this.oleSite != null)
				{
					this.oleSite.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001559D File Offset: 0x0001379D
		private bool GetSiteOwnsDeactivation()
		{
			return this.axState[AxHost.ownDisposing];
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000155AF File Offset: 0x000137AF
		private void DisposeAxControl()
		{
			if (this.GetParentContainer() != null)
			{
				this.GetParentContainer().RemoveControl(this);
			}
			this.TransitionDownTo(2);
			if (this.GetOcState() == 2)
			{
				this.GetOleObject().SetClientSite(null);
				this.SetOcState(1);
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000155EC File Offset: 0x000137EC
		private void ReleaseAxControl()
		{
			int noComponentChangeEvents = this.NoComponentChangeEvents;
			this.NoComponentChangeEvents = noComponentChangeEvents + 1;
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null)
			{
				containerControl.VisibleChanged -= this.onContainerVisibleChanged;
			}
			try
			{
				if (this.instance != null)
				{
					Marshal.FinalReleaseComObject(this.instance);
					this.instance = null;
					this.iOleInPlaceObject = null;
					this.iOleObject = null;
					this.iOleControl = null;
					this.iOleInPlaceActiveObject = null;
					this.iOleInPlaceActiveObjectExternal = null;
					this.iPerPropertyBrowsing = null;
					this.iCategorizeProperties = null;
					this.iPersistStream = null;
					this.iPersistStreamInit = null;
					this.iPersistStorage = null;
				}
				this.axState[AxHost.checkedIppb] = false;
				this.axState[AxHost.checkedCP] = false;
				this.axState[AxHost.disposed] = true;
				this.freezeCount = 0;
				this.axState[AxHost.sinkAttached] = false;
				this.wndprocAddr = IntPtr.Zero;
				this.SetOcState(0);
			}
			finally
			{
				noComponentChangeEvents = this.NoComponentChangeEvents;
				this.NoComponentChangeEvents = noComponentChangeEvents - 1;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00015704 File Offset: 0x00013904
		private void ParseMiscBits(int bits)
		{
			this.axState[AxHost.fOwnWindow] = ((bits & 1024) != 0 && this.IsUserMode());
			this.axState[AxHost.fSimpleFrame] = ((bits & 65536) != 0);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00015744 File Offset: 0x00013944
		private void SlowActivate()
		{
			bool flag = false;
			if ((this.miscStatusBits & 131072) != 0)
			{
				this.GetOleObject().SetClientSite(this.oleSite);
				flag = true;
			}
			this.DepersistControl();
			if (!flag)
			{
				this.GetOleObject().SetClientSite(this.oleSite);
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00015790 File Offset: 0x00013990
		private static NativeMethods.COMRECT FillInRect(NativeMethods.COMRECT dest, Rectangle source)
		{
			dest.left = source.X;
			dest.top = source.Y;
			dest.right = source.Width + source.X;
			dest.bottom = source.Height + source.Y;
			return dest;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000157E4 File Offset: 0x000139E4
		private AxHost.AxContainer GetParentContainer()
		{
			IntSecurity.GetParent.Demand();
			if (this.container == null)
			{
				this.container = AxHost.AxContainer.FindContainerForControl(this);
			}
			if (this.container == null)
			{
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl == null)
				{
					if (this.newParent == null)
					{
						this.newParent = new ContainerControl();
						this.axContainer = this.newParent.CreateAxContainer();
						this.axContainer.AddControl(this);
					}
					return this.axContainer;
				}
				this.container = containerControl.CreateAxContainer();
				this.container.AddControl(this);
				this.containingControl = containerControl;
			}
			return this.container;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001587D File Offset: 0x00013A7D
		private UnsafeNativeMethods.IOleControl GetOleControl()
		{
			if (this.iOleControl == null)
			{
				this.iOleControl = (UnsafeNativeMethods.IOleControl)this.instance;
			}
			return this.iOleControl;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000158A0 File Offset: 0x00013AA0
		private UnsafeNativeMethods.IOleInPlaceActiveObject GetInPlaceActiveObject()
		{
			if (this.iOleInPlaceActiveObjectExternal != null)
			{
				return this.iOleInPlaceActiveObjectExternal;
			}
			if (this.iOleInPlaceActiveObject == null)
			{
				try
				{
					this.iOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this.instance;
				}
				catch (InvalidCastException ex)
				{
				}
			}
			return this.iOleInPlaceActiveObject;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000158F0 File Offset: 0x00013AF0
		private UnsafeNativeMethods.IOleObject GetOleObject()
		{
			if (this.iOleObject == null)
			{
				this.iOleObject = (UnsafeNativeMethods.IOleObject)this.instance;
			}
			return this.iOleObject;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00015911 File Offset: 0x00013B11
		private UnsafeNativeMethods.IOleInPlaceObject GetInPlaceObject()
		{
			if (this.iOleInPlaceObject == null)
			{
				this.iOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this.instance;
			}
			return this.iOleInPlaceObject;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00015934 File Offset: 0x00013B34
		private NativeMethods.ICategorizeProperties GetCategorizeProperties()
		{
			if (this.iCategorizeProperties == null && !this.axState[AxHost.checkedCP] && this.instance != null)
			{
				this.axState[AxHost.checkedCP] = true;
				if (this.instance is NativeMethods.ICategorizeProperties)
				{
					this.iCategorizeProperties = (NativeMethods.ICategorizeProperties)this.instance;
				}
			}
			return this.iCategorizeProperties;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00015998 File Offset: 0x00013B98
		private NativeMethods.IPerPropertyBrowsing GetPerPropertyBrowsing()
		{
			if (this.iPerPropertyBrowsing == null && !this.axState[AxHost.checkedIppb] && this.instance != null)
			{
				this.axState[AxHost.checkedIppb] = true;
				if (this.instance is NativeMethods.IPerPropertyBrowsing)
				{
					this.iPerPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)this.instance;
				}
			}
			return this.iPerPropertyBrowsing;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000159FC File Offset: 0x00013BFC
		private static object GetPICTDESCFromPicture(Image image)
		{
			Bitmap bitmap = image as Bitmap;
			if (bitmap != null)
			{
				return new NativeMethods.PICTDESCbmp(bitmap);
			}
			Metafile metafile = image as Metafile;
			if (metafile != null)
			{
				return new NativeMethods.PICTDESCemf(metafile);
			}
			throw new ArgumentException(SR.GetString("AXUnknownImage"), "image");
		}

		/// <summary>Returns an OLE <see langword="IPicture" /> object corresponding to the specified <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to convert.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the OLE <see langword="IPicture" /> object.</returns>
		// Token: 0x06000747 RID: 1863 RVA: 0x00015A40 File Offset: 0x00013C40
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureFromPicture(Image image)
		{
			if (image == null)
			{
				return null;
			}
			object pictdescfromPicture = AxHost.GetPICTDESCFromPicture(image);
			return UnsafeNativeMethods.OleCreateIPictureIndirect(pictdescfromPicture, ref AxHost.ipicture_Guid, true);
		}

		/// <summary>Returns an OLE <see langword="IPicture" /> object corresponding to the specified <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <param name="cursor">A <see cref="T:System.Windows.Forms.Cursor" />, which is an image that represents the Windows mouse pointer.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the OLE <see langword="IPicture" /> object.</returns>
		// Token: 0x06000748 RID: 1864 RVA: 0x00015A68 File Offset: 0x00013C68
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureFromCursor(Cursor cursor)
		{
			if (cursor == null)
			{
				return null;
			}
			NativeMethods.PICTDESCicon pictdesc = new NativeMethods.PICTDESCicon(Icon.FromHandle(cursor.Handle));
			return UnsafeNativeMethods.OleCreateIPictureIndirect(pictdesc, ref AxHost.ipicture_Guid, true);
		}

		/// <summary>Returns an OLE <see langword="IPictureDisp" /> object corresponding to the specified <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to convert.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the OLE <see langword="IPictureDisp" /> object.</returns>
		// Token: 0x06000749 RID: 1865 RVA: 0x00015AA0 File Offset: 0x00013CA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureDispFromPicture(Image image)
		{
			if (image == null)
			{
				return null;
			}
			object pictdescfromPicture = AxHost.GetPICTDESCFromPicture(image);
			return UnsafeNativeMethods.OleCreateIPictureDispIndirect(pictdescfromPicture, ref AxHost.ipictureDisp_Guid, true);
		}

		/// <summary>Returns an <see cref="T:System.Drawing.Image" /> corresponding to the specified OLE <see langword="IPicture" /> object.</summary>
		/// <param name="picture">The <see langword="IPicture" /> to convert.</param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> representing the <see langword="IPicture" />. </returns>
		// Token: 0x0600074A RID: 1866 RVA: 0x00015AC8 File Offset: 0x00013CC8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Image GetPictureFromIPicture(object picture)
		{
			if (picture == null)
			{
				return null;
			}
			IntPtr paletteHandle = IntPtr.Zero;
			UnsafeNativeMethods.IPicture picture2 = (UnsafeNativeMethods.IPicture)picture;
			int pictureType = (int)picture2.GetPictureType();
			if (pictureType == 1)
			{
				try
				{
					paletteHandle = picture2.GetHPal();
				}
				catch (COMException)
				{
				}
			}
			return AxHost.GetPictureFromParams(picture2, picture2.GetHandle(), pictureType, paletteHandle, picture2.GetWidth(), picture2.GetHeight());
		}

		/// <summary>Returns an <see cref="T:System.Drawing.Image" /> corresponding to the specified OLE <see langword="IPictureDisp" /> object.</summary>
		/// <param name="picture">The <see langword="IPictureDisp" /> to convert.</param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> representing the <see langword="IPictureDisp" />. </returns>
		// Token: 0x0600074B RID: 1867 RVA: 0x00015B28 File Offset: 0x00013D28
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Image GetPictureFromIPictureDisp(object picture)
		{
			if (picture == null)
			{
				return null;
			}
			IntPtr paletteHandle = IntPtr.Zero;
			UnsafeNativeMethods.IPictureDisp pictureDisp = (UnsafeNativeMethods.IPictureDisp)picture;
			int pictureType = (int)pictureDisp.PictureType;
			if (pictureType == 1)
			{
				try
				{
					paletteHandle = pictureDisp.HPal;
				}
				catch (COMException)
				{
				}
			}
			return AxHost.GetPictureFromParams(pictureDisp, pictureDisp.Handle, pictureType, paletteHandle, pictureDisp.Width, pictureDisp.Height);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00015B88 File Offset: 0x00013D88
		private static Image GetPictureFromParams(object pict, IntPtr handle, int type, IntPtr paletteHandle, int width, int height)
		{
			switch (type)
			{
			case -1:
				return null;
			case 0:
				return null;
			case 1:
				return Image.FromHbitmap(handle, paletteHandle);
			case 2:
				return (Image)new Metafile(handle, new WmfPlaceableFileHeader
				{
					BboxRight = (short)width,
					BboxBottom = (short)height
				}, false).Clone();
			case 3:
				return (Image)Icon.FromHandle(handle).Clone();
			case 4:
				return (Image)new Metafile(handle, false).Clone();
			default:
				throw new ArgumentException(SR.GetString("AXUnknownImage"), "type");
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00015C28 File Offset: 0x00013E28
		private static NativeMethods.FONTDESC GetFONTDESCFromFont(Font font)
		{
			NativeMethods.FONTDESC fontdesc = null;
			if (AxHost.fontTable == null)
			{
				AxHost.fontTable = new Hashtable();
			}
			else
			{
				fontdesc = (NativeMethods.FONTDESC)AxHost.fontTable[font];
			}
			if (fontdesc == null)
			{
				fontdesc = new NativeMethods.FONTDESC();
				fontdesc.lpstrName = font.Name;
				fontdesc.cySize = (long)(font.SizeInPoints * 10000f);
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				font.ToLogFont(logfont);
				fontdesc.sWeight = (short)logfont.lfWeight;
				fontdesc.sCharset = (short)logfont.lfCharSet;
				fontdesc.fItalic = font.Italic;
				fontdesc.fUnderline = font.Underline;
				fontdesc.fStrikethrough = font.Strikeout;
				AxHost.fontTable[font] = fontdesc;
			}
			return fontdesc;
		}

		/// <summary>Returns a <see cref="T:System.Drawing.Color" /> structure that corresponds to the specified OLE color value.</summary>
		/// <param name="color">The OLE color value to translate.</param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that represents the translated OLE color value.</returns>
		// Token: 0x0600074E RID: 1870 RVA: 0x00015CDA File Offset: 0x00013EDA
		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Color GetColorFromOleColor(uint color)
		{
			return ColorTranslator.FromOle((int)color);
		}

		/// <summary>Returns an OLE color value that corresponds to the specified <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="color">The <see cref="T:System.Drawing.Color" /> structure to translate.</param>
		/// <returns>The OLE color value that represents the translated <see cref="T:System.Drawing.Color" /> structure.</returns>
		// Token: 0x0600074F RID: 1871 RVA: 0x00015CE2 File Offset: 0x00013EE2
		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static uint GetOleColorFromColor(Color color)
		{
			return (uint)ColorTranslator.ToOle(color);
		}

		/// <summary>Returns an OLE IFont object created from the specified <see cref="T:System.Drawing.Font" /> object.</summary>
		/// <param name="font">The font to create an IFont object from.</param>
		/// <returns>The IFont object created from the specified font, or <see langword="null" /> if <paramref name="font" /> is <see langword="null" /> or the IFont could not be created.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Font.Unit" /> property value is not <see cref="F:System.Drawing.GraphicsUnit.Point" />.</exception>
		// Token: 0x06000750 RID: 1872 RVA: 0x00015CEC File Offset: 0x00013EEC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIFontFromFont(Font font)
		{
			if (font == null)
			{
				return null;
			}
			if (font.Unit != GraphicsUnit.Point)
			{
				throw new ArgumentException(SR.GetString("AXFontUnitNotPoint"), "font");
			}
			object result;
			try
			{
				result = UnsafeNativeMethods.OleCreateIFontIndirect(AxHost.GetFONTDESCFromFont(font), ref AxHost.ifont_Guid);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>Returns a <see cref="T:System.Drawing.Font" /> created from the specified OLE IFont object.</summary>
		/// <param name="font">The IFont to create a <see cref="T:System.Drawing.Font" /> from.</param>
		/// <returns>The <see cref="T:System.Drawing.Font" /> created from the specified IFont, <see cref="P:System.Windows.Forms.Control.DefaultFont" /> if the font could not be created, or <see langword="null" /> if <paramref name="font" /> is <see langword="null" />.</returns>
		// Token: 0x06000751 RID: 1873 RVA: 0x00015D48 File Offset: 0x00013F48
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Font GetFontFromIFont(object font)
		{
			if (font == null)
			{
				return null;
			}
			UnsafeNativeMethods.IFont font2 = (UnsafeNativeMethods.IFont)font;
			Font result;
			try
			{
				Font font3 = Font.FromHfont(font2.GetHFont());
				if (font3.Unit != GraphicsUnit.Point)
				{
					font3 = new Font(font3.Name, font3.SizeInPoints, font3.Style, GraphicsUnit.Point, font3.GdiCharSet, font3.GdiVerticalFont);
				}
				result = font3;
			}
			catch (Exception ex)
			{
				result = Control.DefaultFont;
			}
			return result;
		}

		/// <summary>Returns an OLE IFontDisp object created from the specified <see cref="T:System.Drawing.Font" /> object.</summary>
		/// <param name="font">The font to create an IFontDisp object from.</param>
		/// <returns>The IFontDisp object created from the specified font or <see langword="null" /> if <paramref name="font" /> is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Font.Unit" /> property value is not <see cref="F:System.Drawing.GraphicsUnit.Point" />.</exception>
		// Token: 0x06000752 RID: 1874 RVA: 0x00015DBC File Offset: 0x00013FBC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIFontDispFromFont(Font font)
		{
			if (font == null)
			{
				return null;
			}
			if (font.Unit != GraphicsUnit.Point)
			{
				throw new ArgumentException(SR.GetString("AXFontUnitNotPoint"), "font");
			}
			return SafeNativeMethods.OleCreateIFontDispIndirect(AxHost.GetFONTDESCFromFont(font), ref AxHost.ifontDisp_Guid);
		}

		/// <summary>Returns a <see cref="T:System.Drawing.Font" /> created from the specified OLE IFontDisp object.</summary>
		/// <param name="font">The IFontDisp to create a <see cref="T:System.Drawing.Font" /> from.</param>
		/// <returns>The <see cref="T:System.Drawing.Font" /> created from the specified IFontDisp, <see cref="P:System.Windows.Forms.Control.DefaultFont" /> if the font could not be created, or <see langword="null" /> if <paramref name="font" /> is <see langword="null" />.</returns>
		// Token: 0x06000753 RID: 1875 RVA: 0x00015E00 File Offset: 0x00014000
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Font GetFontFromIFontDisp(object font)
		{
			if (font == null)
			{
				return null;
			}
			UnsafeNativeMethods.IFont font2 = font as UnsafeNativeMethods.IFont;
			if (font2 != null)
			{
				return AxHost.GetFontFromIFont(font2);
			}
			SafeNativeMethods.IFontDisp fontDisp = (SafeNativeMethods.IFontDisp)font;
			FontStyle fontStyle = FontStyle.Regular;
			Font result;
			try
			{
				if (fontDisp.Bold)
				{
					fontStyle |= FontStyle.Bold;
				}
				if (fontDisp.Italic)
				{
					fontStyle |= FontStyle.Italic;
				}
				if (fontDisp.Underline)
				{
					fontStyle |= FontStyle.Underline;
				}
				if (fontDisp.Strikethrough)
				{
					fontStyle |= FontStyle.Strikeout;
				}
				if (fontDisp.Weight >= 700)
				{
					fontStyle |= FontStyle.Bold;
				}
				Font font3 = new Font(fontDisp.Name, (float)fontDisp.Size / 10000f, fontStyle, GraphicsUnit.Point, (byte)fontDisp.Charset);
				result = font3;
			}
			catch (Exception ex)
			{
				result = Control.DefaultFont;
			}
			return result;
		}

		/// <summary>Returns an OLE Automation date that corresponds to the specified <see cref="T:System.DateTime" /> structure. </summary>
		/// <param name="time">The <see cref="T:System.DateTime" /> structure to translate.</param>
		/// <returns>A double-precision floating-point number that contains an OLE Automation date equivalent to specified <paramref name="time" /> value.</returns>
		/// <exception cref="T:System.OverflowException">The value of this instance cannot be represented as an OLE Automation Date. </exception>
		// Token: 0x06000754 RID: 1876 RVA: 0x00015EB4 File Offset: 0x000140B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static double GetOADateFromTime(DateTime time)
		{
			return time.ToOADate();
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> structure that corresponds to the specified OLE Automation date.</summary>
		/// <param name="date">The OLE Automate date to translate.</param>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the same date and time as <paramref name="date" />.</returns>
		/// <exception cref="T:System.ArgumentException">The date is not a valid OLE Automation Date value. </exception>
		// Token: 0x06000755 RID: 1877 RVA: 0x00015EBD File Offset: 0x000140BD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static DateTime GetTimeFromOADate(double date)
		{
			return DateTime.FromOADate(date);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00015EC8 File Offset: 0x000140C8
		private int Convert2int(object o, bool xDirection)
		{
			o = ((Array)o).GetValue(0);
			if (o.GetType() == typeof(float))
			{
				return AxHost.Twip2Pixel(Convert.ToDouble(o, CultureInfo.InvariantCulture), xDirection);
			}
			return Convert.ToInt32(o, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00015F17 File Offset: 0x00014117
		private short Convert2short(object o)
		{
			o = ((Array)o).GetValue(0);
			return Convert.ToInt16(o, CultureInfo.InvariantCulture);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseMove" /> event using the specified objects.</summary>
		/// <param name="o1">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="o2">Not used.</param>
		/// <param name="o3">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="o4">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x06000758 RID: 1880 RVA: 0x00015F32 File Offset: 0x00014132
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseMove(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseMove" /> event using the specified single-precision floating-point numbers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x06000759 RID: 1881 RVA: 0x00015F59 File Offset: 0x00014159
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseMove(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseMove" /> event using the specified 32-bit signed integers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075A RID: 1882 RVA: 0x00015F74 File Offset: 0x00014174
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(short button, short shift, int x, int y)
		{
			base.OnMouseMove(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseUp" /> event using the specified objects.</summary>
		/// <param name="o1">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="o2">Not used.</param>
		/// <param name="o3">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="o4">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075B RID: 1883 RVA: 0x00015F8A File Offset: 0x0001418A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseUp(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseUp" /> event using the specified single-precision floating-point numbers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075C RID: 1884 RVA: 0x00015FB1 File Offset: 0x000141B1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseUp(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseUp" /> event using the specified 32-bit signed integers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075D RID: 1885 RVA: 0x00015FCC File Offset: 0x000141CC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(short button, short shift, int x, int y)
		{
			base.OnMouseUp(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseDown" /> event using the specified objects.</summary>
		/// <param name="o1">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="o2">Not used.</param>
		/// <param name="o3">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="o4">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075E RID: 1886 RVA: 0x00015FE2 File Offset: 0x000141E2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseDown(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseDown" /> event using the specified single-precision floating-point numbers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x0600075F RID: 1887 RVA: 0x00016009 File Offset: 0x00014209
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseDown(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AxHost.MouseDown" /> event using the specified 32-bit signed integers.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="shift">Not used.</param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		// Token: 0x06000760 RID: 1888 RVA: 0x00016024 File Offset: 0x00014224
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(short button, short shift, int x, int y)
		{
			base.OnMouseDown(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		// Token: 0x04000571 RID: 1393
		private static TraceSwitch AxHTraceSwitch = new TraceSwitch("AxHTrace", "ActiveX handle tracing");

		// Token: 0x04000572 RID: 1394
		private static TraceSwitch AxPropTraceSwitch = new TraceSwitch("AxPropTrace", "ActiveX property tracing");

		// Token: 0x04000573 RID: 1395
		private static TraceSwitch AxHostSwitch = new TraceSwitch("AxHost", "ActiveX host creation");

		// Token: 0x04000574 RID: 1396
		private static BooleanSwitch AxIgnoreTMSwitch = new BooleanSwitch("AxIgnoreTM", "ActiveX switch to ignore thread models");

		// Token: 0x04000575 RID: 1397
		private static BooleanSwitch AxAlwaysSaveSwitch = new BooleanSwitch("AxAlwaysSave", "ActiveX to save all controls regardless of their IsDirty function return value");

		// Token: 0x04000576 RID: 1398
		private static COMException E_NOTIMPL = new COMException(SR.GetString("AXNotImplemented"), -2147483647);

		// Token: 0x04000577 RID: 1399
		private static COMException E_INVALIDARG = new COMException(SR.GetString("AXInvalidArgument"), -2147024809);

		// Token: 0x04000578 RID: 1400
		private static COMException E_FAIL = new COMException(SR.GetString("AXUnknownError"), -2147467259);

		// Token: 0x04000579 RID: 1401
		private static COMException E_NOINTERFACE = new COMException(SR.GetString("AxInterfaceNotSupported"), -2147467262);

		// Token: 0x0400057A RID: 1402
		private const int INPROC_SERVER = 1;

		// Token: 0x0400057B RID: 1403
		private const int OC_PASSIVE = 0;

		// Token: 0x0400057C RID: 1404
		private const int OC_LOADED = 1;

		// Token: 0x0400057D RID: 1405
		private const int OC_RUNNING = 2;

		// Token: 0x0400057E RID: 1406
		private const int OC_INPLACE = 4;

		// Token: 0x0400057F RID: 1407
		private const int OC_UIACTIVE = 8;

		// Token: 0x04000580 RID: 1408
		private const int OC_OPEN = 16;

		// Token: 0x04000581 RID: 1409
		private const int EDITM_NONE = 0;

		// Token: 0x04000582 RID: 1410
		private const int EDITM_OBJECT = 1;

		// Token: 0x04000583 RID: 1411
		private const int EDITM_HOST = 2;

		// Token: 0x04000584 RID: 1412
		private const int STG_UNKNOWN = -1;

		// Token: 0x04000585 RID: 1413
		private const int STG_STREAM = 0;

		// Token: 0x04000586 RID: 1414
		private const int STG_STREAMINIT = 1;

		// Token: 0x04000587 RID: 1415
		private const int STG_STORAGE = 2;

		// Token: 0x04000588 RID: 1416
		private const int OLEIVERB_SHOW = -1;

		// Token: 0x04000589 RID: 1417
		private const int OLEIVERB_HIDE = -3;

		// Token: 0x0400058A RID: 1418
		private const int OLEIVERB_UIACTIVATE = -4;

		// Token: 0x0400058B RID: 1419
		private const int OLEIVERB_INPLACEACTIVATE = -5;

		// Token: 0x0400058C RID: 1420
		private const int OLEIVERB_PROPERTIES = -7;

		// Token: 0x0400058D RID: 1421
		private const int OLEIVERB_PRIMARY = 0;

		// Token: 0x0400058E RID: 1422
		private readonly int REGMSG_MSG = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_subclassCheck");

		// Token: 0x0400058F RID: 1423
		private const int REGMSG_RETVAL = 123;

		// Token: 0x04000590 RID: 1424
		private static int logPixelsX = -1;

		// Token: 0x04000591 RID: 1425
		private static int logPixelsY = -1;

		// Token: 0x04000592 RID: 1426
		private static Guid icf2_Guid = typeof(UnsafeNativeMethods.IClassFactory2).GUID;

		// Token: 0x04000593 RID: 1427
		private static Guid ifont_Guid = typeof(UnsafeNativeMethods.IFont).GUID;

		// Token: 0x04000594 RID: 1428
		private static Guid ifontDisp_Guid = typeof(SafeNativeMethods.IFontDisp).GUID;

		// Token: 0x04000595 RID: 1429
		private static Guid ipicture_Guid = typeof(UnsafeNativeMethods.IPicture).GUID;

		// Token: 0x04000596 RID: 1430
		private static Guid ipictureDisp_Guid = typeof(UnsafeNativeMethods.IPictureDisp).GUID;

		// Token: 0x04000597 RID: 1431
		private static Guid ivbformat_Guid = typeof(UnsafeNativeMethods.IVBFormat).GUID;

		// Token: 0x04000598 RID: 1432
		private static Guid ioleobject_Guid = typeof(UnsafeNativeMethods.IOleObject).GUID;

		// Token: 0x04000599 RID: 1433
		private static Guid dataSource_Guid = new Guid("{7C0FFAB3-CD84-11D0-949A-00A0C91110ED}");

		// Token: 0x0400059A RID: 1434
		private static Guid windowsMediaPlayer_Clsid = new Guid("{22d6f312-b0f6-11d0-94ab-0080c74c7e95}");

		// Token: 0x0400059B RID: 1435
		private static Guid comctlImageCombo_Clsid = new Guid("{a98a24c0-b06f-3684-8c12-c52ae341e0bc}");

		// Token: 0x0400059C RID: 1436
		private static Guid maskEdit_Clsid = new Guid("{c932ba85-4374-101b-a56c-00aa003668dc}");

		// Token: 0x0400059D RID: 1437
		private static Hashtable fontTable;

		// Token: 0x0400059E RID: 1438
		private static readonly int ocxStateSet = BitVector32.CreateMask();

		// Token: 0x0400059F RID: 1439
		private static readonly int editorRefresh = BitVector32.CreateMask(AxHost.ocxStateSet);

		// Token: 0x040005A0 RID: 1440
		private static readonly int listeningToIdle = BitVector32.CreateMask(AxHost.editorRefresh);

		// Token: 0x040005A1 RID: 1441
		private static readonly int refreshProperties = BitVector32.CreateMask(AxHost.listeningToIdle);

		// Token: 0x040005A2 RID: 1442
		private static readonly int checkedIppb = BitVector32.CreateMask(AxHost.refreshProperties);

		// Token: 0x040005A3 RID: 1443
		private static readonly int checkedCP = BitVector32.CreateMask(AxHost.checkedIppb);

		// Token: 0x040005A4 RID: 1444
		private static readonly int fNeedOwnWindow = BitVector32.CreateMask(AxHost.checkedCP);

		// Token: 0x040005A5 RID: 1445
		private static readonly int fOwnWindow = BitVector32.CreateMask(AxHost.fNeedOwnWindow);

		// Token: 0x040005A6 RID: 1446
		private static readonly int fSimpleFrame = BitVector32.CreateMask(AxHost.fOwnWindow);

		// Token: 0x040005A7 RID: 1447
		private static readonly int fFakingWindow = BitVector32.CreateMask(AxHost.fSimpleFrame);

		// Token: 0x040005A8 RID: 1448
		private static readonly int rejectSelection = BitVector32.CreateMask(AxHost.fFakingWindow);

		// Token: 0x040005A9 RID: 1449
		private static readonly int ownDisposing = BitVector32.CreateMask(AxHost.rejectSelection);

		// Token: 0x040005AA RID: 1450
		private static readonly int sinkAttached = BitVector32.CreateMask(AxHost.ownDisposing);

		// Token: 0x040005AB RID: 1451
		private static readonly int disposed = BitVector32.CreateMask(AxHost.sinkAttached);

		// Token: 0x040005AC RID: 1452
		private static readonly int manualUpdate = BitVector32.CreateMask(AxHost.disposed);

		// Token: 0x040005AD RID: 1453
		private static readonly int addedSelectionHandler = BitVector32.CreateMask(AxHost.manualUpdate);

		// Token: 0x040005AE RID: 1454
		private static readonly int valueChanged = BitVector32.CreateMask(AxHost.addedSelectionHandler);

		// Token: 0x040005AF RID: 1455
		private static readonly int handlePosRectChanged = BitVector32.CreateMask(AxHost.valueChanged);

		// Token: 0x040005B0 RID: 1456
		private static readonly int siteProcessedInputKey = BitVector32.CreateMask(AxHost.handlePosRectChanged);

		// Token: 0x040005B1 RID: 1457
		private static readonly int needLicenseKey = BitVector32.CreateMask(AxHost.siteProcessedInputKey);

		// Token: 0x040005B2 RID: 1458
		private static readonly int inTransition = BitVector32.CreateMask(AxHost.needLicenseKey);

		// Token: 0x040005B3 RID: 1459
		private static readonly int processingKeyUp = BitVector32.CreateMask(AxHost.inTransition);

		// Token: 0x040005B4 RID: 1460
		private static readonly int assignUniqueID = BitVector32.CreateMask(AxHost.processingKeyUp);

		// Token: 0x040005B5 RID: 1461
		private static readonly int renameEventHooked = BitVector32.CreateMask(AxHost.assignUniqueID);

		// Token: 0x040005B6 RID: 1462
		private BitVector32 axState;

		// Token: 0x040005B7 RID: 1463
		private int storageType = -1;

		// Token: 0x040005B8 RID: 1464
		private int ocState;

		// Token: 0x040005B9 RID: 1465
		private int miscStatusBits;

		// Token: 0x040005BA RID: 1466
		private int freezeCount;

		// Token: 0x040005BB RID: 1467
		private int flags;

		// Token: 0x040005BC RID: 1468
		private int selectionStyle;

		// Token: 0x040005BD RID: 1469
		private int editMode;

		// Token: 0x040005BE RID: 1470
		private int noComponentChange;

		// Token: 0x040005BF RID: 1471
		private IntPtr wndprocAddr = IntPtr.Zero;

		// Token: 0x040005C0 RID: 1472
		private Guid clsid;

		// Token: 0x040005C1 RID: 1473
		private string text = "";

		// Token: 0x040005C2 RID: 1474
		private string licenseKey;

		// Token: 0x040005C3 RID: 1475
		private readonly AxHost.OleInterfaces oleSite;

		// Token: 0x040005C4 RID: 1476
		private AxHost.AxComponentEditor editor;

		// Token: 0x040005C5 RID: 1477
		private AxHost.AxContainer container;

		// Token: 0x040005C6 RID: 1478
		private ContainerControl containingControl;

		// Token: 0x040005C7 RID: 1479
		private ContainerControl newParent;

		// Token: 0x040005C8 RID: 1480
		private AxHost.AxContainer axContainer;

		// Token: 0x040005C9 RID: 1481
		private AxHost.State ocxState;

		// Token: 0x040005CA RID: 1482
		private IntPtr hwndFocus = IntPtr.Zero;

		// Token: 0x040005CB RID: 1483
		private Hashtable properties;

		// Token: 0x040005CC RID: 1484
		private Hashtable propertyInfos;

		// Token: 0x040005CD RID: 1485
		private PropertyDescriptorCollection propsStash;

		// Token: 0x040005CE RID: 1486
		private Attribute[] attribsStash;

		// Token: 0x040005CF RID: 1487
		private object instance;

		// Token: 0x040005D0 RID: 1488
		private UnsafeNativeMethods.IOleInPlaceObject iOleInPlaceObject;

		// Token: 0x040005D1 RID: 1489
		private UnsafeNativeMethods.IOleObject iOleObject;

		// Token: 0x040005D2 RID: 1490
		private UnsafeNativeMethods.IOleControl iOleControl;

		// Token: 0x040005D3 RID: 1491
		private UnsafeNativeMethods.IOleInPlaceActiveObject iOleInPlaceActiveObject;

		// Token: 0x040005D4 RID: 1492
		private UnsafeNativeMethods.IOleInPlaceActiveObject iOleInPlaceActiveObjectExternal;

		// Token: 0x040005D5 RID: 1493
		private NativeMethods.IPerPropertyBrowsing iPerPropertyBrowsing;

		// Token: 0x040005D6 RID: 1494
		private NativeMethods.ICategorizeProperties iCategorizeProperties;

		// Token: 0x040005D7 RID: 1495
		private UnsafeNativeMethods.IPersistPropertyBag iPersistPropBag;

		// Token: 0x040005D8 RID: 1496
		private UnsafeNativeMethods.IPersistStream iPersistStream;

		// Token: 0x040005D9 RID: 1497
		private UnsafeNativeMethods.IPersistStreamInit iPersistStreamInit;

		// Token: 0x040005DA RID: 1498
		private UnsafeNativeMethods.IPersistStorage iPersistStorage;

		// Token: 0x040005DB RID: 1499
		private AxHost.AboutBoxDelegate aboutBoxDelegate;

		// Token: 0x040005DC RID: 1500
		private EventHandler selectionChangeHandler;

		// Token: 0x040005DD RID: 1501
		private bool isMaskEdit;

		// Token: 0x040005DE RID: 1502
		private bool ignoreDialogKeys;

		// Token: 0x040005DF RID: 1503
		private EventHandler onContainerVisibleChanged;

		// Token: 0x040005E0 RID: 1504
		private static CategoryAttribute[] categoryNames = new CategoryAttribute[]
		{
			null,
			new WinCategoryAttribute("Default"),
			new WinCategoryAttribute("Default"),
			new WinCategoryAttribute("Font"),
			new WinCategoryAttribute("Layout"),
			new WinCategoryAttribute("Appearance"),
			new WinCategoryAttribute("Behavior"),
			new WinCategoryAttribute("Data"),
			new WinCategoryAttribute("List"),
			new WinCategoryAttribute("Text"),
			new WinCategoryAttribute("Scale"),
			new WinCategoryAttribute("DDE")
		};

		// Token: 0x040005E1 RID: 1505
		private Hashtable objectDefinedCategoryNames;

		// Token: 0x040005E2 RID: 1506
		private const int HMperInch = 2540;

		// Token: 0x0200054B RID: 1355
		internal class AxFlags
		{
			// Token: 0x0400379F RID: 14239
			internal const int PreventEditMode = 1;

			// Token: 0x040037A0 RID: 14240
			internal const int IncludePropertiesVerb = 2;

			// Token: 0x040037A1 RID: 14241
			internal const int IgnoreThreadModel = 268435456;
		}

		/// <summary>Specifies the CLSID of an ActiveX control hosted by an <see cref="T:System.Windows.Forms.AxHost" /> control.</summary>
		// Token: 0x0200054C RID: 1356
		[AttributeUsage(AttributeTargets.Class, Inherited = false)]
		public sealed class ClsidAttribute : Attribute
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.ClsidAttribute" /> class. </summary>
			/// <param name="clsid">The CLSID of the ActiveX control.</param>
			// Token: 0x06005564 RID: 21860 RVA: 0x00166C4C File Offset: 0x00164E4C
			public ClsidAttribute(string clsid)
			{
				this.val = clsid;
			}

			/// <summary>The CLSID of the ActiveX control.</summary>
			/// <returns>The CLSID of the ActiveX control.</returns>
			// Token: 0x1700146A RID: 5226
			// (get) Token: 0x06005565 RID: 21861 RVA: 0x00166C5B File Offset: 0x00164E5B
			public string Value
			{
				get
				{
					return this.val;
				}
			}

			// Token: 0x040037A2 RID: 14242
			private string val;
		}

		/// <summary>Specifies a date and time associated with the type library of an ActiveX control hosted by an <see cref="T:System.Windows.Forms.AxHost" /> control.</summary>
		// Token: 0x0200054D RID: 1357
		[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
		public sealed class TypeLibraryTimeStampAttribute : Attribute
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.TypeLibraryTimeStampAttribute" /> class. </summary>
			/// <param name="timestamp">A <see cref="T:System.DateTime" /> value representing the date and time to associate with the type library.</param>
			// Token: 0x06005566 RID: 21862 RVA: 0x00166C63 File Offset: 0x00164E63
			public TypeLibraryTimeStampAttribute(string timestamp)
			{
				this.val = DateTime.Parse(timestamp, CultureInfo.InvariantCulture);
			}

			/// <summary>The date and time to associate with the type library.</summary>
			/// <returns>A <see cref="T:System.DateTime" /> value representing the date and time to associate with the type library.</returns>
			// Token: 0x1700146B RID: 5227
			// (get) Token: 0x06005567 RID: 21863 RVA: 0x00166C7C File Offset: 0x00164E7C
			public DateTime Value
			{
				get
				{
					return this.val;
				}
			}

			// Token: 0x040037A3 RID: 14243
			private DateTime val;
		}

		/// <summary>Connects an ActiveX control to a client that handles the control’s events.</summary>
		// Token: 0x0200054E RID: 1358
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class ConnectionPointCookie
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.ConnectionPointCookie" /> class.</summary>
			/// <param name="source">A connectable object that contains connection points.</param>
			/// <param name="sink">The client's sink which receives outgoing calls from the connection point.</param>
			/// <param name="eventInterface">The outgoing interface whose connection point object is being requested.</param>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="source" /> does not implement <paramref name="eventInterface" />.</exception>
			/// <exception cref="T:System.InvalidCastException">
			///         <paramref name="sink" /> does not implement <paramref name="eventInterface" />.-or-
			///         <paramref name="source" /> does not implement <see cref="T:System.Runtime.InteropServices.ComTypes.IConnectionPointContainer" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The connection point has already reached its limit of connections and cannot accept any more.</exception>
			// Token: 0x06005568 RID: 21864 RVA: 0x00166C84 File Offset: 0x00164E84
			public ConnectionPointCookie(object source, object sink, Type eventInterface) : this(source, sink, eventInterface, true)
			{
			}

			// Token: 0x06005569 RID: 21865 RVA: 0x00166C90 File Offset: 0x00164E90
			internal ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException)
			{
				if (source is UnsafeNativeMethods.IConnectionPointContainer)
				{
					UnsafeNativeMethods.IConnectionPointContainer connectionPointContainer = (UnsafeNativeMethods.IConnectionPointContainer)source;
					try
					{
						Guid guid = eventInterface.GUID;
						if (connectionPointContainer.FindConnectionPoint(ref guid, out this.connectionPoint) != 0)
						{
							this.connectionPoint = null;
						}
					}
					catch
					{
						this.connectionPoint = null;
					}
					if (this.connectionPoint == null)
					{
						if (throwException)
						{
							throw new ArgumentException(SR.GetString("AXNoEventInterface", new object[]
							{
								eventInterface.Name
							}));
						}
					}
					else if (sink == null || !eventInterface.IsInstanceOfType(sink))
					{
						if (throwException)
						{
							throw new InvalidCastException(SR.GetString("AXNoSinkImplementation", new object[]
							{
								eventInterface.Name
							}));
						}
					}
					else
					{
						int num = this.connectionPoint.Advise(sink, ref this.cookie);
						if (num == 0)
						{
							this.threadId = Thread.CurrentThread.ManagedThreadId;
						}
						else
						{
							this.cookie = 0;
							Marshal.ReleaseComObject(this.connectionPoint);
							this.connectionPoint = null;
							if (throwException)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, SR.GetString("AXNoSinkAdvise", new object[]
								{
									eventInterface.Name
								}), new object[]
								{
									num
								}));
							}
						}
					}
				}
				else if (throwException)
				{
					throw new InvalidCastException(SR.GetString("AXNoConnectionPointContainer"));
				}
				if (this.connectionPoint == null || this.cookie == 0)
				{
					if (this.connectionPoint != null)
					{
						Marshal.ReleaseComObject(this.connectionPoint);
					}
					if (throwException)
					{
						throw new ArgumentException(SR.GetString("AXNoConnectionPoint", new object[]
						{
							eventInterface.Name
						}));
					}
				}
			}

			/// <summary>Disconnects the ActiveX control from the client.</summary>
			// Token: 0x0600556A RID: 21866 RVA: 0x00166E28 File Offset: 0x00165028
			public void Disconnect()
			{
				if (this.connectionPoint != null && this.cookie != 0)
				{
					try
					{
						this.connectionPoint.Unadvise(this.cookie);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					finally
					{
						this.cookie = 0;
					}
					try
					{
						Marshal.ReleaseComObject(this.connectionPoint);
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsCriticalException(ex2))
						{
							throw;
						}
					}
					finally
					{
						this.connectionPoint = null;
					}
				}
			}

			/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.AxHost.ConnectionPointCookie" /> class.</summary>
			// Token: 0x0600556B RID: 21867 RVA: 0x00166EC8 File Offset: 0x001650C8
			protected override void Finalize()
			{
				try
				{
					if (this.connectionPoint != null && this.cookie != 0 && !AppDomain.CurrentDomain.IsFinalizingForUnload())
					{
						SynchronizationContext synchronizationContext = SynchronizationContext.Current;
						if (synchronizationContext != null)
						{
							synchronizationContext.Post(new SendOrPostCallback(this.AttemptDisconnect), null);
						}
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x0600556C RID: 21868 RVA: 0x00166F28 File Offset: 0x00165128
			private void AttemptDisconnect(object trash)
			{
				if (this.threadId == Thread.CurrentThread.ManagedThreadId)
				{
					this.Disconnect();
				}
			}

			// Token: 0x1700146C RID: 5228
			// (get) Token: 0x0600556D RID: 21869 RVA: 0x00166F42 File Offset: 0x00165142
			internal bool Connected
			{
				get
				{
					return this.connectionPoint != null && this.cookie != 0;
				}
			}

			// Token: 0x040037A4 RID: 14244
			private UnsafeNativeMethods.IConnectionPoint connectionPoint;

			// Token: 0x040037A5 RID: 14245
			private int cookie;

			// Token: 0x040037A6 RID: 14246
			internal int threadId;
		}

		/// <summary>Specifies the type of member that referenced the ActiveX control while it was in an invalid state.</summary>
		// Token: 0x0200054F RID: 1359
		public enum ActiveXInvokeKind
		{
			/// <summary>A method referenced the ActiveX control.</summary>
			// Token: 0x040037A8 RID: 14248
			MethodInvoke,
			/// <summary>The get accessor of a property referenced the ActiveX control.</summary>
			// Token: 0x040037A9 RID: 14249
			PropertyGet,
			/// <summary>The set accessor of a property referenced the ActiveX control.</summary>
			// Token: 0x040037AA RID: 14250
			PropertySet
		}

		/// <summary>The exception that is thrown when the ActiveX control is referenced while in an invalid state.</summary>
		// Token: 0x02000550 RID: 1360
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class InvalidActiveXStateException : Exception
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.InvalidActiveXStateException" /> class and indicates the name of the member that referenced the ActiveX control and the kind of reference it made.</summary>
			/// <param name="name">The name of the member that referenced the ActiveX control while it was in an invalid state. </param>
			/// <param name="kind">One of the <see cref="T:System.Windows.Forms.AxHost.ActiveXInvokeKind" /> values. </param>
			// Token: 0x0600556E RID: 21870 RVA: 0x00166F57 File Offset: 0x00165157
			public InvalidActiveXStateException(string name, AxHost.ActiveXInvokeKind kind)
			{
				this.name = name;
				this.kind = kind;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.InvalidActiveXStateException" /> class without specifying information about the member that referenced the ActiveX control.</summary>
			// Token: 0x0600556F RID: 21871 RVA: 0x00166F6D File Offset: 0x0016516D
			public InvalidActiveXStateException()
			{
			}

			/// <summary>Creates and returns a string representation of the current exception.</summary>
			/// <returns>A string representation of the current exception.</returns>
			// Token: 0x06005570 RID: 21872 RVA: 0x00166F78 File Offset: 0x00165178
			public override string ToString()
			{
				switch (this.kind)
				{
				case AxHost.ActiveXInvokeKind.MethodInvoke:
					return SR.GetString("AXInvalidMethodInvoke", new object[]
					{
						this.name
					});
				case AxHost.ActiveXInvokeKind.PropertyGet:
					return SR.GetString("AXInvalidPropertyGet", new object[]
					{
						this.name
					});
				case AxHost.ActiveXInvokeKind.PropertySet:
					return SR.GetString("AXInvalidPropertySet", new object[]
					{
						this.name
					});
				default:
					return base.ToString();
				}
			}

			// Token: 0x040037AB RID: 14251
			private string name;

			// Token: 0x040037AC RID: 14252
			private AxHost.ActiveXInvokeKind kind;
		}

		// Token: 0x02000551 RID: 1361
		private class OleInterfaces : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.ISimpleFrameSite, UnsafeNativeMethods.IVBGetControl, UnsafeNativeMethods.IGetVBAObject, UnsafeNativeMethods.IPropertyNotifySink, IReflect, IDisposable
		{
			// Token: 0x06005571 RID: 21873 RVA: 0x00166FF4 File Offset: 0x001651F4
			internal OleInterfaces(AxHost host)
			{
				if (host == null)
				{
					throw new ArgumentNullException("host");
				}
				this.host = host;
			}

			// Token: 0x06005572 RID: 21874 RVA: 0x00167014 File Offset: 0x00165214
			private void Dispose(bool disposing)
			{
				if (disposing && !AppDomain.CurrentDomain.IsFinalizingForUnload())
				{
					SynchronizationContext synchronizationContext = SynchronizationContext.Current;
					if (synchronizationContext != null)
					{
						synchronizationContext.Post(new SendOrPostCallback(this.AttemptStopEvents), null);
					}
				}
			}

			// Token: 0x06005573 RID: 21875 RVA: 0x0016704C File Offset: 0x0016524C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06005574 RID: 21876 RVA: 0x0016705B File Offset: 0x0016525B
			internal AxHost GetAxHost()
			{
				return this.host;
			}

			// Token: 0x06005575 RID: 21877 RVA: 0x00167063 File Offset: 0x00165263
			internal void OnOcxCreate()
			{
				this.StartEvents();
			}

			// Token: 0x06005576 RID: 21878 RVA: 0x0016706C File Offset: 0x0016526C
			internal void StartEvents()
			{
				if (this.connectionPoint != null)
				{
					return;
				}
				object ocx = this.host.GetOcx();
				try
				{
					this.connectionPoint = new AxHost.ConnectionPointCookie(ocx, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
				}
				catch
				{
				}
			}

			// Token: 0x06005577 RID: 21879 RVA: 0x001670BC File Offset: 0x001652BC
			private void AttemptStopEvents(object trash)
			{
				if (this.connectionPoint == null)
				{
					return;
				}
				if (this.connectionPoint.threadId == Thread.CurrentThread.ManagedThreadId)
				{
					this.StopEvents();
				}
			}

			// Token: 0x06005578 RID: 21880 RVA: 0x001670E4 File Offset: 0x001652E4
			internal void StopEvents()
			{
				if (this.connectionPoint != null)
				{
					this.connectionPoint.Disconnect();
					this.connectionPoint = null;
				}
			}

			// Token: 0x06005579 RID: 21881 RVA: 0x00167100 File Offset: 0x00165300
			int UnsafeNativeMethods.IGetVBAObject.GetObject(ref Guid riid, UnsafeNativeMethods.IVBFormat[] rval, int dwReserved)
			{
				if (rval == null || riid.Equals(Guid.Empty))
				{
					return -2147024809;
				}
				if (riid.Equals(AxHost.ivbformat_Guid))
				{
					rval[0] = new AxHost.VBFormat();
					return 0;
				}
				rval[0] = null;
				return -2147467262;
			}

			// Token: 0x0600557A RID: 21882 RVA: 0x00167138 File Offset: 0x00165338
			int UnsafeNativeMethods.IVBGetControl.EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = null;
				ppenum = this.host.GetParentContainer().EnumControls(this.host, dwOleContF, dwWhich);
				return 0;
			}

			// Token: 0x0600557B RID: 21883 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.ISimpleFrameSite.PreMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, ref int pdwCookie)
			{
				return 0;
			}

			// Token: 0x0600557C RID: 21884 RVA: 0x0000E214 File Offset: 0x0000C414
			int UnsafeNativeMethods.ISimpleFrameSite.PostMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, int dwCookie)
			{
				return 1;
			}

			// Token: 0x0600557D RID: 21885 RVA: 0x0000DE5C File Offset: 0x0000C05C
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x0600557E RID: 21886 RVA: 0x0000DE5C File Offset: 0x0000C05C
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x0600557F RID: 21887 RVA: 0x00167158 File Offset: 0x00165358
			MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
			{
				return new MethodInfo[0];
			}

			// Token: 0x06005580 RID: 21888 RVA: 0x0000DE5C File Offset: 0x0000C05C
			FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06005581 RID: 21889 RVA: 0x00167160 File Offset: 0x00165360
			FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
			{
				return new FieldInfo[0];
			}

			// Token: 0x06005582 RID: 21890 RVA: 0x0000DE5C File Offset: 0x0000C05C
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06005583 RID: 21891 RVA: 0x0000DE5C File Offset: 0x0000C05C
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x06005584 RID: 21892 RVA: 0x00167168 File Offset: 0x00165368
			PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
			{
				return new PropertyInfo[0];
			}

			// Token: 0x06005585 RID: 21893 RVA: 0x00167170 File Offset: 0x00165370
			MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06005586 RID: 21894 RVA: 0x00167170 File Offset: 0x00165370
			MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06005587 RID: 21895 RVA: 0x00167178 File Offset: 0x00165378
			object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
			{
				if (name.StartsWith("[DISPID="))
				{
					int num = name.IndexOf("]");
					int dispid = int.Parse(name.Substring(8, num - 8), CultureInfo.InvariantCulture);
					object ambientProperty = this.host.GetAmbientProperty(dispid);
					if (ambientProperty != null)
					{
						return ambientProperty;
					}
				}
				throw AxHost.E_FAIL;
			}

			// Token: 0x1700146D RID: 5229
			// (get) Token: 0x06005588 RID: 21896 RVA: 0x0000DE5C File Offset: 0x0000C05C
			Type IReflect.UnderlyingSystemType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06005589 RID: 21897 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
			{
				return 0;
			}

			// Token: 0x0600558A RID: 21898 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
			{
				return -2147467263;
			}

			// Token: 0x0600558B RID: 21899 RVA: 0x001671CA File Offset: 0x001653CA
			int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
			{
				ppDisp = this.host.GetParentContainer().GetProxyForControl(this.host);
				if (ppDisp == null)
				{
					return -2147467263;
				}
				return 0;
			}

			// Token: 0x0600558C RID: 21900 RVA: 0x001671F0 File Offset: 0x001653F0
			int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods._POINTL pPtlHimetric, NativeMethods.tagPOINTF pPtfContainer, int dwFlags)
			{
				int num = AxHost.SetupLogPixels(false);
				if (NativeMethods.Failed(num))
				{
					return num;
				}
				if ((dwFlags & 4) != 0)
				{
					if ((dwFlags & 2) != 0)
					{
						pPtfContainer.x = (float)this.host.HM2Pix(pPtlHimetric.x, AxHost.logPixelsX);
						pPtfContainer.y = (float)this.host.HM2Pix(pPtlHimetric.y, AxHost.logPixelsY);
					}
					else
					{
						if ((dwFlags & 1) == 0)
						{
							return -2147024809;
						}
						pPtfContainer.x = (float)this.host.HM2Pix(pPtlHimetric.x, AxHost.logPixelsX);
						pPtfContainer.y = (float)this.host.HM2Pix(pPtlHimetric.y, AxHost.logPixelsY);
					}
				}
				else
				{
					if ((dwFlags & 8) == 0)
					{
						return -2147024809;
					}
					if ((dwFlags & 2) != 0)
					{
						pPtlHimetric.x = this.host.Pix2HM((int)pPtfContainer.x, AxHost.logPixelsX);
						pPtlHimetric.y = this.host.Pix2HM((int)pPtfContainer.y, AxHost.logPixelsY);
					}
					else
					{
						if ((dwFlags & 1) == 0)
						{
							return -2147024809;
						}
						pPtlHimetric.x = this.host.Pix2HM((int)pPtfContainer.x, AxHost.logPixelsX);
						pPtlHimetric.y = this.host.Pix2HM((int)pPtfContainer.y, AxHost.logPixelsY);
					}
				}
				return 0;
			}

			// Token: 0x0600558D RID: 21901 RVA: 0x0016733C File Offset: 0x0016553C
			int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref NativeMethods.MSG pMsg, int grfModifiers)
			{
				this.host.SetAxState(AxHost.siteProcessedInputKey, true);
				Message message = default(Message);
				message.Msg = pMsg.message;
				message.WParam = pMsg.wParam;
				message.LParam = pMsg.lParam;
				message.HWnd = pMsg.hwnd;
				int result;
				try
				{
					result = (this.host.PreProcessMessage(ref message) ? 0 : 1);
				}
				finally
				{
					this.host.SetAxState(AxHost.siteProcessedInputKey, false);
				}
				return result;
			}

			// Token: 0x0600558E RID: 21902 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
			{
				return 0;
			}

			// Token: 0x0600558F RID: 21903 RVA: 0x001673D4 File Offset: 0x001655D4
			int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
			{
				if (this.host.CanShowPropertyPages())
				{
					this.host.ShowPropertyPages();
					return 0;
				}
				return -2147467263;
			}

			// Token: 0x06005590 RID: 21904 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleClientSite.SaveObject()
			{
				return -2147467263;
			}

			// Token: 0x06005591 RID: 21905 RVA: 0x00033B13 File Offset: 0x00031D13
			int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
			{
				moniker = null;
				return -2147467263;
			}

			// Token: 0x06005592 RID: 21906 RVA: 0x001673F5 File Offset: 0x001655F5
			int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
			{
				container = this.host.GetParentContainer();
				return 0;
			}

			// Token: 0x06005593 RID: 21907 RVA: 0x00167408 File Offset: 0x00165608
			int UnsafeNativeMethods.IOleClientSite.ShowObject()
			{
				if (this.host.GetAxState(AxHost.fOwnWindow))
				{
					return 0;
				}
				if (this.host.GetAxState(AxHost.fFakingWindow))
				{
					this.host.DestroyFakeWindow();
					this.host.TransitionDownTo(1);
					this.host.TransitionUpTo(4);
				}
				if (this.host.GetOcState() < 4)
				{
					return 0;
				}
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.host.GetInPlaceObject().GetWindow(out intPtr)))
				{
					if (this.host.GetHandleNoCreate() != intPtr)
					{
						this.host.DetachWindow();
						if (intPtr != IntPtr.Zero)
						{
							this.host.AttachWindow(intPtr);
						}
					}
				}
				else if (this.host.GetInPlaceObject() is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.GetString("AXWindowlessControl"));
				}
				return 0;
			}

			// Token: 0x06005594 RID: 21908 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
			{
				return 0;
			}

			// Token: 0x06005595 RID: 21909 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
			{
				return -2147467263;
			}

			// Token: 0x06005596 RID: 21910 RVA: 0x001674E4 File Offset: 0x001656E4
			IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
			{
				IntPtr result;
				try
				{
					Control parentInternal = this.host.ParentInternal;
					result = ((parentInternal != null) ? parentInternal.Handle : IntPtr.Zero);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}

			// Token: 0x06005597 RID: 21911 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			// Token: 0x06005598 RID: 21912 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
			{
				return 0;
			}

			// Token: 0x06005599 RID: 21913 RVA: 0x00167524 File Offset: 0x00165724
			int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
			{
				this.host.SetAxState(AxHost.ownDisposing, false);
				this.host.SetAxState(AxHost.rejectSelection, false);
				this.host.SetOcState(4);
				return 0;
			}

			// Token: 0x0600559A RID: 21914 RVA: 0x00167555 File Offset: 0x00165755
			int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
			{
				this.host.SetOcState(8);
				this.host.GetParentContainer().OnUIActivate(this.host);
				return 0;
			}

			// Token: 0x0600559B RID: 21915 RVA: 0x0016757C File Offset: 0x0016577C
			int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
			{
				ppDoc = null;
				ppFrame = this.host.GetParentContainer();
				AxHost.FillInRect(lprcPosRect, this.host.Bounds);
				this.host.GetClipRect(lprcClipRect);
				if (lpFrameInfo != null)
				{
					lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
					lpFrameInfo.fMDIApp = false;
					lpFrameInfo.hAccel = IntPtr.Zero;
					lpFrameInfo.cAccelEntries = 0;
					lpFrameInfo.hwndFrame = this.host.ParentInternal.Handle;
				}
				return 0;
			}

			// Token: 0x0600559C RID: 21916 RVA: 0x00167608 File Offset: 0x00165808
			int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.tagSIZE scrollExtant)
			{
				try
				{
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return 1;
			}

			// Token: 0x0600559D RID: 21917 RVA: 0x0016762C File Offset: 0x0016582C
			int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
			{
				this.host.GetParentContainer().OnUIDeactivate(this.host);
				if (this.host.GetOcState() > 4)
				{
					this.host.SetOcState(4);
				}
				return 0;
			}

			// Token: 0x0600559E RID: 21918 RVA: 0x00167660 File Offset: 0x00165860
			int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
			{
				if (this.host.GetOcState() == 8)
				{
					((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
				}
				this.host.GetParentContainer().OnInPlaceDeactivate(this.host);
				this.host.DetachWindow();
				this.host.SetOcState(2);
				return 0;
			}

			// Token: 0x0600559F RID: 21919 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
			{
				return 0;
			}

			// Token: 0x060055A0 RID: 21920 RVA: 0x001676B1 File Offset: 0x001658B1
			int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
			{
				return this.host.GetInPlaceObject().UIDeactivate();
			}

			// Token: 0x060055A1 RID: 21921 RVA: 0x001676C4 File Offset: 0x001658C4
			int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
			{
				bool flag = true;
				if (AxHost.windowsMediaPlayer_Clsid.Equals(this.host.clsid))
				{
					flag = this.host.GetAxState(AxHost.handlePosRectChanged);
				}
				if (flag)
				{
					this.host.GetInPlaceObject().SetObjectRects(lprcPosRect, this.host.GetClipRect(new NativeMethods.COMRECT()));
					this.host.MakeDirty();
				}
				return 0;
			}

			// Token: 0x060055A2 RID: 21922 RVA: 0x0016772C File Offset: 0x0016592C
			void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
			{
				if (this.host.NoComponentChangeEvents != 0)
				{
					return;
				}
				AxHost axHost = this.host;
				int noComponentChangeEvents = axHost.NoComponentChangeEvents;
				axHost.NoComponentChangeEvents = noComponentChangeEvents + 1;
				try
				{
					AxHost.AxPropertyDescriptor axPropertyDescriptor = null;
					if (dispid != -1)
					{
						axPropertyDescriptor = this.host.GetPropertyDescriptorFromDispid(dispid);
						if (axPropertyDescriptor != null)
						{
							axPropertyDescriptor.OnValueChanged(this.host);
							if (!axPropertyDescriptor.SettingValue)
							{
								axPropertyDescriptor.UpdateTypeConverterAndTypeEditor(true);
							}
						}
					}
					else
					{
						PropertyDescriptorCollection properties = ((ICustomTypeDescriptor)this.host).GetProperties();
						foreach (object obj in properties)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							axPropertyDescriptor = (propertyDescriptor as AxHost.AxPropertyDescriptor);
							if (axPropertyDescriptor != null && !axPropertyDescriptor.SettingValue)
							{
								axPropertyDescriptor.UpdateTypeConverterAndTypeEditor(true);
							}
						}
					}
					ISite site = this.host.Site;
					if (site != null)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							try
							{
								componentChangeService.OnComponentChanging(this.host, axPropertyDescriptor);
							}
							catch (CheckoutException ex)
							{
								if (ex == CheckoutException.Canceled)
								{
									return;
								}
								throw ex;
							}
							componentChangeService.OnComponentChanged(this.host, axPropertyDescriptor, null, (axPropertyDescriptor != null) ? axPropertyDescriptor.GetValue(this.host) : null);
						}
					}
				}
				catch (Exception ex2)
				{
					throw ex2;
				}
				finally
				{
					AxHost axHost2 = this.host;
					noComponentChangeEvents = axHost2.NoComponentChangeEvents;
					axHost2.NoComponentChangeEvents = noComponentChangeEvents - 1;
				}
			}

			// Token: 0x060055A3 RID: 21923 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
			{
				return 0;
			}

			// Token: 0x040037AD RID: 14253
			private AxHost host;

			// Token: 0x040037AE RID: 14254
			private AxHost.ConnectionPointCookie connectionPoint;
		}

		// Token: 0x02000552 RID: 1362
		private class VBFormat : UnsafeNativeMethods.IVBFormat
		{
			// Token: 0x060055A4 RID: 21924 RVA: 0x001678DC File Offset: 0x00165ADC
			int UnsafeNativeMethods.IVBFormat.Format(ref object var, IntPtr pszFormat, IntPtr lpBuffer, short cpBuffer, int lcid, short firstD, short firstW, short[] result)
			{
				if (result == null)
				{
					return -2147024809;
				}
				result[0] = 0;
				if (lpBuffer == IntPtr.Zero || cpBuffer < 2)
				{
					return -2147024809;
				}
				IntPtr zero = IntPtr.Zero;
				int num = UnsafeNativeMethods.VarFormat(ref var, new HandleRef(null, pszFormat), (int)firstD, (int)firstW, 32U, ref zero);
				try
				{
					int num2 = 0;
					if (zero != IntPtr.Zero)
					{
						cpBuffer -= 1;
						short val;
						while (num2 < (int)cpBuffer && (val = Marshal.ReadInt16(zero, num2 * 2)) != 0)
						{
							Marshal.WriteInt16(lpBuffer, num2 * 2, val);
							num2++;
						}
					}
					Marshal.WriteInt16(lpBuffer, num2 * 2, 0);
					result[0] = (short)num2;
				}
				finally
				{
					SafeNativeMethods.SysFreeString(new HandleRef(null, zero));
				}
				return 0;
			}
		}

		// Token: 0x02000553 RID: 1363
		internal class EnumUnknown : UnsafeNativeMethods.IEnumUnknown
		{
			// Token: 0x060055A6 RID: 21926 RVA: 0x00167998 File Offset: 0x00165B98
			internal EnumUnknown(object[] arr)
			{
				this.arr = arr;
				this.loc = 0;
				this.size = ((arr == null) ? 0 : arr.Length);
			}

			// Token: 0x060055A7 RID: 21927 RVA: 0x001679BD File Offset: 0x00165BBD
			private EnumUnknown(object[] arr, int loc) : this(arr)
			{
				this.loc = loc;
			}

			// Token: 0x060055A8 RID: 21928 RVA: 0x001679D0 File Offset: 0x00165BD0
			int UnsafeNativeMethods.IEnumUnknown.Next(int celt, IntPtr rgelt, IntPtr pceltFetched)
			{
				if (pceltFetched != IntPtr.Zero)
				{
					Marshal.WriteInt32(pceltFetched, 0, 0);
				}
				if (celt < 0)
				{
					return -2147024809;
				}
				int num = 0;
				if (this.loc >= this.size)
				{
					num = 0;
				}
				else
				{
					while (this.loc < this.size && num < celt)
					{
						if (this.arr[this.loc] != null)
						{
							Marshal.WriteIntPtr(rgelt, Marshal.GetIUnknownForObject(this.arr[this.loc]));
							rgelt = (IntPtr)((long)rgelt + (long)sizeof(IntPtr));
							num++;
						}
						this.loc++;
					}
				}
				if (pceltFetched != IntPtr.Zero)
				{
					Marshal.WriteInt32(pceltFetched, 0, num);
				}
				if (num != celt)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x060055A9 RID: 21929 RVA: 0x00167A8C File Offset: 0x00165C8C
			int UnsafeNativeMethods.IEnumUnknown.Skip(int celt)
			{
				this.loc += celt;
				if (this.loc >= this.size)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x060055AA RID: 21930 RVA: 0x00167AAD File Offset: 0x00165CAD
			void UnsafeNativeMethods.IEnumUnknown.Reset()
			{
				this.loc = 0;
			}

			// Token: 0x060055AB RID: 21931 RVA: 0x00167AB6 File Offset: 0x00165CB6
			void UnsafeNativeMethods.IEnumUnknown.Clone(out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = new AxHost.EnumUnknown(this.arr, this.loc);
			}

			// Token: 0x040037AF RID: 14255
			private object[] arr;

			// Token: 0x040037B0 RID: 14256
			private int loc;

			// Token: 0x040037B1 RID: 14257
			private int size;
		}

		// Token: 0x02000554 RID: 1364
		internal class AxContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame, IReflect
		{
			// Token: 0x060055AC RID: 21932 RVA: 0x00167ACB File Offset: 0x00165CCB
			internal AxContainer(ContainerControl parent)
			{
				this.parent = parent;
				if (parent.Created)
				{
					this.FormCreated();
				}
			}

			// Token: 0x060055AD RID: 21933 RVA: 0x0000DE5C File Offset: 0x0000C05C
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x060055AE RID: 21934 RVA: 0x0000DE5C File Offset: 0x0000C05C
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x060055AF RID: 21935 RVA: 0x00167158 File Offset: 0x00165358
			MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
			{
				return new MethodInfo[0];
			}

			// Token: 0x060055B0 RID: 21936 RVA: 0x0000DE5C File Offset: 0x0000C05C
			FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x060055B1 RID: 21937 RVA: 0x00167160 File Offset: 0x00165360
			FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
			{
				return new FieldInfo[0];
			}

			// Token: 0x060055B2 RID: 21938 RVA: 0x0000DE5C File Offset: 0x0000C05C
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x060055B3 RID: 21939 RVA: 0x0000DE5C File Offset: 0x0000C05C
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x060055B4 RID: 21940 RVA: 0x00167168 File Offset: 0x00165368
			PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
			{
				return new PropertyInfo[0];
			}

			// Token: 0x060055B5 RID: 21941 RVA: 0x00167170 File Offset: 0x00165370
			MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x060055B6 RID: 21942 RVA: 0x00167170 File Offset: 0x00165370
			MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x060055B7 RID: 21943 RVA: 0x00167AF4 File Offset: 0x00165CF4
			object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
			{
				foreach (object obj in this.containerCache)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string nameForControl = this.GetNameForControl((Control)dictionaryEntry.Key);
					if (nameForControl.Equals(name))
					{
						return this.GetProxyForControl((Control)dictionaryEntry.Value);
					}
				}
				throw AxHost.E_FAIL;
			}

			// Token: 0x1700146E RID: 5230
			// (get) Token: 0x060055B8 RID: 21944 RVA: 0x0000DE5C File Offset: 0x0000C05C
			Type IReflect.UnderlyingSystemType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060055B9 RID: 21945 RVA: 0x00167B84 File Offset: 0x00165D84
			internal UnsafeNativeMethods.IExtender GetProxyForControl(Control ctl)
			{
				UnsafeNativeMethods.IExtender extender = null;
				if (this.proxyCache == null)
				{
					this.proxyCache = new Hashtable();
				}
				else
				{
					extender = (UnsafeNativeMethods.IExtender)this.proxyCache[ctl];
				}
				if (extender == null)
				{
					if (ctl != this.parent && !this.GetControlBelongs(ctl))
					{
						AxHost.AxContainer axContainer = AxHost.AxContainer.FindContainerForControl(ctl);
						if (axContainer == null)
						{
							return null;
						}
						extender = new AxHost.AxContainer.ExtenderProxy(ctl, axContainer);
					}
					else
					{
						extender = new AxHost.AxContainer.ExtenderProxy(ctl, this);
					}
					this.proxyCache.Add(ctl, extender);
				}
				return extender;
			}

			// Token: 0x060055BA RID: 21946 RVA: 0x00167BFC File Offset: 0x00165DFC
			internal string GetNameForControl(Control ctl)
			{
				string text = (ctl.Site != null) ? ctl.Site.Name : ctl.Name;
				if (text != null)
				{
					return text;
				}
				return "";
			}

			// Token: 0x060055BB RID: 21947 RVA: 0x000069BD File Offset: 0x00004BBD
			internal object GetProxyForContainer()
			{
				return this;
			}

			// Token: 0x060055BC RID: 21948 RVA: 0x00167C30 File Offset: 0x00165E30
			internal void AddControl(Control ctl)
			{
				lock (this)
				{
					if (this.containerCache.Contains(ctl))
					{
						throw new ArgumentException(SR.GetString("AXDuplicateControl", new object[]
						{
							this.GetNameForControl(ctl)
						}), "ctl");
					}
					this.containerCache.Add(ctl, ctl);
					if (this.assocContainer == null)
					{
						ISite site = ctl.Site;
						if (site != null)
						{
							this.assocContainer = site.Container;
							IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
							if (componentChangeService != null)
							{
								componentChangeService.ComponentRemoved += this.OnComponentRemoved;
							}
						}
					}
				}
			}

			// Token: 0x060055BD RID: 21949 RVA: 0x00167CF0 File Offset: 0x00165EF0
			internal void RemoveControl(Control ctl)
			{
				lock (this)
				{
					if (this.containerCache.Contains(ctl))
					{
						this.containerCache.Remove(ctl);
					}
				}
			}

			// Token: 0x060055BE RID: 21950 RVA: 0x00167D40 File Offset: 0x00165F40
			private void LockComponents()
			{
				this.lockCount++;
			}

			// Token: 0x060055BF RID: 21951 RVA: 0x00167D50 File Offset: 0x00165F50
			private void UnlockComponents()
			{
				this.lockCount--;
				if (this.lockCount == 0)
				{
					this.components = null;
				}
			}

			// Token: 0x060055C0 RID: 21952 RVA: 0x00167D70 File Offset: 0x00165F70
			internal UnsafeNativeMethods.IEnumUnknown EnumControls(Control ctl, int dwOleContF, int dwWhich)
			{
				this.GetComponents();
				this.LockComponents();
				UnsafeNativeMethods.IEnumUnknown result;
				try
				{
					ArrayList arrayList = null;
					bool selected = (dwWhich & 1073741824) != 0;
					bool flag = (dwWhich & 134217728) != 0;
					bool flag2 = (dwWhich & 268435456) != 0;
					bool flag3 = (dwWhich & 536870912) != 0;
					dwWhich &= -2013265921;
					if (flag2 && flag3)
					{
						throw AxHost.E_INVALIDARG;
					}
					if ((dwWhich == 2 || dwWhich == 3) && (flag2 || flag3))
					{
						throw AxHost.E_INVALIDARG;
					}
					int num = 0;
					int num2 = -1;
					Control[] array = null;
					switch (dwWhich)
					{
					case 1:
					{
						Control parentInternal = ctl.ParentInternal;
						if (parentInternal != null)
						{
							array = parentInternal.GetChildControlsInTabOrder(false);
							if (flag3)
							{
								num2 = ctl.TabIndex;
							}
							else if (flag2)
							{
								num = ctl.TabIndex + 1;
							}
						}
						else
						{
							array = new Control[0];
						}
						ctl = null;
						break;
					}
					case 2:
						arrayList = new ArrayList();
						this.MaybeAdd(arrayList, ctl, selected, dwOleContF, false);
						while (ctl != null)
						{
							AxHost.AxContainer axContainer = AxHost.AxContainer.FindContainerForControl(ctl);
							if (axContainer == null)
							{
								break;
							}
							this.MaybeAdd(arrayList, axContainer.parent, selected, dwOleContF, true);
							ctl = axContainer.parent;
						}
						break;
					case 3:
						array = ctl.GetChildControlsInTabOrder(false);
						ctl = null;
						break;
					case 4:
					{
						Hashtable hashtable = this.GetComponents();
						array = new Control[hashtable.Keys.Count];
						hashtable.Keys.CopyTo(array, 0);
						ctl = this.parent;
						break;
					}
					default:
						throw AxHost.E_INVALIDARG;
					}
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						if (num2 == -1 && array != null)
						{
							num2 = array.Length;
						}
						if (ctl != null)
						{
							this.MaybeAdd(arrayList, ctl, selected, dwOleContF, false);
						}
						for (int i = num; i < num2; i++)
						{
							this.MaybeAdd(arrayList, array[i], selected, dwOleContF, false);
						}
					}
					object[] array2 = new object[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					if (flag)
					{
						int j = 0;
						int num3 = array2.Length - 1;
						while (j < num3)
						{
							object obj = array2[j];
							array2[j] = array2[num3];
							array2[num3] = obj;
							j++;
							num3--;
						}
					}
					result = new AxHost.EnumUnknown(array2);
				}
				finally
				{
					this.UnlockComponents();
				}
				return result;
			}

			// Token: 0x060055C1 RID: 21953 RVA: 0x00167F94 File Offset: 0x00166194
			private void MaybeAdd(ArrayList l, Control ctl, bool selected, int dwOleContF, bool ignoreBelong)
			{
				if (!ignoreBelong && ctl != this.parent && !this.GetControlBelongs(ctl))
				{
					return;
				}
				if (selected)
				{
					ISelectionService selectionService = AxHost.GetSelectionService(ctl);
					if (selectionService == null || !selectionService.GetComponentSelected(this))
					{
						return;
					}
				}
				AxHost axHost = ctl as AxHost;
				if (axHost != null && (dwOleContF & 1) != 0)
				{
					l.Add(axHost.GetOcx());
					return;
				}
				if ((dwOleContF & 4) != 0)
				{
					object proxyForControl = this.GetProxyForControl(ctl);
					if (proxyForControl != null)
					{
						l.Add(proxyForControl);
					}
				}
			}

			// Token: 0x060055C2 RID: 21954 RVA: 0x00168008 File Offset: 0x00166208
			private void FillComponentsTable(IContainer container)
			{
				if (container != null)
				{
					ComponentCollection componentCollection = container.Components;
					if (componentCollection != null)
					{
						this.components = new Hashtable();
						foreach (object obj in componentCollection)
						{
							IComponent component = (IComponent)obj;
							if (component is Control && component != this.parent && component.Site != null)
							{
								this.components.Add(component, component);
							}
						}
						return;
					}
				}
				bool flag = true;
				Control[] array = new Control[this.containerCache.Values.Count];
				this.containerCache.Values.CopyTo(array, 0);
				if (array != null)
				{
					if (array.Length != 0 && this.components == null)
					{
						this.components = new Hashtable();
						flag = false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (flag && !this.components.Contains(array[i]))
						{
							this.components.Add(array[i], array[i]);
						}
					}
				}
				this.GetAllChildren(this.parent);
			}

			// Token: 0x060055C3 RID: 21955 RVA: 0x00168128 File Offset: 0x00166328
			private void GetAllChildren(Control ctl)
			{
				if (ctl == null)
				{
					return;
				}
				if (this.components == null)
				{
					this.components = new Hashtable();
				}
				if (ctl != this.parent && !this.components.Contains(ctl))
				{
					this.components.Add(ctl, ctl);
				}
				foreach (object obj in ctl.Controls)
				{
					Control ctl2 = (Control)obj;
					this.GetAllChildren(ctl2);
				}
			}

			// Token: 0x060055C4 RID: 21956 RVA: 0x001681BC File Offset: 0x001663BC
			private Hashtable GetComponents()
			{
				return this.GetComponents(this.GetParentsContainer());
			}

			// Token: 0x060055C5 RID: 21957 RVA: 0x001681CA File Offset: 0x001663CA
			private Hashtable GetComponents(IContainer cont)
			{
				if (this.lockCount == 0)
				{
					this.FillComponentsTable(cont);
				}
				return this.components;
			}

			// Token: 0x060055C6 RID: 21958 RVA: 0x001681E4 File Offset: 0x001663E4
			private bool GetControlBelongs(Control ctl)
			{
				Hashtable hashtable = this.GetComponents();
				return hashtable[ctl] != null;
			}

			// Token: 0x060055C7 RID: 21959 RVA: 0x00168204 File Offset: 0x00166404
			private IContainer GetParentIsDesigned()
			{
				ISite site = this.parent.Site;
				if (site != null && site.DesignMode)
				{
					return site.Container;
				}
				return null;
			}

			// Token: 0x060055C8 RID: 21960 RVA: 0x00168230 File Offset: 0x00166430
			private IContainer GetParentsContainer()
			{
				IContainer parentIsDesigned = this.GetParentIsDesigned();
				if (parentIsDesigned != null)
				{
					return parentIsDesigned;
				}
				return this.assocContainer;
			}

			// Token: 0x060055C9 RID: 21961 RVA: 0x00168250 File Offset: 0x00166450
			private bool RegisterControl(AxHost ctl)
			{
				ISite site = ctl.Site;
				if (site != null)
				{
					IContainer container = site.Container;
					if (container != null)
					{
						if (this.assocContainer != null)
						{
							return container == this.assocContainer;
						}
						this.assocContainer = container;
						IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							componentChangeService.ComponentRemoved += this.OnComponentRemoved;
						}
						return true;
					}
				}
				return false;
			}

			// Token: 0x060055CA RID: 21962 RVA: 0x001682B8 File Offset: 0x001664B8
			private void OnComponentRemoved(object sender, ComponentEventArgs e)
			{
				Control control = e.Component as Control;
				if (sender == this.assocContainer && control != null)
				{
					this.RemoveControl(control);
				}
			}

			// Token: 0x060055CB RID: 21963 RVA: 0x001682E4 File Offset: 0x001664E4
			internal static AxHost.AxContainer FindContainerForControl(Control ctl)
			{
				AxHost axHost = ctl as AxHost;
				if (axHost != null)
				{
					if (axHost.container != null)
					{
						return axHost.container;
					}
					ContainerControl containingControl = axHost.ContainingControl;
					if (containingControl != null)
					{
						AxHost.AxContainer axContainer = containingControl.CreateAxContainer();
						if (axContainer.RegisterControl(axHost))
						{
							axContainer.AddControl(axHost);
							return axContainer;
						}
					}
				}
				return null;
			}

			// Token: 0x060055CC RID: 21964 RVA: 0x0016832E File Offset: 0x0016652E
			internal void OnInPlaceDeactivate(AxHost site)
			{
				if (this.siteActive == site)
				{
					this.siteActive = null;
					if (site.GetSiteOwnsDeactivation())
					{
						this.parent.ActiveControl = null;
					}
				}
			}

			// Token: 0x060055CD RID: 21965 RVA: 0x00168354 File Offset: 0x00166554
			internal void OnUIDeactivate(AxHost site)
			{
				this.siteUIActive = null;
				site.RemoveSelectionHandler();
				site.SetSelectionStyle(1);
				site.editMode = 0;
				if (site.GetSiteOwnsDeactivation())
				{
					ContainerControl containingControl = site.ContainingControl;
				}
			}

			// Token: 0x060055CE RID: 21966 RVA: 0x00168390 File Offset: 0x00166590
			internal void OnUIActivate(AxHost site)
			{
				if (this.siteUIActive == site)
				{
					return;
				}
				if (this.siteUIActive != null && this.siteUIActive != site)
				{
					AxHost axHost = this.siteUIActive;
					bool axState = axHost.GetAxState(AxHost.ownDisposing);
					try
					{
						axHost.SetAxState(AxHost.ownDisposing, true);
						axHost.GetInPlaceObject().UIDeactivate();
					}
					finally
					{
						axHost.SetAxState(AxHost.ownDisposing, axState);
					}
				}
				site.AddSelectionHandler();
				this.siteUIActive = site;
				ContainerControl containingControl = site.ContainingControl;
				if (containingControl != null)
				{
					containingControl.ActiveControl = site;
				}
			}

			// Token: 0x060055CF RID: 21967 RVA: 0x00168420 File Offset: 0x00166620
			private void ListAxControls(ArrayList list, bool fuseOcx)
			{
				Hashtable hashtable = this.GetComponents();
				if (hashtable == null)
				{
					return;
				}
				Control[] array = new Control[hashtable.Keys.Count];
				hashtable.Keys.CopyTo(array, 0);
				if (array != null)
				{
					foreach (Control control in array)
					{
						AxHost axHost = control as AxHost;
						if (axHost != null)
						{
							if (fuseOcx)
							{
								list.Add(axHost.GetOcx());
							}
							else
							{
								list.Add(control);
							}
						}
					}
				}
			}

			// Token: 0x060055D0 RID: 21968 RVA: 0x00168492 File Offset: 0x00166692
			internal void ControlCreated(AxHost invoker)
			{
				if (this.formAlreadyCreated)
				{
					if (invoker.IsUserMode() && invoker.AwaitingDefreezing())
					{
						invoker.Freeze(false);
						return;
					}
				}
				else
				{
					this.parent.CreateAxContainer();
				}
			}

			// Token: 0x060055D1 RID: 21969 RVA: 0x001684C0 File Offset: 0x001666C0
			internal void FormCreated()
			{
				if (this.formAlreadyCreated)
				{
					return;
				}
				this.formAlreadyCreated = true;
				ArrayList arrayList = new ArrayList();
				this.ListAxControls(arrayList, false);
				AxHost[] array = new AxHost[arrayList.Count];
				arrayList.CopyTo(array, 0);
				foreach (AxHost axHost in array)
				{
					if (axHost.GetOcState() >= 2 && axHost.IsUserMode() && axHost.AwaitingDefreezing())
					{
						axHost.Freeze(false);
					}
				}
			}

			// Token: 0x060055D2 RID: 21970 RVA: 0x00134CD5 File Offset: 0x00132ED5
			int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
			{
				if (ppmkOut != null)
				{
					ppmkOut[0] = null;
				}
				return -2147467263;
			}

			// Token: 0x060055D3 RID: 21971 RVA: 0x00168534 File Offset: 0x00166734
			int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = null;
				if ((grfFlags & 1) != 0)
				{
					ArrayList arrayList = new ArrayList();
					this.ListAxControls(arrayList, true);
					if (arrayList.Count > 0)
					{
						object[] array = new object[arrayList.Count];
						arrayList.CopyTo(array, 0);
						ppenum = new AxHost.EnumUnknown(array);
						return 0;
					}
				}
				ppenum = new AxHost.EnumUnknown(null);
				return 0;
			}

			// Token: 0x060055D4 RID: 21972 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
			{
				return -2147467263;
			}

			// Token: 0x060055D5 RID: 21973 RVA: 0x00168587 File Offset: 0x00166787
			IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
			{
				return this.parent.Handle;
			}

			// Token: 0x060055D6 RID: 21974 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
			{
				return 0;
			}

			// Token: 0x060055D7 RID: 21975 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
			{
				return -2147467263;
			}

			// Token: 0x060055D8 RID: 21976 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
			{
				return -2147467263;
			}

			// Token: 0x060055D9 RID: 21977 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
			{
				return -2147467263;
			}

			// Token: 0x060055DA RID: 21978 RVA: 0x00168594 File Offset: 0x00166794
			internal void OnExitEditMode(AxHost ctl)
			{
				if (this.ctlInEditMode == null || this.ctlInEditMode != ctl)
				{
					return;
				}
				this.ctlInEditMode = null;
			}

			// Token: 0x060055DB RID: 21979 RVA: 0x001685B0 File Offset: 0x001667B0
			int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
			{
				if (this.siteUIActive != null && this.siteUIActive.iOleInPlaceActiveObjectExternal != pActiveObject)
				{
					if (this.siteUIActive.iOleInPlaceActiveObjectExternal != null)
					{
						Marshal.ReleaseComObject(this.siteUIActive.iOleInPlaceActiveObjectExternal);
					}
					this.siteUIActive.iOleInPlaceActiveObjectExternal = pActiveObject;
				}
				if (pActiveObject == null)
				{
					if (this.ctlInEditMode != null)
					{
						this.ctlInEditMode.editMode = 0;
						this.ctlInEditMode = null;
					}
					return 0;
				}
				AxHost axHost = null;
				if (pActiveObject is UnsafeNativeMethods.IOleObject)
				{
					UnsafeNativeMethods.IOleObject oleObject = (UnsafeNativeMethods.IOleObject)pActiveObject;
					try
					{
						UnsafeNativeMethods.IOleClientSite clientSite = oleObject.GetClientSite();
						if (clientSite is AxHost.OleInterfaces)
						{
							axHost = ((AxHost.OleInterfaces)clientSite).GetAxHost();
						}
					}
					catch (COMException ex)
					{
					}
					if (this.ctlInEditMode != null)
					{
						this.ctlInEditMode.SetSelectionStyle(1);
						this.ctlInEditMode.editMode = 0;
					}
					if (axHost == null)
					{
						this.ctlInEditMode = null;
					}
					else if (!axHost.IsUserMode())
					{
						this.ctlInEditMode = axHost;
						axHost.editMode = 1;
						axHost.AddSelectionHandler();
						axHost.SetSelectionStyle(2);
					}
				}
				return 0;
			}

			// Token: 0x060055DC RID: 21980 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
			{
				return 0;
			}

			// Token: 0x060055DD RID: 21981 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
			{
				return -2147467263;
			}

			// Token: 0x060055DE RID: 21982 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
			{
				return -2147467263;
			}

			// Token: 0x060055DF RID: 21983 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
			{
				return -2147467263;
			}

			// Token: 0x060055E0 RID: 21984 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
			{
				return -2147467263;
			}

			// Token: 0x060055E1 RID: 21985 RVA: 0x0000E214 File Offset: 0x0000C414
			int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
			{
				return 1;
			}

			// Token: 0x040037B2 RID: 14258
			internal ContainerControl parent;

			// Token: 0x040037B3 RID: 14259
			private IContainer assocContainer;

			// Token: 0x040037B4 RID: 14260
			private AxHost siteUIActive;

			// Token: 0x040037B5 RID: 14261
			private AxHost siteActive;

			// Token: 0x040037B6 RID: 14262
			private bool formAlreadyCreated;

			// Token: 0x040037B7 RID: 14263
			private Hashtable containerCache = new Hashtable();

			// Token: 0x040037B8 RID: 14264
			private int lockCount;

			// Token: 0x040037B9 RID: 14265
			private Hashtable components;

			// Token: 0x040037BA RID: 14266
			private Hashtable proxyCache;

			// Token: 0x040037BB RID: 14267
			private AxHost ctlInEditMode;

			// Token: 0x040037BC RID: 14268
			private const int GC_CHILD = 1;

			// Token: 0x040037BD RID: 14269
			private const int GC_LASTSIBLING = 2;

			// Token: 0x040037BE RID: 14270
			private const int GC_FIRSTSIBLING = 4;

			// Token: 0x040037BF RID: 14271
			private const int GC_CONTAINER = 32;

			// Token: 0x040037C0 RID: 14272
			private const int GC_PREVSIBLING = 64;

			// Token: 0x040037C1 RID: 14273
			private const int GC_NEXTSIBLING = 128;

			// Token: 0x0200088B RID: 2187
			private class ExtenderProxy : UnsafeNativeMethods.IExtender, UnsafeNativeMethods.IVBGetControl, UnsafeNativeMethods.IGetVBAObject, UnsafeNativeMethods.IGetOleObject, IReflect
			{
				// Token: 0x0600706B RID: 28779 RVA: 0x0019B69B File Offset: 0x0019989B
				internal ExtenderProxy(Control principal, AxHost.AxContainer container)
				{
					this.pRef = new WeakReference(principal);
					this.pContainer = new WeakReference(container);
				}

				// Token: 0x0600706C RID: 28780 RVA: 0x0019B6BB File Offset: 0x001998BB
				private Control GetP()
				{
					return (Control)this.pRef.Target;
				}

				// Token: 0x0600706D RID: 28781 RVA: 0x0019B6CD File Offset: 0x001998CD
				private AxHost.AxContainer GetC()
				{
					return (AxHost.AxContainer)this.pContainer.Target;
				}

				// Token: 0x0600706E RID: 28782 RVA: 0x0019B6DF File Offset: 0x001998DF
				int UnsafeNativeMethods.IVBGetControl.EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum)
				{
					ppenum = this.GetC().EnumControls(this.GetP(), dwOleContF, dwWhich);
					return 0;
				}

				// Token: 0x0600706F RID: 28783 RVA: 0x0019B6F8 File Offset: 0x001998F8
				object UnsafeNativeMethods.IGetOleObject.GetOleObject(ref Guid riid)
				{
					if (!riid.Equals(AxHost.ioleobject_Guid))
					{
						throw AxHost.E_INVALIDARG;
					}
					Control p = this.GetP();
					if (p != null && p is AxHost)
					{
						return ((AxHost)p).GetOcx();
					}
					throw AxHost.E_FAIL;
				}

				// Token: 0x06007070 RID: 28784 RVA: 0x00167100 File Offset: 0x00165300
				int UnsafeNativeMethods.IGetVBAObject.GetObject(ref Guid riid, UnsafeNativeMethods.IVBFormat[] rval, int dwReserved)
				{
					if (rval == null || riid.Equals(Guid.Empty))
					{
						return -2147024809;
					}
					if (riid.Equals(AxHost.ivbformat_Guid))
					{
						rval[0] = new AxHost.VBFormat();
						return 0;
					}
					rval[0] = null;
					return -2147467262;
				}

				// Token: 0x17001868 RID: 6248
				// (get) Token: 0x06007071 RID: 28785 RVA: 0x0019B73C File Offset: 0x0019993C
				// (set) Token: 0x06007072 RID: 28786 RVA: 0x0019B760 File Offset: 0x00199960
				public int Align
				{
					get
					{
						int num = (int)this.GetP().Dock;
						if (num < 0 || num > 4)
						{
							num = 0;
						}
						return num;
					}
					set
					{
						this.GetP().Dock = (DockStyle)value;
					}
				}

				// Token: 0x17001869 RID: 6249
				// (get) Token: 0x06007073 RID: 28787 RVA: 0x0019B76E File Offset: 0x0019996E
				// (set) Token: 0x06007074 RID: 28788 RVA: 0x0019B780 File Offset: 0x00199980
				public uint BackColor
				{
					get
					{
						return AxHost.GetOleColorFromColor(this.GetP().BackColor);
					}
					set
					{
						this.GetP().BackColor = AxHost.GetColorFromOleColor(value);
					}
				}

				// Token: 0x1700186A RID: 6250
				// (get) Token: 0x06007075 RID: 28789 RVA: 0x0019B793 File Offset: 0x00199993
				// (set) Token: 0x06007076 RID: 28790 RVA: 0x0019B7A0 File Offset: 0x001999A0
				public bool Enabled
				{
					get
					{
						return this.GetP().Enabled;
					}
					set
					{
						this.GetP().Enabled = value;
					}
				}

				// Token: 0x1700186B RID: 6251
				// (get) Token: 0x06007077 RID: 28791 RVA: 0x0019B7AE File Offset: 0x001999AE
				// (set) Token: 0x06007078 RID: 28792 RVA: 0x0019B7C0 File Offset: 0x001999C0
				public uint ForeColor
				{
					get
					{
						return AxHost.GetOleColorFromColor(this.GetP().ForeColor);
					}
					set
					{
						this.GetP().ForeColor = AxHost.GetColorFromOleColor(value);
					}
				}

				// Token: 0x1700186C RID: 6252
				// (get) Token: 0x06007079 RID: 28793 RVA: 0x0019B7D3 File Offset: 0x001999D3
				// (set) Token: 0x0600707A RID: 28794 RVA: 0x0019B7E6 File Offset: 0x001999E6
				public int Height
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Height, false);
					}
					set
					{
						this.GetP().Height = AxHost.Twip2Pixel(value, false);
					}
				}

				// Token: 0x1700186D RID: 6253
				// (get) Token: 0x0600707B RID: 28795 RVA: 0x0019B7FA File Offset: 0x001999FA
				// (set) Token: 0x0600707C RID: 28796 RVA: 0x0019B80D File Offset: 0x00199A0D
				public int Left
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Left, true);
					}
					set
					{
						this.GetP().Left = AxHost.Twip2Pixel(value, true);
					}
				}

				// Token: 0x1700186E RID: 6254
				// (get) Token: 0x0600707D RID: 28797 RVA: 0x0019B821 File Offset: 0x00199A21
				public object Parent
				{
					get
					{
						return this.GetC().GetProxyForControl(this.GetC().parent);
					}
				}

				// Token: 0x1700186F RID: 6255
				// (get) Token: 0x0600707E RID: 28798 RVA: 0x0019B839 File Offset: 0x00199A39
				// (set) Token: 0x0600707F RID: 28799 RVA: 0x0019B847 File Offset: 0x00199A47
				public short TabIndex
				{
					get
					{
						return (short)this.GetP().TabIndex;
					}
					set
					{
						this.GetP().TabIndex = (int)value;
					}
				}

				// Token: 0x17001870 RID: 6256
				// (get) Token: 0x06007080 RID: 28800 RVA: 0x0019B855 File Offset: 0x00199A55
				// (set) Token: 0x06007081 RID: 28801 RVA: 0x0019B862 File Offset: 0x00199A62
				public bool TabStop
				{
					get
					{
						return this.GetP().TabStop;
					}
					set
					{
						this.GetP().TabStop = value;
					}
				}

				// Token: 0x17001871 RID: 6257
				// (get) Token: 0x06007082 RID: 28802 RVA: 0x0019B870 File Offset: 0x00199A70
				// (set) Token: 0x06007083 RID: 28803 RVA: 0x0019B883 File Offset: 0x00199A83
				public int Top
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Top, false);
					}
					set
					{
						this.GetP().Top = AxHost.Twip2Pixel(value, false);
					}
				}

				// Token: 0x17001872 RID: 6258
				// (get) Token: 0x06007084 RID: 28804 RVA: 0x0019B897 File Offset: 0x00199A97
				// (set) Token: 0x06007085 RID: 28805 RVA: 0x0019B8A4 File Offset: 0x00199AA4
				public bool Visible
				{
					get
					{
						return this.GetP().Visible;
					}
					set
					{
						this.GetP().Visible = value;
					}
				}

				// Token: 0x17001873 RID: 6259
				// (get) Token: 0x06007086 RID: 28806 RVA: 0x0019B8B2 File Offset: 0x00199AB2
				// (set) Token: 0x06007087 RID: 28807 RVA: 0x0019B8C5 File Offset: 0x00199AC5
				public int Width
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Width, true);
					}
					set
					{
						this.GetP().Width = AxHost.Twip2Pixel(value, true);
					}
				}

				// Token: 0x17001874 RID: 6260
				// (get) Token: 0x06007088 RID: 28808 RVA: 0x0019B8D9 File Offset: 0x00199AD9
				public string Name
				{
					get
					{
						return this.GetC().GetNameForControl(this.GetP());
					}
				}

				// Token: 0x17001875 RID: 6261
				// (get) Token: 0x06007089 RID: 28809 RVA: 0x0019B8EC File Offset: 0x00199AEC
				public IntPtr Hwnd
				{
					get
					{
						return this.GetP().Handle;
					}
				}

				// Token: 0x17001876 RID: 6262
				// (get) Token: 0x0600708A RID: 28810 RVA: 0x0019B8F9 File Offset: 0x00199AF9
				public object Container
				{
					get
					{
						return this.GetC().GetProxyForContainer();
					}
				}

				// Token: 0x17001877 RID: 6263
				// (get) Token: 0x0600708B RID: 28811 RVA: 0x0019B906 File Offset: 0x00199B06
				// (set) Token: 0x0600708C RID: 28812 RVA: 0x0019B913 File Offset: 0x00199B13
				public string Text
				{
					get
					{
						return this.GetP().Text;
					}
					set
					{
						this.GetP().Text = value;
					}
				}

				// Token: 0x0600708D RID: 28813 RVA: 0x0000701A File Offset: 0x0000521A
				public void Move(object left, object top, object width, object height)
				{
				}

				// Token: 0x0600708E RID: 28814 RVA: 0x0000DE5C File Offset: 0x0000C05C
				MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
				{
					return null;
				}

				// Token: 0x0600708F RID: 28815 RVA: 0x0000DE5C File Offset: 0x0000C05C
				MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
				{
					return null;
				}

				// Token: 0x06007090 RID: 28816 RVA: 0x0019B921 File Offset: 0x00199B21
				MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
				{
					return new MethodInfo[]
					{
						base.GetType().GetMethod("Move")
					};
				}

				// Token: 0x06007091 RID: 28817 RVA: 0x0000DE5C File Offset: 0x0000C05C
				FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
				{
					return null;
				}

				// Token: 0x06007092 RID: 28818 RVA: 0x00167160 File Offset: 0x00165360
				FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
				{
					return new FieldInfo[0];
				}

				// Token: 0x06007093 RID: 28819 RVA: 0x0019B93C File Offset: 0x00199B3C
				PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
				{
					PropertyInfo property = this.GetP().GetType().GetProperty(name, bindingAttr);
					if (property == null)
					{
						property = base.GetType().GetProperty(name, bindingAttr);
					}
					return property;
				}

				// Token: 0x06007094 RID: 28820 RVA: 0x0019B974 File Offset: 0x00199B74
				PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
				{
					PropertyInfo property = this.GetP().GetType().GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
					if (property == null)
					{
						property = base.GetType().GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
					}
					return property;
				}

				// Token: 0x06007095 RID: 28821 RVA: 0x0019B9BC File Offset: 0x00199BBC
				PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
				{
					PropertyInfo[] properties = base.GetType().GetProperties(bindingAttr);
					PropertyInfo[] properties2 = this.GetP().GetType().GetProperties(bindingAttr);
					if (properties == null)
					{
						return properties2;
					}
					if (properties2 == null)
					{
						return properties;
					}
					int num = 0;
					PropertyInfo[] array = new PropertyInfo[properties.Length + properties2.Length];
					foreach (PropertyInfo propertyInfo in properties)
					{
						array[num++] = propertyInfo;
					}
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						array[num++] = propertyInfo2;
					}
					return array;
				}

				// Token: 0x06007096 RID: 28822 RVA: 0x0019BA50 File Offset: 0x00199C50
				MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
				{
					MemberInfo[] member = this.GetP().GetType().GetMember(name, bindingAttr);
					if (member == null)
					{
						member = base.GetType().GetMember(name, bindingAttr);
					}
					return member;
				}

				// Token: 0x06007097 RID: 28823 RVA: 0x0019BA84 File Offset: 0x00199C84
				MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
				{
					MemberInfo[] members = base.GetType().GetMembers(bindingAttr);
					MemberInfo[] members2 = this.GetP().GetType().GetMembers(bindingAttr);
					if (members == null)
					{
						return members2;
					}
					if (members2 == null)
					{
						return members;
					}
					MemberInfo[] array = new MemberInfo[members.Length + members2.Length];
					Array.Copy(members, 0, array, 0, members.Length);
					Array.Copy(members2, 0, array, members.Length, members2.Length);
					return array;
				}

				// Token: 0x06007098 RID: 28824 RVA: 0x0019BAE4 File Offset: 0x00199CE4
				object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
				{
					object result;
					try
					{
						result = base.GetType().InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
					}
					catch (MissingMethodException)
					{
						result = this.GetP().GetType().InvokeMember(name, invokeAttr, binder, this.GetP(), args, modifiers, culture, namedParameters);
					}
					return result;
				}

				// Token: 0x17001878 RID: 6264
				// (get) Token: 0x06007099 RID: 28825 RVA: 0x0000DE5C File Offset: 0x0000C05C
				Type IReflect.UnderlyingSystemType
				{
					get
					{
						return null;
					}
				}

				// Token: 0x040043E4 RID: 17380
				private WeakReference pRef;

				// Token: 0x040043E5 RID: 17381
				private WeakReference pContainer;
			}
		}

		/// <summary>Converts <see cref="T:System.Windows.Forms.AxHost.State" /> objects from one data type to another. </summary>
		// Token: 0x02000555 RID: 1365
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class StateConverter : TypeConverter
		{
			/// <summary>Returns whether the <see cref="T:System.Windows.Forms.AxHost.StateConverter" /> can convert an object of the specified type to an <see cref="T:System.Windows.Forms.AxHost.State" />, using the specified context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type from which to convert.</param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.AxHost.StateConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
			// Token: 0x060055E2 RID: 21986 RVA: 0x001686B0 File Offset: 0x001668B0
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
			}

			/// <summary>Returns whether the <see cref="T:System.Windows.Forms.AxHost.StateConverter" /> can convert an object to the given destination type using the context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type from which to convert.</param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.AxHost.StateConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
			// Token: 0x060055E3 RID: 21987 RVA: 0x001686CE File Offset: 0x001668CE
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
			}

			/// <summary>This member overrides <see cref="M:System.ComponentModel.TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)" />.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
			/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
			/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
			/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
			// Token: 0x060055E4 RID: 21988 RVA: 0x001686EC File Offset: 0x001668EC
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is byte[])
				{
					MemoryStream ms = new MemoryStream((byte[])value);
					return new AxHost.State(ms);
				}
				return base.ConvertFrom(context, culture, value);
			}

			/// <summary>This member overrides <see cref="M:System.ComponentModel.TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)" />.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
			/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed.</param>
			/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
			/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to.</param>
			/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
			// Token: 0x060055E5 RID: 21989 RVA: 0x00168720 File Offset: 0x00166920
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (!(destinationType == typeof(byte[])))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					AxHost.State state = (AxHost.State)value;
					state.Save(memoryStream);
					memoryStream.Close();
					return memoryStream.ToArray();
				}
				return new byte[0];
			}
		}

		/// <summary>Encapsulates the persisted state of an ActiveX control.</summary>
		// Token: 0x02000556 RID: 1366
		[TypeConverter(typeof(TypeConverter))]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[Serializable]
		public class State : ISerializable
		{
			// Token: 0x060055E7 RID: 21991 RVA: 0x0016878C File Offset: 0x0016698C
			internal State(MemoryStream ms, int storageType, AxHost ctl, AxHost.PropertyBagStream propBag)
			{
				this.type = storageType;
				this.propBag = propBag;
				this.length = (int)ms.Length;
				this.ms = ms;
				this.manualUpdate = ctl.GetAxState(AxHost.manualUpdate);
				this.licenseKey = ctl.GetLicenseKey();
			}

			// Token: 0x060055E8 RID: 21992 RVA: 0x001687E6 File Offset: 0x001669E6
			internal State(AxHost.PropertyBagStream propBag)
			{
				this.propBag = propBag;
			}

			// Token: 0x060055E9 RID: 21993 RVA: 0x001687FC File Offset: 0x001669FC
			internal State(MemoryStream ms)
			{
				this.ms = ms;
				this.length = (int)ms.Length;
				this.InitializeFromStream(ms);
			}

			// Token: 0x060055EA RID: 21994 RVA: 0x00168826 File Offset: 0x00166A26
			internal State(AxHost ctl)
			{
				this.CreateStorage();
				this.manualUpdate = ctl.GetAxState(AxHost.manualUpdate);
				this.licenseKey = ctl.GetLicenseKey();
				this.type = 2;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.State" /> class for serializing a state. </summary>
			/// <param name="ms">A <see cref="T:System.IO.Stream" /> in which the state is stored. </param>
			/// <param name="storageType">An <see cref="T:System.Int32" /> indicating the storage type.</param>
			/// <param name="manualUpdate">
			///       <see langword="true" /> for manual updates; otherwise, <see langword="false" />.</param>
			/// <param name="licKey">The license key of the control.</param>
			// Token: 0x060055EB RID: 21995 RVA: 0x0016885F File Offset: 0x00166A5F
			public State(Stream ms, int storageType, bool manualUpdate, string licKey)
			{
				this.type = storageType;
				this.length = (int)ms.Length;
				this.manualUpdate = manualUpdate;
				this.licenseKey = licKey;
				this.InitializeBufferFromStream(ms);
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.AxHost.State" /> class for deserializing a state. </summary>
			/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> value.</param>
			/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> value.</param>
			// Token: 0x060055EC RID: 21996 RVA: 0x00168898 File Offset: 0x00166A98
			protected State(SerializationInfo info, StreamingContext context)
			{
				SerializationInfoEnumerator enumerator = info.GetEnumerator();
				if (enumerator == null)
				{
					return;
				}
				while (enumerator.MoveNext())
				{
					if (string.Compare(enumerator.Name, "Data", true, CultureInfo.InvariantCulture) == 0)
					{
						try
						{
							byte[] array = (byte[])enumerator.Value;
							if (array != null)
							{
								this.InitializeFromStream(new MemoryStream(array));
							}
							continue;
						}
						catch (Exception ex)
						{
							continue;
						}
					}
					if (string.Compare(enumerator.Name, "PropertyBagBinary", true, CultureInfo.InvariantCulture) == 0)
					{
						try
						{
							byte[] array2 = (byte[])enumerator.Value;
							if (array2 != null)
							{
								this.propBag = new AxHost.PropertyBagStream();
								this.propBag.Read(new MemoryStream(array2));
							}
						}
						catch (Exception ex2)
						{
						}
					}
				}
			}

			// Token: 0x1700146F RID: 5231
			// (get) Token: 0x060055ED RID: 21997 RVA: 0x00168968 File Offset: 0x00166B68
			// (set) Token: 0x060055EE RID: 21998 RVA: 0x00168970 File Offset: 0x00166B70
			internal int Type
			{
				get
				{
					return this.type;
				}
				set
				{
					this.type = value;
				}
			}

			// Token: 0x060055EF RID: 21999 RVA: 0x00168979 File Offset: 0x00166B79
			internal bool _GetManualUpdate()
			{
				return this.manualUpdate;
			}

			// Token: 0x060055F0 RID: 22000 RVA: 0x00168981 File Offset: 0x00166B81
			internal string _GetLicenseKey()
			{
				return this.licenseKey;
			}

			// Token: 0x060055F1 RID: 22001 RVA: 0x0016898C File Offset: 0x00166B8C
			private void CreateStorage()
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.buffer != null)
				{
					intPtr = UnsafeNativeMethods.GlobalAlloc(2, this.length);
					IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
					try
					{
						if (intPtr2 != IntPtr.Zero)
						{
							Marshal.Copy(this.buffer, 0, intPtr2, this.length);
						}
					}
					finally
					{
						UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
					}
				}
				bool flag = false;
				try
				{
					this.iLockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(new HandleRef(null, intPtr), true);
					if (this.buffer == null)
					{
						this.storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(this.iLockBytes, 4114, 0);
					}
					else
					{
						this.storage = UnsafeNativeMethods.StgOpenStorageOnILockBytes(this.iLockBytes, null, 18, 0, 0);
					}
				}
				catch (Exception ex)
				{
					flag = true;
				}
				if (flag)
				{
					if (this.iLockBytes == null && intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.GlobalFree(new HandleRef(null, intPtr));
					}
					else
					{
						this.iLockBytes = null;
					}
					this.storage = null;
				}
			}

			// Token: 0x060055F2 RID: 22002 RVA: 0x00168A98 File Offset: 0x00166C98
			internal UnsafeNativeMethods.IPropertyBag GetPropBag()
			{
				return this.propBag;
			}

			// Token: 0x060055F3 RID: 22003 RVA: 0x00168AA0 File Offset: 0x00166CA0
			internal UnsafeNativeMethods.IStorage GetStorage()
			{
				if (this.storage == null)
				{
					this.CreateStorage();
				}
				return this.storage;
			}

			// Token: 0x060055F4 RID: 22004 RVA: 0x00168AB8 File Offset: 0x00166CB8
			internal UnsafeNativeMethods.IStream GetStream()
			{
				if (this.ms == null)
				{
					if (this.buffer == null)
					{
						return null;
					}
					this.ms = new MemoryStream(this.buffer);
				}
				else
				{
					this.ms.Seek(0L, SeekOrigin.Begin);
				}
				return new UnsafeNativeMethods.ComStreamFromDataStream(this.ms);
			}

			// Token: 0x060055F5 RID: 22005 RVA: 0x00168B04 File Offset: 0x00166D04
			private void InitializeFromStream(Stream ids)
			{
				BinaryReader binaryReader = new BinaryReader(ids);
				this.type = binaryReader.ReadInt32();
				int num = binaryReader.ReadInt32();
				this.manualUpdate = binaryReader.ReadBoolean();
				int num2 = binaryReader.ReadInt32();
				if (num2 != 0)
				{
					this.licenseKey = new string(binaryReader.ReadChars(num2));
				}
				for (int i = binaryReader.ReadInt32(); i > 0; i--)
				{
					int num3 = binaryReader.ReadInt32();
					ids.Position += (long)num3;
				}
				this.length = binaryReader.ReadInt32();
				if (this.length > 0)
				{
					this.buffer = binaryReader.ReadBytes(this.length);
				}
			}

			// Token: 0x060055F6 RID: 22006 RVA: 0x00168BA4 File Offset: 0x00166DA4
			private void InitializeBufferFromStream(Stream ids)
			{
				BinaryReader binaryReader = new BinaryReader(ids);
				this.length = binaryReader.ReadInt32();
				if (this.length > 0)
				{
					this.buffer = binaryReader.ReadBytes(this.length);
				}
			}

			// Token: 0x060055F7 RID: 22007 RVA: 0x00168BE0 File Offset: 0x00166DE0
			internal AxHost.State RefreshStorage(UnsafeNativeMethods.IPersistStorage iPersistStorage)
			{
				if (this.storage == null || this.iLockBytes == null)
				{
					return null;
				}
				iPersistStorage.Save(this.storage, true);
				this.storage.Commit(0);
				iPersistStorage.HandsOffStorage();
				try
				{
					this.buffer = null;
					this.ms = null;
					NativeMethods.STATSTG statstg = new NativeMethods.STATSTG();
					this.iLockBytes.Stat(statstg, 1);
					this.length = (int)statstg.cbSize;
					this.buffer = new byte[this.length];
					IntPtr hglobalFromILockBytes = UnsafeNativeMethods.GetHGlobalFromILockBytes(this.iLockBytes);
					IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, hglobalFromILockBytes));
					try
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.Copy(intPtr, this.buffer, 0, this.length);
						}
						else
						{
							this.length = 0;
							this.buffer = null;
						}
					}
					finally
					{
						UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, hglobalFromILockBytes));
					}
				}
				finally
				{
					iPersistStorage.SaveCompleted(this.storage);
				}
				return this;
			}

			// Token: 0x060055F8 RID: 22008 RVA: 0x00168CE0 File Offset: 0x00166EE0
			internal void Save(MemoryStream stream)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				binaryWriter.Write(this.type);
				binaryWriter.Write(this.VERSION);
				binaryWriter.Write(this.manualUpdate);
				if (this.licenseKey != null)
				{
					binaryWriter.Write(this.licenseKey.Length);
					binaryWriter.Write(this.licenseKey.ToCharArray());
				}
				else
				{
					binaryWriter.Write(0);
				}
				binaryWriter.Write(0);
				binaryWriter.Write(this.length);
				if (this.buffer != null)
				{
					binaryWriter.Write(this.buffer);
					return;
				}
				if (this.ms != null)
				{
					this.ms.Position = 0L;
					this.ms.WriteTo(stream);
				}
			}

			/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
			/// <param name="si">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
			/// <param name="context">The destination for this serialization.</param>
			/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
			// Token: 0x060055F9 RID: 22009 RVA: 0x00168D94 File Offset: 0x00166F94
			void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
			{
				IntSecurity.UnmanagedCode.Demand();
				MemoryStream memoryStream = new MemoryStream();
				this.Save(memoryStream);
				si.AddValue("Data", memoryStream.ToArray());
				if (this.propBag != null)
				{
					try
					{
						memoryStream = new MemoryStream();
						this.propBag.Write(memoryStream);
						si.AddValue("PropertyBagBinary", memoryStream.ToArray());
					}
					catch (Exception ex)
					{
					}
				}
			}

			// Token: 0x040037C2 RID: 14274
			private int VERSION = 1;

			// Token: 0x040037C3 RID: 14275
			private int length;

			// Token: 0x040037C4 RID: 14276
			private byte[] buffer;

			// Token: 0x040037C5 RID: 14277
			internal int type;

			// Token: 0x040037C6 RID: 14278
			private MemoryStream ms;

			// Token: 0x040037C7 RID: 14279
			private UnsafeNativeMethods.IStorage storage;

			// Token: 0x040037C8 RID: 14280
			private UnsafeNativeMethods.ILockBytes iLockBytes;

			// Token: 0x040037C9 RID: 14281
			private bool manualUpdate;

			// Token: 0x040037CA RID: 14282
			private string licenseKey;

			// Token: 0x040037CB RID: 14283
			private AxHost.PropertyBagStream propBag;
		}

		// Token: 0x02000557 RID: 1367
		internal class PropertyBagStream : UnsafeNativeMethods.IPropertyBag
		{
			// Token: 0x060055FA RID: 22010 RVA: 0x00168E0C File Offset: 0x0016700C
			internal void Read(Stream stream)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				try
				{
					this.bag = (Hashtable)binaryFormatter.Deserialize(stream);
				}
				catch
				{
					this.bag = new Hashtable();
				}
			}

			// Token: 0x060055FB RID: 22011 RVA: 0x00168E54 File Offset: 0x00167054
			int UnsafeNativeMethods.IPropertyBag.Read(string pszPropName, ref object pVar, UnsafeNativeMethods.IErrorLog pErrorLog)
			{
				if (!this.bag.Contains(pszPropName))
				{
					return -2147024809;
				}
				pVar = this.bag[pszPropName];
				if (pVar != null)
				{
					return 0;
				}
				return -2147024809;
			}

			// Token: 0x060055FC RID: 22012 RVA: 0x00168E83 File Offset: 0x00167083
			int UnsafeNativeMethods.IPropertyBag.Write(string pszPropName, ref object pVar)
			{
				if (pVar != null && !pVar.GetType().IsSerializable)
				{
					return 0;
				}
				this.bag[pszPropName] = pVar;
				return 0;
			}

			// Token: 0x060055FD RID: 22013 RVA: 0x00168EA8 File Offset: 0x001670A8
			internal void Write(Stream stream)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, this.bag);
			}

			// Token: 0x040037CC RID: 14284
			private Hashtable bag = new Hashtable();
		}

		/// <summary>Represents the method that will display an ActiveX control's About dialog box.</summary>
		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x06005600 RID: 22016
		protected delegate void AboutBoxDelegate();

		/// <summary>Provides an editor that uses a modal dialog box to display a property page for an ActiveX control.</summary>
		// Token: 0x02000559 RID: 1369
		[ComVisible(false)]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class AxComponentEditor : WindowsFormsComponentEditor
		{
			/// <summary>Creates an editor window that allows the user to edit the specified component.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information. </param>
			/// <param name="obj">The component to edit. </param>
			/// <param name="parent">An <see cref="T:System.Windows.Forms.IWin32Window" /> that the component belongs to. </param>
			/// <returns>
			///     <see langword="true" /> if the component was changed during editing; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005603 RID: 22019 RVA: 0x00168EDC File Offset: 0x001670DC
			public override bool EditComponent(ITypeDescriptorContext context, object obj, IWin32Window parent)
			{
				AxHost axHost = obj as AxHost;
				if (axHost != null)
				{
					try
					{
						((UnsafeNativeMethods.IOleControlSite)axHost.oleSite).ShowPropertyFrame();
						return true;
					}
					catch (Exception ex)
					{
						throw;
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x0200055A RID: 1370
		internal class AxPropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x06005605 RID: 22021 RVA: 0x00168F1C File Offset: 0x0016711C
			internal AxPropertyDescriptor(PropertyDescriptor baseProp, AxHost owner) : base(baseProp)
			{
				this.baseProp = baseProp;
				this.owner = owner;
				this.dispid = (DispIdAttribute)baseProp.Attributes[typeof(DispIdAttribute)];
				if (this.dispid != null)
				{
					if (!this.IsBrowsable && !this.IsReadOnly)
					{
						Guid propertyPage = this.GetPropertyPage(this.dispid.Value);
						if (!Guid.Empty.Equals(propertyPage))
						{
							this.AddAttribute(new BrowsableAttribute(true));
						}
					}
					CategoryAttribute categoryForDispid = owner.GetCategoryForDispid(this.dispid.Value);
					if (categoryForDispid != null)
					{
						this.AddAttribute(categoryForDispid);
					}
					if (this.PropertyType.GUID.Equals(AxHost.dataSource_Guid))
					{
						this.SetFlag(8, true);
					}
				}
			}

			// Token: 0x17001470 RID: 5232
			// (get) Token: 0x06005606 RID: 22022 RVA: 0x00168FED File Offset: 0x001671ED
			public override Type ComponentType
			{
				get
				{
					return this.baseProp.ComponentType;
				}
			}

			// Token: 0x17001471 RID: 5233
			// (get) Token: 0x06005607 RID: 22023 RVA: 0x00168FFA File Offset: 0x001671FA
			public override TypeConverter Converter
			{
				get
				{
					if (this.dispid != null)
					{
						this.UpdateTypeConverterAndTypeEditorInternal(false, this.Dispid);
					}
					if (this.converter == null)
					{
						return base.Converter;
					}
					return this.converter;
				}
			}

			// Token: 0x17001472 RID: 5234
			// (get) Token: 0x06005608 RID: 22024 RVA: 0x00169028 File Offset: 0x00167228
			internal int Dispid
			{
				get
				{
					DispIdAttribute dispIdAttribute = (DispIdAttribute)this.baseProp.Attributes[typeof(DispIdAttribute)];
					if (dispIdAttribute != null)
					{
						return dispIdAttribute.Value;
					}
					return -1;
				}
			}

			// Token: 0x17001473 RID: 5235
			// (get) Token: 0x06005609 RID: 22025 RVA: 0x00169060 File Offset: 0x00167260
			public override bool IsReadOnly
			{
				get
				{
					return this.baseProp.IsReadOnly;
				}
			}

			// Token: 0x17001474 RID: 5236
			// (get) Token: 0x0600560A RID: 22026 RVA: 0x0016906D File Offset: 0x0016726D
			public override Type PropertyType
			{
				get
				{
					return this.baseProp.PropertyType;
				}
			}

			// Token: 0x17001475 RID: 5237
			// (get) Token: 0x0600560B RID: 22027 RVA: 0x0016907A File Offset: 0x0016727A
			internal bool SettingValue
			{
				get
				{
					return this.GetFlag(16);
				}
			}

			// Token: 0x0600560C RID: 22028 RVA: 0x00169084 File Offset: 0x00167284
			private void AddAttribute(Attribute attr)
			{
				this.updateAttrs.Add(attr);
			}

			// Token: 0x0600560D RID: 22029 RVA: 0x00169093 File Offset: 0x00167293
			public override bool CanResetValue(object o)
			{
				return this.baseProp.CanResetValue(o);
			}

			// Token: 0x0600560E RID: 22030 RVA: 0x001690A1 File Offset: 0x001672A1
			public override object GetEditor(Type editorBaseType)
			{
				this.UpdateTypeConverterAndTypeEditorInternal(false, this.dispid.Value);
				if (editorBaseType.Equals(typeof(UITypeEditor)) && this.editor != null)
				{
					return this.editor;
				}
				return base.GetEditor(editorBaseType);
			}

			// Token: 0x0600560F RID: 22031 RVA: 0x001690DD File Offset: 0x001672DD
			private bool GetFlag(int flagValue)
			{
				return (this.flags & flagValue) == flagValue;
			}

			// Token: 0x06005610 RID: 22032 RVA: 0x001690EC File Offset: 0x001672EC
			private Guid GetPropertyPage(int dispid)
			{
				try
				{
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					if (perPropertyBrowsing == null)
					{
						return Guid.Empty;
					}
					Guid result;
					if (NativeMethods.Succeeded(perPropertyBrowsing.MapPropertyToPage(dispid, out result)))
					{
						return result;
					}
				}
				catch (COMException)
				{
				}
				catch (Exception ex)
				{
				}
				return Guid.Empty;
			}

			// Token: 0x06005611 RID: 22033 RVA: 0x00169150 File Offset: 0x00167350
			public override object GetValue(object component)
			{
				if ((!this.GetFlag(8) && !this.owner.CanAccessProperties) || this.GetFlag(4))
				{
					return null;
				}
				object value;
				try
				{
					AxHost axHost = this.owner;
					int noComponentChangeEvents = axHost.NoComponentChangeEvents;
					axHost.NoComponentChangeEvents = noComponentChangeEvents + 1;
					value = this.baseProp.GetValue(component);
				}
				catch (Exception ex)
				{
					if (!this.GetFlag(2))
					{
						this.SetFlag(2, true);
						this.AddAttribute(new BrowsableAttribute(false));
						this.owner.RefreshAllProperties = true;
						this.SetFlag(4, true);
					}
					throw ex;
				}
				finally
				{
					AxHost axHost2 = this.owner;
					int noComponentChangeEvents = axHost2.NoComponentChangeEvents;
					axHost2.NoComponentChangeEvents = noComponentChangeEvents - 1;
				}
				return value;
			}

			// Token: 0x06005612 RID: 22034 RVA: 0x0016920C File Offset: 0x0016740C
			public void OnValueChanged(object component)
			{
				this.OnValueChanged(component, EventArgs.Empty);
			}

			// Token: 0x06005613 RID: 22035 RVA: 0x0016921A File Offset: 0x0016741A
			public override void ResetValue(object o)
			{
				this.baseProp.ResetValue(o);
			}

			// Token: 0x06005614 RID: 22036 RVA: 0x00169228 File Offset: 0x00167428
			private void SetFlag(int flagValue, bool value)
			{
				if (value)
				{
					this.flags |= flagValue;
					return;
				}
				this.flags &= ~flagValue;
			}

			// Token: 0x06005615 RID: 22037 RVA: 0x0016924C File Offset: 0x0016744C
			public override void SetValue(object component, object value)
			{
				if (!this.GetFlag(8) && !this.owner.CanAccessProperties)
				{
					return;
				}
				try
				{
					this.SetFlag(16, true);
					if (this.PropertyType.IsEnum && value.GetType() != this.PropertyType)
					{
						this.baseProp.SetValue(component, Enum.ToObject(this.PropertyType, value));
					}
					else
					{
						this.baseProp.SetValue(component, value);
					}
				}
				finally
				{
					this.SetFlag(16, false);
				}
				this.OnValueChanged(component);
				if (this.owner == component)
				{
					this.owner.SetAxState(AxHost.valueChanged, true);
				}
			}

			// Token: 0x06005616 RID: 22038 RVA: 0x00169300 File Offset: 0x00167500
			public override bool ShouldSerializeValue(object o)
			{
				return this.baseProp.ShouldSerializeValue(o);
			}

			// Token: 0x06005617 RID: 22039 RVA: 0x00169310 File Offset: 0x00167510
			internal void UpdateAttributes()
			{
				if (this.updateAttrs.Count == 0)
				{
					return;
				}
				ArrayList arrayList = new ArrayList(this.AttributeArray);
				foreach (object obj in this.updateAttrs)
				{
					Attribute value = (Attribute)obj;
					arrayList.Add(value);
				}
				Attribute[] array = new Attribute[arrayList.Count];
				arrayList.CopyTo(array, 0);
				this.AttributeArray = array;
				this.updateAttrs.Clear();
			}

			// Token: 0x06005618 RID: 22040 RVA: 0x001693B0 File Offset: 0x001675B0
			internal void UpdateTypeConverterAndTypeEditor(bool force)
			{
				if (this.GetFlag(1) && force)
				{
					this.SetFlag(1, false);
				}
			}

			// Token: 0x06005619 RID: 22041 RVA: 0x001693C8 File Offset: 0x001675C8
			internal void UpdateTypeConverterAndTypeEditorInternal(bool force, int dispid)
			{
				if (this.GetFlag(1) && !force)
				{
					return;
				}
				if (this.owner.GetOcx() == null)
				{
					return;
				}
				try
				{
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					if (perPropertyBrowsing != null)
					{
						NativeMethods.CA_STRUCT ca_STRUCT = new NativeMethods.CA_STRUCT();
						NativeMethods.CA_STRUCT ca_STRUCT2 = new NativeMethods.CA_STRUCT();
						int num = 0;
						try
						{
							num = perPropertyBrowsing.GetPredefinedStrings(dispid, ca_STRUCT, ca_STRUCT2);
						}
						catch (ExternalException ex)
						{
							num = ex.ErrorCode;
						}
						bool flag;
						if (num != 0)
						{
							flag = false;
							if (this.converter is Com2EnumConverter)
							{
								this.converter = null;
							}
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							OleStrCAMarshaler oleStrCAMarshaler = new OleStrCAMarshaler(ca_STRUCT);
							Int32CAMarshaler int32CAMarshaler = new Int32CAMarshaler(ca_STRUCT2);
							if (oleStrCAMarshaler.Count > 0 && int32CAMarshaler.Count > 0)
							{
								if (this.converter == null)
								{
									this.converter = new AxHost.AxEnumConverter(this, new AxHost.AxPerPropertyBrowsingEnum(this, this.owner, oleStrCAMarshaler, int32CAMarshaler, true));
								}
								else if (this.converter is AxHost.AxEnumConverter)
								{
									((AxHost.AxEnumConverter)this.converter).RefreshValues();
									AxHost.AxPerPropertyBrowsingEnum axPerPropertyBrowsingEnum = ((AxHost.AxEnumConverter)this.converter).com2Enum as AxHost.AxPerPropertyBrowsingEnum;
									if (axPerPropertyBrowsingEnum != null)
									{
										axPerPropertyBrowsingEnum.RefreshArrays(oleStrCAMarshaler, int32CAMarshaler);
									}
								}
							}
						}
						else if ((ComAliasNameAttribute)this.baseProp.Attributes[typeof(ComAliasNameAttribute)] == null)
						{
							Guid propertyPage = this.GetPropertyPage(dispid);
							if (!Guid.Empty.Equals(propertyPage))
							{
								this.editor = new AxHost.AxPropertyTypeEditor(this, propertyPage);
								if (!this.IsBrowsable)
								{
									this.AddAttribute(new BrowsableAttribute(true));
								}
							}
						}
					}
					this.SetFlag(1, true);
				}
				catch (Exception ex2)
				{
				}
			}

			// Token: 0x040037CD RID: 14285
			private PropertyDescriptor baseProp;

			// Token: 0x040037CE RID: 14286
			internal AxHost owner;

			// Token: 0x040037CF RID: 14287
			private DispIdAttribute dispid;

			// Token: 0x040037D0 RID: 14288
			private TypeConverter converter;

			// Token: 0x040037D1 RID: 14289
			private UITypeEditor editor;

			// Token: 0x040037D2 RID: 14290
			private ArrayList updateAttrs = new ArrayList();

			// Token: 0x040037D3 RID: 14291
			private int flags;

			// Token: 0x040037D4 RID: 14292
			private const int FlagUpdatedEditorAndConverter = 1;

			// Token: 0x040037D5 RID: 14293
			private const int FlagCheckGetter = 2;

			// Token: 0x040037D6 RID: 14294
			private const int FlagGettterThrew = 4;

			// Token: 0x040037D7 RID: 14295
			private const int FlagIgnoreCanAccessProperties = 8;

			// Token: 0x040037D8 RID: 14296
			private const int FlagSettingValue = 16;
		}

		// Token: 0x0200055B RID: 1371
		private class AxPropertyTypeEditor : UITypeEditor
		{
			// Token: 0x0600561A RID: 22042 RVA: 0x00169594 File Offset: 0x00167794
			public AxPropertyTypeEditor(AxHost.AxPropertyDescriptor pd, Guid guid)
			{
				this.propDesc = pd;
				this.guid = guid;
			}

			// Token: 0x0600561B RID: 22043 RVA: 0x001695AC File Offset: 0x001677AC
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				try
				{
					object instance = context.Instance;
					this.propDesc.owner.ShowPropertyPageForDispid(this.propDesc.Dispid, this.guid);
				}
				catch (Exception ex)
				{
					if (provider != null)
					{
						IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
						if (iuiservice != null)
						{
							iuiservice.ShowError(ex, SR.GetString("ErrorTypeConverterFailed"));
						}
					}
				}
				return value;
			}

			// Token: 0x0600561C RID: 22044 RVA: 0x0000E211 File Offset: 0x0000C411
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}

			// Token: 0x040037D9 RID: 14297
			private AxHost.AxPropertyDescriptor propDesc;

			// Token: 0x040037DA RID: 14298
			private Guid guid;
		}

		// Token: 0x0200055C RID: 1372
		private class AxEnumConverter : Com2EnumConverter
		{
			// Token: 0x0600561D RID: 22045 RVA: 0x00169624 File Offset: 0x00167824
			public AxEnumConverter(AxHost.AxPropertyDescriptor target, Com2Enum com2Enum) : base(com2Enum)
			{
				this.target = target;
			}

			// Token: 0x0600561E RID: 22046 RVA: 0x00169634 File Offset: 0x00167834
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter converter = this.target.Converter;
				return base.GetStandardValues(context);
			}

			// Token: 0x040037DB RID: 14299
			private AxHost.AxPropertyDescriptor target;
		}

		// Token: 0x0200055D RID: 1373
		private class AxPerPropertyBrowsingEnum : Com2Enum
		{
			// Token: 0x0600561F RID: 22047 RVA: 0x00169656 File Offset: 0x00167856
			public AxPerPropertyBrowsingEnum(AxHost.AxPropertyDescriptor targetObject, AxHost owner, OleStrCAMarshaler names, Int32CAMarshaler values, bool allowUnknowns) : base(new string[0], new object[0], allowUnknowns)
			{
				this.target = targetObject;
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.owner = owner;
				this.arraysFetched = false;
			}

			// Token: 0x17001476 RID: 5238
			// (get) Token: 0x06005620 RID: 22048 RVA: 0x00169690 File Offset: 0x00167890
			public override object[] Values
			{
				get
				{
					this.EnsureArrays();
					return base.Values;
				}
			}

			// Token: 0x17001477 RID: 5239
			// (get) Token: 0x06005621 RID: 22049 RVA: 0x0016969E File Offset: 0x0016789E
			public override string[] Names
			{
				get
				{
					this.EnsureArrays();
					return base.Names;
				}
			}

			// Token: 0x06005622 RID: 22050 RVA: 0x001696AC File Offset: 0x001678AC
			private void EnsureArrays()
			{
				if (this.arraysFetched)
				{
					return;
				}
				this.arraysFetched = true;
				try
				{
					object[] items = this.nameMarshaller.Items;
					object[] items2 = this.valueMarshaller.Items;
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					int num = 0;
					if (items.Length != 0)
					{
						object[] array = new object[items2.Length];
						NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
						for (int i = 0; i < items.Length; i++)
						{
							int dwCookie = (int)items2[i];
							if (items[i] != null && items[i] is string)
							{
								variant.vt = 0;
								if (perPropertyBrowsing.GetPredefinedValue(this.target.Dispid, dwCookie, variant) == 0 && variant.vt != 0)
								{
									array[i] = variant.ToObject();
								}
								variant.Clear();
								num++;
							}
						}
						if (num > 0)
						{
							string[] array2 = new string[num];
							Array.Copy(items, 0, array2, 0, num);
							base.PopulateArrays(array2, array);
						}
					}
				}
				catch (Exception ex)
				{
				}
			}

			// Token: 0x06005623 RID: 22051 RVA: 0x001697B0 File Offset: 0x001679B0
			internal void RefreshArrays(OleStrCAMarshaler names, Int32CAMarshaler values)
			{
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.arraysFetched = false;
			}

			// Token: 0x06005624 RID: 22052 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void PopulateArrays(string[] names, object[] values)
			{
			}

			// Token: 0x06005625 RID: 22053 RVA: 0x001697C7 File Offset: 0x001679C7
			public override object FromString(string s)
			{
				this.EnsureArrays();
				return base.FromString(s);
			}

			// Token: 0x06005626 RID: 22054 RVA: 0x001697D6 File Offset: 0x001679D6
			public override string ToString(object v)
			{
				this.EnsureArrays();
				return base.ToString(v);
			}

			// Token: 0x040037DC RID: 14300
			private AxHost.AxPropertyDescriptor target;

			// Token: 0x040037DD RID: 14301
			private AxHost owner;

			// Token: 0x040037DE RID: 14302
			private OleStrCAMarshaler nameMarshaller;

			// Token: 0x040037DF RID: 14303
			private Int32CAMarshaler valueMarshaller;

			// Token: 0x040037E0 RID: 14304
			private bool arraysFetched;
		}
	}
}
