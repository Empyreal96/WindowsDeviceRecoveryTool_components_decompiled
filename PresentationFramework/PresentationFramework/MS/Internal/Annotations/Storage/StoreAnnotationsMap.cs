using System;
using System.Collections.Generic;
using System.Windows.Annotations;

namespace MS.Internal.Annotations.Storage
{
	// Token: 0x020007D0 RID: 2000
	internal class StoreAnnotationsMap
	{
		// Token: 0x06007BBF RID: 31679 RVA: 0x0022C33E File Offset: 0x0022A53E
		internal StoreAnnotationsMap(AnnotationAuthorChangedEventHandler authorChanged, AnnotationResourceChangedEventHandler anchorChanged, AnnotationResourceChangedEventHandler cargoChanged)
		{
			this._authorChanged = authorChanged;
			this._anchorChanged = anchorChanged;
			this._cargoChanged = cargoChanged;
		}

		// Token: 0x06007BC0 RID: 31680 RVA: 0x0022C368 File Offset: 0x0022A568
		public void AddAnnotation(Annotation annotation, bool dirty)
		{
			annotation.AuthorChanged += this.OnAuthorChanged;
			annotation.AnchorChanged += this.OnAnchorChanged;
			annotation.CargoChanged += this.OnCargoChanged;
			this._currentAnnotations.Add(annotation.Id, new StoreAnnotationsMap.CachedAnnotation(annotation, dirty));
		}

		// Token: 0x06007BC1 RID: 31681 RVA: 0x0022C3C4 File Offset: 0x0022A5C4
		public void RemoveAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				cachedAnnotation.Annotation.AuthorChanged -= this.OnAuthorChanged;
				cachedAnnotation.Annotation.AnchorChanged -= this.OnAnchorChanged;
				cachedAnnotation.Annotation.CargoChanged -= this.OnCargoChanged;
				this._currentAnnotations.Remove(id);
			}
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x0022C438 File Offset: 0x0022A638
		public Dictionary<Guid, Annotation> FindAnnotations(ContentLocator anchorLocator)
		{
			if (anchorLocator == null)
			{
				throw new ArgumentNullException("locator");
			}
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (StoreAnnotationsMap.CachedAnnotation cachedAnnotation in this._currentAnnotations.Values)
			{
				Annotation annotation = cachedAnnotation.Annotation;
				bool flag = false;
				foreach (AnnotationResource annotationResource in annotation.Anchors)
				{
					foreach (ContentLocatorBase contentLocatorBase in annotationResource.ContentLocators)
					{
						ContentLocator contentLocator = contentLocatorBase as ContentLocator;
						if (contentLocator != null)
						{
							if (contentLocator.StartsWith(anchorLocator))
							{
								flag = true;
							}
						}
						else
						{
							ContentLocatorGroup contentLocatorGroup = contentLocatorBase as ContentLocatorGroup;
							if (contentLocatorGroup != null)
							{
								foreach (ContentLocator contentLocator2 in contentLocatorGroup.Locators)
								{
									if (contentLocator2.StartsWith(anchorLocator))
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (flag)
						{
							dictionary.Add(annotation.Id, annotation);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06007BC3 RID: 31683 RVA: 0x0022C59C File Offset: 0x0022A79C
		public Dictionary<Guid, Annotation> FindAnnotations()
		{
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Annotation);
			}
			return dictionary;
		}

		// Token: 0x06007BC4 RID: 31684 RVA: 0x0022C608 File Offset: 0x0022A808
		public Annotation FindAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				return cachedAnnotation.Annotation;
			}
			return null;
		}

		// Token: 0x06007BC5 RID: 31685 RVA: 0x0022C630 File Offset: 0x0022A830
		public List<Annotation> FindDirtyAnnotations()
		{
			List<Annotation> list = new List<Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					list.Add(keyValuePair.Value.Annotation);
				}
			}
			return list;
		}

		// Token: 0x06007BC6 RID: 31686 RVA: 0x0022C6A4 File Offset: 0x0022A8A4
		public void ValidateDirtyAnnotations()
		{
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					keyValuePair.Value.Dirty = false;
				}
			}
		}

		// Token: 0x06007BC7 RID: 31687 RVA: 0x0022C70C File Offset: 0x0022A90C
		private void OnAnchorChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._anchorChanged(sender, args);
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x0022C737 File Offset: 0x0022A937
		private void OnCargoChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._cargoChanged(sender, args);
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x0022C762 File Offset: 0x0022A962
		private void OnAuthorChanged(object sender, AnnotationAuthorChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._authorChanged(sender, args);
		}

		// Token: 0x04003A36 RID: 14902
		private Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation> _currentAnnotations = new Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation>();

		// Token: 0x04003A37 RID: 14903
		private AnnotationAuthorChangedEventHandler _authorChanged;

		// Token: 0x04003A38 RID: 14904
		private AnnotationResourceChangedEventHandler _anchorChanged;

		// Token: 0x04003A39 RID: 14905
		private AnnotationResourceChangedEventHandler _cargoChanged;

		// Token: 0x02000B7E RID: 2942
		private class CachedAnnotation
		{
			// Token: 0x06008E3E RID: 36414 RVA: 0x0025B908 File Offset: 0x00259B08
			public CachedAnnotation(Annotation annotation, bool dirty)
			{
				this.Annotation = annotation;
				this.Dirty = dirty;
			}

			// Token: 0x17001FA1 RID: 8097
			// (get) Token: 0x06008E3F RID: 36415 RVA: 0x0025B91E File Offset: 0x00259B1E
			// (set) Token: 0x06008E40 RID: 36416 RVA: 0x0025B926 File Offset: 0x00259B26
			public Annotation Annotation
			{
				get
				{
					return this._annotation;
				}
				set
				{
					this._annotation = value;
				}
			}

			// Token: 0x17001FA2 RID: 8098
			// (get) Token: 0x06008E41 RID: 36417 RVA: 0x0025B92F File Offset: 0x00259B2F
			// (set) Token: 0x06008E42 RID: 36418 RVA: 0x0025B937 File Offset: 0x00259B37
			public bool Dirty
			{
				get
				{
					return this._dirty;
				}
				set
				{
					this._dirty = value;
				}
			}

			// Token: 0x04004B79 RID: 19321
			private Annotation _annotation;

			// Token: 0x04004B7A RID: 19322
			private bool _dirty;
		}
	}
}
