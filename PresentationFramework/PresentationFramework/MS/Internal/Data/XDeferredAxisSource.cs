using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MS.Internal.Data
{
	// Token: 0x0200074D RID: 1869
	internal sealed class XDeferredAxisSource
	{
		// Token: 0x06007727 RID: 30503 RVA: 0x00220D08 File Offset: 0x0021EF08
		internal XDeferredAxisSource(object component, PropertyDescriptor pd)
		{
			this._component = new WeakReference(component);
			this._propertyDescriptor = pd;
			this._table = new HybridDictionary();
		}

		// Token: 0x17001C4E RID: 7246
		public IEnumerable this[string name]
		{
			get
			{
				XDeferredAxisSource.Record record = (XDeferredAxisSource.Record)this._table[name];
				if (record == null)
				{
					object target = this._component.Target;
					if (target == null)
					{
						return null;
					}
					IEnumerable enumerable = this._propertyDescriptor.GetValue(target) as IEnumerable;
					if (enumerable != null && name != "%%FullCollection%%")
					{
						MemberInfo[] defaultMembers = enumerable.GetType().GetDefaultMembers();
						PropertyInfo propertyInfo = (defaultMembers.Length != 0) ? (defaultMembers[0] as PropertyInfo) : null;
						enumerable = ((propertyInfo == null) ? null : (propertyInfo.GetValue(enumerable, BindingFlags.GetProperty, null, new object[]
						{
							name
						}, CultureInfo.InvariantCulture) as IEnumerable));
					}
					record = new XDeferredAxisSource.Record(enumerable);
					this._table[name] = record;
				}
				else
				{
					record.DC.Update(record.XDA);
				}
				return record.Collection;
			}
		}

		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x06007729 RID: 30505 RVA: 0x00220E02 File Offset: 0x0021F002
		internal IEnumerable FullCollection
		{
			get
			{
				return this["%%FullCollection%%"];
			}
		}

		// Token: 0x040038AE RID: 14510
		private WeakReference _component;

		// Token: 0x040038AF RID: 14511
		private PropertyDescriptor _propertyDescriptor;

		// Token: 0x040038B0 RID: 14512
		private HybridDictionary _table;

		// Token: 0x040038B1 RID: 14513
		private const string FullCollectionKey = "%%FullCollection%%";

		// Token: 0x02000B68 RID: 2920
		private class Record
		{
			// Token: 0x06008E14 RID: 36372 RVA: 0x0025B3D9 File Offset: 0x002595D9
			public Record(IEnumerable xda)
			{
				this._xda = xda;
				if (xda != null)
				{
					this._dc = new DifferencingCollection(xda);
					this._rooc = new ReadOnlyObservableCollection<object>(this._dc);
				}
			}

			// Token: 0x17001F97 RID: 8087
			// (get) Token: 0x06008E15 RID: 36373 RVA: 0x0025B408 File Offset: 0x00259608
			public IEnumerable XDA
			{
				get
				{
					return this._xda;
				}
			}

			// Token: 0x17001F98 RID: 8088
			// (get) Token: 0x06008E16 RID: 36374 RVA: 0x0025B410 File Offset: 0x00259610
			public DifferencingCollection DC
			{
				get
				{
					return this._dc;
				}
			}

			// Token: 0x17001F99 RID: 8089
			// (get) Token: 0x06008E17 RID: 36375 RVA: 0x0025B418 File Offset: 0x00259618
			public ReadOnlyObservableCollection<object> Collection
			{
				get
				{
					return this._rooc;
				}
			}

			// Token: 0x04004B39 RID: 19257
			private IEnumerable _xda;

			// Token: 0x04004B3A RID: 19258
			private DifferencingCollection _dc;

			// Token: 0x04004B3B RID: 19259
			private ReadOnlyObservableCollection<object> _rooc;
		}
	}
}
