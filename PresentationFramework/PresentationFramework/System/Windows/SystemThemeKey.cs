using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x0200011A RID: 282
	[TypeConverter(typeof(SystemKeyConverter))]
	internal class SystemThemeKey : ResourceKey
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x0002B1A8 File Offset: 0x000293A8
		internal SystemThemeKey(SystemResourceKeyID id)
		{
			this._id = id;
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0002B1B7 File Offset: 0x000293B7
		public override Assembly Assembly
		{
			get
			{
				if (SystemThemeKey._presentationFrameworkAssembly == null)
				{
					SystemThemeKey._presentationFrameworkAssembly = typeof(FrameworkElement).Assembly;
				}
				return SystemThemeKey._presentationFrameworkAssembly;
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002B1E0 File Offset: 0x000293E0
		public override bool Equals(object o)
		{
			SystemThemeKey systemThemeKey = o as SystemThemeKey;
			return systemThemeKey != null && systemThemeKey._id == this._id;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002B207 File Offset: 0x00029407
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002B20F File Offset: 0x0002940F
		public override string ToString()
		{
			return this._id.ToString();
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0002B207 File Offset: 0x00029407
		internal SystemResourceKeyID InternalKey
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x04000AB6 RID: 2742
		private SystemResourceKeyID _id;

		// Token: 0x04000AB7 RID: 2743
		private static Assembly _presentationFrameworkAssembly;
	}
}
