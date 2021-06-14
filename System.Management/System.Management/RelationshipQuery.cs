using System;

namespace System.Management
{
	/// <summary>Represents a WQL REFERENCES OF data query.           </summary>
	// Token: 0x0200003B RID: 59
	public class RelationshipQuery : WqlObjectQuery
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelationshipQuery" /> class. This is the default constructor.          </summary>
		// Token: 0x06000208 RID: 520 RVA: 0x0000AE0F File Offset: 0x0000900F
		public RelationshipQuery() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelationshipQuery" /> class. If the specified string can be successfully parsed as a WQL query, it is considered to be the query string; otherwise, it is assumed to be the path of the source object for the query. In this case, the query is assumed to be an instances query.           </summary>
		/// <param name="queryOrSourceObject">The query string or the class name for this query.</param>
		// Token: 0x06000209 RID: 521 RVA: 0x0000AE18 File Offset: 0x00009018
		public RelationshipQuery(string queryOrSourceObject)
		{
			if (queryOrSourceObject == null)
			{
				return;
			}
			if (queryOrSourceObject.TrimStart(new char[0]).StartsWith(RelationshipQuery.tokenReferences, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrSourceObject;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrSourceObject);
			if ((managementPath.IsClass || managementPath.IsInstance) && managementPath.NamespacePath.Length == 0)
			{
				this.SourceObject = queryOrSourceObject;
				this.isSchemaQuery = false;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrSourceObject");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelationshipQuery" /> class for the given source object and relationship class. The query is assumed to be an instance query (as opposed to a schema query).          </summary>
		/// <param name="sourceObject"> The path of the source object for this query.</param>
		/// <param name="relationshipClass"> The type of relationship for which to query.</param>
		// Token: 0x0600020A RID: 522 RVA: 0x0000AE97 File Offset: 0x00009097
		public RelationshipQuery(string sourceObject, string relationshipClass) : this(sourceObject, relationshipClass, null, null, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelationshipQuery" /> class for the given set of parameters. The query is assumed to be an instance query (as opposed to a schema query).          </summary>
		/// <param name="sourceObject">The path of the source object for this query.</param>
		/// <param name="relationshipClass">The type of relationship for which to query.</param>
		/// <param name="relationshipQualifier">A qualifier required to be present on the relationship object.</param>
		/// <param name="thisRole">The role that the source object is required to play in the relationship.</param>
		/// <param name="classDefinitionsOnly">When this method returns, it contains a Boolean that indicates that only class definitions for the resulting objects are returned.</param>
		// Token: 0x0600020B RID: 523 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public RelationshipQuery(string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly)
		{
			this.isSchemaQuery = false;
			this.sourceObject = sourceObject;
			this.relationshipClass = relationshipClass;
			this.relationshipQualifier = relationshipQualifier;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = classDefinitionsOnly;
			this.BuildQuery();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelationshipQuery" /> class for a schema query using the given set of parameters. This constructor is used for schema queries only, so the first parameter must be true.          </summary>
		/// <param name="isSchemaQuery">
		///       <see langword="true" /> to indicate that this is a schema query; otherwise, <see langword="false" />.</param>
		/// <param name="sourceObject">The path of the source class for this query.</param>
		/// <param name="relationshipClass">The type of relationship for which to query.</param>
		/// <param name="relationshipQualifier">A qualifier required to be present on the relationship class.</param>
		/// <param name="thisRole">The role that the source class is required to play in the relationship.</param>
		// Token: 0x0600020C RID: 524 RVA: 0x0000AEE0 File Offset: 0x000090E0
		public RelationshipQuery(bool isSchemaQuery, string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.sourceObject = sourceObject;
			this.relationshipClass = relationshipClass;
			this.relationshipQualifier = relationshipQualifier;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = false;
			this.BuildQuery();
		}

		/// <summary>Gets or sets a value indicating whether this query is a schema query or an instance query.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether this query is a schema query.</returns>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000AF3D File Offset: 0x0000913D
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000AF45 File Offset: 0x00009145
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

		/// <summary>Gets or sets the source object for this query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the path of the object to be used for the query.</returns>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000AF5A File Offset: 0x0000915A
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000AF70 File Offset: 0x00009170
		public string SourceObject
		{
			get
			{
				if (this.sourceObject == null)
				{
					return string.Empty;
				}
				return this.sourceObject;
			}
			set
			{
				this.sourceObject = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the class of the relationship objects wanted in the query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the relationship class name.</returns>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000AF85 File Offset: 0x00009185
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000AF9B File Offset: 0x0000919B
		public string RelationshipClass
		{
			get
			{
				if (this.relationshipClass == null)
				{
					return string.Empty;
				}
				return this.relationshipClass;
			}
			set
			{
				this.relationshipClass = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets a qualifier required on the relationship objects.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the qualifier required on the relationship objects.</returns>
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000AFB0 File Offset: 0x000091B0
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000AFC6 File Offset: 0x000091C6
		public string RelationshipQualifier
		{
			get
			{
				if (this.relationshipQualifier == null)
				{
					return string.Empty;
				}
				return this.relationshipQualifier;
			}
			set
			{
				this.relationshipQualifier = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the role of the source object in the relationship.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the role of this object.</returns>
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000AFDB File Offset: 0x000091DB
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000AFF1 File Offset: 0x000091F1
		public string ThisRole
		{
			get
			{
				if (this.thisRole == null)
				{
					return string.Empty;
				}
				return this.thisRole;
			}
			set
			{
				this.thisRole = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets a value indicating that only the class definitions of the relevant relationship objects be returned.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating that only the class definitions of the relevant relationship objects be returned.</returns>
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000B006 File Offset: 0x00009206
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000B00E File Offset: 0x0000920E
		public bool ClassDefinitionsOnly
		{
			get
			{
				return this.classDefinitionsOnly;
			}
			set
			{
				this.classDefinitionsOnly = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Builds the query string according to the current property values.                       </summary>
		// Token: 0x06000219 RID: 537 RVA: 0x0000B024 File Offset: 0x00009224
		protected internal void BuildQuery()
		{
			if (this.sourceObject == null)
			{
				base.SetQueryString(string.Empty);
			}
			if (this.sourceObject == null || this.sourceObject.Length == 0)
			{
				return;
			}
			string text = string.Concat(new string[]
			{
				RelationshipQuery.tokenReferences,
				" ",
				RelationshipQuery.tokenOf,
				" {",
				this.sourceObject,
				"}"
			});
			if (this.RelationshipClass.Length != 0 || this.RelationshipQualifier.Length != 0 || this.ThisRole.Length != 0 || this.classDefinitionsOnly || this.isSchemaQuery)
			{
				text = text + " " + RelationshipQuery.tokenWhere;
				if (this.RelationshipClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenResultClass,
						" = ",
						this.relationshipClass
					});
				}
				if (this.ThisRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenRole,
						" = ",
						this.thisRole
					});
				}
				if (this.RelationshipQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenRequiredQualifier,
						" = ",
						this.relationshipQualifier
					});
				}
				if (!this.isSchemaQuery)
				{
					if (this.classDefinitionsOnly)
					{
						text = text + " " + RelationshipQuery.tokenClassDefsOnly;
					}
				}
				else
				{
					text = text + " " + RelationshipQuery.tokenSchemaOnly;
				}
			}
			base.SetQueryString(text);
		}

		/// <summary>Parses the query string and sets the property values accordingly.                       </summary>
		/// <param name="query">The query string to be parsed.</param>
		// Token: 0x0600021A RID: 538 RVA: 0x0000B1D8 File Offset: 0x000093D8
		protected internal override void ParseQuery(string query)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			bool flag = false;
			bool flag2 = false;
			string text4 = query.Trim();
			if (string.Compare(text4, 0, RelationshipQuery.tokenReferences, 0, RelationshipQuery.tokenReferences.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "references");
			}
			text4 = text4.Remove(0, RelationshipQuery.tokenReferences.Length);
			if (text4.Length == 0 || !char.IsWhiteSpace(text4[0]))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text4 = text4.TrimStart(null);
			if (string.Compare(text4, 0, RelationshipQuery.tokenOf, 0, RelationshipQuery.tokenOf.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "of");
			}
			text4 = text4.Remove(0, RelationshipQuery.tokenOf.Length).TrimStart(null);
			if (text4.IndexOf('{') != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text4 = text4.Remove(0, 1).TrimStart(null);
			int num;
			if (-1 == (num = text4.IndexOf('}')))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			string text5 = text4.Substring(0, num).TrimEnd(null);
			text4 = text4.Remove(0, num + 1).TrimStart(null);
			if (0 < text4.Length)
			{
				if (string.Compare(text4, 0, RelationshipQuery.tokenWhere, 0, RelationshipQuery.tokenWhere.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
				}
				text4 = text4.Remove(0, RelationshipQuery.tokenWhere.Length);
				if (text4.Length == 0 || !char.IsWhiteSpace(text4[0]))
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				text4 = text4.TrimStart(null);
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					if (text4.Length >= RelationshipQuery.tokenResultClass.Length && string.Compare(text4, 0, RelationshipQuery.tokenResultClass, 0, RelationshipQuery.tokenResultClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenResultClass, "=", ref flag3, ref text);
					}
					else if (text4.Length >= RelationshipQuery.tokenRole.Length && string.Compare(text4, 0, RelationshipQuery.tokenRole, 0, RelationshipQuery.tokenRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenRole, "=", ref flag4, ref text2);
					}
					else if (text4.Length >= RelationshipQuery.tokenRequiredQualifier.Length && string.Compare(text4, 0, RelationshipQuery.tokenRequiredQualifier, 0, RelationshipQuery.tokenRequiredQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenRequiredQualifier, "=", ref flag5, ref text3);
					}
					else if (text4.Length >= RelationshipQuery.tokenClassDefsOnly.Length && string.Compare(text4, 0, RelationshipQuery.tokenClassDefsOnly, 0, RelationshipQuery.tokenClassDefsOnly.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenClassDefsOnly, ref flag6);
						flag = true;
					}
					else
					{
						if (text4.Length < RelationshipQuery.tokenSchemaOnly.Length || string.Compare(text4, 0, RelationshipQuery.tokenSchemaOnly, 0, RelationshipQuery.tokenSchemaOnly.Length, StringComparison.OrdinalIgnoreCase) != 0)
						{
							break;
						}
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenSchemaOnly, ref flag7);
						flag2 = true;
					}
				}
				if (text4.Length != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				if (flag && flag2)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
			}
			this.sourceObject = text5;
			this.relationshipClass = text;
			this.thisRole = text2;
			this.relationshipQualifier = text3;
			this.classDefinitionsOnly = flag;
			this.isSchemaQuery = flag2;
		}

		/// <summary>Creates a copy of the object.          </summary>
		/// <returns>The copied object.             </returns>
		// Token: 0x0600021B RID: 539 RVA: 0x0000B578 File Offset: 0x00009778
		public override object Clone()
		{
			if (!this.isSchemaQuery)
			{
				return new RelationshipQuery(this.sourceObject, this.relationshipClass, this.relationshipQualifier, this.thisRole, this.classDefinitionsOnly);
			}
			return new RelationshipQuery(true, this.sourceObject, this.relationshipClass, this.relationshipQualifier, this.thisRole);
		}

		// Token: 0x04000169 RID: 361
		private static readonly string tokenReferences = "references";

		// Token: 0x0400016A RID: 362
		private static readonly string tokenOf = "of";

		// Token: 0x0400016B RID: 363
		private static readonly string tokenWhere = "where";

		// Token: 0x0400016C RID: 364
		private static readonly string tokenResultClass = "resultclass";

		// Token: 0x0400016D RID: 365
		private static readonly string tokenRole = "role";

		// Token: 0x0400016E RID: 366
		private static readonly string tokenRequiredQualifier = "requiredqualifier";

		// Token: 0x0400016F RID: 367
		private static readonly string tokenClassDefsOnly = "classdefsonly";

		// Token: 0x04000170 RID: 368
		private static readonly string tokenSchemaOnly = "schemaonly";

		// Token: 0x04000171 RID: 369
		private string sourceObject;

		// Token: 0x04000172 RID: 370
		private string relationshipClass;

		// Token: 0x04000173 RID: 371
		private string relationshipQualifier;

		// Token: 0x04000174 RID: 372
		private string thisRole;

		// Token: 0x04000175 RID: 373
		private bool classDefinitionsOnly;

		// Token: 0x04000176 RID: 374
		private bool isSchemaQuery;
	}
}
