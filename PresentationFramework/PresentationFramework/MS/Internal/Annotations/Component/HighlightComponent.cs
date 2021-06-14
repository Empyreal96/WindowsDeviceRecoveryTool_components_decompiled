using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using MS.Utility;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E2 RID: 2018
	internal class HighlightComponent : Canvas, IAnnotationComponent, IHighlightRange
	{
		// Token: 0x06007CAD RID: 31917 RVA: 0x00231354 File Offset: 0x0022F554
		public HighlightComponent()
		{
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x002313AC File Offset: 0x0022F5AC
		public HighlightComponent(int priority, bool highlightContent, XmlQualifiedName type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._priority = priority;
			this._type = type;
			this._highlightContent = highlightContent;
		}

		// Token: 0x17001CF7 RID: 7415
		// (get) Token: 0x06007CAF RID: 31919 RVA: 0x0023142C File Offset: 0x0022F62C
		public IList AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x17001CF8 RID: 7416
		// (get) Token: 0x06007CB0 RID: 31920 RVA: 0x00231455 File Offset: 0x0022F655
		// (set) Token: 0x06007CB1 RID: 31921 RVA: 0x0023145D File Offset: 0x0022F65D
		public PresentationContext PresentationContext
		{
			get
			{
				return this._presentationContext;
			}
			set
			{
				this._presentationContext = value;
			}
		}

		// Token: 0x17001CF9 RID: 7417
		// (get) Token: 0x06007CB2 RID: 31922 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		// (set) Token: 0x06007CB3 RID: 31923 RVA: 0x00002137 File Offset: 0x00000337
		public int ZOrder
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		// Token: 0x17001CFA RID: 7418
		// (get) Token: 0x06007CB4 RID: 31924 RVA: 0x00231466 File Offset: 0x0022F666
		public static XmlQualifiedName TypeName
		{
			get
			{
				return HighlightComponent._name;
			}
		}

		// Token: 0x17001CFB RID: 7419
		// (get) Token: 0x06007CB5 RID: 31925 RVA: 0x0023146D File Offset: 0x0022F66D
		// (set) Token: 0x06007CB6 RID: 31926 RVA: 0x00231475 File Offset: 0x0022F675
		public Color DefaultBackground
		{
			get
			{
				return this._defaultBackroundColor;
			}
			set
			{
				this._defaultBackroundColor = value;
			}
		}

		// Token: 0x17001CFC RID: 7420
		// (get) Token: 0x06007CB7 RID: 31927 RVA: 0x0023147E File Offset: 0x0022F67E
		// (set) Token: 0x06007CB8 RID: 31928 RVA: 0x00231486 File Offset: 0x0022F686
		public Color DefaultActiveBackground
		{
			get
			{
				return this._defaultActiveBackgroundColor;
			}
			set
			{
				this._defaultActiveBackgroundColor = value;
			}
		}

		// Token: 0x17001CFD RID: 7421
		// (set) Token: 0x06007CB9 RID: 31929 RVA: 0x0023148F File Offset: 0x0022F68F
		public Brush HighlightBrush
		{
			set
			{
				base.SetValue(HighlightComponent.HighlightBrushProperty, value);
			}
		}

		// Token: 0x17001CFE RID: 7422
		// (get) Token: 0x06007CBA RID: 31930 RVA: 0x0023149D File Offset: 0x0022F69D
		public UIElement AnnotatedElement
		{
			get
			{
				if (this._attachedAnnotation == null)
				{
					return null;
				}
				return this._attachedAnnotation.Parent as UIElement;
			}
		}

		// Token: 0x17001CFF RID: 7423
		// (get) Token: 0x06007CBB RID: 31931 RVA: 0x002314B9 File Offset: 0x0022F6B9
		// (set) Token: 0x06007CBC RID: 31932 RVA: 0x002314C1 File Offset: 0x0022F6C1
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
				if (value)
				{
					this.InvalidateChildren();
				}
			}
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x00012630 File Offset: 0x00010830
		public GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			return transform;
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x002314D4 File Offset: 0x0022F6D4
		public void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (this._attachedAnnotation != null)
			{
				throw new ArgumentException(SR.Get("MoreThanOneAttachedAnnotation"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedHighlightBegin);
			ITextContainer textContainer = this.CheckInputData(attachedAnnotation);
			TextAnchor range = attachedAnnotation.AttachedAnchor as TextAnchor;
			this.GetColors(attachedAnnotation.Annotation, out this._background, out this._selectedBackground);
			this._range = range;
			Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			if (annotationHighlightLayer == null)
			{
				annotationHighlightLayer = new AnnotationHighlightLayer();
				textContainer.Highlights.AddLayer(annotationHighlightLayer);
			}
			this._attachedAnnotation = attachedAnnotation;
			this._attachedAnnotation.Annotation.CargoChanged += this.OnAnnotationUpdated;
			annotationHighlightLayer.AddRange(this);
			this.HighlightBrush = new SolidColorBrush(this._background);
			base.IsHitTestVisible = false;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedHighlightEnd);
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x002315CC File Offset: 0x0022F7CC
		public void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation != this._attachedAnnotation)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnnotation"), "attachedAnnotation");
			}
			Invariant.Assert(this._range != null, "null highlight range");
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedHighlightBegin);
			ITextContainer textContainer = this.CheckInputData(attachedAnnotation);
			Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			Invariant.Assert(annotationHighlightLayer != null, "AnnotationHighlightLayer is not initialized");
			this._attachedAnnotation.Annotation.CargoChanged -= this.OnAnnotationUpdated;
			annotationHighlightLayer.RemoveRange(this);
			this._attachedAnnotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedHighlightEnd);
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x00127C49 File Offset: 0x00125E49
		public void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x002316A0 File Offset: 0x0022F8A0
		public void Activate(bool active)
		{
			if (this._active == active)
			{
				return;
			}
			if (this._attachedAnnotation == null)
			{
				throw new InvalidOperationException(SR.Get("NoAttachedAnnotationToModify"));
			}
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			Invariant.Assert(textAnchor != null, "AttachedAnchor is not a text anchor");
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			Invariant.Assert(annotationHighlightLayer != null, "AnnotationHighlightLayer is not initialized");
			annotationHighlightLayer.ActivateRange(this, active);
			this._active = active;
			if (active)
			{
				this.HighlightBrush = new SolidColorBrush(this._selectedBackground);
				return;
			}
			this.HighlightBrush = new SolidColorBrush(this._background);
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x00231766 File Offset: 0x0022F966
		void IHighlightRange.AddChild(Shape child)
		{
			base.Children.Add(child);
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x00231775 File Offset: 0x0022F975
		void IHighlightRange.RemoveChild(Shape child)
		{
			base.Children.Remove(child);
		}

		// Token: 0x17001D00 RID: 7424
		// (get) Token: 0x06007CC4 RID: 31940 RVA: 0x00231783 File Offset: 0x0022F983
		Color IHighlightRange.Background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x17001D01 RID: 7425
		// (get) Token: 0x06007CC5 RID: 31941 RVA: 0x0023178B File Offset: 0x0022F98B
		Color IHighlightRange.SelectedBackground
		{
			get
			{
				return this._selectedBackground;
			}
		}

		// Token: 0x17001D02 RID: 7426
		// (get) Token: 0x06007CC6 RID: 31942 RVA: 0x00231793 File Offset: 0x0022F993
		TextAnchor IHighlightRange.Range
		{
			get
			{
				return this._range;
			}
		}

		// Token: 0x17001D03 RID: 7427
		// (get) Token: 0x06007CC7 RID: 31943 RVA: 0x0023179B File Offset: 0x0022F99B
		int IHighlightRange.Priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x17001D04 RID: 7428
		// (get) Token: 0x06007CC8 RID: 31944 RVA: 0x002317A3 File Offset: 0x0022F9A3
		bool IHighlightRange.HighlightContent
		{
			get
			{
				return this._highlightContent;
			}
		}

		// Token: 0x06007CC9 RID: 31945 RVA: 0x002317AC File Offset: 0x0022F9AC
		internal bool IsSelected(ITextRange selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			Invariant.Assert(this._attachedAnnotation != null, "No _attachedAnnotation");
			TextAnchor textAnchor = this._attachedAnnotation.FullyAttachedAnchor as TextAnchor;
			return textAnchor != null && textAnchor.IsOverlapping(selection.TextSegments);
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x002317FC File Offset: 0x0022F9FC
		internal static void GetCargoColors(Annotation annot, ref Color? backgroundColor, ref Color? activeBackgroundColor)
		{
			Invariant.Assert(annot != null, "annotation is null");
			ICollection<AnnotationResource> cargos = annot.Cargos;
			if (cargos != null)
			{
				foreach (AnnotationResource annotationResource in cargos)
				{
					if (annotationResource.Name == "Highlight")
					{
						ICollection contents = annotationResource.Contents;
						foreach (object obj in contents)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement.LocalName == "Colors" && xmlElement.NamespaceURI == "http://schemas.microsoft.com/windows/annotations/2003/11/base")
							{
								if (xmlElement.Attributes["Background"] != null)
								{
									backgroundColor = new Color?(HighlightComponent.GetColor(xmlElement.Attributes["Background"].Value));
								}
								if (xmlElement.Attributes["ActiveBackground"] != null)
								{
									activeBackgroundColor = new Color?(HighlightComponent.GetColor(xmlElement.Attributes["ActiveBackground"].Value));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x00231978 File Offset: 0x0022FB78
		private ITextContainer CheckInputData(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			TextAnchor textAnchor = attachedAnnotation.AttachedAnchor as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnchor"), "attachedAnnotation");
			}
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			if (attachedAnnotation.Annotation == null)
			{
				throw new ArgumentException(SR.Get("AnnotationIsNull"), "attachedAnnotation");
			}
			if (!this._type.Equals(attachedAnnotation.Annotation.AnnotationType))
			{
				throw new ArgumentException(SR.Get("NotHighlightAnnotationType", new object[]
				{
					attachedAnnotation.Annotation.AnnotationType.ToString()
				}), "attachedAnnotation");
			}
			return textContainer;
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x00231A35 File Offset: 0x0022FC35
		private static Color GetColor(string color)
		{
			return (Color)ColorConverter.ConvertFromString(color);
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x00231A44 File Offset: 0x0022FC44
		private void GetColors(Annotation annot, out Color backgroundColor, out Color activeBackgroundColor)
		{
			Color? color = new Color?(this._defaultBackroundColor);
			Color? color2 = new Color?(this._defaultActiveBackgroundColor);
			HighlightComponent.GetCargoColors(annot, ref color, ref color2);
			backgroundColor = color.Value;
			activeBackgroundColor = color2.Value;
		}

		// Token: 0x06007CCE RID: 31950 RVA: 0x00231A90 File Offset: 0x0022FC90
		private void OnAnnotationUpdated(object sender, AnnotationResourceChangedEventArgs args)
		{
			Invariant.Assert(this._attachedAnnotation != null && this._attachedAnnotation.Annotation == args.Annotation, "_attachedAnnotation is different than the input one");
			Invariant.Assert(this._range != null, "The highlight range is null");
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			Invariant.Assert(textAnchor != null, "wrong anchor type of the saved attached annotation");
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			Color color;
			Color color2;
			this.GetColors(args.Annotation, out color, out color2);
			if (!this._background.Equals(color) || !this._selectedBackground.Equals(color2))
			{
				Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
				AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
				if (annotationHighlightLayer == null)
				{
					throw new InvalidDataException(SR.Get("MissingAnnotationHighlightLayer"));
				}
				this._background = color;
				this._selectedBackground = color2;
				annotationHighlightLayer.ModifiedRange(this);
			}
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x00231B98 File Offset: 0x0022FD98
		private void InvalidateChildren()
		{
			foreach (object obj in base.Children)
			{
				Visual visual = (Visual)obj;
				Shape shape = visual as Shape;
				Invariant.Assert(shape != null, "HighlightComponent has non-Shape children.");
				shape.InvalidateMeasure();
			}
			this.IsDirty = false;
		}

		// Token: 0x04003A6E RID: 14958
		public static DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrushProperty", typeof(Brush), typeof(HighlightComponent));

		// Token: 0x04003A6F RID: 14959
		public const string HighlightResourceName = "Highlight";

		// Token: 0x04003A70 RID: 14960
		public const string ColorsContentName = "Colors";

		// Token: 0x04003A71 RID: 14961
		public const string BackgroundAttributeName = "Background";

		// Token: 0x04003A72 RID: 14962
		public const string ActiveBackgroundAttributeName = "ActiveBackground";

		// Token: 0x04003A73 RID: 14963
		private Color _background;

		// Token: 0x04003A74 RID: 14964
		private Color _selectedBackground;

		// Token: 0x04003A75 RID: 14965
		private TextAnchor _range;

		// Token: 0x04003A76 RID: 14966
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04003A77 RID: 14967
		private PresentationContext _presentationContext;

		// Token: 0x04003A78 RID: 14968
		private static readonly XmlQualifiedName _name = new XmlQualifiedName("Highlight", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04003A79 RID: 14969
		private XmlQualifiedName _type = HighlightComponent._name;

		// Token: 0x04003A7A RID: 14970
		private int _priority;

		// Token: 0x04003A7B RID: 14971
		private bool _highlightContent = true;

		// Token: 0x04003A7C RID: 14972
		private bool _active;

		// Token: 0x04003A7D RID: 14973
		private bool _isDirty = true;

		// Token: 0x04003A7E RID: 14974
		private Color _defaultBackroundColor = (Color)ColorConverter.ConvertFromString("#33FFFF00");

		// Token: 0x04003A7F RID: 14975
		private Color _defaultActiveBackgroundColor = (Color)ColorConverter.ConvertFromString("#339ACD32");
	}
}
