using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Controls <see cref="T:System.Windows.Forms.ToolStrip" /> rendering and rafting, and the merging of <see cref="T:System.Windows.Forms.MenuStrip" />, <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />, and <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> objects. This class cannot be inherited.</summary>
	// Token: 0x020003D1 RID: 977
	public sealed class ToolStripManager
	{
		// Token: 0x060040AE RID: 16558 RVA: 0x00116B7A File Offset: 0x00114D7A
		private static void InitalizeThread()
		{
			if (!ToolStripManager.initialized)
			{
				ToolStripManager.initialized = true;
				ToolStripManager.currentRendererType = ToolStripManager.ProfessionalRendererType;
			}
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x000027DB File Offset: 0x000009DB
		private ToolStripManager()
		{
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x00116B94 File Offset: 0x00114D94
		static ToolStripManager()
		{
			SystemEvents.UserPreferenceChanging += ToolStripManager.OnUserPreferenceChanging;
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x060040B1 RID: 16561 RVA: 0x00116BF4 File Offset: 0x00114DF4
		internal static Font DefaultFont
		{
			get
			{
				if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					int num = ToolStripManager.CurrentDpi;
					Font font = null;
					if (!ToolStripManager.defaultFontCache.TryGetValue(num, out font) || font == null)
					{
						Font font2 = SystemInformation.GetMenuFontForDpi(num);
						if (font2 != null)
						{
							if (font2.Unit != GraphicsUnit.Point)
							{
								font = ControlPaint.FontInPoints(font2);
								font2.Dispose();
							}
							else
							{
								font = font2;
							}
							ToolStripManager.defaultFontCache[num] = font;
						}
					}
					return font;
				}
				Font font3 = ToolStripManager.defaultFont;
				if (font3 == null)
				{
					object obj = ToolStripManager.internalSyncObject;
					lock (obj)
					{
						font3 = ToolStripManager.defaultFont;
						if (font3 == null)
						{
							Font font2 = SystemFonts.MenuFont;
							if (font2 == null)
							{
								font2 = Control.DefaultFont;
							}
							if (font2 != null)
							{
								if (font2.Unit != GraphicsUnit.Point)
								{
									ToolStripManager.defaultFont = ControlPaint.FontInPoints(font2);
									font3 = ToolStripManager.defaultFont;
									font2.Dispose();
								}
								else
								{
									ToolStripManager.defaultFont = font2;
									font3 = ToolStripManager.defaultFont;
								}
							}
							return font3;
						}
					}
					return font3;
				}
				return font3;
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x00116CE4 File Offset: 0x00114EE4
		// (set) Token: 0x060040B3 RID: 16563 RVA: 0x00116CEB File Offset: 0x00114EEB
		internal static int CurrentDpi
		{
			get
			{
				return ToolStripManager.currentDpi;
			}
			set
			{
				ToolStripManager.currentDpi = value;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x00116CF3 File Offset: 0x00114EF3
		internal static ClientUtils.WeakRefCollection ToolStrips
		{
			get
			{
				if (ToolStripManager.toolStripWeakArrayList == null)
				{
					ToolStripManager.toolStripWeakArrayList = new ClientUtils.WeakRefCollection();
				}
				return ToolStripManager.toolStripWeakArrayList;
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x00116D0C File Offset: 0x00114F0C
		private static void AddEventHandler(int key, Delegate value)
		{
			object obj = ToolStripManager.internalSyncObject;
			lock (obj)
			{
				if (ToolStripManager.staticEventHandlers == null)
				{
					ToolStripManager.staticEventHandlers = new Delegate[1];
				}
				ToolStripManager.staticEventHandlers[key] = Delegate.Combine(ToolStripManager.staticEventHandlers[key], value);
			}
		}

		/// <summary>Finds the specified <see cref="T:System.Windows.Forms.ToolStrip" /> or a type derived from <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <param name="toolStripName">A string specifying the name of the <see cref="T:System.Windows.Forms.ToolStrip" /> or derived <see cref="T:System.Windows.Forms.ToolStrip" /> type to find.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStrip" /> or one of its derived types as specified by the <paramref name="toolStripName" /> parameter, or <see langword="null" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> is not found.</returns>
		// Token: 0x060040B6 RID: 16566 RVA: 0x00116D6C File Offset: 0x00114F6C
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static ToolStrip FindToolStrip(string toolStripName)
		{
			ToolStrip result = null;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				if (ToolStripManager.ToolStrips[i] != null && string.Equals(((ToolStrip)ToolStripManager.ToolStrips[i]).Name, toolStripName, StringComparison.Ordinal))
				{
					result = (ToolStrip)ToolStripManager.ToolStrips[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x00116DD0 File Offset: 0x00114FD0
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		internal static ToolStrip FindToolStrip(Form owningForm, string toolStripName)
		{
			ToolStrip toolStrip = null;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				if (ToolStripManager.ToolStrips[i] != null && string.Equals(((ToolStrip)ToolStripManager.ToolStrips[i]).Name, toolStripName, StringComparison.Ordinal))
				{
					toolStrip = (ToolStrip)ToolStripManager.ToolStrips[i];
					if (toolStrip.FindForm() == owningForm)
					{
						break;
					}
				}
			}
			return toolStrip;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x00116E3C File Offset: 0x0011503C
		private static bool CanChangeSelection(ToolStrip start, ToolStrip toolStrip)
		{
			if (toolStrip == null)
			{
				return false;
			}
			bool flag = !toolStrip.TabStop && toolStrip.Enabled && toolStrip.Visible && !toolStrip.IsDisposed && !toolStrip.Disposing && !toolStrip.IsDropDown && ToolStripManager.IsOnSameWindow(start, toolStrip);
			if (flag)
			{
				foreach (object obj in toolStrip.Items)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					if (toolStripItem.CanSelect)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00116EE4 File Offset: 0x001150E4
		private static bool ChangeSelection(ToolStrip start, ToolStrip toolStrip)
		{
			if (toolStrip == null || start == null)
			{
				return false;
			}
			if (start == toolStrip)
			{
				return false;
			}
			if (ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == start)
				{
					ToolStripManager.ModalMenuFilter.RemoveActiveToolStrip(start);
					start.NotifySelectionChange(null);
				}
				ToolStripManager.ModalMenuFilter.SetActiveToolStrip(toolStrip);
			}
			else
			{
				toolStrip.FocusInternal();
			}
			start.SnapFocusChange(toolStrip);
			toolStrip.SelectNextToolStripItem(null, toolStrip.RightToLeft != RightToLeft.Yes);
			return true;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x00116F48 File Offset: 0x00115148
		private static Delegate GetEventHandler(int key)
		{
			object obj = ToolStripManager.internalSyncObject;
			Delegate result;
			lock (obj)
			{
				if (ToolStripManager.staticEventHandlers == null)
				{
					result = null;
				}
				else
				{
					result = ToolStripManager.staticEventHandlers[key];
				}
			}
			return result;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x00116F98 File Offset: 0x00115198
		private static bool IsOnSameWindow(Control control1, Control control2)
		{
			return WindowsFormsUtils.GetRootHWnd(control1).Handle == WindowsFormsUtils.GetRootHWnd(control2).Handle;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x00116FC6 File Offset: 0x001151C6
		internal static bool IsThreadUsingToolStrips()
		{
			return ToolStripManager.toolStripWeakArrayList != null && ToolStripManager.toolStripWeakArrayList.Count > 0;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00116FE0 File Offset: 0x001151E0
		private static void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					ToolStripManager.defaultFontCache.Clear();
					return;
				}
				object obj = ToolStripManager.internalSyncObject;
				lock (obj)
				{
					ToolStripManager.defaultFont = null;
				}
			}
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x0011703C File Offset: 0x0011523C
		internal static void NotifyMenuModeChange(bool invalidateText, bool activationChange)
		{
			bool flag = false;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
				if (toolStrip == null)
				{
					flag = true;
				}
				else
				{
					if (invalidateText)
					{
						toolStrip.InvalidateTextItems();
					}
					if (activationChange)
					{
						toolStrip.KeyboardActive = false;
					}
				}
			}
			if (flag)
			{
				ToolStripManager.PruneToolStripList();
			}
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00117094 File Offset: 0x00115294
		internal static void PruneToolStripList()
		{
			if (ToolStripManager.toolStripWeakArrayList != null && ToolStripManager.toolStripWeakArrayList.Count > 0)
			{
				for (int i = ToolStripManager.toolStripWeakArrayList.Count - 1; i >= 0; i--)
				{
					if (ToolStripManager.toolStripWeakArrayList[i] == null)
					{
						ToolStripManager.toolStripWeakArrayList.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x001170E4 File Offset: 0x001152E4
		private static void RemoveEventHandler(int key, Delegate value)
		{
			object obj = ToolStripManager.internalSyncObject;
			lock (obj)
			{
				if (ToolStripManager.staticEventHandlers != null)
				{
					ToolStripManager.staticEventHandlers[key] = Delegate.Remove(ToolStripManager.staticEventHandlers[key], value);
				}
			}
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x00117138 File Offset: 0x00115338
		internal static bool SelectNextToolStrip(ToolStrip start, bool forward)
		{
			if (start == null || start.ParentInternal == null)
			{
				return false;
			}
			ToolStrip toolStrip = null;
			ToolStrip toolStrip2 = null;
			int tabIndex = start.TabIndex;
			int num = ToolStripManager.ToolStrips.IndexOf(start);
			int count = ToolStripManager.ToolStrips.Count;
			for (int i = 0; i < count; i++)
			{
				num = (forward ? ((num + 1) % count) : ((num + count - 1) % count));
				ToolStrip toolStrip3 = ToolStripManager.ToolStrips[num] as ToolStrip;
				if (toolStrip3 != null && toolStrip3 != start)
				{
					int tabIndex2 = toolStrip3.TabIndex;
					if (forward)
					{
						if (tabIndex2 >= tabIndex && ToolStripManager.CanChangeSelection(start, toolStrip3))
						{
							if (toolStrip2 == null)
							{
								toolStrip2 = toolStrip3;
							}
							else if (toolStrip3.TabIndex < toolStrip2.TabIndex)
							{
								toolStrip2 = toolStrip3;
							}
						}
						else if ((toolStrip == null || toolStrip3.TabIndex < toolStrip.TabIndex) && ToolStripManager.CanChangeSelection(start, toolStrip3))
						{
							toolStrip = toolStrip3;
						}
					}
					else if (tabIndex2 <= tabIndex && ToolStripManager.CanChangeSelection(start, toolStrip3))
					{
						if (toolStrip2 == null)
						{
							toolStrip2 = toolStrip3;
						}
						else if (toolStrip3.TabIndex > toolStrip2.TabIndex)
						{
							toolStrip2 = toolStrip3;
						}
					}
					else if ((toolStrip == null || toolStrip3.TabIndex > toolStrip.TabIndex) && ToolStripManager.CanChangeSelection(start, toolStrip3))
					{
						toolStrip = toolStrip3;
					}
					if (toolStrip2 != null && Math.Abs(toolStrip2.TabIndex - tabIndex) <= 1)
					{
						break;
					}
				}
			}
			if (toolStrip2 != null)
			{
				return ToolStripManager.ChangeSelection(start, toolStrip2);
			}
			return toolStrip != null && ToolStripManager.ChangeSelection(start, toolStrip);
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0011728E File Offset: 0x0011548E
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x0011729A File Offset: 0x0011549A
		private static Type CurrentRendererType
		{
			get
			{
				ToolStripManager.InitalizeThread();
				return ToolStripManager.currentRendererType;
			}
			set
			{
				ToolStripManager.currentRendererType = value;
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x001172A2 File Offset: 0x001154A2
		private static Type DefaultRendererType
		{
			get
			{
				return ToolStripManager.ProfessionalRendererType;
			}
		}

		/// <summary>Gets or sets the default painting styles for the form.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripRenderer" /> values.</returns>
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x001172A9 File Offset: 0x001154A9
		// (set) Token: 0x060040C6 RID: 16582 RVA: 0x001172C8 File Offset: 0x001154C8
		public static ToolStripRenderer Renderer
		{
			get
			{
				if (ToolStripManager.defaultRenderer == null)
				{
					ToolStripManager.defaultRenderer = ToolStripManager.CreateRenderer(ToolStripManager.RenderMode);
				}
				return ToolStripManager.defaultRenderer;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (ToolStripManager.defaultRenderer != value)
				{
					ToolStripManager.CurrentRendererType = ((value == null) ? ToolStripManager.DefaultRendererType : value.GetType());
					ToolStripManager.defaultRenderer = value;
					EventHandler eventHandler = (EventHandler)ToolStripManager.GetEventHandler(0);
					if (eventHandler != null)
					{
						eventHandler(null, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripManager.Renderer" /> property changes.</summary>
		// Token: 0x14000343 RID: 835
		// (add) Token: 0x060040C7 RID: 16583 RVA: 0x00117313 File Offset: 0x00115513
		// (remove) Token: 0x060040C8 RID: 16584 RVA: 0x0011731C File Offset: 0x0011551C
		public static event EventHandler RendererChanged
		{
			add
			{
				ToolStripManager.AddEventHandler(0, value);
			}
			remove
			{
				ToolStripManager.RemoveEventHandler(0, value);
			}
		}

		/// <summary>Gets or sets the default theme for the form.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripManagerRenderMode" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The set value was not one of the <see cref="T:System.Windows.Forms.ToolStripManagerRenderMode" /> values.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <see cref="T:System.Windows.Forms.ToolStripManagerRenderMode" /> is set to <see cref="F:System.Windows.Forms.ToolStripManagerRenderMode.Custom" />; use the <see cref="P:System.Windows.Forms.ToolStripManager.Renderer" /> property instead.</exception>
		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x060040C9 RID: 16585 RVA: 0x00117328 File Offset: 0x00115528
		// (set) Token: 0x060040CA RID: 16586 RVA: 0x00117370 File Offset: 0x00115570
		public static ToolStripManagerRenderMode RenderMode
		{
			get
			{
				Type left = ToolStripManager.CurrentRendererType;
				if (ToolStripManager.defaultRenderer != null && !ToolStripManager.defaultRenderer.IsAutoGenerated)
				{
					return ToolStripManagerRenderMode.Custom;
				}
				if (left == ToolStripManager.ProfessionalRendererType)
				{
					return ToolStripManagerRenderMode.Professional;
				}
				if (left == ToolStripManager.SystemRendererType)
				{
					return ToolStripManagerRenderMode.System;
				}
				return ToolStripManagerRenderMode.Custom;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripManagerRenderMode));
				}
				if (value == ToolStripManagerRenderMode.Custom)
				{
					throw new NotSupportedException(SR.GetString("ToolStripRenderModeUseRendererPropertyInstead"));
				}
				if (value - ToolStripManagerRenderMode.System <= 1)
				{
					ToolStripManager.Renderer = ToolStripManager.CreateRenderer(value);
					return;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a <see cref="T:System.Windows.Forms.ToolStrip" /> is rendered using visual style information called themes. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is rendered using themes; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x001173C8 File Offset: 0x001155C8
		// (set) Token: 0x060040CC RID: 16588 RVA: 0x001173D8 File Offset: 0x001155D8
		public static bool VisualStylesEnabled
		{
			get
			{
				return ToolStripManager.visualStylesEnabledIfPossible && Application.RenderWithVisualStyles;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				bool visualStylesEnabled = ToolStripManager.VisualStylesEnabled;
				ToolStripManager.visualStylesEnabledIfPossible = value;
				if (visualStylesEnabled != ToolStripManager.VisualStylesEnabled)
				{
					EventHandler eventHandler = (EventHandler)ToolStripManager.GetEventHandler(0);
					if (eventHandler != null)
					{
						eventHandler(null, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00117414 File Offset: 0x00115614
		internal static ToolStripRenderer CreateRenderer(ToolStripManagerRenderMode renderMode)
		{
			switch (renderMode)
			{
			case ToolStripManagerRenderMode.System:
				return new ToolStripSystemRenderer(true);
			case ToolStripManagerRenderMode.Professional:
				return new ToolStripProfessionalRenderer(true);
			}
			return new ToolStripSystemRenderer(true);
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x00117414 File Offset: 0x00115614
		internal static ToolStripRenderer CreateRenderer(ToolStripRenderMode renderMode)
		{
			switch (renderMode)
			{
			case ToolStripRenderMode.System:
				return new ToolStripSystemRenderer(true);
			case ToolStripRenderMode.Professional:
				return new ToolStripProfessionalRenderer(true);
			}
			return new ToolStripSystemRenderer(true);
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x060040CF RID: 16591 RVA: 0x0011743E File Offset: 0x0011563E
		internal static ClientUtils.WeakRefCollection ToolStripPanels
		{
			get
			{
				if (ToolStripManager.toolStripPanelWeakArrayList == null)
				{
					ToolStripManager.toolStripPanelWeakArrayList = new ClientUtils.WeakRefCollection();
				}
				return ToolStripManager.toolStripPanelWeakArrayList;
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00117458 File Offset: 0x00115658
		internal static ToolStripPanel ToolStripPanelFromPoint(Control draggedControl, Point screenLocation)
		{
			if (ToolStripManager.toolStripPanelWeakArrayList != null)
			{
				ISupportToolStripPanel supportToolStripPanel = draggedControl as ISupportToolStripPanel;
				bool isCurrentlyDragging = supportToolStripPanel.IsCurrentlyDragging;
				for (int i = 0; i < ToolStripManager.toolStripPanelWeakArrayList.Count; i++)
				{
					ToolStripPanel toolStripPanel = ToolStripManager.toolStripPanelWeakArrayList[i] as ToolStripPanel;
					if (toolStripPanel != null && toolStripPanel.IsHandleCreated && toolStripPanel.Visible && toolStripPanel.DragBounds.Contains(toolStripPanel.PointToClient(screenLocation)))
					{
						if (!isCurrentlyDragging)
						{
							return toolStripPanel;
						}
						if (ToolStripManager.IsOnSameWindow(draggedControl, toolStripPanel))
						{
							return toolStripPanel;
						}
					}
				}
			}
			return null;
		}

		/// <summary>Loads settings for the given <see cref="T:System.Windows.Forms.Form" /> using the full name of the <see cref="T:System.Windows.Forms.Form" /> as the settings key.</summary>
		/// <param name="targetForm">The <see cref="T:System.Windows.Forms.Form" /> whose name is also the settings key.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="targetForm" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060040D1 RID: 16593 RVA: 0x001174DC File Offset: 0x001156DC
		public static void LoadSettings(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}
			ToolStripManager.LoadSettings(targetForm, targetForm.GetType().FullName);
		}

		/// <summary>Loads settings for the specified <see cref="T:System.Windows.Forms.Form" /> using the specified settings key.</summary>
		/// <param name="targetForm">The <see cref="T:System.Windows.Forms.Form" /> for which to load settings.</param>
		/// <param name="key">A <see cref="T:System.String" /> representing the settings key for this <see cref="T:System.Windows.Forms.Form" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="targetForm" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is <see langword="null" /> or empty.</exception>
		// Token: 0x060040D2 RID: 16594 RVA: 0x00117500 File Offset: 0x00115700
		public static void LoadSettings(Form targetForm, string key)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			ToolStripSettingsManager toolStripSettingsManager = new ToolStripSettingsManager(targetForm, key);
			toolStripSettingsManager.Load();
		}

		/// <summary>Saves settings for the given <see cref="T:System.Windows.Forms.Form" /> using the full name of the <see cref="T:System.Windows.Forms.Form" /> as the settings key.</summary>
		/// <param name="sourceForm">The <see cref="T:System.Windows.Forms.Form" /> whose name is also the settings key.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceForm" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060040D3 RID: 16595 RVA: 0x0011753C File Offset: 0x0011573C
		public static void SaveSettings(Form sourceForm)
		{
			if (sourceForm == null)
			{
				throw new ArgumentNullException("sourceForm");
			}
			ToolStripManager.SaveSettings(sourceForm, sourceForm.GetType().FullName);
		}

		/// <summary>Saves settings for the specified <see cref="T:System.Windows.Forms.Form" /> using the specified settings key.</summary>
		/// <param name="sourceForm">The <see cref="T:System.Windows.Forms.Form" /> for which to save settings.</param>
		/// <param name="key">A <see cref="T:System.String" /> representing the settings key for this <see cref="T:System.Windows.Forms.Form" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceForm" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is <see langword="null" /> or empty.</exception>
		// Token: 0x060040D4 RID: 16596 RVA: 0x00117560 File Offset: 0x00115760
		public static void SaveSettings(Form sourceForm, string key)
		{
			if (sourceForm == null)
			{
				throw new ArgumentNullException("sourceForm");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			ToolStripSettingsManager toolStripSettingsManager = new ToolStripSettingsManager(sourceForm, key);
			toolStripSettingsManager.Save();
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x060040D5 RID: 16597 RVA: 0x0011759C File Offset: 0x0011579C
		internal static bool ShowMenuFocusCues
		{
			get
			{
				return DisplayInformation.MenuAccessKeysUnderlined || ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines;
			}
		}

		/// <summary>Retrieves a value indicating whether a defined shortcut key is valid.</summary>
		/// <param name="shortcut">The shortcut key to test for validity.</param>
		/// <returns>
		///     <see langword="true" /> if the shortcut key is valid; otherwise, <see langword="false" />. </returns>
		// Token: 0x060040D6 RID: 16598 RVA: 0x001175B4 File Offset: 0x001157B4
		public static bool IsValidShortcut(Keys shortcut)
		{
			Keys keys = shortcut & Keys.KeyCode;
			Keys keys2 = shortcut & Keys.Modifiers;
			return shortcut != Keys.None && (keys == Keys.Delete || keys == Keys.Insert || (keys >= Keys.F1 && keys <= Keys.F24) || (keys != Keys.None && keys2 != Keys.None && keys - Keys.ShiftKey > 2 && keys2 != Keys.Shift));
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x00117610 File Offset: 0x00115810
		internal static bool IsMenuKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			return Keys.Menu == keys || Keys.F10 == keys;
		}

		/// <summary>Retrieves a value indicating whether the specified shortcut key is used by any of the <see cref="T:System.Windows.Forms.ToolStrip" /> controls of a form.</summary>
		/// <param name="shortcut">The shortcut key for which to search.</param>
		/// <returns>
		///     <see langword="true" /> if the shortcut key is used by any <see cref="T:System.Windows.Forms.ToolStrip" /> on the form; otherwise, <see langword="false" />. </returns>
		// Token: 0x060040D8 RID: 16600 RVA: 0x00117634 File Offset: 0x00115834
		public static bool IsShortcutDefined(Keys shortcut)
		{
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
				if (toolStrip != null && toolStrip.Shortcuts.Contains(shortcut))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00117680 File Offset: 0x00115880
		internal static bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.IsValidShortcut(keyData))
			{
				return ToolStripManager.ProcessShortcut(ref m, keyData);
			}
			if (m.Msg == 260)
			{
				ToolStripManager.ModalMenuFilter.ProcessMenuKeyDown(ref m);
			}
			return false;
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x001176A8 File Offset: 0x001158A8
		internal static bool ProcessShortcut(ref Message m, Keys shortcut)
		{
			if (!ToolStripManager.IsThreadUsingToolStrips())
			{
				return false;
			}
			Control control = Control.FromChildHandleInternal(m.HWnd);
			Control control2 = control;
			if (control2 != null && ToolStripManager.IsValidShortcut(shortcut))
			{
				for (;;)
				{
					if (control2.ContextMenuStrip != null && control2.ContextMenuStrip.Shortcuts.ContainsKey(shortcut))
					{
						ToolStripMenuItem toolStripMenuItem = control2.ContextMenuStrip.Shortcuts[shortcut] as ToolStripMenuItem;
						if (toolStripMenuItem.ProcessCmdKey(ref m, shortcut))
						{
							break;
						}
					}
					control2 = control2.ParentInternal;
					if (control2 == null)
					{
						goto Block_6;
					}
				}
				return true;
				Block_6:
				if (control2 != null)
				{
					control = control2;
				}
				bool result = false;
				bool flag = false;
				for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
				{
					ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
					bool flag2 = false;
					bool flag3 = false;
					if (toolStrip == null)
					{
						flag = true;
					}
					else if ((control == null || toolStrip != control.ContextMenuStrip) && toolStrip.Shortcuts.ContainsKey(shortcut))
					{
						if (toolStrip.IsDropDown)
						{
							ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
							ContextMenuStrip contextMenuStrip = toolStripDropDown.GetFirstDropDown() as ContextMenuStrip;
							if (contextMenuStrip != null)
							{
								flag3 = contextMenuStrip.IsAssignedToDropDownItem;
								if (!flag3)
								{
									if (contextMenuStrip != control.ContextMenuStrip)
									{
										goto IL_1D0;
									}
									flag2 = true;
								}
							}
						}
						bool flag4 = false;
						if (!flag2)
						{
							ToolStrip toplevelOwnerToolStrip = toolStrip.GetToplevelOwnerToolStrip();
							if (toplevelOwnerToolStrip != null && control != null)
							{
								HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(toplevelOwnerToolStrip);
								HandleRef rootHWnd2 = WindowsFormsUtils.GetRootHWnd(control);
								flag4 = (rootHWnd.Handle == rootHWnd2.Handle);
								if (flag4)
								{
									Form form = Control.FromHandleInternal(rootHWnd2.Handle) as Form;
									if (form != null && form.IsMdiContainer)
									{
										Form form2 = toplevelOwnerToolStrip.FindFormInternal();
										if (form2 != form && form2 != null)
										{
											flag4 = (form2 == form.ActiveMdiChildInternal);
										}
									}
								}
							}
						}
						if (flag2 || flag4 || flag3)
						{
							ToolStripMenuItem toolStripMenuItem2 = toolStrip.Shortcuts[shortcut] as ToolStripMenuItem;
							if (toolStripMenuItem2 != null && toolStripMenuItem2.ProcessCmdKey(ref m, shortcut))
							{
								result = true;
								break;
							}
						}
					}
					IL_1D0:;
				}
				if (flag)
				{
					ToolStripManager.PruneToolStripList();
				}
				return result;
			}
			return false;
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x001178A8 File Offset: 0x00115AA8
		internal static bool ProcessMenuKey(ref Message m)
		{
			if (!ToolStripManager.IsThreadUsingToolStrips())
			{
				return false;
			}
			Keys keys = (Keys)((int)m.LParam);
			Control control = Control.FromHandleInternal(m.HWnd);
			Control control2 = null;
			MenuStrip menuStrip = null;
			if (control != null)
			{
				control2 = control.TopLevelControlInternal;
				if (control2 != null)
				{
					IntPtr menu = UnsafeNativeMethods.GetMenu(new HandleRef(control2, control2.Handle));
					if (menu == IntPtr.Zero)
					{
						menuStrip = ToolStripManager.GetMainMenuStrip(control2);
					}
				}
			}
			if ((ushort)keys == 32)
			{
				ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
			}
			else if ((ushort)keys == 45)
			{
				Form form = control2 as Form;
				if (form != null && form.IsMdiChild && form.WindowState == FormWindowState.Maximized)
				{
					ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
				}
			}
			else
			{
				if (UnsafeNativeMethods.GetKeyState(16) < 0 && keys == Keys.None)
				{
					return ToolStripManager.ModalMenuFilter.InMenuMode;
				}
				if (menuStrip != null && !ToolStripManager.ModalMenuFilter.MenuKeyToggle)
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(menuStrip);
					IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
					if (rootHWnd.Handle == foregroundWindow)
					{
						return menuStrip.OnMenuKey();
					}
				}
				else if (menuStrip != null)
				{
					ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x0011799C File Offset: 0x00115B9C
		internal static MenuStrip GetMainMenuStrip(Control control)
		{
			if (control == null)
			{
				return null;
			}
			Form form = control.FindFormInternal();
			if (form != null && form.MainMenuStrip != null)
			{
				return form.MainMenuStrip;
			}
			return ToolStripManager.GetFirstMenuStripRecursive(control.Controls);
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x001179D4 File Offset: 0x00115BD4
		private static MenuStrip GetFirstMenuStripRecursive(Control.ControlCollection controlsToLookIn)
		{
			try
			{
				for (int i = 0; i < controlsToLookIn.Count; i++)
				{
					if (controlsToLookIn[i] != null && controlsToLookIn[i] is MenuStrip)
					{
						return controlsToLookIn[i] as MenuStrip;
					}
				}
				for (int j = 0; j < controlsToLookIn.Count; j++)
				{
					if (controlsToLookIn[j] != null && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
					{
						MenuStrip firstMenuStripRecursive = ToolStripManager.GetFirstMenuStripRecursive(controlsToLookIn[j].Controls);
						if (firstMenuStripRecursive != null)
						{
							return firstMenuStripRecursive;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x00117A94 File Offset: 0x00115C94
		private static ToolStripItem FindMatch(ToolStripItem source, ToolStripItemCollection destinationItems)
		{
			ToolStripItem toolStripItem = null;
			if (source != null)
			{
				for (int i = 0; i < destinationItems.Count; i++)
				{
					ToolStripItem toolStripItem2 = destinationItems[i];
					if (WindowsFormsUtils.SafeCompareStrings(source.Text, toolStripItem2.Text, true))
					{
						toolStripItem = toolStripItem2;
						break;
					}
				}
				if (toolStripItem == null && source.MergeIndex > -1 && source.MergeIndex < destinationItems.Count)
				{
					toolStripItem = destinationItems[source.MergeIndex];
				}
			}
			return toolStripItem;
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00117B00 File Offset: 0x00115D00
		internal static ArrayList FindMergeableToolStrips(ContainerControl container)
		{
			ArrayList arrayList = new ArrayList();
			if (container != null)
			{
				for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
				{
					ToolStrip toolStrip = (ToolStrip)ToolStripManager.ToolStrips[i];
					if (toolStrip != null && toolStrip.AllowMerge && container == toolStrip.FindFormInternal())
					{
						arrayList.Add(toolStrip);
					}
				}
			}
			arrayList.Sort(new ToolStripCustomIComparer());
			return arrayList;
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00117B64 File Offset: 0x00115D64
		private static bool IsSpecialMDIStrip(ToolStrip toolStrip)
		{
			return toolStrip is MdiControlStrip || toolStrip is MdiWindowListStrip;
		}

		/// <summary>Combines two <see cref="T:System.Windows.Forms.ToolStrip" /> objects of different types.</summary>
		/// <param name="sourceToolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> to be combined with the <see cref="T:System.Windows.Forms.ToolStrip" /> referred to by the <paramref name="targetToolStrip" /> parameter.</param>
		/// <param name="targetToolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> that receives the <see cref="T:System.Windows.Forms.ToolStrip" /> referred to by the <paramref name="sourceToolStrip" /> parameter.</param>
		/// <returns>
		///     <see langword="true" /> if the merge is successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x060040E1 RID: 16609 RVA: 0x00117B7C File Offset: 0x00115D7C
		public static bool Merge(ToolStrip sourceToolStrip, ToolStrip targetToolStrip)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			if (targetToolStrip == null)
			{
				throw new ArgumentNullException("targetToolStrip");
			}
			if (targetToolStrip == sourceToolStrip)
			{
				throw new ArgumentException(SR.GetString("ToolStripMergeImpossibleIdentical"));
			}
			bool flag = ToolStripManager.IsSpecialMDIStrip(sourceToolStrip) || (sourceToolStrip.AllowMerge && targetToolStrip.AllowMerge && (sourceToolStrip.GetType().IsAssignableFrom(targetToolStrip.GetType()) || targetToolStrip.GetType().IsAssignableFrom(sourceToolStrip.GetType())));
			MergeHistory mergeHistory = null;
			if (flag)
			{
				mergeHistory = new MergeHistory(sourceToolStrip);
				int count = sourceToolStrip.Items.Count;
				if (count > 0)
				{
					sourceToolStrip.SuspendLayout();
					targetToolStrip.SuspendLayout();
					try
					{
						int num = count;
						int i = 0;
						int num2 = 0;
						while (i < count)
						{
							ToolStripItem source = sourceToolStrip.Items[num2];
							ToolStripManager.MergeRecursive(source, targetToolStrip.Items, mergeHistory.MergeHistoryItemsStack);
							int num3 = num - sourceToolStrip.Items.Count;
							num2 = ((num3 > 0) ? num2 : (num2 + 1));
							num = sourceToolStrip.Items.Count;
							i++;
						}
					}
					finally
					{
						sourceToolStrip.ResumeLayout();
						targetToolStrip.ResumeLayout();
					}
					if (mergeHistory.MergeHistoryItemsStack.Count > 0)
					{
						targetToolStrip.MergeHistoryStack.Push(mergeHistory);
					}
				}
			}
			bool result = false;
			if (mergeHistory != null && mergeHistory.MergeHistoryItemsStack.Count > 0)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x00117CE4 File Offset: 0x00115EE4
		private static void MergeRecursive(ToolStripItem source, ToolStripItemCollection destinationItems, Stack<MergeHistoryItem> history)
		{
			switch (source.MergeAction)
			{
			case MergeAction.Append:
			{
				MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
				mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
				mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
				mergeHistoryItem.TargetItem = source;
				int index = destinationItems.Add(source);
				mergeHistoryItem.Index = index;
				mergeHistoryItem.IndexCollection = destinationItems;
				history.Push(mergeHistoryItem);
				break;
			}
			case MergeAction.Insert:
				if (source.MergeIndex > -1)
				{
					MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
					mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
					mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
					mergeHistoryItem.TargetItem = source;
					int index2 = Math.Min(destinationItems.Count, source.MergeIndex);
					destinationItems.Insert(index2, source);
					mergeHistoryItem.IndexCollection = destinationItems;
					mergeHistoryItem.Index = index2;
					history.Push(mergeHistoryItem);
					return;
				}
				break;
			case MergeAction.Replace:
			case MergeAction.Remove:
			case MergeAction.MatchOnly:
			{
				ToolStripItem toolStripItem = ToolStripManager.FindMatch(source, destinationItems);
				if (toolStripItem != null)
				{
					MergeAction mergeAction = source.MergeAction;
					if (mergeAction - MergeAction.Replace > 1)
					{
						if (mergeAction != MergeAction.MatchOnly)
						{
							break;
						}
						ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
						ToolStripDropDownItem toolStripDropDownItem2 = source as ToolStripDropDownItem;
						if (toolStripDropDownItem == null || toolStripDropDownItem2 == null || toolStripDropDownItem2.DropDownItems.Count == 0)
						{
							break;
						}
						int count = toolStripDropDownItem2.DropDownItems.Count;
						if (count <= 0)
						{
							break;
						}
						int num = count;
						toolStripDropDownItem2.DropDown.SuspendLayout();
						try
						{
							int i = 0;
							int num2 = 0;
							while (i < count)
							{
								ToolStripManager.MergeRecursive(toolStripDropDownItem2.DropDownItems[num2], toolStripDropDownItem.DropDownItems, history);
								int num3 = num - toolStripDropDownItem2.DropDownItems.Count;
								num2 = ((num3 > 0) ? num2 : (num2 + 1));
								num = toolStripDropDownItem2.DropDownItems.Count;
								i++;
							}
							break;
						}
						finally
						{
							toolStripDropDownItem2.DropDown.ResumeLayout();
						}
					}
					MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Insert);
					mergeHistoryItem.TargetItem = toolStripItem;
					int index3 = destinationItems.IndexOf(toolStripItem);
					destinationItems.RemoveAt(index3);
					mergeHistoryItem.Index = index3;
					mergeHistoryItem.IndexCollection = destinationItems;
					mergeHistoryItem.TargetItem = toolStripItem;
					history.Push(mergeHistoryItem);
					if (source.MergeAction == MergeAction.Replace)
					{
						mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
						mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
						mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
						mergeHistoryItem.TargetItem = source;
						destinationItems.Insert(index3, source);
						mergeHistoryItem.Index = index3;
						mergeHistoryItem.IndexCollection = destinationItems;
						history.Push(mergeHistoryItem);
						return;
					}
				}
				break;
			}
			default:
				return;
			}
		}

		/// <summary>Combines two <see cref="T:System.Windows.Forms.ToolStrip" /> objects of the same type.</summary>
		/// <param name="sourceToolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> to be combined with the <see cref="T:System.Windows.Forms.ToolStrip" /> referred to by the <paramref name="targetName" /> parameter.</param>
		/// <param name="targetName">The name of the <see cref="T:System.Windows.Forms.ToolStrip" /> that receives the <see cref="T:System.Windows.Forms.ToolStrip" /> referred to by the <paramref name="sourceToolStrip" /> parameter.</param>
		/// <returns>
		///     <see langword="true" /> if the merge is successful; otherwise, <see langword="false" />. </returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceToolStrip" /> or <paramref name="targetName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="sourceToolStrip" /> or <paramref name="targetName" /> parameters refer to the same <see cref="T:System.Windows.Forms.ToolStrip" />.</exception>
		// Token: 0x060040E3 RID: 16611 RVA: 0x00117F64 File Offset: 0x00116164
		public static bool Merge(ToolStrip sourceToolStrip, string targetName)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			if (targetName == null)
			{
				throw new ArgumentNullException("targetName");
			}
			ToolStrip toolStrip = ToolStripManager.FindToolStrip(targetName);
			return toolStrip != null && ToolStripManager.Merge(sourceToolStrip, toolStrip);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00117FA0 File Offset: 0x001161A0
		internal static bool RevertMergeInternal(ToolStrip targetToolStrip, ToolStrip sourceToolStrip, bool revertMDIControls)
		{
			bool result = false;
			if (targetToolStrip == null)
			{
				throw new ArgumentNullException("targetToolStrip");
			}
			if (targetToolStrip == sourceToolStrip)
			{
				throw new ArgumentException(SR.GetString("ToolStripMergeImpossibleIdentical"));
			}
			bool flag = false;
			if (sourceToolStrip != null)
			{
				foreach (MergeHistory mergeHistory in targetToolStrip.MergeHistoryStack)
				{
					flag = (mergeHistory.MergedToolStrip == sourceToolStrip);
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (sourceToolStrip != null)
			{
				sourceToolStrip.SuspendLayout();
			}
			targetToolStrip.SuspendLayout();
			try
			{
				Stack<ToolStrip> stack = new Stack<ToolStrip>();
				flag = false;
				while (targetToolStrip.MergeHistoryStack.Count > 0)
				{
					if (flag)
					{
						break;
					}
					result = true;
					MergeHistory mergeHistory2 = targetToolStrip.MergeHistoryStack.Pop();
					if (mergeHistory2.MergedToolStrip == sourceToolStrip)
					{
						flag = true;
					}
					else if (!revertMDIControls && sourceToolStrip == null)
					{
						if (ToolStripManager.IsSpecialMDIStrip(mergeHistory2.MergedToolStrip))
						{
							stack.Push(mergeHistory2.MergedToolStrip);
						}
					}
					else
					{
						stack.Push(mergeHistory2.MergedToolStrip);
					}
					while (mergeHistory2.MergeHistoryItemsStack.Count > 0)
					{
						MergeHistoryItem mergeHistoryItem = mergeHistory2.MergeHistoryItemsStack.Pop();
						MergeAction mergeAction = mergeHistoryItem.MergeAction;
						if (mergeAction != MergeAction.Insert)
						{
							if (mergeAction == MergeAction.Remove)
							{
								mergeHistoryItem.IndexCollection.Remove(mergeHistoryItem.TargetItem);
								mergeHistoryItem.PreviousIndexCollection.Insert(Math.Min(mergeHistoryItem.PreviousIndex, mergeHistoryItem.PreviousIndexCollection.Count), mergeHistoryItem.TargetItem);
							}
						}
						else
						{
							mergeHistoryItem.IndexCollection.Insert(Math.Min(mergeHistoryItem.Index, mergeHistoryItem.IndexCollection.Count), mergeHistoryItem.TargetItem);
						}
					}
				}
				while (stack.Count > 0)
				{
					ToolStrip sourceToolStrip2 = stack.Pop();
					ToolStripManager.Merge(sourceToolStrip2, targetToolStrip);
				}
			}
			finally
			{
				if (sourceToolStrip != null)
				{
					sourceToolStrip.ResumeLayout();
				}
				targetToolStrip.ResumeLayout();
			}
			return result;
		}

		/// <summary>Undoes a merging of two <see cref="T:System.Windows.Forms.ToolStrip" /> objects, returning the specified <see cref="T:System.Windows.Forms.ToolStrip" /> to its state before the merge and nullifying all previous merge operations.</summary>
		/// <param name="targetToolStrip">The <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to undo a merge operation.</param>
		/// <returns>
		///     <see langword="true" /> if the undoing of the merge is successful; otherwise, <see langword="false" />. </returns>
		// Token: 0x060040E5 RID: 16613 RVA: 0x001181AC File Offset: 0x001163AC
		public static bool RevertMerge(ToolStrip targetToolStrip)
		{
			return ToolStripManager.RevertMergeInternal(targetToolStrip, null, false);
		}

		/// <summary>Undoes a merging of two <see cref="T:System.Windows.Forms.ToolStrip" /> objects, returning both <see cref="T:System.Windows.Forms.ToolStrip" /> controls to their state before the merge and nullifying all previous merge operations.</summary>
		/// <param name="targetToolStrip">The name of the <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to undo a merge operation.</param>
		/// <param name="sourceToolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> that was merged with the <paramref name="targetToolStrip" />.</param>
		/// <returns>
		///     <see langword="true" /> if the undoing of the merge is successful; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceToolStrip" /> is <see langword="null" />.</exception>
		// Token: 0x060040E6 RID: 16614 RVA: 0x001181B6 File Offset: 0x001163B6
		public static bool RevertMerge(ToolStrip targetToolStrip, ToolStrip sourceToolStrip)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			return ToolStripManager.RevertMergeInternal(targetToolStrip, sourceToolStrip, false);
		}

		/// <summary>Undoes a merging of two <see cref="T:System.Windows.Forms.ToolStrip" /> objects, returning the <see cref="T:System.Windows.Forms.ToolStrip" /> with the specified name to its state before the merge and nullifying all previous merge operations.</summary>
		/// <param name="targetName">The name of the <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to undo a merge operation.</param>
		/// <returns>
		///     <see langword="true" /> if the undoing of the merge is successful; otherwise, <see langword="false" />. </returns>
		// Token: 0x060040E7 RID: 16615 RVA: 0x001181D0 File Offset: 0x001163D0
		public static bool RevertMerge(string targetName)
		{
			ToolStrip toolStrip = ToolStripManager.FindToolStrip(targetName);
			return toolStrip != null && ToolStripManager.RevertMerge(toolStrip);
		}

		// Token: 0x040024DA RID: 9434
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection toolStripWeakArrayList;

		// Token: 0x040024DB RID: 9435
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection toolStripPanelWeakArrayList;

		// Token: 0x040024DC RID: 9436
		[ThreadStatic]
		private static bool initialized;

		// Token: 0x040024DD RID: 9437
		private static Font defaultFont;

		// Token: 0x040024DE RID: 9438
		private static ConcurrentDictionary<int, Font> defaultFontCache = new ConcurrentDictionary<int, Font>();

		// Token: 0x040024DF RID: 9439
		[ThreadStatic]
		private static Delegate[] staticEventHandlers;

		// Token: 0x040024E0 RID: 9440
		private const int staticEventDefaultRendererChanged = 0;

		// Token: 0x040024E1 RID: 9441
		private const int staticEventCount = 1;

		// Token: 0x040024E2 RID: 9442
		private static object internalSyncObject = new object();

		// Token: 0x040024E3 RID: 9443
		private static int currentDpi = DpiHelper.DeviceDpi;

		// Token: 0x040024E4 RID: 9444
		[ThreadStatic]
		private static ToolStripRenderer defaultRenderer;

		// Token: 0x040024E5 RID: 9445
		internal static Type SystemRendererType = typeof(ToolStripSystemRenderer);

		// Token: 0x040024E6 RID: 9446
		internal static Type ProfessionalRendererType = typeof(ToolStripProfessionalRenderer);

		// Token: 0x040024E7 RID: 9447
		private static bool visualStylesEnabledIfPossible = true;

		// Token: 0x040024E8 RID: 9448
		[ThreadStatic]
		private static Type currentRendererType;

		// Token: 0x0200073E RID: 1854
		internal class ModalMenuFilter : IMessageModifyAndFilter, IMessageFilter
		{
			// Token: 0x1700173E RID: 5950
			// (get) Token: 0x0600613E RID: 24894 RVA: 0x0018DF38 File Offset: 0x0018C138
			internal static ToolStripManager.ModalMenuFilter Instance
			{
				get
				{
					if (ToolStripManager.ModalMenuFilter._instance == null)
					{
						ToolStripManager.ModalMenuFilter._instance = new ToolStripManager.ModalMenuFilter();
					}
					return ToolStripManager.ModalMenuFilter._instance;
				}
			}

			// Token: 0x0600613F RID: 24895 RVA: 0x0018DF50 File Offset: 0x0018C150
			private ModalMenuFilter()
			{
			}

			// Token: 0x1700173F RID: 5951
			// (get) Token: 0x06006140 RID: 24896 RVA: 0x0018DF7A File Offset: 0x0018C17A
			internal static HandleRef ActiveHwnd
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance.ActiveHwndInternal;
				}
			}

			// Token: 0x17001740 RID: 5952
			// (get) Token: 0x06006141 RID: 24897 RVA: 0x0018DF86 File Offset: 0x0018C186
			// (set) Token: 0x06006142 RID: 24898 RVA: 0x0018DF8E File Offset: 0x0018C18E
			public bool ShowUnderlines
			{
				get
				{
					return this._showUnderlines;
				}
				set
				{
					if (this._showUnderlines != value)
					{
						this._showUnderlines = value;
						ToolStripManager.NotifyMenuModeChange(true, false);
					}
				}
			}

			// Token: 0x17001741 RID: 5953
			// (get) Token: 0x06006143 RID: 24899 RVA: 0x0018DFA7 File Offset: 0x0018C1A7
			// (set) Token: 0x06006144 RID: 24900 RVA: 0x0018DFB0 File Offset: 0x0018C1B0
			private HandleRef ActiveHwndInternal
			{
				get
				{
					return this._activeHwnd;
				}
				set
				{
					if (this._activeHwnd.Handle != value.Handle)
					{
						Control control;
						if (this._activeHwnd.Handle != IntPtr.Zero)
						{
							control = Control.FromHandleInternal(this._activeHwnd.Handle);
							if (control != null)
							{
								control.HandleCreated -= this.OnActiveHwndHandleCreated;
							}
						}
						this._activeHwnd = value;
						control = Control.FromHandleInternal(this._activeHwnd.Handle);
						if (control != null)
						{
							control.HandleCreated += this.OnActiveHwndHandleCreated;
						}
					}
				}
			}

			// Token: 0x17001742 RID: 5954
			// (get) Token: 0x06006145 RID: 24901 RVA: 0x0018E042 File Offset: 0x0018C242
			internal static bool InMenuMode
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance._inMenuMode;
				}
			}

			// Token: 0x17001743 RID: 5955
			// (get) Token: 0x06006146 RID: 24902 RVA: 0x0018E04E File Offset: 0x0018C24E
			// (set) Token: 0x06006147 RID: 24903 RVA: 0x0018E05A File Offset: 0x0018C25A
			internal static bool MenuKeyToggle
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle;
				}
				set
				{
					if (ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle != value)
					{
						ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle = value;
					}
				}
			}

			// Token: 0x17001744 RID: 5956
			// (get) Token: 0x06006148 RID: 24904 RVA: 0x0018E074 File Offset: 0x0018C274
			private ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook MessageHook
			{
				get
				{
					if (this.messageHook == null)
					{
						this.messageHook = new ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook();
					}
					return this.messageHook;
				}
			}

			// Token: 0x06006149 RID: 24905 RVA: 0x0018E090 File Offset: 0x0018C290
			private void EnterMenuModeCore()
			{
				if (!ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
					if (activeWindow != IntPtr.Zero)
					{
						this.ActiveHwndInternal = new HandleRef(this, activeWindow);
					}
					Application.ThreadContext.FromCurrent().AddMessageFilter(this);
					Application.ThreadContext.FromCurrent().TrackInput(true);
					if (!Application.ThreadContext.FromCurrent().GetMessageLoop(true))
					{
						this.MessageHook.HookMessages = true;
					}
					this._inMenuMode = true;
					if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
					{
						this.NotifyLastLastFocusedToolAboutFocusLoss();
					}
					this.ProcessMessages(true);
				}
			}

			// Token: 0x0600614A RID: 24906 RVA: 0x0018E110 File Offset: 0x0018C310
			internal void NotifyLastLastFocusedToolAboutFocusLoss()
			{
				IKeyboardToolTip keyboardToolTip = KeyboardToolTipStateMachine.Instance.LastFocusedTool;
				if (keyboardToolTip != null)
				{
					this.lastFocusedTool.SetTarget(keyboardToolTip);
					KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(keyboardToolTip);
				}
			}

			// Token: 0x0600614B RID: 24907 RVA: 0x0018E142 File Offset: 0x0018C342
			internal static void ExitMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance.ExitMenuModeCore();
			}

			// Token: 0x0600614C RID: 24908 RVA: 0x0018E150 File Offset: 0x0018C350
			private void ExitMenuModeCore()
			{
				this.ProcessMessages(false);
				if (ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					try
					{
						if (this.messageHook != null)
						{
							this.messageHook.HookMessages = false;
						}
						Application.ThreadContext.FromCurrent().RemoveMessageFilter(this);
						Application.ThreadContext.FromCurrent().TrackInput(false);
						if (ToolStripManager.ModalMenuFilter.ActiveHwnd.Handle != IntPtr.Zero)
						{
							Control control = Control.FromHandleInternal(ToolStripManager.ModalMenuFilter.ActiveHwnd.Handle);
							if (control != null)
							{
								control.HandleCreated -= this.OnActiveHwndHandleCreated;
							}
							this.ActiveHwndInternal = NativeMethods.NullHandleRef;
						}
						if (this._inputFilterQueue != null)
						{
							this._inputFilterQueue.Clear();
						}
						if (this._caretHidden)
						{
							this._caretHidden = false;
							SafeNativeMethods.ShowCaret(NativeMethods.NullHandleRef);
						}
						IKeyboardToolTip keyboardToolTip;
						if (!AccessibilityImprovements.UseLegacyToolTipDisplay && this.lastFocusedTool.TryGetTarget(out keyboardToolTip) && keyboardToolTip != null)
						{
							KeyboardToolTipStateMachine.Instance.NotifyAboutGotFocus(keyboardToolTip);
						}
					}
					finally
					{
						this._inMenuMode = false;
						bool showUnderlines = this._showUnderlines;
						this._showUnderlines = false;
						ToolStripManager.NotifyMenuModeChange(showUnderlines, true);
					}
				}
			}

			// Token: 0x0600614D RID: 24909 RVA: 0x0018E264 File Offset: 0x0018C464
			internal static ToolStrip GetActiveToolStrip()
			{
				return ToolStripManager.ModalMenuFilter.Instance.GetActiveToolStripInternal();
			}

			// Token: 0x0600614E RID: 24910 RVA: 0x0018E270 File Offset: 0x0018C470
			internal ToolStrip GetActiveToolStripInternal()
			{
				if (this._inputFilterQueue != null && this._inputFilterQueue.Count > 0)
				{
					return this._inputFilterQueue[this._inputFilterQueue.Count - 1];
				}
				return null;
			}

			// Token: 0x0600614F RID: 24911 RVA: 0x0018E2A4 File Offset: 0x0018C4A4
			private ToolStrip GetCurrentToplevelToolStrip()
			{
				if (this._toplevelToolStrip == null)
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal != null)
					{
						this._toplevelToolStrip = activeToolStripInternal.GetToplevelOwnerToolStrip();
					}
				}
				return this._toplevelToolStrip;
			}

			// Token: 0x06006150 RID: 24912 RVA: 0x0018E2D8 File Offset: 0x0018C4D8
			private void OnActiveHwndHandleCreated(object sender, EventArgs e)
			{
				Control control = sender as Control;
				this.ActiveHwndInternal = new HandleRef(this, control.Handle);
			}

			// Token: 0x06006151 RID: 24913 RVA: 0x0018E300 File Offset: 0x0018C500
			internal static void ProcessMenuKeyDown(ref Message m)
			{
				Keys keyData = (Keys)((int)m.WParam);
				ToolStrip toolStrip = Control.FromHandleInternal(m.HWnd) as ToolStrip;
				if (toolStrip != null && !toolStrip.IsDropDown)
				{
					return;
				}
				if (ToolStripManager.IsMenuKey(keyData))
				{
					if (!ToolStripManager.ModalMenuFilter.InMenuMode && ToolStripManager.ModalMenuFilter.MenuKeyToggle)
					{
						ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
						return;
					}
					if (!ToolStripManager.ModalMenuFilter.MenuKeyToggle)
					{
						ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
					}
				}
			}

			// Token: 0x06006152 RID: 24914 RVA: 0x0018E365 File Offset: 0x0018C565
			internal static void CloseActiveDropDown(ToolStripDropDown activeToolStripDropDown, ToolStripDropDownCloseReason reason)
			{
				activeToolStripDropDown.SetCloseReason(reason);
				activeToolStripDropDown.Visible = false;
				if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == null)
				{
					ToolStripManager.ModalMenuFilter.ExitMenuMode();
					if (activeToolStripDropDown.OwnerItem != null)
					{
						activeToolStripDropDown.OwnerItem.Unselect();
					}
				}
			}

			// Token: 0x06006153 RID: 24915 RVA: 0x0018E394 File Offset: 0x0018C594
			private void ProcessMessages(bool process)
			{
				if (process)
				{
					if (this._ensureMessageProcessingTimer == null)
					{
						this._ensureMessageProcessingTimer = new Timer();
					}
					this._ensureMessageProcessingTimer.Interval = 500;
					this._ensureMessageProcessingTimer.Enabled = true;
					return;
				}
				if (this._ensureMessageProcessingTimer != null)
				{
					this._ensureMessageProcessingTimer.Enabled = false;
					this._ensureMessageProcessingTimer.Dispose();
					this._ensureMessageProcessingTimer = null;
				}
			}

			// Token: 0x06006154 RID: 24916 RVA: 0x0018E3FC File Offset: 0x0018C5FC
			private void ProcessMouseButtonPressed(IntPtr hwndMouseMessageIsFrom, int x, int y)
			{
				int count = this._inputFilterQueue.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal == null)
					{
						break;
					}
					NativeMethods.POINT point = new NativeMethods.POINT();
					point.x = x;
					point.y = y;
					UnsafeNativeMethods.MapWindowPoints(new HandleRef(activeToolStripInternal, hwndMouseMessageIsFrom), new HandleRef(activeToolStripInternal, activeToolStripInternal.Handle), point, 1);
					if (activeToolStripInternal.ClientRectangle.Contains(point.x, point.y))
					{
						break;
					}
					ToolStripDropDown toolStripDropDown = activeToolStripInternal as ToolStripDropDown;
					if (toolStripDropDown != null)
					{
						if (toolStripDropDown.OwnerToolStrip == null || !(toolStripDropDown.OwnerToolStrip.Handle == hwndMouseMessageIsFrom) || toolStripDropDown.OwnerDropDownItem == null || !toolStripDropDown.OwnerDropDownItem.DropDownButtonArea.Contains(x, y))
						{
							ToolStripManager.ModalMenuFilter.CloseActiveDropDown(toolStripDropDown, ToolStripDropDownCloseReason.AppClicked);
						}
					}
					else
					{
						activeToolStripInternal.NotifySelectionChange(null);
						this.ExitMenuModeCore();
					}
				}
			}

			// Token: 0x06006155 RID: 24917 RVA: 0x0018E4E4 File Offset: 0x0018C6E4
			private bool ProcessActivationChange()
			{
				int count = this._inputFilterQueue.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStripDropDown toolStripDropDown = this.GetActiveToolStripInternal() as ToolStripDropDown;
					if (toolStripDropDown != null && toolStripDropDown.AutoClose)
					{
						toolStripDropDown.Visible = false;
					}
				}
				this.ExitMenuModeCore();
				return true;
			}

			// Token: 0x06006156 RID: 24918 RVA: 0x0018E52E File Offset: 0x0018C72E
			internal static void SetActiveToolStrip(ToolStrip toolStrip, bool menuKeyPressed)
			{
				if (!ToolStripManager.ModalMenuFilter.InMenuMode && menuKeyPressed)
				{
					ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
				}
				ToolStripManager.ModalMenuFilter.Instance.SetActiveToolStripCore(toolStrip);
			}

			// Token: 0x06006157 RID: 24919 RVA: 0x0018E552 File Offset: 0x0018C752
			internal static void SetActiveToolStrip(ToolStrip toolStrip)
			{
				ToolStripManager.ModalMenuFilter.Instance.SetActiveToolStripCore(toolStrip);
			}

			// Token: 0x06006158 RID: 24920 RVA: 0x0018E560 File Offset: 0x0018C760
			private void SetActiveToolStripCore(ToolStrip toolStrip)
			{
				if (toolStrip == null)
				{
					return;
				}
				if (toolStrip.IsDropDown)
				{
					ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
					if (!toolStripDropDown.AutoClose)
					{
						IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
						if (activeWindow != IntPtr.Zero)
						{
							this.ActiveHwndInternal = new HandleRef(this, activeWindow);
						}
						return;
					}
				}
				toolStrip.KeyboardActive = true;
				if (this._inputFilterQueue == null)
				{
					this._inputFilterQueue = new List<ToolStrip>();
				}
				else
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal != null)
					{
						if (!activeToolStripInternal.IsDropDown)
						{
							this._inputFilterQueue.Remove(activeToolStripInternal);
						}
						else if (toolStrip.IsDropDown && ToolStripDropDown.GetFirstDropDown(toolStrip) != ToolStripDropDown.GetFirstDropDown(activeToolStripInternal))
						{
							this._inputFilterQueue.Remove(activeToolStripInternal);
							ToolStripDropDown toolStripDropDown2 = activeToolStripInternal as ToolStripDropDown;
							toolStripDropDown2.DismissAll();
						}
					}
				}
				this._toplevelToolStrip = null;
				if (!this._inputFilterQueue.Contains(toolStrip))
				{
					this._inputFilterQueue.Add(toolStrip);
				}
				if (!ToolStripManager.ModalMenuFilter.InMenuMode && this._inputFilterQueue.Count > 0)
				{
					this.EnterMenuModeCore();
				}
				if (!this._caretHidden && toolStrip.IsDropDown && ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					this._caretHidden = true;
					SafeNativeMethods.HideCaret(NativeMethods.NullHandleRef);
				}
			}

			// Token: 0x06006159 RID: 24921 RVA: 0x0018E67B File Offset: 0x0018C87B
			internal static void SuspendMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance._suspendMenuMode = true;
			}

			// Token: 0x0600615A RID: 24922 RVA: 0x0018E688 File Offset: 0x0018C888
			internal static void ResumeMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance._suspendMenuMode = false;
			}

			// Token: 0x0600615B RID: 24923 RVA: 0x0018E695 File Offset: 0x0018C895
			internal static void RemoveActiveToolStrip(ToolStrip toolStrip)
			{
				ToolStripManager.ModalMenuFilter.Instance.RemoveActiveToolStripCore(toolStrip);
			}

			// Token: 0x0600615C RID: 24924 RVA: 0x0018E6A2 File Offset: 0x0018C8A2
			private void RemoveActiveToolStripCore(ToolStrip toolStrip)
			{
				this._toplevelToolStrip = null;
				if (this._inputFilterQueue != null)
				{
					this._inputFilterQueue.Remove(toolStrip);
				}
			}

			// Token: 0x0600615D RID: 24925 RVA: 0x0018E6C0 File Offset: 0x0018C8C0
			private static bool IsChildOrSameWindow(HandleRef hwndParent, HandleRef hwndChild)
			{
				return hwndParent.Handle == hwndChild.Handle || UnsafeNativeMethods.IsChild(hwndParent, hwndChild);
			}

			// Token: 0x0600615E RID: 24926 RVA: 0x0018E6E8 File Offset: 0x0018C8E8
			private static bool IsKeyOrMouseMessage(Message m)
			{
				bool result = false;
				if (m.Msg >= 512 && m.Msg <= 522)
				{
					result = true;
				}
				else if (m.Msg >= 161 && m.Msg <= 169)
				{
					result = true;
				}
				else if (m.Msg >= 256 && m.Msg <= 264)
				{
					result = true;
				}
				return result;
			}

			// Token: 0x0600615F RID: 24927 RVA: 0x0018E758 File Offset: 0x0018C958
			public bool PreFilterMessage(ref Message m)
			{
				if (this._suspendMenuMode)
				{
					return false;
				}
				ToolStrip activeToolStrip = ToolStripManager.ModalMenuFilter.GetActiveToolStrip();
				if (activeToolStrip == null)
				{
					return false;
				}
				if (activeToolStrip.IsDisposed)
				{
					this.RemoveActiveToolStripCore(activeToolStrip);
					return false;
				}
				HandleRef handleRef = new HandleRef(activeToolStrip, activeToolStrip.Handle);
				HandleRef handleRef2 = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
				if (handleRef2.Handle != this._lastActiveWindow.Handle)
				{
					if (handleRef2.Handle == IntPtr.Zero)
					{
						this.ProcessActivationChange();
					}
					else if (!(Control.FromChildHandleInternal(handleRef2.Handle) is ToolStripDropDown) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef2, handleRef) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef2, ToolStripManager.ModalMenuFilter.ActiveHwnd))
					{
						this.ProcessActivationChange();
					}
				}
				this._lastActiveWindow = handleRef2;
				if (!ToolStripManager.ModalMenuFilter.IsKeyOrMouseMessage(m))
				{
					return false;
				}
				DpiAwarenessContext awareness = CommonUnsafeNativeMethods.TryGetDpiAwarenessContextForWindow(m.HWnd);
				using (DpiHelper.EnterDpiAwarenessScope(awareness))
				{
					int msg = m.Msg;
					if (msg <= 167)
					{
						switch (msg)
						{
						case 160:
							goto IL_153;
						case 161:
						case 164:
							break;
						case 162:
						case 163:
							goto IL_23E;
						default:
							if (msg != 167)
							{
								goto IL_23E;
							}
							break;
						}
						this.ProcessMouseButtonPressed(IntPtr.Zero, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
						goto IL_23E;
					}
					if (msg - 256 > 7)
					{
						switch (msg)
						{
						case 512:
							goto IL_153;
						case 513:
						case 516:
							break;
						case 514:
						case 515:
							goto IL_23E;
						default:
							if (msg != 519)
							{
								goto IL_23E;
							}
							break;
						}
						this.ProcessMouseButtonPressed(m.HWnd, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
						goto IL_23E;
					}
					if (!activeToolStrip.ContainsFocus)
					{
						m.HWnd = activeToolStrip.Handle;
						goto IL_23E;
					}
					goto IL_23E;
					IL_153:
					Control control = Control.FromChildHandleInternal(m.HWnd);
					if ((control == null || !(control.TopLevelControlInternal is ToolStripDropDown)) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef, new HandleRef(null, m.HWnd)))
					{
						ToolStrip currentToplevelToolStrip = this.GetCurrentToplevelToolStrip();
						if (currentToplevelToolStrip != null && ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(new HandleRef(currentToplevelToolStrip, currentToplevelToolStrip.Handle), new HandleRef(null, m.HWnd)))
						{
							return false;
						}
						if (!ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(ToolStripManager.ModalMenuFilter.ActiveHwnd, new HandleRef(null, m.HWnd)))
						{
							return false;
						}
						return true;
					}
					IL_23E:;
				}
				return false;
			}

			// Token: 0x04004188 RID: 16776
			private HandleRef _activeHwnd = NativeMethods.NullHandleRef;

			// Token: 0x04004189 RID: 16777
			private HandleRef _lastActiveWindow = NativeMethods.NullHandleRef;

			// Token: 0x0400418A RID: 16778
			private List<ToolStrip> _inputFilterQueue;

			// Token: 0x0400418B RID: 16779
			private bool _inMenuMode;

			// Token: 0x0400418C RID: 16780
			private bool _caretHidden;

			// Token: 0x0400418D RID: 16781
			private bool _showUnderlines;

			// Token: 0x0400418E RID: 16782
			private bool menuKeyToggle;

			// Token: 0x0400418F RID: 16783
			private bool _suspendMenuMode;

			// Token: 0x04004190 RID: 16784
			private ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook messageHook;

			// Token: 0x04004191 RID: 16785
			private Timer _ensureMessageProcessingTimer;

			// Token: 0x04004192 RID: 16786
			private const int MESSAGE_PROCESSING_INTERVAL = 500;

			// Token: 0x04004193 RID: 16787
			private ToolStrip _toplevelToolStrip;

			// Token: 0x04004194 RID: 16788
			private readonly WeakReference<IKeyboardToolTip> lastFocusedTool = new WeakReference<IKeyboardToolTip>(null);

			// Token: 0x04004195 RID: 16789
			[ThreadStatic]
			private static ToolStripManager.ModalMenuFilter _instance;

			// Token: 0x020008A0 RID: 2208
			private class HostedWindowsFormsMessageHook
			{
				// Token: 0x17001883 RID: 6275
				// (get) Token: 0x060070E0 RID: 28896 RVA: 0x0019C670 File Offset: 0x0019A870
				// (set) Token: 0x060070E1 RID: 28897 RVA: 0x0019C682 File Offset: 0x0019A882
				public bool HookMessages
				{
					get
					{
						return this.messageHookHandle != IntPtr.Zero;
					}
					set
					{
						if (value)
						{
							this.InstallMessageHook();
							return;
						}
						this.UninstallMessageHook();
					}
				}

				// Token: 0x060070E2 RID: 28898 RVA: 0x0019C694 File Offset: 0x0019A894
				private void InstallMessageHook()
				{
					lock (this)
					{
						if (!(this.messageHookHandle != IntPtr.Zero))
						{
							this.hookProc = new NativeMethods.HookProc(this.MessageHookProc);
							this.messageHookHandle = UnsafeNativeMethods.SetWindowsHookEx(3, this.hookProc, new HandleRef(null, IntPtr.Zero), SafeNativeMethods.GetCurrentThreadId());
							if (this.messageHookHandle != IntPtr.Zero)
							{
								this.isHooked = true;
							}
						}
					}
				}

				// Token: 0x060070E3 RID: 28899 RVA: 0x0019C72C File Offset: 0x0019A92C
				private unsafe IntPtr MessageHookProc(int nCode, IntPtr wparam, IntPtr lparam)
				{
					if (nCode == 0 && this.isHooked && (int)wparam == 1)
					{
						NativeMethods.MSG* ptr = (NativeMethods.MSG*)((void*)lparam);
						if (ptr != null && Application.ThreadContext.FromCurrent().PreTranslateMessage(ref *ptr))
						{
							ptr->message = 0;
						}
					}
					return UnsafeNativeMethods.CallNextHookEx(new HandleRef(this, this.messageHookHandle), nCode, wparam, lparam);
				}

				// Token: 0x060070E4 RID: 28900 RVA: 0x0019C784 File Offset: 0x0019A984
				private void UninstallMessageHook()
				{
					lock (this)
					{
						if (this.messageHookHandle != IntPtr.Zero)
						{
							UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(this, this.messageHookHandle));
							this.hookProc = null;
							this.messageHookHandle = IntPtr.Zero;
							this.isHooked = false;
						}
					}
				}

				// Token: 0x04004406 RID: 17414
				private IntPtr messageHookHandle = IntPtr.Zero;

				// Token: 0x04004407 RID: 17415
				private bool isHooked;

				// Token: 0x04004408 RID: 17416
				private NativeMethods.HookProc hookProc;
			}
		}
	}
}
