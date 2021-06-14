using System;
using System.Collections;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000700 RID: 1792
	internal sealed class AccessorTable
	{
		// Token: 0x06007349 RID: 29513 RVA: 0x00211048 File Offset: 0x0020F248
		internal AccessorTable()
		{
		}

		// Token: 0x17001B5E RID: 7006
		internal AccessorInfo this[SourceValueType sourceValueType, Type type, string name]
		{
			get
			{
				if (type == null || name == null)
				{
					return null;
				}
				AccessorInfo accessorInfo = (AccessorInfo)this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)];
				if (accessorInfo != null)
				{
					accessorInfo.Generation = this._generation;
				}
				return accessorInfo;
			}
			set
			{
				if (type != null && name != null)
				{
					value.Generation = this._generation;
					this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)] = value;
					if (!this._cleanupRequested)
					{
						this.RequestCleanup();
					}
				}
			}
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x002110F6 File Offset: 0x0020F2F6
		private void RequestCleanup()
		{
			this._cleanupRequested = true;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x00211118 File Offset: 0x0020F318
		private object CleanupOperation(object arg)
		{
			object[] array = new object[this._table.Count];
			int num = 0;
			IDictionaryEnumerator enumerator = this._table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AccessorInfo accessorInfo = (AccessorInfo)enumerator.Value;
				int num2 = this._generation - accessorInfo.Generation;
				if (num2 >= 10)
				{
					array[num++] = enumerator.Key;
				}
			}
			for (int i = 0; i < num; i++)
			{
				this._table.Remove(array[i]);
			}
			this._generation++;
			this._cleanupRequested = false;
			return null;
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x00002137 File Offset: 0x00000337
		internal void PrintStats()
		{
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600734F RID: 29519 RVA: 0x002111B1 File Offset: 0x0020F3B1
		// (set) Token: 0x06007350 RID: 29520 RVA: 0x002111B9 File Offset: 0x0020F3B9
		internal bool TraceSize
		{
			get
			{
				return this._traceSize;
			}
			set
			{
				this._traceSize = value;
			}
		}

		// Token: 0x04003790 RID: 14224
		private const int AgeLimit = 10;

		// Token: 0x04003791 RID: 14225
		private Hashtable _table = new Hashtable();

		// Token: 0x04003792 RID: 14226
		private int _generation;

		// Token: 0x04003793 RID: 14227
		private bool _cleanupRequested;

		// Token: 0x04003794 RID: 14228
		private bool _traceSize;

		// Token: 0x02000B42 RID: 2882
		private struct AccessorTableKey
		{
			// Token: 0x06008D84 RID: 36228 RVA: 0x00259A44 File Offset: 0x00257C44
			public AccessorTableKey(SourceValueType sourceValueType, Type type, string name)
			{
				Invariant.Assert(type != null && type != null);
				this._sourceValueType = sourceValueType;
				this._type = type;
				this._name = name;
			}

			// Token: 0x06008D85 RID: 36229 RVA: 0x00259A73 File Offset: 0x00257C73
			public override bool Equals(object o)
			{
				return o is AccessorTable.AccessorTableKey && this == (AccessorTable.AccessorTableKey)o;
			}

			// Token: 0x06008D86 RID: 36230 RVA: 0x00259A90 File Offset: 0x00257C90
			public static bool operator ==(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return k1._sourceValueType == k2._sourceValueType && k1._type == k2._type && k1._name == k2._name;
			}

			// Token: 0x06008D87 RID: 36231 RVA: 0x00259AC6 File Offset: 0x00257CC6
			public static bool operator !=(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return !(k1 == k2);
			}

			// Token: 0x06008D88 RID: 36232 RVA: 0x00259AD2 File Offset: 0x00257CD2
			public override int GetHashCode()
			{
				return this._type.GetHashCode() + this._name.GetHashCode();
			}

			// Token: 0x04004AC0 RID: 19136
			private SourceValueType _sourceValueType;

			// Token: 0x04004AC1 RID: 19137
			private Type _type;

			// Token: 0x04004AC2 RID: 19138
			private string _name;
		}
	}
}
