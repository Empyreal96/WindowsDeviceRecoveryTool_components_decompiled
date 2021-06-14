using System;
using System.ComponentModel;

namespace System.Windows.Data
{
	/// <summary>Declares a mapping between a uniform resource identifier (URI) and a prefix.</summary>
	// Token: 0x020001C0 RID: 448
	public class XmlNamespaceMapping : ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> class.</summary>
		// Token: 0x06001CF1 RID: 7409 RVA: 0x0000326D File Offset: 0x0000146D
		public XmlNamespaceMapping()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> class with the specified prefix and uniform resource identifier (URI).</summary>
		/// <param name="prefix">The prefix to use in Extensible Application Markup Language (XAML).</param>
		/// <param name="uri">The <see cref="T:System.Uri" /> of the namespace to create the mapping for.</param>
		// Token: 0x06001CF2 RID: 7410 RVA: 0x0008754D File Offset: 0x0008574D
		public XmlNamespaceMapping(string prefix, Uri uri)
		{
			this._prefix = prefix;
			this._uri = uri;
		}

		/// <summary>Gets or sets the prefix to use in Extensible Application Markup Language (XAML).</summary>
		/// <returns>The prefix to associate with the URI. The default is an empty string("").</returns>
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00087563 File Offset: 0x00085763
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x0008756C File Offset: 0x0008576C
		public string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"Prefix",
						base.GetType().Name
					}));
				}
				if (this._prefix != null && this._prefix != value)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"Prefix",
						base.GetType().Name
					}));
				}
				this._prefix = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Uri" /> of the namespace for which to create a mapping.</summary>
		/// <returns>The URI of the namespace. The default is <see langword="null" />.</returns>
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000875F6 File Offset: 0x000857F6
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00087600 File Offset: 0x00085800
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"Uri",
						base.GetType().Name
					}));
				}
				if (this._uri != null && this._uri != value)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"Uri",
						base.GetType().Name
					}));
				}
				this._uri = value;
			}
		}

		/// <summary>Returns a value that indicates whether this <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> is equivalent to the specified instance.</summary>
		/// <param name="obj">The instance to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances are the same; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CF7 RID: 7415 RVA: 0x00087690 File Offset: 0x00085890
		public override bool Equals(object obj)
		{
			return this == obj as XmlNamespaceMapping;
		}

		/// <summary>Performs equality comparison by value.</summary>
		/// <param name="mappingA">The first <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to compare.</param>
		/// <param name="mappingB">The second <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects are the same; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CF8 RID: 7416 RVA: 0x0008769E File Offset: 0x0008589E
		public static bool operator ==(XmlNamespaceMapping mappingA, XmlNamespaceMapping mappingB)
		{
			if (mappingA == null)
			{
				return mappingB == null;
			}
			return mappingB != null && mappingA.Prefix == mappingB.Prefix && mappingA.Uri == mappingB.Uri;
		}

		/// <summary>Performs inequality comparison by value.</summary>
		/// <param name="mappingA">The first <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to compare.</param>
		/// <param name="mappingB">The second <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Data.XmlNamespaceMapping" /> objects are not the same; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CF9 RID: 7417 RVA: 0x000876D3 File Offset: 0x000858D3
		public static bool operator !=(XmlNamespaceMapping mappingA, XmlNamespaceMapping mappingB)
		{
			return !(mappingA == mappingB);
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Data.XmlNamespaceMapping" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Data.XmlNamespaceMapping" />.</returns>
		// Token: 0x06001CFA RID: 7418 RVA: 0x000876E0 File Offset: 0x000858E0
		public override int GetHashCode()
		{
			int num = 0;
			if (this._prefix != null)
			{
				num = this._prefix.GetHashCode();
			}
			if (this._uri != null)
			{
				return num + this._uri.GetHashCode();
			}
			return num;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06001CFB RID: 7419 RVA: 0x00087720 File Offset: 0x00085920
		void ISupportInitialize.BeginInit()
		{
			this._initializing = true;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06001CFC RID: 7420 RVA: 0x0008772C File Offset: 0x0008592C
		void ISupportInitialize.EndInit()
		{
			if (this._prefix == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"Prefix",
					base.GetType().Name
				}));
			}
			if (this._uri == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"Uri",
					base.GetType().Name
				}));
			}
			this._initializing = false;
		}

		// Token: 0x040013FC RID: 5116
		private string _prefix;

		// Token: 0x040013FD RID: 5117
		private Uri _uri;

		// Token: 0x040013FE RID: 5118
		private bool _initializing;
	}
}
