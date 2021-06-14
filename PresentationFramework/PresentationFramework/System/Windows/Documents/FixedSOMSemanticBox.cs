using System;

namespace System.Windows.Documents
{
	// Token: 0x02000362 RID: 866
	internal abstract class FixedSOMSemanticBox : IComparable
	{
		// Token: 0x06002E0B RID: 11787 RVA: 0x000D0079 File Offset: 0x000CE279
		public FixedSOMSemanticBox()
		{
			this._boundingRect = Rect.Empty;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000D008C File Offset: 0x000CE28C
		public FixedSOMSemanticBox(Rect boundingRect)
		{
			this._boundingRect = boundingRect;
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06002E0D RID: 11789 RVA: 0x000D009B File Offset: 0x000CE29B
		// (set) Token: 0x06002E0E RID: 11790 RVA: 0x000D00A3 File Offset: 0x000CE2A3
		public Rect BoundingRect
		{
			get
			{
				return this._boundingRect;
			}
			set
			{
				this._boundingRect = value;
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x00002137 File Offset: 0x00000337
		public virtual void SetRTFProperties(FixedElement element)
		{
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000D00AC File Offset: 0x000CE2AC
		public int CompareTo(object o)
		{
			if (!(o is FixedSOMSemanticBox))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedSOMSemanticBox)
				}), "o");
			}
			FixedSOMSemanticBox.SpatialComparison spatialComparison = this._CompareHorizontal(o as FixedSOMSemanticBox, false);
			FixedSOMSemanticBox.SpatialComparison spatialComparison2 = this._CompareVertical(o as FixedSOMSemanticBox);
			int result;
			if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Equal && spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				result = 0;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Before || spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before || spatialComparison == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
			{
				result = -1;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
			{
				result = 1;
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Before)
			{
				result = -1;
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.After)
			{
				result = 1;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
			{
				result = -1;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000D016C File Offset: 0x000CE36C
		int IComparable.CompareTo(object o)
		{
			return this.CompareTo(o);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000D0178 File Offset: 0x000CE378
		protected FixedSOMSemanticBox.SpatialComparison _CompareHorizontal(FixedSOMSemanticBox otherBox, bool RTL)
		{
			Rect boundingRect = this.BoundingRect;
			Rect boundingRect2 = otherBox.BoundingRect;
			double num = RTL ? boundingRect.Right : boundingRect.Left;
			double num2 = RTL ? boundingRect2.Right : boundingRect2.Left;
			FixedSOMSemanticBox.SpatialComparison spatialComparison;
			if (num == num2)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.Equal;
			}
			else if (boundingRect.Right < boundingRect2.Left)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.Before;
			}
			else if (boundingRect2.Right < boundingRect.Left)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.After;
			}
			else
			{
				double num3 = Math.Abs(num - num2);
				double num4 = (boundingRect.Width > boundingRect2.Width) ? boundingRect.Width : boundingRect2.Width;
				if (num3 / num4 < 0.1)
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.Equal;
				}
				else if (boundingRect.Left < boundingRect2.Left)
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
				}
				else
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
				}
			}
			if (RTL && spatialComparison != FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				spatialComparison = this._InvertSpatialComparison(spatialComparison);
			}
			return spatialComparison;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000D0258 File Offset: 0x000CE458
		protected FixedSOMSemanticBox.SpatialComparison _CompareVertical(FixedSOMSemanticBox otherBox)
		{
			Rect boundingRect = this.BoundingRect;
			Rect boundingRect2 = otherBox.BoundingRect;
			FixedSOMSemanticBox.SpatialComparison result;
			if (boundingRect.Top == boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.Equal;
			}
			else if (boundingRect.Bottom <= boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.Before;
			}
			else if (boundingRect2.Bottom <= boundingRect.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.After;
			}
			else if (boundingRect.Top < boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
			}
			else
			{
				result = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
			}
			return result;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000D02C8 File Offset: 0x000CE4C8
		protected int _SpatialToAbsoluteComparison(FixedSOMSemanticBox.SpatialComparison comparison)
		{
			int result = 0;
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				result = -1;
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				result = 0;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000D0300 File Offset: 0x000CE500
		protected FixedSOMSemanticBox.SpatialComparison _InvertSpatialComparison(FixedSOMSemanticBox.SpatialComparison comparison)
		{
			FixedSOMSemanticBox.SpatialComparison result = comparison;
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				result = FixedSOMSemanticBox.SpatialComparison.After;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				result = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				result = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = FixedSOMSemanticBox.SpatialComparison.Before;
				break;
			}
			return result;
		}

		// Token: 0x04001DE8 RID: 7656
		protected Rect _boundingRect;

		// Token: 0x020008D2 RID: 2258
		protected enum SpatialComparison
		{
			// Token: 0x04004259 RID: 16985
			None,
			// Token: 0x0400425A RID: 16986
			Before,
			// Token: 0x0400425B RID: 16987
			OverlapBefore,
			// Token: 0x0400425C RID: 16988
			Equal,
			// Token: 0x0400425D RID: 16989
			OverlapAfter,
			// Token: 0x0400425E RID: 16990
			After
		}
	}
}
