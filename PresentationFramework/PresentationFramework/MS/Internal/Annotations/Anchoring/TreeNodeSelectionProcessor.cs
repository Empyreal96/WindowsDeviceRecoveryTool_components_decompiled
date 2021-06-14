using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007DC RID: 2012
	internal sealed class TreeNodeSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06007C61 RID: 31841 RVA: 0x0022FD0A File Offset: 0x0022DF0A
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			if (selection1 == null)
			{
				throw new ArgumentNullException("selection1");
			}
			if (selection2 == null)
			{
				throw new ArgumentNullException("selection2");
			}
			newSelection = null;
			return false;
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x0022FD2C File Offset: 0x0022DF2C
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			return new DependencyObject[]
			{
				this.GetParent(selection)
			};
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x0022FD40 File Offset: 0x0022DF40
		public override UIElement GetParent(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			UIElement uielement = selection as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			return uielement;
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x0022FD7C File Offset: 0x0022DF7C
		public override Point GetAnchorPoint(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			Visual visual = selection as Visual;
			if (visual == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			Rect visualContentBounds = visual.VisualContentBounds;
			return new Point(visualContentBounds.Left, visualContentBounds.Top);
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x0022FDD0 File Offset: 0x0022DFD0
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			return new List<ContentLocatorPart>(0);
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x0022FDF4 File Offset: 0x0022DFF4
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			attachmentLevel = AttachmentLevel.Full;
			return startNode;
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x0022FE16 File Offset: 0x0022E016
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TreeNodeSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x04003A63 RID: 14947
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[0];
	}
}
