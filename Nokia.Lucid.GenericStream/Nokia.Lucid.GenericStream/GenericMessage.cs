using System;
using System.Collections.Generic;
using System.Text;

namespace Nokia.Lucid.GenericStream
{
	// Token: 0x02000002 RID: 2
	public class GenericMessage
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public GenericMessage()
		{
			this.Message = new List<byte>(1024);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public GenericMessage(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if ((long)data.Length > 1048576L)
			{
				throw new IndexOutOfRangeException("Data too long for Generic message");
			}
			this.Message = new List<byte>(data.Length);
			this.Message.InsertRange(0, data);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BB File Offset: 0x000002BB
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C3 File Offset: 0x000002C3
		public List<byte> Message { get; private set; }

		// Token: 0x17000002 RID: 2
		public byte this[uint index]
		{
			get
			{
				return this.Message[(int)index];
			}
			set
			{
				this.Message[(int)index] = value;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E9 File Offset: 0x000002E9
		public static GenericMessage operator +(GenericMessage message, byte value)
		{
			message.Message.Add(value);
			return message;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F8 File Offset: 0x000002F8
		public static GenericMessage operator +(GenericMessage message, byte[] value)
		{
			message.Message.AddRange(value);
			return message;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002108 File Offset: 0x00000308
		public void SetData(uint index, byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if ((long)data.Length + (long)((ulong)index) > 1048576L)
			{
				throw new IndexOutOfRangeException("Data too long for Generic message");
			}
			if ((long)this.Message.Count < (long)((ulong)index + (ulong)((long)data.Length)))
			{
				this.Message.AddRange(new byte[index + (uint)data.Length - (uint)this.Message.Count]);
			}
			if ((long)this.Message.Count > (long)((ulong)index + (ulong)((long)data.Length)))
			{
				this.Message.RemoveRange((int)index, (int)((long)this.Message.Count - (long)((ulong)index + (ulong)((long)data.Length))));
			}
			for (int i = 0; i < data.Length; i++)
			{
				this.Message[(int)((long)i + (long)((ulong)index))] = data[i];
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021CC File Offset: 0x000003CC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in this.Message)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
				stringBuilder.Append(", ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000001 RID: 1
		private const uint MaxSize = 1048576U;

		// Token: 0x04000002 RID: 2
		private const uint InitialSize = 1024U;
	}
}
