using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Windows.Baml2006;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xaml;
using System.Xaml.Permissions;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Utility;

namespace System.Windows
{
	/// <summary>Provides a hash table / dictionary implementation that contains WPF resources used by components and other elements of a WPF application. </summary>
	// Token: 0x020000E6 RID: 230
	[Localizability(LocalizationCategory.Ignore)]
	[Ambient]
	[UsableDuringInitialization(true)]
	public class ResourceDictionary : IDictionary, ICollection, IEnumerable, ISupportInitialize, IUriContext, INameScope
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ResourceDictionary" /> class. </summary>
		// Token: 0x060007D4 RID: 2004 RVA: 0x00018F20 File Offset: 0x00017120
		public ResourceDictionary()
		{
			this._baseDictionary = new Hashtable();
			this.IsThemeDictionary = SystemResources.IsSystemResourcesParsing;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00018F49 File Offset: 0x00017149
		static ResourceDictionary()
		{
			ResourceDictionary.DummyInheritanceContext.DetachFromDispatcher();
		}

		/// <summary>Copies the <see cref="T:System.Windows.ResourceDictionary" /> elements to a one-dimensional <see cref="T:System.Collections.DictionaryEntry" /> at the specified index. </summary>
		/// <param name="array">The one-dimensional array that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from the <see cref="T:System.Windows.ResourceDictionary" /> instance. The array must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index of <paramref name="array" /> where copying begins.</param>
		// Token: 0x060007D6 RID: 2006 RVA: 0x00018F60 File Offset: 0x00017160
		public void CopyTo(DictionaryEntry[] array, int arrayIndex)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.CopyToWithoutLock(array, arrayIndex);
					return;
				}
			}
			this.CopyToWithoutLock(array, arrayIndex);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00018FB4 File Offset: 0x000171B4
		private void CopyToWithoutLock(DictionaryEntry[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this._baseDictionary.CopyTo(array, arrayIndex);
			int num = arrayIndex + this.Count;
			for (int i = arrayIndex; i < num; i++)
			{
				DictionaryEntry dictionaryEntry = array[i];
				object value = dictionaryEntry.Value;
				bool flag;
				this.OnGettingValuePrivate(dictionaryEntry.Key, ref value, out flag);
				dictionaryEntry.Value = value;
			}
		}

		/// <summary>Gets a collection of the <see cref="T:System.Windows.ResourceDictionary" /> dictionaries that constitute the various resource dictionaries in the merged dictionaries.</summary>
		/// <returns>The collection of merged dictionaries.</returns>
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001901A File Offset: 0x0001721A
		public Collection<ResourceDictionary> MergedDictionaries
		{
			get
			{
				if (this._mergedDictionaries == null)
				{
					this._mergedDictionaries = new ResourceDictionaryCollection(this);
					this._mergedDictionaries.CollectionChanged += this.OnMergedDictionariesChanged;
				}
				return this._mergedDictionaries;
			}
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) to load resources from.</summary>
		/// <returns>The source location of an external resource dictionary. </returns>
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001904D File Offset: 0x0001724D
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00019058 File Offset: 0x00017258
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				if (value == null || string.IsNullOrEmpty(value.OriginalString))
				{
					throw new ArgumentException(SR.Get("ResourceDictionaryLoadFromFailure", new object[]
					{
						(value == null) ? "''" : value.ToString()
					}));
				}
				ResourceDictionaryDiagnostics.RemoveResourceDictionaryForUri(this._source, this);
				ResourceDictionary.ResourceDictionarySourceUriWrapper resourceDictionarySourceUriWrapper = value as ResourceDictionary.ResourceDictionarySourceUriWrapper;
				Uri orgUri;
				if (resourceDictionarySourceUriWrapper == null)
				{
					this._source = value;
					orgUri = this._source;
				}
				else
				{
					this._source = resourceDictionarySourceUriWrapper.OriginalUri;
					orgUri = resourceDictionarySourceUriWrapper.VersionedUri;
				}
				this.Clear();
				Uri resolvedUri = BindUriHelper.GetResolvedUri(this._baseUri, orgUri);
				WebRequest request = WpfWebRequestHelper.CreateRequest(resolvedUri);
				WpfWebRequestHelper.ConfigCachePolicy(request, false);
				ContentType contentType = null;
				Stream s = null;
				try
				{
					s = WpfWebRequestHelper.GetResponseStream(request, out contentType);
				}
				catch (IOException)
				{
					if (this.IsSourcedFromThemeDictionary)
					{
						ResourceDictionary.FallbackState fallbackState = this._fallbackState;
						if (fallbackState != ResourceDictionary.FallbackState.Classic)
						{
							if (fallbackState == ResourceDictionary.FallbackState.Generic)
							{
								this._fallbackState = ResourceDictionary.FallbackState.None;
								Uri source = ThemeDictionaryExtension.GenerateFallbackUri(this, "themes/generic");
								this.Source = source;
							}
						}
						else
						{
							this._fallbackState = ResourceDictionary.FallbackState.Generic;
							Uri source2 = ThemeDictionaryExtension.GenerateFallbackUri(this, "themes/classic");
							this.Source = source2;
							this._fallbackState = ResourceDictionary.FallbackState.Classic;
						}
						return;
					}
					throw;
				}
				System.Windows.Markup.XamlReader xamlReader;
				ResourceDictionary resourceDictionary = MimeObjectFactory.GetObjectAndCloseStream(s, contentType, resolvedUri, false, false, false, false, out xamlReader) as ResourceDictionary;
				if (resourceDictionary == null)
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryLoadFromFailure", new object[]
					{
						this._source.ToString()
					}));
				}
				this._baseDictionary = resourceDictionary._baseDictionary;
				this._mergedDictionaries = resourceDictionary._mergedDictionaries;
				this.CopyDeferredContentFrom(resourceDictionary);
				this.MoveDeferredResourceReferencesFrom(resourceDictionary);
				this.HasImplicitStyles = resourceDictionary.HasImplicitStyles;
				this.HasImplicitDataTemplates = resourceDictionary.HasImplicitDataTemplates;
				this.InvalidatesImplicitDataTemplateResources = resourceDictionary.InvalidatesImplicitDataTemplateResources;
				if (this.InheritanceContext != null)
				{
					this.AddInheritanceContextToValues();
				}
				if (this._mergedDictionaries != null)
				{
					for (int i = 0; i < this._mergedDictionaries.Count; i++)
					{
						this.PropagateParentOwners(this._mergedDictionaries[i]);
					}
				}
				ResourceDictionaryDiagnostics.AddResourceDictionaryForUri(resolvedUri, this);
				if (!this.IsInitializePending)
				{
					this.NotifyOwners(new ResourcesChangeInfo(null, this));
				}
			}
		}

		/// <summary>Not supported by this Dictionary implementation.</summary>
		/// <param name="name">See Remarks.</param>
		/// <param name="scopedElement">See Remarks.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases when this method is called.</exception>
		// Token: 0x060007DB RID: 2011 RVA: 0x00019280 File Offset: 0x00017480
		public void RegisterName(string name, object scopedElement)
		{
			throw new NotSupportedException(SR.Get("NamesNotSupportedInsideResourceDictionary"));
		}

		/// <summary>Not supported by this Dictionary implementation.</summary>
		/// <param name="name">See Remarks</param>
		// Token: 0x060007DC RID: 2012 RVA: 0x00002137 File Offset: 0x00000337
		public void UnregisterName(string name)
		{
		}

		/// <summary>Not supported by this Dictionary implementation.</summary>
		/// <param name="name">The name identifier for the object being requested.</param>
		/// <returns>Always returns <see langword="null" />.
		///
		///   </returns>
		// Token: 0x060007DD RID: 2013 RVA: 0x0000C238 File Offset: 0x0000A438
		public object FindName(string name)
		{
			return null;
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Markup.IUriContext.BaseUri" />.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00019291 File Offset: 0x00017491
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00019299 File Offset: 0x00017499
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				this._baseUri = value;
			}
		}

		/// <summary>Gets whether this <see cref="T:System.Windows.ResourceDictionary" /> is fixed-size. </summary>
		/// <returns>
		///     <see langword="true" /> if the hash table is fixed-size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x000192A2 File Offset: 0x000174A2
		public bool IsFixedSize
		{
			get
			{
				return this._baseDictionary.IsFixedSize;
			}
		}

		/// <summary>Gets whether this <see cref="T:System.Windows.ResourceDictionary" /> is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the hash table is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x000192AF File Offset: 0x000174AF
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x000192B8 File Offset: 0x000174B8
		public bool IsReadOnly
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsReadOnly);
			}
			internal set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsReadOnly, value);
				if (value)
				{
					this.SealValues();
				}
				if (this._mergedDictionaries != null)
				{
					for (int i = 0; i < this._mergedDictionaries.Count; i++)
					{
						this._mergedDictionaries[i].IsReadOnly = value;
					}
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the invalidations fired
		///   by the <see cref="T:System.Windows.ResourceDictionary" /> object cause <see cref="T:System.Windows.Controls.ContentPresenter" /> objects to reevaluate their choice
		///   of template. The invalidations happen when an implicit data template resource
		///   changes.</summary>
		/// <returns>
		///   <see langword="true" /> if the invalidations cause <see cref="T:System.Windows.Controls.ContentPresenter" /> objects to reevaluate their choice
		///   of template; otherwise, <see langword="false" />. 
		///
		/// The default is <see langword="false" />.</returns>
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00019306 File Offset: 0x00017506
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00019310 File Offset: 0x00017510
		[DefaultValue(false)]
		public bool InvalidatesImplicitDataTemplateResources
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.InvalidatesImplicitDataTemplateResources);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.InvalidatesImplicitDataTemplateResources, value);
			}
		}

		/// <summary> Gets or sets the value associated with the given key. </summary>
		/// <param name="key">The desired key to get or set.</param>
		/// <returns>Value of the key.</returns>
		// Token: 0x170001A1 RID: 417
		public object this[object key]
		{
			get
			{
				bool flag;
				return this.GetValue(key, out flag);
			}
			set
			{
				this.SealValue(value);
				if (this.CanBeAccessedAcrossThreads)
				{
					object syncRoot = ((ICollection)this).SyncRoot;
					lock (syncRoot)
					{
						this.SetValueWithoutLock(key, value);
						return;
					}
				}
				this.SetValueWithoutLock(key, value);
			}
		}

		/// <summary>Gets or sets the deferrable content for this resource dictionary.</summary>
		/// <returns>Always returns <see langword="null" />.</returns>
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00019390 File Offset: 0x00017590
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DeferrableContent DeferrableContent
		{
			get
			{
				return null;
			}
			set
			{
				this.SetDeferrableContent(value);
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001939C File Offset: 0x0001759C
		private void SetValueWithoutLock(object key, object value)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			object obj = this._baseDictionary[key];
			if (obj != value)
			{
				this.ValidateDeferredResourceReferences(key);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.AddResource, this, key, value);
				}
				this._baseDictionary[key] = value;
				this.UpdateHasImplicitStyles(key);
				this.UpdateHasImplicitDataTemplates(key);
				this.NotifyOwners(new ResourcesChangeInfo(key));
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.AddResource, this, key, value);
				}
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00019434 File Offset: 0x00017634
		internal object GetValue(object key, out bool canCache)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					return this.GetValueWithoutLock(key, out canCache);
				}
			}
			return this.GetValueWithoutLock(key, out canCache);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001948C File Offset: 0x0001768C
		private object GetValueWithoutLock(object key, out bool canCache)
		{
			object obj = this._baseDictionary[key];
			if (obj != null)
			{
				this.OnGettingValuePrivate(key, ref obj, out canCache);
			}
			else
			{
				canCache = true;
				if (this._mergedDictionaries != null)
				{
					for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
					{
						ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
						if (resourceDictionary != null)
						{
							obj = resourceDictionary.GetValue(key, out canCache);
							if (obj != null)
							{
								break;
							}
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000194F8 File Offset: 0x000176F8
		internal Type GetValueType(object key, out bool found)
		{
			found = false;
			Type result = null;
			object obj = this._baseDictionary[key];
			if (obj != null)
			{
				found = true;
				KeyRecord keyRecord = obj as KeyRecord;
				if (keyRecord != null)
				{
					result = this.GetTypeOfFirstObject(keyRecord);
				}
				else
				{
					result = obj.GetType();
				}
			}
			else if (this._mergedDictionaries != null)
			{
				for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
					if (resourceDictionary != null)
					{
						result = resourceDictionary.GetValueType(key, out found);
						if (found)
						{
							break;
						}
					}
				}
			}
			return result;
		}

		/// <summary>Gets a collection of all keys contained in this <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <returns>The collection of all keys.</returns>
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0001957C File Offset: 0x0001777C
		public ICollection Keys
		{
			get
			{
				object[] array = new object[this.Count];
				this._baseDictionary.Keys.CopyTo(array, 0);
				return array;
			}
		}

		/// <summary> Gets a collection of all values associated with keys contained in this <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <returns>The collection of all values.</returns>
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x000195A8 File Offset: 0x000177A8
		public ICollection Values
		{
			get
			{
				return new ResourceDictionary.ResourceValuesCollection(this);
			}
		}

		/// <summary>Adds a resource by key to this <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <param name="key">The name of the key to add.</param>
		/// <param name="value">The value of the resource to add.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.ResourceDictionary" /> is locked or read-only.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Hashtable" />. </exception>
		// Token: 0x060007EF RID: 2031 RVA: 0x000195B0 File Offset: 0x000177B0
		public void Add(object key, object value)
		{
			this.SealValue(value);
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.AddWithoutLock(key, value);
					return;
				}
			}
			this.AddWithoutLock(key, value);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001960C File Offset: 0x0001780C
		private void AddWithoutLock(object key, object value)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			if (TraceResourceDictionary.IsEnabled)
			{
				TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.AddResource, this, key, value);
			}
			this._baseDictionary.Add(key, value);
			this.UpdateHasImplicitStyles(key);
			this.UpdateHasImplicitDataTemplates(key);
			this.NotifyOwners(new ResourcesChangeInfo(key));
			if (TraceResourceDictionary.IsEnabled)
			{
				TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.AddResource, this, key, value);
			}
		}

		/// <summary>Clears all keys (and values) in the base <see cref="T:System.Windows.ResourceDictionary" />. This does not clear any merged dictionary items.</summary>
		// Token: 0x060007F1 RID: 2033 RVA: 0x00019698 File Offset: 0x00017898
		public void Clear()
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.ClearWithoutLock();
					return;
				}
			}
			this.ClearWithoutLock();
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000196E8 File Offset: 0x000178E8
		private void ClearWithoutLock()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			if (this.Count > 0)
			{
				this.ValidateDeferredResourceReferences(null);
				this.RemoveInheritanceContextFromValues();
				this._baseDictionary.Clear();
				this.NotifyOwners(ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
			}
		}

		/// <summary>Determines whether the <see cref="T:System.Windows.ResourceDictionary" /> contains an element with the specified key. </summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Windows.ResourceDictionary" />.</param>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.ResourceDictionary" /> contains a key-value pair with the specified key; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007F3 RID: 2035 RVA: 0x00019744 File Offset: 0x00017944
		public bool Contains(object key)
		{
			bool flag = this._baseDictionary.Contains(key);
			if (flag)
			{
				KeyRecord keyRecord = this._baseDictionary[key] as KeyRecord;
				if (keyRecord != null && this._deferredLocationList.Contains(keyRecord))
				{
					return false;
				}
			}
			if (this._mergedDictionaries != null)
			{
				int num = this.MergedDictionaries.Count - 1;
				while (num > -1 && !flag)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[num];
					if (resourceDictionary != null)
					{
						flag = resourceDictionary.Contains(key);
					}
					num--;
				}
			}
			return flag;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000197C2 File Offset: 0x000179C2
		private bool ContainsBamlObjectFactory(object key)
		{
			return this.GetBamlObjectFactory(key) != null;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000197D0 File Offset: 0x000179D0
		private KeyRecord GetBamlObjectFactory(object key)
		{
			if (this._baseDictionary.Contains(key))
			{
				return this._baseDictionary[key] as KeyRecord;
			}
			if (this._mergedDictionaries != null)
			{
				for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
					if (resourceDictionary != null)
					{
						KeyRecord bamlObjectFactory = resourceDictionary.GetBamlObjectFactory(key);
						if (bamlObjectFactory != null)
						{
							return bamlObjectFactory;
						}
					}
				}
			}
			return null;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that can be used to iterate through the <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <returns>A specialized enumerator for the <see cref="T:System.Windows.ResourceDictionary" />.</returns>
		// Token: 0x060007F6 RID: 2038 RVA: 0x0001983B File Offset: 0x00017A3B
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ResourceDictionary.ResourceDictionaryEnumerator(this);
		}

		/// <summary>Removes the entry with the specified key from the base dictionary. </summary>
		/// <param name="key">Key of the entry to remove.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.ResourceDictionary" /> is locked or read-only.</exception>
		// Token: 0x060007F7 RID: 2039 RVA: 0x00019844 File Offset: 0x00017A44
		public void Remove(object key)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.RemoveWithoutLock(key);
					return;
				}
			}
			this.RemoveWithoutLock(key);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00019894 File Offset: 0x00017A94
		private void RemoveWithoutLock(object key)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			this.ValidateDeferredResourceReferences(key);
			this.RemoveInheritanceContext(this._baseDictionary[key]);
			this._baseDictionary.Remove(key);
			this.NotifyOwners(new ResourcesChangeInfo(key));
		}

		/// <summary>Gets the number of entries in the base <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <returns>The current number of entries in the base dictionary.</returns>
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000198F5 File Offset: 0x00017AF5
		public int Count
		{
			get
			{
				return this._baseDictionary.Count;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
		/// <returns>
		///     <see langword="true" /> if access to <see cref="T:System.Windows.ResourceDictionary" /> is synchronized (thread safe); otherwise, <see langword="false" />. </returns>
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00019902 File Offset: 0x00017B02
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._baseDictionary.IsSynchronized;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
		/// <returns>An object that can be used to synchronize access to <see cref="T:System.Windows.ResourceDictionary" />. </returns>
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001990F File Offset: 0x00017B0F
		object ICollection.SyncRoot
		{
			get
			{
				if (this.CanBeAccessedAcrossThreads)
				{
					return SystemResources.ThemeDictionaryLock;
				}
				return this._baseDictionary.SyncRoot;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</param>
		/// <param name="arrayIndex">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x060007FC RID: 2044 RVA: 0x0001992A File Offset: 0x00017B2A
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo(array as DictionaryEntry[], arrayIndex);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x060007FD RID: 2045 RVA: 0x00019939 File Offset: 0x00017B39
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary)this).GetEnumerator();
		}

		/// <summary>Begins the initialization phase for this <see cref="T:System.Windows.ResourceDictionary" />. </summary>
		/// <exception cref="T:System.InvalidOperationException">Called <see cref="M:System.Windows.ResourceDictionary.BeginInit" /> more than once before <see cref="M:System.Windows.ResourceDictionary.EndInit" /> was called.</exception>
		// Token: 0x060007FE RID: 2046 RVA: 0x00019941 File Offset: 0x00017B41
		public void BeginInit()
		{
			if (this.IsInitializePending)
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.IsInitializePending = true;
			this.IsInitialized = false;
		}

		/// <summary>Ends the initialization phase, and invalidates the previous tree such that all changes made to keys during the initialization phase can be accounted for. </summary>
		// Token: 0x060007FF RID: 2047 RVA: 0x00019969 File Offset: 0x00017B69
		public void EndInit()
		{
			if (!this.IsInitializePending)
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.IsInitializePending = false;
			this.IsInitialized = true;
			this.NotifyOwners(new ResourcesChangeInfo(null, this));
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001999E File Offset: 0x00017B9E
		private bool CanCache(KeyRecord keyRecord, object value)
		{
			return !keyRecord.SharedSet || keyRecord.Shared;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000199B0 File Offset: 0x00017BB0
		private void OnGettingValuePrivate(object key, ref object value, out bool canCache)
		{
			ResourceDictionaryDiagnostics.RecordLookupResult(key, this);
			this.OnGettingValue(key, ref value, out canCache);
			if ((key != null & canCache) && !object.Equals(this._baseDictionary[key], value))
			{
				if (this.InheritanceContext != null)
				{
					this.AddInheritanceContext(this.InheritanceContext, value);
				}
				this._baseDictionary[key] = value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.ResourceDictionary" /> receives a request for a resource.</summary>
		/// <param name="key">The key of the resource to get.</param>
		/// <param name="value">The value of the requested resource.</param>
		/// <param name="canCache">
		///       <see langword="true" /> if the resource can be saved and used later; otherwise, <see langword="false" />.</param>
		// Token: 0x06000802 RID: 2050 RVA: 0x00019A10 File Offset: 0x00017C10
		protected virtual void OnGettingValue(object key, ref object value, out bool canCache)
		{
			KeyRecord keyRecord = value as KeyRecord;
			if (keyRecord == null)
			{
				canCache = true;
				return;
			}
			if (this._deferredLocationList.Contains(keyRecord))
			{
				canCache = false;
				value = null;
				return;
			}
			this._deferredLocationList.Add(keyRecord);
			try
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.RealizeDeferContent, this, key, value);
				}
				value = this.CreateObject(keyRecord);
			}
			finally
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.RealizeDeferContent, this, key, value);
				}
			}
			this._deferredLocationList.Remove(keyRecord);
			if (key != null)
			{
				canCache = this.CanCache(keyRecord, value);
				if (canCache)
				{
					this.SealValue(value);
					this._numDefer--;
					if (this._numDefer == 0)
					{
						this.CloseReader();
						return;
					}
				}
			}
			else
			{
				canCache = true;
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00019AE4 File Offset: 0x00017CE4
		[SecurityTreatAsSafe]
		[SecurityCritical]
		private void SetDeferrableContent(DeferrableContent deferrableContent)
		{
			Baml2006ReaderSettings baml2006ReaderSettings = new Baml2006ReaderSettings(deferrableContent.SchemaContext.Settings);
			baml2006ReaderSettings.IsBamlFragment = true;
			baml2006ReaderSettings.OwnsStream = true;
			baml2006ReaderSettings.BaseUri = null;
			Baml2006Reader baml2006Reader = new Baml2006Reader(deferrableContent.Stream, deferrableContent.SchemaContext, baml2006ReaderSettings);
			this._objectWriterFactory = deferrableContent.ObjectWriterFactory;
			this._objectWriterSettings = deferrableContent.ObjectWriterParentSettings;
			this._deferredLocationList = new List<KeyRecord>();
			this._rootElement = deferrableContent.RootObject;
			IList<KeyRecord> list = baml2006Reader.ReadKeys();
			if (this._source == null)
			{
				if (this._reader == null)
				{
					this._reader = baml2006Reader;
					this._xamlLoadPermission = deferrableContent.LoadPermission;
					this.SetKeys(list, deferrableContent.ServiceProvider);
					return;
				}
				throw new InvalidOperationException(SR.Get("ResourceDictionaryDuplicateDeferredContent"));
			}
			else
			{
				if (list.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryDeferredContentFailure"));
				}
				return;
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00019BC0 File Offset: 0x00017DC0
		private object GetKeyValue(KeyRecord key, IServiceProvider serviceProvider)
		{
			if (key.KeyString != null)
			{
				return key.KeyString;
			}
			if (key.KeyType != null)
			{
				return key.KeyType;
			}
			System.Xaml.XamlReader reader = key.KeyNodeList.GetReader();
			return this.EvaluateMarkupExtensionNodeList(reader, serviceProvider);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00019C08 File Offset: 0x00017E08
		private object EvaluateMarkupExtensionNodeList(System.Xaml.XamlReader reader, IServiceProvider serviceProvider)
		{
			XamlObjectWriter xamlObjectWriter = this._objectWriterFactory.GetXamlObjectWriter(null);
			XamlServices.Transform(reader, xamlObjectWriter);
			object obj = xamlObjectWriter.Result;
			MarkupExtension markupExtension = obj as MarkupExtension;
			if (markupExtension != null)
			{
				obj = markupExtension.ProvideValue(serviceProvider);
			}
			return obj;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00019C44 File Offset: 0x00017E44
		private object GetStaticResourceKeyValue(StaticResource staticResource, IServiceProvider serviceProvider)
		{
			System.Xaml.XamlReader reader = staticResource.ResourceNodeList.GetReader();
			XamlType xamlType = reader.SchemaContext.GetXamlType(typeof(StaticResourceExtension));
			XamlMember member = xamlType.GetMember("ResourceKey");
			reader.Read();
			if (reader.NodeType == System.Xaml.XamlNodeType.StartObject && reader.Type == xamlType)
			{
				reader.Read();
				while (reader.NodeType == System.Xaml.XamlNodeType.StartMember && reader.Member != XamlLanguage.PositionalParameters && reader.Member != member)
				{
					reader.Skip();
				}
				if (reader.NodeType == System.Xaml.XamlNodeType.StartMember)
				{
					object result = null;
					reader.Read();
					if (reader.NodeType == System.Xaml.XamlNodeType.StartObject)
					{
						System.Xaml.XamlReader reader2 = reader.ReadSubtree();
						result = this.EvaluateMarkupExtensionNodeList(reader2, serviceProvider);
					}
					else if (reader.NodeType == System.Xaml.XamlNodeType.Value)
					{
						result = reader.Value;
					}
					return result;
				}
			}
			return null;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00019D1C File Offset: 0x00017F1C
		private void SetKeys(IList<KeyRecord> keyCollection, IServiceProvider serviceProvider)
		{
			this._numDefer = keyCollection.Count;
			StaticResourceExtension staticResourceWorker = new StaticResourceExtension();
			for (int i = 0; i < keyCollection.Count; i++)
			{
				KeyRecord keyRecord = keyCollection[i];
				if (keyRecord == null)
				{
					throw new ArgumentException(SR.Get("KeyCollectionHasInvalidKey"));
				}
				object keyValue = this.GetKeyValue(keyRecord, serviceProvider);
				this.UpdateHasImplicitStyles(keyValue);
				this.UpdateHasImplicitDataTemplates(keyValue);
				if (keyRecord != null && keyRecord.HasStaticResources)
				{
					this.SetOptimizedStaticResources(keyRecord.StaticResources, serviceProvider, staticResourceWorker);
				}
				this._baseDictionary.Add(keyValue, keyRecord);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.SetKey, this, keyValue);
				}
			}
			this.NotifyOwners(new ResourcesChangeInfo(null, this));
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00019DC8 File Offset: 0x00017FC8
		private void SetOptimizedStaticResources(IList<object> staticResources, IServiceProvider serviceProvider, StaticResourceExtension staticResourceWorker)
		{
			int i = 0;
			while (i < staticResources.Count)
			{
				OptimizedStaticResource optimizedStaticResource = staticResources[i] as OptimizedStaticResource;
				object resourceKey;
				if (optimizedStaticResource != null)
				{
					resourceKey = optimizedStaticResource.KeyValue;
					goto IL_3B;
				}
				StaticResource staticResource = staticResources[i] as StaticResource;
				if (staticResource != null)
				{
					resourceKey = this.GetStaticResourceKeyValue(staticResource, serviceProvider);
					goto IL_3B;
				}
				IL_5F:
				i++;
				continue;
				IL_3B:
				staticResourceWorker.ResourceKey = resourceKey;
				object obj = staticResourceWorker.TryProvideValueInternal(serviceProvider, true, true);
				staticResources[i] = new StaticResourceHolder(resourceKey, obj as DeferredResourceReference);
				goto IL_5F;
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00019E44 File Offset: 0x00018044
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private Type GetTypeOfFirstObject(KeyRecord keyRecord)
		{
			Type typeOfFirstStartObject = this._reader.GetTypeOfFirstStartObject(keyRecord);
			return typeOfFirstStartObject ?? typeof(string);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00019E70 File Offset: 0x00018070
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object CreateObject(KeyRecord key)
		{
			System.Xaml.XamlReader xamlReader = this._reader.ReadObject(key);
			if (xamlReader == null)
			{
				return null;
			}
			Uri baseUri = (this._rootElement is IUriContext) ? ((IUriContext)this._rootElement).BaseUri : this._baseUri;
			if (this._xamlLoadPermission != null)
			{
				this._xamlLoadPermission.Assert();
				try
				{
					return WpfXamlLoader.LoadDeferredContent(xamlReader, this._objectWriterFactory, false, this._rootElement, this._objectWriterSettings, baseUri);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return WpfXamlLoader.LoadDeferredContent(xamlReader, this._objectWriterFactory, false, this._rootElement, this._objectWriterSettings, baseUri);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00019F18 File Offset: 0x00018118
		internal object Lookup(object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, bool canCacheAsThemeResource)
		{
			if (allowDeferredResourceReference)
			{
				bool flag;
				return this.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, canCacheAsThemeResource, out flag);
			}
			if (!mustReturnDeferredResourceReference)
			{
				return this[key];
			}
			return new DeferredResourceReferenceHolder(key, this[key]);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00019F50 File Offset: 0x00018150
		internal void AddOwner(DispatcherObject owner)
		{
			if (this._inheritanceContext == null)
			{
				DependencyObject dependencyObject = owner as DependencyObject;
				if (dependencyObject != null)
				{
					this._inheritanceContext = new WeakReference(dependencyObject);
					this.AddInheritanceContextToValues();
				}
				else
				{
					this._inheritanceContext = new WeakReference(ResourceDictionary.DummyInheritanceContext);
				}
			}
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				if (this._ownerFEs == null)
				{
					this._ownerFEs = new WeakReferenceList(1);
				}
				else if (this._ownerFEs.Contains(frameworkElement) && this.ContainsCycle(this))
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
				}
				if (this.HasImplicitStyles)
				{
					frameworkElement.ShouldLookupImplicitStyles = true;
				}
				this._ownerFEs.Add(frameworkElement);
			}
			else
			{
				FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					if (this._ownerFCEs == null)
					{
						this._ownerFCEs = new WeakReferenceList(1);
					}
					else if (this._ownerFCEs.Contains(frameworkContentElement) && this.ContainsCycle(this))
					{
						throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
					}
					if (this.HasImplicitStyles)
					{
						frameworkContentElement.ShouldLookupImplicitStyles = true;
					}
					this._ownerFCEs.Add(frameworkContentElement);
				}
				else
				{
					Application application = owner as Application;
					if (application != null)
					{
						if (this._ownerApps == null)
						{
							this._ownerApps = new WeakReferenceList(1);
						}
						else if (this._ownerApps.Contains(application) && this.ContainsCycle(this))
						{
							throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
						}
						if (this.HasImplicitStyles)
						{
							application.HasImplicitStylesInResources = true;
						}
						this._ownerApps.Add(application);
						this.CanBeAccessedAcrossThreads = true;
						this.SealValues();
					}
				}
			}
			this.AddOwnerToAllMergedDictionaries(owner);
			this.TryInitialize();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001A0EC File Offset: 0x000182EC
		internal void RemoveOwner(DispatcherObject owner)
		{
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				if (this._ownerFEs != null)
				{
					this._ownerFEs.Remove(frameworkElement);
					if (this._ownerFEs.Count == 0)
					{
						this._ownerFEs = null;
					}
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					if (this._ownerFCEs != null)
					{
						this._ownerFCEs.Remove(frameworkContentElement);
						if (this._ownerFCEs.Count == 0)
						{
							this._ownerFCEs = null;
						}
					}
				}
				else
				{
					Application application = owner as Application;
					if (application != null && this._ownerApps != null)
					{
						this._ownerApps.Remove(application);
						if (this._ownerApps.Count == 0)
						{
							this._ownerApps = null;
						}
					}
				}
			}
			if (owner == this.InheritanceContext)
			{
				this.RemoveInheritanceContextFromValues();
				this._inheritanceContext = null;
			}
			this.RemoveOwnerFromAllMergedDictionaries(owner);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001A1B8 File Offset: 0x000183B8
		internal bool ContainsOwner(DispatcherObject owner)
		{
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				return this._ownerFEs != null && this._ownerFEs.Contains(frameworkElement);
			}
			FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return this._ownerFCEs != null && this._ownerFCEs.Contains(frameworkContentElement);
			}
			Application application = owner as Application;
			return application != null && this._ownerApps != null && this._ownerApps.Contains(application);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001A229 File Offset: 0x00018429
		private void TryInitialize()
		{
			if (!this.IsInitializePending && !this.IsInitialized)
			{
				this.IsInitialized = true;
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001A244 File Offset: 0x00018444
		private void NotifyOwners(ResourcesChangeInfo info)
		{
			bool isInitialized = this.IsInitialized;
			bool flag = info.IsResourceAddOperation && this.HasImplicitStyles;
			if (isInitialized && this.InvalidatesImplicitDataTemplateResources)
			{
				info.SetIsImplicitDataTemplateChange();
			}
			if (isInitialized || flag)
			{
				if (this._ownerFEs != null)
				{
					foreach (object obj in this._ownerFEs)
					{
						FrameworkElement frameworkElement = obj as FrameworkElement;
						if (frameworkElement != null)
						{
							if (flag)
							{
								frameworkElement.ShouldLookupImplicitStyles = true;
							}
							if (isInitialized)
							{
								TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, info);
							}
						}
					}
				}
				if (this._ownerFCEs != null)
				{
					foreach (object obj2 in this._ownerFCEs)
					{
						FrameworkContentElement frameworkContentElement = obj2 as FrameworkContentElement;
						if (frameworkContentElement != null)
						{
							if (flag)
							{
								frameworkContentElement.ShouldLookupImplicitStyles = true;
							}
							if (isInitialized)
							{
								TreeWalkHelper.InvalidateOnResourcesChange(null, frameworkContentElement, info);
							}
						}
					}
				}
				if (this._ownerApps != null)
				{
					foreach (object obj3 in this._ownerApps)
					{
						Application application = obj3 as Application;
						if (application != null)
						{
							if (flag)
							{
								application.HasImplicitStylesInResources = true;
							}
							if (isInitialized)
							{
								application.InvalidateResourceReferences(info);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001A368 File Offset: 0x00018568
		internal object FetchResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			return this.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, true, out canCache);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001A378 File Offset: 0x00018578
		private object FetchResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, bool canCacheAsThemeResource, out bool canCache)
		{
			if (allowDeferredResourceReference && (this.ContainsBamlObjectFactory(resourceKey) || (mustReturnDeferredResourceReference && this.Contains(resourceKey))))
			{
				canCache = false;
				DeferredResourceReference deferredResourceReference;
				if (!this.IsThemeDictionary)
				{
					if (this._ownerApps != null)
					{
						deferredResourceReference = new DeferredAppResourceReference(this, resourceKey);
					}
					else
					{
						deferredResourceReference = new DeferredResourceReference(this, resourceKey);
					}
					if (this._deferredResourceReferences == null)
					{
						this._deferredResourceReferences = new WeakReferenceList();
					}
					this._deferredResourceReferences.Add(deferredResourceReference, true);
				}
				else
				{
					deferredResourceReference = new DeferredThemeResourceReference(this, resourceKey, canCacheAsThemeResource);
				}
				ResourceDictionaryDiagnostics.RecordLookupResult(resourceKey, this);
				return deferredResourceReference;
			}
			return this.GetValue(resourceKey, out canCache);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001A404 File Offset: 0x00018604
		private void ValidateDeferredResourceReferences(object resourceKey)
		{
			if (this._deferredResourceReferences != null)
			{
				foreach (object obj in this._deferredResourceReferences)
				{
					DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
					if (deferredResourceReference != null && (resourceKey == null || object.Equals(resourceKey, deferredResourceReference.Key)))
					{
						deferredResourceReference.GetValue(BaseValueSourceInternal.Unknown);
					}
				}
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001A45C File Offset: 0x0001865C
		private void OnMergedDictionariesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			List<ResourceDictionary> list = null;
			List<ResourceDictionary> list2 = null;
			ResourcesChangeInfo catastrophicDictionaryChangeInfo;
			if (e.Action != NotifyCollectionChangedAction.Reset)
			{
				Invariant.Assert((e.NewItems != null && e.NewItems.Count > 0) || (e.OldItems != null && e.OldItems.Count > 0), "The NotifyCollectionChanged event fired when no dictionaries were added or removed");
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					list = new List<ResourceDictionary>(e.OldItems.Count);
					for (int i = 0; i < e.OldItems.Count; i++)
					{
						ResourceDictionary resourceDictionary = (ResourceDictionary)e.OldItems[i];
						list.Add(resourceDictionary);
						this.RemoveParentOwners(resourceDictionary);
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
				{
					list2 = new List<ResourceDictionary>(e.NewItems.Count);
					for (int j = 0; j < e.NewItems.Count; j++)
					{
						ResourceDictionary resourceDictionary = (ResourceDictionary)e.NewItems[j];
						list2.Add(resourceDictionary);
						if (!this.HasImplicitStyles && resourceDictionary.HasImplicitStyles)
						{
							this.HasImplicitStyles = true;
						}
						if (!this.HasImplicitDataTemplates && resourceDictionary.HasImplicitDataTemplates)
						{
							this.HasImplicitDataTemplates = true;
						}
						if (this.IsThemeDictionary)
						{
							resourceDictionary.IsThemeDictionary = true;
						}
						this.PropagateParentOwners(resourceDictionary);
					}
				}
				catastrophicDictionaryChangeInfo = new ResourcesChangeInfo(list, list2, false, false, null);
			}
			else
			{
				catastrophicDictionaryChangeInfo = ResourcesChangeInfo.CatastrophicDictionaryChangeInfo;
			}
			this.NotifyOwners(catastrophicDictionaryChangeInfo);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001A5CC File Offset: 0x000187CC
		private void AddOwnerToAllMergedDictionaries(DispatcherObject owner)
		{
			if (this._mergedDictionaries != null)
			{
				for (int i = 0; i < this._mergedDictionaries.Count; i++)
				{
					this._mergedDictionaries[i].AddOwner(owner);
				}
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001A60C File Offset: 0x0001880C
		private void RemoveOwnerFromAllMergedDictionaries(DispatcherObject owner)
		{
			if (this._mergedDictionaries != null)
			{
				for (int i = 0; i < this._mergedDictionaries.Count; i++)
				{
					this._mergedDictionaries[i].RemoveOwner(owner);
				}
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001A64C File Offset: 0x0001884C
		private void PropagateParentOwners(ResourceDictionary mergedDictionary)
		{
			if (this._ownerFEs != null)
			{
				Invariant.Assert(this._ownerFEs.Count > 0);
				if (mergedDictionary._ownerFEs == null)
				{
					mergedDictionary._ownerFEs = new WeakReferenceList(this._ownerFEs.Count);
				}
				foreach (object obj in this._ownerFEs)
				{
					FrameworkElement frameworkElement = obj as FrameworkElement;
					if (frameworkElement != null)
					{
						mergedDictionary.AddOwner(frameworkElement);
					}
				}
			}
			if (this._ownerFCEs != null)
			{
				Invariant.Assert(this._ownerFCEs.Count > 0);
				if (mergedDictionary._ownerFCEs == null)
				{
					mergedDictionary._ownerFCEs = new WeakReferenceList(this._ownerFCEs.Count);
				}
				foreach (object obj2 in this._ownerFCEs)
				{
					FrameworkContentElement frameworkContentElement = obj2 as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						mergedDictionary.AddOwner(frameworkContentElement);
					}
				}
			}
			if (this._ownerApps != null)
			{
				Invariant.Assert(this._ownerApps.Count > 0);
				if (mergedDictionary._ownerApps == null)
				{
					mergedDictionary._ownerApps = new WeakReferenceList(this._ownerApps.Count);
				}
				foreach (object obj3 in this._ownerApps)
				{
					Application application = obj3 as Application;
					if (application != null)
					{
						mergedDictionary.AddOwner(application);
					}
				}
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001A7B0 File Offset: 0x000189B0
		internal void RemoveParentOwners(ResourceDictionary mergedDictionary)
		{
			if (this._ownerFEs != null)
			{
				foreach (object obj in this._ownerFEs)
				{
					FrameworkElement owner = obj as FrameworkElement;
					mergedDictionary.RemoveOwner(owner);
				}
			}
			if (this._ownerFCEs != null)
			{
				Invariant.Assert(this._ownerFCEs.Count > 0);
				foreach (object obj2 in this._ownerFCEs)
				{
					FrameworkContentElement owner2 = obj2 as FrameworkContentElement;
					mergedDictionary.RemoveOwner(owner2);
				}
			}
			if (this._ownerApps != null)
			{
				Invariant.Assert(this._ownerApps.Count > 0);
				foreach (object obj3 in this._ownerApps)
				{
					Application owner3 = obj3 as Application;
					mergedDictionary.RemoveOwner(owner3);
				}
			}
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001A88C File Offset: 0x00018A8C
		private bool ContainsCycle(ResourceDictionary origin)
		{
			for (int i = 0; i < this.MergedDictionaries.Count; i++)
			{
				ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
				if (resourceDictionary == origin || resourceDictionary.ContainsCycle(origin))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001A8CC File Offset: 0x00018ACC
		internal WeakReferenceList FrameworkElementOwners
		{
			get
			{
				return this._ownerFEs;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0001A8D4 File Offset: 0x00018AD4
		internal WeakReferenceList FrameworkContentElementOwners
		{
			get
			{
				return this._ownerFCEs;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001A8DC File Offset: 0x00018ADC
		internal WeakReferenceList ApplicationOwners
		{
			get
			{
				return this._ownerApps;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0001A8E4 File Offset: 0x00018AE4
		internal WeakReferenceList DeferredResourceReferences
		{
			get
			{
				return this._deferredResourceReferences;
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001A8EC File Offset: 0x00018AEC
		private void SealValues()
		{
			int count = this._baseDictionary.Count;
			if (count > 0)
			{
				object[] array = new object[count];
				this._baseDictionary.Values.CopyTo(array, 0);
				foreach (object value in array)
				{
					this.SealValue(value);
				}
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001A940 File Offset: 0x00018B40
		private void SealValue(object value)
		{
			DependencyObject inheritanceContext = this.InheritanceContext;
			if (inheritanceContext != null)
			{
				this.AddInheritanceContext(inheritanceContext, value);
			}
			if (this.IsThemeDictionary || this._ownerApps != null || this.IsReadOnly)
			{
				StyleHelper.SealIfSealable(value);
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001A980 File Offset: 0x00018B80
		private void AddInheritanceContext(DependencyObject inheritanceContext, object value)
		{
			if (inheritanceContext.ProvideSelfAsInheritanceContext(value, VisualBrush.VisualProperty))
			{
				DependencyObject dependencyObject = value as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.IsInheritanceContextSealed = true;
				}
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001A9AC File Offset: 0x00018BAC
		private void AddInheritanceContextToValues()
		{
			DependencyObject inheritanceContext = this.InheritanceContext;
			int count = this._baseDictionary.Count;
			if (count > 0)
			{
				object[] array = new object[count];
				this._baseDictionary.Values.CopyTo(array, 0);
				foreach (object value in array)
				{
					this.AddInheritanceContext(inheritanceContext, value);
				}
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001AA0C File Offset: 0x00018C0C
		private void RemoveInheritanceContext(object value)
		{
			DependencyObject dependencyObject = value as DependencyObject;
			DependencyObject inheritanceContext = this.InheritanceContext;
			if (dependencyObject != null && inheritanceContext != null && dependencyObject.IsInheritanceContextSealed && dependencyObject.InheritanceContext == inheritanceContext)
			{
				dependencyObject.IsInheritanceContextSealed = false;
				inheritanceContext.RemoveSelfAsInheritanceContext(dependencyObject, VisualBrush.VisualProperty);
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001AA54 File Offset: 0x00018C54
		private void RemoveInheritanceContextFromValues()
		{
			foreach (object value in this._baseDictionary.Values)
			{
				this.RemoveInheritanceContext(value);
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001AAB0 File Offset: 0x00018CB0
		private void UpdateHasImplicitStyles(object key)
		{
			if (!this.HasImplicitStyles)
			{
				this.HasImplicitStyles = (key as Type != null);
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001AACC File Offset: 0x00018CCC
		private void UpdateHasImplicitDataTemplates(object key)
		{
			if (!this.HasImplicitDataTemplates)
			{
				this.HasImplicitDataTemplates = (key is DataTemplateKey);
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001AAE5 File Offset: 0x00018CE5
		private DependencyObject InheritanceContext
		{
			get
			{
				if (this._inheritanceContext == null)
				{
					return null;
				}
				return (DependencyObject)this._inheritanceContext.Target;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0001AB01 File Offset: 0x00018D01
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0001AB0A File Offset: 0x00018D0A
		private bool IsInitialized
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsInitialized);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsInitialized, value);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0001AB14 File Offset: 0x00018D14
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0001AB1D File Offset: 0x00018D1D
		private bool IsInitializePending
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsInitializePending);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsInitializePending, value);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0001AB27 File Offset: 0x00018D27
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0001AB30 File Offset: 0x00018D30
		private bool IsThemeDictionary
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsThemeDictionary);
			}
			set
			{
				if (this.IsThemeDictionary != value)
				{
					this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsThemeDictionary, value);
					if (value)
					{
						this.SealValues();
					}
					if (this._mergedDictionaries != null)
					{
						for (int i = 0; i < this._mergedDictionaries.Count; i++)
						{
							this._mergedDictionaries[i].IsThemeDictionary = value;
						}
					}
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0001AB87 File Offset: 0x00018D87
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0001AB91 File Offset: 0x00018D91
		internal bool HasImplicitStyles
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitStyles);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitStyles, value);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001AB9C File Offset: 0x00018D9C
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0001ABA9 File Offset: 0x00018DA9
		internal bool HasImplicitDataTemplates
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitDataTemplates);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitDataTemplates, value);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001ABB7 File Offset: 0x00018DB7
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0001ABC1 File Offset: 0x00018DC1
		internal bool CanBeAccessedAcrossThreads
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.CanBeAccessedAcrossThreads);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.CanBeAccessedAcrossThreads, value);
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001ABCC File Offset: 0x00018DCC
		private void WritePrivateFlag(ResourceDictionary.PrivateFlags bit, bool value)
		{
			if (value)
			{
				this._flags |= bit;
				return;
			}
			this._flags &= ~bit;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001ABF0 File Offset: 0x00018DF0
		private bool ReadPrivateFlag(ResourceDictionary.PrivateFlags bit)
		{
			return (this._flags & bit) > (ResourceDictionary.PrivateFlags)0;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001ABFD File Offset: 0x00018DFD
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CloseReader()
		{
			this._reader.Close();
			this._reader = null;
			this._xamlLoadPermission = null;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001AC18 File Offset: 0x00018E18
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CopyDeferredContentFrom(ResourceDictionary loadedRD)
		{
			this._buffer = loadedRD._buffer;
			this._bamlStream = loadedRD._bamlStream;
			this._startPosition = loadedRD._startPosition;
			this._contentSize = loadedRD._contentSize;
			this._objectWriterFactory = loadedRD._objectWriterFactory;
			this._objectWriterSettings = loadedRD._objectWriterSettings;
			this._rootElement = loadedRD._rootElement;
			this._reader = loadedRD._reader;
			this._xamlLoadPermission = loadedRD._xamlLoadPermission;
			this._numDefer = loadedRD._numDefer;
			this._deferredLocationList = loadedRD._deferredLocationList;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001ACAC File Offset: 0x00018EAC
		private void MoveDeferredResourceReferencesFrom(ResourceDictionary loadedRD)
		{
			this._deferredResourceReferences = loadedRD._deferredResourceReferences;
			if (this._deferredResourceReferences != null)
			{
				foreach (object obj in this._deferredResourceReferences)
				{
					DeferredResourceReference deferredResourceReference = (DeferredResourceReference)obj;
					deferredResourceReference.Dictionary = this;
				}
			}
		}

		// Token: 0x04000776 RID: 1910
		internal bool IsSourcedFromThemeDictionary;

		// Token: 0x04000777 RID: 1911
		private ResourceDictionary.FallbackState _fallbackState;

		// Token: 0x04000778 RID: 1912
		private Hashtable _baseDictionary;

		// Token: 0x04000779 RID: 1913
		private WeakReferenceList _ownerFEs;

		// Token: 0x0400077A RID: 1914
		private WeakReferenceList _ownerFCEs;

		// Token: 0x0400077B RID: 1915
		private WeakReferenceList _ownerApps;

		// Token: 0x0400077C RID: 1916
		private WeakReferenceList _deferredResourceReferences;

		// Token: 0x0400077D RID: 1917
		private ObservableCollection<ResourceDictionary> _mergedDictionaries;

		// Token: 0x0400077E RID: 1918
		private Uri _source;

		// Token: 0x0400077F RID: 1919
		private Uri _baseUri;

		// Token: 0x04000780 RID: 1920
		private ResourceDictionary.PrivateFlags _flags;

		// Token: 0x04000781 RID: 1921
		private List<KeyRecord> _deferredLocationList;

		// Token: 0x04000782 RID: 1922
		private byte[] _buffer;

		// Token: 0x04000783 RID: 1923
		private Stream _bamlStream;

		// Token: 0x04000784 RID: 1924
		private long _startPosition;

		// Token: 0x04000785 RID: 1925
		private int _contentSize;

		// Token: 0x04000786 RID: 1926
		private object _rootElement;

		// Token: 0x04000787 RID: 1927
		private int _numDefer;

		// Token: 0x04000788 RID: 1928
		private WeakReference _inheritanceContext;

		// Token: 0x04000789 RID: 1929
		private static readonly DependencyObject DummyInheritanceContext = new DependencyObject();

		// Token: 0x0400078A RID: 1930
		private XamlObjectIds _contextXamlObjectIds = new XamlObjectIds();

		// Token: 0x0400078B RID: 1931
		private IXamlObjectWriterFactory _objectWriterFactory;

		// Token: 0x0400078C RID: 1932
		private XamlObjectWriterSettings _objectWriterSettings;

		// Token: 0x0400078D RID: 1933
		[SecurityCritical]
		private XamlLoadPermission _xamlLoadPermission;

		// Token: 0x0400078E RID: 1934
		[SecurityCritical]
		private Baml2006Reader _reader;

		// Token: 0x02000820 RID: 2080
		private class ResourceDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06007E58 RID: 32344 RVA: 0x002359E9 File Offset: 0x00233BE9
			internal ResourceDictionaryEnumerator(ResourceDictionary owner)
			{
				this._owner = owner;
				this._keysEnumerator = this._owner.Keys.GetEnumerator();
			}

			// Token: 0x17001D51 RID: 7505
			// (get) Token: 0x06007E59 RID: 32345 RVA: 0x00235A0E File Offset: 0x00233C0E
			object IEnumerator.Current
			{
				get
				{
					return ((IDictionaryEnumerator)this).Entry;
				}
			}

			// Token: 0x06007E5A RID: 32346 RVA: 0x00235A1B File Offset: 0x00233C1B
			bool IEnumerator.MoveNext()
			{
				return this._keysEnumerator.MoveNext();
			}

			// Token: 0x06007E5B RID: 32347 RVA: 0x00235A28 File Offset: 0x00233C28
			void IEnumerator.Reset()
			{
				this._keysEnumerator.Reset();
			}

			// Token: 0x17001D52 RID: 7506
			// (get) Token: 0x06007E5C RID: 32348 RVA: 0x00235A38 File Offset: 0x00233C38
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					object key = this._keysEnumerator.Current;
					object value = this._owner[key];
					return new DictionaryEntry(key, value);
				}
			}

			// Token: 0x17001D53 RID: 7507
			// (get) Token: 0x06007E5D RID: 32349 RVA: 0x00235A65 File Offset: 0x00233C65
			object IDictionaryEnumerator.Key
			{
				get
				{
					return this._keysEnumerator.Current;
				}
			}

			// Token: 0x17001D54 RID: 7508
			// (get) Token: 0x06007E5E RID: 32350 RVA: 0x00235A72 File Offset: 0x00233C72
			object IDictionaryEnumerator.Value
			{
				get
				{
					return this._owner[this._keysEnumerator.Current];
				}
			}

			// Token: 0x04003BCF RID: 15311
			private ResourceDictionary _owner;

			// Token: 0x04003BD0 RID: 15312
			private IEnumerator _keysEnumerator;
		}

		// Token: 0x02000821 RID: 2081
		private class ResourceValuesEnumerator : IEnumerator
		{
			// Token: 0x06007E5F RID: 32351 RVA: 0x00235A8A File Offset: 0x00233C8A
			internal ResourceValuesEnumerator(ResourceDictionary owner)
			{
				this._owner = owner;
				this._keysEnumerator = this._owner.Keys.GetEnumerator();
			}

			// Token: 0x17001D55 RID: 7509
			// (get) Token: 0x06007E60 RID: 32352 RVA: 0x00235AAF File Offset: 0x00233CAF
			object IEnumerator.Current
			{
				get
				{
					return this._owner[this._keysEnumerator.Current];
				}
			}

			// Token: 0x06007E61 RID: 32353 RVA: 0x00235AC7 File Offset: 0x00233CC7
			bool IEnumerator.MoveNext()
			{
				return this._keysEnumerator.MoveNext();
			}

			// Token: 0x06007E62 RID: 32354 RVA: 0x00235AD4 File Offset: 0x00233CD4
			void IEnumerator.Reset()
			{
				this._keysEnumerator.Reset();
			}

			// Token: 0x04003BD1 RID: 15313
			private ResourceDictionary _owner;

			// Token: 0x04003BD2 RID: 15314
			private IEnumerator _keysEnumerator;
		}

		// Token: 0x02000822 RID: 2082
		private class ResourceValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x06007E63 RID: 32355 RVA: 0x00235AE1 File Offset: 0x00233CE1
			internal ResourceValuesCollection(ResourceDictionary owner)
			{
				this._owner = owner;
			}

			// Token: 0x17001D56 RID: 7510
			// (get) Token: 0x06007E64 RID: 32356 RVA: 0x00235AF0 File Offset: 0x00233CF0
			int ICollection.Count
			{
				get
				{
					return this._owner.Count;
				}
			}

			// Token: 0x17001D57 RID: 7511
			// (get) Token: 0x06007E65 RID: 32357 RVA: 0x0000B02A File Offset: 0x0000922A
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001D58 RID: 7512
			// (get) Token: 0x06007E66 RID: 32358 RVA: 0x0001B7E3 File Offset: 0x000199E3
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x06007E67 RID: 32359 RVA: 0x00235B00 File Offset: 0x00233D00
			void ICollection.CopyTo(Array array, int index)
			{
				foreach (object key in this._owner.Keys)
				{
					array.SetValue(this._owner[key], index++);
				}
			}

			// Token: 0x06007E68 RID: 32360 RVA: 0x00235B6C File Offset: 0x00233D6C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ResourceDictionary.ResourceValuesEnumerator(this._owner);
			}

			// Token: 0x04003BD3 RID: 15315
			private ResourceDictionary _owner;
		}

		// Token: 0x02000823 RID: 2083
		private enum PrivateFlags : byte
		{
			// Token: 0x04003BD5 RID: 15317
			IsInitialized = 1,
			// Token: 0x04003BD6 RID: 15318
			IsInitializePending,
			// Token: 0x04003BD7 RID: 15319
			IsReadOnly = 4,
			// Token: 0x04003BD8 RID: 15320
			IsThemeDictionary = 8,
			// Token: 0x04003BD9 RID: 15321
			HasImplicitStyles = 16,
			// Token: 0x04003BDA RID: 15322
			CanBeAccessedAcrossThreads = 32,
			// Token: 0x04003BDB RID: 15323
			InvalidatesImplicitDataTemplateResources = 64,
			// Token: 0x04003BDC RID: 15324
			HasImplicitDataTemplates = 128
		}

		// Token: 0x02000824 RID: 2084
		internal class ResourceDictionarySourceUriWrapper : Uri
		{
			// Token: 0x06007E69 RID: 32361 RVA: 0x00235B79 File Offset: 0x00233D79
			public ResourceDictionarySourceUriWrapper(Uri originalUri, Uri versionedUri) : base(originalUri.OriginalString, UriKind.RelativeOrAbsolute)
			{
				this.OriginalUri = originalUri;
				this.VersionedUri = versionedUri;
			}

			// Token: 0x17001D59 RID: 7513
			// (get) Token: 0x06007E6A RID: 32362 RVA: 0x00235B96 File Offset: 0x00233D96
			// (set) Token: 0x06007E6B RID: 32363 RVA: 0x00235B9E File Offset: 0x00233D9E
			internal Uri OriginalUri { get; set; }

			// Token: 0x17001D5A RID: 7514
			// (get) Token: 0x06007E6C RID: 32364 RVA: 0x00235BA7 File Offset: 0x00233DA7
			// (set) Token: 0x06007E6D RID: 32365 RVA: 0x00235BAF File Offset: 0x00233DAF
			internal Uri VersionedUri { get; set; }
		}

		// Token: 0x02000825 RID: 2085
		private enum FallbackState
		{
			// Token: 0x04003BE0 RID: 15328
			Classic,
			// Token: 0x04003BE1 RID: 15329
			Generic,
			// Token: 0x04003BE2 RID: 15330
			None
		}
	}
}
