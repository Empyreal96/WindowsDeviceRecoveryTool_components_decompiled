using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Specifies the contextual information about an application thread.</summary>
	// Token: 0x02000112 RID: 274
	public class ApplicationContext : IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ApplicationContext" /> class with no context.</summary>
		// Token: 0x06000601 RID: 1537 RVA: 0x000117A3 File Offset: 0x0000F9A3
		public ApplicationContext() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ApplicationContext" /> class with the specified <see cref="T:System.Windows.Forms.Form" />.</summary>
		/// <param name="mainForm">The main <see cref="T:System.Windows.Forms.Form" /> of the application to use for context. </param>
		// Token: 0x06000602 RID: 1538 RVA: 0x000117AC File Offset: 0x0000F9AC
		public ApplicationContext(Form mainForm)
		{
			this.MainForm = mainForm;
		}

		/// <summary>Attempts to free resources and perform other cleanup operations before the application context is reclaimed by garbage collection.</summary>
		// Token: 0x06000603 RID: 1539 RVA: 0x000117BC File Offset: 0x0000F9BC
		~ApplicationContext()
		{
			this.Dispose(false);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.Form" /> to use as context.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Form" /> to use as context.</returns>
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000117EC File Offset: 0x0000F9EC
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x000117F4 File Offset: 0x0000F9F4
		public Form MainForm
		{
			get
			{
				return this.mainForm;
			}
			set
			{
				EventHandler value2 = new EventHandler(this.OnMainFormDestroy);
				if (this.mainForm != null)
				{
					this.mainForm.HandleDestroyed -= value2;
				}
				this.mainForm = value;
				if (this.mainForm != null)
				{
					this.mainForm.HandleDestroyed += value2;
				}
			}
		}

		/// <summary>Gets or sets an object that contains data about the control.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the control. The default is <see langword="null" />.</returns>
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0001183D File Offset: 0x0000FA3D
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00011845 File Offset: 0x0000FA45
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
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Occurs when the message loop of the thread should be terminated, by calling <see cref="M:System.Windows.Forms.ApplicationContext.ExitThread" />.</summary>
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000608 RID: 1544 RVA: 0x00011850 File Offset: 0x0000FA50
		// (remove) Token: 0x06000609 RID: 1545 RVA: 0x00011888 File Offset: 0x0000FA88
		public event EventHandler ThreadExit;

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.ApplicationContext" />.</summary>
		// Token: 0x0600060A RID: 1546 RVA: 0x000118BD File Offset: 0x0000FABD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ApplicationContext" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600060B RID: 1547 RVA: 0x000118CC File Offset: 0x0000FACC
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.mainForm != null)
			{
				if (!this.mainForm.IsDisposed)
				{
					this.mainForm.Dispose();
				}
				this.mainForm = null;
			}
		}

		/// <summary>Terminates the message loop of the thread.</summary>
		// Token: 0x0600060C RID: 1548 RVA: 0x000118F8 File Offset: 0x0000FAF8
		public void ExitThread()
		{
			this.ExitThreadCore();
		}

		/// <summary>Terminates the message loop of the thread.</summary>
		// Token: 0x0600060D RID: 1549 RVA: 0x00011900 File Offset: 0x0000FB00
		protected virtual void ExitThreadCore()
		{
			if (this.ThreadExit != null)
			{
				this.ThreadExit(this, EventArgs.Empty);
			}
		}

		/// <summary>Calls <see cref="M:System.Windows.Forms.ApplicationContext.ExitThreadCore" />, which raises the <see cref="E:System.Windows.Forms.ApplicationContext.ThreadExit" /> event.</summary>
		/// <param name="sender">The object that raised the event. </param>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600060E RID: 1550 RVA: 0x000118F8 File Offset: 0x0000FAF8
		protected virtual void OnMainFormClosed(object sender, EventArgs e)
		{
			this.ExitThreadCore();
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001191C File Offset: 0x0000FB1C
		private void OnMainFormDestroy(object sender, EventArgs e)
		{
			Form form = (Form)sender;
			if (!form.RecreatingHandle)
			{
				form.HandleDestroyed -= this.OnMainFormDestroy;
				this.OnMainFormClosed(sender, e);
			}
		}

		// Token: 0x04000536 RID: 1334
		private Form mainForm;

		// Token: 0x04000537 RID: 1335
		private object userData;
	}
}
