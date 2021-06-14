using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Metadata;
using System.Data.Services.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200012F RID: 303
	internal static class WebUtil
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0002AED0 File Offset: 0x000290D0
		private static bool DataServiceCollectionAvailable
		{
			get
			{
				if (WebUtil.dataServiceCollectionAvailable == null)
				{
					try
					{
						WebUtil.dataServiceCollectionAvailable = new bool?(WebUtil.GetDataServiceCollectionOfTType() != null);
					}
					catch (FileNotFoundException)
					{
						WebUtil.dataServiceCollectionAvailable = new bool?(false);
					}
				}
				return WebUtil.dataServiceCollectionAvailable.Value;
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002AF28 File Offset: 0x00029128
		internal static long CopyStream(Stream input, Stream output, ref byte[] refBuffer)
		{
			long num = 0L;
			byte[] array = refBuffer;
			if (array == null)
			{
				refBuffer = (array = new byte[1000]);
			}
			int num2;
			while (input.CanRead && 0 < (num2 = input.Read(array, 0, array.Length)))
			{
				output.Write(array, 0, num2);
				num += (long)num2;
			}
			return num;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002AF78 File Offset: 0x00029178
		internal static InvalidOperationException GetHttpWebResponse(InvalidOperationException exception, ref IODataResponseMessage response)
		{
			if (response == null)
			{
				DataServiceTransportException ex = exception as DataServiceTransportException;
				if (ex != null)
				{
					response = ex.Response;
					return (InvalidOperationException)ex.InnerException;
				}
			}
			return exception;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002AFA8 File Offset: 0x000291A8
		internal static bool SuccessStatusCode(HttpStatusCode status)
		{
			return HttpStatusCode.OK <= status && status < HttpStatusCode.MultipleChoices;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002AFBC File Offset: 0x000291BC
		internal static bool IsCLRTypeCollection(Type type, ClientEdmModel model)
		{
			if (!PrimitiveType.IsKnownNullableType(type))
			{
				Type implementationType = ClientTypeUtil.GetImplementationType(type, typeof(ICollection<>));
				if (implementationType != null && !ClientTypeUtil.TypeIsEntity(implementationType.GetGenericArguments()[0], model))
				{
					if (model.MaxProtocolVersion <= DataServiceProtocolVersion.V2)
					{
						throw new InvalidOperationException(Strings.WebUtil_CollectionTypeNotSupportedInV2OrBelow(type.FullName));
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002B018 File Offset: 0x00029218
		internal static bool IsWireTypeCollection(string wireTypeName)
		{
			return CommonUtil.GetCollectionItemTypeName(wireTypeName, false) != null;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002B027 File Offset: 0x00029227
		internal static string GetCollectionItemWireTypeName(string wireTypeName)
		{
			return CommonUtil.GetCollectionItemTypeName(wireTypeName, false);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002B030 File Offset: 0x00029230
		internal static Type GetBackingTypeForCollectionProperty(Type collectionPropertyType, Type collectionItemType)
		{
			Type result;
			if (collectionPropertyType.IsInterface())
			{
				result = typeof(ObservableCollection<>).MakeGenericType(new Type[]
				{
					collectionItemType
				});
			}
			else
			{
				result = collectionPropertyType;
			}
			return result;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002B068 File Offset: 0x00029268
		internal static T CheckArgumentNull<T>([WebUtil.ValidatedNotNullAttribute] T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw Error.ArgumentNull(parameterName);
			}
			return value;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002B08C File Offset: 0x0002928C
		internal static void ValidateCollection(Type collectionItemType, object propertyValue, string propertyName)
		{
			if (!PrimitiveType.IsKnownNullableType(collectionItemType))
			{
				if (collectionItemType.GetInterfaces().SingleOrDefault((Type t) => t == typeof(IEnumerable)) != null)
				{
					throw Error.InvalidOperation(Strings.ClientType_CollectionOfCollectionNotSupported);
				}
			}
			if (propertyValue != null)
			{
				return;
			}
			if (propertyName != null)
			{
				throw Error.InvalidOperation(Strings.Collection_NullCollectionNotSupported(propertyName));
			}
			throw Error.InvalidOperation(Strings.Collection_NullNonPropertyCollectionNotSupported(collectionItemType));
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002B0FA File Offset: 0x000292FA
		internal static void ValidateCollectionItem(object itemValue)
		{
			if (itemValue == null)
			{
				throw Error.InvalidOperation(Strings.Collection_NullCollectionItemsNotSupported);
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002B10C File Offset: 0x0002930C
		internal static void ValidatePrimitiveCollectionItem(object itemValue, string propertyName, Type collectionItemType)
		{
			Type type = itemValue.GetType();
			if (!PrimitiveType.IsKnownNullableType(type))
			{
				throw Error.InvalidOperation(Strings.Collection_ComplexTypesInCollectionOfPrimitiveTypesNotAllowed);
			}
			if (collectionItemType.IsAssignableFrom(type))
			{
				return;
			}
			if (propertyName != null)
			{
				throw Error.InvalidOperation(Strings.WebUtil_TypeMismatchInCollection(propertyName));
			}
			throw Error.InvalidOperation(Strings.WebUtil_TypeMismatchInNonPropertyCollection(collectionItemType));
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002B158 File Offset: 0x00029358
		internal static void ValidateComplexCollectionItem(object itemValue, string propertyName, Type collectionItemType)
		{
			Type type = itemValue.GetType();
			if (PrimitiveType.IsKnownNullableType(type))
			{
				throw Error.InvalidOperation(Strings.Collection_PrimitiveTypesInCollectionOfComplexTypesNotAllowed);
			}
			if (!(type != collectionItemType))
			{
				return;
			}
			if (propertyName != null)
			{
				throw Error.InvalidOperation(Strings.WebUtil_TypeMismatchInCollection(propertyName));
			}
			throw Error.InvalidOperation(Strings.WebUtil_TypeMismatchInNonPropertyCollection(collectionItemType));
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002B1A4 File Offset: 0x000293A4
		internal static void ValidateIdentityValue(string identity)
		{
			Uri uri = UriUtil.CreateUri(identity, UriKind.RelativeOrAbsolute);
			if (!uri.IsAbsoluteUri)
			{
				throw Error.InvalidOperation(Strings.Context_TrackingExpectsAbsoluteUri);
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002B1CC File Offset: 0x000293CC
		internal static Uri ValidateLocationHeader(string location)
		{
			Uri uri = UriUtil.CreateUri(location, UriKind.RelativeOrAbsolute);
			if (!uri.IsAbsoluteUri)
			{
				throw Error.InvalidOperation(Strings.Context_LocationHeaderExpectsAbsoluteUri);
			}
			return uri;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002B1F8 File Offset: 0x000293F8
		internal static string GetPreferHeaderAndRequestVersion(DataServiceResponsePreference responsePreference, ref Version requestVersion)
		{
			string result = null;
			if (responsePreference != DataServiceResponsePreference.None)
			{
				if (responsePreference == DataServiceResponsePreference.IncludeContent)
				{
					result = "return-content";
				}
				else
				{
					result = "return-no-content";
				}
				WebUtil.RaiseVersion(ref requestVersion, Util.DataServiceVersion3);
			}
			return result;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002B228 File Offset: 0x00029428
		internal static void RaiseVersion(ref Version version, Version minimalVersion)
		{
			if (version == null || version < minimalVersion)
			{
				version = minimalVersion;
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002B241 File Offset: 0x00029441
		internal static bool IsDataServiceCollectionType(Type t)
		{
			return WebUtil.DataServiceCollectionAvailable && t == WebUtil.GetDataServiceCollectionOfTType();
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002B257 File Offset: 0x00029457
		internal static Type GetDataServiceCollectionOfT(params Type[] typeArguments)
		{
			if (WebUtil.DataServiceCollectionAvailable)
			{
				return WebUtil.GetDataServiceCollectionOfTType().MakeGenericType(typeArguments);
			}
			return null;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002B270 File Offset: 0x00029470
		internal static object GetDefaultValue(Type type)
		{
			return WebUtil.getDefaultValueMethodInfo.MakeGenericMethod(new Type[]
			{
				type
			}).Invoke(null, null);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002B29C File Offset: 0x0002949C
		internal static T GetDefaultValue<T>()
		{
			return default(T);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002B2B4 File Offset: 0x000294B4
		internal static void DisposeMessage(IODataResponseMessage responseMessage)
		{
			IDisposable disposable = responseMessage as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002B2D1 File Offset: 0x000294D1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Type GetDataServiceCollectionOfTType()
		{
			return typeof(DataServiceCollection<>);
		}

		// Token: 0x040005E3 RID: 1507
		internal const int DefaultBufferSizeForStreamCopy = 65536;

		// Token: 0x040005E4 RID: 1508
		private static bool? dataServiceCollectionAvailable = null;

		// Token: 0x040005E5 RID: 1509
		private static MethodInfo getDefaultValueMethodInfo = (MethodInfo)typeof(WebUtil).GetMember("GetDefaultValue", BindingFlags.Static | BindingFlags.NonPublic).Single((MemberInfo m) => ((MethodInfo)m).GetGenericArguments().Count<Type>() == 1);

		// Token: 0x02000130 RID: 304
		private sealed class ValidatedNotNullAttribute : Attribute
		{
		}
	}
}
