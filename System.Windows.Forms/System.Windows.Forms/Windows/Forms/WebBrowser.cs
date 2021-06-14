using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Enables the user to navigate Web pages inside your form. </summary>
	// Token: 0x02000423 RID: 1059
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Url")]
	[DefaultEvent("DocumentCompleted")]
	[Docking(DockingBehavior.AutoDock)]
	[SRDescription("DescriptionWebBrowser")]
	[Designer("System.Windows.Forms.Design.WebBrowserDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class WebBrowser : WebBrowserBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowser" /> class.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Windows.Forms.WebBrowser" /> control is hosted inside Internet Explorer.</exception>
		// Token: 0x0600496A RID: 18794 RVA: 0x00132600 File Offset: 0x00130800
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public WebBrowser() : base("8856f961-340a-11d0-a96b-00c04fd705a2")
		{
			this.CheckIfCreatedInIE();
			this.webBrowserState = new BitVector32(37);
			this.AllowNavigation = true;
		}

		/// <summary>Gets or sets a value indicating whether the control can navigate to another page after its initial page has been loaded.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can navigate to another page; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x0600496B RID: 18795 RVA: 0x00132632 File Offset: 0x00130832
		// (set) Token: 0x0600496C RID: 18796 RVA: 0x00132641 File Offset: 0x00130841
		[SRDescription("WebBrowserAllowNavigationDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowNavigation
		{
			get
			{
				return this.webBrowserState[64];
			}
			set
			{
				this.webBrowserState[64] = value;
				if (this.webBrowserEvent != null)
				{
					this.webBrowserEvent.AllowNavigation = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.WebBrowser" /> control navigates to documents that are dropped onto it.</summary>
		/// <returns>
		///     <see langword="true" /> if the control accepts documents that are dropped onto it; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x0600496D RID: 18797 RVA: 0x00132665 File Offset: 0x00130865
		// (set) Token: 0x0600496E RID: 18798 RVA: 0x00132672 File Offset: 0x00130872
		[SRDescription("WebBrowserAllowWebBrowserDropDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowWebBrowserDrop
		{
			get
			{
				return this.AxIWebBrowser2.RegisterAsDropTarget;
			}
			set
			{
				if (value != this.AllowWebBrowserDrop)
				{
					this.AxIWebBrowser2.RegisterAsDropTarget = value;
					this.Refresh();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.WebBrowser" /> displays dialog boxes such as script error messages.</summary>
		/// <returns>
		///     <see langword="true" /> if the control does not display its dialog boxes; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the IWebBrowser2 interface could not be retrieved from the underlying ActiveX WebBrowser control.</exception>
		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x0013268F File Offset: 0x0013088F
		// (set) Token: 0x06004970 RID: 18800 RVA: 0x0013269C File Offset: 0x0013089C
		[SRDescription("WebBrowserScriptErrorsSuppressedDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ScriptErrorsSuppressed
		{
			get
			{
				return this.AxIWebBrowser2.Silent;
			}
			set
			{
				if (value != this.ScriptErrorsSuppressed)
				{
					this.AxIWebBrowser2.Silent = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether keyboard shortcuts are enabled within the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if keyboard shortcuts are enabled within the control; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06004971 RID: 18801 RVA: 0x001326B3 File Offset: 0x001308B3
		// (set) Token: 0x06004972 RID: 18802 RVA: 0x001326C1 File Offset: 0x001308C1
		[SRDescription("WebBrowserWebBrowserShortcutsEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool WebBrowserShortcutsEnabled
		{
			get
			{
				return this.webBrowserState[1];
			}
			set
			{
				this.webBrowserState[1] = value;
			}
		}

		/// <summary>Gets a value indicating whether a previous page in navigation history is available, which allows the <see cref="M:System.Windows.Forms.WebBrowser.GoBack" /> method to succeed.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can navigate backward; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x001326D0 File Offset: 0x001308D0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanGoBack
		{
			get
			{
				return this.CanGoBackInternal;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06004974 RID: 18804 RVA: 0x001326D8 File Offset: 0x001308D8
		// (set) Token: 0x06004975 RID: 18805 RVA: 0x001326E6 File Offset: 0x001308E6
		internal bool CanGoBackInternal
		{
			get
			{
				return this.webBrowserState[8];
			}
			set
			{
				if (value != this.CanGoBackInternal)
				{
					this.webBrowserState[8] = value;
					this.OnCanGoBackChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets a value indicating whether a subsequent page in navigation history is available, which allows the <see cref="M:System.Windows.Forms.WebBrowser.GoForward" /> method to succeed.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can navigate forward; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x00132709 File Offset: 0x00130909
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanGoForward
		{
			get
			{
				return this.CanGoForwardInternal;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x00132711 File Offset: 0x00130911
		// (set) Token: 0x06004978 RID: 18808 RVA: 0x00132720 File Offset: 0x00130920
		internal bool CanGoForwardInternal
		{
			get
			{
				return this.webBrowserState[16];
			}
			set
			{
				if (value != this.CanGoForwardInternal)
				{
					this.webBrowserState[16] = value;
					this.OnCanGoForwardChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets an <see cref="T:System.Windows.Forms.HtmlDocument" /> representing the Web page currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.HtmlDocument" /> representing the current page, or <see langword="null" /> if no page is loaded.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x00132744 File Offset: 0x00130944
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HtmlDocument Document
		{
			get
			{
				object document = this.AxIWebBrowser2.Document;
				if (document != null)
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = null;
					try
					{
						ihtmldocument = (document as UnsafeNativeMethods.IHTMLDocument2);
					}
					catch (InvalidCastException)
					{
					}
					if (ihtmldocument != null)
					{
						UnsafeNativeMethods.IHTMLLocation location = ihtmldocument.GetLocation();
						if (location != null)
						{
							string href = location.GetHref();
							if (!string.IsNullOrEmpty(href))
							{
								Uri url = new Uri(href);
								WebBrowser.EnsureUrlConnectPermission(url);
								return new HtmlDocument(this.ShimManager, ihtmldocument as UnsafeNativeMethods.IHTMLDocument);
							}
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets a stream containing the contents of the Web page displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> containing the contents of the current Web page, or <see langword="null" /> if no page is loaded. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x001327BC File Offset: 0x001309BC
		// (set) Token: 0x0600497B RID: 18811 RVA: 0x00132818 File Offset: 0x00130A18
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Stream DocumentStream
		{
			get
			{
				HtmlDocument document = this.Document;
				if (document == null)
				{
					return null;
				}
				UnsafeNativeMethods.IPersistStreamInit persistStreamInit = document.DomDocument as UnsafeNativeMethods.IPersistStreamInit;
				if (persistStreamInit == null)
				{
					return null;
				}
				MemoryStream memoryStream = new MemoryStream();
				UnsafeNativeMethods.IStream pstm = new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream);
				persistStreamInit.Save(pstm, false);
				return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, false);
			}
			set
			{
				this.documentStreamToSetOnLoad = value;
				try
				{
					this.webBrowserState[2] = true;
					this.Url = new Uri("about:blank");
				}
				finally
				{
					this.webBrowserState[2] = false;
				}
			}
		}

		/// <summary>Gets or sets the HTML contents of the page displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>The HTML text of the displayed page, or the empty string ("") if no document is loaded.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x0600497C RID: 18812 RVA: 0x0013286C File Offset: 0x00130A6C
		// (set) Token: 0x0600497D RID: 18813 RVA: 0x001328A0 File Offset: 0x00130AA0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DocumentText
		{
			get
			{
				Stream documentStream = this.DocumentStream;
				if (documentStream == null)
				{
					return "";
				}
				StreamReader streamReader = new StreamReader(documentStream);
				documentStream.Position = 0L;
				return streamReader.ReadToEnd();
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				MemoryStream memoryStream = new MemoryStream(value.Length);
				StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
				streamWriter.Write(value);
				streamWriter.Flush();
				memoryStream.Position = 0L;
				this.DocumentStream = memoryStream;
			}
		}

		/// <summary>Gets the title of the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>The title of the current document, or the empty string ("") if no document is loaded.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x0600497E RID: 18814 RVA: 0x001328EC File Offset: 0x00130AEC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DocumentTitle
		{
			get
			{
				HtmlDocument document = this.Document;
				string result;
				if (document == null)
				{
					result = this.AxIWebBrowser2.LocationName;
				}
				else
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = document.DomDocument as UnsafeNativeMethods.IHTMLDocument2;
					try
					{
						result = ihtmldocument.GetTitle();
					}
					catch (COMException)
					{
						result = "";
					}
				}
				return result;
			}
		}

		/// <summary>Gets the type of the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>The type of the current document.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x0600497F RID: 18815 RVA: 0x00132948 File Offset: 0x00130B48
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DocumentType
		{
			get
			{
				string result = "";
				HtmlDocument document = this.Document;
				if (document != null)
				{
					UnsafeNativeMethods.IHTMLDocument2 ihtmldocument = document.DomDocument as UnsafeNativeMethods.IHTMLDocument2;
					try
					{
						result = ihtmldocument.GetMimeType();
					}
					catch (COMException)
					{
						result = "";
					}
				}
				return result;
			}
		}

		/// <summary>Gets a value indicating the encryption method used by the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.WebBrowserEncryptionLevel" /> values.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06004980 RID: 18816 RVA: 0x0013299C File Offset: 0x00130B9C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebBrowserEncryptionLevel EncryptionLevel
		{
			get
			{
				if (this.Document == null)
				{
					this.encryptionLevel = WebBrowserEncryptionLevel.Unknown;
				}
				return this.encryptionLevel;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.WebBrowser" /> control is currently loading a new document.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is busy loading a document; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06004981 RID: 18817 RVA: 0x001329B9 File Offset: 0x00130BB9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsBusy
		{
			get
			{
				return !(this.Document == null) && this.AxIWebBrowser2.Busy;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.WebBrowser" /> control is in offline mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.WebBrowser" /> control is in offline mode; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06004982 RID: 18818 RVA: 0x001329D6 File Offset: 0x00130BD6
		[SRDescription("WebBrowserIsOfflineDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOffline
		{
			get
			{
				return this.AxIWebBrowser2.Offline;
			}
		}

		/// <summary>Gets or a sets a value indicating whether the shortcut menu of the <see cref="T:System.Windows.Forms.WebBrowser" /> control is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.WebBrowser" /> control shortcut menu is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06004983 RID: 18819 RVA: 0x001329E3 File Offset: 0x00130BE3
		// (set) Token: 0x06004984 RID: 18820 RVA: 0x001329F1 File Offset: 0x00130BF1
		[SRDescription("WebBrowserIsWebBrowserContextMenuEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool IsWebBrowserContextMenuEnabled
		{
			get
			{
				return this.webBrowserState[4];
			}
			set
			{
				this.webBrowserState[4] = value;
			}
		}

		/// <summary>Gets or sets an object that can be accessed by scripting code that is contained within a Web page displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>The object being made available to the scripting code.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is an instance of a non-public type.-or-The specified value when setting this property is an instance of a type that is not COM-visible. For more information, see <see cref="M:System.Runtime.InteropServices.Marshal.IsTypeVisibleFromCom(System.Type)" />.</exception>
		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x00132A00 File Offset: 0x00130C00
		// (set) Token: 0x06004986 RID: 18822 RVA: 0x00132A08 File Offset: 0x00130C08
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ObjectForScripting
		{
			get
			{
				return this.objectForScripting;
			}
			set
			{
				if (value != null)
				{
					Type type = value.GetType();
					if (!Marshal.IsTypeVisibleFromCom(type))
					{
						throw new ArgumentException(SR.GetString("WebBrowserObjectForScriptingComVisibleOnly"));
					}
				}
				this.objectForScripting = value;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" />
		///   </returns>
		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06004987 RID: 18823 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06004988 RID: 18824 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.WebBrowser.Padding" /> property changes.</summary>
		// Token: 0x140003B8 RID: 952
		// (add) Token: 0x06004989 RID: 18825 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x0600498A RID: 18826 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnPaddingChangedDescr")]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		/// <summary>Gets a value indicating the current state of the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.WebBrowserReadyState" /> values.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x0600498B RID: 18827 RVA: 0x00132A3E File Offset: 0x00130C3E
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebBrowserReadyState ReadyState
		{
			get
			{
				if (this.Document == null)
				{
					return WebBrowserReadyState.Uninitialized;
				}
				return this.AxIWebBrowser2.ReadyState;
			}
		}

		/// <summary>Gets the status text of the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>The status text.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x00132A5B File Offset: 0x00130C5B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string StatusText
		{
			get
			{
				if (this.Document == null)
				{
					this.statusText = "";
				}
				return this.statusText;
			}
		}

		/// <summary>Gets or sets the URL of the current document.</summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the URL of the current document.</returns>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is not an absolute URI. For more information, see <see cref="P:System.Uri.IsAbsoluteUri" />.</exception>
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x00132A7C File Offset: 0x00130C7C
		// (set) Token: 0x0600498E RID: 18830 RVA: 0x00132AC0 File Offset: 0x00130CC0
		[SRDescription("WebBrowserUrlDescr")]
		[Bindable(true)]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(WebBrowserUriTypeConverter))]
		[DefaultValue(null)]
		public Uri Url
		{
			get
			{
				string locationURL = this.AxIWebBrowser2.LocationURL;
				if (string.IsNullOrEmpty(locationURL))
				{
					return null;
				}
				Uri result;
				try
				{
					result = new Uri(locationURL);
				}
				catch (UriFormatException)
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null && value.ToString() == "")
				{
					value = null;
				}
				this.PerformNavigateHelper(this.ReadyNavigateToUrl(value), false, null, null, null);
			}
		}

		/// <summary>Gets the version of Internet Explorer installed.</summary>
		/// <returns>A <see cref="T:System.Version" /> object representing the version of Internet Explorer installed.</returns>
		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x00132AF4 File Offset: 0x00130CF4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Version Version
		{
			get
			{
				string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "mshtml.dll");
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
				return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
			}
		}

		/// <summary>Navigates the <see cref="T:System.Windows.Forms.WebBrowser" /> control to the previous page in the navigation history, if one is available.</summary>
		/// <returns>
		///     <see langword="true" /> if the navigation succeeds; <see langword="false" /> if a previous page in the navigation history is not available.</returns>
		// Token: 0x06004990 RID: 18832 RVA: 0x00132B38 File Offset: 0x00130D38
		public bool GoBack()
		{
			bool result = true;
			try
			{
				this.AxIWebBrowser2.GoBack();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		/// <summary>Navigates the <see cref="T:System.Windows.Forms.WebBrowser" /> control to the next page in the navigation history, if one is available.</summary>
		/// <returns>
		///     <see langword="true" /> if the navigation succeeds; <see langword="false" /> if a subsequent page in the navigation history is not available.</returns>
		// Token: 0x06004991 RID: 18833 RVA: 0x00132B74 File Offset: 0x00130D74
		public bool GoForward()
		{
			bool result = true;
			try
			{
				this.AxIWebBrowser2.GoForward();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		/// <summary>Navigates the <see cref="T:System.Windows.Forms.WebBrowser" /> control to the home page of the current user.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x06004992 RID: 18834 RVA: 0x00132BB0 File Offset: 0x00130DB0
		public void GoHome()
		{
			this.AxIWebBrowser2.GoHome();
		}

		/// <summary>Navigates the <see cref="T:System.Windows.Forms.WebBrowser" /> control to the default search page of the current user.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x06004993 RID: 18835 RVA: 0x00132BBD File Offset: 0x00130DBD
		public void GoSearch()
		{
			this.AxIWebBrowser2.GoSearch();
		}

		/// <summary>Loads the document at the location indicated by the specified <see cref="T:System.Uri" /> into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, replacing the previous document.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the URL of the document to load. </param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="url" /> parameter value does not represent an absolute URI. For more information, see <see cref="P:System.Uri.IsAbsoluteUri" />.</exception>
		// Token: 0x06004994 RID: 18836 RVA: 0x00132BCA File Offset: 0x00130DCA
		public void Navigate(Uri url)
		{
			this.Url = url;
		}

		/// <summary>Loads the document at the specified Uniform Resource Locator (URL) into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, replacing the previous document.</summary>
		/// <param name="urlString">The URL of the document to load.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x06004995 RID: 18837 RVA: 0x00132BD3 File Offset: 0x00130DD3
		public void Navigate(string urlString)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, null, null, null);
		}

		/// <summary>Loads the document at the location indicated by the specified <see cref="T:System.Uri" /> into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, replacing the contents of the Web page frame with the specified name.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the URL of the document to load.</param>
		/// <param name="targetFrameName">The name of the frame in which to load the document. </param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="url" /> parameter value does not represent an absolute URI. For more information, see <see cref="P:System.Uri.IsAbsoluteUri" />.</exception>
		// Token: 0x06004996 RID: 18838 RVA: 0x00132BE6 File Offset: 0x00130DE6
		public void Navigate(Uri url, string targetFrameName)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), false, targetFrameName, null, null);
		}

		/// <summary>Loads the document at the specified Uniform Resource Locator (URL) into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, replacing the contents of the Web page frame with the specified name.</summary>
		/// <param name="urlString">The URL of the document to load.</param>
		/// <param name="targetFrameName">The name of the frame in which to load the document.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x06004997 RID: 18839 RVA: 0x00132BF9 File Offset: 0x00130DF9
		public void Navigate(string urlString, string targetFrameName)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, targetFrameName, null, null);
		}

		/// <summary>Loads the document at the location indicated by the specified <see cref="T:System.Uri" /> into a new browser window or into the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the URL of the document to load.</param>
		/// <param name="newWindow">
		///       <see langword="true" /> to load the document into a new browser window; <see langword="false" /> to load the document into the <see cref="T:System.Windows.Forms.WebBrowser" /> control. </param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="url" /> parameter value does not represent an absolute URI. For more information, see <see cref="P:System.Uri.IsAbsoluteUri" />.</exception>
		// Token: 0x06004998 RID: 18840 RVA: 0x00132C0C File Offset: 0x00130E0C
		public void Navigate(Uri url, bool newWindow)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), newWindow, null, null, null);
		}

		/// <summary>Loads the document at the specified Uniform Resource Locator (URL) into a new browser window or into the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <param name="urlString">The URL of the document to load.</param>
		/// <param name="newWindow">
		///       <see langword="true" /> to load the document into a new browser window; <see langword="false" /> to load the document into the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x06004999 RID: 18841 RVA: 0x00132C1F File Offset: 0x00130E1F
		public void Navigate(string urlString, bool newWindow)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), newWindow, null, null, null);
		}

		/// <summary>Loads the document at the location indicated by the specified <see cref="T:System.Uri" /> into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, requesting it using the specified HTTP data and replacing the contents of the Web page frame with the specified name.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the URL of the document to load.</param>
		/// <param name="targetFrameName">The name of the frame in which to load the document.</param>
		/// <param name="postData">HTTP POST data such as form data.</param>
		/// <param name="additionalHeaders">HTTP headers to add to the default headers.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="url" /> parameter value does not represent an absolute URI. For more information, see <see cref="P:System.Uri.IsAbsoluteUri" />.</exception>
		// Token: 0x0600499A RID: 18842 RVA: 0x00132C32 File Offset: 0x00130E32
		public void Navigate(Uri url, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(url), false, targetFrameName, postData, additionalHeaders);
		}

		/// <summary>Loads the document at the specified Uniform Resource Locator (URL) into the <see cref="T:System.Windows.Forms.WebBrowser" /> control, requesting it using the specified HTTP data and replacing the contents of the Web page frame with the specified name.</summary>
		/// <param name="urlString">The URL of the document to load.</param>
		/// <param name="targetFrameName">The name of the frame in which to load the document.</param>
		/// <param name="postData">HTTP POST data such as form data.</param>
		/// <param name="additionalHeaders">HTTP headers to add to the default headers.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x0600499B RID: 18843 RVA: 0x00132C46 File Offset: 0x00130E46
		public void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			this.PerformNavigateHelper(this.ReadyNavigateToUrl(urlString), false, targetFrameName, postData, additionalHeaders);
		}

		/// <summary>Prints the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control using the current print and page settings.</summary>
		// Token: 0x0600499C RID: 18844 RVA: 0x00132C5C File Offset: 0x00130E5C
		public void Print()
		{
			IntSecurity.DefaultPrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINT, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Reloads the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control by checking the server for an updated version.</summary>
		// Token: 0x0600499D RID: 18845 RVA: 0x00132CA8 File Offset: 0x00130EA8
		public override void Refresh()
		{
			try
			{
				if (this.ShouldSerializeDocumentText())
				{
					string documentText = this.DocumentText;
					this.AxIWebBrowser2.Refresh();
					this.DocumentText = documentText;
				}
				else
				{
					this.AxIWebBrowser2.Refresh();
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Reloads the document currently displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control using the specified refresh options.</summary>
		/// <param name="opt">One of the <see cref="T:System.Windows.Forms.WebBrowserRefreshOption" /> values. </param>
		// Token: 0x0600499E RID: 18846 RVA: 0x00132D04 File Offset: 0x00130F04
		public void Refresh(WebBrowserRefreshOption opt)
		{
			object obj = opt;
			try
			{
				if (this.ShouldSerializeDocumentText())
				{
					string documentText = this.DocumentText;
					this.AxIWebBrowser2.Refresh2(ref obj);
					this.DocumentText = documentText;
				}
				else
				{
					this.AxIWebBrowser2.Refresh2(ref obj);
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether scroll bars are displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if scroll bars are displayed in the control; otherwise, <see langword="false" />. The default is true.</returns>
		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x0600499F RID: 18847 RVA: 0x00132D6C File Offset: 0x00130F6C
		// (set) Token: 0x060049A0 RID: 18848 RVA: 0x00132D7B File Offset: 0x00130F7B
		[SRDescription("WebBrowserScrollBarsEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ScrollBarsEnabled
		{
			get
			{
				return this.webBrowserState[32];
			}
			set
			{
				if (value != this.webBrowserState[32])
				{
					this.webBrowserState[32] = value;
					this.Refresh();
				}
			}
		}

		/// <summary>Opens the Internet Explorer Page Setup dialog box.</summary>
		// Token: 0x060049A1 RID: 18849 RVA: 0x00132DA4 File Offset: 0x00130FA4
		public void ShowPageSetupDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PAGESETUP, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Opens the Internet Explorer Print dialog box without setting header and footer values.</summary>
		// Token: 0x060049A2 RID: 18850 RVA: 0x00132DF0 File Offset: 0x00130FF0
		public void ShowPrintDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINT, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Opens the Internet Explorer Print Preview dialog box.</summary>
		// Token: 0x060049A3 RID: 18851 RVA: 0x00132E3C File Offset: 0x0013103C
		public void ShowPrintPreviewDialog()
		{
			IntSecurity.SafePrinting.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PRINTPREVIEW, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Opens the Internet Explorer Properties dialog box for the current document.</summary>
		// Token: 0x060049A4 RID: 18852 RVA: 0x00132E88 File Offset: 0x00131088
		public void ShowPropertiesDialog()
		{
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_PROPERTIES, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Opens the Internet Explorer Save Web Page dialog box or the Save dialog box of the hosted document if it is not an HTML page.</summary>
		// Token: 0x060049A5 RID: 18853 RVA: 0x00132ECC File Offset: 0x001310CC
		public void ShowSaveAsDialog()
		{
			IntSecurity.FileDialogSaveFile.Demand();
			object obj = null;
			try
			{
				this.AxIWebBrowser2.ExecWB(NativeMethods.OLECMDID.OLECMDID_SAVEAS, NativeMethods.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref obj, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Cancels any pending navigation and stops any dynamic page elements, such as background sounds and animations.</summary>
		// Token: 0x060049A6 RID: 18854 RVA: 0x00132F18 File Offset: 0x00131118
		public void Stop()
		{
			try
			{
				this.AxIWebBrowser2.Stop();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.WebBrowser.CanGoBack" /> property value changes.</summary>
		// Token: 0x140003B9 RID: 953
		// (add) Token: 0x060049A7 RID: 18855 RVA: 0x00132F50 File Offset: 0x00131150
		// (remove) Token: 0x060049A8 RID: 18856 RVA: 0x00132F88 File Offset: 0x00131188
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserCanGoBackChangedDescr")]
		public event EventHandler CanGoBackChanged;

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.WebBrowser.CanGoForward" /> property value changes.</summary>
		// Token: 0x140003BA RID: 954
		// (add) Token: 0x060049A9 RID: 18857 RVA: 0x00132FC0 File Offset: 0x001311C0
		// (remove) Token: 0x060049AA RID: 18858 RVA: 0x00132FF8 File Offset: 0x001311F8
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserCanGoForwardChangedDescr")]
		public event EventHandler CanGoForwardChanged;

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.WebBrowser" /> control finishes loading a document.</summary>
		// Token: 0x140003BB RID: 955
		// (add) Token: 0x060049AB RID: 18859 RVA: 0x00133030 File Offset: 0x00131230
		// (remove) Token: 0x060049AC RID: 18860 RVA: 0x00133068 File Offset: 0x00131268
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserDocumentCompletedDescr")]
		public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.WebBrowser.DocumentTitle" /> property value changes.</summary>
		// Token: 0x140003BC RID: 956
		// (add) Token: 0x060049AD RID: 18861 RVA: 0x001330A0 File Offset: 0x001312A0
		// (remove) Token: 0x060049AE RID: 18862 RVA: 0x001330D8 File Offset: 0x001312D8
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserDocumentTitleChangedDescr")]
		public event EventHandler DocumentTitleChanged;

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.WebBrowser" /> control navigates to or away from a Web site that uses encryption.</summary>
		// Token: 0x140003BD RID: 957
		// (add) Token: 0x060049AF RID: 18863 RVA: 0x00133110 File Offset: 0x00131310
		// (remove) Token: 0x060049B0 RID: 18864 RVA: 0x00133148 File Offset: 0x00131348
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserEncryptionLevelChangedDescr")]
		public event EventHandler EncryptionLevelChanged;

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.WebBrowser" /> control downloads a file.</summary>
		// Token: 0x140003BE RID: 958
		// (add) Token: 0x060049B1 RID: 18865 RVA: 0x00133180 File Offset: 0x00131380
		// (remove) Token: 0x060049B2 RID: 18866 RVA: 0x001331B8 File Offset: 0x001313B8
		[SRCategory("CatBehavior")]
		[SRDescription("WebBrowserFileDownloadDescr")]
		public event EventHandler FileDownload;

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.WebBrowser" /> control has navigated to a new document and has begun loading it.</summary>
		// Token: 0x140003BF RID: 959
		// (add) Token: 0x060049B3 RID: 18867 RVA: 0x001331F0 File Offset: 0x001313F0
		// (remove) Token: 0x060049B4 RID: 18868 RVA: 0x00133228 File Offset: 0x00131428
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserNavigatedDescr")]
		public event WebBrowserNavigatedEventHandler Navigated;

		/// <summary>Occurs before the <see cref="T:System.Windows.Forms.WebBrowser" /> control navigates to a new document.</summary>
		// Token: 0x140003C0 RID: 960
		// (add) Token: 0x060049B5 RID: 18869 RVA: 0x00133260 File Offset: 0x00131460
		// (remove) Token: 0x060049B6 RID: 18870 RVA: 0x00133298 File Offset: 0x00131498
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserNavigatingDescr")]
		public event WebBrowserNavigatingEventHandler Navigating;

		/// <summary>Occurs before a new browser window is opened.</summary>
		// Token: 0x140003C1 RID: 961
		// (add) Token: 0x060049B7 RID: 18871 RVA: 0x001332D0 File Offset: 0x001314D0
		// (remove) Token: 0x060049B8 RID: 18872 RVA: 0x00133308 File Offset: 0x00131508
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserNewWindowDescr")]
		public event CancelEventHandler NewWindow;

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.WebBrowser" /> control has updated information on the download progress of a document it is navigating to.</summary>
		// Token: 0x140003C2 RID: 962
		// (add) Token: 0x060049B9 RID: 18873 RVA: 0x00133340 File Offset: 0x00131540
		// (remove) Token: 0x060049BA RID: 18874 RVA: 0x00133378 File Offset: 0x00131578
		[SRCategory("CatAction")]
		[SRDescription("WebBrowserProgressChangedDescr")]
		public event WebBrowserProgressChangedEventHandler ProgressChanged;

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.WebBrowser.StatusText" /> property value changes.</summary>
		// Token: 0x140003C3 RID: 963
		// (add) Token: 0x060049BB RID: 18875 RVA: 0x001333B0 File Offset: 0x001315B0
		// (remove) Token: 0x060049BC RID: 18876 RVA: 0x001333E8 File Offset: 0x001315E8
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("WebBrowserStatusTextChangedDescr")]
		public event EventHandler StatusTextChanged;

		/// <summary>Gets a value indicating whether the control or any of its child windows has input focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control or any of its child windows has input focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x00133420 File Offset: 0x00131620
		public override bool Focused
		{
			get
			{
				if (base.Focused)
				{
					return true;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return focus != IntPtr.Zero && SafeNativeMethods.IsChild(new HandleRef(this, base.Handle), new HandleRef(null, focus));
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.WebBrowser" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060049BE RID: 18878 RVA: 0x00133464 File Offset: 0x00131664
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.htmlShimManager != null)
				{
					this.htmlShimManager.Dispose();
				}
				this.DetachSink();
				base.ActiveXSite.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>Gets the default size of the control.</returns>
		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x00133494 File Offset: 0x00131694
		protected override Size DefaultSize
		{
			get
			{
				return new Size(250, 250);
			}
		}

		/// <summary>Called by the control when the underlying ActiveX control is created.</summary>
		/// <param name="nativeActiveXObject">An object that represents the underlying ActiveX control.</param>
		// Token: 0x060049C0 RID: 18880 RVA: 0x001334A5 File Offset: 0x001316A5
		protected override void AttachInterfaces(object nativeActiveXObject)
		{
			this.axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
		}

		/// <summary>Called by the control when the underlying ActiveX control is discarded.</summary>
		// Token: 0x060049C1 RID: 18881 RVA: 0x001334B3 File Offset: 0x001316B3
		protected override void DetachInterfaces()
		{
			this.axIWebBrowser2 = null;
		}

		/// <summary>Returns a reference to the unmanaged <see langword="WebBrowser" /> ActiveX control site, which you can extend to customize the managed <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.WebBrowser.WebBrowserSite" /> that represents the <see langword="WebBrowser" /> ActiveX control site.</returns>
		// Token: 0x060049C2 RID: 18882 RVA: 0x001334BC File Offset: 0x001316BC
		protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return new WebBrowser.WebBrowserSite(this);
		}

		/// <summary>Associates the underlying ActiveX control with a client that can handle control events.</summary>
		// Token: 0x060049C3 RID: 18883 RVA: 0x001334C4 File Offset: 0x001316C4
		protected override void CreateSink()
		{
			object activeXInstance = this.activeXInstance;
			if (activeXInstance != null)
			{
				this.webBrowserEvent = new WebBrowser.WebBrowserEvent(this);
				this.webBrowserEvent.AllowNavigation = this.AllowNavigation;
				this.cookie = new AxHost.ConnectionPointCookie(activeXInstance, this.webBrowserEvent, typeof(UnsafeNativeMethods.DWebBrowserEvents2));
			}
		}

		/// <summary>Releases the event-handling client attached in the <see cref="M:System.Windows.Forms.WebBrowser.CreateSink" /> method from the underlying ActiveX control.</summary>
		// Token: 0x060049C4 RID: 18884 RVA: 0x00133514 File Offset: 0x00131714
		protected override void DetachSink()
		{
			if (this.cookie != null)
			{
				this.cookie.Disconnect();
				this.cookie = null;
			}
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x00133530 File Offset: 0x00131730
		internal override void OnTopMostActiveXParentChanged(EventArgs e)
		{
			if (base.TopMostParent.IsIEParent)
			{
				WebBrowser.createdInIE = true;
				this.CheckIfCreatedInIE();
				return;
			}
			WebBrowser.createdInIE = false;
			base.OnTopMostActiveXParentChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.CanGoBackChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060049C6 RID: 18886 RVA: 0x00133559 File Offset: 0x00131759
		protected virtual void OnCanGoBackChanged(EventArgs e)
		{
			if (this.CanGoBackChanged != null)
			{
				this.CanGoBackChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.CanGoForwardChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060049C7 RID: 18887 RVA: 0x00133570 File Offset: 0x00131770
		protected virtual void OnCanGoForwardChanged(EventArgs e)
		{
			if (this.CanGoForwardChanged != null)
			{
				this.CanGoForwardChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.DocumentCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.WebBrowserDocumentCompletedEventArgs" /> that contains the event data. </param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">A reference to an implementation of the <see langword="IWebBrowser2" /> interface could not be retrieved from the underlying ActiveX <see langword="WebBrowser" /> control.</exception>
		// Token: 0x060049C8 RID: 18888 RVA: 0x00133587 File Offset: 0x00131787
		protected virtual void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
		{
			this.AxIWebBrowser2.RegisterAsDropTarget = this.AllowWebBrowserDrop;
			if (this.DocumentCompleted != null)
			{
				this.DocumentCompleted(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.DocumentTitleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060049C9 RID: 18889 RVA: 0x001335AF File Offset: 0x001317AF
		protected virtual void OnDocumentTitleChanged(EventArgs e)
		{
			if (this.DocumentTitleChanged != null)
			{
				this.DocumentTitleChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.EncryptionLevelChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060049CA RID: 18890 RVA: 0x001335C6 File Offset: 0x001317C6
		protected virtual void OnEncryptionLevelChanged(EventArgs e)
		{
			if (this.EncryptionLevelChanged != null)
			{
				this.EncryptionLevelChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.FileDownload" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x060049CB RID: 18891 RVA: 0x001335DD File Offset: 0x001317DD
		protected virtual void OnFileDownload(EventArgs e)
		{
			if (this.FileDownload != null)
			{
				this.FileDownload(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.Navigated" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.WebBrowserNavigatedEventArgs" /> that contains the event data. </param>
		// Token: 0x060049CC RID: 18892 RVA: 0x001335F4 File Offset: 0x001317F4
		protected virtual void OnNavigated(WebBrowserNavigatedEventArgs e)
		{
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.Navigating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.WebBrowserNavigatingEventArgs" /> that contains the event data. </param>
		// Token: 0x060049CD RID: 18893 RVA: 0x0013360B File Offset: 0x0013180B
		protected virtual void OnNavigating(WebBrowserNavigatingEventArgs e)
		{
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.NewWindow" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x060049CE RID: 18894 RVA: 0x00133622 File Offset: 0x00131822
		protected virtual void OnNewWindow(CancelEventArgs e)
		{
			if (this.NewWindow != null)
			{
				this.NewWindow(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.ProgressChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.WebBrowserProgressChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x060049CF RID: 18895 RVA: 0x00133639 File Offset: 0x00131839
		protected virtual void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
		{
			if (this.ProgressChanged != null)
			{
				this.ProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.WebBrowser.StatusTextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060049D0 RID: 18896 RVA: 0x00133650 File Offset: 0x00131850
		protected virtual void OnStatusTextChanged(EventArgs e)
		{
			if (this.StatusTextChanged != null)
			{
				this.StatusTextChanged(this, e);
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x060049D1 RID: 18897 RVA: 0x00133667 File Offset: 0x00131867
		internal HtmlShimManager ShimManager
		{
			get
			{
				if (this.htmlShimManager == null)
				{
					this.htmlShimManager = new HtmlShimManager();
				}
				return this.htmlShimManager;
			}
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00133682 File Offset: 0x00131882
		private void CheckIfCreatedInIE()
		{
			if (!WebBrowser.createdInIE)
			{
				return;
			}
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Controls.Remove(this);
				base.Dispose();
				return;
			}
			base.Dispose();
			throw new NotSupportedException(SR.GetString("WebBrowserInIENotSupported"));
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x001336C4 File Offset: 0x001318C4
		internal static void EnsureUrlConnectPermission(Uri url)
		{
			WebPermission webPermission = new WebPermission(NetworkAccess.Connect, url.AbsoluteUri);
			webPermission.Demand();
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001336E5 File Offset: 0x001318E5
		private string ReadyNavigateToUrl(string urlString)
		{
			if (string.IsNullOrEmpty(urlString))
			{
				urlString = "about:blank";
			}
			if (!this.webBrowserState[2])
			{
				this.documentStreamToSetOnLoad = null;
			}
			return urlString;
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0013370C File Offset: 0x0013190C
		private string ReadyNavigateToUrl(Uri url)
		{
			string urlString;
			if (url == null)
			{
				urlString = "about:blank";
			}
			else
			{
				if (!url.IsAbsoluteUri)
				{
					throw new ArgumentException(SR.GetString("WebBrowserNavigateAbsoluteUri", new object[]
					{
						"uri"
					}));
				}
				urlString = (url.IsFile ? url.OriginalString : url.AbsoluteUri);
			}
			return this.ReadyNavigateToUrl(urlString);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00133770 File Offset: 0x00131970
		private void PerformNavigateHelper(string urlString, bool newWindow, string targetFrameName, byte[] postData, string headers)
		{
			object obj = urlString;
			object obj2 = newWindow ? 1 : 0;
			object obj3 = targetFrameName;
			object obj4 = postData;
			object obj5 = headers;
			this.PerformNavigate2(ref obj, ref obj2, ref obj3, ref obj4, ref obj5);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x001337A8 File Offset: 0x001319A8
		private void PerformNavigate2(ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers)
		{
			try
			{
				this.AxIWebBrowser2.Navigate2(ref URL, ref flags, ref targetFrameName, ref postData, ref headers);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147023673)
				{
					throw;
				}
			}
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x001337EC File Offset: 0x001319EC
		private bool ShouldSerializeDocumentText()
		{
			return this.IsValidUrl;
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x001337F4 File Offset: 0x001319F4
		private bool IsValidUrl
		{
			get
			{
				return this.Url == null || this.Url.AbsoluteUri == "about:blank";
			}
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x0013381B File Offset: 0x00131A1B
		private bool ShouldSerializeUrl()
		{
			return !this.ShouldSerializeDocumentText();
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00133828 File Offset: 0x00131A28
		private bool ShowContextMenu(int x, int y)
		{
			ContextMenuStrip contextMenuStrip = this.ContextMenuStrip;
			ContextMenu contextMenu = (contextMenuStrip != null) ? null : this.ContextMenu;
			if (contextMenuStrip == null && contextMenu == null)
			{
				return false;
			}
			bool isKeyboardActivated = false;
			Point point;
			if (x == -1)
			{
				isKeyboardActivated = true;
				point = new Point(base.Width / 2, base.Height / 2);
			}
			else
			{
				point = base.PointToClientInternal(new Point(x, y));
			}
			if (base.ClientRectangle.Contains(point))
			{
				if (contextMenuStrip != null)
				{
					contextMenuStrip.ShowInternal(this, point, isKeyboardActivated);
				}
				else if (contextMenu != null)
				{
					contextMenu.Show(this, point);
				}
				return true;
			}
			return false;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x060049DC RID: 18908 RVA: 0x001338B0 File Offset: 0x00131AB0
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 123)
			{
				int x = NativeMethods.Util.SignedLOWORD(m.LParam);
				int y = NativeMethods.Util.SignedHIWORD(m.LParam);
				if (!this.ShowContextMenu(x, y))
				{
					this.DefWndProc(ref m);
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x001338FC File Offset: 0x00131AFC
		private UnsafeNativeMethods.IWebBrowser2 AxIWebBrowser2
		{
			get
			{
				if (this.axIWebBrowser2 == null)
				{
					if (base.IsDisposed)
					{
						throw new ObjectDisposedException(base.GetType().Name);
					}
					base.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
				}
				if (this.axIWebBrowser2 == null)
				{
					throw new InvalidOperationException(SR.GetString("WebBrowserNoCastToIWebBrowser2"));
				}
				return this.axIWebBrowser2;
			}
		}

		// Token: 0x040026ED RID: 9965
		private static bool createdInIE;

		// Token: 0x040026EE RID: 9966
		private UnsafeNativeMethods.IWebBrowser2 axIWebBrowser2;

		// Token: 0x040026EF RID: 9967
		private AxHost.ConnectionPointCookie cookie;

		// Token: 0x040026F0 RID: 9968
		private Stream documentStreamToSetOnLoad;

		// Token: 0x040026F1 RID: 9969
		private WebBrowserEncryptionLevel encryptionLevel;

		// Token: 0x040026F2 RID: 9970
		private object objectForScripting;

		// Token: 0x040026F3 RID: 9971
		private WebBrowser.WebBrowserEvent webBrowserEvent;

		// Token: 0x040026F4 RID: 9972
		internal string statusText = "";

		// Token: 0x040026F5 RID: 9973
		private const int WEBBROWSERSTATE_webBrowserShortcutsEnabled = 1;

		// Token: 0x040026F6 RID: 9974
		private const int WEBBROWSERSTATE_documentStreamJustSet = 2;

		// Token: 0x040026F7 RID: 9975
		private const int WEBBROWSERSTATE_isWebBrowserContextMenuEnabled = 4;

		// Token: 0x040026F8 RID: 9976
		private const int WEBBROWSERSTATE_canGoBack = 8;

		// Token: 0x040026F9 RID: 9977
		private const int WEBBROWSERSTATE_canGoForward = 16;

		// Token: 0x040026FA RID: 9978
		private const int WEBBROWSERSTATE_scrollbarsEnabled = 32;

		// Token: 0x040026FB RID: 9979
		private const int WEBBROWSERSTATE_allowNavigation = 64;

		// Token: 0x040026FC RID: 9980
		private BitVector32 webBrowserState;

		// Token: 0x04002708 RID: 9992
		private HtmlShimManager htmlShimManager;

		/// <summary>Represents the host window of a <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
		// Token: 0x020007FD RID: 2045
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected class WebBrowserSite : WebBrowserSiteBase, UnsafeNativeMethods.IDocHostUIHandler
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowser.WebBrowserSite" /> class. </summary>
			/// <param name="host">The <see cref="T:System.Windows.Forms.WebBrowser" /></param>
			// Token: 0x06006DFD RID: 28157 RVA: 0x001931A3 File Offset: 0x001913A3
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public WebBrowserSite(WebBrowser host) : base(host)
			{
			}

			// Token: 0x06006DFE RID: 28158 RVA: 0x001931AC File Offset: 0x001913AC
			int UnsafeNativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				if (webBrowser.IsWebBrowserContextMenuEnabled)
				{
					return 1;
				}
				if (pt.x == 0 && pt.y == 0)
				{
					pt.x = -1;
					pt.y = -1;
				}
				webBrowser.ShowContextMenu(pt.x, pt.y);
				return 0;
			}

			// Token: 0x06006DFF RID: 28159 RVA: 0x00193204 File Offset: 0x00191404
			int UnsafeNativeMethods.IDocHostUIHandler.GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				info.dwDoubleClick = 0;
				info.dwFlags = 2097168;
				if (webBrowser.ScrollBarsEnabled)
				{
					info.dwFlags |= 128;
				}
				else
				{
					info.dwFlags |= 8;
				}
				if (Application.RenderWithVisualStyles)
				{
					info.dwFlags |= 262144;
				}
				else
				{
					info.dwFlags |= 524288;
				}
				return 0;
			}

			// Token: 0x06006E00 RID: 28160 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
			{
				return -2147467263;
			}

			// Token: 0x06006E01 RID: 28161 RVA: 0x0000E214 File Offset: 0x0000C414
			int UnsafeNativeMethods.IDocHostUIHandler.ShowUI(int dwID, UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, UnsafeNativeMethods.IOleInPlaceFrame frame, UnsafeNativeMethods.IOleInPlaceUIWindow doc)
			{
				return 1;
			}

			// Token: 0x06006E02 RID: 28162 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.HideUI()
			{
				return -2147467263;
			}

			// Token: 0x06006E03 RID: 28163 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.UpdateUI()
			{
				return -2147467263;
			}

			// Token: 0x06006E04 RID: 28164 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
			{
				return -2147467263;
			}

			// Token: 0x06006E05 RID: 28165 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
			{
				return -2147467263;
			}

			// Token: 0x06006E06 RID: 28166 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.ResizeBorder(NativeMethods.COMRECT rect, UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
			{
				return -2147467263;
			}

			// Token: 0x06006E07 RID: 28167 RVA: 0x00033B0C File Offset: 0x00031D0C
			int UnsafeNativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
			{
				return -2147467263;
			}

			// Token: 0x06006E08 RID: 28168 RVA: 0x0000E2A3 File Offset: 0x0000C4A3
			int UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget(UnsafeNativeMethods.IOleDropTarget pDropTarget, out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
			{
				ppDropTarget = null;
				return -2147467263;
			}

			// Token: 0x06006E09 RID: 28169 RVA: 0x00193288 File Offset: 0x00191488
			int UnsafeNativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				ppDispatch = webBrowser.ObjectForScripting;
				return 0;
			}

			// Token: 0x06006E0A RID: 28170 RVA: 0x001932AC File Offset: 0x001914AC
			int UnsafeNativeMethods.IDocHostUIHandler.TranslateAccelerator(ref NativeMethods.MSG msg, ref Guid group, int nCmdID)
			{
				WebBrowser webBrowser = (WebBrowser)base.Host;
				if (webBrowser.WebBrowserShortcutsEnabled)
				{
					return 1;
				}
				int num = (int)msg.wParam | (int)Control.ModifierKeys;
				if (msg.message != 258 && Enum.IsDefined(typeof(Shortcut), (Shortcut)num))
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06006E0B RID: 28171 RVA: 0x00193308 File Offset: 0x00191508
			int UnsafeNativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
			{
				pstrUrlOut = null;
				return 1;
			}

			// Token: 0x06006E0C RID: 28172 RVA: 0x0019330E File Offset: 0x0019150E
			int UnsafeNativeMethods.IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
			{
				ppDORet = null;
				return 1;
			}

			// Token: 0x06006E0D RID: 28173 RVA: 0x00193314 File Offset: 0x00191514
			internal override void OnPropertyChanged(int dispid)
			{
				if (dispid != -525)
				{
					base.OnPropertyChanged(dispid);
				}
			}
		}

		// Token: 0x020007FE RID: 2046
		[ClassInterface(ClassInterfaceType.None)]
		private class WebBrowserEvent : StandardOleMarshalObject, UnsafeNativeMethods.DWebBrowserEvents2
		{
			// Token: 0x06006E0E RID: 28174 RVA: 0x00193325 File Offset: 0x00191525
			public WebBrowserEvent(WebBrowser parent)
			{
				this.parent = parent;
			}

			// Token: 0x170017CF RID: 6095
			// (get) Token: 0x06006E0F RID: 28175 RVA: 0x00193334 File Offset: 0x00191534
			// (set) Token: 0x06006E10 RID: 28176 RVA: 0x0019333C File Offset: 0x0019153C
			public bool AllowNavigation
			{
				get
				{
					return this.allowNavigation;
				}
				set
				{
					this.allowNavigation = value;
				}
			}

			// Token: 0x06006E11 RID: 28177 RVA: 0x00193345 File Offset: 0x00191545
			public void CommandStateChange(long command, bool enable)
			{
				if (command == 2L)
				{
					this.parent.CanGoBackInternal = enable;
					return;
				}
				if (command == 1L)
				{
					this.parent.CanGoForwardInternal = enable;
				}
			}

			// Token: 0x06006E12 RID: 28178 RVA: 0x0019336C File Offset: 0x0019156C
			public void BeforeNavigate2(object pDisp, ref object urlObject, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
			{
				if (this.AllowNavigation || !this.haveNavigated)
				{
					if (targetFrameName == null)
					{
						targetFrameName = "";
					}
					if (headers == null)
					{
						headers = "";
					}
					string uriString = (urlObject == null) ? "" : ((string)urlObject);
					WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs = new WebBrowserNavigatingEventArgs(new Uri(uriString), (targetFrameName == null) ? "" : ((string)targetFrameName));
					this.parent.OnNavigating(webBrowserNavigatingEventArgs);
					cancel = webBrowserNavigatingEventArgs.Cancel;
					return;
				}
				cancel = true;
			}

			// Token: 0x06006E13 RID: 28179 RVA: 0x001933F0 File Offset: 0x001915F0
			public void DocumentComplete(object pDisp, ref object urlObject)
			{
				this.haveNavigated = true;
				if (this.parent.documentStreamToSetOnLoad != null && (string)urlObject == "about:blank")
				{
					HtmlDocument document = this.parent.Document;
					if (document != null)
					{
						UnsafeNativeMethods.IPersistStreamInit persistStreamInit = document.DomDocument as UnsafeNativeMethods.IPersistStreamInit;
						UnsafeNativeMethods.IStream pstm = new UnsafeNativeMethods.ComStreamFromDataStream(this.parent.documentStreamToSetOnLoad);
						persistStreamInit.Load(pstm);
						document.Encoding = "unicode";
					}
					this.parent.documentStreamToSetOnLoad = null;
					return;
				}
				string uriString = (urlObject == null) ? "" : urlObject.ToString();
				WebBrowserDocumentCompletedEventArgs e = new WebBrowserDocumentCompletedEventArgs(new Uri(uriString));
				this.parent.OnDocumentCompleted(e);
			}

			// Token: 0x06006E14 RID: 28180 RVA: 0x001934A2 File Offset: 0x001916A2
			public void TitleChange(string text)
			{
				this.parent.OnDocumentTitleChanged(EventArgs.Empty);
			}

			// Token: 0x06006E15 RID: 28181 RVA: 0x001934B4 File Offset: 0x001916B4
			public void SetSecureLockIcon(int secureLockIcon)
			{
				this.parent.encryptionLevel = (WebBrowserEncryptionLevel)secureLockIcon;
				this.parent.OnEncryptionLevelChanged(EventArgs.Empty);
			}

			// Token: 0x06006E16 RID: 28182 RVA: 0x001934D4 File Offset: 0x001916D4
			public void NavigateComplete2(object pDisp, ref object urlObject)
			{
				string uriString = (urlObject == null) ? "" : ((string)urlObject);
				WebBrowserNavigatedEventArgs e = new WebBrowserNavigatedEventArgs(new Uri(uriString));
				this.parent.OnNavigated(e);
			}

			// Token: 0x06006E17 RID: 28183 RVA: 0x0019350C File Offset: 0x0019170C
			public void NewWindow2(ref object ppDisp, ref bool cancel)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				this.parent.OnNewWindow(cancelEventArgs);
				cancel = cancelEventArgs.Cancel;
			}

			// Token: 0x06006E18 RID: 28184 RVA: 0x00193534 File Offset: 0x00191734
			public void ProgressChange(int progress, int progressMax)
			{
				WebBrowserProgressChangedEventArgs e = new WebBrowserProgressChangedEventArgs((long)progress, (long)progressMax);
				this.parent.OnProgressChanged(e);
			}

			// Token: 0x06006E19 RID: 28185 RVA: 0x00193557 File Offset: 0x00191757
			public void StatusTextChange(string text)
			{
				this.parent.statusText = ((text == null) ? "" : text);
				this.parent.OnStatusTextChanged(EventArgs.Empty);
			}

			// Token: 0x06006E1A RID: 28186 RVA: 0x0019357F File Offset: 0x0019177F
			public void DownloadBegin()
			{
				this.parent.OnFileDownload(EventArgs.Empty);
			}

			// Token: 0x06006E1B RID: 28187 RVA: 0x0000701A File Offset: 0x0000521A
			public void FileDownload(ref bool cancel)
			{
			}

			// Token: 0x06006E1C RID: 28188 RVA: 0x0000701A File Offset: 0x0000521A
			public void PrivacyImpactedStateChange(bool bImpacted)
			{
			}

			// Token: 0x06006E1D RID: 28189 RVA: 0x0000701A File Offset: 0x0000521A
			public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
			{
			}

			// Token: 0x06006E1E RID: 28190 RVA: 0x0000701A File Offset: 0x0000521A
			public void PrintTemplateTeardown(object pDisp)
			{
			}

			// Token: 0x06006E1F RID: 28191 RVA: 0x0000701A File Offset: 0x0000521A
			public void PrintTemplateInstantiation(object pDisp)
			{
			}

			// Token: 0x06006E20 RID: 28192 RVA: 0x0000701A File Offset: 0x0000521A
			public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
			{
			}

			// Token: 0x06006E21 RID: 28193 RVA: 0x0000701A File Offset: 0x0000521A
			public void ClientToHostWindow(ref long cX, ref long cY)
			{
			}

			// Token: 0x06006E22 RID: 28194 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowClosing(bool isChildWindow, ref bool cancel)
			{
			}

			// Token: 0x06006E23 RID: 28195 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowSetHeight(int height)
			{
			}

			// Token: 0x06006E24 RID: 28196 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowSetWidth(int width)
			{
			}

			// Token: 0x06006E25 RID: 28197 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowSetTop(int top)
			{
			}

			// Token: 0x06006E26 RID: 28198 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowSetLeft(int left)
			{
			}

			// Token: 0x06006E27 RID: 28199 RVA: 0x0000701A File Offset: 0x0000521A
			public void WindowSetResizable(bool resizable)
			{
			}

			// Token: 0x06006E28 RID: 28200 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnTheaterMode(bool theaterMode)
			{
			}

			// Token: 0x06006E29 RID: 28201 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnFullScreen(bool fullScreen)
			{
			}

			// Token: 0x06006E2A RID: 28202 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnStatusBar(bool statusBar)
			{
			}

			// Token: 0x06006E2B RID: 28203 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnMenuBar(bool menuBar)
			{
			}

			// Token: 0x06006E2C RID: 28204 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnToolBar(bool toolBar)
			{
			}

			// Token: 0x06006E2D RID: 28205 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnVisible(bool visible)
			{
			}

			// Token: 0x06006E2E RID: 28206 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnQuit()
			{
			}

			// Token: 0x06006E2F RID: 28207 RVA: 0x0000701A File Offset: 0x0000521A
			public void PropertyChange(string szProperty)
			{
			}

			// Token: 0x06006E30 RID: 28208 RVA: 0x0000701A File Offset: 0x0000521A
			public void DownloadComplete()
			{
			}

			// Token: 0x04004228 RID: 16936
			private WebBrowser parent;

			// Token: 0x04004229 RID: 16937
			private bool allowNavigation;

			// Token: 0x0400422A RID: 16938
			private bool haveNavigated;
		}
	}
}
