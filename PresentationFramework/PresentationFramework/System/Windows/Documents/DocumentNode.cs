using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003CE RID: 974
	internal class DocumentNode
	{
		// Token: 0x0600342E RID: 13358 RVA: 0x000E7E8C File Offset: 0x000E608C
		internal DocumentNode(DocumentNodeType documentNodeType)
		{
			this._type = documentNodeType;
			this._bPending = true;
			this._childCount = 0;
			this._index = -1;
			this._dna = null;
			this._parent = null;
			this._bTerminated = false;
			this._bMatched = false;
			this._bHasMarkerContent = false;
			this._sCustom = null;
			this._nRowSpan = 1;
			this._nColSpan = 1;
			this._nVirtualListLevel = -1L;
			this._csa = null;
			this._formatState = new FormatState();
			this._contentBuilder = new StringBuilder();
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000E7F18 File Offset: 0x000E6118
		internal void InheritFormatState(FormatState formatState)
		{
			this._formatState = new FormatState(formatState);
			this._formatState.LI = 0L;
			this._formatState.RI = 0L;
			this._formatState.SB = 0L;
			this._formatState.SA = 0L;
			this._formatState.FI = 0L;
			this._formatState.Marker = MarkerStyle.MarkerNone;
			this._formatState.CBPara = -1L;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000E7F8B File Offset: 0x000E618B
		internal string GetTagName()
		{
			return DocumentNode.XamlNames[(int)this.Type];
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000E7F9C File Offset: 0x000E619C
		internal DocumentNode GetParentOfType(DocumentNodeType parentType)
		{
			DocumentNode parent = this.Parent;
			while (parent != null && parent.Type != parentType)
			{
				parent = parent.Parent;
			}
			return parent;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000E7FC8 File Offset: 0x000E61C8
		internal int GetTableDepth()
		{
			DocumentNode parent = this.Parent;
			int num = 0;
			while (parent != null)
			{
				if (parent.Type == DocumentNodeType.dnTable)
				{
					num++;
				}
				parent = parent.Parent;
			}
			return num;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000E7FFC File Offset: 0x000E61FC
		internal int GetListDepth()
		{
			DocumentNode parent = this.Parent;
			int num = 0;
			while (parent != null)
			{
				if (parent.Type == DocumentNodeType.dnList)
				{
					num++;
				}
				else if (parent.Type == DocumentNodeType.dnCell)
				{
					break;
				}
				parent = parent.Parent;
			}
			return num;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000E803C File Offset: 0x000E623C
		internal void Terminate(ConverterState converterState)
		{
			if (!this.IsTerminated)
			{
				string value = this.StripInvalidChars(this.Xaml);
				this.AppendXamlPrefix(converterState);
				StringBuilder stringBuilder = new StringBuilder(this.Xaml);
				stringBuilder.Append(value);
				this.Xaml = stringBuilder.ToString();
				this.AppendXamlPostfix(converterState);
				this.IsTerminated = true;
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000E8094 File Offset: 0x000E6294
		internal void ConstrainFontPropagation(FormatState fsOrig)
		{
			this.FormatState.SetCharDefaults();
			this.FormatState.Font = fsOrig.Font;
			this.FormatState.FontSize = fsOrig.FontSize;
			this.FormatState.Bold = fsOrig.Bold;
			this.FormatState.Italic = fsOrig.Italic;
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000E80F0 File Offset: 0x000E62F0
		internal bool RequiresXamlFontProperties()
		{
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			return formatState.Strike != parentFormatStateForFont.Strike || formatState.UL != parentFormatStateForFont.UL || (formatState.Font != parentFormatStateForFont.Font && formatState.Font >= 0L) || (formatState.FontSize != parentFormatStateForFont.FontSize && formatState.FontSize >= 0L) || formatState.CF != parentFormatStateForFont.CF || formatState.Bold != parentFormatStateForFont.Bold || formatState.Italic != parentFormatStateForFont.Italic || formatState.LangCur != parentFormatStateForFont.LangCur;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000E8194 File Offset: 0x000E6394
		internal void AppendXamlFontProperties(ConverterState converterState, StringBuilder sb)
		{
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			bool flag = formatState.Strike != parentFormatStateForFont.Strike;
			bool flag2 = formatState.UL != parentFormatStateForFont.UL;
			if (flag || flag2)
			{
				sb.Append(" TextDecorations=\"");
				if (flag2)
				{
					sb.Append("Underline");
				}
				if (flag2 && flag)
				{
					sb.Append(", ");
				}
				if (flag)
				{
					sb.Append("Strikethrough");
				}
				sb.Append("\"");
			}
			if (formatState.Font != parentFormatStateForFont.Font && formatState.Font >= 0L)
			{
				FontTableEntry fontTableEntry = converterState.FontTable.FindEntryByIndex((int)formatState.Font);
				if (fontTableEntry != null && fontTableEntry.Name != null && !fontTableEntry.Name.Equals(string.Empty))
				{
					sb.Append(" FontFamily=\"");
					if (fontTableEntry.Name.Length > 32)
					{
						sb.Append(fontTableEntry.Name, 0, 32);
					}
					else
					{
						sb.Append(fontTableEntry.Name);
					}
					sb.Append("\"");
				}
			}
			if (formatState.FontSize != parentFormatStateForFont.FontSize && formatState.FontSize >= 0L)
			{
				sb.Append(" FontSize=\"");
				double num = (double)formatState.FontSize;
				if (num <= 1.0)
				{
					num = 2.0;
				}
				sb.Append((num / 2.0).ToString(CultureInfo.InvariantCulture));
				sb.Append("pt\"");
			}
			if (formatState.Bold != parentFormatStateForFont.Bold)
			{
				if (formatState.Bold)
				{
					sb.Append(" FontWeight=\"Bold\"");
				}
				else
				{
					sb.Append(" FontWeight=\"Normal\"");
				}
			}
			if (formatState.Italic != parentFormatStateForFont.Italic)
			{
				if (formatState.Italic)
				{
					sb.Append(" FontStyle=\"Italic\"");
				}
				else
				{
					sb.Append(" FontStyle=\"Normal\"");
				}
			}
			if (formatState.CF != parentFormatStateForFont.CF)
			{
				ColorTableEntry colorTableEntry = converterState.ColorTable.EntryAt((int)formatState.CF);
				if (colorTableEntry != null && !colorTableEntry.IsAuto)
				{
					sb.Append(" Foreground=\"");
					sb.Append(colorTableEntry.Color.ToString());
					sb.Append("\"");
				}
			}
			if (formatState.LangCur != parentFormatStateForFont.LangCur && formatState.LangCur > 0L && formatState.LangCur != 1024L)
			{
				try
				{
					CultureInfo cultureInfo = new CultureInfo((int)formatState.LangCur);
					sb.Append(" xml:lang=\"");
					sb.Append(cultureInfo.Name);
					sb.Append("\"");
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000E845C File Offset: 0x000E665C
		internal string StripInvalidChars(string text)
		{
			if (text == null || text.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = null;
			for (int i = 0; i < text.Length; i++)
			{
				int num = i;
				while (i < text.Length)
				{
					if ((text[i] & '') == '\ud800')
					{
						if (i + 1 == text.Length || (text[i] & 'ﰀ') == '\udc00' || (text[i + 1] & 'ﰀ') != '\udc00')
						{
							break;
						}
						i++;
					}
					i++;
				}
				if (num != 0 || i != text.Length)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					if (i != num)
					{
						stringBuilder.Append(text, num, i - num);
					}
				}
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000E8520 File Offset: 0x000E6720
		internal void AppendXamlEncoded(string text)
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			int num;
			for (int i = 0; i < text.Length; i = num + 1)
			{
				num = i;
				while (num < text.Length && (text[num] >= ' ' || text[num] == '\t') && text[num] != '&' && text[num] != '>' && text[num] != '<' && text[num] != '\0')
				{
					num++;
				}
				if (num != i)
				{
					string value = text.Substring(i, num - i);
					stringBuilder.Append(value);
				}
				if (num < text.Length)
				{
					if (text[num] < ' ' && text[num] != '\t')
					{
						char c = text[num];
						if (c == '\f')
						{
							stringBuilder.Append("&#x");
							stringBuilder.Append(((int)text[num]).ToString("x", CultureInfo.InvariantCulture));
							stringBuilder.Append(";");
						}
					}
					else
					{
						char c = text[num];
						if (c <= '&')
						{
							if (c != '\0')
							{
								if (c == '&')
								{
									stringBuilder.Append("&amp;");
								}
							}
						}
						else if (c != '<')
						{
							if (c == '>')
							{
								stringBuilder.Append("&gt;");
							}
						}
						else
						{
							stringBuilder.Append("&lt;");
						}
					}
				}
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000E8684 File Offset: 0x000E6884
		internal void AppendXamlPrefix(ConverterState converterState)
		{
			DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
			if (this.IsHidden)
			{
				return;
			}
			if (this.Type == DocumentNodeType.dnImage)
			{
				this.AppendImageXamlPrefix();
				return;
			}
			if (this.Type == DocumentNodeType.dnText || this.Type == DocumentNodeType.dnInline)
			{
				this.AppendInlineXamlPrefix(converterState);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsEmptyNode && this.RequiresXamlFontProperties())
			{
				stringBuilder.Append("<");
				stringBuilder.Append(DocumentNode.XamlNames[2]);
				this.AppendXamlFontProperties(converterState, stringBuilder);
				stringBuilder.Append(">");
			}
			stringBuilder.Append("<");
			stringBuilder.Append(this.GetTagName());
			switch (this.Type)
			{
			case DocumentNodeType.dnHyperlink:
				this.AppendXamlPrefixHyperlinkProperties(stringBuilder);
				break;
			case DocumentNodeType.dnParagraph:
				this.AppendXamlPrefixParagraphProperties(stringBuilder, converterState);
				break;
			case DocumentNodeType.dnList:
				this.AppendXamlPrefixListProperties(stringBuilder);
				break;
			case DocumentNodeType.dnListItem:
				this.AppendXamlPrefixListItemProperties(stringBuilder);
				break;
			case DocumentNodeType.dnTable:
				this.AppendXamlPrefixTableProperties(stringBuilder);
				break;
			case DocumentNodeType.dnCell:
				this.AppendXamlPrefixCellProperties(stringBuilder, documentNodeArray, converterState);
				break;
			}
			if (this.IsEmptyNode)
			{
				stringBuilder.Append(" /");
			}
			stringBuilder.Append(">");
			if (this.IsEmptyNode && this.RequiresXamlFontProperties())
			{
				stringBuilder.Append("</");
				stringBuilder.Append(DocumentNode.XamlNames[2]);
				stringBuilder.Append(">");
			}
			DocumentNodeType type = this.Type;
			if (type == DocumentNodeType.dnTable)
			{
				this.AppendXamlTableColumnsAfterStartTag(stringBuilder);
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000E8814 File Offset: 0x000E6A14
		private void AppendXamlPrefixTableProperties(StringBuilder xamlStringBuilder)
		{
			if (this.FormatState.HasRowFormat)
			{
				if (this.FormatState.RowFormat.Dir == DirState.DirRTL)
				{
					xamlStringBuilder.Append(" FlowDirection=\"RightToLeft\"");
				}
				RowFormat rowFormat = this.FormatState.RowFormat;
				CellFormat rowCellFormat = rowFormat.RowCellFormat;
				xamlStringBuilder.Append(" CellSpacing=\"");
				xamlStringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)rowCellFormat.SpacingLeft));
				xamlStringBuilder.Append("\"");
				xamlStringBuilder.Append(" Margin=\"");
				xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)rowFormat.Trleft));
				xamlStringBuilder.Append(",0,0,0\"");
				return;
			}
			xamlStringBuilder.Append(" CellSpacing=\"0\" Margin=\"0,0,0,0\"");
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000E88C8 File Offset: 0x000E6AC8
		private void AppendXamlPrefixCellProperties(StringBuilder xamlStringBuilder, DocumentNodeArray dna, ConverterState converterState)
		{
			Color color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnRow);
			if (parentOfType != null && parentOfType.FormatState.HasRowFormat)
			{
				int cellColumn = this.GetCellColumn();
				CellFormat cellFormat = parentOfType.FormatState.RowFormat.NthCellFormat(cellColumn);
				if (Converters.ColorToUse(converterState, cellFormat.CB, cellFormat.CF, cellFormat.Shading, ref color))
				{
					xamlStringBuilder.Append(" Background=\"");
					xamlStringBuilder.Append(color.ToString(CultureInfo.InvariantCulture));
					xamlStringBuilder.Append("\"");
				}
				if (cellFormat.HasBorder)
				{
					xamlStringBuilder.Append(cellFormat.GetBorderAttributeString(converterState));
				}
				xamlStringBuilder.Append(cellFormat.GetPaddingAttributeString());
			}
			else
			{
				xamlStringBuilder.Append(" BorderBrush=\"#FF000000\" BorderThickness=\"1,1,1,1\"");
			}
			if (this.ColSpan > 1)
			{
				xamlStringBuilder.Append(" ColumnSpan=\"");
				xamlStringBuilder.Append(this.ColSpan.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			if (this.RowSpan > 1)
			{
				xamlStringBuilder.Append(" RowSpan=\"");
				xamlStringBuilder.Append(this.RowSpan.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000E8A0B File Offset: 0x000E6C0B
		private void AppendXamlDir(StringBuilder xamlStringBuilder)
		{
			if (this.RequiresXamlDir)
			{
				if (this.XamlDir == DirState.DirLTR)
				{
					xamlStringBuilder.Append(" FlowDirection=\"LeftToRight\"");
					return;
				}
				xamlStringBuilder.Append(" FlowDirection=\"RightToLeft\"");
			}
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000E8A38 File Offset: 0x000E6C38
		private void AppendXamlPrefixParagraphProperties(StringBuilder xamlStringBuilder, ConverterState converterState)
		{
			Color color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			FormatState formatState = this.FormatState;
			if (Converters.ColorToUse(converterState, formatState.CBPara, formatState.CFPara, formatState.ParaShading, ref color))
			{
				xamlStringBuilder.Append(" Background=\"");
				xamlStringBuilder.Append(color.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			this.AppendXamlDir(xamlStringBuilder);
			xamlStringBuilder.Append(" Margin=\"");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)this.NearMargin));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)formatState.SB));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)this.FarMargin));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)formatState.SA));
			xamlStringBuilder.Append("\"");
			this.AppendXamlFontProperties(converterState, xamlStringBuilder);
			if (formatState.FI != 0L)
			{
				xamlStringBuilder.Append(" TextIndent=\"");
				xamlStringBuilder.Append(Converters.TwipToPxString((double)formatState.FI));
				xamlStringBuilder.Append("\"");
			}
			if (formatState.HAlign != HAlign.AlignDefault)
			{
				xamlStringBuilder.Append(" TextAlignment=\"");
				xamlStringBuilder.Append(Converters.AlignmentToString(formatState.HAlign, formatState.DirPara));
				xamlStringBuilder.Append("\"");
			}
			if (formatState.HasParaBorder)
			{
				xamlStringBuilder.Append(formatState.GetBorderAttributeString(converterState));
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000E8BBC File Offset: 0x000E6DBC
		private void AppendXamlPrefixListItemProperties(StringBuilder xamlStringBuilder)
		{
			long num = this.NearMargin;
			if (num < 360L && this.GetListDepth() == 1)
			{
				DocumentNode parent = this.Parent;
				if (parent != null && parent.FormatState.Marker != MarkerStyle.MarkerHidden)
				{
					num = 360L;
				}
			}
			xamlStringBuilder.Append(" Margin=\"");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)num));
			xamlStringBuilder.Append(",0,0,0\"");
			this.AppendXamlDir(xamlStringBuilder);
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000E8C34 File Offset: 0x000E6E34
		private void AppendXamlPrefixListProperties(StringBuilder xamlStringBuilder)
		{
			xamlStringBuilder.Append(" Margin=\"0,0,0,0\"");
			xamlStringBuilder.Append(" Padding=\"0,0,0,0\"");
			xamlStringBuilder.Append(" MarkerStyle=\"");
			xamlStringBuilder.Append(Converters.MarkerStyleToString(this.FormatState.Marker));
			xamlStringBuilder.Append("\"");
			if (this.FormatState.StartIndex > 0L && this.FormatState.StartIndex != 1L)
			{
				xamlStringBuilder.Append(" StartIndex=\"");
				xamlStringBuilder.Append(this.FormatState.StartIndex.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			this.AppendXamlDir(xamlStringBuilder);
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000E8CE4 File Offset: 0x000E6EE4
		private void AppendXamlPrefixHyperlinkProperties(StringBuilder xamlStringBuilder)
		{
			if (this.NavigateUri != null && this.NavigateUri.Length > 0)
			{
				xamlStringBuilder.Append(" NavigateUri=\"");
				xamlStringBuilder.Append(Converters.StringToXMLAttribute(this.NavigateUri));
				xamlStringBuilder.Append("\"");
			}
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000E8D34 File Offset: 0x000E6F34
		private void AppendXamlTableColumnsAfterStartTag(StringBuilder xamlStringBuilder)
		{
			if (this.ColumnStateArray != null && this.ColumnStateArray.Count > 0)
			{
				xamlStringBuilder.Append("<Table.Columns>");
				long num = 0L;
				if (this.FormatState.HasRowFormat)
				{
					num = this.FormatState.RowFormat.Trleft;
				}
				for (int i = 0; i < this.ColumnStateArray.Count; i++)
				{
					ColumnState columnState = this.ColumnStateArray.EntryAt(i);
					long num2 = columnState.CellX - num;
					if (num2 <= 0L)
					{
						num2 = 1L;
					}
					num = columnState.CellX;
					xamlStringBuilder.Append("<TableColumn Width=\"");
					xamlStringBuilder.Append(Converters.TwipToPxString((double)num2));
					xamlStringBuilder.Append("\" />");
				}
				xamlStringBuilder.Append("</Table.Columns>");
			}
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000E8DF8 File Offset: 0x000E6FF8
		internal void AppendXamlPostfix(ConverterState converterState)
		{
			if (this.IsHidden)
			{
				return;
			}
			if (this.IsEmptyNode)
			{
				return;
			}
			if (this.Type == DocumentNodeType.dnImage)
			{
				this.AppendImageXamlPostfix();
				return;
			}
			if (this.Type == DocumentNodeType.dnText || this.Type == DocumentNodeType.dnInline)
			{
				this.AppendInlineXamlPostfix(converterState);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</");
			stringBuilder.Append(this.GetTagName());
			stringBuilder.Append(">");
			if (this.IsBlock)
			{
				stringBuilder.Append("\r\n");
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000E8E94 File Offset: 0x000E7094
		internal void AppendInlineXamlPrefix(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			stringBuilder.Append("<Span");
			this.AppendXamlDir(stringBuilder);
			if (formatState.CB != parentFormatStateForFont.CB)
			{
				ColorTableEntry colorTableEntry = converterState.ColorTable.EntryAt((int)formatState.CB);
				if (colorTableEntry != null && !colorTableEntry.IsAuto)
				{
					stringBuilder.Append(" Background=\"");
					stringBuilder.Append(colorTableEntry.Color.ToString());
					stringBuilder.Append("\"");
				}
			}
			this.AppendXamlFontProperties(converterState, stringBuilder);
			if (formatState.Super != parentFormatStateForFont.Super)
			{
				stringBuilder.Append(" Typography.Variants=\"Superscript\"");
			}
			if (formatState.Sub != parentFormatStateForFont.Sub)
			{
				stringBuilder.Append(" Typography.Variants=\"Subscript\"");
			}
			stringBuilder.Append(">");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000E8F7C File Offset: 0x000E717C
		internal void AppendInlineXamlPostfix(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</Span>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000E8FB0 File Offset: 0x000E71B0
		internal void AppendImageXamlPrefix()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<InlineUIContainer>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000E8FDC File Offset: 0x000E71DC
		internal void AppendImageXamlPostfix()
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</InlineUIContainer>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000E9010 File Offset: 0x000E7210
		internal bool IsAncestorOf(DocumentNode documentNode)
		{
			int index = this.Index;
			int num = this.Index + this.ChildCount;
			return documentNode.Index > index && documentNode.Index <= num;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000E904C File Offset: 0x000E724C
		internal bool IsLastParagraphInCell()
		{
			DocumentNodeArray dna = this.DNA;
			if (this.Type != DocumentNodeType.dnParagraph)
			{
				return false;
			}
			DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnCell);
			if (parentOfType == null)
			{
				return false;
			}
			int i = parentOfType.Index + 1;
			int num = parentOfType.Index + parentOfType.ChildCount;
			while (i <= num)
			{
				DocumentNode documentNode = dna.EntryAt(num);
				if (documentNode == this)
				{
					return true;
				}
				if (documentNode.IsBlock)
				{
					return false;
				}
				num--;
			}
			return false;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000E90B8 File Offset: 0x000E72B8
		internal DocumentNodeArray GetTableRows()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			if (this.Type == DocumentNodeType.dnTable)
			{
				int i = this.Index + 1;
				int num = this.Index + this.ChildCount;
				while (i <= num)
				{
					DocumentNode documentNode = dna.EntryAt(i);
					if (documentNode.Type == DocumentNodeType.dnRow && this == documentNode.GetParentOfType(DocumentNodeType.dnTable))
					{
						documentNodeArray.Push(documentNode);
					}
					i++;
				}
			}
			return documentNodeArray;
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000E9128 File Offset: 0x000E7328
		internal DocumentNodeArray GetRowsCells()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			if (this.Type == DocumentNodeType.dnRow)
			{
				int i = this.Index + 1;
				int num = this.Index + this.ChildCount;
				while (i <= num)
				{
					DocumentNode documentNode = dna.EntryAt(i);
					if (documentNode.Type == DocumentNodeType.dnCell && this == documentNode.GetParentOfType(DocumentNodeType.dnRow))
					{
						documentNodeArray.Push(documentNode);
					}
					i++;
				}
			}
			return documentNodeArray;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000E9198 File Offset: 0x000E7398
		internal int GetCellColumn()
		{
			DocumentNodeArray dna = this.DNA;
			int num = 0;
			if (this.Type == DocumentNodeType.dnCell)
			{
				DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnRow);
				if (parentOfType != null)
				{
					int i = parentOfType.Index + 1;
					int num2 = parentOfType.Index + parentOfType.ChildCount;
					while (i <= num2)
					{
						DocumentNode documentNode = dna.EntryAt(i);
						if (documentNode == this)
						{
							break;
						}
						if (documentNode.Type == DocumentNodeType.dnCell && documentNode.GetParentOfType(DocumentNodeType.dnRow) == parentOfType)
						{
							num++;
						}
						i++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000E9214 File Offset: 0x000E7414
		internal ColumnStateArray ComputeColumns()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray tableRows = this.GetTableRows();
			ColumnStateArray columnStateArray = new ColumnStateArray();
			for (int i = 0; i < tableRows.Count; i++)
			{
				DocumentNode documentNode = tableRows.EntryAt(i);
				RowFormat rowFormat = documentNode.FormatState.RowFormat;
				long num = 0L;
				for (int j = 0; j < rowFormat.CellCount; j++)
				{
					CellFormat cellFormat = rowFormat.NthCellFormat(j);
					bool flag = false;
					long num2 = 0L;
					if (!cellFormat.IsHMerge)
					{
						for (int k = 0; k < columnStateArray.Count; k++)
						{
							ColumnState columnState = (ColumnState)columnStateArray[k];
							if (columnState.CellX == cellFormat.CellX)
							{
								if (!columnState.IsFilled && num2 == num)
								{
									columnState.IsFilled = true;
								}
								flag = true;
								break;
							}
							if (columnState.CellX > cellFormat.CellX)
							{
								columnStateArray.Insert(k, new ColumnState
								{
									Row = documentNode,
									CellX = cellFormat.CellX,
									IsFilled = (num2 == num)
								});
								flag = true;
								break;
							}
							num2 = columnState.CellX;
						}
						if (!flag)
						{
							columnStateArray.Add(new ColumnState
							{
								Row = documentNode,
								CellX = cellFormat.CellX,
								IsFilled = (num2 == num)
							});
						}
						num = cellFormat.CellX;
					}
				}
			}
			return columnStateArray;
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x000E938C File Offset: 0x000E758C
		internal bool IsInline
		{
			get
			{
				return this._type == DocumentNodeType.dnText || this._type == DocumentNodeType.dnInline || this._type == DocumentNodeType.dnImage || this._type == DocumentNodeType.dnLineBreak || this._type == DocumentNodeType.dnListText || this._type == DocumentNodeType.dnHyperlink;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x000E93C8 File Offset: 0x000E75C8
		internal bool IsBlock
		{
			get
			{
				return this._type == DocumentNodeType.dnParagraph || this._type == DocumentNodeType.dnList || this._type == DocumentNodeType.dnListItem || this._type == DocumentNodeType.dnTable || this._type == DocumentNodeType.dnTableBody || this._type == DocumentNodeType.dnRow || this._type == DocumentNodeType.dnCell || this._type == DocumentNodeType.dnSection || this._type == DocumentNodeType.dnFigure || this._type == DocumentNodeType.dnFloater;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x000E943A File Offset: 0x000E763A
		internal bool IsEmptyNode
		{
			get
			{
				return this._type == DocumentNodeType.dnLineBreak;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x000E9445 File Offset: 0x000E7645
		internal bool IsHidden
		{
			get
			{
				return this._type == DocumentNodeType.dnFieldBegin || this._type == DocumentNodeType.dnFieldEnd || this._type == DocumentNodeType.dnShape || this._type == DocumentNodeType.dnListText;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x000E9474 File Offset: 0x000E7674
		internal bool IsWhiteSpace
		{
			get
			{
				if (this.IsTerminated)
				{
					return false;
				}
				if (this._type == DocumentNodeType.dnText)
				{
					string text = this.Xaml.Trim();
					return text.Length == 0;
				}
				return false;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x000E94AB File Offset: 0x000E76AB
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x000E94BE File Offset: 0x000E76BE
		internal bool IsPending
		{
			get
			{
				return this.Index >= 0 && this._bPending;
			}
			set
			{
				this._bPending = value;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x000E94C7 File Offset: 0x000E76C7
		// (set) Token: 0x06003456 RID: 13398 RVA: 0x000E94CF File Offset: 0x000E76CF
		internal bool IsTerminated
		{
			get
			{
				return this._bTerminated;
			}
			set
			{
				this._bTerminated = value;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x000E94D8 File Offset: 0x000E76D8
		// (set) Token: 0x06003458 RID: 13400 RVA: 0x000E94EC File Offset: 0x000E76EC
		internal bool IsMatched
		{
			get
			{
				return this.Type != DocumentNodeType.dnFieldBegin || this._bMatched;
			}
			set
			{
				this._bMatched = value;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003459 RID: 13401 RVA: 0x000E94F5 File Offset: 0x000E76F5
		internal bool IsTrackedAsOpen
		{
			get
			{
				return this.Index >= 0 && this.Type != DocumentNodeType.dnFieldEnd && ((this.IsPending && !this.IsTerminated) || !this.IsMatched);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x000E952B File Offset: 0x000E772B
		// (set) Token: 0x0600345B RID: 13403 RVA: 0x000E9533 File Offset: 0x000E7733
		internal bool HasMarkerContent
		{
			get
			{
				return this._bHasMarkerContent;
			}
			set
			{
				this._bHasMarkerContent = value;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x000E953C File Offset: 0x000E773C
		internal bool IsNonEmpty
		{
			get
			{
				return this.ChildCount > 0 || this.Xaml != null;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x000E9552 File Offset: 0x000E7752
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x000E955A File Offset: 0x000E775A
		internal string ListLabel
		{
			get
			{
				return this._sCustom;
			}
			set
			{
				this._sCustom = value;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x000E9563 File Offset: 0x000E7763
		// (set) Token: 0x06003460 RID: 13408 RVA: 0x000E956B File Offset: 0x000E776B
		internal long VirtualListLevel
		{
			get
			{
				return this._nVirtualListLevel;
			}
			set
			{
				this._nVirtualListLevel = value;
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x000E9552 File Offset: 0x000E7752
		// (set) Token: 0x06003462 RID: 13410 RVA: 0x000E955A File Offset: 0x000E775A
		internal string NavigateUri
		{
			get
			{
				return this._sCustom;
			}
			set
			{
				this._sCustom = value;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x000E9574 File Offset: 0x000E7774
		internal DocumentNodeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x000E957C File Offset: 0x000E777C
		// (set) Token: 0x06003465 RID: 13413 RVA: 0x000E9584 File Offset: 0x000E7784
		internal FormatState FormatState
		{
			get
			{
				return this._formatState;
			}
			set
			{
				this._formatState = value;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003466 RID: 13414 RVA: 0x000E9590 File Offset: 0x000E7790
		internal FormatState ParentFormatStateForFont
		{
			get
			{
				DocumentNode parent = this.Parent;
				if (parent != null && parent.Type == DocumentNodeType.dnHyperlink)
				{
					parent = parent.Parent;
				}
				if (this.Type == DocumentNodeType.dnParagraph || parent == null)
				{
					return FormatState.EmptyFormatState;
				}
				return parent.FormatState;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x000E95CF File Offset: 0x000E77CF
		// (set) Token: 0x06003468 RID: 13416 RVA: 0x000E95D7 File Offset: 0x000E77D7
		internal int ChildCount
		{
			get
			{
				return this._childCount;
			}
			set
			{
				if (value >= 0)
				{
					this._childCount = value;
				}
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x000E95E4 File Offset: 0x000E77E4
		// (set) Token: 0x0600346A RID: 13418 RVA: 0x000E95EC File Offset: 0x000E77EC
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x000E95F5 File Offset: 0x000E77F5
		// (set) Token: 0x0600346C RID: 13420 RVA: 0x000E95FD File Offset: 0x000E77FD
		internal DocumentNodeArray DNA
		{
			get
			{
				return this._dna;
			}
			set
			{
				this._dna = value;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x000E9606 File Offset: 0x000E7806
		internal int LastChildIndex
		{
			get
			{
				return this.Index + this.ChildCount;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x0600346E RID: 13422 RVA: 0x000E9615 File Offset: 0x000E7815
		internal DocumentNode ClosedParent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x000E961D File Offset: 0x000E781D
		// (set) Token: 0x06003470 RID: 13424 RVA: 0x000E9642 File Offset: 0x000E7842
		internal DocumentNode Parent
		{
			get
			{
				if (this._parent == null && this.DNA != null)
				{
					return this.DNA.GetOpenParentWhileParsing(this);
				}
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x000E964B File Offset: 0x000E784B
		// (set) Token: 0x06003472 RID: 13426 RVA: 0x000E9653 File Offset: 0x000E7853
		internal string Xaml
		{
			get
			{
				return this._xaml;
			}
			set
			{
				this._xaml = value;
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x000E965C File Offset: 0x000E785C
		internal StringBuilder Content
		{
			get
			{
				return this._contentBuilder;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x000E9664 File Offset: 0x000E7864
		// (set) Token: 0x06003475 RID: 13429 RVA: 0x000E966C File Offset: 0x000E786C
		internal int RowSpan
		{
			get
			{
				return this._nRowSpan;
			}
			set
			{
				this._nRowSpan = value;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x000E9675 File Offset: 0x000E7875
		// (set) Token: 0x06003477 RID: 13431 RVA: 0x000E967D File Offset: 0x000E787D
		internal int ColSpan
		{
			get
			{
				return this._nColSpan;
			}
			set
			{
				this._nColSpan = value;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x000E9686 File Offset: 0x000E7886
		// (set) Token: 0x06003479 RID: 13433 RVA: 0x000E968E File Offset: 0x000E788E
		internal ColumnStateArray ColumnStateArray
		{
			get
			{
				return this._csa;
			}
			set
			{
				this._csa = value;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x000E9698 File Offset: 0x000E7898
		internal DirState XamlDir
		{
			get
			{
				if (this.IsInline)
				{
					return this.FormatState.DirChar;
				}
				if (this.Type == DocumentNodeType.dnTable)
				{
					if (this.FormatState.HasRowFormat)
					{
						return this.FormatState.RowFormat.Dir;
					}
					return this.ParentXamlDir;
				}
				else
				{
					if (this.Type == DocumentNodeType.dnList || this.Type == DocumentNodeType.dnParagraph)
					{
						return this.FormatState.DirPara;
					}
					for (DocumentNode parent = this.Parent; parent != null; parent = parent.Parent)
					{
						DocumentNodeType type = parent.Type;
						if (type == DocumentNodeType.dnParagraph || type == DocumentNodeType.dnList || type == DocumentNodeType.dnTable)
						{
							return parent.XamlDir;
						}
					}
					return DirState.DirLTR;
				}
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x000E9737 File Offset: 0x000E7937
		internal DirState ParentXamlDir
		{
			get
			{
				if (this.Parent != null)
				{
					return this.Parent.XamlDir;
				}
				return DirState.DirLTR;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x000E974E File Offset: 0x000E794E
		internal bool RequiresXamlDir
		{
			get
			{
				return this.XamlDir != this.ParentXamlDir;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x000E9761 File Offset: 0x000E7961
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x000E9783 File Offset: 0x000E7983
		internal long NearMargin
		{
			get
			{
				if (this.ParentXamlDir != DirState.DirLTR)
				{
					return this.FormatState.RI;
				}
				return this.FormatState.LI;
			}
			set
			{
				if (this.ParentXamlDir == DirState.DirLTR)
				{
					this.FormatState.LI = value;
					return;
				}
				this.FormatState.RI = value;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000E97A7 File Offset: 0x000E79A7
		internal long FarMargin
		{
			get
			{
				if (this.ParentXamlDir != DirState.DirLTR)
				{
					return this.FormatState.LI;
				}
				return this.FormatState.RI;
			}
		}

		// Token: 0x040024C6 RID: 9414
		internal static string[] HtmlNames = new string[]
		{
			"",
			"",
			"span",
			"br",
			"a",
			"p",
			"ul",
			"li",
			"table",
			"tbody",
			"tr",
			"td"
		};

		// Token: 0x040024C7 RID: 9415
		internal static int[] HtmlLengths = new int[]
		{
			0,
			0,
			4,
			2,
			1,
			1,
			2,
			2,
			5,
			6,
			2,
			2
		};

		// Token: 0x040024C8 RID: 9416
		internal static string[] XamlNames = new string[]
		{
			"",
			"",
			"Span",
			"LineBreak",
			"Hyperlink",
			"Paragraph",
			"InlineUIContainer",
			"BlockUIContainer",
			"Image",
			"List",
			"ListItem",
			"Table",
			"TableRowGroup",
			"TableRow",
			"TableCell",
			"Section",
			"Figure",
			"Floater",
			"Field",
			"ListText"
		};

		// Token: 0x040024C9 RID: 9417
		private bool _bPending;

		// Token: 0x040024CA RID: 9418
		private bool _bTerminated;

		// Token: 0x040024CB RID: 9419
		private DocumentNodeType _type;

		// Token: 0x040024CC RID: 9420
		private FormatState _formatState;

		// Token: 0x040024CD RID: 9421
		private string _xaml;

		// Token: 0x040024CE RID: 9422
		private StringBuilder _contentBuilder;

		// Token: 0x040024CF RID: 9423
		private int _childCount;

		// Token: 0x040024D0 RID: 9424
		private int _index;

		// Token: 0x040024D1 RID: 9425
		private DocumentNode _parent;

		// Token: 0x040024D2 RID: 9426
		private DocumentNodeArray _dna;

		// Token: 0x040024D3 RID: 9427
		private ColumnStateArray _csa;

		// Token: 0x040024D4 RID: 9428
		private int _nRowSpan;

		// Token: 0x040024D5 RID: 9429
		private int _nColSpan;

		// Token: 0x040024D6 RID: 9430
		private string _sCustom;

		// Token: 0x040024D7 RID: 9431
		private long _nVirtualListLevel;

		// Token: 0x040024D8 RID: 9432
		private bool _bHasMarkerContent;

		// Token: 0x040024D9 RID: 9433
		private bool _bMatched;
	}
}
