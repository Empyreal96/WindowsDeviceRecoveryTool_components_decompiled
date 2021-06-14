using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides focus-management functionality for controls that can function as a container for other controls.</summary>
	// Token: 0x02000156 RID: 342
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ContainerControl : ScrollableControl, IContainerControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContainerControl" /> class.</summary>
		// Token: 0x06000B9F RID: 2975 RVA: 0x00024EF0 File Offset: 0x000230F0
		public ContainerControl()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			base.SetState2(2048, true);
		}

		/// <summary>Gets or sets the dimensions that the control was designed to.</summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> containing the dots per inch (DPI) or <see cref="T:System.Drawing.Font" /> size that the control was designed to.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The width or height of the <see cref="T:System.Drawing.SizeF" /> value is less than 0 when setting this value.</exception>
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00024F3F File Offset: 0x0002313F
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00024F48 File Offset: 0x00023148
		[Localizable(true)]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SizeF AutoScaleDimensions
		{
			get
			{
				return this.autoScaleDimensions;
			}
			set
			{
				if (value.Width < 0f || value.Height < 0f)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("ContainerControlInvalidAutoScaleDimensions"), "value");
				}
				this.autoScaleDimensions = value;
				if (!this.autoScaleDimensions.IsEmpty)
				{
					this.LayoutScalingNeeded();
				}
			}
		}

		/// <summary>Gets the scaling factor between the current and design-time automatic scaling dimensions. </summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> containing the scaling ratio between the current and design-time scaling automatic scaling dimensions.</returns>
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00024FA0 File Offset: 0x000231A0
		protected SizeF AutoScaleFactor
		{
			get
			{
				SizeF sizeF = this.CurrentAutoScaleDimensions;
				SizeF sizeF2 = this.AutoScaleDimensions;
				if (sizeF2.IsEmpty)
				{
					return new SizeF(1f, 1f);
				}
				return new SizeF(sizeF.Width / sizeF2.Width, sizeF.Height / sizeF2.Height);
			}
		}

		/// <summary>Gets or sets the automatic scaling mode of the control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoScaleMode" /> that represents the current scaling mode. The default is <see cref="F:System.Windows.Forms.AutoScaleMode.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">An <see cref="T:System.Windows.Forms.AutoScaleMode" /> value that is not valid was used to set this property.</exception>
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00024FF7 File Offset: 0x000231F7
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x00025000 File Offset: 0x00023200
		[SRCategory("CatLayout")]
		[SRDescription("ContainerControlAutoScaleModeDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AutoScaleMode AutoScaleMode
		{
			get
			{
				return this.autoScaleMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoScaleMode));
				}
				bool flag = false;
				if (value != this.autoScaleMode)
				{
					if (this.autoScaleMode != AutoScaleMode.Inherit)
					{
						this.autoScaleDimensions = SizeF.Empty;
					}
					this.currentAutoScaleDimensions = SizeF.Empty;
					this.autoScaleMode = value;
					flag = true;
				}
				this.OnAutoScaleModeChanged();
				if (flag)
				{
					this.LayoutScalingNeeded();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether controls in this container will be automatically validated when the focus changes.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoValidate" /> enumerated value that indicates whether contained controls are implicitly validated on focus change. The default is <see cref="F:System.Windows.Forms.AutoValidate.Inherit" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A <see cref="T:System.Windows.Forms.AutoValidate" /> value that is not valid was used to set this property.</exception>
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00025075 File Offset: 0x00023275
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0002508D File Offset: 0x0002328D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[AmbientValue(AutoValidate.Inherit)]
		[SRCategory("CatBehavior")]
		[SRDescription("ContainerControlAutoValidate")]
		public virtual AutoValidate AutoValidate
		{
			get
			{
				if (this.autoValidate == AutoValidate.Inherit)
				{
					return Control.GetAutoValidateForControl(this);
				}
				return this.autoValidate;
			}
			set
			{
				if (value - AutoValidate.Inherit > 3)
				{
					throw new InvalidEnumArgumentException("AutoValidate", (int)value, typeof(AutoValidate));
				}
				if (this.autoValidate != value)
				{
					this.autoValidate = value;
					this.OnAutoValidateChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ContainerControl.AutoValidate" /> property changes.</summary>
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06000BA7 RID: 2983 RVA: 0x000250C6 File Offset: 0x000232C6
		// (remove) Token: 0x06000BA8 RID: 2984 RVA: 0x000250DF File Offset: 0x000232DF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ContainerControlOnAutoValidateChangedDescr")]
		public event EventHandler AutoValidateChanged
		{
			add
			{
				this.autoValidateChanged = (EventHandler)Delegate.Combine(this.autoValidateChanged, value);
			}
			remove
			{
				this.autoValidateChanged = (EventHandler)Delegate.Remove(this.autoValidateChanged, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.BindingContext" /> for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingContext" /> for the control.</returns>
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000250F8 File Offset: 0x000232F8
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0002511D File Offset: 0x0002331D
		[Browsable(false)]
		[SRDescription("ContainerControlBindingContextDescr")]
		public override BindingContext BindingContext
		{
			get
			{
				BindingContext bindingContext = base.BindingContext;
				if (bindingContext == null)
				{
					bindingContext = new BindingContext();
					this.BindingContext = bindingContext;
				}
				return bindingContext;
			}
			set
			{
				base.BindingContext = value;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property can be set to an active value, to enable IME support.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override bool CanEnableIme
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the active control on the container control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that is currently active on the <see cref="T:System.Windows.Forms.ContainerControl" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Forms.Control" /> assigned could not be activated. </exception>
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00025126 File Offset: 0x00023326
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0002512E File Offset: 0x0002332E
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ContainerControlActiveControlDescr")]
		public Control ActiveControl
		{
			get
			{
				return this.activeControl;
			}
			set
			{
				this.SetActiveControl(value);
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00025138 File Offset: 0x00023338
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				return createParams;
			}
		}

		/// <summary>Gets the current run-time dimensions of the screen.</summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> containing the current dots per inch (DPI) or <see cref="T:System.Drawing.Font" /> size of the screen.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 device context could not be created for the current screen.</exception>
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00025160 File Offset: 0x00023360
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public SizeF CurrentAutoScaleDimensions
		{
			get
			{
				if (this.currentAutoScaleDimensions.IsEmpty)
				{
					AutoScaleMode autoScaleMode = this.AutoScaleMode;
					if (autoScaleMode != AutoScaleMode.Font)
					{
						if (autoScaleMode != AutoScaleMode.Dpi)
						{
							this.currentAutoScaleDimensions = this.AutoScaleDimensions;
						}
						else if (DpiHelper.EnableDpiChangedMessageHandling)
						{
							this.currentAutoScaleDimensions = new SizeF((float)this.deviceDpi, (float)this.deviceDpi);
						}
						else
						{
							this.currentAutoScaleDimensions = WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.Dpi;
						}
					}
					else
					{
						this.currentAutoScaleDimensions = this.GetFontAutoScaleDimensions();
					}
				}
				return this.currentAutoScaleDimensions;
			}
		}

		/// <summary>Gets the form that the container control is assigned to.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Form" /> that the container control is assigned to. This property will return null if the control is hosted inside of Internet Explorer or in another hosting context where there is no parent form. </returns>
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x000251E9 File Offset: 0x000233E9
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ContainerControlParentFormDescr")]
		public Form ParentForm
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.ParentFormInternal;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x000251FB File Offset: 0x000233FB
		internal Form ParentFormInternal
		{
			get
			{
				if (this.ParentInternal != null)
				{
					return this.ParentInternal.FindFormInternal();
				}
				if (this is Form)
				{
					return null;
				}
				return base.FindFormInternal();
			}
		}

		/// <summary>Activates the specified control.</summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> to activate.</param>
		/// <returns>
		///     <see langword="true" /> if the control is successfully activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BB2 RID: 2994 RVA: 0x00025221 File Offset: 0x00023421
		bool IContainerControl.ActivateControl(Control control)
		{
			IntSecurity.ModifyFocus.Demand();
			return this.ActivateControlInternal(control, true);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00025235 File Offset: 0x00023435
		internal bool ActivateControlInternal(Control control)
		{
			return this.ActivateControlInternal(control, true);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00025240 File Offset: 0x00023440
		internal bool ActivateControlInternal(Control control, bool originator)
		{
			bool result = true;
			bool flag = false;
			ContainerControl containerControl = null;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				containerControl = (parentInternal.GetContainerControlInternal() as ContainerControl);
				if (containerControl != null)
				{
					flag = (containerControl.ActiveControl != this);
				}
			}
			if (control != this.activeControl || flag)
			{
				if (flag && !containerControl.ActivateControlInternal(this, false))
				{
					return false;
				}
				result = this.AssignActiveControlInternal((control == this) ? null : control);
			}
			if (originator)
			{
				this.ScrollActiveControlIntoView();
			}
			return result;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000252B4 File Offset: 0x000234B4
		internal bool HasFocusableChild()
		{
			Control control = null;
			do
			{
				control = base.GetNextControl(control, true);
			}
			while ((control == null || !control.CanSelect || !control.TabStop) && control != null);
			return control != null;
		}

		/// <summary>Adjusts the scroll bars on the container based on the current control positions and the control currently selected.</summary>
		/// <param name="displayScrollbars">
		///       <see langword="true" /> to show the scroll bars; otherwise, <see langword="false" />. </param>
		// Token: 0x06000BB6 RID: 2998 RVA: 0x000252E6 File Offset: 0x000234E6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void AdjustFormScrollbars(bool displayScrollbars)
		{
			base.AdjustFormScrollbars(displayScrollbars);
			if (!base.GetScrollState(8))
			{
				this.ScrollActiveControlIntoView();
			}
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00025300 File Offset: 0x00023500
		internal virtual void AfterControlRemoved(Control control, Control oldParent)
		{
			ContainerControl containerControl;
			if (control == this.activeControl || control.Contains(this.activeControl))
			{
				IntSecurity.ModifyFocus.Assert();
				bool flag;
				try
				{
					flag = base.SelectNextControl(control, true, true, true, true);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (flag && this.activeControl != control)
				{
					if (!this.activeControl.Parent.IsTopMdiWindowClosing)
					{
						this.FocusActiveControlInternal();
					}
				}
				else
				{
					this.SetActiveControlInternal(null);
				}
			}
			else if (this.activeControl == null && this.ParentInternal != null)
			{
				containerControl = (this.ParentInternal.GetContainerControlInternal() as ContainerControl);
				if (containerControl != null && containerControl.ActiveControl == this)
				{
					Form form = base.FindFormInternal();
					if (form != null)
					{
						IntSecurity.ModifyFocus.Assert();
						try
						{
							form.SelectNextControl(this, true, true, true, true);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
			containerControl = this;
			while (containerControl != null)
			{
				Control parentInternal = containerControl.ParentInternal;
				if (parentInternal == null)
				{
					break;
				}
				containerControl = (parentInternal.GetContainerControlInternal() as ContainerControl);
				if (containerControl != null && containerControl.unvalidatedControl != null && (containerControl.unvalidatedControl == control || control.Contains(containerControl.unvalidatedControl)))
				{
					containerControl.unvalidatedControl = oldParent;
				}
			}
			if (control == this.unvalidatedControl || control.Contains(this.unvalidatedControl))
			{
				this.unvalidatedControl = null;
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00025448 File Offset: 0x00023648
		private bool AssignActiveControlInternal(Control value)
		{
			if (this.activeControl != value)
			{
				try
				{
					if (value != null)
					{
						value.BecomingActiveControl = true;
					}
					this.activeControl = value;
					this.UpdateFocusedControl();
				}
				finally
				{
					if (value != null)
					{
						value.BecomingActiveControl = false;
					}
				}
				if (this.activeControl == value)
				{
					Form form = base.FindFormInternal();
					if (form != null)
					{
						form.UpdateDefaultButton();
					}
				}
			}
			else
			{
				this.focusedControl = this.activeControl;
			}
			return this.activeControl == value;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000254C4 File Offset: 0x000236C4
		private void AxContainerFormCreated()
		{
			((AxHost.AxContainer)base.Properties.GetObject(ContainerControl.PropAxContainer)).FormCreated();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000254E0 File Offset: 0x000236E0
		internal override bool CanProcessMnemonic()
		{
			return this.state[ContainerControl.stateProcessingMnemonic] || base.CanProcessMnemonic();
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000254FC File Offset: 0x000236FC
		internal AxHost.AxContainer CreateAxContainer()
		{
			object obj = base.Properties.GetObject(ContainerControl.PropAxContainer);
			if (obj == null)
			{
				obj = new AxHost.AxContainer(this);
				base.Properties.SetObject(ContainerControl.PropAxContainer, obj);
			}
			return (AxHost.AxContainer)obj;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000BBC RID: 3004 RVA: 0x0002553B File Offset: 0x0002373B
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.activeControl = null;
			}
			base.Dispose(disposing);
			this.focusedControl = null;
			this.unvalidatedControl = null;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002555C File Offset: 0x0002375C
		private void EnableRequiredScaling(Control start, bool enable)
		{
			start.RequiredScalingEnabled = enable;
			foreach (object obj in start.Controls)
			{
				Control start2 = (Control)obj;
				this.EnableRequiredScaling(start2, enable);
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000255C0 File Offset: 0x000237C0
		internal void FocusActiveControlInternal()
		{
			if (this.activeControl != null && this.activeControl.Visible)
			{
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				if (focus == IntPtr.Zero || Control.FromChildHandleInternal(focus) != this.activeControl)
				{
					UnsafeNativeMethods.SetFocus(new HandleRef(this.activeControl, this.activeControl.Handle));
					return;
				}
			}
			else
			{
				ContainerControl containerControl = this;
				while (containerControl != null && !containerControl.Visible)
				{
					Control parentInternal = containerControl.ParentInternal;
					if (parentInternal == null)
					{
						break;
					}
					containerControl = (parentInternal.GetContainerControlInternal() as ContainerControl);
				}
				if (containerControl != null && containerControl.Visible)
				{
					UnsafeNativeMethods.SetFocus(new HandleRef(containerControl, containerControl.Handle));
				}
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00025664 File Offset: 0x00023864
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size sz = this.SizeFromClientSize(Size.Empty);
			Size sz2 = sz + base.Padding.Size;
			return this.LayoutEngine.GetPreferredSize(this, proposedSize - sz2) + sz2;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000256AC File Offset: 0x000238AC
		internal override Rectangle GetToolNativeScreenRectangle()
		{
			if (base.GetTopLevel())
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
				NativeMethods.POINT point = new NativeMethods.POINT(0, 0);
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
				return new Rectangle(point.x, point.y, rect.right, rect.bottom);
			}
			return base.GetToolNativeScreenRectangle();
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002571C File Offset: 0x0002391C
		private SizeF GetFontAutoScaleDimensions()
		{
			SizeF empty = SizeF.Empty;
			IntPtr intPtr = UnsafeNativeMethods.CreateCompatibleDC(NativeMethods.NullHandleRef);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			HandleRef hDC = new HandleRef(this, intPtr);
			try
			{
				HandleRef hObject = new HandleRef(this, base.FontHandle);
				HandleRef hObject2 = new HandleRef(this, SafeNativeMethods.SelectObject(hDC, hObject));
				try
				{
					NativeMethods.TEXTMETRIC textmetric = default(NativeMethods.TEXTMETRIC);
					SafeNativeMethods.GetTextMetrics(hDC, ref textmetric);
					empty.Height = (float)textmetric.tmHeight;
					if ((textmetric.tmPitchAndFamily & 1) != 0)
					{
						IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
						IntUnsafeNativeMethods.GetTextExtentPoint32(hDC, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", size);
						empty.Width = (float)((int)Math.Round((double)((float)size.cx / (float)"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)));
					}
					else
					{
						empty.Width = (float)textmetric.tmAveCharWidth;
					}
				}
				finally
				{
					SafeNativeMethods.SelectObject(hDC, hObject2);
				}
			}
			finally
			{
				UnsafeNativeMethods.DeleteCompatibleDC(hDC);
			}
			return empty;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00025818 File Offset: 0x00023A18
		private void LayoutScalingNeeded()
		{
			this.EnableRequiredScaling(this, true);
			this.state[ContainerControl.stateScalingNeededOnLayout] = true;
			if (!base.IsLayoutSuspended)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAutoScaleModeChanged()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ContainerControl.AutoValidateChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000BC4 RID: 3012 RVA: 0x00025847 File Offset: 0x00023A47
		protected virtual void OnAutoValidateChanged(EventArgs e)
		{
			if (this.autoValidateChanged != null)
			{
				this.autoValidateChanged(this, e);
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00025860 File Offset: 0x00023A60
		internal override void OnFrameWindowActivate(bool fActivate)
		{
			if (fActivate)
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					if (this.ActiveControl == null)
					{
						base.SelectNextControl(null, true, true, true, false);
					}
					this.InnerMostActiveContainerControl.FocusActiveControlInternal();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000258B4 File Offset: 0x00023AB4
		internal override void OnChildLayoutResuming(Control child, bool performLayout)
		{
			base.OnChildLayoutResuming(child, performLayout);
			if (DpiHelper.EnableSinglePassScalingOfDpiForms && this.AutoScaleMode == AutoScaleMode.Dpi)
			{
				return;
			}
			if (!this.state[ContainerControl.stateScalingChild] && !performLayout && this.AutoScaleMode != AutoScaleMode.None && this.AutoScaleMode != AutoScaleMode.Inherit && this.state[ContainerControl.stateScalingNeededOnLayout])
			{
				this.state[ContainerControl.stateScalingChild] = true;
				try
				{
					child.Scale(this.AutoScaleFactor, SizeF.Empty, this);
				}
				finally
				{
					this.state[ContainerControl.stateScalingChild] = false;
				}
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.</summary>
		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002595C File Offset: 0x00023B5C
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (base.Properties.GetObject(ContainerControl.PropAxContainer) != null)
			{
				this.AxContainerFormCreated();
			}
			this.OnBindingContextChanged(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000BC8 RID: 3016 RVA: 0x00025988 File Offset: 0x00023B88
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnFontChanged(EventArgs e)
		{
			if (this.AutoScaleMode == AutoScaleMode.Font)
			{
				this.currentAutoScaleDimensions = SizeF.Empty;
				this.SuspendAllLayout(this);
				try
				{
					this.PerformAutoScale(!base.RequiredScalingEnabled, true);
				}
				finally
				{
					this.ResumeAllLayout(this, false);
				}
			}
			base.OnFontChanged(e);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000259E4 File Offset: 0x00023BE4
		internal void FormDpiChanged(float factor)
		{
			this.currentAutoScaleDimensions = SizeF.Empty;
			this.SuspendAllLayout(this);
			SizeF sizeF = new SizeF(factor, factor);
			try
			{
				base.ScaleChildControls(sizeF, sizeF, this, true);
			}
			finally
			{
				this.ResumeAllLayout(this, false);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x06000BCA RID: 3018 RVA: 0x00025A34 File Offset: 0x00023C34
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.PerformNeededAutoScaleOnLayout();
			base.OnLayout(e);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00025A43 File Offset: 0x00023C43
		internal override void OnLayoutResuming(bool performLayout)
		{
			this.PerformNeededAutoScaleOnLayout();
			base.OnLayoutResuming(performLayout);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000BCC RID: 3020 RVA: 0x00025A52 File Offset: 0x00023C52
		protected override void OnParentChanged(EventArgs e)
		{
			this.state[ContainerControl.stateParentChanged] = !base.RequiredScalingEnabled;
			base.OnParentChanged(e);
		}

		/// <summary>Performs scaling of the container control and its children.</summary>
		// Token: 0x06000BCD RID: 3021 RVA: 0x00025A74 File Offset: 0x00023C74
		public void PerformAutoScale()
		{
			this.PerformAutoScale(true, true);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00025A80 File Offset: 0x00023C80
		private void PerformAutoScale(bool includedBounds, bool excludedBounds)
		{
			bool flag = false;
			try
			{
				if (this.AutoScaleMode != AutoScaleMode.None && this.AutoScaleMode != AutoScaleMode.Inherit)
				{
					this.SuspendAllLayout(this);
					flag = true;
					SizeF includedFactor = SizeF.Empty;
					SizeF excludedFactor = SizeF.Empty;
					if (includedBounds)
					{
						includedFactor = this.AutoScaleFactor;
					}
					if (excludedBounds)
					{
						excludedFactor = this.AutoScaleFactor;
					}
					this.Scale(includedFactor, excludedFactor, this);
					this.autoScaleDimensions = this.CurrentAutoScaleDimensions;
				}
			}
			finally
			{
				if (includedBounds)
				{
					this.state[ContainerControl.stateScalingNeededOnLayout] = false;
					this.EnableRequiredScaling(this, false);
				}
				this.state[ContainerControl.stateParentChanged] = false;
				if (flag)
				{
					this.ResumeAllLayout(this, false);
				}
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00025B2C File Offset: 0x00023D2C
		private void PerformNeededAutoScaleOnLayout()
		{
			if (this.state[ContainerControl.stateScalingNeededOnLayout])
			{
				this.PerformAutoScale(this.state[ContainerControl.stateScalingNeededOnLayout], false);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00025B58 File Offset: 0x00023D58
		internal void ResumeAllLayout(Control start, bool performLayout)
		{
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.ResumeAllLayout(controls[i], performLayout);
			}
			start.ResumeLayout(performLayout);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00025B94 File Offset: 0x00023D94
		internal void SuspendAllLayout(Control start)
		{
			start.SuspendLayout();
			CommonProperties.xClearPreferredSizeCache(start);
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.SuspendAllLayout(controls[i]);
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00025BD4 File Offset: 0x00023DD4
		internal override void Scale(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			if (this.AutoScaleMode == AutoScaleMode.Inherit)
			{
				base.Scale(includedFactor, excludedFactor, requestingControl);
				return;
			}
			SizeF sizeF = excludedFactor;
			SizeF includedFactor2 = includedFactor;
			if (!sizeF.IsEmpty)
			{
				sizeF = this.AutoScaleFactor;
			}
			if (this.AutoScaleMode == AutoScaleMode.None)
			{
				includedFactor2 = this.AutoScaleFactor;
			}
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				SizeF excludedFactor2 = sizeF;
				if (!excludedFactor.IsEmpty && this.ParentInternal != null)
				{
					excludedFactor2 = SizeF.Empty;
					bool flag = requestingControl != this || this.state[ContainerControl.stateParentChanged];
					if (!flag)
					{
						bool flag2 = false;
						bool flag3 = false;
						ISite site = this.Site;
						ISite site2 = this.ParentInternal.Site;
						if (site != null)
						{
							flag2 = site.DesignMode;
						}
						if (site2 != null)
						{
							flag3 = site2.DesignMode;
						}
						if (flag2 && !flag3)
						{
							flag = true;
						}
					}
					if (flag)
					{
						excludedFactor2 = excludedFactor;
					}
				}
				base.ScaleControl(includedFactor, excludedFactor2, requestingControl);
				base.ScaleChildControls(includedFactor2, sizeF, requestingControl, false);
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00025CD0 File Offset: 0x00023ED0
		private bool ProcessArrowKey(bool forward)
		{
			Control control = this;
			if (this.activeControl != null)
			{
				control = this.activeControl.ParentInternal;
			}
			return control.SelectNextControl(this.activeControl, forward, false, false, true);
		}

		/// <summary>Processes a dialog character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///   <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BD4 RID: 3028 RVA: 0x00025D04 File Offset: 0x00023F04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogChar(char charCode)
		{
			ContainerControl containerControl = base.GetContainerControlInternal() as ContainerControl;
			return (containerControl != null && charCode != ' ' && this.ProcessMnemonic(charCode)) || base.ProcessDialogChar(charCode);
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///   <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BD5 RID: 3029 RVA: 0x00025D38 File Offset: 0x00023F38
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys != Keys.Tab)
				{
					if (keys - Keys.Left <= 3)
					{
						if (this.ProcessArrowKey(keys == Keys.Right || keys == Keys.Down))
						{
							return true;
						}
					}
				}
				else if (this.ProcessTabKey((keyData & Keys.Shift) == Keys.None))
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BD6 RID: 3030 RVA: 0x00025D96 File Offset: 0x00023F96
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			return base.ProcessCmdKey(ref msg, keyData) || (this.ParentInternal == null && ToolStripManager.ProcessCmdKey(ref msg, keyData));
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///   <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BD7 RID: 3031 RVA: 0x00025DB8 File Offset: 0x00023FB8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			if (base.Controls.Count == 0)
			{
				return false;
			}
			Control control = this.ActiveControl;
			this.state[ContainerControl.stateProcessingMnemonic] = true;
			bool result = false;
			try
			{
				bool flag = false;
				Control control2 = control;
				for (;;)
				{
					control2 = base.GetNextControl(control2, true);
					if (control2 != null)
					{
						if (control2.ProcessMnemonic(charCode))
						{
							break;
						}
					}
					else
					{
						if (flag)
						{
							goto Block_7;
						}
						flag = true;
					}
					if (control2 == control)
					{
						goto Block_8;
					}
				}
				result = true;
				Block_7:
				Block_8:;
			}
			finally
			{
				this.state[ContainerControl.stateProcessingMnemonic] = false;
			}
			return result;
		}

		/// <summary>Selects the next available control and makes it the active control.</summary>
		/// <param name="forward">
		///       <see langword="true" /> to cycle forward through the controls in the <see cref="T:System.Windows.Forms.ContainerControl" />; otherwise, <see langword="false" />. </param>
		/// <returns>
		///     <see langword="true" /> if a control is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000BD8 RID: 3032 RVA: 0x00025E48 File Offset: 0x00024048
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessTabKey(bool forward)
		{
			return base.SelectNextControl(this.activeControl, forward, true, true, false);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00025E60 File Offset: 0x00024060
		private ScrollableControl FindScrollableParent(Control ctl)
		{
			Control parentInternal = ctl.ParentInternal;
			while (parentInternal != null && !(parentInternal is ScrollableControl))
			{
				parentInternal = parentInternal.ParentInternal;
			}
			if (parentInternal != null)
			{
				return (ScrollableControl)parentInternal;
			}
			return null;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00025E94 File Offset: 0x00024094
		private void ScrollActiveControlIntoView()
		{
			Control control = this.activeControl;
			if (control != null)
			{
				for (ScrollableControl scrollableControl = this.FindScrollableParent(control); scrollableControl != null; scrollableControl = this.FindScrollableParent(scrollableControl))
				{
					scrollableControl.ScrollControlIntoView(this.activeControl);
				}
			}
		}

		/// <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
		/// <param name="directed">
		///   <see langword="true" /> to specify the direction of the control to select; otherwise, <see langword="false" />.</param>
		/// <param name="forward">
		///   <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order.</param>
		// Token: 0x06000BDB RID: 3035 RVA: 0x00025ED0 File Offset: 0x000240D0
		protected override void Select(bool directed, bool forward)
		{
			bool flag = true;
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					containerControlInternal.ActiveControl = this;
					flag = (containerControlInternal.ActiveControl == this);
				}
			}
			if (directed && flag)
			{
				base.SelectNextControl(null, forward, true, true, false);
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00025F18 File Offset: 0x00024118
		private void SetActiveControl(Control ctl)
		{
			this.SetActiveControlInternal(ctl);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00025F24 File Offset: 0x00024124
		internal void SetActiveControlInternal(Control value)
		{
			if (this.activeControl != value || (value != null && !value.Focused))
			{
				if (value != null && !base.Contains(value))
				{
					throw new ArgumentException(SR.GetString("CannotActivateControl"));
				}
				ContainerControl containerControl = this;
				if (value != null && value.ParentInternal != null)
				{
					containerControl = (value.ParentInternal.GetContainerControlInternal() as ContainerControl);
				}
				bool flag;
				if (containerControl != null)
				{
					flag = containerControl.ActivateControlInternal(value, false);
				}
				else
				{
					flag = this.AssignActiveControlInternal(value);
				}
				if (containerControl != null && flag)
				{
					ContainerControl containerControl2 = this;
					while (containerControl2.ParentInternal != null && containerControl2.ParentInternal.GetContainerControlInternal() is ContainerControl)
					{
						containerControl2 = (containerControl2.ParentInternal.GetContainerControlInternal() as ContainerControl);
					}
					if (containerControl2.ContainsFocus && (value == null || !(value is UserControl) || (value is UserControl && !((UserControl)value).HasFocusableChild())))
					{
						containerControl.FocusActiveControlInternal();
					}
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00026000 File Offset: 0x00024200
		internal ContainerControl InnerMostActiveContainerControl
		{
			get
			{
				ContainerControl containerControl = this;
				while (containerControl.ActiveControl is ContainerControl)
				{
					containerControl = (ContainerControl)containerControl.ActiveControl;
				}
				return containerControl;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002602C File Offset: 0x0002422C
		internal ContainerControl InnerMostFocusedContainerControl
		{
			get
			{
				ContainerControl containerControl = this;
				while (containerControl.focusedControl is ContainerControl)
				{
					containerControl = (ContainerControl)containerControl.focusedControl;
				}
				return containerControl;
			}
		}

		/// <summary>When overridden by a derived class, updates which button is the default button.</summary>
		// Token: 0x06000BE0 RID: 3040 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void UpdateDefaultButton()
		{
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00026058 File Offset: 0x00024258
		internal void UpdateFocusedControl()
		{
			this.EnsureUnvalidatedControl(this.focusedControl);
			Control control = this.focusedControl;
			while (this.activeControl != control)
			{
				if (control == null || control.IsDescendant(this.activeControl))
				{
					Control parentInternal = this.activeControl;
					for (;;)
					{
						Control parentInternal2 = parentInternal.ParentInternal;
						if (parentInternal2 == this || parentInternal2 == control)
						{
							break;
						}
						parentInternal = parentInternal.ParentInternal;
					}
					Control control2 = this.focusedControl = control;
					this.EnterValidation(parentInternal);
					if (this.focusedControl != control2)
					{
						control = this.focusedControl;
						continue;
					}
					control = parentInternal;
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						control.NotifyEnter();
						continue;
					}
					try
					{
						control.NotifyEnter();
						continue;
					}
					catch (Exception t)
					{
						Application.OnThreadException(t);
						continue;
					}
				}
				ContainerControl innerMostFocusedContainerControl = this.InnerMostFocusedContainerControl;
				Control control3 = null;
				if (innerMostFocusedContainerControl.focusedControl != null)
				{
					control = innerMostFocusedContainerControl.focusedControl;
					control3 = innerMostFocusedContainerControl;
					if (innerMostFocusedContainerControl != this)
					{
						innerMostFocusedContainerControl.focusedControl = null;
						if (innerMostFocusedContainerControl.ParentInternal == null || !(innerMostFocusedContainerControl.ParentInternal is MdiClient))
						{
							innerMostFocusedContainerControl.activeControl = null;
						}
					}
				}
				else
				{
					control = innerMostFocusedContainerControl;
					if (innerMostFocusedContainerControl.ParentInternal != null)
					{
						ContainerControl containerControl = innerMostFocusedContainerControl.ParentInternal.GetContainerControlInternal() as ContainerControl;
						control3 = containerControl;
						if (containerControl != null && containerControl != this)
						{
							containerControl.focusedControl = null;
							containerControl.activeControl = null;
						}
					}
				}
				do
				{
					Control control4 = control;
					if (control != null)
					{
						control = control.ParentInternal;
					}
					if (control == this)
					{
						control = null;
					}
					if (control4 != null)
					{
						if (NativeWindow.WndProcShouldBeDebuggable)
						{
							control4.NotifyLeave();
						}
						else
						{
							try
							{
								control4.NotifyLeave();
							}
							catch (Exception t2)
							{
								Application.OnThreadException(t2);
							}
						}
					}
				}
				while (control != null && control != control3 && !control.IsDescendant(this.activeControl));
			}
			this.focusedControl = this.activeControl;
			if (this.activeControl != null)
			{
				this.EnterValidation(this.activeControl);
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00026228 File Offset: 0x00024428
		private void EnsureUnvalidatedControl(Control candidate)
		{
			if (this.state[ContainerControl.stateValidating])
			{
				return;
			}
			if (this.unvalidatedControl != null)
			{
				return;
			}
			if (candidate == null)
			{
				return;
			}
			if (!candidate.ShouldAutoValidate)
			{
				return;
			}
			this.unvalidatedControl = candidate;
			while (this.unvalidatedControl is ContainerControl)
			{
				ContainerControl containerControl = this.unvalidatedControl as ContainerControl;
				if (containerControl.unvalidatedControl != null && containerControl.unvalidatedControl.ShouldAutoValidate)
				{
					this.unvalidatedControl = containerControl.unvalidatedControl;
				}
				else
				{
					if (containerControl.activeControl == null || !containerControl.activeControl.ShouldAutoValidate)
					{
						break;
					}
					this.unvalidatedControl = containerControl.activeControl;
				}
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000262C4 File Offset: 0x000244C4
		private void EnterValidation(Control enterControl)
		{
			if (this.unvalidatedControl == null)
			{
				return;
			}
			if (!enterControl.CausesValidation)
			{
				return;
			}
			AutoValidate autoValidateForControl = Control.GetAutoValidateForControl(this.unvalidatedControl);
			if (autoValidateForControl == AutoValidate.Disable)
			{
				return;
			}
			Control control = enterControl;
			while (control != null && !control.IsDescendant(this.unvalidatedControl))
			{
				control = control.ParentInternal;
			}
			bool preventFocusChangeOnError = autoValidateForControl == AutoValidate.EnablePreventFocusChange;
			this.ValidateThroughAncestor(control, preventFocusChangeOnError);
		}

		/// <summary>Verifies the value of the control losing focus by causing the <see cref="E:System.Windows.Forms.Control.Validating" /> and <see cref="E:System.Windows.Forms.Control.Validated" /> events to occur, in that order. </summary>
		/// <returns>
		///     <see langword="true" /> if validation is successful; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002631D File Offset: 0x0002451D
		public bool Validate()
		{
			return this.Validate(false);
		}

		/// <summary>Verifies the value of the control that is losing focus; conditionally dependent on whether automatic validation is turned on. </summary>
		/// <param name="checkAutoValidate">If <see langword="true" />, the value of the <see cref="P:System.Windows.Forms.ContainerControl.AutoValidate" /> property is used to determine if validation should be performed; if <see langword="false" />, validation is unconditionally performed.</param>
		/// <returns>
		///     <see langword="true" /> if validation is successful; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06000BE5 RID: 3045 RVA: 0x00026328 File Offset: 0x00024528
		public bool Validate(bool checkAutoValidate)
		{
			bool flag;
			return this.ValidateInternal(checkAutoValidate, out flag);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00026340 File Offset: 0x00024540
		internal bool ValidateInternal(bool checkAutoValidate, out bool validatedControlAllowsFocusChange)
		{
			validatedControlAllowsFocusChange = false;
			if (this.AutoValidate == AutoValidate.EnablePreventFocusChange || (this.activeControl != null && this.activeControl.CausesValidation))
			{
				if (this.unvalidatedControl == null)
				{
					if (this.focusedControl is ContainerControl && this.focusedControl.CausesValidation)
					{
						ContainerControl containerControl = (ContainerControl)this.focusedControl;
						if (!containerControl.ValidateInternal(checkAutoValidate, out validatedControlAllowsFocusChange))
						{
							return false;
						}
					}
					else
					{
						this.unvalidatedControl = this.focusedControl;
					}
				}
				bool preventFocusChangeOnError = true;
				Control control = (this.unvalidatedControl != null) ? this.unvalidatedControl : this.focusedControl;
				if (control != null)
				{
					AutoValidate autoValidateForControl = Control.GetAutoValidateForControl(control);
					if (checkAutoValidate && autoValidateForControl == AutoValidate.Disable)
					{
						return true;
					}
					preventFocusChangeOnError = (autoValidateForControl == AutoValidate.EnablePreventFocusChange);
					validatedControlAllowsFocusChange = (autoValidateForControl == AutoValidate.EnableAllowFocusChange);
				}
				return this.ValidateThroughAncestor(null, preventFocusChangeOnError);
			}
			return true;
		}

		/// <summary>Causes all of the child controls within a control that support validation to validate their data. </summary>
		/// <returns>
		///     <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06000BE7 RID: 3047 RVA: 0x000263FA File Offset: 0x000245FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ValidateChildren()
		{
			return this.ValidateChildren(ValidationConstraints.Selectable);
		}

		/// <summary>Causes all of the child controls within a control that support validation to validate their data. </summary>
		/// <param name="validationConstraints">Places restrictions on which controls have their <see cref="E:System.Windows.Forms.Control.Validating" /> event raised.</param>
		/// <returns>
		///     <see langword="true" /> if all of the children validated successfully; otherwise, <see langword="false" />. If called from the <see cref="E:System.Windows.Forms.Control.Validating" /> or <see cref="E:System.Windows.Forms.Control.Validated" /> event handlers, this method will always return <see langword="false" />.</returns>
		// Token: 0x06000BE8 RID: 3048 RVA: 0x00026403 File Offset: 0x00024603
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			if (validationConstraints < ValidationConstraints.None || validationConstraints > (ValidationConstraints.Selectable | ValidationConstraints.Enabled | ValidationConstraints.Visible | ValidationConstraints.TabStop | ValidationConstraints.ImmediateChildren))
			{
				throw new InvalidEnumArgumentException("validationConstraints", (int)validationConstraints, typeof(ValidationConstraints));
			}
			return !base.PerformContainerValidation(validationConstraints);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00026430 File Offset: 0x00024630
		private bool ValidateThroughAncestor(Control ancestorControl, bool preventFocusChangeOnError)
		{
			if (ancestorControl == null)
			{
				ancestorControl = this;
			}
			if (this.state[ContainerControl.stateValidating])
			{
				return false;
			}
			if (this.unvalidatedControl == null)
			{
				this.unvalidatedControl = this.focusedControl;
			}
			if (this.unvalidatedControl == null)
			{
				return true;
			}
			if (!ancestorControl.IsDescendant(this.unvalidatedControl))
			{
				return false;
			}
			this.state[ContainerControl.stateValidating] = true;
			bool flag = false;
			Control control = this.activeControl;
			Control parentInternal = this.unvalidatedControl;
			if (control != null)
			{
				control.ValidationCancelled = false;
				if (control is ContainerControl)
				{
					ContainerControl containerControl = control as ContainerControl;
					containerControl.ResetValidationFlag();
				}
			}
			try
			{
				while (parentInternal != null && parentInternal != ancestorControl)
				{
					try
					{
						flag = parentInternal.PerformControlValidation(false);
					}
					catch
					{
						flag = true;
						throw;
					}
					if (flag)
					{
						break;
					}
					parentInternal = parentInternal.ParentInternal;
				}
				if (flag && preventFocusChangeOnError)
				{
					if (this.unvalidatedControl == null && parentInternal != null && ancestorControl.IsDescendant(parentInternal))
					{
						this.unvalidatedControl = parentInternal;
					}
					if (control == this.activeControl && control != null)
					{
						control.NotifyValidationResult(parentInternal, new CancelEventArgs
						{
							Cancel = true
						});
						if (control is ContainerControl)
						{
							ContainerControl containerControl2 = control as ContainerControl;
							if (containerControl2.focusedControl != null)
							{
								containerControl2.focusedControl.ValidationCancelled = true;
							}
							containerControl2.ResetActiveAndFocusedControlsRecursive();
						}
					}
					this.SetActiveControlInternal(this.unvalidatedControl);
				}
			}
			finally
			{
				this.unvalidatedControl = null;
				this.state[ContainerControl.stateValidating] = false;
			}
			return !flag;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000265A4 File Offset: 0x000247A4
		private void ResetValidationFlag()
		{
			Control.ControlCollection controls = base.Controls;
			int count = controls.Count;
			for (int i = 0; i < count; i++)
			{
				controls[i].ValidationCancelled = false;
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000265D8 File Offset: 0x000247D8
		internal void ResetActiveAndFocusedControlsRecursive()
		{
			if (this.activeControl is ContainerControl)
			{
				((ContainerControl)this.activeControl).ResetActiveAndFocusedControlsRecursive();
			}
			this.activeControl = null;
			this.focusedControl = null;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00026605 File Offset: 0x00024805
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAutoValidate()
		{
			return this.autoValidate != AutoValidate.Inherit;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00026614 File Offset: 0x00024814
		private void WmSetFocus(ref Message m)
		{
			if (base.HostedInWin32DialogManager)
			{
				base.WndProc(ref m);
				return;
			}
			if (this.ActiveControl != null)
			{
				base.WmImeSetFocus();
				if (!this.ActiveControl.Visible)
				{
					base.InvokeGotFocus(this, EventArgs.Empty);
				}
				this.FocusActiveControlInternal();
				return;
			}
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					bool flag = false;
					ContainerControl containerControl = containerControlInternal as ContainerControl;
					if (containerControl != null)
					{
						flag = containerControl.ActivateControlInternal(this);
					}
					else
					{
						IntSecurity.ModifyFocus.Assert();
						try
						{
							flag = containerControlInternal.ActivateControl(this);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (!flag)
					{
						return;
					}
				}
			}
			base.WndProc(ref m);
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06000BEE RID: 3054 RVA: 0x000266C4 File Offset: 0x000248C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 7)
			{
				this.WmSetFocus(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04000754 RID: 1876
		private Control activeControl;

		// Token: 0x04000755 RID: 1877
		private Control focusedControl;

		// Token: 0x04000756 RID: 1878
		private Control unvalidatedControl;

		// Token: 0x04000757 RID: 1879
		private AutoValidate autoValidate = AutoValidate.Inherit;

		// Token: 0x04000758 RID: 1880
		private EventHandler autoValidateChanged;

		// Token: 0x04000759 RID: 1881
		private SizeF autoScaleDimensions = SizeF.Empty;

		// Token: 0x0400075A RID: 1882
		private SizeF currentAutoScaleDimensions = SizeF.Empty;

		// Token: 0x0400075B RID: 1883
		private AutoScaleMode autoScaleMode = AutoScaleMode.Inherit;

		// Token: 0x0400075C RID: 1884
		private BitVector32 state;

		// Token: 0x0400075D RID: 1885
		private static readonly int stateScalingNeededOnLayout = BitVector32.CreateMask();

		// Token: 0x0400075E RID: 1886
		private static readonly int stateValidating = BitVector32.CreateMask(ContainerControl.stateScalingNeededOnLayout);

		// Token: 0x0400075F RID: 1887
		private static readonly int stateProcessingMnemonic = BitVector32.CreateMask(ContainerControl.stateValidating);

		// Token: 0x04000760 RID: 1888
		private static readonly int stateScalingChild = BitVector32.CreateMask(ContainerControl.stateProcessingMnemonic);

		// Token: 0x04000761 RID: 1889
		private static readonly int stateParentChanged = BitVector32.CreateMask(ContainerControl.stateScalingChild);

		// Token: 0x04000762 RID: 1890
		private static readonly int PropAxContainer = PropertyStore.CreateKey();

		// Token: 0x04000763 RID: 1891
		private const string fontMeasureString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	}
}
