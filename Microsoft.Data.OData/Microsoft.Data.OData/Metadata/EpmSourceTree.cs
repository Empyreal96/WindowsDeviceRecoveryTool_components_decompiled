using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000210 RID: 528
	internal sealed class EpmSourceTree
	{
		// Token: 0x06001043 RID: 4163 RVA: 0x0003B475 File Offset: 0x00039675
		internal EpmSourceTree(EpmTargetTree epmTargetTree)
		{
			this.root = new EpmSourcePathSegment();
			this.epmTargetTree = epmTargetTree;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0003B48F File Offset: 0x0003968F
		internal EpmSourcePathSegment Root
		{
			get
			{
				return this.root;
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003B4B4 File Offset: 0x000396B4
		internal void Add(EntityPropertyMappingInfo epmInfo)
		{
			List<EpmSourcePathSegment> list = new List<EpmSourcePathSegment>();
			EpmSourcePathSegment epmSourcePathSegment = this.Root;
			EpmSourcePathSegment epmSourcePathSegment2 = null;
			IEdmType edmType = epmInfo.ActualPropertyType;
			string[] array = epmInfo.Attribute.SourcePath.Split(new char[]
			{
				'/'
			});
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				string propertyName = array[i];
				if (propertyName.Length == 0)
				{
					throw new ODataException(Strings.EpmSourceTree_InvalidSourcePath(epmInfo.DefiningType.ODataFullName(), epmInfo.Attribute.SourcePath));
				}
				IEdmTypeReference propertyType = EpmSourceTree.GetPropertyType(edmType, propertyName);
				IEdmType edmType2 = (propertyType == null) ? null : propertyType.Definition;
				if (edmType2 == null && i < num - 1)
				{
					throw new ODataException(Strings.EpmSourceTree_OpenComplexPropertyCannotBeMapped(propertyName, edmType.ODataFullName()));
				}
				edmType = edmType2;
				epmSourcePathSegment2 = epmSourcePathSegment.SubProperties.SingleOrDefault((EpmSourcePathSegment e) => e.PropertyName == propertyName);
				if (epmSourcePathSegment2 != null)
				{
					epmSourcePathSegment = epmSourcePathSegment2;
				}
				else
				{
					EpmSourcePathSegment epmSourcePathSegment3 = new EpmSourcePathSegment(propertyName);
					epmSourcePathSegment.SubProperties.Add(epmSourcePathSegment3);
					epmSourcePathSegment = epmSourcePathSegment3;
				}
				list.Add(epmSourcePathSegment);
			}
			if (edmType != null && !edmType.IsODataPrimitiveTypeKind())
			{
				throw new ODataException(Strings.EpmSourceTree_EndsWithNonPrimitiveType(epmSourcePathSegment.PropertyName));
			}
			if (epmSourcePathSegment2 != null)
			{
				if (epmSourcePathSegment2.EpmInfo.DefiningTypesAreEqual(epmInfo))
				{
					throw new ODataException(Strings.EpmSourceTree_DuplicateEpmAttributesWithSameSourceName(epmInfo.DefiningType.ODataFullName(), epmInfo.Attribute.SourcePath));
				}
				this.epmTargetTree.Remove(epmSourcePathSegment2.EpmInfo);
			}
			epmInfo.SetPropertyValuePath(list.ToArray());
			epmSourcePathSegment.EpmInfo = epmInfo;
			this.epmTargetTree.Add(epmInfo);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003B661 File Offset: 0x00039861
		internal void Validate(IEdmEntityType entityType)
		{
			EpmSourceTree.Validate(this.Root, entityType);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0003B670 File Offset: 0x00039870
		private static void Validate(EpmSourcePathSegment pathSegment, IEdmType type)
		{
			foreach (EpmSourcePathSegment epmSourcePathSegment in pathSegment.SubProperties)
			{
				IEdmTypeReference propertyType = EpmSourceTree.GetPropertyType(type, epmSourcePathSegment.PropertyName);
				IEdmType type2 = (propertyType == null) ? null : propertyType.Definition;
				EpmSourceTree.Validate(epmSourcePathSegment, type2);
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0003B6E0 File Offset: 0x000398E0
		private static IEdmTypeReference GetPropertyType(IEdmType type, string propertyName)
		{
			IEdmStructuredType edmStructuredType = type as IEdmStructuredType;
			IEdmProperty edmProperty = (edmStructuredType == null) ? null : edmStructuredType.FindProperty(propertyName);
			if (edmProperty != null)
			{
				IEdmTypeReference type2 = edmProperty.Type;
				if (type2.IsNonEntityCollectionType())
				{
					throw new ODataException(Strings.EpmSourceTree_CollectionPropertyCannotBeMapped(propertyName, type.ODataFullName()));
				}
				if (type2.IsStream())
				{
					throw new ODataException(Strings.EpmSourceTree_StreamPropertyCannotBeMapped(propertyName, type.ODataFullName()));
				}
				if (type2.IsSpatial())
				{
					throw new ODataException(Strings.EpmSourceTree_SpatialTypeCannotBeMapped(propertyName, type.ODataFullName()));
				}
				return edmProperty.Type;
			}
			else
			{
				if (type != null && !type.IsOpenType())
				{
					throw new ODataException(Strings.EpmSourceTree_MissingPropertyOnType(propertyName, type.ODataFullName()));
				}
				return null;
			}
		}

		// Token: 0x040005F5 RID: 1525
		private readonly EpmSourcePathSegment root;

		// Token: 0x040005F6 RID: 1526
		private readonly EpmTargetTree epmTargetTree;
	}
}
