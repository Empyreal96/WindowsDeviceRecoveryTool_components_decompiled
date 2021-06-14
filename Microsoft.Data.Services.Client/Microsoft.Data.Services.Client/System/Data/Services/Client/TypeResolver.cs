using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace System.Data.Services.Client
{
	// Token: 0x02000124 RID: 292
	internal class TypeResolver
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x00027975 File Offset: 0x00025B75
		internal TypeResolver(ClientEdmModel model, Func<string, Type> resolveTypeFromName, Func<Type, string> resolveNameFromType, IEdmModel serviceModel)
		{
			this.resolveTypeFromName = resolveTypeFromName;
			this.resolveNameFromType = resolveNameFromType;
			this.clientEdmModel = model;
			this.serviceModel = serviceModel;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x000279AA File Offset: 0x00025BAA
		internal IEdmModel ReaderModel
		{
			get
			{
				if (this.serviceModel != null)
				{
					return this.serviceModel;
				}
				return this.clientEdmModel;
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000279C1 File Offset: 0x00025BC1
		internal void IsProjectionRequest()
		{
			this.skipTypeAssignabilityCheck = true;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x000279CC File Offset: 0x00025BCC
		internal ClientTypeAnnotation ResolveTypeForMaterialization(Type expectedType, string readerTypeName)
		{
			string collectionItemWireTypeName = WebUtil.GetCollectionItemWireTypeName(readerTypeName);
			if (collectionItemWireTypeName != null)
			{
				Type implementationType = ClientTypeUtil.GetImplementationType(expectedType, typeof(ICollection<>));
				Type type = implementationType.GetGenericArguments()[0];
				if (!PrimitiveType.IsKnownType(type))
				{
					type = this.ResolveTypeForMaterialization(type, collectionItemWireTypeName).ElementType;
				}
				Type backingTypeForCollectionProperty = WebUtil.GetBackingTypeForCollectionProperty(expectedType, type);
				return this.clientEdmModel.GetClientTypeAnnotation(backingTypeForCollectionProperty);
			}
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(readerTypeName, out primitiveType))
			{
				return this.clientEdmModel.GetClientTypeAnnotation(primitiveType.ClrType);
			}
			ClientTypeAnnotation result;
			if (this.edmTypeNameMap.TryGetValue(readerTypeName, out result))
			{
				return result;
			}
			if (this.serviceModel != null)
			{
				Type type2 = this.ResolveTypeFromName(readerTypeName, expectedType);
				return this.clientEdmModel.GetClientTypeAnnotation(type2);
			}
			return this.clientEdmModel.GetClientTypeAnnotation(readerTypeName);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00027A8C File Offset: 0x00025C8C
		internal IEdmType ResolveWireTypeName(IEdmType expectedEdmType, string wireName)
		{
			if (expectedEdmType != null && expectedEdmType.TypeKind == EdmTypeKind.Primitive)
			{
				return expectedEdmType;
			}
			Type expectedType;
			if (expectedEdmType != null)
			{
				ClientTypeAnnotation clientTypeAnnotation = this.clientEdmModel.GetClientTypeAnnotation(expectedEdmType);
				expectedType = clientTypeAnnotation.ElementType;
			}
			else
			{
				expectedType = typeof(object);
			}
			Type type = this.ResolveTypeFromName(wireName, expectedType);
			ClientTypeAnnotation clientTypeAnnotation2 = this.clientEdmModel.GetClientTypeAnnotation(this.clientEdmModel.GetOrCreateEdmType(type));
			if (clientTypeAnnotation2.IsEntityType)
			{
				clientTypeAnnotation2.EnsureEPMLoaded();
			}
			IEdmType edmType = clientTypeAnnotation2.EdmType;
			EdmTypeKind typeKind = edmType.TypeKind;
			if (typeKind == EdmTypeKind.Entity || typeKind == EdmTypeKind.Complex)
			{
				string key = edmType.FullName();
				if (!this.edmTypeNameMap.ContainsKey(key))
				{
					this.edmTypeNameMap.Add(key, clientTypeAnnotation2);
				}
			}
			return edmType;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00027B40 File Offset: 0x00025D40
		internal IEdmType ResolveExpectedTypeForReading(Type clientClrType)
		{
			ClientTypeAnnotation clientTypeAnnotation = this.clientEdmModel.GetClientTypeAnnotation(clientClrType);
			clientTypeAnnotation.EnsureEPMLoaded();
			IEdmType edmType = clientTypeAnnotation.EdmType;
			if (this.serviceModel == null)
			{
				return edmType;
			}
			if (edmType.TypeKind == EdmTypeKind.Primitive)
			{
				return edmType;
			}
			if (edmType.TypeKind == EdmTypeKind.Collection)
			{
				IEdmTypeReference elementType = ((IEdmCollectionType)edmType).ElementType;
				if (elementType.IsPrimitive())
				{
					return edmType;
				}
				Type clientClrType2 = clientClrType.GetGenericArguments()[0];
				IEdmType edmType2 = this.ResolveExpectedTypeForReading(clientClrType2);
				if (edmType2 == null)
				{
					return null;
				}
				return new EdmCollectionType(edmType2.ToEdmTypeReference(elementType.IsNullable));
			}
			else
			{
				IEdmStructuredType result;
				if (!this.TryToResolveServerType(clientTypeAnnotation, out result))
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00027BD8 File Offset: 0x00025DD8
		internal bool ShouldWriteClientTypeForOpenServerProperty(IEdmProperty clientProperty, string serverTypeName)
		{
			if (serverTypeName == null)
			{
				return false;
			}
			if (this.serviceModel == null)
			{
				return false;
			}
			if (clientProperty.DeclaringType.TypeKind != EdmTypeKind.Entity)
			{
				return false;
			}
			IEdmStructuredType edmStructuredType = this.serviceModel.FindType(serverTypeName) as IEdmStructuredType;
			return edmStructuredType != null && edmStructuredType.FindProperty(clientProperty.Name) == null;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00027C2C File Offset: 0x00025E2C
		internal bool TryResolveEntitySetBaseTypeName(string entitySetName, out string serverTypeName)
		{
			serverTypeName = null;
			if (this.serviceModel == null)
			{
				return false;
			}
			IEdmEntitySet edmEntitySet = this.serviceModel.ResolveEntitySet(entitySetName);
			if (edmEntitySet != null)
			{
				serverTypeName = edmEntitySet.ElementType.FullName();
				return true;
			}
			return false;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00027C68 File Offset: 0x00025E68
		internal bool TryResolveNavigationTargetTypeName(string serverSourceTypeName, string navigationPropertyName, out string serverTypeName)
		{
			serverTypeName = null;
			if (this.serviceModel == null || serverSourceTypeName == null)
			{
				return false;
			}
			IEdmEntityType edmEntityType = this.serviceModel.FindType(serverSourceTypeName) as IEdmEntityType;
			if (edmEntityType == null)
			{
				return false;
			}
			IEdmNavigationProperty edmNavigationProperty = edmEntityType.FindProperty(navigationPropertyName) as IEdmNavigationProperty;
			if (edmNavigationProperty == null)
			{
				return false;
			}
			IEdmTypeReference type = edmNavigationProperty.Type;
			if (type.IsCollection())
			{
				type = type.AsCollection().ElementType();
			}
			serverTypeName = type.FullName();
			return true;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00027CD4 File Offset: 0x00025ED4
		private bool TryToResolveServerType(ClientTypeAnnotation clientTypeAnnotation, out IEdmStructuredType serverType)
		{
			string text = this.resolveNameFromType(clientTypeAnnotation.ElementType);
			if (text == null)
			{
				serverType = null;
				return false;
			}
			serverType = (this.serviceModel.FindType(text) as IEdmStructuredType);
			return serverType != null;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00027D18 File Offset: 0x00025F18
		private Type ResolveTypeFromName(string wireName, Type expectedType)
		{
			Type type;
			if (!ClientConvert.ToNamedType(wireName, out type))
			{
				type = this.resolveTypeFromName(wireName);
				if (type == null)
				{
					type = ClientTypeCache.ResolveFromName(wireName, expectedType);
				}
				if (!this.skipTypeAssignabilityCheck && type != null && !expectedType.IsAssignableFrom(type))
				{
					throw Error.InvalidOperation(Strings.Deserialize_Current(expectedType, type));
				}
			}
			return type ?? expectedType;
		}

		// Token: 0x0400059B RID: 1435
		private readonly IDictionary<string, ClientTypeAnnotation> edmTypeNameMap = new Dictionary<string, ClientTypeAnnotation>(StringComparer.Ordinal);

		// Token: 0x0400059C RID: 1436
		private readonly Func<string, Type> resolveTypeFromName;

		// Token: 0x0400059D RID: 1437
		private readonly Func<Type, string> resolveNameFromType;

		// Token: 0x0400059E RID: 1438
		private readonly ClientEdmModel clientEdmModel;

		// Token: 0x0400059F RID: 1439
		private readonly IEdmModel serviceModel;

		// Token: 0x040005A0 RID: 1440
		private bool skipTypeAssignabilityCheck;
	}
}
