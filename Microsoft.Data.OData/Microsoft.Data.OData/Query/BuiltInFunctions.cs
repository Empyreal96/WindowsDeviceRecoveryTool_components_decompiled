using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000AA RID: 170
	internal static class BuiltInFunctions
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		internal static bool TryGetBuiltInFunction(string name, out FunctionSignatureWithReturnType[] signatures)
		{
			return BuiltInFunctions.builtInFunctions.TryGetValue(name, out signatures);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		internal static string BuildFunctionSignatureListDescription(string name, IEnumerable<FunctionSignature> signatures)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			foreach (FunctionSignature functionSignature in signatures)
			{
				FunctionSignatureWithReturnType functionSignatureWithReturnType = (FunctionSignatureWithReturnType)functionSignature;
				stringBuilder.Append(value);
				value = "; ";
				string value2 = "";
				stringBuilder.Append(name);
				stringBuilder.Append('(');
				foreach (IEdmTypeReference edmTypeReference in functionSignatureWithReturnType.ArgumentTypes)
				{
					stringBuilder.Append(value2);
					value2 = ", ";
					if (edmTypeReference.IsODataPrimitiveTypeKind() && edmTypeReference.IsNullable)
					{
						stringBuilder.Append(edmTypeReference.ODataFullName());
						stringBuilder.Append(" Nullable=true");
					}
					else
					{
						stringBuilder.Append(edmTypeReference.ODataFullName());
					}
				}
				stringBuilder.Append(')');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		internal static void CreateSpatialFunctions(IDictionary<string, FunctionSignatureWithReturnType[]> functions)
		{
			FunctionSignatureWithReturnType[] value = new FunctionSignatureWithReturnType[]
			{
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPoint, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPoint, true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPoint, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPoint, true)
				})
			};
			functions.Add("geo.distance", value);
			value = new FunctionSignatureWithReturnType[]
			{
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPoint, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPolygon, true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPolygon, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryPoint, true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPoint, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPolygon, true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPolygon, true),
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyPoint, true)
				})
			};
			functions.Add("geo.intersects", value);
			value = new FunctionSignatureWithReturnType[]
			{
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeometryLineString, true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.GeographyLineString, true)
				})
			};
			functions.Add("geo.length", value);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
		private static Dictionary<string, FunctionSignatureWithReturnType[]> InitializeBuiltInFunctions()
		{
			Dictionary<string, FunctionSignatureWithReturnType[]> dictionary = new Dictionary<string, FunctionSignatureWithReturnType[]>(StringComparer.Ordinal);
			BuiltInFunctions.CreateStringFunctions(dictionary);
			BuiltInFunctions.CreateSpatialFunctions(dictionary);
			BuiltInFunctions.CreateDateTimeFunctions(dictionary);
			BuiltInFunctions.CreateMathFunctions(dictionary);
			return dictionary;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000CA14 File Offset: 0x0000AC14
		private static void CreateStringFunctions(IDictionary<string, FunctionSignatureWithReturnType[]> functions)
		{
			FunctionSignatureWithReturnType functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("endswith", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("indexof", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("replace", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("startswith", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("tolower", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("toupper", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("trim", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			FunctionSignatureWithReturnType[] value = new FunctionSignatureWithReturnType[]
			{
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(false)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(false),
					EdmCoreModel.Instance.GetInt32(false)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(true),
					EdmCoreModel.Instance.GetInt32(false)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(false),
					EdmCoreModel.Instance.GetInt32(true)
				}),
				new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
				{
					EdmCoreModel.Instance.GetString(true),
					EdmCoreModel.Instance.GetInt32(true),
					EdmCoreModel.Instance.GetInt32(true)
				})
			};
			functions.Add("substring", value);
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetBoolean(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("substringof", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetString(true), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("concat", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
			functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true)
			});
			functions.Add("length", new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType
			});
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		private static void CreateDateTimeFunctions(IDictionary<string, FunctionSignatureWithReturnType[]> functions)
		{
			FunctionSignatureWithReturnType[] array = BuiltInFunctions.CreateDateTimeFunctionSignatureArray();
			FunctionSignatureWithReturnType[] value = array.Concat(BuiltInFunctions.CreateTimeSpanFunctionSignatures()).ToArray<FunctionSignatureWithReturnType>();
			functions.Add("year", array);
			functions.Add("month", array);
			functions.Add("day", array);
			functions.Add("hour", value);
			functions.Add("minute", value);
			functions.Add("second", value);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000CF54 File Offset: 0x0000B154
		private static FunctionSignatureWithReturnType[] CreateDateTimeFunctionSignatureArray()
		{
			FunctionSignatureWithReturnType functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, false)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType2 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, true)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType3 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, false)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType4 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, true)
			});
			return new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType,
				functionSignatureWithReturnType2,
				functionSignatureWithReturnType3,
				functionSignatureWithReturnType4
			};
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000D158 File Offset: 0x0000B358
		private static IEnumerable<FunctionSignatureWithReturnType> CreateTimeSpanFunctionSignatures()
		{
			yield return new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, false)
			});
			yield return new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetInt32(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, true)
			});
			yield break;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000D16E File Offset: 0x0000B36E
		private static void CreateMathFunctions(IDictionary<string, FunctionSignatureWithReturnType[]> functions)
		{
			functions.Add("round", BuiltInFunctions.CreateMathFunctionSignatureArray());
			functions.Add("floor", BuiltInFunctions.CreateMathFunctionSignatureArray());
			functions.Add("ceiling", BuiltInFunctions.CreateMathFunctionSignatureArray());
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
		private static FunctionSignatureWithReturnType[] CreateMathFunctionSignatureArray()
		{
			FunctionSignatureWithReturnType functionSignatureWithReturnType = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(false)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType2 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDouble(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(true)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType3 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDecimal(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(false)
			});
			FunctionSignatureWithReturnType functionSignatureWithReturnType4 = new FunctionSignatureWithReturnType(EdmCoreModel.Instance.GetDecimal(false), new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(true)
			});
			return new FunctionSignatureWithReturnType[]
			{
				functionSignatureWithReturnType,
				functionSignatureWithReturnType3,
				functionSignatureWithReturnType2,
				functionSignatureWithReturnType4
			};
		}

		// Token: 0x0400014E RID: 334
		private static readonly Dictionary<string, FunctionSignatureWithReturnType[]> builtInFunctions = BuiltInFunctions.InitializeBuiltInFunctions();
	}
}
