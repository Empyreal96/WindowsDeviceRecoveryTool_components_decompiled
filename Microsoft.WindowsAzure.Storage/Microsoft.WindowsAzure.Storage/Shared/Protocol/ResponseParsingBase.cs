using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x020000C4 RID: 196
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class ResponseParsingBase<T> : IDisposable
	{
		// Token: 0x06001114 RID: 4372 RVA: 0x0003F4E0 File Offset: 0x0003D6E0
		protected ResponseParsingBase(Stream stream)
		{
			this.reader = XmlReader.Create(stream, new XmlReaderSettings
			{
				IgnoreWhitespace = false
			});
			this.parser = this.ParseXmlAndClose().GetEnumerator();
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x0003F768 File Offset: 0x0003D968
		protected IEnumerable<T> ObjectsToParse
		{
			get
			{
				if (this.enumerableConsumed)
				{
					throw new InvalidOperationException("Resource consumed");
				}
				this.enumerableConsumed = true;
				while (!this.allObjectsParsed && this.parser.MoveNext())
				{
					if (this.parser.Current != null)
					{
						yield return this.parser.Current;
					}
				}
				foreach (T parsableObject in this.outstandingObjectsToParse)
				{
					yield return parsableObject;
				}
				this.outstandingObjectsToParse = null;
				yield break;
			}
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0003F785 File Offset: 0x0003D985
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001117 RID: 4375
		protected abstract IEnumerable<T> ParseXml();

		// Token: 0x06001118 RID: 4376 RVA: 0x0003F794 File Offset: 0x0003D994
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
			}
			this.reader = null;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0003F7B4 File Offset: 0x0003D9B4
		protected void Variable(ref bool consumable)
		{
			if (!consumable)
			{
				while (this.parser.MoveNext())
				{
					if (this.parser.Current != null)
					{
						this.outstandingObjectsToParse.Add(this.parser.Current);
					}
					if (consumable)
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0003F9B8 File Offset: 0x0003DBB8
		private IEnumerable<T> ParseXmlAndClose()
		{
			foreach (T item in this.ParseXml())
			{
				yield return item;
			}
			this.reader.Close();
			this.reader = null;
			yield break;
		}

		// Token: 0x0400046B RID: 1131
		protected bool allObjectsParsed;

		// Token: 0x0400046C RID: 1132
		protected IList<T> outstandingObjectsToParse = new List<T>();

		// Token: 0x0400046D RID: 1133
		protected XmlReader reader;

		// Token: 0x0400046E RID: 1134
		private IEnumerator<T> parser;

		// Token: 0x0400046F RID: 1135
		private bool enumerableConsumed;
	}
}
