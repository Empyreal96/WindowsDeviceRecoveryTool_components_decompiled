using System;
using System.Collections.Generic;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x02000174 RID: 372
	internal class WpfSharedXamlSchemaContext : WpfSharedBamlSchemaContext
	{
		// Token: 0x060015AB RID: 5547 RVA: 0x0006A445 File Offset: 0x00068645
		public WpfSharedXamlSchemaContext(XamlSchemaContextSettings settings, bool useV3Rules) : base(settings)
		{
			this._useV3Rules = useV3Rules;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0006A46C File Offset: 0x0006866C
		public override XamlType GetXamlType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					WpfSharedXamlSchemaContext.RequireRuntimeType(type);
					xamlType = base.CreateKnownBamlType(type.Name, false, this._useV3Rules);
					if (xamlType == null || xamlType.UnderlyingType != type)
					{
						xamlType = new WpfXamlType(type, this, false, this._useV3Rules);
					}
					this._masterTypeTable.Add(type, xamlType);
				}
			}
			return xamlType;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0006A518 File Offset: 0x00068718
		internal static void RequireRuntimeType(Type type)
		{
			Type type2 = typeof(object).GetType();
			if (!type2.IsAssignableFrom(type.GetType()))
			{
				throw new ArgumentException(SR.Get("RuntimeTypeRequired", new object[]
				{
					type
				}), "type");
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0006A562 File Offset: 0x00068762
		internal XamlType GetXamlTypeInternal(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x04001270 RID: 4720
		private Dictionary<Type, XamlType> _masterTypeTable = new Dictionary<Type, XamlType>();

		// Token: 0x04001271 RID: 4721
		private object _syncObject = new object();

		// Token: 0x04001272 RID: 4722
		private bool _useV3Rules;
	}
}
