using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace System.Data.Services.Client.Metadata
{
	// Token: 0x02000134 RID: 308
	[DebuggerDisplay("{PropertyName}")]
	internal static class ClientTypeCache
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x0002C25C File Offset: 0x0002A45C
		internal static Type ResolveFromName(string wireName, Type userType)
		{
			ClientTypeCache.TypeName key;
			key.Type = userType;
			key.Name = wireName;
			Type type;
			bool flag2;
			lock (ClientTypeCache.namedTypes)
			{
				flag2 = ClientTypeCache.namedTypes.TryGetValue(key, out type);
			}
			if (!flag2)
			{
				string text = wireName;
				int num = wireName.LastIndexOf('.');
				if (0 <= num && num < wireName.Length - 1)
				{
					text = wireName.Substring(num + 1);
				}
				if (userType.Name == text)
				{
					type = userType;
				}
				else
				{
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
					{
						Type type2 = assembly.GetType(wireName, false);
						ClientTypeCache.ResolveSubclass(text, userType, type2, ref type);
						if (null == type2)
						{
							IEnumerable<Type> enumerable = null;
							try
							{
								enumerable = assembly.GetTypes();
							}
							catch (ReflectionTypeLoadException)
							{
							}
							if (enumerable != null)
							{
								foreach (Type type3 in enumerable)
								{
									ClientTypeCache.ResolveSubclass(text, userType, type3, ref type);
								}
							}
						}
					}
				}
				lock (ClientTypeCache.namedTypes)
				{
					ClientTypeCache.namedTypes[key] = type;
				}
			}
			return type;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002C3DC File Offset: 0x0002A5DC
		private static void ResolveSubclass(string wireClassName, Type userType, Type type, ref Type existing)
		{
			if (null != type && type.IsVisible() && wireClassName == type.Name && userType.IsAssignableFrom(type))
			{
				if (null != existing)
				{
					throw Error.InvalidOperation(Strings.ClientType_Ambiguous(wireClassName, userType));
				}
				existing = type;
			}
		}

		// Token: 0x04000601 RID: 1537
		private static readonly Dictionary<ClientTypeCache.TypeName, Type> namedTypes = new Dictionary<ClientTypeCache.TypeName, Type>(new ClientTypeCache.TypeNameEqualityComparer());

		// Token: 0x02000135 RID: 309
		private struct TypeName
		{
			// Token: 0x04000602 RID: 1538
			internal Type Type;

			// Token: 0x04000603 RID: 1539
			internal string Name;
		}

		// Token: 0x02000136 RID: 310
		private sealed class TypeNameEqualityComparer : IEqualityComparer<ClientTypeCache.TypeName>
		{
			// Token: 0x06000B11 RID: 2833 RVA: 0x0002C43C File Offset: 0x0002A63C
			public bool Equals(ClientTypeCache.TypeName x, ClientTypeCache.TypeName y)
			{
				return x.Type == y.Type && x.Name == y.Name;
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x0002C468 File Offset: 0x0002A668
			public int GetHashCode(ClientTypeCache.TypeName obj)
			{
				return obj.Type.GetHashCode() ^ obj.Name.GetHashCode();
			}
		}
	}
}
