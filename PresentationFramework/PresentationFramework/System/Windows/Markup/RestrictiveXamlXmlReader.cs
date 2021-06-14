using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Xaml;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x02000229 RID: 553
	internal class RestrictiveXamlXmlReader : XamlXmlReader
	{
		// Token: 0x06002237 RID: 8759 RVA: 0x000AA464 File Offset: 0x000A8664
		static RestrictiveXamlXmlReader()
		{
			RestrictiveXamlXmlReader._unloadedTypes = new ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>>();
			foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
			{
				if (!string.IsNullOrEmpty(restrictedType.AssemblyName))
				{
					if (!RestrictiveXamlXmlReader._unloadedTypes.ContainsKey(restrictedType.AssemblyName))
					{
						RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName] = new List<RestrictiveXamlXmlReader.RestrictedType>();
					}
					RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName].Add(restrictedType);
				}
				else
				{
					Type type = System.Type.GetType(restrictedType.TypeName, false);
					if (type != null)
					{
						restrictedType.TypeReference = type;
					}
				}
			}
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000AA59C File Offset: 0x000A879C
		public RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings) : base(xmlReader, schemaContext, settings)
		{
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000AA5B4 File Offset: 0x000A87B4
		internal RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings, List<Type> safeTypes) : base(xmlReader, schemaContext, settings)
		{
			if (safeTypes != null)
			{
				foreach (Type item in safeTypes)
				{
					this._safeTypesSet.Add(item);
				}
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000AA624 File Offset: 0x000A8824
		public override bool Read()
		{
			int num = 0;
			bool result;
			while (result = base.Read())
			{
				if (num <= 0)
				{
					if (this.NodeType != XamlNodeType.StartObject || !this.IsRestrictedType(this.Type.UnderlyingType))
					{
						break;
					}
					num = 1;
				}
				else if (this.NodeType == XamlNodeType.StartObject || this.NodeType == XamlNodeType.GetObject)
				{
					num++;
				}
				else if (this.NodeType == XamlNodeType.EndObject)
				{
					num--;
				}
			}
			return result;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000AA68C File Offset: 0x000A888C
		private bool IsRestrictedType(Type type)
		{
			if (type != null)
			{
				if (this._safeTypesSet.Contains(type))
				{
					return false;
				}
				RestrictiveXamlXmlReader.EnsureLatestAssemblyLoadInformation();
				foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
				{
					Type typeReference = restrictedType.TypeReference;
					if (typeReference != null && typeReference.IsAssignableFrom(type))
					{
						return true;
					}
				}
				this._safeTypesSet.Add(type);
				return false;
			}
			return false;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000AA720 File Offset: 0x000A8920
		private static void EnsureLatestAssemblyLoadInformation()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies.Length != RestrictiveXamlXmlReader._loadedAssembliesCount)
			{
				foreach (Assembly assembly in assemblies)
				{
					RestrictiveXamlXmlReader.RegisterAssembly(assembly);
				}
				RestrictiveXamlXmlReader._loadedAssembliesCount = assemblies.Length;
			}
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000AA764 File Offset: 0x000A8964
		private static void RegisterAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				string fullName = assembly.FullName;
				List<RestrictiveXamlXmlReader.RestrictedType> list = null;
				if (RestrictiveXamlXmlReader._unloadedTypes.TryGetValue(fullName, out list))
				{
					if (list != null)
					{
						foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in list)
						{
							Type type = assembly.GetType(restrictedType.TypeName, false);
							restrictedType.TypeReference = type;
						}
					}
					RestrictiveXamlXmlReader._unloadedTypes.TryRemove(fullName, out list);
				}
			}
		}

		// Token: 0x040019D8 RID: 6616
		private static List<RestrictiveXamlXmlReader.RestrictedType> _restrictedTypes = new List<RestrictiveXamlXmlReader.RestrictedType>
		{
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Data.ObjectDataProvider", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.ResourceDictionary", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Configuration.Install.AssemblyInstaller", "System.Configuration.Install, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Activities.Presentation.WorkflowDesigner", "System.Activities.Presentation, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 31bf3856ad364e35"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Forms.BindingSource", "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
		};

		// Token: 0x040019D9 RID: 6617
		private static ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>> _unloadedTypes = null;

		// Token: 0x040019DA RID: 6618
		private HashSet<Type> _safeTypesSet = new HashSet<Type>();

		// Token: 0x040019DB RID: 6619
		[ThreadStatic]
		private static int _loadedAssembliesCount;

		// Token: 0x02000897 RID: 2199
		private class RestrictedType
		{
			// Token: 0x0600839A RID: 33690 RVA: 0x00245D3A File Offset: 0x00243F3A
			public RestrictedType(string typeName, string assemblyName)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x17001DD6 RID: 7638
			// (get) Token: 0x0600839B RID: 33691 RVA: 0x00245D50 File Offset: 0x00243F50
			// (set) Token: 0x0600839C RID: 33692 RVA: 0x00245D58 File Offset: 0x00243F58
			public string TypeName { get; set; }

			// Token: 0x17001DD7 RID: 7639
			// (get) Token: 0x0600839D RID: 33693 RVA: 0x00245D61 File Offset: 0x00243F61
			// (set) Token: 0x0600839E RID: 33694 RVA: 0x00245D69 File Offset: 0x00243F69
			public string AssemblyName { get; set; }

			// Token: 0x17001DD8 RID: 7640
			// (get) Token: 0x0600839F RID: 33695 RVA: 0x00245D72 File Offset: 0x00243F72
			// (set) Token: 0x060083A0 RID: 33696 RVA: 0x00245D7A File Offset: 0x00243F7A
			public Type TypeReference { get; set; }
		}
	}
}
