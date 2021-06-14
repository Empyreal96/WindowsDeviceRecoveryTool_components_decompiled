using System;
using System.Drawing;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200047E RID: 1150
	internal sealed class DropDownButton : Button
	{
		// Token: 0x06004D58 RID: 19800 RVA: 0x0013D2F5 File Offset: 0x0013B4F5
		public DropDownButton()
		{
			base.SetStyle(ControlStyles.Selectable, true);
			this.SetAccessibleName();
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06004D59 RID: 19801 RVA: 0x0013D30F File Offset: 0x0013B50F
		// (set) Token: 0x06004D5A RID: 19802 RVA: 0x0013D317 File Offset: 0x0013B517
		public bool IgnoreMouse
		{
			get
			{
				return this.ignoreMouse;
			}
			set
			{
				this.ignoreMouse = value;
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06004D5B RID: 19803 RVA: 0x000A010F File Offset: 0x0009E30F
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3;
			}
		}

		// Token: 0x17001319 RID: 4889
		// (set) Token: 0x06004D5C RID: 19804 RVA: 0x0013D320 File Offset: 0x0013B520
		public bool UseComboBoxTheme
		{
			set
			{
				if (this.useComboBoxTheme != value)
				{
					this.useComboBoxTheme = value;
					if (AccessibilityImprovements.Level1)
					{
						this.SetAccessibleName();
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0013D345 File Offset: 0x0013B545
		protected override void OnClick(EventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnClick(e);
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0013D356 File Offset: 0x0013B556
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnMouseUp(e);
			}
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0013D367 File Offset: 0x0013B567
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnMouseDown(e);
			}
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0013D378 File Offset: 0x0013B578
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			if (Application.RenderWithVisualStyles & this.useComboBoxTheme)
			{
				ComboBoxState comboBoxState = ComboBoxState.Normal;
				if (base.MouseIsDown)
				{
					comboBoxState = ComboBoxState.Pressed;
				}
				else if (base.MouseIsOver)
				{
					comboBoxState = ComboBoxState.Hot;
				}
				Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
				if (comboBoxState == ComboBoxState.Normal)
				{
					pevent.Graphics.FillRectangle(SystemBrushes.Window, rectangle);
				}
				if (!DpiHelper.EnableDpiChangedHighDpiImprovements)
				{
					ComboBoxRenderer.DrawDropDownButton(pevent.Graphics, rectangle, comboBoxState);
				}
				else
				{
					ComboBoxRenderer.DrawDropDownButtonForHandle(pevent.Graphics, rectangle, comboBoxState, base.HandleInternal);
				}
				if (AccessibilityImprovements.Level1 && this.Focused)
				{
					rectangle.Inflate(-1, -1);
					ControlPaint.DrawFocusRectangle(pevent.Graphics, rectangle, this.ForeColor, this.BackColor);
				}
			}
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x0013D438 File Offset: 0x0013B638
		internal void PerformButtonClick()
		{
			if (base.Visible && base.Enabled)
			{
				this.OnClick(EventArgs.Empty);
			}
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x0013D455 File Offset: 0x0013B655
		private void SetAccessibleName()
		{
			if (AccessibilityImprovements.Level1 && this.useComboBoxTheme)
			{
				base.AccessibleName = SR.GetString("PropertyGridDropDownButtonComboBoxAccessibleName");
				return;
			}
			base.AccessibleName = SR.GetString("PropertyGridDropDownButtonAccessibleName");
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x0013D487 File Offset: 0x0013B687
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new DropDownButtonAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x0013D49D File Offset: 0x0013B69D
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new DropDownButtonAdapter(this);
		}

		// Token: 0x040032F4 RID: 13044
		private bool useComboBoxTheme;

		// Token: 0x040032F5 RID: 13045
		private bool ignoreMouse;
	}
}
