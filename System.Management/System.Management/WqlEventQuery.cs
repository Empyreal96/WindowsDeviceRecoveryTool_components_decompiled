using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Management
{
	/// <summary>Represents a WMI event query in WQL format.          </summary>
	// Token: 0x0200003C RID: 60
	public class WqlEventQuery : EventQuery
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class. This is the default constructor.          </summary>
		// Token: 0x0600021D RID: 541 RVA: 0x0000B62D File Offset: 0x0000982D
		public WqlEventQuery() : this(null, TimeSpan.Zero, null, TimeSpan.Zero, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class based on the given query string or event class name.          </summary>
		/// <param name="queryOrEventClassName">The string representing either the entire event query or the name of the event class to query. The object will try to parse the string as a valid event query. If unsuccessful, the parser will assume that the parameter represents an event class name.</param>
		// Token: 0x0600021E RID: 542 RVA: 0x0000B644 File Offset: 0x00009844
		public WqlEventQuery(string queryOrEventClassName)
		{
			this.groupByPropertyList = new StringCollection();
			if (queryOrEventClassName == null)
			{
				return;
			}
			if (queryOrEventClassName.TrimStart(new char[0]).StartsWith(WqlEventQuery.tokenSelectAll, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrEventClassName;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrEventClassName);
			if (managementPath.IsClass && managementPath.NamespacePath.Length == 0)
			{
				this.EventClassName = queryOrEventClassName;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrEventClassName");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class for the specified event class name, with the specified condition.          </summary>
		/// <param name="eventClassName">The name of the event class to query.</param>
		/// <param name="condition">The condition to apply to events of the specified class. </param>
		// Token: 0x0600021F RID: 543 RVA: 0x0000B6BF File Offset: 0x000098BF
		public WqlEventQuery(string eventClassName, string condition) : this(eventClassName, TimeSpan.Zero, condition, TimeSpan.Zero, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class for the specified event class, with the specified latency time.          </summary>
		/// <param name="eventClassName">The name of the event class to query.</param>
		/// <param name="withinInterval">A <see cref="T:System.TimeSpan" /> value specifying the latency acceptable for receiving this event. This value is used in cases where there is no explicit event provider for the query requested, and WMI is required to poll for the condition. This interval is the maximum amount of time that can pass before notification of an event must be delivered.  </param>
		// Token: 0x06000220 RID: 544 RVA: 0x0000B6D5 File Offset: 0x000098D5
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval) : this(eventClassName, withinInterval, null, TimeSpan.Zero, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class with the specified event class name, polling interval, and condition.          </summary>
		/// <param name="eventClassName">The name of the event class to query. </param>
		/// <param name="withinInterval">A <see cref="T:System.TimeSpan" /> value specifying the latency acceptable for receiving this event. This value is used in cases where there is no explicit event provider for the query requested and WMI is required to poll for the condition. This interval is the maximum amount of time that can pass before notification of an event must be delivered. </param>
		/// <param name="condition">The condition to apply to events of the specified class. </param>
		// Token: 0x06000221 RID: 545 RVA: 0x0000B6E7 File Offset: 0x000098E7
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition) : this(eventClassName, withinInterval, condition, TimeSpan.Zero, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class with the specified event class name, condition, and grouping interval.          </summary>
		/// <param name="eventClassName">The name of the event class to query. </param>
		/// <param name="condition">The condition to apply to events of the specified class.</param>
		/// <param name="groupWithinInterval">The specified interval at which WMI sends one <paramref name="aggregate event" />, rather than many events. </param>
		// Token: 0x06000222 RID: 546 RVA: 0x0000B6F9 File Offset: 0x000098F9
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval) : this(eventClassName, TimeSpan.Zero, condition, groupWithinInterval, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class with the specified event class name, condition, grouping interval, and grouping properties.          </summary>
		/// <param name="eventClassName">The name of the event class to query. </param>
		/// <param name="condition">The condition to apply to events of the specified class.</param>
		/// <param name="groupWithinInterval">The specified interval at which WMI sends one <paramref name="aggregate event" />, rather than many events.</param>
		/// <param name="groupByPropertyList">The properties in the event class by which the events should be grouped.  </param>
		// Token: 0x06000223 RID: 547 RVA: 0x0000B70B File Offset: 0x0000990B
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList) : this(eventClassName, TimeSpan.Zero, condition, groupWithinInterval, groupByPropertyList, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.WqlEventQuery" /> class with the specified event class name, condition, grouping interval, grouping properties, and specified number of events.          </summary>
		/// <param name="eventClassName">The name of the event class on which to be queried.</param>
		/// <param name="withinInterval">A <see cref="T:System.TimeSpan" /> value specifying the latency acceptable for receiving this event. This value is used in cases where there is no explicit event provider for the query requested, and WMI is required to poll for the condition. This interval is the maximum amount of time that can pass before notification of an event must be delivered.</param>
		/// <param name="condition">The condition to apply to events of the specified class. </param>
		/// <param name="groupWithinInterval">The specified interval at which WMI sends one <paramref name="aggregate event" />, rather than many events. </param>
		/// <param name="groupByPropertyList">The properties in the event class by which the events should be grouped. </param>
		/// <param name="havingCondition">The condition to apply to the number of events. </param>
		// Token: 0x06000224 RID: 548 RVA: 0x0000B720 File Offset: 0x00009920
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList, string havingCondition)
		{
			this.eventClassName = eventClassName;
			this.withinInterval = withinInterval;
			this.condition = condition;
			this.groupWithinInterval = groupWithinInterval;
			this.groupByPropertyList = new StringCollection();
			if (groupByPropertyList != null)
			{
				this.groupByPropertyList.AddRange(groupByPropertyList);
			}
			this.havingCondition = havingCondition;
			this.BuildQuery();
		}

		/// <summary>Gets  the language of the query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value that contains the query language that the query is written in.</returns>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00009BA7 File Offset: 0x00007DA7
		public override string QueryLanguage
		{
			get
			{
				return base.QueryLanguage;
			}
		}

		/// <summary>Gets or sets the string representing the query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the query.</returns>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000B77A File Offset: 0x0000997A
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00009CE7 File Offset: 0x00007EE7
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

		/// <summary>Gets or sets the event class to query.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the event class in the event query.</returns>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000B788 File Offset: 0x00009988
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000B79E File Offset: 0x0000999E
		public string EventClassName
		{
			get
			{
				if (this.eventClassName == null)
				{
					return string.Empty;
				}
				return this.eventClassName;
			}
			set
			{
				this.eventClassName = value;
				this.BuildQuery();
			}
		}

		/// <summary>Gets or sets the condition to be applied to events of the specified class.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the condition or conditions in the event query.</returns>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000B7AD File Offset: 0x000099AD
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000B7C3 File Offset: 0x000099C3
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
			}
		}

		/// <summary>Gets or sets the polling interval to be used in this query.          </summary>
		/// <returns>Returns a <see cref="T:System.TimeSpan" /> value containing the polling interval used in the event query.</returns>
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000B7D2 File Offset: 0x000099D2
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000B7DA File Offset: 0x000099DA
		public TimeSpan WithinInterval
		{
			get
			{
				return this.withinInterval;
			}
			set
			{
				this.withinInterval = value;
				this.BuildQuery();
			}
		}

		/// <summary>Gets or sets the interval to be used for grouping events of the same type.          </summary>
		/// <returns>Returns a <see cref="T:System.TimeSpan" /> value containing the interval used for grouping events of the same type.</returns>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000B7E9 File Offset: 0x000099E9
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000B7F1 File Offset: 0x000099F1
		public TimeSpan GroupWithinInterval
		{
			get
			{
				return this.groupWithinInterval;
			}
			set
			{
				this.groupWithinInterval = value;
				this.BuildQuery();
			}
		}

		/// <summary>Gets or sets properties in the event to be used for grouping events of the same type.          </summary>
		/// <returns>Returns a <see cref="T:System.Collections.Specialized.StringCollection" /> containing the properties in the event to be used for grouping events of the same type.</returns>
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B800 File Offset: 0x00009A00
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000B808 File Offset: 0x00009A08
		public StringCollection GroupByPropertyList
		{
			get
			{
				return this.groupByPropertyList;
			}
			set
			{
				StringCollection stringCollection = new StringCollection();
				foreach (string value2 in value)
				{
					stringCollection.Add(value2);
				}
				this.groupByPropertyList = stringCollection;
				this.BuildQuery();
			}
		}

		/// <summary>Gets or sets the condition to be applied to the aggregation of events, based on the number of events received.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the condition applied to the aggregation of events, based on the number of events received.</returns>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000B870 File Offset: 0x00009A70
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000B886 File Offset: 0x00009A86
		public string HavingCondition
		{
			get
			{
				if (this.havingCondition == null)
				{
					return string.Empty;
				}
				return this.havingCondition;
			}
			set
			{
				this.havingCondition = value;
				this.BuildQuery();
			}
		}

		/// <summary>Builds the query string according to the current property values.                       </summary>
		// Token: 0x06000234 RID: 564 RVA: 0x0000B898 File Offset: 0x00009A98
		protected internal void BuildQuery()
		{
			if (this.eventClassName == null || this.eventClassName.Length == 0)
			{
				base.SetQueryString(string.Empty);
				return;
			}
			string text = WqlEventQuery.tokenSelectAll;
			text = text + "from " + this.eventClassName;
			if (this.withinInterval != TimeSpan.Zero)
			{
				text = text + " within " + this.withinInterval.TotalSeconds.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
			}
			if (this.Condition.Length != 0)
			{
				text = text + " where " + this.condition;
			}
			if (this.groupWithinInterval != TimeSpan.Zero)
			{
				text = text + " group within " + this.groupWithinInterval.TotalSeconds.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
				if (this.groupByPropertyList != null && 0 < this.groupByPropertyList.Count)
				{
					int count = this.groupByPropertyList.Count;
					text += " by ";
					for (int i = 0; i < count; i++)
					{
						text = text + this.groupByPropertyList[i] + ((i == count - 1) ? "" : ",");
					}
				}
				if (this.HavingCondition.Length != 0)
				{
					text = text + " having " + this.havingCondition;
				}
			}
			base.SetQueryString(text);
		}

		/// <summary>Parses the query string and sets the property values accordingly.                       </summary>
		/// <param name="query">The query string to be parsed.</param>
		// Token: 0x06000235 RID: 565 RVA: 0x0000BA1C File Offset: 0x00009C1C
		protected internal override void ParseQuery(string query)
		{
			this.eventClassName = null;
			this.withinInterval = TimeSpan.Zero;
			this.condition = null;
			this.groupWithinInterval = TimeSpan.Zero;
			if (this.groupByPropertyList != null)
			{
				this.groupByPropertyList.Clear();
			}
			this.havingCondition = null;
			string text = query.Trim();
			bool flag = false;
			string text2 = ManagementQuery.tokenSelect;
			if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text = text.Remove(0, text2.Length).TrimStart(null);
			if (!text.StartsWith("*", StringComparison.Ordinal))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "*");
			}
			text = text.Remove(0, 1).TrimStart(null);
			text2 = "from ";
			if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "from");
			}
			ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref this.eventClassName);
			text2 = "within ";
			if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				string text3 = null;
				flag = false;
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref text3);
				this.withinInterval = TimeSpan.FromSeconds(((IConvertible)text3).ToDouble(null));
			}
			text2 = "group within ";
			int num;
			string text4;
			if (text.Length >= text2.Length && (num = text.ToLower(CultureInfo.InvariantCulture).IndexOf(text2, StringComparison.Ordinal)) != -1)
			{
				text4 = text.Substring(0, num).Trim();
				text = text.Remove(0, num);
				string text5 = null;
				flag = false;
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref text5);
				this.groupWithinInterval = TimeSpan.FromSeconds(((IConvertible)text5).ToDouble(null));
				text2 = "by ";
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = text.Remove(0, text2.Length);
					if (this.groupByPropertyList != null)
					{
						this.groupByPropertyList.Clear();
					}
					else
					{
						this.groupByPropertyList = new StringCollection();
					}
					string text6;
					while ((num = text.IndexOf(',')) > 0)
					{
						text6 = text.Substring(0, num);
						text = text.Remove(0, num + 1).TrimStart(null);
						text6 = text6.Trim();
						if (text6.Length > 0)
						{
							this.groupByPropertyList.Add(text6);
						}
					}
					if ((num = text.IndexOf(' ')) <= 0)
					{
						this.groupByPropertyList.Add(text);
						return;
					}
					text6 = text.Substring(0, num);
					text = text.Remove(0, num).TrimStart(null);
					this.groupByPropertyList.Add(text6);
				}
				text2 = "having ";
				flag = false;
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = text.Remove(0, text2.Length);
					if (text.Length == 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"), "having");
					}
					this.havingCondition = text;
				}
			}
			else
			{
				text4 = text.Trim();
			}
			text2 = "where ";
			if (text4.Length >= text2.Length && string.Compare(text4, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.condition = text4.Substring(text2.Length);
			}
		}

		/// <summary>Creates a copy of the object.          </summary>
		/// <returns>The copied object.             </returns>
		// Token: 0x06000236 RID: 566 RVA: 0x0000BD94 File Offset: 0x00009F94
		public override object Clone()
		{
			string[] array = null;
			if (this.groupByPropertyList != null)
			{
				int count = this.groupByPropertyList.Count;
				if (0 < count)
				{
					array = new string[count];
					this.groupByPropertyList.CopyTo(array, 0);
				}
			}
			return new WqlEventQuery(this.eventClassName, this.withinInterval, this.condition, this.groupWithinInterval, array, this.havingCondition);
		}

		// Token: 0x04000177 RID: 375
		private static readonly string tokenSelectAll = "select * ";

		// Token: 0x04000178 RID: 376
		private string eventClassName;

		// Token: 0x04000179 RID: 377
		private TimeSpan withinInterval;

		// Token: 0x0400017A RID: 378
		private string condition;

		// Token: 0x0400017B RID: 379
		private TimeSpan groupWithinInterval;

		// Token: 0x0400017C RID: 380
		private StringCollection groupByPropertyList;

		// Token: 0x0400017D RID: 381
		private string havingCondition;
	}
}
