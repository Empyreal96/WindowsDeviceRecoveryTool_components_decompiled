using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x02000770 RID: 1904
	internal class BamlStream : Stream, IStreamInfo
	{
		// Token: 0x060078BE RID: 30910 RVA: 0x0022674A File Offset: 0x0022494A
		[SecurityCritical]
		internal BamlStream(Stream stream, Assembly assembly)
		{
			this._assembly.Value = assembly;
			this._stream = stream;
		}

		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x060078BF RID: 30911 RVA: 0x00226765 File Offset: 0x00224965
		Assembly IStreamInfo.Assembly
		{
			get
			{
				return this._assembly.Value;
			}
		}

		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x060078C0 RID: 30912 RVA: 0x00226772 File Offset: 0x00224972
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x060078C1 RID: 30913 RVA: 0x0022677F File Offset: 0x0022497F
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x060078C2 RID: 30914 RVA: 0x0022678C File Offset: 0x0022498C
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x060078C3 RID: 30915 RVA: 0x00226799 File Offset: 0x00224999
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x060078C4 RID: 30916 RVA: 0x002267A6 File Offset: 0x002249A6
		// (set) Token: 0x060078C5 RID: 30917 RVA: 0x002267B3 File Offset: 0x002249B3
		public override long Position
		{
			get
			{
				return this._stream.Position;
			}
			set
			{
				this._stream.Position = value;
			}
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x002267C1 File Offset: 0x002249C1
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x002267D5 File Offset: 0x002249D5
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x002267E9 File Offset: 0x002249E9
		public override void Close()
		{
			this._stream.Close();
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x002267F6 File Offset: 0x002249F6
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._stream.EndRead(asyncResult);
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x00226804 File Offset: 0x00224A04
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._stream.EndWrite(asyncResult);
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x00226812 File Offset: 0x00224A12
		public override bool Equals(object obj)
		{
			return this._stream.Equals(obj);
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x00226820 File Offset: 0x00224A20
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x0022682D File Offset: 0x00224A2D
		public override int GetHashCode()
		{
			return this._stream.GetHashCode();
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x0022683A File Offset: 0x00224A3A
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._stream.Read(buffer, offset, count);
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x0022684A File Offset: 0x00224A4A
		public override int ReadByte()
		{
			return this._stream.ReadByte();
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x00226857 File Offset: 0x00224A57
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._stream.Seek(offset, origin);
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x00226866 File Offset: 0x00224A66
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x060078D2 RID: 30930 RVA: 0x00226874 File Offset: 0x00224A74
		public override string ToString()
		{
			return this._stream.ToString();
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x00226881 File Offset: 0x00224A81
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x00226891 File Offset: 0x00224A91
		public override void WriteByte(byte value)
		{
			this._stream.WriteByte(value);
		}

		// Token: 0x04003935 RID: 14645
		private SecurityCriticalDataForSet<Assembly> _assembly;

		// Token: 0x04003936 RID: 14646
		private Stream _stream;
	}
}
