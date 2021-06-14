using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000016 RID: 22
	public class DataElement : INotifyPropertyChanged
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600008A RID: 138 RVA: 0x000053A8 File Offset: 0x000035A8
		// (remove) Token: 0x0600008B RID: 139 RVA: 0x000053E0 File Offset: 0x000035E0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600008C RID: 140 RVA: 0x00005418 File Offset: 0x00003618
		internal DataElement(DataElementType type, ushort length, DataType dataType, string name)
		{
			this.Type = type;
			this.mLength = (int)length;
			bool flag = this.Length != 0;
			if (flag)
			{
				this.mValue = new byte[(int)length];
			}
			this.DataType = dataType;
			this.Name = name;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005480 File Offset: 0x00003680
		public void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000054AD File Offset: 0x000036AD
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000054B5 File Offset: 0x000036B5
		public string Name { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000054BE File Offset: 0x000036BE
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000054C6 File Offset: 0x000036C6
		internal DataType DataType { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000054CF File Offset: 0x000036CF
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000054D7 File Offset: 0x000036D7
		internal DataElementType Type { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000054E0 File Offset: 0x000036E0
		// (set) Token: 0x06000095 RID: 149 RVA: 0x000054F8 File Offset: 0x000036F8
		public int Length
		{
			get
			{
				return this.mLength;
			}
			set
			{
				this.mLength = value;
				this.mValue = null;
				this.mValue = new byte[this.mLength];
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000551A File Offset: 0x0000371A
		public void SetRawData(byte[] value)
		{
			this.mValue = value;
			this.OnPropertyChanged("Data");
			this.OnPropertyChanged("HexData");
			this.OnPropertyChanged("CookedData");
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005548 File Offset: 0x00003748
		public byte[] GetRawData()
		{
			this.OnPropertyChanged("Data");
			this.OnPropertyChanged("HexData");
			this.OnPropertyChanged("CookedData");
			return this.mValue;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005584 File Offset: 0x00003784
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00005644 File Offset: 0x00003844
		public string HexData
		{
			get
			{
				DataType dataType = this.DataType;
				if (dataType != DataType.DATA_TYPE_UINT32)
				{
					if (dataType == DataType.DATA_TYPE_BINARYSTREAM)
					{
						this.mHexData = "";
						int num;
						for (int i = 0; i < this.mLength; i = num + 1)
						{
							this.mHexData = this.mHexData + "0x" + this.GetRawData()[i].ToString("X02", CultureInfo.InvariantCulture) + " ";
							num = i;
						}
					}
				}
				else
				{
					this.mHexData = "0x" + ((uint)this.Data).ToString("X08", CultureInfo.InvariantCulture);
				}
				return this.mHexData;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				DataType dataType = this.DataType;
				if (dataType == DataType.DATA_TYPE_UINT32)
				{
					bool flag2 = value.Length > 1 && value[1] == 'x';
					if (flag2)
					{
						value = value.Remove(0, 2);
					}
					this.Data = Convert.ToUInt32(value, 16);
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000056B0 File Offset: 0x000038B0
		public string CookedData
		{
			get
			{
				DataElementType type = this.Type;
				if (type != DataElementType.DI_D_STATUS)
				{
					if (type == DataElementType.DI_FW_UPDATE_DATE)
					{
						DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
						uint num = (uint)this.Data;
						dateTime = dateTime.AddSeconds(num);
						this.mCookedData = dateTime.ToString(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					this.mCookedData = Enum.GetName(typeof(FStatus), this.Data);
				}
				return this.mCookedData;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005734 File Offset: 0x00003934
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00005838 File Offset: 0x00003A38
		public object Data
		{
			get
			{
				switch (this.DataType)
				{
				case DataType.DATA_TYPE_UINT8:
					return this.mValue[0];
				case DataType.DATA_TYPE_INT16:
					return BitConverter.ToInt16(this.mValue, 0);
				case DataType.DATA_TYPE_UINT16:
					return BitConverter.ToUInt16(this.mValue, 0);
				case DataType.DATA_TYPE_INT32:
					return BitConverter.ToInt32(this.mValue, 0);
				case DataType.DATA_TYPE_UINT32:
					return BitConverter.ToUInt32(this.mValue, 0);
				case DataType.DATA_TYPE_UINT64:
					return BitConverter.ToUInt64(this.mValue, 0);
				case DataType.DATA_TYPE_ASCIISTRING:
					return Encoding.Default.GetString(this.mValue);
				case DataType.DATA_TYPE_UNICODESTRING:
				{
					Encoding encoding = new UnicodeEncoding(false, true, true);
					return encoding.GetString(this.mValue);
				}
				}
				return null;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				switch (this.DataType)
				{
				case DataType.DATA_TYPE_UINT8:
					this.mValue[0] = Convert.ToByte(value.ToString(), CultureInfo.InvariantCulture);
					break;
				case DataType.DATA_TYPE_UINT16:
					Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(value.ToString(), CultureInfo.InvariantCulture)), this.mValue, this.mLength);
					break;
				case DataType.DATA_TYPE_INT32:
					Array.Copy(BitConverter.GetBytes(Convert.ToInt32(value.ToString(), CultureInfo.InvariantCulture)), this.mValue, this.mLength);
					break;
				case DataType.DATA_TYPE_UINT32:
					Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(value.ToString(), CultureInfo.InvariantCulture)), this.mValue, this.mLength);
					break;
				case DataType.DATA_TYPE_UINT64:
					Array.Copy(BitConverter.GetBytes(Convert.ToUInt64(value.ToString(), CultureInfo.InvariantCulture)), this.mValue, this.mLength);
					break;
				case DataType.DATA_TYPE_ASCIISTRING:
				{
					Encoding ascii = Encoding.ASCII;
					this.mValue = ascii.GetBytes(value.ToString());
					this.mLength = (int)((ushort)this.mValue.Length);
					break;
				}
				case DataType.DATA_TYPE_UNICODESTRING:
				{
					Encoding unicode = Encoding.Unicode;
					this.mValue = unicode.GetBytes(value.ToString());
					break;
				}
				}
				this.OnPropertyChanged("Data");
				this.OnPropertyChanged("HexData");
				this.OnPropertyChanged("CookedData");
			}
		}

		// Token: 0x040000B0 RID: 176
		private int mLength;

		// Token: 0x040000B1 RID: 177
		private byte[] mValue;

		// Token: 0x040000B2 RID: 178
		private string mHexData = "";

		// Token: 0x040000B3 RID: 179
		private string mCookedData = "";
	}
}
