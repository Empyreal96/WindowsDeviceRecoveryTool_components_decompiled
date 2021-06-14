using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A5 RID: 165
	public class DefaultSerializationBinder : SerializationBinder
	{
		// Token: 0x0600084B RID: 2123 RVA: 0x000203C8 File Offset: 0x0001E5C8
		private static Type GetTypeFromTypeNameKey(DefaultSerializationBinder.TypeNameKey typeNameKey)
		{
			string assemblyName = typeNameKey.AssemblyName;
			string typeName = typeNameKey.TypeName;
			if (assemblyName == null)
			{
				return Type.GetType(typeName);
			}
			Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
			if (assembly == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly2 in assemblies)
				{
					if (assembly2.FullName == assemblyName)
					{
						assembly = assembly2;
						break;
					}
				}
			}
			if (assembly == null)
			{
				throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, assemblyName));
			}
			Type type = assembly.GetType(typeName);
			if (type == null)
			{
				throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
			}
			return type;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002048E File Offset: 0x0001E68E
		public override Type BindToType(string assemblyName, string typeName)
		{
			return this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000204A2 File Offset: 0x0001E6A2
		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = serializedType.Assembly.FullName;
			typeName = serializedType.FullName;
		}

		// Token: 0x040002C6 RID: 710
		internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();

		// Token: 0x040002C7 RID: 711
		private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

		// Token: 0x020000A6 RID: 166
		internal struct TypeNameKey : IEquatable<DefaultSerializationBinder.TypeNameKey>
		{
			// Token: 0x06000850 RID: 2128 RVA: 0x000204E4 File Offset: 0x0001E6E4
			public TypeNameKey(string assemblyName, string typeName)
			{
				this.AssemblyName = assemblyName;
				this.TypeName = typeName;
			}

			// Token: 0x06000851 RID: 2129 RVA: 0x000204F4 File Offset: 0x0001E6F4
			public override int GetHashCode()
			{
				return ((this.AssemblyName != null) ? this.AssemblyName.GetHashCode() : 0) ^ ((this.TypeName != null) ? this.TypeName.GetHashCode() : 0);
			}

			// Token: 0x06000852 RID: 2130 RVA: 0x00020523 File Offset: 0x0001E723
			public override bool Equals(object obj)
			{
				return obj is DefaultSerializationBinder.TypeNameKey && this.Equals((DefaultSerializationBinder.TypeNameKey)obj);
			}

			// Token: 0x06000853 RID: 2131 RVA: 0x0002053B File Offset: 0x0001E73B
			public bool Equals(DefaultSerializationBinder.TypeNameKey other)
			{
				return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
			}

			// Token: 0x040002C8 RID: 712
			internal readonly string AssemblyName;

			// Token: 0x040002C9 RID: 713
			internal readonly string TypeName;
		}
	}
}
