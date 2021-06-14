using System;
using System.IO;
using Microsoft.Data.OData.VerboseJson;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x0200016F RID: 367
	internal class BufferingJsonReader : JsonReader
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x00021DF4 File Offset: 0x0001FFF4
		internal BufferingJsonReader(TextReader reader, string inStreamErrorPropertyName, int maxInnerErrorDepth, ODataFormat jsonFormat) : base(reader, jsonFormat)
		{
			this.inStreamErrorPropertyName = inStreamErrorPropertyName;
			this.maxInnerErrorDepth = maxInnerErrorDepth;
			this.bufferedNodesHead = null;
			this.currentBufferedNode = null;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00021E1B File Offset: 0x0002001B
		public override JsonNodeType NodeType
		{
			get
			{
				if (this.bufferedNodesHead == null)
				{
					return base.NodeType;
				}
				if (this.isBuffering)
				{
					return this.currentBufferedNode.NodeType;
				}
				return this.bufferedNodesHead.NodeType;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00021E4B File Offset: 0x0002004B
		public override object Value
		{
			get
			{
				if (this.bufferedNodesHead == null)
				{
					return base.Value;
				}
				if (this.isBuffering)
				{
					return this.currentBufferedNode.Value;
				}
				return this.bufferedNodesHead.Value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00021E7B File Offset: 0x0002007B
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x00021E83 File Offset: 0x00020083
		internal bool DisableInStreamErrorDetection
		{
			get
			{
				return this.disableInStreamErrorDetection;
			}
			set
			{
				this.disableInStreamErrorDetection = value;
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00021E8C File Offset: 0x0002008C
		public override bool Read()
		{
			return this.ReadInternal();
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00021E94 File Offset: 0x00020094
		internal void StartBuffering()
		{
			if (this.bufferedNodesHead == null)
			{
				this.bufferedNodesHead = new BufferingJsonReader.BufferedNode(base.NodeType, base.Value);
			}
			else
			{
				this.removeOnNextRead = false;
			}
			if (this.currentBufferedNode == null)
			{
				this.currentBufferedNode = this.bufferedNodesHead;
			}
			this.isBuffering = true;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00021EE4 File Offset: 0x000200E4
		internal object BookmarkCurrentPosition()
		{
			return this.currentBufferedNode;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00021EEC File Offset: 0x000200EC
		internal void MoveToBookmark(object bookmark)
		{
			BufferingJsonReader.BufferedNode bufferedNode = bookmark as BufferingJsonReader.BufferedNode;
			this.currentBufferedNode = bufferedNode;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00021F07 File Offset: 0x00020107
		internal void StopBuffering()
		{
			this.isBuffering = false;
			this.removeOnNextRead = true;
			this.currentBufferedNode = null;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00021F20 File Offset: 0x00020120
		internal bool StartBufferingAndTryToReadInStreamErrorPropertyValue(out ODataError error)
		{
			error = null;
			this.StartBuffering();
			this.parsingInStreamError = true;
			bool result;
			try
			{
				result = this.TryReadInStreamErrorPropertyValue(out error);
			}
			finally
			{
				this.StopBuffering();
				this.parsingInStreamError = false;
			}
			return result;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00021F68 File Offset: 0x00020168
		protected bool ReadInternal()
		{
			if (this.removeOnNextRead)
			{
				this.RemoveFirstNodeInBuffer();
				this.removeOnNextRead = false;
			}
			bool result;
			if (this.isBuffering)
			{
				if (this.currentBufferedNode.Next != this.bufferedNodesHead)
				{
					this.currentBufferedNode = this.currentBufferedNode.Next;
					result = true;
				}
				else if (this.parsingInStreamError)
				{
					result = base.Read();
					BufferingJsonReader.BufferedNode bufferedNode = new BufferingJsonReader.BufferedNode(base.NodeType, base.Value);
					bufferedNode.Previous = this.bufferedNodesHead.Previous;
					bufferedNode.Next = this.bufferedNodesHead;
					this.bufferedNodesHead.Previous.Next = bufferedNode;
					this.bufferedNodesHead.Previous = bufferedNode;
					this.currentBufferedNode = bufferedNode;
				}
				else
				{
					result = this.ReadNextAndCheckForInStreamError();
				}
			}
			else if (this.bufferedNodesHead == null)
			{
				result = (this.parsingInStreamError ? base.Read() : this.ReadNextAndCheckForInStreamError());
			}
			else
			{
				result = (this.bufferedNodesHead.NodeType != JsonNodeType.EndOfInput);
				this.removeOnNextRead = true;
			}
			return result;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00022068 File Offset: 0x00020268
		protected virtual void ProcessObjectValue()
		{
			ODataError error = null;
			if (!this.DisableInStreamErrorDetection)
			{
				this.ReadInternal();
				bool flag = false;
				while (this.currentBufferedNode.NodeType == JsonNodeType.Property)
				{
					string strB = (string)this.currentBufferedNode.Value;
					if (string.CompareOrdinal(this.inStreamErrorPropertyName, strB) != 0 || flag)
					{
						return;
					}
					flag = true;
					this.ReadInternal();
					if (!this.TryReadInStreamErrorPropertyValue(out error))
					{
						return;
					}
				}
				if (flag)
				{
					throw new ODataErrorException(error);
				}
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000220DC File Offset: 0x000202DC
		private bool ReadNextAndCheckForInStreamError()
		{
			this.parsingInStreamError = true;
			bool result;
			try
			{
				bool flag = this.ReadInternal();
				if (base.NodeType == JsonNodeType.StartObject)
				{
					bool flag2 = this.isBuffering;
					BufferingJsonReader.BufferedNode bufferedNode = null;
					if (this.isBuffering)
					{
						bufferedNode = this.currentBufferedNode;
					}
					else
					{
						this.StartBuffering();
					}
					this.ProcessObjectValue();
					if (flag2)
					{
						this.currentBufferedNode = bufferedNode;
					}
					else
					{
						this.StopBuffering();
					}
				}
				result = flag;
			}
			finally
			{
				this.parsingInStreamError = false;
			}
			return result;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00022158 File Offset: 0x00020358
		private bool TryReadInStreamErrorPropertyValue(out ODataError error)
		{
			error = null;
			if (this.currentBufferedNode.NodeType != JsonNodeType.StartObject)
			{
				return false;
			}
			this.ReadInternal();
			error = new ODataError();
			ODataVerboseJsonReaderUtils.ErrorPropertyBitMask errorPropertyBitMask = ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
			while (this.currentBufferedNode.NodeType == JsonNodeType.Property)
			{
				string text = (string)this.currentBufferedNode.Value;
				string a;
				if ((a = text) != null)
				{
					if (!(a == "code"))
					{
						if (!(a == "message"))
						{
							if (!(a == "innererror"))
							{
								return false;
							}
							if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.InnerError))
							{
								return false;
							}
							ODataInnerError innerError;
							if (!this.TryReadInnerErrorPropertyValue(out innerError, 0))
							{
								return false;
							}
							error.InnerError = innerError;
						}
						else
						{
							if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.Message))
							{
								return false;
							}
							if (!this.TryReadMessagePropertyValue(error))
							{
								return false;
							}
						}
					}
					else
					{
						if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.Code))
						{
							return false;
						}
						string errorCode;
						if (!this.TryReadErrorStringPropertyValue(out errorCode))
						{
							return false;
						}
						error.ErrorCode = errorCode;
					}
					this.ReadInternal();
					continue;
				}
				return false;
			}
			this.ReadInternal();
			return errorPropertyBitMask != ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002225C File Offset: 0x0002045C
		private bool TryReadMessagePropertyValue(ODataError error)
		{
			this.ReadInternal();
			if (this.currentBufferedNode.NodeType != JsonNodeType.StartObject)
			{
				return false;
			}
			this.ReadInternal();
			ODataVerboseJsonReaderUtils.ErrorPropertyBitMask errorPropertyBitMask = ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
			while (this.currentBufferedNode.NodeType == JsonNodeType.Property)
			{
				string text = (string)this.currentBufferedNode.Value;
				string a;
				if ((a = text) != null)
				{
					if (!(a == "lang"))
					{
						if (!(a == "value"))
						{
							return false;
						}
						if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageValue))
						{
							return false;
						}
						string message;
						if (!this.TryReadErrorStringPropertyValue(out message))
						{
							return false;
						}
						error.Message = message;
					}
					else
					{
						if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageLanguage))
						{
							return false;
						}
						string messageLanguage;
						if (!this.TryReadErrorStringPropertyValue(out messageLanguage))
						{
							return false;
						}
						error.MessageLanguage = messageLanguage;
					}
					this.ReadInternal();
					continue;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00022324 File Offset: 0x00020524
		private bool TryReadInnerErrorPropertyValue(out ODataInnerError innerError, int recursionDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, this.maxInnerErrorDepth);
			this.ReadInternal();
			if (this.currentBufferedNode.NodeType != JsonNodeType.StartObject)
			{
				innerError = null;
				return false;
			}
			this.ReadInternal();
			innerError = new ODataInnerError();
			ODataVerboseJsonReaderUtils.ErrorPropertyBitMask errorPropertyBitMask = ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.None;
			while (this.currentBufferedNode.NodeType == JsonNodeType.Property)
			{
				string text = (string)this.currentBufferedNode.Value;
				string a;
				if ((a = text) == null)
				{
					goto IL_125;
				}
				if (!(a == "message"))
				{
					if (!(a == "type"))
					{
						if (!(a == "stacktrace"))
						{
							if (!(a == "internalexception"))
							{
								goto IL_125;
							}
							if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.InnerError))
							{
								return false;
							}
							ODataInnerError innerError2;
							if (!this.TryReadInnerErrorPropertyValue(out innerError2, recursionDepth))
							{
								return false;
							}
							innerError.InnerError = innerError2;
						}
						else
						{
							if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.StackTrace))
							{
								return false;
							}
							string stackTrace;
							if (!this.TryReadErrorStringPropertyValue(out stackTrace))
							{
								return false;
							}
							innerError.StackTrace = stackTrace;
						}
					}
					else
					{
						if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.TypeName))
						{
							return false;
						}
						string typeName;
						if (!this.TryReadErrorStringPropertyValue(out typeName))
						{
							return false;
						}
						innerError.TypeName = typeName;
					}
				}
				else
				{
					if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref errorPropertyBitMask, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask.MessageValue))
					{
						return false;
					}
					string message;
					if (!this.TryReadErrorStringPropertyValue(out message))
					{
						return false;
					}
					innerError.Message = message;
				}
				IL_12B:
				this.ReadInternal();
				continue;
				IL_125:
				this.SkipValueInternal();
				goto IL_12B;
			}
			return true;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00022478 File Offset: 0x00020678
		private bool TryReadErrorStringPropertyValue(out string stringValue)
		{
			this.ReadInternal();
			stringValue = (this.currentBufferedNode.Value as string);
			return this.currentBufferedNode.NodeType == JsonNodeType.PrimitiveValue && (this.currentBufferedNode.Value == null || stringValue != null);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000224C8 File Offset: 0x000206C8
		private void SkipValueInternal()
		{
			int num = 0;
			do
			{
				switch (this.currentBufferedNode.NodeType)
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
				this.ReadInternal();
			}
			while (num > 0);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0002251C File Offset: 0x0002071C
		private void RemoveFirstNodeInBuffer()
		{
			if (this.bufferedNodesHead.Next == this.bufferedNodesHead)
			{
				this.bufferedNodesHead = null;
				return;
			}
			this.bufferedNodesHead.Previous.Next = this.bufferedNodesHead.Next;
			this.bufferedNodesHead.Next.Previous = this.bufferedNodesHead.Previous;
			this.bufferedNodesHead = this.bufferedNodesHead.Next;
		}

		// Token: 0x040003DB RID: 987
		protected BufferingJsonReader.BufferedNode bufferedNodesHead;

		// Token: 0x040003DC RID: 988
		protected BufferingJsonReader.BufferedNode currentBufferedNode;

		// Token: 0x040003DD RID: 989
		private readonly int maxInnerErrorDepth;

		// Token: 0x040003DE RID: 990
		private readonly string inStreamErrorPropertyName;

		// Token: 0x040003DF RID: 991
		private bool isBuffering;

		// Token: 0x040003E0 RID: 992
		private bool removeOnNextRead;

		// Token: 0x040003E1 RID: 993
		private bool parsingInStreamError;

		// Token: 0x040003E2 RID: 994
		private bool disableInStreamErrorDetection;

		// Token: 0x02000170 RID: 368
		protected internal sealed class BufferedNode
		{
			// Token: 0x06000A6D RID: 2669 RVA: 0x0002258B File Offset: 0x0002078B
			internal BufferedNode(JsonNodeType nodeType, object value)
			{
				this.nodeType = nodeType;
				this.nodeValue = value;
				this.Previous = this;
				this.Next = this;
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000225AF File Offset: 0x000207AF
			internal JsonNodeType NodeType
			{
				get
				{
					return this.nodeType;
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000225B7 File Offset: 0x000207B7
			internal object Value
			{
				get
				{
					return this.nodeValue;
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06000A70 RID: 2672 RVA: 0x000225BF File Offset: 0x000207BF
			// (set) Token: 0x06000A71 RID: 2673 RVA: 0x000225C7 File Offset: 0x000207C7
			internal BufferingJsonReader.BufferedNode Previous { get; set; }

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06000A72 RID: 2674 RVA: 0x000225D0 File Offset: 0x000207D0
			// (set) Token: 0x06000A73 RID: 2675 RVA: 0x000225D8 File Offset: 0x000207D8
			internal BufferingJsonReader.BufferedNode Next { get; set; }

			// Token: 0x040003E3 RID: 995
			private readonly JsonNodeType nodeType;

			// Token: 0x040003E4 RID: 996
			private readonly object nodeValue;
		}
	}
}
