using System;

namespace System.Windows
{
	// Token: 0x02000118 RID: 280
	internal class DeferredThemeResourceReference : DeferredResourceReference
	{
		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002B010 File Offset: 0x00029210
		internal DeferredThemeResourceReference(ResourceDictionary dictionary, object resourceKey, bool canCacheAsThemeResource) : base(dictionary, resourceKey)
		{
			this._canCacheAsThemeResource = canCacheAsThemeResource;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002B024 File Offset: 0x00029224
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			object result;
			lock (themeDictionaryLock)
			{
				if (base.Dictionary != null)
				{
					object key = this.Key;
					SystemResources.IsSystemResourcesParsing = true;
					bool flag2;
					object value;
					try
					{
						value = base.Dictionary.GetValue(key, out flag2);
						if (flag2)
						{
							this.Value = value;
							base.Dictionary = null;
						}
					}
					finally
					{
						SystemResources.IsSystemResourcesParsing = false;
					}
					if ((key is Type || key is ResourceKey) && this._canCacheAsThemeResource && flag2)
					{
						SystemResources.CacheResource(key, value, false);
					}
					result = value;
				}
				else
				{
					result = this.Value;
				}
			}
			return result;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0002B0E0 File Offset: 0x000292E0
		internal override Type GetValueType()
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			Type valueType;
			lock (themeDictionaryLock)
			{
				valueType = base.GetValueType();
			}
			return valueType;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00002137 File Offset: 0x00000337
		internal override void RemoveFromDictionary()
		{
		}

		// Token: 0x04000AB5 RID: 2741
		private bool _canCacheAsThemeResource;
	}
}
