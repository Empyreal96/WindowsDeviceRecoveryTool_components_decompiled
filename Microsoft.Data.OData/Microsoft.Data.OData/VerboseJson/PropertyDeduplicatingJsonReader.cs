using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000177 RID: 375
	internal sealed class PropertyDeduplicatingJsonReader : BufferingJsonReader
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00023557 File Offset: 0x00021757
		internal PropertyDeduplicatingJsonReader(TextReader reader, int maxInnerErrorDepth) : base(reader, "error", maxInnerErrorDepth, ODataFormat.VerboseJson)
		{
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002356C File Offset: 0x0002176C
		protected override void ProcessObjectValue()
		{
			Stack<PropertyDeduplicatingJsonReader.ObjectRecordPropertyDeduplicationRecord> stack = new Stack<PropertyDeduplicatingJsonReader.ObjectRecordPropertyDeduplicationRecord>();
			for (;;)
			{
				if (this.currentBufferedNode.NodeType == JsonNodeType.StartObject)
				{
					stack.Push(new PropertyDeduplicatingJsonReader.ObjectRecordPropertyDeduplicationRecord());
					BufferingJsonReader.BufferedNode currentBufferedNode = this.currentBufferedNode;
					base.ProcessObjectValue();
					this.currentBufferedNode = currentBufferedNode;
				}
				else if (this.currentBufferedNode.NodeType == JsonNodeType.EndObject)
				{
					PropertyDeduplicatingJsonReader.ObjectRecordPropertyDeduplicationRecord objectRecordPropertyDeduplicationRecord = stack.Pop();
					if (objectRecordPropertyDeduplicationRecord.CurrentPropertyRecord != null)
					{
						objectRecordPropertyDeduplicationRecord.CurrentPropertyRecord.LastPropertyValueNode = this.currentBufferedNode.Previous;
					}
					foreach (List<PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord> list in objectRecordPropertyDeduplicationRecord.Values)
					{
						if (list.Count > 1)
						{
							PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord propertyDeduplicationRecord = list[0];
							for (int i = 1; i < list.Count; i++)
							{
								PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord propertyDeduplicationRecord2 = list[i];
								propertyDeduplicationRecord2.PropertyNode.Previous.Next = propertyDeduplicationRecord2.LastPropertyValueNode.Next;
								propertyDeduplicationRecord2.LastPropertyValueNode.Next.Previous = propertyDeduplicationRecord2.PropertyNode.Previous;
								propertyDeduplicationRecord.PropertyNode.Previous.Next = propertyDeduplicationRecord2.PropertyNode;
								propertyDeduplicationRecord2.PropertyNode.Previous = propertyDeduplicationRecord.PropertyNode.Previous;
								propertyDeduplicationRecord.LastPropertyValueNode.Next.Previous = propertyDeduplicationRecord2.LastPropertyValueNode;
								propertyDeduplicationRecord2.LastPropertyValueNode.Next = propertyDeduplicationRecord.LastPropertyValueNode.Next;
								propertyDeduplicationRecord = propertyDeduplicationRecord2;
							}
						}
					}
					if (stack.Count == 0)
					{
						break;
					}
				}
				else if (this.currentBufferedNode.NodeType == JsonNodeType.Property)
				{
					PropertyDeduplicatingJsonReader.ObjectRecordPropertyDeduplicationRecord objectRecordPropertyDeduplicationRecord2 = stack.Peek();
					if (objectRecordPropertyDeduplicationRecord2.CurrentPropertyRecord != null)
					{
						objectRecordPropertyDeduplicationRecord2.CurrentPropertyRecord.LastPropertyValueNode = this.currentBufferedNode.Previous;
					}
					objectRecordPropertyDeduplicationRecord2.CurrentPropertyRecord = new PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord(this.currentBufferedNode);
					string key = (string)this.currentBufferedNode.Value;
					List<PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord> list2;
					if (!objectRecordPropertyDeduplicationRecord2.TryGetValue(key, out list2))
					{
						list2 = new List<PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord>();
						objectRecordPropertyDeduplicationRecord2.Add(key, list2);
					}
					list2.Add(objectRecordPropertyDeduplicationRecord2.CurrentPropertyRecord);
				}
				if (!base.ReadInternal())
				{
					return;
				}
			}
		}

		// Token: 0x02000178 RID: 376
		private sealed class ObjectRecordPropertyDeduplicationRecord : Dictionary<string, List<PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord>>
		{
			// Token: 0x1700027D RID: 637
			// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x000237A4 File Offset: 0x000219A4
			// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x000237AC File Offset: 0x000219AC
			internal PropertyDeduplicatingJsonReader.PropertyDeduplicationRecord CurrentPropertyRecord { get; set; }
		}

		// Token: 0x02000179 RID: 377
		private sealed class PropertyDeduplicationRecord
		{
			// Token: 0x06000AA6 RID: 2726 RVA: 0x000237BD File Offset: 0x000219BD
			internal PropertyDeduplicationRecord(BufferingJsonReader.BufferedNode propertyNode)
			{
				this.propertyNode = propertyNode;
			}

			// Token: 0x1700027E RID: 638
			// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x000237CC File Offset: 0x000219CC
			internal BufferingJsonReader.BufferedNode PropertyNode
			{
				get
				{
					return this.propertyNode;
				}
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x000237D4 File Offset: 0x000219D4
			// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x000237DC File Offset: 0x000219DC
			internal BufferingJsonReader.BufferedNode LastPropertyValueNode
			{
				get
				{
					return this.lastPropertyValueNode;
				}
				set
				{
					this.lastPropertyValueNode = value;
				}
			}

			// Token: 0x040003F5 RID: 1013
			private readonly BufferingJsonReader.BufferedNode propertyNode;

			// Token: 0x040003F6 RID: 1014
			private BufferingJsonReader.BufferedNode lastPropertyValueNode;
		}
	}
}
