using System;
using System.Collections.Specialized;

namespace System.Management
{
	/// <summary>Represents a WQL SELECT data query.          </summary>
	// Token: 0x02000039 RID: 57
	public class SelectQuery : WqlObjectQuery
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.SelectQuery" /> class. This is the default constructor.          </summary>
		// Token: 0x060001DB RID: 475 RVA: 0x00009BBC File Offset: 0x00007DBC
		public SelectQuery() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.SelectQuery" /> class for the specified query or the specified class name.          </summary>
		/// <param name="queryOrClassName">The entire query or the class name to use in the query. The parser in this class attempts to parse the string as a valid WQL SELECT query. If the parser is unsuccessful, it assumes the string is a class name.</param>
		// Token: 0x060001DC RID: 476 RVA: 0x00009BC8 File Offset: 0x00007DC8
		public SelectQuery(string queryOrClassName)
		{
			this.selectedProperties = new StringCollection();
			if (queryOrClassName == null)
			{
				return;
			}
			if (queryOrClassName.TrimStart(new char[0]).StartsWith(ManagementQuery.tokenSelect, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrClassName;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrClassName);
			if (managementPath.IsClass && managementPath.NamespacePath.Length == 0)
			{
				this.ClassName = queryOrClassName;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrClassName");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.SelectQuery" /> class with the specified class name and condition.          </summary>
		/// <param name="className">The name of the class to select in the query.</param>
		/// <param name="condition">The condition to be applied in the query. </param>
		// Token: 0x060001DD RID: 477 RVA: 0x00009C43 File Offset: 0x00007E43
		public SelectQuery(string className, string condition) : this(className, condition, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.SelectQuery" /> class with the specified class name and condition, selecting only the specified properties.          </summary>
		/// <param name="className">The name of the class from which to select.</param>
		/// <param name="condition">The condition to be applied to instances of the selected class. </param>
		/// <param name="selectedProperties">An array of property names to be returned in the query results. </param>
		// Token: 0x060001DE RID: 478 RVA: 0x00009C4E File Offset: 0x00007E4E
		public SelectQuery(string className, string condition, string[] selectedProperties)
		{
			this.isSchemaQuery = false;
			this.className = className;
			this.condition = condition;
			this.selectedProperties = new StringCollection();
			if (selectedProperties != null)
			{
				this.selectedProperties.AddRange(selectedProperties);
			}
			this.BuildQuery();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.SelectQuery" /> class for a schema query, optionally specifying a condition.          </summary>
		/// <param name="isSchemaQuery">
		///       <see langword="true" /> to indicate that this is a schema query; otherwise, <see langword="false" />. A <see langword="false" /> value is invalid in this constructor.</param>
		/// <param name="condition">The condition to be applied to form the result set of classes. </param>
		// Token: 0x060001DF RID: 479 RVA: 0x00009C8C File Offset: 0x00007E8C
		public SelectQuery(bool isSchemaQuery, string condition)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.className = null;
			this.condition = condition;
			this.selectedProperties = null;
			this.BuildQuery();
		}

		/// <summary>Gets or sets the query in the <see cref="T:System.Management.SelectQuery" /> object, in string form.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the query.</returns>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00009CD9 File Offset: 0x00007ED9
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00009CE7 File Offset: 0x00007EE7
		public override string QueryString
		{
			get
			{
				this.BuildQuery();
				return base.QueryString;
			}
			set
			{
				base.QueryString = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether this query is a schema query or an instances query.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the query is a schema query.</returns>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00009CF0 File Offset: 0x00007EF0
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00009CF8 File Offset: 0x00007EF8
		public bool IsSchemaQuery
		{
			get
			{
				return this.isSchemaQuery;
			}
			set
			{
				this.isSchemaQuery = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the class name to be selected from in the query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the class in the query.</returns>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00009D0D File Offset: 0x00007F0D
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00009D23 File Offset: 0x00007F23
		public string ClassName
		{
			get
			{
				if (this.className == null)
				{
					return string.Empty;
				}
				return this.className;
			}
			set
			{
				this.className = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the condition to be applied in the SELECT query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the condition to be applied to the SELECT query.</returns>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009D38 File Offset: 0x00007F38
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00009D4E File Offset: 0x00007F4E
		public string Condition
		{
			get
			{
				if (this.condition == null)
				{
					return string.Empty;
				}
				return this.condition;
			}
			set
			{
				this.condition = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Ggets or sets an array of property names to be selected in the query.          </summary>
		/// <returns>Returns a <see cref="T:System.Collections.Specialized.StringCollection" /> containing the names of the properties to be selected in the query.</returns>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00009D63 File Offset: 0x00007F63
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00009D6C File Offset: 0x00007F6C
		public StringCollection SelectedProperties
		{
			get
			{
				return this.selectedProperties;
			}
			set
			{
				if (value != null)
				{
					StringCollection stringCollection = new StringCollection();
					foreach (string value2 in value)
					{
						stringCollection.Add(value2);
					}
					this.selectedProperties = stringCollection;
				}
				else
				{
					this.selectedProperties = new StringCollection();
				}
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Builds the query string according to the current property values.                       </summary>
		// Token: 0x060001EA RID: 490 RVA: 0x00009DEC File Offset: 0x00007FEC
		protected internal void BuildQuery()
		{
			string text;
			if (!this.isSchemaQuery)
			{
				if (this.className == null)
				{
					base.SetQueryString(string.Empty);
				}
				if (this.className == null || this.className.Length == 0)
				{
					return;
				}
				text = ManagementQuery.tokenSelect;
				if (this.selectedProperties != null && 0 < this.selectedProperties.Count)
				{
					int count = this.selectedProperties.Count;
					for (int i = 0; i < count; i++)
					{
						text = text + this.selectedProperties[i] + ((i == count - 1) ? " " : ",");
					}
				}
				else
				{
					text += "* ";
				}
				text = text + "from " + this.className;
			}
			else
			{
				text = "select * from meta_class";
			}
			if (this.Condition != null && this.Condition.Length != 0)
			{
				text = text + " where " + this.condition;
			}
			base.SetQueryString(text);
		}

		/// <summary>Parses the query string and sets the property values accordingly.                       </summary>
		/// <param name="query">The query string to be parsed.</param>
		// Token: 0x060001EB RID: 491 RVA: 0x00009EDC File Offset: 0x000080DC
		protected internal override void ParseQuery(string query)
		{
			this.className = null;
			this.condition = null;
			if (this.selectedProperties != null)
			{
				this.selectedProperties.Clear();
			}
			string text = query.Trim();
			bool flag = false;
			if (!this.isSchemaQuery)
			{
				string text2 = ManagementQuery.tokenSelect;
				if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				ManagementQuery.ParseToken(ref text, text2, ref flag);
				if (text[0] != '*')
				{
					if (this.selectedProperties != null)
					{
						this.selectedProperties.Clear();
					}
					else
					{
						this.selectedProperties = new StringCollection();
					}
					int num;
					string text3;
					while ((num = text.IndexOf(',')) > 0)
					{
						text3 = text.Substring(0, num);
						text = text.Remove(0, num + 1).TrimStart(null);
						text3 = text3.Trim();
						if (text3.Length > 0)
						{
							this.selectedProperties.Add(text3);
						}
					}
					if ((num = text.IndexOf(' ')) <= 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"));
					}
					text3 = text.Substring(0, num);
					text = text.Remove(0, num).TrimStart(null);
					this.selectedProperties.Add(text3);
				}
				else
				{
					text = text.Remove(0, 1).TrimStart(null);
				}
				text2 = "from ";
				flag = false;
				if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref this.className);
				text2 = "where ";
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.condition = text.Substring(text2.Length).Trim();
					return;
				}
			}
			else
			{
				string text4 = "select";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "select");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				if (text.IndexOf('*', 0) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "*");
				}
				text = text.Remove(0, 1).TrimStart(null);
				text4 = "from";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "from");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				text4 = "meta_class";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "meta_class");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				if (0 < text.Length)
				{
					text4 = "where";
					if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
					}
					text = text.Remove(0, text4.Length);
					if (text.Length == 0 || !char.IsWhiteSpace(text[0]))
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"));
					}
					text = text.TrimStart(null);
					this.condition = text;
				}
				else
				{
					this.condition = string.Empty;
				}
				this.className = null;
				this.selectedProperties = null;
			}
		}

		/// <summary>Creates a copy of the object.          </summary>
		/// <returns>The copied object.             </returns>
		// Token: 0x060001EC RID: 492 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public override object Clone()
		{
			string[] array = null;
			if (this.selectedProperties != null)
			{
				int count = this.selectedProperties.Count;
				if (0 < count)
				{
					array = new string[count];
					this.selectedProperties.CopyTo(array, 0);
				}
			}
			if (!this.isSchemaQuery)
			{
				return new SelectQuery(this.className, this.condition, array);
			}
			return new SelectQuery(true, this.condition);
		}

		// Token: 0x04000151 RID: 337
		private bool isSchemaQuery;

		// Token: 0x04000152 RID: 338
		private string className;

		// Token: 0x04000153 RID: 339
		private string condition;

		// Token: 0x04000154 RID: 340
		private StringCollection selectedProperties;
	}
}
