using System;
using System.Collections.Generic;
using System.Text;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000002 RID: 2
	public class IsiMessage
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public IsiMessage()
		{
			this.Message = new List<byte>(10);
			this.InitializeHeader();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
		public IsiMessage(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if ((long)data.Length < (long)((ulong)IsiMessage.FirstDataByteIndex))
			{
				throw new ArgumentException("Not enough data to form a Isi Message");
			}
			if ((long)data.Length > 65542L)
			{
				throw new IndexOutOfRangeException("Data too long for Isi message");
			}
			this.Message = new List<byte>(data.Length);
			this.Message.InsertRange(0, data);
			this.UpdatePhonetLengthToMessage();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020DC File Offset: 0x000002DC
		public static uint MediaIndex
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020DF File Offset: 0x000002DF
		public static uint ReceiverIndex
		{
			get
			{
				return 1U;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020E2 File Offset: 0x000002E2
		public static uint SenderIndex
		{
			get
			{
				return 2U;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020E5 File Offset: 0x000002E5
		public static uint FunctionIndex
		{
			get
			{
				return 3U;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020E8 File Offset: 0x000002E8
		public static uint ResourceIndex
		{
			get
			{
				return 3U;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020EB File Offset: 0x000002EB
		public static uint LengthIndex
		{
			get
			{
				return 4U;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020EE File Offset: 0x000002EE
		public static uint ReceiverObjectIndex
		{
			get
			{
				return 6U;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020F1 File Offset: 0x000002F1
		public static uint SenderObjectIndex
		{
			get
			{
				return 7U;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020F4 File Offset: 0x000002F4
		public static uint FirstDataByteIndex
		{
			get
			{
				return 10U;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020F8 File Offset: 0x000002F8
		public static uint UtidIndex
		{
			get
			{
				return 8U;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020FB File Offset: 0x000002FB
		public static uint MessageIdIndex
		{
			get
			{
				return 9U;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002100 File Offset: 0x00000300
		public uint PhonetLength
		{
			get
			{
				uint num = (uint)((uint)this.Message[4] << 8);
				return num + (uint)this.Message[5];
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000212C File Offset: 0x0000032C
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000213E File Offset: 0x0000033E
		public byte Media
		{
			get
			{
				return this.Message[(int)IsiMessage.MediaIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.MediaIndex] = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002151 File Offset: 0x00000351
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002163 File Offset: 0x00000363
		public byte Receiver
		{
			get
			{
				return this.Message[(int)IsiMessage.ReceiverIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.ReceiverIndex] = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002176 File Offset: 0x00000376
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002188 File Offset: 0x00000388
		public byte Sender
		{
			get
			{
				return this.Message[(int)IsiMessage.SenderIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.SenderIndex] = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000219B File Offset: 0x0000039B
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000021AD File Offset: 0x000003AD
		public byte Resource
		{
			get
			{
				return this.Message[(int)IsiMessage.ResourceIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.ResourceIndex] = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000021C0 File Offset: 0x000003C0
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000021D2 File Offset: 0x000003D2
		public byte ReceiverObject
		{
			get
			{
				return this.Message[(int)IsiMessage.ReceiverObjectIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.ReceiverObjectIndex] = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000021E5 File Offset: 0x000003E5
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000021F7 File Offset: 0x000003F7
		public byte SenderObject
		{
			get
			{
				return this.Message[(int)IsiMessage.SenderObjectIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.SenderObjectIndex] = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000220A File Offset: 0x0000040A
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000221C File Offset: 0x0000041C
		public byte Utid
		{
			get
			{
				return this.Message[(int)IsiMessage.UtidIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.UtidIndex] = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000222F File Offset: 0x0000042F
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002241 File Offset: 0x00000441
		public byte MessageId
		{
			get
			{
				return this.Message[(int)IsiMessage.MessageIdIndex];
			}
			set
			{
				this.Message[(int)IsiMessage.MessageIdIndex] = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000225C File Offset: 0x0000045C
		public List<byte> Message { get; private set; }

		// Token: 0x17000016 RID: 22
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

		// Token: 0x06000023 RID: 35 RVA: 0x00002282 File Offset: 0x00000482
		public static IsiMessage operator +(IsiMessage message, byte value)
		{
			message.Message.Add(value);
			message.UpdatePhonetLengthToMessage();
			return message;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002297 File Offset: 0x00000497
		public static IsiMessage operator +(IsiMessage message, byte[] value)
		{
			message.Message.AddRange(value);
			message.UpdatePhonetLengthToMessage();
			return message;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000022AC File Offset: 0x000004AC
		public void SetData(uint index, byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if ((long)data.Length + (long)((ulong)index) > 65542L)
			{
				throw new IndexOutOfRangeException("Data too long for Isi message");
			}
			if ((long)this.Message.Capacity < (long)((ulong)index + (ulong)((long)data.Length)))
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
			this.UpdatePhonetLengthToMessage();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002375 File Offset: 0x00000575
		public void SetIsiData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.SetData(10U, data);
			this.UpdatePhonetLengthToMessage();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002394 File Offset: 0x00000594
		public void SetIsiHeader(IsiMessage.EnumMsgIndex index, byte data)
		{
			this.Message[(int)index] = data;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000023A4 File Offset: 0x000005A4
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

		// Token: 0x06000029 RID: 41 RVA: 0x0000241C File Offset: 0x0000061C
		private void InitializeHeader()
		{
			byte[] array = new byte[10];
			array[2] = 16;
			array[5] = 4;
			byte[] collection = array;
			this.Message.InsertRange(0, collection);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000244C File Offset: 0x0000064C
		private void UpdatePhonetLengthToMessage()
		{
			int num = this.Message.Count - 6;
			this[4U] = (byte)(num >> 8);
			this[5U] = (byte)(num & 255);
		}

		// Token: 0x04000001 RID: 1
		private const uint MaxSize = 65542U;

		// Token: 0x04000002 RID: 2
		private const uint InitialSize = 10U;

		// Token: 0x02000003 RID: 3
		public enum EnumMsgIndex
		{
			// Token: 0x04000005 RID: 5
			Media,
			// Token: 0x04000006 RID: 6
			Receiver,
			// Token: 0x04000007 RID: 7
			Sender,
			// Token: 0x04000008 RID: 8
			Function,
			// Token: 0x04000009 RID: 9
			Resource = 3,
			// Token: 0x0400000A RID: 10
			Length,
			// Token: 0x0400000B RID: 11
			ReceiverObj = 6,
			// Token: 0x0400000C RID: 12
			SenderObj,
			// Token: 0x0400000D RID: 13
			Utid,
			// Token: 0x0400000E RID: 14
			MessageId,
			// Token: 0x0400000F RID: 15
			DataFirst
		}
	}
}
