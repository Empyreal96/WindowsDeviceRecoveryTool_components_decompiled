using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000428 RID: 1064
	internal class WebBrowserContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame
	{
		// Token: 0x06004A8E RID: 19086 RVA: 0x00134CBB File Offset: 0x00132EBB
		internal WebBrowserContainer(WebBrowserBase parent)
		{
			this.parent = parent;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x00134CD5 File Offset: 0x00132ED5
		int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
		{
			if (ppmkOut != null)
			{
				ppmkOut[0] = null;
			}
			return -2147467263;
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x00134CE8 File Offset: 0x00132EE8
		int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = null;
			if ((grfFlags & 1) != 0)
			{
				ArrayList arrayList = new ArrayList();
				this.ListAXControls(arrayList, true);
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

		// Token: 0x06004A91 RID: 19089 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
		{
			return -2147467263;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x00134D3B File Offset: 0x00132F3B
		IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
		{
			return this.parent.Handle;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
		{
			return 0;
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			return -2147467263;
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x00134D48 File Offset: 0x00132F48
		int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
			if (pActiveObject == null)
			{
				if (this.ctlInEditMode != null)
				{
					this.ctlInEditMode.SetEditMode(WebBrowserHelper.AXEditMode.None);
					this.ctlInEditMode = null;
				}
				return 0;
			}
			WebBrowserBase webBrowserBase = null;
			UnsafeNativeMethods.IOleObject oleObject = pActiveObject as UnsafeNativeMethods.IOleObject;
			if (oleObject != null)
			{
				try
				{
					UnsafeNativeMethods.IOleClientSite clientSite = oleObject.GetClientSite();
					WebBrowserSiteBase webBrowserSiteBase = clientSite as WebBrowserSiteBase;
					if (webBrowserSiteBase != null)
					{
						webBrowserBase = webBrowserSiteBase.GetAXHost();
					}
				}
				catch (COMException ex)
				{
				}
				if (this.ctlInEditMode != null)
				{
					this.ctlInEditMode.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
					this.ctlInEditMode.SetEditMode(WebBrowserHelper.AXEditMode.None);
				}
				if (webBrowserBase == null)
				{
					this.ctlInEditMode = null;
				}
				else if (!webBrowserBase.IsUserMode)
				{
					this.ctlInEditMode = webBrowserBase;
					webBrowserBase.SetEditMode(WebBrowserHelper.AXEditMode.Object);
					webBrowserBase.AddSelectionHandler();
					webBrowserBase.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Active);
				}
			}
			return 0;
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
		{
			return 0;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			return -2147467263;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
		{
			return -2147467263;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
		{
			return -2147467263;
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x00033B0C File Offset: 0x00031D0C
		int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0000E214 File Offset: 0x0000C414
		int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
		{
			return 1;
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00134E04 File Offset: 0x00133004
		private void ListAXControls(ArrayList list, bool fuseOcx)
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
					WebBrowserBase webBrowserBase = control as WebBrowserBase;
					if (webBrowserBase != null)
					{
						if (fuseOcx)
						{
							object activeXInstance = webBrowserBase.activeXInstance;
							if (activeXInstance != null)
							{
								list.Add(activeXInstance);
							}
						}
						else
						{
							list.Add(control);
						}
					}
				}
			}
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00134E7E File Offset: 0x0013307E
		private Hashtable GetComponents()
		{
			return this.GetComponents(this.GetParentsContainer());
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x00134E8C File Offset: 0x0013308C
		private IContainer GetParentsContainer()
		{
			IContainer parentIContainer = this.GetParentIContainer();
			if (parentIContainer != null)
			{
				return parentIContainer;
			}
			return this.assocContainer;
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00134EAC File Offset: 0x001330AC
		private IContainer GetParentIContainer()
		{
			ISite site = this.parent.Site;
			if (site != null && site.DesignMode)
			{
				return site.Container;
			}
			return null;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x00134ED8 File Offset: 0x001330D8
		private Hashtable GetComponents(IContainer cont)
		{
			this.FillComponentsTable(cont);
			return this.components;
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x00134EE8 File Offset: 0x001330E8
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

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00135008 File Offset: 0x00133208
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

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0013509C File Offset: 0x0013329C
		private bool RegisterControl(WebBrowserBase ctl)
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

		// Token: 0x06004AA6 RID: 19110 RVA: 0x00135104 File Offset: 0x00133304
		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (sender == this.assocContainer && control != null)
			{
				this.RemoveControl(control);
			}
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x00135130 File Offset: 0x00133330
		internal void AddControl(Control ctl)
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

		// Token: 0x06004AA8 RID: 19112 RVA: 0x001351C6 File Offset: 0x001333C6
		internal void RemoveControl(Control ctl)
		{
			this.containerCache.Remove(ctl);
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x001351D4 File Offset: 0x001333D4
		internal static WebBrowserContainer FindContainerForControl(WebBrowserBase ctl)
		{
			if (ctl != null)
			{
				if (ctl.container != null)
				{
					return ctl.container;
				}
				ScrollableControl containingControl = ctl.ContainingControl;
				if (containingControl != null)
				{
					WebBrowserContainer webBrowserContainer = ctl.CreateWebBrowserContainer();
					if (webBrowserContainer.RegisterControl(ctl))
					{
						webBrowserContainer.AddControl(ctl);
						return webBrowserContainer;
					}
				}
			}
			return null;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00135218 File Offset: 0x00133418
		internal string GetNameForControl(Control ctl)
		{
			string text = (ctl.Site != null) ? ctl.Site.Name : ctl.Name;
			return text ?? "";
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0013524C File Offset: 0x0013344C
		internal void OnUIActivate(WebBrowserBase site)
		{
			if (this.siteUIActive == site)
			{
				return;
			}
			if (this.siteUIActive != null && this.siteUIActive != site)
			{
				WebBrowserBase webBrowserBase = this.siteUIActive;
				webBrowserBase.AXInPlaceObject.UIDeactivate();
			}
			site.AddSelectionHandler();
			this.siteUIActive = site;
			ContainerControl containingControl = site.ContainingControl;
			if (containingControl != null && containingControl.Contains(site))
			{
				containingControl.SetActiveControlInternal(site);
			}
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x001352AE File Offset: 0x001334AE
		internal void OnUIDeactivate(WebBrowserBase site)
		{
			this.siteUIActive = null;
			site.RemoveSelectionHandler();
			site.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
			site.SetEditMode(WebBrowserHelper.AXEditMode.None);
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x001352CC File Offset: 0x001334CC
		internal void OnInPlaceDeactivate(WebBrowserBase site)
		{
			if (this.siteActive == site)
			{
				this.siteActive = null;
				ContainerControl containerControl = this.parent.FindContainerControlInternal();
				if (containerControl != null)
				{
					containerControl.SetActiveControlInternal(null);
				}
			}
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x001352FF File Offset: 0x001334FF
		internal void OnExitEditMode(WebBrowserBase ctl)
		{
			if (this.ctlInEditMode == ctl)
			{
				this.ctlInEditMode = null;
			}
		}

		// Token: 0x04002732 RID: 10034
		private WebBrowserBase parent;

		// Token: 0x04002733 RID: 10035
		private IContainer assocContainer;

		// Token: 0x04002734 RID: 10036
		private WebBrowserBase siteUIActive;

		// Token: 0x04002735 RID: 10037
		private WebBrowserBase siteActive;

		// Token: 0x04002736 RID: 10038
		private Hashtable containerCache = new Hashtable();

		// Token: 0x04002737 RID: 10039
		private Hashtable components;

		// Token: 0x04002738 RID: 10040
		private WebBrowserBase ctlInEditMode;
	}
}
