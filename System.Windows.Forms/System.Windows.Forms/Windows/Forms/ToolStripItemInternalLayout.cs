using System;
using System.Drawing;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003BC RID: 956
	internal class ToolStripItemInternalLayout
	{
		// Token: 0x0600401B RID: 16411 RVA: 0x00115797 File Offset: 0x00113997
		public ToolStripItemInternalLayout(ToolStripItem ownerItem)
		{
			if (ownerItem == null)
			{
				throw new ArgumentNullException("ownerItem");
			}
			this.ownerItem = ownerItem;
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x001157BF File Offset: 0x001139BF
		protected virtual ToolStripItem Owner
		{
			get
			{
				return this.ownerItem;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x0600401D RID: 16413 RVA: 0x001157C8 File Offset: 0x001139C8
		public virtual Rectangle ImageRectangle
		{
			get
			{
				Rectangle imageBounds = this.LayoutData.imageBounds;
				imageBounds.Intersect(this.layoutData.field);
				return imageBounds;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x001157F4 File Offset: 0x001139F4
		internal ButtonBaseAdapter.LayoutData LayoutData
		{
			get
			{
				this.EnsureLayout();
				return this.layoutData;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x0600401F RID: 16415 RVA: 0x00115803 File Offset: 0x00113A03
		public Size PreferredImageSize
		{
			get
			{
				return this.Owner.PreferredImageSize;
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x00115810 File Offset: 0x00113A10
		protected virtual ToolStrip ParentInternal
		{
			get
			{
				if (this.ownerItem == null)
				{
					return null;
				}
				return this.ownerItem.ParentInternal;
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x00115828 File Offset: 0x00113A28
		public virtual Rectangle TextRectangle
		{
			get
			{
				Rectangle textBounds = this.LayoutData.textBounds;
				textBounds.Intersect(this.layoutData.field);
				return textBounds;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x00115854 File Offset: 0x00113A54
		public virtual Rectangle ContentRectangle
		{
			get
			{
				return this.LayoutData.field;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x00115861 File Offset: 0x00113A61
		public virtual TextFormatFlags TextFormat
		{
			get
			{
				if (this.currentLayoutOptions != null)
				{
					return this.currentLayoutOptions.gdiTextFormatFlags;
				}
				return this.CommonLayoutOptions().gdiTextFormatFlags;
			}
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00115884 File Offset: 0x00113A84
		internal static TextFormatFlags ContentAlignToTextFormat(ContentAlignment alignment, bool rightToLeft)
		{
			TextFormatFlags textFormatFlags = TextFormatFlags.Default;
			if (rightToLeft)
			{
				textFormatFlags |= TextFormatFlags.RightToLeft;
			}
			textFormatFlags |= ControlPaint.TranslateAlignmentForGDI(alignment);
			return textFormatFlags | ControlPaint.TranslateLineAlignmentForGDI(alignment);
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x001158B4 File Offset: 0x00113AB4
		protected virtual ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
		{
			ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = new ToolStripItemInternalLayout.ToolStripItemLayoutOptions();
			Rectangle client = new Rectangle(Point.Empty, this.ownerItem.Size);
			toolStripItemLayoutOptions.client = client;
			toolStripItemLayoutOptions.growBorderBy1PxWhenDefault = false;
			toolStripItemLayoutOptions.borderSize = 2;
			toolStripItemLayoutOptions.paddingSize = 0;
			toolStripItemLayoutOptions.maxFocus = true;
			toolStripItemLayoutOptions.focusOddEvenFixup = false;
			toolStripItemLayoutOptions.font = this.ownerItem.Font;
			toolStripItemLayoutOptions.text = (((this.Owner.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text) ? this.Owner.Text : string.Empty);
			toolStripItemLayoutOptions.imageSize = this.PreferredImageSize;
			toolStripItemLayoutOptions.checkSize = 0;
			toolStripItemLayoutOptions.checkPaddingSize = 0;
			toolStripItemLayoutOptions.checkAlign = ContentAlignment.TopLeft;
			toolStripItemLayoutOptions.imageAlign = this.Owner.ImageAlign;
			toolStripItemLayoutOptions.textAlign = this.Owner.TextAlign;
			toolStripItemLayoutOptions.hintTextUp = false;
			toolStripItemLayoutOptions.shadowedText = !this.ownerItem.Enabled;
			toolStripItemLayoutOptions.layoutRTL = (RightToLeft.Yes == this.Owner.RightToLeft);
			toolStripItemLayoutOptions.textImageRelation = this.Owner.TextImageRelation;
			toolStripItemLayoutOptions.textImageInset = 0;
			toolStripItemLayoutOptions.everettButtonCompat = false;
			toolStripItemLayoutOptions.gdiTextFormatFlags = ToolStripItemInternalLayout.ContentAlignToTextFormat(this.Owner.TextAlign, this.Owner.RightToLeft == RightToLeft.Yes);
			toolStripItemLayoutOptions.gdiTextFormatFlags = (this.Owner.ShowKeyboardCues ? toolStripItemLayoutOptions.gdiTextFormatFlags : (toolStripItemLayoutOptions.gdiTextFormatFlags | TextFormatFlags.HidePrefix));
			return toolStripItemLayoutOptions;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00115A1E File Offset: 0x00113C1E
		private bool EnsureLayout()
		{
			if (this.layoutData == null || this.parentLayoutData == null || !this.parentLayoutData.IsCurrent(this.ParentInternal))
			{
				this.PerformLayout();
				return true;
			}
			return false;
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00115A4C File Offset: 0x00113C4C
		private ButtonBaseAdapter.LayoutData GetLayoutData()
		{
			this.currentLayoutOptions = this.CommonLayoutOptions();
			if (this.Owner.TextDirection != ToolStripTextDirection.Horizontal)
			{
				this.currentLayoutOptions.verticalText = true;
			}
			return this.currentLayoutOptions.Layout();
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00115A8C File Offset: 0x00113C8C
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			this.EnsureLayout();
			if (this.ownerItem != null)
			{
				this.lastPreferredSize = this.currentLayoutOptions.GetPreferredSizeCore(constrainingSize);
				return this.lastPreferredSize;
			}
			return Size.Empty;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00115ACC File Offset: 0x00113CCC
		internal void PerformLayout()
		{
			this.layoutData = this.GetLayoutData();
			ToolStrip parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				this.parentLayoutData = new ToolStripItemInternalLayout.ToolStripLayoutData(parentInternal);
				return;
			}
			this.parentLayoutData = null;
		}

		// Token: 0x04002489 RID: 9353
		private ToolStripItemInternalLayout.ToolStripItemLayoutOptions currentLayoutOptions;

		// Token: 0x0400248A RID: 9354
		private ToolStripItem ownerItem;

		// Token: 0x0400248B RID: 9355
		private ButtonBaseAdapter.LayoutData layoutData;

		// Token: 0x0400248C RID: 9356
		private const int BORDER_WIDTH = 2;

		// Token: 0x0400248D RID: 9357
		private const int BORDER_HEIGHT = 3;

		// Token: 0x0400248E RID: 9358
		private static readonly Size INVALID_SIZE = new Size(int.MinValue, int.MinValue);

		// Token: 0x0400248F RID: 9359
		private Size lastPreferredSize = ToolStripItemInternalLayout.INVALID_SIZE;

		// Token: 0x04002490 RID: 9360
		private ToolStripItemInternalLayout.ToolStripLayoutData parentLayoutData;

		// Token: 0x0200073A RID: 1850
		internal class ToolStripItemLayoutOptions : ButtonBaseAdapter.LayoutOptions
		{
			// Token: 0x06006132 RID: 24882 RVA: 0x0018DD70 File Offset: 0x0018BF70
			protected override Size GetTextSize(Size proposedConstraints)
			{
				if (this.cachedSize != LayoutUtils.InvalidSize && (this.cachedProposedConstraints == proposedConstraints || this.cachedSize.Width <= proposedConstraints.Width))
				{
					return this.cachedSize;
				}
				this.cachedSize = base.GetTextSize(proposedConstraints);
				this.cachedProposedConstraints = proposedConstraints;
				return this.cachedSize;
			}

			// Token: 0x04004181 RID: 16769
			private Size cachedSize = LayoutUtils.InvalidSize;

			// Token: 0x04004182 RID: 16770
			private Size cachedProposedConstraints = LayoutUtils.InvalidSize;
		}

		// Token: 0x0200073B RID: 1851
		private class ToolStripLayoutData
		{
			// Token: 0x06006134 RID: 24884 RVA: 0x0018DDF0 File Offset: 0x0018BFF0
			public ToolStripLayoutData(ToolStrip toolStrip)
			{
				this.layoutStyle = toolStrip.LayoutStyle;
				this.autoSize = toolStrip.AutoSize;
				this.size = toolStrip.Size;
			}

			// Token: 0x06006135 RID: 24885 RVA: 0x0018DE1C File Offset: 0x0018C01C
			public bool IsCurrent(ToolStrip toolStrip)
			{
				return toolStrip != null && (toolStrip.Size == this.size && toolStrip.LayoutStyle == this.layoutStyle) && toolStrip.AutoSize == this.autoSize;
			}

			// Token: 0x04004183 RID: 16771
			private ToolStripLayoutStyle layoutStyle;

			// Token: 0x04004184 RID: 16772
			private bool autoSize;

			// Token: 0x04004185 RID: 16773
			private Size size;
		}
	}
}
