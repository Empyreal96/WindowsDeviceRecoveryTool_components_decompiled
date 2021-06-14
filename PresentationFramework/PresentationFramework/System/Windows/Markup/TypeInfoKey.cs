using System;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001CC RID: 460
	internal struct TypeInfoKey
	{
		// Token: 0x06001D78 RID: 7544 RVA: 0x000896B8 File Offset: 0x000878B8
		public override bool Equals(object o)
		{
			if (!(o is TypeInfoKey))
			{
				return false;
			}
			TypeInfoKey typeInfoKey = (TypeInfoKey)o;
			if (!((typeInfoKey.DeclaringAssembly != null) ? typeInfoKey.DeclaringAssembly.Equals(this.DeclaringAssembly) : (this.DeclaringAssembly == null)))
			{
				return false;
			}
			if (typeInfoKey.TypeFullName == null)
			{
				return this.TypeFullName == null;
			}
			return typeInfoKey.TypeFullName.Equals(this.TypeFullName);
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00089721 File Offset: 0x00087921
		public static bool operator ==(TypeInfoKey key1, TypeInfoKey key2)
		{
			return key1.Equals(key2);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x00089736 File Offset: 0x00087936
		public static bool operator !=(TypeInfoKey key1, TypeInfoKey key2)
		{
			return !key1.Equals(key2);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0008974E File Offset: 0x0008794E
		public override int GetHashCode()
		{
			return ((this.DeclaringAssembly != null) ? this.DeclaringAssembly.GetHashCode() : 0) ^ ((this.TypeFullName != null) ? this.TypeFullName.GetHashCode() : 0);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x00089780 File Offset: 0x00087980
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("TypeInfoKey: Assembly=");
			stringBuilder.Append((this.DeclaringAssembly != null) ? this.DeclaringAssembly : "null");
			stringBuilder.Append(" Type=");
			stringBuilder.Append((this.TypeFullName != null) ? this.TypeFullName : "null");
			return stringBuilder.ToString();
		}

		// Token: 0x04001430 RID: 5168
		internal string DeclaringAssembly;

		// Token: 0x04001431 RID: 5169
		internal string TypeFullName;
	}
}
