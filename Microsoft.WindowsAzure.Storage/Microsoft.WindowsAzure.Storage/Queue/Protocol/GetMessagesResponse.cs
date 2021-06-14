using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x020000FB RID: 251
	public sealed class GetMessagesResponse : ResponseParsingBase<QueueMessage>
	{
		// Token: 0x0600126E RID: 4718 RVA: 0x00044429 File Offset: 0x00042629
		public GetMessagesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x00044432 File Offset: 0x00042632
		public IEnumerable<QueueMessage> Messages
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0004443C File Offset: 0x0004263C
		private QueueMessage ParseMessageEntry()
		{
			string id = null;
			string popReceipt = null;
			DateTime? dateTime = null;
			DateTime? dateTime2 = null;
			DateTime? dateTime3 = null;
			string text = null;
			int dequeueCount = 0;
			this.reader.ReadStartElement();
			while (this.reader.IsStartElement())
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					string name;
					switch (name = this.reader.Name)
					{
					case "MessageId":
						id = this.reader.ReadElementContentAsString();
						continue;
					case "PopReceipt":
						popReceipt = this.reader.ReadElementContentAsString();
						continue;
					case "InsertionTime":
						dateTime = new DateTime?(this.reader.ReadElementContentAsString().ToUTCTime());
						continue;
					case "ExpirationTime":
						dateTime2 = new DateTime?(this.reader.ReadElementContentAsString().ToUTCTime());
						continue;
					case "TimeNextVisible":
						dateTime3 = new DateTime?(this.reader.ReadElementContentAsString().ToUTCTime());
						continue;
					case "MessageText":
						text = this.reader.ReadElementContentAsString();
						continue;
					case "DequeueCount":
						dequeueCount = this.reader.ReadElementContentAsInt();
						continue;
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			QueueMessage queueMessage = new QueueMessage();
			queueMessage.Text = text;
			queueMessage.Id = id;
			queueMessage.PopReceipt = popReceipt;
			queueMessage.DequeueCount = dequeueCount;
			if (dateTime != null)
			{
				queueMessage.InsertionTime = new DateTimeOffset?(dateTime.Value);
			}
			if (dateTime2 != null)
			{
				queueMessage.ExpirationTime = new DateTimeOffset?(dateTime2.Value);
			}
			if (dateTime3 != null)
			{
				queueMessage.NextVisibleTime = new DateTimeOffset?(dateTime3.Value);
			}
			return queueMessage;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00044854 File Offset: 0x00042A54
		protected override IEnumerable<QueueMessage> ParseXml()
		{
			if (this.reader.ReadToFollowing("QueueMessagesList"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					this.reader.ReadStartElement();
					while (this.reader.IsStartElement())
					{
						if (this.reader.IsEmptyElement)
						{
							this.reader.Skip();
						}
						else
						{
							string name;
							if ((name = this.reader.Name) != null)
							{
								if (name == "QueueMessage")
								{
									while (this.reader.IsStartElement())
									{
										yield return this.ParseMessageEntry();
									}
									continue;
								}
							}
							this.reader.Skip();
						}
					}
					this.allObjectsParsed = true;
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}
	}
}
