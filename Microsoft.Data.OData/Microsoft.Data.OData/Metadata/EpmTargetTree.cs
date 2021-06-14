using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000212 RID: 530
	internal sealed class EpmTargetTree
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x0003B841 File Offset: 0x00039A41
		internal EpmTargetTree()
		{
			this.syndicationRoot = new EpmTargetPathSegment();
			this.nonSyndicationRoot = new EpmTargetPathSegment();
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0003B85F File Offset: 0x00039A5F
		internal EpmTargetPathSegment SyndicationRoot
		{
			get
			{
				return this.syndicationRoot;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x0003B867 File Offset: 0x00039A67
		internal EpmTargetPathSegment NonSyndicationRoot
		{
			get
			{
				return this.nonSyndicationRoot;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0003B86F File Offset: 0x00039A6F
		internal ODataVersion MinimumODataProtocolVersion
		{
			get
			{
				if (this.countOfNonContentV2Mappings > 0)
				{
					return ODataVersion.V2;
				}
				return ODataVersion.V1;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0003B8DC File Offset: 0x00039ADC
		internal void Add(EntityPropertyMappingInfo epmInfo)
		{
			string targetPath = epmInfo.Attribute.TargetPath;
			string namespaceUri = epmInfo.Attribute.TargetNamespaceUri;
			string targetNamespacePrefix = epmInfo.Attribute.TargetNamespacePrefix;
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
					throw new ODataException(Strings.EpmTargetTree_InvalidTargetPath_EmptySegment(targetPath));
				}
				if (targetSegment[0] == '@' && i != array.Length - 1)
				{
					throw new ODataException(Strings.EpmTargetTree_AttributeInMiddle(targetSegment));
				}
				EpmTargetPathSegment epmTargetPathSegment2 = subSegments.SingleOrDefault((EpmTargetPathSegment segment) => segment.SegmentName == targetSegment && (epmInfo.IsSyndicationMapping || segment.SegmentNamespaceUri == namespaceUri));
				if (epmTargetPathSegment2 != null)
				{
					epmTargetPathSegment = epmTargetPathSegment2;
				}
				else
				{
					epmTargetPathSegment = new EpmTargetPathSegment(targetSegment, namespaceUri, targetNamespacePrefix, epmTargetPathSegment);
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
				throw new ODataException(Strings.EpmTargetTree_DuplicateEpmAttributesWithSameTargetName(epmTargetPathSegment.EpmInfo.DefiningType.ODataFullName(), EpmTargetTree.GetPropertyNameFromEpmInfo(epmTargetPathSegment.EpmInfo), epmTargetPathSegment.EpmInfo.Attribute.SourcePath, epmInfo.Attribute.SourcePath));
			}
			if (!epmInfo.Attribute.KeepInContent)
			{
				this.countOfNonContentV2Mappings++;
			}
			epmTargetPathSegment.EpmInfo = epmInfo;
			List<EntityPropertyMappingAttribute> list = new List<EntityPropertyMappingAttribute>(2);
			if (EpmTargetTree.HasMixedContent(this.NonSyndicationRoot, list))
			{
				throw new ODataException(Strings.EpmTargetTree_InvalidTargetPath_MixedContent(list[0].TargetPath, list[1].TargetPath));
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0003BB64 File Offset: 0x00039D64
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
					this.countOfNonContentV2Mappings--;
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

		// Token: 0x0600105B RID: 4187 RVA: 0x0003BC99 File Offset: 0x00039E99
		[Conditional("DEBUG")]
		internal void Validate()
		{
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0003BCA8 File Offset: 0x00039EA8
		private static bool HasMixedContent(EpmTargetPathSegment currentSegment, List<EntityPropertyMappingAttribute> ancestorsWithContent)
		{
			foreach (EpmTargetPathSegment epmTargetPathSegment in from s in currentSegment.SubSegments
			where !s.IsAttribute
			select s)
			{
				if (epmTargetPathSegment.HasContent && ancestorsWithContent.Count == 1)
				{
					ancestorsWithContent.Add(epmTargetPathSegment.EpmInfo.Attribute);
					return true;
				}
				if (epmTargetPathSegment.HasContent)
				{
					ancestorsWithContent.Add(epmTargetPathSegment.EpmInfo.Attribute);
				}
				if (EpmTargetTree.HasMixedContent(epmTargetPathSegment, ancestorsWithContent))
				{
					return true;
				}
				if (epmTargetPathSegment.HasContent)
				{
					ancestorsWithContent.Clear();
				}
			}
			return false;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0003BD70 File Offset: 0x00039F70
		private static string GetPropertyNameFromEpmInfo(EntityPropertyMappingInfo epmInfo)
		{
			if (epmInfo.Attribute.TargetSyndicationItem == SyndicationItemProperty.CustomProperty)
			{
				return epmInfo.Attribute.TargetPath;
			}
			return epmInfo.Attribute.TargetSyndicationItem.ToString();
		}

		// Token: 0x040005FE RID: 1534
		private readonly EpmTargetPathSegment syndicationRoot;

		// Token: 0x040005FF RID: 1535
		private readonly EpmTargetPathSegment nonSyndicationRoot;

		// Token: 0x04000600 RID: 1536
		private int countOfNonContentV2Mappings;
	}
}
