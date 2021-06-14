using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020001CA RID: 458
	internal class BamlMapTable
	{
		// Token: 0x06001D2A RID: 7466 RVA: 0x00087FE0 File Offset: 0x000861E0
		static BamlMapTable()
		{
			BamlMapTable.KnownAssemblyInfoRecord = new BamlAssemblyInfoRecord();
			BamlMapTable.KnownAssemblyInfoRecord.AssemblyId = -1;
			BamlMapTable.KnownAssemblyInfoRecord.Assembly = ReflectionHelper.LoadAssembly("PresentationFramework", string.Empty);
			BamlMapTable.KnownAssemblyInfoRecord.AssemblyFullName = BamlMapTable.KnownAssemblyInfoRecord.Assembly.FullName;
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00088068 File Offset: 0x00086268
		internal BamlMapTable(XamlTypeMapper xamlTypeMapper)
		{
			this._xamlTypeMapper = xamlTypeMapper;
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000880BE File Offset: 0x000862BE
		internal object CreateKnownTypeFromId(short id)
		{
			if (id < 0)
			{
				return KnownTypes.CreateKnownElement((KnownElements)(-(KnownElements)id));
			}
			return null;
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000880CE File Offset: 0x000862CE
		internal static Type GetKnownTypeFromId(short id)
		{
			if (id < 0)
			{
				return KnownTypes.Types[(int)(-(int)id)];
			}
			return null;
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000880E4 File Offset: 0x000862E4
		internal static short GetKnownTypeIdFromName(string assemblyFullName, string clrNamespace, string typeShortName)
		{
			if (typeShortName == string.Empty)
			{
				return 0;
			}
			int num = 759;
			int i = 1;
			while (i <= num)
			{
				int num2 = (num + i) / 2;
				Type type = KnownTypes.Types[num2];
				int num3 = string.CompareOrdinal(typeShortName, type.Name);
				if (num3 == 0)
				{
					if (type.Namespace == clrNamespace && type.Assembly.FullName == assemblyFullName)
					{
						return (short)(-(short)num2);
					}
					return 0;
				}
				else if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return 0;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x00088169 File Offset: 0x00086369
		internal static short GetKnownTypeIdFromType(Type type)
		{
			if (type == null)
			{
				return 0;
			}
			return BamlMapTable.GetKnownTypeIdFromName(type.Assembly.FullName, type.Namespace, type.Name);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x00088194 File Offset: 0x00086394
		private static short GetKnownStringIdFromName(string stringValue)
		{
			int num = BamlMapTable._knownStrings.Length;
			for (int i = 1; i < num; i++)
			{
				if (BamlMapTable._knownStrings[i] == stringValue)
				{
					return (short)(-(short)i);
				}
			}
			return 0;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000881CC File Offset: 0x000863CC
		internal static KnownElements GetKnownTypeConverterIdFromType(Type type)
		{
			KnownElements result;
			if (ReflectionHelper.IsNullableType(type))
			{
				result = KnownElements.NullableConverter;
			}
			else if (type == typeof(Type))
			{
				result = KnownElements.TypeTypeConverter;
			}
			else
			{
				short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(type);
				if (knownTypeIdFromType < 0)
				{
					result = KnownTypes.GetKnownTypeConverterId((KnownElements)(-(KnownElements)knownTypeIdFromType));
				}
				else
				{
					result = KnownElements.UnknownElement;
				}
			}
			return result;
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0008821C File Offset: 0x0008641C
		internal TypeConverter GetKnownConverterFromType(Type type)
		{
			KnownElements knownTypeConverterIdFromType = BamlMapTable.GetKnownTypeConverterIdFromType(type);
			TypeConverter result;
			if (knownTypeConverterIdFromType != KnownElements.UnknownElement)
			{
				result = this.GetConverterFromId((short)(-(short)knownTypeConverterIdFromType), type, null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x00088244 File Offset: 0x00086444
		internal static TypeConverter GetKnownConverterFromType_NoCache(Type type)
		{
			KnownElements knownTypeConverterIdFromType = BamlMapTable.GetKnownTypeConverterIdFromType(type);
			TypeConverter result;
			if (knownTypeConverterIdFromType != KnownElements.UnknownElement)
			{
				if (knownTypeConverterIdFromType != KnownElements.EnumConverter)
				{
					if (knownTypeConverterIdFromType != KnownElements.NullableConverter)
					{
						result = (KnownTypes.CreateKnownElement(knownTypeConverterIdFromType) as TypeConverter);
					}
					else
					{
						result = new NullableConverter(type);
					}
				}
				else
				{
					result = new EnumConverter(type);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x00088290 File Offset: 0x00086490
		internal Type GetKnownConverterTypeFromType(Type type)
		{
			if (type == typeof(Type))
			{
				return typeof(TypeTypeConverter);
			}
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(type);
			if (knownTypeIdFromType == 0)
			{
				return null;
			}
			KnownElements knownElement = (KnownElements)(-(KnownElements)knownTypeIdFromType);
			KnownElements knownTypeConverterId = KnownTypes.GetKnownTypeConverterId(knownElement);
			if (knownTypeConverterId == KnownElements.UnknownElement)
			{
				return null;
			}
			return KnownTypes.Types[(int)knownTypeConverterId];
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000882E4 File Offset: 0x000864E4
		private static Type GetKnownConverterTypeFromPropName(Type propOwnerType, string propName)
		{
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(propOwnerType);
			if (knownTypeIdFromType == 0)
			{
				return null;
			}
			KnownElements id = (KnownElements)(-(KnownElements)knownTypeIdFromType);
			KnownElements knownTypeConverterIdForProperty = KnownTypes.GetKnownTypeConverterIdForProperty(id, propName);
			if (knownTypeConverterIdForProperty == KnownElements.UnknownElement)
			{
				return null;
			}
			return KnownTypes.Types[(int)knownTypeConverterIdForProperty];
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0008831C File Offset: 0x0008651C
		internal void Initialize()
		{
			if (this.AttributeIdMap.Count > 0 || this.TypeIdMap.Count > 0)
			{
				this._reusingMapTable = true;
				if (this.ObjectHashTable.Count == 0)
				{
					for (int i = 0; i < this.AttributeIdMap.Count; i++)
					{
						BamlAttributeInfoRecord bamlAttributeInfoRecord = this.AttributeIdMap[i] as BamlAttributeInfoRecord;
						if (bamlAttributeInfoRecord.PropInfo != null)
						{
							object attributeInfoKey = this.GetAttributeInfoKey(bamlAttributeInfoRecord.OwnerType.FullName, bamlAttributeInfoRecord.Name);
							this.ObjectHashTable.Add(attributeInfoKey, bamlAttributeInfoRecord);
						}
					}
					for (int j = 0; j < this.TypeIdMap.Count; j++)
					{
						BamlTypeInfoRecord bamlTypeInfoRecord = this.TypeIdMap[j] as BamlTypeInfoRecord;
						if (bamlTypeInfoRecord.Type != null)
						{
							BamlAssemblyInfoRecord assemblyInfoFromId = this.GetAssemblyInfoFromId(bamlTypeInfoRecord.AssemblyId);
							TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyInfoFromId.AssemblyFullName, bamlTypeInfoRecord.TypeFullName);
							this.ObjectHashTable.Add(typeInfoKey, bamlTypeInfoRecord);
						}
					}
				}
			}
			this.AssemblyIdMap.Clear();
			this.TypeIdMap.Clear();
			this.AttributeIdMap.Clear();
			this.StringIdMap.Clear();
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x00088458 File Offset: 0x00086658
		internal Type GetTypeFromId(short id)
		{
			Type type = null;
			if (id < 0)
			{
				return KnownTypes.Types[(int)(-(int)id)];
			}
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.TypeIdMap[(int)id];
			if (bamlTypeInfoRecord != null)
			{
				type = this.GetTypeFromTypeInfo(bamlTypeInfoRecord);
				if (null == type)
				{
					this.ThrowException("ParserFailFindType", bamlTypeInfoRecord.TypeFullName);
				}
			}
			return type;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000884B0 File Offset: 0x000866B0
		internal bool HasSerializerForTypeId(short id)
		{
			return id < 0 && (-id == 620 || -id == 231 || -id == 108 || -id == 120 || -id == 271 || -id == 330);
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000884EC File Offset: 0x000866EC
		internal BamlTypeInfoRecord GetTypeInfoFromId(short id)
		{
			if (id < 0)
			{
				BamlTypeInfoRecord bamlTypeInfoRecord;
				if (-id == 620)
				{
					bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = -750;
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerType = KnownTypes.Types[750];
					bamlTypeInfoRecord.AssemblyId = -1;
				}
				else if (-id == 108 || -id == 120 || -id == 271 || -id == 330)
				{
					bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = -751;
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerType = KnownTypes.Types[751];
					bamlTypeInfoRecord.AssemblyId = -1;
				}
				else
				{
					bamlTypeInfoRecord = new BamlTypeInfoRecord();
					bamlTypeInfoRecord.AssemblyId = this.GetAssemblyIdForType(KnownTypes.Types[(int)(-(int)id)]);
				}
				bamlTypeInfoRecord.TypeId = id;
				bamlTypeInfoRecord.Type = KnownTypes.Types[(int)(-(int)id)];
				bamlTypeInfoRecord.TypeFullName = bamlTypeInfoRecord.Type.FullName;
				return bamlTypeInfoRecord;
			}
			return (BamlTypeInfoRecord)this.TypeIdMap[(int)id];
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000885F4 File Offset: 0x000867F4
		private short GetAssemblyIdForType(Type t)
		{
			string fullName = t.Assembly.FullName;
			for (int i = 0; i < this.AssemblyIdMap.Count; i++)
			{
				string assemblyFullName = ((BamlAssemblyInfoRecord)this.AssemblyIdMap[i]).AssemblyFullName;
				if (assemblyFullName == fullName)
				{
					return (short)i;
				}
			}
			return -1;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00088648 File Offset: 0x00086848
		internal TypeConverter GetConverterFromId(short typeId, Type propType, ParserContext pc)
		{
			TypeConverter typeConverter;
			if (typeId < 0)
			{
				KnownElements knownElements = (KnownElements)(-(KnownElements)typeId);
				if (knownElements != KnownElements.EnumConverter)
				{
					if (knownElements != KnownElements.NullableConverter)
					{
						typeConverter = this.GetConverterFromCache(typeId);
						if (typeConverter == null)
						{
							typeConverter = (this.CreateKnownTypeFromId(typeId) as TypeConverter);
							this.ConverterCache.Add(typeId, typeConverter);
						}
					}
					else
					{
						typeConverter = this.GetConverterFromCache(propType);
						if (typeConverter == null)
						{
							typeConverter = new NullableConverter(propType);
							this.ConverterCache.Add(propType, typeConverter);
						}
					}
				}
				else
				{
					typeConverter = this.GetConverterFromCache(propType);
					if (typeConverter == null)
					{
						typeConverter = new EnumConverter(propType);
						this.ConverterCache.Add(propType, typeConverter);
					}
				}
			}
			else
			{
				Type typeFromId = this.GetTypeFromId(typeId);
				typeConverter = this.GetConverterFromCache(typeFromId);
				if (typeConverter == null)
				{
					if (ReflectionHelper.IsPublicType(typeFromId))
					{
						typeConverter = (Activator.CreateInstance(typeFromId) as TypeConverter);
					}
					else
					{
						typeConverter = (XamlTypeMapper.CreateInternalInstance(pc, typeFromId) as TypeConverter);
					}
					if (typeConverter == null)
					{
						this.ThrowException("ParserNoTypeConv", propType.Name);
					}
					else
					{
						this.ConverterCache.Add(typeFromId, typeConverter);
					}
				}
			}
			return typeConverter;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00088748 File Offset: 0x00086948
		internal string GetStringFromStringId(int id)
		{
			if (id < 0)
			{
				return BamlMapTable._knownStrings[-id];
			}
			BamlStringInfoRecord bamlStringInfoRecord = (BamlStringInfoRecord)this.StringIdMap[id];
			return bamlStringInfoRecord.Value;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0008877C File Offset: 0x0008697C
		internal BamlAttributeInfoRecord GetAttributeInfoFromId(short id)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				BamlAttributeInfoRecord bamlAttributeInfoRecord = new BamlAttributeInfoRecord();
				bamlAttributeInfoRecord.AttributeId = id;
				bamlAttributeInfoRecord.OwnerTypeId = (short)(-(short)KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties));
				this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.Name = this.GetAttributeNameFromKnownId(knownProperties);
				if (knownProperties < KnownProperties.MaxDependencyProperty)
				{
					DependencyProperty knownDependencyPropertyFromId = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
					bamlAttributeInfoRecord.DP = knownDependencyPropertyFromId;
				}
				else
				{
					Type ownerType = bamlAttributeInfoRecord.OwnerType;
					bamlAttributeInfoRecord.PropInfo = ownerType.GetProperty(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public);
				}
				return bamlAttributeInfoRecord;
			}
			return (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00088808 File Offset: 0x00086A08
		internal BamlAttributeInfoRecord GetAttributeInfoFromIdWithOwnerType(short attributeId)
		{
			BamlAttributeInfoRecord attributeInfoFromId = this.GetAttributeInfoFromId(attributeId);
			this.GetAttributeOwnerType(attributeInfoFromId);
			return attributeInfoFromId;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00088828 File Offset: 0x00086A28
		private string GetAttributeNameFromKnownId(KnownProperties knownId)
		{
			if (knownId < KnownProperties.MaxDependencyProperty)
			{
				DependencyProperty knownDependencyPropertyFromId = KnownTypes.GetKnownDependencyPropertyFromId(knownId);
				return knownDependencyPropertyFromId.Name;
			}
			return KnownTypes.GetKnownClrPropertyNameFromId(knownId);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00088854 File Offset: 0x00086A54
		internal string GetAttributeNameFromId(short id)
		{
			if (id < 0)
			{
				return this.GetAttributeNameFromKnownId((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			if (bamlAttributeInfoRecord != null)
			{
				return bamlAttributeInfoRecord.Name;
			}
			return null;
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0008888C File Offset: 0x00086A8C
		internal bool DoesAttributeMatch(short id, short ownerTypeId, string name)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				string attributeNameFromKnownId = this.GetAttributeNameFromKnownId(knownProperties);
				KnownElements knownElementFromKnownCommonProperty = KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties);
				return ownerTypeId == (short)(-(short)knownElementFromKnownCommonProperty) && string.CompareOrdinal(attributeNameFromKnownId, name) == 0;
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			return bamlAttributeInfoRecord.OwnerTypeId == ownerTypeId && string.CompareOrdinal(bamlAttributeInfoRecord.Name, name) == 0;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000888F0 File Offset: 0x00086AF0
		internal bool DoesAttributeMatch(short id, string name)
		{
			string attributeNameFromId = this.GetAttributeNameFromId(id);
			return attributeNameFromId != null && string.CompareOrdinal(attributeNameFromId, name) == 0;
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00088914 File Offset: 0x00086B14
		internal bool DoesAttributeMatch(short id, BamlAttributeUsage attributeUsage)
		{
			if (id < 0)
			{
				return attributeUsage == BamlMapTable.GetAttributeUsageFromKnownAttribute((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			return attributeUsage == bamlAttributeInfoRecord.AttributeUsage;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0008894C File Offset: 0x00086B4C
		internal void GetAttributeInfoFromId(short id, out short ownerTypeId, out string name, out BamlAttributeUsage attributeUsage)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				name = this.GetAttributeNameFromKnownId(knownProperties);
				ownerTypeId = (short)(-(short)KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties));
				attributeUsage = BamlMapTable.GetAttributeUsageFromKnownAttribute(knownProperties);
				return;
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			name = bamlAttributeInfoRecord.Name;
			ownerTypeId = bamlAttributeInfoRecord.OwnerTypeId;
			attributeUsage = bamlAttributeInfoRecord.AttributeUsage;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000889A9 File Offset: 0x00086BA9
		private static BamlAttributeUsage GetAttributeUsageFromKnownAttribute(KnownProperties knownId)
		{
			if (knownId == KnownProperties.FrameworkElement_Name)
			{
				return BamlAttributeUsage.RuntimeName;
			}
			return BamlAttributeUsage.Default;
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000889B4 File Offset: 0x00086BB4
		internal Type GetTypeFromTypeInfo(BamlTypeInfoRecord typeInfo)
		{
			if (null == typeInfo.Type)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.GetAssemblyInfoFromId(typeInfo.AssemblyId);
				if (assemblyInfoFromId != null)
				{
					TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyInfoFromId.AssemblyFullName, typeInfo.TypeFullName);
					BamlTypeInfoRecord bamlTypeInfoRecord = this.GetHashTableData(typeInfoKey) as BamlTypeInfoRecord;
					if (bamlTypeInfoRecord != null && bamlTypeInfoRecord.Type != null)
					{
						typeInfo.Type = bamlTypeInfoRecord.Type;
					}
					else
					{
						Assembly assemblyFromAssemblyInfo = this.GetAssemblyFromAssemblyInfo(assemblyInfoFromId);
						if (null != assemblyFromAssemblyInfo)
						{
							Type type = assemblyFromAssemblyInfo.GetType(typeInfo.TypeFullName);
							typeInfo.Type = type;
							this.AddHashTableData(typeInfoKey, typeInfo);
						}
					}
				}
			}
			return typeInfo.Type;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x00088A60 File Offset: 0x00086C60
		private Type GetAttributeOwnerType(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.OwnerType == null)
			{
				if (bamlAttributeInfoRecord.OwnerTypeId < 0)
				{
					bamlAttributeInfoRecord.OwnerType = BamlMapTable.GetKnownTypeFromId(bamlAttributeInfoRecord.OwnerTypeId);
				}
				else
				{
					BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.TypeIdMap[(int)bamlAttributeInfoRecord.OwnerTypeId];
					if (bamlTypeInfoRecord != null)
					{
						bamlAttributeInfoRecord.OwnerType = this.GetTypeFromTypeInfo(bamlTypeInfoRecord);
					}
				}
			}
			return bamlAttributeInfoRecord.OwnerType;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x00088AC4 File Offset: 0x00086CC4
		internal Type GetCLRPropertyTypeAndNameFromId(short attributeId, out string propName)
		{
			propName = null;
			Type type = null;
			BamlAttributeInfoRecord attributeInfoFromIdWithOwnerType = this.GetAttributeInfoFromIdWithOwnerType(attributeId);
			if (attributeInfoFromIdWithOwnerType != null && attributeInfoFromIdWithOwnerType.OwnerType != null)
			{
				this.XamlTypeMapper.UpdateClrPropertyInfo(attributeInfoFromIdWithOwnerType.OwnerType, attributeInfoFromIdWithOwnerType);
				type = attributeInfoFromIdWithOwnerType.GetPropertyType();
			}
			else
			{
				propName = string.Empty;
			}
			if (type == null)
			{
				if (propName == null)
				{
					propName = attributeInfoFromIdWithOwnerType.OwnerType.FullName + "." + attributeInfoFromIdWithOwnerType.Name;
				}
				this.ThrowException("ParserNoPropType", propName);
			}
			else
			{
				propName = attributeInfoFromIdWithOwnerType.Name;
			}
			return type;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x00088B54 File Offset: 0x00086D54
		internal DependencyProperty GetDependencyPropertyValueFromId(short memberId, string memberName, out Type declaringType)
		{
			declaringType = null;
			DependencyProperty result = null;
			if (memberName == null)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)memberId);
				if (knownProperties < KnownProperties.MaxDependencyProperty || knownProperties == KnownProperties.Run_Text)
				{
					result = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
				}
			}
			else
			{
				declaringType = this.GetTypeFromId(memberId);
				result = DependencyProperty.FromName(memberName, declaringType);
			}
			return result;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00088B9C File Offset: 0x00086D9C
		internal DependencyProperty GetDependencyPropertyValueFromId(short memberId)
		{
			DependencyProperty dependencyProperty = null;
			if (memberId < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)memberId);
				if (knownProperties < KnownProperties.MaxDependencyProperty)
				{
					dependencyProperty = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
				}
			}
			if (dependencyProperty == null)
			{
				short id;
				string name;
				BamlAttributeUsage bamlAttributeUsage;
				this.GetAttributeInfoFromId(memberId, out id, out name, out bamlAttributeUsage);
				Type typeFromId = this.GetTypeFromId(id);
				dependencyProperty = DependencyProperty.FromName(name, typeFromId);
			}
			return dependencyProperty;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00088BE8 File Offset: 0x00086DE8
		internal DependencyProperty GetDependencyProperty(int id)
		{
			if (id < 0)
			{
				return KnownTypes.GetKnownDependencyPropertyFromId((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[id];
			return this.GetDependencyProperty(bamlAttributeInfoRecord);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00088C1C File Offset: 0x00086E1C
		internal DependencyProperty GetDependencyProperty(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.DP == null && null == bamlAttributeInfoRecord.PropInfo)
			{
				this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				if (null != bamlAttributeInfoRecord.OwnerType)
				{
					bamlAttributeInfoRecord.DP = DependencyProperty.FromName(bamlAttributeInfoRecord.Name, bamlAttributeInfoRecord.OwnerType);
				}
			}
			return bamlAttributeInfoRecord.DP;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00088C74 File Offset: 0x00086E74
		internal RoutedEvent GetRoutedEvent(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.Event == null)
			{
				Type attributeOwnerType = this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				if (null != attributeOwnerType)
				{
					bamlAttributeInfoRecord.Event = this.XamlTypeMapper.RoutedEventFromName(bamlAttributeInfoRecord.Name, attributeOwnerType);
				}
			}
			return bamlAttributeInfoRecord.Event;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00088CB8 File Offset: 0x00086EB8
		internal short GetAttributeOrTypeId(BinaryWriter binaryWriter, Type declaringType, string memberName, out short typeId)
		{
			short result = 0;
			if (!this.GetTypeInfoId(binaryWriter, declaringType.Assembly.FullName, declaringType.FullName, out typeId))
			{
				typeId = this.AddTypeInfoMap(binaryWriter, declaringType.Assembly.FullName, declaringType.FullName, declaringType, string.Empty, string.Empty);
			}
			else if (typeId < 0)
			{
				result = -KnownTypes.GetKnownPropertyAttributeId((KnownElements)(-(KnownElements)typeId), memberName);
			}
			return result;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00088D20 File Offset: 0x00086F20
		internal BamlAssemblyInfoRecord GetAssemblyInfoFromId(short id)
		{
			if (id == -1)
			{
				return BamlMapTable.KnownAssemblyInfoRecord;
			}
			return (BamlAssemblyInfoRecord)this.AssemblyIdMap[(int)id];
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00088D40 File Offset: 0x00086F40
		private Assembly GetAssemblyFromAssemblyInfo(BamlAssemblyInfoRecord assemblyInfoRecord)
		{
			if (null == assemblyInfoRecord.Assembly)
			{
				string assemblyPath = this.XamlTypeMapper.AssemblyPathFor(assemblyInfoRecord.AssemblyFullName);
				assemblyInfoRecord.Assembly = ReflectionHelper.LoadAssembly(assemblyInfoRecord.AssemblyFullName, assemblyPath);
			}
			return assemblyInfoRecord.Assembly;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00088D88 File Offset: 0x00086F88
		internal BamlAssemblyInfoRecord AddAssemblyMap(BinaryWriter binaryWriter, string assemblyFullName)
		{
			AssemblyInfoKey assemblyInfoKey = new AssemblyInfoKey
			{
				AssemblyFullName = assemblyFullName
			};
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)this.GetHashTableData(assemblyInfoKey);
			if (bamlAssemblyInfoRecord == null)
			{
				bamlAssemblyInfoRecord = new BamlAssemblyInfoRecord();
				bamlAssemblyInfoRecord.AssemblyFullName = assemblyFullName;
				bamlAssemblyInfoRecord.AssemblyId = (short)this.AssemblyIdMap.Add(bamlAssemblyInfoRecord);
				this.ObjectHashTable.Add(assemblyInfoKey, bamlAssemblyInfoRecord);
				bamlAssemblyInfoRecord.Write(binaryWriter);
			}
			else if (bamlAssemblyInfoRecord.AssemblyId == -1)
			{
				bamlAssemblyInfoRecord.AssemblyId = (short)this.AssemblyIdMap.Add(bamlAssemblyInfoRecord);
				bamlAssemblyInfoRecord.Write(binaryWriter);
			}
			return bamlAssemblyInfoRecord;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00088E19 File Offset: 0x00087019
		internal void LoadAssemblyInfoRecord(BamlAssemblyInfoRecord record)
		{
			if (this.AssemblyIdMap.Count == (int)record.AssemblyId)
			{
				this.AssemblyIdMap.Add(record);
			}
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x00088E3C File Offset: 0x0008703C
		internal void EnsureAssemblyRecord(Assembly asm)
		{
			string fullName = asm.FullName;
			if (!(this.ObjectHashTable[fullName] is BamlAssemblyInfoRecord))
			{
				BamlAssemblyInfoRecord bamlAssemblyInfoRecord = new BamlAssemblyInfoRecord();
				bamlAssemblyInfoRecord.AssemblyFullName = fullName;
				bamlAssemblyInfoRecord.Assembly = asm;
				this.ObjectHashTable[fullName] = bamlAssemblyInfoRecord;
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00088E88 File Offset: 0x00087088
		private TypeInfoKey GetTypeInfoKey(string assemblyFullName, string typeFullName)
		{
			return new TypeInfoKey
			{
				DeclaringAssembly = assemblyFullName,
				TypeFullName = typeFullName
			};
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00088EB0 File Offset: 0x000870B0
		internal bool GetTypeInfoId(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, out short typeId)
		{
			int num = typeFullName.LastIndexOf(".", StringComparison.Ordinal);
			string typeShortName;
			string clrNamespace;
			if (num >= 0)
			{
				typeShortName = typeFullName.Substring(num + 1);
				clrNamespace = typeFullName.Substring(0, num);
			}
			else
			{
				typeShortName = typeFullName;
				clrNamespace = string.Empty;
			}
			typeId = BamlMapTable.GetKnownTypeIdFromName(assemblyFullName, clrNamespace, typeShortName);
			if (typeId < 0)
			{
				return true;
			}
			TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyFullName, typeFullName);
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.GetHashTableData(typeInfoKey);
			if (bamlTypeInfoRecord == null)
			{
				return false;
			}
			typeId = bamlTypeInfoRecord.TypeId;
			return true;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00088F2C File Offset: 0x0008712C
		internal short AddTypeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type elementType, string serializerAssemblyFullName, string serializerTypeFullName)
		{
			TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyFullName, typeFullName);
			BamlTypeInfoRecord bamlTypeInfoRecord;
			if (serializerTypeFullName == string.Empty)
			{
				bamlTypeInfoRecord = new BamlTypeInfoRecord();
			}
			else
			{
				bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
				short serializerTypeId;
				if (!this.GetTypeInfoId(binaryWriter, serializerAssemblyFullName, serializerTypeFullName, out serializerTypeId))
				{
					serializerTypeId = this.AddTypeInfoMap(binaryWriter, serializerAssemblyFullName, serializerTypeFullName, null, string.Empty, string.Empty);
				}
				((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = serializerTypeId;
			}
			bamlTypeInfoRecord.TypeFullName = typeFullName;
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = this.AddAssemblyMap(binaryWriter, assemblyFullName);
			bamlTypeInfoRecord.AssemblyId = bamlAssemblyInfoRecord.AssemblyId;
			bamlTypeInfoRecord.IsInternalType = (elementType != null && ReflectionHelper.IsInternalType(elementType));
			bamlTypeInfoRecord.TypeId = (short)this.TypeIdMap.Add(bamlTypeInfoRecord);
			this.ObjectHashTable.Add(typeInfoKey, bamlTypeInfoRecord);
			bamlTypeInfoRecord.Write(binaryWriter);
			return bamlTypeInfoRecord.TypeId;
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x00088FF8 File Offset: 0x000871F8
		internal void LoadTypeInfoRecord(BamlTypeInfoRecord record)
		{
			if (this.TypeIdMap.Count == (int)record.TypeId)
			{
				this.TypeIdMap.Add(record);
			}
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x0008901A File Offset: 0x0008721A
		internal object GetAttributeInfoKey(string ownerTypeName, string attributeName)
		{
			return ownerTypeName + "." + attributeName;
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00089028 File Offset: 0x00087228
		internal short AddAttributeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type owningType, string fieldName, Type attributeType, BamlAttributeUsage attributeUsage)
		{
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			return this.AddAttributeInfoMap(binaryWriter, assemblyFullName, typeFullName, owningType, fieldName, attributeType, attributeUsage, out bamlAttributeInfoRecord);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x00089048 File Offset: 0x00087248
		internal short AddAttributeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type owningType, string fieldName, Type attributeType, BamlAttributeUsage attributeUsage, out BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			short num;
			if (!this.GetTypeInfoId(binaryWriter, assemblyFullName, typeFullName, out num))
			{
				Type xamlSerializerForType = this.XamlTypeMapper.GetXamlSerializerForType(owningType);
				string serializerAssemblyFullName = (xamlSerializerForType == null) ? string.Empty : xamlSerializerForType.Assembly.FullName;
				string serializerTypeFullName = (xamlSerializerForType == null) ? string.Empty : xamlSerializerForType.FullName;
				num = this.AddTypeInfoMap(binaryWriter, assemblyFullName, typeFullName, owningType, serializerAssemblyFullName, serializerTypeFullName);
			}
			else if (num < 0)
			{
				short num2 = -KnownTypes.GetKnownPropertyAttributeId((KnownElements)(-(KnownElements)num), fieldName);
				if (num2 < 0)
				{
					bamlAttributeInfoRecord = null;
					return num2;
				}
			}
			object attributeInfoKey = this.GetAttributeInfoKey(typeFullName, fieldName);
			bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.GetHashTableData(attributeInfoKey);
			if (bamlAttributeInfoRecord == null)
			{
				bamlAttributeInfoRecord = new BamlAttributeInfoRecord();
				bamlAttributeInfoRecord.Name = fieldName;
				bamlAttributeInfoRecord.OwnerTypeId = num;
				bamlAttributeInfoRecord.AttributeId = (short)this.AttributeIdMap.Add(bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.AttributeUsage = attributeUsage;
				this.ObjectHashTable.Add(attributeInfoKey, bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.Write(binaryWriter);
			}
			return bamlAttributeInfoRecord.AttributeId;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00089150 File Offset: 0x00087350
		internal bool GetCustomSerializerOrConverter(BinaryWriter binaryWriter, Type ownerType, Type attributeType, object piOrMi, string fieldName, out short converterOrSerializerTypeId, out Type converterOrSerializerType)
		{
			converterOrSerializerType = null;
			converterOrSerializerTypeId = 0;
			if (!this.ShouldBypassCustomCheck(ownerType, attributeType))
			{
				converterOrSerializerType = this.GetCustomSerializer(attributeType, out converterOrSerializerTypeId);
				if (converterOrSerializerType != null)
				{
					return true;
				}
				converterOrSerializerType = this.GetCustomConverter(piOrMi, ownerType, fieldName, attributeType);
				if (converterOrSerializerType == null && attributeType.IsEnum)
				{
					converterOrSerializerTypeId = 195;
					converterOrSerializerType = KnownTypes.Types[(int)converterOrSerializerTypeId];
					return true;
				}
				if (converterOrSerializerType != null)
				{
					string fullName = converterOrSerializerType.FullName;
					this.EnsureAssemblyRecord(converterOrSerializerType.Assembly);
					if (!this.GetTypeInfoId(binaryWriter, converterOrSerializerType.Assembly.FullName, fullName, out converterOrSerializerTypeId))
					{
						converterOrSerializerTypeId = this.AddTypeInfoMap(binaryWriter, converterOrSerializerType.Assembly.FullName, fullName, null, string.Empty, string.Empty);
					}
				}
			}
			return false;
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x00089228 File Offset: 0x00087428
		internal bool GetStringInfoId(string stringValue, out short stringId)
		{
			stringId = BamlMapTable.GetKnownStringIdFromName(stringValue);
			if (stringId < 0)
			{
				return true;
			}
			BamlStringInfoRecord bamlStringInfoRecord = (BamlStringInfoRecord)this.GetHashTableData(stringValue);
			if (bamlStringInfoRecord == null)
			{
				return false;
			}
			stringId = bamlStringInfoRecord.StringId;
			return true;
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x00089260 File Offset: 0x00087460
		internal short AddStringInfoMap(BinaryWriter binaryWriter, string stringValue)
		{
			BamlStringInfoRecord bamlStringInfoRecord = new BamlStringInfoRecord();
			bamlStringInfoRecord.StringId = (short)this.StringIdMap.Add(bamlStringInfoRecord);
			bamlStringInfoRecord.Value = stringValue;
			this.ObjectHashTable.Add(stringValue, bamlStringInfoRecord);
			bamlStringInfoRecord.Write(binaryWriter);
			return bamlStringInfoRecord.StringId;
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000892A8 File Offset: 0x000874A8
		internal short GetStaticMemberId(BinaryWriter binaryWriter, ParserContext pc, short extensionTypeId, string memberValue, Type defaultTargetType)
		{
			short num = 0;
			Type type = null;
			string text = null;
			if (extensionTypeId != 602)
			{
				if (extensionTypeId == 634)
				{
					type = this.XamlTypeMapper.GetDependencyPropertyOwnerAndName(memberValue, pc, defaultTargetType, out text);
				}
			}
			else
			{
				type = this.XamlTypeMapper.GetTargetTypeAndMember(memberValue, pc, true, out text);
				MemberInfo staticMemberInfo = this.XamlTypeMapper.GetStaticMemberInfo(type, text, false);
				if (staticMemberInfo is PropertyInfo)
				{
					num = SystemResourceKey.GetBamlIdBasedOnSystemResourceKeyId(type, text);
				}
			}
			if (num == 0)
			{
				num = this.AddAttributeInfoMap(binaryWriter, type.Assembly.FullName, type.FullName, type, text, null, BamlAttributeUsage.Default);
			}
			return num;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x00089334 File Offset: 0x00087534
		private bool ShouldBypassCustomCheck(Type declaringType, Type attributeType)
		{
			return declaringType == null || attributeType == null;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00089350 File Offset: 0x00087550
		private Type GetCustomConverter(object piOrMi, Type ownerType, string fieldName, Type attributeType)
		{
			Type type = BamlMapTable.GetKnownConverterTypeFromPropName(ownerType, fieldName);
			if (type != null)
			{
				return type;
			}
			Assembly assembly = ownerType.Assembly;
			if (!assembly.FullName.StartsWith("PresentationFramework", StringComparison.OrdinalIgnoreCase) && !assembly.FullName.StartsWith("PresentationCore", StringComparison.OrdinalIgnoreCase) && !assembly.FullName.StartsWith("WindowsBase", StringComparison.OrdinalIgnoreCase))
			{
				type = this.XamlTypeMapper.GetPropertyConverterType(attributeType, piOrMi);
				if (type != null)
				{
					return type;
				}
			}
			return this.XamlTypeMapper.GetTypeConverterType(attributeType);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000893DC File Offset: 0x000875DC
		private Type GetCustomSerializer(Type type, out short converterOrSerializerTypeId)
		{
			int num;
			if (type == typeof(bool))
			{
				num = 46;
			}
			else if (type == KnownTypes.Types[136])
			{
				num = 137;
			}
			else
			{
				num = this.XamlTypeMapper.GetCustomBamlSerializerIdForType(type);
				if (num == 0)
				{
					converterOrSerializerTypeId = 0;
					return null;
				}
			}
			converterOrSerializerTypeId = (short)num;
			return KnownTypes.Types[num];
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00089444 File Offset: 0x00087644
		private void ThrowException(string id, string parameter)
		{
			ApplicationException ex = new ApplicationException(SR.Get(id, new object[]
			{
				parameter
			}));
			throw ex;
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x00089468 File Offset: 0x00087668
		internal void LoadAttributeInfoRecord(BamlAttributeInfoRecord record)
		{
			if (this.AttributeIdMap.Count == (int)record.AttributeId)
			{
				this.AttributeIdMap.Add(record);
			}
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0008948A File Offset: 0x0008768A
		internal void LoadStringInfoRecord(BamlStringInfoRecord record)
		{
			if (this.StringIdMap.Count == (int)record.StringId)
			{
				this.StringIdMap.Add(record);
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000894AC File Offset: 0x000876AC
		internal object GetHashTableData(object key)
		{
			return this.ObjectHashTable[key];
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000894BA File Offset: 0x000876BA
		internal void AddHashTableData(object key, object data)
		{
			if (this._reusingMapTable)
			{
				this.ObjectHashTable[key] = data;
			}
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000894D4 File Offset: 0x000876D4
		internal BamlMapTable Clone()
		{
			return new BamlMapTable(this._xamlTypeMapper)
			{
				_objectHashTable = (Hashtable)this._objectHashTable.Clone(),
				_assemblyIdToInfo = (ArrayList)this._assemblyIdToInfo.Clone(),
				_typeIdToInfo = (ArrayList)this._typeIdToInfo.Clone(),
				_attributeIdToInfo = (ArrayList)this._attributeIdToInfo.Clone(),
				_stringIdToInfo = (ArrayList)this._stringIdToInfo.Clone()
			};
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0008955C File Offset: 0x0008775C
		private TypeConverter GetConverterFromCache(short typeId)
		{
			TypeConverter result = null;
			if (this._converterCache != null)
			{
				result = (this._converterCache[typeId] as TypeConverter);
			}
			return result;
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0008958C File Offset: 0x0008778C
		private TypeConverter GetConverterFromCache(Type type)
		{
			TypeConverter result = null;
			if (this._converterCache != null)
			{
				result = (this._converterCache[type] as TypeConverter);
			}
			return result;
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000895B6 File Offset: 0x000877B6
		internal void ClearConverterCache()
		{
			if (this._converterCache != null)
			{
				this._converterCache.Clear();
				this._converterCache = null;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x000895D2 File Offset: 0x000877D2
		private Hashtable ObjectHashTable
		{
			get
			{
				return this._objectHashTable;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000895DA File Offset: 0x000877DA
		private ArrayList AssemblyIdMap
		{
			get
			{
				return this._assemblyIdToInfo;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x000895E2 File Offset: 0x000877E2
		private ArrayList TypeIdMap
		{
			get
			{
				return this._typeIdToInfo;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000895EA File Offset: 0x000877EA
		private ArrayList AttributeIdMap
		{
			get
			{
				return this._attributeIdToInfo;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x000895F2 File Offset: 0x000877F2
		private ArrayList StringIdMap
		{
			get
			{
				return this._stringIdToInfo;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000895FA File Offset: 0x000877FA
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x00089602 File Offset: 0x00087802
		internal XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this._xamlTypeMapper;
			}
			set
			{
				this._xamlTypeMapper = value;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x0008960B File Offset: 0x0008780B
		private Hashtable ConverterCache
		{
			get
			{
				if (this._converterCache == null)
				{
					this._converterCache = new Hashtable();
				}
				return this._converterCache;
			}
		}

		// Token: 0x04001420 RID: 5152
		private const string _coreAssembly = "PresentationCore";

		// Token: 0x04001421 RID: 5153
		private const string _frameworkAssembly = "PresentationFramework";

		// Token: 0x04001422 RID: 5154
		private static BamlAssemblyInfoRecord KnownAssemblyInfoRecord;

		// Token: 0x04001423 RID: 5155
		private static string[] _knownStrings = new string[]
		{
			null,
			"Name",
			"Uid"
		};

		// Token: 0x04001424 RID: 5156
		internal static short NameStringId = -1;

		// Token: 0x04001425 RID: 5157
		internal static short UidStringId = -2;

		// Token: 0x04001426 RID: 5158
		internal static string NameString = "Name";

		// Token: 0x04001427 RID: 5159
		private Hashtable _objectHashTable = new Hashtable();

		// Token: 0x04001428 RID: 5160
		private ArrayList _assemblyIdToInfo = new ArrayList(1);

		// Token: 0x04001429 RID: 5161
		private ArrayList _typeIdToInfo = new ArrayList(0);

		// Token: 0x0400142A RID: 5162
		private ArrayList _attributeIdToInfo = new ArrayList(10);

		// Token: 0x0400142B RID: 5163
		private ArrayList _stringIdToInfo = new ArrayList(1);

		// Token: 0x0400142C RID: 5164
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x0400142D RID: 5165
		private Hashtable _converterCache;

		// Token: 0x0400142E RID: 5166
		private bool _reusingMapTable;
	}
}
