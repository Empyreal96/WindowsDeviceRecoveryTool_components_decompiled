using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D7 RID: 2007
	internal abstract class SelectionProcessor
	{
		// Token: 0x06007C28 RID: 31784
		public abstract bool MergeSelections(object selection1, object selection2, out object newSelection);

		// Token: 0x06007C29 RID: 31785
		public abstract IList<DependencyObject> GetSelectedNodes(object selection);

		// Token: 0x06007C2A RID: 31786
		public abstract UIElement GetParent(object selection);

		// Token: 0x06007C2B RID: 31787
		public abstract Point GetAnchorPoint(object selection);

		// Token: 0x06007C2C RID: 31788
		public abstract IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode);

		// Token: 0x06007C2D RID: 31789
		public abstract object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel);

		// Token: 0x06007C2E RID: 31790
		public abstract XmlQualifiedName[] GetLocatorPartTypes();
	}
}
