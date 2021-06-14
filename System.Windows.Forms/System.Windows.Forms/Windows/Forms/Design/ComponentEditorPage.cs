using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	/// <summary>Provides a base implementation for a <see cref="T:System.Windows.Forms.Design.ComponentEditorPage" />.</summary>
	// Token: 0x02000497 RID: 1175
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ComponentEditorPage : Panel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Design.ComponentEditorPage" /> class.</summary>
		// Token: 0x06004FD6 RID: 20438 RVA: 0x0014C192 File Offset: 0x0014A392
		public ComponentEditorPage()
		{
			this.commitOnDeactivate = false;
			this.firstActivate = true;
			this.loadRequired = false;
			this.loading = 0;
			base.Visible = false;
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x000F7531 File Offset: 0x000F5731
		// (set) Token: 0x06004FD8 RID: 20440 RVA: 0x000F7539 File Offset: 0x000F5739
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000408 RID: 1032
		// (add) Token: 0x06004FD9 RID: 20441 RVA: 0x000F7542 File Offset: 0x000F5742
		// (remove) Token: 0x06004FDA RID: 20442 RVA: 0x000F754B File Offset: 0x000F574B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Gets or sets the page site.</summary>
		/// <returns>The page site.</returns>
		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06004FDB RID: 20443 RVA: 0x0014C1BD File Offset: 0x0014A3BD
		// (set) Token: 0x06004FDC RID: 20444 RVA: 0x0014C1C5 File Offset: 0x0014A3C5
		protected IComponentEditorPageSite PageSite
		{
			get
			{
				return this.pageSite;
			}
			set
			{
				this.pageSite = value;
			}
		}

		/// <summary>Gets or sets the component to edit.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.IComponent" /> this page allows you to edit.</returns>
		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x0014C1CE File Offset: 0x0014A3CE
		// (set) Token: 0x06004FDE RID: 20446 RVA: 0x0014C1D6 File Offset: 0x0014A3D6
		protected IComponent Component
		{
			get
			{
				return this.component;
			}
			set
			{
				this.component = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the page is being activated for the first time.</summary>
		/// <returns>
		///     <see langword="true" /> if the page has not previously been activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06004FDF RID: 20447 RVA: 0x0014C1DF File Offset: 0x0014A3DF
		// (set) Token: 0x06004FE0 RID: 20448 RVA: 0x0014C1E7 File Offset: 0x0014A3E7
		protected bool FirstActivate
		{
			get
			{
				return this.firstActivate;
			}
			set
			{
				this.firstActivate = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a component must be loaded before editing can occur.</summary>
		/// <returns>
		///     <see langword="true" /> if a component must be loaded before editing can occur; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06004FE1 RID: 20449 RVA: 0x0014C1F0 File Offset: 0x0014A3F0
		// (set) Token: 0x06004FE2 RID: 20450 RVA: 0x0014C1F8 File Offset: 0x0014A3F8
		protected bool LoadRequired
		{
			get
			{
				return this.loadRequired;
			}
			set
			{
				this.loadRequired = value;
			}
		}

		/// <summary>Indicates how many load dependencies remain until loading has been completed.</summary>
		/// <returns>The number of remaining load dependencies.</returns>
		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06004FE3 RID: 20451 RVA: 0x0014C201 File Offset: 0x0014A401
		// (set) Token: 0x06004FE4 RID: 20452 RVA: 0x0014C209 File Offset: 0x0014A409
		protected int Loading
		{
			get
			{
				return this.loading;
			}
			set
			{
				this.loading = value;
			}
		}

		/// <summary>Specifies whether the editor should apply its changes before it is deactivated.</summary>
		/// <returns>
		///     <see langword="true" /> if the editor should apply its changes; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x0014C212 File Offset: 0x0014A412
		// (set) Token: 0x06004FE6 RID: 20454 RVA: 0x0014C21A File Offset: 0x0014A41A
		public bool CommitOnDeactivate
		{
			get
			{
				return this.commitOnDeactivate;
			}
			set
			{
				this.commitOnDeactivate = value;
			}
		}

		/// <summary>Gets the creation parameters for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that indicates the creation parameters for the control.</returns>
		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x0014C224 File Offset: 0x0014A424
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -12582913;
				return createParams;
			}
		}

		/// <summary>Gets or sets the icon for the page.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> used to represent the page.</returns>
		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x0014C24B File Offset: 0x0014A44B
		// (set) Token: 0x06004FE9 RID: 20457 RVA: 0x0014C275 File Offset: 0x0014A475
		public Icon Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = new Icon(typeof(ComponentEditorPage), "ComponentEditorPage.ico");
				}
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}

		/// <summary>Gets the title of the page.</summary>
		/// <returns>The title of the page.</returns>
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06004FEA RID: 20458 RVA: 0x000FEA84 File Offset: 0x000FCC84
		public virtual string Title
		{
			get
			{
				return base.Text;
			}
		}

		/// <summary>Activates and displays the page.</summary>
		// Token: 0x06004FEB RID: 20459 RVA: 0x0014C27E File Offset: 0x0014A47E
		public virtual void Activate()
		{
			if (this.loadRequired)
			{
				this.EnterLoadingMode();
				this.LoadComponent();
				this.ExitLoadingMode();
				this.loadRequired = false;
			}
			base.Visible = true;
			this.firstActivate = false;
		}

		/// <summary>Applies changes to all the components being edited.</summary>
		// Token: 0x06004FEC RID: 20460 RVA: 0x0014C2AF File Offset: 0x0014A4AF
		public virtual void ApplyChanges()
		{
			this.SaveComponent();
		}

		/// <summary>Deactivates and hides the page.</summary>
		// Token: 0x06004FED RID: 20461 RVA: 0x0002C34D File Offset: 0x0002A54D
		public virtual void Deactivate()
		{
			base.Visible = false;
		}

		/// <summary>Increments the loading counter.</summary>
		// Token: 0x06004FEE RID: 20462 RVA: 0x0014C2B7 File Offset: 0x0014A4B7
		protected void EnterLoadingMode()
		{
			this.loading++;
		}

		/// <summary>Decrements the loading counter.</summary>
		// Token: 0x06004FEF RID: 20463 RVA: 0x0014C2C7 File Offset: 0x0014A4C7
		protected void ExitLoadingMode()
		{
			this.loading--;
		}

		/// <summary>Gets the control that represents the window for this page.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that represents the window for this page.</returns>
		// Token: 0x06004FF0 RID: 20464 RVA: 0x000069BD File Offset: 0x00004BBD
		public virtual Control GetControl()
		{
			return this;
		}

		/// <summary>Gets the component that is to be edited.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.IComponent" /> that is to be edited.</returns>
		// Token: 0x06004FF1 RID: 20465 RVA: 0x0014C1CE File Offset: 0x0014A3CE
		protected IComponent GetSelectedComponent()
		{
			return this.component;
		}

		/// <summary>Processes messages that could be handled by the page.</summary>
		/// <param name="msg">The message to process. </param>
		/// <returns>
		///     <see langword="true" /> if the page processed the message; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004FF2 RID: 20466 RVA: 0x0014C2D7 File Offset: 0x0014A4D7
		public virtual bool IsPageMessage(ref Message msg)
		{
			return this.PreProcessMessage(ref msg);
		}

		/// <summary>Gets a value indicating whether the page is being activated for the first time.</summary>
		/// <returns>
		///     <see langword="true" /> if this is the first time the page is being activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004FF3 RID: 20467 RVA: 0x0014C1DF File Offset: 0x0014A3DF
		protected bool IsFirstActivate()
		{
			return this.firstActivate;
		}

		/// <summary>Gets a value indicating whether the page is being loaded.</summary>
		/// <returns>
		///     <see langword="true" /> if the page is being loaded; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004FF4 RID: 20468 RVA: 0x0014C2E0 File Offset: 0x0014A4E0
		protected bool IsLoading()
		{
			return this.loading != 0;
		}

		/// <summary>Loads the component into the page user interface (UI).</summary>
		// Token: 0x06004FF5 RID: 20469
		protected abstract void LoadComponent();

		/// <summary>Called when the page and any sibling pages have applied their changes.</summary>
		// Token: 0x06004FF6 RID: 20470 RVA: 0x0014C2EB File Offset: 0x0014A4EB
		public virtual void OnApplyComplete()
		{
			this.ReloadComponent();
		}

		/// <summary>Reloads the component for the page.</summary>
		// Token: 0x06004FF7 RID: 20471 RVA: 0x0014C2F3 File Offset: 0x0014A4F3
		protected virtual void ReloadComponent()
		{
			if (!base.Visible)
			{
				this.loadRequired = true;
			}
		}

		/// <summary>Saves the component from the page user interface (UI).</summary>
		// Token: 0x06004FF8 RID: 20472
		protected abstract void SaveComponent();

		/// <summary>Sets the page as changed since the last load or save.</summary>
		// Token: 0x06004FF9 RID: 20473 RVA: 0x0014C304 File Offset: 0x0014A504
		protected virtual void SetDirty()
		{
			if (!this.IsLoading())
			{
				this.pageSite.SetDirty();
			}
		}

		/// <summary>Sets the component to be edited.</summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to be edited. </param>
		// Token: 0x06004FFA RID: 20474 RVA: 0x0014C319 File Offset: 0x0014A519
		public virtual void SetComponent(IComponent component)
		{
			this.component = component;
			this.loadRequired = true;
		}

		/// <summary>Sets the site for this page.</summary>
		/// <param name="site">The site for this page. </param>
		// Token: 0x06004FFB RID: 20475 RVA: 0x0014C329 File Offset: 0x0014A529
		public virtual void SetSite(IComponentEditorPageSite site)
		{
			this.pageSite = site;
			this.pageSite.GetControl().Controls.Add(this);
		}

		/// <summary>Shows Help information if the page supports Help information.</summary>
		// Token: 0x06004FFC RID: 20476 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void ShowHelp()
		{
		}

		/// <summary>Gets a value indicating whether the editor supports Help.</summary>
		/// <returns>
		///     <see langword="true" /> if the editor supports Help; otherwise, <see langword="false" />. The default implementation returns <see langword="false" />.</returns>
		// Token: 0x06004FFD RID: 20477 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool SupportsHelp()
		{
			return false;
		}

		// Token: 0x040033F0 RID: 13296
		private IComponentEditorPageSite pageSite;

		// Token: 0x040033F1 RID: 13297
		private IComponent component;

		// Token: 0x040033F2 RID: 13298
		private bool firstActivate;

		// Token: 0x040033F3 RID: 13299
		private bool loadRequired;

		// Token: 0x040033F4 RID: 13300
		private int loading;

		// Token: 0x040033F5 RID: 13301
		private Icon icon;

		// Token: 0x040033F6 RID: 13302
		private bool commitOnDeactivate;
	}
}
