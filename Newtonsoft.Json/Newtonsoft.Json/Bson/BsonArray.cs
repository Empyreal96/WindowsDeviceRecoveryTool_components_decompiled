using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000C RID: 12
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x0600007F RID: 127 RVA: 0x0000442A File Offset: 0x0000262A
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000443F File Offset: 0x0000263F
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004442 File Offset: 0x00002642
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004454 File Offset: 0x00002654
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400004B RID: 75
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
