using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000B RID: 11
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x0600007A RID: 122 RVA: 0x000043C0 File Offset: 0x000025C0
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000043FA File Offset: 0x000025FA
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000043FD File Offset: 0x000025FD
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000440F File Offset: 0x0000260F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400004A RID: 74
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
