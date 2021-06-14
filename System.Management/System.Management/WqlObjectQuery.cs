using System;

namespace System.Management
{
	/// <summary>Represents a WMI data query in WQL format.          </summary>
	// Token: 0x02000038 RID: 56
	public class WqlObjectQuery : ObjectQuery
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlObjectQuery" /> class. This is the default constructor.          </summary>
		// Token: 0x060001D7 RID: 471 RVA: 0x00009B95 File Offset: 0x00007D95
		public WqlObjectQuery() : base(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlObjectQuery" /> class initialized to the specified query.          </summary>
		/// <param name="query"> The representation of the data query.</param>
		// Token: 0x060001D8 RID: 472 RVA: 0x00009B9E File Offset: 0x00007D9E
		public WqlObjectQuery(string query) : base(query)
		{
		}

		/// <summary>Gets the language of the query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the language of the query.</returns>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00009BA7 File Offset: 0x00007DA7
		public override string QueryLanguage
		{
			get
			{
				return base.QueryLanguage;
			}
		}

		/// <summary>Creates a copy of the object.          </summary>
		/// <returns>The copied object.             </returns>
		// Token: 0x060001DA RID: 474 RVA: 0x00009BAF File Offset: 0x00007DAF
		public override object Clone()
		{
			return new WqlObjectQuery(this.QueryString);
		}
	}
}
