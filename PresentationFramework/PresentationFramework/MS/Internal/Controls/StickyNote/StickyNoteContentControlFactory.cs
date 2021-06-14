using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x0200076B RID: 1899
	internal static class StickyNoteContentControlFactory
	{
		// Token: 0x06007876 RID: 30838 RVA: 0x002251C8 File Offset: 0x002233C8
		public static StickyNoteContentControl CreateContentControl(StickyNoteType type, UIElement content)
		{
			StickyNoteContentControl result = null;
			if (type != StickyNoteType.Text)
			{
				if (type == StickyNoteType.Ink)
				{
					InkCanvas inkCanvas = content as InkCanvas;
					if (inkCanvas == null)
					{
						throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
						{
							type,
							typeof(InkCanvas),
							"PART_ContentControl"
						}));
					}
					result = new StickyNoteContentControlFactory.StickyNoteInkCanvas(inkCanvas);
				}
			}
			else
			{
				RichTextBox richTextBox = content as RichTextBox;
				if (richTextBox == null)
				{
					throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
					{
						type,
						typeof(RichTextBox),
						"PART_ContentControl"
					}));
				}
				result = new StickyNoteContentControlFactory.StickyNoteRichTextBox(richTextBox);
			}
			return result;
		}

		// Token: 0x02000B6D RID: 2925
		private class StickyNoteRichTextBox : StickyNoteContentControl
		{
			// Token: 0x06008E1B RID: 36379 RVA: 0x0025B46E File Offset: 0x0025966E
			public StickyNoteRichTextBox(RichTextBox rtb) : base(rtb)
			{
				DataObject.AddPastingHandler(rtb, new DataObjectPastingEventHandler(this.OnPastingDataObject));
			}

			// Token: 0x06008E1C RID: 36380 RVA: 0x0025B489 File Offset: 0x00259689
			public override void Clear()
			{
				((RichTextBox)base.InnerControl).Document = new FlowDocument(new Paragraph(new Run()));
			}

			// Token: 0x06008E1D RID: 36381 RVA: 0x0025B4AC File Offset: 0x002596AC
			public override void Save(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
				if (!textRange.IsEmpty)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						textRange.Save(memoryStream, DataFormats.Xaml);
						if (memoryStream.Length.CompareTo(1610612733L) > 0)
						{
							throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
						}
						node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}

			// Token: 0x06008E1E RID: 36382 RVA: 0x0025B558 File Offset: 0x00259758
			public override void Load(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				FlowDocument flowDocument = new FlowDocument();
				TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd, true);
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
				{
					textRange.Load(memoryStream, DataFormats.Xaml);
				}
				richTextBox.Document = flowDocument;
			}

			// Token: 0x17001F9A RID: 8090
			// (get) Token: 0x06008E1F RID: 36383 RVA: 0x0025B5CC File Offset: 0x002597CC
			public override bool IsEmpty
			{
				get
				{
					RichTextBox richTextBox = (RichTextBox)base.InnerControl;
					TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
					return textRange.IsEmpty;
				}
			}

			// Token: 0x17001F9B RID: 8091
			// (get) Token: 0x06008E20 RID: 36384 RVA: 0x0000B02A File Offset: 0x0000922A
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Text;
				}
			}

			// Token: 0x06008E21 RID: 36385 RVA: 0x0025B608 File Offset: 0x00259808
			private void OnPastingDataObject(object sender, DataObjectPastingEventArgs e)
			{
				if (e.FormatToApply == DataFormats.Rtf)
				{
					UTF8Encoding utf8Encoding = new UTF8Encoding();
					string s = e.DataObject.GetData(DataFormats.Rtf) as string;
					MemoryStream stream = new MemoryStream(utf8Encoding.GetBytes(s));
					FlowDocument flowDocument = new FlowDocument();
					TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
					textRange.Load(stream, DataFormats.Rtf);
					MemoryStream memoryStream = new MemoryStream();
					textRange.Save(memoryStream, DataFormats.Xaml);
					DataObject dataObject = new DataObject();
					dataObject.SetData(DataFormats.Xaml, utf8Encoding.GetString(memoryStream.GetBuffer()));
					e.DataObject = dataObject;
					e.FormatToApply = DataFormats.Xaml;
					return;
				}
				if (e.FormatToApply == DataFormats.Bitmap || e.FormatToApply == DataFormats.EnhancedMetafile || e.FormatToApply == DataFormats.MetafilePicture || e.FormatToApply == DataFormats.Tiff)
				{
					e.CancelCommand();
					return;
				}
				if (e.FormatToApply == DataFormats.XamlPackage)
				{
					e.FormatToApply = DataFormats.Xaml;
				}
			}
		}

		// Token: 0x02000B6E RID: 2926
		private class StickyNoteInkCanvas : StickyNoteContentControl
		{
			// Token: 0x06008E22 RID: 36386 RVA: 0x0025B72E File Offset: 0x0025992E
			public StickyNoteInkCanvas(InkCanvas canvas) : base(canvas)
			{
			}

			// Token: 0x06008E23 RID: 36387 RVA: 0x0025B737 File Offset: 0x00259937
			public override void Clear()
			{
				((InkCanvas)base.InnerControl).Strokes.Clear();
			}

			// Token: 0x06008E24 RID: 36388 RVA: 0x0025B750 File Offset: 0x00259950
			public override void Save(XmlNode node)
			{
				StrokeCollection strokes = ((InkCanvas)base.InnerControl).Strokes;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					strokes.Save(memoryStream);
					if (memoryStream.Length.CompareTo(1610612733L) > 0)
					{
						throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
					}
					node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
			}

			// Token: 0x06008E25 RID: 36389 RVA: 0x0025B7D8 File Offset: 0x002599D8
			public override void Load(XmlNode node)
			{
				StrokeCollection strokes = null;
				if (string.IsNullOrEmpty(node.InnerText))
				{
					strokes = new StrokeCollection();
				}
				else
				{
					using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
					{
						strokes = new StrokeCollection(memoryStream);
					}
				}
				((InkCanvas)base.InnerControl).Strokes = strokes;
			}

			// Token: 0x17001F9C RID: 8092
			// (get) Token: 0x06008E26 RID: 36390 RVA: 0x0025B844 File Offset: 0x00259A44
			public override bool IsEmpty
			{
				get
				{
					return ((InkCanvas)base.InnerControl).Strokes.Count == 0;
				}
			}

			// Token: 0x17001F9D RID: 8093
			// (get) Token: 0x06008E27 RID: 36391 RVA: 0x00016748 File Offset: 0x00014948
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Ink;
				}
			}
		}
	}
}
