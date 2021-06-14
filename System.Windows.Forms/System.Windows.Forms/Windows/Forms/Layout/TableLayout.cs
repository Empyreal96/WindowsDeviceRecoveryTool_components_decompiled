using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004E0 RID: 1248
	internal class TableLayout : LayoutEngine
	{
		// Token: 0x060052C5 RID: 21189 RVA: 0x0015A549 File Offset: 0x00158749
		private static int GetMedian(int low, int hi)
		{
			return low + (hi - low >> 1);
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x0015A554 File Offset: 0x00158754
		private static void Sort(object[] array, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length > 1)
			{
				TableLayout.SorterObjectArray sorterObjectArray = new TableLayout.SorterObjectArray(array, comparer);
				sorterObjectArray.QuickSort(0, array.Length - 1);
			}
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0015A58B File Offset: 0x0015878B
		internal static TableLayoutSettings CreateSettings(IArrangedElement owner)
		{
			return new TableLayoutSettings(owner);
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0015A594 File Offset: 0x00158794
		internal override void ProcessSuspendedLayoutEventArgs(IArrangedElement container, LayoutEventArgs args)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			foreach (string text in TableLayout._propertiesWhichInvalidateCache)
			{
				if (args.AffectedProperty == text)
				{
					TableLayout.ClearCachedAssignments(containerInfo);
					return;
				}
			}
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x0015A5D0 File Offset: 0x001587D0
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs args)
		{
			this.ProcessSuspendedLayoutEventArgs(container, args);
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			this.EnsureRowAndColumnAssignments(container, containerInfo, false);
			int cellBorderWidth = containerInfo.CellBorderWidth;
			Size size = container.DisplayRectangle.Size - new Size(cellBorderWidth, cellBorderWidth);
			size.Width = Math.Max(size.Width, 1);
			size.Height = Math.Max(size.Height, 1);
			Size usedSpace = this.ApplyStyles(containerInfo, size, false);
			this.ExpandLastElement(containerInfo, usedSpace, size);
			RectangleF displayRectF = container.DisplayRectangle;
			displayRectF.Inflate(-((float)cellBorderWidth / 2f), (float)(-(float)cellBorderWidth) / 2f);
			this.SetElementBounds(containerInfo, displayRectF);
			CommonProperties.SetLayoutBounds(containerInfo.Container, new Size(this.SumStrips(containerInfo.Columns, 0, containerInfo.Columns.Length), this.SumStrips(containerInfo.Rows, 0, containerInfo.Rows.Length)));
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x0015A6C4 File Offset: 0x001588C4
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			bool flag = false;
			float num = -1f;
			Size size = containerInfo.GetCachedPreferredSize(proposedConstraints, out flag);
			if (flag)
			{
				return size;
			}
			TableLayout.ContainerInfo containerInfo2 = new TableLayout.ContainerInfo(containerInfo);
			int cellBorderWidth = containerInfo.CellBorderWidth;
			if (containerInfo.MaxColumns == 1 && containerInfo.ColumnStyles.Count > 0 && containerInfo.ColumnStyles[0].SizeType == SizeType.Absolute)
			{
				Size size2 = container.DisplayRectangle.Size - new Size(cellBorderWidth * 2, cellBorderWidth * 2);
				size2.Width = Math.Max(size2.Width, 1);
				size2.Height = Math.Max(size2.Height, 1);
				num = containerInfo.ColumnStyles[0].Size;
				containerInfo.ColumnStyles[0].SetSize(Math.Max(num, (float)Math.Min(proposedConstraints.Width, size2.Width)));
			}
			this.EnsureRowAndColumnAssignments(container, containerInfo2, true);
			Size sz = new Size(cellBorderWidth, cellBorderWidth);
			proposedConstraints -= sz;
			proposedConstraints.Width = Math.Max(proposedConstraints.Width, 1);
			proposedConstraints.Height = Math.Max(proposedConstraints.Height, 1);
			if (containerInfo2.Columns != null && containerInfo.Columns != null && containerInfo2.Columns.Length != containerInfo.Columns.Length)
			{
				TableLayout.ClearCachedAssignments(containerInfo);
			}
			if (containerInfo2.Rows != null && containerInfo.Rows != null && containerInfo2.Rows.Length != containerInfo.Rows.Length)
			{
				TableLayout.ClearCachedAssignments(containerInfo);
			}
			size = this.ApplyStyles(containerInfo2, proposedConstraints, true);
			if (num >= 0f)
			{
				containerInfo.ColumnStyles[0].SetSize(num);
			}
			return size + sz;
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x0015A883 File Offset: 0x00158A83
		private void EnsureRowAndColumnAssignments(IArrangedElement container, TableLayout.ContainerInfo containerInfo, bool doNotCache)
		{
			if (!TableLayout.HasCachedAssignments(containerInfo) || doNotCache)
			{
				this.AssignRowsAndColumns(containerInfo);
			}
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x0015A89C File Offset: 0x00158A9C
		private void ExpandLastElement(TableLayout.ContainerInfo containerInfo, Size usedSpace, Size totalSpace)
		{
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayout.Strip[] columns = containerInfo.Columns;
			if (columns.Length != 0 && totalSpace.Width > usedSpace.Width)
			{
				TableLayout.Strip[] array = columns;
				int num = columns.Length - 1;
				array[num].MinSize = array[num].MinSize + (totalSpace.Width - usedSpace.Width);
			}
			if (rows.Length != 0 && totalSpace.Height > usedSpace.Height)
			{
				TableLayout.Strip[] array2 = rows;
				int num2 = rows.Length - 1;
				array2[num2].MinSize = array2[num2].MinSize + (totalSpace.Height - usedSpace.Height);
			}
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x0015A92C File Offset: 0x00158B2C
		private void AssignRowsAndColumns(TableLayout.ContainerInfo containerInfo)
		{
			int num = containerInfo.MaxColumns;
			int num2 = containerInfo.MaxRows;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			int minRowsAndColumns = containerInfo.MinRowsAndColumns;
			int minColumns = containerInfo.MinColumns;
			int minRows = containerInfo.MinRows;
			TableLayoutPanelGrowStyle growStyle = containerInfo.GrowStyle;
			if (growStyle == TableLayoutPanelGrowStyle.FixedSize)
			{
				if (containerInfo.MinRowsAndColumns > num * num2)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelFullDesc"));
				}
				if (minColumns > num || minRows > num2)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelSpanDesc"));
				}
				num2 = Math.Max(1, num2);
				num = Math.Max(1, num);
			}
			else if (growStyle == TableLayoutPanelGrowStyle.AddRows)
			{
				num2 = 0;
			}
			else
			{
				num = 0;
			}
			if (num > 0)
			{
				this.xAssignRowsAndColumns(containerInfo, childrenInfo, num, (num2 == 0) ? int.MaxValue : num2, growStyle);
				return;
			}
			if (num2 > 0)
			{
				int num3 = Math.Max((int)Math.Ceiling((double)((float)minRowsAndColumns / (float)num2)), minColumns);
				num3 = Math.Max(num3, 1);
				while (!this.xAssignRowsAndColumns(containerInfo, childrenInfo, num3, num2, growStyle))
				{
					num3++;
				}
				return;
			}
			this.xAssignRowsAndColumns(containerInfo, childrenInfo, Math.Max(minColumns, 1), int.MaxValue, growStyle);
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x0015AA34 File Offset: 0x00158C34
		private bool xAssignRowsAndColumns(TableLayout.ContainerInfo containerInfo, TableLayout.LayoutInfo[] childrenInfo, int maxColumns, int maxRows, TableLayoutPanelGrowStyle growStyle)
		{
			int num = 0;
			int num2 = 0;
			TableLayout.ReservationGrid reservationGrid = new TableLayout.ReservationGrid();
			int num3 = 0;
			int num4 = 0;
			int num5 = -1;
			int num6 = -1;
			TableLayout.LayoutInfo[] fixedChildrenInfo = containerInfo.FixedChildrenInfo;
			TableLayout.LayoutInfo nextLayoutInfo = TableLayout.GetNextLayoutInfo(fixedChildrenInfo, ref num5, true);
			TableLayout.LayoutInfo nextLayoutInfo2 = TableLayout.GetNextLayoutInfo(childrenInfo, ref num6, false);
			while (nextLayoutInfo != null || nextLayoutInfo2 != null)
			{
				int num7 = num4;
				if (nextLayoutInfo2 != null)
				{
					nextLayoutInfo2.RowStart = num3;
					nextLayoutInfo2.ColumnStart = num4;
					this.AdvanceUntilFits(maxColumns, reservationGrid, nextLayoutInfo2, out num7);
					if (nextLayoutInfo2.RowStart >= maxRows)
					{
						return false;
					}
				}
				int num8;
				if (nextLayoutInfo2 != null && (nextLayoutInfo == null || (!this.IsCursorPastInsertionPoint(nextLayoutInfo, nextLayoutInfo2.RowStart, num7) && !this.IsOverlappingWithReservationGrid(nextLayoutInfo, reservationGrid, num3))))
				{
					for (int i = 0; i < nextLayoutInfo2.RowStart - num3; i++)
					{
						reservationGrid.AdvanceRow();
					}
					num3 = nextLayoutInfo2.RowStart;
					num8 = Math.Min(num3 + nextLayoutInfo2.RowSpan, maxRows);
					reservationGrid.ReserveAll(nextLayoutInfo2, num8, num7);
					nextLayoutInfo2 = TableLayout.GetNextLayoutInfo(childrenInfo, ref num6, false);
				}
				else
				{
					if (num4 >= maxColumns)
					{
						num4 = 0;
						num3++;
						reservationGrid.AdvanceRow();
					}
					nextLayoutInfo.RowStart = Math.Min(nextLayoutInfo.RowPosition, maxRows - 1);
					nextLayoutInfo.ColumnStart = Math.Min(nextLayoutInfo.ColumnPosition, maxColumns - 1);
					if (num3 > nextLayoutInfo.RowStart)
					{
						nextLayoutInfo.ColumnStart = num4;
					}
					else if (num3 == nextLayoutInfo.RowStart)
					{
						nextLayoutInfo.ColumnStart = Math.Max(nextLayoutInfo.ColumnStart, num4);
					}
					nextLayoutInfo.RowStart = Math.Max(nextLayoutInfo.RowStart, num3);
					int j;
					for (j = 0; j < nextLayoutInfo.RowStart - num3; j++)
					{
						reservationGrid.AdvanceRow();
					}
					this.AdvanceUntilFits(maxColumns, reservationGrid, nextLayoutInfo, out num7);
					if (nextLayoutInfo.RowStart >= maxRows)
					{
						return false;
					}
					while (j < nextLayoutInfo.RowStart - num3)
					{
						reservationGrid.AdvanceRow();
						j++;
					}
					num3 = nextLayoutInfo.RowStart;
					num7 = Math.Min(nextLayoutInfo.ColumnStart + nextLayoutInfo.ColumnSpan, maxColumns);
					num8 = Math.Min(nextLayoutInfo.RowStart + nextLayoutInfo.RowSpan, maxRows);
					reservationGrid.ReserveAll(nextLayoutInfo, num8, num7);
					nextLayoutInfo = TableLayout.GetNextLayoutInfo(fixedChildrenInfo, ref num5, true);
				}
				num4 = num7;
				num2 = ((num2 == int.MaxValue) ? num8 : Math.Max(num2, num8));
				num = ((num == int.MaxValue) ? num7 : Math.Max(num, num7));
			}
			if (growStyle == TableLayoutPanelGrowStyle.FixedSize)
			{
				num = maxColumns;
				num2 = maxRows;
			}
			else if (growStyle == TableLayoutPanelGrowStyle.AddRows)
			{
				num = maxColumns;
				num2 = Math.Max(containerInfo.MaxRows, num2);
			}
			else
			{
				num2 = ((maxRows == int.MaxValue) ? num2 : maxRows);
				num = Math.Max(containerInfo.MaxColumns, num);
			}
			if (containerInfo.Rows == null || containerInfo.Rows.Length != num2)
			{
				containerInfo.Rows = new TableLayout.Strip[num2];
			}
			if (containerInfo.Columns == null || containerInfo.Columns.Length != num)
			{
				containerInfo.Columns = new TableLayout.Strip[num];
			}
			containerInfo.Valid = true;
			return true;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x0015AD0C File Offset: 0x00158F0C
		private static TableLayout.LayoutInfo GetNextLayoutInfo(TableLayout.LayoutInfo[] layoutInfo, ref int index, bool absolutelyPositioned)
		{
			int num = index + 1;
			index = num;
			for (int i = num; i < layoutInfo.Length; i++)
			{
				if (absolutelyPositioned == layoutInfo[i].IsAbsolutelyPositioned)
				{
					index = i;
					return layoutInfo[i];
				}
			}
			index = layoutInfo.Length;
			return null;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x0015AD47 File Offset: 0x00158F47
		private bool IsCursorPastInsertionPoint(TableLayout.LayoutInfo fixedLayoutInfo, int insertionRow, int insertionCol)
		{
			return fixedLayoutInfo.RowPosition < insertionRow || (fixedLayoutInfo.RowPosition == insertionRow && fixedLayoutInfo.ColumnPosition < insertionCol);
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x0015AD6C File Offset: 0x00158F6C
		private bool IsOverlappingWithReservationGrid(TableLayout.LayoutInfo fixedLayoutInfo, TableLayout.ReservationGrid reservationGrid, int currentRow)
		{
			if (fixedLayoutInfo.RowPosition < currentRow)
			{
				return true;
			}
			for (int i = fixedLayoutInfo.RowPosition - currentRow; i < fixedLayoutInfo.RowPosition - currentRow + fixedLayoutInfo.RowSpan; i++)
			{
				for (int j = fixedLayoutInfo.ColumnPosition; j < fixedLayoutInfo.ColumnPosition + fixedLayoutInfo.ColumnSpan; j++)
				{
					if (reservationGrid.IsReserved(j, i))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x0015ADD0 File Offset: 0x00158FD0
		private void AdvanceUntilFits(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, out int colStop)
		{
			int rowStart = layoutInfo.RowStart;
			do
			{
				this.GetColStartAndStop(maxColumns, reservationGrid, layoutInfo, out colStop);
			}
			while (this.ScanRowForOverlap(maxColumns, reservationGrid, layoutInfo, colStop, layoutInfo.RowStart - rowStart));
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x0015AE08 File Offset: 0x00159008
		private void GetColStartAndStop(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, out int colStop)
		{
			colStop = layoutInfo.ColumnStart + layoutInfo.ColumnSpan;
			if (colStop > maxColumns)
			{
				if (layoutInfo.ColumnStart != 0)
				{
					layoutInfo.ColumnStart = 0;
					int rowStart = layoutInfo.RowStart;
					layoutInfo.RowStart = rowStart + 1;
				}
				colStop = Math.Min(layoutInfo.ColumnSpan, maxColumns);
			}
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x0015AE5C File Offset: 0x0015905C
		private bool ScanRowForOverlap(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, int stopCol, int rowOffset)
		{
			for (int i = layoutInfo.ColumnStart; i < stopCol; i++)
			{
				if (reservationGrid.IsReserved(i, rowOffset))
				{
					layoutInfo.ColumnStart = i + 1;
					while (layoutInfo.ColumnStart < maxColumns && reservationGrid.IsReserved(layoutInfo.ColumnStart, rowOffset))
					{
						int columnStart = layoutInfo.ColumnStart;
						layoutInfo.ColumnStart = columnStart + 1;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x0015AEC0 File Offset: 0x001590C0
		private Size ApplyStyles(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, bool measureOnly)
		{
			Size empty = Size.Empty;
			this.InitializeStrips(containerInfo.Columns, containerInfo.ColumnStyles);
			this.InitializeStrips(containerInfo.Rows, containerInfo.RowStyles);
			containerInfo.ChildHasColumnSpan = false;
			containerInfo.ChildHasRowSpan = false;
			foreach (TableLayout.LayoutInfo layoutInfo in containerInfo.ChildrenInfo)
			{
				containerInfo.Columns[layoutInfo.ColumnStart].IsStart = true;
				containerInfo.Rows[layoutInfo.RowStart].IsStart = true;
				if (layoutInfo.ColumnSpan > 1)
				{
					containerInfo.ChildHasColumnSpan = true;
				}
				if (layoutInfo.RowSpan > 1)
				{
					containerInfo.ChildHasRowSpan = true;
				}
			}
			empty.Width = this.InflateColumns(containerInfo, proposedConstraints, measureOnly);
			int expandLastElementWidth = Math.Max(0, proposedConstraints.Width - empty.Width);
			empty.Height = this.InflateRows(containerInfo, proposedConstraints, expandLastElementWidth, measureOnly);
			return empty;
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x0015AFAC File Offset: 0x001591AC
		private void InitializeStrips(TableLayout.Strip[] strips, IList styles)
		{
			for (int i = 0; i < strips.Length; i++)
			{
				TableLayoutStyle tableLayoutStyle = (i < styles.Count) ? ((TableLayoutStyle)styles[i]) : null;
				TableLayout.Strip strip = strips[i];
				if (tableLayoutStyle != null && tableLayoutStyle.SizeType == SizeType.Absolute)
				{
					strip.MinSize = (int)Math.Round((double)((TableLayoutStyle)styles[i]).Size);
					strip.MaxSize = strip.MinSize;
				}
				else
				{
					strip.MinSize = 0;
					strip.MaxSize = 0;
				}
				strip.IsStart = false;
				strips[i] = strip;
			}
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x0015B04C File Offset: 0x0015924C
		private int InflateColumns(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, bool measureOnly)
		{
			bool flag = measureOnly;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			if (containerInfo.ChildHasColumnSpan)
			{
				TableLayout.Sort(childrenInfo, TableLayout.ColumnSpanComparer.GetInstance);
			}
			if (flag && proposedConstraints.Width < 32767)
			{
				TableLayoutPanel tableLayoutPanel = containerInfo.Container as TableLayoutPanel;
				if (tableLayoutPanel != null && tableLayoutPanel.ParentInternal != null && tableLayoutPanel.ParentInternal.LayoutEngine == DefaultLayout.Instance)
				{
					if (tableLayoutPanel.Dock == DockStyle.Top || tableLayoutPanel.Dock == DockStyle.Bottom || tableLayoutPanel.Dock == DockStyle.Fill)
					{
						flag = false;
					}
					if ((tableLayoutPanel.Anchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right))
					{
						flag = false;
					}
				}
			}
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				int columnSpan = layoutInfo.ColumnSpan;
				if (columnSpan > 1 || !this.IsAbsolutelySized(layoutInfo.ColumnStart, containerInfo.ColumnStyles))
				{
					int num;
					int num2;
					if (columnSpan == 1 && layoutInfo.RowSpan == 1 && this.IsAbsolutelySized(layoutInfo.RowStart, containerInfo.RowStyles))
					{
						int height = (int)containerInfo.RowStyles[layoutInfo.RowStart].Size;
						num = this.GetElementSize(element, new Size(0, height)).Width;
						num2 = num;
					}
					else
					{
						num = this.GetElementSize(element, new Size(1, 0)).Width;
						num2 = this.GetElementSize(element, Size.Empty).Width;
					}
					Padding margin = CommonProperties.GetMargin(element);
					num += margin.Horizontal;
					num2 += margin.Horizontal;
					int stop = Math.Min(layoutInfo.ColumnStart + layoutInfo.ColumnSpan, containerInfo.Columns.Length);
					this.DistributeSize(containerInfo.ColumnStyles, containerInfo.Columns, layoutInfo.ColumnStart, stop, num, num2, containerInfo.CellBorderWidth);
				}
			}
			int num3 = this.DistributeStyles(containerInfo.CellBorderWidth, containerInfo.ColumnStyles, containerInfo.Columns, proposedConstraints.Width, flag);
			if (flag && num3 > proposedConstraints.Width && proposedConstraints.Width > 1)
			{
				TableLayout.Strip[] columns = containerInfo.Columns;
				float num4 = 0f;
				int num5 = 0;
				TableLayoutStyleCollection columnStyles = containerInfo.ColumnStyles;
				for (int j = 0; j < columns.Length; j++)
				{
					TableLayout.Strip strip = columns[j];
					if (j < columnStyles.Count)
					{
						TableLayoutStyle tableLayoutStyle = columnStyles[j];
						if (tableLayoutStyle.SizeType == SizeType.Percent)
						{
							num4 += tableLayoutStyle.Size;
							num5 += strip.MinSize;
						}
					}
				}
				int val = num3 - proposedConstraints.Width;
				int num6 = Math.Min(val, num5);
				for (int k = 0; k < columns.Length; k++)
				{
					if (k < columnStyles.Count)
					{
						TableLayoutStyle tableLayoutStyle2 = columnStyles[k];
						if (tableLayoutStyle2.SizeType == SizeType.Percent)
						{
							float num7 = tableLayoutStyle2.Size / num4;
							TableLayout.Strip[] array2 = columns;
							int num8 = k;
							array2[num8].MinSize = array2[num8].MinSize - (int)(num7 * (float)num6);
						}
					}
				}
				return num3 - num6;
			}
			return num3;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x0015B354 File Offset: 0x00159554
		private int InflateRows(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, int expandLastElementWidth, bool measureOnly)
		{
			bool flag = measureOnly;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			if (containerInfo.ChildHasRowSpan)
			{
				TableLayout.Sort(childrenInfo, TableLayout.RowSpanComparer.GetInstance);
			}
			bool hasMultiplePercentColumns = containerInfo.HasMultiplePercentColumns;
			if (flag && proposedConstraints.Height < 32767)
			{
				TableLayoutPanel tableLayoutPanel = containerInfo.Container as TableLayoutPanel;
				if (tableLayoutPanel != null && tableLayoutPanel.ParentInternal != null && tableLayoutPanel.ParentInternal.LayoutEngine == DefaultLayout.Instance)
				{
					if (tableLayoutPanel.Dock == DockStyle.Left || tableLayoutPanel.Dock == DockStyle.Right || tableLayoutPanel.Dock == DockStyle.Fill)
					{
						flag = false;
					}
					if ((tableLayoutPanel.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
					{
						flag = false;
					}
				}
			}
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				int rowSpan = layoutInfo.RowSpan;
				if (rowSpan > 1 || !this.IsAbsolutelySized(layoutInfo.RowStart, containerInfo.RowStyles))
				{
					int num = this.SumStrips(containerInfo.Columns, layoutInfo.ColumnStart, layoutInfo.ColumnSpan);
					if (!flag && layoutInfo.ColumnStart + layoutInfo.ColumnSpan >= containerInfo.MaxColumns && !hasMultiplePercentColumns)
					{
						num += expandLastElementWidth;
					}
					Padding margin = CommonProperties.GetMargin(element);
					int num2 = this.GetElementSize(element, new Size(num - margin.Horizontal, 0)).Height + margin.Vertical;
					int max = num2;
					int stop = Math.Min(layoutInfo.RowStart + layoutInfo.RowSpan, containerInfo.Rows.Length);
					this.DistributeSize(containerInfo.RowStyles, containerInfo.Rows, layoutInfo.RowStart, stop, num2, max, containerInfo.CellBorderWidth);
				}
			}
			return this.DistributeStyles(containerInfo.CellBorderWidth, containerInfo.RowStyles, containerInfo.Rows, proposedConstraints.Height, flag);
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x0015B518 File Offset: 0x00159718
		private Size GetElementSize(IArrangedElement element, Size proposedConstraints)
		{
			if (CommonProperties.GetAutoSize(element))
			{
				return element.GetPreferredSize(proposedConstraints);
			}
			return CommonProperties.GetSpecifiedBounds(element).Size;
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x0015B544 File Offset: 0x00159744
		internal int SumStrips(TableLayout.Strip[] strips, int start, int span)
		{
			int num = 0;
			for (int i = start; i < Math.Min(start + span, strips.Length); i++)
			{
				TableLayout.Strip strip = strips[i];
				num += strip.MinSize;
			}
			return num;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x0015B57C File Offset: 0x0015977C
		private void DistributeSize(IList styles, TableLayout.Strip[] strips, int start, int stop, int min, int max, int cellBorderWidth)
		{
			this.xDistributeSize(styles, strips, start, stop, min, TableLayout.MinSizeProxy.GetInstance, cellBorderWidth);
			this.xDistributeSize(styles, strips, start, stop, max, TableLayout.MaxSizeProxy.GetInstance, cellBorderWidth);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x0015B5A8 File Offset: 0x001597A8
		private void xDistributeSize(IList styles, TableLayout.Strip[] strips, int start, int stop, int desiredLength, TableLayout.SizeProxy sizeProxy, int cellBorderWidth)
		{
			int num = 0;
			int num2 = 0;
			desiredLength -= cellBorderWidth * (stop - start - 1);
			desiredLength = Math.Max(0, desiredLength);
			for (int i = start; i < stop; i++)
			{
				sizeProxy.Strip = strips[i];
				if (!this.IsAbsolutelySized(i, styles) && sizeProxy.Size == 0)
				{
					num2++;
				}
				num += sizeProxy.Size;
			}
			int num3 = desiredLength - num;
			if (num3 <= 0)
			{
				return;
			}
			if (num2 == 0)
			{
				int num4 = stop - 1;
				while (num4 >= start && (num4 >= styles.Count || ((TableLayoutStyle)styles[num4]).SizeType != SizeType.Percent))
				{
					num4--;
				}
				if (num4 != start - 1)
				{
					stop = num4 + 1;
				}
				for (int j = stop - 1; j >= start; j--)
				{
					if (!this.IsAbsolutelySized(j, styles))
					{
						sizeProxy.Strip = strips[j];
						if (j != strips.Length - 1 && !strips[j + 1].IsStart && !this.IsAbsolutelySized(j + 1, styles))
						{
							sizeProxy.Strip = strips[j + 1];
							int num5 = Math.Min(sizeProxy.Size, num3);
							sizeProxy.Size -= num5;
							strips[j + 1] = sizeProxy.Strip;
							sizeProxy.Strip = strips[j];
						}
						sizeProxy.Size += num3;
						strips[j] = sizeProxy.Strip;
						return;
					}
				}
				return;
			}
			int num6 = num3 / num2;
			int num7 = 0;
			for (int k = start; k < stop; k++)
			{
				sizeProxy.Strip = strips[k];
				if (!this.IsAbsolutelySized(k, styles) && sizeProxy.Size == 0)
				{
					num7++;
					if (num7 == num2)
					{
						num6 = num3 - num6 * (num2 - 1);
					}
					sizeProxy.Size += num6;
					strips[k] = sizeProxy.Strip;
				}
			}
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x0015B799 File Offset: 0x00159999
		private bool IsAbsolutelySized(int index, IList styles)
		{
			return index < styles.Count && ((TableLayoutStyle)styles[index]).SizeType == SizeType.Absolute;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x0015B7BC File Offset: 0x001599BC
		private int DistributeStyles(int cellBorderWidth, IList styles, TableLayout.Strip[] strips, int maxSize, bool dontHonorConstraint)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			bool flag = false;
			for (int i = 0; i < strips.Length; i++)
			{
				TableLayout.Strip strip = strips[i];
				if (i < styles.Count)
				{
					TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)styles[i];
					SizeType sizeType = tableLayoutStyle.SizeType;
					if (sizeType != SizeType.Absolute)
					{
						if (sizeType != SizeType.Percent)
						{
							num5 += (float)strip.MinSize;
							flag = true;
						}
						else
						{
							num3 += tableLayoutStyle.Size;
							num4 += (float)strip.MinSize;
						}
					}
					else
					{
						num5 += (float)strip.MinSize;
					}
				}
				else
				{
					flag = true;
				}
				strip.MaxSize += cellBorderWidth;
				strip.MinSize += cellBorderWidth;
				strips[i] = strip;
				num += strip.MinSize;
			}
			int num6 = maxSize - num;
			if (num3 > 0f)
			{
				if (!dontHonorConstraint)
				{
					if (num4 > (float)maxSize - num5)
					{
						num4 = Math.Max(0f, (float)maxSize - num5);
					}
					if (num6 > 0)
					{
						num4 += (float)num6;
					}
					else if (num6 < 0)
					{
						num4 = (float)maxSize - num5 - (float)(strips.Length * cellBorderWidth);
					}
					for (int j = 0; j < strips.Length; j++)
					{
						TableLayout.Strip strip2 = strips[j];
						SizeType sizeType2 = (j < styles.Count) ? ((TableLayoutStyle)styles[j]).SizeType : SizeType.AutoSize;
						if (sizeType2 == SizeType.Percent)
						{
							TableLayoutStyle tableLayoutStyle2 = (TableLayoutStyle)styles[j];
							int num7 = (int)(tableLayoutStyle2.Size * num4 / num3);
							num -= strip2.MinSize;
							num += num7 + cellBorderWidth;
							strip2.MinSize = num7 + cellBorderWidth;
							strips[j] = strip2;
						}
					}
				}
				else
				{
					int num8 = 0;
					for (int k = 0; k < strips.Length; k++)
					{
						TableLayout.Strip strip3 = strips[k];
						SizeType sizeType3 = (k < styles.Count) ? ((TableLayoutStyle)styles[k]).SizeType : SizeType.AutoSize;
						if (sizeType3 == SizeType.Percent)
						{
							TableLayoutStyle tableLayoutStyle3 = (TableLayoutStyle)styles[k];
							int val = (int)Math.Round((double)((float)strip3.MinSize * num3 / tableLayoutStyle3.Size));
							num8 = Math.Max(num8, val);
							num -= strip3.MinSize;
						}
					}
					num += num8;
				}
			}
			num6 = maxSize - num;
			if (flag && num6 > 0)
			{
				if ((float)num6 < num2)
				{
					float num9 = (float)num6 / num2;
				}
				num6 -= (int)Math.Ceiling((double)num2);
				for (int l = 0; l < strips.Length; l++)
				{
					TableLayout.Strip strip4 = strips[l];
					if (l >= styles.Count || ((TableLayoutStyle)styles[l]).SizeType == SizeType.AutoSize)
					{
						int num10 = Math.Min(strip4.MaxSize - strip4.MinSize, num6);
						if (num10 > 0)
						{
							num += num10;
							num6 -= num10;
							strip4.MinSize += num10;
							strips[l] = strip4;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x0015BAC8 File Offset: 0x00159CC8
		private void SetElementBounds(TableLayout.ContainerInfo containerInfo, RectangleF displayRectF)
		{
			int cellBorderWidth = containerInfo.CellBorderWidth;
			float num = displayRectF.Y;
			int i = 0;
			int j = 0;
			bool flag = false;
			Rectangle rectangle = Rectangle.Truncate(displayRectF);
			if (containerInfo.Container is Control)
			{
				Control control = containerInfo.Container as Control;
				flag = (control.RightToLeft == RightToLeft.Yes);
			}
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			float num2 = flag ? displayRectF.Right : displayRectF.X;
			TableLayout.Sort(childrenInfo, TableLayout.PostAssignedPositionComparer.GetInstance);
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				if (j != layoutInfo.RowStart)
				{
					while (j < layoutInfo.RowStart)
					{
						num += (float)containerInfo.Rows[j].MinSize;
						j++;
					}
					num2 = (flag ? displayRectF.Right : displayRectF.X);
					i = 0;
				}
				while (i < layoutInfo.ColumnStart)
				{
					if (flag)
					{
						num2 -= (float)containerInfo.Columns[i].MinSize;
					}
					else
					{
						num2 += (float)containerInfo.Columns[i].MinSize;
					}
					i++;
				}
				int num3 = i + layoutInfo.ColumnSpan;
				int num4 = 0;
				while (i < num3 && i < containerInfo.Columns.Length)
				{
					num4 += containerInfo.Columns[i].MinSize;
					i++;
				}
				if (flag)
				{
					num2 -= (float)num4;
				}
				int num5 = j + layoutInfo.RowSpan;
				int num6 = 0;
				int num7 = j;
				while (num7 < num5 && num7 < containerInfo.Rows.Length)
				{
					num6 += containerInfo.Rows[num7].MinSize;
					num7++;
				}
				Rectangle rectangle2 = new Rectangle((int)(num2 + (float)cellBorderWidth / 2f), (int)(num + (float)cellBorderWidth / 2f), num4 - cellBorderWidth, num6 - cellBorderWidth);
				Padding margin = CommonProperties.GetMargin(element);
				if (flag)
				{
					int right = margin.Right;
					margin.Right = margin.Left;
					margin.Left = right;
				}
				rectangle2 = LayoutUtils.DeflateRect(rectangle2, margin);
				rectangle2.Width = Math.Max(rectangle2.Width, 1);
				rectangle2.Height = Math.Max(rectangle2.Height, 1);
				AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(element);
				Rectangle bounds = LayoutUtils.AlignAndStretch(this.GetElementSize(element, rectangle2.Size), rectangle2, unifiedAnchor);
				bounds.Width = Math.Min(rectangle2.Width, bounds.Width);
				bounds.Height = Math.Min(rectangle2.Height, bounds.Height);
				if (flag)
				{
					bounds.X = rectangle2.X + (rectangle2.Right - bounds.Right);
				}
				element.SetBounds(bounds, BoundsSpecified.None);
				if (!flag)
				{
					num2 += (float)num4;
				}
			}
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x0015BD94 File Offset: 0x00159F94
		internal IArrangedElement GetControlFromPosition(IArrangedElement container, int column, int row)
		{
			if (row < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"RowPosition",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (column < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"ColumnPosition",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			ArrangedElementCollection children = container.Children;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			if (children == null || children.Count == 0)
			{
				return null;
			}
			if (!containerInfo.Valid)
			{
				this.EnsureRowAndColumnAssignments(container, containerInfo, true);
			}
			for (int i = 0; i < children.Count; i++)
			{
				TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(children[i]);
				if (layoutInfo.ColumnStart <= column && layoutInfo.ColumnStart + layoutInfo.ColumnSpan - 1 >= column && layoutInfo.RowStart <= row && layoutInfo.RowStart + layoutInfo.RowSpan - 1 >= row)
				{
					return layoutInfo.Element;
				}
			}
			return null;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x0015BE8C File Offset: 0x0015A08C
		internal TableLayoutPanelCellPosition GetPositionFromControl(IArrangedElement container, IArrangedElement child)
		{
			if (container == null || child == null)
			{
				return new TableLayoutPanelCellPosition(-1, -1);
			}
			ArrangedElementCollection children = container.Children;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			if (children == null || children.Count == 0)
			{
				return new TableLayoutPanelCellPosition(-1, -1);
			}
			if (!containerInfo.Valid)
			{
				this.EnsureRowAndColumnAssignments(container, containerInfo, true);
			}
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(child);
			return new TableLayoutPanelCellPosition(layoutInfo.ColumnStart, layoutInfo.RowStart);
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x0015BEF4 File Offset: 0x0015A0F4
		internal static TableLayout.LayoutInfo GetLayoutInfo(IArrangedElement element)
		{
			TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)element.Properties.GetObject(TableLayout._layoutInfoProperty);
			if (layoutInfo == null)
			{
				layoutInfo = new TableLayout.LayoutInfo(element);
				TableLayout.SetLayoutInfo(element, layoutInfo);
			}
			return layoutInfo;
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x0015BF29 File Offset: 0x0015A129
		internal static void SetLayoutInfo(IArrangedElement element, TableLayout.LayoutInfo value)
		{
			element.Properties.SetObject(TableLayout._layoutInfoProperty, value);
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x0015BF3C File Offset: 0x0015A13C
		internal static bool HasCachedAssignments(TableLayout.ContainerInfo containerInfo)
		{
			return containerInfo.Valid;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x0015BF44 File Offset: 0x0015A144
		internal static void ClearCachedAssignments(TableLayout.ContainerInfo containerInfo)
		{
			containerInfo.Valid = false;
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x0015BF50 File Offset: 0x0015A150
		internal static TableLayout.ContainerInfo GetContainerInfo(IArrangedElement container)
		{
			TableLayout.ContainerInfo containerInfo = (TableLayout.ContainerInfo)container.Properties.GetObject(TableLayout._containerInfoProperty);
			if (containerInfo == null)
			{
				containerInfo = new TableLayout.ContainerInfo(container);
				container.Properties.SetObject(TableLayout._containerInfoProperty, containerInfo);
			}
			return containerInfo;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG_LAYOUT")]
		private void Debug_VerifyAssignmentsAreCurrent(IArrangedElement container, TableLayout.ContainerInfo containerInfo)
		{
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x0015BF90 File Offset: 0x0015A190
		[Conditional("DEBUG_LAYOUT")]
		private void Debug_VerifyNoOverlapping(IArrangedElement container)
		{
			ArrayList arrayList = new ArrayList(container.Children.Count);
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayout.Strip[] columns = containerInfo.Columns;
			foreach (object obj in container.Children)
			{
				IArrangedElement arrangedElement = (IArrangedElement)obj;
				if (arrangedElement.ParticipatesInLayout)
				{
					arrayList.Add(TableLayout.GetLayoutInfo(arrangedElement));
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)arrayList[i];
				Rectangle bounds = layoutInfo.Element.Bounds;
				Rectangle rectangle = new Rectangle(layoutInfo.ColumnStart, layoutInfo.RowStart, layoutInfo.ColumnSpan, layoutInfo.RowSpan);
				for (int j = i + 1; j < arrayList.Count; j++)
				{
					TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)arrayList[j];
					Rectangle bounds2 = layoutInfo2.Element.Bounds;
					Rectangle rectangle2 = new Rectangle(layoutInfo2.ColumnStart, layoutInfo2.RowStart, layoutInfo2.ColumnSpan, layoutInfo2.RowSpan);
					if (LayoutUtils.IsIntersectHorizontally(bounds, bounds2))
					{
						for (int k = layoutInfo.ColumnStart; k < layoutInfo.ColumnStart + layoutInfo.ColumnSpan; k++)
						{
						}
						for (int k = layoutInfo2.ColumnStart; k < layoutInfo2.ColumnStart + layoutInfo2.ColumnSpan; k++)
						{
						}
					}
					if (LayoutUtils.IsIntersectVertically(bounds, bounds2))
					{
						for (int l = layoutInfo.RowStart; l < layoutInfo.RowStart + layoutInfo.RowSpan; l++)
						{
						}
						for (int l = layoutInfo2.RowStart; l < layoutInfo2.RowStart + layoutInfo2.RowSpan; l++)
						{
						}
					}
				}
			}
		}

		// Token: 0x040034F4 RID: 13556
		internal static readonly TableLayout Instance = new TableLayout();

		// Token: 0x040034F5 RID: 13557
		private static readonly int _containerInfoProperty = PropertyStore.CreateKey();

		// Token: 0x040034F6 RID: 13558
		private static readonly int _layoutInfoProperty = PropertyStore.CreateKey();

		// Token: 0x040034F7 RID: 13559
		private static string[] _propertiesWhichInvalidateCache = new string[]
		{
			null,
			PropertyNames.ChildIndex,
			PropertyNames.Parent,
			PropertyNames.Visible,
			PropertyNames.Items,
			PropertyNames.Rows,
			PropertyNames.Columns,
			PropertyNames.RowStyles,
			PropertyNames.ColumnStyles
		};

		// Token: 0x02000855 RID: 2133
		private struct SorterObjectArray
		{
			// Token: 0x06006FD0 RID: 28624 RVA: 0x0019A56E File Offset: 0x0019876E
			internal SorterObjectArray(object[] keys, IComparer comparer)
			{
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				this.keys = keys;
				this.comparer = comparer;
			}

			// Token: 0x06006FD1 RID: 28625 RVA: 0x0019A588 File Offset: 0x00198788
			internal void SwapIfGreaterWithItems(int a, int b)
			{
				if (a != b)
				{
					try
					{
						if (this.comparer.Compare(this.keys[a], this.keys[b]) > 0)
						{
							object obj = this.keys[a];
							this.keys[a] = this.keys[b];
							this.keys[b] = obj;
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new ArgumentException();
					}
					catch (Exception)
					{
						throw new InvalidOperationException();
					}
				}
			}

			// Token: 0x06006FD2 RID: 28626 RVA: 0x0019A608 File Offset: 0x00198808
			internal void QuickSort(int left, int right)
			{
				do
				{
					int num = left;
					int num2 = right;
					int median = TableLayout.GetMedian(num, num2);
					this.SwapIfGreaterWithItems(num, median);
					this.SwapIfGreaterWithItems(num, num2);
					this.SwapIfGreaterWithItems(median, num2);
					object obj = this.keys[median];
					do
					{
						try
						{
							while (this.comparer.Compare(this.keys[num], obj) < 0)
							{
								num++;
							}
							while (this.comparer.Compare(obj, this.keys[num2]) < 0)
							{
								num2--;
							}
						}
						catch (IndexOutOfRangeException)
						{
							throw new ArgumentException();
						}
						catch (Exception)
						{
							throw new InvalidOperationException();
						}
						if (num > num2)
						{
							break;
						}
						if (num < num2)
						{
							object obj2 = this.keys[num];
							this.keys[num] = this.keys[num2];
							this.keys[num2] = obj2;
						}
						num++;
						num2--;
					}
					while (num <= num2);
					if (num2 - left <= right - num)
					{
						if (left < num2)
						{
							this.QuickSort(left, num2);
						}
						left = num;
					}
					else
					{
						if (num < right)
						{
							this.QuickSort(num, right);
						}
						right = num2;
					}
				}
				while (left < right);
			}

			// Token: 0x04004337 RID: 17207
			private object[] keys;

			// Token: 0x04004338 RID: 17208
			private IComparer comparer;
		}

		// Token: 0x02000856 RID: 2134
		internal sealed class LayoutInfo
		{
			// Token: 0x06006FD3 RID: 28627 RVA: 0x0019A714 File Offset: 0x00198914
			public LayoutInfo(IArrangedElement element)
			{
				this._element = element;
			}

			// Token: 0x1700183C RID: 6204
			// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x0019A74D File Offset: 0x0019894D
			internal bool IsAbsolutelyPositioned
			{
				get
				{
					return this._rowPos >= 0 && this._colPos >= 0;
				}
			}

			// Token: 0x1700183D RID: 6205
			// (get) Token: 0x06006FD5 RID: 28629 RVA: 0x0019A766 File Offset: 0x00198966
			internal IArrangedElement Element
			{
				get
				{
					return this._element;
				}
			}

			// Token: 0x1700183E RID: 6206
			// (get) Token: 0x06006FD6 RID: 28630 RVA: 0x0019A76E File Offset: 0x0019896E
			// (set) Token: 0x06006FD7 RID: 28631 RVA: 0x0019A776 File Offset: 0x00198976
			internal int RowPosition
			{
				get
				{
					return this._rowPos;
				}
				set
				{
					this._rowPos = value;
				}
			}

			// Token: 0x1700183F RID: 6207
			// (get) Token: 0x06006FD8 RID: 28632 RVA: 0x0019A77F File Offset: 0x0019897F
			// (set) Token: 0x06006FD9 RID: 28633 RVA: 0x0019A787 File Offset: 0x00198987
			internal int ColumnPosition
			{
				get
				{
					return this._colPos;
				}
				set
				{
					this._colPos = value;
				}
			}

			// Token: 0x17001840 RID: 6208
			// (get) Token: 0x06006FDA RID: 28634 RVA: 0x0019A790 File Offset: 0x00198990
			// (set) Token: 0x06006FDB RID: 28635 RVA: 0x0019A798 File Offset: 0x00198998
			internal int RowStart
			{
				get
				{
					return this._rowStart;
				}
				set
				{
					this._rowStart = value;
				}
			}

			// Token: 0x17001841 RID: 6209
			// (get) Token: 0x06006FDC RID: 28636 RVA: 0x0019A7A1 File Offset: 0x001989A1
			// (set) Token: 0x06006FDD RID: 28637 RVA: 0x0019A7A9 File Offset: 0x001989A9
			internal int ColumnStart
			{
				get
				{
					return this._columnStart;
				}
				set
				{
					this._columnStart = value;
				}
			}

			// Token: 0x17001842 RID: 6210
			// (get) Token: 0x06006FDE RID: 28638 RVA: 0x0019A7B2 File Offset: 0x001989B2
			// (set) Token: 0x06006FDF RID: 28639 RVA: 0x0019A7BA File Offset: 0x001989BA
			internal int ColumnSpan
			{
				get
				{
					return this._columnSpan;
				}
				set
				{
					this._columnSpan = value;
				}
			}

			// Token: 0x17001843 RID: 6211
			// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x0019A7C3 File Offset: 0x001989C3
			// (set) Token: 0x06006FE1 RID: 28641 RVA: 0x0019A7CB File Offset: 0x001989CB
			internal int RowSpan
			{
				get
				{
					return this._rowSpan;
				}
				set
				{
					this._rowSpan = value;
				}
			}

			// Token: 0x04004339 RID: 17209
			private int _rowStart = -1;

			// Token: 0x0400433A RID: 17210
			private int _columnStart = -1;

			// Token: 0x0400433B RID: 17211
			private int _columnSpan = 1;

			// Token: 0x0400433C RID: 17212
			private int _rowSpan = 1;

			// Token: 0x0400433D RID: 17213
			private int _rowPos = -1;

			// Token: 0x0400433E RID: 17214
			private int _colPos = -1;

			// Token: 0x0400433F RID: 17215
			private IArrangedElement _element;
		}

		// Token: 0x02000857 RID: 2135
		internal sealed class ContainerInfo
		{
			// Token: 0x06006FE2 RID: 28642 RVA: 0x0019A7D4 File Offset: 0x001989D4
			public ContainerInfo(IArrangedElement container)
			{
				this._container = container;
				this._growStyle = TableLayoutPanelGrowStyle.AddRows;
			}

			// Token: 0x06006FE3 RID: 28643 RVA: 0x0019A800 File Offset: 0x00198A00
			public ContainerInfo(TableLayout.ContainerInfo containerInfo)
			{
				this._cellBorderWidth = containerInfo.CellBorderWidth;
				this._maxRows = containerInfo.MaxRows;
				this._maxColumns = containerInfo.MaxColumns;
				this._growStyle = containerInfo.GrowStyle;
				this._container = containerInfo.Container;
				this._rowStyles = containerInfo.RowStyles;
				this._colStyles = containerInfo.ColumnStyles;
			}

			// Token: 0x17001844 RID: 6212
			// (get) Token: 0x06006FE4 RID: 28644 RVA: 0x0019A87D File Offset: 0x00198A7D
			public IArrangedElement Container
			{
				get
				{
					return this._container;
				}
			}

			// Token: 0x17001845 RID: 6213
			// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x0019A885 File Offset: 0x00198A85
			// (set) Token: 0x06006FE6 RID: 28646 RVA: 0x0019A88D File Offset: 0x00198A8D
			public int CellBorderWidth
			{
				get
				{
					return this._cellBorderWidth;
				}
				set
				{
					this._cellBorderWidth = value;
				}
			}

			// Token: 0x17001846 RID: 6214
			// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x0019A896 File Offset: 0x00198A96
			// (set) Token: 0x06006FE8 RID: 28648 RVA: 0x0019A89E File Offset: 0x00198A9E
			public TableLayout.Strip[] Columns
			{
				get
				{
					return this._cols;
				}
				set
				{
					this._cols = value;
				}
			}

			// Token: 0x17001847 RID: 6215
			// (get) Token: 0x06006FE9 RID: 28649 RVA: 0x0019A8A7 File Offset: 0x00198AA7
			// (set) Token: 0x06006FEA RID: 28650 RVA: 0x0019A8AF File Offset: 0x00198AAF
			public TableLayout.Strip[] Rows
			{
				get
				{
					return this._rows;
				}
				set
				{
					this._rows = value;
				}
			}

			// Token: 0x17001848 RID: 6216
			// (get) Token: 0x06006FEB RID: 28651 RVA: 0x0019A8B8 File Offset: 0x00198AB8
			// (set) Token: 0x06006FEC RID: 28652 RVA: 0x0019A8C0 File Offset: 0x00198AC0
			public int MaxRows
			{
				get
				{
					return this._maxRows;
				}
				set
				{
					if (this._maxRows != value)
					{
						this._maxRows = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x17001849 RID: 6217
			// (get) Token: 0x06006FED RID: 28653 RVA: 0x0019A8D9 File Offset: 0x00198AD9
			// (set) Token: 0x06006FEE RID: 28654 RVA: 0x0019A8E1 File Offset: 0x00198AE1
			public int MaxColumns
			{
				get
				{
					return this._maxColumns;
				}
				set
				{
					if (this._maxColumns != value)
					{
						this._maxColumns = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x1700184A RID: 6218
			// (get) Token: 0x06006FEF RID: 28655 RVA: 0x0019A8FA File Offset: 0x00198AFA
			public int MinRowsAndColumns
			{
				get
				{
					return this._minRowsAndColumns;
				}
			}

			// Token: 0x1700184B RID: 6219
			// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x0019A902 File Offset: 0x00198B02
			public int MinColumns
			{
				get
				{
					return this._minColumns;
				}
			}

			// Token: 0x1700184C RID: 6220
			// (get) Token: 0x06006FF1 RID: 28657 RVA: 0x0019A90A File Offset: 0x00198B0A
			public int MinRows
			{
				get
				{
					return this._minRows;
				}
			}

			// Token: 0x1700184D RID: 6221
			// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x0019A912 File Offset: 0x00198B12
			// (set) Token: 0x06006FF3 RID: 28659 RVA: 0x0019A91A File Offset: 0x00198B1A
			public TableLayoutPanelGrowStyle GrowStyle
			{
				get
				{
					return this._growStyle;
				}
				set
				{
					if (this._growStyle != value)
					{
						this._growStyle = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x1700184E RID: 6222
			// (get) Token: 0x06006FF4 RID: 28660 RVA: 0x0019A933 File Offset: 0x00198B33
			// (set) Token: 0x06006FF5 RID: 28661 RVA: 0x0019A954 File Offset: 0x00198B54
			public TableLayoutRowStyleCollection RowStyles
			{
				get
				{
					if (this._rowStyles == null)
					{
						this._rowStyles = new TableLayoutRowStyleCollection(this._container);
					}
					return this._rowStyles;
				}
				set
				{
					this._rowStyles = value;
					if (this._rowStyles != null)
					{
						this._rowStyles.EnsureOwnership(this._container);
					}
				}
			}

			// Token: 0x1700184F RID: 6223
			// (get) Token: 0x06006FF6 RID: 28662 RVA: 0x0019A976 File Offset: 0x00198B76
			// (set) Token: 0x06006FF7 RID: 28663 RVA: 0x0019A997 File Offset: 0x00198B97
			public TableLayoutColumnStyleCollection ColumnStyles
			{
				get
				{
					if (this._colStyles == null)
					{
						this._colStyles = new TableLayoutColumnStyleCollection(this._container);
					}
					return this._colStyles;
				}
				set
				{
					this._colStyles = value;
					if (this._colStyles != null)
					{
						this._colStyles.EnsureOwnership(this._container);
					}
				}
			}

			// Token: 0x17001850 RID: 6224
			// (get) Token: 0x06006FF8 RID: 28664 RVA: 0x0019A9BC File Offset: 0x00198BBC
			public TableLayout.LayoutInfo[] ChildrenInfo
			{
				get
				{
					if (!this._state[TableLayout.ContainerInfo.stateChildInfoValid])
					{
						this._countFixedChildren = 0;
						this._minRowsAndColumns = 0;
						this._minColumns = 0;
						this._minRows = 0;
						ArrangedElementCollection children = this.Container.Children;
						TableLayout.LayoutInfo[] array = new TableLayout.LayoutInfo[children.Count];
						int num = 0;
						int num2 = 0;
						for (int i = 0; i < children.Count; i++)
						{
							IArrangedElement arrangedElement = children[i];
							if (!arrangedElement.ParticipatesInLayout)
							{
								num++;
							}
							else
							{
								TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(arrangedElement);
								if (layoutInfo.IsAbsolutelyPositioned)
								{
									this._countFixedChildren++;
								}
								array[num2++] = layoutInfo;
								this._minRowsAndColumns += layoutInfo.RowSpan * layoutInfo.ColumnSpan;
								if (layoutInfo.IsAbsolutelyPositioned)
								{
									this._minColumns = Math.Max(this._minColumns, layoutInfo.ColumnPosition + layoutInfo.ColumnSpan);
									this._minRows = Math.Max(this._minRows, layoutInfo.RowPosition + layoutInfo.RowSpan);
								}
							}
						}
						if (num > 0)
						{
							TableLayout.LayoutInfo[] array2 = new TableLayout.LayoutInfo[array.Length - num];
							Array.Copy(array, array2, array2.Length);
							this._childInfo = array2;
						}
						else
						{
							this._childInfo = array;
						}
						this._state[TableLayout.ContainerInfo.stateChildInfoValid] = true;
					}
					if (this._childInfo != null)
					{
						return this._childInfo;
					}
					return new TableLayout.LayoutInfo[0];
				}
			}

			// Token: 0x17001851 RID: 6225
			// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x0019AB2E File Offset: 0x00198D2E
			public bool ChildInfoValid
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildInfoValid];
				}
			}

			// Token: 0x17001852 RID: 6226
			// (get) Token: 0x06006FFA RID: 28666 RVA: 0x0019AB40 File Offset: 0x00198D40
			public TableLayout.LayoutInfo[] FixedChildrenInfo
			{
				get
				{
					TableLayout.LayoutInfo[] array = new TableLayout.LayoutInfo[this._countFixedChildren];
					if (this.HasChildWithAbsolutePositioning)
					{
						int num = 0;
						for (int i = 0; i < this._childInfo.Length; i++)
						{
							if (this._childInfo[i].IsAbsolutelyPositioned)
							{
								array[num++] = this._childInfo[i];
							}
						}
						TableLayout.Sort(array, TableLayout.PreAssignedPositionComparer.GetInstance);
					}
					return array;
				}
			}

			// Token: 0x17001853 RID: 6227
			// (get) Token: 0x06006FFB RID: 28667 RVA: 0x0019ABA0 File Offset: 0x00198DA0
			// (set) Token: 0x06006FFC RID: 28668 RVA: 0x0019ABB2 File Offset: 0x00198DB2
			public bool Valid
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateValid];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateValid] = value;
					if (!this._state[TableLayout.ContainerInfo.stateValid])
					{
						this._state[TableLayout.ContainerInfo.stateChildInfoValid] = false;
					}
				}
			}

			// Token: 0x17001854 RID: 6228
			// (get) Token: 0x06006FFD RID: 28669 RVA: 0x0019ABE8 File Offset: 0x00198DE8
			public bool HasChildWithAbsolutePositioning
			{
				get
				{
					return this._countFixedChildren > 0;
				}
			}

			// Token: 0x17001855 RID: 6229
			// (get) Token: 0x06006FFE RID: 28670 RVA: 0x0019ABF4 File Offset: 0x00198DF4
			public bool HasMultiplePercentColumns
			{
				get
				{
					if (this._colStyles != null)
					{
						bool flag = false;
						foreach (object obj in ((IEnumerable)this._colStyles))
						{
							ColumnStyle columnStyle = (ColumnStyle)obj;
							if (columnStyle.SizeType == SizeType.Percent)
							{
								if (flag)
								{
									return true;
								}
								flag = true;
							}
						}
						return false;
					}
					return false;
				}
			}

			// Token: 0x17001856 RID: 6230
			// (get) Token: 0x06006FFF RID: 28671 RVA: 0x0019AC68 File Offset: 0x00198E68
			// (set) Token: 0x06007000 RID: 28672 RVA: 0x0019AC7A File Offset: 0x00198E7A
			public bool ChildHasColumnSpan
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildHasColumnSpan];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateChildHasColumnSpan] = value;
				}
			}

			// Token: 0x17001857 RID: 6231
			// (get) Token: 0x06007001 RID: 28673 RVA: 0x0019AC8D File Offset: 0x00198E8D
			// (set) Token: 0x06007002 RID: 28674 RVA: 0x0019AC9F File Offset: 0x00198E9F
			public bool ChildHasRowSpan
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildHasRowSpan];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateChildHasRowSpan] = value;
				}
			}

			// Token: 0x06007003 RID: 28675 RVA: 0x0019ACB4 File Offset: 0x00198EB4
			public Size GetCachedPreferredSize(Size proposedContstraints, out bool isValid)
			{
				isValid = false;
				if (proposedContstraints.Height == 0 || proposedContstraints.Width == 0)
				{
					Size result = CommonProperties.xGetPreferredSizeCache(this.Container);
					if (!result.IsEmpty)
					{
						isValid = true;
						return result;
					}
				}
				return Size.Empty;
			}

			// Token: 0x04004340 RID: 17216
			private static TableLayout.Strip[] emptyStrip = new TableLayout.Strip[0];

			// Token: 0x04004341 RID: 17217
			private static readonly int stateValid = BitVector32.CreateMask();

			// Token: 0x04004342 RID: 17218
			private static readonly int stateChildInfoValid = BitVector32.CreateMask(TableLayout.ContainerInfo.stateValid);

			// Token: 0x04004343 RID: 17219
			private static readonly int stateChildHasColumnSpan = BitVector32.CreateMask(TableLayout.ContainerInfo.stateChildInfoValid);

			// Token: 0x04004344 RID: 17220
			private static readonly int stateChildHasRowSpan = BitVector32.CreateMask(TableLayout.ContainerInfo.stateChildHasColumnSpan);

			// Token: 0x04004345 RID: 17221
			private int _cellBorderWidth;

			// Token: 0x04004346 RID: 17222
			private TableLayout.Strip[] _cols = TableLayout.ContainerInfo.emptyStrip;

			// Token: 0x04004347 RID: 17223
			private TableLayout.Strip[] _rows = TableLayout.ContainerInfo.emptyStrip;

			// Token: 0x04004348 RID: 17224
			private int _maxRows;

			// Token: 0x04004349 RID: 17225
			private int _maxColumns;

			// Token: 0x0400434A RID: 17226
			private TableLayoutRowStyleCollection _rowStyles;

			// Token: 0x0400434B RID: 17227
			private TableLayoutColumnStyleCollection _colStyles;

			// Token: 0x0400434C RID: 17228
			private TableLayoutPanelGrowStyle _growStyle;

			// Token: 0x0400434D RID: 17229
			private IArrangedElement _container;

			// Token: 0x0400434E RID: 17230
			private TableLayout.LayoutInfo[] _childInfo;

			// Token: 0x0400434F RID: 17231
			private int _countFixedChildren;

			// Token: 0x04004350 RID: 17232
			private int _minRowsAndColumns;

			// Token: 0x04004351 RID: 17233
			private int _minColumns;

			// Token: 0x04004352 RID: 17234
			private int _minRows;

			// Token: 0x04004353 RID: 17235
			private BitVector32 _state;
		}

		// Token: 0x02000858 RID: 2136
		private abstract class SizeProxy
		{
			// Token: 0x17001858 RID: 6232
			// (get) Token: 0x06007005 RID: 28677 RVA: 0x0019AD47 File Offset: 0x00198F47
			// (set) Token: 0x06007006 RID: 28678 RVA: 0x0019AD4F File Offset: 0x00198F4F
			public TableLayout.Strip Strip
			{
				get
				{
					return this.strip;
				}
				set
				{
					this.strip = value;
				}
			}

			// Token: 0x17001859 RID: 6233
			// (get) Token: 0x06007007 RID: 28679
			// (set) Token: 0x06007008 RID: 28680
			public abstract int Size { get; set; }

			// Token: 0x04004354 RID: 17236
			protected TableLayout.Strip strip;
		}

		// Token: 0x02000859 RID: 2137
		private class MinSizeProxy : TableLayout.SizeProxy
		{
			// Token: 0x1700185A RID: 6234
			// (get) Token: 0x0600700A RID: 28682 RVA: 0x0019AD58 File Offset: 0x00198F58
			// (set) Token: 0x0600700B RID: 28683 RVA: 0x0019AD65 File Offset: 0x00198F65
			public override int Size
			{
				get
				{
					return this.strip.MinSize;
				}
				set
				{
					this.strip.MinSize = value;
				}
			}

			// Token: 0x1700185B RID: 6235
			// (get) Token: 0x0600700C RID: 28684 RVA: 0x0019AD73 File Offset: 0x00198F73
			public static TableLayout.MinSizeProxy GetInstance
			{
				get
				{
					return TableLayout.MinSizeProxy.instance;
				}
			}

			// Token: 0x04004355 RID: 17237
			private static readonly TableLayout.MinSizeProxy instance = new TableLayout.MinSizeProxy();
		}

		// Token: 0x0200085A RID: 2138
		private class MaxSizeProxy : TableLayout.SizeProxy
		{
			// Token: 0x1700185C RID: 6236
			// (get) Token: 0x0600700F RID: 28687 RVA: 0x0019AD8E File Offset: 0x00198F8E
			// (set) Token: 0x06007010 RID: 28688 RVA: 0x0019AD9B File Offset: 0x00198F9B
			public override int Size
			{
				get
				{
					return this.strip.MaxSize;
				}
				set
				{
					this.strip.MaxSize = value;
				}
			}

			// Token: 0x1700185D RID: 6237
			// (get) Token: 0x06007011 RID: 28689 RVA: 0x0019ADA9 File Offset: 0x00198FA9
			public static TableLayout.MaxSizeProxy GetInstance
			{
				get
				{
					return TableLayout.MaxSizeProxy.instance;
				}
			}

			// Token: 0x04004356 RID: 17238
			private static readonly TableLayout.MaxSizeProxy instance = new TableLayout.MaxSizeProxy();
		}

		// Token: 0x0200085B RID: 2139
		private abstract class SpanComparer : IComparer
		{
			// Token: 0x06007014 RID: 28692
			public abstract int GetSpan(TableLayout.LayoutInfo layoutInfo);

			// Token: 0x06007015 RID: 28693 RVA: 0x0019ADBC File Offset: 0x00198FBC
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				return this.GetSpan(layoutInfo) - this.GetSpan(layoutInfo2);
			}
		}

		// Token: 0x0200085C RID: 2140
		private class RowSpanComparer : TableLayout.SpanComparer
		{
			// Token: 0x06007017 RID: 28695 RVA: 0x0019ADE6 File Offset: 0x00198FE6
			public override int GetSpan(TableLayout.LayoutInfo layoutInfo)
			{
				return layoutInfo.RowSpan;
			}

			// Token: 0x1700185E RID: 6238
			// (get) Token: 0x06007018 RID: 28696 RVA: 0x0019ADEE File Offset: 0x00198FEE
			public static TableLayout.RowSpanComparer GetInstance
			{
				get
				{
					return TableLayout.RowSpanComparer.instance;
				}
			}

			// Token: 0x04004357 RID: 17239
			private static readonly TableLayout.RowSpanComparer instance = new TableLayout.RowSpanComparer();
		}

		// Token: 0x0200085D RID: 2141
		private class ColumnSpanComparer : TableLayout.SpanComparer
		{
			// Token: 0x0600701B RID: 28699 RVA: 0x0019AE09 File Offset: 0x00199009
			public override int GetSpan(TableLayout.LayoutInfo layoutInfo)
			{
				return layoutInfo.ColumnSpan;
			}

			// Token: 0x1700185F RID: 6239
			// (get) Token: 0x0600701C RID: 28700 RVA: 0x0019AE11 File Offset: 0x00199011
			public static TableLayout.ColumnSpanComparer GetInstance
			{
				get
				{
					return TableLayout.ColumnSpanComparer.instance;
				}
			}

			// Token: 0x04004358 RID: 17240
			private static readonly TableLayout.ColumnSpanComparer instance = new TableLayout.ColumnSpanComparer();
		}

		// Token: 0x0200085E RID: 2142
		private class PostAssignedPositionComparer : IComparer
		{
			// Token: 0x17001860 RID: 6240
			// (get) Token: 0x0600701F RID: 28703 RVA: 0x0019AE24 File Offset: 0x00199024
			public static TableLayout.PostAssignedPositionComparer GetInstance
			{
				get
				{
					return TableLayout.PostAssignedPositionComparer.instance;
				}
			}

			// Token: 0x06007020 RID: 28704 RVA: 0x0019AE2C File Offset: 0x0019902C
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				if (layoutInfo.RowStart < layoutInfo2.RowStart)
				{
					return -1;
				}
				if (layoutInfo.RowStart > layoutInfo2.RowStart)
				{
					return 1;
				}
				if (layoutInfo.ColumnStart < layoutInfo2.ColumnStart)
				{
					return -1;
				}
				if (layoutInfo.ColumnStart > layoutInfo2.ColumnStart)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04004359 RID: 17241
			private static readonly TableLayout.PostAssignedPositionComparer instance = new TableLayout.PostAssignedPositionComparer();
		}

		// Token: 0x0200085F RID: 2143
		private class PreAssignedPositionComparer : IComparer
		{
			// Token: 0x17001861 RID: 6241
			// (get) Token: 0x06007023 RID: 28707 RVA: 0x0019AE94 File Offset: 0x00199094
			public static TableLayout.PreAssignedPositionComparer GetInstance
			{
				get
				{
					return TableLayout.PreAssignedPositionComparer.instance;
				}
			}

			// Token: 0x06007024 RID: 28708 RVA: 0x0019AE9C File Offset: 0x0019909C
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				if (layoutInfo.RowPosition < layoutInfo2.RowPosition)
				{
					return -1;
				}
				if (layoutInfo.RowPosition > layoutInfo2.RowPosition)
				{
					return 1;
				}
				if (layoutInfo.ColumnPosition < layoutInfo2.ColumnPosition)
				{
					return -1;
				}
				if (layoutInfo.ColumnPosition > layoutInfo2.ColumnPosition)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x0400435A RID: 17242
			private static readonly TableLayout.PreAssignedPositionComparer instance = new TableLayout.PreAssignedPositionComparer();
		}

		// Token: 0x02000860 RID: 2144
		private sealed class ReservationGrid
		{
			// Token: 0x06007027 RID: 28711 RVA: 0x0019AF04 File Offset: 0x00199104
			public bool IsReserved(int column, int rowOffset)
			{
				return rowOffset < this._rows.Count && column < ((BitArray)this._rows[rowOffset]).Length && ((BitArray)this._rows[rowOffset])[column];
			}

			// Token: 0x06007028 RID: 28712 RVA: 0x0019AF54 File Offset: 0x00199154
			public void Reserve(int column, int rowOffset)
			{
				while (rowOffset >= this._rows.Count)
				{
					this._rows.Add(new BitArray(this._numColumns));
				}
				if (column >= ((BitArray)this._rows[rowOffset]).Length)
				{
					((BitArray)this._rows[rowOffset]).Length = column + 1;
					if (column >= this._numColumns)
					{
						this._numColumns = column + 1;
					}
				}
				((BitArray)this._rows[rowOffset])[column] = true;
			}

			// Token: 0x06007029 RID: 28713 RVA: 0x0019AFE4 File Offset: 0x001991E4
			public void ReserveAll(TableLayout.LayoutInfo layoutInfo, int rowStop, int colStop)
			{
				for (int i = 1; i < rowStop - layoutInfo.RowStart; i++)
				{
					for (int j = layoutInfo.ColumnStart; j < colStop; j++)
					{
						this.Reserve(j, i);
					}
				}
			}

			// Token: 0x0600702A RID: 28714 RVA: 0x0019B01D File Offset: 0x0019921D
			public void AdvanceRow()
			{
				if (this._rows.Count > 0)
				{
					this._rows.RemoveAt(0);
				}
			}

			// Token: 0x0400435B RID: 17243
			private int _numColumns = 1;

			// Token: 0x0400435C RID: 17244
			private ArrayList _rows = new ArrayList();
		}

		// Token: 0x02000861 RID: 2145
		internal struct Strip
		{
			// Token: 0x17001862 RID: 6242
			// (get) Token: 0x0600702C RID: 28716 RVA: 0x0019B053 File Offset: 0x00199253
			// (set) Token: 0x0600702D RID: 28717 RVA: 0x0019B05B File Offset: 0x0019925B
			public int MinSize
			{
				get
				{
					return this._minSize;
				}
				set
				{
					this._minSize = value;
				}
			}

			// Token: 0x17001863 RID: 6243
			// (get) Token: 0x0600702E RID: 28718 RVA: 0x0019B064 File Offset: 0x00199264
			// (set) Token: 0x0600702F RID: 28719 RVA: 0x0019B06C File Offset: 0x0019926C
			public int MaxSize
			{
				get
				{
					return this._maxSize;
				}
				set
				{
					this._maxSize = value;
				}
			}

			// Token: 0x17001864 RID: 6244
			// (get) Token: 0x06007030 RID: 28720 RVA: 0x0019B075 File Offset: 0x00199275
			// (set) Token: 0x06007031 RID: 28721 RVA: 0x0019B07D File Offset: 0x0019927D
			public bool IsStart
			{
				get
				{
					return this._isStart;
				}
				set
				{
					this._isStart = value;
				}
			}

			// Token: 0x0400435D RID: 17245
			private int _maxSize;

			// Token: 0x0400435E RID: 17246
			private int _minSize;

			// Token: 0x0400435F RID: 17247
			private bool _isStart;
		}
	}
}
