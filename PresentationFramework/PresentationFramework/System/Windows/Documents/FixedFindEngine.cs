using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Xml;

namespace System.Windows.Documents
{
	// Token: 0x02000369 RID: 873
	internal sealed class FixedFindEngine
	{
		// Token: 0x06002E72 RID: 11890 RVA: 0x000D24E8 File Offset: 0x000D06E8
		internal static TextRange Find(ITextPointer start, ITextPointer end, string findPattern, CultureInfo cultureInfo, bool matchCase, bool matchWholeWord, bool matchLast, bool matchDiacritics, bool matchKashida, bool matchAlefHamza)
		{
			if (findPattern.Length == 0)
			{
				return null;
			}
			IDocumentPaginatorSource documentPaginatorSource = start.TextContainer.Parent as IDocumentPaginatorSource;
			DynamicDocumentPaginator dynamicDocumentPaginator = documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator;
			int pageNumber;
			int num;
			if (matchLast)
			{
				pageNumber = dynamicDocumentPaginator.GetPageNumber((ContentPosition)start);
				num = dynamicDocumentPaginator.GetPageNumber((ContentPosition)end);
			}
			else
			{
				pageNumber = dynamicDocumentPaginator.GetPageNumber((ContentPosition)end);
				num = dynamicDocumentPaginator.GetPageNumber((ContentPosition)start);
			}
			TextRange textRange = null;
			CompareInfo compareInfo = cultureInfo.CompareInfo;
			bool replaceAlefWithAlefHamza = false;
			CompareOptions compareOptions = FixedFindEngine._InitializeSearch(cultureInfo, matchCase, matchAlefHamza, matchDiacritics, ref findPattern, out replaceAlefWithAlefHamza);
			int num2 = num;
			FixedDocumentSequence fixedDocumentSequence = documentPaginatorSource as FixedDocumentSequence;
			DynamicDocumentPaginator dynamicDocumentPaginator2 = null;
			if (fixedDocumentSequence != null)
			{
				fixedDocumentSequence.TranslatePageNumber(num, out dynamicDocumentPaginator2, out num2);
			}
			if (num - pageNumber != 0)
			{
				ITextPointer startPosition = null;
				ITextPointer endPosition = null;
				FixedFindEngine._GetFirstPageSearchPointers(start, end, num2, matchLast, out startPosition, out endPosition);
				textRange = TextFindEngine.InternalFind(startPosition, endPosition, findPattern, cultureInfo, matchCase, matchWholeWord, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
				if (textRange == null)
				{
					num = (matchLast ? (num - 1) : (num + 1));
					int num3 = matchLast ? -1 : 1;
					while (matchLast ? (num >= pageNumber) : (num <= pageNumber))
					{
						num2 = num;
						dynamicDocumentPaginator2 = null;
						FixedDocument fixedDocument;
						if (fixedDocumentSequence != null)
						{
							fixedDocumentSequence.TranslatePageNumber(num, out dynamicDocumentPaginator2, out num2);
							fixedDocument = (FixedDocument)dynamicDocumentPaginator2.Source;
						}
						else
						{
							fixedDocument = (documentPaginatorSource as FixedDocument);
						}
						string text = FixedFindEngine._GetPageString(fixedDocument, num2, replaceAlefWithAlefHamza);
						if (text == null)
						{
							return TextFindEngine.InternalFind(start, end, findPattern, cultureInfo, matchCase, matchWholeWord, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
						}
						if (FixedFindEngine._FoundOnPage(text, findPattern, cultureInfo, compareOptions))
						{
							if (fixedDocumentSequence != null)
							{
								ChildDocumentBlock childBlock = fixedDocumentSequence.TextContainer.FindChildBlock(fixedDocument.DocumentReference);
								if (matchLast)
								{
									end = new DocumentSequenceTextPointer(childBlock, new FixedTextPointer(false, LogicalDirection.Backward, fixedDocument.FixedContainer.FixedTextBuilder.GetPageEndFlowPosition(num2)));
									start = new DocumentSequenceTextPointer(childBlock, new FixedTextPointer(false, LogicalDirection.Forward, fixedDocument.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(num2)));
								}
								else
								{
									start = new DocumentSequenceTextPointer(childBlock, new FixedTextPointer(false, LogicalDirection.Forward, fixedDocument.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(num2)));
									end = new DocumentSequenceTextPointer(childBlock, new FixedTextPointer(false, LogicalDirection.Backward, fixedDocument.FixedContainer.FixedTextBuilder.GetPageEndFlowPosition(num2)));
								}
							}
							else
							{
								FixedTextBuilder fixedTextBuilder = ((FixedDocument)documentPaginatorSource).FixedContainer.FixedTextBuilder;
								if (matchLast)
								{
									end = new FixedTextPointer(false, LogicalDirection.Backward, fixedTextBuilder.GetPageEndFlowPosition(num));
									start = new FixedTextPointer(false, LogicalDirection.Forward, fixedTextBuilder.GetPageStartFlowPosition(num));
								}
								else
								{
									start = new FixedTextPointer(false, LogicalDirection.Forward, fixedTextBuilder.GetPageStartFlowPosition(num));
									end = new FixedTextPointer(false, LogicalDirection.Backward, fixedTextBuilder.GetPageEndFlowPosition(num));
								}
							}
							textRange = TextFindEngine.InternalFind(start, end, findPattern, cultureInfo, matchCase, matchWholeWord, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
							if (textRange != null)
							{
								return textRange;
							}
						}
						num += num3;
					}
				}
			}
			else
			{
				FixedDocument doc = (dynamicDocumentPaginator2 != null) ? (dynamicDocumentPaginator2.Source as FixedDocument) : (documentPaginatorSource as FixedDocument);
				string text2 = FixedFindEngine._GetPageString(doc, num2, replaceAlefWithAlefHamza);
				if (text2 == null || FixedFindEngine._FoundOnPage(text2, findPattern, cultureInfo, compareOptions))
				{
					textRange = TextFindEngine.InternalFind(start, end, findPattern, cultureInfo, matchCase, matchWholeWord, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
				}
			}
			return textRange;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000D27FC File Offset: 0x000D09FC
		private static bool _FoundOnPage(string pageString, string findPattern, CultureInfo cultureInfo, CompareOptions compareOptions)
		{
			CompareInfo compareInfo = cultureInfo.CompareInfo;
			string[] array = findPattern.Split(null);
			if (array != null)
			{
				foreach (string value in array)
				{
					if (!string.IsNullOrEmpty(value) && compareInfo.IndexOf(pageString, value, compareOptions) == -1)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000D284C File Offset: 0x000D0A4C
		private static CompareOptions _InitializeSearch(CultureInfo cultureInfo, bool matchCase, bool matchAlefHamza, bool matchDiacritics, ref string findPattern, out bool replaceAlefWithAlefHamza)
		{
			CompareOptions compareOptions = CompareOptions.None;
			replaceAlefWithAlefHamza = false;
			if (!matchCase)
			{
				compareOptions |= CompareOptions.IgnoreCase;
			}
			bool flag;
			bool flag2;
			TextFindEngine.InitializeBidiFlags(findPattern, out flag, out flag2);
			if (flag2 && !matchAlefHamza)
			{
				findPattern = TextFindEngine.ReplaceAlefHamzaWithAlef(findPattern);
				replaceAlefWithAlefHamza = true;
			}
			if (!matchDiacritics && flag)
			{
				compareOptions |= CompareOptions.IgnoreNonSpace;
			}
			return compareOptions;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000D2894 File Offset: 0x000D0A94
		private static void _GetFirstPageSearchPointers(ITextPointer start, ITextPointer end, int pageNumber, bool matchLast, out ITextPointer firstSearchPageStart, out ITextPointer firstSearchPageEnd)
		{
			if (matchLast)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = end as DocumentSequenceTextPointer;
				if (documentSequenceTextPointer != null)
				{
					FlowPosition pageStartFlowPosition = ((FixedTextContainer)documentSequenceTextPointer.ChildBlock.ChildContainer).FixedTextBuilder.GetPageStartFlowPosition(pageNumber);
					firstSearchPageStart = new DocumentSequenceTextPointer(documentSequenceTextPointer.ChildBlock, new FixedTextPointer(false, LogicalDirection.Forward, pageStartFlowPosition));
				}
				else
				{
					FixedTextPointer fixedTextPointer = end as FixedTextPointer;
					firstSearchPageStart = new FixedTextPointer(false, LogicalDirection.Forward, fixedTextPointer.FixedTextContainer.FixedTextBuilder.GetPageStartFlowPosition(pageNumber));
				}
				firstSearchPageEnd = end;
				return;
			}
			DocumentSequenceTextPointer documentSequenceTextPointer2 = start as DocumentSequenceTextPointer;
			if (documentSequenceTextPointer2 != null)
			{
				FlowPosition pageEndFlowPosition = ((FixedTextContainer)documentSequenceTextPointer2.ChildBlock.ChildContainer).FixedTextBuilder.GetPageEndFlowPosition(pageNumber);
				firstSearchPageEnd = new DocumentSequenceTextPointer(documentSequenceTextPointer2.ChildBlock, new FixedTextPointer(false, LogicalDirection.Backward, pageEndFlowPosition));
			}
			else
			{
				FixedTextPointer fixedTextPointer2 = start as FixedTextPointer;
				firstSearchPageEnd = new FixedTextPointer(false, LogicalDirection.Backward, fixedTextPointer2.FixedTextContainer.FixedTextBuilder.GetPageEndFlowPosition(pageNumber));
			}
			firstSearchPageStart = start;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000D2974 File Offset: 0x000D0B74
		private static string _GetPageString(FixedDocument doc, int translatedPageNo, bool replaceAlefWithAlefHamza)
		{
			string text = null;
			PageContent pageContent = doc.Pages[translatedPageNo];
			Stream pageStream = pageContent.GetPageStream();
			bool reverseRTL = true;
			if (doc.HasExplicitStructure)
			{
				reverseRTL = false;
			}
			if (pageStream != null)
			{
				text = FixedFindEngine._ConstructPageString(pageStream, reverseRTL);
				if (replaceAlefWithAlefHamza)
				{
					text = TextFindEngine.ReplaceAlefHamzaWithAlef(text);
				}
			}
			return text;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000D29BC File Offset: 0x000D0BBC
		private static string _ConstructPageString(Stream pageStream, bool reverseRTL)
		{
			XmlTextReader baseReader = new XmlTextReader(pageStream);
			XmlReader xmlReader = new XmlCompatibilityReader(baseReader, FixedFindEngine._predefinedNamespaces);
			xmlReader = XmlReader.Create(xmlReader, new XmlReaderSettings
			{
				IgnoreWhitespace = true,
				IgnoreComments = true,
				ProhibitDtd = true
			});
			xmlReader.MoveToContent();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			string text = null;
			while (xmlReader.Read())
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				if (nodeType == XmlNodeType.Element && xmlReader.Name == "Glyphs")
				{
					text = xmlReader.GetAttribute("UnicodeString");
					if (!string.IsNullOrEmpty(text))
					{
						string attribute = xmlReader.GetAttribute("IsSideways");
						flag = false;
						if (attribute != null && string.Compare(attribute, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
						{
							flag = true;
						}
						if (reverseRTL)
						{
							string attribute2 = xmlReader.GetAttribute("BidiLevel");
							int num = 0;
							if (!string.IsNullOrEmpty(attribute2))
							{
								try
								{
									num = Convert.ToInt32(attribute2, CultureInfo.InvariantCulture);
								}
								catch (Exception)
								{
								}
							}
							string attribute3 = xmlReader.GetAttribute("CaretStops");
							if (num == 0 && !flag && string.IsNullOrEmpty(attribute3) && FixedTextBuilder.MostlyRTL(text))
							{
								char[] array = text.ToCharArray();
								Array.Reverse(array);
								text = new string(array);
							}
						}
						stringBuilder.Append(text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x0000326D File Offset: 0x0000146D
		private FixedFindEngine()
		{
		}

		// Token: 0x04001E0A RID: 7690
		private static string[] _predefinedNamespaces = new string[]
		{
			"http://schemas.microsoft.com/xps/2005/06",
			"http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key"
		};
	}
}
