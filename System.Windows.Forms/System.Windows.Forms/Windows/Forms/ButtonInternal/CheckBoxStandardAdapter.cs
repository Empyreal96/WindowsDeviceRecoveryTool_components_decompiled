using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004D0 RID: 1232
	internal sealed class CheckBoxStandardAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x060051DB RID: 20955 RVA: 0x00155333 File Offset: 0x00153533
		internal CheckBoxStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001559C8 File Offset: 0x00153BC8
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			if (!layoutData.options.everettButtonCompat)
			{
				layoutData.textBounds.Offset(-1, -1);
			}
			layoutData.imageBounds.Offset(-1, -1);
			base.AdjustFocusRectangle(layoutData);
			if (!AccessibilityImprovements.Level2 || !string.IsNullOrEmpty(base.Control.Text))
			{
				int num = layoutData.focus.X & 1;
				if (!Application.RenderWithVisualStyles)
				{
					num = 1 - num;
				}
				layoutData.focus.Offset(-(num + 1), -2);
				layoutData.focus.Width = layoutData.textBounds.Width + layoutData.imageBounds.Width - 1;
				layoutData.focus.Intersect(layoutData.textBounds);
				if (layoutData.options.textAlign != (ContentAlignment)273 && layoutData.options.useCompatibleTextRendering && layoutData.options.font.Italic)
				{
					ButtonBaseAdapter.LayoutData layoutData2 = layoutData;
					layoutData2.focus.Width = layoutData2.focus.Width + 2;
				}
			}
			base.PaintImage(e, layoutData);
			base.DrawCheckBox(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x00155B3A File Offset: 0x00153D3A
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.CheckState);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x00155B6A File Offset: 0x00153D6A
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.CheckState);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x00155B9C File Offset: 0x00153D9C
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonStandardAdapter buttonStandardAdapter = new ButtonStandardAdapter(base.Control);
				return buttonStandardAdapter.GetPreferredSizeCore(proposedSize);
			}
			Size preferredSizeCore;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, default(Rectangle)))
				{
					ButtonBaseAdapter.LayoutOptions layoutOptions = this.Layout(paintEventArgs);
					preferredSizeCore = layoutOptions.GetPreferredSizeCore(proposedSize);
				}
			}
			return preferredSizeCore;
		}

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x060051E0 RID: 20960 RVA: 0x00155C28 File Offset: 0x00153E28
		private new ButtonStandardAdapter ButtonAdapter
		{
			get
			{
				return (ButtonStandardAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00155C35 File Offset: 0x00153E35
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonStandardAdapter(base.Control);
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00155C44 File Offset: 0x00153E44
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkPaddingSize = 1;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			if (Application.RenderWithVisualStyles)
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					layoutOptions.checkSize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxRenderer.ConvertFromButtonState(base.GetState(), true, base.Control.MouseIsOver), base.Control.HandleInternal).Width;
					return layoutOptions;
				}
			}
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				layoutOptions.checkSize = base.Control.LogicalToDeviceUnits(layoutOptions.checkSize);
			}
			else
			{
				layoutOptions.checkSize = (int)((double)layoutOptions.checkSize * base.GetDpiScaleRatio(e.Graphics));
			}
			return layoutOptions;
		}
	}
}
