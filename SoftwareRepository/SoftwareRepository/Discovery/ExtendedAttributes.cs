using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public class ExtendedAttributes : ISerializable
	{
		// Token: 0x0600011C RID: 284 RVA: 0x0000422A File Offset: 0x0000242A
		public ExtendedAttributes()
		{
			this.Dictionary = new Dictionary<string, string>();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004240 File Offset: 0x00002440
		protected ExtendedAttributes(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				this.Dictionary = new Dictionary<string, string>();
				foreach (SerializationEntry serializationEntry in info)
				{
					this.Dictionary.Add(serializationEntry.Name, (string)serializationEntry.Value);
				}
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004298 File Offset: 0x00002498
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				foreach (string text in this.Dictionary.Keys)
				{
					info.AddValue(text, this.Dictionary[text]);
				}
			}
		}

		// Token: 0x040000A4 RID: 164
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		public Dictionary<string, string> Dictionary;
	}
}
