using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000489 RID: 1161
	internal class GridToolTip : Control
	{
		// Token: 0x06004E3D RID: 20029 RVA: 0x00140FAC File Offset: 0x0013F1AC
		internal GridToolTip(Control[] controls)
		{
			this.controls = controls;
			base.SetStyle(ControlStyles.UserPaint, false);
			this.Font = controls[0].Font;
			this.toolInfos = new NativeMethods.TOOLINFO_T[controls.Length];
			for (int i = 0; i < controls.Length; i++)
			{
				controls[i].HandleCreated += this.OnControlCreateHandle;
				controls[i].HandleDestroyed += this.OnControlDestroyHandle;
				if (controls[i].IsHandleCreated)
				{
					this.SetupToolTip(controls[i]);
				}
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x00141049 File Offset: 0x0013F249
		// (set) Token: 0x06004E3F RID: 20031 RVA: 0x00141054 File Offset: 0x0013F254
		public string ToolTip
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (base.IsHandleCreated || !string.IsNullOrEmpty(value))
				{
					this.Reset();
				}
				if (value != null && value.Length > this.maximumToolTipLength)
				{
					value = value.Substring(0, this.maximumToolTipLength) + "...";
				}
				this.toolTipText = value;
				if (base.IsHandleCreated)
				{
					bool visible = base.Visible;
					if (visible)
					{
						base.Visible = false;
					}
					if (value == null || value.Length == 0)
					{
						this.dontShow = true;
						value = "";
					}
					else
					{
						this.dontShow = false;
					}
					for (int i = 0; i < this.controls.Length; i++)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_UPDATETIPTEXT, 0, this.GetTOOLINFO(this.controls[i]));
					}
					if (visible && !this.dontShow)
					{
						base.Visible = true;
					}
				}
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06004E40 RID: 20032 RVA: 0x00141130 File Offset: 0x0013F330
		protected override CreateParams CreateParams
		{
			get
			{
				SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
				{
					dwICC = 8
				});
				CreateParams createParams = new CreateParams();
				createParams.Parent = IntPtr.Zero;
				createParams.ClassName = "tooltips_class32";
				createParams.Style |= 3;
				createParams.ExStyle = 0;
				createParams.Caption = this.ToolTip;
				return createParams;
			}
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x00141190 File Offset: 0x0013F390
		private NativeMethods.TOOLINFO_T GetTOOLINFO(Control c)
		{
			int num = Array.IndexOf<Control>(this.controls, c);
			if (this.toolInfos[num] == null)
			{
				this.toolInfos[num] = new NativeMethods.TOOLINFO_T();
				this.toolInfos[num].cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				this.toolInfos[num].uFlags |= 273;
			}
			this.toolInfos[num].lpszText = this.toolTipText;
			this.toolInfos[num].hwnd = c.Handle;
			this.toolInfos[num].uId = c.Handle;
			return this.toolInfos[num];
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x00141237 File Offset: 0x0013F437
		private void OnControlCreateHandle(object sender, EventArgs e)
		{
			this.SetupToolTip((Control)sender);
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x00141245 File Offset: 0x0013F445
		private void OnControlDestroyHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetTOOLINFO((Control)sender));
			}
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x00141274 File Offset: 0x0013F474
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			for (int i = 0; i < this.controls.Length; i++)
			{
				if (this.controls[i].IsHandleCreated)
				{
					this.SetupToolTip(this.controls[i]);
				}
			}
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x001412B8 File Offset: 0x0013F4B8
		internal void PositionToolTip(Control parent, Rectangle itemRect)
		{
			if (this._positioned && DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				return;
			}
			base.Visible = false;
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height);
			base.SendMessage(1055, 1, ref rect);
			Point location = parent.PointToScreen(new Point(rect.left, rect.top));
			base.Location = location;
			int num = base.Location.X + base.Size.Width - SystemInformation.VirtualScreen.Width;
			if (num > 0)
			{
				location.X -= num;
				base.Location = location;
			}
			base.Visible = true;
			this._positioned = true;
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x00141384 File Offset: 0x0013F584
		private void SetupToolTip(Control c)
		{
			if (base.IsHandleCreated)
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(c));
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			}
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x00141400 File Offset: 0x0013F600
		public void Reset()
		{
			string toolTip = this.ToolTip;
			this.toolTipText = "";
			for (int i = 0; i < this.controls.Length; i++)
			{
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TTM_UPDATETIPTEXT, 0, this.GetTOOLINFO(this.controls[i]));
			}
			this.toolTipText = toolTip;
			base.SendMessage(1053, 0, 0);
			this._positioned = false;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x0014147C File Offset: 0x0013F67C
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 != 24)
			{
				if (msg2 == 132)
				{
					msg.Result = (IntPtr)(-1);
					return;
				}
			}
			else if ((int)((long)msg.WParam) != 0 && this.dontShow)
			{
				msg.WParam = IntPtr.Zero;
			}
			base.WndProc(ref msg);
		}

		// Token: 0x04003341 RID: 13121
		private Control[] controls;

		// Token: 0x04003342 RID: 13122
		private string toolTipText;

		// Token: 0x04003343 RID: 13123
		private NativeMethods.TOOLINFO_T[] toolInfos;

		// Token: 0x04003344 RID: 13124
		private bool dontShow;

		// Token: 0x04003345 RID: 13125
		private Point lastMouseMove = Point.Empty;

		// Token: 0x04003346 RID: 13126
		private int maximumToolTipLength = 1000;

		// Token: 0x04003347 RID: 13127
		private bool _positioned;
	}
}
