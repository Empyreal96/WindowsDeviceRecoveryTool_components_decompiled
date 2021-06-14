using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000171 RID: 369
	internal sealed class ReorderingJsonReader : BufferingJsonReader
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x000225E1 File Offset: 0x000207E1
		internal ReorderingJsonReader(TextReader reader, int maxInnerErrorDepth) : base(reader, "odata.error", maxInnerErrorDepth, ODataFormat.Json)
		{
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x000225F8 File Offset: 0x000207F8
		protected override void ProcessObjectValue()
		{
			Stack<ReorderingJsonReader.BufferedObject> stack = new Stack<ReorderingJsonReader.BufferedObject>();
			for (;;)
			{
				switch (this.currentBufferedNode.NodeType)
				{
				case JsonNodeType.StartObject:
				{
					ReorderingJsonReader.BufferedObject bufferedObject = new ReorderingJsonReader.BufferedObject
					{
						ObjectStart = this.currentBufferedNode
					};
					stack.Push(bufferedObject);
					base.ProcessObjectValue();
					this.currentBufferedNode = bufferedObject.ObjectStart;
					base.ReadInternal();
					continue;
				}
				case JsonNodeType.EndObject:
				{
					ReorderingJsonReader.BufferedObject bufferedObject2 = stack.Pop();
					if (bufferedObject2.CurrentProperty != null)
					{
						bufferedObject2.CurrentProperty.EndOfPropertyValueNode = this.currentBufferedNode.Previous;
					}
					bufferedObject2.Reorder();
					if (stack.Count == 0)
					{
						return;
					}
					base.ReadInternal();
					continue;
				}
				case JsonNodeType.Property:
				{
					ReorderingJsonReader.BufferedObject bufferedObject3 = stack.Peek();
					if (bufferedObject3.CurrentProperty != null)
					{
						bufferedObject3.CurrentProperty.EndOfPropertyValueNode = this.currentBufferedNode.Previous;
					}
					ReorderingJsonReader.BufferedProperty bufferedProperty = new ReorderingJsonReader.BufferedProperty();
					bufferedProperty.PropertyNameNode = this.currentBufferedNode;
					string propertyName;
					string text;
					this.ReadPropertyName(out propertyName, out text);
					bufferedProperty.PropertyAnnotationName = text;
					bufferedObject3.AddBufferedProperty(propertyName, text, bufferedProperty);
					if (text != null)
					{
						this.BufferValue();
						continue;
					}
					continue;
				}
				}
				base.ReadInternal();
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002272C File Offset: 0x0002092C
		private void ReadPropertyName(out string propertyName, out string annotationName)
		{
			string propertyName2 = this.GetPropertyName();
			base.ReadInternal();
			int num = propertyName2.IndexOf('@');
			if (num >= 0)
			{
				propertyName = propertyName2.Substring(0, num);
				annotationName = propertyName2.Substring(num + 1);
				return;
			}
			int num2 = propertyName2.IndexOf('.');
			if (num2 < 0)
			{
				propertyName = propertyName2;
				annotationName = null;
				return;
			}
			propertyName = null;
			annotationName = propertyName2;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00022784 File Offset: 0x00020984
		private void BufferValue()
		{
			int num = 0;
			do
			{
				switch (this.NodeType)
				{
				case JsonNodeType.StartObject:
				case JsonNodeType.StartArray:
					num++;
					break;
				case JsonNodeType.EndObject:
				case JsonNodeType.EndArray:
					num--;
					break;
				}
				base.ReadInternal();
			}
			while (num > 0);
		}

		// Token: 0x02000172 RID: 370
		private sealed class BufferedObject
		{
			// Token: 0x06000A78 RID: 2680 RVA: 0x000227D1 File Offset: 0x000209D1
			internal BufferedObject()
			{
				this.propertyNamesWithAnnotations = new List<KeyValuePair<string, string>>();
				this.dataProperties = new HashSet<string>(StringComparer.Ordinal);
				this.propertyCache = new Dictionary<string, object>(StringComparer.Ordinal);
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00022804 File Offset: 0x00020A04
			// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0002280C File Offset: 0x00020A0C
			internal BufferingJsonReader.BufferedNode ObjectStart { get; set; }

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00022815 File Offset: 0x00020A15
			// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0002281D File Offset: 0x00020A1D
			internal ReorderingJsonReader.BufferedProperty CurrentProperty { get; private set; }

			// Token: 0x06000A7D RID: 2685 RVA: 0x00022828 File Offset: 0x00020A28
			internal void AddBufferedProperty(string propertyName, string annotationName, ReorderingJsonReader.BufferedProperty bufferedProperty)
			{
				this.CurrentProperty = bufferedProperty;
				string key = propertyName ?? annotationName;
				if (propertyName == null)
				{
					this.propertyNamesWithAnnotations.Add(new KeyValuePair<string, string>(annotationName, null));
				}
				else if (!this.dataProperties.Contains(propertyName))
				{
					if (annotationName == null)
					{
						this.dataProperties.Add(propertyName);
					}
					this.propertyNamesWithAnnotations.Add(new KeyValuePair<string, string>(propertyName, annotationName));
				}
				object obj;
				if (this.propertyCache.TryGetValue(key, out obj))
				{
					ReorderingJsonReader.BufferedProperty bufferedProperty2 = obj as ReorderingJsonReader.BufferedProperty;
					List<ReorderingJsonReader.BufferedProperty> list;
					if (bufferedProperty2 != null)
					{
						list = new List<ReorderingJsonReader.BufferedProperty>(4);
						list.Add(bufferedProperty2);
						this.propertyCache[key] = list;
					}
					else
					{
						list = (List<ReorderingJsonReader.BufferedProperty>)obj;
					}
					list.Add(bufferedProperty);
					return;
				}
				this.propertyCache.Add(key, bufferedProperty);
			}

			// Token: 0x06000A7E RID: 2686 RVA: 0x000228E0 File Offset: 0x00020AE0
			internal void Reorder()
			{
				BufferingJsonReader.BufferedNode node = this.ObjectStart;
				IEnumerable<string> enumerable = this.SortPropertyNames();
				foreach (string key in enumerable)
				{
					object obj = this.propertyCache[key];
					ReorderingJsonReader.BufferedProperty bufferedProperty = obj as ReorderingJsonReader.BufferedProperty;
					if (bufferedProperty != null)
					{
						bufferedProperty.InsertAfter(node);
						node = bufferedProperty.EndOfPropertyValueNode;
					}
					else
					{
						IEnumerable<ReorderingJsonReader.BufferedProperty> enumerable2 = ReorderingJsonReader.BufferedObject.SortBufferedProperties((IList<ReorderingJsonReader.BufferedProperty>)obj);
						foreach (ReorderingJsonReader.BufferedProperty bufferedProperty2 in enumerable2)
						{
							bufferedProperty2.InsertAfter(node);
							node = bufferedProperty2.EndOfPropertyValueNode;
						}
					}
				}
			}

			// Token: 0x06000A7F RID: 2687 RVA: 0x00022B84 File Offset: 0x00020D84
			private static IEnumerable<ReorderingJsonReader.BufferedProperty> SortBufferedProperties(IList<ReorderingJsonReader.BufferedProperty> bufferedProperties)
			{
				List<ReorderingJsonReader.BufferedProperty> delayedProperties = null;
				for (int i = 0; i < bufferedProperties.Count; i++)
				{
					ReorderingJsonReader.BufferedProperty bufferedProperty = bufferedProperties[i];
					string annotationName = bufferedProperty.PropertyAnnotationName;
					if (annotationName == null || !ReorderingJsonReader.BufferedObject.IsODataInstanceAnnotation(annotationName))
					{
						if (delayedProperties == null)
						{
							delayedProperties = new List<ReorderingJsonReader.BufferedProperty>();
						}
						delayedProperties.Add(bufferedProperty);
					}
					else
					{
						yield return bufferedProperty;
					}
				}
				if (delayedProperties != null)
				{
					for (int j = 0; j < delayedProperties.Count; j++)
					{
						yield return delayedProperties[j];
					}
				}
				yield break;
			}

			// Token: 0x06000A80 RID: 2688 RVA: 0x00022BA1 File Offset: 0x00020DA1
			private static bool IsODataInstanceAnnotation(string annotationName)
			{
				return annotationName.StartsWith("odata.", StringComparison.Ordinal);
			}

			// Token: 0x06000A81 RID: 2689 RVA: 0x00022BAF File Offset: 0x00020DAF
			private static bool IsODataMetadataAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.metadata", annotationName) == 0;
			}

			// Token: 0x06000A82 RID: 2690 RVA: 0x00022BBF File Offset: 0x00020DBF
			private static bool IsODataAnnotationGroupReferenceAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.annotationGroupReference", annotationName) == 0;
			}

			// Token: 0x06000A83 RID: 2691 RVA: 0x00022BCF File Offset: 0x00020DCF
			private static bool IsODataAnnotationGroupAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.annotationGroup", annotationName) == 0;
			}

			// Token: 0x06000A84 RID: 2692 RVA: 0x00022BDF File Offset: 0x00020DDF
			private static bool IsODataTypeAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.type", annotationName) == 0;
			}

			// Token: 0x06000A85 RID: 2693 RVA: 0x00022BEF File Offset: 0x00020DEF
			private static bool IsODataIdAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.id", annotationName) == 0;
			}

			// Token: 0x06000A86 RID: 2694 RVA: 0x00022BFF File Offset: 0x00020DFF
			private static bool IsODataETagAnnotation(string annotationName)
			{
				return string.CompareOrdinal("odata.etag", annotationName) == 0;
			}

			// Token: 0x06000A87 RID: 2695 RVA: 0x000230F8 File Offset: 0x000212F8
			private IEnumerable<string> SortPropertyNames()
			{
				string metadataAnnotationName = null;
				string typeAnnotationName = null;
				string idAnnotationName = null;
				string etagAnnotationName = null;
				string annotationGroupDeclarationName = null;
				string annotationGroupReferenceName = null;
				List<string> odataAnnotationNames = null;
				List<string> otherNames = null;
				foreach (KeyValuePair<string, string> keyValuePair in this.propertyNamesWithAnnotations)
				{
					string key = keyValuePair.Key;
					if (keyValuePair.Value == null || !this.dataProperties.Contains(key))
					{
						this.dataProperties.Add(key);
						if (ReorderingJsonReader.BufferedObject.IsODataMetadataAnnotation(key))
						{
							metadataAnnotationName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataAnnotationGroupAnnotation(key))
						{
							annotationGroupDeclarationName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataAnnotationGroupReferenceAnnotation(key))
						{
							annotationGroupReferenceName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataTypeAnnotation(key))
						{
							typeAnnotationName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataIdAnnotation(key))
						{
							idAnnotationName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataETagAnnotation(key))
						{
							etagAnnotationName = key;
						}
						else if (ReorderingJsonReader.BufferedObject.IsODataInstanceAnnotation(key))
						{
							if (odataAnnotationNames == null)
							{
								odataAnnotationNames = new List<string>();
							}
							odataAnnotationNames.Add(key);
						}
						else
						{
							if (otherNames == null)
							{
								otherNames = new List<string>();
							}
							otherNames.Add(key);
						}
					}
				}
				if (metadataAnnotationName != null)
				{
					yield return metadataAnnotationName;
				}
				if (annotationGroupDeclarationName != null)
				{
					yield return annotationGroupDeclarationName;
				}
				if (annotationGroupReferenceName != null)
				{
					yield return annotationGroupReferenceName;
				}
				if (typeAnnotationName != null)
				{
					yield return typeAnnotationName;
				}
				if (idAnnotationName != null)
				{
					yield return idAnnotationName;
				}
				if (etagAnnotationName != null)
				{
					yield return etagAnnotationName;
				}
				if (odataAnnotationNames != null)
				{
					foreach (string propertyName in odataAnnotationNames)
					{
						yield return propertyName;
					}
				}
				if (otherNames != null)
				{
					foreach (string propertyName2 in otherNames)
					{
						yield return propertyName2;
					}
				}
				yield break;
			}

			// Token: 0x040003E7 RID: 999
			private readonly Dictionary<string, object> propertyCache;

			// Token: 0x040003E8 RID: 1000
			private readonly HashSet<string> dataProperties;

			// Token: 0x040003E9 RID: 1001
			private readonly List<KeyValuePair<string, string>> propertyNamesWithAnnotations;
		}

		// Token: 0x02000173 RID: 371
		private sealed class BufferedProperty
		{
			// Token: 0x17000278 RID: 632
			// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00023115 File Offset: 0x00021315
			// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0002311D File Offset: 0x0002131D
			internal string PropertyAnnotationName { get; set; }

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00023126 File Offset: 0x00021326
			// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0002312E File Offset: 0x0002132E
			internal BufferingJsonReader.BufferedNode PropertyNameNode { get; set; }

			// Token: 0x1700027A RID: 634
			// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00023137 File Offset: 0x00021337
			// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0002313F File Offset: 0x0002133F
			internal BufferingJsonReader.BufferedNode EndOfPropertyValueNode { get; set; }

			// Token: 0x06000A8E RID: 2702 RVA: 0x00023148 File Offset: 0x00021348
			internal void InsertAfter(BufferingJsonReader.BufferedNode node)
			{
				BufferingJsonReader.BufferedNode previous = this.PropertyNameNode.Previous;
				BufferingJsonReader.BufferedNode next = this.EndOfPropertyValueNode.Next;
				previous.Next = next;
				next.Previous = previous;
				next = node.Next;
				node.Next = this.PropertyNameNode;
				this.PropertyNameNode.Previous = node;
				this.EndOfPropertyValueNode.Next = next;
				if (next != null)
				{
					next.Previous = this.EndOfPropertyValueNode;
				}
			}
		}
	}
}
