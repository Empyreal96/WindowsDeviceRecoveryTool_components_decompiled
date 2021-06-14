using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000360 RID: 864
	internal sealed class FixedSOMPageConstructor
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x000CE8A8 File Offset: 0x000CCAA8
		public FixedSOMPageConstructor(FixedPage fixedPage, int pageIndex)
		{
			this._fixedPage = fixedPage;
			this._pageIndex = pageIndex;
			this._fixedSOMPage = new FixedSOMPage();
			this._fixedSOMPage.CultureInfo = this._fixedPage.Language.GetCompatibleCulture();
			this._fixedNodes = new List<FixedNode>();
			this._lines = new FixedSOMLineCollection();
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000CE908 File Offset: 0x000CCB08
		public FixedSOMPage ConstructPageStructure(List<FixedNode> fixedNodes)
		{
			foreach (FixedNode fixedNode in fixedNodes)
			{
				DependencyObject element = this._fixedPage.GetElement(fixedNode);
				if (element is Glyphs)
				{
					this._ProcessGlyphsElement(element as Glyphs, fixedNode);
				}
				else if (element is Image || (element is Path && (element as Path).Fill is ImageBrush))
				{
					this._ProcessImage(element, fixedNode);
				}
			}
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._fixedSOMPage.SemanticBoxes)
			{
				FixedSOMContainer fixedSOMContainer = fixedSOMSemanticBox as FixedSOMContainer;
				fixedSOMContainer.SemanticBoxes.Sort();
			}
			this._DetectTables();
			this._CombinePass();
			this._CreateGroups(this._fixedSOMPage);
			this._fixedSOMPage.SemanticBoxes.Sort();
			return this._fixedSOMPage;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000CEA24 File Offset: 0x000CCC24
		public void ProcessPath(Path path, Matrix transform)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Geometry data = path.Data;
			bool flag = path.Fill != null;
			bool flag2 = path.Stroke != null;
			if (data == null || (!flag && !flag2))
			{
				return;
			}
			Transform renderTransform = path.RenderTransform;
			if (renderTransform != null)
			{
				transform *= renderTransform.Value;
			}
			if (flag && this._ProcessFilledRect(transform, data.Bounds))
			{
				flag = false;
				if (!flag2)
				{
					return;
				}
			}
			StreamGeometry streamGeometry = data as StreamGeometry;
			if (streamGeometry != null)
			{
				if (this._geometryWalker == null)
				{
					this._geometryWalker = new GeometryWalker(this);
				}
				this._geometryWalker.FindLines(streamGeometry, flag2, flag, transform);
				return;
			}
			PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(data);
			if (pathGeometry != null)
			{
				if (flag)
				{
					this._ProcessSolidPath(transform, pathGeometry);
				}
				if (flag2)
				{
					this._ProcessOutlinePath(transform, pathGeometry);
				}
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x000CEAE9 File Offset: 0x000CCCE9
		public FixedSOMPage FixedSOMPage
		{
			get
			{
				return this._fixedSOMPage;
			}
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000CEAF4 File Offset: 0x000CCCF4
		private void _ProcessImage(DependencyObject obj, FixedNode fixedNode)
		{
			Image image;
			Path path;
			for (;;)
			{
				image = (obj as Image);
				if (image != null)
				{
					break;
				}
				path = (obj as Path);
				if (path != null)
				{
					goto Block_1;
				}
			}
			FixedSOMImage image2 = FixedSOMImage.Create(this._fixedPage, image, fixedNode);
			goto IL_34;
			Block_1:
			image2 = FixedSOMImage.Create(this._fixedPage, path, fixedNode);
			IL_34:
			FixedSOMFixedBlock fixedSOMFixedBlock = new FixedSOMFixedBlock(this._fixedSOMPage);
			fixedSOMFixedBlock.AddImage(image2);
			this._fixedSOMPage.AddFixedBlock(fixedSOMFixedBlock);
			this._currentFixedBlock = fixedSOMFixedBlock;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000CEB5C File Offset: 0x000CCD5C
		private void _ProcessGlyphsElement(Glyphs glyphs, FixedNode node)
		{
			string unicodeString = glyphs.UnicodeString;
			if (unicodeString.Length == 0 || glyphs.FontRenderingEmSize <= 0.0)
			{
				return;
			}
			GlyphRun glyphRun = glyphs.ToGlyphRun();
			if (glyphRun == null)
			{
				return;
			}
			Rect rect = glyphRun.ComputeAlignmentBox();
			rect.Offset(glyphs.OriginX, glyphs.OriginY);
			GlyphTypeface glyphTypeface = glyphRun.GlyphTypeface;
			GeneralTransform trans = glyphs.TransformToAncestor(this._fixedPage);
			int num = -1;
			double num2 = 0.0;
			int num3 = 0;
			int num4 = 0;
			double num5 = rect.Left;
			do
			{
				num = unicodeString.IndexOf(" ", num + 1, unicodeString.Length - num - 1, StringComparison.Ordinal);
				if (num >= 0)
				{
					int num6;
					if (glyphRun.ClusterMap != null && glyphRun.ClusterMap.Count > 0)
					{
						num6 = (int)glyphRun.ClusterMap[num];
					}
					else
					{
						num6 = num;
					}
					double num7 = glyphTypeface.AdvanceWidths[glyphRun.GlyphIndices[num6]] * glyphRun.FontRenderingEmSize;
					double num8 = glyphRun.AdvanceWidths[num6];
					if (num8 / num7 > 2.0)
					{
						double num9 = 0.0;
						for (int i = num3; i < num6; i++)
						{
							num9 += glyphRun.AdvanceWidths[i];
						}
						num2 += num9;
						num3 = num6 + 1;
						if (this._lines.IsVerticallySeparated(glyphRun.BaselineOrigin.X + num2, rect.Top, glyphRun.BaselineOrigin.X + num2 + num8, rect.Bottom))
						{
							Rect boundingRect = new Rect(num5, rect.Top, num9 + num7, rect.Height);
							int endIndex = num;
							if ((num == 0 || unicodeString[num - 1] == ' ') && num != unicodeString.Length - 1)
							{
								endIndex = num + 1;
							}
							this._CreateTextRun(boundingRect, trans, glyphs, node, num4, endIndex);
							num5 = num5 + num9 + num8;
							num4 = num + 1;
						}
						num2 += num8;
					}
				}
			}
			while (num >= 0 && num < unicodeString.Length - 1);
			if (num4 < unicodeString.Length)
			{
				Rect boundingRect2 = new Rect(num5, rect.Top, rect.Right - num5, rect.Height);
				this._CreateTextRun(boundingRect2, trans, glyphs, node, num4, unicodeString.Length);
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000CEDC0 File Offset: 0x000CCFC0
		private void _CreateTextRun(Rect boundingRect, GeneralTransform trans, Glyphs glyphs, FixedNode node, int startIndex, int endIndex)
		{
			if (startIndex < endIndex)
			{
				FixedSOMTextRun textRun = FixedSOMTextRun.Create(boundingRect, trans, glyphs, node, startIndex, endIndex, true);
				FixedSOMFixedBlock fixedSOMFixedBlock = this._GetContainingFixedBlock(textRun);
				if (fixedSOMFixedBlock == null)
				{
					fixedSOMFixedBlock = new FixedSOMFixedBlock(this._fixedSOMPage);
					fixedSOMFixedBlock.AddTextRun(textRun);
					this._fixedSOMPage.AddFixedBlock(fixedSOMFixedBlock);
				}
				else
				{
					fixedSOMFixedBlock.AddTextRun(textRun);
				}
				this._currentFixedBlock = fixedSOMFixedBlock;
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000CEE20 File Offset: 0x000CD020
		private FixedSOMFixedBlock _GetContainingFixedBlock(FixedSOMTextRun textRun)
		{
			FixedSOMFixedBlock result = null;
			if (this._currentFixedBlock == null)
			{
				return null;
			}
			if (this._currentFixedBlock != null && this._IsCombinable(this._currentFixedBlock, textRun))
			{
				result = this._currentFixedBlock;
			}
			else
			{
				Rect boundingRect = textRun.BoundingRect;
				Rect boundingRect2 = this._currentFixedBlock.BoundingRect;
				if (Math.Abs(boundingRect.Left - boundingRect2.Left) <= textRun.DefaultCharWidth || Math.Abs(boundingRect.Right - boundingRect2.Right) <= textRun.DefaultCharWidth)
				{
					return null;
				}
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._fixedSOMPage.SemanticBoxes)
				{
					if (fixedSOMSemanticBox is FixedSOMFixedBlock && this._IsCombinable(fixedSOMSemanticBox as FixedSOMFixedBlock, textRun))
					{
						result = (fixedSOMSemanticBox as FixedSOMFixedBlock);
					}
				}
			}
			return result;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000CEF10 File Offset: 0x000CD110
		private bool _IsCombinable(FixedSOMFixedBlock fixedBlock, FixedSOMTextRun textRun)
		{
			if (fixedBlock.SemanticBoxes.Count == 0)
			{
				return false;
			}
			if (fixedBlock.IsFloatingImage)
			{
				return false;
			}
			Rect boundingRect = textRun.BoundingRect;
			Rect boundingRect2 = fixedBlock.BoundingRect;
			FixedSOMTextRun fixedSOMTextRun = null;
			FixedSOMTextRun fixedSOMTextRun2 = fixedBlock.SemanticBoxes[fixedBlock.SemanticBoxes.Count - 1] as FixedSOMTextRun;
			if (fixedSOMTextRun2 != null && boundingRect.Bottom <= fixedSOMTextRun2.BoundingRect.Top)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			double num = boundingRect.Height * 0.2;
			if (boundingRect.Bottom - num < boundingRect2.Top)
			{
				flag = true;
				fixedSOMTextRun = (fixedBlock.SemanticBoxes[0] as FixedSOMTextRun);
			}
			else if (boundingRect.Top + num > boundingRect2.Bottom)
			{
				flag2 = true;
				fixedSOMTextRun = (fixedBlock.SemanticBoxes[fixedBlock.SemanticBoxes.Count - 1] as FixedSOMTextRun);
			}
			if ((fixedBlock.IsWhiteSpace || textRun.IsWhiteSpace) && (fixedBlock != this._currentFixedBlock || fixedSOMTextRun != null || !this._IsSpatiallyCombinable(boundingRect2, boundingRect, textRun.DefaultCharWidth * 3.0, 0.0)))
			{
				return false;
			}
			if (fixedBlock.Matrix.M11 != textRun.Matrix.M11 || fixedBlock.Matrix.M12 != textRun.Matrix.M12 || fixedBlock.Matrix.M21 != textRun.Matrix.M21 || fixedBlock.Matrix.M22 != textRun.Matrix.M22)
			{
				return false;
			}
			if (fixedSOMTextRun != null)
			{
				double num2 = fixedBlock.LineHeight / boundingRect.Height;
				if (num2 < 1.0)
				{
					num2 = 1.0 / num2;
				}
				if (num2 > 1.1 && !FixedTextBuilder.IsSameLine(fixedSOMTextRun.BoundingRect.Top - boundingRect.Top, boundingRect.Height, fixedSOMTextRun.BoundingRect.Height))
				{
					return false;
				}
			}
			double num3 = textRun.DefaultCharWidth;
			if (num3 < 1.0)
			{
				num3 = 1.0;
			}
			double num4 = fixedBlock.LineHeight / boundingRect.Height;
			if (num4 < 1.0)
			{
				num4 = 1.0 / num4;
			}
			double inflateH;
			if (fixedBlock == this._currentFixedBlock && fixedSOMTextRun == null && num4 < 1.5)
			{
				inflateH = 200.0;
			}
			else
			{
				inflateH = num3 * 1.5;
			}
			if (!this._IsSpatiallyCombinable(boundingRect2, boundingRect, inflateH, boundingRect.Height * 0.7))
			{
				return false;
			}
			FixedSOMElement fixedSOMElement = fixedBlock.SemanticBoxes[fixedBlock.SemanticBoxes.Count - 1] as FixedSOMElement;
			if (fixedSOMElement != null && fixedSOMElement.FixedNode.CompareTo(textRun.FixedNode) == 0)
			{
				return false;
			}
			if (flag || flag2)
			{
				double num5 = boundingRect.Height * 0.2;
				double top;
				double bottom;
				if (flag2)
				{
					top = boundingRect2.Bottom - num5;
					bottom = boundingRect.Top + num5;
				}
				else
				{
					top = boundingRect.Bottom - num5;
					bottom = boundingRect2.Top + num5;
				}
				double left = (boundingRect2.Left > boundingRect.Left) ? boundingRect2.Left : boundingRect.Left;
				double right = (boundingRect2.Right < boundingRect.Right) ? boundingRect2.Right : boundingRect.Right;
				return !this._lines.IsHorizontallySeparated(left, top, right, bottom);
			}
			double num6 = (boundingRect2.Right < boundingRect.Right) ? boundingRect2.Right : boundingRect.Right;
			double num7 = (boundingRect2.Left > boundingRect.Left) ? boundingRect2.Left : boundingRect.Left;
			if (FrameworkAppContextSwitches.OptOutOfFixedDocumentModelConstructionFix)
			{
				if (num6 > num7)
				{
					double num8 = num6;
					num6 = num7;
					num7 = num8;
				}
				return !this._lines.IsVerticallySeparated(num6, boundingRect.Top, num7, boundingRect.Bottom);
			}
			return num6 >= num7 || !this._lines.IsVerticallySeparated(num6, boundingRect.Top, num7, boundingRect.Bottom);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000CF37C File Offset: 0x000CD57C
		private bool _IsSpatiallyCombinable(FixedSOMSemanticBox box1, FixedSOMSemanticBox box2, double inflateH, double inflateV)
		{
			return this._IsSpatiallyCombinable(box1.BoundingRect, box2.BoundingRect, inflateH, inflateV);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000CF393 File Offset: 0x000CD593
		private bool _IsSpatiallyCombinable(Rect rect1, Rect rect2, double inflateH, double inflateV)
		{
			if (rect1.IntersectsWith(rect2))
			{
				return true;
			}
			rect1.Inflate(inflateH, inflateV);
			return rect1.IntersectsWith(rect2);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000CF3B8 File Offset: 0x000CD5B8
		private void _DetectTables()
		{
			double minLineSeparation = FixedSOMLineRanges.MinLineSeparation;
			List<FixedSOMLineRanges> horizontalLines = this._lines.HorizontalLines;
			List<FixedSOMLineRanges> verticalLines = this._lines.VerticalLines;
			if (horizontalLines.Count < 2 || verticalLines.Count < 2)
			{
				return;
			}
			List<FixedSOMTableRow> list = new List<FixedSOMTableRow>();
			FixedSOMTableRow fixedSOMTableRow = null;
			for (int i = 0; i < horizontalLines.Count; i++)
			{
				int j = 0;
				int num = -1;
				int num2 = -1;
				int num3 = -1;
				int num4 = -1;
				double line = horizontalLines[i].Line + minLineSeparation;
				for (int k = 0; k < horizontalLines[i].Count; k++)
				{
					double num5 = horizontalLines[i].Start[k] - minLineSeparation;
					double num6 = horizontalLines[i].End[k] + minLineSeparation;
					int num7 = -1;
					while (j < verticalLines.Count)
					{
						if (verticalLines[j].Line >= num5)
						{
							break;
						}
						j++;
					}
					while (j < verticalLines.Count && verticalLines[j].Line < num6)
					{
						int lineAt = verticalLines[j].GetLineAt(line);
						if (lineAt != -1)
						{
							double num8 = verticalLines[j].End[lineAt];
							if (num7 != -1 && horizontalLines[num].Line < num8 + minLineSeparation && horizontalLines[num].End[num2] + minLineSeparation > verticalLines[j].Line)
							{
								double line2 = horizontalLines[i].Line;
								double line3 = horizontalLines[num].Line;
								double line4 = verticalLines[num7].Line;
								double line5 = verticalLines[j].Line;
								FixedSOMTableCell cell = new FixedSOMTableCell(line4, line2, line5, line3);
								if (num7 != num4 || num != num3)
								{
									fixedSOMTableRow = new FixedSOMTableRow();
									list.Add(fixedSOMTableRow);
								}
								fixedSOMTableRow.AddCell(cell);
								num4 = j;
								num3 = num;
							}
							num7 = -1;
							num = i + 1;
							while (num < horizontalLines.Count && horizontalLines[num].Line < num8 + minLineSeparation)
							{
								num2 = horizontalLines[num].GetLineAt(verticalLines[j].Line + minLineSeparation);
								if (num2 != -1)
								{
									num7 = j;
									break;
								}
								num++;
							}
						}
						j++;
					}
				}
			}
			this._FillTables(list);
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000CF628 File Offset: 0x000CD828
		public void _AddLine(Point startP, Point endP, Matrix transform)
		{
			startP = transform.Transform(startP);
			endP = transform.Transform(endP);
			if (startP.X == endP.X)
			{
				this._lines.AddVertical(startP, endP);
				return;
			}
			if (startP.Y == endP.Y)
			{
				this._lines.AddHorizontal(startP, endP);
			}
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000CF684 File Offset: 0x000CD884
		private void _CombinePass()
		{
			if (this._fixedSOMPage.SemanticBoxes.Count < 2)
			{
				return;
			}
			int count;
			do
			{
				count = this._fixedSOMPage.SemanticBoxes.Count;
				List<FixedSOMSemanticBox> semanticBoxes = this._fixedSOMPage.SemanticBoxes;
				for (int i = 0; i < semanticBoxes.Count; i++)
				{
					FixedSOMTable fixedSOMTable = semanticBoxes[i] as FixedSOMTable;
					if (fixedSOMTable != null)
					{
						for (int j = i + 1; j < semanticBoxes.Count; j++)
						{
							FixedSOMTable fixedSOMTable2 = semanticBoxes[j] as FixedSOMTable;
							if (fixedSOMTable2 != null && fixedSOMTable.AddContainer(fixedSOMTable2))
							{
								semanticBoxes.Remove(fixedSOMTable2);
							}
						}
					}
					else
					{
						FixedSOMFixedBlock fixedSOMFixedBlock = semanticBoxes[i] as FixedSOMFixedBlock;
						if (fixedSOMFixedBlock != null && !fixedSOMFixedBlock.IsFloatingImage)
						{
							for (int k = i + 1; k < semanticBoxes.Count; k++)
							{
								FixedSOMFixedBlock fixedSOMFixedBlock2 = semanticBoxes[k] as FixedSOMFixedBlock;
								if (fixedSOMFixedBlock2 != null && !fixedSOMFixedBlock2.IsFloatingImage && fixedSOMFixedBlock2.Matrix.Equals(fixedSOMFixedBlock.Matrix) && this._IsSpatiallyCombinable(fixedSOMFixedBlock, fixedSOMFixedBlock2, 0.0, 0.0))
								{
									fixedSOMFixedBlock.CombineWith(fixedSOMFixedBlock2);
									semanticBoxes.Remove(fixedSOMFixedBlock2);
								}
							}
						}
					}
				}
			}
			while (this._fixedSOMPage.SemanticBoxes.Count > 1 && this._fixedSOMPage.SemanticBoxes.Count != count);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000CF7F0 File Offset: 0x000CD9F0
		internal bool _ProcessFilledRect(Matrix transform, Rect bounds)
		{
			if (bounds.Height > bounds.Width && bounds.Width < 10.0 && bounds.Height > bounds.Width * 5.0)
			{
				double x = bounds.Left + 0.5 * bounds.Width;
				this._AddLine(new Point(x, bounds.Top), new Point(x, bounds.Bottom), transform);
				return true;
			}
			if (bounds.Height < 10.0 && bounds.Width > bounds.Height * 5.0)
			{
				double y = bounds.Top + 0.5 * bounds.Height;
				this._AddLine(new Point(bounds.Left, y), new Point(bounds.Right, y), transform);
				return true;
			}
			return false;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000CF8E0 File Offset: 0x000CDAE0
		private void _ProcessSolidPath(Matrix transform, PathGeometry pathGeom)
		{
			PathFigureCollection figures = pathGeom.Figures;
			if (figures != null && figures.Count > 1)
			{
				foreach (PathFigure value in figures)
				{
					this._ProcessFilledRect(transform, new PathGeometry
					{
						Figures = 
						{
							value
						}
					}.Bounds);
				}
			}
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000CF95C File Offset: 0x000CDB5C
		private void _ProcessOutlinePath(Matrix transform, PathGeometry pathGeom)
		{
			PathFigureCollection figures = pathGeom.Figures;
			foreach (PathFigure pathFigure in figures)
			{
				PathSegmentCollection segments = pathFigure.Segments;
				Point startPoint = pathFigure.StartPoint;
				Point startP = startPoint;
				foreach (PathSegment pathSegment in segments)
				{
					if (pathSegment is ArcSegment)
					{
						startP = (pathSegment as ArcSegment).Point;
					}
					else if (pathSegment is BezierSegment)
					{
						startP = (pathSegment as BezierSegment).Point3;
					}
					else if (pathSegment is LineSegment)
					{
						Point point = (pathSegment as LineSegment).Point;
						this._AddLine(startP, point, transform);
						startP = point;
					}
					else if (pathSegment is PolyBezierSegment)
					{
						PointCollection points = (pathSegment as PolyBezierSegment).Points;
						startP = points[points.Count - 1];
					}
					else
					{
						if (pathSegment is PolyLineSegment)
						{
							PointCollection points2 = (pathSegment as PolyLineSegment).Points;
							using (PointCollection.Enumerator enumerator3 = points2.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									Point point2 = enumerator3.Current;
									this._AddLine(startP, point2, transform);
									startP = point2;
								}
								continue;
							}
						}
						if (pathSegment is PolyQuadraticBezierSegment)
						{
							PointCollection points3 = (pathSegment as PolyQuadraticBezierSegment).Points;
							startP = points3[points3.Count - 1];
						}
						else if (pathSegment is QuadraticBezierSegment)
						{
							startP = (pathSegment as QuadraticBezierSegment).Point2;
						}
					}
				}
				if (pathFigure.IsClosed)
				{
					this._AddLine(startP, startPoint, transform);
				}
			}
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000CFB6C File Offset: 0x000CDD6C
		private void _FillTables(List<FixedSOMTableRow> tableRows)
		{
			List<FixedSOMTable> list = new List<FixedSOMTable>();
			foreach (FixedSOMTableRow fixedSOMTableRow in tableRows)
			{
				FixedSOMTable fixedSOMTable = null;
				double num = 0.01;
				foreach (FixedSOMTable fixedSOMTable2 in list)
				{
					if (Math.Abs(fixedSOMTable2.BoundingRect.Left - fixedSOMTableRow.BoundingRect.Left) < num && Math.Abs(fixedSOMTable2.BoundingRect.Right - fixedSOMTableRow.BoundingRect.Right) < num && Math.Abs(fixedSOMTable2.BoundingRect.Bottom - fixedSOMTableRow.BoundingRect.Top) < num)
					{
						fixedSOMTable = fixedSOMTable2;
						break;
					}
				}
				if (fixedSOMTable == null)
				{
					fixedSOMTable = new FixedSOMTable(this._fixedSOMPage);
					list.Add(fixedSOMTable);
				}
				fixedSOMTable.AddRow(fixedSOMTableRow);
			}
			for (int i = 0; i < list.Count - 1; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[i].BoundingRect.Contains(list[j].BoundingRect) && list[i].AddContainer(list[j]))
					{
						list.RemoveAt(j--);
					}
					else if (list[j].BoundingRect.Contains(list[i].BoundingRect) && list[j].AddContainer(list[i]))
					{
						list.RemoveAt(i--);
						if (i < 0)
						{
							break;
						}
					}
				}
			}
			foreach (FixedSOMTable fixedSOMTable3 in list)
			{
				if (!fixedSOMTable3.IsSingleCelled)
				{
					bool flag = false;
					int k = 0;
					while (k < this._fixedSOMPage.SemanticBoxes.Count)
					{
						if (this._fixedSOMPage.SemanticBoxes[k] is FixedSOMFixedBlock && fixedSOMTable3.AddContainer(this._fixedSOMPage.SemanticBoxes[k] as FixedSOMContainer))
						{
							this._fixedSOMPage.SemanticBoxes.RemoveAt(k);
							flag = true;
						}
						else
						{
							k++;
						}
					}
					if (flag)
					{
						fixedSOMTable3.DeleteEmptyRows();
						fixedSOMTable3.DeleteEmptyColumns();
						foreach (FixedSOMSemanticBox fixedSOMSemanticBox in fixedSOMTable3.SemanticBoxes)
						{
							FixedSOMTableRow fixedSOMTableRow2 = (FixedSOMTableRow)fixedSOMSemanticBox;
							foreach (FixedSOMSemanticBox fixedSOMSemanticBox2 in fixedSOMTableRow2.SemanticBoxes)
							{
								FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMSemanticBox2;
								int l = 0;
								while (l < fixedSOMTableCell.SemanticBoxes.Count)
								{
									FixedSOMTable fixedSOMTable4 = fixedSOMTableCell.SemanticBoxes[l] as FixedSOMTable;
									if (fixedSOMTable4 != null && fixedSOMTable4.IsEmpty)
									{
										fixedSOMTableCell.SemanticBoxes.Remove(fixedSOMTable4);
									}
									else
									{
										l++;
									}
								}
								this._CreateGroups(fixedSOMTableCell);
								fixedSOMTableCell.SemanticBoxes.Sort();
							}
						}
						this._fixedSOMPage.AddTable(fixedSOMTable3);
					}
				}
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000CFF84 File Offset: 0x000CE184
		private void _CreateGroups(FixedSOMContainer container)
		{
			if (container.SemanticBoxes.Count > 0)
			{
				List<FixedSOMSemanticBox> list = new List<FixedSOMSemanticBox>();
				FixedSOMGroup fixedSOMGroup = new FixedSOMGroup(this._fixedSOMPage);
				FixedSOMPageElement fixedSOMPageElement = container.SemanticBoxes[0] as FixedSOMPageElement;
				fixedSOMGroup.AddContainer(fixedSOMPageElement);
				list.Add(fixedSOMGroup);
				for (int i = 1; i < container.SemanticBoxes.Count; i++)
				{
					FixedSOMPageElement fixedSOMPageElement2 = container.SemanticBoxes[i] as FixedSOMPageElement;
					if (!this._IsSpatiallyCombinable(fixedSOMPageElement, fixedSOMPageElement2, 0.0, 30.0) || fixedSOMPageElement2.BoundingRect.Top < fixedSOMPageElement.BoundingRect.Top)
					{
						fixedSOMGroup = new FixedSOMGroup(this._fixedSOMPage);
						list.Add(fixedSOMGroup);
					}
					fixedSOMGroup.AddContainer(fixedSOMPageElement2);
					fixedSOMPageElement = fixedSOMPageElement2;
				}
				container.SemanticBoxes = list;
			}
		}

		// Token: 0x04001DE0 RID: 7648
		private FixedSOMFixedBlock _currentFixedBlock;

		// Token: 0x04001DE1 RID: 7649
		private int _pageIndex;

		// Token: 0x04001DE2 RID: 7650
		private FixedPage _fixedPage;

		// Token: 0x04001DE3 RID: 7651
		private FixedSOMPage _fixedSOMPage;

		// Token: 0x04001DE4 RID: 7652
		private List<FixedNode> _fixedNodes;

		// Token: 0x04001DE5 RID: 7653
		private FixedSOMLineCollection _lines;

		// Token: 0x04001DE6 RID: 7654
		private GeometryWalker _geometryWalker;
	}
}
