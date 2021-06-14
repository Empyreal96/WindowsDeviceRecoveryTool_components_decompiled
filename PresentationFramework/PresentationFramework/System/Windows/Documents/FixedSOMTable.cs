using System;

namespace System.Windows.Documents
{
	// Token: 0x02000364 RID: 868
	internal sealed class FixedSOMTable : FixedSOMPageElement
	{
		// Token: 0x06002E20 RID: 11808 RVA: 0x000D06B3 File Offset: 0x000CE8B3
		public FixedSOMTable(FixedSOMPage page) : base(page)
		{
			this._numCols = 0;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000D06C4 File Offset: 0x000CE8C4
		public void AddRow(FixedSOMTableRow row)
		{
			base.Add(row);
			int count = row.SemanticBoxes.Count;
			if (count > this._numCols)
			{
				this._numCols = count;
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000D06F4 File Offset: 0x000CE8F4
		public bool AddContainer(FixedSOMContainer container)
		{
			Rect boundingRect = container.BoundingRect;
			double num = boundingRect.Height * 0.2;
			double num2 = boundingRect.Width * 0.2;
			boundingRect.Inflate(-num2, -num);
			if (base.BoundingRect.Contains(boundingRect))
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMTableRow fixedSOMTableRow = (FixedSOMTableRow)fixedSOMSemanticBox;
					if (fixedSOMTableRow.BoundingRect.Contains(boundingRect))
					{
						foreach (FixedSOMSemanticBox fixedSOMSemanticBox2 in fixedSOMTableRow.SemanticBoxes)
						{
							FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMSemanticBox2;
							if (fixedSOMTableCell.BoundingRect.Contains(boundingRect))
							{
								fixedSOMTableCell.AddContainer(container);
								FixedSOMFixedBlock fixedSOMFixedBlock = container as FixedSOMFixedBlock;
								if (fixedSOMFixedBlock != null)
								{
									if (fixedSOMFixedBlock.IsRTL)
									{
										this._RTLCount++;
									}
									else
									{
										this._LTRCount++;
									}
								}
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000D0848 File Offset: 0x000CEA48
		public override void SetRTFProperties(FixedElement element)
		{
			if (element.Type == typeof(Table))
			{
				element.SetValue(Table.CellSpacingProperty, 0.0);
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x000D087A File Offset: 0x000CEA7A
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x000D088A File Offset: 0x000CEA8A
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.Table,
					FixedElement.ElementType.TableRowGroup
				};
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x000D089C File Offset: 0x000CEA9C
		internal bool IsEmpty
		{
			get
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMTableRow fixedSOMTableRow = (FixedSOMTableRow)fixedSOMSemanticBox;
					if (!fixedSOMTableRow.IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06002E27 RID: 11815 RVA: 0x000D08FC File Offset: 0x000CEAFC
		internal bool IsSingleCelled
		{
			get
			{
				if (base.SemanticBoxes.Count == 1)
				{
					FixedSOMTableRow fixedSOMTableRow = base.SemanticBoxes[0] as FixedSOMTableRow;
					return fixedSOMTableRow.SemanticBoxes.Count == 1;
				}
				return false;
			}
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000D093C File Offset: 0x000CEB3C
		internal void DeleteEmptyRows()
		{
			int i = 0;
			while (i < base.SemanticBoxes.Count)
			{
				FixedSOMTableRow fixedSOMTableRow = base.SemanticBoxes[i] as FixedSOMTableRow;
				if (fixedSOMTableRow != null && fixedSOMTableRow.IsEmpty && fixedSOMTableRow.BoundingRect.Height < 10.0)
				{
					base.SemanticBoxes.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000D09A4 File Offset: 0x000CEBA4
		internal void DeleteEmptyColumns()
		{
			int count = base.SemanticBoxes.Count;
			int[] array = new int[count];
			for (;;)
			{
				double num = double.MaxValue;
				bool flag = true;
				for (int i = 0; i < count; i++)
				{
					FixedSOMTableRow fixedSOMTableRow = (FixedSOMTableRow)base.SemanticBoxes[i];
					int num2 = array[i];
					flag = (flag && num2 < fixedSOMTableRow.SemanticBoxes.Count);
					if (flag)
					{
						FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMTableRow.SemanticBoxes[num2];
						flag = (fixedSOMTableCell.IsEmpty && fixedSOMTableCell.BoundingRect.Width < 5.0);
					}
					if (num2 + 1 < fixedSOMTableRow.SemanticBoxes.Count)
					{
						FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMTableRow.SemanticBoxes[num2 + 1];
						double left = fixedSOMTableCell.BoundingRect.Left;
						if (left < num)
						{
							if (num != 1.7976931348623157E+308)
							{
								flag = false;
							}
							num = left;
						}
						else if (left > num)
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					for (int i = 0; i < count; i++)
					{
						FixedSOMTableRow fixedSOMTableRow2 = (FixedSOMTableRow)base.SemanticBoxes[i];
						fixedSOMTableRow2.SemanticBoxes.RemoveAt(array[i]);
					}
					if (num == 1.7976931348623157E+308)
					{
						break;
					}
				}
				else
				{
					if (num == 1.7976931348623157E+308)
					{
						return;
					}
					for (int i = 0; i < count; i++)
					{
						FixedSOMTableRow fixedSOMTableRow3 = (FixedSOMTableRow)base.SemanticBoxes[i];
						int num3 = array[i];
						if (num3 + 1 < fixedSOMTableRow3.SemanticBoxes.Count && fixedSOMTableRow3.SemanticBoxes[num3 + 1].BoundingRect.Left == num)
						{
							array[i] = num3 + 1;
						}
						else
						{
							FixedSOMTableCell fixedSOMTableCell2 = (FixedSOMTableCell)fixedSOMTableRow3.SemanticBoxes[num3];
							int columnSpan = fixedSOMTableCell2.ColumnSpan;
							fixedSOMTableCell2.ColumnSpan = columnSpan + 1;
						}
					}
				}
			}
		}

		// Token: 0x04001DEB RID: 7659
		private const double _minColumnWidth = 5.0;

		// Token: 0x04001DEC RID: 7660
		private const double _minRowHeight = 10.0;

		// Token: 0x04001DED RID: 7661
		private int _RTLCount;

		// Token: 0x04001DEE RID: 7662
		private int _LTRCount;

		// Token: 0x04001DEF RID: 7663
		private int _numCols;
	}
}
