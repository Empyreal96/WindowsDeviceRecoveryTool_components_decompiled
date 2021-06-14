using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using MS.Internal.Annotations.Component;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000767 RID: 1895
	internal class SNCAnnotation
	{
		// Token: 0x06007856 RID: 30806 RVA: 0x002245B8 File Offset: 0x002227B8
		static SNCAnnotation()
		{
			foreach (object obj in Enum.GetValues(typeof(XmlToken)))
			{
				XmlToken token = (XmlToken)obj;
				SNCAnnotation.AddXmlTokenNames(token);
			}
		}

		// Token: 0x06007857 RID: 30807 RVA: 0x00224624 File Offset: 0x00222824
		public SNCAnnotation(Annotation annotation)
		{
			this._annotation = annotation;
			this._isNewAnnotation = (this._annotation.Cargos.Count == 0);
			this._cachedXmlElements = new Dictionary<XmlToken, object>();
		}

		// Token: 0x06007858 RID: 30808 RVA: 0x0000326D File Offset: 0x0000146D
		private SNCAnnotation()
		{
		}

		// Token: 0x06007859 RID: 30809 RVA: 0x00224658 File Offset: 0x00222858
		public static void UpdateAnnotation(XmlToken token, StickyNoteControl snc, SNCAnnotation sncAnnotation)
		{
			AnnotationService annotationService = null;
			bool autoFlush = false;
			try
			{
				annotationService = AnnotationService.GetService(((IAnnotationComponent)snc).AnnotatedElement);
				if (annotationService != null && annotationService.Store != null)
				{
					autoFlush = annotationService.Store.AutoFlush;
					annotationService.Store.AutoFlush = false;
				}
				if ((token & XmlToken.Ink) != (XmlToken)0 && snc.Content.Type == StickyNoteType.Ink)
				{
					sncAnnotation.UpdateContent(snc, true, XmlToken.Ink);
				}
				if ((token & XmlToken.Text) != (XmlToken)0 && snc.Content.Type == StickyNoteType.Text)
				{
					sncAnnotation.UpdateContent(snc, true, XmlToken.Text);
				}
				if ((token & (XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.ZOrder)) != (XmlToken)0)
				{
					SNCAnnotation.UpdateMetaData(token, snc, sncAnnotation);
				}
			}
			finally
			{
				if (annotationService != null && annotationService.Store != null)
				{
					annotationService.Store.AutoFlush = autoFlush;
				}
			}
		}

		// Token: 0x0600785A RID: 30810 RVA: 0x0022471C File Offset: 0x0022291C
		public static void UpdateStickyNoteControl(XmlToken token, StickyNoteControl snc, SNCAnnotation sncAnnotation)
		{
			Invariant.Assert((token & (XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder)) > (XmlToken)0, "No token specified.");
			Invariant.Assert(snc != null, "Sticky Note Control is null.");
			Invariant.Assert(sncAnnotation != null, "Annotation is null.");
			if ((token & XmlToken.Ink) != (XmlToken)0 && sncAnnotation.HasInkData)
			{
				sncAnnotation.UpdateContent(snc, false, XmlToken.Ink);
			}
			if ((token & XmlToken.Text) != (XmlToken)0 && sncAnnotation.HasTextData)
			{
				sncAnnotation.UpdateContent(snc, false, XmlToken.Text);
			}
			if ((token & XmlToken.Author) != (XmlToken)0)
			{
				int count = sncAnnotation._annotation.Authors.Count;
				string listSeparator = snc.Language.GetSpecificCulture().TextInfo.ListSeparator;
				string text = string.Empty;
				for (int i = 0; i < count; i++)
				{
					if (i != 0)
					{
						text = text + listSeparator + sncAnnotation._annotation.Authors[i];
					}
					else
					{
						text += sncAnnotation._annotation.Authors[i];
					}
				}
				snc.SetValue(StickyNoteControl.AuthorPropertyKey, text);
			}
			if ((token & XmlToken.Height) != (XmlToken)0)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.Height);
				if (xmlAttribute != null)
				{
					double num = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
					snc.SetValue(FrameworkElement.HeightProperty, num);
				}
				else
				{
					snc.ClearValue(FrameworkElement.HeightProperty);
				}
			}
			if ((token & XmlToken.Width) != (XmlToken)0)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.Width);
				if (xmlAttribute != null)
				{
					double num2 = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
					snc.SetValue(FrameworkElement.WidthProperty, num2);
				}
				else
				{
					snc.ClearValue(FrameworkElement.WidthProperty);
				}
			}
			if ((token & XmlToken.IsExpanded) != (XmlToken)0)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.IsExpanded);
				if (xmlAttribute != null)
				{
					bool isExpanded = Convert.ToBoolean(xmlAttribute.Value, CultureInfo.InvariantCulture);
					snc.IsExpanded = isExpanded;
				}
				else
				{
					snc.ClearValue(StickyNoteControl.IsExpandedProperty);
				}
			}
			if ((token & XmlToken.ZOrder) != (XmlToken)0)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.ZOrder);
				if (xmlAttribute != null)
				{
					((IAnnotationComponent)snc).ZOrder = Convert.ToInt32(xmlAttribute.Value, CultureInfo.InvariantCulture);
				}
			}
			if ((token & (XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset)) != (XmlToken)0)
			{
				TranslateTransform translateTransform = new TranslateTransform();
				if ((token & XmlToken.Left) != (XmlToken)0)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.Left);
					if (xmlAttribute != null)
					{
						double num3 = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
						if (snc.FlipBothOrigins)
						{
							num3 = -(num3 + snc.Width);
						}
						translateTransform.X = num3;
					}
				}
				if ((token & XmlToken.Top) != (XmlToken)0)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.Top);
					if (xmlAttribute != null)
					{
						double y = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
						translateTransform.Y = y;
					}
				}
				if ((token & XmlToken.XOffset) != (XmlToken)0)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.XOffset);
					if (xmlAttribute != null)
					{
						snc.XOffset = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
				}
				if ((token & XmlToken.YOffset) != (XmlToken)0)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)sncAnnotation.FindData(XmlToken.YOffset);
					if (xmlAttribute != null)
					{
						snc.YOffset = Convert.ToDouble(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
				}
				snc.PositionTransform = translateTransform;
			}
		}

		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x0600785B RID: 30811 RVA: 0x00224A1C File Offset: 0x00222C1C
		public bool IsNewAnnotation
		{
			get
			{
				return this._isNewAnnotation;
			}
		}

		// Token: 0x17001C86 RID: 7302
		// (get) Token: 0x0600785C RID: 30812 RVA: 0x00224A24 File Offset: 0x00222C24
		public bool HasInkData
		{
			get
			{
				return this.FindData(XmlToken.Ink) != null;
			}
		}

		// Token: 0x17001C87 RID: 7303
		// (get) Token: 0x0600785D RID: 30813 RVA: 0x00224A34 File Offset: 0x00222C34
		public bool HasTextData
		{
			get
			{
				return this.FindData(XmlToken.Text) != null;
			}
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x00224A44 File Offset: 0x00222C44
		private AnnotationResource FindCargo(string cargoName)
		{
			foreach (AnnotationResource annotationResource in this._annotation.Cargos)
			{
				if (cargoName.Equals(annotationResource.Name))
				{
					return annotationResource;
				}
			}
			return null;
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x00224AA4 File Offset: 0x00222CA4
		private object FindData(XmlToken token)
		{
			object obj = null;
			if (this._cachedXmlElements.ContainsKey(token))
			{
				obj = this._cachedXmlElements[token];
			}
			else
			{
				AnnotationResource annotationResource = this.FindCargo(SNCAnnotation.GetCargoName(token));
				if (annotationResource != null)
				{
					obj = SNCAnnotation.FindContent(token, annotationResource);
					if (obj != null)
					{
						this._cachedXmlElements.Add(token, obj);
					}
				}
			}
			return obj;
		}

		// Token: 0x06007860 RID: 30816 RVA: 0x00224AFC File Offset: 0x00222CFC
		private static void GetCargoAndRoot(SNCAnnotation annotation, XmlToken token, out AnnotationResource cargo, out XmlElement root, out bool newCargo, out bool newRoot)
		{
			Invariant.Assert(annotation != null, "Annotation is null.");
			Invariant.Assert((token & (XmlToken.MetaData | XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder)) > (XmlToken)0, "No token specified.");
			string cargoName = SNCAnnotation.GetCargoName(token);
			newRoot = false;
			newCargo = false;
			cargo = annotation.FindCargo(cargoName);
			if (cargo != null)
			{
				root = SNCAnnotation.FindRootXmlElement(token, cargo);
				if (root == null)
				{
					newRoot = true;
					XmlDocument xmlDocument = new XmlDocument();
					root = xmlDocument.CreateElement(SNCAnnotation.GetXmlName(token), "http://schemas.microsoft.com/windows/annotations/2003/11/base");
					return;
				}
			}
			else
			{
				newCargo = true;
				cargo = new AnnotationResource(cargoName);
				XmlDocument xmlDocument2 = new XmlDocument();
				root = xmlDocument2.CreateElement(SNCAnnotation.GetXmlName(token), "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				cargo.Contents.Add(root);
			}
		}

		// Token: 0x06007861 RID: 30817 RVA: 0x00224BA8 File Offset: 0x00222DA8
		private void UpdateAttribute(XmlElement root, XmlToken token, string value)
		{
			string xmlName = SNCAnnotation.GetXmlName(token);
			XmlNode attributeNode = root.GetAttributeNode(xmlName, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
			if (attributeNode == null)
			{
				if (value == null)
				{
					return;
				}
				root.SetAttribute(xmlName, "http://schemas.microsoft.com/windows/annotations/2003/11/base", value);
				return;
			}
			else
			{
				if (value == null)
				{
					root.RemoveAttribute(xmlName, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
					return;
				}
				if (attributeNode.Value != value)
				{
					root.SetAttribute(xmlName, "http://schemas.microsoft.com/windows/annotations/2003/11/base", value);
				}
				return;
			}
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x00224C0B File Offset: 0x00222E0B
		private static string GetXmlName(XmlToken token)
		{
			return SNCAnnotation.s_xmlTokeFullNames[token];
		}

		// Token: 0x06007863 RID: 30819 RVA: 0x00224C18 File Offset: 0x00222E18
		private static void AddXmlTokenNames(XmlToken token)
		{
			string text = token.ToString();
			if (token <= XmlToken.YOffset)
			{
				if (token <= XmlToken.Left)
				{
					if (token != XmlToken.MetaData)
					{
						if (token != XmlToken.Left)
						{
							goto IL_84;
						}
						goto IL_84;
					}
				}
				else
				{
					if (token != XmlToken.Top && token != XmlToken.XOffset && token != XmlToken.YOffset)
					{
						goto IL_84;
					}
					goto IL_84;
				}
			}
			else if (token <= XmlToken.IsExpanded)
			{
				if (token != XmlToken.Width && token != XmlToken.Height && token != XmlToken.IsExpanded)
				{
					goto IL_84;
				}
				goto IL_84;
			}
			else if (token != XmlToken.Text && token != XmlToken.Ink)
			{
				if (token != XmlToken.ZOrder)
				{
					goto IL_84;
				}
				goto IL_84;
			}
			SNCAnnotation.s_xmlTokeFullNames.Add(token, "anb:" + text);
			return;
			IL_84:
			SNCAnnotation.s_xmlTokeFullNames.Add(token, text);
		}

		// Token: 0x06007864 RID: 30820 RVA: 0x00224CB8 File Offset: 0x00222EB8
		private static string GetCargoName(XmlToken token)
		{
			if (token <= XmlToken.YOffset)
			{
				if (token <= XmlToken.Left)
				{
					if (token != XmlToken.MetaData && token != XmlToken.Left)
					{
						goto IL_75;
					}
				}
				else if (token != XmlToken.Top && token != XmlToken.XOffset && token != XmlToken.YOffset)
				{
					goto IL_75;
				}
			}
			else if (token <= XmlToken.IsExpanded)
			{
				if (token != XmlToken.Width && token != XmlToken.Height && token != XmlToken.IsExpanded)
				{
					goto IL_75;
				}
			}
			else
			{
				if (token == XmlToken.Text)
				{
					return "Text Data";
				}
				if (token == XmlToken.Ink)
				{
					return "Ink Data";
				}
				if (token != XmlToken.ZOrder)
				{
					goto IL_75;
				}
			}
			return "Meta Data";
			IL_75:
			return string.Empty;
		}

		// Token: 0x06007865 RID: 30821 RVA: 0x00224D44 File Offset: 0x00222F44
		private static XmlElement FindRootXmlElement(XmlToken token, AnnotationResource cargo)
		{
			XmlElement result = null;
			string value = string.Empty;
			if (token <= XmlToken.YOffset)
			{
				if (token <= XmlToken.Left)
				{
					if (token != XmlToken.MetaData && token != XmlToken.Left)
					{
						goto IL_77;
					}
				}
				else if (token != XmlToken.Top && token != XmlToken.XOffset && token != XmlToken.YOffset)
				{
					goto IL_77;
				}
			}
			else if (token <= XmlToken.IsExpanded)
			{
				if (token != XmlToken.Width && token != XmlToken.Height && token != XmlToken.IsExpanded)
				{
					goto IL_77;
				}
			}
			else
			{
				if (token == XmlToken.Text || token == XmlToken.Ink)
				{
					value = SNCAnnotation.GetXmlName(token);
					goto IL_77;
				}
				if (token != XmlToken.ZOrder)
				{
					goto IL_77;
				}
			}
			value = SNCAnnotation.GetXmlName(XmlToken.MetaData);
			IL_77:
			foreach (XmlElement xmlElement in cargo.Contents)
			{
				if (xmlElement.Name.Equals(value))
				{
					result = xmlElement;
					break;
				}
			}
			return result;
		}

		// Token: 0x06007866 RID: 30822 RVA: 0x00224E14 File Offset: 0x00223014
		private static object FindContent(XmlToken token, AnnotationResource cargo)
		{
			object result = null;
			XmlElement xmlElement = SNCAnnotation.FindRootXmlElement(token, cargo);
			if (xmlElement != null)
			{
				if (token <= XmlToken.Width)
				{
					if (token <= XmlToken.Top)
					{
						if (token != XmlToken.Left && token != XmlToken.Top)
						{
							return result;
						}
					}
					else if (token != XmlToken.XOffset && token != XmlToken.YOffset && token != XmlToken.Width)
					{
						return result;
					}
				}
				else if (token <= XmlToken.IsExpanded)
				{
					if (token != XmlToken.Height && token != XmlToken.IsExpanded)
					{
						return result;
					}
				}
				else
				{
					if (token == XmlToken.Text || token == XmlToken.Ink)
					{
						return xmlElement;
					}
					if (token != XmlToken.ZOrder)
					{
						return result;
					}
				}
				return xmlElement.GetAttributeNode(SNCAnnotation.GetXmlName(token), "http://schemas.microsoft.com/windows/annotations/2003/11/base");
			}
			return result;
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x00224EA4 File Offset: 0x002230A4
		private void UpdateContent(StickyNoteControl snc, bool updateAnnotation, XmlToken token)
		{
			Invariant.Assert(snc != null, "Sticky Note Control is null.");
			Invariant.Assert((token & (XmlToken.Text | XmlToken.Ink)) > (XmlToken)0, "No token specified.");
			StickyNoteContentControl content = snc.Content;
			if (content == null)
			{
				return;
			}
			if ((token == XmlToken.Ink && content.Type != StickyNoteType.Ink) || (token == XmlToken.Text && content.Type != StickyNoteType.Text))
			{
				return;
			}
			XmlElement xmlElement = null;
			if (updateAnnotation)
			{
				AnnotationResource annotationResource = null;
				bool flag = false;
				bool flag2 = false;
				if (!content.IsEmpty)
				{
					SNCAnnotation.GetCargoAndRoot(this, token, out annotationResource, out xmlElement, out flag2, out flag);
					content.Save(xmlElement);
				}
				else
				{
					string cargoName = SNCAnnotation.GetCargoName(token);
					annotationResource = this.FindCargo(cargoName);
					if (annotationResource != null)
					{
						this._annotation.Cargos.Remove(annotationResource);
						this._cachedXmlElements.Remove(token);
					}
				}
				if (flag)
				{
					Invariant.Assert(xmlElement != null, "XmlElement should have been created.");
					Invariant.Assert(annotationResource != null, "Cargo should have been retrieved.");
					annotationResource.Contents.Add(xmlElement);
				}
				if (flag2)
				{
					Invariant.Assert(annotationResource != null, "Cargo should have been created.");
					this._annotation.Cargos.Add(annotationResource);
					return;
				}
			}
			else
			{
				XmlElement xmlElement2 = (XmlElement)this.FindData(token);
				if (xmlElement2 != null)
				{
					content.Load(xmlElement2);
					return;
				}
				if (!content.IsEmpty)
				{
					content.Clear();
				}
			}
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x00224FDC File Offset: 0x002231DC
		private static void UpdateMetaData(XmlToken token, StickyNoteControl snc, SNCAnnotation sncAnnotation)
		{
			AnnotationResource annotationResource;
			XmlElement xmlElement;
			bool flag;
			bool flag2;
			SNCAnnotation.GetCargoAndRoot(sncAnnotation, XmlToken.MetaData, out annotationResource, out xmlElement, out flag, out flag2);
			if ((token & XmlToken.IsExpanded) != (XmlToken)0)
			{
				bool isExpanded = snc.IsExpanded;
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.IsExpanded, isExpanded.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.Height) != (XmlToken)0)
			{
				double num = (double)snc.GetValue(FrameworkElement.HeightProperty);
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.Height, num.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.Width) != (XmlToken)0)
			{
				double num2 = (double)snc.GetValue(FrameworkElement.WidthProperty);
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.Width, num2.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.Left) != (XmlToken)0)
			{
				double num3 = snc.PositionTransform.X;
				if (snc.FlipBothOrigins)
				{
					num3 = -(num3 + snc.Width);
				}
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.Left, num3.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.Top) != (XmlToken)0)
			{
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.Top, snc.PositionTransform.Y.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.XOffset) != (XmlToken)0)
			{
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.XOffset, snc.XOffset.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.YOffset) != (XmlToken)0)
			{
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.YOffset, snc.YOffset.ToString(CultureInfo.InvariantCulture));
			}
			if ((token & XmlToken.ZOrder) != (XmlToken)0)
			{
				sncAnnotation.UpdateAttribute(xmlElement, XmlToken.ZOrder, ((IAnnotationComponent)snc).ZOrder.ToString(CultureInfo.InvariantCulture));
			}
			if (flag2)
			{
				annotationResource.Contents.Add(xmlElement);
			}
			if (flag)
			{
				sncAnnotation._annotation.Cargos.Add(annotationResource);
			}
		}

		// Token: 0x04003900 RID: 14592
		public const XmlToken AllValues = XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder;

		// Token: 0x04003901 RID: 14593
		public const XmlToken PositionValues = XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset;

		// Token: 0x04003902 RID: 14594
		public const XmlToken Sizes = XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height;

		// Token: 0x04003903 RID: 14595
		public const XmlToken AllContents = XmlToken.Text | XmlToken.Ink;

		// Token: 0x04003904 RID: 14596
		public const XmlToken NegativeAllContents = XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.ZOrder;

		// Token: 0x04003905 RID: 14597
		private static Dictionary<XmlToken, string> s_xmlTokeFullNames = new Dictionary<XmlToken, string>();

		// Token: 0x04003906 RID: 14598
		private Dictionary<XmlToken, object> _cachedXmlElements;

		// Token: 0x04003907 RID: 14599
		private Annotation _annotation;

		// Token: 0x04003908 RID: 14600
		private readonly bool _isNewAnnotation;
	}
}
