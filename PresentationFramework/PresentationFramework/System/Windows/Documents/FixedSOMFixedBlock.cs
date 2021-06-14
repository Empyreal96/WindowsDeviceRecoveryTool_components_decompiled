using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000359 RID: 857
	internal sealed class FixedSOMFixedBlock : FixedSOMPageElement
	{
		// Token: 0x06002DB5 RID: 11701 RVA: 0x000CDB53 File Offset: 0x000CBD53
		public FixedSOMFixedBlock(FixedSOMPage page) : base(page)
		{
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000CDB5C File Offset: 0x000CBD5C
		public double LineHeight
		{
			get
			{
				FixedSOMTextRun lastTextRun = this.LastTextRun;
				if (lastTextRun != null)
				{
					if (base.SemanticBoxes.Count > 1)
					{
						FixedSOMTextRun fixedSOMTextRun = base.SemanticBoxes[base.SemanticBoxes.Count - 2] as FixedSOMTextRun;
						if (fixedSOMTextRun != null && lastTextRun.BoundingRect.Height / fixedSOMTextRun.BoundingRect.Height < 0.75 && fixedSOMTextRun.BoundingRect.Left != lastTextRun.BoundingRect.Left && fixedSOMTextRun.BoundingRect.Right != lastTextRun.BoundingRect.Right && fixedSOMTextRun.BoundingRect.Top != lastTextRun.BoundingRect.Top && fixedSOMTextRun.BoundingRect.Bottom != lastTextRun.BoundingRect.Bottom)
						{
							return fixedSOMTextRun.BoundingRect.Height;
						}
					}
					return lastTextRun.BoundingRect.Height;
				}
				return 0.0;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x000CDC75 File Offset: 0x000CBE75
		public bool IsFloatingImage
		{
			get
			{
				return this._semanticBoxes.Count == 1 && this._semanticBoxes[0] is FixedSOMImage;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000CDC9B File Offset: 0x000CBE9B
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[1];
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000CDCA4 File Offset: 0x000CBEA4
		public bool IsWhiteSpace
		{
			get
			{
				if (this._semanticBoxes.Count == 0)
				{
					return false;
				}
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
				{
					FixedSOMTextRun fixedSOMTextRun = fixedSOMSemanticBox as FixedSOMTextRun;
					if (fixedSOMTextRun == null || !fixedSOMTextRun.IsWhiteSpace)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002DBA RID: 11706 RVA: 0x000CDD18 File Offset: 0x000CBF18
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000CDD28 File Offset: 0x000CBF28
		public Matrix Matrix
		{
			get
			{
				return this._matrix;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x000CDD30 File Offset: 0x000CBF30
		private FixedSOMTextRun LastTextRun
		{
			get
			{
				FixedSOMTextRun fixedSOMTextRun = null;
				int num = this._semanticBoxes.Count - 1;
				while (num >= 0 && fixedSOMTextRun == null)
				{
					fixedSOMTextRun = (this._semanticBoxes[num] as FixedSOMTextRun);
					num--;
				}
				return fixedSOMTextRun;
			}
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000CDD70 File Offset: 0x000CBF70
		public void CombineWith(FixedSOMFixedBlock block)
		{
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in block.SemanticBoxes)
			{
				FixedSOMTextRun fixedSOMTextRun = fixedSOMSemanticBox as FixedSOMTextRun;
				if (fixedSOMTextRun != null)
				{
					this.AddTextRun(fixedSOMTextRun);
				}
				else
				{
					base.Add(fixedSOMSemanticBox);
				}
			}
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000CDDD8 File Offset: 0x000CBFD8
		public void AddTextRun(FixedSOMTextRun textRun)
		{
			this._AddElement(textRun);
			textRun.FixedBlock = this;
			if (!textRun.IsWhiteSpace)
			{
				if (textRun.IsLTR)
				{
					this._LTRCount++;
					return;
				}
				this._RTLCount++;
			}
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000CDE15 File Offset: 0x000CC015
		public void AddImage(FixedSOMImage image)
		{
			this._AddElement(image);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000CDE1E File Offset: 0x000CC01E
		public override void SetRTFProperties(FixedElement element)
		{
			if (this.IsRTL)
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000CDE3C File Offset: 0x000CC03C
		private void _AddElement(FixedSOMElement element)
		{
			base.Add(element);
			if (this._semanticBoxes.Count == 1)
			{
				this._matrix = element.Matrix;
				this._matrix.OffsetX = 0.0;
				this._matrix.OffsetY = 0.0;
			}
		}

		// Token: 0x04001DC2 RID: 7618
		private int _RTLCount;

		// Token: 0x04001DC3 RID: 7619
		private int _LTRCount;

		// Token: 0x04001DC4 RID: 7620
		private Matrix _matrix;
	}
}
