using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;

namespace System.Data.Services.Client.Serializers
{
	// Token: 0x02000019 RID: 25
	internal sealed class EpmTargetTree
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00003C6B File Offset: 0x00001E6B
		internal EpmTargetTree()
		{
			this.SyndicationRoot = new EpmTargetPathSegment();
			this.NonSyndicationRoot = new EpmTargetPathSegment();
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003C89 File Offset: 0x00001E89
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003C91 File Offset: 0x00001E91
		internal EpmTargetPathSegment SyndicationRoot { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003C9A File Offset: 0x00001E9A
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003CA2 File Offset: 0x00001EA2
		internal EpmTargetPathSegment NonSyndicationRoot { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003CAB File Offset: 0x00001EAB
		internal DataServiceProtocolVersion MinimumDataServiceProtocolVersion
		{
			get
			{
				if (this.countOfNonContentV2mappings > 0)
				{
					return DataServiceProtocolVersion.V2;
				}
				return DataServiceProtocolVersion.V1;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003D18 File Offset: 0x00001F18
		internal void Add(EntityPropertyMappingInfo epmInfo)
		{
			string targetPath = epmInfo.Attribute.TargetPath;
			string namespaceUri = epmInfo.Attribute.TargetNamespaceUri;
			EpmTargetPathSegment epmTargetPathSegment = epmInfo.IsSyndicationMapping ? this.SyndicationRoot : this.NonSyndicationRoot;
			IList<EpmTargetPathSegment> subSegments = epmTargetPathSegment.SubSegments;
			string[] array = targetPath.Split(new char[]
			{
				'/'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string targetSegment = array[i];
				if (targetSegment.Length == 0)
				{
					throw new InvalidOperationException(Strings.EpmTargetTree_InvalidTargetPath(targetPath));
				}
				if (targetSegment[0] == '@' && i != array.Length - 1)
				{
					throw new InvalidOperationException(Strings.EpmTargetTree_AttributeInMiddle(targetSegment));
				}
				EpmTargetPathSegment epmTargetPathSegment2 = subSegments.SingleOrDefault((EpmTargetPathSegment segment) => segment.SegmentName == targetSegment && (epmInfo.IsSyndicationMapping || segment.SegmentNamespaceUri == namespaceUri));
				if (epmTargetPathSegment2 != null)
				{
					epmTargetPathSegment = epmTargetPathSegment2;
				}
				else
				{
					epmTargetPathSegment = new EpmTargetPathSegment(targetSegment, namespaceUri, epmTargetPathSegment);
					if (targetSegment[0] == '@')
					{
						subSegments.Insert(0, epmTargetPathSegment);
					}
					else
					{
						subSegments.Add(epmTargetPathSegment);
					}
				}
				subSegments = epmTargetPathSegment.SubSegments;
			}
			if (epmTargetPathSegment.EpmInfo != null)
			{
				throw new ArgumentException(Strings.EpmTargetTree_DuplicateEpmAttrsWithSameTargetName(EpmTargetTree.GetPropertyNameFromEpmInfo(epmTargetPathSegment.EpmInfo), epmTargetPathSegment.EpmInfo.DefiningType.Name, epmTargetPathSegment.EpmInfo.Attribute.SourcePath, epmInfo.Attribute.SourcePath));
			}
			if (!epmInfo.Attribute.KeepInContent)
			{
				this.countOfNonContentV2mappings++;
			}
			epmTargetPathSegment.EpmInfo = epmInfo;
			if (EpmTargetTree.HasMixedContent(this.NonSyndicationRoot, false))
			{
				throw new InvalidOperationException(Strings.EpmTargetTree_InvalidTargetPath(targetPath));
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003F64 File Offset: 0x00002164
		internal void Remove(EntityPropertyMappingInfo epmInfo)
		{
			string targetPath = epmInfo.Attribute.TargetPath;
			string namespaceUri = epmInfo.Attribute.TargetNamespaceUri;
			EpmTargetPathSegment epmTargetPathSegment = epmInfo.IsSyndicationMapping ? this.SyndicationRoot : this.NonSyndicationRoot;
			List<EpmTargetPathSegment> subSegments = epmTargetPathSegment.SubSegments;
			string[] array = targetPath.Split(new char[]
			{
				'/'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string targetSegment = array[i];
				EpmTargetPathSegment epmTargetPathSegment2 = subSegments.FirstOrDefault((EpmTargetPathSegment segment) => segment.SegmentName == targetSegment && (epmInfo.IsSyndicationMapping || segment.SegmentNamespaceUri == namespaceUri));
				if (epmTargetPathSegment2 == null)
				{
					return;
				}
				epmTargetPathSegment = epmTargetPathSegment2;
				subSegments = epmTargetPathSegment.SubSegments;
			}
			if (epmTargetPathSegment.EpmInfo != null)
			{
				if (!epmTargetPathSegment.EpmInfo.Attribute.KeepInContent)
				{
					this.countOfNonContentV2mappings--;
				}
				do
				{
					EpmTargetPathSegment parentSegment = epmTargetPathSegment.ParentSegment;
					parentSegment.SubSegments.Remove(epmTargetPathSegment);
					epmTargetPathSegment = parentSegment;
				}
				while (epmTargetPathSegment.ParentSegment != null && !epmTargetPathSegment.HasContent && epmTargetPathSegment.SubSegments.Count == 0);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004099 File Offset: 0x00002299
		[Conditional("DEBUG")]
		internal void Validate()
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000040A8 File Offset: 0x000022A8
		private static bool HasMixedContent(EpmTargetPathSegment currentSegment, bool ancestorHasContent)
		{
			foreach (EpmTargetPathSegment epmTargetPathSegment in from s in currentSegment.SubSegments
			where !s.IsAttribute
			select s)
			{
				if (epmTargetPathSegment.HasContent && ancestorHasContent)
				{
					return true;
				}
				if (EpmTargetTree.HasMixedContent(epmTargetPathSegment, epmTargetPathSegment.HasContent || ancestorHasContent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000413C File Offset: 0x0000233C
		private static string GetPropertyNameFromEpmInfo(EntityPropertyMappingInfo epmInfo)
		{
			if (epmInfo.Attribute.TargetSyndicationItem == SyndicationItemProperty.CustomProperty)
			{
				return epmInfo.Attribute.TargetPath;
			}
			return epmInfo.Attribute.TargetSyndicationItem.ToString();
		}

		// Token: 0x04000170 RID: 368
		private int countOfNonContentV2mappings;
	}
}
