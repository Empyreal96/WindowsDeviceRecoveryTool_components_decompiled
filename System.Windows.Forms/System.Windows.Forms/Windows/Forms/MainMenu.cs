using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the menu structure of a form. Although <see cref="T:System.Windows.Forms.MenuStrip" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.MainMenu" /> control of previous versions, <see cref="T:System.Windows.Forms.MainMenu" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x020002D5 RID: 725
	[ToolboxItemFilter("System.Windows.Forms.MainMenu")]
	public class MainMenu : Menu
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MainMenu" /> class without any specified menu items.</summary>
		// Token: 0x06002B64 RID: 11108 RVA: 0x000CAF9D File Offset: 0x000C919D
		public MainMenu() : base(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MainMenu" /> class with the specified container. </summary>
		/// <param name="container">An <see cref="T:System.ComponentModel.IContainer" /> representing the container of the <see cref="T:System.Windows.Forms.MainMenu" />.</param>
		// Token: 0x06002B65 RID: 11109 RVA: 0x000CAFAD File Offset: 0x000C91AD
		public MainMenu(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MainMenu" /> with a specified set of <see cref="T:System.Windows.Forms.MenuItem" /> objects.</summary>
		/// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects that will be added to the <see cref="T:System.Windows.Forms.MainMenu" />. </param>
		// Token: 0x06002B66 RID: 11110 RVA: 0x000CAFCA File Offset: 0x000C91CA
		public MainMenu(MenuItem[] items) : base(items)
		{
		}

		/// <summary>Occurs when the main menu collapses.</summary>
		// Token: 0x14000206 RID: 518
		// (add) Token: 0x06002B67 RID: 11111 RVA: 0x000CAFDA File Offset: 0x000C91DA
		// (remove) Token: 0x06002B68 RID: 11112 RVA: 0x000CAFF3 File Offset: 0x000C91F3
		[SRDescription("MainMenuCollapseDescr")]
		public event EventHandler Collapse
		{
			add
			{
				this.onCollapse = (EventHandler)Delegate.Combine(this.onCollapse, value);
			}
			remove
			{
				this.onCollapse = (EventHandler)Delegate.Remove(this.onCollapse, value);
			}
		}

		/// <summary>Gets or sets whether the text displayed by the control is displayed from right to left.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a valid member of the <see cref="T:System.Windows.Forms.RightToLeft" /> enumeration. </exception>
		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x000CB00C File Offset: 0x000C920C
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x000CB034 File Offset: 0x000C9234
		[Localizable(true)]
		[AmbientValue(RightToLeft.Inherit)]
		[SRDescription("MenuRightToLeftDescr")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				if (RightToLeft.Inherit != this.rightToLeft)
				{
					return this.rightToLeft;
				}
				if (this.form != null)
				{
					return this.form.RightToLeft;
				}
				return RightToLeft.Inherit;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				if (this.rightToLeft != value)
				{
					this.rightToLeft = value;
					base.UpdateRtl(value == RightToLeft.Yes);
				}
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x000CB081 File Offset: 0x000C9281
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.RightToLeft == RightToLeft.Yes && (this.form == null || !this.form.IsMirrored);
			}
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.MainMenu" /> that is a duplicate of the current <see cref="T:System.Windows.Forms.MainMenu" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MainMenu" /> that represents the cloned menu.</returns>
		// Token: 0x06002B6C RID: 11116 RVA: 0x000CB0A8 File Offset: 0x000C92A8
		public virtual MainMenu CloneMenu()
		{
			MainMenu mainMenu = new MainMenu();
			mainMenu.CloneMenu(this);
			return mainMenu;
		}

		/// <summary>Creates a new handle to the Menu.</summary>
		/// <returns>A handle to the menu if the method succeeds; otherwise, <see langword="null" />.</returns>
		// Token: 0x06002B6D RID: 11117 RVA: 0x000CB0C3 File Offset: 0x000C92C3
		protected override IntPtr CreateMenuHandle()
		{
			return UnsafeNativeMethods.CreateMenu();
		}

		/// <summary>Disposes of the resources, other than memory, used by the <see cref="T:System.Windows.Forms.MainMenu" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06002B6E RID: 11118 RVA: 0x000CB0CA File Offset: 0x000C92CA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.form != null && (this.ownerForm == null || this.form == this.ownerForm))
			{
				this.form.Menu = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.Form" /> that contains this control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Form" /> that is the container for this control. Returns <see langword="null" /> if the <see cref="T:System.Windows.Forms.MainMenu" /> is not currently hosted on a form.</returns>
		// Token: 0x06002B6F RID: 11119 RVA: 0x000CB100 File Offset: 0x000C9300
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public Form GetForm()
		{
			return this.form;
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000CB100 File Offset: 0x000C9300
		internal Form GetFormUnsafe()
		{
			return this.form;
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000CB108 File Offset: 0x000C9308
		internal override void ItemsChanged(int change)
		{
			base.ItemsChanged(change);
			if (this.form != null)
			{
				this.form.MenuChanged(change, this);
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x000CB126 File Offset: 0x000C9326
		internal virtual void ItemsChanged(int change, Menu menu)
		{
			if (this.form != null)
			{
				this.form.MenuChanged(change, menu);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MainMenu.Collapse" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002B73 RID: 11123 RVA: 0x000CB13D File Offset: 0x000C933D
		protected internal virtual void OnCollapse(EventArgs e)
		{
			if (this.onCollapse != null)
			{
				this.onCollapse(this, e);
			}
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000CB154 File Offset: 0x000C9354
		internal virtual bool ShouldSerializeRightToLeft()
		{
			return RightToLeft.Inherit != this.RightToLeft;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.MainMenu" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.MainMenu" />.</returns>
		// Token: 0x06002B75 RID: 11125 RVA: 0x000CB162 File Offset: 0x000C9362
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x040012AA RID: 4778
		internal Form form;

		// Token: 0x040012AB RID: 4779
		internal Form ownerForm;

		// Token: 0x040012AC RID: 4780
		private RightToLeft rightToLeft = RightToLeft.Inherit;

		// Token: 0x040012AD RID: 4781
		private EventHandler onCollapse;
	}
}
