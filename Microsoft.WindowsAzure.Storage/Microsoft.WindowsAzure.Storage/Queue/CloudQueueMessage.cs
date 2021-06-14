using System;
using System.Globalization;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x02000036 RID: 54
	public sealed class CloudQueueMessage
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x00025E3F File Offset: 0x0002403F
		public CloudQueueMessage(byte[] content)
		{
			this.SetMessageContent(content);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00025E4E File Offset: 0x0002404E
		public void SetMessageContent(byte[] content)
		{
			this.RawBytes = content;
			this.MessageType = QueueMessageType.RawBytes;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00025E5E File Offset: 0x0002405E
		public static long MaxMessageSize
		{
			get
			{
				return 65536L;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00025E66 File Offset: 0x00024066
		public static TimeSpan MaxTimeToLive
		{
			get
			{
				return CloudQueueMessage.MaximumTimeToLive;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00025E6D File Offset: 0x0002406D
		public static int MaxNumberOfMessagesToPeek
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00025E71 File Offset: 0x00024071
		internal CloudQueueMessage()
		{
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00025E79 File Offset: 0x00024079
		public CloudQueueMessage(string content)
		{
			this.SetMessageContent(content);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00025E88 File Offset: 0x00024088
		public CloudQueueMessage(string messageId, string popReceipt)
		{
			this.Id = messageId;
			this.PopReceipt = popReceipt;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00025E9E File Offset: 0x0002409E
		internal CloudQueueMessage(string content, bool isBase64Encoded)
		{
			if (content == null)
			{
				content = string.Empty;
			}
			this.RawString = content;
			this.MessageType = (isBase64Encoded ? QueueMessageType.Base64Encoded : QueueMessageType.RawString);
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x00025EC4 File Offset: 0x000240C4
		public byte[] AsBytes
		{
			get
			{
				if (this.MessageType == QueueMessageType.RawString)
				{
					return Encoding.UTF8.GetBytes(this.RawString);
				}
				if (this.MessageType == QueueMessageType.Base64Encoded)
				{
					return Convert.FromBase64String(this.RawString);
				}
				return this.RawBytes;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00025EFA File Offset: 0x000240FA
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00025F02 File Offset: 0x00024102
		public string Id { get; internal set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00025F0B File Offset: 0x0002410B
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x00025F13 File Offset: 0x00024113
		public string PopReceipt { get; internal set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00025F1C File Offset: 0x0002411C
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00025F24 File Offset: 0x00024124
		public DateTimeOffset? InsertionTime { get; internal set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00025F2D File Offset: 0x0002412D
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00025F35 File Offset: 0x00024135
		public DateTimeOffset? ExpirationTime { get; internal set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00025F3E File Offset: 0x0002413E
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x00025F46 File Offset: 0x00024146
		public DateTimeOffset? NextVisibleTime { get; internal set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00025F50 File Offset: 0x00024150
		public string AsString
		{
			get
			{
				if (this.MessageType == QueueMessageType.RawString)
				{
					return this.RawString;
				}
				if (this.MessageType == QueueMessageType.Base64Encoded)
				{
					byte[] array = Convert.FromBase64String(this.RawString);
					return CloudQueueMessage.utf8Encoder.GetString(array, 0, array.Length);
				}
				return CloudQueueMessage.utf8Encoder.GetString(this.RawBytes, 0, this.RawBytes.Length);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00025FAA File Offset: 0x000241AA
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x00025FB2 File Offset: 0x000241B2
		public int DequeueCount { get; internal set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00025FBB File Offset: 0x000241BB
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00025FC3 File Offset: 0x000241C3
		internal QueueMessageType MessageType { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00025FCC File Offset: 0x000241CC
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x00025FD4 File Offset: 0x000241D4
		internal string RawString { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00025FDD File Offset: 0x000241DD
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00025FE5 File Offset: 0x000241E5
		internal byte[] RawBytes { get; set; }

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00025FF0 File Offset: 0x000241F0
		internal string GetMessageContentForTransfer(bool shouldEncodeMessage, QueueRequestOptions options = null)
		{
			if (!shouldEncodeMessage && this.MessageType != QueueMessageType.RawString)
			{
				throw new ArgumentException("EncodeMessage should be true for binary message.");
			}
			if (options != null)
			{
				options.AssertPolicyIfRequired();
				if (options.EncryptionPolicy != null)
				{
					string text = options.EncryptionPolicy.EncryptMessage(this.AsBytes);
					if ((long)text.Length > CloudQueueMessage.MaxMessageSize)
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Encrypted Messages cannot be larger than {0} bytes. Please note that encrypting queue messages can increase their size.", new object[]
						{
							CloudQueueMessage.MaxMessageSize
						}));
					}
					return text;
				}
			}
			string text2;
			if (this.MessageType != QueueMessageType.Base64Encoded)
			{
				if (shouldEncodeMessage)
				{
					text2 = Convert.ToBase64String(this.AsBytes);
					if ((long)text2.Length > CloudQueueMessage.MaxMessageSize)
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Messages cannot be larger than {0} bytes.", new object[]
						{
							CloudQueueMessage.MaxMessageSize
						}));
					}
				}
				else
				{
					text2 = this.RawString;
					if ((long)Encoding.UTF8.GetBytes(text2).Length > CloudQueueMessage.MaxMessageSize)
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Messages cannot be larger than {0} bytes.", new object[]
						{
							CloudQueueMessage.MaxMessageSize
						}));
					}
				}
			}
			else
			{
				text2 = this.RawString;
				if ((long)text2.Length > CloudQueueMessage.MaxMessageSize)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Messages cannot be larger than {0} bytes.", new object[]
					{
						CloudQueueMessage.MaxMessageSize
					}));
				}
			}
			return text2;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00026154 File Offset: 0x00024354
		public void SetMessageContent(string content)
		{
			if (content == null)
			{
				content = string.Empty;
			}
			this.RawString = content;
			this.MessageType = QueueMessageType.RawString;
		}

		// Token: 0x0400013E RID: 318
		private const long MaximumMessageSize = 65536L;

		// Token: 0x0400013F RID: 319
		private const int MaximumNumberOfMessagesToPeek = 32;

		// Token: 0x04000140 RID: 320
		private static readonly TimeSpan MaximumTimeToLive = TimeSpan.FromDays(7.0);

		// Token: 0x04000141 RID: 321
		private static UTF8Encoding utf8Encoder = new UTF8Encoding(false, true);
	}
}
