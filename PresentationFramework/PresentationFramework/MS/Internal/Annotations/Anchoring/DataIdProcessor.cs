using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D4 RID: 2004
	internal sealed class DataIdProcessor : SubTreeProcessor
	{
		// Token: 0x06007BF2 RID: 31730 RVA: 0x0022C78D File Offset: 0x0022A98D
		public DataIdProcessor(LocatorManager manager) : base(manager)
		{
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x0022D6F4 File Offset: 0x0022B8F4
		public override IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			object obj = node.ReadLocalValue(DataIdProcessor.DataIdProperty);
			bool flag = (bool)node.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty);
			if (flag && obj != DependencyProperty.UnsetValue)
			{
				calledProcessAnnotations = true;
				return base.Manager.ProcessAnnotations(node);
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x0022D74C File Offset: 0x0022B94C
		public override IList<IAttachedAnnotation> PostProcessNode(DependencyObject node, bool childrenCalledProcessAnnotations, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			object obj = node.ReadLocalValue(DataIdProcessor.DataIdProperty);
			if (!(bool)node.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty) && !childrenCalledProcessAnnotations && obj != DependencyProperty.UnsetValue)
			{
				FrameworkElement frameworkElement = null;
				FrameworkElement frameworkElement2 = node as FrameworkElement;
				if (frameworkElement2 != null)
				{
					frameworkElement = (frameworkElement2.Parent as FrameworkElement);
				}
				AnnotationService service = AnnotationService.GetService(node);
				if (service != null && (service.Root == node || (frameworkElement != null && service.Root == frameworkElement.TemplatedParent)))
				{
					calledProcessAnnotations = true;
					return base.Manager.ProcessAnnotations(node);
				}
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06007BF5 RID: 31733 RVA: 0x0022D7E8 File Offset: 0x0022B9E8
		public override ContentLocator GenerateLocator(PathNode node, out bool continueGenerating)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			continueGenerating = true;
			ContentLocator contentLocator = null;
			ContentLocatorPart contentLocatorPart = this.CreateLocatorPart(node.Node);
			if (contentLocatorPart != null)
			{
				contentLocator = new ContentLocator();
				contentLocator.Parts.Add(contentLocatorPart);
			}
			return contentLocator;
		}

		// Token: 0x06007BF6 RID: 31734 RVA: 0x0022D82C File Offset: 0x0022BA2C
		public override DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (DataIdProcessor.DataIdElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			continueResolving = true;
			string text = locatorPart.NameValuePairs["Value"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			string nodeId = this.GetNodeId(startNode);
			if (nodeId != null)
			{
				if (nodeId.Equals(text))
				{
					return startNode;
				}
				continueResolving = false;
			}
			return null;
		}

		// Token: 0x06007BF7 RID: 31735 RVA: 0x0022D914 File Offset: 0x0022BB14
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])DataIdProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06007BF8 RID: 31736 RVA: 0x0022D925 File Offset: 0x0022BB25
		public static void SetDataId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(DataIdProcessor.DataIdProperty, id);
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x0022D941 File Offset: 0x0022BB41
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static string GetDataId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(DataIdProcessor.DataIdProperty) as string;
		}

		// Token: 0x06007BFA RID: 31738 RVA: 0x0022D961 File Offset: 0x0022BB61
		public static void SetFetchAnnotationsAsBatch(DependencyObject d, bool id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty, id);
		}

		// Token: 0x06007BFB RID: 31739 RVA: 0x0022D97D File Offset: 0x0022BB7D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool GetFetchAnnotationsAsBatch(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return (bool)d.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty);
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x0022D9A0 File Offset: 0x0022BBA0
		private static void OnDataIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			string a = (string)e.OldValue;
			string b = (string)e.NewValue;
			if (!string.Equals(a, b))
			{
				AnnotationService service = AnnotationService.GetService(d);
				if (service != null && service.IsEnabled)
				{
					service.UnloadAnnotations(d);
					service.LoadAnnotations(d);
				}
			}
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x0022D9F0 File Offset: 0x0022BBF0
		private static object CoerceDataId(DependencyObject d, object value)
		{
			string text = (string)value;
			if (text == null || text.Length != 0)
			{
				return value;
			}
			return null;
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x0022DA14 File Offset: 0x0022BC14
		private ContentLocatorPart CreateLocatorPart(DependencyObject node)
		{
			string nodeId = this.GetNodeId(node);
			if (nodeId == null || nodeId.Length == 0)
			{
				return null;
			}
			return new ContentLocatorPart(DataIdProcessor.DataIdElementName)
			{
				NameValuePairs = 
				{
					{
						"Value",
						nodeId
					}
				}
			};
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x0022DA54 File Offset: 0x0022BC54
		internal string GetNodeId(DependencyObject d)
		{
			string text = d.GetValue(DataIdProcessor.DataIdProperty) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = null;
			}
			return text;
		}

		// Token: 0x04003A4A RID: 14922
		public const string Id = "Id";

		// Token: 0x04003A4B RID: 14923
		public static readonly DependencyProperty DataIdProperty = DependencyProperty.RegisterAttached("DataId", typeof(string), typeof(DataIdProcessor), new PropertyMetadata(null, new PropertyChangedCallback(DataIdProcessor.OnDataIdPropertyChanged), new CoerceValueCallback(DataIdProcessor.CoerceDataId)));

		// Token: 0x04003A4C RID: 14924
		public static readonly DependencyProperty FetchAnnotationsAsBatchProperty = DependencyProperty.RegisterAttached("FetchAnnotationsAsBatch", typeof(bool), typeof(DataIdProcessor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04003A4D RID: 14925
		private static readonly XmlQualifiedName DataIdElementName = new XmlQualifiedName("DataId", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04003A4E RID: 14926
		private const string ValueAttributeName = "Value";

		// Token: 0x04003A4F RID: 14927
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			DataIdProcessor.DataIdElementName
		};
	}
}
