using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004CE RID: 1230
	internal class CheckBoxFlatAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x060051CB RID: 20939 RVA: 0x00155333 File Offset: 0x00153533
		internal CheckBoxFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x0015533C File Offset: 0x0015353C
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x001553C4 File Offset: 0x001535C4
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.lowHighlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x0015544C File Offset: 0x0015364C
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x001554D4 File Offset: 0x001536D4
		private void PaintFlatWorker(PaintEventArgs e, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckFlat(e, layout, checkColor, colors.options.highContrast ? colors.buttonFace : checkBackground, checkBorder, colors);
			base.AdjustFocusRectangle(layout);
			base.PaintField(e, layout, colors, checkColor, true);
		}

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x00155548 File Offset: 0x00153748
		private new ButtonFlatAdapter ButtonAdapter
		{
			get
			{
				return (ButtonFlatAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00155555 File Offset: 0x00153755
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonFlatAdapter(base.Control);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00155564 File Offset: 0x00153764
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkSize = (int)(11.0 * base.GetDpiScaleRatio(e.Graphics));
			layoutOptions.shadowedText = false;
			return layoutOptions;
		}
	}
}
