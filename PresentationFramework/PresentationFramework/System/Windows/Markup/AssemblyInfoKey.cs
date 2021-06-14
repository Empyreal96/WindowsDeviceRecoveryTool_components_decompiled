using System;

namespace System.Windows.Markup
{
	// Token: 0x020001CB RID: 459
	internal struct AssemblyInfoKey
	{
		// Token: 0x06001D73 RID: 7539 RVA: 0x00089628 File Offset: 0x00087828
		public override bool Equals(object o)
		{
			if (!(o is AssemblyInfoKey))
			{
				return false;
			}
			AssemblyInfoKey assemblyInfoKey = (AssemblyInfoKey)o;
			if (assemblyInfoKey.AssemblyFullName == null)
			{
				return this.AssemblyFullName == null;
			}
			return assemblyInfoKey.AssemblyFullName.Equals(this.AssemblyFullName);
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x00089669 File Offset: 0x00087869
		public static bool operator ==(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return key1.Equals(key2);
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x0008967E File Offset: 0x0008787E
		public static bool operator !=(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return !key1.Equals(key2);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00089696 File Offset: 0x00087896
		public override int GetHashCode()
		{
			if (this.AssemblyFullName == null)
			{
				return 0;
			}
			return this.AssemblyFullName.GetHashCode();
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000896AD File Offset: 0x000878AD
		public override string ToString()
		{
			return this.AssemblyFullName;
		}

		// Token: 0x0400142F RID: 5167
		internal string AssemblyFullName;
	}
}
