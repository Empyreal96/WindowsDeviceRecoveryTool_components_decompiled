using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x0200012D RID: 301
	public static class CsdlConstants
	{
		// Token: 0x0400023A RID: 570
		internal const string CsdlFileExtension = ".csdl";

		// Token: 0x0400023B RID: 571
		internal const string Version1Namespace = "http://schemas.microsoft.com/ado/2006/04/edm";

		// Token: 0x0400023C RID: 572
		internal const string Version1Xsd = "Edm.Csdl.CSDLSchema_1.xsd";

		// Token: 0x0400023D RID: 573
		internal const string Version1_1Namespace = "http://schemas.microsoft.com/ado/2007/05/edm";

		// Token: 0x0400023E RID: 574
		internal const string Version1_1Xsd = "Edm.Csdl.CSDLSchema_1_1.xsd";

		// Token: 0x0400023F RID: 575
		internal const string Version1_2Namespace = "http://schemas.microsoft.com/ado/2008/01/edm";

		// Token: 0x04000240 RID: 576
		internal const string Version2Namespace = "http://schemas.microsoft.com/ado/2008/09/edm";

		// Token: 0x04000241 RID: 577
		internal const string Version2NamespaceAlternate = "http://schemas.microsoft.com/ado/2009/08/edm";

		// Token: 0x04000242 RID: 578
		internal const string Version2Xsd = "Edm.Csdl.CSDLSchema_2.xsd";

		// Token: 0x04000243 RID: 579
		internal const string Version3Namespace = "http://schemas.microsoft.com/ado/2009/11/edm";

		// Token: 0x04000244 RID: 580
		internal const string AnnotationNamespace = "http://schemas.microsoft.com/ado/2009/02/edm/annotation";

		// Token: 0x04000245 RID: 581
		internal const string AnnotationXsd = "Edm.Csdl.AnnotationSchema.xsd";

		// Token: 0x04000246 RID: 582
		internal const string CodeGenerationSchemaNamespace = "http://schemas.microsoft.com/ado/2006/04/codegeneration";

		// Token: 0x04000247 RID: 583
		internal const string CodeGenerationSchemaXsd = "Edm.Csdl.CodeGenerationSchema.xsd";

		// Token: 0x04000248 RID: 584
		internal const string AssociationAnnotationsAnnotation = "AssociationAnnotations";

		// Token: 0x04000249 RID: 585
		internal const string AssociationNameAnnotation = "AssociationName";

		// Token: 0x0400024A RID: 586
		internal const string AssociationNamespaceAnnotation = "AssociationNamespace";

		// Token: 0x0400024B RID: 587
		internal const string AssociationEndNameAnnotation = "AssociationEndName";

		// Token: 0x0400024C RID: 588
		internal const string AssociationSetAnnotationsAnnotation = "AssociationSetAnnotations";

		// Token: 0x0400024D RID: 589
		internal const string AssociationSetNameAnnotation = "AssociationSetName";

		// Token: 0x0400024E RID: 590
		internal const string SchemaNamespaceAnnotation = "SchemaNamespace";

		// Token: 0x0400024F RID: 591
		internal const string AnnotationSerializationLocationAnnotation = "AnnotationSerializationLocation";

		// Token: 0x04000250 RID: 592
		internal const string NamespacePrefixAnnotation = "NamespacePrefix";

		// Token: 0x04000251 RID: 593
		internal const string IsEnumMemberValueExplicitAnnotation = "IsEnumMemberValueExplicit";

		// Token: 0x04000252 RID: 594
		internal const string IsSerializedAsElementAnnotation = "IsSerializedAsElement";

		// Token: 0x04000253 RID: 595
		internal const string NamespaceAliasAnnotation = "NamespaceAlias";

		// Token: 0x04000254 RID: 596
		internal const string Attribute_Abstract = "Abstract";

		// Token: 0x04000255 RID: 597
		internal const string Attribute_Action = "Action";

		// Token: 0x04000256 RID: 598
		internal const string Attribute_Alias = "Alias";

		// Token: 0x04000257 RID: 599
		internal const string Attribute_Association = "Association";

		// Token: 0x04000258 RID: 600
		internal const string Attribute_BaseType = "BaseType";

		// Token: 0x04000259 RID: 601
		internal const string Attribute_Binary = "Binary";

		// Token: 0x0400025A RID: 602
		internal const string Attribute_Bool = "Bool";

		// Token: 0x0400025B RID: 603
		internal const string Attribute_Collation = "Collation";

		// Token: 0x0400025C RID: 604
		internal const string Attribute_ConcurrencyMode = "ConcurrencyMode";

		// Token: 0x0400025D RID: 605
		internal const string Attribute_ContainsTarget = "ContainsTarget";

		// Token: 0x0400025E RID: 606
		internal const string Attribute_DateTime = "DateTime";

		// Token: 0x0400025F RID: 607
		internal const string Attribute_DateTimeOffset = "DateTimeOffset";

		// Token: 0x04000260 RID: 608
		internal const string Attribute_Decimal = "Decimal";

		// Token: 0x04000261 RID: 609
		internal const string Attribute_DefaultValue = "DefaultValue";

		// Token: 0x04000262 RID: 610
		internal const string Attribute_FromRole = "FromRole";

		// Token: 0x04000263 RID: 611
		internal const string Attribute_ElementType = "ElementType";

		// Token: 0x04000264 RID: 612
		internal const string Attribute_Extends = "Extends";

		// Token: 0x04000265 RID: 613
		internal const string Attribute_EntityType = "EntityType";

		// Token: 0x04000266 RID: 614
		internal const string Attribute_EntitySet = "EntitySet";

		// Token: 0x04000267 RID: 615
		internal const string Attribute_EntitySetPath = "EntitySetPath";

		// Token: 0x04000268 RID: 616
		internal const string Attribute_FixedLength = "FixedLength";

		// Token: 0x04000269 RID: 617
		internal const string Attribute_Float = "Float";

		// Token: 0x0400026A RID: 618
		internal const string Attribute_Function = "Function";

		// Token: 0x0400026B RID: 619
		internal const string Attribute_Guid = "Guid";

		// Token: 0x0400026C RID: 620
		internal const string Attribute_Int = "Int";

		// Token: 0x0400026D RID: 621
		internal const string Attribute_IsBindable = "IsBindable";

		// Token: 0x0400026E RID: 622
		internal const string Attribute_IsComposable = "IsComposable";

		// Token: 0x0400026F RID: 623
		internal const string Attribute_IsFlags = "IsFlags";

		// Token: 0x04000270 RID: 624
		internal const string Attribute_IsSideEffecting = "IsSideEffecting";

		// Token: 0x04000271 RID: 625
		internal const string Attribute_MaxLength = "MaxLength";

		// Token: 0x04000272 RID: 626
		internal const string Attribute_MethodAccess = "MethodAccess";

		// Token: 0x04000273 RID: 627
		internal const string Attribute_Mode = "Mode";

		// Token: 0x04000274 RID: 628
		internal const string Attribute_Multiplicity = "Multiplicity";

		// Token: 0x04000275 RID: 629
		internal const string Attribute_Name = "Name";

		// Token: 0x04000276 RID: 630
		internal const string Attribute_Namespace = "Namespace";

		// Token: 0x04000277 RID: 631
		internal const string Attribute_Nullable = "Nullable";

		// Token: 0x04000278 RID: 632
		internal const string Attribute_OpenType = "OpenType";

		// Token: 0x04000279 RID: 633
		internal const string Attribute_Path = "Path";

		// Token: 0x0400027A RID: 634
		internal const string Attribute_Precision = "Precision";

		// Token: 0x0400027B RID: 635
		internal const string Attribute_Property = "Property";

		// Token: 0x0400027C RID: 636
		internal const string Attribute_Qualifier = "Qualifier";

		// Token: 0x0400027D RID: 637
		internal const string Attribute_Relationship = "Relationship";

		// Token: 0x0400027E RID: 638
		internal const string Attribute_ResultEnd = "ResultEnd";

		// Token: 0x0400027F RID: 639
		internal const string Attribute_ReturnType = "ReturnType";

		// Token: 0x04000280 RID: 640
		internal const string Attribute_Role = "Role";

		// Token: 0x04000281 RID: 641
		internal const string Attribute_Scale = "Scale";

		// Token: 0x04000282 RID: 642
		internal const string Attribute_Srid = "SRID";

		// Token: 0x04000283 RID: 643
		internal const string Attribute_String = "String";

		// Token: 0x04000284 RID: 644
		internal const string Attribute_Target = "Target";

		// Token: 0x04000285 RID: 645
		internal const string Attribute_Term = "Term";

		// Token: 0x04000286 RID: 646
		internal const string Attribute_Time = "Time";

		// Token: 0x04000287 RID: 647
		internal const string Attribute_ToRole = "ToRole";

		// Token: 0x04000288 RID: 648
		internal const string Attribute_Type = "Type";

		// Token: 0x04000289 RID: 649
		internal const string Attribute_UnderlyingType = "UnderlyingType";

		// Token: 0x0400028A RID: 650
		internal const string Attribute_Unicode = "Unicode";

		// Token: 0x0400028B RID: 651
		internal const string Attribute_Value = "Value";

		// Token: 0x0400028C RID: 652
		internal const string Element_Annotations = "Annotations";

		// Token: 0x0400028D RID: 653
		internal const string Element_Apply = "Apply";

		// Token: 0x0400028E RID: 654
		internal const string Element_AssertType = "AssertType";

		// Token: 0x0400028F RID: 655
		internal const string Element_Association = "Association";

		// Token: 0x04000290 RID: 656
		internal const string Element_AssociationSet = "AssociationSet";

		// Token: 0x04000291 RID: 657
		internal const string Element_Binary = "Binary";

		// Token: 0x04000292 RID: 658
		internal const string Element_Bool = "Bool";

		// Token: 0x04000293 RID: 659
		internal const string Element_Collection = "Collection";

		// Token: 0x04000294 RID: 660
		internal const string Element_CollectionType = "CollectionType";

		// Token: 0x04000295 RID: 661
		internal const string Element_ComplexType = "ComplexType";

		// Token: 0x04000296 RID: 662
		internal const string Element_DateTime = "DateTime";

		// Token: 0x04000297 RID: 663
		internal const string Element_DateTimeOffset = "DateTimeOffset";

		// Token: 0x04000298 RID: 664
		internal const string Element_Decimal = "Decimal";

		// Token: 0x04000299 RID: 665
		internal const string Element_DefiningExpression = "DefiningExpression";

		// Token: 0x0400029A RID: 666
		internal const string Element_Dependent = "Dependent";

		// Token: 0x0400029B RID: 667
		internal const string Element_Documentation = "Documentation";

		// Token: 0x0400029C RID: 668
		internal const string Element_End = "End";

		// Token: 0x0400029D RID: 669
		internal const string Element_EntityContainer = "EntityContainer";

		// Token: 0x0400029E RID: 670
		internal const string Element_EntitySet = "EntitySet";

		// Token: 0x0400029F RID: 671
		internal const string Element_EntitySetReference = "EntitySetReference";

		// Token: 0x040002A0 RID: 672
		internal const string Element_EntityType = "EntityType";

		// Token: 0x040002A1 RID: 673
		internal const string Element_EnumMemberReference = "EnumMemberReference";

		// Token: 0x040002A2 RID: 674
		internal const string Element_EnumType = "EnumType";

		// Token: 0x040002A3 RID: 675
		internal const string Element_Float = "Float";

		// Token: 0x040002A4 RID: 676
		internal const string Element_Guid = "Guid";

		// Token: 0x040002A5 RID: 677
		internal const string Element_Function = "Function";

		// Token: 0x040002A6 RID: 678
		internal const string Element_FunctionImport = "FunctionImport";

		// Token: 0x040002A7 RID: 679
		internal const string Element_FunctionReference = "FunctionReference";

		// Token: 0x040002A8 RID: 680
		internal const string Element_If = "If";

		// Token: 0x040002A9 RID: 681
		internal const string Element_IsType = "IsType";

		// Token: 0x040002AA RID: 682
		internal const string Element_Int = "Int";

		// Token: 0x040002AB RID: 683
		internal const string Element_Key = "Key";

		// Token: 0x040002AC RID: 684
		internal const string Element_LabeledElement = "LabeledElement";

		// Token: 0x040002AD RID: 685
		internal const string Element_LabeledElementReference = "LabeledElementReference";

		// Token: 0x040002AE RID: 686
		internal const string Element_LongDescription = "LongDescription";

		// Token: 0x040002AF RID: 687
		internal const string Element_Member = "Member";

		// Token: 0x040002B0 RID: 688
		internal const string Element_NavigationProperty = "NavigationProperty";

		// Token: 0x040002B1 RID: 689
		internal const string Element_Null = "Null";

		// Token: 0x040002B2 RID: 690
		internal const string Element_OnDelete = "OnDelete";

		// Token: 0x040002B3 RID: 691
		internal const string Element_Parameter = "Parameter";

		// Token: 0x040002B4 RID: 692
		internal const string Element_ParameterReference = "ParameterReference";

		// Token: 0x040002B5 RID: 693
		internal const string Element_Path = "Path";

		// Token: 0x040002B6 RID: 694
		internal const string Element_Principal = "Principal";

		// Token: 0x040002B7 RID: 695
		internal const string Element_Property = "Property";

		// Token: 0x040002B8 RID: 696
		internal const string Element_PropertyRef = "PropertyRef";

		// Token: 0x040002B9 RID: 697
		internal const string Element_PropertyReference = "PropertyReference";

		// Token: 0x040002BA RID: 698
		internal const string Element_PropertyValue = "PropertyValue";

		// Token: 0x040002BB RID: 699
		internal const string Element_Record = "Record";

		// Token: 0x040002BC RID: 700
		internal const string Element_ReferenceType = "ReferenceType";

		// Token: 0x040002BD RID: 701
		internal const string Element_ReferentialConstraint = "ReferentialConstraint";

		// Token: 0x040002BE RID: 702
		internal const string Element_ReturnType = "ReturnType";

		// Token: 0x040002BF RID: 703
		internal const string Element_RowType = "RowType";

		// Token: 0x040002C0 RID: 704
		internal const string Element_Schema = "Schema";

		// Token: 0x040002C1 RID: 705
		internal const string Element_String = "String";

		// Token: 0x040002C2 RID: 706
		internal const string Element_Summary = "Summary";

		// Token: 0x040002C3 RID: 707
		internal const string Element_Time = "Time";

		// Token: 0x040002C4 RID: 708
		internal const string Element_TypeAnnotation = "TypeAnnotation";

		// Token: 0x040002C5 RID: 709
		internal const string Element_TypeRef = "TypeRef";

		// Token: 0x040002C6 RID: 710
		internal const string Element_Using = "Using";

		// Token: 0x040002C7 RID: 711
		internal const string Element_ValueAnnotation = "ValueAnnotation";

		// Token: 0x040002C8 RID: 712
		internal const string Element_ValueTerm = "ValueTerm";

		// Token: 0x040002C9 RID: 713
		internal const string Property_ElementType = "ElementType";

		// Token: 0x040002CA RID: 714
		internal const string Property_TargetSet = "TargetSet";

		// Token: 0x040002CB RID: 715
		internal const string Property_SourceSet = "SourceSet";

		// Token: 0x040002CC RID: 716
		internal const string Value_Bag = "Bag";

		// Token: 0x040002CD RID: 717
		internal const string Value_Cascade = "Cascade";

		// Token: 0x040002CE RID: 718
		internal const string Value_Collection = "Collection";

		// Token: 0x040002CF RID: 719
		internal const string Value_Computed = "Computed";

		// Token: 0x040002D0 RID: 720
		internal const string Value_EndMany = "*";

		// Token: 0x040002D1 RID: 721
		internal const string Value_EndOptional = "0..1";

		// Token: 0x040002D2 RID: 722
		internal const string Value_EndRequired = "1";

		// Token: 0x040002D3 RID: 723
		internal const string Value_False = "false";

		// Token: 0x040002D4 RID: 724
		internal const string Value_Fixed = "Fixed";

		// Token: 0x040002D5 RID: 725
		internal const string Value_Identity = "Identity";

		// Token: 0x040002D6 RID: 726
		internal const string Value_ModeIn = "In";

		// Token: 0x040002D7 RID: 727
		internal const string Value_ModeOut = "Out";

		// Token: 0x040002D8 RID: 728
		internal const string Value_ModeInOut = "InOut";

		// Token: 0x040002D9 RID: 729
		internal const string Value_List = "List";

		// Token: 0x040002DA RID: 730
		internal const string Value_Max = "Max";

		// Token: 0x040002DB RID: 731
		internal const string Value_None = "None";

		// Token: 0x040002DC RID: 732
		internal const string Value_Ref = "Ref";

		// Token: 0x040002DD RID: 733
		internal const string Value_Self = "Self";

		// Token: 0x040002DE RID: 734
		internal const string Value_True = "true";

		// Token: 0x040002DF RID: 735
		internal const string Value_UnknownNamespace = "[UnknownNamespace]";

		// Token: 0x040002E0 RID: 736
		internal const string Value_SridVariable = "Variable";

		// Token: 0x040002E1 RID: 737
		internal const bool Default_Abstract = false;

		// Token: 0x040002E2 RID: 738
		internal const bool Default_CollectionNullable = false;

		// Token: 0x040002E3 RID: 739
		internal const EdmConcurrencyMode Default_ConcurrencyMode = EdmConcurrencyMode.None;

		// Token: 0x040002E4 RID: 740
		internal const bool Default_ContainsTarget = false;

		// Token: 0x040002E5 RID: 741
		internal const EdmFunctionParameterMode Default_FunctionParameterMode = EdmFunctionParameterMode.In;

		// Token: 0x040002E6 RID: 742
		internal const bool Default_IsAtomic = false;

		// Token: 0x040002E7 RID: 743
		internal const bool Default_IsBindable = false;

		// Token: 0x040002E8 RID: 744
		internal const bool Default_IsComposable = false;

		// Token: 0x040002E9 RID: 745
		internal const bool Default_IsFlags = false;

		// Token: 0x040002EA RID: 746
		internal const bool Default_IsSideEffecting = true;

		// Token: 0x040002EB RID: 747
		internal const bool Default_OpenType = false;

		// Token: 0x040002EC RID: 748
		internal const bool Default_Nullable = true;

		// Token: 0x040002ED RID: 749
		internal const int Default_SpatialGeographySrid = 4326;

		// Token: 0x040002EE RID: 750
		internal const int Default_SpatialGeometrySrid = 0;

		// Token: 0x040002EF RID: 751
		internal const int Max_NameLength = 480;

		// Token: 0x040002F0 RID: 752
		internal const int Max_NamespaceLength = 512;

		// Token: 0x040002F1 RID: 753
		internal const string Version3Xsd = "Edm.Csdl.CSDLSchema_3.xsd";

		// Token: 0x040002F2 RID: 754
		internal const string EdmxFileExtension = ".edmx";

		// Token: 0x040002F3 RID: 755
		internal const string EdmxVersion1Namespace = "http://schemas.microsoft.com/ado/2007/06/edmx";

		// Token: 0x040002F4 RID: 756
		internal const string EdmxVersion2Namespace = "http://schemas.microsoft.com/ado/2008/10/edmx";

		// Token: 0x040002F5 RID: 757
		internal const string EdmxVersion3Namespace = "http://schemas.microsoft.com/ado/2009/11/edmx";

		// Token: 0x040002F6 RID: 758
		internal const string ODataMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

		// Token: 0x040002F7 RID: 759
		internal const string EdmxVersionAnnotation = "EdmxVersion";

		// Token: 0x040002F8 RID: 760
		internal const string Prefix_Edmx = "edmx";

		// Token: 0x040002F9 RID: 761
		internal const string Prefix_ODataMetadata = "m";

		// Token: 0x040002FA RID: 762
		internal const string Attribute_Version = "Version";

		// Token: 0x040002FB RID: 763
		internal const string Attribute_DataServiceVersion = "DataServiceVersion";

		// Token: 0x040002FC RID: 764
		internal const string Attribute_MaxDataServiceVersion = "MaxDataServiceVersion";

		// Token: 0x040002FD RID: 765
		internal const string Element_ConceptualModels = "ConceptualModels";

		// Token: 0x040002FE RID: 766
		internal const string Element_Edmx = "Edmx";

		// Token: 0x040002FF RID: 767
		internal const string Element_Runtime = "Runtime";

		// Token: 0x04000300 RID: 768
		internal const string Element_DataServices = "DataServices";

		// Token: 0x04000301 RID: 769
		public static readonly Version EdmxVersion1 = EdmConstants.EdmVersion1;

		// Token: 0x04000302 RID: 770
		public static readonly Version EdmxVersion2 = EdmConstants.EdmVersion2;

		// Token: 0x04000303 RID: 771
		public static readonly Version EdmxVersion3 = EdmConstants.EdmVersion3;

		// Token: 0x04000304 RID: 772
		public static readonly Version EdmxVersionLatest = CsdlConstants.EdmxVersion3;

		// Token: 0x04000305 RID: 773
		internal static Dictionary<Version, string[]> SupportedVersions = new Dictionary<Version, string[]>
		{
			{
				EdmConstants.EdmVersion1,
				new string[]
				{
					"http://schemas.microsoft.com/ado/2006/04/edm"
				}
			},
			{
				EdmConstants.EdmVersion1_1,
				new string[]
				{
					"http://schemas.microsoft.com/ado/2007/05/edm"
				}
			},
			{
				EdmConstants.EdmVersion1_2,
				new string[]
				{
					"http://schemas.microsoft.com/ado/2008/01/edm"
				}
			},
			{
				EdmConstants.EdmVersion2,
				new string[]
				{
					"http://schemas.microsoft.com/ado/2008/09/edm",
					"http://schemas.microsoft.com/ado/2009/08/edm"
				}
			},
			{
				EdmConstants.EdmVersion3,
				new string[]
				{
					"http://schemas.microsoft.com/ado/2009/11/edm"
				}
			}
		};

		// Token: 0x04000306 RID: 774
		internal static Dictionary<Version, string> SupportedEdmxVersions = new Dictionary<Version, string>
		{
			{
				CsdlConstants.EdmxVersion1,
				"http://schemas.microsoft.com/ado/2007/06/edmx"
			},
			{
				CsdlConstants.EdmxVersion2,
				"http://schemas.microsoft.com/ado/2008/10/edmx"
			},
			{
				CsdlConstants.EdmxVersion3,
				"http://schemas.microsoft.com/ado/2009/11/edmx"
			}
		};

		// Token: 0x04000307 RID: 775
		internal static Dictionary<string, Version> SupportedEdmxNamespaces = CsdlConstants.SupportedEdmxVersions.ToDictionary((KeyValuePair<Version, string> v) => v.Value, (KeyValuePair<Version, string> v) => v.Key);

		// Token: 0x04000308 RID: 776
		internal static Dictionary<Version, Version> EdmToEdmxVersions = new Dictionary<Version, Version>
		{
			{
				EdmConstants.EdmVersion1,
				CsdlConstants.EdmxVersion1
			},
			{
				EdmConstants.EdmVersion1_1,
				CsdlConstants.EdmxVersion1
			},
			{
				EdmConstants.EdmVersion1_2,
				CsdlConstants.EdmxVersion1
			},
			{
				EdmConstants.EdmVersion2,
				CsdlConstants.EdmxVersion2
			},
			{
				EdmConstants.EdmVersion3,
				CsdlConstants.EdmxVersion3
			}
		};
	}
}
