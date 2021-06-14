using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004D4 RID: 1236
	internal class RadioButtonStandardAdapter : RadioButtonBaseAdapter
	{
		// Token: 0x060051FC RID: 20988 RVA: 0x001564EA File Offset: 0x001546EA
		internal RadioButtonStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x00156A48 File Offset: 0x00154C48
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckBox(e, layout);
			base.AdjustFocusRectangle(layout);
			base.PaintField(e, layout, colorData, colorData.windowText, true);
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x00156ADA File Offset: 0x00154CDA
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x00156B10 File Offset: 0x00154D10
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x00155C28 File Offset: 0x00153E28
		private new ButtonStandardAdapter ButtonAdapter
		{
			get
			{
				return (ButtonStandardAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x00156B46 File Offset: 0x00154D46
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonStandardAdapter(base.Control);
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x00156B54 File Offset: 0x00154D54
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.hintTextUp = false;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			if (Application.RenderWithVisualStyles)
			{
				ButtonBase control = base.Control;
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					layoutOptions.checkSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonRenderer.ConvertFromButtonState(base.GetState(), control.MouseIsOver), control.HandleInternal).Width;
					return layoutOptions;
				}
			}
			layoutOptions.checkSize = (int)((double)layoutOptions.checkSize * base.GetDpiScaleRatio(e.Graphics));
			return layoutOptions;
		}
	}
}
