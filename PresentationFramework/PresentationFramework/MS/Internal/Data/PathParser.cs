using System;
using System.Collections;
using System.Text;
using System.Windows;
using MS.Utility;

namespace MS.Internal.Data
{
	// Token: 0x0200073D RID: 1853
	internal class PathParser
	{
		// Token: 0x17001C22 RID: 7202
		// (get) Token: 0x06007628 RID: 30248 RVA: 0x0021AE75 File Offset: 0x00219075
		public string Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x0021AE7D File Offset: 0x0021907D
		private void SetError(string id, params object[] args)
		{
			this._error = SR.Get(id, args);
		}

		// Token: 0x0600762A RID: 30250 RVA: 0x0021AE8C File Offset: 0x0021908C
		public SourceValueInfo[] Parse(string path)
		{
			this._path = ((path != null) ? path.Trim() : string.Empty);
			this._n = this._path.Length;
			if (this._n == 0)
			{
				return new SourceValueInfo[]
				{
					new SourceValueInfo(SourceValueType.Direct, DrillIn.Never, null)
				};
			}
			this._index = 0;
			this._drillIn = DrillIn.IfNeeded;
			this._al.Clear();
			this._error = null;
			this._state = PathParser.State.Init;
			while (this._state != PathParser.State.Done)
			{
				char c = (this._index < this._n) ? this._path[this._index] : '\0';
				if (char.IsWhiteSpace(c))
				{
					this._index++;
				}
				else
				{
					switch (this._state)
					{
					case PathParser.State.Init:
						if (c == '\0' || c == '.' || c == '/')
						{
							this._state = PathParser.State.DrillIn;
						}
						else
						{
							this._state = PathParser.State.Prop;
						}
						break;
					case PathParser.State.DrillIn:
						if (c <= '.')
						{
							if (c != '\0')
							{
								if (c != '.')
								{
									goto IL_13E;
								}
								this._drillIn = DrillIn.Never;
								this._index++;
							}
						}
						else if (c != '/')
						{
							if (c != '[')
							{
								goto IL_13E;
							}
						}
						else
						{
							this._drillIn = DrillIn.Always;
							this._index++;
						}
						this._state = PathParser.State.Prop;
						break;
						IL_13E:
						this.SetError("PathSyntax", new object[]
						{
							this._path.Substring(0, this._index),
							this._path.Substring(this._index)
						});
						return PathParser.EmptyInfo;
					case PathParser.State.Prop:
					{
						bool flag = false;
						if (c == '[')
						{
							flag = true;
						}
						if (flag)
						{
							this.AddIndexer();
						}
						else
						{
							this.AddProperty();
						}
						break;
					}
					}
				}
			}
			SourceValueInfo[] array;
			if (this._error == null)
			{
				array = new SourceValueInfo[this._al.Count];
				this._al.CopyTo(array);
			}
			else
			{
				array = PathParser.EmptyInfo;
			}
			return array;
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x0021B074 File Offset: 0x00219274
		private void AddProperty()
		{
			int index = this._index;
			int num = 0;
			while (this._index < this._n)
			{
				if (this._path[this._index] != '.')
				{
					break;
				}
				this._index++;
			}
			while (this._index < this._n && (num > 0 || PathParser.SpecialChars.IndexOf(this._path[this._index]) < 0))
			{
				if (this._path[this._index] == '(')
				{
					num++;
				}
				else if (this._path[this._index] == ')')
				{
					num--;
				}
				this._index++;
			}
			if (num > 0)
			{
				this.SetError("UnmatchedParen", new object[]
				{
					this._path.Substring(index)
				});
				return;
			}
			if (num < 0)
			{
				this.SetError("UnmatchedParen", new object[]
				{
					this._path.Substring(0, this._index)
				});
				return;
			}
			string text = this._path.Substring(index, this._index - index).Trim();
			SourceValueInfo sourceValueInfo = (text.Length > 0) ? new SourceValueInfo(SourceValueType.Property, this._drillIn, text) : new SourceValueInfo(SourceValueType.Direct, this._drillIn, null);
			this._al.Add(sourceValueInfo);
			this.StartNewLevel();
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x0021B1DC File Offset: 0x002193DC
		private void AddIndexer()
		{
			int num = this._index + 1;
			this._index = num;
			int num2 = num;
			int num3 = 1;
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			FrugalObjectList<IndexerParamInfo> frugalObjectList = new FrugalObjectList<IndexerParamInfo>();
			PathParser.IndexerState indexerState = PathParser.IndexerState.BeginParam;
			while (indexerState != PathParser.IndexerState.Done)
			{
				if (this._index >= this._n)
				{
					this.SetError("UnmatchedBracket", new object[]
					{
						this._path.Substring(num2 - 1)
					});
					return;
				}
				string path = this._path;
				num = this._index;
				this._index = num + 1;
				char c = path[num];
				if (c == '^' && !flag)
				{
					flag = true;
				}
				else
				{
					switch (indexerState)
					{
					case PathParser.IndexerState.BeginParam:
						if (flag)
						{
							indexerState = PathParser.IndexerState.ValueString;
							goto IL_108;
						}
						if (c == '(')
						{
							indexerState = PathParser.IndexerState.ParenString;
						}
						else if (!char.IsWhiteSpace(c))
						{
							indexerState = PathParser.IndexerState.ValueString;
							goto IL_108;
						}
						break;
					case PathParser.IndexerState.ParenString:
						if (flag)
						{
							stringBuilder.Append(c);
						}
						else if (c == ')')
						{
							indexerState = PathParser.IndexerState.ValueString;
						}
						else
						{
							stringBuilder.Append(c);
						}
						break;
					case PathParser.IndexerState.ValueString:
						goto IL_108;
					}
					IL_1CC:
					flag = false;
					continue;
					IL_108:
					if (flag)
					{
						stringBuilder2.Append(c);
						flag2 = false;
						goto IL_1CC;
					}
					if (num3 > 1)
					{
						stringBuilder2.Append(c);
						flag2 = false;
						if (c == ']')
						{
							num3--;
							goto IL_1CC;
						}
						goto IL_1CC;
					}
					else
					{
						if (char.IsWhiteSpace(c))
						{
							stringBuilder2.Append(c);
							flag2 = true;
							goto IL_1CC;
						}
						if (c == ',' || c == ']')
						{
							string paren = stringBuilder.ToString();
							string text = stringBuilder2.ToString();
							if (flag2)
							{
								text = text.TrimEnd(new char[0]);
							}
							frugalObjectList.Add(new IndexerParamInfo(paren, text));
							stringBuilder.Length = 0;
							stringBuilder2.Length = 0;
							flag2 = false;
							indexerState = ((c == ']') ? PathParser.IndexerState.Done : PathParser.IndexerState.BeginParam);
							goto IL_1CC;
						}
						stringBuilder2.Append(c);
						flag2 = false;
						if (c == '[')
						{
							num3++;
							goto IL_1CC;
						}
						goto IL_1CC;
					}
				}
			}
			SourceValueInfo sourceValueInfo = new SourceValueInfo(SourceValueType.Indexer, this._drillIn, frugalObjectList);
			this._al.Add(sourceValueInfo);
			this.StartNewLevel();
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x0021B3E8 File Offset: 0x002195E8
		private void StartNewLevel()
		{
			this._state = ((this._index < this._n) ? PathParser.State.DrillIn : PathParser.State.Done);
			this._drillIn = DrillIn.Never;
		}

		// Token: 0x04003868 RID: 14440
		private string _error;

		// Token: 0x04003869 RID: 14441
		private PathParser.State _state;

		// Token: 0x0400386A RID: 14442
		private string _path;

		// Token: 0x0400386B RID: 14443
		private int _index;

		// Token: 0x0400386C RID: 14444
		private int _n;

		// Token: 0x0400386D RID: 14445
		private DrillIn _drillIn;

		// Token: 0x0400386E RID: 14446
		private ArrayList _al = new ArrayList();

		// Token: 0x0400386F RID: 14447
		private const char NullChar = '\0';

		// Token: 0x04003870 RID: 14448
		private const char EscapeChar = '^';

		// Token: 0x04003871 RID: 14449
		private static SourceValueInfo[] EmptyInfo = new SourceValueInfo[0];

		// Token: 0x04003872 RID: 14450
		private static string SpecialChars = "./[]";

		// Token: 0x02000B55 RID: 2901
		private enum State
		{
			// Token: 0x04004AFC RID: 19196
			Init,
			// Token: 0x04004AFD RID: 19197
			DrillIn,
			// Token: 0x04004AFE RID: 19198
			Prop,
			// Token: 0x04004AFF RID: 19199
			Done
		}

		// Token: 0x02000B56 RID: 2902
		private enum IndexerState
		{
			// Token: 0x04004B01 RID: 19201
			BeginParam,
			// Token: 0x04004B02 RID: 19202
			ParenString,
			// Token: 0x04004B03 RID: 19203
			ValueString,
			// Token: 0x04004B04 RID: 19204
			Done
		}
	}
}
