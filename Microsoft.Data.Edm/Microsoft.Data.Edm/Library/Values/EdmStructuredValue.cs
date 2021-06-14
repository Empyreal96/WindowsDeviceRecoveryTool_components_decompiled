using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x020001DD RID: 477
	public class EdmStructuredValue : EdmValue, IEdmStructuredValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x00020D38 File Offset: 0x0001EF38
		public EdmStructuredValue(IEdmStructuredTypeReference type, IEnumerable<IEdmPropertyValue> propertyValues) : base(type)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<IEdmPropertyValue>>(propertyValues, "propertyValues");
			this.propertyValues = propertyValues;
			if (propertyValues != null)
			{
				int num = 0;
				foreach (IEdmPropertyValue edmPropertyValue in propertyValues)
				{
					num++;
					if (num > 5)
					{
						this.propertiesDictionaryCache = new Cache<EdmStructuredValue, Dictionary<string, IEdmPropertyValue>>();
						break;
					}
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00020DB0 File Offset: 0x0001EFB0
		public IEnumerable<IEdmPropertyValue> PropertyValues
		{
			get
			{
				return this.propertyValues;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00020DB8 File Offset: 0x0001EFB8
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Structured;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00020DBC File Offset: 0x0001EFBC
		private Dictionary<string, IEdmPropertyValue> PropertiesDictionary
		{
			get
			{
				if (this.propertiesDictionaryCache != null)
				{
					return this.propertiesDictionaryCache.GetValue(this, EdmStructuredValue.ComputePropertiesDictionaryFunc, null);
				}
				return null;
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00020DDC File Offset: 0x0001EFDC
		public IEdmPropertyValue FindPropertyValue(string propertyName)
		{
			Dictionary<string, IEdmPropertyValue> propertiesDictionary = this.PropertiesDictionary;
			if (propertiesDictionary != null)
			{
				IEdmPropertyValue result;
				propertiesDictionary.TryGetValue(propertyName, out result);
				return result;
			}
			foreach (IEdmPropertyValue edmPropertyValue in this.propertyValues)
			{
				if (edmPropertyValue.Name == propertyName)
				{
					return edmPropertyValue;
				}
			}
			return null;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00020E54 File Offset: 0x0001F054
		private Dictionary<string, IEdmPropertyValue> ComputePropertiesDictionary()
		{
			Dictionary<string, IEdmPropertyValue> dictionary = new Dictionary<string, IEdmPropertyValue>();
			foreach (IEdmPropertyValue edmPropertyValue in this.propertyValues)
			{
				dictionary[edmPropertyValue.Name] = edmPropertyValue;
			}
			return dictionary;
		}

		// Token: 0x0400054D RID: 1357
		private readonly IEnumerable<IEdmPropertyValue> propertyValues;

		// Token: 0x0400054E RID: 1358
		private readonly Cache<EdmStructuredValue, Dictionary<string, IEdmPropertyValue>> propertiesDictionaryCache;

		// Token: 0x0400054F RID: 1359
		private static readonly Func<EdmStructuredValue, Dictionary<string, IEdmPropertyValue>> ComputePropertiesDictionaryFunc = (EdmStructuredValue me) => me.ComputePropertiesDictionary();
	}
}
