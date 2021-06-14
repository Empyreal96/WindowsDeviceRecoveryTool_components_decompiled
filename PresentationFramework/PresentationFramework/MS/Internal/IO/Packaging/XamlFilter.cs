using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Xml;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000662 RID: 1634
	internal class XamlFilter : IManagedFilter
	{
		// Token: 0x06006C41 RID: 27713 RVA: 0x001F2347 File Offset: 0x001F0547
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddPresentationDescriptor(string Key)
		{
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", Key), new ContentDescriptor(true, false, null, null));
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x001F2368 File Offset: 0x001F0568
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddPresentationDescriptor(string Key, string Value)
		{
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", Key), new ContentDescriptor(true, false, Value, null));
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x001F238C File Offset: 0x001F058C
		private void InitElementDictionary()
		{
			if (this._xamlElementContentDescriptorDictionary != null)
			{
				return;
			}
			this._xamlElementContentDescriptorDictionary = new Hashtable(300);
			this.AddPresentationDescriptor("TextBox", "Text");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Italic"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("GridViewColumnHeader", "Content");
			this.AddPresentationDescriptor("Canvas");
			this.AddPresentationDescriptor("ListBox");
			this.AddPresentationDescriptor("ItemsControl");
			this.AddPresentationDescriptor("AdornerDecorator");
			this.AddPresentationDescriptor("ComponentResourceKey");
			this.AddPresentationDescriptor("Button", "Content");
			this.AddPresentationDescriptor("FrameworkRichTextComposition", "Text");
			this.AddPresentationDescriptor("LinkTarget");
			this.AddPresentationDescriptor("TextBlock", "Text");
			this.AddPresentationDescriptor("DataTemplateSelector");
			this.AddPresentationDescriptor("MediaElement");
			this.AddPresentationDescriptor("PrintDialogException");
			this.AddPresentationDescriptor("DialogResultConverter");
			this.AddPresentationDescriptor("ComboBoxItem", "Content");
			this.AddPresentationDescriptor("AttachedPropertyBrowsableForChildrenAttribute");
			this.AddPresentationDescriptor("RowDefinition");
			this.AddPresentationDescriptor("TextSearch");
			this.AddPresentationDescriptor("DocumentReference");
			this.AddPresentationDescriptor("GridViewColumn");
			this.AddPresentationDescriptor("ValidationError");
			this.AddPresentationDescriptor("PasswordBox");
			this.AddPresentationDescriptor("InkCanvas");
			this.AddPresentationDescriptor("DataTrigger");
			this.AddPresentationDescriptor("TemplatePartAttribute");
			this.AddPresentationDescriptor("BlockUIContainer");
			this.AddPresentationDescriptor("LengthConverter");
			this.AddPresentationDescriptor("TextChange");
			this.AddPresentationDescriptor("Decorator");
			this.AddPresentationDescriptor("ToolTip", "Content");
			this.AddPresentationDescriptor("FigureLengthConverter");
			this.AddPresentationDescriptor("ValidationResult");
			this.AddPresentationDescriptor("ContentControl", "Content");
			this.AddPresentationDescriptor("CornerRadiusConverter");
			this.AddPresentationDescriptor("JournalEntryListConverter");
			this.AddPresentationDescriptor("ToggleButton", "Content");
			this.AddPresentationDescriptor("Paragraph");
			this.AddPresentationDescriptor("HeaderedContentControl", "Content");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "LineBreak"), new ContentDescriptor(true, true, null, null));
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Window"), new ContentDescriptor(true, false, "Content", "Title"));
			this.AddPresentationDescriptor("StyleSelector");
			this.AddPresentationDescriptor("FixedPage");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Path"), new ContentDescriptor(false, false, null, null));
			this.AddPresentationDescriptor("GroupStyleSelector");
			this.AddPresentationDescriptor("GroupStyle");
			this.AddPresentationDescriptor("BorderGapMaskConverter");
			this.AddPresentationDescriptor("Slider");
			this.AddPresentationDescriptor("GroupItem", "Content");
			this.AddPresentationDescriptor("ResourceDictionary");
			this.AddPresentationDescriptor("StackPanel");
			this.AddPresentationDescriptor("DockPanel");
			this.AddPresentationDescriptor("Image");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Ellipse"), new ContentDescriptor(false, false, null, null));
			this.AddPresentationDescriptor("HeaderedItemsControl");
			this.AddPresentationDescriptor("ColumnDefinition");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Polygon"), new ContentDescriptor(false, false, null, null));
			this.AddPresentationDescriptor("PropertyPathConverter");
			this.AddPresentationDescriptor("Menu");
			this.AddPresentationDescriptor("Condition");
			this.AddPresentationDescriptor("TemplateBindingExtension");
			this.AddPresentationDescriptor("TextElementEditingBehaviorAttribute");
			this.AddPresentationDescriptor("RepeatButton", "Content");
			this.AddPresentationDescriptor("AdornedElementPlaceholder");
			this.AddPresentationDescriptor("JournalEntry");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Figure"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("BulletDecorator");
			this.AddPresentationDescriptor("SpellingError");
			this.AddPresentationDescriptor("InkPresenter");
			this.AddPresentationDescriptor("DataTemplateKey");
			this.AddPresentationDescriptor("ItemsPanelTemplate");
			this.AddPresentationDescriptor("FlowDocumentPageViewer");
			this.AddPresentationDescriptor("GridViewRowPresenter", "Content");
			this.AddPresentationDescriptor("ThicknessConverter");
			this.AddPresentationDescriptor("FixedDocumentSequence");
			this.AddPresentationDescriptor("MenuScrollingVisibilityConverter");
			this.AddPresentationDescriptor("TemplateBindingExpressionConverter");
			this.AddPresentationDescriptor("GridViewHeaderRowPresenter");
			this.AddPresentationDescriptor("TreeViewItem");
			this.AddPresentationDescriptor("TemplateBindingExtensionConverter");
			this.AddPresentationDescriptor("MultiTrigger");
			this.AddPresentationDescriptor("ComboBox", "Text");
			this.AddPresentationDescriptor("UniformGrid");
			this.AddPresentationDescriptor("ListBoxItem", "Content");
			this.AddPresentationDescriptor("Grid");
			this.AddPresentationDescriptor("Trigger");
			this.AddPresentationDescriptor("RichTextBox");
			this.AddPresentationDescriptor("GroupBox", "Content");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "InlineUIContainer"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("CheckBox", "Content");
			this.AddPresentationDescriptor("ToolBarPanel");
			this.AddPresentationDescriptor("DynamicResourceExtension");
			this.AddPresentationDescriptor("FontSizeConverter");
			this.AddPresentationDescriptor("Separator");
			this.AddPresentationDescriptor("Table");
			this.AddPresentationDescriptor("VirtualizingStackPanel");
			this.AddPresentationDescriptor("DocumentViewer");
			this.AddPresentationDescriptor("TableRow");
			this.AddPresentationDescriptor("RadioButton", "Content");
			this.AddPresentationDescriptor("StaticResourceExtension");
			this.AddPresentationDescriptor("TableColumn");
			this.AddPresentationDescriptor("Track");
			this.AddPresentationDescriptor("ProgressBar");
			this.AddPresentationDescriptor("ListViewItem", "Content");
			this.AddPresentationDescriptor("ZoomPercentageConverter");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Floater"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("TabItem", "Content");
			this.AddPresentationDescriptor("FlowDocument");
			this.AddPresentationDescriptor("Label", "Content");
			this.AddPresentationDescriptor("WrapPanel");
			this.AddPresentationDescriptor("ListItem");
			this.AddPresentationDescriptor("FrameworkPropertyMetadata");
			this.AddPresentationDescriptor("NameScope");
			this.AddPresentationDescriptor("TreeView");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Rectangle"), new ContentDescriptor(false, false, null, null));
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Hyperlink"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("TableRowGroup");
			this.AddPresentationDescriptor("Application");
			this.AddPresentationDescriptor("TickBar");
			this.AddPresentationDescriptor("ResizeGrip");
			this.AddPresentationDescriptor("FrameworkElement");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Run"), new ContentDescriptor(true, true, "Text", null));
			this.AddPresentationDescriptor("FrameworkContentElement");
			this.AddPresentationDescriptor("ItemContainerGenerator");
			this.AddPresentationDescriptor("ThemeDictionaryExtension");
			this.AddPresentationDescriptor("AccessText", "Text");
			this.AddPresentationDescriptor("Frame", "Content");
			this.AddPresentationDescriptor("LostFocusEventManager");
			this.AddPresentationDescriptor("EventTrigger");
			this.AddPresentationDescriptor("DataErrorValidationRule");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Page"), new ContentDescriptor(true, false, "Content", "WindowTitle"));
			this.AddPresentationDescriptor("GridLengthConverter");
			this.AddPresentationDescriptor("TextSelection", "Text");
			this.AddPresentationDescriptor("FixedDocument");
			this.AddPresentationDescriptor("HierarchicalDataTemplate");
			this.AddPresentationDescriptor("MessageBox");
			this.AddPresentationDescriptor("Style");
			this.AddPresentationDescriptor("ScrollContentPresenter", "Content");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Span"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("TextPointer");
			this.AddPresentationDescriptor("FrameworkElementFactory", "Text");
			this.AddPresentationDescriptor("ExceptionValidationRule");
			this.AddPresentationDescriptor("DocumentPageView");
			this.AddPresentationDescriptor("ToolBar");
			this.AddPresentationDescriptor("ListView");
			this.AddPresentationDescriptor("StyleTypedPropertyAttribute");
			this.AddPresentationDescriptor("ToolBarOverflowPanel");
			this.AddPresentationDescriptor("BooleanToVisibilityConverter");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Line"), new ContentDescriptor(false, false, null, null));
			this.AddPresentationDescriptor("MenuItem");
			this.AddPresentationDescriptor("Section");
			this.AddPresentationDescriptor("DynamicResourceExtensionConverter");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Underline"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("TemplateBindingExpression");
			this.AddPresentationDescriptor("Viewport3D");
			this.AddPresentationDescriptor("PrintDialog");
			this.AddPresentationDescriptor("ItemsPresenter");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/xps/2005/06", "Polyline"), new ContentDescriptor(false, false, null, null));
			this.AddPresentationDescriptor("FrameworkTextComposition", "Text");
			this.AddPresentationDescriptor("TextRange", "Text");
			this.AddPresentationDescriptor("StatusBarItem", "Content");
			this.AddPresentationDescriptor("FlowDocumentReader");
			this.AddPresentationDescriptor("TextEffectTarget");
			this.AddPresentationDescriptor("ColorConvertedBitmapExtension");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "NavigationWindow"), new ContentDescriptor(true, false, "Content", "Title"));
			this.AddPresentationDescriptor("AdornerLayer");
			this.AddPresentationDescriptor("GridView");
			this.AddPresentationDescriptor("CustomPopupPlacementCallback");
			this.AddPresentationDescriptor("MultiDataTrigger");
			this.AddPresentationDescriptor("NavigationService", "Content");
			this.AddPresentationDescriptor("PropertyPath");
			this._xamlElementContentDescriptorDictionary.Add(new ElementTableKey("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Bold"), new ContentDescriptor(true, true, null, null));
			this.AddPresentationDescriptor("ResourceReferenceKeyNotFoundException");
			this.AddPresentationDescriptor("StatusBar");
			this.AddPresentationDescriptor("Border");
			this.AddPresentationDescriptor("SpellCheck");
			this.AddPresentationDescriptor("SoundPlayerAction");
			this.AddPresentationDescriptor("ContentPresenter", "Content");
			this.AddPresentationDescriptor("EventSetter");
			this.AddPresentationDescriptor("StickyNoteControl");
			this.AddPresentationDescriptor("UserControl", "Content");
			this.AddPresentationDescriptor("FlowDocumentScrollViewer");
			this.AddPresentationDescriptor("ThemeInfoAttribute");
			this.AddPresentationDescriptor("List");
			this.AddPresentationDescriptor("DataTemplate");
			this.AddPresentationDescriptor("GridSplitter");
			this.AddPresentationDescriptor("TableCell");
			this.AddPresentationDescriptor("Thumb");
			this.AddPresentationDescriptor("Glyphs");
			this.AddPresentationDescriptor("ScrollViewer", "Content");
			this.AddPresentationDescriptor("TabPanel");
			this.AddPresentationDescriptor("Setter");
			this.AddPresentationDescriptor("PageContent");
			this.AddPresentationDescriptor("TabControl");
			this.AddPresentationDescriptor("Typography");
			this.AddPresentationDescriptor("ScrollBar");
			this.AddPresentationDescriptor("NullableBoolConverter");
			this.AddPresentationDescriptor("ControlTemplate");
			this.AddPresentationDescriptor("ContextMenu");
			this.AddPresentationDescriptor("Popup");
			this.AddPresentationDescriptor("Control");
			this.AddPresentationDescriptor("ToolBarTray");
			this.AddPresentationDescriptor("Expander", "Content");
			this.AddPresentationDescriptor("JournalEntryUnifiedViewConverter");
			this.AddPresentationDescriptor("Viewbox");
		}

		// Token: 0x06006C44 RID: 27716 RVA: 0x001F2F1C File Offset: 0x001F111C
		static XamlFilter()
		{
			EventLog eventLog = new EventLog();
			eventLog.Log = "Application";
			eventLog.Source = "XAML filter";
			Trace.Listeners.Add(new EventLogTraceListener(eventLog));
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x001F2F68 File Offset: 0x001F1168
		internal XamlFilter(Stream stream)
		{
			Trace.TraceInformation("New Xaml filter created.");
			this._lcidDictionary = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			this._contextStack = new Stack(32);
			this.InitializeDeclaredFields();
			this._xamlStream = stream;
			this.CreateXmlReader();
			this._filterState = XamlFilter.FilterState.FindNextUnit;
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x001F2FD8 File Offset: 0x001F11D8
		private void InitializeDeclaredFields()
		{
			this.ClearStack();
			this._filterState = XamlFilter.FilterState.Uninitialized;
			this._currentChunkID = 0U;
			this.LoadContentDescriptorDictionary();
			this._countOfCharactersReturned = 0;
			this._currentContent = null;
			this._indexingContentUnit = null;
			this._expectingBlockStart = true;
			this._topLevelReader = null;
			this._fixedPageContentExtractor = null;
			this._fixedPageDomTree = null;
		}

		// Token: 0x06006C47 RID: 27719 RVA: 0x001F3030 File Offset: 0x001F1230
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes)
		{
			this._filterContents = true;
			if (aAttributes != null && aAttributes.Length != 0)
			{
				this._filterContents = false;
				for (int i = 0; i < aAttributes.Length; i++)
				{
					if (aAttributes[i].Guid == IndexingFilterMarshaler.PSGUID_STORAGE && aAttributes[i].Property.PropType == PropSpecType.Id && aAttributes[i].Property.PropId == 19U)
					{
						this._filterContents = true;
						break;
					}
				}
			}
			this._returnCanonicalParagraphBreaks = ((grfFlags & IFILTER_INIT.IFILTER_INIT_CANON_PARAGRAPHS) > (IFILTER_INIT)0);
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06006C48 RID: 27720 RVA: 0x001F30AC File Offset: 0x001F12AC
		public ManagedChunk GetChunk()
		{
			if (!this._filterContents)
			{
				this._currentContent = null;
				return null;
			}
			if (this._xamlReader == null)
			{
				throw new COMException(SR.Get("FilterGetChunkNoStream"), -2147215613);
			}
			if (this._filterState == XamlFilter.FilterState.EndOfStream)
			{
				this.EnsureXmlReaderIsClosed();
				return null;
			}
			IndexingContentUnit indexingContentUnit;
			try
			{
				indexingContentUnit = this.NextContentUnit();
			}
			catch (XmlException ex)
			{
				this.EnsureXmlReaderIsClosed();
				throw new COMException(ex.Message, -2147215604);
			}
			if (indexingContentUnit == null)
			{
				this._currentContent = null;
				this.EnsureXmlReaderIsClosed();
				return null;
			}
			this._currentContent = indexingContentUnit.Text;
			this._countOfCharactersReturned = 0;
			return indexingContentUnit;
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x001F3150 File Offset: 0x001F1350
		public string GetText(int bufferCharacterCount)
		{
			if (this._currentContent == null)
			{
				SecurityHelper.ThrowExceptionForHR(-2147215611);
			}
			int num = this._currentContent.Length - this._countOfCharactersReturned;
			if (num <= 0)
			{
				SecurityHelper.ThrowExceptionForHR(-2147215615);
			}
			if (num > bufferCharacterCount)
			{
				num = bufferCharacterCount;
			}
			string result = this._currentContent.Substring(this._countOfCharactersReturned, num);
			this._countOfCharactersReturned += num;
			return result;
		}

		// Token: 0x06006C4A RID: 27722 RVA: 0x001F31B8 File Offset: 0x001F13B8
		public object GetValue()
		{
			SecurityHelper.ThrowExceptionForHR(-2147215610);
			return null;
		}

		// Token: 0x06006C4B RID: 27723 RVA: 0x001F31C8 File Offset: 0x001F13C8
		internal IndexingContentUnit NextContentUnit()
		{
			IndexingContentUnit indexingContentUnit = null;
			while (indexingContentUnit == null)
			{
				if (this._filterState == XamlFilter.FilterState.UseContentExtractor)
				{
					if (!this._fixedPageContentExtractor.AtEndOfPage)
					{
						bool flag;
						uint lcid;
						string text = this._fixedPageContentExtractor.NextGlyphContent(out flag, out lcid);
						this._expectingBlockStart = !flag;
						return this.BuildIndexingContentUnit(text, lcid);
					}
					this._fixedPageContentExtractor = null;
					this._topLevelReader = this._xamlReader;
					this._xamlReader = new XmlNodeReader(this._fixedPageDomTree.DocumentElement);
					this._filterState = XamlFilter.FilterState.FindNextFlowUnit;
				}
				if (this._xamlReader.EOF)
				{
					XamlFilter.FilterState filterState = this._filterState;
					if (filterState == XamlFilter.FilterState.FindNextUnit)
					{
						this._filterState = XamlFilter.FilterState.EndOfStream;
						return null;
					}
					if (filterState == XamlFilter.FilterState.FindNextFlowUnit)
					{
						this._xamlReader.Close();
						this._xamlReader = this._topLevelReader;
						this._filterState = XamlFilter.FilterState.FindNextUnit;
					}
				}
				XmlNodeType nodeType = this._xamlReader.NodeType;
				if (nodeType <= XmlNodeType.CDATA)
				{
					if (nodeType == XmlNodeType.Element)
					{
						indexingContentUnit = this.HandleElementStart();
						continue;
					}
					if (nodeType - XmlNodeType.Text > 1)
					{
						goto IL_103;
					}
				}
				else if (nodeType != XmlNodeType.SignificantWhitespace)
				{
					if (nodeType != XmlNodeType.EndElement)
					{
						goto IL_103;
					}
					indexingContentUnit = this.HandleElementEnd();
					continue;
				}
				indexingContentUnit = this.HandleTextData();
				continue;
				IL_103:
				this._xamlReader.Read();
			}
			return indexingContentUnit;
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x001F32EB File Offset: 0x001F14EB
		private void LoadContentDescriptorDictionary()
		{
			this.InitElementDictionary();
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x001F32F4 File Offset: 0x001F14F4
		private IndexingContentUnit BuildIndexingContentUnit(string text, uint lcid)
		{
			CHUNK_BREAKTYPE breakType = CHUNK_BREAKTYPE.CHUNK_NO_BREAK;
			if (this._expectingBlockStart)
			{
				breakType = CHUNK_BREAKTYPE.CHUNK_EOP;
				if (this._returnCanonicalParagraphBreaks)
				{
					text = "\u2029" + text;
				}
			}
			if (this._indexingContentUnit == null)
			{
				this._indexingContentUnit = new IndexingContentUnit(text, this.AllocateChunkID(), breakType, XamlFilter._propSpec, lcid);
			}
			else
			{
				this._indexingContentUnit.InitIndexingContentUnit(text, this.AllocateChunkID(), breakType, XamlFilter._propSpec, lcid);
			}
			this._expectingBlockStart = false;
			return this._indexingContentUnit;
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x001F336B File Offset: 0x001F156B
		private ContentDescriptor GetContentInformationAboutCustomElement(ElementTableKey customElement)
		{
			return this._defaultContentDescriptor;
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x001F3374 File Offset: 0x001F1574
		private IndexingContentUnit HandleTextData()
		{
			ContentDescriptor contentDescriptor = this.TopOfStack();
			if (contentDescriptor != null)
			{
				IndexingContentUnit result = this.BuildIndexingContentUnit(this._xamlReader.Value, this.GetCurrentLcid());
				this._xamlReader.Read();
				return result;
			}
			this._xamlReader.Read();
			return null;
		}

		// Token: 0x06006C50 RID: 27728 RVA: 0x001F33C0 File Offset: 0x001F15C0
		private IndexingContentUnit HandleElementStart()
		{
			ElementTableKey elementTableKey = new ElementTableKey(this._xamlReader.NamespaceURI, this._xamlReader.LocalName);
			string a;
			if (this.IsPrefixedPropertyName(elementTableKey.BaseName, out a))
			{
				ContentDescriptor contentDescriptor = this.TopOfStack();
				if (contentDescriptor == null)
				{
					this.SkipCurrentElement();
					return null;
				}
				bool flag = elementTableKey.XmlNamespace.Equals(ElementTableKey.XamlNamespace, StringComparison.Ordinal) && (a == contentDescriptor.ContentProp || a == contentDescriptor.TitleProp);
				if (!flag)
				{
					this.SkipCurrentElement();
					return null;
				}
				this.Push(new ContentDescriptor(flag, this.TopOfStack().IsInline, string.Empty, null));
				this._xamlReader.Read();
				return null;
			}
			else
			{
				bool flag2;
				IndexingContentUnit indexingContentUnit = this.HandleFixedFormatTag(elementTableKey, out flag2);
				if (flag2)
				{
					return indexingContentUnit;
				}
				Invariant.Assert(indexingContentUnit == null);
				ContentDescriptor contentDescriptor2 = (ContentDescriptor)this._xamlElementContentDescriptorDictionary[elementTableKey];
				if (contentDescriptor2 == null)
				{
					if (elementTableKey.XmlNamespace.Equals(ElementTableKey.XamlNamespace, StringComparison.Ordinal))
					{
						contentDescriptor2 = this._defaultContentDescriptor;
					}
					else if (elementTableKey.XmlNamespace.Equals("http://schemas.microsoft.com/winfx/2006/xaml", StringComparison.Ordinal))
					{
						contentDescriptor2 = this._nonIndexableElementDescriptor;
					}
					else
					{
						contentDescriptor2 = this.GetContentInformationAboutCustomElement(elementTableKey);
					}
					this._xamlElementContentDescriptorDictionary.Add(elementTableKey, contentDescriptor2);
				}
				if (!contentDescriptor2.HasIndexableContent)
				{
					this.SkipCurrentElement();
					return null;
				}
				if (contentDescriptor2.TitleProp != null && (this._attributesToIgnore & XamlFilter.AttributesToIgnore.Title) == XamlFilter.AttributesToIgnore.None)
				{
					string propertyAsAttribute = this.GetPropertyAsAttribute(contentDescriptor2.TitleProp);
					if (propertyAsAttribute != null && propertyAsAttribute.Length > 0)
					{
						this._attributesToIgnore |= XamlFilter.AttributesToIgnore.Title;
						this._expectingBlockStart = true;
						IndexingContentUnit result = this.BuildIndexingContentUnit(propertyAsAttribute, this.GetCurrentLcid());
						this._expectingBlockStart = true;
						return result;
					}
				}
				if (contentDescriptor2.ContentProp != null && (this._attributesToIgnore & XamlFilter.AttributesToIgnore.Content) == XamlFilter.AttributesToIgnore.None)
				{
					string propertyAsAttribute2 = this.GetPropertyAsAttribute(contentDescriptor2.ContentProp);
					if (propertyAsAttribute2 != null && propertyAsAttribute2.Length > 0)
					{
						this._attributesToIgnore |= XamlFilter.AttributesToIgnore.Content;
						if (!contentDescriptor2.IsInline)
						{
							this._expectingBlockStart = true;
						}
						IndexingContentUnit result2 = this.BuildIndexingContentUnit(propertyAsAttribute2, this.GetCurrentLcid());
						this._expectingBlockStart = !contentDescriptor2.IsInline;
						return result2;
					}
				}
				this._attributesToIgnore = XamlFilter.AttributesToIgnore.None;
				if (this._xamlReader.IsEmptyElement)
				{
					if (!contentDescriptor2.IsInline)
					{
						this._expectingBlockStart = true;
					}
					this._xamlReader.Read();
					return null;
				}
				this.Push(contentDescriptor2);
				this._xamlReader.Read();
				return null;
			}
		}

		// Token: 0x06006C51 RID: 27729 RVA: 0x001F3630 File Offset: 0x001F1830
		private IndexingContentUnit HandleElementEnd()
		{
			ContentDescriptor contentDescriptor = this.Pop();
			this._xamlReader.Read();
			return null;
		}

		// Token: 0x06006C52 RID: 27730 RVA: 0x001F3654 File Offset: 0x001F1854
		private IndexingContentUnit HandleFixedFormatTag(ElementTableKey elementFullName, out bool handled)
		{
			handled = true;
			if (!elementFullName.XmlNamespace.Equals(ElementTableKey.FixedMarkupNamespace, StringComparison.Ordinal))
			{
				handled = false;
				return null;
			}
			if (string.CompareOrdinal(elementFullName.BaseName, "Glyphs") == 0)
			{
				if (this._filterState == XamlFilter.FilterState.FindNextFlowUnit)
				{
					this.SkipCurrentElement();
					return null;
				}
				return this.ProcessGlyphRun();
			}
			else if (string.CompareOrdinal(elementFullName.BaseName, "FixedPage") == 0)
			{
				if (this._filterState == XamlFilter.FilterState.FindNextFlowUnit)
				{
					this.Push(this._defaultContentDescriptor);
					this._xamlReader.Read();
					return null;
				}
				return this.ProcessFixedPage();
			}
			else
			{
				if (string.CompareOrdinal(elementFullName.BaseName, "PageContent") != 0)
				{
					handled = false;
					return null;
				}
				string attribute = this._xamlReader.GetAttribute("Source");
				if (attribute != null)
				{
					this.SkipCurrentElement();
					return null;
				}
				this.Push(this._defaultContentDescriptor);
				this._xamlReader.Read();
				return null;
			}
		}

		// Token: 0x06006C53 RID: 27731 RVA: 0x001F3730 File Offset: 0x001F1930
		private IndexingContentUnit ProcessGlyphRun()
		{
			string attribute = this._xamlReader.GetAttribute("UnicodeString");
			if (attribute == null || attribute.Length == 0)
			{
				this.SkipCurrentElement();
				return null;
			}
			this._expectingBlockStart = true;
			uint currentLcid = this.GetCurrentLcid();
			this.SkipCurrentElement();
			return this.BuildIndexingContentUnit(attribute, currentLcid);
		}

		// Token: 0x06006C54 RID: 27732 RVA: 0x001F3780 File Offset: 0x001F1980
		private IndexingContentUnit ProcessFixedPage()
		{
			if (this._filterState == XamlFilter.FilterState.FindNextFlowUnit)
			{
				throw new XmlException(SR.Get("XamlFilterNestedFixedPage"));
			}
			string xml = this._xamlReader.ReadOuterXml();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			if (this._xamlReader.XmlLang.Length > 0)
			{
				xmlDocument.DocumentElement.SetAttribute("xml:lang", this._xamlReader.XmlLang);
			}
			this._fixedPageContentExtractor = new FixedPageContentExtractor(xmlDocument.DocumentElement);
			this._fixedPageDomTree = xmlDocument;
			this._filterState = XamlFilter.FilterState.UseContentExtractor;
			return null;
		}

		// Token: 0x06006C55 RID: 27733 RVA: 0x001F380D File Offset: 0x001F1A0D
		private void CreateXmlReader()
		{
			if (this._xamlReader != null)
			{
				this._xamlReader.Close();
			}
			this._xamlReader = new XmlTextReader(this._xamlStream);
			((XmlTextReader)this._xamlReader).WhitespaceHandling = WhitespaceHandling.Significant;
			this._attributesToIgnore = XamlFilter.AttributesToIgnore.None;
		}

		// Token: 0x06006C56 RID: 27734 RVA: 0x001F384B File Offset: 0x001F1A4B
		private void EnsureXmlReaderIsClosed()
		{
			if (this._xamlReader != null)
			{
				this._xamlReader.Close();
			}
		}

		// Token: 0x06006C57 RID: 27735 RVA: 0x001F3860 File Offset: 0x001F1A60
		private uint GetCurrentLcid()
		{
			string languageString = this.GetLanguageString();
			if (languageString.Length == 0)
			{
				return (uint)CultureInfo.InvariantCulture.LCID;
			}
			if (this._lcidDictionary.ContainsKey(languageString))
			{
				return this._lcidDictionary[languageString];
			}
			CultureInfo cultureInfo = new CultureInfo(languageString);
			this._lcidDictionary.Add(languageString, (uint)cultureInfo.LCID);
			return (uint)cultureInfo.LCID;
		}

		// Token: 0x06006C58 RID: 27736 RVA: 0x001F38C4 File Offset: 0x001F1AC4
		private string GetLanguageString()
		{
			string xmlLang = this._xamlReader.XmlLang;
			if (xmlLang.Length == 0 && this._topLevelReader != null)
			{
				xmlLang = this._topLevelReader.XmlLang;
			}
			return xmlLang;
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x001F38FA File Offset: 0x001F1AFA
		private void SkipCurrentElement()
		{
			this._xamlReader.Skip();
		}

		// Token: 0x06006C5A RID: 27738 RVA: 0x001F3908 File Offset: 0x001F1B08
		private bool IsPrefixedPropertyName(string name, out string propertyName)
		{
			int num = name.IndexOf('.');
			if (num == -1)
			{
				propertyName = null;
				return false;
			}
			propertyName = name.Substring(num + 1);
			return true;
		}

		// Token: 0x06006C5B RID: 27739 RVA: 0x001F3933 File Offset: 0x001F1B33
		private uint AllocateChunkID()
		{
			Invariant.Assert(this._currentChunkID <= uint.MaxValue);
			this._currentChunkID += 1U;
			return this._currentChunkID;
		}

		// Token: 0x06006C5C RID: 27740 RVA: 0x001F395C File Offset: 0x001F1B5C
		private string GetPropertyAsAttribute(string propertyName)
		{
			string text = this._xamlReader.GetAttribute(propertyName);
			if (text != null)
			{
				return text;
			}
			bool flag = this._xamlReader.MoveToFirstAttribute();
			while (flag)
			{
				string text2;
				if (this.IsPrefixedPropertyName(this._xamlReader.LocalName, out text2) && text2.Equals(propertyName, StringComparison.Ordinal))
				{
					text = this._xamlReader.Value;
					break;
				}
				flag = this._xamlReader.MoveToNextAttribute();
			}
			this._xamlReader.MoveToElement();
			return text;
		}

		// Token: 0x06006C5D RID: 27741 RVA: 0x001F39D2 File Offset: 0x001F1BD2
		private ContentDescriptor TopOfStack()
		{
			return (ContentDescriptor)this._contextStack.Peek();
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x001F39E4 File Offset: 0x001F1BE4
		private void Push(ContentDescriptor contentDescriptor)
		{
			if (!contentDescriptor.IsInline)
			{
				this._expectingBlockStart = true;
			}
			this._contextStack.Push(contentDescriptor);
		}

		// Token: 0x06006C5F RID: 27743 RVA: 0x001F3A04 File Offset: 0x001F1C04
		private ContentDescriptor Pop()
		{
			ContentDescriptor contentDescriptor = (ContentDescriptor)this._contextStack.Pop();
			if (!contentDescriptor.IsInline)
			{
				this._expectingBlockStart = true;
			}
			return contentDescriptor;
		}

		// Token: 0x06006C60 RID: 27744 RVA: 0x001F3A32 File Offset: 0x001F1C32
		private void ClearStack()
		{
			this._contextStack.Clear();
		}

		// Token: 0x0400351E RID: 13598
		private const string _inDocumentCodeURI = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x0400351F RID: 13599
		private const string _pageContentName = "PageContent";

		// Token: 0x04003520 RID: 13600
		private const string _glyphRunName = "Glyphs";

		// Token: 0x04003521 RID: 13601
		private const string _pageContentSourceAttribute = "Source";

		// Token: 0x04003522 RID: 13602
		private const string _fixedPageName = "FixedPage";

		// Token: 0x04003523 RID: 13603
		private const string _xmlLangAttribute = "xml:lang";

		// Token: 0x04003524 RID: 13604
		private const string _paragraphSeparator = "\u2029";

		// Token: 0x04003525 RID: 13605
		private const string _unicodeStringAttribute = "UnicodeString";

		// Token: 0x04003526 RID: 13606
		private readonly ContentDescriptor _defaultContentDescriptor = new ContentDescriptor(true, false, null, null);

		// Token: 0x04003527 RID: 13607
		private readonly ContentDescriptor _nonIndexableElementDescriptor = new ContentDescriptor(false);

		// Token: 0x04003528 RID: 13608
		private static readonly ManagedFullPropSpec _propSpec = new ManagedFullPropSpec(IndexingFilterMarshaler.PSGUID_STORAGE, 19U);

		// Token: 0x04003529 RID: 13609
		private Stack _contextStack;

		// Token: 0x0400352A RID: 13610
		private XamlFilter.FilterState _filterState;

		// Token: 0x0400352B RID: 13611
		private string _currentContent;

		// Token: 0x0400352C RID: 13612
		private uint _currentChunkID;

		// Token: 0x0400352D RID: 13613
		private int _countOfCharactersReturned;

		// Token: 0x0400352E RID: 13614
		private IndexingContentUnit _indexingContentUnit;

		// Token: 0x0400352F RID: 13615
		private bool _expectingBlockStart;

		// Token: 0x04003530 RID: 13616
		private XmlReader _topLevelReader;

		// Token: 0x04003531 RID: 13617
		private FixedPageContentExtractor _fixedPageContentExtractor;

		// Token: 0x04003532 RID: 13618
		private XmlDocument _fixedPageDomTree;

		// Token: 0x04003533 RID: 13619
		private Stream _xamlStream;

		// Token: 0x04003534 RID: 13620
		private bool _filterContents;

		// Token: 0x04003535 RID: 13621
		private bool _returnCanonicalParagraphBreaks;

		// Token: 0x04003536 RID: 13622
		private XmlReader _xamlReader;

		// Token: 0x04003537 RID: 13623
		private XamlFilter.AttributesToIgnore _attributesToIgnore;

		// Token: 0x04003538 RID: 13624
		private Hashtable _xamlElementContentDescriptorDictionary;

		// Token: 0x04003539 RID: 13625
		private Dictionary<string, uint> _lcidDictionary;

		// Token: 0x02000B1D RID: 2845
		internal enum FilterState
		{
			// Token: 0x04004A47 RID: 19015
			Uninitialized = 1,
			// Token: 0x04004A48 RID: 19016
			FindNextUnit,
			// Token: 0x04004A49 RID: 19017
			FindNextFlowUnit,
			// Token: 0x04004A4A RID: 19018
			UseContentExtractor,
			// Token: 0x04004A4B RID: 19019
			EndOfStream
		}

		// Token: 0x02000B1E RID: 2846
		[Flags]
		internal enum AttributesToIgnore
		{
			// Token: 0x04004A4D RID: 19021
			None = 0,
			// Token: 0x04004A4E RID: 19022
			Title = 1,
			// Token: 0x04004A4F RID: 19023
			Content = 2
		}
	}
}
