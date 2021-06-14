using System;
using System.Diagnostics;
using System.Reflection;

namespace System.Windows.Markup
{
	/// <summary>Provides information that the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> uses for mapping between an XML namespace, a CLR namespace, and the assembly that contains the relevant types for that CLR namespace.</summary>
	// Token: 0x0200021E RID: 542
	[DebuggerDisplay("'{_xmlNamespace}'={_clrNamespace}:{_assemblyName}")]
	public class NamespaceMapEntry
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.NamespaceMapEntry" /> class. </summary>
		// Token: 0x06002199 RID: 8601 RVA: 0x0000326D File Offset: 0x0000146D
		public NamespaceMapEntry()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.NamespaceMapEntry" /> class, using provided XML namespace, CLR namespace, and assembly information. </summary>
		/// <param name="xmlNamespace">The mapping prefix for the XML namespace.</param>
		/// <param name="assemblyName">The assembly that contains the CLR namespace and types to map to the XML namespace.</param>
		/// <param name="clrNamespace">The CLR  namespace in the assembly that contains the relevant types.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlNamespace" /> is <see langword="null" />-or- 
		///         <paramref name="assemblyName" /> is <see langword="null" />-or- 
		///         <paramref name="clrNamespace" /> is <see langword="null" />.</exception>
		// Token: 0x0600219A RID: 8602 RVA: 0x000A7B8C File Offset: 0x000A5D8C
		public NamespaceMapEntry(string xmlNamespace, string assemblyName, string clrNamespace)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (clrNamespace == null)
			{
				throw new ArgumentNullException("clrNamespace");
			}
			this._xmlNamespace = xmlNamespace;
			this._assemblyName = assemblyName;
			this._clrNamespace = clrNamespace;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000A7BDE File Offset: 0x000A5DDE
		internal NamespaceMapEntry(string xmlNamespace, string assemblyName, string clrNamespace, string assemblyPath) : this(xmlNamespace, assemblyName, clrNamespace)
		{
			this._assemblyPath = assemblyPath;
		}

		/// <summary>Gets or sets the mapping prefix for the XML namespace being mapped to. </summary>
		/// <returns>The mapping prefix for the XML namespace.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value <see cref="P:System.Windows.Markup.NamespaceMapEntry.XmlNamespace" /> is being set to is <see langword="null" />.</exception>
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x000A7BF1 File Offset: 0x000A5DF1
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x000A7BF9 File Offset: 0x000A5DF9
		public string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._xmlNamespace == null)
				{
					this._xmlNamespace = value;
				}
			}
		}

		/// <summary>Gets or sets the assembly name that contains the types in the CLR namespace. </summary>
		/// <returns>The assembly name.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value <see cref="P:System.Windows.Markup.NamespaceMapEntry.AssemblyName" /> is being set to is <see langword="null" />.</exception>
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x000A7C18 File Offset: 0x000A5E18
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x000A7C20 File Offset: 0x000A5E20
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._assemblyName == null)
				{
					this._assemblyName = value;
				}
			}
		}

		/// <summary>Gets or sets the CLR namespace that contains the types being mapped. </summary>
		/// <returns>The CLR namespace.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value <see cref="P:System.Windows.Markup.NamespaceMapEntry.ClrNamespace" /> is being set to is <see langword="null" />.</exception>
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x000A7C3F File Offset: 0x000A5E3F
		// (set) Token: 0x060021A1 RID: 8609 RVA: 0x000A7C47 File Offset: 0x000A5E47
		public string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._clrNamespace == null)
				{
					this._clrNamespace = value;
				}
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x000A7C66 File Offset: 0x000A5E66
		internal Assembly Assembly
		{
			get
			{
				if (null == this._assembly && this._assemblyName.Length > 0)
				{
					this._assembly = ReflectionHelper.LoadAssembly(this._assemblyName, this._assemblyPath);
				}
				return this._assembly;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000A7CA1 File Offset: 0x000A5EA1
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x000A7CA9 File Offset: 0x000A5EA9
		internal string AssemblyPath
		{
			get
			{
				return this._assemblyPath;
			}
			set
			{
				this._assemblyPath = value;
			}
		}

		// Token: 0x040019A5 RID: 6565
		private string _xmlNamespace;

		// Token: 0x040019A6 RID: 6566
		private string _assemblyName;

		// Token: 0x040019A7 RID: 6567
		private string _assemblyPath;

		// Token: 0x040019A8 RID: 6568
		private Assembly _assembly;

		// Token: 0x040019A9 RID: 6569
		private string _clrNamespace;
	}
}
