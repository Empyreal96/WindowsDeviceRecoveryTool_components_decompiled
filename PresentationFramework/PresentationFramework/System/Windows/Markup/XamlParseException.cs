using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using MS.Internal;

namespace System.Windows.Markup
{
	/// <summary>Represents the exception class for parser-specific exceptions from a WPF XAML parser. This exception is used in XAML API or WPF XAML parser operations from .NET Framework 3.0 and .NET Framework 3.5, or for specific use of the WPF XAML parser by calling <see cref="T:System.Windows.Markup.XamlReader" /> API. </summary>
	// Token: 0x02000263 RID: 611
	[Serializable]
	public class XamlParseException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class.</summary>
		// Token: 0x06002314 RID: 8980 RVA: 0x0008680E File Offset: 0x00084A0E
		public XamlParseException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class, using the specified exception message string.</summary>
		/// <param name="message">The exception message.</param>
		// Token: 0x06002315 RID: 8981 RVA: 0x00086816 File Offset: 0x00084A16
		public XamlParseException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class, using the specified exception message string and inner exception. </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="innerException">The initial exception that occurred.</param>
		// Token: 0x06002316 RID: 8982 RVA: 0x0008681F File Offset: 0x00084A1F
		public XamlParseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class, using the specified exception message string, and the specified line number and position in the line.</summary>
		/// <param name="message">The exception message.</param>
		/// <param name="lineNumber">The line number where the exception occurred.</param>
		/// <param name="linePosition">The position in the line at which the exception occurred.</param>
		// Token: 0x06002317 RID: 8983 RVA: 0x000AC8EC File Offset: 0x000AAAEC
		public XamlParseException(string message, int lineNumber, int linePosition) : this(message)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class, using the specified exception message, inner exception, line number, and position in the line.</summary>
		/// <param name="message">The exception message.</param>
		/// <param name="lineNumber">The line number where the exception occurred.</param>
		/// <param name="linePosition">The position in the line at which the exception occurred.</param>
		/// <param name="innerException">The initial exception that occurred.</param>
		// Token: 0x06002318 RID: 8984 RVA: 0x000AC903 File Offset: 0x000AAB03
		public XamlParseException(string message, int lineNumber, int linePosition, Exception innerException) : this(message, innerException)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000AC91C File Offset: 0x000AAB1C
		internal XamlParseException(string message, int lineNumber, int linePosition, Uri baseUri, Exception innerException) : this(message, innerException)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			this._baseUri = baseUri;
		}

		/// <summary>Gets the line number where the exception occurred. </summary>
		/// <returns>The line number.</returns>
		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000AC93D File Offset: 0x000AAB3D
		// (set) Token: 0x0600231B RID: 8987 RVA: 0x000AC945 File Offset: 0x000AAB45
		public int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			internal set
			{
				this._lineNumber = value;
			}
		}

		/// <summary>Gets the position in the line where the exception occurred. </summary>
		/// <returns>The line position.</returns>
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000AC94E File Offset: 0x000AAB4E
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x000AC956 File Offset: 0x000AAB56
		public int LinePosition
		{
			get
			{
				return this._linePosition;
			}
			internal set
			{
				this._linePosition = value;
			}
		}

		/// <summary>Gets or sets the key value of the item in a dictionary where the exception occurred. </summary>
		/// <returns>The relevant XAML <see langword="x:Key" /> value.</returns>
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x000AC95F File Offset: 0x000AAB5F
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x000AC967 File Offset: 0x000AAB67
		public object KeyContext
		{
			get
			{
				return this._keyContext;
			}
			internal set
			{
				this._keyContext = value;
			}
		}

		/// <summary>Gets or sets the x:Uid Directive of the object where the exception occurred. </summary>
		/// <returns>The value of the <see langword="Uid" /> string.</returns>
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x000AC970 File Offset: 0x000AAB70
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x000AC978 File Offset: 0x000AAB78
		public string UidContext
		{
			get
			{
				return this._uidContext;
			}
			internal set
			{
				this._uidContext = value;
			}
		}

		/// <summary>Gets or sets the XAML name of the object where the exception occurred.</summary>
		/// <returns>The XAML name of the object.</returns>
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x000AC981 File Offset: 0x000AAB81
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x000AC989 File Offset: 0x000AAB89
		public string NameContext
		{
			get
			{
				return this._nameContext;
			}
			internal set
			{
				this._nameContext = value;
			}
		}

		/// <summary>Gets base URI information when the exception is thrown.</summary>
		/// <returns>The parser context base URI. </returns>
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x000AC992 File Offset: 0x000AAB92
		// (set) Token: 0x06002325 RID: 8997 RVA: 0x000AC99A File Offset: 0x000AAB9A
		public Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			internal set
			{
				this._baseUri = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlParseException" /> class. </summary>
		/// <param name="info">Contains all the information that is required to serialize or deserialize the object.</param>
		/// <param name="context">The source and destination of a serialized stream.</param>
		// Token: 0x06002326 RID: 8998 RVA: 0x000AC9A3 File Offset: 0x000AABA3
		protected XamlParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._lineNumber = info.GetInt32("Line");
			this._linePosition = info.GetInt32("Position");
		}

		/// <summary>Gets the data that is required to serialize the specified object by populating the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</summary>
		/// <param name="info">The serialization information object to add the serialization data to.</param>
		/// <param name="context">The destination for this serialization.</param>
		// Token: 0x06002327 RID: 8999 RVA: 0x000AC9CF File Offset: 0x000AABCF
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Line", this._lineNumber);
			info.AddValue("Position", this._linePosition);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000AC9FC File Offset: 0x000AABFC
		internal static string GetMarkupFilePath(Uri resourceUri)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (resourceUri != null)
			{
				if (resourceUri.IsAbsoluteUri)
				{
					text = resourceUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
				}
				else
				{
					text = resourceUri.OriginalString;
				}
				text2 = text.Replace(".baml", ".xaml");
				if (-1 == text2.LastIndexOf(".xaml", StringComparison.Ordinal))
				{
					text2 = string.Empty;
				}
			}
			return text2;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000ACA60 File Offset: 0x000AAC60
		internal static string GenerateErrorMessageContext(int lineNumber, int linePosition, Uri baseUri, XamlObjectIds xamlObjectIds, Type objectType)
		{
			string result = " ";
			string markupFilePath = XamlParseException.GetMarkupFilePath(baseUri);
			string text = null;
			if (xamlObjectIds != null)
			{
				if (xamlObjectIds.Name != null)
				{
					text = xamlObjectIds.Name;
				}
				else if (xamlObjectIds.Key != null)
				{
					text = xamlObjectIds.Key.ToString();
				}
				else if (xamlObjectIds.Uid != null)
				{
					text = xamlObjectIds.Uid;
				}
			}
			if (text == null && objectType != null)
			{
				text = objectType.ToString();
			}
			XamlParseException.ContextBits contextBits = (XamlParseException.ContextBits)0;
			if (text != null)
			{
				contextBits |= XamlParseException.ContextBits.Type;
			}
			if (!string.IsNullOrEmpty(markupFilePath))
			{
				contextBits |= XamlParseException.ContextBits.File;
			}
			if (lineNumber > 0)
			{
				contextBits |= XamlParseException.ContextBits.Line;
			}
			switch (contextBits)
			{
			case (XamlParseException.ContextBits)0:
				result = string.Empty;
				break;
			case XamlParseException.ContextBits.Type:
				result = SR.Get("ParserErrorContext_Type", new object[]
				{
					text
				});
				break;
			case XamlParseException.ContextBits.File:
				result = SR.Get("ParserErrorContext_File", new object[]
				{
					markupFilePath
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.File:
				result = SR.Get("ParserErrorContext_Type_File", new object[]
				{
					text,
					markupFilePath
				});
				break;
			case XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Line", new object[]
				{
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Type_Line", new object[]
				{
					text,
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.File | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_File_Line", new object[]
				{
					markupFilePath,
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.File | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Type_File_Line", new object[]
				{
					text,
					markupFilePath,
					lineNumber,
					linePosition
				});
				break;
			}
			return result;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000ACC14 File Offset: 0x000AAE14
		internal static void ThrowException(string message, Exception innerException, int lineNumber, int linePosition, Uri baseUri, XamlObjectIds currentXamlObjectIds, XamlObjectIds contextXamlObjectIds, Type objectType)
		{
			if (innerException != null && innerException.Message != null)
			{
				StringBuilder stringBuilder = new StringBuilder(message);
				if (innerException.Message != string.Empty)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(innerException.Message);
				message = stringBuilder.ToString();
			}
			string str = XamlParseException.GenerateErrorMessageContext(lineNumber, linePosition, baseUri, currentXamlObjectIds, objectType);
			message = message + "  " + str;
			XamlParseException ex;
			if (innerException is TargetInvocationException && innerException.InnerException is XamlParseException)
			{
				ex = (XamlParseException)innerException.InnerException;
			}
			else if (lineNumber > 0)
			{
				ex = new XamlParseException(message, lineNumber, linePosition, innerException);
			}
			else
			{
				ex = new XamlParseException(message, innerException);
			}
			if (contextXamlObjectIds != null)
			{
				ex.NameContext = contextXamlObjectIds.Name;
				ex.UidContext = contextXamlObjectIds.Uid;
				ex.KeyContext = contextXamlObjectIds.Key;
			}
			ex.BaseUri = baseUri;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.TraceActivityItem(TraceMarkup.ThrowException, ex);
			}
			throw ex;
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000ACD03 File Offset: 0x000AAF03
		internal static void ThrowException(ParserContext parserContext, int lineNumber, int linePosition, string message, Exception innerException)
		{
			XamlParseException.ThrowException(message, innerException, lineNumber, linePosition);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000ACD0F File Offset: 0x000AAF0F
		internal static void ThrowException(string message, Exception innerException, int lineNumber, int linePosition)
		{
			XamlParseException.ThrowException(message, innerException, lineNumber, linePosition, null, null, null, null);
		}

		// Token: 0x04001A6A RID: 6762
		internal const string BamlExt = ".baml";

		// Token: 0x04001A6B RID: 6763
		internal const string XamlExt = ".xaml";

		// Token: 0x04001A6C RID: 6764
		private int _lineNumber;

		// Token: 0x04001A6D RID: 6765
		private int _linePosition;

		// Token: 0x04001A6E RID: 6766
		private object _keyContext;

		// Token: 0x04001A6F RID: 6767
		private string _uidContext;

		// Token: 0x04001A70 RID: 6768
		private string _nameContext;

		// Token: 0x04001A71 RID: 6769
		private Uri _baseUri;

		// Token: 0x0200089C RID: 2204
		[Flags]
		private enum ContextBits
		{
			// Token: 0x040041A7 RID: 16807
			Type = 1,
			// Token: 0x040041A8 RID: 16808
			File = 2,
			// Token: 0x040041A9 RID: 16809
			Line = 4
		}
	}
}
