using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000100 RID: 256
	internal class StringBuffer
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0003109E File Offset: 0x0002F29E
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x000310A6 File Offset: 0x0002F2A6
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000310AF File Offset: 0x0002F2AF
		public StringBuffer()
		{
			this._buffer = StringBuffer.EmptyBuffer;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000310C2 File Offset: 0x0002F2C2
		public StringBuffer(int initalSize)
		{
			this._buffer = new char[initalSize];
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000310D8 File Offset: 0x0002F2D8
		public void Append(char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(1);
			}
			this._buffer[this._position++] = value;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00031115 File Offset: 0x0002F315
		public void Append(char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00031152 File Offset: 0x0002F352
		public void Clear()
		{
			this._buffer = StringBuffer.EmptyBuffer;
			this._position = 0;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00031168 File Offset: 0x0002F368
		private void EnsureSize(int appendLength)
		{
			char[] array = new char[(this._position + appendLength) * 2];
			Array.Copy(this._buffer, array, this._position);
			this._buffer = array;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003119E File Offset: 0x0002F39E
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000311AD File Offset: 0x0002F3AD
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000311BC File Offset: 0x0002F3BC
		public char[] GetInternalBuffer()
		{
			return this._buffer;
		}

		// Token: 0x04000455 RID: 1109
		private char[] _buffer;

		// Token: 0x04000456 RID: 1110
		private int _position;

		// Token: 0x04000457 RID: 1111
		private static readonly char[] EmptyBuffer = new char[0];
	}
}
