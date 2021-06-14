using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007DB RID: 2011
	internal class TextViewSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007C56 RID: 31830 RVA: 0x0022FABF File Offset: 0x0022DCBF
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			newSelection = null;
			return false;
		}

		// Token: 0x06007C57 RID: 31831 RVA: 0x0022FAC5 File Offset: 0x0022DCC5
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			this.VerifySelection(selection);
			return new DependencyObject[]
			{
				(DependencyObject)selection
			};
		}

		// Token: 0x06007C58 RID: 31832 RVA: 0x0022FADE File Offset: 0x0022DCDE
		public override UIElement GetParent(object selection)
		{
			this.VerifySelection(selection);
			return (UIElement)selection;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x0022FAEE File Offset: 0x0022DCEE
		public override Point GetAnchorPoint(object selection)
		{
			this.VerifySelection(selection);
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x0022FB10 File Offset: 0x0022DD10
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			ITextView textView = this.VerifySelection(selection);
			List<ContentLocatorPart> list = new List<ContentLocatorPart>(1);
			int num;
			int num2;
			if (textView != null && textView.IsValid)
			{
				TextViewSelectionProcessor.GetTextViewTextRange(textView, out num, out num2);
			}
			else
			{
				num = -1;
				num2 = -1;
			}
			list.Add(new ContentLocatorPart(TextSelectionProcessor.CharacterRangeElementName)
			{
				NameValuePairs = 
				{
					{
						"Count",
						1.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"Segment" + 0.ToString(NumberFormatInfo.InvariantInfo),
						num.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + num2.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"IncludeOverlaps",
						bool.TrueString
					}
				}
			});
			return list;
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x0022FBF5 File Offset: 0x0022DDF5
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			return null;
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x0022FC17 File Offset: 0x0022DE17
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TextViewSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0022FC28 File Offset: 0x0022DE28
		internal static TextRange GetTextViewTextRange(ITextView textView, out int startOffset, out int endOffset)
		{
			startOffset = int.MinValue;
			endOffset = 0;
			TextRange result = null;
			IList<TextSegment> textSegments = textView.TextSegments;
			if (textSegments != null && textSegments.Count > 0)
			{
				ITextPointer start = textSegments[0].Start;
				ITextPointer end = textSegments[textSegments.Count - 1].End;
				startOffset = end.TextContainer.Start.GetOffsetToPosition(start);
				endOffset = end.TextContainer.Start.GetOffsetToPosition(end);
				result = new TextRange(start, end);
			}
			return result;
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x0022FCAC File Offset: 0x0022DEAC
		private ITextView VerifySelection(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			IServiceProvider serviceProvider = selection as IServiceProvider;
			if (serviceProvider == null)
			{
				throw new ArgumentException(SR.Get("SelectionMustBeServiceProvider"), "selection");
			}
			return serviceProvider.GetService(typeof(ITextView)) as ITextView;
		}

		// Token: 0x04003A62 RID: 14946
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[0];
	}
}
