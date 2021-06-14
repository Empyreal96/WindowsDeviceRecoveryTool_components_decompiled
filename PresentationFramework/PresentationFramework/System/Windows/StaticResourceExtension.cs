using System;
using System.Collections.Generic;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Xaml;

namespace System.Windows
{
	/// <summary>Implements a markup extension that supports static (XAML load time) resource references made from XAML. </summary>
	// Token: 0x020000EB RID: 235
	[MarkupExtensionReturnType(typeof(object))]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class StaticResourceExtension : MarkupExtension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.StaticResourceExtension" /> class.</summary>
		// Token: 0x0600085B RID: 2139 RVA: 0x0000B03E File Offset: 0x0000923E
		public StaticResourceExtension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.StaticResourceExtension" /> class, with the provided initial key.</summary>
		/// <param name="resourceKey">The key of the resource that this markup extension references.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="resourceKey" /> parameter is <see langword="null" />, either through markup extension usage or explicit construction.</exception>
		// Token: 0x0600085C RID: 2140 RVA: 0x0001B317 File Offset: 0x00019517
		public StaticResourceExtension(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			this._resourceKey = resourceKey;
		}

		/// <summary>Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.StaticResourceExtension" />, this is the object found in a resource dictionary, where the object to find is identified by the <see cref="P:System.Windows.StaticResourceExtension.ResourceKey" />.</summary>
		/// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
		/// <returns>The object value to set on the property where the markup extension provided value is evaluated.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="serviceProvider" /> was <see langword="null" />, or failed to implement a required service.</exception>
		// Token: 0x0600085D RID: 2141 RVA: 0x0001B334 File Offset: 0x00019534
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.ResourceKey is SystemResourceKey)
			{
				return (this.ResourceKey as SystemResourceKey).Resource;
			}
			return this.ProvideValueInternal(serviceProvider, false);
		}

		/// <summary>Gets or sets the key value passed by this static resource reference. They key is used  to return the object matching that key in resource dictionaries. </summary>
		/// <returns>The resource key for a resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">Specified value as <see langword="null" />, either through markup extension usage or explicit construction.</exception>
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0001B35C File Offset: 0x0001955C
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0001B364 File Offset: 0x00019564
		[ConstructorArgument("resourceKey")]
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._resourceKey = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual DeferredResourceReference PrefetchedValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001B37C File Offset: 0x0001957C
		internal object ProvideValueInternal(IServiceProvider serviceProvider, bool allowDeferredReference)
		{
			object obj = this.TryProvideValueInternal(serviceProvider, allowDeferredReference, false);
			if (obj == DependencyProperty.UnsetValue)
			{
				throw new Exception(SR.Get("ParserNoResource", new object[]
				{
					this.ResourceKey.ToString()
				}));
			}
			return obj;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001B3C0 File Offset: 0x000195C0
		internal object TryProvideValueInternal(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			if (!ResourceDictionaryDiagnostics.HasStaticResourceResolvedListeners)
			{
				return this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			return this.TryProvideValueWithDiagnosticEvent(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001B3DC File Offset: 0x000195DC
		private object TryProvideValueWithDiagnosticEvent(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null || provideValueTarget.TargetObject == null || provideValueTarget.TargetProperty == null || ResourceDictionaryDiagnostics.ShouldIgnoreProperty(provideValueTarget.TargetProperty))
			{
				return this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			bool flag = false;
			ResourceDictionaryDiagnostics.LookupResult result;
			object obj;
			try
			{
				result = ResourceDictionaryDiagnostics.RequestLookupResult(this);
				obj = this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
				DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
				if (deferredResourceReference != null)
				{
					flag = true;
					ResourceDictionary dictionary = deferredResourceReference.Dictionary;
					if (dictionary != null)
					{
						ResourceDictionaryDiagnostics.RecordLookupResult(this.ResourceKey, dictionary);
					}
				}
				else
				{
					flag = (obj != DependencyProperty.UnsetValue);
				}
			}
			finally
			{
				ResourceDictionaryDiagnostics.RevertRequest(this, flag);
			}
			if (flag)
			{
				ResourceDictionaryDiagnostics.OnStaticResourceResolved(provideValueTarget.TargetObject, provideValueTarget.TargetProperty, result);
			}
			return obj;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001B4A4 File Offset: 0x000196A4
		private object TryProvideValueImpl(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			DeferredResourceReference prefetchedValue = this.PrefetchedValue;
			object obj;
			if (prefetchedValue == null)
			{
				obj = this.FindResourceInEnviroment(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			else
			{
				obj = this.FindResourceInDeferredContent(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = (allowDeferredReference ? prefetchedValue : prefetchedValue.GetValue(BaseValueSourceInternal.Unknown));
				}
			}
			return obj;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001B4E8 File Offset: 0x000196E8
		private ResourceDictionary FindTheResourceDictionary(IServiceProvider serviceProvider, bool isDeferredContentSearch)
		{
			IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
			if (xamlSchemaContextProvider == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IXamlSchemaContextProvider"
				}));
			}
			IAmbientProvider ambientProvider = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
			if (ambientProvider == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IAmbientProvider"
				}));
			}
			XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
			XamlType xamlType = schemaContext.GetXamlType(typeof(FrameworkElement));
			XamlType xamlType2 = schemaContext.GetXamlType(typeof(Style));
			XamlType xamlType3 = schemaContext.GetXamlType(typeof(FrameworkTemplate));
			XamlType xamlType4 = schemaContext.GetXamlType(typeof(Application));
			XamlType xamlType5 = schemaContext.GetXamlType(typeof(FrameworkContentElement));
			XamlMember member = xamlType5.GetMember("Resources");
			XamlMember member2 = xamlType.GetMember("Resources");
			XamlMember member3 = xamlType2.GetMember("Resources");
			XamlMember member4 = xamlType2.GetMember("BasedOn");
			XamlMember member5 = xamlType3.GetMember("Resources");
			XamlMember member6 = xamlType4.GetMember("Resources");
			XamlType[] types = new XamlType[]
			{
				schemaContext.GetXamlType(typeof(ResourceDictionary))
			};
			IEnumerable<AmbientPropertyValue> allAmbientValues = ambientProvider.GetAllAmbientValues(null, isDeferredContentSearch, types, new XamlMember[]
			{
				member,
				member2,
				member3,
				member4,
				member5,
				member6
			});
			List<AmbientPropertyValue> list = allAmbientValues as List<AmbientPropertyValue>;
			for (int i = 0; i < list.Count; i++)
			{
				AmbientPropertyValue ambientPropertyValue = list[i];
				if (ambientPropertyValue.Value is ResourceDictionary)
				{
					ResourceDictionary resourceDictionary = (ResourceDictionary)ambientPropertyValue.Value;
					if (resourceDictionary.Contains(this.ResourceKey))
					{
						return resourceDictionary;
					}
				}
				if (ambientPropertyValue.Value is Style)
				{
					Style style = (Style)ambientPropertyValue.Value;
					ResourceDictionary resourceDictionary2 = style.FindResourceDictionary(this.ResourceKey);
					if (resourceDictionary2 != null)
					{
						return resourceDictionary2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001B70C File Offset: 0x0001990C
		internal object FindResourceInDeferredContent(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary = this.FindTheResourceDictionary(serviceProvider, true);
			object obj = DependencyProperty.UnsetValue;
			if (resourceDictionary != null)
			{
				obj = resourceDictionary.Lookup(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference, false);
			}
			if (mustReturnDeferredResourceReference && obj == DependencyProperty.UnsetValue)
			{
				obj = new DeferredResourceReferenceHolder(this.ResourceKey, obj);
			}
			return obj;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001B754 File Offset: 0x00019954
		private object FindResourceInAppOrSystem(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			object result;
			if (!SystemResources.IsSystemResourcesParsing)
			{
				object obj;
				result = FrameworkElement.FindResourceFromAppOrSystem(this.ResourceKey, out obj, false, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			else
			{
				result = SystemResources.FindResourceInternal(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			return result;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001B78C File Offset: 0x0001998C
		private object FindResourceInEnviroment(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary = this.FindTheResourceDictionary(serviceProvider, false);
			if (resourceDictionary != null)
			{
				return resourceDictionary.Lookup(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference, false);
			}
			object obj = this.FindResourceInAppOrSystem(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			if (obj == null)
			{
				obj = DependencyProperty.UnsetValue;
			}
			if (mustReturnDeferredResourceReference && !(obj is DeferredResourceReference))
			{
				obj = new DeferredResourceReferenceHolder(this.ResourceKey, obj);
			}
			return obj;
		}

		// Token: 0x0400079D RID: 1949
		private object _resourceKey;
	}
}
