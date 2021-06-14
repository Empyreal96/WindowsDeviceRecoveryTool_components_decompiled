using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Internal;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Automation;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Accessibility;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Defines the base class for controls, which are components with visual representation.</summary>
	// Token: 0x02000159 RID: 345
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Text")]
	[DefaultEvent("Click")]
	[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItemFilter("System.Windows.Forms")]
	public class Control : Component, UnsafeNativeMethods.IOleControl, UnsafeNativeMethods.IOleObject, UnsafeNativeMethods.IOleInPlaceObject, UnsafeNativeMethods.IOleInPlaceActiveObject, UnsafeNativeMethods.IOleWindow, UnsafeNativeMethods.IViewObject, UnsafeNativeMethods.IViewObject2, UnsafeNativeMethods.IPersist, UnsafeNativeMethods.IPersistStreamInit, UnsafeNativeMethods.IPersistPropertyBag, UnsafeNativeMethods.IPersistStorage, UnsafeNativeMethods.IQuickActivate, ISupportOleDropSource, IDropTarget, ISynchronizeInvoke, IWin32Window, IArrangedElement, IComponent, IDisposable, IBindableComponent, IKeyboardToolTip
	{
		// Token: 0x06000C0A RID: 3082 RVA: 0x00026B14 File Offset: 0x00024D14
		static Control()
		{
			Control.WM_GETCONTROLNAME = SafeNativeMethods.RegisterWindowMessage("WM_GETCONTROLNAME");
			Control.WM_GETCONTROLTYPE = SafeNativeMethods.RegisterWindowMessage("WM_GETCONTROLTYPE");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control" /> class with default settings.</summary>
		// Token: 0x06000C0B RID: 3083 RVA: 0x00026FC3 File Offset: 0x000251C3
		public Control() : this(true)
		{
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00026FCC File Offset: 0x000251CC
		internal Control(bool autoInstallSyncContext)
		{
			this.propertyStore = new PropertyStore();
			DpiHelper.InitializeDpiHelperForWinforms();
			this.deviceDpi = DpiHelper.DeviceDpi;
			this.window = new Control.ControlNativeWindow(this);
			this.RequiredScalingEnabled = true;
			this.RequiredScaling = BoundsSpecified.All;
			this.tabIndex = -1;
			this.state = 131086;
			this.state2 = 8;
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.Selectable | ControlStyles.StandardDoubleClick | ControlStyles.AllPaintingInWmPaint | ControlStyles.UseTextForAccessibility, true);
			this.InitMouseWheelSupport();
			if (this.DefaultMargin != CommonProperties.DefaultMargin)
			{
				this.Margin = this.DefaultMargin;
			}
			if (this.DefaultMinimumSize != CommonProperties.DefaultMinimumSize)
			{
				this.MinimumSize = this.DefaultMinimumSize;
			}
			if (this.DefaultMaximumSize != CommonProperties.DefaultMaximumSize)
			{
				this.MaximumSize = this.DefaultMaximumSize;
			}
			Size defaultSize = this.DefaultSize;
			this.width = defaultSize.Width;
			this.height = defaultSize.Height;
			CommonProperties.xClearPreferredSizeCache(this);
			if (this.width != 0 && this.height != 0)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				rect.left = (rect.right = (rect.top = (rect.bottom = 0)));
				CreateParams createParams = this.CreateParams;
				this.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
				this.clientWidth = this.width - (rect.right - rect.left);
				this.clientHeight = this.height - (rect.bottom - rect.top);
			}
			if (autoInstallSyncContext)
			{
				WindowsFormsSynchronizationContext.InstallIfNeeded();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control" /> class with specific text.</summary>
		/// <param name="text">The text displayed by the control. </param>
		// Token: 0x06000C0D RID: 3085 RVA: 0x00027166 File Offset: 0x00025366
		public Control(string text) : this(null, text)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control" /> class with specific text, size, and location.</summary>
		/// <param name="text">The text displayed by the control. </param>
		/// <param name="left">The <see cref="P:System.Drawing.Point.X" /> position of the control, in pixels, from the left edge of the control's container. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Left" /> property. </param>
		/// <param name="top">The <see cref="P:System.Drawing.Point.Y" /> position of the control, in pixels, from the top edge of the control's container. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Top" /> property. </param>
		/// <param name="width">The width of the control, in pixels. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Width" /> property. </param>
		/// <param name="height">The height of the control, in pixels. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Height" /> property. </param>
		// Token: 0x06000C0E RID: 3086 RVA: 0x00027170 File Offset: 0x00025370
		public Control(string text, int left, int top, int width, int height) : this(null, text, left, top, width, height)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control" /> class as a child control, with specific text.</summary>
		/// <param name="parent">The <see cref="T:System.Windows.Forms.Control" /> to be the parent of the control. </param>
		/// <param name="text">The text displayed by the control. </param>
		// Token: 0x06000C0F RID: 3087 RVA: 0x00027180 File Offset: 0x00025380
		public Control(Control parent, string text) : this()
		{
			this.Parent = parent;
			this.Text = text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control" /> class as a child control, with specific text, size, and location.</summary>
		/// <param name="parent">The <see cref="T:System.Windows.Forms.Control" /> to be the parent of the control. </param>
		/// <param name="text">The text displayed by the control. </param>
		/// <param name="left">The <see cref="P:System.Drawing.Point.X" /> position of the control, in pixels, from the left edge of the control's container. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Left" /> property. </param>
		/// <param name="top">The <see cref="P:System.Drawing.Point.Y" /> position of the control, in pixels, from the top edge of the control's container. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Top" /> property. </param>
		/// <param name="width">The width of the control, in pixels. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Width" /> property. </param>
		/// <param name="height">The height of the control, in pixels. The value is assigned to the <see cref="P:System.Windows.Forms.Control.Height" /> property. </param>
		// Token: 0x06000C10 RID: 3088 RVA: 0x00027196 File Offset: 0x00025396
		public Control(Control parent, string text, int left, int top, int width, int height) : this(parent, text)
		{
			this.Location = new Point(left, top);
			this.Size = new Size(width, height);
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x000271BD File Offset: 0x000253BD
		internal DpiAwarenessContext DpiAwarenessContext
		{
			get
			{
				return this.window.DpiAwarenessContext;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.AccessibleObject" /> assigned to the control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.AccessibleObject" /> assigned to the control.</returns>
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x000271CC File Offset: 0x000253CC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlAccessibilityObjectDescr")]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(Control.PropAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					if (!(accessibleObject is Control.ControlAccessibleObject))
					{
						return null;
					}
					this.Properties.SetObject(Control.PropAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00027218 File Offset: 0x00025418
		private AccessibleObject NcAccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(Control.PropNcAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = new Control.ControlAccessibleObject(this, 0);
					this.Properties.SetObject(Control.PropNcAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00027258 File Offset: 0x00025458
		private AccessibleObject GetAccessibilityObject(int accObjId)
		{
			AccessibleObject result;
			if (accObjId != -4)
			{
				if (accObjId != 0)
				{
					if (accObjId > 0)
					{
						result = this.GetAccessibilityObjectById(accObjId);
					}
					else
					{
						result = null;
					}
				}
				else
				{
					result = this.NcAccessibilityObject;
				}
			}
			else
			{
				result = this.AccessibilityObject;
			}
			return result;
		}

		/// <summary>Retrieves the specified <see cref="T:System.Windows.Forms.AccessibleObject" />.</summary>
		/// <param name="objectId">An <see langword="Int32" /> that identifies the <see cref="T:System.Windows.Forms.AccessibleObject" /> to retrieve.</param>
		/// <returns>The specified <see cref="T:System.Windows.Forms.AccessibleObject" />.</returns>
		// Token: 0x06000C15 RID: 3093 RVA: 0x00027292 File Offset: 0x00025492
		protected virtual AccessibleObject GetAccessibilityObjectById(int objectId)
		{
			if (AccessibilityImprovements.Level3 && this is IAutomationLiveRegion)
			{
				return this.AccessibilityObject;
			}
			return null;
		}

		/// <summary>Gets or sets the default action description of the control for use by accessibility client applications.</summary>
		/// <returns>The default action description of the control for use by accessibility client applications.</returns>
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x000272AB File Offset: 0x000254AB
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x000272C2 File Offset: 0x000254C2
		[SRCategory("CatAccessibility")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlAccessibleDefaultActionDescr")]
		public string AccessibleDefaultActionDescription
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleDefaultActionDescription);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleDefaultActionDescription, value);
			}
		}

		/// <summary>Gets or sets the description of the control used by accessibility client applications.</summary>
		/// <returns>The description of the control used by accessibility client applications. The default is <see langword="null" />.</returns>
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000272D5 File Offset: 0x000254D5
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x000272EC File Offset: 0x000254EC
		[SRCategory("CatAccessibility")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ControlAccessibleDescriptionDescr")]
		public string AccessibleDescription
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleDescription);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleDescription, value);
			}
		}

		/// <summary>Gets or sets the name of the control used by accessibility client applications.</summary>
		/// <returns>The name of the control used by accessibility client applications. The default is <see langword="null" />.</returns>
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x000272FF File Offset: 0x000254FF
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x00027316 File Offset: 0x00025516
		[SRCategory("CatAccessibility")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ControlAccessibleNameDescr")]
		public string AccessibleName
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleName);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleName, value);
			}
		}

		/// <summary>Gets or sets the accessible role of the control </summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AccessibleRole" />. The default is <see langword="Default" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values. </exception>
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0002732C File Offset: 0x0002552C
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x00027352 File Offset: 0x00025552
		[SRCategory("CatAccessibility")]
		[DefaultValue(AccessibleRole.Default)]
		[SRDescription("ControlAccessibleRoleDescr")]
		public AccessibleRole AccessibleRole
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropAccessibleRole, out flag);
				if (flag)
				{
					return (AccessibleRole)integer;
				}
				return AccessibleRole.Default;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 64))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AccessibleRole));
				}
				this.Properties.SetInteger(Control.PropAccessibleRole, (int)value);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0002738C File Offset: 0x0002558C
		private Color ActiveXAmbientBackColor
		{
			get
			{
				return this.ActiveXInstance.AmbientBackColor;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00027399 File Offset: 0x00025599
		private Color ActiveXAmbientForeColor
		{
			get
			{
				return this.ActiveXInstance.AmbientForeColor;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x000273A6 File Offset: 0x000255A6
		private Font ActiveXAmbientFont
		{
			get
			{
				return this.ActiveXInstance.AmbientFont;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x000273B3 File Offset: 0x000255B3
		private bool ActiveXEventsFrozen
		{
			get
			{
				return this.ActiveXInstance.EventsFrozen;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x000273C0 File Offset: 0x000255C0
		private IntPtr ActiveXHWNDParent
		{
			get
			{
				return this.ActiveXInstance.HWNDParent;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x000273D0 File Offset: 0x000255D0
		private Control.ActiveXImpl ActiveXInstance
		{
			get
			{
				Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)this.Properties.GetObject(Control.PropActiveXImpl);
				if (activeXImpl == null)
				{
					if (this.GetState(524288))
					{
						throw new NotSupportedException(SR.GetString("AXTopLevelSource"));
					}
					activeXImpl = new Control.ActiveXImpl(this);
					this.SetState2(1024, true);
					this.Properties.SetObject(Control.PropActiveXImpl, activeXImpl);
				}
				return activeXImpl;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control can accept data that the user drags onto it.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop operations are allowed in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00027438 File Offset: 0x00025638
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00027444 File Offset: 0x00025644
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ControlAllowDropDescr")]
		public virtual bool AllowDrop
		{
			get
			{
				return this.GetState(64);
			}
			set
			{
				if (this.GetState(64) != value)
				{
					if (value && !this.IsHandleCreated)
					{
						IntSecurity.ClipboardRead.Demand();
					}
					this.SetState(64, value);
					if (this.IsHandleCreated)
					{
						try
						{
							this.SetAcceptDrops(value);
						}
						catch
						{
							this.SetState(64, !value);
							throw;
						}
					}
				}
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000274AC File Offset: 0x000256AC
		private AmbientProperties AmbientPropertiesService
		{
			get
			{
				bool flag;
				AmbientProperties ambientProperties = (AmbientProperties)this.Properties.GetObject(Control.PropAmbientPropertiesService, out flag);
				if (!flag)
				{
					if (this.Site != null)
					{
						ambientProperties = (AmbientProperties)this.Site.GetService(typeof(AmbientProperties));
					}
					else
					{
						ambientProperties = (AmbientProperties)this.GetService(typeof(AmbientProperties));
					}
					if (ambientProperties != null)
					{
						this.Properties.SetObject(Control.PropAmbientPropertiesService, ambientProperties);
					}
				}
				return ambientProperties;
			}
		}

		/// <summary>Gets or sets the edges of the container to which a control is bound and determines how a control is resized with its parent. </summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values. The default is <see langword="Top" /> and <see langword="Left" />.</returns>
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00027524 File Offset: 0x00025724
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0002752C File Offset: 0x0002572C
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
		[SRDescription("ControlAnchorDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual AnchorStyles Anchor
		{
			get
			{
				return DefaultLayout.GetAnchor(this);
			}
			set
			{
				DefaultLayout.SetAnchor(this.ParentInternal, this, value);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0002753B File Offset: 0x0002573B
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00027544 File Offset: 0x00025744
		[SRCategory("CatLayout")]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlAutoSizeDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AutoSize
		{
			get
			{
				return CommonProperties.GetAutoSize(this);
			}
			set
			{
				if (value != this.AutoSize)
				{
					CommonProperties.SetAutoSize(this, value);
					if (this.ParentInternal != null)
					{
						if (value && this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06000C2B RID: 3115 RVA: 0x000275AD File Offset: 0x000257AD
		// (remove) Token: 0x06000C2C RID: 3116 RVA: 0x000275C0 File Offset: 0x000257C0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler AutoSizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventAutoSizeChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventAutoSizeChanged, value);
			}
		}

		/// <summary>Gets or sets where this control is scrolled to in <see cref="M:System.Windows.Forms.ScrollableControl.ScrollControlIntoView(System.Windows.Forms.Control)" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> specifying the scroll location. The default is the upper-left corner of the control.</returns>
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x000275D3 File Offset: 0x000257D3
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x00027602 File Offset: 0x00025802
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(typeof(Point), "0, 0")]
		public virtual Point AutoScrollOffset
		{
			get
			{
				if (this.Properties.ContainsObject(Control.PropAutoScrollOffset))
				{
					return (Point)this.Properties.GetObject(Control.PropAutoScrollOffset);
				}
				return Point.Empty;
			}
			set
			{
				if (this.AutoScrollOffset != value)
				{
					this.Properties.SetObject(Control.PropAutoScrollOffset, value);
				}
			}
		}

		/// <summary>Sets a value indicating how a control will behave when its <see cref="P:System.Windows.Forms.Control.AutoSize" /> property is enabled.</summary>
		/// <param name="mode">One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values.</param>
		// Token: 0x06000C2F RID: 3119 RVA: 0x00027628 File Offset: 0x00025828
		protected void SetAutoSizeMode(AutoSizeMode mode)
		{
			CommonProperties.SetAutoSizeMode(this, mode);
		}

		/// <summary>Retrieves a value indicating how a control will behave when its <see cref="P:System.Windows.Forms.Control.AutoSize" /> property is enabled.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values. </returns>
		// Token: 0x06000C30 RID: 3120 RVA: 0x00027631 File Offset: 0x00025831
		protected AutoSizeMode GetAutoSizeMode()
		{
			return CommonProperties.GetAutoSizeMode(this);
		}

		/// <summary>Gets a cached instance of the control's layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> for the control's contents.</returns>
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00027639 File Offset: 0x00025839
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual LayoutEngine LayoutEngine
		{
			get
			{
				return DefaultLayout.Instance;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00027640 File Offset: 0x00025840
		internal IntPtr BackColorBrush
		{
			get
			{
				object @object = this.Properties.GetObject(Control.PropBackBrush);
				if (@object != null)
				{
					return (IntPtr)@object;
				}
				if (!this.Properties.ContainsObject(Control.PropBackColor) && this.parent != null && this.parent.BackColor == this.BackColor)
				{
					return this.parent.BackColorBrush;
				}
				Color backColor = this.BackColor;
				IntPtr intPtr;
				if (ColorTranslator.ToOle(backColor) < 0)
				{
					intPtr = SafeNativeMethods.GetSysColorBrush(ColorTranslator.ToOle(backColor) & 255);
					this.SetState(2097152, false);
				}
				else
				{
					intPtr = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
					this.SetState(2097152, true);
				}
				this.Properties.SetObject(Control.PropBackBrush, intPtr);
				return intPtr;
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00027708 File Offset: 0x00025908
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x00027790 File Offset: 0x00025990
		[SRCategory("CatAppearance")]
		[DispId(-501)]
		[SRDescription("ControlBackColorDescr")]
		public virtual Color BackColor
		{
			get
			{
				Color color = this.RawBackColor;
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					color = parentInternal.BackColor;
					if (this.IsValidBackColor(color))
					{
						return color;
					}
				}
				if (this.IsActiveX)
				{
					color = this.ActiveXAmbientBackColor;
				}
				if (color.IsEmpty)
				{
					AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
					if (ambientPropertiesService != null)
					{
						color = ambientPropertiesService.BackColor;
					}
				}
				if (!color.IsEmpty && this.IsValidBackColor(color))
				{
					return color;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				if (!value.Equals(Color.Empty) && !this.GetStyle(ControlStyles.SupportsTransparentBackColor) && value.A < 255)
				{
					throw new ArgumentException(SR.GetString("TransparentBackColorNotAllowed"));
				}
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(Control.PropBackColor))
				{
					this.Properties.SetColor(Control.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.BackColor" /> property changes.</summary>
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06000C35 RID: 3125 RVA: 0x00027839 File Offset: 0x00025A39
		// (remove) Token: 0x06000C36 RID: 3126 RVA: 0x0002784C File Offset: 0x00025A4C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBackColorChangedDescr")]
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackColor, value);
			}
		}

		/// <summary>Gets or sets the background image displayed in the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0002785F File Offset: 0x00025A5F
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00027876 File Offset: 0x00025A76
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageDescr")]
		public virtual Image BackgroundImage
		{
			get
			{
				return (Image)this.Properties.GetObject(Control.PropBackgroundImage);
			}
			set
			{
				if (this.BackgroundImage != value)
				{
					this.Properties.SetObject(Control.PropBackgroundImage, value);
					this.OnBackgroundImageChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.BackgroundImage" /> property changes.</summary>
		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06000C39 RID: 3129 RVA: 0x0002789D File Offset: 0x00025A9D
		// (remove) Token: 0x06000C3A RID: 3130 RVA: 0x000278B0 File Offset: 0x00025AB0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBackgroundImageChangedDescr")]
		public event EventHandler BackgroundImageChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackgroundImage, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackgroundImage, value);
			}
		}

		/// <summary>Gets or sets the background image layout as defined in the <see cref="T:System.Windows.Forms.ImageLayout" /> enumeration.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" /> (<see cref="F:System.Windows.Forms.ImageLayout.Center" /> , <see cref="F:System.Windows.Forms.ImageLayout.None" />, <see cref="F:System.Windows.Forms.ImageLayout.Stretch" />, <see cref="F:System.Windows.Forms.ImageLayout.Tile" />, or <see cref="F:System.Windows.Forms.ImageLayout.Zoom" />). <see cref="F:System.Windows.Forms.ImageLayout.Tile" /> is the default value.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified enumeration value does not exist. </exception>
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x000278C4 File Offset: 0x00025AC4
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x000278FC File Offset: 0x00025AFC
		[SRCategory("CatAppearance")]
		[DefaultValue(ImageLayout.Tile)]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		public virtual ImageLayout BackgroundImageLayout
		{
			get
			{
				if (!this.Properties.ContainsObject(Control.PropBackgroundImageLayout))
				{
					return ImageLayout.Tile;
				}
				return (ImageLayout)this.Properties.GetObject(Control.PropBackgroundImageLayout);
			}
			set
			{
				if (this.BackgroundImageLayout != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ImageLayout));
					}
					if (value == ImageLayout.Center || value == ImageLayout.Zoom || value == ImageLayout.Stretch)
					{
						this.SetStyle(ControlStyles.ResizeRedraw, true);
						if (ControlPaint.IsImageTransparent(this.BackgroundImage))
						{
							this.DoubleBuffered = true;
						}
					}
					this.Properties.SetObject(Control.PropBackgroundImageLayout, value);
					this.OnBackgroundImageLayoutChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06000C3D RID: 3133 RVA: 0x00027982 File Offset: 0x00025B82
		// (remove) Token: 0x06000C3E RID: 3134 RVA: 0x00027995 File Offset: 0x00025B95
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBackgroundImageLayoutChangedDescr")]
		public event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackgroundImageLayout, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackgroundImageLayout, value);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000279A8 File Offset: 0x00025BA8
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x000279B2 File Offset: 0x00025BB2
		internal bool BecomingActiveControl
		{
			get
			{
				return this.GetState2(32);
			}
			set
			{
				if (value != this.BecomingActiveControl)
				{
					Application.ThreadContext.FromCurrent().ActivatingControl = (value ? this : null);
					this.SetState2(32, value);
				}
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000279D8 File Offset: 0x00025BD8
		private bool ShouldSerializeAccessibleName()
		{
			string accessibleName = this.AccessibleName;
			return accessibleName != null && accessibleName.Length > 0;
		}

		/// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread all the items in the list and refresh their displayed values.</summary>
		// Token: 0x06000C42 RID: 3138 RVA: 0x000279FC File Offset: 0x00025BFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBindings()
		{
			ControlBindingsCollection controlBindingsCollection = (ControlBindingsCollection)this.Properties.GetObject(Control.PropBindings);
			if (controlBindingsCollection != null)
			{
				controlBindingsCollection.Clear();
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00027A28 File Offset: 0x00025C28
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00027A6C File Offset: 0x00025C6C
		internal BindingContext BindingContextInternal
		{
			get
			{
				BindingContext bindingContext = (BindingContext)this.Properties.GetObject(Control.PropBindingManager);
				if (bindingContext != null)
				{
					return bindingContext;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.BindingContext;
				}
				return null;
			}
			set
			{
				BindingContext bindingContext = (BindingContext)this.Properties.GetObject(Control.PropBindingManager);
				if (bindingContext != value)
				{
					this.Properties.SetObject(Control.PropBindingManager, value);
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.BindingContext" /> for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingContext" /> for the control.</returns>
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00027AB1 File Offset: 0x00025CB1
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00027AB9 File Offset: 0x00025CB9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlBindingContextDescr")]
		public virtual BindingContext BindingContext
		{
			get
			{
				return this.BindingContextInternal;
			}
			set
			{
				this.BindingContextInternal = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="T:System.Windows.Forms.BindingContext" /> property changes.</summary>
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06000C47 RID: 3143 RVA: 0x00027AC2 File Offset: 0x00025CC2
		// (remove) Token: 0x06000C48 RID: 3144 RVA: 0x00027AD5 File Offset: 0x00025CD5
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBindingContextChangedDescr")]
		public event EventHandler BindingContextChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBindingContext, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBindingContext, value);
			}
		}

		/// <summary>Gets the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.</returns>
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00027AE8 File Offset: 0x00025CE8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlBottomDescr")]
		[SRCategory("CatLayout")]
		public int Bottom
		{
			get
			{
				return this.y + this.height;
			}
		}

		/// <summary>Gets or sets the size and location of the control including its nonclient elements, in pixels, relative to the parent control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> in pixels relative to the parent control that represents the size and location of the control including its nonclient elements.</returns>
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00027AF7 File Offset: 0x00025CF7
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x00027B16 File Offset: 0x00025D16
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlBoundsDescr")]
		[SRCategory("CatLayout")]
		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(this.x, this.y, this.width, this.height);
			}
			set
			{
				this.SetBounds(value.X, value.Y, value.Width, value.Height, BoundsSpecified.All);
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool CanAccessProperties
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the control can receive focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can receive focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00027B3C File Offset: 0x00025D3C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatFocus")]
		[SRDescription("ControlCanFocusDescr")]
		public bool CanFocus
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return false;
				}
				bool flag = SafeNativeMethods.IsWindowVisible(new HandleRef(this.window, this.Handle));
				bool flag2 = SafeNativeMethods.IsWindowEnabled(new HandleRef(this.window, this.Handle));
				return flag && flag2;
			}
		}

		/// <summary>Determines if events can be raised on the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is hosted as an ActiveX control whose events are not frozen; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00027B84 File Offset: 0x00025D84
		protected override bool CanRaiseEvents
		{
			get
			{
				return !this.IsActiveX || !this.ActiveXEventsFrozen;
			}
		}

		/// <summary>Gets a value indicating whether the control can be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can be selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00027B99 File Offset: 0x00025D99
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatFocus")]
		[SRDescription("ControlCanSelectDescr")]
		public bool CanSelect
		{
			get
			{
				return this.CanSelectCore();
			}
		}

		/// <summary>Gets or sets a value indicating whether the control has captured the mouse.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has captured the mouse; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00027BA1 File Offset: 0x00025DA1
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00027BA9 File Offset: 0x00025DA9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatFocus")]
		[SRDescription("ControlCaptureDescr")]
		public bool Capture
		{
			get
			{
				return this.CaptureInternal;
			}
			set
			{
				if (value)
				{
					IntSecurity.GetCapture.Demand();
				}
				this.CaptureInternal = value;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00027BBF File Offset: 0x00025DBF
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x00027BDB File Offset: 0x00025DDB
		internal bool CaptureInternal
		{
			get
			{
				return this.IsHandleCreated && UnsafeNativeMethods.GetCapture() == this.Handle;
			}
			set
			{
				if (this.CaptureInternal != value)
				{
					if (value)
					{
						UnsafeNativeMethods.SetCapture(new HandleRef(this, this.Handle));
						return;
					}
					SafeNativeMethods.ReleaseCapture();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the control causes validation to be performed on any controls that require validation when it receives focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control causes validation to be performed on any controls requiring validation when it receives focus; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00027C02 File Offset: 0x00025E02
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x00027C0F File Offset: 0x00025E0F
		[SRCategory("CatFocus")]
		[DefaultValue(true)]
		[SRDescription("ControlCausesValidationDescr")]
		public bool CausesValidation
		{
			get
			{
				return this.GetState(131072);
			}
			set
			{
				if (value != this.CausesValidation)
				{
					this.SetState(131072, value);
					this.OnCausesValidationChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.CausesValidation" /> property changes.</summary>
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06000C56 RID: 3158 RVA: 0x00027C31 File Offset: 0x00025E31
		// (remove) Token: 0x06000C57 RID: 3159 RVA: 0x00027C44 File Offset: 0x00025E44
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnCausesValidationChangedDescr")]
		public event EventHandler CausesValidationChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventCausesValidation, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventCausesValidation, value);
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00027C58 File Offset: 0x00025E58
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00027C8C File Offset: 0x00025E8C
		internal bool CacheTextInternal
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropCacheTextCount, out flag);
				return integer > 0 || this.GetStyle(ControlStyles.CacheText);
			}
			set
			{
				if (this.GetStyle(ControlStyles.CacheText) || !this.IsHandleCreated)
				{
					return;
				}
				bool flag;
				int num = this.Properties.GetInteger(Control.PropCacheTextCount, out flag);
				if (value)
				{
					if (num == 0)
					{
						this.Properties.SetObject(Control.PropCacheTextField, this.text);
						if (this.text == null)
						{
							this.text = this.WindowText;
						}
					}
					num++;
				}
				else
				{
					num--;
					if (num == 0)
					{
						this.text = (string)this.Properties.GetObject(Control.PropCacheTextField, out flag);
					}
				}
				this.Properties.SetInteger(Control.PropCacheTextCount, num);
			}
		}

		/// <summary>Gets or sets a value indicating whether to catch calls on the wrong thread that access a control's <see cref="P:System.Windows.Forms.Control.Handle" /> property when an application is being debugged.</summary>
		/// <returns>
		///     <see langword="true" /> if calls on the wrong thread are caught; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00027D2D File Offset: 0x00025F2D
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x00027D34 File Offset: 0x00025F34
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlCheckForIllegalCrossThreadCalls")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool CheckForIllegalCrossThreadCalls
		{
			get
			{
				return Control.checkForIllegalCrossThreadCalls;
			}
			set
			{
				Control.checkForIllegalCrossThreadCalls = value;
			}
		}

		/// <summary>Gets the rectangle that represents the client area of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the client area of the control.</returns>
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00027D3C File Offset: 0x00025F3C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[SRDescription("ControlClientRectangleDescr")]
		public Rectangle ClientRectangle
		{
			get
			{
				return new Rectangle(0, 0, this.clientWidth, this.clientHeight);
			}
		}

		/// <summary>Gets or sets the height and width of the client area of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the dimensions of the client area of the control.</returns>
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00027D51 File Offset: 0x00025F51
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x00027D64 File Offset: 0x00025F64
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlClientSizeDescr")]
		public Size ClientSize
		{
			get
			{
				return new Size(this.clientWidth, this.clientHeight);
			}
			set
			{
				this.SetClientSizeCore(value.Width, value.Height);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ClientSize" /> property changes. </summary>
		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06000C5F RID: 3167 RVA: 0x00027D7A File Offset: 0x00025F7A
		// (remove) Token: 0x06000C60 RID: 3168 RVA: 0x00027D8D File Offset: 0x00025F8D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnClientSizeChangedDescr")]
		public event EventHandler ClientSizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventClientSize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventClientSize, value);
			}
		}

		/// <summary>Gets the name of the company or creator of the application containing the control.</summary>
		/// <returns>The company name or creator of the application containing the control.</returns>
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00027DA0 File Offset: 0x00025FA0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Description("ControlCompanyNameDescr")]
		public string CompanyName
		{
			get
			{
				return this.VersionInfo.CompanyName;
			}
		}

		/// <summary>Gets a value indicating whether the control, or one of its child controls, currently has the input focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control or one of its child controls currently has the input focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00027DB0 File Offset: 0x00025FB0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlContainsFocusDescr")]
		public bool ContainsFocus
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return false;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return !(focus == IntPtr.Zero) && (focus == this.Handle || UnsafeNativeMethods.IsChild(new HandleRef(this, this.Handle), new HandleRef(this, focus)));
			}
		}

		/// <summary>Gets or sets the shortcut menu associated with the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenu" /> that represents the shortcut menu associated with the control.</returns>
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00027E09 File Offset: 0x00026009
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00027E20 File Offset: 0x00026020
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ControlContextMenuDescr")]
		[Browsable(false)]
		public virtual ContextMenu ContextMenu
		{
			get
			{
				return (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			}
			set
			{
				ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
				if (contextMenu != value)
				{
					EventHandler value2 = new EventHandler(this.DetachContextMenu);
					if (contextMenu != null)
					{
						contextMenu.Disposed -= value2;
					}
					this.Properties.SetObject(Control.PropContextMenu, value);
					if (value != null)
					{
						value.Disposed += value2;
					}
					this.OnContextMenuChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ContextMenu" /> property changes.</summary>
		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06000C65 RID: 3173 RVA: 0x00027E84 File Offset: 0x00026084
		// (remove) Token: 0x06000C66 RID: 3174 RVA: 0x00027E97 File Offset: 0x00026097
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnContextMenuChangedDescr")]
		[Browsable(false)]
		public event EventHandler ContextMenuChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventContextMenu, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventContextMenu, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with this control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> for this control, or <see langword="null" /> if there is no <see cref="T:System.Windows.Forms.ContextMenuStrip" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00027EAA File Offset: 0x000260AA
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x00027EC4 File Offset: 0x000260C4
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ControlContextMenuDescr")]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(Control.PropContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = this.Properties.GetObject(Control.PropContextMenuStrip) as ContextMenuStrip;
				if (contextMenuStrip != value)
				{
					EventHandler value2 = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= value2;
					}
					this.Properties.SetObject(Control.PropContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += value2;
					}
					this.OnContextMenuStripChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.ContextMenuStrip" /> property changes. </summary>
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06000C69 RID: 3177 RVA: 0x00027F28 File Offset: 0x00026128
		// (remove) Token: 0x06000C6A RID: 3178 RVA: 0x00027F3B File Offset: 0x0002613B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlContextMenuStripChangedDescr")]
		public event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventContextMenuStrip, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventContextMenuStrip, value);
			}
		}

		/// <summary>Gets the collection of controls contained within the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control.ControlCollection" /> representing the collection of controls contained within the control.</returns>
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00027F50 File Offset: 0x00026150
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ControlControlsDescr")]
		public Control.ControlCollection Controls
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection == null)
				{
					controlCollection = this.CreateControlsInstance();
					this.Properties.SetObject(Control.PropControlsCollection, controlCollection);
				}
				return controlCollection;
			}
		}

		/// <summary>Gets a value indicating whether the control has been created.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has been created; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00027F8F File Offset: 0x0002618F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlCreatedDescr")]
		public bool Created
		{
			get
			{
				return (this.state & 1) != 0;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00027F9C File Offset: 0x0002619C
		protected virtual CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (Control.needToLoadComCtl)
				{
					if (!(UnsafeNativeMethods.GetModuleHandle("comctl32.dll") != IntPtr.Zero) && !(UnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable("comctl32.dll") != IntPtr.Zero))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						throw new Win32Exception(lastWin32Error, SR.GetString("LoadDLLError", new object[]
						{
							"comctl32.dll"
						}));
					}
					Control.needToLoadComCtl = false;
				}
				if (this.createParams == null)
				{
					this.createParams = new CreateParams();
				}
				CreateParams createParams = this.createParams;
				createParams.Style = 0;
				createParams.ExStyle = 0;
				createParams.ClassStyle = 0;
				createParams.Caption = this.text;
				createParams.X = this.x;
				createParams.Y = this.y;
				createParams.Width = this.width;
				createParams.Height = this.height;
				createParams.Style = 33554432;
				if (this.GetStyle(ControlStyles.ContainerControl))
				{
					createParams.ExStyle |= 65536;
				}
				createParams.ClassStyle = 8;
				if ((this.state & 524288) == 0)
				{
					createParams.Parent = ((this.parent == null) ? IntPtr.Zero : this.parent.InternalHandle);
					createParams.Style |= 1140850688;
				}
				else
				{
					createParams.Parent = IntPtr.Zero;
				}
				if ((this.state & 8) != 0)
				{
					createParams.Style |= 65536;
				}
				if ((this.state & 2) != 0)
				{
					createParams.Style |= 268435456;
				}
				if (!this.Enabled)
				{
					createParams.Style |= 134217728;
				}
				if (createParams.Parent == IntPtr.Zero && this.IsActiveX)
				{
					createParams.Parent = this.ActiveXHWNDParent;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					createParams.ExStyle |= 8192;
					createParams.ExStyle |= 4096;
					createParams.ExStyle |= 16384;
				}
				return createParams;
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x000281A6 File Offset: 0x000263A6
		internal virtual void NotifyValidationResult(object sender, CancelEventArgs ev)
		{
			this.ValidationCancelled = ev.Cancel;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x000281B4 File Offset: 0x000263B4
		internal bool ValidateActiveControl(out bool validatedControlAllowsFocusChange)
		{
			bool result = true;
			validatedControlAllowsFocusChange = false;
			IContainerControl containerControlInternal = this.GetContainerControlInternal();
			if (containerControlInternal != null && this.CausesValidation)
			{
				ContainerControl containerControl = containerControlInternal as ContainerControl;
				if (containerControl != null)
				{
					while (containerControl.ActiveControl == null)
					{
						Control parentInternal = containerControl.ParentInternal;
						if (parentInternal == null)
						{
							break;
						}
						ContainerControl containerControl2 = parentInternal.GetContainerControlInternal() as ContainerControl;
						if (containerControl2 == null)
						{
							break;
						}
						containerControl = containerControl2;
					}
					result = containerControl.ValidateInternal(true, out validatedControlAllowsFocusChange);
				}
			}
			return result;
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00028224 File Offset: 0x00026424
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x00028214 File Offset: 0x00026414
		internal bool ValidationCancelled
		{
			get
			{
				if (this.GetState(268435456))
				{
					return true;
				}
				Control parentInternal = this.ParentInternal;
				return parentInternal != null && parentInternal.ValidationCancelled;
			}
			set
			{
				this.SetState(268435456, value);
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00028260 File Offset: 0x00026460
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x00028252 File Offset: 0x00026452
		internal bool IsTopMdiWindowClosing
		{
			get
			{
				return this.GetState2(4096);
			}
			set
			{
				this.SetState2(4096, value);
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0002827B File Offset: 0x0002647B
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0002826D File Offset: 0x0002646D
		internal bool IsCurrentlyBeingScaled
		{
			get
			{
				return this.GetState2(8192);
			}
			private set
			{
				this.SetState2(8192, value);
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00028288 File Offset: 0x00026488
		internal int CreateThreadId
		{
			get
			{
				if (this.IsHandleCreated)
				{
					int num;
					return SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, this.Handle), out num);
				}
				return SafeNativeMethods.GetCurrentThreadId();
			}
		}

		/// <summary>Gets or sets the cursor that is displayed when the mouse pointer is over the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor to display when the mouse pointer is over the control.</returns>
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000282B8 File Offset: 0x000264B8
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x00028340 File Offset: 0x00026540
		[SRCategory("CatAppearance")]
		[SRDescription("ControlCursorDescr")]
		[AmbientValue(null)]
		public virtual Cursor Cursor
		{
			get
			{
				if (this.GetState(1024))
				{
					return Cursors.WaitCursor;
				}
				Cursor cursor = (Cursor)this.Properties.GetObject(Control.PropCursor);
				if (cursor != null)
				{
					return cursor;
				}
				Cursor defaultCursor = this.DefaultCursor;
				if (defaultCursor != Cursors.Default)
				{
					return defaultCursor;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.Cursor;
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Cursor != null)
				{
					return ambientPropertiesService.Cursor;
				}
				return defaultCursor;
			}
			set
			{
				Cursor left = (Cursor)this.Properties.GetObject(Control.PropCursor);
				Cursor cursor = this.Cursor;
				if (left != value)
				{
					IntSecurity.ModifyCursor.Demand();
					this.Properties.SetObject(Control.PropCursor, value);
				}
				if (this.IsHandleCreated)
				{
					NativeMethods.POINT point = new NativeMethods.POINT();
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetCursorPos(point);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
					if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == this.Handle)
					{
						this.SendMessage(32, this.Handle, (IntPtr)1);
					}
				}
				if (!cursor.Equals(value))
				{
					this.OnCursorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Cursor" /> property changes.</summary>
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06000C79 RID: 3193 RVA: 0x00028435 File Offset: 0x00026635
		// (remove) Token: 0x06000C7A RID: 3194 RVA: 0x00028448 File Offset: 0x00026648
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnCursorChangedDescr")]
		public event EventHandler CursorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventCursor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventCursor, value);
			}
		}

		/// <summary>Gets the data bindings for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ControlBindingsCollection" /> that contains the <see cref="T:System.Windows.Forms.Binding" /> objects for the control.</returns>
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0002845C File Offset: 0x0002665C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatData")]
		[SRDescription("ControlBindingsDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[ParenthesizePropertyName(true)]
		public ControlBindingsCollection DataBindings
		{
			get
			{
				ControlBindingsCollection controlBindingsCollection = (ControlBindingsCollection)this.Properties.GetObject(Control.PropBindings);
				if (controlBindingsCollection == null)
				{
					controlBindingsCollection = new ControlBindingsCollection(this);
					this.Properties.SetObject(Control.PropBindings, controlBindingsCollection);
				}
				return controlBindingsCollection;
			}
		}

		/// <summary>Gets the default background color of the control.</summary>
		/// <returns>The default background <see cref="T:System.Drawing.Color" /> of the control. The default is <see cref="P:System.Drawing.SystemColors.Control" />.</returns>
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0002849B File Offset: 0x0002669B
		public static Color DefaultBackColor
		{
			get
			{
				return SystemColors.Control;
			}
		}

		/// <summary>Gets or sets the default cursor for the control.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.Cursor" /> representing the current default cursor.</returns>
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000284A2 File Offset: 0x000266A2
		protected virtual Cursor DefaultCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		/// <summary>Gets the default font of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Font" /> of the control. The value returned will vary depending on the user's operating system the local culture setting of their system.</returns>
		/// <exception cref="T:System.ArgumentException">The default font or the regional alternative fonts are not installed on the client computer. </exception>
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000284A9 File Offset: 0x000266A9
		public static Font DefaultFont
		{
			get
			{
				if (Control.defaultFont == null)
				{
					Control.defaultFont = SystemFonts.DefaultFont;
				}
				return Control.defaultFont;
			}
		}

		/// <summary>Gets the default foreground color of the control.</summary>
		/// <returns>The default foreground <see cref="T:System.Drawing.Color" /> of the control. The default is <see cref="P:System.Drawing.SystemColors.ControlText" />.</returns>
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x000284C1 File Offset: 0x000266C1
		public static Color DefaultForeColor
		{
			get
			{
				return SystemColors.ControlText;
			}
		}

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the default space between controls.</returns>
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000284C8 File Offset: 0x000266C8
		protected virtual Padding DefaultMargin
		{
			get
			{
				return CommonProperties.DefaultMargin;
			}
		}

		/// <summary>Gets the length and height, in pixels, that is specified as the default maximum size of a control.</summary>
		/// <returns>A <see cref="M:System.Drawing.Point.#ctor(System.Drawing.Size)" /> representing the size of the control.</returns>
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000284CF File Offset: 0x000266CF
		protected virtual Size DefaultMaximumSize
		{
			get
			{
				return CommonProperties.DefaultMaximumSize;
			}
		}

		/// <summary>Gets the length and height, in pixels, that is specified as the default minimum size of a control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the size of the control.</returns>
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000284D6 File Offset: 0x000266D6
		protected virtual Size DefaultMinimumSize
		{
			get
			{
				return CommonProperties.DefaultMinimumSize;
			}
		}

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the internal spacing of the contents of a control.</returns>
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		private RightToLeft DefaultRightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x000284DD File Offset: 0x000266DD
		protected virtual Size DefaultSize
		{
			get
			{
				return Size.Empty;
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000284E4 File Offset: 0x000266E4
		private void DetachContextMenu(object sender, EventArgs e)
		{
			this.ContextMenu = null;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000284ED File Offset: 0x000266ED
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStrip = null;
		}

		/// <summary>Gets the DPI value for the display device where the control is currently being displayed.</summary>
		/// <returns>The DPI value of the display device.</returns>
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x000284F6 File Offset: 0x000266F6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int DeviceDpi
		{
			get
			{
				if (DpiHelper.EnableDpiChangedMessageHandling)
				{
					return this.deviceDpi;
				}
				return DpiHelper.DeviceDpi;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0002850C File Offset: 0x0002670C
		internal Color DisabledColor
		{
			get
			{
				Color result = this.BackColor;
				if (result.A == 0)
				{
					Control parentInternal = this.ParentInternal;
					while (result.A == 0)
					{
						if (parentInternal == null)
						{
							result = SystemColors.Control;
							break;
						}
						result = parentInternal.BackColor;
						parentInternal = parentInternal.ParentInternal;
					}
				}
				return result;
			}
		}

		/// <summary>Gets the rectangle that represents the display area of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the display area of the control.</returns>
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00027D3C File Offset: 0x00025F3C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlDisplayRectangleDescr")]
		public virtual Rectangle DisplayRectangle
		{
			get
			{
				return new Rectangle(0, 0, this.clientWidth, this.clientHeight);
			}
		}

		/// <summary>Gets a value indicating whether the control has been disposed of.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has been disposed of; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00028555 File Offset: 0x00026755
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlDisposedDescr")]
		public bool IsDisposed
		{
			get
			{
				return this.GetState(2048);
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00028564 File Offset: 0x00026764
		private void DisposeFontHandle()
		{
			if (this.Properties.ContainsObject(Control.PropFontHandleWrapper))
			{
				Control.FontHandleWrapper fontHandleWrapper = this.Properties.GetObject(Control.PropFontHandleWrapper) as Control.FontHandleWrapper;
				if (fontHandleWrapper != null)
				{
					fontHandleWrapper.Dispose();
				}
				this.Properties.SetObject(Control.PropFontHandleWrapper, null);
			}
		}

		/// <summary>Gets a value indicating whether the base <see cref="T:System.Windows.Forms.Control" /> class is in the process of disposing.</summary>
		/// <returns>
		///     <see langword="true" /> if the base <see cref="T:System.Windows.Forms.Control" /> class is in the process of disposing; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x000285B3 File Offset: 0x000267B3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlDisposingDescr")]
		public bool Disposing
		{
			get
			{
				return this.GetState(4096);
			}
		}

		/// <summary>Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.DockStyle" /> values. </exception>
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x000285C0 File Offset: 0x000267C0
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x000285C8 File Offset: 0x000267C8
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(DockStyle.None)]
		[SRDescription("ControlDockDescr")]
		public virtual DockStyle Dock
		{
			get
			{
				return DefaultLayout.GetDock(this);
			}
			set
			{
				if (value != this.Dock)
				{
					this.SuspendLayout();
					try
					{
						DefaultLayout.SetDock(this, value);
						this.OnDockChanged(EventArgs.Empty);
					}
					finally
					{
						this.ResumeLayout();
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Dock" /> property changes.</summary>
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06000C90 RID: 3216 RVA: 0x00028610 File Offset: 0x00026810
		// (remove) Token: 0x06000C91 RID: 3217 RVA: 0x00028623 File Offset: 0x00026823
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnDockChangedDescr")]
		public event EventHandler DockChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventDock, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDock, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.</summary>
		/// <returns>
		///     <see langword="true" /> if the surface of the control should be drawn using double buffering; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00028636 File Offset: 0x00026836
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00028643 File Offset: 0x00026843
		[SRCategory("CatBehavior")]
		[SRDescription("ControlDoubleBufferedDescr")]
		protected virtual bool DoubleBuffered
		{
			get
			{
				return this.GetStyle(ControlStyles.OptimizedDoubleBuffer);
			}
			set
			{
				if (value != this.DoubleBuffered)
				{
					if (value)
					{
						this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value);
						return;
					}
					this.SetStyle(ControlStyles.OptimizedDoubleBuffer, value);
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0002866A File Offset: 0x0002686A
		private bool DoubleBufferingEnabled
		{
			get
			{
				return this.GetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer);
			}
		}

		/// <summary>Gets or sets a value indicating whether the control can respond to user interaction.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can respond to user interaction; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00028677 File Offset: 0x00026877
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0002869C File Offset: 0x0002689C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DispId(-514)]
		[SRDescription("ControlEnabledDescr")]
		public bool Enabled
		{
			get
			{
				return this.GetState(4) && (this.ParentInternal == null || this.ParentInternal.Enabled);
			}
			set
			{
				bool enabled = this.Enabled;
				this.SetState(4, value);
				if (enabled != value)
				{
					if (!value)
					{
						this.SelectNextIfFocused();
					}
					this.OnEnabledChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Enabled" /> property value has changed.</summary>
		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06000C97 RID: 3223 RVA: 0x000286D0 File Offset: 0x000268D0
		// (remove) Token: 0x06000C98 RID: 3224 RVA: 0x000286E3 File Offset: 0x000268E3
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnEnabledChangedDescr")]
		public event EventHandler EnabledChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventEnabled, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventEnabled, value);
			}
		}

		/// <summary>Gets a value indicating whether the control has input focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x000286F6 File Offset: 0x000268F6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlFocusedDescr")]
		public virtual bool Focused
		{
			get
			{
				return this.IsHandleCreated && UnsafeNativeMethods.GetFocus() == this.Handle;
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00028714 File Offset: 0x00026914
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x0002877C File Offset: 0x0002697C
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DispId(-512)]
		[AmbientValue(null)]
		[SRDescription("ControlFontDescr")]
		public virtual Font Font
		{
			[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Windows.Forms.Control/ActiveXFontMarshaler)]
			get
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					return font;
				}
				Font font2 = this.GetParentFont();
				if (font2 != null)
				{
					return font2;
				}
				if (this.IsActiveX)
				{
					font2 = this.ActiveXAmbientFont;
					if (font2 != null)
					{
						return font2;
					}
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Font != null)
				{
					return ambientPropertiesService.Font;
				}
				return Control.DefaultFont;
			}
			[param: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Windows.Forms.Control/ActiveXFontMarshaler)]
			set
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				Font font2 = this.Font;
				bool flag = false;
				if (value == null)
				{
					if (font != null)
					{
						flag = true;
					}
				}
				else
				{
					flag = (font == null || !value.Equals(font));
				}
				if (flag)
				{
					this.Properties.SetObject(Control.PropFont, value);
					if (!font2.Equals(value))
					{
						this.DisposeFontHandle();
						if (this.Properties.ContainsInteger(Control.PropFontHeight))
						{
							this.Properties.SetInteger(Control.PropFontHeight, (value == null) ? -1 : value.Height);
						}
						using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Font))
						{
							this.OnFontChanged(EventArgs.Empty);
							return;
						}
					}
					if (this.IsHandleCreated && !this.GetStyle(ControlStyles.UserPaint))
					{
						this.DisposeFontHandle();
						this.SetWindowFont();
					}
				}
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00028870 File Offset: 0x00026A70
		internal void ScaleFont(float factor)
		{
			Font font = (Font)this.Properties.GetObject(Control.PropFont);
			Font font2 = this.Font;
			Font font3 = DpiHelper.EnableDpiChangedHighDpiImprovements ? new Font(this.Font.FontFamily, this.Font.Size * factor, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont) : new Font(this.Font.FontFamily, this.Font.Size * factor, this.Font.Style);
			if (font == null || !font.Equals(font3))
			{
				this.Properties.SetObject(Control.PropFont, font3);
				if (!font2.Equals(font3))
				{
					this.DisposeFontHandle();
					if (this.Properties.ContainsInteger(Control.PropFontHeight))
					{
						this.Properties.SetInteger(Control.PropFontHeight, font3.Height);
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Font" /> property value changes.</summary>
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06000C9D RID: 3229 RVA: 0x00028969 File Offset: 0x00026B69
		// (remove) Token: 0x06000C9E RID: 3230 RVA: 0x0002897C File Offset: 0x00026B7C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnFontChangedDescr")]
		public event EventHandler FontChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventFont, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventFont, value);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00028990 File Offset: 0x00026B90
		internal IntPtr FontHandle
		{
			get
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					Control.FontHandleWrapper fontHandleWrapper = (Control.FontHandleWrapper)this.Properties.GetObject(Control.PropFontHandleWrapper);
					if (fontHandleWrapper == null)
					{
						fontHandleWrapper = new Control.FontHandleWrapper(font);
						this.Properties.SetObject(Control.PropFontHandleWrapper, fontHandleWrapper);
					}
					return fontHandleWrapper.Handle;
				}
				if (this.parent != null)
				{
					return this.parent.FontHandle;
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Font != null)
				{
					Control.FontHandleWrapper fontHandleWrapper2 = null;
					Font font2 = (Font)this.Properties.GetObject(Control.PropCurrentAmbientFont);
					if (font2 != null && font2 == ambientPropertiesService.Font)
					{
						fontHandleWrapper2 = (Control.FontHandleWrapper)this.Properties.GetObject(Control.PropFontHandleWrapper);
					}
					else
					{
						this.Properties.SetObject(Control.PropCurrentAmbientFont, ambientPropertiesService.Font);
					}
					if (fontHandleWrapper2 == null)
					{
						font = ambientPropertiesService.Font;
						fontHandleWrapper2 = new Control.FontHandleWrapper(font);
						this.Properties.SetObject(Control.PropFontHandleWrapper, fontHandleWrapper2);
					}
					return fontHandleWrapper2.Handle;
				}
				return Control.GetDefaultFontHandleWrapper().Handle;
			}
		}

		/// <summary>Gets or sets the height of the font of the control.</summary>
		/// <returns>The height of the <see cref="T:System.Drawing.Font" /> of the control in pixels.</returns>
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00028AA0 File Offset: 0x00026CA0
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00028B41 File Offset: 0x00026D41
		protected int FontHeight
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropFontHeight, out flag);
				if (flag && integer != -1)
				{
					return integer;
				}
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					integer = font.Height;
					this.Properties.SetInteger(Control.PropFontHeight, integer);
					return integer;
				}
				int num = -1;
				if (this.ParentInternal != null && this.ParentInternal.CanAccessProperties)
				{
					num = this.ParentInternal.FontHeight;
				}
				if (num == -1)
				{
					num = this.Font.Height;
					this.Properties.SetInteger(Control.PropFontHeight, num);
				}
				return num;
			}
			set
			{
				this.Properties.SetInteger(Control.PropFontHeight, value);
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00028B54 File Offset: 0x00026D54
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x00028BD8 File Offset: 0x00026DD8
		[SRCategory("CatAppearance")]
		[DispId(-513)]
		[SRDescription("ControlForeColorDescr")]
		public virtual Color ForeColor
		{
			get
			{
				Color color = this.Properties.GetColor(Control.PropForeColor);
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.ForeColor;
				}
				Color result = Color.Empty;
				if (this.IsActiveX)
				{
					result = this.ActiveXAmbientForeColor;
				}
				if (result.IsEmpty)
				{
					AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
					if (ambientPropertiesService != null)
					{
						result = ambientPropertiesService.ForeColor;
					}
				}
				if (!result.IsEmpty)
				{
					return result;
				}
				return Control.DefaultForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(Control.PropForeColor))
				{
					this.Properties.SetColor(Control.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property value changes.</summary>
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06000CA4 RID: 3236 RVA: 0x00028C3D File Offset: 0x00026E3D
		// (remove) Token: 0x06000CA5 RID: 3237 RVA: 0x00028C50 File Offset: 0x00026E50
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnForeColorChangedDescr")]
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventForeColor, value);
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00028C63 File Offset: 0x00026E63
		private Font GetParentFont()
		{
			if (this.ParentInternal != null && this.ParentInternal.CanAccessProperties)
			{
				return this.ParentInternal.Font;
			}
			return null;
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="proposedSize">The custom-sized area for a control. </param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06000CA7 RID: 3239 RVA: 0x00028C88 File Offset: 0x00026E88
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual Size GetPreferredSize(Size proposedSize)
		{
			Size size;
			if (this.GetState(6144))
			{
				size = CommonProperties.xGetPreferredSizeCache(this);
			}
			else
			{
				proposedSize = LayoutUtils.ConvertZeroToUnbounded(proposedSize);
				proposedSize = this.ApplySizeConstraints(proposedSize);
				if (this.GetState2(2048))
				{
					Size result = CommonProperties.xGetPreferredSizeCache(this);
					if (!result.IsEmpty && proposedSize == LayoutUtils.MaxSize)
					{
						return result;
					}
				}
				this.CacheTextInternal = true;
				try
				{
					size = this.GetPreferredSizeCore(proposedSize);
				}
				finally
				{
					this.CacheTextInternal = false;
				}
				size = this.ApplySizeConstraints(size);
				if (this.GetState2(2048) && proposedSize == LayoutUtils.MaxSize)
				{
					CommonProperties.xSetPreferredSizeCache(this, size);
				}
			}
			return size;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00028D3C File Offset: 0x00026F3C
		internal virtual Size GetPreferredSizeCore(Size proposedSize)
		{
			return CommonProperties.GetSpecifiedBounds(this).Size;
		}

		/// <summary>Gets the window handle that the control is bound to.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that contains the window handle (<see langword="HWND" />) of the control.</returns>
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00028D58 File Offset: 0x00026F58
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(-515)]
		[SRDescription("ControlHandleDescr")]
		public IntPtr Handle
		{
			get
			{
				if (Control.checkForIllegalCrossThreadCalls && !Control.inCrossThreadSafeCall && this.InvokeRequired)
				{
					throw new InvalidOperationException(SR.GetString("IllegalCrossThreadCall", new object[]
					{
						this.Name
					}));
				}
				if (!this.IsHandleCreated)
				{
					this.CreateHandle();
				}
				return this.HandleInternal;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00028DAE File Offset: 0x00026FAE
		internal IntPtr HandleInternal
		{
			get
			{
				return this.window.Handle;
			}
		}

		/// <summary>Gets a value indicating whether the control contains one or more child controls.</summary>
		/// <returns>
		///     <see langword="true" /> if the control contains one or more child controls; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00028DBC File Offset: 0x00026FBC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHasChildrenDescr")]
		public bool HasChildren
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				return controlCollection != null && controlCollection.Count > 0;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool HasMenu
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the height of the control.</summary>
		/// <returns>The height of the control in pixels.</returns>
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00028DED File Offset: 0x00026FED
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x00028DF5 File Offset: 0x00026FF5
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHeightDescr")]
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.SetBounds(this.x, this.y, this.width, value, BoundsSpecified.Height);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00028E14 File Offset: 0x00027014
		internal bool HostedInWin32DialogManager
		{
			get
			{
				if (!this.GetState(16777216))
				{
					Control topMostParent = this.TopMostParent;
					if (this != topMostParent)
					{
						this.SetState(33554432, topMostParent.HostedInWin32DialogManager);
					}
					else
					{
						IntPtr intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle));
						IntPtr handle = intPtr;
						StringBuilder stringBuilder = new StringBuilder(32);
						this.SetState(33554432, false);
						while (intPtr != IntPtr.Zero)
						{
							int className = UnsafeNativeMethods.GetClassName(new HandleRef(null, handle), null, 0);
							if (className > stringBuilder.Capacity)
							{
								stringBuilder.Capacity = className + 5;
							}
							UnsafeNativeMethods.GetClassName(new HandleRef(null, handle), stringBuilder, stringBuilder.Capacity);
							if (stringBuilder.ToString() == "#32770")
							{
								this.SetState(33554432, true);
								break;
							}
							handle = intPtr;
							intPtr = UnsafeNativeMethods.GetParent(new HandleRef(null, intPtr));
						}
					}
					this.SetState(16777216, true);
				}
				return this.GetState(33554432);
			}
		}

		/// <summary>Gets a value indicating whether the control has a handle associated with it.</summary>
		/// <returns>
		///     <see langword="true" /> if a handle has been assigned to the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00028F07 File Offset: 0x00027107
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHandleCreatedDescr")]
		public bool IsHandleCreated
		{
			get
			{
				return this.window.Handle != IntPtr.Zero;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00028F1E File Offset: 0x0002711E
		internal bool IsLayoutSuspended
		{
			get
			{
				return this.layoutSuspendCount > 0;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00028F2C File Offset: 0x0002712C
		internal bool IsWindowObscured
		{
			get
			{
				if (!this.IsHandleCreated || !this.Visible)
				{
					return false;
				}
				bool result = false;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					while (parentInternal.ParentInternal != null)
					{
						parentInternal = parentInternal.ParentInternal;
					}
				}
				UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
				Region region = new Region(Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom));
				try
				{
					IntPtr handle;
					if (parentInternal != null)
					{
						handle = parentInternal.Handle;
					}
					else
					{
						handle = this.Handle;
					}
					IntPtr handle2 = handle;
					IntPtr intPtr;
					while ((intPtr = UnsafeNativeMethods.GetWindow(new HandleRef(null, handle2), 3)) != IntPtr.Zero)
					{
						UnsafeNativeMethods.GetWindowRect(new HandleRef(null, intPtr), ref rect);
						Rectangle rect2 = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
						if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr)))
						{
							region.Exclude(rect2);
						}
						handle2 = intPtr;
					}
					Graphics graphics = this.CreateGraphics();
					try
					{
						result = region.IsEmpty(graphics);
					}
					finally
					{
						graphics.Dispose();
					}
				}
				finally
				{
					region.Dispose();
				}
				return result;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00029068 File Offset: 0x00027268
		internal IntPtr InternalHandle
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return IntPtr.Zero;
				}
				return this.Handle;
			}
		}

		/// <summary>Gets a value indicating whether the caller must call an invoke method when making method calls to the control because the caller is on a different thread than the one the control was created on.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's <see cref="P:System.Windows.Forms.Control.Handle" /> was created on a different thread than the calling thread (indicating that you must make calls to the control through an invoke method); otherwise, <see langword="false" />.</returns>
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00029080 File Offset: 0x00027280
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlInvokeRequiredDescr")]
		public bool InvokeRequired
		{
			get
			{
				bool result;
				using (new Control.MultithreadSafeCallScope())
				{
					HandleRef hWnd;
					if (this.IsHandleCreated)
					{
						hWnd = new HandleRef(this, this.Handle);
					}
					else
					{
						Control control = this.FindMarshalingControl();
						if (!control.IsHandleCreated)
						{
							return false;
						}
						hWnd = new HandleRef(control, control.Handle);
					}
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(hWnd, out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					result = (windowThreadProcessId != currentThreadId);
				}
				return result;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control is visible to accessibility applications.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is visible to accessibility applications; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0002910C File Offset: 0x0002730C
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x00029119 File Offset: 0x00027319
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlIsAccessibleDescr")]
		public bool IsAccessible
		{
			get
			{
				return this.GetState(1048576);
			}
			set
			{
				this.SetState(1048576, value);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00029127 File Offset: 0x00027327
		internal bool IsActiveX
		{
			get
			{
				return this.GetState2(1024);
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsContainerControl
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00029134 File Offset: 0x00027334
		internal bool IsIEParent
		{
			get
			{
				return this.IsActiveX && this.ActiveXInstance.IsIE;
			}
		}

		/// <summary>Gets a value indicating whether the control is mirrored.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is mirrored; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0002914C File Offset: 0x0002734C
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("IsMirroredDescr")]
		public bool IsMirrored
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					CreateParams createParams = this.CreateParams;
					this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				}
				return this.GetState(1073741824);
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002918D File Offset: 0x0002738D
		private bool IsValidBackColor(Color c)
		{
			return c.IsEmpty || this.GetStyle(ControlStyles.SupportsTransparentBackColor) || c.A >= byte.MaxValue;
		}

		/// <summary>Gets or sets the distance, in pixels, between the left edge of the control and the left edge of its container's client area.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the distance, in pixels, between the left edge of the control and the left edge of its container's client area.</returns>
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x000291B6 File Offset: 0x000273B6
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x000291BE File Offset: 0x000273BE
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlLeftDescr")]
		public int Left
		{
			get
			{
				return this.x;
			}
			set
			{
				this.SetBounds(value, this.y, this.width, this.height, BoundsSpecified.X);
			}
		}

		/// <summary>Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the control relative to the upper-left corner of its container.</returns>
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000291DA File Offset: 0x000273DA
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x000291ED File Offset: 0x000273ED
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ControlLocationDescr")]
		public Point Location
		{
			get
			{
				return new Point(this.x, this.y);
			}
			set
			{
				this.SetBounds(value.X, value.Y, this.width, this.height, BoundsSpecified.Location);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Location" /> property value has changed.</summary>
		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06000CC1 RID: 3265 RVA: 0x00029210 File Offset: 0x00027410
		// (remove) Token: 0x06000CC2 RID: 3266 RVA: 0x00029223 File Offset: 0x00027423
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnLocationChangedDescr")]
		public event EventHandler LocationChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventLocation, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLocation, value);
			}
		}

		/// <summary>Gets or sets the space between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the space between controls.</returns>
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x000119E5 File Offset: 0x0000FBE5
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00029236 File Offset: 0x00027436
		[SRDescription("ControlMarginDescr")]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (value != this.Margin)
				{
					CommonProperties.SetMargin(this, value);
					this.OnMarginChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the control's margin changes.</summary>
		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06000CC5 RID: 3269 RVA: 0x00029260 File Offset: 0x00027460
		// (remove) Token: 0x06000CC6 RID: 3270 RVA: 0x00029273 File Offset: 0x00027473
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnMarginChangedDescr")]
		public event EventHandler MarginChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventMarginChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMarginChanged, value);
			}
		}

		/// <summary>Gets or sets the size that is the upper limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00029286 File Offset: 0x00027486
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00029294 File Offset: 0x00027494
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ControlMaximumSizeDescr")]
		[AmbientValue(typeof(Size), "0, 0")]
		public virtual Size MaximumSize
		{
			get
			{
				return CommonProperties.GetMaximumSize(this, this.DefaultMaximumSize);
			}
			set
			{
				if (value == Size.Empty)
				{
					CommonProperties.ClearMaximumSize(this);
					return;
				}
				if (value != this.MaximumSize)
				{
					CommonProperties.SetMaximumSize(this, value);
				}
			}
		}

		/// <summary>Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000292BF File Offset: 0x000274BF
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x000292CD File Offset: 0x000274CD
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ControlMinimumSizeDescr")]
		public virtual Size MinimumSize
		{
			get
			{
				return CommonProperties.GetMinimumSize(this, this.DefaultMinimumSize);
			}
			set
			{
				if (value != this.MinimumSize)
				{
					CommonProperties.SetMinimumSize(this, value);
				}
			}
		}

		/// <summary>Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in a pressed state.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.Keys" /> values. The default is <see cref="F:System.Windows.Forms.Keys.None" />.</returns>
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x000292E4 File Offset: 0x000274E4
		public static Keys ModifierKeys
		{
			get
			{
				Keys keys = Keys.None;
				if (UnsafeNativeMethods.GetKeyState(16) < 0)
				{
					keys |= Keys.Shift;
				}
				if (UnsafeNativeMethods.GetKeyState(17) < 0)
				{
					keys |= Keys.Control;
				}
				if (UnsafeNativeMethods.GetKeyState(18) < 0)
				{
					keys |= Keys.Alt;
				}
				return keys;
			}
		}

		/// <summary>Gets a value indicating which of the mouse buttons is in a pressed state.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.MouseButtons" /> enumeration values. The default is <see cref="F:System.Windows.Forms.MouseButtons.None" />.</returns>
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0002932C File Offset: 0x0002752C
		public static MouseButtons MouseButtons
		{
			get
			{
				MouseButtons mouseButtons = MouseButtons.None;
				if (UnsafeNativeMethods.GetKeyState(1) < 0)
				{
					mouseButtons |= MouseButtons.Left;
				}
				if (UnsafeNativeMethods.GetKeyState(2) < 0)
				{
					mouseButtons |= MouseButtons.Right;
				}
				if (UnsafeNativeMethods.GetKeyState(4) < 0)
				{
					mouseButtons |= MouseButtons.Middle;
				}
				if (UnsafeNativeMethods.GetKeyState(5) < 0)
				{
					mouseButtons |= MouseButtons.XButton1;
				}
				if (UnsafeNativeMethods.GetKeyState(6) < 0)
				{
					mouseButtons |= MouseButtons.XButton2;
				}
				return mouseButtons;
			}
		}

		/// <summary>Gets the position of the mouse cursor in screen coordinates.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that contains the coordinates of the mouse cursor relative to the upper-left corner of the screen.</returns>
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00029394 File Offset: 0x00027594
		public static Point MousePosition
		{
			get
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				return new Point(point.x, point.y);
			}
		}

		/// <summary>Gets or sets the name of the control.</summary>
		/// <returns>The name of the control. The default is an empty string ("").</returns>
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000293C0 File Offset: 0x000275C0
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x00029409 File Offset: 0x00027609
		[Browsable(false)]
		public string Name
		{
			get
			{
				string text = (string)this.Properties.GetObject(Control.PropName);
				if (string.IsNullOrEmpty(text))
				{
					if (this.Site != null)
					{
						text = this.Site.Name;
					}
					if (text == null)
					{
						text = "";
					}
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Properties.SetObject(Control.PropName, null);
					return;
				}
				this.Properties.SetObject(Control.PropName, value);
			}
		}

		/// <summary>Gets or sets the parent container of the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> that represents the parent or container control of the control.</returns>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00029436 File Offset: 0x00027636
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x00029448 File Offset: 0x00027648
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlParentDescr")]
		public Control Parent
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.ParentInternal;
			}
			set
			{
				this.ParentInternal = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00029451 File Offset: 0x00027651
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x00029459 File Offset: 0x00027659
		internal virtual Control ParentInternal
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					if (value != null)
					{
						value.Controls.Add(this);
						return;
					}
					this.parent.Controls.Remove(this);
				}
			}
		}

		/// <summary>Gets the product name of the assembly containing the control.</summary>
		/// <returns>The product name of the assembly containing the control.</returns>
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00029485 File Offset: 0x00027685
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlProductNameDescr")]
		public string ProductName
		{
			get
			{
				return this.VersionInfo.ProductName;
			}
		}

		/// <summary>Gets the version of the assembly containing the control.</summary>
		/// <returns>The file version of the assembly containing the control.</returns>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00029492 File Offset: 0x00027692
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlProductVersionDescr")]
		public string ProductVersion
		{
			get
			{
				return this.VersionInfo.ProductVersion;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0002949F File Offset: 0x0002769F
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000294A7 File Offset: 0x000276A7
		internal Color RawBackColor
		{
			get
			{
				return this.Properties.GetColor(Control.PropBackColor);
			}
		}

		/// <summary>Gets a value indicating whether the control is currently re-creating its handle.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is currently re-creating its handle; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000294B9 File Offset: 0x000276B9
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlRecreatingHandleDescr")]
		public bool RecreatingHandle
		{
			get
			{
				return (this.state & 16) != 0;
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void AddReflectChild()
		{
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void RemoveReflectChild()
		{
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000294C7 File Offset: 0x000276C7
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x000294D0 File Offset: 0x000276D0
		private Control ReflectParent
		{
			get
			{
				return this.reflectParent;
			}
			set
			{
				if (value != null)
				{
					value.AddReflectChild();
				}
				Control control = this.ReflectParent;
				this.reflectParent = value;
				if (control != null)
				{
					control.RemoveReflectChild();
				}
			}
		}

		/// <summary>Gets or sets the window region associated with the control.</summary>
		/// <returns>The window <see cref="T:System.Drawing.Region" /> associated with the control.</returns>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x000294FD File Offset: 0x000276FD
		// (set) Token: 0x06000CDE RID: 3294 RVA: 0x00029514 File Offset: 0x00027714
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlRegionDescr")]
		public Region Region
		{
			get
			{
				return (Region)this.Properties.GetObject(Control.PropRegion);
			}
			set
			{
				if (this.GetState(524288))
				{
					IntSecurity.ChangeWindowRegionForTopLevel.Demand();
				}
				Region region = this.Region;
				if (region != value)
				{
					this.Properties.SetObject(Control.PropRegion, value);
					if (region != null)
					{
						region.Dispose();
					}
					if (this.IsHandleCreated)
					{
						IntPtr intPtr = IntPtr.Zero;
						try
						{
							if (value != null)
							{
								intPtr = this.GetHRgn(value);
							}
							if (this.IsActiveX)
							{
								intPtr = this.ActiveXMergeRegion(intPtr);
							}
							if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, this.Handle), new HandleRef(this, intPtr), SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle))) != 0)
							{
								intPtr = IntPtr.Zero;
							}
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
							}
						}
					}
					this.OnRegionChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.Region" /> property changes.</summary>
		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06000CDF RID: 3295 RVA: 0x000295F4 File Offset: 0x000277F4
		// (remove) Token: 0x06000CE0 RID: 3296 RVA: 0x00029607 File Offset: 0x00027807
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlRegionChangedDescr")]
		public event EventHandler RegionChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventRegionChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventRegionChanged, value);
			}
		}

		/// <summary>This property is now obsolete.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is rendered from right to left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0000E214 File Offset: 0x0000C414
		[Obsolete("This property has been deprecated. Please use RightToLeft instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected internal bool RenderRightToLeft
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0002961C File Offset: 0x0002781C
		internal bool RenderTransparent
		{
			get
			{
				return this.GetStyle(ControlStyles.SupportsTransparentBackColor) && this.BackColor.A < byte.MaxValue;
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0002964D File Offset: 0x0002784D
		private bool RenderColorTransparent(Color c)
		{
			return this.GetStyle(ControlStyles.SupportsTransparentBackColor) && c.A < byte.MaxValue;
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool RenderTransparencyWithVisualStyles
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0002966C File Offset: 0x0002786C
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00029684 File Offset: 0x00027884
		internal BoundsSpecified RequiredScaling
		{
			get
			{
				if ((this.requiredScaling & 16) != 0)
				{
					return (BoundsSpecified)(this.requiredScaling & 15);
				}
				return BoundsSpecified.None;
			}
			set
			{
				byte b = this.requiredScaling & 16;
				this.requiredScaling = (byte)((value & BoundsSpecified.All) | (BoundsSpecified)b);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x000296A9 File Offset: 0x000278A9
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x000296B8 File Offset: 0x000278B8
		internal bool RequiredScalingEnabled
		{
			get
			{
				return (this.requiredScaling & 16) > 0;
			}
			set
			{
				byte b = this.requiredScaling & 15;
				this.requiredScaling = b;
				if (value)
				{
					this.requiredScaling |= 16;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the control redraws itself when resized.</summary>
		/// <returns>
		///     <see langword="true" /> if the control redraws itself when resized; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x000296EA File Offset: 0x000278EA
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x000296F4 File Offset: 0x000278F4
		[SRDescription("ControlResizeRedrawDescr")]
		protected bool ResizeRedraw
		{
			get
			{
				return this.GetStyle(ControlStyles.ResizeRedraw);
			}
			set
			{
				this.SetStyle(ControlStyles.ResizeRedraw, value);
			}
		}

		/// <summary>Gets the distance, in pixels, between the right edge of the control and the left edge of its container's client area.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the distance, in pixels, between the right edge of the control and the left edge of its container's client area.</returns>
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000296FF File Offset: 0x000278FF
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlRightDescr")]
		public int Right
		{
			get
			{
				return this.x + this.width;
			}
		}

		/// <summary>Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft" /> values. </exception>
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00029710 File Offset: 0x00027910
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00029754 File Offset: 0x00027954
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[AmbientValue(RightToLeft.Inherit)]
		[SRDescription("ControlRightToLeftDescr")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				bool flag;
				int num = this.Properties.GetInteger(Control.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						num = (int)parentInternal.RightToLeft;
					}
					else
					{
						num = (int)this.DefaultRightToLeft;
					}
				}
				return (RightToLeft)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				RightToLeft rightToLeft = this.RightToLeft;
				if (this.Properties.ContainsInteger(Control.PropRightToLeft) || value != RightToLeft.Inherit)
				{
					this.Properties.SetInteger(Control.PropRightToLeft, (int)value);
				}
				if (rightToLeft != this.RightToLeft)
				{
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeft))
					{
						this.OnRightToLeftChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property value changes.</summary>
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06000CEE RID: 3310 RVA: 0x000297F4 File Offset: 0x000279F4
		// (remove) Token: 0x06000CEF RID: 3311 RVA: 0x00029807 File Offset: 0x00027A07
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftChangedDescr")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventRightToLeft, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventRightToLeft, value);
			}
		}

		/// <summary>Gets a value that determines the scaling of child controls. </summary>
		/// <returns>
		///     <see langword="true" /> if child controls will be scaled when the <see cref="M:System.Windows.Forms.Control.Scale(System.Single)" /> method on this control is called; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0000E214 File Offset: 0x0000C414
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual bool ScaleChildren
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the site of the control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the <see cref="T:System.Windows.Forms.Control" />, if any.</returns>
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0002981A File Offset: 0x00027A1A
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x00029824 File Offset: 0x00027A24
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				AmbientProperties ambientProperties = null;
				if (value != null)
				{
					ambientProperties = (AmbientProperties)value.GetService(typeof(AmbientProperties));
				}
				if (ambientPropertiesService != ambientProperties)
				{
					bool flag = !this.Properties.ContainsObject(Control.PropFont);
					bool flag2 = !this.Properties.ContainsObject(Control.PropBackColor);
					bool flag3 = !this.Properties.ContainsObject(Control.PropForeColor);
					bool flag4 = !this.Properties.ContainsObject(Control.PropCursor);
					Font font = null;
					Color color = Color.Empty;
					Color color2 = Color.Empty;
					Cursor cursor = null;
					if (flag)
					{
						font = this.Font;
					}
					if (flag2)
					{
						color = this.BackColor;
					}
					if (flag3)
					{
						color2 = this.ForeColor;
					}
					if (flag4)
					{
						cursor = this.Cursor;
					}
					this.Properties.SetObject(Control.PropAmbientPropertiesService, ambientProperties);
					base.Site = value;
					if (flag && !font.Equals(this.Font))
					{
						this.OnFontChanged(EventArgs.Empty);
					}
					if (flag3 && !color2.Equals(this.ForeColor))
					{
						this.OnForeColorChanged(EventArgs.Empty);
					}
					if (flag2 && !color.Equals(this.BackColor))
					{
						this.OnBackColorChanged(EventArgs.Empty);
					}
					if (flag4 && cursor.Equals(this.Cursor))
					{
						this.OnCursorChanged(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.Site = value;
				}
			}
		}

		/// <summary>Gets or sets the height and width of the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> that represents the height and width of the control in pixels.</returns>
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0002999A File Offset: 0x00027B9A
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x000299AD File Offset: 0x00027BAD
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ControlSizeDescr")]
		public Size Size
		{
			get
			{
				return new Size(this.width, this.height);
			}
			set
			{
				this.SetBounds(this.x, this.y, value.Width, value.Height, BoundsSpecified.Size);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Size" /> property value changes.</summary>
		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06000CF5 RID: 3317 RVA: 0x000299D1 File Offset: 0x00027BD1
		// (remove) Token: 0x06000CF6 RID: 3318 RVA: 0x000299E4 File Offset: 0x00027BE4
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnSizeChangedDescr")]
		public event EventHandler SizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventSize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventSize, value);
			}
		}

		/// <summary>Gets or sets the tab order of the control within its container.</summary>
		/// <returns>The index value of the control within the set of controls within its container. The controls in the container are included in the tab order.</returns>
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000299F7 File Offset: 0x00027BF7
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x00029A0C File Offset: 0x00027C0C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[MergableProperty(false)]
		[SRDescription("ControlTabIndexDescr")]
		public int TabIndex
		{
			get
			{
				if (this.tabIndex != -1)
				{
					return this.tabIndex;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("TabIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"TabIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.tabIndex != value)
				{
					this.tabIndex = value;
					this.OnTabIndexChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.TabIndex" /> property value changes.</summary>
		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06000CF9 RID: 3321 RVA: 0x00029A7B File Offset: 0x00027C7B
		// (remove) Token: 0x06000CFA RID: 3322 RVA: 0x00029A8E File Offset: 0x00027C8E
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnTabIndexChangedDescr")]
		public event EventHandler TabIndexChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventTabIndex, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventTabIndex, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the control using the TAB key; otherwise, <see langword="false" />. The default is <see langword="true" />.This property will always return <see langword="true" /> for an instance of the <see cref="T:System.Windows.Forms.Form" /> class.</returns>
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00029AA1 File Offset: 0x00027CA1
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00029AA9 File Offset: 0x00027CA9
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[DispId(-516)]
		[SRDescription("ControlTabStopDescr")]
		public bool TabStop
		{
			get
			{
				return this.TabStopInternal;
			}
			set
			{
				if (this.TabStop != value)
				{
					this.TabStopInternal = value;
					if (this.IsHandleCreated)
					{
						this.SetWindowStyle(65536, value);
					}
					this.OnTabStopChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00029ADA File Offset: 0x00027CDA
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00029AE7 File Offset: 0x00027CE7
		internal bool TabStopInternal
		{
			get
			{
				return (this.state & 8) != 0;
			}
			set
			{
				if (this.TabStopInternal != value)
				{
					this.SetState(8, value);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.TabStop" /> property value changes.</summary>
		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06000CFF RID: 3327 RVA: 0x00029AFA File Offset: 0x00027CFA
		// (remove) Token: 0x06000D00 RID: 3328 RVA: 0x00029B0D File Offset: 0x00027D0D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnTabStopChangedDescr")]
		public event EventHandler TabStopChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventTabStop, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventTabStop, value);
			}
		}

		/// <summary>Gets or sets the object that contains data about the control.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the control. The default is <see langword="null" />.</returns>
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00029B20 File Offset: 0x00027D20
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x00029B32 File Offset: 0x00027D32
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(Control.PropUserData);
			}
			set
			{
				this.Properties.SetObject(Control.PropUserData, value);
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00029B45 File Offset: 0x00027D45
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x00029B6C File Offset: 0x00027D6C
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[Bindable(true)]
		[DispId(-517)]
		[SRDescription("ControlTextDescr")]
		public virtual string Text
		{
			get
			{
				if (!this.CacheTextInternal)
				{
					return this.WindowText;
				}
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value == this.Text)
				{
					return;
				}
				if (this.CacheTextInternal)
				{
					this.text = value;
				}
				this.WindowText = value;
				this.OnTextChanged(EventArgs.Empty);
				if (this.IsMnemonicsListenerAxSourced)
				{
					for (Control control = this; control != null; control = control.ParentInternal)
					{
						Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)control.Properties.GetObject(Control.PropActiveXImpl);
						if (activeXImpl != null)
						{
							activeXImpl.UpdateAccelTable();
							return;
						}
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Text" /> property value changes.</summary>
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06000D05 RID: 3333 RVA: 0x00029BE9 File Offset: 0x00027DE9
		// (remove) Token: 0x06000D06 RID: 3334 RVA: 0x00029BFC File Offset: 0x00027DFC
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnTextChangedDescr")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventText, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventText, value);
			}
		}

		/// <summary>Gets or sets the distance, in pixels, between the top edge of the control and the top edge of its container's client area.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the distance, in pixels, between the bottom edge of the control and the top edge of its container's client area.</returns>
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00029C0F File Offset: 0x00027E0F
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00029C17 File Offset: 0x00027E17
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlTopDescr")]
		public int Top
		{
			get
			{
				return this.y;
			}
			set
			{
				this.SetBounds(this.x, value, this.width, this.height, BoundsSpecified.Y);
			}
		}

		/// <summary>Gets the parent control that is not parented by another Windows Forms control. Typically, this is the outermost <see cref="T:System.Windows.Forms.Form" /> that the control is contained in.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that represents the top-level control that contains the current control.</returns>
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00029C33 File Offset: 0x00027E33
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlTopLevelControlDescr")]
		public Control TopLevelControl
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.TopLevelControlInternal;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00029C48 File Offset: 0x00027E48
		internal Control TopLevelControlInternal
		{
			get
			{
				Control control = this;
				while (control != null && !control.GetTopLevel())
				{
					control = control.ParentInternal;
				}
				return control;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00029C6C File Offset: 0x00027E6C
		internal Control TopMostParent
		{
			get
			{
				Control control = this;
				while (control.ParentInternal != null)
				{
					control = control.ParentInternal;
				}
				return control;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00029C8D File Offset: 0x00027E8D
		private BufferedGraphicsContext BufferContext
		{
			get
			{
				return BufferedGraphicsManager.Current;
			}
		}

		/// <summary>Gets a value indicating whether the user interface is in the appropriate state to show or hide keyboard accelerators.</summary>
		/// <returns>
		///     <see langword="true" /> if the keyboard accelerators are visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00029C94 File Offset: 0x00027E94
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal virtual bool ShowKeyboardCues
		{
			get
			{
				if (!this.IsHandleCreated || base.DesignMode)
				{
					return true;
				}
				if ((this.uiCuesState & 240) == 0)
				{
					if (SystemInformation.MenuAccessKeysUnderlined)
					{
						this.uiCuesState |= 32;
					}
					else
					{
						int num = (2 | (AccessibilityImprovements.Level1 ? 0 : 1)) << 16;
						this.uiCuesState |= 16;
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TopMostParent, this.TopMostParent.Handle), 295, (IntPtr)(num | 1), IntPtr.Zero);
					}
				}
				return (this.uiCuesState & 240) == 32;
			}
		}

		/// <summary>Gets a value indicating whether the control should display focus rectangles.</summary>
		/// <returns>
		///     <see langword="true" /> if the control should display focus rectangles; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00029D38 File Offset: 0x00027F38
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal virtual bool ShowFocusCues
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return true;
				}
				if ((this.uiCuesState & 15) == 0)
				{
					if (SystemInformation.MenuAccessKeysUnderlined)
					{
						this.uiCuesState |= 2;
					}
					else
					{
						this.uiCuesState |= 1;
						int num = 196608;
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TopMostParent, this.TopMostParent.Handle), 295, (IntPtr)(num | 1), IntPtr.Zero);
					}
				}
				return (this.uiCuesState & 15) == 2;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00029DBF File Offset: 0x00027FBF
		internal virtual int ShowParams
		{
			get
			{
				return 5;
			}
		}

		/// <summary>Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.</summary>
		/// <returns>
		///     <see langword="true" /> to use the wait cursor for the current control and all child controls; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00029DC2 File Offset: 0x00027FC2
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x00029DD0 File Offset: 0x00027FD0
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ControlUseWaitCursorDescr")]
		public bool UseWaitCursor
		{
			get
			{
				return this.GetState(1024);
			}
			set
			{
				if (this.GetState(1024) != value)
				{
					this.SetState(1024, value);
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							controlCollection[i].UseWaitCursor = value;
						}
					}
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00029E30 File Offset: 0x00028030
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00029E70 File Offset: 0x00028070
		internal bool UseCompatibleTextRenderingInt
		{
			get
			{
				if (this.Properties.ContainsInteger(Control.PropUseCompatibleTextRendering))
				{
					bool flag;
					int integer = this.Properties.GetInteger(Control.PropUseCompatibleTextRendering, out flag);
					if (flag)
					{
						return integer == 1;
					}
				}
				return Control.UseCompatibleTextRenderingDefault;
			}
			set
			{
				if (this.SupportsUseCompatibleTextRendering && this.UseCompatibleTextRenderingInt != value)
				{
					this.Properties.SetInteger(Control.PropUseCompatibleTextRendering, value ? 1 : 0);
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.UseCompatibleTextRendering);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00029EC4 File Offset: 0x000280C4
		private Control.ControlVersionInfo VersionInfo
		{
			get
			{
				Control.ControlVersionInfo controlVersionInfo = (Control.ControlVersionInfo)this.Properties.GetObject(Control.PropControlVersionInfo);
				if (controlVersionInfo == null)
				{
					controlVersionInfo = new Control.ControlVersionInfo(this);
					this.Properties.SetObject(Control.PropControlVersionInfo, controlVersionInfo);
				}
				return controlVersionInfo;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control and all its child controls are displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the control and all its child controls are displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00029F03 File Offset: 0x00028103
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x00029F0B File Offset: 0x0002810B
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ControlVisibleDescr")]
		public bool Visible
		{
			get
			{
				return this.GetVisibleCore();
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Visible" /> property value changes.</summary>
		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06000D18 RID: 3352 RVA: 0x00029F14 File Offset: 0x00028114
		// (remove) Token: 0x06000D19 RID: 3353 RVA: 0x00029F27 File Offset: 0x00028127
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnVisibleChangedDescr")]
		public event EventHandler VisibleChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventVisible, value);
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00029F3C File Offset: 0x0002813C
		private void WaitForWaitHandle(WaitHandle waitHandle)
		{
			int createThreadId = this.CreateThreadId;
			Application.ThreadContext threadContext = Application.ThreadContext.FromId(createThreadId);
			if (threadContext == null)
			{
				return;
			}
			IntPtr handle = threadContext.GetHandle();
			bool flag = false;
			uint num = 0U;
			while (!flag)
			{
				bool exitCodeThread = UnsafeNativeMethods.GetExitCodeThread(handle, out num);
				if ((exitCodeThread && num != 259U) || (!exitCodeThread && Marshal.GetLastWin32Error() == 6) || AppDomain.CurrentDomain.IsFinalizingForUnload())
				{
					if (!waitHandle.WaitOne(1, false))
					{
						throw new InvalidAsynchronousStateException(SR.GetString("ThreadNoLongerValid"));
					}
					break;
				}
				else
				{
					if (this.IsDisposed && this.threadCallbackList != null && this.threadCallbackList.Count > 0)
					{
						Queue obj = this.threadCallbackList;
						lock (obj)
						{
							Exception exception = new ObjectDisposedException(base.GetType().Name);
							while (this.threadCallbackList.Count > 0)
							{
								Control.ThreadMethodEntry threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
								threadMethodEntry.exception = exception;
								threadMethodEntry.Complete();
							}
						}
					}
					flag = waitHandle.WaitOne(1000, false);
				}
			}
		}

		/// <summary>Gets or sets the width of the control.</summary>
		/// <returns>The width of the control in pixels.</returns>
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0002A064 File Offset: 0x00028264
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x0002A06C File Offset: 0x0002826C
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWidthDescr")]
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.SetBounds(this.x, this.y, value, this.height, BoundsSpecified.Width);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0002A088 File Offset: 0x00028288
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x0002A0A3 File Offset: 0x000282A3
		private int WindowExStyle
		{
			get
			{
				return (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -20));
			}
			set
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -20, new HandleRef(null, (IntPtr)value));
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0002A0C5 File Offset: 0x000282C5
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x0002A0E0 File Offset: 0x000282E0
		internal int WindowStyle
		{
			get
			{
				return (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16));
			}
			set
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)value));
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The NativeWindow contained within the control.</returns>
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0002A102 File Offset: 0x00028302
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x0002A10F File Offset: 0x0002830F
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWindowTargetDescr")]
		public IWindowTarget WindowTarget
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.window.WindowTarget;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				this.window.WindowTarget = value;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0002A120 File Offset: 0x00028320
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x0002A1C0 File Offset: 0x000283C0
		internal virtual string WindowText
		{
			get
			{
				if (this.IsHandleCreated)
				{
					string result;
					using (new Control.MultithreadSafeCallScope())
					{
						int num = SafeNativeMethods.GetWindowTextLength(new HandleRef(this.window, this.Handle));
						if (SystemInformation.DbcsEnabled)
						{
							num = num * 2 + 1;
						}
						StringBuilder stringBuilder = new StringBuilder(num + 1);
						UnsafeNativeMethods.GetWindowText(new HandleRef(this.window, this.Handle), stringBuilder, stringBuilder.Capacity);
						result = stringBuilder.ToString();
					}
					return result;
				}
				if (this.text == null)
				{
					return "";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.WindowText.Equals(value))
				{
					if (this.IsHandleCreated)
					{
						UnsafeNativeMethods.SetWindowText(new HandleRef(this.window, this.Handle), value);
						return;
					}
					if (value.Length == 0)
					{
						this.text = null;
						return;
					}
					this.text = value;
				}
			}
		}

		/// <summary>Occurs when the control is clicked.</summary>
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06000D25 RID: 3365 RVA: 0x0002A21D File Offset: 0x0002841D
		// (remove) Token: 0x06000D26 RID: 3366 RVA: 0x0002A230 File Offset: 0x00028430
		[SRCategory("CatAction")]
		[SRDescription("ControlOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(Control.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventClick, value);
			}
		}

		/// <summary>Occurs when a new control is added to the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</summary>
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06000D27 RID: 3367 RVA: 0x0002A243 File Offset: 0x00028443
		// (remove) Token: 0x06000D28 RID: 3368 RVA: 0x0002A256 File Offset: 0x00028456
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnControlAddedDescr")]
		public event ControlEventHandler ControlAdded
		{
			add
			{
				base.Events.AddHandler(Control.EventControlAdded, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventControlAdded, value);
			}
		}

		/// <summary>Occurs when a control is removed from the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</summary>
		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06000D29 RID: 3369 RVA: 0x0002A269 File Offset: 0x00028469
		// (remove) Token: 0x06000D2A RID: 3370 RVA: 0x0002A27C File Offset: 0x0002847C
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnControlRemovedDescr")]
		public event ControlEventHandler ControlRemoved
		{
			add
			{
				base.Events.AddHandler(Control.EventControlRemoved, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventControlRemoved, value);
			}
		}

		/// <summary>Occurs when a drag-and-drop operation is completed.</summary>
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06000D2B RID: 3371 RVA: 0x0002A28F File Offset: 0x0002848F
		// (remove) Token: 0x06000D2C RID: 3372 RVA: 0x0002A2A2 File Offset: 0x000284A2
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragDropDescr")]
		public event DragEventHandler DragDrop
		{
			add
			{
				base.Events.AddHandler(Control.EventDragDrop, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragDrop, value);
			}
		}

		/// <summary>Occurs when an object is dragged into the control's bounds.</summary>
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06000D2D RID: 3373 RVA: 0x0002A2B5 File Offset: 0x000284B5
		// (remove) Token: 0x06000D2E RID: 3374 RVA: 0x0002A2C8 File Offset: 0x000284C8
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragEnterDescr")]
		public event DragEventHandler DragEnter
		{
			add
			{
				base.Events.AddHandler(Control.EventDragEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragEnter, value);
			}
		}

		/// <summary>Occurs when an object is dragged over the control's bounds.</summary>
		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06000D2F RID: 3375 RVA: 0x0002A2DB File Offset: 0x000284DB
		// (remove) Token: 0x06000D30 RID: 3376 RVA: 0x0002A2EE File Offset: 0x000284EE
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragOverDescr")]
		public event DragEventHandler DragOver
		{
			add
			{
				base.Events.AddHandler(Control.EventDragOver, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragOver, value);
			}
		}

		/// <summary>Occurs when an object is dragged out of the control's bounds.</summary>
		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06000D31 RID: 3377 RVA: 0x0002A301 File Offset: 0x00028501
		// (remove) Token: 0x06000D32 RID: 3378 RVA: 0x0002A314 File Offset: 0x00028514
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragLeaveDescr")]
		public event EventHandler DragLeave
		{
			add
			{
				base.Events.AddHandler(Control.EventDragLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragLeave, value);
			}
		}

		/// <summary>Occurs during a drag operation.</summary>
		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06000D33 RID: 3379 RVA: 0x0002A327 File Offset: 0x00028527
		// (remove) Token: 0x06000D34 RID: 3380 RVA: 0x0002A33A File Offset: 0x0002853A
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnGiveFeedbackDescr")]
		public event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.Events.AddHandler(Control.EventGiveFeedback, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventGiveFeedback, value);
			}
		}

		/// <summary>Occurs when a handle is created for the control.</summary>
		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06000D35 RID: 3381 RVA: 0x0002A34D File Offset: 0x0002854D
		// (remove) Token: 0x06000D36 RID: 3382 RVA: 0x0002A360 File Offset: 0x00028560
		[SRCategory("CatPrivate")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnCreateHandleDescr")]
		public event EventHandler HandleCreated
		{
			add
			{
				base.Events.AddHandler(Control.EventHandleCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHandleCreated, value);
			}
		}

		/// <summary>Occurs when the control's handle is in the process of being destroyed.</summary>
		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06000D37 RID: 3383 RVA: 0x0002A373 File Offset: 0x00028573
		// (remove) Token: 0x06000D38 RID: 3384 RVA: 0x0002A386 File Offset: 0x00028586
		[SRCategory("CatPrivate")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnDestroyHandleDescr")]
		public event EventHandler HandleDestroyed
		{
			add
			{
				base.Events.AddHandler(Control.EventHandleDestroyed, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHandleDestroyed, value);
			}
		}

		/// <summary>Occurs when the user requests help for a control.</summary>
		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06000D39 RID: 3385 RVA: 0x0002A399 File Offset: 0x00028599
		// (remove) Token: 0x06000D3A RID: 3386 RVA: 0x0002A3AC File Offset: 0x000285AC
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnHelpDescr")]
		public event HelpEventHandler HelpRequested
		{
			add
			{
				base.Events.AddHandler(Control.EventHelpRequested, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHelpRequested, value);
			}
		}

		/// <summary>Occurs when a control's display requires redrawing.</summary>
		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06000D3B RID: 3387 RVA: 0x0002A3BF File Offset: 0x000285BF
		// (remove) Token: 0x06000D3C RID: 3388 RVA: 0x0002A3D2 File Offset: 0x000285D2
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnInvalidateDescr")]
		public event InvalidateEventHandler Invalidated
		{
			add
			{
				base.Events.AddHandler(Control.EventInvalidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventInvalidated, value);
			}
		}

		/// <summary>Gets the size of a rectangular area into which the control can fit.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> containing the height and width, in pixels.</returns>
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0002A3E5 File Offset: 0x000285E5
		[Browsable(false)]
		public Size PreferredSize
		{
			get
			{
				return this.GetPreferredSize(Size.Empty);
			}
		}

		/// <summary>Gets or sets padding within the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the control's internal spacing characteristics.</returns>
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0002A3F2 File Offset: 0x000285F2
		// (set) Token: 0x06000D3F RID: 3391 RVA: 0x0002A400 File Offset: 0x00028600
		[SRDescription("ControlPaddingDescr")]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		public Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (value != this.Padding)
				{
					CommonProperties.SetPadding(this, value);
					this.SetState(8388608, true);
					using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Padding))
					{
						this.OnPaddingChanged(EventArgs.Empty);
					}
					if (this.GetState(8388608))
					{
						LayoutTransaction.DoLayout(this, this, PropertyNames.Padding);
					}
				}
			}
		}

		/// <summary>Occurs when the control's padding changes.</summary>
		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06000D40 RID: 3392 RVA: 0x0002A480 File Offset: 0x00028680
		// (remove) Token: 0x06000D41 RID: 3393 RVA: 0x0002A493 File Offset: 0x00028693
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnPaddingChangedDescr")]
		public event EventHandler PaddingChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventPaddingChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventPaddingChanged, value);
			}
		}

		/// <summary>Occurs when the control is redrawn.</summary>
		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06000D42 RID: 3394 RVA: 0x0002A4A6 File Offset: 0x000286A6
		// (remove) Token: 0x06000D43 RID: 3395 RVA: 0x0002A4B9 File Offset: 0x000286B9
		[SRCategory("CatAppearance")]
		[SRDescription("ControlOnPaintDescr")]
		public event PaintEventHandler Paint
		{
			add
			{
				base.Events.AddHandler(Control.EventPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventPaint, value);
			}
		}

		/// <summary>Occurs during a drag-and-drop operation and enables the drag source to determine whether the drag-and-drop operation should be canceled.</summary>
		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06000D44 RID: 3396 RVA: 0x0002A4CC File Offset: 0x000286CC
		// (remove) Token: 0x06000D45 RID: 3397 RVA: 0x0002A4DF File Offset: 0x000286DF
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnQueryContinueDragDescr")]
		public event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.Events.AddHandler(Control.EventQueryContinueDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventQueryContinueDrag, value);
			}
		}

		/// <summary>Occurs when <see cref="T:System.Windows.Forms.AccessibleObject" /> is providing help to accessibility applications.</summary>
		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06000D46 RID: 3398 RVA: 0x0002A4F2 File Offset: 0x000286F2
		// (remove) Token: 0x06000D47 RID: 3399 RVA: 0x0002A505 File Offset: 0x00028705
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnQueryAccessibilityHelpDescr")]
		public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				base.Events.AddHandler(Control.EventQueryAccessibilityHelp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventQueryAccessibilityHelp, value);
			}
		}

		/// <summary>Occurs when the control is double-clicked.</summary>
		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06000D48 RID: 3400 RVA: 0x0002A518 File Offset: 0x00028718
		// (remove) Token: 0x06000D49 RID: 3401 RVA: 0x0002A52B File Offset: 0x0002872B
		[SRCategory("CatAction")]
		[SRDescription("ControlOnDoubleClickDescr")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(Control.EventDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDoubleClick, value);
			}
		}

		/// <summary>Occurs when the control is entered.</summary>
		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06000D4A RID: 3402 RVA: 0x0002A53E File Offset: 0x0002873E
		// (remove) Token: 0x06000D4B RID: 3403 RVA: 0x0002A551 File Offset: 0x00028751
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnEnterDescr")]
		public event EventHandler Enter
		{
			add
			{
				base.Events.AddHandler(Control.EventEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventEnter, value);
			}
		}

		/// <summary>Occurs when the control receives focus.</summary>
		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06000D4C RID: 3404 RVA: 0x0002A564 File Offset: 0x00028764
		// (remove) Token: 0x06000D4D RID: 3405 RVA: 0x0002A577 File Offset: 0x00028777
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnGotFocusDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler GotFocus
		{
			add
			{
				base.Events.AddHandler(Control.EventGotFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventGotFocus, value);
			}
		}

		/// <summary>Occurs when a key is pressed while the control has focus.</summary>
		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06000D4E RID: 3406 RVA: 0x0002A58A File Offset: 0x0002878A
		// (remove) Token: 0x06000D4F RID: 3407 RVA: 0x0002A59D File Offset: 0x0002879D
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyDownDescr")]
		public event KeyEventHandler KeyDown
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyDown, value);
			}
		}

		/// <summary>Occurs when a character. space or backspace key is pressed while the control has focus.</summary>
		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06000D50 RID: 3408 RVA: 0x0002A5B0 File Offset: 0x000287B0
		// (remove) Token: 0x06000D51 RID: 3409 RVA: 0x0002A5C3 File Offset: 0x000287C3
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyPressDescr")]
		public event KeyPressEventHandler KeyPress
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyPress, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyPress, value);
			}
		}

		/// <summary>Occurs when a key is released while the control has focus.</summary>
		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06000D52 RID: 3410 RVA: 0x0002A5D6 File Offset: 0x000287D6
		// (remove) Token: 0x06000D53 RID: 3411 RVA: 0x0002A5E9 File Offset: 0x000287E9
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyUpDescr")]
		public event KeyEventHandler KeyUp
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyUp, value);
			}
		}

		/// <summary>Occurs when a control should reposition its child controls.</summary>
		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06000D54 RID: 3412 RVA: 0x0002A5FC File Offset: 0x000287FC
		// (remove) Token: 0x06000D55 RID: 3413 RVA: 0x0002A60F File Offset: 0x0002880F
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnLayoutDescr")]
		public event LayoutEventHandler Layout
		{
			add
			{
				base.Events.AddHandler(Control.EventLayout, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLayout, value);
			}
		}

		/// <summary>Occurs when the input focus leaves the control.</summary>
		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06000D56 RID: 3414 RVA: 0x0002A622 File Offset: 0x00028822
		// (remove) Token: 0x06000D57 RID: 3415 RVA: 0x0002A635 File Offset: 0x00028835
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnLeaveDescr")]
		public event EventHandler Leave
		{
			add
			{
				base.Events.AddHandler(Control.EventLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLeave, value);
			}
		}

		/// <summary>Occurs when the control loses focus.</summary>
		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06000D58 RID: 3416 RVA: 0x0002A648 File Offset: 0x00028848
		// (remove) Token: 0x06000D59 RID: 3417 RVA: 0x0002A65B File Offset: 0x0002885B
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnLostFocusDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler LostFocus
		{
			add
			{
				base.Events.AddHandler(Control.EventLostFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLostFocus, value);
			}
		}

		/// <summary>Occurs when the control is clicked by the mouse.</summary>
		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06000D5A RID: 3418 RVA: 0x0002A66E File Offset: 0x0002886E
		// (remove) Token: 0x06000D5B RID: 3419 RVA: 0x0002A681 File Offset: 0x00028881
		[SRCategory("CatAction")]
		[SRDescription("ControlOnMouseClickDescr")]
		public event MouseEventHandler MouseClick
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseClick, value);
			}
		}

		/// <summary>Occurs when the control is double clicked by the mouse.</summary>
		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06000D5C RID: 3420 RVA: 0x0002A694 File Offset: 0x00028894
		// (remove) Token: 0x06000D5D RID: 3421 RVA: 0x0002A6A7 File Offset: 0x000288A7
		[SRCategory("CatAction")]
		[SRDescription("ControlOnMouseDoubleClickDescr")]
		public event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseDoubleClick, value);
			}
		}

		/// <summary>Occurs when the control loses mouse capture.</summary>
		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06000D5E RID: 3422 RVA: 0x0002A6BA File Offset: 0x000288BA
		// (remove) Token: 0x06000D5F RID: 3423 RVA: 0x0002A6CD File Offset: 0x000288CD
		[SRCategory("CatAction")]
		[SRDescription("ControlOnMouseCaptureChangedDescr")]
		public event EventHandler MouseCaptureChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseCaptureChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseCaptureChanged, value);
			}
		}

		/// <summary>Occurs when the mouse pointer is over the control and a mouse button is pressed.</summary>
		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06000D60 RID: 3424 RVA: 0x0002A6E0 File Offset: 0x000288E0
		// (remove) Token: 0x06000D61 RID: 3425 RVA: 0x0002A6F3 File Offset: 0x000288F3
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseDownDescr")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseDown, value);
			}
		}

		/// <summary>Occurs when the mouse pointer enters the control.</summary>
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06000D62 RID: 3426 RVA: 0x0002A706 File Offset: 0x00028906
		// (remove) Token: 0x06000D63 RID: 3427 RVA: 0x0002A719 File Offset: 0x00028919
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseEnterDescr")]
		public event EventHandler MouseEnter
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseEnter, value);
			}
		}

		/// <summary>Occurs when the mouse pointer leaves the control.</summary>
		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06000D64 RID: 3428 RVA: 0x0002A72C File Offset: 0x0002892C
		// (remove) Token: 0x06000D65 RID: 3429 RVA: 0x0002A73F File Offset: 0x0002893F
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseLeaveDescr")]
		public event EventHandler MouseLeave
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseLeave, value);
			}
		}

		/// <summary>Occurs when the DPI setting for a control is changed programmatically before a DPI change event for its parent control or form has occurred.</summary>
		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06000D66 RID: 3430 RVA: 0x0002A752 File Offset: 0x00028952
		// (remove) Token: 0x06000D67 RID: 3431 RVA: 0x0002A765 File Offset: 0x00028965
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnDpiChangedBeforeParentDescr")]
		public event EventHandler DpiChangedBeforeParent
		{
			add
			{
				base.Events.AddHandler(Control.EventDpiChangedBeforeParent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDpiChangedBeforeParent, value);
			}
		}

		/// <summary>Occurs when the DPI setting for a control is changed programmatically after the DPI of its parent control or form has changed.</summary>
		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06000D68 RID: 3432 RVA: 0x0002A778 File Offset: 0x00028978
		// (remove) Token: 0x06000D69 RID: 3433 RVA: 0x0002A78B File Offset: 0x0002898B
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnDpiChangedAfterParentDescr")]
		public event EventHandler DpiChangedAfterParent
		{
			add
			{
				base.Events.AddHandler(Control.EventDpiChangedAfterParent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDpiChangedAfterParent, value);
			}
		}

		/// <summary>Occurs when the mouse pointer rests on the control.</summary>
		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06000D6A RID: 3434 RVA: 0x0002A79E File Offset: 0x0002899E
		// (remove) Token: 0x06000D6B RID: 3435 RVA: 0x0002A7B1 File Offset: 0x000289B1
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseHoverDescr")]
		public event EventHandler MouseHover
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseHover, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseHover, value);
			}
		}

		/// <summary>Occurs when the mouse pointer is moved over the control.</summary>
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06000D6C RID: 3436 RVA: 0x0002A7C4 File Offset: 0x000289C4
		// (remove) Token: 0x06000D6D RID: 3437 RVA: 0x0002A7D7 File Offset: 0x000289D7
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseMoveDescr")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseMove, value);
			}
		}

		/// <summary>Occurs when the mouse pointer is over the control and a mouse button is released.</summary>
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06000D6E RID: 3438 RVA: 0x0002A7EA File Offset: 0x000289EA
		// (remove) Token: 0x06000D6F RID: 3439 RVA: 0x0002A7FD File Offset: 0x000289FD
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseUpDescr")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseUp, value);
			}
		}

		/// <summary>Occurs when the mouse wheel moves while the control has focus.</summary>
		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06000D70 RID: 3440 RVA: 0x0002A810 File Offset: 0x00028A10
		// (remove) Token: 0x06000D71 RID: 3441 RVA: 0x0002A823 File Offset: 0x00028A23
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseWheelDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event MouseEventHandler MouseWheel
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseWheel, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseWheel, value);
			}
		}

		/// <summary>Occurs when the control is moved.</summary>
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06000D72 RID: 3442 RVA: 0x0002A836 File Offset: 0x00028A36
		// (remove) Token: 0x06000D73 RID: 3443 RVA: 0x0002A849 File Offset: 0x00028A49
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnMoveDescr")]
		public event EventHandler Move
		{
			add
			{
				base.Events.AddHandler(Control.EventMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMove, value);
			}
		}

		/// <summary>Occurs before the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event when a key is pressed while focus is on this control.</summary>
		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06000D74 RID: 3444 RVA: 0x0002A85C File Offset: 0x00028A5C
		// (remove) Token: 0x06000D75 RID: 3445 RVA: 0x0002A86F File Offset: 0x00028A6F
		[SRCategory("CatKey")]
		[SRDescription("PreviewKeyDownDescr")]
		public event PreviewKeyDownEventHandler PreviewKeyDown
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				base.Events.AddHandler(Control.EventPreviewKeyDown, value);
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				base.Events.RemoveHandler(Control.EventPreviewKeyDown, value);
			}
		}

		/// <summary>Occurs when the control is resized.</summary>
		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06000D76 RID: 3446 RVA: 0x0002A882 File Offset: 0x00028A82
		// (remove) Token: 0x06000D77 RID: 3447 RVA: 0x0002A895 File Offset: 0x00028A95
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnResizeDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Resize
		{
			add
			{
				base.Events.AddHandler(Control.EventResize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventResize, value);
			}
		}

		/// <summary>Occurs when the focus or keyboard user interface (UI) cues change.</summary>
		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06000D78 RID: 3448 RVA: 0x0002A8A8 File Offset: 0x00028AA8
		// (remove) Token: 0x06000D79 RID: 3449 RVA: 0x0002A8BB File Offset: 0x00028ABB
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnChangeUICuesDescr")]
		public event UICuesEventHandler ChangeUICues
		{
			add
			{
				base.Events.AddHandler(Control.EventChangeUICues, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventChangeUICues, value);
			}
		}

		/// <summary>Occurs when the control style changes.</summary>
		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06000D7A RID: 3450 RVA: 0x0002A8CE File Offset: 0x00028ACE
		// (remove) Token: 0x06000D7B RID: 3451 RVA: 0x0002A8E1 File Offset: 0x00028AE1
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnStyleChangedDescr")]
		public event EventHandler StyleChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventStyleChanged, value);
			}
		}

		/// <summary>Occurs when the system colors change.</summary>
		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06000D7C RID: 3452 RVA: 0x0002A8F4 File Offset: 0x00028AF4
		// (remove) Token: 0x06000D7D RID: 3453 RVA: 0x0002A907 File Offset: 0x00028B07
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnSystemColorsChangedDescr")]
		public event EventHandler SystemColorsChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventSystemColorsChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventSystemColorsChanged, value);
			}
		}

		/// <summary>Occurs when the control is validating.</summary>
		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06000D7E RID: 3454 RVA: 0x0002A91A File Offset: 0x00028B1A
		// (remove) Token: 0x06000D7F RID: 3455 RVA: 0x0002A92D File Offset: 0x00028B2D
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatingDescr")]
		public event CancelEventHandler Validating
		{
			add
			{
				base.Events.AddHandler(Control.EventValidating, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventValidating, value);
			}
		}

		/// <summary>Occurs when the control is finished validating.</summary>
		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06000D80 RID: 3456 RVA: 0x0002A940 File Offset: 0x00028B40
		// (remove) Token: 0x06000D81 RID: 3457 RVA: 0x0002A953 File Offset: 0x00028B53
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatedDescr")]
		public event EventHandler Validated
		{
			add
			{
				base.Events.AddHandler(Control.EventValidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventValidated, value);
			}
		}

		/// <summary>Notifies the accessibility client applications of the specified <see cref="T:System.Windows.Forms.AccessibleEvents" /> for the specified child control.</summary>
		/// <param name="accEvent">The <see cref="T:System.Windows.Forms.AccessibleEvents" /> to notify the accessibility client applications of. </param>
		/// <param name="childID">The child <see cref="T:System.Windows.Forms.Control" /> to notify of the accessible event. </param>
		// Token: 0x06000D82 RID: 3458 RVA: 0x0002A966 File Offset: 0x00028B66
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal void AccessibilityNotifyClients(AccessibleEvents accEvent, int childID)
		{
			this.AccessibilityNotifyClients(accEvent, -4, childID);
		}

		/// <summary>Notifies the accessibility client applications of the specified <see cref="T:System.Windows.Forms.AccessibleEvents" /> for the specified child control .</summary>
		/// <param name="accEvent">The <see cref="T:System.Windows.Forms.AccessibleEvents" /> to notify the accessibility client applications of.</param>
		/// <param name="objectID">The identifier of the <see cref="T:System.Windows.Forms.AccessibleObject" />.</param>
		/// <param name="childID">The child <see cref="T:System.Windows.Forms.Control" /> to notify of the accessible event.</param>
		// Token: 0x06000D83 RID: 3459 RVA: 0x0002A972 File Offset: 0x00028B72
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void AccessibilityNotifyClients(AccessibleEvents accEvent, int objectID, int childID)
		{
			if (this.IsHandleCreated)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), objectID, childID + 1);
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002A992 File Offset: 0x00028B92
		private IntPtr ActiveXMergeRegion(IntPtr region)
		{
			return this.ActiveXInstance.MergeRegion(region);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002A9A0 File Offset: 0x00028BA0
		private void ActiveXOnFocus(bool focus)
		{
			this.ActiveXInstance.OnFocus(focus);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002A9AE File Offset: 0x00028BAE
		private void ActiveXViewChanged()
		{
			this.ActiveXInstance.ViewChangedInternal();
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002A9BB File Offset: 0x00028BBB
		private void ActiveXUpdateBounds(ref int x, ref int y, ref int width, ref int height, int flags)
		{
			this.ActiveXInstance.UpdateBounds(ref x, ref y, ref width, ref height, flags);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002A9D0 File Offset: 0x00028BD0
		internal virtual void AssignParent(Control value)
		{
			if (value != null)
			{
				this.RequiredScalingEnabled = value.RequiredScalingEnabled;
			}
			if (this.CanAccessProperties)
			{
				Font font = this.Font;
				Color foreColor = this.ForeColor;
				Color backColor = this.BackColor;
				RightToLeft rightToLeft = this.RightToLeft;
				bool enabled = this.Enabled;
				bool visible = this.Visible;
				this.parent = value;
				this.OnParentChanged(EventArgs.Empty);
				if (this.GetAnyDisposingInHierarchy())
				{
					return;
				}
				if (enabled != this.Enabled)
				{
					this.OnEnabledChanged(EventArgs.Empty);
				}
				bool visible2 = this.Visible;
				if (visible != visible2 && (visible || !visible2 || this.parent != null || this.GetTopLevel()))
				{
					this.OnVisibleChanged(EventArgs.Empty);
				}
				if (!font.Equals(this.Font))
				{
					this.OnFontChanged(EventArgs.Empty);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
				if (rightToLeft != this.RightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
				if (this.Properties.GetObject(Control.PropBindingManager) == null && this.Created)
				{
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
			else
			{
				this.parent = value;
				this.OnParentChanged(EventArgs.Empty);
			}
			this.SetState(16777216, false);
			if (this.ParentInternal != null)
			{
				this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.All);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.Parent" /> property value changes.</summary>
		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06000D89 RID: 3465 RVA: 0x0002AB5D File Offset: 0x00028D5D
		// (remove) Token: 0x06000D8A RID: 3466 RVA: 0x0002AB70 File Offset: 0x00028D70
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnParentChangedDescr")]
		public event EventHandler ParentChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventParent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventParent, value);
			}
		}

		/// <summary>Executes the specified delegate asynchronously on the thread that the control's underlying handle was created on.</summary>
		/// <param name="method">A delegate to a method that takes no parameters. </param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that represents the result of the <see cref="M:System.Windows.Forms.Control.BeginInvoke(System.Delegate)" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
		// Token: 0x06000D8B RID: 3467 RVA: 0x0002AB83 File Offset: 0x00028D83
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IAsyncResult BeginInvoke(Delegate method)
		{
			return this.BeginInvoke(method, null);
		}

		/// <summary>Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.</summary>
		/// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the <paramref name="args" /> parameter. </param>
		/// <param name="args">An array of objects to pass as arguments to the given method. This can be <see langword="null" /> if no arguments are needed. </param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that represents the result of the <see cref="M:System.Windows.Forms.Control.BeginInvoke(System.Delegate)" /> operation.</returns>
		/// <exception cref="T:System.InvalidOperationException">No appropriate window handle can be found.</exception>
		// Token: 0x06000D8C RID: 3468 RVA: 0x0002AB90 File Offset: 0x00028D90
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IAsyncResult BeginInvoke(Delegate method, params object[] args)
		{
			IAsyncResult result;
			using (new Control.MultithreadSafeCallScope())
			{
				Control control = this.FindMarshalingControl();
				result = (IAsyncResult)control.MarshaledInvoke(this, method, args, false);
			}
			return result;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0002ABD8 File Offset: 0x00028DD8
		internal void BeginUpdateInternal()
		{
			if (!this.IsHandleCreated)
			{
				return;
			}
			if (this.updateCount == 0)
			{
				this.SendMessage(11, 0, 0);
			}
			this.updateCount += 1;
		}

		/// <summary>Brings the control to the front of the z-order.</summary>
		// Token: 0x06000D8E RID: 3470 RVA: 0x0002AC08 File Offset: 0x00028E08
		public void BringToFront()
		{
			if (this.parent != null)
			{
				this.parent.Controls.SetChildIndex(this, 0);
				return;
			}
			if (this.IsHandleCreated && this.GetTopLevel() && SafeNativeMethods.IsWindowEnabled(new HandleRef(this.window, this.Handle)))
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.HWND_TOP, 0, 0, 0, 0, 3);
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0002AC79 File Offset: 0x00028E79
		internal virtual bool CanProcessMnemonic()
		{
			return this.Enabled && this.Visible && (this.parent == null || this.parent.CanProcessMnemonic());
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0002ACA4 File Offset: 0x00028EA4
		internal virtual bool CanSelectCore()
		{
			if ((this.controlStyle & ControlStyles.Selectable) != ControlStyles.Selectable)
			{
				return false;
			}
			for (Control control = this; control != null; control = control.parent)
			{
				if (!control.Enabled || !control.Visible)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		internal static void CheckParentingCycle(Control bottom, Control toFind)
		{
			Form form = null;
			Control control = null;
			for (Control control2 = bottom; control2 != null; control2 = control2.ParentInternal)
			{
				control = control2;
				if (control2 == toFind)
				{
					throw new ArgumentException(SR.GetString("CircularOwner"));
				}
			}
			if (control != null && control is Form)
			{
				Form form2 = (Form)control;
				for (Form form3 = form2; form3 != null; form3 = form3.OwnerInternal)
				{
					form = form3;
					if (form3 == toFind)
					{
						throw new ArgumentException(SR.GetString("CircularOwner"));
					}
				}
			}
			if (form != null && form.ParentInternal != null)
			{
				Control.CheckParentingCycle(form.ParentInternal, toFind);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0002AD70 File Offset: 0x00028F70
		private void ChildGotFocus(Control child)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(true);
			}
			if (this.parent != null)
			{
				this.parent.ChildGotFocus(child);
			}
		}

		/// <summary>Retrieves a value indicating whether the specified control is a child of the control.</summary>
		/// <param name="ctl">The <see cref="T:System.Windows.Forms.Control" /> to evaluate. </param>
		/// <returns>
		///     <see langword="true" /> if the specified control is a child of the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000D93 RID: 3475 RVA: 0x0002AD95 File Offset: 0x00028F95
		public bool Contains(Control ctl)
		{
			while (ctl != null)
			{
				ctl = ctl.ParentInternal;
				if (ctl == null)
				{
					return false;
				}
				if (ctl == this)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06000D94 RID: 3476 RVA: 0x0002ADB0 File Offset: 0x00028FB0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new Control.ControlAccessibleObject(this);
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x06000D95 RID: 3477 RVA: 0x0002ADB8 File Offset: 0x00028FB8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Control.ControlCollection CreateControlsInstance()
		{
			return new Control.ControlCollection(this);
		}

		/// <summary>Creates the <see cref="T:System.Drawing.Graphics" /> for the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> for the control.</returns>
		// Token: 0x06000D96 RID: 3478 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		public Graphics CreateGraphics()
		{
			Graphics result;
			using (new Control.MultithreadSafeCallScope())
			{
				IntSecurity.CreateGraphicsForControl.Demand();
				result = this.CreateGraphicsInternal();
			}
			return result;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002AE04 File Offset: 0x00029004
		internal Graphics CreateGraphicsInternal()
		{
			return Graphics.FromHwndInternal(this.Handle);
		}

		/// <summary>Creates a handle for the control.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The object is in a disposed state. </exception>
		// Token: 0x06000D98 RID: 3480 RVA: 0x0002AE14 File Offset: 0x00029014
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual void CreateHandle()
		{
			IntPtr userCookie = IntPtr.Zero;
			if (this.GetState(2048))
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.GetState(262144))
			{
				return;
			}
			Rectangle bounds;
			try
			{
				this.SetState(262144, true);
				bounds = this.Bounds;
				if (Application.UseVisualStyles)
				{
					userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				}
				CreateParams createParams = this.CreateParams;
				this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				if (this.parent != null)
				{
					Rectangle clientRectangle = this.parent.ClientRectangle;
					if (!clientRectangle.IsEmpty)
					{
						if (createParams.X != -2147483648)
						{
							createParams.X -= clientRectangle.X;
						}
						if (createParams.Y != -2147483648)
						{
							createParams.Y -= clientRectangle.Y;
						}
					}
				}
				if (createParams.Parent == IntPtr.Zero && (createParams.Style & 1073741824) != 0)
				{
					Application.ParkHandle(createParams, this.DpiAwarenessContext);
				}
				this.window.CreateHandle(createParams);
				this.UpdateReflectParent(true);
			}
			finally
			{
				this.SetState(262144, false);
				UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
			}
			if (this.Bounds != bounds)
			{
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			}
		}

		/// <summary>Forces the creation of the visible control, including the creation of the handle and any visible child controls.</summary>
		// Token: 0x06000D99 RID: 3481 RVA: 0x0002AF78 File Offset: 0x00029178
		public void CreateControl()
		{
			bool created = this.Created;
			this.CreateControl(false);
			if (this.Properties.GetObject(Control.PropBindingManager) == null && this.ParentInternal != null && !created)
			{
				this.OnBindingContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002AFBC File Offset: 0x000291BC
		internal void CreateControl(bool fIgnoreVisible)
		{
			bool flag = (this.state & 1) == 0;
			flag = (flag && this.Visible);
			if (flag || fIgnoreVisible)
			{
				this.state |= 1;
				bool flag2 = false;
				try
				{
					if (!this.IsHandleCreated)
					{
						this.CreateHandle();
					}
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						Control[] array = new Control[controlCollection.Count];
						controlCollection.CopyTo(array, 0);
						foreach (Control control in array)
						{
							if (control.IsHandleCreated)
							{
								control.SetParentHandle(this.Handle);
							}
							control.CreateControl(fIgnoreVisible);
						}
					}
					flag2 = true;
				}
				finally
				{
					if (!flag2)
					{
						this.state &= -2;
					}
				}
				this.OnCreateControl();
			}
		}

		/// <summary>Sends the specified message to the default window procedure.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06000D9B RID: 3483 RVA: 0x0002B0A0 File Offset: 0x000292A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void DefWndProc(ref Message m)
		{
			this.window.DefWndProc(ref m);
		}

		/// <summary>Destroys the handle associated with the control.</summary>
		// Token: 0x06000D9C RID: 3484 RVA: 0x0002B0B0 File Offset: 0x000292B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual void DestroyHandle()
		{
			if (this.RecreatingHandle && this.threadCallbackList != null)
			{
				Queue obj = this.threadCallbackList;
				lock (obj)
				{
					if (Control.threadCallbackMessage != 0)
					{
						NativeMethods.MSG msg = default(NativeMethods.MSG);
						if (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, this.Handle), Control.threadCallbackMessage, Control.threadCallbackMessage, 0))
						{
							this.SetState(32768, true);
						}
					}
				}
			}
			if (!this.RecreatingHandle && this.threadCallbackList != null)
			{
				Queue obj2 = this.threadCallbackList;
				lock (obj2)
				{
					Exception exception = new ObjectDisposedException(base.GetType().Name);
					while (this.threadCallbackList.Count > 0)
					{
						Control.ThreadMethodEntry threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
						threadMethodEntry.exception = exception;
						threadMethodEntry.Complete();
					}
				}
			}
			if ((64 & (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this.window, this.InternalHandle), -20))) != 0)
			{
				UnsafeNativeMethods.DefMDIChildProc(this.InternalHandle, 16, IntPtr.Zero, IntPtr.Zero);
			}
			else
			{
				this.window.DestroyHandle();
			}
			this.trackMouseEvent = null;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000D9D RID: 3485 RVA: 0x0002B204 File Offset: 0x00029404
		protected override void Dispose(bool disposing)
		{
			if (this.GetState(2097152))
			{
				object @object = this.Properties.GetObject(Control.PropBackBrush);
				if (@object != null)
				{
					IntPtr intPtr = (IntPtr)@object;
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
					}
					this.Properties.SetObject(Control.PropBackBrush, null);
				}
			}
			this.UpdateReflectParent(false);
			if (disposing)
			{
				if (this.GetState(4096))
				{
					return;
				}
				if (this.GetState(262144))
				{
					throw new InvalidOperationException(SR.GetString("ClosingWhileCreatingHandle", new object[]
					{
						"Dispose"
					}));
				}
				this.SetState(4096, true);
				this.SuspendLayout();
				try
				{
					this.DisposeAxControls();
					ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
					if (contextMenu != null)
					{
						contextMenu.Disposed -= this.DetachContextMenu;
					}
					this.ResetBindings();
					if (this.IsHandleCreated)
					{
						this.DestroyHandle();
					}
					if (this.parent != null)
					{
						this.parent.Controls.Remove(this);
					}
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							Control control = controlCollection[i];
							control.parent = null;
							control.Dispose();
						}
						this.Properties.SetObject(Control.PropControlsCollection, null);
					}
					base.Dispose(disposing);
					return;
				}
				finally
				{
					this.ResumeLayout(false);
					this.SetState(4096, false);
					this.SetState(2048, true);
				}
			}
			if (this.window != null)
			{
				this.window.ForceExitMessageLoop();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002B3C8 File Offset: 0x000295C8
		internal virtual void DisposeAxControls()
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].DisposeAxControls();
				}
			}
		}

		/// <summary>Begins a drag-and-drop operation.</summary>
		/// <param name="data">The data to drag. </param>
		/// <param name="allowedEffects">One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values. </param>
		/// <returns>A value from the <see cref="T:System.Windows.Forms.DragDropEffects" /> enumeration that represents the final effect that was performed during the drag-and-drop operation.</returns>
		// Token: 0x06000D9F RID: 3487 RVA: 0x0002B40C File Offset: 0x0002960C
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
		{
			int[] array = new int[1];
			UnsafeNativeMethods.IOleDropSource dropSource = new DropSource(this);
			IDataObject dataObject;
			if (data is IDataObject)
			{
				dataObject = (IDataObject)data;
			}
			else
			{
				DataObject dataObject2;
				if (data is IDataObject)
				{
					dataObject2 = new DataObject((IDataObject)data);
				}
				else
				{
					dataObject2 = new DataObject();
					dataObject2.SetData(data);
				}
				dataObject = dataObject2;
			}
			try
			{
				SafeNativeMethods.DoDragDrop(dataObject, dropSource, (int)allowedEffects, array);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return (DragDropEffects)array[0];
		}

		/// <summary>Supports rendering to the specified bitmap.</summary>
		/// <param name="bitmap">The bitmap to be drawn to.</param>
		/// <param name="targetBounds">The bounds within which the control is rendered.</param>
		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002B490 File Offset: 0x00029690
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			if (targetBounds.Width <= 0 || targetBounds.Height <= 0 || targetBounds.X < 0 || targetBounds.Y < 0)
			{
				throw new ArgumentException("targetBounds");
			}
			if (!this.IsHandleCreated)
			{
				this.CreateHandle();
			}
			int nWidth = Math.Min(this.Width, targetBounds.Width);
			int nHeight = Math.Min(this.Height, targetBounds.Height);
			using (Bitmap bitmap2 = new Bitmap(nWidth, nHeight, bitmap.PixelFormat))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap2))
				{
					IntPtr hdc = graphics.GetHdc();
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 791, hdc, (IntPtr)30);
					using (Graphics graphics2 = Graphics.FromImage(bitmap))
					{
						IntPtr hdc2 = graphics2.GetHdc();
						SafeNativeMethods.BitBlt(new HandleRef(graphics2, hdc2), targetBounds.X, targetBounds.Y, nWidth, nHeight, new HandleRef(graphics, hdc), 0, 0, 13369376);
						graphics2.ReleaseHdcInternal(hdc2);
					}
					graphics.ReleaseHdcInternal(hdc);
				}
			}
		}

		/// <summary>Retrieves the return value of the asynchronous operation represented by the <see cref="T:System.IAsyncResult" /> passed.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> that represents a specific invoke asynchronous operation, returned when calling <see cref="M:System.Windows.Forms.Control.BeginInvoke(System.Delegate)" />. </param>
		/// <returns>The <see cref="T:System.Object" /> generated by the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> parameter value is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="asyncResult" /> object was not created by a preceding call of the <see cref="M:System.Windows.Forms.Control.BeginInvoke(System.Delegate)" /> method from the same control. </exception>
		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002B5EC File Offset: 0x000297EC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object EndInvoke(IAsyncResult asyncResult)
		{
			object retVal;
			using (new Control.MultithreadSafeCallScope())
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				Control.ThreadMethodEntry threadMethodEntry = asyncResult as Control.ThreadMethodEntry;
				if (threadMethodEntry == null)
				{
					throw new ArgumentException(SR.GetString("ControlBadAsyncResult"), "asyncResult");
				}
				if (!asyncResult.IsCompleted)
				{
					Control control = this.FindMarshalingControl();
					int num;
					if (SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(control, control.Handle), out num) == SafeNativeMethods.GetCurrentThreadId())
					{
						control.InvokeMarshaledCallbacks();
					}
					else
					{
						control = threadMethodEntry.marshaler;
						control.WaitForWaitHandle(asyncResult.AsyncWaitHandle);
					}
				}
				if (threadMethodEntry.exception != null)
				{
					throw threadMethodEntry.exception;
				}
				retVal = threadMethodEntry.retVal;
			}
			return retVal;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002B6A8 File Offset: 0x000298A8
		internal bool EndUpdateInternal()
		{
			return this.EndUpdateInternal(true);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002B6B1 File Offset: 0x000298B1
		internal bool EndUpdateInternal(bool invalidate)
		{
			if (this.updateCount > 0)
			{
				this.updateCount -= 1;
				if (this.updateCount == 0)
				{
					this.SendMessage(11, -1, 0);
					if (invalidate)
					{
						this.Invalidate();
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Retrieves the form that the control is on.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Form" /> that the control is on.</returns>
		// Token: 0x06000DA4 RID: 3492 RVA: 0x0002B6EA File Offset: 0x000298EA
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public Form FindForm()
		{
			return this.FindFormInternal();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0002B6F4 File Offset: 0x000298F4
		internal Form FindFormInternal()
		{
			Control control = this;
			while (control != null && !(control is Form))
			{
				control = control.ParentInternal;
			}
			return (Form)control;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002B720 File Offset: 0x00029920
		private Control FindMarshalingControl()
		{
			Control result;
			lock (this)
			{
				Control control = this;
				while (control != null && !control.IsHandleCreated)
				{
					Control parentInternal = control.ParentInternal;
					control = parentInternal;
				}
				if (control == null)
				{
					control = this;
				}
				result = control;
			}
			return result;
		}

		/// <summary>Determines if the control is a top-level control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is a top-level control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002B778 File Offset: 0x00029978
		protected bool GetTopLevel()
		{
			return (this.state & 524288) != 0;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002B78C File Offset: 0x0002998C
		internal void RaiseCreateHandleEvent(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleCreated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the appropriate key event.</summary>
		/// <param name="key">The event to raise. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002B7BC File Offset: 0x000299BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseKeyEvent(object key, KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[key];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		/// <summary>Raises the appropriate mouse event.</summary>
		/// <param name="key">The event to raise. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000DAA RID: 3498 RVA: 0x0002B7E8 File Offset: 0x000299E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseMouseEvent(object key, MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[key];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Sets input focus to the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the input focus request was successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DAB RID: 3499 RVA: 0x0002B812 File Offset: 0x00029A12
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool Focus()
		{
			IntSecurity.ModifyFocus.Demand();
			return this.FocusInternal();
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002B824 File Offset: 0x00029A24
		internal virtual bool FocusInternal()
		{
			if (this.CanFocus)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(this, this.Handle));
			}
			if (this.Focused && this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					if (containerControlInternal is ContainerControl)
					{
						((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
					}
					else
					{
						containerControlInternal.ActiveControl = this;
					}
				}
			}
			return this.Focused;
		}

		/// <summary>Retrieves the control that contains the specified handle.</summary>
		/// <param name="handle">The window handle (<see langword="HWND" />) to search for. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that represents the control associated with the specified handle; returns <see langword="null" /> if no control with the specified handle is found.</returns>
		// Token: 0x06000DAD RID: 3501 RVA: 0x0002B88D File Offset: 0x00029A8D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Control FromChildHandle(IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return Control.FromChildHandleInternal(handle);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002B8A0 File Offset: 0x00029AA0
		internal static Control FromChildHandleInternal(IntPtr handle)
		{
			while (handle != IntPtr.Zero)
			{
				Control control = Control.FromHandleInternal(handle);
				if (control != null)
				{
					return control;
				}
				handle = UnsafeNativeMethods.GetAncestor(new HandleRef(null, handle), 1);
			}
			return null;
		}

		/// <summary>Returns the control that is currently associated with the specified handle.</summary>
		/// <param name="handle">The window handle (<see langword="HWND" />) to search for. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> that represents the control associated with the specified handle; returns <see langword="null" /> if no control with the specified handle is found.</returns>
		// Token: 0x06000DAF RID: 3503 RVA: 0x0002B8D8 File Offset: 0x00029AD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Control FromHandle(IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return Control.FromHandleInternal(handle);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002B8EC File Offset: 0x00029AEC
		internal static Control FromHandleInternal(IntPtr handle)
		{
			NativeWindow nativeWindow = NativeWindow.FromHandle(handle);
			while (nativeWindow != null && !(nativeWindow is Control.ControlNativeWindow))
			{
				nativeWindow = nativeWindow.PreviousWindow;
			}
			if (nativeWindow is Control.ControlNativeWindow)
			{
				return ((Control.ControlNativeWindow)nativeWindow).GetControl();
			}
			return null;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002B92C File Offset: 0x00029B2C
		internal Size ApplySizeConstraints(int width, int height)
		{
			return this.ApplyBoundsConstraints(0, 0, width, height).Size;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002B94C File Offset: 0x00029B4C
		internal Size ApplySizeConstraints(Size proposedSize)
		{
			return this.ApplyBoundsConstraints(0, 0, proposedSize.Width, proposedSize.Height).Size;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002B978 File Offset: 0x00029B78
		internal virtual Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			if (this.MaximumSize != Size.Empty || this.MinimumSize != Size.Empty)
			{
				Size b = LayoutUtils.ConvertZeroToUnbounded(this.MaximumSize);
				Rectangle result = new Rectangle(suggestedX, suggestedY, 0, 0);
				result.Size = LayoutUtils.IntersectSizes(new Size(proposedWidth, proposedHeight), b);
				result.Size = LayoutUtils.UnionSizes(result.Size, this.MinimumSize);
				return result;
			}
			return new Rectangle(suggestedX, suggestedY, proposedWidth, proposedHeight);
		}

		/// <summary>Retrieves the child control that is located at the specified coordinates, specifying whether to ignore child controls of a certain type.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that contains the coordinates where you want to look for a control. Coordinates are expressed relative to the upper-left corner of the control's client area.</param>
		/// <param name="skipValue">One of the values of <see cref="T:System.Windows.Forms.GetChildAtPointSkip" />, determining whether to ignore child controls of a certain type.</param>
		/// <returns>The child <see cref="T:System.Windows.Forms.Control" /> at the specified coordinates.</returns>
		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002B9FC File Offset: 0x00029BFC
		public Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
		{
			if (skipValue < GetChildAtPointSkip.None || skipValue > (GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Disabled | GetChildAtPointSkip.Transparent))
			{
				throw new InvalidEnumArgumentException("skipValue", (int)skipValue, typeof(GetChildAtPointSkip));
			}
			IntPtr handle = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, this.Handle), pt.X, pt.Y, (int)skipValue);
			Control control = Control.FromChildHandleInternal(handle);
			if (control != null && !this.IsDescendant(control))
			{
				IntSecurity.ControlFromHandleOrLocation.Demand();
			}
			if (control != this)
			{
				return control;
			}
			return null;
		}

		/// <summary>Retrieves the child control that is located at the specified coordinates.</summary>
		/// <param name="pt">A <see cref="T:System.Drawing.Point" /> that contains the coordinates where you want to look for a control. Coordinates are expressed relative to the upper-left corner of the control's client area. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> that represents the control that is located at the specified point.</returns>
		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002BA6E File Offset: 0x00029C6E
		public Control GetChildAtPoint(Point pt)
		{
			return this.GetChildAtPoint(pt, GetChildAtPointSkip.None);
		}

		/// <summary>Returns the next <see cref="T:System.Windows.Forms.ContainerControl" /> up the control's chain of parent controls.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.IContainerControl" />, that represents the parent of the <see cref="T:System.Windows.Forms.Control" />.</returns>
		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002BA78 File Offset: 0x00029C78
		public IContainerControl GetContainerControl()
		{
			IntSecurity.GetParent.Demand();
			return this.GetContainerControlInternal();
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002BA8A File Offset: 0x00029C8A
		private static bool IsFocusManagingContainerControl(Control ctl)
		{
			return (ctl.controlStyle & ControlStyles.ContainerControl) == ControlStyles.ContainerControl && ctl is IContainerControl;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002BAA2 File Offset: 0x00029CA2
		internal bool IsUpdating()
		{
			return this.updateCount > 0;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002BAB0 File Offset: 0x00029CB0
		internal IContainerControl GetContainerControlInternal()
		{
			Control control = this;
			if (control != null && this.IsContainerControl)
			{
				control = control.ParentInternal;
			}
			while (control != null && !Control.IsFocusManagingContainerControl(control))
			{
				control = control.ParentInternal;
			}
			return (IContainerControl)control;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0002BAEB File Offset: 0x00029CEB
		private static Control.FontHandleWrapper GetDefaultFontHandleWrapper()
		{
			if (Control.defaultFontHandleWrapper == null)
			{
				Control.defaultFontHandleWrapper = new Control.FontHandleWrapper(Control.DefaultFont);
			}
			return Control.defaultFontHandleWrapper;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002BB08 File Offset: 0x00029D08
		internal IntPtr GetHRgn(Region region)
		{
			Graphics graphics = this.CreateGraphicsInternal();
			IntPtr hrgn = region.GetHrgn(graphics);
			System.Internal.HandleCollector.Add(hrgn, NativeMethods.CommonHandles.GDI);
			graphics.Dispose();
			return hrgn;
		}

		/// <summary>Retrieves the bounds within which the control is scaled.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the display bounds.</param>
		/// <param name="factor">The height and width of the control's bounds.</param>
		/// <param name="specified">One of the values of <see cref="T:System.Windows.Forms.BoundsSpecified" /> that specifies the bounds of the control to use when defining its size and position.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the bounds within which the control is scaled.</returns>
		// Token: 0x06000DBC RID: 3516 RVA: 0x0002BB38 File Offset: 0x00029D38
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
			CreateParams createParams = this.CreateParams;
			this.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			float num = factor.Width;
			float num2 = factor.Height;
			int num3 = bounds.X;
			int num4 = bounds.Y;
			bool flag = !this.GetState(524288);
			if (flag)
			{
				ISite site = this.Site;
				if (site != null && site.DesignMode)
				{
					IDesignerHost designerHost = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null && designerHost.RootComponent == this)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
				{
					num3 = (int)Math.Round((double)((float)bounds.X * num));
				}
				if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
				{
					num4 = (int)Math.Round((double)((float)bounds.Y * num2));
				}
			}
			int num5 = bounds.Width;
			int num6 = bounds.Height;
			if ((this.controlStyle & ControlStyles.FixedWidth) != ControlStyles.FixedWidth && (specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				int num7 = rect.right - rect.left;
				int num8 = bounds.Width - num7;
				num5 = (int)Math.Round((double)((float)num8 * num)) + num7;
			}
			if ((this.controlStyle & ControlStyles.FixedHeight) != ControlStyles.FixedHeight && (specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				int num9 = rect.bottom - rect.top;
				int num10 = bounds.Height - num9;
				num6 = (int)Math.Round((double)((float)num10 * num2)) + num9;
			}
			return new Rectangle(num3, num4, num5, num6);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002BCB4 File Offset: 0x00029EB4
		private MouseButtons GetXButton(int wparam)
		{
			if (wparam == 1)
			{
				return MouseButtons.XButton1;
			}
			if (wparam != 2)
			{
				return MouseButtons.None;
			}
			return MouseButtons.XButton2;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002BCCD File Offset: 0x00029ECD
		internal virtual bool GetVisibleCore()
		{
			return this.GetState(2) && (this.ParentInternal == null || this.ParentInternal.GetVisibleCore());
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002BCF0 File Offset: 0x00029EF0
		internal bool GetAnyDisposingInHierarchy()
		{
			Control control = this;
			bool result = false;
			while (control != null)
			{
				if (control.Disposing)
				{
					result = true;
					break;
				}
				control = control.parent;
			}
			return result;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002BD1C File Offset: 0x00029F1C
		private MenuItem GetMenuItemFromHandleId(IntPtr hmenu, int item)
		{
			MenuItem result = null;
			int menuItemID = UnsafeNativeMethods.GetMenuItemID(new HandleRef(null, hmenu), item);
			if (menuItemID == -1)
			{
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.GetSubMenu(new HandleRef(null, hmenu), item);
				int menuItemCount = UnsafeNativeMethods.GetMenuItemCount(new HandleRef(null, intPtr));
				MenuItem menuItem = null;
				for (int i = 0; i < menuItemCount; i++)
				{
					menuItem = this.GetMenuItemFromHandleId(intPtr, i);
					if (menuItem != null)
					{
						Menu menu = menuItem.Parent;
						if (menu != null && menu is MenuItem)
						{
							menuItem = (MenuItem)menu;
							break;
						}
						menuItem = null;
					}
				}
				result = menuItem;
			}
			else
			{
				Command commandFromID = Command.GetCommandFromID(menuItemID);
				if (commandFromID != null)
				{
					object target = commandFromID.Target;
					if (target != null && target is MenuItem.MenuItemData)
					{
						result = ((MenuItem.MenuItemData)target).baseItem;
					}
				}
			}
			return result;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002BDDC File Offset: 0x00029FDC
		private ArrayList GetChildControlsTabOrderList(bool handleCreatedOnly)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				if (!handleCreatedOnly || control.IsHandleCreated)
				{
					arrayList.Add(new Control.ControlTabOrderHolder(arrayList.Count, control.TabIndex, control));
				}
			}
			arrayList.Sort(new Control.ControlTabOrderComparer());
			return arrayList;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002BE64 File Offset: 0x0002A064
		private int[] GetChildWindowsInTabOrder()
		{
			ArrayList childWindowsTabOrderList = this.GetChildWindowsTabOrderList();
			int[] array = new int[childWindowsTabOrderList.Count];
			for (int i = 0; i < childWindowsTabOrderList.Count; i++)
			{
				array[i] = ((Control.ControlTabOrderHolder)childWindowsTabOrderList[i]).oldOrder;
			}
			return array;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002BEAC File Offset: 0x0002A0AC
		internal Control[] GetChildControlsInTabOrder(bool handleCreatedOnly)
		{
			ArrayList childControlsTabOrderList = this.GetChildControlsTabOrderList(handleCreatedOnly);
			Control[] array = new Control[childControlsTabOrderList.Count];
			for (int i = 0; i < childControlsTabOrderList.Count; i++)
			{
				array[i] = ((Control.ControlTabOrderHolder)childControlsTabOrderList[i]).control;
			}
			return array;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
		private static ArrayList GetChildWindows(IntPtr hWndParent)
		{
			ArrayList arrayList = new ArrayList();
			IntPtr intPtr = UnsafeNativeMethods.GetWindow(new HandleRef(null, hWndParent), 5);
			while (intPtr != IntPtr.Zero)
			{
				arrayList.Add(intPtr);
				intPtr = UnsafeNativeMethods.GetWindow(new HandleRef(null, intPtr), 2);
			}
			return arrayList;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002BF40 File Offset: 0x0002A140
		private ArrayList GetChildWindowsTabOrderList()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList childWindows = Control.GetChildWindows(this.Handle);
			foreach (object obj in childWindows)
			{
				IntPtr handle = (IntPtr)obj;
				Control control = Control.FromHandleInternal(handle);
				int newOrder = (control == null) ? -1 : control.TabIndex;
				arrayList.Add(new Control.ControlTabOrderHolder(arrayList.Count, newOrder, control));
			}
			arrayList.Sort(new Control.ControlTabOrderComparer());
			return arrayList;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002BFDC File Offset: 0x0002A1DC
		internal virtual Control GetFirstChildControlInTabOrder(bool forward)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			Control control = null;
			if (controlCollection != null)
			{
				if (forward)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						if (control == null || control.tabIndex > controlCollection[i].tabIndex)
						{
							control = controlCollection[i];
						}
					}
				}
				else
				{
					for (int j = controlCollection.Count - 1; j >= 0; j--)
					{
						if (control == null || control.tabIndex < controlCollection[j].tabIndex)
						{
							control = controlCollection[j];
						}
					}
				}
			}
			return control;
		}

		/// <summary>Retrieves the next control forward or back in the tab order of child controls.</summary>
		/// <param name="ctl">The <see cref="T:System.Windows.Forms.Control" /> to start the search with. </param>
		/// <param name="forward">
		///       <see langword="true" /> to search forward in the tab order; <see langword="false" /> to search backward. </param>
		/// <returns>The next <see cref="T:System.Windows.Forms.Control" /> in the tab order.</returns>
		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002C06C File Offset: 0x0002A26C
		public Control GetNextControl(Control ctl, bool forward)
		{
			if (!this.Contains(ctl))
			{
				ctl = this;
			}
			if (forward)
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null && controlCollection.Count > 0 && (ctl == this || !Control.IsFocusManagingContainerControl(ctl)))
				{
					Control firstChildControlInTabOrder = ctl.GetFirstChildControlInTabOrder(true);
					if (firstChildControlInTabOrder != null)
					{
						return firstChildControlInTabOrder;
					}
				}
				while (ctl != this)
				{
					int num = ctl.tabIndex;
					bool flag = false;
					Control control = null;
					Control control2 = ctl.parent;
					int num2 = 0;
					Control.ControlCollection controlCollection2 = (Control.ControlCollection)control2.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection2 != null)
					{
						num2 = controlCollection2.Count;
					}
					for (int i = 0; i < num2; i++)
					{
						if (controlCollection2[i] != ctl)
						{
							if (controlCollection2[i].tabIndex >= num && (control == null || control.tabIndex > controlCollection2[i].tabIndex) && (controlCollection2[i].tabIndex != num || flag))
							{
								control = controlCollection2[i];
							}
						}
						else
						{
							flag = true;
						}
					}
					if (control != null)
					{
						return control;
					}
					ctl = ctl.parent;
				}
			}
			else
			{
				if (ctl != this)
				{
					int num3 = ctl.tabIndex;
					bool flag2 = false;
					Control control3 = null;
					Control control4 = ctl.parent;
					int num4 = 0;
					Control.ControlCollection controlCollection3 = (Control.ControlCollection)control4.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection3 != null)
					{
						num4 = controlCollection3.Count;
					}
					for (int j = num4 - 1; j >= 0; j--)
					{
						if (controlCollection3[j] != ctl)
						{
							if (controlCollection3[j].tabIndex <= num3 && (control3 == null || control3.tabIndex < controlCollection3[j].tabIndex) && (controlCollection3[j].tabIndex != num3 || flag2))
							{
								control3 = controlCollection3[j];
							}
						}
						else
						{
							flag2 = true;
						}
					}
					if (control3 != null)
					{
						ctl = control3;
					}
					else
					{
						if (control4 == this)
						{
							return null;
						}
						return control4;
					}
				}
				Control.ControlCollection controlCollection4 = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				while (controlCollection4 != null && controlCollection4.Count > 0 && (ctl == this || !Control.IsFocusManagingContainerControl(ctl)))
				{
					Control firstChildControlInTabOrder2 = ctl.GetFirstChildControlInTabOrder(false);
					if (firstChildControlInTabOrder2 == null)
					{
						break;
					}
					ctl = firstChildControlInTabOrder2;
					controlCollection4 = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				}
			}
			if (ctl != this)
			{
				return ctl;
			}
			return null;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002C2CC File Offset: 0x0002A4CC
		internal static IntPtr GetSafeHandle(IWin32Window window)
		{
			IntPtr intPtr = IntPtr.Zero;
			Control control = window as Control;
			if (control != null)
			{
				return control.Handle;
			}
			IntSecurity.AllWindows.Demand();
			intPtr = window.Handle;
			if (intPtr == IntPtr.Zero || UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
			{
				return intPtr;
			}
			throw new Win32Exception(6);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002C326 File Offset: 0x0002A526
		internal bool GetState(int flag)
		{
			return (this.state & flag) != 0;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002C333 File Offset: 0x0002A533
		private bool GetState2(int flag)
		{
			return (this.state2 & flag) != 0;
		}

		/// <summary>Retrieves the value of the specified control style bit for the control.</summary>
		/// <param name="flag">The <see cref="T:System.Windows.Forms.ControlStyles" /> bit to return the value from. </param>
		/// <returns>
		///     <see langword="true" /> if the specified control style bit is set to <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DCB RID: 3531 RVA: 0x0002C340 File Offset: 0x0002A540
		protected bool GetStyle(ControlStyles flag)
		{
			return (this.controlStyle & flag) == flag;
		}

		/// <summary>Conceals the control from the user.</summary>
		// Token: 0x06000DCC RID: 3532 RVA: 0x0002C34D File Offset: 0x0002A54D
		public void Hide()
		{
			this.Visible = false;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002C358 File Offset: 0x0002A558
		private void HookMouseEvent()
		{
			if (!this.GetState(16384))
			{
				this.SetState(16384, true);
				if (this.trackMouseEvent == null)
				{
					this.trackMouseEvent = new NativeMethods.TRACKMOUSEEVENT();
					this.trackMouseEvent.dwFlags = 3;
					this.trackMouseEvent.hwndTrack = this.Handle;
				}
				SafeNativeMethods.TrackMouseEvent(this.trackMouseEvent);
			}
		}

		/// <summary>Called after the control has been added to another container.</summary>
		// Token: 0x06000DCE RID: 3534 RVA: 0x0002C3BA File Offset: 0x0002A5BA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void InitLayout()
		{
			this.LayoutEngine.InitLayout(this, BoundsSpecified.All);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002C3CA File Offset: 0x0002A5CA
		private void InitScaling(BoundsSpecified specified)
		{
			this.requiredScaling |= (byte)(specified & BoundsSpecified.All);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002C3E0 File Offset: 0x0002A5E0
		internal virtual IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (!this.GetStyle(ControlStyles.UserPaint))
			{
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.ForeColor));
				SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.BackColor));
				return this.BackColorBrush;
			}
			return UnsafeNativeMethods.GetStockObject(5);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002C434 File Offset: 0x0002A634
		private void InitMouseWheelSupport()
		{
			if (!Control.mouseWheelInit)
			{
				Control.mouseWheelRoutingNeeded = !SystemInformation.NativeMouseWheelSupport;
				if (Control.mouseWheelRoutingNeeded)
				{
					IntPtr value = IntPtr.Zero;
					value = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
					if (value != IntPtr.Zero)
					{
						int num = SafeNativeMethods.RegisterWindowMessage("MSWHEEL_ROLLMSG");
						if (num != 0)
						{
							Control.mouseWheelMessage = num;
						}
					}
				}
				Control.mouseWheelInit = true;
			}
		}

		/// <summary>Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted at the next paint operation), and causes a paint message to be sent to the control.</summary>
		/// <param name="region">The <see cref="T:System.Drawing.Region" /> to invalidate. </param>
		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002C499 File Offset: 0x0002A699
		public void Invalidate(Region region)
		{
			this.Invalidate(region, false);
		}

		/// <summary>Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted at the next paint operation), and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.</summary>
		/// <param name="region">The <see cref="T:System.Drawing.Region" /> to invalidate. </param>
		/// <param name="invalidateChildren">
		///       <see langword="true" /> to invalidate the control's child controls; otherwise, <see langword="false" />. </param>
		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002C4A4 File Offset: 0x0002A6A4
		public void Invalidate(Region region, bool invalidateChildren)
		{
			if (region == null)
			{
				this.Invalidate(invalidateChildren);
				return;
			}
			if (this.IsHandleCreated)
			{
				IntPtr hrgn = this.GetHRgn(region);
				try
				{
					if (invalidateChildren)
					{
						SafeNativeMethods.RedrawWindow(new HandleRef(this, this.Handle), null, new HandleRef(region, hrgn), 133);
					}
					else
					{
						using (new Control.MultithreadSafeCallScope())
						{
							SafeNativeMethods.InvalidateRgn(new HandleRef(this, this.Handle), new HandleRef(region, hrgn), !this.GetStyle(ControlStyles.Opaque));
						}
					}
				}
				finally
				{
					SafeNativeMethods.DeleteObject(new HandleRef(region, hrgn));
				}
				Rectangle invalidRect = Rectangle.Empty;
				using (Graphics graphics = this.CreateGraphicsInternal())
				{
					invalidRect = Rectangle.Ceiling(region.GetBounds(graphics));
				}
				this.OnInvalidated(new InvalidateEventArgs(invalidRect));
			}
		}

		/// <summary>Invalidates the entire surface of the control and causes the control to be redrawn.</summary>
		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002C590 File Offset: 0x0002A790
		public void Invalidate()
		{
			this.Invalidate(false);
		}

		/// <summary>Invalidates a specific region of the control and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.</summary>
		/// <param name="invalidateChildren">
		///       <see langword="true" /> to invalidate the control's child controls; otherwise, <see langword="false" />. </param>
		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002C59C File Offset: 0x0002A79C
		public void Invalidate(bool invalidateChildren)
		{
			if (this.IsHandleCreated)
			{
				if (invalidateChildren)
				{
					SafeNativeMethods.RedrawWindow(new HandleRef(this.window, this.Handle), null, NativeMethods.NullHandleRef, 133);
				}
				else
				{
					using (new Control.MultithreadSafeCallScope())
					{
						SafeNativeMethods.InvalidateRect(new HandleRef(this.window, this.Handle), null, (this.controlStyle & ControlStyles.Opaque) != ControlStyles.Opaque);
					}
				}
				this.NotifyInvalidate(this.ClientRectangle);
			}
		}

		/// <summary>Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted at the next paint operation), and causes a paint message to be sent to the control.</summary>
		/// <param name="rc">A <see cref="T:System.Drawing.Rectangle" /> that represents the region to invalidate. </param>
		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002C62C File Offset: 0x0002A82C
		public void Invalidate(Rectangle rc)
		{
			this.Invalidate(rc, false);
		}

		/// <summary>Invalidates the specified region of the control (adds it to the control's update region, which is the area that will be repainted at the next paint operation), and causes a paint message to be sent to the control. Optionally, invalidates the child controls assigned to the control.</summary>
		/// <param name="rc">A <see cref="T:System.Drawing.Rectangle" /> that represents the region to invalidate. </param>
		/// <param name="invalidateChildren">
		///       <see langword="true" /> to invalidate the control's child controls; otherwise, <see langword="false" />. </param>
		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002C638 File Offset: 0x0002A838
		public void Invalidate(Rectangle rc, bool invalidateChildren)
		{
			if (rc.IsEmpty)
			{
				this.Invalidate(invalidateChildren);
				return;
			}
			if (this.IsHandleCreated)
			{
				if (invalidateChildren)
				{
					NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(rc.X, rc.Y, rc.Width, rc.Height);
					SafeNativeMethods.RedrawWindow(new HandleRef(this.window, this.Handle), ref rect, NativeMethods.NullHandleRef, 133);
				}
				else
				{
					NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(rc.X, rc.Y, rc.Width, rc.Height);
					using (new Control.MultithreadSafeCallScope())
					{
						SafeNativeMethods.InvalidateRect(new HandleRef(this.window, this.Handle), ref rect2, (this.controlStyle & ControlStyles.Opaque) != ControlStyles.Opaque);
					}
				}
				this.NotifyInvalidate(rc);
			}
		}

		/// <summary>Executes the specified delegate on the thread that owns the control's underlying window handle.</summary>
		/// <param name="method">A delegate that contains a method to be called in the control's thread context. </param>
		/// <returns>The return value from the delegate being invoked, or <see langword="null" /> if the delegate has no return value.</returns>
		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002C720 File Offset: 0x0002A920
		public object Invoke(Delegate method)
		{
			return this.Invoke(method, null);
		}

		/// <summary>Executes the specified delegate, on the thread that owns the control's underlying window handle, with the specified list of arguments.</summary>
		/// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the <paramref name="args" /> parameter. </param>
		/// <param name="args">An array of objects to pass as arguments to the specified method. This parameter can be <see langword="null" /> if the method takes no arguments. </param>
		/// <returns>An <see cref="T:System.Object" /> that contains the return value from the delegate being invoked, or <see langword="null" /> if the delegate has no return value.</returns>
		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002C72C File Offset: 0x0002A92C
		public object Invoke(Delegate method, params object[] args)
		{
			object result;
			using (new Control.MultithreadSafeCallScope())
			{
				Control control = this.FindMarshalingControl();
				result = control.MarshaledInvoke(this, method, args, true);
			}
			return result;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002C770 File Offset: 0x0002A970
		private void InvokeMarshaledCallback(Control.ThreadMethodEntry tme)
		{
			if (tme.executionContext != null)
			{
				if (Control.invokeMarshaledCallbackHelperDelegate == null)
				{
					Control.invokeMarshaledCallbackHelperDelegate = new ContextCallback(Control.InvokeMarshaledCallbackHelper);
				}
				if (SynchronizationContext.Current == null)
				{
					WindowsFormsSynchronizationContext.InstallIfNeeded();
				}
				tme.syncContext = SynchronizationContext.Current;
				ExecutionContext.Run(tme.executionContext, Control.invokeMarshaledCallbackHelperDelegate, tme);
				return;
			}
			Control.InvokeMarshaledCallbackHelper(tme);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002C7CC File Offset: 0x0002A9CC
		private static void InvokeMarshaledCallbackHelper(object obj)
		{
			Control.ThreadMethodEntry threadMethodEntry = (Control.ThreadMethodEntry)obj;
			if (threadMethodEntry.syncContext != null)
			{
				SynchronizationContext synchronizationContext = SynchronizationContext.Current;
				try
				{
					SynchronizationContext.SetSynchronizationContext(threadMethodEntry.syncContext);
					Control.InvokeMarshaledCallbackDo(threadMethodEntry);
					return;
				}
				finally
				{
					SynchronizationContext.SetSynchronizationContext(synchronizationContext);
				}
			}
			Control.InvokeMarshaledCallbackDo(threadMethodEntry);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002C820 File Offset: 0x0002AA20
		private static void InvokeMarshaledCallbackDo(Control.ThreadMethodEntry tme)
		{
			if (tme.method is EventHandler)
			{
				if (tme.args == null || tme.args.Length < 1)
				{
					((EventHandler)tme.method)(tme.caller, EventArgs.Empty);
					return;
				}
				if (tme.args.Length < 2)
				{
					((EventHandler)tme.method)(tme.args[0], EventArgs.Empty);
					return;
				}
				((EventHandler)tme.method)(tme.args[0], (EventArgs)tme.args[1]);
				return;
			}
			else
			{
				if (tme.method is MethodInvoker)
				{
					((MethodInvoker)tme.method)();
					return;
				}
				if (tme.method is WaitCallback)
				{
					((WaitCallback)tme.method)(tme.args[0]);
					return;
				}
				tme.retVal = tme.method.DynamicInvoke(tme.args);
				return;
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002C914 File Offset: 0x0002AB14
		private void InvokeMarshaledCallbacks()
		{
			Control.ThreadMethodEntry threadMethodEntry = null;
			Queue obj = this.threadCallbackList;
			lock (obj)
			{
				if (this.threadCallbackList.Count > 0)
				{
					threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
				}
				goto IL_E8;
			}
			IL_41:
			if (threadMethodEntry.method != null)
			{
				try
				{
					if (NativeWindow.WndProcShouldBeDebuggable && !threadMethodEntry.synchronous)
					{
						this.InvokeMarshaledCallback(threadMethodEntry);
					}
					else
					{
						try
						{
							this.InvokeMarshaledCallback(threadMethodEntry);
						}
						catch (Exception ex)
						{
							threadMethodEntry.exception = ex.GetBaseException();
						}
					}
				}
				finally
				{
					threadMethodEntry.Complete();
					if (!NativeWindow.WndProcShouldBeDebuggable && threadMethodEntry.exception != null && !threadMethodEntry.synchronous)
					{
						Application.OnThreadException(threadMethodEntry.exception);
					}
				}
			}
			Queue obj2 = this.threadCallbackList;
			lock (obj2)
			{
				if (this.threadCallbackList.Count > 0)
				{
					threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
				}
				else
				{
					threadMethodEntry = null;
				}
			}
			IL_E8:
			if (threadMethodEntry == null)
			{
				return;
			}
			goto IL_41;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event for the specified control.</summary>
		/// <param name="c">The <see cref="T:System.Windows.Forms.Control" /> to assign the <see cref="E:System.Windows.Forms.Control.Paint" /> event to. </param>
		/// <param name="e">An <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000DDE RID: 3550 RVA: 0x0002CA44 File Offset: 0x0002AC44
		protected void InvokePaint(Control c, PaintEventArgs e)
		{
			c.OnPaint(e);
		}

		/// <summary>Raises the <see langword="PaintBackground" /> event for the specified control.</summary>
		/// <param name="c">The <see cref="T:System.Windows.Forms.Control" /> to assign the <see cref="E:System.Windows.Forms.Control.Paint" /> event to. </param>
		/// <param name="e">An <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000DDF RID: 3551 RVA: 0x0002CA4D File Offset: 0x0002AC4D
		protected void InvokePaintBackground(Control c, PaintEventArgs e)
		{
			c.OnPaintBackground(e);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002CA58 File Offset: 0x0002AC58
		internal bool IsFontSet()
		{
			return (Font)this.Properties.GetObject(Control.PropFont) != null;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002CA84 File Offset: 0x0002AC84
		internal bool IsDescendant(Control descendant)
		{
			for (Control control = descendant; control != null; control = control.ParentInternal)
			{
				if (control == this)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether the CAPS LOCK, NUM LOCK, or SCROLL LOCK key is in effect.</summary>
		/// <param name="keyVal">The CAPS LOCK, NUM LOCK, or SCROLL LOCK member of the <see cref="T:System.Windows.Forms.Keys" /> enumeration. </param>
		/// <returns>
		///     <see langword="true" /> if the specified key or keys are in effect; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="keyVal" /> parameter refers to a key other than the CAPS LOCK, NUM LOCK, or SCROLL LOCK key. </exception>
		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
		public static bool IsKeyLocked(Keys keyVal)
		{
			if (keyVal != Keys.Insert && keyVal != Keys.NumLock && keyVal != Keys.Capital && keyVal != Keys.Scroll)
			{
				throw new NotSupportedException(SR.GetString("ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly"));
			}
			int keyState = (int)UnsafeNativeMethods.GetKeyState((int)keyVal);
			if (keyVal == Keys.Insert || keyVal == Keys.Capital)
			{
				return (keyState & 1) != 0;
			}
			return (keyState & 32769) != 0;
		}

		/// <summary>Determines if a character is an input character that the control recognizes.</summary>
		/// <param name="charCode">The character to test. </param>
		/// <returns>
		///     <see langword="true" /> if the character should be sent directly to the control and not preprocessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002CB04 File Offset: 0x0002AD04
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool IsInputChar(char charCode)
		{
			int num;
			if (charCode == '\t')
			{
				num = 134;
			}
			else
			{
				num = 132;
			}
			return ((int)((long)this.SendMessage(135, 0, 0)) & num) != 0;
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002CB40 File Offset: 0x0002AD40
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			int num = 4;
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Tab)
			{
				if (keys - Keys.Left <= 3)
				{
					num = 5;
				}
			}
			else
			{
				num = 6;
			}
			return this.IsHandleCreated && ((int)((long)this.SendMessage(135, 0, 0)) & num) != 0;
		}

		/// <summary>Determines if the specified character is the mnemonic character assigned to the control in the specified string.</summary>
		/// <param name="charCode">The character to test. </param>
		/// <param name="text">The string to search. </param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="charCode" /> character is the mnemonic character assigned to the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		public static bool IsMnemonic(char charCode, string text)
		{
			if (charCode == '&')
			{
				return false;
			}
			if (text != null)
			{
				int num = -1;
				char c = char.ToUpper(charCode, CultureInfo.CurrentCulture);
				while (num + 1 < text.Length)
				{
					num = text.IndexOf('&', num + 1) + 1;
					if (num <= 0 || num >= text.Length)
					{
						break;
					}
					char c2 = char.ToUpper(text[num], CultureInfo.CurrentCulture);
					if (c2 == c || char.ToLower(c2, CultureInfo.CurrentCulture) == char.ToLower(c, CultureInfo.CurrentCulture))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002CC1C File Offset: 0x0002AE1C
		private void ListenToUserPreferenceChanged(bool listen)
		{
			if (this.GetState2(4))
			{
				if (!listen)
				{
					this.SetState2(4, false);
					SystemEvents.UserPreferenceChanged -= this.UserPreferenceChanged;
					return;
				}
			}
			else if (listen)
			{
				this.SetState2(4, true);
				SystemEvents.UserPreferenceChanged += this.UserPreferenceChanged;
			}
		}

		/// <summary>Converts a Logical DPI value to it's equivalent DeviceUnit DPI value.</summary>
		/// <param name="value">The Logical value to convert.</param>
		/// <returns>The resulting DeviceUnit value.</returns>
		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002CC6B File Offset: 0x0002AE6B
		public int LogicalToDeviceUnits(int value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, this.DeviceDpi);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002CC79 File Offset: 0x0002AE79
		public Size LogicalToDeviceUnits(Size value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, this.DeviceDpi);
		}

		/// <summary>Scales a logical bitmap value to it's equivalent device unit value when a DPI change occurs.</summary>
		/// <param name="logicalBitmap">The bitmap to scale.</param>
		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002CC87 File Offset: 0x0002AE87
		public void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
		{
			DpiHelper.ScaleBitmapLogicalToDevice(ref logicalBitmap, this.DeviceDpi);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002CC95 File Offset: 0x0002AE95
		internal void AdjustWindowRectEx(ref NativeMethods.RECT rect, int style, bool bMenu, int exStyle)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				SafeNativeMethods.AdjustWindowRectExForDpi(ref rect, style, bMenu, exStyle, (uint)this.deviceDpi);
				return;
			}
			SafeNativeMethods.AdjustWindowRectEx(ref rect, style, bMenu, exStyle);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0002CCBC File Offset: 0x0002AEBC
		private object MarshaledInvoke(Control caller, Delegate method, object[] args, bool synchronous)
		{
			if (!this.IsHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("ErrorNoMarshalingThread"));
			}
			Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)this.Properties.GetObject(Control.PropActiveXImpl);
			if (activeXImpl != null)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			bool flag = false;
			int num;
			if (SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, this.Handle), out num) == SafeNativeMethods.GetCurrentThreadId() && synchronous)
			{
				flag = true;
			}
			ExecutionContext executionContext = null;
			if (!flag)
			{
				executionContext = ExecutionContext.Capture();
			}
			Control.ThreadMethodEntry threadMethodEntry = new Control.ThreadMethodEntry(caller, this, method, args, synchronous, executionContext);
			lock (this)
			{
				if (this.threadCallbackList == null)
				{
					this.threadCallbackList = new Queue();
				}
			}
			Queue obj = this.threadCallbackList;
			lock (obj)
			{
				if (Control.threadCallbackMessage == 0)
				{
					Control.threadCallbackMessage = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_ThreadCallbackMessage");
				}
				this.threadCallbackList.Enqueue(threadMethodEntry);
			}
			if (flag)
			{
				this.InvokeMarshaledCallbacks();
			}
			else
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, this.Handle), Control.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
			}
			if (!synchronous)
			{
				return threadMethodEntry;
			}
			if (!threadMethodEntry.IsCompleted)
			{
				this.WaitForWaitHandle(threadMethodEntry.AsyncWaitHandle);
			}
			if (threadMethodEntry.exception != null)
			{
				throw threadMethodEntry.exception;
			}
			return threadMethodEntry.retVal;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002CE3C File Offset: 0x0002B03C
		private void MarshalStringToMessage(string value, ref Message m)
		{
			if (m.LParam == IntPtr.Zero)
			{
				m.Result = (IntPtr)((value.Length + 1) * Marshal.SystemDefaultCharSize);
				return;
			}
			if ((int)((long)m.WParam) < value.Length + 1)
			{
				m.Result = (IntPtr)(-1);
				return;
			}
			char[] chars = new char[1];
			byte[] bytes;
			byte[] bytes2;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				bytes = Encoding.Default.GetBytes(value);
				bytes2 = Encoding.Default.GetBytes(chars);
			}
			else
			{
				bytes = Encoding.Unicode.GetBytes(value);
				bytes2 = Encoding.Unicode.GetBytes(chars);
			}
			Marshal.Copy(bytes, 0, m.LParam, bytes.Length);
			Marshal.Copy(bytes2, 0, (IntPtr)((long)m.LParam + (long)bytes.Length), bytes2.Length);
			m.Result = (IntPtr)((bytes.Length + bytes2.Length) / Marshal.SystemDefaultCharSize);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002CF20 File Offset: 0x0002B120
		internal void NotifyEnter()
		{
			this.OnEnter(EventArgs.Empty);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002CF2D File Offset: 0x0002B12D
		internal void NotifyLeave()
		{
			this.OnLeave(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Invalidated" /> event with a specified region of the control to invalidate.</summary>
		/// <param name="invalidatedArea">A <see cref="T:System.Drawing.Rectangle" /> representing the area to invalidate. </param>
		// Token: 0x06000DEF RID: 3567 RVA: 0x0002CF3A File Offset: 0x0002B13A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void NotifyInvalidate(Rectangle invalidatedArea)
		{
			this.OnInvalidated(new InvalidateEventArgs(invalidatedArea));
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002CF48 File Offset: 0x0002B148
		private bool NotifyValidating()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			this.OnValidating(cancelEventArgs);
			return cancelEventArgs.Cancel;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002CF68 File Offset: 0x0002B168
		private void NotifyValidated()
		{
			this.OnValidated(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event for the specified control.</summary>
		/// <param name="toInvoke">The <see cref="T:System.Windows.Forms.Control" /> to assign the <see cref="E:System.Windows.Forms.Control.Click" /> event to. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002CF75 File Offset: 0x0002B175
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeOnClick(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnClick(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.AutoSizeChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002CF84 File Offset: 0x0002B184
		protected virtual void OnAutoSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventAutoSizeChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002CFB4 File Offset: 0x0002B1B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			object @object = this.Properties.GetObject(Control.PropBackBrush);
			if (@object != null)
			{
				if (this.GetState(2097152))
				{
					IntPtr intPtr = (IntPtr)@object;
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
					}
				}
				this.Properties.SetObject(Control.PropBackBrush, null);
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBackColorChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002D080 File Offset: 0x0002B280
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackgroundImageChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackgroundImage] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBackgroundImageChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageLayoutChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002D0F4 File Offset: 0x0002B2F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackgroundImageLayoutChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackgroundImageLayout] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BindingContextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002D134 File Offset: 0x0002B334
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBindingContextChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropBindings) != null)
			{
				this.UpdateBindings();
			}
			EventHandler eventHandler = base.Events[Control.EventBindingContext] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBindingContextChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.CausesValidationChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCausesValidationChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventCausesValidation] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002D1E2 File Offset: 0x0002B3E2
		internal virtual void OnChildLayoutResuming(Control child, bool performLayout)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.OnChildLayoutResuming(child, performLayout);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ContextMenuChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DFA RID: 3578 RVA: 0x0002D1FC File Offset: 0x0002B3FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnContextMenuChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventContextMenu] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ContextMenuStripChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000DFB RID: 3579 RVA: 0x0002D22C File Offset: 0x0002B42C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnContextMenuStripChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventContextMenuStrip] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.CursorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DFC RID: 3580 RVA: 0x0002D25C File Offset: 0x0002B45C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCursorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventCursor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentCursorChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DockChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DFD RID: 3581 RVA: 0x0002D2C4 File Offset: 0x0002B4C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDockChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventDock] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000DFE RID: 3582 RVA: 0x0002D2F4 File Offset: 0x0002B4F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.IsHandleCreated)
			{
				SafeNativeMethods.EnableWindow(new HandleRef(this, this.Handle), this.Enabled);
				if (this.GetStyle(ControlStyles.UserPaint))
				{
					this.Invalidate();
					this.Update();
				}
			}
			EventHandler eventHandler = base.Events[Control.EventEnabled] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentEnabledChanged(e);
				}
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnFrameWindowActivate(bool fActivate)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E00 RID: 3584 RVA: 0x0002D398 File Offset: 0x0002B598
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFontChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			if (this.Properties.ContainsInteger(Control.PropFontHeight))
			{
				this.Properties.SetInteger(Control.PropFontHeight, -1);
			}
			this.DisposeFontHandle();
			if (this.IsHandleCreated && !this.GetStyle(ControlStyles.UserPaint))
			{
				this.SetWindowFont();
			}
			EventHandler eventHandler = base.Events[Control.EventFont] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			using (new LayoutTransaction(this, this, PropertyNames.Font, false))
			{
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						controlCollection[i].OnParentFontChanged(e);
					}
				}
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Font);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E01 RID: 3585 RVA: 0x0002D484 File Offset: 0x0002B684
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentForeColorChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E02 RID: 3586 RVA: 0x0002D4F8 File Offset: 0x0002B6F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.SetState2(2, true);
			this.RecreateHandle();
			EventHandler eventHandler = base.Events[Control.EventRightToLeft] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentRightToLeftChanged(e);
				}
			}
		}

		/// <summary>Notifies the control of Windows messages.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that represents the Windows message. </param>
		// Token: 0x06000E03 RID: 3587 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnNotifyMessage(Message m)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event when the <see cref="P:System.Windows.Forms.Control.BackColor" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E04 RID: 3588 RVA: 0x0002D574 File Offset: 0x0002B774
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(Control.PropBackColor).IsEmpty)
			{
				this.OnBackColorChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageChanged" /> event when the <see cref="P:System.Windows.Forms.Control.BackgroundImage" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E05 RID: 3589 RVA: 0x0002D5A2 File Offset: 0x0002B7A2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackgroundImageChanged(EventArgs e)
		{
			this.OnBackgroundImageChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BindingContextChanged" /> event when the <see cref="P:System.Windows.Forms.Control.BindingContext" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E06 RID: 3590 RVA: 0x0002D5AB File Offset: 0x0002B7AB
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBindingContextChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropBindingManager) == null)
			{
				this.OnBindingContextChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.CursorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000E07 RID: 3591 RVA: 0x0002D5C6 File Offset: 0x0002B7C6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentCursorChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropCursor) == null)
			{
				this.OnCursorChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event when the <see cref="P:System.Windows.Forms.Control.Enabled" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E08 RID: 3592 RVA: 0x0002D5E1 File Offset: 0x0002B7E1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentEnabledChanged(EventArgs e)
		{
			if (this.GetState(4))
			{
				this.OnEnabledChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event when the <see cref="P:System.Windows.Forms.Control.Font" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E09 RID: 3593 RVA: 0x0002D5F3 File Offset: 0x0002B7F3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentFontChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropFont) == null)
			{
				this.OnFontChanged(e);
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0002D610 File Offset: 0x0002B810
		internal virtual void OnParentHandleRecreated()
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null && this.IsHandleCreated)
			{
				UnsafeNativeMethods.SetParent(new HandleRef(this, this.Handle), new HandleRef(parentInternal, parentInternal.Handle));
				this.UpdateZOrder();
			}
			this.SetState(536870912, false);
			if (this.ReflectParent == this.ParentInternal)
			{
				this.RecreateHandle();
			}
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002D673 File Offset: 0x0002B873
		internal virtual void OnParentHandleRecreating()
		{
			this.SetState(536870912, true);
			if (this.IsHandleCreated)
			{
				Application.ParkHandle(new HandleRef(this, this.Handle), this.DpiAwarenessContext);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event when the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E0C RID: 3596 RVA: 0x0002D6A0 File Offset: 0x0002B8A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentForeColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(Control.PropForeColor).IsEmpty)
			{
				this.OnForeColorChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event when the <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E0D RID: 3597 RVA: 0x0002D6CE File Offset: 0x0002B8CE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentRightToLeftChanged(EventArgs e)
		{
			if (!this.Properties.ContainsInteger(Control.PropRightToLeft) || this.Properties.GetInteger(Control.PropRightToLeft) == 2)
			{
				this.OnRightToLeftChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event when the <see cref="P:System.Windows.Forms.Control.Visible" /> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E0E RID: 3598 RVA: 0x0002D6FC File Offset: 0x0002B8FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentVisibleChanged(EventArgs e)
		{
			if (this.GetState(2))
			{
				this.OnVisibleChanged(e);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0002D710 File Offset: 0x0002B910
		internal virtual void OnParentBecameInvisible()
		{
			if (this.GetState(2))
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						Control control = controlCollection[i];
						control.OnParentBecameInvisible();
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000E10 RID: 3600 RVA: 0x0002D760 File Offset: 0x0002B960
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPrint(PaintEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				this.PaintWithErrorHandling(e, 1);
				e.ResetGraphics();
				this.PaintWithErrorHandling(e, 2);
				return;
			}
			Control.PrintPaintEventArgs printPaintEventArgs = e as Control.PrintPaintEventArgs;
			bool flag = false;
			IntPtr intPtr = IntPtr.Zero;
			Message message;
			if (printPaintEventArgs == null)
			{
				IntPtr lparam = (IntPtr)30;
				intPtr = e.HDC;
				if (intPtr == IntPtr.Zero)
				{
					intPtr = e.Graphics.GetHdc();
					flag = true;
				}
				message = Message.Create(this.Handle, 792, intPtr, lparam);
			}
			else
			{
				message = printPaintEventArgs.Message;
			}
			try
			{
				this.DefWndProc(ref message);
			}
			finally
			{
				if (flag)
				{
					e.Graphics.ReleaseHdcInternal(intPtr);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TabIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E11 RID: 3601 RVA: 0x0002D820 File Offset: 0x0002BA20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTabIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventTabIndex] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TabStopChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E12 RID: 3602 RVA: 0x0002D850 File Offset: 0x0002BA50
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTabStopChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventTabStop] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E13 RID: 3603 RVA: 0x0002D880 File Offset: 0x0002BA80
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E14 RID: 3604 RVA: 0x0002D8B0 File Offset: 0x0002BAB0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnVisibleChanged(EventArgs e)
		{
			bool visible = this.Visible;
			if (visible)
			{
				this.UnhookMouseEvent();
				this.trackMouseEvent = null;
			}
			if (this.parent != null && visible && !this.Created && !this.GetAnyDisposingInHierarchy())
			{
				this.CreateControl();
			}
			EventHandler eventHandler = base.Events[Control.EventVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					Control control = controlCollection[i];
					if (control.Visible)
					{
						control.OnParentVisibleChanged(e);
					}
					if (!visible)
					{
						control.OnParentBecameInvisible();
					}
				}
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002D970 File Offset: 0x0002BB70
		internal virtual void OnTopMostActiveXParentChanged(EventArgs e)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnTopMostActiveXParentChanged(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E16 RID: 3606 RVA: 0x0002D9B4 File Offset: 0x0002BBB4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventParent] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.TopMostParent.IsActiveX)
			{
				this.OnTopMostActiveXParentChanged(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E17 RID: 3607 RVA: 0x0002D9FC File Offset: 0x0002BBFC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ClientSizeChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E18 RID: 3608 RVA: 0x0002DA2C File Offset: 0x0002BC2C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClientSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventClientSize] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E19 RID: 3609 RVA: 0x0002DA5C File Offset: 0x0002BC5C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnControlAdded(ControlEventArgs e)
		{
			ControlEventHandler controlEventHandler = (ControlEventHandler)base.Events[Control.EventControlAdded];
			if (controlEventHandler != null)
			{
				controlEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E1A RID: 3610 RVA: 0x0002DA8C File Offset: 0x0002BC8C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnControlRemoved(ControlEventArgs e)
		{
			ControlEventHandler controlEventHandler = (ControlEventHandler)base.Events[Control.EventControlRemoved];
			if (controlEventHandler != null)
			{
				controlEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.</summary>
		// Token: 0x06000E1B RID: 3611 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCreateControl()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E1C RID: 3612 RVA: 0x0002DABC File Offset: 0x0002BCBC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHandleCreated(EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				if (!this.GetStyle(ControlStyles.UserPaint))
				{
					this.SetWindowFont();
				}
				if (DpiHelper.EnableDpiChangedMessageHandling && !typeof(Form).IsAssignableFrom(base.GetType()))
				{
					int num = this.deviceDpi;
					this.deviceDpi = (int)UnsafeNativeMethods.GetDpiForWindow(new HandleRef(this, this.HandleInternal));
					if (num != this.deviceDpi)
					{
						this.RescaleConstantsForDpi(num, this.deviceDpi);
					}
				}
				this.SetAcceptDrops(this.AllowDrop);
				Region region = (Region)this.Properties.GetObject(Control.PropRegion);
				if (region != null)
				{
					IntPtr intPtr = this.GetHRgn(region);
					try
					{
						if (this.IsActiveX)
						{
							intPtr = this.ActiveXMergeRegion(intPtr);
						}
						if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, this.Handle), new HandleRef(this, intPtr), SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle))) != 0)
						{
							intPtr = IntPtr.Zero;
						}
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						}
					}
				}
				Control.ControlAccessibleObject controlAccessibleObject = this.Properties.GetObject(Control.PropAccessibility) as Control.ControlAccessibleObject;
				Control.ControlAccessibleObject controlAccessibleObject2 = this.Properties.GetObject(Control.PropNcAccessibility) as Control.ControlAccessibleObject;
				IntPtr handle = this.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (controlAccessibleObject != null)
					{
						controlAccessibleObject.Handle = handle;
					}
					if (controlAccessibleObject2 != null)
					{
						controlAccessibleObject2.Handle = handle;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (this.text != null && this.text.Length != 0)
				{
					UnsafeNativeMethods.SetWindowText(new HandleRef(this, this.Handle), this.text);
				}
				if (!(this is ScrollableControl) && !this.IsMirrored && this.GetState2(2) && !this.GetState2(1))
				{
					this.BeginInvoke(new EventHandler(this.OnSetScrollPosition));
					this.SetState2(1, true);
					this.SetState2(2, false);
				}
				if (this.GetState2(8))
				{
					this.ListenToUserPreferenceChanged(this.GetTopLevel());
				}
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleCreated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.IsHandleCreated && this.GetState(32768))
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, this.Handle), Control.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
				this.SetState(32768, false);
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0002DD2C File Offset: 0x0002BF2C
		private void OnSetScrollPosition(object sender, EventArgs e)
		{
			this.SetState2(1, false);
			this.OnInvokedSetScrollPosition(sender, e);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0002DD40 File Offset: 0x0002BF40
		internal virtual void OnInvokedSetScrollPosition(object sender, EventArgs e)
		{
			if (!(this is ScrollableControl) && !this.IsMirrored)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 1;
				if (UnsafeNativeMethods.GetScrollInfo(new HandleRef(this, this.Handle), 0, scrollinfo))
				{
					scrollinfo.nPos = ((this.RightToLeft == RightToLeft.Yes) ? scrollinfo.nMax : scrollinfo.nMin);
					this.SendMessage(276, NativeMethods.Util.MAKELPARAM(4, scrollinfo.nPos), 0);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LocationChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E1F RID: 3615 RVA: 0x0002DDCC File Offset: 0x0002BFCC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.OnMove(EventArgs.Empty);
			EventHandler eventHandler = base.Events[Control.EventLocation] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E20 RID: 3616 RVA: 0x0002DE08 File Offset: 0x0002C008
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHandleDestroyed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleDestroyed];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			this.UpdateReflectParent(false);
			if (!this.RecreatingHandle)
			{
				if (this.GetState(2097152))
				{
					object @object = this.Properties.GetObject(Control.PropBackBrush);
					if (@object != null)
					{
						IntPtr intPtr = (IntPtr)@object;
						if (intPtr != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
						}
						this.Properties.SetObject(Control.PropBackBrush, null);
					}
				}
				this.ListenToUserPreferenceChanged(false);
			}
			try
			{
				if (!this.GetAnyDisposingInHierarchy())
				{
					this.text = this.Text;
					if (this.text != null && this.text.Length == 0)
					{
						this.text = null;
					}
				}
				this.SetAcceptDrops(false);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E21 RID: 3617 RVA: 0x0002DEF8 File Offset: 0x0002C0F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDoubleClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDoubleClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragEnter" /> event.</summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E22 RID: 3618 RVA: 0x0002DF28 File Offset: 0x0002C128
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragEnter(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragEnter];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.</summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E23 RID: 3619 RVA: 0x0002DF58 File Offset: 0x0002C158
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragOver(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragOver];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E24 RID: 3620 RVA: 0x0002DF88 File Offset: 0x0002C188
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDragLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.</summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E25 RID: 3621 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragDrop(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragDrop];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GiveFeedback" /> event.</summary>
		/// <param name="gfbevent">A <see cref="T:System.Windows.Forms.GiveFeedbackEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E26 RID: 3622 RVA: 0x0002DFE8 File Offset: 0x0002C1E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
		{
			GiveFeedbackEventHandler giveFeedbackEventHandler = (GiveFeedbackEventHandler)base.Events[Control.EventGiveFeedback];
			if (giveFeedbackEventHandler != null)
			{
				giveFeedbackEventHandler(this, gfbevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E27 RID: 3623 RVA: 0x0002E018 File Offset: 0x0002C218
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnEnter(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventEnter];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event for the specified control.</summary>
		/// <param name="toInvoke">The <see cref="T:System.Windows.Forms.Control" /> to assign the event to. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E28 RID: 3624 RVA: 0x0002E046 File Offset: 0x0002C246
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeGotFocus(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnGotFocus(e);
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutGotFocus(toInvoke);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E29 RID: 3625 RVA: 0x0002E064 File Offset: 0x0002C264
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGotFocus(EventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(true);
			}
			if (this.parent != null)
			{
				this.parent.ChildGotFocus(this);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventGotFocus];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HelpRequested" /> event.</summary>
		/// <param name="hevent">A <see cref="T:System.Windows.Forms.HelpEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2A RID: 3626 RVA: 0x0002E0B8 File Offset: 0x0002C2B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHelpRequested(HelpEventArgs hevent)
		{
			HelpEventHandler helpEventHandler = (HelpEventHandler)base.Events[Control.EventHelpRequested];
			if (helpEventHandler != null)
			{
				helpEventHandler(this, hevent);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.ParentInternal != null)
			{
				this.ParentInternal.OnHelpRequested(hevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Invalidated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.InvalidateEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2B RID: 3627 RVA: 0x0002E10C File Offset: 0x0002C30C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInvalidated(InvalidateEventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXViewChanged();
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentInvalidated(e);
				}
			}
			InvalidateEventHandler invalidateEventHandler = (InvalidateEventHandler)base.Events[Control.EventInvalidated];
			if (invalidateEventHandler != null)
			{
				invalidateEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2C RID: 3628 RVA: 0x0002E180 File Offset: 0x0002C380
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[Control.EventKeyDown];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2D RID: 3629 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
			KeyPressEventHandler keyPressEventHandler = (KeyPressEventHandler)base.Events[Control.EventKeyPress];
			if (keyPressEventHandler != null)
			{
				keyPressEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2E RID: 3630 RVA: 0x0002E1E0 File Offset: 0x0002C3E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[Control.EventKeyUp];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E2F RID: 3631 RVA: 0x0002E210 File Offset: 0x0002C410
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLayout(LayoutEventArgs levent)
		{
			if (this.IsActiveX)
			{
				this.ActiveXViewChanged();
			}
			LayoutEventHandler layoutEventHandler = (LayoutEventHandler)base.Events[Control.EventLayout];
			if (layoutEventHandler != null)
			{
				layoutEventHandler(this, levent);
			}
			bool flag = this.LayoutEngine.Layout(this, levent);
			if (flag && this.ParentInternal != null)
			{
				this.ParentInternal.SetState(8388608, true);
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0002E276 File Offset: 0x0002C476
		internal virtual void OnLayoutResuming(bool performLayout)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.OnChildLayoutResuming(this, performLayout);
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnLayoutSuspended()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E32 RID: 3634 RVA: 0x0002E290 File Offset: 0x0002C490
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event for the specified control.</summary>
		/// <param name="toInvoke">The <see cref="T:System.Windows.Forms.Control" /> to assign the event to. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E33 RID: 3635 RVA: 0x0002E2BE File Offset: 0x0002C4BE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeLostFocus(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnLostFocus(e);
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(toInvoke);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E34 RID: 3636 RVA: 0x0002E2DC File Offset: 0x0002C4DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLostFocus(EventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(false);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventLostFocus];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MarginChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000E35 RID: 3637 RVA: 0x0002E31C File Offset: 0x0002C51C
		protected virtual void OnMarginChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMarginChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDoubleClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E36 RID: 3638 RVA: 0x0002E34C File Offset: 0x0002C54C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseDoubleClick(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseDoubleClick];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E37 RID: 3639 RVA: 0x0002E37C File Offset: 0x0002C57C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseClick(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseClick];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseCaptureChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E38 RID: 3640 RVA: 0x0002E3AC File Offset: 0x0002C5AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseCaptureChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseCaptureChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E39 RID: 3641 RVA: 0x0002E3DC File Offset: 0x0002C5DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseDown];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E3A RID: 3642 RVA: 0x0002E40C File Offset: 0x0002C60C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseEnter(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseEnter];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E3B RID: 3643 RVA: 0x0002E43C File Offset: 0x0002C63C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DpiChangedBeforeParent" /> event. </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.DpiChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x06000E3C RID: 3644 RVA: 0x0002E46A File Offset: 0x0002C66A
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		protected virtual void OnDpiChangedBeforeParent(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDpiChangedBeforeParent];
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DpiChangedAfterParent" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.DpiChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x06000E3D RID: 3645 RVA: 0x0002E48D File Offset: 0x0002C68D
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		protected virtual void OnDpiChangedAfterParent(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDpiChangedAfterParent];
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseHover" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E3E RID: 3646 RVA: 0x0002E4B0 File Offset: 0x0002C6B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseHover(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseHover];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E3F RID: 3647 RVA: 0x0002E4E0 File Offset: 0x0002C6E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseMove];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E40 RID: 3648 RVA: 0x0002E510 File Offset: 0x0002C710
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseUp];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E41 RID: 3649 RVA: 0x0002E540 File Offset: 0x0002C740
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseWheel(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseWheel];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Move" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E42 RID: 3650 RVA: 0x0002E570 File Offset: 0x0002C770
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMove(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMove];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.RenderTransparent)
			{
				this.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E43 RID: 3651 RVA: 0x0002E5AC File Offset: 0x0002C7AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPaint(PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[Control.EventPaint];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000E44 RID: 3652 RVA: 0x0002E5DC File Offset: 0x0002C7DC
		protected virtual void OnPaddingChanged(EventArgs e)
		{
			if (this.GetStyle(ControlStyles.ResizeRedraw))
			{
				this.Invalidate();
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventPaddingChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the control to paint. </param>
		// Token: 0x06000E45 RID: 3653 RVA: 0x0002E61C File Offset: 0x0002C81C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPaintBackground(PaintEventArgs pevent)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			this.PaintBackground(pevent, new Rectangle(rect.left, rect.top, rect.right, rect.bottom));
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0002E670 File Offset: 0x0002C870
		private void OnParentInvalidated(InvalidateEventArgs e)
		{
			if (!this.RenderTransparent)
			{
				return;
			}
			if (this.IsHandleCreated)
			{
				Rectangle rectangle = e.InvalidRect;
				Point location = this.Location;
				rectangle.Offset(-location.X, -location.Y);
				rectangle = Rectangle.Intersect(this.ClientRectangle, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				this.Invalidate(rectangle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.QueryContinueDrag" /> event.</summary>
		/// <param name="qcdevent">A <see cref="T:System.Windows.Forms.QueryContinueDragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E47 RID: 3655 RVA: 0x0002E6D4 File Offset: 0x0002C8D4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
		{
			QueryContinueDragEventHandler queryContinueDragEventHandler = (QueryContinueDragEventHandler)base.Events[Control.EventQueryContinueDrag];
			if (queryContinueDragEventHandler != null)
			{
				queryContinueDragEventHandler(this, qcdevent);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RegionChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000E48 RID: 3656 RVA: 0x0002E704 File Offset: 0x0002C904
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRegionChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventRegionChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E49 RID: 3657 RVA: 0x0002E734 File Offset: 0x0002C934
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResize(EventArgs e)
		{
			if ((this.controlStyle & ControlStyles.ResizeRedraw) == ControlStyles.ResizeRedraw || this.GetState(4194304))
			{
				this.Invalidate();
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventResize];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.PreviewKeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PreviewKeyDownEventArgs" /> that contains the event data.</param>
		// Token: 0x06000E4A RID: 3658 RVA: 0x0002E790 File Offset: 0x0002C990
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			PreviewKeyDownEventHandler previewKeyDownEventHandler = (PreviewKeyDownEventHandler)base.Events[Control.EventPreviewKeyDown];
			if (previewKeyDownEventHandler != null)
			{
				previewKeyDownEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E4B RID: 3659 RVA: 0x0002E7C0 File Offset: 0x0002C9C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnSizeChanged(EventArgs e)
		{
			this.OnResize(EventArgs.Empty);
			EventHandler eventHandler = base.Events[Control.EventSize] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ChangeUICues" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.UICuesEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E4C RID: 3660 RVA: 0x0002E7FC File Offset: 0x0002C9FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnChangeUICues(UICuesEventArgs e)
		{
			UICuesEventHandler uicuesEventHandler = (UICuesEventHandler)base.Events[Control.EventChangeUICues];
			if (uicuesEventHandler != null)
			{
				uicuesEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.StyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E4D RID: 3661 RVA: 0x0002E82C File Offset: 0x0002CA2C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventStyleChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SystemColorsChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E4E RID: 3662 RVA: 0x0002E85C File Offset: 0x0002CA5C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnSystemColorsChanged(EventArgs e)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnSystemColorsChanged(EventArgs.Empty);
				}
			}
			this.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventSystemColorsChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Validating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E4F RID: 3663 RVA: 0x0002E8CC File Offset: 0x0002CACC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnValidating(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Control.EventValidating];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Validated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000E50 RID: 3664 RVA: 0x0002E8FC File Offset: 0x0002CAFC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnValidated(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventValidated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Provides constants for rescaling the control when a DPI change occurs.</summary>
		/// <param name="deviceDpiOld">The DPI value prior to the change.</param>
		/// <param name="deviceDpiNew">The DPI value after the change.</param>
		// Token: 0x06000E51 RID: 3665 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002E92A File Offset: 0x0002CB2A
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle)
		{
			this.PaintBackground(e, rectangle, this.BackColor, Point.Empty);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002E93F File Offset: 0x0002CB3F
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle, Color backColor)
		{
			this.PaintBackground(e, rectangle, backColor, Point.Empty);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0002E950 File Offset: 0x0002CB50
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle, Color backColor, Point scrollOffset)
		{
			if (this.RenderColorTransparent(backColor))
			{
				this.PaintTransparentBackground(e, rectangle);
			}
			bool flag = (this is Form || this is MdiClient) && this.IsMirrored;
			if (this.BackgroundImage != null && !DisplayInformation.HighContrast && !flag)
			{
				if (this.BackgroundImageLayout == ImageLayout.Tile && ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					this.PaintTransparentBackground(e, rectangle);
				}
				Point point = scrollOffset;
				ScrollableControl scrollableControl = this as ScrollableControl;
				if (scrollableControl != null && point != Point.Empty)
				{
					point = ((ScrollableControl)this).AutoScrollPosition;
				}
				if (ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					Control.PaintBackColor(e, rectangle, backColor);
				}
				ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, backColor, this.BackgroundImageLayout, this.ClientRectangle, rectangle, point, this.RightToLeft);
				return;
			}
			Control.PaintBackColor(e, rectangle, backColor);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0002EA2C File Offset: 0x0002CC2C
		private static void PaintBackColor(PaintEventArgs e, Rectangle rectangle, Color backColor)
		{
			Color color = backColor;
			if (color.A == 255)
			{
				using (WindowsGraphics windowsGraphics = (e.HDC != IntPtr.Zero && DisplayInformation.BitsPerPixel > 8) ? WindowsGraphics.FromHdc(e.HDC) : WindowsGraphics.FromGraphics(e.Graphics))
				{
					color = windowsGraphics.GetNearestColor(color);
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, color))
					{
						windowsGraphics.FillRectangle(windowsBrush, rectangle);
						return;
					}
				}
			}
			if (color.A > 0)
			{
				using (Brush brush = new SolidBrush(color))
				{
					e.Graphics.FillRectangle(brush, rectangle);
				}
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0002EB04 File Offset: 0x0002CD04
		private void PaintException(PaintEventArgs e)
		{
			int num = 2;
			using (Pen pen = new Pen(Color.Red, (float)num))
			{
				Rectangle clientRectangle = this.ClientRectangle;
				Rectangle rect = clientRectangle;
				int num2 = rect.X;
				rect.X = num2 + 1;
				num2 = rect.Y;
				rect.Y = num2 + 1;
				num2 = rect.Width;
				rect.Width = num2 - 1;
				num2 = rect.Height;
				rect.Height = num2 - 1;
				e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				rect.Inflate(-1, -1);
				e.Graphics.FillRectangle(Brushes.White, rect);
				e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, clientRectangle.Bottom);
				e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom, clientRectangle.Right, clientRectangle.Top);
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0002EC28 File Offset: 0x0002CE28
		internal void PaintTransparentBackground(PaintEventArgs e, Rectangle rectangle)
		{
			this.PaintTransparentBackground(e, rectangle, null);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0002EC34 File Offset: 0x0002CE34
		internal void PaintTransparentBackground(PaintEventArgs e, Rectangle rectangle, Region transparentRegion)
		{
			Graphics graphics = e.Graphics;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				if (Application.RenderWithVisualStyles && parentInternal.RenderTransparencyWithVisualStyles)
				{
					GraphicsState graphicsState = null;
					if (transparentRegion != null)
					{
						graphicsState = graphics.Save();
					}
					try
					{
						if (transparentRegion != null)
						{
							graphics.Clip = transparentRegion;
						}
						ButtonRenderer.DrawParentBackground(graphics, rectangle, this);
						return;
					}
					finally
					{
						if (graphicsState != null)
						{
							graphics.Restore(graphicsState);
						}
					}
				}
				Rectangle rectangle2 = new Rectangle(-this.Left, -this.Top, parentInternal.Width, parentInternal.Height);
				Rectangle clipRect = new Rectangle(rectangle.Left + this.Left, rectangle.Top + this.Top, rectangle.Width, rectangle.Height);
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics))
				{
					windowsGraphics.DeviceContext.TranslateTransform(-this.Left, -this.Top);
					using (PaintEventArgs paintEventArgs = new PaintEventArgs(windowsGraphics.GetHdc(), clipRect))
					{
						if (transparentRegion != null)
						{
							paintEventArgs.Graphics.Clip = transparentRegion;
							paintEventArgs.Graphics.TranslateClip(-rectangle2.X, -rectangle2.Y);
						}
						try
						{
							this.InvokePaintBackground(parentInternal, paintEventArgs);
							this.InvokePaint(parentInternal, paintEventArgs);
							return;
						}
						finally
						{
							if (transparentRegion != null)
							{
								paintEventArgs.Graphics.TranslateClip(rectangle2.X, rectangle2.Y);
							}
						}
					}
				}
			}
			graphics.FillRectangle(SystemBrushes.Control, rectangle);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0002EDD0 File Offset: 0x0002CFD0
		private void PaintWithErrorHandling(PaintEventArgs e, short layer)
		{
			try
			{
				this.CacheTextInternal = true;
				if (this.GetState(4194304))
				{
					if (layer == 1)
					{
						this.PaintException(e);
					}
				}
				else
				{
					bool flag = true;
					try
					{
						if (layer != 1)
						{
							if (layer == 2)
							{
								this.OnPaint(e);
							}
						}
						else if (!this.GetStyle(ControlStyles.Opaque))
						{
							this.OnPaintBackground(e);
						}
						flag = false;
					}
					finally
					{
						if (flag)
						{
							this.SetState(4194304, true);
							this.Invalidate();
						}
					}
				}
			}
			finally
			{
				this.CacheTextInternal = false;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0002EE64 File Offset: 0x0002D064
		internal ContainerControl ParentContainerControl
		{
			get
			{
				for (Control parentInternal = this.ParentInternal; parentInternal != null; parentInternal = parentInternal.ParentInternal)
				{
					if (parentInternal is ContainerControl)
					{
						return parentInternal as ContainerControl;
					}
				}
				return null;
			}
		}

		/// <summary>Forces the control to apply layout logic to all its child controls.</summary>
		// Token: 0x06000E5B RID: 3675 RVA: 0x0002EE94 File Offset: 0x0002D094
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void PerformLayout()
		{
			if (this.cachedLayoutEventArgs != null)
			{
				this.PerformLayout(this.cachedLayoutEventArgs);
				this.cachedLayoutEventArgs = null;
				this.SetState2(64, false);
				return;
			}
			this.PerformLayout(null, null);
		}

		/// <summary>Forces the control to apply layout logic to all its child controls.</summary>
		/// <param name="affectedControl">A <see cref="T:System.Windows.Forms.Control" /> that represents the most recently changed control. </param>
		/// <param name="affectedProperty">The name of the most recently changed property on the control. </param>
		// Token: 0x06000E5C RID: 3676 RVA: 0x0002EEC3 File Offset: 0x0002D0C3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void PerformLayout(Control affectedControl, string affectedProperty)
		{
			this.PerformLayout(new LayoutEventArgs(affectedControl, affectedProperty));
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0002EED4 File Offset: 0x0002D0D4
		internal void PerformLayout(LayoutEventArgs args)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.layoutSuspendCount > 0)
			{
				this.SetState(512, true);
				if (this.cachedLayoutEventArgs == null || (this.GetState2(64) && args != null))
				{
					this.cachedLayoutEventArgs = args;
					if (this.GetState2(64))
					{
						this.SetState2(64, false);
					}
				}
				this.LayoutEngine.ProcessSuspendedLayoutEventArgs(this, args);
				return;
			}
			this.layoutSuspendCount = 1;
			try
			{
				this.CacheTextInternal = true;
				this.OnLayout(args);
			}
			finally
			{
				this.CacheTextInternal = false;
				this.SetState(8389120, false);
				this.layoutSuspendCount = 0;
				if (this.ParentInternal != null && this.ParentInternal.GetState(8388608))
				{
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.PreferredSize);
				}
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0002EFAC File Offset: 0x0002D1AC
		internal bool PerformControlValidation(bool bulkValidation)
		{
			if (!this.CausesValidation)
			{
				return false;
			}
			if (this.NotifyValidating())
			{
				return true;
			}
			if (bulkValidation || NativeWindow.WndProcShouldBeDebuggable)
			{
				this.NotifyValidated();
			}
			else
			{
				try
				{
					this.NotifyValidated();
				}
				catch (Exception t)
				{
					Application.OnThreadException(t);
				}
			}
			return false;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0002F004 File Offset: 0x0002D204
		internal bool PerformContainerValidation(ValidationConstraints validationConstraints)
		{
			bool result = false;
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				if ((validationConstraints & ValidationConstraints.ImmediateChildren) != ValidationConstraints.ImmediateChildren && control.ShouldPerformContainerValidation() && control.PerformContainerValidation(validationConstraints))
				{
					result = true;
				}
				if (((validationConstraints & ValidationConstraints.Selectable) != ValidationConstraints.Selectable || control.GetStyle(ControlStyles.Selectable)) && ((validationConstraints & ValidationConstraints.Enabled) != ValidationConstraints.Enabled || control.Enabled) && ((validationConstraints & ValidationConstraints.Visible) != ValidationConstraints.Visible || control.Visible) && ((validationConstraints & ValidationConstraints.TabStop) != ValidationConstraints.TabStop || control.TabStop) && control.PerformControlValidation(true))
				{
					result = true;
				}
			}
			return result;
		}

		/// <summary>Computes the location of the specified screen point into client coordinates.</summary>
		/// <param name="p">The screen coordinate <see cref="T:System.Drawing.Point" /> to convert. </param>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the converted <see cref="T:System.Drawing.Point" />, <paramref name="p" />, in client coordinates.</returns>
		// Token: 0x06000E60 RID: 3680 RVA: 0x0002F0BC File Offset: 0x0002D2BC
		public Point PointToClient(Point p)
		{
			return this.PointToClientInternal(p);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
		internal Point PointToClientInternal(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(this, this.Handle), point, 1);
			return new Point(point.x, point.y);
		}

		/// <summary>Computes the location of the specified client point into screen coordinates.</summary>
		/// <param name="p">The client coordinate <see cref="T:System.Drawing.Point" /> to convert. </param>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the converted <see cref="T:System.Drawing.Point" />, <paramref name="p" />, in screen coordinates.</returns>
		// Token: 0x06000E62 RID: 3682 RVA: 0x0002F114 File Offset: 0x0002D314
		public Point PointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, point, 1);
			return new Point(point.x, point.y);
		}

		/// <summary>Preprocesses keyboard or input messages within the message loop before they are dispatched.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR. </param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E63 RID: 3683 RVA: 0x0002F160 File Offset: 0x0002D360
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual bool PreProcessMessage(ref Message msg)
		{
			if (msg.Msg == 256 || msg.Msg == 260)
			{
				if (!this.GetState2(512))
				{
					this.ProcessUICues(ref msg);
				}
				Keys keyData = (Keys)((long)msg.WParam) | Control.ModifierKeys;
				if (this.ProcessCmdKey(ref msg, keyData))
				{
					return true;
				}
				if (this.IsInputKey(keyData))
				{
					this.SetState2(128, true);
					return false;
				}
				IntSecurity.ModifyFocus.Assert();
				try
				{
					return this.ProcessDialogKey(keyData);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			bool result;
			if (msg.Msg == 258 || msg.Msg == 262)
			{
				if (msg.Msg == 258 && this.IsInputChar((char)((int)msg.WParam)))
				{
					this.SetState2(256, true);
					result = false;
				}
				else
				{
					result = this.ProcessDialogChar((char)((int)msg.WParam));
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		/// <summary>Preprocesses keyboard or input messages within the message loop before they are dispatched.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" /> that represents the message to process.</param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.PreProcessControlState" /> values, depending on whether <see cref="M:System.Windows.Forms.Control.PreProcessMessage(System.Windows.Forms.Message@)" /> is <see langword="true" /> or <see langword="false" /> and whether <see cref="M:System.Windows.Forms.Control.IsInputKey(System.Windows.Forms.Keys)" /> or <see cref="M:System.Windows.Forms.Control.IsInputChar(System.Char)" /> are <see langword="true" /> or <see langword="false" />.</returns>
		// Token: 0x06000E64 RID: 3684 RVA: 0x0002F268 File Offset: 0x0002D468
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public PreProcessControlState PreProcessControlMessage(ref Message msg)
		{
			return Control.PreProcessControlMessageInternal(null, ref msg);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0002F274 File Offset: 0x0002D474
		internal static PreProcessControlState PreProcessControlMessageInternal(Control target, ref Message msg)
		{
			if (target == null)
			{
				target = Control.FromChildHandleInternal(msg.HWnd);
			}
			if (target == null)
			{
				return PreProcessControlState.MessageNotNeeded;
			}
			target.SetState2(128, false);
			target.SetState2(256, false);
			target.SetState2(512, true);
			PreProcessControlState result;
			try
			{
				Keys keyData = (Keys)((long)msg.WParam) | Control.ModifierKeys;
				if (msg.Msg == 256 || msg.Msg == 260)
				{
					target.ProcessUICues(ref msg);
					PreviewKeyDownEventArgs previewKeyDownEventArgs = new PreviewKeyDownEventArgs(keyData);
					target.OnPreviewKeyDown(previewKeyDownEventArgs);
					if (previewKeyDownEventArgs.IsInputKey)
					{
						return PreProcessControlState.MessageNeeded;
					}
				}
				PreProcessControlState preProcessControlState = PreProcessControlState.MessageNotNeeded;
				if (!target.PreProcessMessage(ref msg))
				{
					if (msg.Msg == 256 || msg.Msg == 260)
					{
						if (target.GetState2(128) || target.IsInputKey(keyData))
						{
							preProcessControlState = PreProcessControlState.MessageNeeded;
						}
					}
					else if ((msg.Msg == 258 || msg.Msg == 262) && (target.GetState2(256) || target.IsInputChar((char)((int)msg.WParam))))
					{
						preProcessControlState = PreProcessControlState.MessageNeeded;
					}
				}
				else
				{
					preProcessControlState = PreProcessControlState.MessageProcessed;
				}
				result = preProcessControlState;
			}
			finally
			{
				target.SetState2(512, false);
			}
			return result;
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E66 RID: 3686 RVA: 0x0002F3B0 File Offset: 0x0002D5B0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			return (contextMenu != null && contextMenu.ProcessCmdKey(ref msg, keyData, this)) || (this.parent != null && this.parent.ProcessCmdKey(ref msg, keyData));
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0002F3FC File Offset: 0x0002D5FC
		private void PrintToMetaFile(HandleRef hDC, IntPtr lParam)
		{
			lParam = (IntPtr)((long)lParam & -17L);
			NativeMethods.POINT point = new NativeMethods.POINT();
			bool flag = SafeNativeMethods.GetViewportOrgEx(hDC, point);
			HandleRef handleRef = new HandleRef(null, SafeNativeMethods.CreateRectRgn(point.x, point.y, point.x + this.Width, point.y + this.Height));
			try
			{
				NativeMethods.RegionFlags regionFlags = (NativeMethods.RegionFlags)SafeNativeMethods.SelectClipRgn(hDC, handleRef);
				this.PrintToMetaFileRecursive(hDC, lParam, new Rectangle(Point.Empty, this.Size));
			}
			finally
			{
				flag = SafeNativeMethods.DeleteObject(handleRef);
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0002F498 File Offset: 0x0002D698
		internal virtual void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				this.PrintToMetaFile_SendPrintMessage(hDC, (IntPtr)((long)lParam & -5L));
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				bool windowRect = UnsafeNativeMethods.GetWindowRect(new HandleRef(null, this.Handle), ref rect);
				Point location = this.PointToScreen(Point.Empty);
				location = new Point(location.X - rect.left, location.Y - rect.top);
				Rectangle bounds2 = new Rectangle(location, this.ClientSize);
				using (new WindowsFormsUtils.DCMapping(hDC, bounds2))
				{
					this.PrintToMetaFile_SendPrintMessage(hDC, (IntPtr)((long)lParam & -3L));
					int count = this.Controls.Count;
					for (int i = count - 1; i >= 0; i--)
					{
						Control control = this.Controls[i];
						if (control.Visible)
						{
							control.PrintToMetaFileRecursive(hDC, lParam, control.Bounds);
						}
					}
				}
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0002F5C0 File Offset: 0x0002D7C0
		private void PrintToMetaFile_SendPrintMessage(HandleRef hDC, IntPtr lParam)
		{
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				this.SendMessage(791, hDC.Handle, lParam);
				return;
			}
			if (this.Controls.Count == 0)
			{
				lParam = (IntPtr)((long)lParam | 16L);
			}
			using (Control.MetafileDCWrapper metafileDCWrapper = new Control.MetafileDCWrapper(hDC, this.Size))
			{
				this.SendMessage(791, metafileDCWrapper.HDC, lParam);
			}
		}

		/// <summary>Processes a dialog character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6A RID: 3690 RVA: 0x0002F648 File Offset: 0x0002D848
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessDialogChar(char charCode)
		{
			return this.parent != null && this.parent.ProcessDialogChar(charCode);
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6B RID: 3691 RVA: 0x0002F660 File Offset: 0x0002D860
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessDialogKey(Keys keyData)
		{
			return this.parent != null && this.parent.ProcessDialogKey(keyData);
		}

		/// <summary>Processes a key message and generates the appropriate control events.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6C RID: 3692 RVA: 0x0002F678 File Offset: 0x0002D878
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessKeyEventArgs(ref Message m)
		{
			KeyEventArgs keyEventArgs = null;
			KeyPressEventArgs keyPressEventArgs = null;
			IntPtr wparam = IntPtr.Zero;
			if (m.Msg == 258 || m.Msg == 262)
			{
				int num = this.ImeWmCharsToIgnore;
				if (num > 0)
				{
					num--;
					this.ImeWmCharsToIgnore = num;
					return false;
				}
				keyPressEventArgs = new KeyPressEventArgs((char)((long)m.WParam));
				this.OnKeyPress(keyPressEventArgs);
				wparam = (IntPtr)((int)keyPressEventArgs.KeyChar);
			}
			else if (m.Msg == 646)
			{
				int num2 = this.ImeWmCharsToIgnore;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					char keyChar = '\0';
					byte[] array = new byte[]
					{
						(byte)((int)((long)m.WParam) >> 8),
						(byte)((long)m.WParam)
					};
					char[] array2 = new char[1];
					int num3 = UnsafeNativeMethods.MultiByteToWideChar(0, 1, array, array.Length, array2, 0);
					if (num3 <= 0)
					{
						throw new Win32Exception();
					}
					array2 = new char[num3];
					UnsafeNativeMethods.MultiByteToWideChar(0, 1, array, array.Length, array2, array2.Length);
					if (array2[0] != '\0')
					{
						keyChar = array2[0];
						num2 += 2;
					}
					else if (array2[0] == '\0' && array2.Length >= 2)
					{
						keyChar = array2[1];
						num2++;
					}
					this.ImeWmCharsToIgnore = num2;
					keyPressEventArgs = new KeyPressEventArgs(keyChar);
				}
				else
				{
					num2 += 3 - Marshal.SystemDefaultCharSize;
					this.ImeWmCharsToIgnore = num2;
					keyPressEventArgs = new KeyPressEventArgs((char)((long)m.WParam));
				}
				char keyChar2 = keyPressEventArgs.KeyChar;
				this.OnKeyPress(keyPressEventArgs);
				if (keyPressEventArgs.KeyChar == keyChar2)
				{
					wparam = m.WParam;
				}
				else if (Marshal.SystemDefaultCharSize == 1)
				{
					string text = new string(new char[]
					{
						keyPressEventArgs.KeyChar
					});
					int num4 = UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
					if (num4 >= 2)
					{
						byte[] array3 = new byte[num4];
						UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array3, array3.Length, IntPtr.Zero, IntPtr.Zero);
						int num5 = Marshal.SizeOf(typeof(IntPtr));
						if (num4 > num5)
						{
							num4 = num5;
						}
						long num6 = 0L;
						for (int i = 0; i < num4; i++)
						{
							num6 <<= 8;
							num6 |= (long)((ulong)array3[i]);
						}
						wparam = (IntPtr)num6;
					}
					else if (num4 == 1)
					{
						byte[] array3 = new byte[num4];
						UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array3, array3.Length, IntPtr.Zero, IntPtr.Zero);
						wparam = (IntPtr)((int)array3[0]);
					}
					else
					{
						wparam = m.WParam;
					}
				}
				else
				{
					wparam = (IntPtr)((int)keyPressEventArgs.KeyChar);
				}
			}
			else
			{
				keyEventArgs = new KeyEventArgs((Keys)((long)m.WParam) | Control.ModifierKeys);
				if (m.Msg == 256 || m.Msg == 260)
				{
					this.OnKeyDown(keyEventArgs);
				}
				else
				{
					this.OnKeyUp(keyEventArgs);
				}
			}
			if (keyPressEventArgs != null)
			{
				m.WParam = wparam;
				return keyPressEventArgs.Handled;
			}
			if (keyEventArgs.SuppressKeyPress)
			{
				this.RemovePendingMessages(258, 258);
				this.RemovePendingMessages(262, 262);
				this.RemovePendingMessages(646, 646);
			}
			return keyEventArgs.Handled;
		}

		/// <summary>Processes a keyboard message.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6D RID: 3693 RVA: 0x0002F9AA File Offset: 0x0002DBAA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessKeyMessage(ref Message m)
		{
			return (this.parent != null && this.parent.ProcessKeyPreview(ref m)) || this.ProcessKeyEventArgs(ref m);
		}

		/// <summary>Previews a keyboard message.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6E RID: 3694 RVA: 0x0002F9CB File Offset: 0x0002DBCB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessKeyPreview(ref Message m)
		{
			return this.parent != null && this.parent.ProcessKeyPreview(ref m);
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E6F RID: 3695 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessMnemonic(char charCode)
		{
			return false;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002F9E4 File Offset: 0x0002DBE4
		internal void ProcessUICues(ref Message msg)
		{
			Keys keys = (Keys)((int)msg.WParam & 65535);
			if (keys != Keys.F10 && keys != Keys.Menu && keys != Keys.Tab)
			{
				return;
			}
			Control control = null;
			int num = (int)((long)this.SendMessage(297, 0, 0));
			if (num == 0)
			{
				control = this.TopMostParent;
				num = (int)control.SendMessage(297, 0, 0);
			}
			int num2 = 0;
			if ((keys == Keys.F10 || keys == Keys.Menu) && (num & 2) != 0)
			{
				num2 |= 2;
			}
			if (keys == Keys.Tab && (num & 1) != 0)
			{
				num2 |= 1;
			}
			if (num2 != 0)
			{
				if (control == null)
				{
					control = this.TopMostParent;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(control, control.Handle), (UnsafeNativeMethods.GetParent(new HandleRef(null, control.Handle)) == IntPtr.Zero) ? 295 : 296, (IntPtr)(2 | num2 << 16), IntPtr.Zero);
			}
		}

		/// <summary>Raises the appropriate drag event.</summary>
		/// <param name="key">The event to raise. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E71 RID: 3697 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseDragEvent(object key, DragEventArgs e)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[key];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, e);
			}
		}

		/// <summary>Raises the appropriate paint event.</summary>
		/// <param name="key">The event to raise. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000E72 RID: 3698 RVA: 0x0002FAF0 File Offset: 0x0002DCF0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaisePaintEvent(object key, PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[Control.EventPaint];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0002FB20 File Offset: 0x0002DD20
		private void RemovePendingMessages(int msgMin, int msgMax)
		{
			if (!this.IsDisposed)
			{
				NativeMethods.MSG msg = default(NativeMethods.MSG);
				IntPtr handle = this.Handle;
				while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, handle), msgMin, msgMax, 1))
				{
				}
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.BackColor" /> property to its default value.</summary>
		// Token: 0x06000E74 RID: 3700 RVA: 0x0002FB57 File Offset: 0x0002DD57
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetBackColor()
		{
			this.BackColor = Color.Empty;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.Cursor" /> property to its default value.</summary>
		// Token: 0x06000E75 RID: 3701 RVA: 0x0002FB64 File Offset: 0x0002DD64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetCursor()
		{
			this.Cursor = null;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0002FB6D File Offset: 0x0002DD6D
		private void ResetEnabled()
		{
			this.Enabled = true;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.Font" /> property to its default value.</summary>
		// Token: 0x06000E77 RID: 3703 RVA: 0x0002FB76 File Offset: 0x0002DD76
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetFont()
		{
			this.Font = null;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property to its default value.</summary>
		// Token: 0x06000E78 RID: 3704 RVA: 0x0002FB7F File Offset: 0x0002DD7F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetForeColor()
		{
			this.ForeColor = Color.Empty;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0002FB8C File Offset: 0x0002DD8C
		private void ResetLocation()
		{
			this.Location = new Point(0, 0);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002FB9B File Offset: 0x0002DD9B
		private void ResetMargin()
		{
			this.Margin = this.DefaultMargin;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0002FBA9 File Offset: 0x0002DDA9
		private void ResetMinimumSize()
		{
			this.MinimumSize = this.DefaultMinimumSize;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0002FBB7 File Offset: 0x0002DDB7
		private void ResetPadding()
		{
			CommonProperties.ResetPadding(this);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0002FBBF File Offset: 0x0002DDBF
		private void ResetSize()
		{
			this.Size = this.DefaultSize;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.RightToLeft" /> property to its default value.</summary>
		// Token: 0x06000E7E RID: 3710 RVA: 0x0002FBCD File Offset: 0x0002DDCD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.Inherit;
		}

		/// <summary>Forces the re-creation of the handle for the control.</summary>
		// Token: 0x06000E7F RID: 3711 RVA: 0x0002FBD6 File Offset: 0x0002DDD6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RecreateHandle()
		{
			this.RecreateHandleCore();
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0002FBE0 File Offset: 0x0002DDE0
		internal virtual void RecreateHandleCore()
		{
			lock (this)
			{
				if (this.IsHandleCreated)
				{
					bool containsFocus = this.ContainsFocus;
					bool flag2 = (this.state & 1) != 0;
					if (this.GetState(16384))
					{
						this.SetState(8192, true);
						this.UnhookMouseEvent();
					}
					HandleRef handleRef = new HandleRef(this, UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle)));
					try
					{
						Control[] array = null;
						this.state |= 16;
						try
						{
							Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
							if (controlCollection != null && controlCollection.Count > 0)
							{
								array = new Control[controlCollection.Count];
								for (int i = 0; i < controlCollection.Count; i++)
								{
									Control control = controlCollection[i];
									if (control != null && control.IsHandleCreated)
									{
										control.OnParentHandleRecreating();
										array[i] = control;
									}
									else
									{
										array[i] = null;
									}
								}
							}
							this.DestroyHandle();
							this.CreateHandle();
						}
						finally
						{
							this.state &= -17;
							if (array != null)
							{
								foreach (Control control2 in array)
								{
									if (control2 != null && control2.IsHandleCreated)
									{
										control2.OnParentHandleRecreated();
									}
								}
							}
						}
						if (flag2)
						{
							this.CreateControl();
						}
					}
					finally
					{
						if (handleRef.Handle != IntPtr.Zero && (Control.FromHandleInternal(handleRef.Handle) == null || this.parent == null) && UnsafeNativeMethods.IsWindow(handleRef))
						{
							UnsafeNativeMethods.SetParent(new HandleRef(this, this.Handle), handleRef);
						}
					}
					if (containsFocus)
					{
						this.FocusInternal();
					}
				}
			}
		}

		/// <summary>Computes the size and location of the specified screen rectangle in client coordinates.</summary>
		/// <param name="r">The screen coordinate <see cref="T:System.Drawing.Rectangle" /> to convert. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the converted <see cref="T:System.Drawing.Rectangle" />, <paramref name="r" />, in client coordinates.</returns>
		// Token: 0x06000E81 RID: 3713 RVA: 0x0002FDDC File Offset: 0x0002DFDC
		public Rectangle RectangleToClient(Rectangle r)
		{
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
			UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(this, this.Handle), ref rect, 2);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Computes the size and location of the specified client rectangle in screen coordinates.</summary>
		/// <param name="r">The client coordinate <see cref="T:System.Drawing.Rectangle" /> to convert. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the converted <see cref="T:System.Drawing.Rectangle" />, <paramref name="p" />, in screen coordinates.</returns>
		// Token: 0x06000E82 RID: 3714 RVA: 0x0002FE44 File Offset: 0x0002E044
		public Rectangle RectangleToScreen(Rectangle r)
		{
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, ref rect, 2);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Reflects the specified message to the control that is bound to the specified handle.</summary>
		/// <param name="hWnd">An <see cref="T:System.IntPtr" /> representing the handle of the control to reflect the message to. </param>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> representing the Windows message to reflect. </param>
		/// <returns>
		///     <see langword="true" /> if the message was reflected; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E83 RID: 3715 RVA: 0x0002FEAA File Offset: 0x0002E0AA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static bool ReflectMessage(IntPtr hWnd, ref Message m)
		{
			IntSecurity.SendMessages.Demand();
			return Control.ReflectMessageInternal(hWnd, ref m);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0002FEC0 File Offset: 0x0002E0C0
		internal static bool ReflectMessageInternal(IntPtr hWnd, ref Message m)
		{
			Control control = Control.FromHandleInternal(hWnd);
			if (control == null)
			{
				return false;
			}
			m.Result = control.SendMessage(8192 + m.Msg, m.WParam, m.LParam);
			return true;
		}

		/// <summary>Forces the control to invalidate its client area and immediately redraw itself and any child controls.</summary>
		// Token: 0x06000E85 RID: 3717 RVA: 0x0002FEFE File Offset: 0x0002E0FE
		public virtual void Refresh()
		{
			this.Invalidate(true);
			this.Update();
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0002FF0D File Offset: 0x0002E10D
		internal virtual void ReleaseUiaProvider(IntPtr handle)
		{
			if (this.IsHandleCreated)
			{
				UnsafeNativeMethods.UiaReturnRawElementProvider(new HandleRef(this, handle), new IntPtr(0), new IntPtr(0), null);
			}
		}

		/// <summary>Resets the control to handle the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		// Token: 0x06000E87 RID: 3719 RVA: 0x0002FF31 File Offset: 0x0002E131
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void ResetMouseEventArgs()
		{
			if (this.GetState(16384))
			{
				this.UnhookMouseEvent();
				this.HookMouseEvent();
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.Text" /> property to its default value.</summary>
		// Token: 0x06000E88 RID: 3720 RVA: 0x0002FF4C File Offset: 0x0002E14C
		public virtual void ResetText()
		{
			this.Text = string.Empty;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0002FF59 File Offset: 0x0002E159
		private void ResetVisible()
		{
			this.Visible = true;
		}

		/// <summary>Resumes usual layout logic.</summary>
		// Token: 0x06000E8A RID: 3722 RVA: 0x0002FF62 File Offset: 0x0002E162
		public void ResumeLayout()
		{
			this.ResumeLayout(true);
		}

		/// <summary>Resumes usual layout logic, optionally forcing an immediate layout of pending layout requests.</summary>
		/// <param name="performLayout">
		///       <see langword="true" /> to execute pending layout requests; otherwise, <see langword="false" />. </param>
		// Token: 0x06000E8B RID: 3723 RVA: 0x0002FF6C File Offset: 0x0002E16C
		public void ResumeLayout(bool performLayout)
		{
			bool flag = false;
			if (this.layoutSuspendCount > 0)
			{
				if (this.layoutSuspendCount == 1)
				{
					this.layoutSuspendCount += 1;
					try
					{
						this.OnLayoutResuming(performLayout);
					}
					finally
					{
						this.layoutSuspendCount -= 1;
					}
				}
				this.layoutSuspendCount -= 1;
				if (this.layoutSuspendCount == 0 && this.GetState(512) && performLayout)
				{
					this.PerformLayout();
					flag = true;
				}
			}
			if (!flag)
			{
				this.SetState2(64, true);
			}
			if (!performLayout)
			{
				CommonProperties.xClearPreferredSizeCache(this);
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						this.LayoutEngine.InitLayout(controlCollection[i], BoundsSpecified.All);
						CommonProperties.xClearPreferredSizeCache(controlCollection[i]);
					}
				}
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00030058 File Offset: 0x0002E258
		internal void SetAcceptDrops(bool accept)
		{
			if (accept != this.GetState(128) && this.IsHandleCreated)
			{
				try
				{
					if (Application.OleRequired() != ApartmentState.STA)
					{
						throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
					}
					if (accept)
					{
						IntSecurity.ClipboardRead.Demand();
						int num = UnsafeNativeMethods.RegisterDragDrop(new HandleRef(this, this.Handle), new DropTarget(this));
						if (num != 0 && num != -2147221247)
						{
							throw new Win32Exception(num);
						}
					}
					else
					{
						int num2 = UnsafeNativeMethods.RevokeDragDrop(new HandleRef(this, this.Handle));
						if (num2 != 0 && num2 != -2147221248)
						{
							throw new Win32Exception(num2);
						}
					}
					this.SetState(128, accept);
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
				}
			}
		}

		/// <summary>Scales the control and any child controls.</summary>
		/// <param name="ratio">The ratio to use for scaling.</param>
		// Token: 0x06000E8D RID: 3725 RVA: 0x00030124 File Offset: 0x0002E324
		[Obsolete("This method has been deprecated. Use the Scale(SizeF ratio) method instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Scale(float ratio)
		{
			this.ScaleCore(ratio, ratio);
		}

		/// <summary>Scales the entire control and any child controls.</summary>
		/// <param name="dx">The horizontal scaling factor.</param>
		/// <param name="dy">The vertical scaling factor.</param>
		// Token: 0x06000E8E RID: 3726 RVA: 0x00030130 File Offset: 0x0002E330
		[Obsolete("This method has been deprecated. Use the Scale(SizeF ratio) method instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Scale(float dx, float dy)
		{
			this.SuspendLayout();
			try
			{
				this.ScaleCore(dx, dy);
			}
			finally
			{
				this.ResumeLayout();
			}
		}

		/// <summary>Scales the control and all child controls by the specified scaling factor.</summary>
		/// <param name="factor">A <see cref="T:System.Drawing.SizeF" /> containing the horizontal and vertical scaling factors.</param>
		// Token: 0x06000E8F RID: 3727 RVA: 0x00030164 File Offset: 0x0002E364
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Scale(SizeF factor)
		{
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				this.ScaleControl(factor, factor, this);
				if (this.ScaleChildren)
				{
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							Control control = controlCollection[i];
							control.Scale(factor);
						}
					}
				}
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000301F4 File Offset: 0x0002E3F4
		internal virtual void Scale(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				this.ScaleControl(includedFactor, excludedFactor, requestingControl);
				this.ScaleChildControls(includedFactor, excludedFactor, requestingControl, false);
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0003024C File Offset: 0x0002E44C
		internal void ScaleChildControls(SizeF includedFactor, SizeF excludedFactor, Control requestingControl, bool updateWindowFontIfNeeded = false)
		{
			if (this.ScaleChildren)
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						Control control = controlCollection[i];
						if (updateWindowFontIfNeeded)
						{
							control.UpdateWindowFontIfNeeded();
						}
						control.Scale(includedFactor, excludedFactor, requestingControl);
					}
				}
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000302A6 File Offset: 0x0002E4A6
		internal void UpdateWindowFontIfNeeded()
		{
			if (DpiHelper.EnableDpiChangedHighDpiImprovements && !this.GetStyle(ControlStyles.UserPaint) && this.Properties.GetObject(Control.PropFont) == null)
			{
				this.SetWindowFont();
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000302D0 File Offset: 0x0002E4D0
		internal void ScaleControl(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			try
			{
				this.IsCurrentlyBeingScaled = true;
				BoundsSpecified boundsSpecified = BoundsSpecified.None;
				BoundsSpecified boundsSpecified2 = BoundsSpecified.None;
				if (!includedFactor.IsEmpty)
				{
					boundsSpecified = this.RequiredScaling;
				}
				if (!excludedFactor.IsEmpty)
				{
					boundsSpecified2 |= (~this.RequiredScaling & BoundsSpecified.All);
				}
				if (boundsSpecified != BoundsSpecified.None)
				{
					this.ScaleControl(includedFactor, boundsSpecified);
				}
				if (boundsSpecified2 != BoundsSpecified.None)
				{
					this.ScaleControl(excludedFactor, boundsSpecified2);
				}
				if (!includedFactor.IsEmpty)
				{
					this.RequiredScaling = BoundsSpecified.None;
				}
			}
			finally
			{
				this.IsCurrentlyBeingScaled = false;
			}
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06000E94 RID: 3732 RVA: 0x00030350 File Offset: 0x0002E550
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			CreateParams createParams = this.CreateParams;
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
			this.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			Size size = this.MinimumSize;
			Size size2 = this.MaximumSize;
			this.MinimumSize = Size.Empty;
			this.MaximumSize = Size.Empty;
			Rectangle scaledBounds = this.GetScaledBounds(this.Bounds, factor, specified);
			float num = factor.Width;
			float num2 = factor.Height;
			Padding padding = this.Padding;
			Padding margin = this.Margin;
			if (num == 1f)
			{
				specified &= ~(BoundsSpecified.X | BoundsSpecified.Width);
			}
			if (num2 == 1f)
			{
				specified &= ~(BoundsSpecified.Y | BoundsSpecified.Height);
			}
			if (num != 1f)
			{
				padding.Left = (int)Math.Round((double)((float)padding.Left * num));
				padding.Right = (int)Math.Round((double)((float)padding.Right * num));
				margin.Left = (int)Math.Round((double)((float)margin.Left * num));
				margin.Right = (int)Math.Round((double)((float)margin.Right * num));
			}
			if (num2 != 1f)
			{
				padding.Top = (int)Math.Round((double)((float)padding.Top * num2));
				padding.Bottom = (int)Math.Round((double)((float)padding.Bottom * num2));
				margin.Top = (int)Math.Round((double)((float)margin.Top * num2));
				margin.Bottom = (int)Math.Round((double)((float)margin.Bottom * num2));
			}
			this.Padding = padding;
			this.Margin = margin;
			Size size3 = rect.Size;
			if (!size.IsEmpty)
			{
				size -= size3;
				size = this.ScaleSize(LayoutUtils.UnionSizes(Size.Empty, size), factor.Width, factor.Height) + size3;
			}
			if (!size2.IsEmpty)
			{
				size2 -= size3;
				size2 = this.ScaleSize(LayoutUtils.UnionSizes(Size.Empty, size2), factor.Width, factor.Height) + size3;
			}
			Size b = LayoutUtils.ConvertZeroToUnbounded(size2);
			Size a = LayoutUtils.IntersectSizes(scaledBounds.Size, b);
			a = LayoutUtils.UnionSizes(a, size);
			if (DpiHelper.EnableAnchorLayoutHighDpiImprovements && this.ParentInternal != null && this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
			{
				DefaultLayout.ScaleAnchorInfo(this, factor);
			}
			this.SetBoundsCore(scaledBounds.X, scaledBounds.Y, a.Width, a.Height, BoundsSpecified.All);
			this.MaximumSize = size2;
			this.MinimumSize = size;
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="dx">The horizontal scaling factor.</param>
		/// <param name="dy">The vertical scaling factor.</param>
		// Token: 0x06000E95 RID: 3733 RVA: 0x000305E0 File Offset: 0x0002E7E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void ScaleCore(float dx, float dy)
		{
			this.SuspendLayout();
			try
			{
				int num = (int)Math.Round((double)((float)this.x * dx));
				int num2 = (int)Math.Round((double)((float)this.y * dy));
				int num3 = this.width;
				if ((this.controlStyle & ControlStyles.FixedWidth) != ControlStyles.FixedWidth)
				{
					num3 = (int)Math.Round((double)((float)(this.x + this.width) * dx)) - num;
				}
				int num4 = this.height;
				if ((this.controlStyle & ControlStyles.FixedHeight) != ControlStyles.FixedHeight)
				{
					num4 = (int)Math.Round((double)((float)(this.y + this.height) * dy)) - num2;
				}
				this.SetBounds(num, num2, num3, num4, BoundsSpecified.All);
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						controlCollection[i].Scale(dx, dy);
					}
				}
			}
			finally
			{
				this.ResumeLayout();
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000306D8 File Offset: 0x0002E8D8
		internal Size ScaleSize(Size startSize, float x, float y)
		{
			Size result = startSize;
			if (!this.GetStyle(ControlStyles.FixedWidth))
			{
				result.Width = (int)Math.Round((double)((float)result.Width * x));
			}
			if (!this.GetStyle(ControlStyles.FixedHeight))
			{
				result.Height = (int)Math.Round((double)((float)result.Height * y));
			}
			return result;
		}

		/// <summary>Activates the control.</summary>
		// Token: 0x06000E97 RID: 3735 RVA: 0x0003072C File Offset: 0x0002E92C
		public void Select()
		{
			this.Select(false, false);
		}

		/// <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
		/// <param name="directed">
		///       <see langword="true" /> to specify the direction of the control to select; otherwise, <see langword="false" />. </param>
		/// <param name="forward">
		///       <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order. </param>
		// Token: 0x06000E98 RID: 3736 RVA: 0x00030738 File Offset: 0x0002E938
		protected virtual void Select(bool directed, bool forward)
		{
			IContainerControl containerControlInternal = this.GetContainerControlInternal();
			if (containerControlInternal != null)
			{
				containerControlInternal.ActiveControl = this;
			}
		}

		/// <summary>Activates the next control.</summary>
		/// <param name="ctl">The <see cref="T:System.Windows.Forms.Control" /> at which to start the search. </param>
		/// <param name="forward">
		///       <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order. </param>
		/// <param name="tabStopOnly">
		///       <see langword="true" /> to ignore the controls with the <see cref="P:System.Windows.Forms.Control.TabStop" /> property set to <see langword="false" />; otherwise, <see langword="false" />. </param>
		/// <param name="nested">
		///       <see langword="true" /> to include nested (children of child controls) child controls; otherwise, <see langword="false" />. </param>
		/// <param name="wrap">
		///       <see langword="true" /> to continue searching from the first control in the tab order after the last control has been reached; otherwise, <see langword="false" />. </param>
		/// <returns>
		///     <see langword="true" /> if a control was activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000E99 RID: 3737 RVA: 0x00030758 File Offset: 0x0002E958
		public bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			Control nextSelectableControl = this.GetNextSelectableControl(ctl, forward, tabStopOnly, nested, wrap);
			if (nextSelectableControl != null)
			{
				nextSelectableControl.Select(true, forward);
				return true;
			}
			return false;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00030784 File Offset: 0x0002E984
		private Control GetNextSelectableControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!this.Contains(ctl) || (!nested && ctl.parent != this))
			{
				ctl = null;
			}
			bool flag = false;
			Control control = ctl;
			for (;;)
			{
				ctl = this.GetNextControl(ctl, forward);
				if (ctl == null)
				{
					if (!wrap)
					{
						goto IL_71;
					}
					if (flag)
					{
						break;
					}
					flag = true;
				}
				else if (ctl.CanSelect && (!tabStopOnly || ctl.TabStop) && (nested || ctl.parent == this) && (!AccessibilityImprovements.Level3 || !(ctl.parent is ToolStrip)))
				{
					return ctl;
				}
				if (ctl == control)
				{
					goto IL_71;
				}
			}
			return null;
			IL_71:
			return null;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00030803 File Offset: 0x0002EA03
		internal bool SelectNextControlInternal(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			return this.SelectNextControl(ctl, forward, tabStopOnly, nested, wrap);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00030814 File Offset: 0x0002EA14
		private void SelectNextIfFocused()
		{
			if (this.ContainsFocus && this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					((Control)containerControlInternal).SelectNextControlInternal(this, true, true, true, true);
				}
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00030851 File Offset: 0x0002EA51
		internal IntPtr SendMessage(int msg, int wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00030867 File Offset: 0x0002EA67
		internal IntPtr SendMessage(int msg, ref int wparam, ref int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, ref wparam, ref lparam);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0003087D File Offset: 0x0002EA7D
		internal IntPtr SendMessage(int msg, int wparam, IntPtr lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, (IntPtr)wparam, lparam);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00030898 File Offset: 0x0002EA98
		internal IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x000308AE File Offset: 0x0002EAAE
		internal IntPtr SendMessage(int msg, IntPtr wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, (IntPtr)lparam);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x000308C9 File Offset: 0x0002EAC9
		internal IntPtr SendMessage(int msg, int wparam, ref NativeMethods.RECT lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, ref lparam);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x000308DF File Offset: 0x0002EADF
		internal IntPtr SendMessage(int msg, bool wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000308F5 File Offset: 0x0002EAF5
		internal IntPtr SendMessage(int msg, int wparam, string lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		/// <summary>Sends the control to the back of the z-order.</summary>
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0003090C File Offset: 0x0002EB0C
		public void SendToBack()
		{
			if (this.parent != null)
			{
				this.parent.Controls.SetChildIndex(this, -1);
				return;
			}
			if (this.IsHandleCreated && this.GetTopLevel())
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.HWND_BOTTOM, 0, 0, 0, 0, 3);
			}
		}

		/// <summary>Sets the bounds of the control to the specified location and size.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control. </param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control. </param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control. </param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control. </param>
		// Token: 0x06000EA6 RID: 3750 RVA: 0x00030968 File Offset: 0x0002EB68
		public void SetBounds(int x, int y, int width, int height)
		{
			if (this.x != x || this.y != y || this.width != width || this.height != height)
			{
				this.SetBoundsCore(x, y, width, height, BoundsSpecified.All);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
				return;
			}
			this.InitScaling(BoundsSpecified.All);
		}

		/// <summary>Sets the specified bounds of the control to the specified location and size.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control. </param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control. </param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control. </param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control. </param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values. For any parameter not specified, the current value will be used. </param>
		// Token: 0x06000EA7 RID: 3751 RVA: 0x000309C4 File Offset: 0x0002EBC4
		public void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.X) == BoundsSpecified.None)
			{
				x = this.x;
			}
			if ((specified & BoundsSpecified.Y) == BoundsSpecified.None)
			{
				y = this.y;
			}
			if ((specified & BoundsSpecified.Width) == BoundsSpecified.None)
			{
				width = this.width;
			}
			if ((specified & BoundsSpecified.Height) == BoundsSpecified.None)
			{
				height = this.height;
			}
			if (this.x != x || this.y != y || this.width != width || this.height != height)
			{
				this.SetBoundsCore(x, y, width, height, specified);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
				return;
			}
			this.InitScaling(specified);
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control. </param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control. </param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control. </param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control. </param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values. </param>
		// Token: 0x06000EA8 RID: 3752 RVA: 0x00030A58 File Offset: 0x0002EC58
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.SuspendLayout();
			}
			try
			{
				if (this.x != x || this.y != y || this.width != width || this.height != height)
				{
					CommonProperties.UpdateSpecifiedBounds(this, x, y, width, height, specified);
					Rectangle rectangle = this.ApplyBoundsConstraints(x, y, width, height);
					width = rectangle.Width;
					height = rectangle.Height;
					x = rectangle.X;
					y = rectangle.Y;
					if (!this.IsHandleCreated)
					{
						this.UpdateBounds(x, y, width, height);
					}
					else if (!this.GetState(65536))
					{
						int num = 20;
						if (this.x == x && this.y == y)
						{
							num |= 2;
						}
						if (this.width == width && this.height == height)
						{
							num |= 1;
						}
						this.OnBoundsUpdate(x, y, width, height);
						SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, x, y, width, height, num);
					}
				}
			}
			finally
			{
				this.InitScaling(specified);
				if (this.ParentInternal != null)
				{
					CommonProperties.xClearPreferredSizeCache(this.ParentInternal);
					this.ParentInternal.LayoutEngine.InitLayout(this, specified);
					this.ParentInternal.ResumeLayout(true);
				}
			}
		}

		/// <summary>Sets the size of the client area of the control.</summary>
		/// <param name="x">The client area width, in pixels. </param>
		/// <param name="y">The client area height, in pixels. </param>
		// Token: 0x06000EA9 RID: 3753 RVA: 0x00030BB0 File Offset: 0x0002EDB0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void SetClientSizeCore(int x, int y)
		{
			this.Size = this.SizeFromClientSize(x, y);
			this.clientWidth = x;
			this.clientHeight = y;
			this.OnClientSizeChanged(EventArgs.Empty);
		}

		/// <summary>Determines the size of the entire control from the height and width of its client area.</summary>
		/// <param name="clientSize">A <see cref="T:System.Drawing.Size" /> value representing the height and width of the control's client area.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value representing the height and width of the entire control.</returns>
		// Token: 0x06000EAA RID: 3754 RVA: 0x00030BD9 File Offset: 0x0002EDD9
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Size SizeFromClientSize(Size clientSize)
		{
			return this.SizeFromClientSize(clientSize.Width, clientSize.Height);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00030BF0 File Offset: 0x0002EDF0
		internal Size SizeFromClientSize(int width, int height)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, width, height);
			CreateParams createParams = this.CreateParams;
			this.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			return rect.Size;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00030C30 File Offset: 0x0002EE30
		private void SetHandle(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				this.SetState(1, false);
			}
			this.UpdateRoot();
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00030C50 File Offset: 0x0002EE50
		private void SetParentHandle(IntPtr value)
		{
			if (this.IsHandleCreated)
			{
				IntPtr value2 = UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.Handle));
				bool topLevel = this.GetTopLevel();
				if (value2 != value || (value2 == IntPtr.Zero && !topLevel))
				{
					bool flag = (value2 == IntPtr.Zero && !topLevel) || (value == IntPtr.Zero && topLevel);
					if (flag)
					{
						Form form = this as Form;
						if (form != null && !form.CanRecreateHandle())
						{
							flag = false;
							this.UpdateStyles();
						}
					}
					if (flag)
					{
						this.RecreateHandle();
					}
					if (!this.GetTopLevel())
					{
						if (value == IntPtr.Zero)
						{
							Application.ParkHandle(new HandleRef(this.window, this.Handle), this.DpiAwarenessContext);
							this.UpdateRoot();
							return;
						}
						UnsafeNativeMethods.SetParent(new HandleRef(this.window, this.Handle), new HandleRef(null, value));
						if (this.parent != null)
						{
							this.parent.UpdateChildZOrder(this);
						}
						Application.UnparkHandle(new HandleRef(this.window, this.Handle), this.window.DpiAwarenessContext);
						return;
					}
				}
				else if (value == IntPtr.Zero && value2 == IntPtr.Zero && topLevel)
				{
					UnsafeNativeMethods.SetParent(new HandleRef(this.window, this.Handle), new HandleRef(null, IntPtr.Zero));
					Application.UnparkHandle(new HandleRef(this.window, this.Handle), this.window.DpiAwarenessContext);
				}
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00030DDD File Offset: 0x0002EFDD
		internal void SetState(int flag, bool value)
		{
			this.state = (value ? (this.state | flag) : (this.state & ~flag));
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00030DFB File Offset: 0x0002EFFB
		internal void SetState2(int flag, bool value)
		{
			this.state2 = (value ? (this.state2 | flag) : (this.state2 & ~flag));
		}

		/// <summary>Sets a specified <see cref="T:System.Windows.Forms.ControlStyles" /> flag to either <see langword="true" /> or <see langword="false" />.</summary>
		/// <param name="flag">The <see cref="T:System.Windows.Forms.ControlStyles" /> bit to set. </param>
		/// <param name="value">
		///       <see langword="true" /> to apply the specified style to the control; otherwise, <see langword="false" />. </param>
		// Token: 0x06000EB0 RID: 3760 RVA: 0x00030E19 File Offset: 0x0002F019
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void SetStyle(ControlStyles flag, bool value)
		{
			if ((flag & ControlStyles.EnableNotifyMessage) > (ControlStyles)0 && value)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			this.controlStyle = (value ? (this.controlStyle | flag) : (this.controlStyle & ~flag));
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00030E50 File Offset: 0x0002F050
		internal static IntPtr SetUpPalette(IntPtr dc, bool force, bool realizePalette)
		{
			IntPtr halftonePalette = Graphics.GetHalftonePalette();
			IntPtr intPtr = SafeNativeMethods.SelectPalette(new HandleRef(null, dc), new HandleRef(null, halftonePalette), force ? 0 : 1);
			if (intPtr != IntPtr.Zero && realizePalette)
			{
				SafeNativeMethods.RealizePalette(new HandleRef(null, dc));
			}
			return intPtr;
		}

		/// <summary>Sets the control as the top-level control.</summary>
		/// <param name="value">
		///       <see langword="true" /> to set the control as the top-level control; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="value" /> parameter is set to <see langword="true" /> and the control is an ActiveX control. </exception>
		/// <exception cref="T:System.Exception">The <see cref="M:System.Windows.Forms.Control.GetTopLevel" /> return value is not equal to the <paramref name="value" /> parameter and the <see cref="P:System.Windows.Forms.Control.Parent" /> property is not <see langword="null" />. </exception>
		// Token: 0x06000EB2 RID: 3762 RVA: 0x00030E9C File Offset: 0x0002F09C
		protected void SetTopLevel(bool value)
		{
			if (value && this.IsActiveX)
			{
				throw new InvalidOperationException(SR.GetString("TopLevelNotAllowedIfActiveX"));
			}
			if (value)
			{
				if (this is Form)
				{
					IntSecurity.TopLevelWindow.Demand();
				}
				else
				{
					IntSecurity.UnrestrictedWindows.Demand();
				}
			}
			this.SetTopLevelInternal(value);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00030EEC File Offset: 0x0002F0EC
		internal void SetTopLevelInternal(bool value)
		{
			if (this.GetTopLevel() != value)
			{
				if (this.parent != null)
				{
					throw new ArgumentException(SR.GetString("TopLevelParentedControl"), "value");
				}
				this.SetState(524288, value);
				if (this.IsHandleCreated && this.GetState2(8))
				{
					this.ListenToUserPreferenceChanged(value);
				}
				this.UpdateStyles();
				this.SetParentHandle(IntPtr.Zero);
				if (value && this.Visible)
				{
					this.CreateControl();
				}
				this.UpdateRoot();
			}
		}

		/// <summary>Sets the control to the specified visible state.</summary>
		/// <param name="value">
		///       <see langword="true" /> to make the control visible; otherwise, <see langword="false" />. </param>
		// Token: 0x06000EB4 RID: 3764 RVA: 0x00030F6C File Offset: 0x0002F16C
		protected virtual void SetVisibleCore(bool value)
		{
			try
			{
				System.Internal.HandleCollector.SuspendCollect();
				if (this.GetVisibleCore() != value)
				{
					if (!value)
					{
						this.SelectNextIfFocused();
					}
					bool flag = false;
					if (this.GetTopLevel())
					{
						if (this.IsHandleCreated || value)
						{
							SafeNativeMethods.ShowWindow(new HandleRef(this, this.Handle), value ? this.ShowParams : 0);
						}
					}
					else if (this.IsHandleCreated || (value && this.parent != null && this.parent.Created))
					{
						this.SetState(2, value);
						flag = true;
						try
						{
							if (value)
							{
								this.CreateControl();
							}
							SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 23 | (value ? 64 : 128));
						}
						catch
						{
							this.SetState(2, !value);
							throw;
						}
					}
					if (this.GetVisibleCore() != value)
					{
						this.SetState(2, value);
						flag = true;
					}
					if (flag)
					{
						using (new LayoutTransaction(this.parent, this, PropertyNames.Visible))
						{
							this.OnVisibleChanged(EventArgs.Empty);
						}
					}
					this.UpdateRoot();
				}
				else if (this.GetState(2) || value || !this.IsHandleCreated || SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle)))
				{
					this.SetState(2, value);
					if (this.IsHandleCreated)
					{
						SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 23 | (value ? 64 : 128));
					}
				}
			}
			finally
			{
				System.Internal.HandleCollector.ResumeCollect();
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00031140 File Offset: 0x0002F340
		internal static AutoValidate GetAutoValidateForControl(Control control)
		{
			ContainerControl parentContainerControl = control.ParentContainerControl;
			if (parentContainerControl == null)
			{
				return AutoValidate.EnablePreventFocusChange;
			}
			return parentContainerControl.AutoValidate;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0003115F File Offset: 0x0002F35F
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldAutoValidate
		{
			get
			{
				return Control.GetAutoValidateForControl(this) > AutoValidate.Disable;
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0003116A File Offset: 0x0002F36A
		internal virtual bool ShouldPerformContainerValidation()
		{
			return this.GetStyle(ControlStyles.ContainerControl);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00031174 File Offset: 0x0002F374
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeBackColor()
		{
			return !this.Properties.GetColor(Control.PropBackColor).IsEmpty;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0003119C File Offset: 0x0002F39C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeCursor()
		{
			bool flag;
			object @object = this.Properties.GetObject(Control.PropCursor, out flag);
			return flag && @object != null;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x000311C5 File Offset: 0x0002F3C5
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeEnabled()
		{
			return !this.GetState(4);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000311D4 File Offset: 0x0002F3D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeForeColor()
		{
			return !this.Properties.GetColor(Control.PropForeColor).IsEmpty;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000311FC File Offset: 0x0002F3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeFont()
		{
			bool flag;
			object @object = this.Properties.GetObject(Control.PropFont, out flag);
			return flag && @object != null;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00031228 File Offset: 0x0002F428
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeRightToLeft()
		{
			bool flag;
			int integer = this.Properties.GetInteger(Control.PropRightToLeft, out flag);
			return flag && integer != 2;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00031254 File Offset: 0x0002F454
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisible()
		{
			return !this.GetState(2);
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Forms.HorizontalAlignment" /> to the appropriate <see cref="T:System.Windows.Forms.HorizontalAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</returns>
		// Token: 0x06000EBF RID: 3775 RVA: 0x00031260 File Offset: 0x0002F460
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected HorizontalAlignment RtlTranslateAlignment(HorizontalAlignment align)
		{
			return this.RtlTranslateHorizontal(align);
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Forms.LeftRightAlignment" /> to the appropriate <see cref="T:System.Windows.Forms.LeftRightAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values.</returns>
		// Token: 0x06000EC0 RID: 3776 RVA: 0x00031269 File Offset: 0x0002F469
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected LeftRightAlignment RtlTranslateAlignment(LeftRightAlignment align)
		{
			return this.RtlTranslateLeftRight(align);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.ContentAlignment" /> to the appropriate <see cref="T:System.Drawing.ContentAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Drawing.ContentAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values.</returns>
		// Token: 0x06000EC1 RID: 3777 RVA: 0x00031272 File Offset: 0x0002F472
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected ContentAlignment RtlTranslateAlignment(ContentAlignment align)
		{
			return this.RtlTranslateContent(align);
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Forms.HorizontalAlignment" /> to the appropriate <see cref="T:System.Windows.Forms.HorizontalAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</returns>
		// Token: 0x06000EC2 RID: 3778 RVA: 0x0003127B File Offset: 0x0002F47B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected HorizontalAlignment RtlTranslateHorizontal(HorizontalAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if (align == HorizontalAlignment.Left)
				{
					return HorizontalAlignment.Right;
				}
				if (HorizontalAlignment.Right == align)
				{
					return HorizontalAlignment.Left;
				}
			}
			return align;
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Forms.LeftRightAlignment" /> to the appropriate <see cref="T:System.Windows.Forms.LeftRightAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values.</returns>
		// Token: 0x06000EC3 RID: 3779 RVA: 0x0003127B File Offset: 0x0002F47B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected LeftRightAlignment RtlTranslateLeftRight(LeftRightAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if (align == LeftRightAlignment.Left)
				{
					return LeftRightAlignment.Right;
				}
				if (LeftRightAlignment.Right == align)
				{
					return LeftRightAlignment.Left;
				}
			}
			return align;
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.ContentAlignment" /> to the appropriate <see cref="T:System.Drawing.ContentAlignment" /> to support right-to-left text.</summary>
		/// <param name="align">One of the <see cref="T:System.Drawing.ContentAlignment" /> values. </param>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values.</returns>
		// Token: 0x06000EC4 RID: 3780 RVA: 0x00031294 File Offset: 0x0002F494
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal ContentAlignment RtlTranslateContent(ContentAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if ((align & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.TopLeft)
					{
						return ContentAlignment.TopRight;
					}
					if (align == ContentAlignment.TopRight)
					{
						return ContentAlignment.TopLeft;
					}
				}
				if ((align & WindowsFormsUtils.AnyMiddleAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.MiddleLeft)
					{
						return ContentAlignment.MiddleRight;
					}
					if (align == ContentAlignment.MiddleRight)
					{
						return ContentAlignment.MiddleLeft;
					}
				}
				if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.BottomLeft)
					{
						return ContentAlignment.BottomRight;
					}
					if (align == ContentAlignment.BottomRight)
					{
						return ContentAlignment.BottomLeft;
					}
				}
			}
			return align;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00031304 File Offset: 0x0002F504
		private void SetWindowFont()
		{
			this.SendMessage(48, this.FontHandle, 0);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00031318 File Offset: 0x0002F518
		private void SetWindowStyle(int flag, bool value)
		{
			int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16));
			UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)(value ? (num | flag) : (num & ~flag))));
		}

		/// <summary>Displays the control to the user.</summary>
		// Token: 0x06000EC7 RID: 3783 RVA: 0x0002FF59 File Offset: 0x0002E159
		public void Show()
		{
			this.Visible = true;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0003136C File Offset: 0x0002F56C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeMargin()
		{
			return !this.Margin.Equals(this.DefaultMargin);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0003139B File Offset: 0x0002F59B
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeMaximumSize()
		{
			return this.MaximumSize != this.DefaultMaximumSize;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000313AE File Offset: 0x0002F5AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeMinimumSize()
		{
			return this.MinimumSize != this.DefaultMinimumSize;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000313C4 File Offset: 0x0002F5C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializePadding()
		{
			return !this.Padding.Equals(this.DefaultPadding);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000313F4 File Offset: 0x0002F5F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeSize()
		{
			Size defaultSize = this.DefaultSize;
			return this.width != defaultSize.Width || this.height != defaultSize.Height;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0003142B File Offset: 0x0002F62B
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeText()
		{
			return this.Text.Length != 0;
		}

		/// <summary>Temporarily suspends the layout logic for the control.</summary>
		// Token: 0x06000ECE RID: 3790 RVA: 0x0003143B File Offset: 0x0002F63B
		public void SuspendLayout()
		{
			this.layoutSuspendCount += 1;
			if (this.layoutSuspendCount == 1)
			{
				this.OnLayoutSuspended();
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0003145B File Offset: 0x0002F65B
		private void UnhookMouseEvent()
		{
			this.SetState(16384, false);
		}

		/// <summary>Causes the control to redraw the invalidated regions within its client area.</summary>
		// Token: 0x06000ED0 RID: 3792 RVA: 0x00031469 File Offset: 0x0002F669
		public void Update()
		{
			SafeNativeMethods.UpdateWindow(new HandleRef(this.window, this.InternalHandle));
		}

		/// <summary>Updates the bounds of the control with the current size and location.</summary>
		// Token: 0x06000ED1 RID: 3793 RVA: 0x00031484 File Offset: 0x0002F684
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal void UpdateBounds()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			int right = rect.right;
			int bottom = rect.bottom;
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			if (!this.GetTopLevel())
			{
				UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(null, UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.InternalHandle))), ref rect, 2);
			}
			this.UpdateBounds(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, right, bottom);
		}

		/// <summary>Updates the bounds of the control with the specified size and location.</summary>
		/// <param name="x">The <see cref="P:System.Drawing.Point.X" /> coordinate of the control. </param>
		/// <param name="y">The <see cref="P:System.Drawing.Point.Y" /> coordinate of the control. </param>
		/// <param name="width">The <see cref="P:System.Drawing.Size.Width" /> of the control. </param>
		/// <param name="height">The <see cref="P:System.Drawing.Size.Height" /> of the control. </param>
		// Token: 0x06000ED2 RID: 3794 RVA: 0x0003153C File Offset: 0x0002F73C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateBounds(int x, int y, int width, int height)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (rect.right = (rect.top = (rect.bottom = 0)));
			CreateParams createParams = this.CreateParams;
			this.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
			int num = width - (rect.right - rect.left);
			int num2 = height - (rect.bottom - rect.top);
			this.UpdateBounds(x, y, width, height, num, num2);
		}

		/// <summary>Updates the bounds of the control with the specified size, location, and client size.</summary>
		/// <param name="x">The <see cref="P:System.Drawing.Point.X" /> coordinate of the control. </param>
		/// <param name="y">The <see cref="P:System.Drawing.Point.Y" /> coordinate of the control. </param>
		/// <param name="width">The <see cref="P:System.Drawing.Size.Width" /> of the control. </param>
		/// <param name="height">The <see cref="P:System.Drawing.Size.Height" /> of the control. </param>
		/// <param name="clientWidth">The client <see cref="P:System.Drawing.Size.Width" /> of the control. </param>
		/// <param name="clientHeight">The client <see cref="P:System.Drawing.Size.Height" /> of the control. </param>
		// Token: 0x06000ED3 RID: 3795 RVA: 0x000315C8 File Offset: 0x0002F7C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateBounds(int x, int y, int width, int height, int clientWidth, int clientHeight)
		{
			bool flag = this.x != x || this.y != y;
			bool flag2 = this.Width != width || this.Height != height || this.clientWidth != clientWidth || this.clientHeight != clientHeight;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.clientWidth = clientWidth;
			this.clientHeight = clientHeight;
			if (flag)
			{
				this.OnLocationChanged(EventArgs.Empty);
			}
			if (flag2)
			{
				this.OnSizeChanged(EventArgs.Empty);
				this.OnClientSizeChanged(EventArgs.Empty);
				CommonProperties.xClearPreferredSizeCache(this);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00031688 File Offset: 0x0002F888
		private void UpdateBindings()
		{
			for (int i = 0; i < this.DataBindings.Count; i++)
			{
				BindingContext.UpdateBinding(this.BindingContext, this.DataBindings[i]);
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000316C4 File Offset: 0x0002F8C4
		private void UpdateChildControlIndex(Control ctl)
		{
			if (!LocalAppContextSwitches.AllowUpdateChildControlIndexForTabControls && base.GetType().IsAssignableFrom(typeof(TabControl)))
			{
				return;
			}
			int num = 0;
			int childIndex = this.Controls.GetChildIndex(ctl);
			IntPtr internalHandle = ctl.InternalHandle;
			while ((internalHandle = UnsafeNativeMethods.GetWindow(new HandleRef(null, internalHandle), 3)) != IntPtr.Zero)
			{
				Control control = Control.FromHandleInternal(internalHandle);
				if (control != null)
				{
					num = this.Controls.GetChildIndex(control, false) + 1;
					break;
				}
			}
			if (num > childIndex)
			{
				num--;
			}
			if (num != childIndex)
			{
				this.Controls.SetChildIndex(ctl, num);
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00031758 File Offset: 0x0002F958
		private void UpdateReflectParent(bool findNewParent)
		{
			if (!this.Disposing && findNewParent && this.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle));
				if (intPtr != IntPtr.Zero)
				{
					this.ReflectParent = Control.FromHandleInternal(intPtr);
					return;
				}
			}
			this.ReflectParent = null;
		}

		/// <summary>Updates the control in its parent's z-order.</summary>
		// Token: 0x06000ED7 RID: 3799 RVA: 0x000317AD File Offset: 0x0002F9AD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateZOrder()
		{
			if (this.parent != null)
			{
				this.parent.UpdateChildZOrder(this);
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000317C4 File Offset: 0x0002F9C4
		private void UpdateChildZOrder(Control ctl)
		{
			if (!this.IsHandleCreated || !ctl.IsHandleCreated || ctl.parent != this)
			{
				return;
			}
			IntPtr intPtr = (IntPtr)NativeMethods.HWND_TOP;
			int num = this.Controls.GetChildIndex(ctl);
			while (--num >= 0)
			{
				Control control = this.Controls[num];
				if (control.IsHandleCreated && control.parent == this)
				{
					intPtr = control.Handle;
					break;
				}
			}
			if (UnsafeNativeMethods.GetWindow(new HandleRef(ctl.window, ctl.Handle), 3) != intPtr)
			{
				this.state |= 256;
				try
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(ctl.window, ctl.Handle), new HandleRef(null, intPtr), 0, 0, 0, 0, 3);
				}
				finally
				{
					this.state &= -257;
				}
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000318B0 File Offset: 0x0002FAB0
		private void UpdateRoot()
		{
			this.window.LockReference(this.GetTopLevel() && this.Visible);
		}

		/// <summary>Forces the assigned styles to be reapplied to the control.</summary>
		// Token: 0x06000EDA RID: 3802 RVA: 0x000318CE File Offset: 0x0002FACE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateStyles()
		{
			this.UpdateStylesCore();
			this.OnStyleChanged(EventArgs.Empty);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000318E4 File Offset: 0x0002FAE4
		internal virtual void UpdateStylesCore()
		{
			if (this.IsHandleCreated)
			{
				CreateParams createParams = this.CreateParams;
				int windowStyle = this.WindowStyle;
				int windowExStyle = this.WindowExStyle;
				if ((this.state & 2) != 0)
				{
					createParams.Style |= 268435456;
				}
				if (windowStyle != createParams.Style)
				{
					this.WindowStyle = createParams.Style;
				}
				if (windowExStyle != createParams.ExStyle)
				{
					this.WindowExStyle = createParams.ExStyle;
					this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 55);
				this.Invalidate(true);
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00031995 File Offset: 0x0002FB95
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Color)
			{
				Control.defaultFont = null;
				this.OnSystemColorsChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnBoundsUpdate(int x, int y, int width, int height)
		{
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000319B1 File Offset: 0x0002FBB1
		internal void WindowAssignHandle(IntPtr handle, bool value)
		{
			this.window.AssignHandle(handle, value);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000319C0 File Offset: 0x0002FBC0
		internal void WindowReleaseHandle()
		{
			this.window.ReleaseHandle();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000319D0 File Offset: 0x0002FBD0
		private void WmClose(ref Message m)
		{
			if (this.ParentInternal != null)
			{
				IntPtr handle = this.Handle;
				IntPtr intPtr = handle;
				while (handle != IntPtr.Zero)
				{
					intPtr = handle;
					handle = UnsafeNativeMethods.GetParent(new HandleRef(null, handle));
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(null, intPtr), -16));
					if ((num & 1073741824) == 0)
					{
						break;
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(null, intPtr), 16, IntPtr.Zero, IntPtr.Zero);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00031A57 File Offset: 0x0002FC57
		private void WmCaptureChanged(ref Message m)
		{
			this.OnMouseCaptureChanged(EventArgs.Empty);
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00031A6B File Offset: 0x0002FC6B
		private void WmCommand(ref Message m)
		{
			if (IntPtr.Zero == m.LParam)
			{
				if (Command.DispatchID(NativeMethods.Util.LOWORD(m.WParam)))
				{
					return;
				}
			}
			else if (Control.ReflectMessageInternal(m.LParam, ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00031AA8 File Offset: 0x0002FCA8
		internal virtual void WmContextMenu(ref Message m)
		{
			this.WmContextMenu(ref m, this);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00031AB4 File Offset: 0x0002FCB4
		internal void WmContextMenu(ref Message m, Control sourceControl)
		{
			ContextMenu contextMenu = this.Properties.GetObject(Control.PropContextMenu) as ContextMenu;
			ContextMenuStrip contextMenuStrip = (contextMenu != null) ? null : (this.Properties.GetObject(Control.PropContextMenuStrip) as ContextMenuStrip);
			if (contextMenu == null && contextMenuStrip == null)
			{
				this.DefWndProc(ref m);
				return;
			}
			int num = NativeMethods.Util.SignedLOWORD(m.LParam);
			int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
			bool isKeyboardActivated = false;
			Point point;
			if ((int)((long)m.LParam) == -1)
			{
				isKeyboardActivated = true;
				point = new Point(this.Width / 2, this.Height / 2);
			}
			else
			{
				point = this.PointToClientInternal(new Point(num, num2));
			}
			if (!this.ClientRectangle.Contains(point))
			{
				this.DefWndProc(ref m);
				return;
			}
			if (contextMenu != null)
			{
				contextMenu.Show(sourceControl, point);
				return;
			}
			if (contextMenuStrip != null)
			{
				contextMenuStrip.ShowInternal(sourceControl, point, isKeyboardActivated);
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00031B98 File Offset: 0x0002FD98
		private void WmCtlColorControl(ref Message m)
		{
			Control control = Control.FromHandleInternal(m.LParam);
			if (control != null)
			{
				m.Result = control.InitializeDCForWmCtlColor(m.WParam, m.Msg);
				if (m.Result != IntPtr.Zero)
				{
					return;
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00031BE6 File Offset: 0x0002FDE6
		private void WmDisplayChange(ref Message m)
		{
			BufferedGraphicsManager.Current.Invalidate();
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00031BF9 File Offset: 0x0002FDF9
		private void WmDrawItem(ref Message m)
		{
			if (m.WParam == IntPtr.Zero)
			{
				this.WmDrawItemMenuItem(ref m);
				return;
			}
			this.WmOwnerDraw(ref m);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00031C1C File Offset: 0x0002FE1C
		private void WmDrawItemMenuItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(drawitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmDrawItem(ref m);
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00031C58 File Offset: 0x0002FE58
		private void WmEraseBkgnd(ref Message m)
		{
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				if (!this.GetStyle(ControlStyles.AllPaintingInWmPaint))
				{
					IntPtr wparam = m.WParam;
					if (wparam == IntPtr.Zero)
					{
						m.Result = (IntPtr)0;
						return;
					}
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetClientRect(new HandleRef(this, this.Handle), ref rect);
					using (PaintEventArgs paintEventArgs = new PaintEventArgs(wparam, Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom)))
					{
						this.PaintWithErrorHandling(paintEventArgs, 1);
					}
				}
				m.Result = (IntPtr)1;
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00031D18 File Offset: 0x0002FF18
		private void WmExitMenuLoop(ref Message m)
		{
			bool flag = (int)((long)m.WParam) != 0;
			if (flag)
			{
				ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
				if (contextMenu != null)
				{
					contextMenu.OnCollapse(EventArgs.Empty);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00031D68 File Offset: 0x0002FF68
		private void WmGetControlName(ref Message m)
		{
			string text;
			if (this.Site != null)
			{
				text = this.Site.Name;
			}
			else
			{
				text = this.Name;
			}
			if (text == null)
			{
				text = "";
			}
			this.MarshalStringToMessage(text, ref m);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00031DA4 File Offset: 0x0002FFA4
		private void WmGetControlType(ref Message m)
		{
			string assemblyQualifiedName = base.GetType().AssemblyQualifiedName;
			this.MarshalStringToMessage(assemblyQualifiedName, ref m);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00031DC8 File Offset: 0x0002FFC8
		private void WmGetObject(ref Message m)
		{
			InternalAccessibleObject internalAccessibleObject = null;
			if (m.Msg == 61 && m.LParam == (IntPtr)(-25) && this.SupportsUiaProviders)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					internalAccessibleObject = new InternalAccessibleObject(this.AccessibilityObject);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				m.Result = UnsafeNativeMethods.UiaReturnRawElementProvider(new HandleRef(this, this.Handle), m.WParam, m.LParam, internalAccessibleObject);
				return;
			}
			AccessibleObject accessibilityObject = this.GetAccessibilityObject((int)((long)m.LParam));
			if (accessibilityObject != null)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					internalAccessibleObject = new InternalAccessibleObject(accessibilityObject);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (internalAccessibleObject != null)
			{
				Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
				try
				{
					object obj = internalAccessibleObject;
					IAccessible accessible = obj as IAccessible;
					if (accessible != null)
					{
						throw new InvalidOperationException(SR.GetString("ControlAccessibileObjectInvalid"));
					}
					UnsafeNativeMethods.IAccessibleInternal accessibleInternal = internalAccessibleObject;
					if (accessibleInternal == null)
					{
						m.Result = (IntPtr)0;
					}
					else
					{
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(accessibleInternal);
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							m.Result = UnsafeNativeMethods.LresultFromObject(ref guid, m.WParam, new HandleRef(accessibilityObject, iunknownForObject));
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							Marshal.Release(iunknownForObject);
						}
					}
					return;
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(SR.GetString("RichControlLresult"), innerException);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00031F44 File Offset: 0x00030144
		private void WmHelp(ref Message m)
		{
			HelpInfo helpInfo = MessageBox.HelpInfo;
			if (helpInfo != null)
			{
				switch (helpInfo.Option)
				{
				case 1:
					Help.ShowHelp(this, helpInfo.HelpFilePath);
					break;
				case 2:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Keyword);
					break;
				case 3:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Navigator);
					break;
				case 4:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Navigator, helpInfo.Param);
					break;
				}
			}
			NativeMethods.HELPINFO helpinfo = (NativeMethods.HELPINFO)m.GetLParam(typeof(NativeMethods.HELPINFO));
			HelpEventArgs helpEventArgs = new HelpEventArgs(new Point(helpinfo.MousePos.x, helpinfo.MousePos.y));
			this.OnHelpRequested(helpEventArgs);
			if (!helpEventArgs.Handled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00032018 File Offset: 0x00030218
		private void WmInitMenuPopup(ref Message m)
		{
			ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			if (contextMenu != null && contextMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00032054 File Offset: 0x00030254
		private void WmMeasureItem(ref Message m)
		{
			if (m.WParam == IntPtr.Zero)
			{
				NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
				MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(measureitemstruct.itemData);
				if (menuItemFromItemData != null)
				{
					menuItemFromItemData.WmMeasureItem(ref m);
					return;
				}
			}
			else
			{
				this.WmOwnerDraw(ref m);
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000320A8 File Offset: 0x000302A8
		private void WmMenuChar(ref Message m)
		{
			Menu contextMenu = this.ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.WmMenuChar(ref m);
				m.Result != IntPtr.Zero;
				return;
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000320D8 File Offset: 0x000302D8
		private void WmMenuSelect(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			int num2 = NativeMethods.Util.HIWORD(m.WParam);
			IntPtr lparam = m.LParam;
			MenuItem menuItem = null;
			if ((num2 & 8192) == 0)
			{
				if ((num2 & 16) == 0)
				{
					Command commandFromID = Command.GetCommandFromID(num);
					if (commandFromID != null)
					{
						object target = commandFromID.Target;
						if (target != null && target is MenuItem.MenuItemData)
						{
							menuItem = ((MenuItem.MenuItemData)target).baseItem;
						}
					}
				}
				else
				{
					menuItem = this.GetMenuItemFromHandleId(lparam, num);
				}
			}
			if (menuItem != null)
			{
				menuItem.PerformSelect();
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00032160 File Offset: 0x00030360
		private void WmCreate(ref Message m)
		{
			this.DefWndProc(ref m);
			if (this.parent != null)
			{
				this.parent.UpdateChildZOrder(this);
			}
			this.UpdateBounds();
			this.OnHandleCreated(EventArgs.Empty);
			if (!this.GetStyle(ControlStyles.CacheText))
			{
				this.text = null;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000321B0 File Offset: 0x000303B0
		private void WmDestroy(ref Message m)
		{
			if (!this.RecreatingHandle && !this.Disposing && !this.IsDisposed && this.GetState(16384))
			{
				this.OnMouseLeave(EventArgs.Empty);
				this.UnhookMouseEvent();
			}
			if (this.SupportsUiaProviders)
			{
				this.ReleaseUiaProvider(this.Handle);
			}
			this.OnHandleDestroyed(EventArgs.Empty);
			if (!this.Disposing)
			{
				if (!this.RecreatingHandle)
				{
					this.SetState(1, false);
				}
			}
			else
			{
				this.SetState(2, false);
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003223B File Offset: 0x0003043B
		private void WmKeyChar(ref Message m)
		{
			if (this.ProcessKeyMessage(ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0003224E File Offset: 0x0003044E
		private void WmKillFocus(ref Message m)
		{
			this.WmImeKillFocus();
			this.DefWndProc(ref m);
			this.InvokeLostFocus(this, EventArgs.Empty);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003226C File Offset: 0x0003046C
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			MouseButtons mouseButtons = Control.MouseButtons;
			this.SetState(134217728, true);
			if (!this.GetStyle(ControlStyles.UserMouse))
			{
				this.DefWndProc(ref m);
				if (this.IsDisposed)
				{
					return;
				}
			}
			else if (button == MouseButtons.Left && this.GetStyle(ControlStyles.Selectable))
			{
				this.FocusInternal();
			}
			if (mouseButtons != Control.MouseButtons)
			{
				return;
			}
			if (!this.GetState2(16))
			{
				this.CaptureInternal = true;
			}
			if (mouseButtons != Control.MouseButtons)
			{
				return;
			}
			if (this.Enabled)
			{
				this.OnMouseDown(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00032313 File Offset: 0x00030513
		private void WmMouseEnter(ref Message m)
		{
			this.DefWndProc(ref m);
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				KeyboardToolTipStateMachine.Instance.NotifyAboutMouseEnter(this);
			}
			this.OnMouseEnter(EventArgs.Empty);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00032339 File Offset: 0x00030539
		private void WmMouseLeave(ref Message m)
		{
			this.DefWndProc(ref m);
			this.OnMouseLeave(EventArgs.Empty);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00032350 File Offset: 0x00030550
		private void WmDpiChangedBeforeParent(ref Message m)
		{
			this.DefWndProc(ref m);
			if (this.IsHandleCreated)
			{
				int num = this.deviceDpi;
				this.deviceDpi = (int)UnsafeNativeMethods.GetDpiForWindow(new HandleRef(this, this.HandleInternal));
				if (num != this.deviceDpi)
				{
					if (DpiHelper.EnableDpiChangedHighDpiImprovements)
					{
						Font font = (Font)this.Properties.GetObject(Control.PropFont);
						if (font != null)
						{
							float num2 = (float)this.deviceDpi / (float)num;
							this.Font = new Font(font.FontFamily, font.Size * num2, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
						}
					}
					this.RescaleConstantsForDpi(num, this.deviceDpi);
				}
			}
			this.OnDpiChangedBeforeParent(EventArgs.Empty);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0003240C File Offset: 0x0003060C
		private void WmDpiChangedAfterParent(ref Message m)
		{
			this.DefWndProc(ref m);
			uint dpiForWindow = UnsafeNativeMethods.GetDpiForWindow(new HandleRef(this, this.HandleInternal));
			this.OnDpiChangedAfterParent(EventArgs.Empty);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003243D File Offset: 0x0003063D
		private void WmMouseHover(ref Message m)
		{
			this.DefWndProc(ref m);
			this.OnMouseHover(EventArgs.Empty);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00032451 File Offset: 0x00030651
		private void WmMouseMove(ref Message m)
		{
			if (!this.GetStyle(ControlStyles.UserMouse))
			{
				this.DefWndProc(ref m);
			}
			this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00032490 File Offset: 0x00030690
		private void WmMouseUp(ref Message m, MouseButtons button, int clicks)
		{
			try
			{
				int num = NativeMethods.Util.SignedLOWORD(m.LParam);
				int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
				Point p = new Point(num, num2);
				p = this.PointToScreen(p);
				if (!this.GetStyle(ControlStyles.UserMouse))
				{
					this.DefWndProc(ref m);
				}
				else if (button == MouseButtons.Right)
				{
					this.SendMessage(123, this.Handle, NativeMethods.Util.MAKELPARAM(p.X, p.Y));
				}
				bool flag = false;
				if ((this.controlStyle & ControlStyles.StandardClick) == ControlStyles.StandardClick && this.GetState(134217728) && !this.IsDisposed && UnsafeNativeMethods.WindowFromPoint(p.X, p.Y) == this.Handle)
				{
					flag = true;
				}
				if (flag && !this.ValidationCancelled)
				{
					if (!this.GetState(67108864))
					{
						this.OnClick(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.OnMouseClick(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
					else
					{
						this.OnDoubleClick(new MouseEventArgs(button, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.OnMouseDoubleClick(new MouseEventArgs(button, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
				}
				this.OnMouseUp(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			}
			finally
			{
				this.SetState(67108864, false);
				this.SetState(134217728, false);
				this.SetState(268435456, false);
				this.CaptureInternal = false;
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00032670 File Offset: 0x00030870
		private void WmMouseWheel(ref Message m)
		{
			Point p = new Point(NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
			p = this.PointToClient(p);
			HandledMouseEventArgs handledMouseEventArgs = new HandledMouseEventArgs(MouseButtons.None, 0, p.X, p.Y, NativeMethods.Util.SignedHIWORD(m.WParam));
			this.OnMouseWheel(handledMouseEventArgs);
			m.Result = (IntPtr)(handledMouseEventArgs.Handled ? 0 : 1);
			if (!handledMouseEventArgs.Handled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000326F0 File Offset: 0x000308F0
		private void WmMove(ref Message m)
		{
			this.DefWndProc(ref m);
			this.UpdateBounds();
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00032700 File Offset: 0x00030900
		private unsafe void WmNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			if (!Control.ReflectMessageInternal(ptr->hwndFrom, ref m))
			{
				if (ptr->code == -521)
				{
					m.Result = UnsafeNativeMethods.SendMessage(new HandleRef(null, ptr->hwndFrom), 8192 + m.Msg, m.WParam, m.LParam);
					return;
				}
				if (ptr->code == -522)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(null, ptr->hwndFrom), 8192 + m.Msg, m.WParam, m.LParam);
				}
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000327A2 File Offset: 0x000309A2
		private void WmNotifyFormat(ref Message m)
		{
			if (!Control.ReflectMessageInternal(m.WParam, ref m))
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000327BC File Offset: 0x000309BC
		private void WmOwnerDraw(ref Message m)
		{
			bool flag = false;
			int num = (int)((long)m.WParam);
			IntPtr intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, m.HWnd), num);
			if (intPtr == IntPtr.Zero)
			{
				intPtr = (IntPtr)((long)num);
			}
			if (!Control.ReflectMessageInternal(intPtr, ref m))
			{
				IntPtr handleFromID = this.window.GetHandleFromID((short)NativeMethods.Util.LOWORD(m.WParam));
				if (handleFromID != IntPtr.Zero)
				{
					Control control = Control.FromHandleInternal(handleFromID);
					if (control != null)
					{
						m.Result = control.SendMessage(8192 + m.Msg, handleFromID, m.LParam);
						flag = true;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003286C File Offset: 0x00030A6C
		private void WmPaint(ref Message m)
		{
			bool flag = this.DoubleBuffered || (this.GetStyle(ControlStyles.AllPaintingInWmPaint) && this.DoubleBufferingEnabled);
			IntPtr handle = IntPtr.Zero;
			NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
			bool flag2 = false;
			try
			{
				IntPtr intPtr;
				Rectangle rectangle;
				if (m.WParam == IntPtr.Zero)
				{
					handle = this.Handle;
					intPtr = UnsafeNativeMethods.BeginPaint(new HandleRef(this, handle), ref paintstruct);
					if (intPtr == IntPtr.Zero)
					{
						return;
					}
					flag2 = true;
					rectangle = new Rectangle(paintstruct.rcPaint_left, paintstruct.rcPaint_top, paintstruct.rcPaint_right - paintstruct.rcPaint_left, paintstruct.rcPaint_bottom - paintstruct.rcPaint_top);
				}
				else
				{
					intPtr = m.WParam;
					rectangle = this.ClientRectangle;
				}
				if (!flag || (rectangle.Width > 0 && rectangle.Height > 0))
				{
					IntPtr intPtr2 = IntPtr.Zero;
					BufferedGraphics bufferedGraphics = null;
					PaintEventArgs paintEventArgs = null;
					GraphicsState graphicsState = null;
					try
					{
						if (flag || m.WParam == IntPtr.Zero)
						{
							intPtr2 = Control.SetUpPalette(intPtr, false, false);
						}
						if (flag)
						{
							try
							{
								bufferedGraphics = this.BufferContext.Allocate(intPtr, this.ClientRectangle);
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsCriticalException(ex) && !(ex is OutOfMemoryException))
								{
									throw;
								}
								flag = false;
							}
						}
						if (bufferedGraphics != null)
						{
							bufferedGraphics.Graphics.SetClip(rectangle);
							paintEventArgs = new PaintEventArgs(bufferedGraphics.Graphics, rectangle);
							graphicsState = paintEventArgs.Graphics.Save();
						}
						else
						{
							paintEventArgs = new PaintEventArgs(intPtr, rectangle);
						}
						using (paintEventArgs)
						{
							try
							{
								if ((m.WParam == IntPtr.Zero && this.GetStyle(ControlStyles.AllPaintingInWmPaint)) || flag)
								{
									this.PaintWithErrorHandling(paintEventArgs, 1);
								}
							}
							finally
							{
								if (graphicsState != null)
								{
									paintEventArgs.Graphics.Restore(graphicsState);
								}
								else
								{
									paintEventArgs.ResetGraphics();
								}
							}
							this.PaintWithErrorHandling(paintEventArgs, 2);
							if (bufferedGraphics != null)
							{
								bufferedGraphics.Render();
							}
						}
					}
					finally
					{
						if (intPtr2 != IntPtr.Zero)
						{
							SafeNativeMethods.SelectPalette(new HandleRef(null, intPtr), new HandleRef(null, intPtr2), 0);
						}
						if (bufferedGraphics != null)
						{
							bufferedGraphics.Dispose();
						}
					}
				}
			}
			finally
			{
				if (flag2)
				{
					UnsafeNativeMethods.EndPaint(new HandleRef(this, handle), ref paintstruct);
				}
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00032B18 File Offset: 0x00030D18
		private void WmPrintClient(ref Message m)
		{
			using (PaintEventArgs paintEventArgs = new Control.PrintPaintEventArgs(m, m.WParam, this.ClientRectangle))
			{
				this.OnPrint(paintEventArgs);
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00032B60 File Offset: 0x00030D60
		private void WmQueryNewPalette(ref Message m)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(new HandleRef(this, this.Handle));
			try
			{
				Control.SetUpPalette(dc, true, true);
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.Handle), new HandleRef(null, dc));
			}
			this.Invalidate(true);
			m.Result = (IntPtr)1;
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00032BD0 File Offset: 0x00030DD0
		private void WmSetCursor(ref Message m)
		{
			if (m.WParam == this.InternalHandle && NativeMethods.Util.LOWORD(m.LParam) == 1)
			{
				Cursor.CurrentInternal = this.Cursor;
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00032C08 File Offset: 0x00030E08
		private unsafe void WmWindowPosChanging(ref Message m)
		{
			if (this.IsActiveX)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
				bool flag = false;
				if ((ptr->flags & 2) == 0 && (ptr->x != this.Left || ptr->y != this.Top))
				{
					flag = true;
				}
				if ((ptr->flags & 1) == 0 && (ptr->cx != this.Width || ptr->cy != this.Height))
				{
					flag = true;
				}
				if (flag)
				{
					this.ActiveXUpdateBounds(ref ptr->x, ref ptr->y, ref ptr->cx, ref ptr->cy, ptr->flags);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00032CAC File Offset: 0x00030EAC
		private void WmParentNotify(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			IntPtr intPtr = IntPtr.Zero;
			if (num != 1)
			{
				if (num != 2)
				{
					intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(this, this.Handle), NativeMethods.Util.HIWORD(m.WParam));
				}
			}
			else
			{
				intPtr = m.LParam;
			}
			if (intPtr == IntPtr.Zero || !Control.ReflectMessageInternal(intPtr, ref m))
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00032D18 File Offset: 0x00030F18
		private void WmSetFocus(ref Message m)
		{
			this.WmImeSetFocus();
			if (!this.HostedInWin32DialogManager)
			{
				IContainerControl containerControlInternal = this.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					ContainerControl containerControl = containerControlInternal as ContainerControl;
					bool flag;
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
			this.DefWndProc(ref m);
			this.InvokeGotFocus(this, EventArgs.Empty);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00032D94 File Offset: 0x00030F94
		private void WmShowWindow(ref Message m)
		{
			this.DefWndProc(ref m);
			if ((this.state & 16) == 0)
			{
				bool flag = m.WParam != IntPtr.Zero;
				bool visible = this.Visible;
				if (flag)
				{
					bool value = this.GetState(2);
					this.SetState(2, true);
					bool flag2 = false;
					try
					{
						this.CreateControl();
						flag2 = true;
						goto IL_81;
					}
					finally
					{
						if (!flag2)
						{
							this.SetState(2, value);
						}
					}
				}
				bool flag3 = this.GetTopLevel();
				if (this.ParentInternal != null)
				{
					flag3 = this.ParentInternal.Visible;
				}
				if (flag3)
				{
					this.SetState(2, false);
				}
				IL_81:
				if (!this.GetState(536870912) && visible != flag)
				{
					this.OnVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00032E50 File Offset: 0x00031050
		private void WmUpdateUIState(ref Message m)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = (this.uiCuesState & 240) != 0;
			bool flag4 = (this.uiCuesState & 15) != 0;
			if (flag3)
			{
				flag = this.ShowKeyboardCues;
			}
			if (flag4)
			{
				flag2 = this.ShowFocusCues;
			}
			this.DefWndProc(ref m);
			int num = NativeMethods.Util.LOWORD(m.WParam);
			if (num == 3)
			{
				return;
			}
			UICues uicues = UICues.None;
			if ((NativeMethods.Util.HIWORD(m.WParam) & 2) != 0)
			{
				bool flag5 = num == 2;
				if (flag5 != flag || !flag3)
				{
					uicues |= UICues.ChangeKeyboard;
					this.uiCuesState &= -241;
					this.uiCuesState |= (flag5 ? 32 : 16);
				}
				if (flag5)
				{
					uicues |= UICues.ShowKeyboard;
				}
			}
			if ((NativeMethods.Util.HIWORD(m.WParam) & 1) != 0)
			{
				bool flag6 = num == 2;
				if (flag6 != flag2 || !flag4)
				{
					uicues |= UICues.ChangeFocus;
					this.uiCuesState &= -16;
					this.uiCuesState |= (flag6 ? 2 : 1);
				}
				if (flag6)
				{
					uicues |= UICues.ShowFocus;
				}
			}
			if ((uicues & UICues.Changed) != UICues.None)
			{
				this.OnChangeUICues(new UICuesEventArgs(uicues));
				this.Invalidate(true);
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00032F74 File Offset: 0x00031174
		private unsafe void WmWindowPosChanged(ref Message m)
		{
			this.DefWndProc(ref m);
			this.UpdateBounds();
			if (this.parent != null && UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.InternalHandle)) == this.parent.InternalHandle && (this.state & 256) == 0)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
				if ((ptr->flags & 4) == 0)
				{
					this.parent.UpdateChildControlIndex(this);
				}
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06000F0E RID: 3854 RVA: 0x00032FF0 File Offset: 0x000311F0
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void WndProc(ref Message m)
		{
			if ((this.controlStyle & ControlStyles.EnableNotifyMessage) == ControlStyles.EnableNotifyMessage)
			{
				this.OnNotifyMessage(m);
			}
			int msg = m.Msg;
			if (msg <= 261)
			{
				if (msg <= 47)
				{
					if (msg <= 20)
					{
						if (msg <= 15)
						{
							switch (msg)
							{
							case 1:
								this.WmCreate(ref m);
								return;
							case 2:
								this.WmDestroy(ref m);
								return;
							case 3:
								this.WmMove(ref m);
								return;
							case 4:
							case 5:
							case 6:
								goto IL_65D;
							case 7:
								this.WmSetFocus(ref m);
								return;
							case 8:
								this.WmKillFocus(ref m);
								return;
							default:
								if (msg != 15)
								{
									goto IL_65D;
								}
								if (this.GetStyle(ControlStyles.UserPaint))
								{
									this.WmPaint(ref m);
									return;
								}
								this.DefWndProc(ref m);
								return;
							}
						}
						else
						{
							if (msg == 16)
							{
								this.WmClose(ref m);
								return;
							}
							if (msg != 20)
							{
								goto IL_65D;
							}
							this.WmEraseBkgnd(ref m);
							return;
						}
					}
					else if (msg <= 25)
					{
						if (msg == 24)
						{
							this.WmShowWindow(ref m);
							return;
						}
						if (msg != 25)
						{
							goto IL_65D;
						}
					}
					else
					{
						if (msg == 32)
						{
							this.WmSetCursor(ref m);
							return;
						}
						switch (msg)
						{
						case 43:
							this.WmDrawItem(ref m);
							return;
						case 44:
							this.WmMeasureItem(ref m);
							return;
						case 45:
						case 46:
						case 47:
							goto IL_426;
						default:
							goto IL_65D;
						}
					}
				}
				else if (msg <= 71)
				{
					if (msg <= 61)
					{
						if (msg == 57)
						{
							goto IL_426;
						}
						if (msg != 61)
						{
							goto IL_65D;
						}
						this.WmGetObject(ref m);
						return;
					}
					else
					{
						if (msg == 70)
						{
							this.WmWindowPosChanging(ref m);
							return;
						}
						if (msg != 71)
						{
							goto IL_65D;
						}
						this.WmWindowPosChanged(ref m);
						return;
					}
				}
				else if (msg <= 123)
				{
					switch (msg)
					{
					case 78:
						this.WmNotify(ref m);
						return;
					case 79:
					case 82:
					case 84:
						goto IL_65D;
					case 80:
						this.WmInputLangChangeRequest(ref m);
						return;
					case 81:
						this.WmInputLangChange(ref m);
						return;
					case 83:
						this.WmHelp(ref m);
						return;
					case 85:
						this.WmNotifyFormat(ref m);
						return;
					default:
						if (msg != 123)
						{
							goto IL_65D;
						}
						this.WmContextMenu(ref m);
						return;
					}
				}
				else
				{
					if (msg == 126)
					{
						this.WmDisplayChange(ref m);
						return;
					}
					if (msg - 256 > 2 && msg - 260 > 1)
					{
						goto IL_65D;
					}
					this.WmKeyChar(ref m);
					return;
				}
			}
			else if (msg <= 646)
			{
				if (msg <= 296)
				{
					if (msg <= 287)
					{
						switch (msg)
						{
						case 269:
							this.WmImeStartComposition(ref m);
							return;
						case 270:
							this.WmImeEndComposition(ref m);
							return;
						case 271:
						case 272:
						case 275:
						case 278:
							goto IL_65D;
						case 273:
							this.WmCommand(ref m);
							return;
						case 274:
							if (((int)((long)m.WParam) & 65520) == 61696 && ToolStripManager.ProcessMenuKey(ref m))
							{
								m.Result = IntPtr.Zero;
								return;
							}
							this.DefWndProc(ref m);
							return;
						case 276:
						case 277:
							goto IL_426;
						case 279:
							this.WmInitMenuPopup(ref m);
							return;
						default:
							if (msg != 287)
							{
								goto IL_65D;
							}
							this.WmMenuSelect(ref m);
							return;
						}
					}
					else
					{
						if (msg == 288)
						{
							this.WmMenuChar(ref m);
							return;
						}
						if (msg != 296)
						{
							goto IL_65D;
						}
						this.WmUpdateUIState(ref m);
						return;
					}
				}
				else if (msg <= 533)
				{
					if (msg - 306 > 6)
					{
						switch (msg)
						{
						case 512:
							this.WmMouseMove(ref m);
							return;
						case 513:
							this.WmMouseDown(ref m, MouseButtons.Left, 1);
							return;
						case 514:
							this.WmMouseUp(ref m, MouseButtons.Left, 1);
							return;
						case 515:
							this.WmMouseDown(ref m, MouseButtons.Left, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 516:
							this.WmMouseDown(ref m, MouseButtons.Right, 1);
							return;
						case 517:
							this.WmMouseUp(ref m, MouseButtons.Right, 1);
							return;
						case 518:
							this.WmMouseDown(ref m, MouseButtons.Right, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 519:
							this.WmMouseDown(ref m, MouseButtons.Middle, 1);
							return;
						case 520:
							this.WmMouseUp(ref m, MouseButtons.Middle, 1);
							return;
						case 521:
							this.WmMouseDown(ref m, MouseButtons.Middle, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 522:
							this.WmMouseWheel(ref m);
							return;
						case 523:
							this.WmMouseDown(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 1);
							return;
						case 524:
							this.WmMouseUp(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 1);
							return;
						case 525:
							this.WmMouseDown(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 526:
						case 527:
						case 529:
						case 531:
						case 532:
							goto IL_65D;
						case 528:
							this.WmParentNotify(ref m);
							return;
						case 530:
							this.WmExitMenuLoop(ref m);
							return;
						case 533:
							this.WmCaptureChanged(ref m);
							return;
						default:
							goto IL_65D;
						}
					}
				}
				else
				{
					if (msg == 642)
					{
						this.WmImeNotify(ref m);
						return;
					}
					if (msg != 646)
					{
						goto IL_65D;
					}
					this.WmImeChar(ref m);
					return;
				}
			}
			else if (msg <= 739)
			{
				if (msg <= 675)
				{
					if (msg == 673)
					{
						this.WmMouseHover(ref m);
						return;
					}
					if (msg != 675)
					{
						goto IL_65D;
					}
					this.WmMouseLeave(ref m);
					return;
				}
				else if (msg != 738)
				{
					if (msg != 739)
					{
						goto IL_65D;
					}
					if (DpiHelper.EnableDpiChangedMessageHandling)
					{
						this.WmDpiChangedAfterParent(ref m);
						m.Result = IntPtr.Zero;
						return;
					}
					return;
				}
				else
				{
					if (DpiHelper.EnableDpiChangedMessageHandling)
					{
						this.WmDpiChangedBeforeParent(ref m);
						m.Result = IntPtr.Zero;
						return;
					}
					return;
				}
			}
			else if (msg <= 792)
			{
				if (msg == 783)
				{
					this.WmQueryNewPalette(ref m);
					return;
				}
				if (msg != 792)
				{
					goto IL_65D;
				}
				if (this.GetStyle(ControlStyles.UserPaint))
				{
					this.WmPrintClient(ref m);
					return;
				}
				this.DefWndProc(ref m);
				return;
			}
			else if (msg != 8217)
			{
				if (msg == 8277)
				{
					m.Result = (IntPtr)((Marshal.SystemDefaultCharSize == 1) ? 1 : 2);
					return;
				}
				if (msg - 8498 > 6)
				{
					goto IL_65D;
				}
			}
			this.WmCtlColorControl(ref m);
			return;
			IL_426:
			if (!Control.ReflectMessageInternal(m.LParam, ref m))
			{
				this.DefWndProc(ref m);
				return;
			}
			return;
			IL_65D:
			if (m.Msg == Control.threadCallbackMessage && m.Msg != 0)
			{
				this.InvokeMarshaledCallbacks();
				return;
			}
			if (m.Msg == Control.WM_GETCONTROLNAME)
			{
				this.WmGetControlName(ref m);
				return;
			}
			if (m.Msg == Control.WM_GETCONTROLTYPE)
			{
				this.WmGetControlType(ref m);
				return;
			}
			if (Control.mouseWheelRoutingNeeded && m.Msg == Control.mouseWheelMessage)
			{
				Keys keys = Keys.None;
				keys |= ((UnsafeNativeMethods.GetKeyState(17) < 0) ? Keys.Back : Keys.None);
				keys |= ((UnsafeNativeMethods.GetKeyState(16) < 0) ? Keys.MButton : Keys.None);
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				if (focus == IntPtr.Zero)
				{
					this.SendMessage(m.Msg, (IntPtr)((int)((long)m.WParam) << 16 | (int)keys), m.LParam);
				}
				else
				{
					IntPtr value = IntPtr.Zero;
					IntPtr desktopWindow = UnsafeNativeMethods.GetDesktopWindow();
					while (value == IntPtr.Zero && focus != IntPtr.Zero && focus != desktopWindow)
					{
						value = UnsafeNativeMethods.SendMessage(new HandleRef(null, focus), 522, (int)((long)m.WParam) << 16 | (int)keys, m.LParam);
						focus = UnsafeNativeMethods.GetParent(new HandleRef(null, focus));
					}
				}
			}
			if (m.Msg == NativeMethods.WM_MOUSEENTER)
			{
				this.WmMouseEnter(ref m);
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000337A1 File Offset: 0x000319A1
		private void WndProcException(Exception e)
		{
			Application.OnThreadException(e);
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x000337AC File Offset: 0x000319AC
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection == null)
				{
					return ArrangedElementCollection.Empty;
				}
				return controlCollection;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x000337D9 File Offset: 0x000319D9
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ParentInternal;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x000337E1 File Offset: 0x000319E1
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.GetState(2);
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x000337EA File Offset: 0x000319EA
		void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string affectedProperty)
		{
			this.PerformLayout(new LayoutEventArgs(affectedElement, affectedProperty));
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x000337F9 File Offset: 0x000319F9
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00033804 File Offset: 0x00031A04
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			ISite site = this.Site;
			IComponentChangeService componentChangeService = null;
			PropertyDescriptor propertyDescriptor = null;
			PropertyDescriptor propertyDescriptor2 = null;
			bool flag = false;
			bool flag2 = false;
			if (site != null && site.DesignMode)
			{
				componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(this)[PropertyNames.Size];
					propertyDescriptor2 = TypeDescriptor.GetProperties(this)[PropertyNames.Location];
					try
					{
						if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly && (bounds.Width != this.Width || bounds.Height != this.Height))
						{
							if (!(site is INestedSite))
							{
								componentChangeService.OnComponentChanging(this, propertyDescriptor);
							}
							flag = true;
						}
						if (propertyDescriptor2 != null && !propertyDescriptor2.IsReadOnly && (bounds.X != this.x || bounds.Y != this.y))
						{
							if (!(site is INestedSite))
							{
								componentChangeService.OnComponentChanging(this, propertyDescriptor2);
							}
							flag2 = true;
						}
					}
					catch (InvalidOperationException)
					{
					}
				}
			}
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
			if (site != null && componentChangeService != null)
			{
				try
				{
					if (flag)
					{
						componentChangeService.OnComponentChanged(this, propertyDescriptor, null, null);
					}
					if (flag2)
					{
						componentChangeService.OnComponentChanged(this, propertyDescriptor2, null, null);
					}
				}
				catch (InvalidOperationException)
				{
				}
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool SupportsUiaProviders
		{
			get
			{
				return false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragEnter" /> event.</summary>
		/// <param name="drgEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000F17 RID: 3863 RVA: 0x00033958 File Offset: 0x00031B58
		void IDropTarget.OnDragEnter(DragEventArgs drgEvent)
		{
			this.OnDragEnter(drgEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.</summary>
		/// <param name="drgEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000F18 RID: 3864 RVA: 0x00033961 File Offset: 0x00031B61
		void IDropTarget.OnDragOver(DragEventArgs drgEvent)
		{
			this.OnDragOver(drgEvent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000F19 RID: 3865 RVA: 0x0003396A File Offset: 0x00031B6A
		void IDropTarget.OnDragLeave(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.</summary>
		/// <param name="drgEvent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
		// Token: 0x06000F1A RID: 3866 RVA: 0x00033973 File Offset: 0x00031B73
		void IDropTarget.OnDragDrop(DragEventArgs drgEvent)
		{
			this.OnDragDrop(drgEvent);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0003397C File Offset: 0x00031B7C
		void ISupportOleDropSource.OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEventArgs)
		{
			this.OnGiveFeedback(giveFeedbackEventArgs);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00033985 File Offset: 0x00031B85
		void ISupportOleDropSource.OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEventArgs)
		{
			this.OnQueryContinueDrag(queryContinueDragEventArgs);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00033990 File Offset: 0x00031B90
		int UnsafeNativeMethods.IOleControl.GetControlInfo(NativeMethods.tagCONTROLINFO pCI)
		{
			pCI.cb = Marshal.SizeOf(typeof(NativeMethods.tagCONTROLINFO));
			pCI.hAccel = IntPtr.Zero;
			pCI.cAccel = 0;
			pCI.dwFlags = 0;
			if (this.IsInputKey(Keys.Return))
			{
				pCI.dwFlags |= 1;
			}
			if (this.IsInputKey(Keys.Escape))
			{
				pCI.dwFlags |= 2;
			}
			this.ActiveXInstance.GetControlInfo(pCI);
			return 0;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00033A0C File Offset: 0x00031C0C
		int UnsafeNativeMethods.IOleControl.OnMnemonic(ref NativeMethods.MSG pMsg)
		{
			bool flag = this.ProcessMnemonic((char)((int)pMsg.wParam));
			return 0;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00033A2D File Offset: 0x00031C2D
		int UnsafeNativeMethods.IOleControl.OnAmbientPropertyChange(int dispID)
		{
			this.ActiveXInstance.OnAmbientPropertyChange(dispID);
			return 0;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00033A3C File Offset: 0x00031C3C
		int UnsafeNativeMethods.IOleControl.FreezeEvents(int bFreeze)
		{
			this.ActiveXInstance.EventsFrozen = (bFreeze != 0);
			return 0;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00033A4E File Offset: 0x00031C4E
		int UnsafeNativeMethods.IOleInPlaceActiveObject.GetWindow(out IntPtr hwnd)
		{
			return ((UnsafeNativeMethods.IOleInPlaceObject)this).GetWindow(out hwnd);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00033A57 File Offset: 0x00031C57
		void UnsafeNativeMethods.IOleInPlaceActiveObject.ContextSensitiveHelp(int fEnterMode)
		{
			((UnsafeNativeMethods.IOleInPlaceObject)this).ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00033A60 File Offset: 0x00031C60
		int UnsafeNativeMethods.IOleInPlaceActiveObject.TranslateAccelerator(ref NativeMethods.MSG lpmsg)
		{
			return this.ActiveXInstance.TranslateAccelerator(ref lpmsg);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00033A6E File Offset: 0x00031C6E
		void UnsafeNativeMethods.IOleInPlaceActiveObject.OnFrameWindowActivate(bool fActivate)
		{
			this.OnFrameWindowActivate(fActivate);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00033A77 File Offset: 0x00031C77
		void UnsafeNativeMethods.IOleInPlaceActiveObject.OnDocWindowActivate(int fActivate)
		{
			this.ActiveXInstance.OnDocWindowActivate(fActivate);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IOleInPlaceActiveObject.ResizeBorder(NativeMethods.COMRECT prcBorder, UnsafeNativeMethods.IOleInPlaceUIWindow pUIWindow, bool fFrameWindow)
		{
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IOleInPlaceActiveObject.EnableModeless(int fEnable)
		{
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00033A88 File Offset: 0x00031C88
		int UnsafeNativeMethods.IOleInPlaceObject.GetWindow(out IntPtr hwnd)
		{
			return this.ActiveXInstance.GetWindow(out hwnd);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00033AA3 File Offset: 0x00031CA3
		void UnsafeNativeMethods.IOleInPlaceObject.ContextSensitiveHelp(int fEnterMode)
		{
			if (fEnterMode != 0)
			{
				this.OnHelpRequested(new HelpEventArgs(Control.MousePosition));
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00033AB8 File Offset: 0x00031CB8
		void UnsafeNativeMethods.IOleInPlaceObject.InPlaceDeactivate()
		{
			this.ActiveXInstance.InPlaceDeactivate();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00033AC5 File Offset: 0x00031CC5
		int UnsafeNativeMethods.IOleInPlaceObject.UIDeactivate()
		{
			return this.ActiveXInstance.UIDeactivate();
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00033AD2 File Offset: 0x00031CD2
		void UnsafeNativeMethods.IOleInPlaceObject.SetObjectRects(NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect)
		{
			this.ActiveXInstance.SetObjectRects(lprcPosRect, lprcClipRect);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IOleInPlaceObject.ReactivateAndUndo()
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00033AE1 File Offset: 0x00031CE1
		int UnsafeNativeMethods.IOleObject.SetClientSite(UnsafeNativeMethods.IOleClientSite pClientSite)
		{
			this.ActiveXInstance.SetClientSite(pClientSite);
			return 0;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00033AF0 File Offset: 0x00031CF0
		UnsafeNativeMethods.IOleClientSite UnsafeNativeMethods.IOleObject.GetClientSite()
		{
			return this.ActiveXInstance.GetClientSite();
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleObject.SetHostNames(string szContainerApp, string szContainerObj)
		{
			return 0;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00033AFD File Offset: 0x00031CFD
		int UnsafeNativeMethods.IOleObject.Close(int dwSaveOption)
		{
			this.ActiveXInstance.Close(dwSaveOption);
			return 0;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleObject.SetMoniker(int dwWhichMoniker, object pmk)
		{
			return -2147467263;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00033B13 File Offset: 0x00031D13
		int UnsafeNativeMethods.IOleObject.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleObject.InitFromData(IDataObject pDataObject, int fCreation, int dwReserved)
		{
			return -2147467263;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0000E2A3 File Offset: 0x0000C4A3
		int UnsafeNativeMethods.IOleObject.GetClipboardData(int dwReserved, out IDataObject data)
		{
			data = null;
			return -2147467263;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00033B20 File Offset: 0x00031D20
		int UnsafeNativeMethods.IOleObject.DoVerb(int iVerb, IntPtr lpmsg, UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, NativeMethods.COMRECT lprcPosRect)
		{
			short num = (short)iVerb;
			iVerb = (int)num;
			try
			{
				this.ActiveXInstance.DoVerb(iVerb, lpmsg, pActiveSite, lindex, hwndParent, lprcPosRect);
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
			}
			return 0;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00033B6C File Offset: 0x00031D6C
		int UnsafeNativeMethods.IOleObject.EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e)
		{
			return Control.ActiveXImpl.EnumVerbs(out e);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleObject.OleUpdate()
		{
			return 0;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleObject.IsUpToDate()
		{
			return 0;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00033B74 File Offset: 0x00031D74
		int UnsafeNativeMethods.IOleObject.GetUserClassID(ref Guid pClsid)
		{
			pClsid = base.GetType().GUID;
			return 0;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00033B88 File Offset: 0x00031D88
		int UnsafeNativeMethods.IOleObject.GetUserType(int dwFormOfType, out string userType)
		{
			if (dwFormOfType == 1)
			{
				userType = base.GetType().FullName;
			}
			else
			{
				userType = base.GetType().Name;
			}
			return 0;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00033BAB File Offset: 0x00031DAB
		int UnsafeNativeMethods.IOleObject.SetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.SetExtent(dwDrawAspect, pSizel);
			return 0;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00033BBB File Offset: 0x00031DBB
		int UnsafeNativeMethods.IOleObject.GetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.GetExtent(dwDrawAspect, pSizel);
			return 0;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00033BCB File Offset: 0x00031DCB
		int UnsafeNativeMethods.IOleObject.Advise(IAdviseSink pAdvSink, out int cookie)
		{
			cookie = this.ActiveXInstance.Advise(pAdvSink);
			return 0;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00033BDC File Offset: 0x00031DDC
		int UnsafeNativeMethods.IOleObject.Unadvise(int dwConnection)
		{
			this.ActiveXInstance.Unadvise(dwConnection);
			return 0;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00033BEB File Offset: 0x00031DEB
		int UnsafeNativeMethods.IOleObject.EnumAdvise(out IEnumSTATDATA e)
		{
			e = null;
			return -2147467263;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00033BF8 File Offset: 0x00031DF8
		int UnsafeNativeMethods.IOleObject.GetMiscStatus(int dwAspect, out int cookie)
		{
			if ((dwAspect & 1) != 0)
			{
				int num = 131456;
				if (this.GetStyle(ControlStyles.ResizeRedraw))
				{
					num |= 1;
				}
				if (this is IButtonControl)
				{
					num |= 4096;
				}
				cookie = num;
				return 0;
			}
			cookie = 0;
			return -2147221397;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleObject.SetColorScheme(NativeMethods.tagLOGPALETTE pLogpal)
		{
			return 0;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00033A4E File Offset: 0x00031C4E
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			return ((UnsafeNativeMethods.IOleInPlaceObject)this).GetWindow(out hwnd);
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00033A57 File Offset: 0x00031C57
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			((UnsafeNativeMethods.IOleInPlaceObject)this).ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00033C3D File Offset: 0x00031E3D
		void UnsafeNativeMethods.IPersist.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistPropertyBag.InitNew()
		{
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00033C3D File Offset: 0x00031E3D
		void UnsafeNativeMethods.IPersistPropertyBag.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00033C50 File Offset: 0x00031E50
		void UnsafeNativeMethods.IPersistPropertyBag.Load(UnsafeNativeMethods.IPropertyBag pPropBag, UnsafeNativeMethods.IErrorLog pErrorLog)
		{
			this.ActiveXInstance.Load(pPropBag, pErrorLog);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00033C5F File Offset: 0x00031E5F
		void UnsafeNativeMethods.IPersistPropertyBag.Save(UnsafeNativeMethods.IPropertyBag pPropBag, bool fClearDirty, bool fSaveAllProperties)
		{
			this.ActiveXInstance.Save(pPropBag, fClearDirty, fSaveAllProperties);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00033C3D File Offset: 0x00031E3D
		void UnsafeNativeMethods.IPersistStorage.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00033C6F File Offset: 0x00031E6F
		int UnsafeNativeMethods.IPersistStorage.IsDirty()
		{
			return this.ActiveXInstance.IsDirty();
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistStorage.InitNew(UnsafeNativeMethods.IStorage pstg)
		{
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00033C7C File Offset: 0x00031E7C
		int UnsafeNativeMethods.IPersistStorage.Load(UnsafeNativeMethods.IStorage pstg)
		{
			this.ActiveXInstance.Load(pstg);
			return 0;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00033C8B File Offset: 0x00031E8B
		void UnsafeNativeMethods.IPersistStorage.Save(UnsafeNativeMethods.IStorage pstg, bool fSameAsLoad)
		{
			this.ActiveXInstance.Save(pstg, fSameAsLoad);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistStorage.SaveCompleted(UnsafeNativeMethods.IStorage pStgNew)
		{
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistStorage.HandsOffStorage()
		{
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00033C3D File Offset: 0x00031E3D
		void UnsafeNativeMethods.IPersistStreamInit.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00033C6F File Offset: 0x00031E6F
		int UnsafeNativeMethods.IPersistStreamInit.IsDirty()
		{
			return this.ActiveXInstance.IsDirty();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00033C9A File Offset: 0x00031E9A
		void UnsafeNativeMethods.IPersistStreamInit.Load(UnsafeNativeMethods.IStream pstm)
		{
			this.ActiveXInstance.Load(pstm);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00033CA8 File Offset: 0x00031EA8
		void UnsafeNativeMethods.IPersistStreamInit.Save(UnsafeNativeMethods.IStream pstm, bool fClearDirty)
		{
			this.ActiveXInstance.Save(pstm, fClearDirty);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistStreamInit.GetSizeMax(long pcbSize)
		{
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0000701A File Offset: 0x0000521A
		void UnsafeNativeMethods.IPersistStreamInit.InitNew()
		{
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00033CB7 File Offset: 0x00031EB7
		void UnsafeNativeMethods.IQuickActivate.QuickActivate(UnsafeNativeMethods.tagQACONTAINER pQaContainer, UnsafeNativeMethods.tagQACONTROL pQaControl)
		{
			this.ActiveXInstance.QuickActivate(pQaContainer, pQaControl);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00033CC6 File Offset: 0x00031EC6
		void UnsafeNativeMethods.IQuickActivate.SetContentExtent(NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.SetExtent(1, pSizel);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00033CD5 File Offset: 0x00031ED5
		void UnsafeNativeMethods.IQuickActivate.GetContentExtent(NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.GetExtent(1, pSizel);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00033CE4 File Offset: 0x00031EE4
		int UnsafeNativeMethods.IViewObject.Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT lprcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
		{
			try
			{
				this.ActiveXInstance.Draw(dwDrawAspect, lindex, pvAspect, ptd, hdcTargetDev, hdcDraw, lprcBounds, lprcWBounds, pfnContinue, dwContinue);
			}
			catch (ExternalException ex)
			{
				return ex.ErrorCode;
			}
			finally
			{
			}
			return 0;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject.GetColorSet(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, NativeMethods.tagLOGPALETTE ppColorSet)
		{
			return -2147467263;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject.Freeze(int dwDrawAspect, int lindex, IntPtr pvAspect, IntPtr pdwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject.Unfreeze(int dwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00033D3C File Offset: 0x00031F3C
		void UnsafeNativeMethods.IViewObject.SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
		{
			this.ActiveXInstance.SetAdvise(aspects, advf, pAdvSink);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00033D4C File Offset: 0x00031F4C
		void UnsafeNativeMethods.IViewObject.GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
		{
			this.ActiveXInstance.GetAdvise(paspects, padvf, pAdvSink);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00033D5C File Offset: 0x00031F5C
		void UnsafeNativeMethods.IViewObject2.Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT lprcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
		{
			this.ActiveXInstance.Draw(dwDrawAspect, lindex, pvAspect, ptd, hdcTargetDev, hdcDraw, lprcBounds, lprcWBounds, pfnContinue, dwContinue);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject2.GetColorSet(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, NativeMethods.tagLOGPALETTE ppColorSet)
		{
			return -2147467263;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject2.Freeze(int dwDrawAspect, int lindex, IntPtr pvAspect, IntPtr pdwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IViewObject2.Unfreeze(int dwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00033D3C File Offset: 0x00031F3C
		void UnsafeNativeMethods.IViewObject2.SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
		{
			this.ActiveXInstance.SetAdvise(aspects, advf, pAdvSink);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00033D4C File Offset: 0x00031F4C
		void UnsafeNativeMethods.IViewObject2.GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
		{
			this.ActiveXInstance.GetAdvise(paspects, padvf, pAdvSink);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00033D85 File Offset: 0x00031F85
		void UnsafeNativeMethods.IViewObject2.GetExtent(int dwDrawAspect, int lindex, NativeMethods.tagDVTARGETDEVICE ptd, NativeMethods.tagSIZEL lpsizel)
		{
			((UnsafeNativeMethods.IOleObject)this).GetExtent(dwDrawAspect, lpsizel);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00033D94 File Offset: 0x00031F94
		bool IKeyboardToolTip.CanShowToolTipsNow()
		{
			IKeyboardToolTip toolStripControlHost = this.ToolStripControlHost;
			return this.IsHandleCreated && this.Visible && (toolStripControlHost == null || toolStripControlHost.CanShowToolTipsNow());
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00033DC5 File Offset: 0x00031FC5
		Rectangle IKeyboardToolTip.GetNativeScreenRectangle()
		{
			return this.GetToolNativeScreenRectangle();
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00033DD0 File Offset: 0x00031FD0
		IList<Rectangle> IKeyboardToolTip.GetNeighboringToolsRectangles()
		{
			IKeyboardToolTip toolStripControlHost = this.ToolStripControlHost;
			if (toolStripControlHost == null)
			{
				return this.GetOwnNeighboringToolsRectangles();
			}
			return toolStripControlHost.GetNeighboringToolsRectangles();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00033DF4 File Offset: 0x00031FF4
		bool IKeyboardToolTip.IsHoveredWithMouse()
		{
			return this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition));
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00033E1C File Offset: 0x0003201C
		bool IKeyboardToolTip.HasRtlModeEnabled()
		{
			Control topLevelControlInternal = this.TopLevelControlInternal;
			return topLevelControlInternal != null && topLevelControlInternal.RightToLeft == RightToLeft.Yes && !this.IsMirrored;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00033E48 File Offset: 0x00032048
		bool IKeyboardToolTip.AllowsToolTip()
		{
			IKeyboardToolTip toolStripControlHost = this.ToolStripControlHost;
			return (toolStripControlHost == null || toolStripControlHost.AllowsToolTip()) && this.AllowsKeyboardToolTip();
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000069BD File Offset: 0x00004BBD
		IWin32Window IKeyboardToolTip.GetOwnerWindow()
		{
			return this;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00033E6F File Offset: 0x0003206F
		void IKeyboardToolTip.OnHooked(ToolTip toolTip)
		{
			this.OnKeyboardToolTipHook(toolTip);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00033E78 File Offset: 0x00032078
		void IKeyboardToolTip.OnUnhooked(ToolTip toolTip)
		{
			this.OnKeyboardToolTipUnhook(toolTip);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00033E84 File Offset: 0x00032084
		string IKeyboardToolTip.GetCaptionForTool(ToolTip toolTip)
		{
			IKeyboardToolTip toolStripControlHost = this.ToolStripControlHost;
			if (toolStripControlHost == null)
			{
				return toolTip.GetCaptionForTool(this);
			}
			return toolStripControlHost.GetCaptionForTool(toolTip);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00033EAC File Offset: 0x000320AC
		bool IKeyboardToolTip.ShowsOwnToolTip()
		{
			IKeyboardToolTip toolStripControlHost = this.ToolStripControlHost;
			return (toolStripControlHost == null || toolStripControlHost.ShowsOwnToolTip()) && this.ShowsOwnKeyboardToolTip();
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00033ED3 File Offset: 0x000320D3
		bool IKeyboardToolTip.IsBeingTabbedTo()
		{
			return Control.AreCommonNavigationalKeysDown();
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00033EDA File Offset: 0x000320DA
		bool IKeyboardToolTip.AllowsChildrenToShowToolTips()
		{
			return this.AllowsChildrenToShowToolTips();
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00033EE4 File Offset: 0x000320E4
		private IList<Rectangle> GetOwnNeighboringToolsRectangles()
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				Control[] array = new Control[]
				{
					parentInternal.GetNextSelectableControl(this, true, true, true, false),
					parentInternal.GetNextSelectableControl(this, false, true, true, false),
					parentInternal.GetNextSelectableControl(this, true, false, false, true),
					parentInternal.GetNextSelectableControl(this, false, false, false, true)
				};
				List<Rectangle> list = new List<Rectangle>(4);
				foreach (Control control in array)
				{
					if (control != null && control.IsHandleCreated)
					{
						list.Add(((IKeyboardToolTip)control).GetNativeScreenRectangle());
					}
				}
				return list;
			}
			return new Rectangle[0];
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool ShowsOwnKeyboardToolTip()
		{
			return true;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnKeyboardToolTipHook(ToolTip toolTip)
		{
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnKeyboardToolTipUnhook(ToolTip toolTip)
		{
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00033F80 File Offset: 0x00032180
		internal virtual Rectangle GetToolNativeScreenRectangle()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool AllowsKeyboardToolTip()
		{
			return true;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00033FC6 File Offset: 0x000321C6
		private static bool IsKeyDown(Keys key)
		{
			return (Control.tempKeyboardStateArray[(int)key] & 128) > 0;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00033FD8 File Offset: 0x000321D8
		internal static bool AreCommonNavigationalKeysDown()
		{
			if (Control.tempKeyboardStateArray == null)
			{
				Control.tempKeyboardStateArray = new byte[256];
			}
			UnsafeNativeMethods.GetKeyboardState(Control.tempKeyboardStateArray);
			return Control.IsKeyDown(Keys.Tab) || Control.IsKeyDown(Keys.Up) || Control.IsKeyDown(Keys.Down) || Control.IsKeyDown(Keys.Left) || Control.IsKeyDown(Keys.Right) || Control.IsKeyDown(Keys.Menu) || Control.IsKeyDown(Keys.F10) || Control.IsKeyDown(Keys.Escape);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00034050 File Offset: 0x00032250
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x0003406C File Offset: 0x0003226C
		internal ToolStripControlHost ToolStripControlHost
		{
			get
			{
				ToolStripControlHost result;
				this.toolStripControlHostReference.TryGetTarget(out result);
				return result;
			}
			set
			{
				this.toolStripControlHostReference.SetTarget(value);
			}
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool AllowsChildrenToShowToolTips()
		{
			return true;
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0003407C File Offset: 0x0003227C
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x000340C0 File Offset: 0x000322C0
		internal ImeMode CachedImeMode
		{
			get
			{
				bool flag;
				ImeMode imeMode = (ImeMode)this.Properties.GetInteger(Control.PropImeMode, out flag);
				if (!flag)
				{
					imeMode = this.DefaultImeMode;
				}
				if (imeMode == ImeMode.Inherit)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						imeMode = parentInternal.CachedImeMode;
					}
					else
					{
						imeMode = ImeMode.NoControl;
					}
				}
				return imeMode;
			}
			set
			{
				this.Properties.SetInteger(Control.PropImeMode, (int)value);
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property can be set to an active value, to enable IME support.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x000340D3 File Offset: 0x000322D3
		protected virtual bool CanEnableIme
		{
			get
			{
				return this.ImeSupported;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000340DB File Offset: 0x000322DB
		internal ImeMode CurrentImeContextMode
		{
			get
			{
				if (this.IsHandleCreated)
				{
					return ImeContext.GetImeMode(this.Handle);
				}
				return ImeMode.Inherit;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0000DE5F File Offset: 0x0000C05F
		protected virtual ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Inherit;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x000340F4 File Offset: 0x000322F4
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00034115 File Offset: 0x00032315
		internal int DisableImeModeChangedCount
		{
			get
			{
				bool flag;
				return this.Properties.GetInteger(Control.PropDisableImeModeChangedCount, out flag);
			}
			set
			{
				this.Properties.SetInteger(Control.PropDisableImeModeChangedCount, value);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00034128 File Offset: 0x00032328
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0003413C File Offset: 0x0003233C
		private static bool IgnoreWmImeNotify
		{
			get
			{
				return Control.ignoreWmImeNotify;
			}
			set
			{
				Control.ignoreWmImeNotify = value;
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode of the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values. The default is <see cref="F:System.Windows.Forms.ImeMode.Inherit" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ImeMode" /> enumeration values. </exception>
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00034144 File Offset: 0x00032344
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x00034160 File Offset: 0x00032360
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[AmbientValue(ImeMode.Inherit)]
		[SRDescription("ControlIMEModeDescr")]
		public ImeMode ImeMode
		{
			get
			{
				ImeMode imeMode = this.ImeModeBase;
				if (imeMode == ImeMode.OnHalf)
				{
					imeMode = ImeMode.On;
				}
				return imeMode;
			}
			set
			{
				this.ImeModeBase = value;
			}
		}

		/// <summary>Gets or sets the IME mode of a control.</summary>
		/// <returns>The IME mode of the control.</returns>
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0003416C File Offset: 0x0003236C
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00034184 File Offset: 0x00032384
		protected virtual ImeMode ImeModeBase
		{
			get
			{
				return this.CachedImeMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 12))
				{
					throw new InvalidEnumArgumentException("ImeMode", (int)value, typeof(ImeMode));
				}
				ImeMode cachedImeMode = this.CachedImeMode;
				this.CachedImeMode = value;
				if (cachedImeMode != value)
				{
					Control control = null;
					if (!base.DesignMode && ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
					{
						if (this.Focused)
						{
							control = this;
						}
						else if (this.ContainsFocus)
						{
							control = Control.FromChildHandleInternal(UnsafeNativeMethods.GetFocus());
						}
						if (control != null && control.CanEnableIme)
						{
							int disableImeModeChangedCount = this.DisableImeModeChangedCount;
							this.DisableImeModeChangedCount = disableImeModeChangedCount + 1;
							try
							{
								control.UpdateImeContextMode();
							}
							finally
							{
								disableImeModeChangedCount = this.DisableImeModeChangedCount;
								this.DisableImeModeChangedCount = disableImeModeChangedCount - 1;
							}
						}
					}
					this.VerifyImeModeChanged(cachedImeMode, this.CachedImeMode);
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00034250 File Offset: 0x00032450
		private bool ImeSupported
		{
			get
			{
				return this.DefaultImeMode != ImeMode.Disable;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property has changed.</summary>
		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06000F8D RID: 3981 RVA: 0x0003425E File Offset: 0x0003245E
		// (remove) Token: 0x06000F8E RID: 3982 RVA: 0x00034271 File Offset: 0x00032471
		[WinCategory("Behavior")]
		[SRDescription("ControlOnImeModeChangedDescr")]
		public event EventHandler ImeModeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventImeModeChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventImeModeChanged, value);
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00034284 File Offset: 0x00032484
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x00034296 File Offset: 0x00032496
		internal int ImeWmCharsToIgnore
		{
			get
			{
				return this.Properties.GetInteger(Control.PropImeWmCharsToIgnore);
			}
			set
			{
				if (this.ImeWmCharsToIgnore != -1)
				{
					this.Properties.SetInteger(Control.PropImeWmCharsToIgnore, value);
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x000342B4 File Offset: 0x000324B4
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x000342E1 File Offset: 0x000324E1
		private bool LastCanEnableIme
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropLastCanEnableIme, out flag);
				flag = (!flag || integer == 1);
				return flag;
			}
			set
			{
				this.Properties.SetInteger(Control.PropLastCanEnableIme, value ? 1 : 0);
			}
		}

		/// <summary>Gets an object that represents a propagating IME mode.</summary>
		/// <returns>An object that represents a propagating IME mode.</returns>
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x000342FC File Offset: 0x000324FC
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x0003435E File Offset: 0x0003255E
		private protected static ImeMode PropagatingImeMode
		{
			protected get
			{
				if (Control.propagatingImeMode == ImeMode.Inherit)
				{
					ImeMode imeMode = ImeMode.Inherit;
					IntPtr intPtr = UnsafeNativeMethods.GetFocus();
					if (intPtr != IntPtr.Zero)
					{
						imeMode = ImeContext.GetImeMode(intPtr);
						if (imeMode == ImeMode.Disable)
						{
							intPtr = UnsafeNativeMethods.GetAncestor(new HandleRef(null, intPtr), 2);
							if (intPtr != IntPtr.Zero)
							{
								imeMode = ImeContext.GetImeMode(intPtr);
							}
						}
					}
					Control.PropagatingImeMode = imeMode;
				}
				return Control.propagatingImeMode;
			}
			private set
			{
				if (Control.propagatingImeMode != value)
				{
					if (value == ImeMode.NoControl || value == ImeMode.Disable)
					{
						return;
					}
					Control.propagatingImeMode = value;
				}
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00034378 File Offset: 0x00032578
		internal void UpdateImeContextMode()
		{
			ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
			if (!base.DesignMode && inputLanguageTable != ImeModeConversion.UnsupportedTable && this.Focused)
			{
				ImeMode imeMode = ImeMode.Disable;
				ImeMode cachedImeMode = this.CachedImeMode;
				if (this.ImeSupported && this.CanEnableIme)
				{
					imeMode = ((cachedImeMode == ImeMode.NoControl) ? Control.PropagatingImeMode : cachedImeMode);
				}
				if (this.CurrentImeContextMode != imeMode && imeMode != ImeMode.Inherit)
				{
					int disableImeModeChangedCount = this.DisableImeModeChangedCount;
					this.DisableImeModeChangedCount = disableImeModeChangedCount + 1;
					ImeMode imeMode2 = Control.PropagatingImeMode;
					try
					{
						ImeContext.SetImeStatus(imeMode, this.Handle);
					}
					finally
					{
						disableImeModeChangedCount = this.DisableImeModeChangedCount;
						this.DisableImeModeChangedCount = disableImeModeChangedCount - 1;
						if (imeMode == ImeMode.Disable && inputLanguageTable == ImeModeConversion.ChineseTable)
						{
							Control.PropagatingImeMode = imeMode2;
						}
					}
					if (cachedImeMode == ImeMode.NoControl)
					{
						if (this.CanEnableIme)
						{
							Control.PropagatingImeMode = this.CurrentImeContextMode;
							return;
						}
					}
					else
					{
						if (this.CanEnableIme)
						{
							this.CachedImeMode = this.CurrentImeContextMode;
						}
						this.VerifyImeModeChanged(imeMode, this.CachedImeMode);
					}
				}
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00034478 File Offset: 0x00032678
		private void VerifyImeModeChanged(ImeMode oldMode, ImeMode newMode)
		{
			if (this.ImeSupported && this.DisableImeModeChangedCount == 0 && newMode != ImeMode.NoControl && oldMode != newMode)
			{
				this.OnImeModeChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0003449C File Offset: 0x0003269C
		internal void VerifyImeRestrictedModeChanged()
		{
			bool canEnableIme = this.CanEnableIme;
			if (this.LastCanEnableIme != canEnableIme)
			{
				if (this.Focused)
				{
					int disableImeModeChangedCount = this.DisableImeModeChangedCount;
					this.DisableImeModeChangedCount = disableImeModeChangedCount + 1;
					try
					{
						this.UpdateImeContextMode();
					}
					finally
					{
						disableImeModeChangedCount = this.DisableImeModeChangedCount;
						this.DisableImeModeChangedCount = disableImeModeChangedCount - 1;
					}
				}
				ImeMode imeMode = this.CachedImeMode;
				ImeMode newMode = ImeMode.Disable;
				if (canEnableIme)
				{
					newMode = imeMode;
					imeMode = ImeMode.Disable;
				}
				this.VerifyImeModeChanged(imeMode, newMode);
				this.LastCanEnableIme = canEnableIme;
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0003451C File Offset: 0x0003271C
		internal void OnImeContextStatusChanged(IntPtr handle)
		{
			ImeMode imeMode = ImeContext.GetImeMode(handle);
			if (imeMode != ImeMode.Inherit)
			{
				ImeMode cachedImeMode = this.CachedImeMode;
				if (this.CanEnableIme)
				{
					if (cachedImeMode != ImeMode.NoControl)
					{
						this.CachedImeMode = imeMode;
						this.VerifyImeModeChanged(cachedImeMode, this.CachedImeMode);
						return;
					}
					Control.PropagatingImeMode = imeMode;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ImeModeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000F99 RID: 3993 RVA: 0x00034564 File Offset: 0x00032764
		protected virtual void OnImeModeChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventImeModeChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property to its default value.</summary>
		// Token: 0x06000F9A RID: 3994 RVA: 0x00034592 File Offset: 0x00032792
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImeMode()
		{
			this.ImeMode = this.DefaultImeMode;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000345A0 File Offset: 0x000327A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeImeMode()
		{
			bool flag;
			int integer = this.Properties.GetInteger(Control.PropImeMode, out flag);
			return flag && integer != (int)this.DefaultImeMode;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x000345D4 File Offset: 0x000327D4
		private void WmInputLangChange(ref Message m)
		{
			this.UpdateImeContextMode();
			if (ImeModeConversion.InputLanguageTable == ImeModeConversion.UnsupportedTable)
			{
				Control.PropagatingImeMode = ImeMode.Off;
			}
			if (LocalAppContextSwitches.EnableLegacyChineseIMEIndicator && ImeModeConversion.InputLanguageTable == ImeModeConversion.ChineseTable)
			{
				Control.IgnoreWmImeNotify = false;
			}
			Form form = this.FindFormInternal();
			if (form != null)
			{
				InputLanguageChangedEventArgs iplevent = InputLanguage.CreateInputLanguageChangedEventArgs(m);
				form.PerformOnInputLanguageChanged(iplevent);
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00034638 File Offset: 0x00032838
		private void WmInputLangChangeRequest(ref Message m)
		{
			InputLanguageChangingEventArgs inputLanguageChangingEventArgs = InputLanguage.CreateInputLanguageChangingEventArgs(m);
			Form form = this.FindFormInternal();
			if (form != null)
			{
				form.PerformOnInputLanguageChanging(inputLanguageChangingEventArgs);
			}
			if (!inputLanguageChangingEventArgs.Cancel)
			{
				this.DefWndProc(ref m);
				return;
			}
			m.Result = IntPtr.Zero;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003467D File Offset: 0x0003287D
		private void WmImeChar(ref Message m)
		{
			if (this.ProcessKeyEventArgs(ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00034690 File Offset: 0x00032890
		private void WmImeEndComposition(ref Message m)
		{
			this.ImeWmCharsToIgnore = -1;
			this.DefWndProc(ref m);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000346A0 File Offset: 0x000328A0
		private void WmImeNotify(ref Message m)
		{
			ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
			if (LocalAppContextSwitches.EnableLegacyChineseIMEIndicator && inputLanguageTable == ImeModeConversion.ChineseTable && !Control.lastLanguageChinese)
			{
				Control.IgnoreWmImeNotify = true;
			}
			Control.lastLanguageChinese = (inputLanguageTable == ImeModeConversion.ChineseTable);
			if (this.ImeSupported && inputLanguageTable != ImeModeConversion.UnsupportedTable && !Control.IgnoreWmImeNotify)
			{
				int num = (int)m.WParam;
				if (num == 6 || num == 8)
				{
					this.OnImeContextStatusChanged(this.Handle);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003471A File Offset: 0x0003291A
		internal void WmImeSetFocus()
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				this.UpdateImeContextMode();
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003472E File Offset: 0x0003292E
		private void WmImeStartComposition(ref Message m)
		{
			this.Properties.SetInteger(Control.PropImeWmCharsToIgnore, 0);
			this.DefWndProc(ref m);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00034748 File Offset: 0x00032948
		private void WmImeKillFocus()
		{
			Control topMostParent = this.TopMostParent;
			Form form = topMostParent as Form;
			if ((form == null || form.Modal) && !topMostParent.ContainsFocus && Control.propagatingImeMode != ImeMode.Inherit)
			{
				Control.IgnoreWmImeNotify = true;
				try
				{
					ImeContext.SetImeStatus(Control.PropagatingImeMode, topMostParent.Handle);
					Control.PropagatingImeMode = ImeMode.Inherit;
				}
				finally
				{
					Control.IgnoreWmImeNotify = false;
				}
			}
		}

		// Token: 0x04000768 RID: 1896
		internal static readonly TraceSwitch ControlKeyboardRouting;

		// Token: 0x04000769 RID: 1897
		internal static readonly TraceSwitch PaletteTracing;

		// Token: 0x0400076A RID: 1898
		internal static readonly TraceSwitch FocusTracing;

		// Token: 0x0400076B RID: 1899
		internal static readonly BooleanSwitch BufferPinkRect;

		// Token: 0x0400076C RID: 1900
		private static int WM_GETCONTROLNAME;

		// Token: 0x0400076D RID: 1901
		private static int WM_GETCONTROLTYPE;

		// Token: 0x0400076E RID: 1902
		internal const int STATE_CREATED = 1;

		// Token: 0x0400076F RID: 1903
		internal const int STATE_VISIBLE = 2;

		// Token: 0x04000770 RID: 1904
		internal const int STATE_ENABLED = 4;

		// Token: 0x04000771 RID: 1905
		internal const int STATE_TABSTOP = 8;

		// Token: 0x04000772 RID: 1906
		internal const int STATE_RECREATE = 16;

		// Token: 0x04000773 RID: 1907
		internal const int STATE_MODAL = 32;

		// Token: 0x04000774 RID: 1908
		internal const int STATE_ALLOWDROP = 64;

		// Token: 0x04000775 RID: 1909
		internal const int STATE_DROPTARGET = 128;

		// Token: 0x04000776 RID: 1910
		internal const int STATE_NOZORDER = 256;

		// Token: 0x04000777 RID: 1911
		internal const int STATE_LAYOUTDEFERRED = 512;

		// Token: 0x04000778 RID: 1912
		internal const int STATE_USEWAITCURSOR = 1024;

		// Token: 0x04000779 RID: 1913
		internal const int STATE_DISPOSED = 2048;

		// Token: 0x0400077A RID: 1914
		internal const int STATE_DISPOSING = 4096;

		// Token: 0x0400077B RID: 1915
		internal const int STATE_MOUSEENTERPENDING = 8192;

		// Token: 0x0400077C RID: 1916
		internal const int STATE_TRACKINGMOUSEEVENT = 16384;

		// Token: 0x0400077D RID: 1917
		internal const int STATE_THREADMARSHALLPENDING = 32768;

		// Token: 0x0400077E RID: 1918
		internal const int STATE_SIZELOCKEDBYOS = 65536;

		// Token: 0x0400077F RID: 1919
		internal const int STATE_CAUSESVALIDATION = 131072;

		// Token: 0x04000780 RID: 1920
		internal const int STATE_CREATINGHANDLE = 262144;

		// Token: 0x04000781 RID: 1921
		internal const int STATE_TOPLEVEL = 524288;

		// Token: 0x04000782 RID: 1922
		internal const int STATE_ISACCESSIBLE = 1048576;

		// Token: 0x04000783 RID: 1923
		internal const int STATE_OWNCTLBRUSH = 2097152;

		// Token: 0x04000784 RID: 1924
		internal const int STATE_EXCEPTIONWHILEPAINTING = 4194304;

		// Token: 0x04000785 RID: 1925
		internal const int STATE_LAYOUTISDIRTY = 8388608;

		// Token: 0x04000786 RID: 1926
		internal const int STATE_CHECKEDHOST = 16777216;

		// Token: 0x04000787 RID: 1927
		internal const int STATE_HOSTEDINDIALOG = 33554432;

		// Token: 0x04000788 RID: 1928
		internal const int STATE_DOUBLECLICKFIRED = 67108864;

		// Token: 0x04000789 RID: 1929
		internal const int STATE_MOUSEPRESSED = 134217728;

		// Token: 0x0400078A RID: 1930
		internal const int STATE_VALIDATIONCANCELLED = 268435456;

		// Token: 0x0400078B RID: 1931
		internal const int STATE_PARENTRECREATING = 536870912;

		// Token: 0x0400078C RID: 1932
		internal const int STATE_MIRRORED = 1073741824;

		// Token: 0x0400078D RID: 1933
		private const int STATE2_HAVEINVOKED = 1;

		// Token: 0x0400078E RID: 1934
		private const int STATE2_SETSCROLLPOS = 2;

		// Token: 0x0400078F RID: 1935
		private const int STATE2_LISTENINGTOUSERPREFERENCECHANGED = 4;

		// Token: 0x04000790 RID: 1936
		internal const int STATE2_INTERESTEDINUSERPREFERENCECHANGED = 8;

		// Token: 0x04000791 RID: 1937
		internal const int STATE2_MAINTAINSOWNCAPTUREMODE = 16;

		// Token: 0x04000792 RID: 1938
		private const int STATE2_BECOMINGACTIVECONTROL = 32;

		// Token: 0x04000793 RID: 1939
		private const int STATE2_CLEARLAYOUTARGS = 64;

		// Token: 0x04000794 RID: 1940
		private const int STATE2_INPUTKEY = 128;

		// Token: 0x04000795 RID: 1941
		private const int STATE2_INPUTCHAR = 256;

		// Token: 0x04000796 RID: 1942
		private const int STATE2_UICUES = 512;

		// Token: 0x04000797 RID: 1943
		private const int STATE2_ISACTIVEX = 1024;

		// Token: 0x04000798 RID: 1944
		internal const int STATE2_USEPREFERREDSIZECACHE = 2048;

		// Token: 0x04000799 RID: 1945
		internal const int STATE2_TOPMDIWINDOWCLOSING = 4096;

		// Token: 0x0400079A RID: 1946
		internal const int STATE2_CURRENTLYBEINGSCALED = 8192;

		// Token: 0x0400079B RID: 1947
		private static readonly object EventAutoSizeChanged = new object();

		// Token: 0x0400079C RID: 1948
		private static readonly object EventKeyDown = new object();

		// Token: 0x0400079D RID: 1949
		private static readonly object EventKeyPress = new object();

		// Token: 0x0400079E RID: 1950
		private static readonly object EventKeyUp = new object();

		// Token: 0x0400079F RID: 1951
		private static readonly object EventMouseDown = new object();

		// Token: 0x040007A0 RID: 1952
		private static readonly object EventMouseEnter = new object();

		// Token: 0x040007A1 RID: 1953
		private static readonly object EventMouseLeave = new object();

		// Token: 0x040007A2 RID: 1954
		private static readonly object EventDpiChangedBeforeParent = new object();

		// Token: 0x040007A3 RID: 1955
		private static readonly object EventDpiChangedAfterParent = new object();

		// Token: 0x040007A4 RID: 1956
		private static readonly object EventMouseHover = new object();

		// Token: 0x040007A5 RID: 1957
		private static readonly object EventMouseMove = new object();

		// Token: 0x040007A6 RID: 1958
		private static readonly object EventMouseUp = new object();

		// Token: 0x040007A7 RID: 1959
		private static readonly object EventMouseWheel = new object();

		// Token: 0x040007A8 RID: 1960
		private static readonly object EventClick = new object();

		// Token: 0x040007A9 RID: 1961
		private static readonly object EventClientSize = new object();

		// Token: 0x040007AA RID: 1962
		private static readonly object EventDoubleClick = new object();

		// Token: 0x040007AB RID: 1963
		private static readonly object EventMouseClick = new object();

		// Token: 0x040007AC RID: 1964
		private static readonly object EventMouseDoubleClick = new object();

		// Token: 0x040007AD RID: 1965
		private static readonly object EventMouseCaptureChanged = new object();

		// Token: 0x040007AE RID: 1966
		private static readonly object EventMove = new object();

		// Token: 0x040007AF RID: 1967
		private static readonly object EventResize = new object();

		// Token: 0x040007B0 RID: 1968
		private static readonly object EventLayout = new object();

		// Token: 0x040007B1 RID: 1969
		private static readonly object EventGotFocus = new object();

		// Token: 0x040007B2 RID: 1970
		private static readonly object EventLostFocus = new object();

		// Token: 0x040007B3 RID: 1971
		private static readonly object EventEnabledChanged = new object();

		// Token: 0x040007B4 RID: 1972
		private static readonly object EventEnter = new object();

		// Token: 0x040007B5 RID: 1973
		private static readonly object EventLeave = new object();

		// Token: 0x040007B6 RID: 1974
		private static readonly object EventHandleCreated = new object();

		// Token: 0x040007B7 RID: 1975
		private static readonly object EventHandleDestroyed = new object();

		// Token: 0x040007B8 RID: 1976
		private static readonly object EventVisibleChanged = new object();

		// Token: 0x040007B9 RID: 1977
		private static readonly object EventControlAdded = new object();

		// Token: 0x040007BA RID: 1978
		private static readonly object EventControlRemoved = new object();

		// Token: 0x040007BB RID: 1979
		private static readonly object EventChangeUICues = new object();

		// Token: 0x040007BC RID: 1980
		private static readonly object EventSystemColorsChanged = new object();

		// Token: 0x040007BD RID: 1981
		private static readonly object EventValidating = new object();

		// Token: 0x040007BE RID: 1982
		private static readonly object EventValidated = new object();

		// Token: 0x040007BF RID: 1983
		private static readonly object EventStyleChanged = new object();

		// Token: 0x040007C0 RID: 1984
		private static readonly object EventImeModeChanged = new object();

		// Token: 0x040007C1 RID: 1985
		private static readonly object EventHelpRequested = new object();

		// Token: 0x040007C2 RID: 1986
		private static readonly object EventPaint = new object();

		// Token: 0x040007C3 RID: 1987
		private static readonly object EventInvalidated = new object();

		// Token: 0x040007C4 RID: 1988
		private static readonly object EventQueryContinueDrag = new object();

		// Token: 0x040007C5 RID: 1989
		private static readonly object EventGiveFeedback = new object();

		// Token: 0x040007C6 RID: 1990
		private static readonly object EventDragEnter = new object();

		// Token: 0x040007C7 RID: 1991
		private static readonly object EventDragLeave = new object();

		// Token: 0x040007C8 RID: 1992
		private static readonly object EventDragOver = new object();

		// Token: 0x040007C9 RID: 1993
		private static readonly object EventDragDrop = new object();

		// Token: 0x040007CA RID: 1994
		private static readonly object EventQueryAccessibilityHelp = new object();

		// Token: 0x040007CB RID: 1995
		private static readonly object EventBackgroundImage = new object();

		// Token: 0x040007CC RID: 1996
		private static readonly object EventBackgroundImageLayout = new object();

		// Token: 0x040007CD RID: 1997
		private static readonly object EventBindingContext = new object();

		// Token: 0x040007CE RID: 1998
		private static readonly object EventBackColor = new object();

		// Token: 0x040007CF RID: 1999
		private static readonly object EventParent = new object();

		// Token: 0x040007D0 RID: 2000
		private static readonly object EventVisible = new object();

		// Token: 0x040007D1 RID: 2001
		private static readonly object EventText = new object();

		// Token: 0x040007D2 RID: 2002
		private static readonly object EventTabStop = new object();

		// Token: 0x040007D3 RID: 2003
		private static readonly object EventTabIndex = new object();

		// Token: 0x040007D4 RID: 2004
		private static readonly object EventSize = new object();

		// Token: 0x040007D5 RID: 2005
		private static readonly object EventRightToLeft = new object();

		// Token: 0x040007D6 RID: 2006
		private static readonly object EventLocation = new object();

		// Token: 0x040007D7 RID: 2007
		private static readonly object EventForeColor = new object();

		// Token: 0x040007D8 RID: 2008
		private static readonly object EventFont = new object();

		// Token: 0x040007D9 RID: 2009
		private static readonly object EventEnabled = new object();

		// Token: 0x040007DA RID: 2010
		private static readonly object EventDock = new object();

		// Token: 0x040007DB RID: 2011
		private static readonly object EventCursor = new object();

		// Token: 0x040007DC RID: 2012
		private static readonly object EventContextMenu = new object();

		// Token: 0x040007DD RID: 2013
		private static readonly object EventContextMenuStrip = new object();

		// Token: 0x040007DE RID: 2014
		private static readonly object EventCausesValidation = new object();

		// Token: 0x040007DF RID: 2015
		private static readonly object EventRegionChanged = new object();

		// Token: 0x040007E0 RID: 2016
		private static readonly object EventMarginChanged = new object();

		// Token: 0x040007E1 RID: 2017
		internal static readonly object EventPaddingChanged = new object();

		// Token: 0x040007E2 RID: 2018
		private static readonly object EventPreviewKeyDown = new object();

		// Token: 0x040007E3 RID: 2019
		private static int mouseWheelMessage = 522;

		// Token: 0x040007E4 RID: 2020
		private static bool mouseWheelRoutingNeeded;

		// Token: 0x040007E5 RID: 2021
		private static bool mouseWheelInit;

		// Token: 0x040007E6 RID: 2022
		private static int threadCallbackMessage;

		// Token: 0x040007E7 RID: 2023
		private static bool checkForIllegalCrossThreadCalls = Debugger.IsAttached;

		// Token: 0x040007E8 RID: 2024
		private static ContextCallback invokeMarshaledCallbackHelperDelegate;

		// Token: 0x040007E9 RID: 2025
		[ThreadStatic]
		private static bool inCrossThreadSafeCall = false;

		// Token: 0x040007EA RID: 2026
		[ThreadStatic]
		internal static HelpInfo currentHelpInfo = null;

		// Token: 0x040007EB RID: 2027
		private static Control.FontHandleWrapper defaultFontHandleWrapper;

		// Token: 0x040007EC RID: 2028
		private const short PaintLayerBackground = 1;

		// Token: 0x040007ED RID: 2029
		private const short PaintLayerForeground = 2;

		// Token: 0x040007EE RID: 2030
		private const byte RequiredScalingEnabledMask = 16;

		// Token: 0x040007EF RID: 2031
		private const byte RequiredScalingMask = 15;

		// Token: 0x040007F0 RID: 2032
		private const byte HighOrderBitMask = 128;

		// Token: 0x040007F1 RID: 2033
		private static Font defaultFont;

		// Token: 0x040007F2 RID: 2034
		private static readonly int PropName = PropertyStore.CreateKey();

		// Token: 0x040007F3 RID: 2035
		private static readonly int PropBackBrush = PropertyStore.CreateKey();

		// Token: 0x040007F4 RID: 2036
		private static readonly int PropFontHeight = PropertyStore.CreateKey();

		// Token: 0x040007F5 RID: 2037
		private static readonly int PropCurrentAmbientFont = PropertyStore.CreateKey();

		// Token: 0x040007F6 RID: 2038
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x040007F7 RID: 2039
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x040007F8 RID: 2040
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x040007F9 RID: 2041
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x040007FA RID: 2042
		private static readonly int PropBackgroundImage = PropertyStore.CreateKey();

		// Token: 0x040007FB RID: 2043
		private static readonly int PropFontHandleWrapper = PropertyStore.CreateKey();

		// Token: 0x040007FC RID: 2044
		private static readonly int PropUserData = PropertyStore.CreateKey();

		// Token: 0x040007FD RID: 2045
		private static readonly int PropContextMenu = PropertyStore.CreateKey();

		// Token: 0x040007FE RID: 2046
		private static readonly int PropCursor = PropertyStore.CreateKey();

		// Token: 0x040007FF RID: 2047
		private static readonly int PropRegion = PropertyStore.CreateKey();

		// Token: 0x04000800 RID: 2048
		private static readonly int PropRightToLeft = PropertyStore.CreateKey();

		// Token: 0x04000801 RID: 2049
		private static readonly int PropBindings = PropertyStore.CreateKey();

		// Token: 0x04000802 RID: 2050
		private static readonly int PropBindingManager = PropertyStore.CreateKey();

		// Token: 0x04000803 RID: 2051
		private static readonly int PropAccessibleDefaultActionDescription = PropertyStore.CreateKey();

		// Token: 0x04000804 RID: 2052
		private static readonly int PropAccessibleDescription = PropertyStore.CreateKey();

		// Token: 0x04000805 RID: 2053
		private static readonly int PropAccessibility = PropertyStore.CreateKey();

		// Token: 0x04000806 RID: 2054
		private static readonly int PropNcAccessibility = PropertyStore.CreateKey();

		// Token: 0x04000807 RID: 2055
		private static readonly int PropAccessibleName = PropertyStore.CreateKey();

		// Token: 0x04000808 RID: 2056
		private static readonly int PropAccessibleRole = PropertyStore.CreateKey();

		// Token: 0x04000809 RID: 2057
		private static readonly int PropPaintingException = PropertyStore.CreateKey();

		// Token: 0x0400080A RID: 2058
		private static readonly int PropActiveXImpl = PropertyStore.CreateKey();

		// Token: 0x0400080B RID: 2059
		private static readonly int PropControlVersionInfo = PropertyStore.CreateKey();

		// Token: 0x0400080C RID: 2060
		private static readonly int PropBackgroundImageLayout = PropertyStore.CreateKey();

		// Token: 0x0400080D RID: 2061
		private static readonly int PropAccessibleHelpProvider = PropertyStore.CreateKey();

		// Token: 0x0400080E RID: 2062
		private static readonly int PropContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x0400080F RID: 2063
		private static readonly int PropAutoScrollOffset = PropertyStore.CreateKey();

		// Token: 0x04000810 RID: 2064
		private static readonly int PropUseCompatibleTextRendering = PropertyStore.CreateKey();

		// Token: 0x04000811 RID: 2065
		private static readonly int PropImeWmCharsToIgnore = PropertyStore.CreateKey();

		// Token: 0x04000812 RID: 2066
		private static readonly int PropImeMode = PropertyStore.CreateKey();

		// Token: 0x04000813 RID: 2067
		private static readonly int PropDisableImeModeChangedCount = PropertyStore.CreateKey();

		// Token: 0x04000814 RID: 2068
		private static readonly int PropLastCanEnableIme = PropertyStore.CreateKey();

		// Token: 0x04000815 RID: 2069
		private static readonly int PropCacheTextCount = PropertyStore.CreateKey();

		// Token: 0x04000816 RID: 2070
		private static readonly int PropCacheTextField = PropertyStore.CreateKey();

		// Token: 0x04000817 RID: 2071
		private static readonly int PropAmbientPropertiesService = PropertyStore.CreateKey();

		// Token: 0x04000818 RID: 2072
		private static bool needToLoadComCtl = true;

		// Token: 0x04000819 RID: 2073
		internal static bool UseCompatibleTextRenderingDefault = true;

		// Token: 0x0400081A RID: 2074
		private Control.ControlNativeWindow window;

		// Token: 0x0400081B RID: 2075
		private Control parent;

		// Token: 0x0400081C RID: 2076
		private Control reflectParent;

		// Token: 0x0400081D RID: 2077
		private CreateParams createParams;

		// Token: 0x0400081E RID: 2078
		private int x;

		// Token: 0x0400081F RID: 2079
		private int y;

		// Token: 0x04000820 RID: 2080
		private int width;

		// Token: 0x04000821 RID: 2081
		private int height;

		// Token: 0x04000822 RID: 2082
		private int clientWidth;

		// Token: 0x04000823 RID: 2083
		private int clientHeight;

		// Token: 0x04000824 RID: 2084
		private int state;

		// Token: 0x04000825 RID: 2085
		private int state2;

		// Token: 0x04000826 RID: 2086
		private ControlStyles controlStyle;

		// Token: 0x04000827 RID: 2087
		private int tabIndex;

		// Token: 0x04000828 RID: 2088
		private string text;

		// Token: 0x04000829 RID: 2089
		private byte layoutSuspendCount;

		// Token: 0x0400082A RID: 2090
		private byte requiredScaling;

		// Token: 0x0400082B RID: 2091
		private PropertyStore propertyStore;

		// Token: 0x0400082C RID: 2092
		private NativeMethods.TRACKMOUSEEVENT trackMouseEvent;

		// Token: 0x0400082D RID: 2093
		private short updateCount;

		// Token: 0x0400082E RID: 2094
		private LayoutEventArgs cachedLayoutEventArgs;

		// Token: 0x0400082F RID: 2095
		private Queue threadCallbackList;

		// Token: 0x04000830 RID: 2096
		internal int deviceDpi;

		// Token: 0x04000831 RID: 2097
		private int uiCuesState;

		// Token: 0x04000832 RID: 2098
		private const int UISTATE_FOCUS_CUES_MASK = 15;

		// Token: 0x04000833 RID: 2099
		private const int UISTATE_FOCUS_CUES_HIDDEN = 1;

		// Token: 0x04000834 RID: 2100
		private const int UISTATE_FOCUS_CUES_SHOW = 2;

		// Token: 0x04000835 RID: 2101
		private const int UISTATE_KEYBOARD_CUES_MASK = 240;

		// Token: 0x04000836 RID: 2102
		private const int UISTATE_KEYBOARD_CUES_HIDDEN = 16;

		// Token: 0x04000837 RID: 2103
		private const int UISTATE_KEYBOARD_CUES_SHOW = 32;

		// Token: 0x04000838 RID: 2104
		[ThreadStatic]
		private static byte[] tempKeyboardStateArray;

		// Token: 0x04000839 RID: 2105
		private readonly WeakReference<ToolStripControlHost> toolStripControlHostReference = new WeakReference<ToolStripControlHost>(null);

		// Token: 0x0400083A RID: 2106
		private const int ImeCharsToIgnoreDisabled = -1;

		// Token: 0x0400083B RID: 2107
		private const int ImeCharsToIgnoreEnabled = 0;

		// Token: 0x0400083C RID: 2108
		private static ImeMode propagatingImeMode = ImeMode.Inherit;

		// Token: 0x0400083D RID: 2109
		private static bool ignoreWmImeNotify;

		// Token: 0x0400083E RID: 2110
		private static bool lastLanguageChinese = false;

		// Token: 0x02000578 RID: 1400
		private class ControlTabOrderHolder
		{
			// Token: 0x06005714 RID: 22292 RVA: 0x0016CA58 File Offset: 0x0016AC58
			internal ControlTabOrderHolder(int oldOrder, int newOrder, Control control)
			{
				this.oldOrder = oldOrder;
				this.newOrder = newOrder;
				this.control = control;
			}

			// Token: 0x0400381D RID: 14365
			internal readonly int oldOrder;

			// Token: 0x0400381E RID: 14366
			internal readonly int newOrder;

			// Token: 0x0400381F RID: 14367
			internal readonly Control control;
		}

		// Token: 0x02000579 RID: 1401
		private class ControlTabOrderComparer : IComparer
		{
			// Token: 0x06005715 RID: 22293 RVA: 0x0016CA78 File Offset: 0x0016AC78
			int IComparer.Compare(object x, object y)
			{
				Control.ControlTabOrderHolder controlTabOrderHolder = (Control.ControlTabOrderHolder)x;
				Control.ControlTabOrderHolder controlTabOrderHolder2 = (Control.ControlTabOrderHolder)y;
				int num = controlTabOrderHolder.newOrder - controlTabOrderHolder2.newOrder;
				if (num == 0)
				{
					num = controlTabOrderHolder.oldOrder - controlTabOrderHolder2.oldOrder;
				}
				return num;
			}
		}

		// Token: 0x0200057A RID: 1402
		internal sealed class ControlNativeWindow : NativeWindow, IWindowTarget
		{
			// Token: 0x06005717 RID: 22295 RVA: 0x0016CAB3 File Offset: 0x0016ACB3
			internal ControlNativeWindow(Control control)
			{
				this.control = control;
				this.target = this;
			}

			// Token: 0x06005718 RID: 22296 RVA: 0x0016CAC9 File Offset: 0x0016ACC9
			internal Control GetControl()
			{
				return this.control;
			}

			// Token: 0x06005719 RID: 22297 RVA: 0x0016CAD1 File Offset: 0x0016ACD1
			protected override void OnHandleChange()
			{
				this.target.OnHandleChange(base.Handle);
			}

			// Token: 0x0600571A RID: 22298 RVA: 0x0016CAE4 File Offset: 0x0016ACE4
			public void OnHandleChange(IntPtr newHandle)
			{
				this.control.SetHandle(newHandle);
			}

			// Token: 0x0600571B RID: 22299 RVA: 0x0016CAF2 File Offset: 0x0016ACF2
			internal void LockReference(bool locked)
			{
				if (locked)
				{
					if (!this.rootRef.IsAllocated)
					{
						this.rootRef = GCHandle.Alloc(this.GetControl(), GCHandleType.Normal);
						return;
					}
				}
				else if (this.rootRef.IsAllocated)
				{
					this.rootRef.Free();
				}
			}

			// Token: 0x0600571C RID: 22300 RVA: 0x0016CB2F File Offset: 0x0016AD2F
			protected override void OnThreadException(Exception e)
			{
				this.control.WndProcException(e);
			}

			// Token: 0x0600571D RID: 22301 RVA: 0x0016CB3D File Offset: 0x0016AD3D
			public void OnMessage(ref Message m)
			{
				this.control.WndProc(ref m);
			}

			// Token: 0x170014C8 RID: 5320
			// (get) Token: 0x0600571E RID: 22302 RVA: 0x0016CB4B File Offset: 0x0016AD4B
			// (set) Token: 0x0600571F RID: 22303 RVA: 0x0016CB53 File Offset: 0x0016AD53
			internal IWindowTarget WindowTarget
			{
				get
				{
					return this.target;
				}
				set
				{
					this.target = value;
				}
			}

			// Token: 0x06005720 RID: 22304 RVA: 0x0016CB5C File Offset: 0x0016AD5C
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 512)
				{
					if (msg != 522)
					{
						if (msg == 675)
						{
							this.control.UnhookMouseEvent();
						}
					}
					else
					{
						this.control.ResetMouseEventArgs();
					}
				}
				else if (!this.control.GetState(16384))
				{
					this.control.HookMouseEvent();
					if (!this.control.GetState(8192))
					{
						this.control.SendMessage(NativeMethods.WM_MOUSEENTER, 0, 0);
					}
					else
					{
						this.control.SetState(8192, false);
					}
				}
				this.target.OnMessage(ref m);
			}

			// Token: 0x04003820 RID: 14368
			private Control control;

			// Token: 0x04003821 RID: 14369
			private GCHandle rootRef;

			// Token: 0x04003822 RID: 14370
			internal IWindowTarget target;
		}

		/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.Control" /> objects.</summary>
		// Token: 0x0200057B RID: 1403
		[ListBindable(false)]
		[ComVisible(false)]
		public class ControlCollection : ArrangedElementCollection, IList, ICollection, IEnumerable, ICloneable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control.ControlCollection" /> class.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.Control" /> representing the control that owns the control collection. </param>
			// Token: 0x06005721 RID: 22305 RVA: 0x0016CC03 File Offset: 0x0016AE03
			public ControlCollection(Control owner)
			{
				this.owner = owner;
			}

			/// <summary>Determines whether the <see cref="T:System.Windows.Forms.Control.ControlCollection" /> contains an item with the specified key.</summary>
			/// <param name="key">The key to locate in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />. </param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.Control.ControlCollection" /> contains an item with the specified key; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005722 RID: 22306 RVA: 0x0016CC19 File Offset: 0x0016AE19
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Adds the specified control to the control collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.Control" /> to add to the control collection. </param>
			/// <exception cref="T:System.Exception">The specified control is a top-level control, or a circular control reference would result if this control were added to the control collection. </exception>
			/// <exception cref="T:System.ArgumentException">The object assigned to the <paramref name="value" /> parameter is not a <see cref="T:System.Windows.Forms.Control" />. </exception>
			// Token: 0x06005723 RID: 22307 RVA: 0x0016CC28 File Offset: 0x0016AE28
			public virtual void Add(Control value)
			{
				if (value == null)
				{
					return;
				}
				if (value.GetTopLevel())
				{
					throw new ArgumentException(SR.GetString("TopLevelControlAdd"));
				}
				if (this.owner.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"));
				}
				Control.CheckParentingCycle(this.owner, value);
				if (value.parent == this.owner)
				{
					value.SendToBack();
					return;
				}
				if (value.parent != null)
				{
					value.parent.Controls.Remove(value);
				}
				base.InnerList.Add(value);
				if (value.tabIndex == -1)
				{
					int num = 0;
					for (int i = 0; i < this.Count - 1; i++)
					{
						int tabIndex = this[i].TabIndex;
						if (num <= tabIndex)
						{
							num = tabIndex + 1;
						}
					}
					value.tabIndex = num;
				}
				this.owner.SuspendLayout();
				try
				{
					Control parent = value.parent;
					try
					{
						value.AssignParent(this.owner);
					}
					finally
					{
						if (parent != value.parent && (this.owner.state & 1) != 0)
						{
							value.SetParentHandle(this.owner.InternalHandle);
							if (value.Visible)
							{
								value.CreateControl();
							}
						}
					}
					value.InitLayout();
				}
				finally
				{
					this.owner.ResumeLayout(false);
				}
				LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
				this.owner.OnControlAdded(new ControlEventArgs(value));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			// Token: 0x06005724 RID: 22308 RVA: 0x0016CDA0 File Offset: 0x0016AFA0
			int IList.Add(object control)
			{
				if (control is Control)
				{
					this.Add((Control)control);
					return this.IndexOf((Control)control);
				}
				throw new ArgumentException(SR.GetString("ControlBadControl"), "control");
			}

			/// <summary>Adds an array of control objects to the collection.</summary>
			/// <param name="controls">An array of <see cref="T:System.Windows.Forms.Control" /> objects to add to the collection. </param>
			// Token: 0x06005725 RID: 22309 RVA: 0x0016CDD8 File Offset: 0x0016AFD8
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public virtual void AddRange(Control[] controls)
			{
				if (controls == null)
				{
					throw new ArgumentNullException("controls");
				}
				if (controls.Length != 0)
				{
					this.owner.SuspendLayout();
					try
					{
						for (int i = 0; i < controls.Length; i++)
						{
							this.Add(controls[i]);
						}
					}
					finally
					{
						this.owner.ResumeLayout(true);
					}
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.ICloneable.Clone" />.</summary>
			// Token: 0x06005726 RID: 22310 RVA: 0x0016CE38 File Offset: 0x0016B038
			object ICloneable.Clone()
			{
				Control.ControlCollection controlCollection = this.owner.CreateControlsInstance();
				controlCollection.InnerList.AddRange(this);
				return controlCollection;
			}

			/// <summary>Determines whether the specified control is a member of the collection.</summary>
			/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.Control" /> is a member of the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005727 RID: 22311 RVA: 0x00115D88 File Offset: 0x00113F88
			public bool Contains(Control control)
			{
				return base.InnerList.Contains(control);
			}

			/// <summary>Searches for controls by their <see cref="P:System.Windows.Forms.Control.Name" /> property and builds an array of all the controls that match.</summary>
			/// <param name="key">The key to locate in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />. </param>
			/// <param name="searchAllChildren">
			///       <see langword="true" /> to search all child controls; otherwise, <see langword="false" />. </param>
			/// <returns>An array of type <see cref="T:System.Windows.Forms.Control" /> containing the matching controls.</returns>
			/// <exception cref="T:System.ArgumentException">The <paramref name="key" /> parameter is <see langword="null" /> or the empty string (""). </exception>
			// Token: 0x06005728 RID: 22312 RVA: 0x0016CE60 File Offset: 0x0016B060
			public Control[] Find(string key, bool searchAllChildren)
			{
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
				}
				ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
				Control[] array = new Control[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06005729 RID: 22313 RVA: 0x0016CEB0 File Offset: 0x0016B0B0
			private ArrayList FindInternal(string key, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
			{
				if (controlsToLookIn == null || foundControls == null)
				{
					return null;
				}
				try
				{
					for (int i = 0; i < controlsToLookIn.Count; i++)
					{
						if (controlsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(controlsToLookIn[i].Name, key, true))
						{
							foundControls.Add(controlsToLookIn[i]);
						}
					}
					if (searchAllChildren)
					{
						for (int j = 0; j < controlsToLookIn.Count; j++)
						{
							if (controlsToLookIn[j] != null && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
							{
								foundControls = this.FindInternal(key, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return foundControls;
			}

			/// <summary>Retrieves a reference to an enumerator object that is used to iterate over a <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" />.</returns>
			// Token: 0x0600572A RID: 22314 RVA: 0x0016CF80 File Offset: 0x0016B180
			public override IEnumerator GetEnumerator()
			{
				return new Control.ControlCollection.ControlCollectionEnumerator(this);
			}

			/// <summary>Retrieves the index of the specified control in the control collection.</summary>
			/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> to locate in the collection. </param>
			/// <returns>A zero-based index value that represents the position of the specified <see cref="T:System.Windows.Forms.Control" /> in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</returns>
			// Token: 0x0600572B RID: 22315 RVA: 0x001160EC File Offset: 0x001142EC
			public int IndexOf(Control control)
			{
				return base.InnerList.IndexOf(control);
			}

			/// <summary>Retrieves the index of the first occurrence of the specified item within the collection.</summary>
			/// <param name="key">The name of the control to search for. </param>
			/// <returns>The zero-based index of the first occurrence of the control with the specified name in the collection.</returns>
			// Token: 0x0600572C RID: 22316 RVA: 0x0016CF88 File Offset: 0x0016B188
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x0600572D RID: 22317 RVA: 0x0011617C File Offset: 0x0011437C
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Gets the control that owns this <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</summary>
			/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that owns this <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</returns>
			// Token: 0x170014C9 RID: 5321
			// (get) Token: 0x0600572E RID: 22318 RVA: 0x0016D005 File Offset: 0x0016B205
			public Control Owner
			{
				get
				{
					return this.owner;
				}
			}

			/// <summary>Removes the specified control from the control collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.Control" /> to remove from the <see cref="T:System.Windows.Forms.Control.ControlCollection" />. </param>
			// Token: 0x0600572F RID: 22319 RVA: 0x0016D010 File Offset: 0x0016B210
			public virtual void Remove(Control value)
			{
				if (value == null)
				{
					return;
				}
				if (value.ParentInternal == this.owner)
				{
					value.SetParentHandle(IntPtr.Zero);
					base.InnerList.Remove(value);
					value.AssignParent(null);
					LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
					this.owner.OnControlRemoved(new ControlEventArgs(value));
					ContainerControl containerControl = this.owner.GetContainerControlInternal() as ContainerControl;
					if (containerControl != null)
					{
						containerControl.AfterControlRemoved(value, this.owner);
					}
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			// Token: 0x06005730 RID: 22320 RVA: 0x0016D090 File Offset: 0x0016B290
			void IList.Remove(object control)
			{
				if (control is Control)
				{
					this.Remove((Control)control);
				}
			}

			/// <summary>Removes a control from the control collection at the specified indexed location.</summary>
			/// <param name="index">The index value of the <see cref="T:System.Windows.Forms.Control" /> to remove. </param>
			// Token: 0x06005731 RID: 22321 RVA: 0x0016D0A6 File Offset: 0x0016B2A6
			public void RemoveAt(int index)
			{
				this.Remove(this[index]);
			}

			/// <summary>Removes the child control with the specified key.</summary>
			/// <param name="key">The name of the child control to remove. </param>
			// Token: 0x06005732 RID: 22322 RVA: 0x0016D0B8 File Offset: 0x0016B2B8
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Indicates the <see cref="T:System.Windows.Forms.Control" /> at the specified indexed location in the collection.</summary>
			/// <param name="index">The index of the control to retrieve from the control collection. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.Control" /> located at the specified index location within the control collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> value is less than zero or is greater than or equal to the number of controls in the collection. </exception>
			// Token: 0x170014CA RID: 5322
			public virtual Control this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[]
						{
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return (Control)base.InnerList[index];
				}
			}

			/// <summary>Indicates a <see cref="T:System.Windows.Forms.Control" /> with the specified key in the collection.</summary>
			/// <param name="key">The name of the control to retrieve from the control collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.Control" /> with the specified key within the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</returns>
			// Token: 0x170014CB RID: 5323
			public virtual Control this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Removes all controls from the collection.</summary>
			// Token: 0x06005735 RID: 22325 RVA: 0x0016D16C File Offset: 0x0016B36C
			public virtual void Clear()
			{
				this.owner.SuspendLayout();
				CommonProperties.xClearAllPreferredSizeCaches(this.owner);
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					this.owner.ResumeLayout();
				}
			}

			/// <summary>Retrieves the index of the specified child control within the control collection.</summary>
			/// <param name="child">The <see cref="T:System.Windows.Forms.Control" /> to search for in the control collection. </param>
			/// <returns>A zero-based index value that represents the location of the specified child control within the control collection.</returns>
			/// <exception cref="T:System.ArgumentException">The <paramref name="child" /><see cref="T:System.Windows.Forms.Control" /> is not in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />. </exception>
			// Token: 0x06005736 RID: 22326 RVA: 0x0016D1C8 File Offset: 0x0016B3C8
			public int GetChildIndex(Control child)
			{
				return this.GetChildIndex(child, true);
			}

			/// <summary>Retrieves the index of the specified child control within the control collection, and optionally raises an exception if the specified control is not within the control collection.</summary>
			/// <param name="child">The <see cref="T:System.Windows.Forms.Control" /> to search for in the control collection. </param>
			/// <param name="throwException">
			///       <see langword="true" /> to throw an exception if the <see cref="T:System.Windows.Forms.Control" /> specified in the <paramref name="child" /> parameter is not a control in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />; otherwise, <see langword="false" />. </param>
			/// <returns>A zero-based index value that represents the location of the specified child control within the control collection; otherwise -1 if the specified <see cref="T:System.Windows.Forms.Control" /> is not found in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />.</returns>
			/// <exception cref="T:System.ArgumentException">The <paramref name="child" /><see cref="T:System.Windows.Forms.Control" /> is not in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />, and the <paramref name="throwException" /> parameter value is <see langword="true" />. </exception>
			// Token: 0x06005737 RID: 22327 RVA: 0x0016D1D4 File Offset: 0x0016B3D4
			public virtual int GetChildIndex(Control child, bool throwException)
			{
				int num = this.IndexOf(child);
				if (num == -1 && throwException)
				{
					throw new ArgumentException(SR.GetString("ControlNotChild"));
				}
				return num;
			}

			// Token: 0x06005738 RID: 22328 RVA: 0x0016D204 File Offset: 0x0016B404
			internal virtual void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child == null)
				{
					throw new ArgumentNullException("child");
				}
				int childIndex = this.GetChildIndex(child);
				if (childIndex == newIndex)
				{
					return;
				}
				if (newIndex >= this.Count || newIndex == -1)
				{
					newIndex = this.Count - 1;
				}
				base.MoveElement(child, childIndex, newIndex);
				child.UpdateZOrder();
				LayoutTransaction.DoLayout(this.owner, child, PropertyNames.ChildIndex);
			}

			/// <summary>Sets the index of the specified child control in the collection to the specified index value.</summary>
			/// <param name="child">The <paramref name="child" /><see cref="T:System.Windows.Forms.Control" /> to search for. </param>
			/// <param name="newIndex">The new index value of the control. </param>
			/// <exception cref="T:System.ArgumentException">The <paramref name="child" /> control is not in the <see cref="T:System.Windows.Forms.Control.ControlCollection" />. </exception>
			// Token: 0x06005739 RID: 22329 RVA: 0x0016D263 File Offset: 0x0016B463
			public virtual void SetChildIndex(Control child, int newIndex)
			{
				this.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x04003823 RID: 14371
			private Control owner;

			// Token: 0x04003824 RID: 14372
			private int lastAccessedIndex = -1;

			// Token: 0x0200088C RID: 2188
			private class ControlCollectionEnumerator : IEnumerator
			{
				// Token: 0x0600709A RID: 28826 RVA: 0x0019BB44 File Offset: 0x00199D44
				public ControlCollectionEnumerator(Control.ControlCollection controls)
				{
					this.controls = controls;
					this.originalCount = controls.Count;
					this.current = -1;
				}

				// Token: 0x0600709B RID: 28827 RVA: 0x0019BB66 File Offset: 0x00199D66
				public bool MoveNext()
				{
					if (this.current < this.controls.Count - 1 && this.current < this.originalCount - 1)
					{
						this.current++;
						return true;
					}
					return false;
				}

				// Token: 0x0600709C RID: 28828 RVA: 0x0019BB9E File Offset: 0x00199D9E
				public void Reset()
				{
					this.current = -1;
				}

				// Token: 0x17001879 RID: 6265
				// (get) Token: 0x0600709D RID: 28829 RVA: 0x0019BBA7 File Offset: 0x00199DA7
				public object Current
				{
					get
					{
						if (this.current == -1)
						{
							return null;
						}
						return this.controls[this.current];
					}
				}

				// Token: 0x040043E6 RID: 17382
				private Control.ControlCollection controls;

				// Token: 0x040043E7 RID: 17383
				private int current;

				// Token: 0x040043E8 RID: 17384
				private int originalCount;
			}
		}

		// Token: 0x0200057C RID: 1404
		private class ActiveXImpl : MarshalByRefObject, IWindowTarget
		{
			// Token: 0x0600573A RID: 22330 RVA: 0x0016D270 File Offset: 0x0016B470
			internal ActiveXImpl(Control control)
			{
				this.control = control;
				this.controlWindowTarget = control.WindowTarget;
				control.WindowTarget = this;
				this.adviseList = new ArrayList();
				this.activeXState = default(BitVector32);
				this.ambientProperties = new Control.AmbientProperty[]
				{
					new Control.AmbientProperty("Font", -703),
					new Control.AmbientProperty("BackColor", -701),
					new Control.AmbientProperty("ForeColor", -704)
				};
			}

			// Token: 0x170014CC RID: 5324
			// (get) Token: 0x0600573B RID: 22331 RVA: 0x0016D300 File Offset: 0x0016B500
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			internal Color AmbientBackColor
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-701);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-701, ref obj) && obj != null)
						{
							try
							{
								ambientProperty.Value = ColorTranslator.FromOle(Convert.ToInt32(obj, CultureInfo.InvariantCulture));
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
							}
						}
					}
					if (ambientProperty.Value == null)
					{
						return Color.Empty;
					}
					return (Color)ambientProperty.Value;
				}
			}

			// Token: 0x170014CD RID: 5325
			// (get) Token: 0x0600573C RID: 22332 RVA: 0x0016D388 File Offset: 0x0016B588
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			internal Font AmbientFont
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-703);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-703, ref obj))
						{
							try
							{
								IntPtr hfont = IntPtr.Zero;
								UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)obj;
								IntSecurity.ObjectFromWin32Handle.Assert();
								Font value = null;
								try
								{
									hfont = font.GetHFont();
									value = Font.FromHfont(hfont);
								}
								finally
								{
									CodeAccessPermission.RevertAssert();
								}
								ambientProperty.Value = value;
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
								ambientProperty.Value = null;
							}
						}
					}
					return (Font)ambientProperty.Value;
				}
			}

			// Token: 0x170014CE RID: 5326
			// (get) Token: 0x0600573D RID: 22333 RVA: 0x0016D434 File Offset: 0x0016B634
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			internal Color AmbientForeColor
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-704);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-704, ref obj) && obj != null)
						{
							try
							{
								ambientProperty.Value = ColorTranslator.FromOle(Convert.ToInt32(obj, CultureInfo.InvariantCulture));
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
							}
						}
					}
					if (ambientProperty.Value == null)
					{
						return Color.Empty;
					}
					return (Color)ambientProperty.Value;
				}
			}

			// Token: 0x170014CF RID: 5327
			// (get) Token: 0x0600573E RID: 22334 RVA: 0x0016D4BC File Offset: 0x0016B6BC
			// (set) Token: 0x0600573F RID: 22335 RVA: 0x0016D4CE File Offset: 0x0016B6CE
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			internal bool EventsFrozen
			{
				get
				{
					return this.activeXState[Control.ActiveXImpl.eventsFrozen];
				}
				set
				{
					this.activeXState[Control.ActiveXImpl.eventsFrozen] = value;
				}
			}

			// Token: 0x170014D0 RID: 5328
			// (get) Token: 0x06005740 RID: 22336 RVA: 0x0016D4E1 File Offset: 0x0016B6E1
			internal IntPtr HWNDParent
			{
				get
				{
					return this.hwndParent;
				}
			}

			// Token: 0x170014D1 RID: 5329
			// (get) Token: 0x06005741 RID: 22337 RVA: 0x0016D4EC File Offset: 0x0016B6EC
			internal bool IsIE
			{
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (!Control.ActiveXImpl.checkedIE)
					{
						if (this.clientSite == null)
						{
							return false;
						}
						if (Assembly.GetEntryAssembly() == null)
						{
							UnsafeNativeMethods.IOleContainer oleContainer;
							if (NativeMethods.Succeeded(this.clientSite.GetContainer(out oleContainer)) && oleContainer is NativeMethods.IHTMLDocument)
							{
								Control.ActiveXImpl.isIE = true;
							}
							if (oleContainer != null && UnsafeNativeMethods.IsComObject(oleContainer))
							{
								UnsafeNativeMethods.ReleaseComObject(oleContainer);
							}
						}
						Control.ActiveXImpl.checkedIE = true;
					}
					return Control.ActiveXImpl.isIE;
				}
			}

			// Token: 0x170014D2 RID: 5330
			// (get) Token: 0x06005742 RID: 22338 RVA: 0x0016D558 File Offset: 0x0016B758
			private Point LogPixels
			{
				get
				{
					if (Control.ActiveXImpl.logPixels.IsEmpty)
					{
						Control.ActiveXImpl.logPixels = default(Point);
						IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
						Control.ActiveXImpl.logPixels.X = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						Control.ActiveXImpl.logPixels.Y = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
					return Control.ActiveXImpl.logPixels;
				}
			}

			// Token: 0x06005743 RID: 22339 RVA: 0x0016D5CE File Offset: 0x0016B7CE
			internal int Advise(IAdviseSink pAdvSink)
			{
				this.adviseList.Add(pAdvSink);
				return this.adviseList.Count;
			}

			// Token: 0x06005744 RID: 22340 RVA: 0x0016D5E8 File Offset: 0x0016B7E8
			internal void Close(int dwSaveOption)
			{
				if (this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					this.InPlaceDeactivate();
				}
				if ((dwSaveOption == 0 || dwSaveOption == 2) && this.activeXState[Control.ActiveXImpl.isDirty])
				{
					if (this.clientSite != null)
					{
						this.clientSite.SaveObject();
					}
					this.SendOnSave();
				}
			}

			// Token: 0x06005745 RID: 22341 RVA: 0x0016D640 File Offset: 0x0016B840
			internal void DoVerb(int iVerb, IntPtr lpmsg, UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, NativeMethods.COMRECT lprcPosRect)
			{
				switch (iVerb)
				{
				case -5:
				case -4:
				case -1:
				case 0:
				{
					this.InPlaceActivate(iVerb);
					if (!(lpmsg != IntPtr.Zero))
					{
						return;
					}
					NativeMethods.MSG msg = (NativeMethods.MSG)UnsafeNativeMethods.PtrToStructure(lpmsg, typeof(NativeMethods.MSG));
					Control control = this.control;
					if (msg.hwnd != this.control.Handle && msg.message >= 512 && msg.message <= 522)
					{
						IntPtr handle = (msg.hwnd == IntPtr.Zero) ? hwndParent : msg.hwnd;
						NativeMethods.POINT point = new NativeMethods.POINT();
						point.x = NativeMethods.Util.LOWORD(msg.lParam);
						point.y = NativeMethods.Util.HIWORD(msg.lParam);
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(null, handle), new HandleRef(this.control, this.control.Handle), point, 1);
						Control childAtPoint = control.GetChildAtPoint(new Point(point.x, point.y));
						if (childAtPoint != null && childAtPoint != control)
						{
							UnsafeNativeMethods.MapWindowPoints(new HandleRef(control, control.Handle), new HandleRef(childAtPoint, childAtPoint.Handle), point, 1);
							control = childAtPoint;
						}
						msg.lParam = NativeMethods.Util.MAKELPARAM(point.x, point.y);
					}
					if (msg.message == 256 && msg.wParam == (IntPtr)9)
					{
						control.SelectNextControl(null, Control.ModifierKeys != Keys.Shift, true, true, true);
						return;
					}
					control.SendMessage(msg.message, msg.wParam, msg.lParam);
					return;
				}
				case -3:
					this.UIDeactivate();
					this.InPlaceDeactivate();
					if (this.activeXState[Control.ActiveXImpl.inPlaceVisible])
					{
						this.SetInPlaceVisible(false);
						return;
					}
					return;
				}
				Control.ActiveXImpl.ThrowHr(-2147467263);
			}

			// Token: 0x06005746 RID: 22342 RVA: 0x0016D834 File Offset: 0x0016BA34
			internal void Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT prcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
			{
				if (dwDrawAspect != 1 && dwDrawAspect != 16 && dwDrawAspect != 32)
				{
					Control.ActiveXImpl.ThrowHr(-2147221397);
				}
				int objectType = UnsafeNativeMethods.GetObjectType(new HandleRef(null, hdcDraw));
				if (objectType == 4)
				{
					Control.ActiveXImpl.ThrowHr(-2147221184);
				}
				NativeMethods.POINT point = new NativeMethods.POINT();
				NativeMethods.POINT point2 = new NativeMethods.POINT();
				NativeMethods.SIZE size = new NativeMethods.SIZE();
				NativeMethods.SIZE size2 = new NativeMethods.SIZE();
				int nMapMode = 1;
				if (!this.control.IsHandleCreated)
				{
					this.control.CreateHandle();
				}
				if (prcBounds != null)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(prcBounds.left, prcBounds.top, prcBounds.right, prcBounds.bottom);
					SafeNativeMethods.LPtoDP(new HandleRef(null, hdcDraw), ref rect, 2);
					nMapMode = SafeNativeMethods.SetMapMode(new HandleRef(null, hdcDraw), 8);
					SafeNativeMethods.SetWindowOrgEx(new HandleRef(null, hdcDraw), 0, 0, point2);
					SafeNativeMethods.SetWindowExtEx(new HandleRef(null, hdcDraw), this.control.Width, this.control.Height, size);
					SafeNativeMethods.SetViewportOrgEx(new HandleRef(null, hdcDraw), rect.left, rect.top, point);
					SafeNativeMethods.SetViewportExtEx(new HandleRef(null, hdcDraw), rect.right - rect.left, rect.bottom - rect.top, size2);
				}
				try
				{
					IntPtr intPtr = (IntPtr)30;
					if (objectType != 12)
					{
						this.control.SendMessage(791, hdcDraw, intPtr);
					}
					else
					{
						this.control.PrintToMetaFile(new HandleRef(null, hdcDraw), intPtr);
					}
				}
				finally
				{
					if (prcBounds != null)
					{
						SafeNativeMethods.SetWindowOrgEx(new HandleRef(null, hdcDraw), point2.x, point2.y, null);
						SafeNativeMethods.SetWindowExtEx(new HandleRef(null, hdcDraw), size.cx, size.cy, null);
						SafeNativeMethods.SetViewportOrgEx(new HandleRef(null, hdcDraw), point.x, point.y, null);
						SafeNativeMethods.SetViewportExtEx(new HandleRef(null, hdcDraw), size2.cx, size2.cy, null);
						SafeNativeMethods.SetMapMode(new HandleRef(null, hdcDraw), nMapMode);
					}
				}
			}

			// Token: 0x06005747 RID: 22343 RVA: 0x0016DA4C File Offset: 0x0016BC4C
			internal static int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e)
			{
				if (Control.ActiveXImpl.axVerbs == null)
				{
					NativeMethods.tagOLEVERB tagOLEVERB = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB2 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB3 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB4 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB5 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB6 = new NativeMethods.tagOLEVERB();
					tagOLEVERB.lVerb = -1;
					tagOLEVERB2.lVerb = -5;
					tagOLEVERB3.lVerb = -4;
					tagOLEVERB4.lVerb = -3;
					tagOLEVERB5.lVerb = 0;
					tagOLEVERB6.lVerb = -7;
					tagOLEVERB6.lpszVerbName = SR.GetString("AXProperties");
					tagOLEVERB6.grfAttribs = 2;
					Control.ActiveXImpl.axVerbs = new NativeMethods.tagOLEVERB[]
					{
						tagOLEVERB,
						tagOLEVERB2,
						tagOLEVERB3,
						tagOLEVERB4,
						tagOLEVERB5
					};
				}
				e = new Control.ActiveXVerbEnum(Control.ActiveXImpl.axVerbs);
				return 0;
			}

			// Token: 0x06005748 RID: 22344 RVA: 0x0016DB00 File Offset: 0x0016BD00
			private static byte[] FromBase64WrappedString(string text)
			{
				if (text.IndexOfAny(new char[]
				{
					' ',
					'\r',
					'\n'
				}) != -1)
				{
					StringBuilder stringBuilder = new StringBuilder(text.Length);
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c != '\n' && c != '\r' && c != ' ')
						{
							stringBuilder.Append(text[i]);
						}
					}
					return Convert.FromBase64String(stringBuilder.ToString());
				}
				return Convert.FromBase64String(text);
			}

			// Token: 0x06005749 RID: 22345 RVA: 0x0016DB7C File Offset: 0x0016BD7C
			internal void GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
			{
				if (paspects != null)
				{
					paspects[0] = 1;
				}
				if (padvf != null)
				{
					padvf[0] = 0;
					if (this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce])
					{
						padvf[0] |= 4;
					}
					if (this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst])
					{
						padvf[0] |= 2;
					}
				}
				if (pAdvSink != null)
				{
					pAdvSink[0] = this.viewAdviseSink;
				}
			}

			// Token: 0x0600574A RID: 22346 RVA: 0x0016DBE0 File Offset: 0x0016BDE0
			private bool GetAmbientProperty(int dispid, ref object obj)
			{
				if (this.clientSite is UnsafeNativeMethods.IDispatch)
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)this.clientSite;
					object[] array = new object[1];
					Guid empty = Guid.Empty;
					int hr = -2147467259;
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						hr = dispatch.Invoke(dispid, ref empty, NativeMethods.LOCALE_USER_DEFAULT, 2, new NativeMethods.tagDISPPARAMS(), array, null, null);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Succeeded(hr))
					{
						obj = array[0];
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600574B RID: 22347 RVA: 0x0016DC64 File Offset: 0x0016BE64
			internal UnsafeNativeMethods.IOleClientSite GetClientSite()
			{
				return this.clientSite;
			}

			// Token: 0x0600574C RID: 22348 RVA: 0x0016DC6C File Offset: 0x0016BE6C
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal int GetControlInfo(NativeMethods.tagCONTROLINFO pCI)
			{
				if (this.accelCount == -1)
				{
					ArrayList arrayList = new ArrayList();
					this.GetMnemonicList(this.control, arrayList);
					this.accelCount = (short)arrayList.Count;
					if (this.accelCount > 0)
					{
						int num = UnsafeNativeMethods.SizeOf(typeof(NativeMethods.ACCEL));
						IntPtr intPtr = Marshal.AllocHGlobal(num * (int)this.accelCount * 2);
						try
						{
							NativeMethods.ACCEL accel = new NativeMethods.ACCEL();
							accel.cmd = 0;
							this.accelCount = 0;
							foreach (object obj in arrayList)
							{
								char c = (char)obj;
								IntPtr intPtr2 = (IntPtr)((long)intPtr + (long)((int)this.accelCount * num));
								if (c >= 'A' && c <= 'Z')
								{
									accel.fVirt = 17;
									accel.key = (UnsafeNativeMethods.VkKeyScan(c) & 255);
									Marshal.StructureToPtr(accel, intPtr2, false);
									this.accelCount += 1;
									intPtr2 = (IntPtr)((long)intPtr2 + (long)num);
									accel.fVirt = 21;
									Marshal.StructureToPtr(accel, intPtr2, false);
								}
								else
								{
									accel.fVirt = 17;
									short num2 = UnsafeNativeMethods.VkKeyScan(c);
									if ((num2 & 256) != 0)
									{
										NativeMethods.ACCEL accel2 = accel;
										accel2.fVirt |= 4;
									}
									accel.key = (num2 & 255);
									Marshal.StructureToPtr(accel, intPtr2, false);
								}
								NativeMethods.ACCEL accel3 = accel;
								accel3.cmd += 1;
								this.accelCount += 1;
							}
							if (this.accelTable != IntPtr.Zero)
							{
								UnsafeNativeMethods.DestroyAcceleratorTable(new HandleRef(this, this.accelTable));
								this.accelTable = IntPtr.Zero;
							}
							this.accelTable = UnsafeNativeMethods.CreateAcceleratorTable(new HandleRef(null, intPtr), (int)this.accelCount);
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								Marshal.FreeHGlobal(intPtr);
							}
						}
					}
				}
				pCI.cAccel = this.accelCount;
				pCI.hAccel = this.accelTable;
				return 0;
			}

			// Token: 0x0600574D RID: 22349 RVA: 0x0016DEA8 File Offset: 0x0016C0A8
			internal void GetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
			{
				if ((dwDrawAspect & 1) != 0)
				{
					Size size = this.control.Size;
					Point point = this.PixelToHiMetric(size.Width, size.Height);
					pSizel.cx = point.X;
					pSizel.cy = point.Y;
					return;
				}
				Control.ActiveXImpl.ThrowHr(-2147221397);
			}

			// Token: 0x0600574E RID: 22350 RVA: 0x0016DF00 File Offset: 0x0016C100
			private void GetMnemonicList(Control control, ArrayList mnemonicList)
			{
				char mnemonic = WindowsFormsUtils.GetMnemonic(control.Text, true);
				if (mnemonic != '\0')
				{
					mnemonicList.Add(mnemonic);
				}
				foreach (object obj in control.Controls)
				{
					Control control2 = (Control)obj;
					if (control2 != null)
					{
						this.GetMnemonicList(control2, mnemonicList);
					}
				}
			}

			// Token: 0x0600574F RID: 22351 RVA: 0x0016DF7C File Offset: 0x0016C17C
			private string GetStreamName()
			{
				string text = this.control.GetType().FullName;
				int length = text.Length;
				if (length > 31)
				{
					text = text.Substring(length - 31);
				}
				return text;
			}

			// Token: 0x06005750 RID: 22352 RVA: 0x0016DFB2 File Offset: 0x0016C1B2
			internal int GetWindow(out IntPtr hwnd)
			{
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					hwnd = IntPtr.Zero;
					return -2147467259;
				}
				hwnd = this.control.Handle;
				return 0;
			}

			// Token: 0x06005751 RID: 22353 RVA: 0x0016DFE4 File Offset: 0x0016C1E4
			private Point HiMetricToPixel(int x, int y)
			{
				return new Point
				{
					X = (this.LogPixels.X * x + Control.ActiveXImpl.hiMetricPerInch / 2) / Control.ActiveXImpl.hiMetricPerInch,
					Y = (this.LogPixels.Y * y + Control.ActiveXImpl.hiMetricPerInch / 2) / Control.ActiveXImpl.hiMetricPerInch
				};
			}

			// Token: 0x06005752 RID: 22354 RVA: 0x0016E044 File Offset: 0x0016C244
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void InPlaceActivate(int verb)
			{
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite == null)
				{
					return;
				}
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					int num = oleInPlaceSite.CanInPlaceActivate();
					if (num != 0)
					{
						if (NativeMethods.Succeeded(num))
						{
							num = -2147467259;
						}
						Control.ActiveXImpl.ThrowHr(num);
					}
					oleInPlaceSite.OnInPlaceActivate();
					this.activeXState[Control.ActiveXImpl.inPlaceActive] = true;
				}
				if (!this.activeXState[Control.ActiveXImpl.inPlaceVisible])
				{
					NativeMethods.tagOIFI tagOIFI = new NativeMethods.tagOIFI();
					tagOIFI.cb = UnsafeNativeMethods.SizeOf(typeof(NativeMethods.tagOIFI));
					IntPtr handle = IntPtr.Zero;
					handle = oleInPlaceSite.GetWindow();
					NativeMethods.COMRECT lprcPosRect = new NativeMethods.COMRECT();
					NativeMethods.COMRECT lprcClipRect = new NativeMethods.COMRECT();
					if (this.inPlaceUiWindow != null && UnsafeNativeMethods.IsComObject(this.inPlaceUiWindow))
					{
						UnsafeNativeMethods.ReleaseComObject(this.inPlaceUiWindow);
						this.inPlaceUiWindow = null;
					}
					if (this.inPlaceFrame != null && UnsafeNativeMethods.IsComObject(this.inPlaceFrame))
					{
						UnsafeNativeMethods.ReleaseComObject(this.inPlaceFrame);
						this.inPlaceFrame = null;
					}
					UnsafeNativeMethods.IOleInPlaceFrame oleInPlaceFrame;
					UnsafeNativeMethods.IOleInPlaceUIWindow oleInPlaceUIWindow;
					oleInPlaceSite.GetWindowContext(out oleInPlaceFrame, out oleInPlaceUIWindow, lprcPosRect, lprcClipRect, tagOIFI);
					this.SetObjectRects(lprcPosRect, lprcClipRect);
					this.inPlaceFrame = oleInPlaceFrame;
					this.inPlaceUiWindow = oleInPlaceUIWindow;
					this.hwndParent = handle;
					UnsafeNativeMethods.SetParent(new HandleRef(this.control, this.control.Handle), new HandleRef(null, handle));
					this.control.CreateControl();
					this.clientSite.ShowObject();
					this.SetInPlaceVisible(true);
				}
				if (verb != 0 && verb != -4)
				{
					return;
				}
				if (!this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.activeXState[Control.ActiveXImpl.uiActive] = true;
					oleInPlaceSite.OnUIActivate();
					if (!this.control.ContainsFocus)
					{
						this.control.FocusInternal();
					}
					this.inPlaceFrame.SetActiveObject(this.control, null);
					if (this.inPlaceUiWindow != null)
					{
						this.inPlaceUiWindow.SetActiveObject(this.control, null);
					}
					int num2 = this.inPlaceFrame.SetBorderSpace(null);
					if (NativeMethods.Failed(num2) && num2 != -2147221491 && num2 != -2147221087 && num2 != -2147467263)
					{
						UnsafeNativeMethods.ThrowExceptionForHR(num2);
					}
					if (this.inPlaceUiWindow != null)
					{
						num2 = this.inPlaceFrame.SetBorderSpace(null);
						if (NativeMethods.Failed(num2) && num2 != -2147221491 && num2 != -2147221087 && num2 != -2147467263)
						{
							UnsafeNativeMethods.ThrowExceptionForHR(num2);
						}
					}
				}
			}

			// Token: 0x06005753 RID: 22355 RVA: 0x0016E2AC File Offset: 0x0016C4AC
			internal void InPlaceDeactivate()
			{
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					return;
				}
				if (this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.UIDeactivate();
				}
				this.activeXState[Control.ActiveXImpl.inPlaceActive] = false;
				this.activeXState[Control.ActiveXImpl.inPlaceVisible] = false;
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						oleInPlaceSite.OnInPlaceDeactivate();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				this.control.Visible = false;
				this.hwndParent = IntPtr.Zero;
				if (this.inPlaceUiWindow != null && UnsafeNativeMethods.IsComObject(this.inPlaceUiWindow))
				{
					UnsafeNativeMethods.ReleaseComObject(this.inPlaceUiWindow);
					this.inPlaceUiWindow = null;
				}
				if (this.inPlaceFrame != null && UnsafeNativeMethods.IsComObject(this.inPlaceFrame))
				{
					UnsafeNativeMethods.ReleaseComObject(this.inPlaceFrame);
					this.inPlaceFrame = null;
				}
			}

			// Token: 0x06005754 RID: 22356 RVA: 0x0016E3A8 File Offset: 0x0016C5A8
			internal int IsDirty()
			{
				if (this.activeXState[Control.ActiveXImpl.isDirty])
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06005755 RID: 22357 RVA: 0x0016E3C0 File Offset: 0x0016C5C0
			private bool IsResourceProp(PropertyDescriptor prop)
			{
				TypeConverter converter = prop.Converter;
				Type[] array = new Type[]
				{
					typeof(string),
					typeof(byte[])
				};
				foreach (Type type in array)
				{
					if (converter.CanConvertTo(type) && converter.CanConvertFrom(type))
					{
						return false;
					}
				}
				return prop.GetValue(this.control) is ISerializable;
			}

			// Token: 0x06005756 RID: 22358 RVA: 0x0016E434 File Offset: 0x0016C634
			internal void Load(UnsafeNativeMethods.IStorage stg)
			{
				UnsafeNativeMethods.IStream stream = null;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					stream = stg.OpenStream(this.GetStreamName(), IntPtr.Zero, 16, 0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147287038)
					{
						throw;
					}
					stream = stg.OpenStream(base.GetType().FullName, IntPtr.Zero, 16, 0);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.Load(stream);
				stream = null;
				if (UnsafeNativeMethods.IsComObject(stg))
				{
					UnsafeNativeMethods.ReleaseComObject(stg);
				}
			}

			// Token: 0x06005757 RID: 22359 RVA: 0x0016E4CC File Offset: 0x0016C6CC
			internal void Load(UnsafeNativeMethods.IStream stream)
			{
				Control.ActiveXImpl.PropertyBagStream propertyBagStream = new Control.ActiveXImpl.PropertyBagStream();
				propertyBagStream.Read(stream);
				this.Load(propertyBagStream, null);
				if (UnsafeNativeMethods.IsComObject(stream))
				{
					UnsafeNativeMethods.ReleaseComObject(stream);
				}
			}

			// Token: 0x06005758 RID: 22360 RVA: 0x0016E500 File Offset: 0x0016C700
			internal void Load(UnsafeNativeMethods.IPropertyBag pPropBag, UnsafeNativeMethods.IErrorLog pErrorLog)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.control, new Attribute[]
				{
					DesignerSerializationVisibilityAttribute.Visible
				});
				for (int i = 0; i < properties.Count; i++)
				{
					try
					{
						object obj = null;
						int hr = -2147467259;
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							hr = pPropBag.Read(properties[i].Name, ref obj, pErrorLog);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (NativeMethods.Succeeded(hr) && obj != null)
						{
							string text = null;
							int scode = 0;
							try
							{
								if (obj.GetType() != typeof(string))
								{
									obj = Convert.ToString(obj, CultureInfo.InvariantCulture);
								}
								if (this.IsResourceProp(properties[i]))
								{
									byte[] buffer = Convert.FromBase64String(obj.ToString());
									MemoryStream serializationStream = new MemoryStream(buffer);
									BinaryFormatter binaryFormatter = new BinaryFormatter();
									properties[i].SetValue(this.control, binaryFormatter.Deserialize(serializationStream));
								}
								else
								{
									TypeConverter converter = properties[i].Converter;
									object value = null;
									if (converter.CanConvertFrom(typeof(string)))
									{
										value = converter.ConvertFromInvariantString(obj.ToString());
									}
									else if (converter.CanConvertFrom(typeof(byte[])))
									{
										string text2 = obj.ToString();
										value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, Control.ActiveXImpl.FromBase64WrappedString(text2));
									}
									properties[i].SetValue(this.control, value);
								}
							}
							catch (Exception ex)
							{
								text = ex.ToString();
								if (ex is ExternalException)
								{
									scode = ((ExternalException)ex).ErrorCode;
								}
								else
								{
									scode = -2147467259;
								}
							}
							if (text != null && pErrorLog != null)
							{
								NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
								tagEXCEPINFO.bstrSource = this.control.GetType().FullName;
								tagEXCEPINFO.bstrDescription = text;
								tagEXCEPINFO.scode = scode;
								pErrorLog.AddError(properties[i].Name, tagEXCEPINFO);
							}
						}
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex2))
						{
							throw;
						}
					}
				}
				if (UnsafeNativeMethods.IsComObject(pPropBag))
				{
					UnsafeNativeMethods.ReleaseComObject(pPropBag);
				}
			}

			// Token: 0x06005759 RID: 22361 RVA: 0x0016E754 File Offset: 0x0016C954
			private Control.AmbientProperty LookupAmbient(int dispid)
			{
				for (int i = 0; i < this.ambientProperties.Length; i++)
				{
					if (this.ambientProperties[i].DispID == dispid)
					{
						return this.ambientProperties[i];
					}
				}
				return this.ambientProperties[0];
			}

			// Token: 0x0600575A RID: 22362 RVA: 0x0016E798 File Offset: 0x0016C998
			internal IntPtr MergeRegion(IntPtr region)
			{
				if (this.clipRegion == IntPtr.Zero)
				{
					return region;
				}
				if (region == IntPtr.Zero)
				{
					return this.clipRegion;
				}
				IntPtr result;
				try
				{
					IntPtr intPtr = SafeNativeMethods.CreateRectRgn(0, 0, 0, 0);
					try
					{
						SafeNativeMethods.CombineRgn(new HandleRef(null, intPtr), new HandleRef(null, region), new HandleRef(this, this.clipRegion), 4);
						SafeNativeMethods.DeleteObject(new HandleRef(null, region));
					}
					catch
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						throw;
					}
					result = intPtr;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					result = region;
				}
				return result;
			}

			// Token: 0x0600575B RID: 22363 RVA: 0x0016E848 File Offset: 0x0016CA48
			private void CallParentPropertyChanged(Control control, string propName)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(propName);
				if (num <= 2626085950U)
				{
					if (num <= 777198197U)
					{
						if (num != 41545325U)
						{
							if (num != 777198197U)
							{
								return;
							}
							if (!(propName == "BackColor"))
							{
								return;
							}
							control.OnParentBackColorChanged(EventArgs.Empty);
							return;
						}
						else
						{
							if (!(propName == "BindingContext"))
							{
								return;
							}
							control.OnParentBindingContextChanged(EventArgs.Empty);
							return;
						}
					}
					else if (num != 1495943489U)
					{
						if (num != 2626085950U)
						{
							return;
						}
						if (!(propName == "Enabled"))
						{
							return;
						}
						control.OnParentEnabledChanged(EventArgs.Empty);
						return;
					}
					else
					{
						if (!(propName == "Visible"))
						{
							return;
						}
						control.OnParentVisibleChanged(EventArgs.Empty);
						return;
					}
				}
				else if (num <= 2936102910U)
				{
					if (num != 2809814704U)
					{
						if (num != 2936102910U)
						{
							return;
						}
						if (!(propName == "ForeColor"))
						{
							return;
						}
						control.OnParentForeColorChanged(EventArgs.Empty);
						return;
					}
					else
					{
						if (!(propName == "Font"))
						{
							return;
						}
						control.OnParentFontChanged(EventArgs.Empty);
						return;
					}
				}
				else if (num != 3049818181U)
				{
					if (num != 3770400898U)
					{
						return;
					}
					if (!(propName == "BackgroundImage"))
					{
						return;
					}
					control.OnParentBackgroundImageChanged(EventArgs.Empty);
					return;
				}
				else
				{
					if (!(propName == "RightToLeft"))
					{
						return;
					}
					control.OnParentRightToLeftChanged(EventArgs.Empty);
					return;
				}
			}

			// Token: 0x0600575C RID: 22364 RVA: 0x0016E98C File Offset: 0x0016CB8C
			internal void OnAmbientPropertyChange(int dispID)
			{
				if (dispID != -1)
				{
					for (int i = 0; i < this.ambientProperties.Length; i++)
					{
						if (this.ambientProperties[i].DispID == dispID)
						{
							this.ambientProperties[i].ResetValue();
							this.CallParentPropertyChanged(this.control, this.ambientProperties[i].Name);
							return;
						}
					}
					object obj = new object();
					if (dispID != -713)
					{
						if (dispID == -710 && this.GetAmbientProperty(-710, ref obj))
						{
							this.activeXState[Control.ActiveXImpl.uiDead] = (bool)obj;
							return;
						}
					}
					else
					{
						IButtonControl buttonControl = this.control as IButtonControl;
						if (buttonControl != null && this.GetAmbientProperty(-713, ref obj))
						{
							buttonControl.NotifyDefault((bool)obj);
							return;
						}
					}
				}
				else
				{
					for (int j = 0; j < this.ambientProperties.Length; j++)
					{
						this.ambientProperties[j].ResetValue();
						this.CallParentPropertyChanged(this.control, this.ambientProperties[j].Name);
					}
				}
			}

			// Token: 0x0600575D RID: 22365 RVA: 0x0016EA90 File Offset: 0x0016CC90
			internal void OnDocWindowActivate(int fActivate)
			{
				if (this.activeXState[Control.ActiveXImpl.uiActive] && fActivate != 0 && this.inPlaceFrame != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					int num;
					try
					{
						num = this.inPlaceFrame.SetBorderSpace(null);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Failed(num) && num != -2147221087 && num != -2147467263)
					{
						UnsafeNativeMethods.ThrowExceptionForHR(num);
					}
				}
			}

			// Token: 0x0600575E RID: 22366 RVA: 0x0016EB08 File Offset: 0x0016CD08
			internal void OnFocus(bool focus)
			{
				if (this.activeXState[Control.ActiveXImpl.inPlaceActive] && this.clientSite is UnsafeNativeMethods.IOleControlSite)
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						((UnsafeNativeMethods.IOleControlSite)this.clientSite).OnFocus(focus ? 1 : 0);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				if (focus && this.activeXState[Control.ActiveXImpl.inPlaceActive] && !this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.InPlaceActivate(-4);
				}
			}

			// Token: 0x0600575F RID: 22367 RVA: 0x0016EBA0 File Offset: 0x0016CDA0
			private Point PixelToHiMetric(int x, int y)
			{
				return new Point
				{
					X = (Control.ActiveXImpl.hiMetricPerInch * x + (this.LogPixels.X >> 1)) / this.LogPixels.X,
					Y = (Control.ActiveXImpl.hiMetricPerInch * y + (this.LogPixels.Y >> 1)) / this.LogPixels.Y
				};
			}

			// Token: 0x06005760 RID: 22368 RVA: 0x0016EC14 File Offset: 0x0016CE14
			internal void QuickActivate(UnsafeNativeMethods.tagQACONTAINER pQaContainer, UnsafeNativeMethods.tagQACONTROL pQaControl)
			{
				Control.AmbientProperty ambientProperty = this.LookupAmbient(-701);
				ambientProperty.Value = ColorTranslator.FromOle((int)pQaContainer.colorBack);
				ambientProperty = this.LookupAmbient(-704);
				ambientProperty.Value = ColorTranslator.FromOle((int)pQaContainer.colorFore);
				if (pQaContainer.pFont != null)
				{
					ambientProperty = this.LookupAmbient(-703);
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						IntPtr hfont = IntPtr.Zero;
						object pFont = pQaContainer.pFont;
						UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)pFont;
						hfont = font.GetHFont();
						Font value = Font.FromHfont(hfont);
						ambientProperty.Value = value;
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
						ambientProperty.Value = null;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				pQaControl.cbSize = UnsafeNativeMethods.SizeOf(typeof(UnsafeNativeMethods.tagQACONTROL));
				this.SetClientSite(pQaContainer.pClientSite);
				if (pQaContainer.pAdviseSink != null)
				{
					this.SetAdvise(1, 0, (IAdviseSink)pQaContainer.pAdviseSink);
				}
				IntSecurity.UnmanagedCode.Assert();
				int dwMiscStatus;
				try
				{
					((UnsafeNativeMethods.IOleObject)this.control).GetMiscStatus(1, out dwMiscStatus);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				pQaControl.dwMiscStatus = dwMiscStatus;
				if (pQaContainer.pUnkEventSink != null && this.control is UserControl)
				{
					Type defaultEventsInterface = Control.ActiveXImpl.GetDefaultEventsInterface(this.control.GetType());
					if (defaultEventsInterface != null)
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							Control.ActiveXImpl.AdviseHelper.AdviseConnectionPoint(this.control, pQaContainer.pUnkEventSink, defaultEventsInterface, out pQaControl.dwEventCookie);
						}
						catch (Exception ex2)
						{
							if (ClientUtils.IsSecurityOrCriticalException(ex2))
							{
								throw;
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				if (pQaContainer.pPropertyNotifySink != null && UnsafeNativeMethods.IsComObject(pQaContainer.pPropertyNotifySink))
				{
					UnsafeNativeMethods.ReleaseComObject(pQaContainer.pPropertyNotifySink);
				}
				if (pQaContainer.pUnkEventSink != null && UnsafeNativeMethods.IsComObject(pQaContainer.pUnkEventSink))
				{
					UnsafeNativeMethods.ReleaseComObject(pQaContainer.pUnkEventSink);
				}
			}

			// Token: 0x06005761 RID: 22369 RVA: 0x0016EE20 File Offset: 0x0016D020
			private static Type GetDefaultEventsInterface(Type controlType)
			{
				Type type = null;
				object[] customAttributes = controlType.GetCustomAttributes(typeof(ComSourceInterfacesAttribute), false);
				if (customAttributes.Length != 0)
				{
					ComSourceInterfacesAttribute comSourceInterfacesAttribute = (ComSourceInterfacesAttribute)customAttributes[0];
					string text = comSourceInterfacesAttribute.Value.Split(new char[1])[0];
					type = controlType.Module.Assembly.GetType(text, false);
					if (type == null)
					{
						type = Type.GetType(text, false);
					}
				}
				return type;
			}

			// Token: 0x06005762 RID: 22370 RVA: 0x0016EE88 File Offset: 0x0016D088
			internal void Save(UnsafeNativeMethods.IStorage stg, bool fSameAsLoad)
			{
				UnsafeNativeMethods.IStream stream = null;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					stream = stg.CreateStream(this.GetStreamName(), 4113, 0, 0);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.Save(stream, true);
				UnsafeNativeMethods.ReleaseComObject(stream);
			}

			// Token: 0x06005763 RID: 22371 RVA: 0x0016EEDC File Offset: 0x0016D0DC
			internal void Save(UnsafeNativeMethods.IStream stream, bool fClearDirty)
			{
				Control.ActiveXImpl.PropertyBagStream propertyBagStream = new Control.ActiveXImpl.PropertyBagStream();
				this.Save(propertyBagStream, fClearDirty, false);
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					propertyBagStream.Write(stream);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (UnsafeNativeMethods.IsComObject(stream))
				{
					UnsafeNativeMethods.ReleaseComObject(stream);
				}
			}

			// Token: 0x06005764 RID: 22372 RVA: 0x0016EF30 File Offset: 0x0016D130
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void Save(UnsafeNativeMethods.IPropertyBag pPropBag, bool fClearDirty, bool fSaveAllProperties)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.control, new Attribute[]
				{
					DesignerSerializationVisibilityAttribute.Visible
				});
				for (int i = 0; i < properties.Count; i++)
				{
					if (fSaveAllProperties || properties[i].ShouldSerializeValue(this.control))
					{
						if (this.IsResourceProp(properties[i]))
						{
							MemoryStream memoryStream = new MemoryStream();
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							binaryFormatter.Serialize(memoryStream, properties[i].GetValue(this.control));
							byte[] array = new byte[(int)memoryStream.Length];
							memoryStream.Position = 0L;
							memoryStream.Read(array, 0, array.Length);
							object obj = Convert.ToBase64String(array);
							pPropBag.Write(properties[i].Name, ref obj);
						}
						else
						{
							TypeConverter converter = properties[i].Converter;
							if (converter.CanConvertFrom(typeof(string)))
							{
								object obj = converter.ConvertToInvariantString(properties[i].GetValue(this.control));
								pPropBag.Write(properties[i].Name, ref obj);
							}
							else if (converter.CanConvertFrom(typeof(byte[])))
							{
								byte[] inArray = (byte[])converter.ConvertTo(null, CultureInfo.InvariantCulture, properties[i].GetValue(this.control), typeof(byte[]));
								object obj = Convert.ToBase64String(inArray);
								pPropBag.Write(properties[i].Name, ref obj);
							}
						}
					}
				}
				if (UnsafeNativeMethods.IsComObject(pPropBag))
				{
					UnsafeNativeMethods.ReleaseComObject(pPropBag);
				}
				if (fClearDirty)
				{
					this.activeXState[Control.ActiveXImpl.isDirty] = false;
				}
			}

			// Token: 0x06005765 RID: 22373 RVA: 0x0016F0DC File Offset: 0x0016D2DC
			private void SendOnSave()
			{
				int count = this.adviseList.Count;
				IntSecurity.UnmanagedCode.Assert();
				for (int i = 0; i < count; i++)
				{
					IAdviseSink adviseSink = (IAdviseSink)this.adviseList[i];
					adviseSink.OnSave();
				}
			}

			// Token: 0x06005766 RID: 22374 RVA: 0x0016F124 File Offset: 0x0016D324
			internal void SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
			{
				if ((aspects & 1) == 0)
				{
					Control.ActiveXImpl.ThrowHr(-2147221397);
				}
				this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst] = ((advf & 2) != 0);
				this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce] = ((advf & 4) != 0);
				if (this.viewAdviseSink != null && UnsafeNativeMethods.IsComObject(this.viewAdviseSink))
				{
					UnsafeNativeMethods.ReleaseComObject(this.viewAdviseSink);
				}
				this.viewAdviseSink = pAdvSink;
				if (this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst])
				{
					this.ViewChanged();
				}
			}

			// Token: 0x06005767 RID: 22375 RVA: 0x0016F1B4 File Offset: 0x0016D3B4
			internal void SetClientSite(UnsafeNativeMethods.IOleClientSite value)
			{
				if (this.clientSite != null)
				{
					if (value == null)
					{
						Control.ActiveXImpl.globalActiveXCount--;
						if (Control.ActiveXImpl.globalActiveXCount == 0 && this.IsIE)
						{
							new PermissionSet(PermissionState.Unrestricted).Assert();
							try
							{
								MethodInfo method = typeof(SystemEvents).GetMethod("Shutdown", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[0], new ParameterModifier[0]);
								if (method != null)
								{
									method.Invoke(null, null);
								}
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
					if (UnsafeNativeMethods.IsComObject(this.clientSite))
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							Marshal.FinalReleaseComObject(this.clientSite);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				this.clientSite = value;
				if (this.clientSite != null)
				{
					this.control.Site = new Control.AxSourcingSite(this.control, this.clientSite, "ControlAxSourcingSite");
				}
				else
				{
					this.control.Site = null;
				}
				object obj = new object();
				if (this.GetAmbientProperty(-710, ref obj))
				{
					this.activeXState[Control.ActiveXImpl.uiDead] = (bool)obj;
				}
				if (this.control is IButtonControl && this.GetAmbientProperty(-710, ref obj))
				{
					((IButtonControl)this.control).NotifyDefault((bool)obj);
				}
				if (this.clientSite == null)
				{
					if (this.accelTable != IntPtr.Zero)
					{
						UnsafeNativeMethods.DestroyAcceleratorTable(new HandleRef(this, this.accelTable));
						this.accelTable = IntPtr.Zero;
						this.accelCount = -1;
					}
					if (this.IsIE)
					{
						this.control.Dispose();
					}
				}
				else
				{
					Control.ActiveXImpl.globalActiveXCount++;
					if (Control.ActiveXImpl.globalActiveXCount == 1 && this.IsIE)
					{
						new PermissionSet(PermissionState.Unrestricted).Assert();
						try
						{
							MethodInfo method2 = typeof(SystemEvents).GetMethod("Startup", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[0], new ParameterModifier[0]);
							if (method2 != null)
							{
								method2.Invoke(null, null);
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				this.control.OnTopMostActiveXParentChanged(EventArgs.Empty);
			}

			// Token: 0x06005768 RID: 22376 RVA: 0x0016F3F0 File Offset: 0x0016D5F0
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void SetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
			{
				if ((dwDrawAspect & 1) != 0)
				{
					if (this.activeXState[Control.ActiveXImpl.changingExtents])
					{
						return;
					}
					this.activeXState[Control.ActiveXImpl.changingExtents] = true;
					try
					{
						Size size = new Size(this.HiMetricToPixel(pSizel.cx, pSizel.cy));
						if (this.activeXState[Control.ActiveXImpl.inPlaceActive])
						{
							UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
							if (oleInPlaceSite != null)
							{
								Rectangle bounds = this.control.Bounds;
								bounds.Location = new Point(bounds.X, bounds.Y);
								Size size2 = new Size(size.Width, size.Height);
								bounds.Width = size2.Width;
								bounds.Height = size2.Height;
								oleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT.FromXYWH(bounds.X, bounds.Y, bounds.Width, bounds.Height));
							}
						}
						this.control.Size = size;
						if (!this.control.Size.Equals(size))
						{
							this.activeXState[Control.ActiveXImpl.isDirty] = true;
							if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
							{
								this.ViewChanged();
							}
							if (!this.activeXState[Control.ActiveXImpl.inPlaceActive] && this.clientSite != null)
							{
								this.clientSite.RequestNewObjectLayout();
							}
						}
						return;
					}
					finally
					{
						this.activeXState[Control.ActiveXImpl.changingExtents] = false;
					}
				}
				Control.ActiveXImpl.ThrowHr(-2147221397);
			}

			// Token: 0x06005769 RID: 22377 RVA: 0x0016F59C File Offset: 0x0016D79C
			private void SetInPlaceVisible(bool visible)
			{
				this.activeXState[Control.ActiveXImpl.inPlaceVisible] = visible;
				this.control.Visible = visible;
			}

			// Token: 0x0600576A RID: 22378 RVA: 0x0016F5BC File Offset: 0x0016D7BC
			internal void SetObjectRects(NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect)
			{
				Rectangle rectangle = Rectangle.FromLTRB(lprcPosRect.left, lprcPosRect.top, lprcPosRect.right, lprcPosRect.bottom);
				if (this.activeXState[Control.ActiveXImpl.adjustingRect])
				{
					this.adjustRect.left = rectangle.X;
					this.adjustRect.top = rectangle.Y;
					this.adjustRect.right = rectangle.Width + rectangle.X;
					this.adjustRect.bottom = rectangle.Height + rectangle.Y;
				}
				else
				{
					this.activeXState[Control.ActiveXImpl.adjustingRect] = true;
					try
					{
						this.control.Bounds = rectangle;
					}
					finally
					{
						this.activeXState[Control.ActiveXImpl.adjustingRect] = false;
					}
				}
				bool flag = false;
				if (this.clipRegion != IntPtr.Zero)
				{
					this.clipRegion = IntPtr.Zero;
					flag = true;
				}
				if (lprcClipRect != null)
				{
					Rectangle b = Rectangle.FromLTRB(lprcClipRect.left, lprcClipRect.top, lprcClipRect.right, lprcClipRect.bottom);
					Rectangle rectangle2;
					if (!b.IsEmpty)
					{
						rectangle2 = Rectangle.Intersect(rectangle, b);
					}
					else
					{
						rectangle2 = rectangle;
					}
					if (!rectangle2.Equals(rectangle))
					{
						NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height);
						IntPtr parent = UnsafeNativeMethods.GetParent(new HandleRef(this.control, this.control.Handle));
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(null, parent), new HandleRef(this.control, this.control.Handle), ref rect, 2);
						this.clipRegion = SafeNativeMethods.CreateRectRgn(rect.left, rect.top, rect.right, rect.bottom);
						flag = true;
					}
				}
				if (flag && this.control.IsHandleCreated)
				{
					IntPtr handle = this.clipRegion;
					Region region = this.control.Region;
					if (region != null)
					{
						IntPtr hrgn = this.control.GetHRgn(region);
						handle = this.MergeRegion(hrgn);
					}
					UnsafeNativeMethods.SetWindowRgn(new HandleRef(this.control, this.control.Handle), new HandleRef(this, handle), SafeNativeMethods.IsWindowVisible(new HandleRef(this.control, this.control.Handle)));
				}
				this.control.Invalidate();
			}

			// Token: 0x0600576B RID: 22379 RVA: 0x0016F824 File Offset: 0x0016DA24
			internal static void ThrowHr(int hr)
			{
				ExternalException ex = new ExternalException(SR.GetString("ExternalException"), hr);
				throw ex;
			}

			// Token: 0x0600576C RID: 22380 RVA: 0x0016F844 File Offset: 0x0016DA44
			internal int TranslateAccelerator(ref NativeMethods.MSG lpmsg)
			{
				bool flag = false;
				switch (lpmsg.message)
				{
				case 256:
				case 258:
				case 260:
				case 262:
					flag = true;
					break;
				}
				Message message = Message.Create(lpmsg.hwnd, lpmsg.message, lpmsg.wParam, lpmsg.lParam);
				if (flag)
				{
					Control control = Control.FromChildHandleInternal(lpmsg.hwnd);
					if (control != null && (this.control == control || this.control.Contains(control)))
					{
						switch (Control.PreProcessControlMessageInternal(control, ref message))
						{
						case PreProcessControlState.MessageProcessed:
							lpmsg.message = message.Msg;
							lpmsg.wParam = message.WParam;
							lpmsg.lParam = message.LParam;
							return 0;
						case PreProcessControlState.MessageNeeded:
							UnsafeNativeMethods.TranslateMessage(ref lpmsg);
							if (SafeNativeMethods.IsWindowUnicode(new HandleRef(null, lpmsg.hwnd)))
							{
								UnsafeNativeMethods.DispatchMessageW(ref lpmsg);
							}
							else
							{
								UnsafeNativeMethods.DispatchMessageA(ref lpmsg);
							}
							return 0;
						}
					}
				}
				int result = 1;
				UnsafeNativeMethods.IOleControlSite oleControlSite = this.clientSite as UnsafeNativeMethods.IOleControlSite;
				if (oleControlSite != null)
				{
					int num = 0;
					if (UnsafeNativeMethods.GetKeyState(16) < 0)
					{
						num |= 1;
					}
					if (UnsafeNativeMethods.GetKeyState(17) < 0)
					{
						num |= 2;
					}
					if (UnsafeNativeMethods.GetKeyState(18) < 0)
					{
						num |= 4;
					}
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						result = oleControlSite.TranslateAccelerator(ref lpmsg, num);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return result;
			}

			// Token: 0x0600576D RID: 22381 RVA: 0x0016F9C0 File Offset: 0x0016DBC0
			internal int UIDeactivate()
			{
				if (!this.activeXState[Control.ActiveXImpl.uiActive])
				{
					return 0;
				}
				this.activeXState[Control.ActiveXImpl.uiActive] = false;
				if (this.inPlaceUiWindow != null)
				{
					this.inPlaceUiWindow.SetActiveObject(null, null);
				}
				IntSecurity.UnmanagedCode.Assert();
				this.inPlaceFrame.SetActiveObject(null, null);
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite != null)
				{
					oleInPlaceSite.OnUIDeactivate(0);
				}
				return 0;
			}

			// Token: 0x0600576E RID: 22382 RVA: 0x0016FA38 File Offset: 0x0016DC38
			internal void Unadvise(int dwConnection)
			{
				if (dwConnection > this.adviseList.Count || this.adviseList[dwConnection - 1] == null)
				{
					Control.ActiveXImpl.ThrowHr(-2147221500);
				}
				IAdviseSink adviseSink = (IAdviseSink)this.adviseList[dwConnection - 1];
				this.adviseList.RemoveAt(dwConnection - 1);
				if (adviseSink != null && UnsafeNativeMethods.IsComObject(adviseSink))
				{
					UnsafeNativeMethods.ReleaseComObject(adviseSink);
				}
			}

			// Token: 0x0600576F RID: 22383 RVA: 0x0016FAA4 File Offset: 0x0016DCA4
			internal void UpdateBounds(ref int x, ref int y, ref int width, ref int height, int flags)
			{
				if (!this.activeXState[Control.ActiveXImpl.adjustingRect] && this.activeXState[Control.ActiveXImpl.inPlaceVisible])
				{
					UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
					if (oleInPlaceSite != null)
					{
						NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
						if ((flags & 2) != 0)
						{
							comrect.left = this.control.Left;
							comrect.top = this.control.Top;
						}
						else
						{
							comrect.left = x;
							comrect.top = y;
						}
						if ((flags & 1) != 0)
						{
							comrect.right = comrect.left + this.control.Width;
							comrect.bottom = comrect.top + this.control.Height;
						}
						else
						{
							comrect.right = comrect.left + width;
							comrect.bottom = comrect.top + height;
						}
						this.adjustRect = comrect;
						this.activeXState[Control.ActiveXImpl.adjustingRect] = true;
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							oleInPlaceSite.OnPosRectChange(comrect);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							this.adjustRect = null;
							this.activeXState[Control.ActiveXImpl.adjustingRect] = false;
						}
						if ((flags & 2) == 0)
						{
							x = comrect.left;
							y = comrect.top;
						}
						if ((flags & 1) == 0)
						{
							width = comrect.right - comrect.left;
							height = comrect.bottom - comrect.top;
						}
					}
				}
			}

			// Token: 0x06005770 RID: 22384 RVA: 0x0016FC1C File Offset: 0x0016DE1C
			internal void UpdateAccelTable()
			{
				this.accelCount = -1;
				UnsafeNativeMethods.IOleControlSite oleControlSite = this.clientSite as UnsafeNativeMethods.IOleControlSite;
				if (oleControlSite != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					oleControlSite.OnControlInfoChanged();
				}
			}

			// Token: 0x06005771 RID: 22385 RVA: 0x0016FC50 File Offset: 0x0016DE50
			internal void ViewChangedInternal()
			{
				this.ViewChanged();
			}

			// Token: 0x06005772 RID: 22386 RVA: 0x0016FC58 File Offset: 0x0016DE58
			private void ViewChanged()
			{
				if (this.viewAdviseSink != null && !this.activeXState[Control.ActiveXImpl.saving])
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						this.viewAdviseSink.OnViewChange(1, -1);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce])
					{
						if (UnsafeNativeMethods.IsComObject(this.viewAdviseSink))
						{
							UnsafeNativeMethods.ReleaseComObject(this.viewAdviseSink);
						}
						this.viewAdviseSink = null;
					}
				}
			}

			// Token: 0x06005773 RID: 22387 RVA: 0x0016FCE0 File Offset: 0x0016DEE0
			void IWindowTarget.OnHandleChange(IntPtr newHandle)
			{
				this.controlWindowTarget.OnHandleChange(newHandle);
			}

			// Token: 0x06005774 RID: 22388 RVA: 0x0016FCF0 File Offset: 0x0016DEF0
			void IWindowTarget.OnMessage(ref Message m)
			{
				if (this.activeXState[Control.ActiveXImpl.uiDead])
				{
					if (m.Msg >= 512 && m.Msg <= 522)
					{
						return;
					}
					if (m.Msg >= 161 && m.Msg <= 169)
					{
						return;
					}
					if (m.Msg >= 256 && m.Msg <= 264)
					{
						return;
					}
				}
				IntSecurity.UnmanagedCode.Assert();
				this.controlWindowTarget.OnMessage(ref m);
			}

			// Token: 0x04003825 RID: 14373
			private static readonly int hiMetricPerInch = 2540;

			// Token: 0x04003826 RID: 14374
			private static readonly int viewAdviseOnlyOnce = BitVector32.CreateMask();

			// Token: 0x04003827 RID: 14375
			private static readonly int viewAdvisePrimeFirst = BitVector32.CreateMask(Control.ActiveXImpl.viewAdviseOnlyOnce);

			// Token: 0x04003828 RID: 14376
			private static readonly int eventsFrozen = BitVector32.CreateMask(Control.ActiveXImpl.viewAdvisePrimeFirst);

			// Token: 0x04003829 RID: 14377
			private static readonly int changingExtents = BitVector32.CreateMask(Control.ActiveXImpl.eventsFrozen);

			// Token: 0x0400382A RID: 14378
			private static readonly int saving = BitVector32.CreateMask(Control.ActiveXImpl.changingExtents);

			// Token: 0x0400382B RID: 14379
			private static readonly int isDirty = BitVector32.CreateMask(Control.ActiveXImpl.saving);

			// Token: 0x0400382C RID: 14380
			private static readonly int inPlaceActive = BitVector32.CreateMask(Control.ActiveXImpl.isDirty);

			// Token: 0x0400382D RID: 14381
			private static readonly int inPlaceVisible = BitVector32.CreateMask(Control.ActiveXImpl.inPlaceActive);

			// Token: 0x0400382E RID: 14382
			private static readonly int uiActive = BitVector32.CreateMask(Control.ActiveXImpl.inPlaceVisible);

			// Token: 0x0400382F RID: 14383
			private static readonly int uiDead = BitVector32.CreateMask(Control.ActiveXImpl.uiActive);

			// Token: 0x04003830 RID: 14384
			private static readonly int adjustingRect = BitVector32.CreateMask(Control.ActiveXImpl.uiDead);

			// Token: 0x04003831 RID: 14385
			private static Point logPixels = Point.Empty;

			// Token: 0x04003832 RID: 14386
			private static NativeMethods.tagOLEVERB[] axVerbs;

			// Token: 0x04003833 RID: 14387
			private static int globalActiveXCount = 0;

			// Token: 0x04003834 RID: 14388
			private static bool checkedIE;

			// Token: 0x04003835 RID: 14389
			private static bool isIE;

			// Token: 0x04003836 RID: 14390
			private Control control;

			// Token: 0x04003837 RID: 14391
			private IWindowTarget controlWindowTarget;

			// Token: 0x04003838 RID: 14392
			private IntPtr clipRegion;

			// Token: 0x04003839 RID: 14393
			private UnsafeNativeMethods.IOleClientSite clientSite;

			// Token: 0x0400383A RID: 14394
			private UnsafeNativeMethods.IOleInPlaceUIWindow inPlaceUiWindow;

			// Token: 0x0400383B RID: 14395
			private UnsafeNativeMethods.IOleInPlaceFrame inPlaceFrame;

			// Token: 0x0400383C RID: 14396
			private ArrayList adviseList;

			// Token: 0x0400383D RID: 14397
			private IAdviseSink viewAdviseSink;

			// Token: 0x0400383E RID: 14398
			private BitVector32 activeXState;

			// Token: 0x0400383F RID: 14399
			private Control.AmbientProperty[] ambientProperties;

			// Token: 0x04003840 RID: 14400
			private IntPtr hwndParent;

			// Token: 0x04003841 RID: 14401
			private IntPtr accelTable;

			// Token: 0x04003842 RID: 14402
			private short accelCount = -1;

			// Token: 0x04003843 RID: 14403
			private NativeMethods.COMRECT adjustRect;

			// Token: 0x0200088D RID: 2189
			internal static class AdviseHelper
			{
				// Token: 0x0600709E RID: 28830 RVA: 0x0019BBC8 File Offset: 0x00199DC8
				public static bool AdviseConnectionPoint(object connectionPoint, object sink, Type eventInterface, out int cookie)
				{
					bool result;
					using (Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer comConnectionPointContainer = new Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer(connectionPoint, true))
					{
						result = Control.ActiveXImpl.AdviseHelper.AdviseConnectionPoint(comConnectionPointContainer, sink, eventInterface, out cookie);
					}
					return result;
				}

				// Token: 0x0600709F RID: 28831 RVA: 0x0019BC04 File Offset: 0x00199E04
				internal static bool AdviseConnectionPoint(Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer cpc, object sink, Type eventInterface, out int cookie)
				{
					bool result;
					using (Control.ActiveXImpl.AdviseHelper.ComConnectionPoint comConnectionPoint = cpc.FindConnectionPoint(eventInterface))
					{
						using (Control.ActiveXImpl.AdviseHelper.SafeIUnknown safeIUnknown = new Control.ActiveXImpl.AdviseHelper.SafeIUnknown(sink, true))
						{
							result = comConnectionPoint.Advise(safeIUnknown.DangerousGetHandle(), out cookie);
						}
					}
					return result;
				}

				// Token: 0x0200095A RID: 2394
				internal class SafeIUnknown : SafeHandle
				{
					// Token: 0x06007377 RID: 29559 RVA: 0x001A11BC File Offset: 0x0019F3BC
					public SafeIUnknown(object obj, bool addRefIntPtr) : this(obj, addRefIntPtr, Guid.Empty)
					{
					}

					// Token: 0x06007378 RID: 29560 RVA: 0x001A11CC File Offset: 0x0019F3CC
					public SafeIUnknown(object obj, bool addRefIntPtr, Guid iid) : base(IntPtr.Zero, true)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							IntPtr intPtr;
							if (obj is IntPtr)
							{
								intPtr = (IntPtr)obj;
								if (addRefIntPtr)
								{
									Marshal.AddRef(intPtr);
								}
							}
							else
							{
								intPtr = Marshal.GetIUnknownForObject(obj);
							}
							if (iid != Guid.Empty)
							{
								IntPtr pUnk = intPtr;
								try
								{
									intPtr = Control.ActiveXImpl.AdviseHelper.SafeIUnknown.InternalQueryInterface(intPtr, ref iid);
								}
								finally
								{
									Marshal.Release(pUnk);
								}
							}
							this.handle = intPtr;
						}
					}

					// Token: 0x06007379 RID: 29561 RVA: 0x001A1254 File Offset: 0x0019F454
					private static IntPtr InternalQueryInterface(IntPtr pUnk, ref Guid iid)
					{
						IntPtr intPtr;
						if (Marshal.QueryInterface(pUnk, ref iid, out intPtr) != 0 || intPtr == IntPtr.Zero)
						{
							throw new InvalidCastException(SR.GetString("AxInterfaceNotSupported"));
						}
						return intPtr;
					}

					// Token: 0x17001A48 RID: 6728
					// (get) Token: 0x0600737A RID: 29562 RVA: 0x001A128C File Offset: 0x0019F48C
					public sealed override bool IsInvalid
					{
						get
						{
							return base.IsClosed || IntPtr.Zero == this.handle;
						}
					}

					// Token: 0x0600737B RID: 29563 RVA: 0x001A12A8 File Offset: 0x0019F4A8
					protected sealed override bool ReleaseHandle()
					{
						IntPtr handle = this.handle;
						this.handle = IntPtr.Zero;
						if (IntPtr.Zero != handle)
						{
							Marshal.Release(handle);
						}
						return true;
					}

					// Token: 0x0600737C RID: 29564 RVA: 0x001A12DC File Offset: 0x0019F4DC
					protected V LoadVtable<V>()
					{
						IntPtr ptr = Marshal.ReadIntPtr(this.handle, 0);
						return (V)((object)Marshal.PtrToStructure(ptr, typeof(V)));
					}
				}

				// Token: 0x0200095B RID: 2395
				internal sealed class ComConnectionPointContainer : Control.ActiveXImpl.AdviseHelper.SafeIUnknown
				{
					// Token: 0x0600737D RID: 29565 RVA: 0x001A130B File Offset: 0x0019F50B
					public ComConnectionPointContainer(object obj, bool addRefIntPtr) : base(obj, addRefIntPtr, typeof(IConnectionPointContainer).GUID)
					{
						this.vtbl = base.LoadVtable<Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.VTABLE>();
					}

					// Token: 0x0600737E RID: 29566 RVA: 0x001A1330 File Offset: 0x0019F530
					public Control.ActiveXImpl.AdviseHelper.ComConnectionPoint FindConnectionPoint(Type eventInterface)
					{
						Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD findConnectionPointD = (Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD)Marshal.GetDelegateForFunctionPointer(this.vtbl.FindConnectionPointPtr, typeof(Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD));
						IntPtr zero = IntPtr.Zero;
						Guid guid = eventInterface.GUID;
						if (findConnectionPointD(this.handle, ref guid, out zero) != 0 || zero == IntPtr.Zero)
						{
							throw new ArgumentException(SR.GetString("AXNoConnectionPoint", new object[]
							{
								eventInterface.Name
							}));
						}
						return new Control.ActiveXImpl.AdviseHelper.ComConnectionPoint(zero, false);
					}

					// Token: 0x04004692 RID: 18066
					private Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.VTABLE vtbl;

					// Token: 0x0200095E RID: 2398
					[StructLayout(LayoutKind.Sequential)]
					private class VTABLE
					{
						// Token: 0x04004696 RID: 18070
						public IntPtr QueryInterfacePtr;

						// Token: 0x04004697 RID: 18071
						public IntPtr AddRefPtr;

						// Token: 0x04004698 RID: 18072
						public IntPtr ReleasePtr;

						// Token: 0x04004699 RID: 18073
						public IntPtr EnumConnectionPointsPtr;

						// Token: 0x0400469A RID: 18074
						public IntPtr FindConnectionPointPtr;
					}

					// Token: 0x0200095F RID: 2399
					// (Invoke) Token: 0x06007389 RID: 29577
					[UnmanagedFunctionPointer(CallingConvention.StdCall)]
					private delegate int FindConnectionPointD(IntPtr This, ref Guid iid, out IntPtr ppv);
				}

				// Token: 0x0200095C RID: 2396
				internal sealed class ComConnectionPoint : Control.ActiveXImpl.AdviseHelper.SafeIUnknown
				{
					// Token: 0x0600737F RID: 29567 RVA: 0x001A13B6 File Offset: 0x0019F5B6
					public ComConnectionPoint(object obj, bool addRefIntPtr) : base(obj, addRefIntPtr, typeof(IConnectionPoint).GUID)
					{
						this.vtbl = base.LoadVtable<Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.VTABLE>();
					}

					// Token: 0x06007380 RID: 29568 RVA: 0x001A13DC File Offset: 0x0019F5DC
					public bool Advise(IntPtr punkEventSink, out int cookie)
					{
						Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD adviseD = (Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD)Marshal.GetDelegateForFunctionPointer(this.vtbl.AdvisePtr, typeof(Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD));
						return adviseD(this.handle, punkEventSink, out cookie) == 0;
					}

					// Token: 0x04004693 RID: 18067
					private Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.VTABLE vtbl;

					// Token: 0x02000960 RID: 2400
					[StructLayout(LayoutKind.Sequential)]
					private class VTABLE
					{
						// Token: 0x0400469B RID: 18075
						public IntPtr QueryInterfacePtr;

						// Token: 0x0400469C RID: 18076
						public IntPtr AddRefPtr;

						// Token: 0x0400469D RID: 18077
						public IntPtr ReleasePtr;

						// Token: 0x0400469E RID: 18078
						public IntPtr GetConnectionInterfacePtr;

						// Token: 0x0400469F RID: 18079
						public IntPtr GetConnectionPointContainterPtr;

						// Token: 0x040046A0 RID: 18080
						public IntPtr AdvisePtr;

						// Token: 0x040046A1 RID: 18081
						public IntPtr UnadvisePtr;

						// Token: 0x040046A2 RID: 18082
						public IntPtr EnumConnectionsPtr;
					}

					// Token: 0x02000961 RID: 2401
					// (Invoke) Token: 0x0600738E RID: 29582
					[UnmanagedFunctionPointer(CallingConvention.StdCall)]
					private delegate int AdviseD(IntPtr This, IntPtr punkEventSink, out int cookie);
				}
			}

			// Token: 0x0200088E RID: 2190
			private class PropertyBagStream : UnsafeNativeMethods.IPropertyBag
			{
				// Token: 0x060070A0 RID: 28832 RVA: 0x0019BC64 File Offset: 0x00199E64
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				internal void Read(UnsafeNativeMethods.IStream istream)
				{
					Stream stream = new DataStreamFromComStream(istream);
					byte[] array = new byte[4096];
					int num = 0;
					int num2 = stream.Read(array, num, 4096);
					int num3 = num2;
					while (num2 == 4096)
					{
						byte[] array2 = new byte[array.Length + 4096];
						Array.Copy(array, array2, array.Length);
						array = array2;
						num += 4096;
						num2 = stream.Read(array, num, 4096);
						num3 += num2;
					}
					stream = new MemoryStream(array);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					try
					{
						this.bag = (Hashtable)binaryFormatter.Deserialize(stream);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
						this.bag = new Hashtable();
					}
				}

				// Token: 0x060070A1 RID: 28833 RVA: 0x0019BD28 File Offset: 0x00199F28
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				int UnsafeNativeMethods.IPropertyBag.Read(string pszPropName, ref object pVar, UnsafeNativeMethods.IErrorLog pErrorLog)
				{
					if (!this.bag.Contains(pszPropName))
					{
						return -2147024809;
					}
					pVar = this.bag[pszPropName];
					return 0;
				}

				// Token: 0x060070A2 RID: 28834 RVA: 0x0019BD4D File Offset: 0x00199F4D
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				int UnsafeNativeMethods.IPropertyBag.Write(string pszPropName, ref object pVar)
				{
					this.bag[pszPropName] = pVar;
					return 0;
				}

				// Token: 0x060070A3 RID: 28835 RVA: 0x0019BD60 File Offset: 0x00199F60
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				internal void Write(UnsafeNativeMethods.IStream istream)
				{
					Stream serializationStream = new DataStreamFromComStream(istream);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(serializationStream, this.bag);
				}

				// Token: 0x040043E9 RID: 17385
				private Hashtable bag = new Hashtable();
			}
		}

		// Token: 0x0200057D RID: 1405
		private class AxSourcingSite : ISite, IServiceProvider
		{
			// Token: 0x06005776 RID: 22390 RVA: 0x0016FE3F File Offset: 0x0016E03F
			internal AxSourcingSite(IComponent component, UnsafeNativeMethods.IOleClientSite clientSite, string name)
			{
				this.component = component;
				this.clientSite = clientSite;
				this.name = name;
			}

			// Token: 0x170014D3 RID: 5331
			// (get) Token: 0x06005777 RID: 22391 RVA: 0x0016FE5C File Offset: 0x0016E05C
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x170014D4 RID: 5332
			// (get) Token: 0x06005778 RID: 22392 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06005779 RID: 22393 RVA: 0x0016FE64 File Offset: 0x0016E064
			public object GetService(Type service)
			{
				object result = null;
				if (service == typeof(HtmlDocument))
				{
					UnsafeNativeMethods.IOleContainer oleContainer;
					int container;
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						container = this.clientSite.GetContainer(out oleContainer);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Succeeded(container) && oleContainer is UnsafeNativeMethods.IHTMLDocument)
					{
						if (this.shimManager == null)
						{
							this.shimManager = new HtmlShimManager();
						}
						result = new HtmlDocument(this.shimManager, oleContainer as UnsafeNativeMethods.IHTMLDocument);
					}
				}
				else if (this.clientSite.GetType().IsAssignableFrom(service))
				{
					IntSecurity.UnmanagedCode.Demand();
					result = this.clientSite;
				}
				return result;
			}

			// Token: 0x170014D5 RID: 5333
			// (get) Token: 0x0600577A RID: 22394 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170014D6 RID: 5334
			// (get) Token: 0x0600577B RID: 22395 RVA: 0x0016FF14 File Offset: 0x0016E114
			// (set) Token: 0x0600577C RID: 22396 RVA: 0x0016FF1C File Offset: 0x0016E11C
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					if (value == null || this.name == null)
					{
						this.name = value;
					}
				}
			}

			// Token: 0x04003844 RID: 14404
			private IComponent component;

			// Token: 0x04003845 RID: 14405
			private UnsafeNativeMethods.IOleClientSite clientSite;

			// Token: 0x04003846 RID: 14406
			private string name;

			// Token: 0x04003847 RID: 14407
			private HtmlShimManager shimManager;
		}

		// Token: 0x0200057E RID: 1406
		private class ActiveXFontMarshaler : ICustomMarshaler
		{
			// Token: 0x0600577D RID: 22397 RVA: 0x0000701A File Offset: 0x0000521A
			public void CleanUpManagedData(object obj)
			{
			}

			// Token: 0x0600577E RID: 22398 RVA: 0x0016FF30 File Offset: 0x0016E130
			public void CleanUpNativeData(IntPtr pObj)
			{
				Marshal.Release(pObj);
			}

			// Token: 0x0600577F RID: 22399 RVA: 0x0016FF39 File Offset: 0x0016E139
			internal static ICustomMarshaler GetInstance(string cookie)
			{
				if (Control.ActiveXFontMarshaler.instance == null)
				{
					Control.ActiveXFontMarshaler.instance = new Control.ActiveXFontMarshaler();
				}
				return Control.ActiveXFontMarshaler.instance;
			}

			// Token: 0x06005780 RID: 22400 RVA: 0x0000DE5F File Offset: 0x0000C05F
			public int GetNativeDataSize()
			{
				return -1;
			}

			// Token: 0x06005781 RID: 22401 RVA: 0x0016FF54 File Offset: 0x0016E154
			public IntPtr MarshalManagedToNative(object obj)
			{
				Font font = (Font)obj;
				NativeMethods.tagFONTDESC tagFONTDESC = new NativeMethods.tagFONTDESC();
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					font.ToLogFont(logfont);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				tagFONTDESC.lpstrName = font.Name;
				tagFONTDESC.cySize = (long)(font.SizeInPoints * 10000f);
				tagFONTDESC.sWeight = (short)logfont.lfWeight;
				tagFONTDESC.sCharset = (short)logfont.lfCharSet;
				tagFONTDESC.fItalic = font.Italic;
				tagFONTDESC.fUnderline = font.Underline;
				tagFONTDESC.fStrikethrough = font.Strikeout;
				Guid guid = typeof(UnsafeNativeMethods.IFont).GUID;
				UnsafeNativeMethods.IFont o = UnsafeNativeMethods.OleCreateFontIndirect(tagFONTDESC, ref guid);
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(o);
				IntPtr result;
				int num = Marshal.QueryInterface(iunknownForObject, ref guid, out result);
				Marshal.Release(iunknownForObject);
				if (NativeMethods.Failed(num))
				{
					Marshal.ThrowExceptionForHR(num);
				}
				return result;
			}

			// Token: 0x06005782 RID: 22402 RVA: 0x00170044 File Offset: 0x0016E244
			public object MarshalNativeToManaged(IntPtr pObj)
			{
				UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)Marshal.GetObjectForIUnknown(pObj);
				IntPtr hfont = IntPtr.Zero;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					hfont = font.GetHFont();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				Font result = null;
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					result = Font.FromHfont(hfont);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					result = Control.DefaultFont;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return result;
			}

			// Token: 0x04003848 RID: 14408
			private static Control.ActiveXFontMarshaler instance;
		}

		// Token: 0x0200057F RID: 1407
		private class ActiveXVerbEnum : UnsafeNativeMethods.IEnumOLEVERB
		{
			// Token: 0x06005784 RID: 22404 RVA: 0x001700D4 File Offset: 0x0016E2D4
			internal ActiveXVerbEnum(NativeMethods.tagOLEVERB[] verbs)
			{
				this.verbs = verbs;
				this.current = 0;
			}

			// Token: 0x06005785 RID: 22405 RVA: 0x001700EC File Offset: 0x0016E2EC
			public int Next(int celt, NativeMethods.tagOLEVERB rgelt, int[] pceltFetched)
			{
				int num = 0;
				if (celt != 1)
				{
					celt = 1;
				}
				while (celt > 0 && this.current < this.verbs.Length)
				{
					rgelt.lVerb = this.verbs[this.current].lVerb;
					rgelt.lpszVerbName = this.verbs[this.current].lpszVerbName;
					rgelt.fuFlags = this.verbs[this.current].fuFlags;
					rgelt.grfAttribs = this.verbs[this.current].grfAttribs;
					celt--;
					this.current++;
					num++;
				}
				if (pceltFetched != null)
				{
					pceltFetched[0] = num;
				}
				if (celt != 0)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06005786 RID: 22406 RVA: 0x0017019F File Offset: 0x0016E39F
			public int Skip(int celt)
			{
				if (this.current + celt < this.verbs.Length)
				{
					this.current += celt;
					return 0;
				}
				this.current = this.verbs.Length;
				return 1;
			}

			// Token: 0x06005787 RID: 22407 RVA: 0x001701D2 File Offset: 0x0016E3D2
			public void Reset()
			{
				this.current = 0;
			}

			// Token: 0x06005788 RID: 22408 RVA: 0x001701DB File Offset: 0x0016E3DB
			public void Clone(out UnsafeNativeMethods.IEnumOLEVERB ppenum)
			{
				ppenum = new Control.ActiveXVerbEnum(this.verbs);
			}

			// Token: 0x04003849 RID: 14409
			private NativeMethods.tagOLEVERB[] verbs;

			// Token: 0x0400384A RID: 14410
			private int current;
		}

		// Token: 0x02000580 RID: 1408
		private class AmbientProperty
		{
			// Token: 0x06005789 RID: 22409 RVA: 0x001701EA File Offset: 0x0016E3EA
			internal AmbientProperty(string name, int dispID)
			{
				this.name = name;
				this.dispID = dispID;
				this.value = null;
				this.empty = true;
			}

			// Token: 0x170014D7 RID: 5335
			// (get) Token: 0x0600578A RID: 22410 RVA: 0x0017020E File Offset: 0x0016E40E
			internal string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x170014D8 RID: 5336
			// (get) Token: 0x0600578B RID: 22411 RVA: 0x00170216 File Offset: 0x0016E416
			internal int DispID
			{
				get
				{
					return this.dispID;
				}
			}

			// Token: 0x170014D9 RID: 5337
			// (get) Token: 0x0600578C RID: 22412 RVA: 0x0017021E File Offset: 0x0016E41E
			internal bool Empty
			{
				get
				{
					return this.empty;
				}
			}

			// Token: 0x170014DA RID: 5338
			// (get) Token: 0x0600578D RID: 22413 RVA: 0x00170226 File Offset: 0x0016E426
			// (set) Token: 0x0600578E RID: 22414 RVA: 0x0017022E File Offset: 0x0016E42E
			internal object Value
			{
				get
				{
					return this.value;
				}
				set
				{
					this.value = value;
					this.empty = false;
				}
			}

			// Token: 0x0600578F RID: 22415 RVA: 0x0017023E File Offset: 0x0016E43E
			internal void ResetValue()
			{
				this.empty = true;
				this.value = null;
			}

			// Token: 0x0400384B RID: 14411
			private string name;

			// Token: 0x0400384C RID: 14412
			private int dispID;

			// Token: 0x0400384D RID: 14413
			private object value;

			// Token: 0x0400384E RID: 14414
			private bool empty;
		}

		// Token: 0x02000581 RID: 1409
		private class MetafileDCWrapper : IDisposable
		{
			// Token: 0x06005790 RID: 22416 RVA: 0x00170250 File Offset: 0x0016E450
			internal MetafileDCWrapper(HandleRef hOriginalDC, Size size)
			{
				if (size.Width < 0 || size.Height < 0)
				{
					throw new ArgumentException("size", SR.GetString("ControlMetaFileDCWrapperSizeInvalid"));
				}
				this.hMetafileDC = hOriginalDC;
				this.destRect = new NativeMethods.RECT(0, 0, size.Width, size.Height);
				this.hBitmapDC = new HandleRef(this, UnsafeNativeMethods.CreateCompatibleDC(NativeMethods.NullHandleRef));
				int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(this.hBitmapDC, 14);
				int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(this.hBitmapDC, 12);
				this.hBitmap = new HandleRef(this, SafeNativeMethods.CreateBitmap(size.Width, size.Height, deviceCaps, deviceCaps2, IntPtr.Zero));
				this.hOriginalBmp = new HandleRef(this, SafeNativeMethods.SelectObject(this.hBitmapDC, this.hBitmap));
			}

			// Token: 0x06005791 RID: 22417 RVA: 0x00170350 File Offset: 0x0016E550
			~MetafileDCWrapper()
			{
				((IDisposable)this).Dispose();
			}

			// Token: 0x06005792 RID: 22418 RVA: 0x0017037C File Offset: 0x0016E57C
			void IDisposable.Dispose()
			{
				if (this.hBitmapDC.Handle == IntPtr.Zero || this.hMetafileDC.Handle == IntPtr.Zero || this.hBitmap.Handle == IntPtr.Zero)
				{
					return;
				}
				try
				{
					bool flag = this.DICopy(this.hMetafileDC, this.hBitmapDC, this.destRect, true);
					SafeNativeMethods.SelectObject(this.hBitmapDC, this.hOriginalBmp);
					flag = SafeNativeMethods.DeleteObject(this.hBitmap);
					flag = UnsafeNativeMethods.DeleteCompatibleDC(this.hBitmapDC);
				}
				finally
				{
					this.hBitmapDC = NativeMethods.NullHandleRef;
					this.hBitmap = NativeMethods.NullHandleRef;
					this.hOriginalBmp = NativeMethods.NullHandleRef;
					GC.SuppressFinalize(this);
				}
			}

			// Token: 0x170014DB RID: 5339
			// (get) Token: 0x06005793 RID: 22419 RVA: 0x00170450 File Offset: 0x0016E650
			internal IntPtr HDC
			{
				get
				{
					return this.hBitmapDC.Handle;
				}
			}

			// Token: 0x06005794 RID: 22420 RVA: 0x00170460 File Offset: 0x0016E660
			private unsafe bool DICopy(HandleRef hdcDest, HandleRef hdcSrc, NativeMethods.RECT rect, bool bStretch)
			{
				bool result = false;
				HandleRef hObject = new HandleRef(this, SafeNativeMethods.CreateBitmap(1, 1, 1, 1, IntPtr.Zero));
				if (hObject.Handle == IntPtr.Zero)
				{
					return result;
				}
				try
				{
					HandleRef handleRef = new HandleRef(this, SafeNativeMethods.SelectObject(hdcSrc, hObject));
					if (handleRef.Handle == IntPtr.Zero)
					{
						return result;
					}
					SafeNativeMethods.SelectObject(hdcSrc, handleRef);
					NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
					if (UnsafeNativeMethods.GetObject(handleRef, Marshal.SizeOf(bitmap), bitmap) == 0)
					{
						return result;
					}
					NativeMethods.BITMAPINFO_FLAT bitmapinfo_FLAT = default(NativeMethods.BITMAPINFO_FLAT);
					bitmapinfo_FLAT.bmiHeader_biSize = Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
					bitmapinfo_FLAT.bmiHeader_biWidth = bitmap.bmWidth;
					bitmapinfo_FLAT.bmiHeader_biHeight = bitmap.bmHeight;
					bitmapinfo_FLAT.bmiHeader_biPlanes = 1;
					bitmapinfo_FLAT.bmiHeader_biBitCount = bitmap.bmBitsPixel;
					bitmapinfo_FLAT.bmiHeader_biCompression = 0;
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
					bitmapinfo_FLAT.bmiHeader_biXPelsPerMeter = 0;
					bitmapinfo_FLAT.bmiHeader_biYPelsPerMeter = 0;
					bitmapinfo_FLAT.bmiHeader_biClrUsed = 0;
					bitmapinfo_FLAT.bmiHeader_biClrImportant = 0;
					bitmapinfo_FLAT.bmiColors = new byte[1024];
					long num = 1L << (int)(bitmap.bmBitsPixel * bitmap.bmPlanes & 31);
					if (num <= 256L)
					{
						byte[] array = new byte[Marshal.SizeOf(typeof(NativeMethods.PALETTEENTRY)) * 256];
						SafeNativeMethods.GetSystemPaletteEntries(hdcSrc, 0, (int)num, array);
						try
						{
							fixed (byte* ptr = bitmapinfo_FLAT.bmiColors)
							{
								try
								{
									fixed (byte* ptr2 = array)
									{
										NativeMethods.RGBQUAD* ptr3 = (NativeMethods.RGBQUAD*)ptr;
										NativeMethods.PALETTEENTRY* ptr4 = (NativeMethods.PALETTEENTRY*)ptr2;
										for (long num2 = 0L; num2 < (long)((int)num); num2 += 1L)
										{
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbRed = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peRed;
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbBlue = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peBlue;
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbGreen = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peGreen;
										}
									}
								}
								finally
								{
									byte* ptr2 = null;
								}
							}
						}
						finally
						{
							byte* ptr = null;
						}
					}
					long num3 = (long)bitmap.bmBitsPixel * (long)bitmap.bmWidth;
					long num4 = (num3 + 7L) / 8L;
					long num5 = num4 * (long)bitmap.bmHeight;
					byte[] array2 = new byte[num5];
					if (SafeNativeMethods.GetDIBits(hdcSrc, handleRef, 0, bitmap.bmHeight, array2, ref bitmapinfo_FLAT, 0) == 0)
					{
						return result;
					}
					int left;
					int top;
					int nDestWidth;
					int nDestHeight;
					if (bStretch)
					{
						left = rect.left;
						top = rect.top;
						nDestWidth = rect.right - rect.left;
						nDestHeight = rect.bottom - rect.top;
					}
					else
					{
						left = rect.left;
						top = rect.top;
						nDestWidth = bitmap.bmWidth;
						nDestHeight = bitmap.bmHeight;
					}
					int num6 = SafeNativeMethods.StretchDIBits(hdcDest, left, top, nDestWidth, nDestHeight, 0, 0, bitmap.bmWidth, bitmap.bmHeight, array2, ref bitmapinfo_FLAT, 0, 13369376);
					if (num6 == -1)
					{
						return result;
					}
					result = true;
				}
				finally
				{
					SafeNativeMethods.DeleteObject(hObject);
				}
				return result;
			}

			// Token: 0x0400384F RID: 14415
			private HandleRef hBitmapDC = NativeMethods.NullHandleRef;

			// Token: 0x04003850 RID: 14416
			private HandleRef hBitmap = NativeMethods.NullHandleRef;

			// Token: 0x04003851 RID: 14417
			private HandleRef hOriginalBmp = NativeMethods.NullHandleRef;

			// Token: 0x04003852 RID: 14418
			private HandleRef hMetafileDC = NativeMethods.NullHandleRef;

			// Token: 0x04003853 RID: 14419
			private NativeMethods.RECT destRect;
		}

		/// <summary>Provides information about a control that can be used by an accessibility application.</summary>
		// Token: 0x02000582 RID: 1410
		[ComVisible(true)]
		public class ControlAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" /> class.</summary>
			/// <param name="ownerControl">The <see cref="T:System.Windows.Forms.Control" /> that owns the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />. </param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="ownerControl" /> parameter value is <see langword="null" />. </exception>
			// Token: 0x06005795 RID: 22421 RVA: 0x001707DC File Offset: 0x0016E9DC
			public ControlAccessibleObject(Control ownerControl)
			{
				if (ownerControl == null)
				{
					throw new ArgumentNullException("ownerControl");
				}
				this.ownerControl = ownerControl;
				IntPtr intPtr = ownerControl.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.Handle = intPtr;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x06005796 RID: 22422 RVA: 0x00170840 File Offset: 0x0016EA40
			internal ControlAccessibleObject(Control ownerControl, int accObjId)
			{
				if (ownerControl == null)
				{
					throw new ArgumentNullException("ownerControl");
				}
				base.AccessibleObjectId = accObjId;
				this.ownerControl = ownerControl;
				IntPtr intPtr = ownerControl.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.Handle = intPtr;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x06005797 RID: 22423 RVA: 0x001708AC File Offset: 0x0016EAAC
			internal override int[] GetSysChildOrder()
			{
				if (this.ownerControl.GetStyle(ControlStyles.ContainerControl))
				{
					return this.ownerControl.GetChildWindowsInTabOrder();
				}
				return base.GetSysChildOrder();
			}

			// Token: 0x06005798 RID: 22424 RVA: 0x001708D0 File Offset: 0x0016EAD0
			internal override bool GetSysChild(AccessibleNavigation navdir, out AccessibleObject accessibleObject)
			{
				accessibleObject = null;
				Control parentInternal = this.ownerControl.ParentInternal;
				int num = -1;
				Control[] array = null;
				switch (navdir)
				{
				case AccessibleNavigation.Next:
					if (base.IsNonClientObject && parentInternal != null)
					{
						array = parentInternal.GetChildControlsInTabOrder(true);
						num = Array.IndexOf<Control>(array, this.ownerControl);
						if (num != -1)
						{
							num++;
						}
					}
					break;
				case AccessibleNavigation.Previous:
					if (base.IsNonClientObject && parentInternal != null)
					{
						array = parentInternal.GetChildControlsInTabOrder(true);
						num = Array.IndexOf<Control>(array, this.ownerControl);
						if (num != -1)
						{
							num--;
						}
					}
					break;
				case AccessibleNavigation.FirstChild:
					if (base.IsClientObject)
					{
						array = this.ownerControl.GetChildControlsInTabOrder(true);
						num = 0;
					}
					break;
				case AccessibleNavigation.LastChild:
					if (base.IsClientObject)
					{
						array = this.ownerControl.GetChildControlsInTabOrder(true);
						num = array.Length - 1;
					}
					break;
				}
				if (array == null || array.Length == 0)
				{
					return false;
				}
				if (num >= 0 && num < array.Length)
				{
					accessibleObject = array[num].NcAccessibilityObject;
				}
				return true;
			}

			/// <summary>Gets a string that describes the default action of the object. Not all objects have a default action.</summary>
			/// <returns>A description of the default action for an object, or <see langword="null" /> if this object has no default action.</returns>
			// Token: 0x170014DC RID: 5340
			// (get) Token: 0x06005799 RID: 22425 RVA: 0x001709B8 File Offset: 0x0016EBB8
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = this.ownerControl.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return base.DefaultAction;
				}
			}

			// Token: 0x170014DD RID: 5341
			// (get) Token: 0x0600579A RID: 22426 RVA: 0x001709DC File Offset: 0x0016EBDC
			internal override int[] RuntimeId
			{
				get
				{
					if (this.runtimeId == null)
					{
						this.runtimeId = new int[2];
						this.runtimeId[0] = 42;
						this.runtimeId[1] = (int)((long)this.Handle);
					}
					return this.runtimeId;
				}
			}

			/// <summary>Gets the description of the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />.</summary>
			/// <returns>A string describing the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />.</returns>
			// Token: 0x170014DE RID: 5342
			// (get) Token: 0x0600579B RID: 22427 RVA: 0x00170A18 File Offset: 0x0016EC18
			public override string Description
			{
				get
				{
					string accessibleDescription = this.ownerControl.AccessibleDescription;
					if (accessibleDescription != null)
					{
						return accessibleDescription;
					}
					return base.Description;
				}
			}

			/// <summary>Gets or sets the handle of the accessible object.</summary>
			/// <returns>An <see cref="T:System.IntPtr" /> that represents the handle of the control.</returns>
			// Token: 0x170014DF RID: 5343
			// (get) Token: 0x0600579C RID: 22428 RVA: 0x00170A3C File Offset: 0x0016EC3C
			// (set) Token: 0x0600579D RID: 22429 RVA: 0x00170A44 File Offset: 0x0016EC44
			public IntPtr Handle
			{
				get
				{
					return this.handle;
				}
				set
				{
					IntSecurity.UnmanagedCode.Demand();
					if (this.handle != value)
					{
						this.handle = value;
						if (Control.ControlAccessibleObject.oleAccAvailable == IntPtr.Zero)
						{
							return;
						}
						bool flag = false;
						if (Control.ControlAccessibleObject.oleAccAvailable == NativeMethods.InvalidIntPtr)
						{
							Control.ControlAccessibleObject.oleAccAvailable = UnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable("oleacc.dll");
							flag = (Control.ControlAccessibleObject.oleAccAvailable != IntPtr.Zero);
						}
						if (this.handle != IntPtr.Zero && Control.ControlAccessibleObject.oleAccAvailable != IntPtr.Zero)
						{
							base.UseStdAccessibleObjects(this.handle);
						}
						if (flag)
						{
							UnsafeNativeMethods.FreeLibrary(new HandleRef(null, Control.ControlAccessibleObject.oleAccAvailable));
						}
					}
				}
			}

			/// <summary>Gets the description of what the object does or how the object is used.</summary>
			/// <returns>The description of what the object does or how the object is used.</returns>
			// Token: 0x170014E0 RID: 5344
			// (get) Token: 0x0600579E RID: 22430 RVA: 0x00170AFC File Offset: 0x0016ECFC
			public override string Help
			{
				get
				{
					QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[Control.EventQueryAccessibilityHelp];
					if (queryAccessibilityHelpEventHandler != null)
					{
						QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
						queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
						return queryAccessibilityHelpEventArgs.HelpString;
					}
					return base.Help;
				}
			}

			/// <summary>Gets the object shortcut key or access key for an accessible object.</summary>
			/// <returns>The object shortcut key or access key for an accessible object, or <see langword="null" /> if there is no shortcut key associated with the object.</returns>
			// Token: 0x170014E1 RID: 5345
			// (get) Token: 0x0600579F RID: 22431 RVA: 0x00170B48 File Offset: 0x0016ED48
			public override string KeyboardShortcut
			{
				get
				{
					char mnemonic = WindowsFormsUtils.GetMnemonic(this.TextLabel, false);
					if (mnemonic != '\0')
					{
						return "Alt+" + mnemonic.ToString();
					}
					return null;
				}
			}

			/// <summary>Gets or sets the accessible object name.</summary>
			/// <returns>The accessible object name.</returns>
			// Token: 0x170014E2 RID: 5346
			// (get) Token: 0x060057A0 RID: 22432 RVA: 0x00170B78 File Offset: 0x0016ED78
			// (set) Token: 0x060057A1 RID: 22433 RVA: 0x00170BA1 File Offset: 0x0016EDA1
			public override string Name
			{
				get
				{
					string accessibleName = this.ownerControl.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return WindowsFormsUtils.TextWithoutMnemonics(this.TextLabel);
				}
				set
				{
					this.ownerControl.AccessibleName = value;
				}
			}

			/// <summary>Gets the parent of an accessible object.</summary>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the parent of an accessible object, or <see langword="null" /> if there is no parent object.</returns>
			// Token: 0x170014E3 RID: 5347
			// (get) Token: 0x060057A2 RID: 22434 RVA: 0x00170BAF File Offset: 0x0016EDAF
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return base.Parent;
				}
			}

			// Token: 0x170014E4 RID: 5348
			// (get) Token: 0x060057A3 RID: 22435 RVA: 0x00170BB8 File Offset: 0x0016EDB8
			internal string TextLabel
			{
				get
				{
					if (this.ownerControl.GetStyle(ControlStyles.UseTextForAccessibility))
					{
						string text = this.ownerControl.Text;
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
					Label previousLabel = this.PreviousLabel;
					if (previousLabel != null)
					{
						string text2 = previousLabel.Text;
						if (!string.IsNullOrEmpty(text2))
						{
							return text2;
						}
					}
					return null;
				}
			}

			/// <summary>Gets the owner of the accessible object.</summary>
			/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that owns the <see cref="T:System.Windows.Forms.Control.ControlAccessibleObject" />.</returns>
			// Token: 0x170014E5 RID: 5349
			// (get) Token: 0x060057A4 RID: 22436 RVA: 0x00170C09 File Offset: 0x0016EE09
			public Control Owner
			{
				get
				{
					return this.ownerControl;
				}
			}

			// Token: 0x170014E6 RID: 5350
			// (get) Token: 0x060057A5 RID: 22437 RVA: 0x00170C14 File Offset: 0x0016EE14
			internal Label PreviousLabel
			{
				get
				{
					Control parentInternal = this.Owner.ParentInternal;
					if (parentInternal == null)
					{
						return null;
					}
					ContainerControl containerControl = parentInternal.GetContainerControlInternal() as ContainerControl;
					if (containerControl == null)
					{
						return null;
					}
					for (Control nextControl = containerControl.GetNextControl(this.Owner, false); nextControl != null; nextControl = containerControl.GetNextControl(nextControl, false))
					{
						if (nextControl is Label)
						{
							return nextControl as Label;
						}
						if (nextControl.Visible && nextControl.TabStop)
						{
							break;
						}
					}
					return null;
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values.</returns>
			// Token: 0x170014E7 RID: 5351
			// (get) Token: 0x060057A6 RID: 22438 RVA: 0x00170C80 File Offset: 0x0016EE80
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerControl.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return base.Role;
				}
			}

			/// <summary>Gets an identifier for a Help topic and the path to the Help file associated with this accessible object.</summary>
			/// <param name="fileName">When this method returns, contains a string that represents the path to the Help file associated with this accessible object. This parameter is passed uninitialized. </param>
			/// <returns>An identifier for a Help topic, or -1 if there is no Help topic. On return, the <paramref name="fileName" /> parameter will contain the path to the Help file associated with this accessible object, or <see langword="null" /> if there is no <see langword="IAccessible" /> interface specified.</returns>
			// Token: 0x060057A7 RID: 22439 RVA: 0x00170CA8 File Offset: 0x0016EEA8
			public override int GetHelpTopic(out string fileName)
			{
				int result = 0;
				QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[Control.EventQueryAccessibilityHelp];
				if (queryAccessibilityHelpEventHandler != null)
				{
					QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
					queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
					fileName = queryAccessibilityHelpEventArgs.HelpNamespace;
					if (!string.IsNullOrEmpty(fileName))
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					try
					{
						result = int.Parse(queryAccessibilityHelpEventArgs.HelpKeyword, CultureInfo.InvariantCulture);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
					}
					return result;
				}
				return base.GetHelpTopic(out fileName);
			}

			/// <summary>Notifies accessibility client applications of the specified <see cref="T:System.Windows.Forms.AccessibleEvents" />.</summary>
			/// <param name="accEvent">The <see cref="T:System.Windows.Forms.AccessibleEvents" /> to notify the accessibility client applications of. </param>
			// Token: 0x060057A8 RID: 22440 RVA: 0x00170D3C File Offset: 0x0016EF3C
			public void NotifyClients(AccessibleEvents accEvent)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), -4, 0);
			}

			/// <summary>Notifies the accessibility client applications of the specified <see cref="T:System.Windows.Forms.AccessibleEvents" /> for the specified child control.</summary>
			/// <param name="accEvent">The <see cref="T:System.Windows.Forms.AccessibleEvents" /> to notify the accessibility client applications of. </param>
			/// <param name="childID">The child <see cref="T:System.Windows.Forms.Control" /> to notify of the accessible event. </param>
			// Token: 0x060057A9 RID: 22441 RVA: 0x00170D53 File Offset: 0x0016EF53
			public void NotifyClients(AccessibleEvents accEvent, int childID)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), -4, childID + 1);
			}

			/// <summary>Notifies the accessibility client applications of the specified <see cref="T:System.Windows.Forms.AccessibleEvents" /> for the specified child control, giving the identification of the <see cref="T:System.Windows.Forms.AccessibleObject" />.</summary>
			/// <param name="accEvent">The <see cref="T:System.Windows.Forms.AccessibleEvents" /> to notify the accessibility client applications of.</param>
			/// <param name="objectID">The identifier of the <see cref="T:System.Windows.Forms.AccessibleObject" />.</param>
			/// <param name="childID">The child <see cref="T:System.Windows.Forms.Control" /> to notify of the accessible event.</param>
			// Token: 0x060057AA RID: 22442 RVA: 0x00170D6C File Offset: 0x0016EF6C
			public void NotifyClients(AccessibleEvents accEvent, int objectID, int childID)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), objectID, childID + 1);
			}

			// Token: 0x060057AB RID: 22443 RVA: 0x00170D84 File Offset: 0x0016EF84
			public override bool RaiseLiveRegionChanged()
			{
				if (!(this.Owner is IAutomationLiveRegion))
				{
					throw new InvalidOperationException(SR.GetString("OwnerControlIsNotALiveRegion"));
				}
				return base.RaiseAutomationEvent(20024);
			}

			// Token: 0x060057AC RID: 22444 RVA: 0x00170DAE File Offset: 0x0016EFAE
			internal override bool IsIAccessibleExSupported()
			{
				return (AccessibilityImprovements.Level3 && this.Owner is IAutomationLiveRegion) || base.IsIAccessibleExSupported();
			}

			// Token: 0x060057AD RID: 22445 RVA: 0x00170DCC File Offset: 0x0016EFCC
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30135 && this.Owner is IAutomationLiveRegion)
				{
					return ((IAutomationLiveRegion)this.Owner).LiveSetting;
				}
				if (this.Owner.SupportsUiaProviders)
				{
					if (propertyID <= 30009)
					{
						if (propertyID == 30007)
						{
							return string.Empty;
						}
						if (propertyID == 30009)
						{
							return this.Owner.CanSelect;
						}
					}
					else if (propertyID != 30013)
					{
						if (propertyID == 30019 || propertyID == 30022)
						{
							return false;
						}
					}
					else
					{
						string help = this.Help;
						if (!AccessibilityImprovements.Level3)
						{
							return help;
						}
						return help ?? string.Empty;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170014E8 RID: 5352
			// (get) Token: 0x060057AE RID: 22446 RVA: 0x00170E8C File Offset: 0x0016F08C
			internal override UnsafeNativeMethods.IRawElementProviderSimple HostRawElementProvider
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						UnsafeNativeMethods.IRawElementProviderSimple result;
						UnsafeNativeMethods.UiaHostProviderFromHwnd(new HandleRef(this, this.Handle), out result);
						return result;
					}
					return base.HostRawElementProvider;
				}
			}

			/// <summary>Returns a string that represents the current object.</summary>
			/// <returns>A string that represents the current object.</returns>
			// Token: 0x060057AF RID: 22447 RVA: 0x00170EBC File Offset: 0x0016F0BC
			public override string ToString()
			{
				if (this.Owner != null)
				{
					return "ControlAccessibleObject: Owner = " + this.Owner.ToString();
				}
				return "ControlAccessibleObject: Owner = null";
			}

			// Token: 0x04003854 RID: 14420
			private static IntPtr oleAccAvailable = NativeMethods.InvalidIntPtr;

			// Token: 0x04003855 RID: 14421
			private IntPtr handle = IntPtr.Zero;

			// Token: 0x04003856 RID: 14422
			private Control ownerControl;

			// Token: 0x04003857 RID: 14423
			private int[] runtimeId;
		}

		// Token: 0x02000583 RID: 1411
		internal sealed class FontHandleWrapper : MarshalByRefObject, IDisposable
		{
			// Token: 0x060057B1 RID: 22449 RVA: 0x00170EED File Offset: 0x0016F0ED
			internal FontHandleWrapper(Font font)
			{
				this.handle = font.ToHfont();
				System.Internal.HandleCollector.Add(this.handle, NativeMethods.CommonHandles.GDI);
			}

			// Token: 0x170014E9 RID: 5353
			// (get) Token: 0x060057B2 RID: 22450 RVA: 0x00170F12 File Offset: 0x0016F112
			internal IntPtr Handle
			{
				get
				{
					return this.handle;
				}
			}

			// Token: 0x060057B3 RID: 22451 RVA: 0x00170F1A File Offset: 0x0016F11A
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060057B4 RID: 22452 RVA: 0x00170F29 File Offset: 0x0016F129
			private void Dispose(bool disposing)
			{
				if (this.handle != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(this, this.handle));
					this.handle = IntPtr.Zero;
				}
			}

			// Token: 0x060057B5 RID: 22453 RVA: 0x00170F5C File Offset: 0x0016F15C
			~FontHandleWrapper()
			{
				this.Dispose(false);
			}

			// Token: 0x04003858 RID: 14424
			private IntPtr handle;
		}

		// Token: 0x02000584 RID: 1412
		private class ThreadMethodEntry : IAsyncResult
		{
			// Token: 0x060057B6 RID: 22454 RVA: 0x00170F8C File Offset: 0x0016F18C
			internal ThreadMethodEntry(Control caller, Control marshaler, Delegate method, object[] args, bool synchronous, ExecutionContext executionContext)
			{
				this.caller = caller;
				this.marshaler = marshaler;
				this.method = method;
				this.args = args;
				this.exception = null;
				this.retVal = null;
				this.synchronous = synchronous;
				this.isCompleted = false;
				this.resetEvent = null;
				this.executionContext = executionContext;
			}

			// Token: 0x060057B7 RID: 22455 RVA: 0x00170FF4 File Offset: 0x0016F1F4
			~ThreadMethodEntry()
			{
				if (this.resetEvent != null)
				{
					this.resetEvent.Close();
				}
			}

			// Token: 0x170014EA RID: 5354
			// (get) Token: 0x060057B8 RID: 22456 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public object AsyncState
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170014EB RID: 5355
			// (get) Token: 0x060057B9 RID: 22457 RVA: 0x00171030 File Offset: 0x0016F230
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					if (this.resetEvent == null)
					{
						object obj = this.invokeSyncObject;
						lock (obj)
						{
							if (this.resetEvent == null)
							{
								this.resetEvent = new ManualResetEvent(false);
								if (this.isCompleted)
								{
									this.resetEvent.Set();
								}
							}
						}
					}
					return this.resetEvent;
				}
			}

			// Token: 0x170014EC RID: 5356
			// (get) Token: 0x060057BA RID: 22458 RVA: 0x001710A0 File Offset: 0x0016F2A0
			public bool CompletedSynchronously
			{
				get
				{
					return this.isCompleted && this.synchronous;
				}
			}

			// Token: 0x170014ED RID: 5357
			// (get) Token: 0x060057BB RID: 22459 RVA: 0x001710B5 File Offset: 0x0016F2B5
			public bool IsCompleted
			{
				get
				{
					return this.isCompleted;
				}
			}

			// Token: 0x060057BC RID: 22460 RVA: 0x001710C0 File Offset: 0x0016F2C0
			internal void Complete()
			{
				object obj = this.invokeSyncObject;
				lock (obj)
				{
					this.isCompleted = true;
					if (this.resetEvent != null)
					{
						this.resetEvent.Set();
					}
				}
			}

			// Token: 0x04003859 RID: 14425
			internal Control caller;

			// Token: 0x0400385A RID: 14426
			internal Control marshaler;

			// Token: 0x0400385B RID: 14427
			internal Delegate method;

			// Token: 0x0400385C RID: 14428
			internal object[] args;

			// Token: 0x0400385D RID: 14429
			internal object retVal;

			// Token: 0x0400385E RID: 14430
			internal Exception exception;

			// Token: 0x0400385F RID: 14431
			internal bool synchronous;

			// Token: 0x04003860 RID: 14432
			private bool isCompleted;

			// Token: 0x04003861 RID: 14433
			private ManualResetEvent resetEvent;

			// Token: 0x04003862 RID: 14434
			private object invokeSyncObject = new object();

			// Token: 0x04003863 RID: 14435
			internal ExecutionContext executionContext;

			// Token: 0x04003864 RID: 14436
			internal SynchronizationContext syncContext;
		}

		// Token: 0x02000585 RID: 1413
		private class ControlVersionInfo
		{
			// Token: 0x060057BD RID: 22461 RVA: 0x00171118 File Offset: 0x0016F318
			internal ControlVersionInfo(Control owner)
			{
				this.owner = owner;
			}

			// Token: 0x170014EE RID: 5358
			// (get) Token: 0x060057BE RID: 22462 RVA: 0x00171128 File Offset: 0x0016F328
			internal string CompanyName
			{
				get
				{
					if (this.companyName == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							this.companyName = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
						}
						if (this.companyName == null || this.companyName.Length == 0)
						{
							this.companyName = this.GetFileVersionInfo().CompanyName;
							if (this.companyName != null)
							{
								this.companyName = this.companyName.Trim();
							}
						}
						if (this.companyName == null || this.companyName.Length == 0)
						{
							string text = this.owner.GetType().Namespace;
							if (text == null)
							{
								text = string.Empty;
							}
							int num = text.IndexOf("/");
							if (num != -1)
							{
								this.companyName = text.Substring(0, num);
							}
							else
							{
								this.companyName = text;
							}
						}
					}
					return this.companyName;
				}
			}

			// Token: 0x170014EF RID: 5359
			// (get) Token: 0x060057BF RID: 22463 RVA: 0x0017121C File Offset: 0x0016F41C
			internal string ProductName
			{
				get
				{
					if (this.productName == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							this.productName = ((AssemblyProductAttribute)customAttributes[0]).Product;
						}
						if (this.productName == null || this.productName.Length == 0)
						{
							this.productName = this.GetFileVersionInfo().ProductName;
							if (this.productName != null)
							{
								this.productName = this.productName.Trim();
							}
						}
						if (this.productName == null || this.productName.Length == 0)
						{
							string text = this.owner.GetType().Namespace;
							if (text == null)
							{
								text = string.Empty;
							}
							int num = text.IndexOf(".");
							if (num != -1)
							{
								this.productName = text.Substring(num + 1);
							}
							else
							{
								this.productName = text;
							}
						}
					}
					return this.productName;
				}
			}

			// Token: 0x170014F0 RID: 5360
			// (get) Token: 0x060057C0 RID: 22464 RVA: 0x00171310 File Offset: 0x0016F510
			internal string ProductVersion
			{
				get
				{
					if (this.productVersion == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							this.productVersion = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
						}
						if (this.productVersion == null || this.productVersion.Length == 0)
						{
							this.productVersion = this.GetFileVersionInfo().ProductVersion;
							if (this.productVersion != null)
							{
								this.productVersion = this.productVersion.Trim();
							}
						}
						if (this.productVersion == null || this.productVersion.Length == 0)
						{
							this.productVersion = "1.0.0.0";
						}
					}
					return this.productVersion;
				}
			}

			// Token: 0x060057C1 RID: 22465 RVA: 0x001713D0 File Offset: 0x0016F5D0
			private FileVersionInfo GetFileVersionInfo()
			{
				if (this.versionInfo == null)
				{
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Assert();
					string fullyQualifiedName;
					try
					{
						fullyQualifiedName = this.owner.GetType().Module.FullyQualifiedName;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					new FileIOPermission(FileIOPermissionAccess.Read, fullyQualifiedName).Assert();
					try
					{
						this.versionInfo = FileVersionInfo.GetVersionInfo(fullyQualifiedName);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return this.versionInfo;
			}

			// Token: 0x04003865 RID: 14437
			private string companyName;

			// Token: 0x04003866 RID: 14438
			private string productName;

			// Token: 0x04003867 RID: 14439
			private string productVersion;

			// Token: 0x04003868 RID: 14440
			private FileVersionInfo versionInfo;

			// Token: 0x04003869 RID: 14441
			private Control owner;
		}

		// Token: 0x02000586 RID: 1414
		private sealed class MultithreadSafeCallScope : IDisposable
		{
			// Token: 0x060057C2 RID: 22466 RVA: 0x0017145C File Offset: 0x0016F65C
			internal MultithreadSafeCallScope()
			{
				if (Control.checkForIllegalCrossThreadCalls && !Control.inCrossThreadSafeCall)
				{
					Control.inCrossThreadSafeCall = true;
					this.resultedInSet = true;
					return;
				}
				this.resultedInSet = false;
			}

			// Token: 0x060057C3 RID: 22467 RVA: 0x00171487 File Offset: 0x0016F687
			void IDisposable.Dispose()
			{
				if (this.resultedInSet)
				{
					Control.inCrossThreadSafeCall = false;
				}
			}

			// Token: 0x0400386A RID: 14442
			private bool resultedInSet;
		}

		// Token: 0x02000587 RID: 1415
		private sealed class PrintPaintEventArgs : PaintEventArgs
		{
			// Token: 0x060057C4 RID: 22468 RVA: 0x00171497 File Offset: 0x0016F697
			internal PrintPaintEventArgs(Message m, IntPtr dc, Rectangle clipRect) : base(dc, clipRect)
			{
				this.m = m;
			}

			// Token: 0x170014F1 RID: 5361
			// (get) Token: 0x060057C5 RID: 22469 RVA: 0x001714A8 File Offset: 0x0016F6A8
			internal Message Message
			{
				get
				{
					return this.m;
				}
			}

			// Token: 0x0400386B RID: 14443
			private Message m;
		}
	}
}
