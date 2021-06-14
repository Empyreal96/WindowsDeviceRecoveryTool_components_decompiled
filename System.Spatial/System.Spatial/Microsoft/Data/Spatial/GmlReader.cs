using System;
using System.Collections.Generic;
using System.Spatial;
using System.Xml;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000064 RID: 100
	internal class GmlReader : SpatialReader<XmlReader>
	{
		// Token: 0x0600027E RID: 638 RVA: 0x00006DF0 File Offset: 0x00004FF0
		public GmlReader(SpatialPipeline destination) : base(destination)
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00006DF9 File Offset: 0x00004FF9
		protected override void ReadGeographyImplementation(XmlReader input)
		{
			new GmlReader.Parser(input, new TypeWashedToGeographyLatLongPipeline(base.Destination)).Read();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00006E11 File Offset: 0x00005011
		protected override void ReadGeometryImplementation(XmlReader input)
		{
			new GmlReader.Parser(input, new TypeWashedToGeometryPipeline(base.Destination)).Read();
		}

		// Token: 0x02000065 RID: 101
		private class Parser
		{
			// Token: 0x06000281 RID: 641 RVA: 0x00006E2C File Offset: 0x0000502C
			internal Parser(XmlReader reader, TypeWashedPipeline pipeline)
			{
				this.reader = reader;
				this.pipeline = pipeline;
				XmlNameTable nameTable = this.reader.NameTable;
				this.gmlNamespace = nameTable.Add("http://www.opengis.net/gml");
				this.fullGlobeNamespace = nameTable.Add("http://schemas.microsoft.com/sqlserver/2011/geography");
			}

			// Token: 0x06000282 RID: 642 RVA: 0x00006E7B File Offset: 0x0000507B
			public void Read()
			{
				this.ParseGmlGeometry(true);
			}

			// Token: 0x06000283 RID: 643 RVA: 0x00006E84 File Offset: 0x00005084
			private void ParseGmlGeometry(bool readCoordinateSystem)
			{
				if (!this.reader.IsStartElement())
				{
					throw new FormatException(Strings.GmlReader_ExpectReaderAtElement);
				}
				if (object.ReferenceEquals(this.reader.NamespaceURI, this.gmlNamespace))
				{
					this.ReadAttributes(readCoordinateSystem);
					string localName;
					switch (localName = this.reader.LocalName)
					{
					case "Point":
						this.ParseGmlPointShape();
						return;
					case "LineString":
						this.ParseGmlLineStringShape();
						return;
					case "Polygon":
						this.ParseGmlPolygonShape();
						return;
					case "MultiPoint":
						this.ParseGmlMultiPointShape();
						return;
					case "MultiCurve":
						this.ParseGmlMultiCurveShape();
						return;
					case "MultiSurface":
						this.ParseGmlMultiSurfaceShape();
						return;
					case "MultiGeometry":
						this.ParseGmlMultiGeometryShape();
						return;
					}
					throw new FormatException(Strings.GmlReader_InvalidSpatialType(this.reader.LocalName));
				}
				if (object.ReferenceEquals(this.reader.NamespaceURI, this.fullGlobeNamespace) && this.reader.LocalName.Equals("FullGlobe"))
				{
					this.ReadAttributes(readCoordinateSystem);
					this.ParseGmlFullGlobeElement();
					return;
				}
				throw new FormatException(Strings.GmlReader_ExpectReaderAtElement);
			}

			// Token: 0x06000284 RID: 644 RVA: 0x0000700C File Offset: 0x0000520C
			private void ReadAttributes(bool expectSrsName)
			{
				bool flag = false;
				this.reader.MoveToContent();
				if (this.reader.MoveToFirstAttribute())
				{
					string localName;
					for (;;)
					{
						if (!this.reader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/", StringComparison.Ordinal))
						{
							localName = this.reader.LocalName;
							string a;
							if ((a = localName) == null)
							{
								goto IL_11B;
							}
							if (!(a == "axisLabels") && !(a == "uomLabels") && !(a == "count") && !(a == "id"))
							{
								if (!(a == "srsName"))
								{
									goto IL_11B;
								}
								if (!expectSrsName)
								{
									break;
								}
								string value = this.reader.Value;
								if (!value.StartsWith("http://www.opengis.net/def/crs/EPSG/0/", StringComparison.Ordinal))
								{
									goto IL_10B;
								}
								int value2 = XmlConvert.ToInt32(value.Substring("http://www.opengis.net/def/crs/EPSG/0/".Length));
								this.pipeline.SetCoordinateSystem(new int?(value2));
								flag = true;
							}
						}
						if (!this.reader.MoveToNextAttribute())
						{
							goto Block_10;
						}
					}
					this.reader.MoveToElement();
					throw new FormatException(Strings.GmlReader_InvalidAttribute(localName, this.reader.Name));
					IL_10B:
					throw new FormatException(Strings.GmlReader_InvalidSrsName("http://www.opengis.net/def/crs/EPSG/0/"));
					IL_11B:
					this.reader.MoveToElement();
					throw new FormatException(Strings.GmlReader_InvalidAttribute(localName, this.reader.Name));
					Block_10:
					this.reader.MoveToElement();
				}
				if (expectSrsName && !flag)
				{
					this.pipeline.SetCoordinateSystem(null);
				}
			}

			// Token: 0x06000285 RID: 645 RVA: 0x0000718E File Offset: 0x0000538E
			private void ParseGmlPointShape()
			{
				this.pipeline.BeginGeo(SpatialType.Point);
				this.PrepareFigure();
				this.ParseGmlPointElement(true);
				this.EndFigure();
				this.pipeline.EndGeo();
			}

			// Token: 0x06000286 RID: 646 RVA: 0x000071BA File Offset: 0x000053BA
			private void ParseGmlLineStringShape()
			{
				this.pipeline.BeginGeo(SpatialType.LineString);
				this.PrepareFigure();
				this.ParseGmlLineString();
				this.EndFigure();
				this.pipeline.EndGeo();
			}

			// Token: 0x06000287 RID: 647 RVA: 0x000071E8 File Offset: 0x000053E8
			private void ParseGmlPolygonShape()
			{
				this.pipeline.BeginGeo(SpatialType.Polygon);
				if (this.ReadStartOrEmptyElement("Polygon"))
				{
					this.ReadSkippableElements();
					if (!this.IsEndElement("Polygon"))
					{
						this.PrepareFigure();
						this.ParseGmlRingElement("exterior");
						this.EndFigure();
						this.ReadSkippableElements();
						while (this.IsStartElement("interior"))
						{
							this.PrepareFigure();
							this.ParseGmlRingElement("interior");
							this.EndFigure();
							this.ReadSkippableElements();
						}
					}
					this.ReadSkippableElements();
					this.ReadEndElement();
				}
				this.pipeline.EndGeo();
			}

			// Token: 0x06000288 RID: 648 RVA: 0x00007281 File Offset: 0x00005481
			private void ParseGmlMultiPointShape()
			{
				this.pipeline.BeginGeo(SpatialType.MultiPoint);
				this.ParseMultiItemElement("MultiPoint", "pointMember", "pointMembers", new Action(this.ParseGmlPointShape));
				this.pipeline.EndGeo();
			}

			// Token: 0x06000289 RID: 649 RVA: 0x000072BB File Offset: 0x000054BB
			private void ParseGmlMultiCurveShape()
			{
				this.pipeline.BeginGeo(SpatialType.MultiLineString);
				this.ParseMultiItemElement("MultiCurve", "curveMember", "curveMembers", new Action(this.ParseGmlLineStringShape));
				this.pipeline.EndGeo();
			}

			// Token: 0x0600028A RID: 650 RVA: 0x000072F5 File Offset: 0x000054F5
			private void ParseGmlMultiSurfaceShape()
			{
				this.pipeline.BeginGeo(SpatialType.MultiPolygon);
				this.ParseMultiItemElement("MultiSurface", "surfaceMember", "surfaceMembers", new Action(this.ParseGmlPolygonShape));
				this.pipeline.EndGeo();
			}

			// Token: 0x0600028B RID: 651 RVA: 0x00007338 File Offset: 0x00005538
			private void ParseGmlMultiGeometryShape()
			{
				this.pipeline.BeginGeo(SpatialType.Collection);
				this.ParseMultiItemElement("MultiGeometry", "geometryMember", "geometryMembers", delegate
				{
					this.ParseGmlGeometry(false);
				});
				this.pipeline.EndGeo();
			}

			// Token: 0x0600028C RID: 652 RVA: 0x00007372 File Offset: 0x00005572
			private void ParseGmlFullGlobeElement()
			{
				this.pipeline.BeginGeo(SpatialType.FullGlobe);
				if (this.ReadStartOrEmptyElement("FullGlobe") && this.IsEndElement("FullGlobe"))
				{
					this.ReadEndElement();
				}
				this.pipeline.EndGeo();
			}

			// Token: 0x0600028D RID: 653 RVA: 0x000073AC File Offset: 0x000055AC
			private void ParseGmlPointElement(bool allowEmpty)
			{
				if (this.ReadStartOrEmptyElement("Point"))
				{
					this.ReadSkippableElements();
					this.ParseGmlPosElement(allowEmpty);
					this.ReadSkippableElements();
					this.ReadEndElement();
				}
			}

			// Token: 0x0600028E RID: 654 RVA: 0x000073D4 File Offset: 0x000055D4
			private void ParseGmlLineString()
			{
				if (this.ReadStartOrEmptyElement("LineString"))
				{
					this.ReadSkippableElements();
					if (this.IsPosListStart())
					{
						this.ParsePosList(false);
					}
					else
					{
						this.ParseGmlPosListElement(true);
					}
					this.ReadSkippableElements();
					this.ReadEndElement();
				}
			}

			// Token: 0x0600028F RID: 655 RVA: 0x0000740D File Offset: 0x0000560D
			private void ParseGmlRingElement(string ringTag)
			{
				if (this.ReadStartOrEmptyElement(ringTag))
				{
					if (!this.IsEndElement(ringTag))
					{
						this.ParseGmlLinearRingElement();
					}
					this.ReadEndElement();
				}
			}

			// Token: 0x06000290 RID: 656 RVA: 0x00007430 File Offset: 0x00005630
			private void ParseGmlLinearRingElement()
			{
				if (this.ReadStartOrEmptyElement("LinearRing"))
				{
					if (this.IsEndElement("LinearRing"))
					{
						throw new FormatException(Strings.GmlReader_EmptyRingsNotAllowed);
					}
					if (this.IsPosListStart())
					{
						this.ParsePosList(false);
					}
					else
					{
						this.ParseGmlPosListElement(false);
					}
					this.ReadEndElement();
				}
			}

			// Token: 0x06000291 RID: 657 RVA: 0x00007480 File Offset: 0x00005680
			private void ParseMultiItemElement(string header, string member, string members, Action parseItem)
			{
				if (this.ReadStartOrEmptyElement(header))
				{
					this.ReadSkippableElements();
					if (!this.IsEndElement(header))
					{
						while (this.IsStartElement(member))
						{
							if (this.ReadStartOrEmptyElement(member) && !this.IsEndElement(member))
							{
								parseItem();
								this.ReadEndElement();
								this.ReadSkippableElements();
							}
						}
						if (this.IsStartElement(members) && this.ReadStartOrEmptyElement(members))
						{
							while (this.reader.IsStartElement())
							{
								parseItem();
							}
							this.ReadEndElement();
						}
					}
					this.ReadSkippableElements();
					this.ReadEndElement();
				}
			}

			// Token: 0x06000292 RID: 658 RVA: 0x00007510 File Offset: 0x00005710
			private void ParseGmlPosElement(bool allowEmpty)
			{
				this.ReadAttributes(false);
				if (this.ReadStartOrEmptyElement("pos"))
				{
					double[] array = this.ReadContentAsDoubleArray();
					if (array.Length != 0)
					{
						if (array.Length < 2)
						{
							throw new FormatException(Strings.GmlReader_PosNeedTwoNumbers);
						}
						this.AddPoint(array[0], array[1], (array.Length > 2) ? new double?(array[2]) : null, (array.Length > 3) ? new double?(array[3]) : null);
					}
					else if (!allowEmpty)
					{
						throw new FormatException(Strings.GmlReader_PosNeedTwoNumbers);
					}
					this.ReadEndElement();
					return;
				}
				if (!allowEmpty)
				{
					throw new FormatException(Strings.GmlReader_PosNeedTwoNumbers);
				}
			}

			// Token: 0x06000293 RID: 659 RVA: 0x000075B1 File Offset: 0x000057B1
			private void ParsePosList(bool allowEmpty)
			{
				do
				{
					if (this.IsStartElement("pos"))
					{
						this.ParseGmlPosElement(allowEmpty);
					}
					else
					{
						this.ParseGmlPointPropertyElement(allowEmpty);
					}
				}
				while (this.IsPosListStart());
			}

			// Token: 0x06000294 RID: 660 RVA: 0x000075D8 File Offset: 0x000057D8
			private void ParseGmlPointPropertyElement(bool allowEmpty)
			{
				if (this.ReadStartOrEmptyElement("pointProperty"))
				{
					this.ParseGmlPointElement(allowEmpty);
					this.ReadEndElement();
				}
			}

			// Token: 0x06000295 RID: 661 RVA: 0x000075F4 File Offset: 0x000057F4
			private void ParseGmlPosListElement(bool allowEmpty)
			{
				if (this.ReadStartOrEmptyElement("posList"))
				{
					if (!this.IsEndElement("posList"))
					{
						double[] array = this.ReadContentAsDoubleArray();
						if (array.Length == 0)
						{
							throw new FormatException(Strings.GmlReader_PosListNeedsEvenCount);
						}
						if (array.Length % 2 != 0)
						{
							throw new FormatException(Strings.GmlReader_PosListNeedsEvenCount);
						}
						for (int i = 0; i < array.Length; i += 2)
						{
							this.AddPoint(array[i], array[i + 1], null, null);
						}
					}
					else if (!allowEmpty)
					{
						throw new FormatException(Strings.GmlReader_PosListNeedsEvenCount);
					}
					this.ReadEndElement();
					return;
				}
				if (!allowEmpty)
				{
					throw new FormatException(Strings.GmlReader_PosListNeedsEvenCount);
				}
			}

			// Token: 0x06000296 RID: 662 RVA: 0x00007698 File Offset: 0x00005898
			private double[] ReadContentAsDoubleArray()
			{
				string[] array = this.reader.ReadContentAsString().Split(GmlReader.Parser.coordinateDelimiter, StringSplitOptions.RemoveEmptyEntries);
				double[] array2 = new double[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = XmlConvert.ToDouble(array[i]);
				}
				return array2;
			}

			// Token: 0x06000297 RID: 663 RVA: 0x000076E0 File Offset: 0x000058E0
			private bool ReadStartOrEmptyElement(string element)
			{
				bool isEmptyElement = this.reader.IsEmptyElement;
				if (element != "FullGlobe")
				{
					this.reader.ReadStartElement(element, this.gmlNamespace);
				}
				else
				{
					this.reader.ReadStartElement(element, "http://schemas.microsoft.com/sqlserver/2011/geography");
				}
				return !isEmptyElement;
			}

			// Token: 0x06000298 RID: 664 RVA: 0x0000772F File Offset: 0x0000592F
			private bool IsStartElement(string element)
			{
				return this.reader.IsStartElement(element, this.gmlNamespace);
			}

			// Token: 0x06000299 RID: 665 RVA: 0x00007743 File Offset: 0x00005943
			private bool IsEndElement(string element)
			{
				this.reader.MoveToContent();
				return this.reader.NodeType == XmlNodeType.EndElement && this.reader.LocalName.Equals(element, StringComparison.Ordinal);
			}

			// Token: 0x0600029A RID: 666 RVA: 0x00007774 File Offset: 0x00005974
			private void ReadEndElement()
			{
				this.reader.MoveToContent();
				if (this.reader.NodeType != XmlNodeType.EndElement)
				{
					throw new FormatException(Strings.GmlReader_UnexpectedElement(this.reader.Name));
				}
				this.reader.ReadEndElement();
			}

			// Token: 0x0600029B RID: 667 RVA: 0x000077B4 File Offset: 0x000059B4
			private void ReadSkippableElements()
			{
				bool flag = true;
				while (flag)
				{
					this.reader.MoveToContent();
					if (this.reader.NodeType == XmlNodeType.Element && object.ReferenceEquals(this.reader.NamespaceURI, this.gmlNamespace))
					{
						string localName = this.reader.LocalName;
						flag = GmlReader.Parser.skippableElements.ContainsKey(localName);
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						this.reader.Skip();
					}
				}
			}

			// Token: 0x0600029C RID: 668 RVA: 0x00007824 File Offset: 0x00005A24
			private bool IsPosListStart()
			{
				return this.IsStartElement("pos") || this.IsStartElement("pointProperty");
			}

			// Token: 0x0600029D RID: 669 RVA: 0x00007840 File Offset: 0x00005A40
			private void PrepareFigure()
			{
				this.points = 0;
			}

			// Token: 0x0600029E RID: 670 RVA: 0x0000784C File Offset: 0x00005A4C
			private void AddPoint(double x, double y, double? z, double? m)
			{
				if (z != null && double.IsNaN(z.Value))
				{
					z = null;
				}
				if (m != null && double.IsNaN(m.Value))
				{
					m = null;
				}
				if (this.points == 0)
				{
					this.pipeline.BeginFigure(x, y, z, m);
				}
				else
				{
					this.pipeline.LineTo(x, y, z, m);
				}
				this.points++;
			}

			// Token: 0x0600029F RID: 671 RVA: 0x000078CF File Offset: 0x00005ACF
			private void EndFigure()
			{
				if (this.points > 0)
				{
					this.pipeline.EndFigure();
				}
			}

			// Token: 0x04000079 RID: 121
			private static readonly char[] coordinateDelimiter = new char[]
			{
				' ',
				'\t',
				'\r',
				'\n'
			};

			// Token: 0x0400007A RID: 122
			private static readonly Dictionary<string, string> skippableElements = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{
					"name",
					"name"
				},
				{
					"description",
					"description"
				},
				{
					"metaDataProperty",
					"metaDataProperty"
				},
				{
					"descriptionReference",
					"descriptionReference"
				},
				{
					"identifier",
					"identifier"
				}
			};

			// Token: 0x0400007B RID: 123
			private readonly string gmlNamespace;

			// Token: 0x0400007C RID: 124
			private readonly string fullGlobeNamespace;

			// Token: 0x0400007D RID: 125
			private readonly TypeWashedPipeline pipeline;

			// Token: 0x0400007E RID: 126
			private readonly XmlReader reader;

			// Token: 0x0400007F RID: 127
			private int points;
		}
	}
}
