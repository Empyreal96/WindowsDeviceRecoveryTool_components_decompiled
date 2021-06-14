using System;
using System.Collections.Generic;
using System.Xaml;
using MS.Internal.Xaml.Context;

namespace System.Windows.Baml2006
{
	// Token: 0x02000163 RID: 355
	internal class Baml2006ReaderFrame : XamlFrame
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x00041645 File Offset: 0x0003F845
		public Baml2006ReaderFrame()
		{
			this.DelayedConnectionId = -1;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00041654 File Offset: 0x0003F854
		public Baml2006ReaderFrame(Baml2006ReaderFrame source)
		{
			this.XamlType = source.XamlType;
			this.Member = source.Member;
			if (source._namespaces != null)
			{
				this._namespaces = new Dictionary<string, string>(source._namespaces);
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0004168D File Offset: 0x0003F88D
		public override XamlFrame Clone()
		{
			return new Baml2006ReaderFrame(this);
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x00041695 File Offset: 0x0003F895
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x0004169D File Offset: 0x0003F89D
		public XamlType XamlType { get; set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x000416A6 File Offset: 0x0003F8A6
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x000416AE File Offset: 0x0003F8AE
		public XamlMember Member { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x000416B7 File Offset: 0x0003F8B7
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x000416BF File Offset: 0x0003F8BF
		public KeyRecord Key { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x000416C8 File Offset: 0x0003F8C8
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x000416D0 File Offset: 0x0003F8D0
		public int DelayedConnectionId { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x000416D9 File Offset: 0x0003F8D9
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x000416E1 File Offset: 0x0003F8E1
		public XamlMember ContentProperty { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x000416EA File Offset: 0x0003F8EA
		// (set) Token: 0x06001063 RID: 4195 RVA: 0x000416F2 File Offset: 0x0003F8F2
		public bool FreezeFreezables { get; set; }

		// Token: 0x06001064 RID: 4196 RVA: 0x000416FB File Offset: 0x0003F8FB
		public void AddNamespace(string prefix, string xamlNs)
		{
			if (this._namespaces == null)
			{
				this._namespaces = new Dictionary<string, string>();
			}
			this._namespaces.Add(prefix, xamlNs);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0004171D File Offset: 0x0003F91D
		public void SetNamespaces(Dictionary<string, string> namespaces)
		{
			this._namespaces = namespaces;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00041726 File Offset: 0x0003F926
		public bool TryGetNamespaceByPrefix(string prefix, out string xamlNs)
		{
			if (this._namespaces != null && this._namespaces.TryGetValue(prefix, out xamlNs))
			{
				return true;
			}
			xamlNs = null;
			return false;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00041748 File Offset: 0x0003F948
		public bool TryGetPrefixByNamespace(string xamlNs, out string prefix)
		{
			if (this._namespaces != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this._namespaces)
				{
					if (keyValuePair.Value == xamlNs)
					{
						prefix = keyValuePair.Key;
						return true;
					}
				}
			}
			prefix = null;
			return false;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x000417C0 File Offset: 0x0003F9C0
		public override void Reset()
		{
			this.XamlType = null;
			this.Member = null;
			if (this._namespaces != null)
			{
				this._namespaces.Clear();
			}
			this.Flags = Baml2006ReaderFrameFlags.None;
			this.IsDeferredContent = false;
			this.Key = null;
			this.DelayedConnectionId = -1;
			this.ContentProperty = null;
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00041811 File Offset: 0x0003FA11
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x00041819 File Offset: 0x0003FA19
		public Baml2006ReaderFrameFlags Flags { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00041822 File Offset: 0x0003FA22
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0004182A File Offset: 0x0003FA2A
		public bool IsDeferredContent { get; set; }

		// Token: 0x040011DF RID: 4575
		protected Dictionary<string, string> _namespaces;
	}
}
