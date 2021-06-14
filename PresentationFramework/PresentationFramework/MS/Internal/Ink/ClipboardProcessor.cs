using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace MS.Internal.Ink
{
	// Token: 0x02000682 RID: 1666
	internal class ClipboardProcessor
	{
		// Token: 0x06006D10 RID: 27920 RVA: 0x001F54A7 File Offset: 0x001F36A7
		internal ClipboardProcessor(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._preferredClipboardData = new Dictionary<InkCanvasClipboardFormat, ClipboardData>();
			this._preferredClipboardData.Add(InkCanvasClipboardFormat.InkSerializedFormat, new ISFClipboardData());
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x001F54E0 File Offset: 0x001F36E0
		internal bool CheckDataFormats(IDataObject dataObject)
		{
			foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
			{
				if (keyValuePair.Value.CanPaste(dataObject))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x001F5544 File Offset: 0x001F3744
		[SecurityCritical]
		internal InkCanvasClipboardDataFormats CopySelectedData(IDataObject dataObject)
		{
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = InkCanvasClipboardDataFormats.None;
			InkCanvasSelection inkCanvasSelection = this.InkCanvas.InkCanvasSelection;
			StrokeCollection strokeCollection = inkCanvasSelection.SelectedStrokes;
			if (strokeCollection.Count > 1)
			{
				StrokeCollection strokeCollection2 = new StrokeCollection();
				StrokeCollection strokes = this.InkCanvas.Strokes;
				int num = 0;
				while (num < strokes.Count && strokeCollection.Count != strokeCollection2.Count)
				{
					for (int i = 0; i < strokeCollection.Count; i++)
					{
						if (strokes[num] == strokeCollection[i])
						{
							strokeCollection2.Add(strokeCollection[i]);
							break;
						}
					}
					num++;
				}
				strokeCollection = strokeCollection2.Clone();
			}
			else
			{
				strokeCollection = strokeCollection.Clone();
			}
			List<UIElement> list = new List<UIElement>(inkCanvasSelection.SelectedElements);
			Rect selectionBounds = inkCanvasSelection.SelectionBounds;
			if (strokeCollection.Count != 0 || list.Count != 0)
			{
				Matrix identity = Matrix.Identity;
				identity.OffsetX = -selectionBounds.Left;
				identity.OffsetY = -selectionBounds.Top;
				if (strokeCollection.Count != 0)
				{
					inkCanvasSelection.TransformStrokes(strokeCollection, identity);
					ClipboardData clipboardData = new ISFClipboardData(strokeCollection);
					clipboardData.CopyToDataObject(dataObject);
					inkCanvasClipboardDataFormats |= InkCanvasClipboardDataFormats.ISF;
				}
				if (this.CopySelectionInXAML(dataObject, strokeCollection, list, identity, selectionBounds.Size))
				{
					inkCanvasClipboardDataFormats |= InkCanvasClipboardDataFormats.XAML;
				}
			}
			return inkCanvasClipboardDataFormats;
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x001F567C File Offset: 0x001F387C
		internal bool PasteData(IDataObject dataObject, ref StrokeCollection newStrokes, ref List<UIElement> newElements)
		{
			foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
			{
				InkCanvasClipboardFormat key = keyValuePair.Key;
				ClipboardData value = keyValuePair.Value;
				if (value.CanPaste(dataObject))
				{
					switch (key)
					{
					case InkCanvasClipboardFormat.InkSerializedFormat:
					{
						ISFClipboardData isfclipboardData = (ISFClipboardData)value;
						isfclipboardData.PasteFromDataObject(dataObject);
						newStrokes = isfclipboardData.Strokes;
						break;
					}
					case InkCanvasClipboardFormat.Text:
					{
						TextClipboardData textClipboardData = (TextClipboardData)value;
						textClipboardData.PasteFromDataObject(dataObject);
						newElements = textClipboardData.Elements;
						break;
					}
					case InkCanvasClipboardFormat.Xaml:
					{
						XamlClipboardData xamlClipboardData = (XamlClipboardData)value;
						xamlClipboardData.PasteFromDataObject(dataObject);
						List<UIElement> elements = xamlClipboardData.Elements;
						if (elements != null && elements.Count != 0)
						{
							if (elements.Count == 1 && ClipboardProcessor.InkCanvasDType.IsInstanceOfType(elements[0]))
							{
								this.TearDownInkCanvasContainer((InkCanvas)elements[0], ref newStrokes, ref newElements);
							}
							else
							{
								newElements = elements;
							}
						}
						break;
					}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x06006D14 RID: 27924 RVA: 0x001F57A0 File Offset: 0x001F39A0
		// (set) Token: 0x06006D15 RID: 27925 RVA: 0x001F57C0 File Offset: 0x001F39C0
		internal IEnumerable<InkCanvasClipboardFormat> PreferredFormats
		{
			get
			{
				foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
				{
					yield return keyValuePair.Key;
				}
				Dictionary<InkCanvasClipboardFormat, ClipboardData>.Enumerator enumerator = default(Dictionary<InkCanvasClipboardFormat, ClipboardData>.Enumerator);
				yield break;
				yield break;
			}
			set
			{
				Dictionary<InkCanvasClipboardFormat, ClipboardData> dictionary = new Dictionary<InkCanvasClipboardFormat, ClipboardData>();
				foreach (InkCanvasClipboardFormat key in value)
				{
					if (!dictionary.ContainsKey(key))
					{
						ClipboardData value2;
						switch (key)
						{
						case InkCanvasClipboardFormat.InkSerializedFormat:
							value2 = new ISFClipboardData();
							break;
						case InkCanvasClipboardFormat.Text:
							value2 = new TextClipboardData();
							break;
						case InkCanvasClipboardFormat.Xaml:
							value2 = new XamlClipboardData();
							break;
						default:
							throw new ArgumentException(SR.Get("InvalidClipboardFormat"), "value");
						}
						dictionary.Add(key, value2);
					}
				}
				this._preferredClipboardData = dictionary;
			}
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x001F5864 File Offset: 0x001F3A64
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool CopySelectionInXAML(IDataObject dataObject, StrokeCollection strokes, List<UIElement> elements, Matrix transform, Size size)
		{
			if (!SecurityHelper.CheckUnmanagedCodePermission())
			{
				return false;
			}
			InkCanvas inkCanvas = new InkCanvas();
			if (strokes.Count != 0)
			{
				inkCanvas.Strokes = strokes;
			}
			int count = elements.Count;
			if (count != 0)
			{
				InkCanvasSelection inkCanvasSelection = this.InkCanvas.InkCanvasSelection;
				for (int i = 0; i < count; i++)
				{
					try
					{
						string s = XamlWriter.Save(elements[i]);
						UIElement uielement = XamlReader.Load(new XmlTextReader(new StringReader(s))) as UIElement;
						((IAddChild)inkCanvas).AddChild(uielement);
						inkCanvasSelection.UpdateElementBounds(elements[i], uielement, transform);
					}
					catch (SecurityException)
					{
						inkCanvas = null;
						break;
					}
				}
			}
			if (inkCanvas != null)
			{
				inkCanvas.Width = size.Width;
				inkCanvas.Height = size.Height;
				ClipboardData clipboardData = new XamlClipboardData(new UIElement[]
				{
					inkCanvas
				});
				try
				{
					clipboardData.CopyToDataObject(dataObject);
				}
				catch (SecurityException)
				{
					inkCanvas = null;
				}
			}
			return inkCanvas != null;
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x001F5958 File Offset: 0x001F3B58
		private void TearDownInkCanvasContainer(InkCanvas rootInkCanvas, ref StrokeCollection newStrokes, ref List<UIElement> newElements)
		{
			newStrokes = rootInkCanvas.Strokes;
			if (rootInkCanvas.Children.Count != 0)
			{
				List<UIElement> list = new List<UIElement>(rootInkCanvas.Children.Count);
				foreach (object obj in rootInkCanvas.Children)
				{
					UIElement item = (UIElement)obj;
					list.Add(item);
				}
				foreach (UIElement element in list)
				{
					rootInkCanvas.Children.Remove(element);
				}
				newElements = list;
			}
		}

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x06006D18 RID: 27928 RVA: 0x001F5A24 File Offset: 0x001F3C24
		private InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x06006D19 RID: 27929 RVA: 0x001F5A2C File Offset: 0x001F3C2C
		private static DependencyObjectType InkCanvasDType
		{
			get
			{
				if (ClipboardProcessor.s_InkCanvasDType == null)
				{
					ClipboardProcessor.s_InkCanvasDType = DependencyObjectType.FromSystemTypeInternal(typeof(InkCanvas));
				}
				return ClipboardProcessor.s_InkCanvasDType;
			}
		}

		// Token: 0x040035D0 RID: 13776
		private InkCanvas _inkCanvas;

		// Token: 0x040035D1 RID: 13777
		private static DependencyObjectType s_InkCanvasDType;

		// Token: 0x040035D2 RID: 13778
		private Dictionary<InkCanvasClipboardFormat, ClipboardData> _preferredClipboardData;
	}
}
