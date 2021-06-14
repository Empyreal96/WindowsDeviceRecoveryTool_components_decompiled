using System;
using System.Collections.Generic;
using System.Data.Services.Client.Serializers;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Metadata;

namespace System.Data.Services.Client.Metadata
{
	// Token: 0x02000105 RID: 261
	[DebuggerDisplay("{ElementTypeName}")]
	internal sealed class ClientTypeAnnotation
	{
		// Token: 0x0600086D RID: 2157 RVA: 0x00023460 File Offset: 0x00021660
		internal ClientTypeAnnotation(IEdmType edmType, Type type, string qualifiedName, ClientEdmModel model)
		{
			this.EdmType = edmType;
			this.EdmTypeReference = this.EdmType.ToEdmTypeReference(Util.IsNullableType(type));
			this.ElementTypeName = qualifiedName;
			this.ElementType = (Nullable.GetUnderlyingType(type) ?? type);
			this.model = model;
			this.epmLazyLoader = new ClientTypeAnnotation.EpmLazyLoader(this);
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000234BD File Offset: 0x000216BD
		internal bool IsEntityType
		{
			get
			{
				return this.EdmType.TypeKind == EdmTypeKind.Entity;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x000234CD File Offset: 0x000216CD
		internal ClientPropertyAnnotation MediaDataMember
		{
			get
			{
				if (this.isMediaLinkEntry == null)
				{
					this.CheckMediaLinkEntry();
				}
				return this.mediaDataMember;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x000234E8 File Offset: 0x000216E8
		internal bool IsMediaLinkEntry
		{
			get
			{
				if (this.isMediaLinkEntry == null)
				{
					this.CheckMediaLinkEntry();
				}
				return this.isMediaLinkEntry.Value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00023508 File Offset: 0x00021708
		internal EpmTargetTree EpmTargetTree
		{
			get
			{
				return this.epmLazyLoader.EpmTargetTree;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00023515 File Offset: 0x00021715
		internal bool HasEntityPropertyMappings
		{
			get
			{
				return this.epmLazyLoader.EpmSourceTree.Root.SubProperties.Count > 0;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x00023534 File Offset: 0x00021734
		internal DataServiceProtocolVersion EpmMinimumDataServiceProtocolVersion
		{
			get
			{
				if (!this.HasEntityPropertyMappings)
				{
					return DataServiceProtocolVersion.V1;
				}
				return this.EpmTargetTree.MinimumDataServiceProtocolVersion;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0002354B File Offset: 0x0002174B
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x00023553 File Offset: 0x00021753
		internal IEdmTypeReference EdmTypeReference { get; private set; }

		// Token: 0x06000876 RID: 2166 RVA: 0x0002355C File Offset: 0x0002175C
		internal void EnsureEPMLoaded()
		{
			this.epmLazyLoader.EnsureEPMLoaded();
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00023569 File Offset: 0x00021769
		internal IEnumerable<IEdmProperty> EdmProperties()
		{
			if (this.edmPropertyCache == null)
			{
				this.edmPropertyCache = this.DiscoverEdmProperties().ToArray<IEdmProperty>();
			}
			return this.edmPropertyCache;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002358A File Offset: 0x0002178A
		internal IEnumerable<ClientPropertyAnnotation> Properties()
		{
			if (this.clientPropertyCache == null)
			{
				this.BuildPropertyCache();
			}
			return this.clientPropertyCache.Values;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000235B6 File Offset: 0x000217B6
		internal IEnumerable<ClientPropertyAnnotation> PropertiesToSerialize()
		{
			return from p in this.Properties()
			where ClientTypeAnnotation.ShouldSerializeProperty(this, p)
			orderby p.PropertyName
			select p;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000235F4 File Offset: 0x000217F4
		internal ClientPropertyAnnotation GetProperty(string propertyName, bool ignoreMissingProperties)
		{
			if (this.clientPropertyCache == null)
			{
				this.BuildPropertyCache();
			}
			ClientPropertyAnnotation result;
			if (!this.clientPropertyCache.TryGetValue(propertyName, out result) && !ignoreMissingProperties)
			{
				throw Error.InvalidOperation(Strings.ClientType_MissingProperty(this.ElementTypeName, propertyName));
			}
			return result;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00023638 File Offset: 0x00021838
		internal Version GetMetadataVersion()
		{
			if (this.metadataVersion == null)
			{
				Version dataServiceVersion = Util.DataServiceVersion1;
				WebUtil.RaiseVersion(ref dataServiceVersion, this.ComputeVersionForPropertyCollection(this.EdmProperties(), null));
				this.metadataVersion = dataServiceVersion;
			}
			return this.metadataVersion;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002367A File Offset: 0x0002187A
		private static bool ShouldSerializeProperty(ClientTypeAnnotation type, ClientPropertyAnnotation property)
		{
			return !property.IsDictionary && property != type.MediaDataMember && !property.IsStreamLinkProperty && (type.MediaDataMember == null || type.MediaDataMember.MimeTypeProperty != property);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000236B4 File Offset: 0x000218B4
		private void BuildPropertyCache()
		{
			lock (this)
			{
				if (this.clientPropertyCache == null)
				{
					Dictionary<string, ClientPropertyAnnotation> dictionary = new Dictionary<string, ClientPropertyAnnotation>(StringComparer.Ordinal);
					foreach (IEdmProperty edmProperty in this.EdmProperties())
					{
						dictionary.Add(edmProperty.Name, this.model.GetClientPropertyAnnotation(edmProperty));
					}
					this.clientPropertyCache = dictionary;
				}
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00023778 File Offset: 0x00021978
		private void CheckMediaLinkEntry()
		{
			this.isMediaLinkEntry = new bool?(false);
			MediaEntryAttribute mediaEntryAttribute = (MediaEntryAttribute)this.ElementType.GetCustomAttributes(typeof(MediaEntryAttribute), true).SingleOrDefault<object>();
			if (mediaEntryAttribute != null)
			{
				this.isMediaLinkEntry = new bool?(true);
				ClientPropertyAnnotation clientPropertyAnnotation = this.Properties().SingleOrDefault((ClientPropertyAnnotation p) => p.PropertyName == mediaEntryAttribute.MediaMemberName);
				if (clientPropertyAnnotation == null)
				{
					throw Error.InvalidOperation(Strings.ClientType_MissingMediaEntryProperty(this.ElementTypeName, mediaEntryAttribute.MediaMemberName));
				}
				this.mediaDataMember = clientPropertyAnnotation;
			}
			bool flag = this.ElementType.GetCustomAttributes(typeof(HasStreamAttribute), true).Any<object>();
			if (flag)
			{
				this.isMediaLinkEntry = new bool?(true);
			}
			if (this.isMediaLinkEntry != null && this.isMediaLinkEntry.Value)
			{
				this.SetMediaLinkEntryAnnotation();
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00023861 File Offset: 0x00021A61
		private void SetMediaLinkEntryAnnotation()
		{
			this.model.SetHasDefaultStream((IEdmEntityType)this.EdmType, true);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002387C File Offset: 0x00021A7C
		private Version ComputeVersionForPropertyCollection(IEnumerable<IEdmProperty> propertyCollection, HashSet<IEdmType> visitedComplexTypes)
		{
			Version dataServiceVersion = Util.DataServiceVersion1;
			foreach (IEdmProperty edmProperty in propertyCollection)
			{
				ClientPropertyAnnotation clientPropertyAnnotation = this.model.GetClientPropertyAnnotation(edmProperty);
				if (clientPropertyAnnotation.IsPrimitiveOrComplexCollection || clientPropertyAnnotation.IsSpatialType)
				{
					WebUtil.RaiseVersion(ref dataServiceVersion, Util.DataServiceVersion3);
				}
				else if (edmProperty.Type.TypeKind() == EdmTypeKind.Complex && !clientPropertyAnnotation.IsDictionary)
				{
					if (visitedComplexTypes == null)
					{
						visitedComplexTypes = new HashSet<IEdmType>(EqualityComparer<IEdmType>.Default);
					}
					else if (visitedComplexTypes.Contains(edmProperty.Type.Definition))
					{
						continue;
					}
					visitedComplexTypes.Add(edmProperty.Type.Definition);
					WebUtil.RaiseVersion(ref dataServiceVersion, this.ComputeVersionForPropertyCollection(this.model.GetClientTypeAnnotation(edmProperty).EdmProperties(), visitedComplexTypes));
				}
			}
			return dataServiceVersion;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00023B84 File Offset: 0x00021D84
		private IEnumerable<IEdmProperty> DiscoverEdmProperties()
		{
			IEdmStructuredType edmStructuredType = this.EdmType as IEdmStructuredType;
			if (edmStructuredType != null)
			{
				HashSet<string> propertyNames = new HashSet<string>(StringComparer.Ordinal);
				do
				{
					foreach (IEdmProperty property in edmStructuredType.DeclaredProperties)
					{
						string propertyName = property.Name;
						if (!propertyNames.Contains(propertyName))
						{
							propertyNames.Add(propertyName);
							yield return property;
						}
					}
					edmStructuredType = edmStructuredType.BaseType;
				}
				while (edmStructuredType != null);
			}
			yield break;
		}

		// Token: 0x040004F9 RID: 1273
		internal readonly IEdmType EdmType;

		// Token: 0x040004FA RID: 1274
		internal readonly string ElementTypeName;

		// Token: 0x040004FB RID: 1275
		internal readonly Type ElementType;

		// Token: 0x040004FC RID: 1276
		private readonly ClientEdmModel model;

		// Token: 0x040004FD RID: 1277
		private bool? isMediaLinkEntry;

		// Token: 0x040004FE RID: 1278
		private ClientPropertyAnnotation mediaDataMember;

		// Token: 0x040004FF RID: 1279
		private Version metadataVersion;

		// Token: 0x04000500 RID: 1280
		private ClientTypeAnnotation.EpmLazyLoader epmLazyLoader;

		// Token: 0x04000501 RID: 1281
		private Dictionary<string, ClientPropertyAnnotation> clientPropertyCache;

		// Token: 0x04000502 RID: 1282
		private IEdmProperty[] edmPropertyCache;

		// Token: 0x02000106 RID: 262
		private class EpmLazyLoader
		{
			// Token: 0x06000884 RID: 2180 RVA: 0x00023BA1 File Offset: 0x00021DA1
			internal EpmLazyLoader(ClientTypeAnnotation clientTypeAnnotation)
			{
				this.clientTypeAnnotation = clientTypeAnnotation;
			}

			// Token: 0x170001F5 RID: 501
			// (get) Token: 0x06000885 RID: 2181 RVA: 0x00023BBB File Offset: 0x00021DBB
			internal EpmTargetTree EpmTargetTree
			{
				get
				{
					if (this.EpmNeedsInitializing)
					{
						this.InitializeAndBuildTree();
					}
					return this.epmTargetTree;
				}
			}

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x06000886 RID: 2182 RVA: 0x00023BD1 File Offset: 0x00021DD1
			internal EpmSourceTree EpmSourceTree
			{
				get
				{
					if (this.EpmNeedsInitializing)
					{
						this.InitializeAndBuildTree();
					}
					return this.epmSourceTree;
				}
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x06000887 RID: 2183 RVA: 0x00023BE7 File Offset: 0x00021DE7
			private bool EpmNeedsInitializing
			{
				get
				{
					return this.epmSourceTree == null || this.epmTargetTree == null;
				}
			}

			// Token: 0x06000888 RID: 2184 RVA: 0x00023BFC File Offset: 0x00021DFC
			internal void EnsureEPMLoaded()
			{
				if (this.EpmNeedsInitializing)
				{
					this.InitializeAndBuildTree();
				}
			}

			// Token: 0x06000889 RID: 2185 RVA: 0x00023C0C File Offset: 0x00021E0C
			private static void BuildEpmInfo(ClientTypeAnnotation clientTypeAnnotation, EpmSourceTree sourceTree)
			{
				ClientTypeAnnotation.EpmLazyLoader.BuildEpmInfo(clientTypeAnnotation.ElementType, clientTypeAnnotation, sourceTree);
			}

			// Token: 0x0600088A RID: 2186 RVA: 0x00023C80 File Offset: 0x00021E80
			private static void BuildEpmInfo(Type type, ClientTypeAnnotation clientTypeAnnotation, EpmSourceTree sourceTree)
			{
				if (clientTypeAnnotation.IsEntityType)
				{
					Type baseType = type.GetBaseType();
					ClientEdmModel model = clientTypeAnnotation.model;
					ODataEntityPropertyMappingCollection mappings = null;
					if (baseType != null && baseType != typeof(object))
					{
						if (((EdmStructuredType)clientTypeAnnotation.EdmType).BaseType == null)
						{
							ClientTypeAnnotation.EpmLazyLoader.BuildEpmInfo(baseType, clientTypeAnnotation, sourceTree);
							mappings = model.GetAnnotationValue(clientTypeAnnotation.EdmType);
						}
						else
						{
							ClientTypeAnnotation clientTypeAnnotation2 = model.GetClientTypeAnnotation(baseType);
							ClientTypeAnnotation.EpmLazyLoader.BuildEpmInfo(baseType, clientTypeAnnotation2, sourceTree);
						}
					}
					foreach (EntityPropertyMappingAttribute entityPropertyMappingAttribute in type.GetCustomAttributes(typeof(EntityPropertyMappingAttribute), false))
					{
						ClientTypeAnnotation.EpmLazyLoader.BuildEpmInfo(entityPropertyMappingAttribute, type, clientTypeAnnotation, sourceTree);
						if (mappings == null)
						{
							mappings = new ODataEntityPropertyMappingCollection();
						}
						mappings.Add(entityPropertyMappingAttribute);
					}
					if (mappings != null)
					{
						ODataEntityPropertyMappingCollection annotationValue = model.GetAnnotationValue(clientTypeAnnotation.EdmType);
						if (annotationValue != null)
						{
							List<EntityPropertyMappingAttribute> list = (from oldM in annotationValue
							where !mappings.Any((EntityPropertyMappingAttribute newM) => oldM.SourcePath == newM.SourcePath)
							select oldM).ToList<EntityPropertyMappingAttribute>();
							foreach (EntityPropertyMappingAttribute mapping in list)
							{
								mappings.Add(mapping);
							}
						}
						model.SetAnnotationValue(clientTypeAnnotation.EdmType, mappings);
					}
				}
			}

			// Token: 0x0600088B RID: 2187 RVA: 0x00023E10 File Offset: 0x00022010
			private static void BuildEpmInfo(EntityPropertyMappingAttribute epmAttr, Type definingType, ClientTypeAnnotation clientTypeAnnotation, EpmSourceTree sourceTree)
			{
				sourceTree.Add(new EntityPropertyMappingInfo(epmAttr, definingType, clientTypeAnnotation));
			}

			// Token: 0x0600088C RID: 2188 RVA: 0x00023E20 File Offset: 0x00022020
			private void InitializeAndBuildTree()
			{
				lock (this.epmDataLock)
				{
					if (this.EpmNeedsInitializing)
					{
						EpmTargetTree epmTargetTree = new EpmTargetTree();
						EpmSourceTree epmSourceTree = new EpmSourceTree(epmTargetTree);
						ClientTypeAnnotation.EpmLazyLoader.BuildEpmInfo(this.clientTypeAnnotation, epmSourceTree);
						epmSourceTree.Validate(this.clientTypeAnnotation);
						this.epmTargetTree = epmTargetTree;
						this.epmSourceTree = epmSourceTree;
					}
				}
			}

			// Token: 0x04000505 RID: 1285
			private EpmSourceTree epmSourceTree;

			// Token: 0x04000506 RID: 1286
			private EpmTargetTree epmTargetTree;

			// Token: 0x04000507 RID: 1287
			private object epmDataLock = new object();

			// Token: 0x04000508 RID: 1288
			private ClientTypeAnnotation clientTypeAnnotation;
		}
	}
}
