using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace System.Resources
{
	// Token: 0x020000EC RID: 236
	internal class AssemblyNamesTypeResolutionService : ITypeResolutionService
	{
		// Token: 0x06000352 RID: 850 RVA: 0x00009F6C File Offset: 0x0000816C
		internal AssemblyNamesTypeResolutionService(AssemblyName[] names)
		{
			this.names = names;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00009F7B File Offset: 0x0000817B
		public Assembly GetAssembly(AssemblyName name)
		{
			return this.GetAssembly(name, true);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00009F88 File Offset: 0x00008188
		public Assembly GetAssembly(AssemblyName name, bool throwOnError)
		{
			Assembly assembly = null;
			if (this.cachedAssemblies == null)
			{
				this.cachedAssemblies = Hashtable.Synchronized(new Hashtable());
			}
			if (this.cachedAssemblies.Contains(name))
			{
				assembly = (this.cachedAssemblies[name] as Assembly);
			}
			if (assembly == null)
			{
				assembly = Assembly.LoadWithPartialName(name.FullName);
				if (assembly != null)
				{
					this.cachedAssemblies[name] = assembly;
				}
				else if (this.names != null)
				{
					for (int i = 0; i < this.names.Length; i++)
					{
						if (name.Equals(this.names[i]))
						{
							try
							{
								assembly = Assembly.LoadFrom(this.GetPathOfAssembly(name));
								if (assembly != null)
								{
									this.cachedAssemblies[name] = assembly;
								}
							}
							catch
							{
								if (throwOnError)
								{
									throw;
								}
							}
						}
					}
				}
			}
			return assembly;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000A068 File Offset: 0x00008268
		public string GetPathOfAssembly(AssemblyName name)
		{
			return name.CodeBase;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000A070 File Offset: 0x00008270
		public Type GetType(string name)
		{
			return this.GetType(name, true);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A07A File Offset: 0x0000827A
		public Type GetType(string name, bool throwOnError)
		{
			return this.GetType(name, throwOnError, false);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000A088 File Offset: 0x00008288
		public Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			Type type = null;
			if (this.cachedTypes == null)
			{
				this.cachedTypes = Hashtable.Synchronized(new Hashtable(StringComparer.Ordinal));
			}
			if (this.cachedTypes.Contains(name))
			{
				type = (this.cachedTypes[name] as Type);
				return type;
			}
			if (name.IndexOf(',') != -1)
			{
				type = Type.GetType(name, false, ignoreCase);
			}
			if (type == null && this.names != null)
			{
				int num = name.IndexOf(',');
				if (num > 0 && num < name.Length - 1)
				{
					string assemblyName = name.Substring(num + 1).Trim();
					AssemblyName assemblyName2 = null;
					try
					{
						assemblyName2 = new AssemblyName(assemblyName);
					}
					catch
					{
					}
					if (assemblyName2 != null)
					{
						List<AssemblyName> list = new List<AssemblyName>(this.names.Length);
						for (int i = 0; i < this.names.Length; i++)
						{
							if (string.Compare(assemblyName2.Name, this.names[i].Name, StringComparison.OrdinalIgnoreCase) == 0)
							{
								list.Insert(0, this.names[i]);
							}
							else
							{
								list.Add(this.names[i]);
							}
						}
						this.names = list.ToArray();
					}
				}
				for (int j = 0; j < this.names.Length; j++)
				{
					Assembly assembly = this.GetAssembly(this.names[j], false);
					if (assembly != null)
					{
						type = assembly.GetType(name, false, ignoreCase);
						if (type == null)
						{
							int num2 = name.IndexOf(",");
							if (num2 != -1)
							{
								string name2 = name.Substring(0, num2);
								type = assembly.GetType(name2, false, ignoreCase);
							}
						}
					}
					if (type != null)
					{
						break;
					}
				}
			}
			if (type == null && throwOnError)
			{
				throw new ArgumentException(SR.GetString("InvalidResXNoType", new object[]
				{
					name
				}));
			}
			if (type != null && (type.Assembly.GlobalAssemblyCache || this.IsNetFrameworkAssembly(type.Assembly.Location)))
			{
				this.cachedTypes[name] = type;
			}
			return type;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000A298 File Offset: 0x00008498
		private bool IsNetFrameworkAssembly(string assemblyPath)
		{
			return assemblyPath != null && assemblyPath.StartsWith(AssemblyNamesTypeResolutionService.NetFrameworkPath, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000A2AB File Offset: 0x000084AB
		public void ReferenceAssembly(AssemblyName name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040003BD RID: 957
		private AssemblyName[] names;

		// Token: 0x040003BE RID: 958
		private Hashtable cachedAssemblies;

		// Token: 0x040003BF RID: 959
		private Hashtable cachedTypes;

		// Token: 0x040003C0 RID: 960
		private static string NetFrameworkPath = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "Microsoft.Net\\Framework");
	}
}
