using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200000B RID: 11
	internal sealed class GeoJsonObjectWriter : GeoJsonWriterBase
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002A0A File Offset: 0x00000C0A
		internal IDictionary<string, object> JsonObject
		{
			get
			{
				return this.lastCompletedObject as IDictionary<string, object>;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002A17 File Offset: 0x00000C17
		private bool IsArray
		{
			get
			{
				return this.containers.Peek() is IList;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002A2C File Offset: 0x00000C2C
		protected override void StartObjectScope()
		{
			object obj = new Dictionary<string, object>(StringComparer.Ordinal);
			if (this.containers.Count > 0)
			{
				this.AddToScope(obj);
			}
			this.containers.Push(obj);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002A68 File Offset: 0x00000C68
		protected override void StartArrayScope()
		{
			object obj = new List<object>();
			this.AddToScope(obj);
			this.containers.Push(obj);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002A8E File Offset: 0x00000C8E
		protected override void AddPropertyName(string name)
		{
			this.currentPropertyName = name;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002A97 File Offset: 0x00000C97
		protected override void AddValue(string value)
		{
			this.AddToScope(value);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002AA0 File Offset: 0x00000CA0
		protected override void AddValue(double value)
		{
			this.AddToScope(value);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002AAE File Offset: 0x00000CAE
		protected override void EndArrayScope()
		{
			this.containers.Pop();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002ABC File Offset: 0x00000CBC
		protected override void EndObjectScope()
		{
			object obj = this.containers.Pop();
			if (this.containers.Count == 0)
			{
				this.lastCompletedObject = obj;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002AEC File Offset: 0x00000CEC
		private void AddToScope(object jsonObject)
		{
			if (this.IsArray)
			{
				this.AsList().Add(jsonObject);
				return;
			}
			string andClearCurrentPropertyName = this.GetAndClearCurrentPropertyName();
			this.AsDictionary().Add(andClearCurrentPropertyName, jsonObject);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002B24 File Offset: 0x00000D24
		private string GetAndClearCurrentPropertyName()
		{
			string result = this.currentPropertyName;
			this.currentPropertyName = null;
			return result;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002B40 File Offset: 0x00000D40
		private IList AsList()
		{
			return this.containers.Peek() as IList;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002B60 File Offset: 0x00000D60
		private IDictionary<string, object> AsDictionary()
		{
			return this.containers.Peek() as IDictionary<string, object>;
		}

		// Token: 0x0400000C RID: 12
		private readonly Stack<object> containers = new Stack<object>();

		// Token: 0x0400000D RID: 13
		private string currentPropertyName;

		// Token: 0x0400000E RID: 14
		private object lastCompletedObject;
	}
}
