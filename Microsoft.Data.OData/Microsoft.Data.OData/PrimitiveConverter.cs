using System;
using System.Collections.Generic;
using System.Spatial;
using System.Xml;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData
{
	// Token: 0x0200023E RID: 574
	internal sealed class PrimitiveConverter
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x00044A08 File Offset: 0x00042C08
		internal PrimitiveConverter(KeyValuePair<Type, IPrimitiveTypeConverter>[] spatialPrimitiveTypeConverters)
		{
			this.spatialPrimitiveTypeConverters = new Dictionary<Type, IPrimitiveTypeConverter>(EqualityComparer<Type>.Default);
			foreach (KeyValuePair<Type, IPrimitiveTypeConverter> keyValuePair in spatialPrimitiveTypeConverters)
			{
				this.spatialPrimitiveTypeConverters.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x00044A61 File Offset: 0x00042C61
		internal static PrimitiveConverter Instance
		{
			get
			{
				return PrimitiveConverter.primitiveConverter;
			}
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00044A68 File Offset: 0x00042C68
		internal bool TryTokenizeFromXml(XmlReader reader, Type targetType, out object tokenizedPropertyValue)
		{
			tokenizedPropertyValue = null;
			IPrimitiveTypeConverter primitiveTypeConverter;
			if (this.TryGetConverter(targetType, out primitiveTypeConverter))
			{
				tokenizedPropertyValue = primitiveTypeConverter.TokenizeFromXml(reader);
				return true;
			}
			return false;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00044AAC File Offset: 0x00042CAC
		internal bool TryWriteAtom(object instance, XmlWriter writer)
		{
			return this.TryWriteValue(instance, delegate(IPrimitiveTypeConverter ptc)
			{
				ptc.WriteAtom(instance, writer);
			});
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00044AE8 File Offset: 0x00042CE8
		internal void WriteVerboseJson(object instance, IJsonWriter jsonWriter, string typeName, ODataVersion odataVersion)
		{
			Type type = instance.GetType();
			IPrimitiveTypeConverter primitiveTypeConverter;
			this.TryGetConverter(type, out primitiveTypeConverter);
			primitiveTypeConverter.WriteVerboseJson(instance, jsonWriter, typeName, odataVersion);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00044B14 File Offset: 0x00042D14
		internal void WriteJsonLight(object instance, IJsonWriter jsonWriter, ODataVersion odataVersion)
		{
			Type type = instance.GetType();
			IPrimitiveTypeConverter primitiveTypeConverter;
			this.TryGetConverter(type, out primitiveTypeConverter);
			primitiveTypeConverter.WriteJsonLight(instance, jsonWriter, odataVersion);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00044B3C File Offset: 0x00042D3C
		private bool TryWriteValue(object instance, Action<IPrimitiveTypeConverter> writeMethod)
		{
			Type type = instance.GetType();
			IPrimitiveTypeConverter obj;
			if (this.TryGetConverter(type, out obj))
			{
				writeMethod(obj);
				return true;
			}
			return false;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00044B68 File Offset: 0x00042D68
		private bool TryGetConverter(Type type, out IPrimitiveTypeConverter primitiveTypeConverter)
		{
			if (typeof(ISpatial).IsAssignableFrom(type))
			{
				KeyValuePair<Type, IPrimitiveTypeConverter> keyValuePair = new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(object), null);
				foreach (KeyValuePair<Type, IPrimitiveTypeConverter> keyValuePair2 in this.spatialPrimitiveTypeConverters)
				{
					if (keyValuePair2.Key.IsAssignableFrom(type) && keyValuePair.Key.IsAssignableFrom(keyValuePair2.Key))
					{
						keyValuePair = keyValuePair2;
					}
				}
				primitiveTypeConverter = keyValuePair.Value;
				return keyValuePair.Value != null;
			}
			primitiveTypeConverter = null;
			return false;
		}

		// Token: 0x040006A2 RID: 1698
		private static readonly IPrimitiveTypeConverter geographyTypeConverter = new GeographyTypeConverter();

		// Token: 0x040006A3 RID: 1699
		private static readonly IPrimitiveTypeConverter geometryTypeConverter = new GeometryTypeConverter();

		// Token: 0x040006A4 RID: 1700
		private static readonly PrimitiveConverter primitiveConverter = new PrimitiveConverter(new KeyValuePair<Type, IPrimitiveTypeConverter>[]
		{
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyPoint), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyLineString), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyPolygon), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyCollection), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyMultiPoint), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyMultiLineString), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeographyMultiPolygon), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(Geography), PrimitiveConverter.geographyTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryPoint), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryLineString), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryPolygon), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryCollection), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryMultiPoint), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryMultiLineString), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(GeometryMultiPolygon), PrimitiveConverter.geometryTypeConverter),
			new KeyValuePair<Type, IPrimitiveTypeConverter>(typeof(Geometry), PrimitiveConverter.geometryTypeConverter)
		});

		// Token: 0x040006A5 RID: 1701
		private readonly Dictionary<Type, IPrimitiveTypeConverter> spatialPrimitiveTypeConverters;
	}
}
