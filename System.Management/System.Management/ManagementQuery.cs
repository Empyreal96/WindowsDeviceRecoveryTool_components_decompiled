using System;
using System.ComponentModel;

namespace System.Management
{
	/// <summary>Provides an abstract base class for all management query objects.           </summary>
	// Token: 0x02000035 RID: 53
	[TypeConverter(typeof(ManagementQueryConverter))]
	public abstract class ManagementQuery : ICloneable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060001BF RID: 447 RVA: 0x0000992C File Offset: 0x00007B2C
		// (remove) Token: 0x060001C0 RID: 448 RVA: 0x00009964 File Offset: 0x00007B64
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x060001C1 RID: 449 RVA: 0x00009999 File Offset: 0x00007B99
		internal void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000099B0 File Offset: 0x00007BB0
		internal void SetQueryString(string qString)
		{
			this.queryString = qString;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000099B9 File Offset: 0x00007BB9
		internal ManagementQuery() : this("WQL", null)
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000099C7 File Offset: 0x00007BC7
		internal ManagementQuery(string query) : this("WQL", query)
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000099D5 File Offset: 0x00007BD5
		internal ManagementQuery(string language, string query)
		{
			this.QueryLanguage = language;
			this.QueryString = query;
		}

		/// <summary>Parses the query string and sets the property values accordingly. If the query is valid, the class name property and condition property of the query will be parsed.                       </summary>
		/// <param name="query">The query string to be parsed.</param>
		// Token: 0x060001C6 RID: 454 RVA: 0x00002208 File Offset: 0x00000408
		protected internal virtual void ParseQuery(string query)
		{
		}

		/// <summary>Gets or sets the query in text format.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the query.</returns>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000099EB File Offset: 0x00007BEB
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00009A01 File Offset: 0x00007C01
		public virtual string QueryString
		{
			get
			{
				if (this.queryString == null)
				{
					return string.Empty;
				}
				return this.queryString;
			}
			set
			{
				if (this.queryString != value)
				{
					this.ParseQuery(value);
					this.queryString = value;
					this.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Gets or sets the query language used in the query string, defining the format of the query string.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the format of the query string.</returns>
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00009A25 File Offset: 0x00007C25
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00009A3B File Offset: 0x00007C3B
		public virtual string QueryLanguage
		{
			get
			{
				if (this.queryLanguage == null)
				{
					return string.Empty;
				}
				return this.queryLanguage;
			}
			set
			{
				if (this.queryLanguage != value)
				{
					this.queryLanguage = value;
					this.FireIdentifierChanged();
				}
			}
		}

		/// <summary>Returns a copy of the object.          </summary>
		/// <returns>The cloned object.             </returns>
		// Token: 0x060001CB RID: 459
		public abstract object Clone();

		// Token: 0x060001CC RID: 460 RVA: 0x00009A58 File Offset: 0x00007C58
		internal static void ParseToken(ref string q, string token, string op, ref bool bTokenFound, ref string tokenValue)
		{
			if (bTokenFound)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_DUP_TOKEN"));
			}
			bTokenFound = true;
			q = q.Remove(0, token.Length).TrimStart(null);
			if (op != null)
			{
				if (q.IndexOf(op, StringComparison.Ordinal) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				q = q.Remove(0, op.Length).TrimStart(null);
			}
			if (q.Length == 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_NULL_TOKEN"));
			}
			int length;
			if (-1 == (length = q.IndexOf(' ')))
			{
				length = q.Length;
			}
			tokenValue = q.Substring(0, length);
			q = q.Remove(0, tokenValue.Length).TrimStart(null);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009B19 File Offset: 0x00007D19
		internal static void ParseToken(ref string q, string token, ref bool bTokenFound)
		{
			if (bTokenFound)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_DUP_TOKEN"));
			}
			bTokenFound = true;
			q = q.Remove(0, token.Length).TrimStart(null);
		}

		// Token: 0x0400014C RID: 332
		internal const string DEFAULTQUERYLANGUAGE = "WQL";

		// Token: 0x0400014D RID: 333
		internal static readonly string tokenSelect = "select ";

		// Token: 0x0400014F RID: 335
		private string queryLanguage;

		// Token: 0x04000150 RID: 336
		private string queryString;
	}
}
