using System;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F3 RID: 2035
	internal class TextSegment
	{
		// Token: 0x06007D5A RID: 32090 RVA: 0x00233974 File Offset: 0x00231B74
		static TextSegment()
		{
			try
			{
				TextSegment.s_WinRTType = Type.GetType(TextSegment.s_TypeName);
			}
			catch
			{
				TextSegment.s_WinRTType = null;
			}
		}

		// Token: 0x06007D5B RID: 32091 RVA: 0x002339B8 File Offset: 0x00231BB8
		public TextSegment(object textSegment)
		{
			if (TextSegment.s_WinRTType == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (textSegment.GetType() != TextSegment.s_WinRTType)
			{
				throw new ArgumentException();
			}
			this._textSegment = textSegment;
		}

		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x06007D5C RID: 32092 RVA: 0x002339F2 File Offset: 0x00231BF2
		public uint Length
		{
			get
			{
				if (this._length == null)
				{
					this._length = new uint?(this._textSegment.ReflectionGetField("Length"));
				}
				return this._length.Value;
			}
		}

		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x06007D5D RID: 32093 RVA: 0x00233A27 File Offset: 0x00231C27
		public uint StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = new uint?(this._textSegment.ReflectionGetField("StartPosition"));
				}
				return this._startPosition.Value;
			}
		}

		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x06007D5E RID: 32094 RVA: 0x00233A5C File Offset: 0x00231C5C
		public static Type WinRTType
		{
			get
			{
				return TextSegment.s_WinRTType;
			}
		}

		// Token: 0x04003AFC RID: 15100
		private static Type s_WinRTType;

		// Token: 0x04003AFD RID: 15101
		private static string s_TypeName = "Windows.Data.Text.TextSegment, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003AFE RID: 15102
		private object _textSegment;

		// Token: 0x04003AFF RID: 15103
		private uint? _length;

		// Token: 0x04003B00 RID: 15104
		private uint? _startPosition;
	}
}
