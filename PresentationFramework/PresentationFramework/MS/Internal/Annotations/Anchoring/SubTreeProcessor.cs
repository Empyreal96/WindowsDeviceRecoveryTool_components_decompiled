using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D8 RID: 2008
	internal abstract class SubTreeProcessor
	{
		// Token: 0x06007C2F RID: 31791 RVA: 0x0022ECB1 File Offset: 0x0022CEB1
		protected SubTreeProcessor(LocatorManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this._manager = manager;
		}

		// Token: 0x06007C30 RID: 31792
		public abstract IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations);

		// Token: 0x06007C31 RID: 31793 RVA: 0x0022ECCE File Offset: 0x0022CECE
		public virtual IList<IAttachedAnnotation> PostProcessNode(DependencyObject node, bool childrenCalledProcessAnnotations, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06007C32 RID: 31794
		public abstract ContentLocator GenerateLocator(PathNode node, out bool continueGenerating);

		// Token: 0x06007C33 RID: 31795
		public abstract DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving);

		// Token: 0x06007C34 RID: 31796
		public abstract XmlQualifiedName[] GetLocatorPartTypes();

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x06007C35 RID: 31797 RVA: 0x0022ECE2 File Offset: 0x0022CEE2
		protected LocatorManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x04003A59 RID: 14937
		private LocatorManager _manager;
	}
}
