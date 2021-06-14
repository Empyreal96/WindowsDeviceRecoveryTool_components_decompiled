using System;
using System.IO;

namespace System.Windows.Resources
{
	/// <summary>Stores information for a stream resource used in Windows Presentation Foundation (WPF), such as images.</summary>
	// Token: 0x02000178 RID: 376
	public class StreamResourceInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Resources.StreamResourceInfo" /> class.</summary>
		// Token: 0x060015ED RID: 5613 RVA: 0x0000326D File Offset: 0x0000146D
		public StreamResourceInfo()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Resources.StreamResourceInfo" /> class based on a provided stream.</summary>
		/// <param name="stream">The reference stream.</param>
		/// <param name="contentType">The Multipurpose Internet Mail Extensions (MIME)  content type of the stream.</param>
		// Token: 0x060015EE RID: 5614 RVA: 0x0006B1F4 File Offset: 0x000693F4
		public StreamResourceInfo(Stream stream, string contentType)
		{
			this._stream = stream;
			this._contentType = contentType;
		}

		/// <summary> Gets or sets the content type of a stream. </summary>
		/// <returns>The Multipurpose Internet Mail Extensions (MIME) content type.</returns>
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x0006B20A File Offset: 0x0006940A
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		/// <summary> Gets or sets the actual stream of the resource. </summary>
		/// <returns>The stream for the resource.</returns>
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x0006B212 File Offset: 0x00069412
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x0400127E RID: 4734
		private string _contentType;

		// Token: 0x0400127F RID: 4735
		private Stream _stream;
	}
}
