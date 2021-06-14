using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x020005FF RID: 1535
	internal sealed class InlineObject : TextEmbeddedObject
	{
		// Token: 0x06006624 RID: 26148 RVA: 0x001CB8F5 File Offset: 0x001C9AF5
		internal InlineObject(int dcp, int cch, UIElement element, TextRunProperties textProps, TextBlock host)
		{
			this._dcp = dcp;
			this._cch = cch;
			this._element = element;
			this._textProps = textProps;
			this._host = host;
		}

		// Token: 0x06006625 RID: 26149 RVA: 0x001CB924 File Offset: 0x001C9B24
		public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
		{
			Size size = this._host.MeasureChild(this);
			TextDpi.EnsureValidObjSize(ref size);
			double baseline = size.Height;
			double num = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
			if (!DoubleUtil.IsNaN(num))
			{
				baseline = num;
			}
			return new TextEmbeddedObjectMetrics(size.Width, size.Height, baseline);
		}

		// Token: 0x06006626 RID: 26150 RVA: 0x001CB984 File Offset: 0x001C9B84
		public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
		{
			if (this._element.IsArrangeValid)
			{
				Size desiredSize = this._element.DesiredSize;
				double num = (!sideways) ? desiredSize.Height : desiredSize.Width;
				double num2 = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
				if (!sideways && !DoubleUtil.IsNaN(num2))
				{
					num = num2;
				}
				return new Rect(0.0, -num, sideways ? desiredSize.Height : desiredSize.Width, sideways ? desiredSize.Width : desiredSize.Height);
			}
			return Rect.Empty;
		}

		// Token: 0x06006627 RID: 26151 RVA: 0x00002137 File Offset: 0x00000337
		public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
		{
		}

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x001CBA1E File Offset: 0x001C9C1E
		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(string.Empty, 0);
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x001CBA2B File Offset: 0x001C9C2B
		public override int Length
		{
			get
			{
				return this._cch;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x0600662A RID: 26154 RVA: 0x001CBA33 File Offset: 0x001C9C33
		public override TextRunProperties Properties
		{
			get
			{
				return this._textProps;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x0600662B RID: 26155 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x0600662C RID: 26156 RVA: 0x0000B02A File Offset: 0x0000922A
		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x0600662D RID: 26157 RVA: 0x00016748 File Offset: 0x00014948
		public override bool HasFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x0600662E RID: 26158 RVA: 0x001CBA3B File Offset: 0x001C9C3B
		internal int Dcp
		{
			get
			{
				return this._dcp;
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600662F RID: 26159 RVA: 0x001CBA43 File Offset: 0x001C9C43
		internal UIElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x040032F4 RID: 13044
		private readonly int _dcp;

		// Token: 0x040032F5 RID: 13045
		private readonly int _cch;

		// Token: 0x040032F6 RID: 13046
		private readonly UIElement _element;

		// Token: 0x040032F7 RID: 13047
		private readonly TextRunProperties _textProps;

		// Token: 0x040032F8 RID: 13048
		private readonly TextBlock _host;
	}
}
