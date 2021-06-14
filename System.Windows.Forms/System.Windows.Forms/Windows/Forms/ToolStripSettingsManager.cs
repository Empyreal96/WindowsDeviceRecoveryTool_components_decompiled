using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
	// Token: 0x020003F2 RID: 1010
	internal class ToolStripSettingsManager
	{
		// Token: 0x060043F6 RID: 17398 RVA: 0x001222FE File Offset: 0x001204FE
		internal ToolStripSettingsManager(Form owner, string formKey)
		{
			this.form = owner;
			this.formKey = formKey;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00122314 File Offset: 0x00120514
		internal void Load()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.FindToolStrips(true, this.form.Controls))
			{
				ToolStrip toolStrip = (ToolStrip)obj;
				if (toolStrip != null && !string.IsNullOrEmpty(toolStrip.Name))
				{
					ToolStripSettings toolStripSettings = new ToolStripSettings(this.GetSettingsKey(toolStrip));
					if (!toolStripSettings.IsDefault)
					{
						arrayList.Add(new ToolStripSettingsManager.SettingsStub(toolStripSettings));
					}
				}
			}
			this.ApplySettings(arrayList);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x001223BC File Offset: 0x001205BC
		internal void Save()
		{
			foreach (object obj in this.FindToolStrips(true, this.form.Controls))
			{
				ToolStrip toolStrip = (ToolStrip)obj;
				if (toolStrip != null && !string.IsNullOrEmpty(toolStrip.Name))
				{
					ToolStripSettings toolStripSettings = new ToolStripSettings(this.GetSettingsKey(toolStrip));
					ToolStripSettingsManager.SettingsStub settingsStub = new ToolStripSettingsManager.SettingsStub(toolStrip);
					toolStripSettings.ItemOrder = settingsStub.ItemOrder;
					toolStripSettings.Name = settingsStub.Name;
					toolStripSettings.Location = settingsStub.Location;
					toolStripSettings.Size = settingsStub.Size;
					toolStripSettings.ToolStripPanelName = settingsStub.ToolStripPanelName;
					toolStripSettings.Visible = settingsStub.Visible;
					toolStripSettings.Save();
				}
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x00122494 File Offset: 0x00120694
		internal static string GetItemOrder(ToolStrip toolStrip)
		{
			StringBuilder stringBuilder = new StringBuilder(toolStrip.Items.Count);
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				stringBuilder.Append((toolStrip.Items[i].Name == null) ? "null" : toolStrip.Items[i].Name);
				if (i != toolStrip.Items.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0012251C File Offset: 0x0012071C
		private void ApplySettings(ArrayList toolStripSettingsToApply)
		{
			if (toolStripSettingsToApply.Count == 0)
			{
				return;
			}
			this.SuspendAllLayout(this.form);
			Dictionary<string, ToolStrip> itemLocationHash = this.BuildItemOriginationHash();
			Dictionary<object, List<ToolStripSettingsManager.SettingsStub>> dictionary = new Dictionary<object, List<ToolStripSettingsManager.SettingsStub>>();
			foreach (object obj in toolStripSettingsToApply)
			{
				ToolStripSettingsManager.SettingsStub settingsStub = (ToolStripSettingsManager.SettingsStub)obj;
				object obj2 = (!string.IsNullOrEmpty(settingsStub.ToolStripPanelName)) ? settingsStub.ToolStripPanelName : null;
				if (obj2 == null)
				{
					if (!string.IsNullOrEmpty(settingsStub.Name))
					{
						ToolStrip toolStrip = ToolStripManager.FindToolStrip(this.form, settingsStub.Name);
						this.ApplyToolStripSettings(toolStrip, settingsStub, itemLocationHash);
					}
				}
				else
				{
					if (!dictionary.ContainsKey(obj2))
					{
						dictionary[obj2] = new List<ToolStripSettingsManager.SettingsStub>();
					}
					dictionary[obj2].Add(settingsStub);
				}
			}
			ArrayList arrayList = this.FindToolStripPanels(true, this.form.Controls);
			foreach (object obj3 in arrayList)
			{
				ToolStripPanel toolStripPanel = (ToolStripPanel)obj3;
				foreach (object obj4 in toolStripPanel.Controls)
				{
					Control control = (Control)obj4;
					control.Visible = false;
				}
				string text = toolStripPanel.Name;
				if (string.IsNullOrEmpty(text) && toolStripPanel.Parent is ToolStripContainer && !string.IsNullOrEmpty(toolStripPanel.Parent.Name))
				{
					text = toolStripPanel.Parent.Name + "." + toolStripPanel.Dock.ToString();
				}
				toolStripPanel.BeginInit();
				if (dictionary.ContainsKey(text))
				{
					List<ToolStripSettingsManager.SettingsStub> list = dictionary[text];
					if (list != null)
					{
						foreach (ToolStripSettingsManager.SettingsStub settingsStub2 in list)
						{
							if (!string.IsNullOrEmpty(settingsStub2.Name))
							{
								ToolStrip toolStrip2 = ToolStripManager.FindToolStrip(this.form, settingsStub2.Name);
								this.ApplyToolStripSettings(toolStrip2, settingsStub2, itemLocationHash);
								toolStripPanel.Join(toolStrip2, settingsStub2.Location);
							}
						}
					}
				}
				toolStripPanel.EndInit();
			}
			this.ResumeAllLayout(this.form, true);
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x001227F4 File Offset: 0x001209F4
		private void ApplyToolStripSettings(ToolStrip toolStrip, ToolStripSettingsManager.SettingsStub settings, Dictionary<string, ToolStrip> itemLocationHash)
		{
			if (toolStrip != null)
			{
				toolStrip.Visible = settings.Visible;
				toolStrip.Size = settings.Size;
				string itemOrder = settings.ItemOrder;
				if (!string.IsNullOrEmpty(itemOrder))
				{
					string[] array = itemOrder.Split(new char[]
					{
						','
					});
					Regex regex = new Regex("(\\S+)");
					int num = 0;
					while (num < toolStrip.Items.Count && num < array.Length)
					{
						Match match = regex.Match(array[num]);
						if (match != null && match.Success)
						{
							string value = match.Value;
							if (!string.IsNullOrEmpty(value) && itemLocationHash.ContainsKey(value))
							{
								toolStrip.Items.Insert(num, itemLocationHash[value].Items[value]);
							}
						}
						num++;
					}
				}
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x001228C0 File Offset: 0x00120AC0
		private Dictionary<string, ToolStrip> BuildItemOriginationHash()
		{
			ArrayList arrayList = this.FindToolStrips(true, this.form.Controls);
			Dictionary<string, ToolStrip> dictionary = new Dictionary<string, ToolStrip>();
			if (arrayList != null)
			{
				foreach (object obj in arrayList)
				{
					ToolStrip toolStrip = (ToolStrip)obj;
					foreach (object obj2 in toolStrip.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj2;
						if (!string.IsNullOrEmpty(toolStripItem.Name))
						{
							dictionary[toolStripItem.Name] = toolStrip;
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x00122998 File Offset: 0x00120B98
		private ArrayList FindControls(Type baseType, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
		{
			if (controlsToLookIn == null || foundControls == null)
			{
				return null;
			}
			try
			{
				for (int i = 0; i < controlsToLookIn.Count; i++)
				{
					if (controlsToLookIn[i] != null && baseType.IsAssignableFrom(controlsToLookIn[i].GetType()))
					{
						foundControls.Add(controlsToLookIn[i]);
					}
				}
				if (searchAllChildren)
				{
					for (int j = 0; j < controlsToLookIn.Count; j++)
					{
						if (controlsToLookIn[j] != null && !(controlsToLookIn[j] is Form) && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
						{
							foundControls = this.FindControls(baseType, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
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
			return foundControls;
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x00122A74 File Offset: 0x00120C74
		private ArrayList FindToolStripPanels(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
		{
			return this.FindControls(typeof(ToolStripPanel), true, this.form.Controls, new ArrayList());
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x00122A97 File Offset: 0x00120C97
		private ArrayList FindToolStrips(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
		{
			return this.FindControls(typeof(ToolStrip), true, this.form.Controls, new ArrayList());
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x00122ABA File Offset: 0x00120CBA
		private string GetSettingsKey(ToolStrip toolStrip)
		{
			if (toolStrip != null)
			{
				return this.formKey + "." + toolStrip.Name;
			}
			return string.Empty;
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x00122ADC File Offset: 0x00120CDC
		private void ResumeAllLayout(Control start, bool performLayout)
		{
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.ResumeAllLayout(controls[i], performLayout);
			}
			start.ResumeLayout(performLayout);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x00122B18 File Offset: 0x00120D18
		private void SuspendAllLayout(Control start)
		{
			start.SuspendLayout();
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.SuspendAllLayout(controls[i]);
			}
		}

		// Token: 0x040025B1 RID: 9649
		private Form form;

		// Token: 0x040025B2 RID: 9650
		private string formKey;

		// Token: 0x0200074F RID: 1871
		private struct SettingsStub
		{
			// Token: 0x060061EE RID: 25070 RVA: 0x00191314 File Offset: 0x0018F514
			public SettingsStub(ToolStrip toolStrip)
			{
				this.ToolStripPanelName = string.Empty;
				ToolStripPanel toolStripPanel = toolStrip.Parent as ToolStripPanel;
				if (toolStripPanel != null)
				{
					if (!string.IsNullOrEmpty(toolStripPanel.Name))
					{
						this.ToolStripPanelName = toolStripPanel.Name;
					}
					else if (toolStripPanel.Parent is ToolStripContainer && !string.IsNullOrEmpty(toolStripPanel.Parent.Name))
					{
						this.ToolStripPanelName = toolStripPanel.Parent.Name + "." + toolStripPanel.Dock.ToString();
					}
				}
				this.Visible = toolStrip.Visible;
				this.Size = toolStrip.Size;
				this.Location = toolStrip.Location;
				this.Name = toolStrip.Name;
				this.ItemOrder = ToolStripSettingsManager.GetItemOrder(toolStrip);
			}

			// Token: 0x060061EF RID: 25071 RVA: 0x001913E0 File Offset: 0x0018F5E0
			public SettingsStub(ToolStripSettings toolStripSettings)
			{
				this.ToolStripPanelName = toolStripSettings.ToolStripPanelName;
				this.Visible = toolStripSettings.Visible;
				this.Size = toolStripSettings.Size;
				this.Location = toolStripSettings.Location;
				this.Name = toolStripSettings.Name;
				this.ItemOrder = toolStripSettings.ItemOrder;
			}

			// Token: 0x040041A8 RID: 16808
			public bool Visible;

			// Token: 0x040041A9 RID: 16809
			public string ToolStripPanelName;

			// Token: 0x040041AA RID: 16810
			public Point Location;

			// Token: 0x040041AB RID: 16811
			public Size Size;

			// Token: 0x040041AC RID: 16812
			public string ItemOrder;

			// Token: 0x040041AD RID: 16813
			public string Name;
		}
	}
}
