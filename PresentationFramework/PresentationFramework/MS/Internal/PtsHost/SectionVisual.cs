using System;
using System.Windows;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000645 RID: 1605
	internal class SectionVisual : DrawingVisual
	{
		// Token: 0x06006A55 RID: 27221 RVA: 0x0012AAD9 File Offset: 0x00128CD9
		internal SectionVisual()
		{
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x001E438C File Offset: 0x001E258C
		internal void DrawColumnRules(ref PTS.FSTRACKDESCRIPTION[] arrayColumnDesc, double columnVStart, double columnHeight, ColumnPropertiesGroup columnProperties)
		{
			Point[] array = null;
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			if (arrayColumnDesc.Length > 1 && columnRuleWidth > 0.0)
			{
				int num = (arrayColumnDesc[1].fsrc.u - (arrayColumnDesc[0].fsrc.u + arrayColumnDesc[0].fsrc.du)) / 2;
				array = new Point[(arrayColumnDesc.Length - 1) * 2];
				for (int i = 1; i < arrayColumnDesc.Length; i++)
				{
					double x = TextDpi.FromTextDpi(arrayColumnDesc[i].fsrc.u - num);
					array[(i - 1) * 2].X = x;
					array[(i - 1) * 2].Y = columnVStart;
					array[(i - 1) * 2 + 1].X = x;
					array[(i - 1) * 2 + 1].Y = columnVStart + columnHeight;
				}
			}
			bool flag = this._ruleWidth != columnRuleWidth;
			if (!flag && this._rulePositions != array)
			{
				int num2 = (this._rulePositions == null) ? 0 : this._rulePositions.Length;
				int num3 = (array == null) ? 0 : array.Length;
				if (num2 == num3)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (!DoubleUtil.AreClose(array[j].X, this._rulePositions[j].X) || !DoubleUtil.AreClose(array[j].Y, this._rulePositions[j].Y))
						{
							flag = true;
							break;
						}
					}
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				this._ruleWidth = columnRuleWidth;
				this._rulePositions = array;
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					if (array != null)
					{
						Brush brush = (Brush)FreezableOperations.GetAsFrozenIfPossible(columnProperties.ColumnRuleBrush);
						Pen pen = new Pen(brush, columnRuleWidth);
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						for (int k = 0; k < array.Length; k += 2)
						{
							drawingContext.DrawLine(pen, array[k], array[k + 1]);
						}
					}
				}
			}
		}

		// Token: 0x04003436 RID: 13366
		private Point[] _rulePositions;

		// Token: 0x04003437 RID: 13367
		private double _ruleWidth;
	}
}
