using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Library.Annotations
{
	// Token: 0x02000049 RID: 73
	public class EdmDirectValueAnnotationsManager : IEdmDirectValueAnnotationsManager
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00003899 File Offset: 0x00001A99
		public EdmDirectValueAnnotationsManager()
		{
			this.annotationsDictionary = VersioningDictionary<IEdmElement, object>.Create(new Func<IEdmElement, IEdmElement, int>(this.CompareElements));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public IEnumerable<IEdmDirectValueAnnotation> GetDirectValueAnnotations(IEdmElement element)
		{
			VersioningDictionary<IEdmElement, object> annotationsDictionary = this.annotationsDictionary;
			IEnumerable<IEdmDirectValueAnnotation> immutableAnnotations = this.GetAttachedAnnotations(element);
			object transientAnnotations = EdmDirectValueAnnotationsManager.GetTransientAnnotations(element, annotationsDictionary);
			if (immutableAnnotations != null)
			{
				foreach (IEdmDirectValueAnnotation existingAnnotation in immutableAnnotations)
				{
					if (!EdmDirectValueAnnotationsManager.IsDead(existingAnnotation.NamespaceUri, existingAnnotation.Name, transientAnnotations))
					{
						yield return existingAnnotation;
					}
				}
			}
			foreach (IEdmDirectValueAnnotation existingAnnotation2 in EdmDirectValueAnnotationsManager.TransientAnnotations(transientAnnotations))
			{
				yield return existingAnnotation2;
			}
			yield break;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public void SetAnnotationValue(IEdmElement element, string namespaceName, string localName, object value)
		{
			lock (this.annotationsDictionaryLock)
			{
				VersioningDictionary<IEdmElement, object> versioningDictionary = this.annotationsDictionary;
				this.SetAnnotationValue(element, namespaceName, localName, value, ref versioningDictionary);
				this.annotationsDictionary = versioningDictionary;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003C20 File Offset: 0x00001E20
		public void SetAnnotationValues(IEnumerable<IEdmDirectValueAnnotationBinding> annotations)
		{
			lock (this.annotationsDictionaryLock)
			{
				VersioningDictionary<IEdmElement, object> versioningDictionary = this.annotationsDictionary;
				foreach (IEdmDirectValueAnnotationBinding edmDirectValueAnnotationBinding in annotations)
				{
					this.SetAnnotationValue(edmDirectValueAnnotationBinding.Element, edmDirectValueAnnotationBinding.NamespaceUri, edmDirectValueAnnotationBinding.Name, edmDirectValueAnnotationBinding.Value, ref versioningDictionary);
				}
				this.annotationsDictionary = versioningDictionary;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00003CC0 File Offset: 0x00001EC0
		public object GetAnnotationValue(IEdmElement element, string namespaceName, string localName)
		{
			VersioningDictionary<IEdmElement, object> versioningDictionary = this.annotationsDictionary;
			return this.GetAnnotationValue(element, namespaceName, localName, versioningDictionary);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public object[] GetAnnotationValues(IEnumerable<IEdmDirectValueAnnotationBinding> annotations)
		{
			VersioningDictionary<IEdmElement, object> versioningDictionary = this.annotationsDictionary;
			object[] array = new object[annotations.Count<IEdmDirectValueAnnotationBinding>()];
			int num = 0;
			foreach (IEdmDirectValueAnnotationBinding edmDirectValueAnnotationBinding in annotations)
			{
				array[num++] = this.GetAnnotationValue(edmDirectValueAnnotationBinding.Element, edmDirectValueAnnotationBinding.NamespaceUri, edmDirectValueAnnotationBinding.Name, versioningDictionary);
			}
			return array;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003D5C File Offset: 0x00001F5C
		protected virtual IEnumerable<IEdmDirectValueAnnotation> GetAttachedAnnotations(IEdmElement element)
		{
			return null;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D90 File Offset: 0x00001F90
		private static void SetAnnotation(IEnumerable<IEdmDirectValueAnnotation> immutableAnnotations, ref object transientAnnotations, string namespaceName, string localName, object value)
		{
			bool flag = false;
			if (immutableAnnotations != null)
			{
				if (immutableAnnotations.Any((IEdmDirectValueAnnotation existingAnnotation) => existingAnnotation.NamespaceUri == namespaceName && existingAnnotation.Name == localName))
				{
					flag = true;
				}
			}
			if (value == null && !flag)
			{
				EdmDirectValueAnnotationsManager.RemoveTransientAnnotation(ref transientAnnotations, namespaceName, localName);
				return;
			}
			if (namespaceName == "http://schemas.microsoft.com/ado/2011/04/edm/documentation" && value != null && !(value is IEdmDocumentation))
			{
				throw new InvalidOperationException(Strings.Annotations_DocumentationPun(value.GetType().Name));
			}
			IEdmDirectValueAnnotation edmDirectValueAnnotation = (value != null) ? new EdmDirectValueAnnotation(namespaceName, localName, value) : new EdmDirectValueAnnotation(namespaceName, localName);
			if (transientAnnotations == null)
			{
				transientAnnotations = edmDirectValueAnnotation;
				return;
			}
			IEdmDirectValueAnnotation edmDirectValueAnnotation2 = transientAnnotations as IEdmDirectValueAnnotation;
			if (edmDirectValueAnnotation2 == null)
			{
				VersioningList<IEdmDirectValueAnnotation> versioningList = (VersioningList<IEdmDirectValueAnnotation>)transientAnnotations;
				for (int i = 0; i < versioningList.Count; i++)
				{
					IEdmDirectValueAnnotation edmDirectValueAnnotation3 = versioningList[i];
					if (edmDirectValueAnnotation3.NamespaceUri == namespaceName && edmDirectValueAnnotation3.Name == localName)
					{
						versioningList = versioningList.RemoveAt(i);
						break;
					}
				}
				transientAnnotations = versioningList.Add(edmDirectValueAnnotation);
				return;
			}
			if (edmDirectValueAnnotation2.NamespaceUri == namespaceName && edmDirectValueAnnotation2.Name == localName)
			{
				transientAnnotations = edmDirectValueAnnotation;
				return;
			}
			transientAnnotations = VersioningList<IEdmDirectValueAnnotation>.Create().Add(edmDirectValueAnnotation2).Add(edmDirectValueAnnotation);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003F48 File Offset: 0x00002148
		private static IEdmDirectValueAnnotation FindTransientAnnotation(object transientAnnotations, string namespaceName, string localName)
		{
			if (transientAnnotations != null)
			{
				IEdmDirectValueAnnotation edmDirectValueAnnotation = transientAnnotations as IEdmDirectValueAnnotation;
				if (edmDirectValueAnnotation == null)
				{
					VersioningList<IEdmDirectValueAnnotation> source = (VersioningList<IEdmDirectValueAnnotation>)transientAnnotations;
					return source.FirstOrDefault((IEdmDirectValueAnnotation existingAnnotation) => existingAnnotation.NamespaceUri == namespaceName && existingAnnotation.Name == localName);
				}
				if (edmDirectValueAnnotation.NamespaceUri == namespaceName && edmDirectValueAnnotation.Name == localName)
				{
					return edmDirectValueAnnotation;
				}
			}
			return null;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003FC0 File Offset: 0x000021C0
		private static void RemoveTransientAnnotation(ref object transientAnnotations, string namespaceName, string localName)
		{
			if (transientAnnotations != null)
			{
				IEdmDirectValueAnnotation edmDirectValueAnnotation = transientAnnotations as IEdmDirectValueAnnotation;
				if (edmDirectValueAnnotation != null)
				{
					if (edmDirectValueAnnotation.NamespaceUri == namespaceName && edmDirectValueAnnotation.Name == localName)
					{
						transientAnnotations = null;
						return;
					}
				}
				else
				{
					VersioningList<IEdmDirectValueAnnotation> versioningList = (VersioningList<IEdmDirectValueAnnotation>)transientAnnotations;
					int i = 0;
					while (i < versioningList.Count)
					{
						IEdmDirectValueAnnotation edmDirectValueAnnotation2 = versioningList[i];
						if (edmDirectValueAnnotation2.NamespaceUri == namespaceName && edmDirectValueAnnotation2.Name == localName)
						{
							versioningList = versioningList.RemoveAt(i);
							if (versioningList.Count == 1)
							{
								transientAnnotations = versioningList.Single<IEdmDirectValueAnnotation>();
								return;
							}
							transientAnnotations = versioningList;
							return;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004264 File Offset: 0x00002464
		private static IEnumerable<IEdmDirectValueAnnotation> TransientAnnotations(object transientAnnotations)
		{
			if (transientAnnotations != null)
			{
				IEdmDirectValueAnnotation singleAnnotation = transientAnnotations as IEdmDirectValueAnnotation;
				if (singleAnnotation != null)
				{
					if (singleAnnotation.Value != null)
					{
						yield return singleAnnotation;
					}
				}
				else
				{
					VersioningList<IEdmDirectValueAnnotation> annotationsList = (VersioningList<IEdmDirectValueAnnotation>)transientAnnotations;
					foreach (IEdmDirectValueAnnotation existingAnnotation in annotationsList)
					{
						if (existingAnnotation.Value != null)
						{
							yield return existingAnnotation;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004281 File Offset: 0x00002481
		private static bool IsDead(string namespaceName, string localName, object transientAnnotations)
		{
			return EdmDirectValueAnnotationsManager.FindTransientAnnotation(transientAnnotations, namespaceName, localName) != null;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004294 File Offset: 0x00002494
		private static object GetTransientAnnotations(IEdmElement element, VersioningDictionary<IEdmElement, object> annotationsDictionary)
		{
			object result;
			annotationsDictionary.TryGetValue(element, out result);
			return result;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000042AC File Offset: 0x000024AC
		private void SetAnnotationValue(IEdmElement element, string namespaceName, string localName, object value, ref VersioningDictionary<IEdmElement, object> annotationsDictionary)
		{
			object transientAnnotations = EdmDirectValueAnnotationsManager.GetTransientAnnotations(element, annotationsDictionary);
			object obj = transientAnnotations;
			EdmDirectValueAnnotationsManager.SetAnnotation(this.GetAttachedAnnotations(element), ref transientAnnotations, namespaceName, localName, value);
			if (transientAnnotations != obj)
			{
				annotationsDictionary = annotationsDictionary.Set(element, transientAnnotations);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000042E8 File Offset: 0x000024E8
		private object GetAnnotationValue(IEdmElement element, string namespaceName, string localName, VersioningDictionary<IEdmElement, object> annotationsDictionary)
		{
			IEdmDirectValueAnnotation edmDirectValueAnnotation = EdmDirectValueAnnotationsManager.FindTransientAnnotation(EdmDirectValueAnnotationsManager.GetTransientAnnotations(element, annotationsDictionary), namespaceName, localName);
			if (edmDirectValueAnnotation != null)
			{
				return edmDirectValueAnnotation.Value;
			}
			IEnumerable<IEdmDirectValueAnnotation> attachedAnnotations = this.GetAttachedAnnotations(element);
			if (attachedAnnotations != null)
			{
				foreach (IEdmDirectValueAnnotation edmDirectValueAnnotation2 in attachedAnnotations)
				{
					if (edmDirectValueAnnotation2.NamespaceUri == namespaceName && edmDirectValueAnnotation2.Name == localName)
					{
						return edmDirectValueAnnotation2.Value;
					}
				}
			}
			return null;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000437C File Offset: 0x0000257C
		private int CompareElements(IEdmElement left, IEdmElement right)
		{
			if (left == right)
			{
				return 0;
			}
			int hashCode = left.GetHashCode();
			int hashCode2 = right.GetHashCode();
			if (hashCode < hashCode2)
			{
				return -1;
			}
			if (hashCode > hashCode2)
			{
				return 1;
			}
			IEdmNamedElement edmNamedElement = left as IEdmNamedElement;
			IEdmNamedElement edmNamedElement2 = right as IEdmNamedElement;
			if (edmNamedElement == null)
			{
				if (edmNamedElement2 != null)
				{
					return -1;
				}
			}
			else
			{
				if (edmNamedElement2 == null)
				{
					return 1;
				}
				int num = string.Compare(edmNamedElement.Name, edmNamedElement2.Name, StringComparison.Ordinal);
				if (num != 0)
				{
					return num;
				}
			}
			for (;;)
			{
				foreach (IEdmElement edmElement in this.unsortedElements)
				{
					if (edmElement == left)
					{
						return 1;
					}
					if (edmElement == right)
					{
						return -1;
					}
				}
				lock (this.unsortedElementsLock)
				{
					this.unsortedElements = this.unsortedElements.Add(left);
					continue;
				}
				break;
			}
			int result;
			return result;
		}

		// Token: 0x04000065 RID: 101
		private VersioningDictionary<IEdmElement, object> annotationsDictionary;

		// Token: 0x04000066 RID: 102
		private object annotationsDictionaryLock = new object();

		// Token: 0x04000067 RID: 103
		private VersioningList<IEdmElement> unsortedElements = VersioningList<IEdmElement>.Create();

		// Token: 0x04000068 RID: 104
		private object unsortedElementsLock = new object();
	}
}
