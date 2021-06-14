using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200048A RID: 1162
	internal class HotCommands : PropertyGrid.SnappableControl
	{
		// Token: 0x06004E49 RID: 20041 RVA: 0x001414D6 File Offset: 0x0013F6D6
		internal HotCommands(PropertyGrid owner) : base(owner)
		{
			this.Text = "Command Pane";
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x001414F8 File Offset: 0x0013F6F8
		// (set) Token: 0x06004E4B RID: 20043 RVA: 0x00141500 File Offset: 0x0013F700
		public virtual bool AllowVisible
		{
			get
			{
				return this.allowVisible;
			}
			set
			{
				if (this.allowVisible != value)
				{
					this.allowVisible = value;
					if (value && this.WouldBeVisible)
					{
						base.Visible = true;
						return;
					}
					base.Visible = false;
				}
			}
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x0014152C File Offset: 0x0013F72C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new HotCommandsAccessibleObject(this, this.ownerGrid);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x00141548 File Offset: 0x0013F748
		public override Rectangle DisplayRectangle
		{
			get
			{
				Size clientSize = base.ClientSize;
				return new Rectangle(4, 4, clientSize.Width - 8, clientSize.Height - 8);
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06004E4E RID: 20046 RVA: 0x00141578 File Offset: 0x0013F778
		public LinkLabel Label
		{
			get
			{
				if (this.label == null)
				{
					this.label = new LinkLabel();
					this.label.Dock = DockStyle.Fill;
					this.label.LinkBehavior = LinkBehavior.AlwaysUnderline;
					this.label.DisabledLinkColor = SystemColors.ControlDark;
					this.label.LinkClicked += this.LinkClicked;
					base.Controls.Add(this.label);
				}
				return this.label;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x001415EE File Offset: 0x0013F7EE
		public virtual bool WouldBeVisible
		{
			get
			{
				return this.component != null;
			}
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001415FC File Offset: 0x0013F7FC
		public override int GetOptimalHeight(int width)
		{
			if (this.optimalHeight == -1)
			{
				int num = (int)(1.5 * (double)this.Font.Height);
				int num2 = 0;
				if (this.verbs != null)
				{
					num2 = this.verbs.Length;
				}
				this.optimalHeight = num2 * num + 8;
			}
			return this.optimalHeight;
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x00012E22 File Offset: 0x00011022
		public override int SnapHeightRequest(int request)
		{
			return request;
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x000A010F File Offset: 0x0009E30F
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3;
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x00141650 File Offset: 0x0013F850
		private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if (e.Link.Enabled)
				{
					((DesignerVerb)e.Link.LinkData).Invoke();
				}
			}
			catch (Exception ex)
			{
				RTLAwareMessageBox.Show(this, ex.Message, SR.GetString("PBRSErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			}
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001416B4 File Offset: 0x0013F8B4
		private void OnCommandChanged(object sender, EventArgs e)
		{
			this.SetupLabel();
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x001416BC File Offset: 0x0013F8BC
		protected override void OnGotFocus(EventArgs e)
		{
			this.Label.FocusInternal();
			this.Label.Invalidate();
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x001416D5 File Offset: 0x0013F8D5
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.optimalHeight = -1;
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x001416E8 File Offset: 0x0013F8E8
		internal void SetColors(Color background, Color normalText, Color link, Color activeLink, Color visitedLink, Color disabledLink)
		{
			this.Label.BackColor = background;
			this.Label.ForeColor = normalText;
			this.Label.LinkColor = link;
			this.Label.ActiveLinkColor = activeLink;
			this.Label.VisitedLinkColor = visitedLink;
			this.Label.DisabledLinkColor = disabledLink;
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x00141740 File Offset: 0x0013F940
		public void Select(bool forward)
		{
			this.Label.FocusInternal();
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x00141750 File Offset: 0x0013F950
		public virtual void SetVerbs(object component, DesignerVerb[] verbs)
		{
			if (this.verbs != null)
			{
				for (int i = 0; i < this.verbs.Length; i++)
				{
					this.verbs[i].CommandChanged -= this.OnCommandChanged;
				}
				this.component = null;
				this.verbs = null;
			}
			if (component == null || verbs == null || verbs.Length == 0)
			{
				base.Visible = false;
				this.Label.Links.Clear();
				this.Label.Text = null;
			}
			else
			{
				this.component = component;
				this.verbs = verbs;
				for (int j = 0; j < verbs.Length; j++)
				{
					verbs[j].CommandChanged += this.OnCommandChanged;
				}
				if (this.allowVisible)
				{
					base.Visible = true;
				}
				this.SetupLabel();
			}
			this.optimalHeight = -1;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0014181C File Offset: 0x0013FA1C
		private void SetupLabel()
		{
			this.Label.Links.Clear();
			StringBuilder stringBuilder = new StringBuilder();
			Point[] array = new Point[this.verbs.Length];
			int num = 0;
			bool flag = true;
			for (int i = 0; i < this.verbs.Length; i++)
			{
				if (this.verbs[i].Visible && this.verbs[i].Supported)
				{
					if (!flag)
					{
						stringBuilder.Append(Application.CurrentCulture.TextInfo.ListSeparator);
						stringBuilder.Append(" ");
						num += 2;
					}
					string text = this.verbs[i].Text;
					array[i] = new Point(num, text.Length);
					stringBuilder.Append(text);
					num += text.Length;
					flag = false;
				}
			}
			this.Label.Text = stringBuilder.ToString();
			for (int j = 0; j < this.verbs.Length; j++)
			{
				if (this.verbs[j].Visible && this.verbs[j].Supported)
				{
					LinkLabel.Link link = this.Label.Links.Add(array[j].X, array[j].Y, this.verbs[j]);
					if (!this.verbs[j].Enabled)
					{
						link.Enabled = false;
					}
				}
			}
		}

		// Token: 0x04003348 RID: 13128
		private object component;

		// Token: 0x04003349 RID: 13129
		private DesignerVerb[] verbs;

		// Token: 0x0400334A RID: 13130
		private LinkLabel label;

		// Token: 0x0400334B RID: 13131
		private bool allowVisible = true;

		// Token: 0x0400334C RID: 13132
		private int optimalHeight = -1;
	}
}
