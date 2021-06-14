using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Defines a 5 x 5 matrix that contains the coordinates for the RGBAW space. Several methods of the <see cref="T:System.Drawing.Imaging.ImageAttributes" /> class adjust image colors by using a color matrix. This class cannot be inherited.</summary>
	// Token: 0x02000091 RID: 145
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ColorMatrix
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ColorMatrix" /> class.</summary>
		// Token: 0x060008E9 RID: 2281 RVA: 0x00022685 File Offset: 0x00020885
		public ColorMatrix()
		{
			this.matrix00 = 1f;
			this.matrix11 = 1f;
			this.matrix22 = 1f;
			this.matrix33 = 1f;
			this.matrix44 = 1f;
		}

		/// <summary>Gets or sets the element at the 0 (zero) row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the 0 row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x000226C4 File Offset: 0x000208C4
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x000226CC File Offset: 0x000208CC
		public float Matrix00
		{
			get
			{
				return this.matrix00;
			}
			set
			{
				this.matrix00 = value;
			}
		}

		/// <summary>Gets or sets the element at the 0 (zero) row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the 0 row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" /> .</returns>
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000226D5 File Offset: 0x000208D5
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x000226DD File Offset: 0x000208DD
		public float Matrix01
		{
			get
			{
				return this.matrix01;
			}
			set
			{
				this.matrix01 = value;
			}
		}

		/// <summary>Gets or sets the element at the 0 (zero) row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the 0 row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x000226E6 File Offset: 0x000208E6
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x000226EE File Offset: 0x000208EE
		public float Matrix02
		{
			get
			{
				return this.matrix02;
			}
			set
			{
				this.matrix02 = value;
			}
		}

		/// <summary>Gets or sets the element at the 0 (zero) row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />. Represents the alpha component.</summary>
		/// <returns>The element at the 0 row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x000226F7 File Offset: 0x000208F7
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x000226FF File Offset: 0x000208FF
		public float Matrix03
		{
			get
			{
				return this.matrix03;
			}
			set
			{
				this.matrix03 = value;
			}
		}

		/// <summary>Gets or sets the element at the 0 (zero) row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the 0 row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00022708 File Offset: 0x00020908
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00022710 File Offset: 0x00020910
		public float Matrix04
		{
			get
			{
				return this.matrix04;
			}
			set
			{
				this.matrix04 = value;
			}
		}

		/// <summary>Gets or sets the element at the first row and 0 (zero) column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the first row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00022719 File Offset: 0x00020919
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00022721 File Offset: 0x00020921
		public float Matrix10
		{
			get
			{
				return this.matrix10;
			}
			set
			{
				this.matrix10 = value;
			}
		}

		/// <summary>Gets or sets the element at the first row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the first row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0002272A File Offset: 0x0002092A
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00022732 File Offset: 0x00020932
		public float Matrix11
		{
			get
			{
				return this.matrix11;
			}
			set
			{
				this.matrix11 = value;
			}
		}

		/// <summary>Gets or sets the element at the first row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the first row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0002273B File Offset: 0x0002093B
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x00022743 File Offset: 0x00020943
		public float Matrix12
		{
			get
			{
				return this.matrix12;
			}
			set
			{
				this.matrix12 = value;
			}
		}

		/// <summary>Gets or sets the element at the first row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />. Represents the alpha component.</summary>
		/// <returns>The element at the first row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0002274C File Offset: 0x0002094C
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00022754 File Offset: 0x00020954
		public float Matrix13
		{
			get
			{
				return this.matrix13;
			}
			set
			{
				this.matrix13 = value;
			}
		}

		/// <summary>Gets or sets the element at the first row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the first row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0002275D File Offset: 0x0002095D
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00022765 File Offset: 0x00020965
		public float Matrix14
		{
			get
			{
				return this.matrix14;
			}
			set
			{
				this.matrix14 = value;
			}
		}

		/// <summary>Gets or sets the element at the second row and 0 (zero) column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the second row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0002276E File Offset: 0x0002096E
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00022776 File Offset: 0x00020976
		public float Matrix20
		{
			get
			{
				return this.matrix20;
			}
			set
			{
				this.matrix20 = value;
			}
		}

		/// <summary>Gets or sets the element at the second row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the second row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0002277F File Offset: 0x0002097F
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00022787 File Offset: 0x00020987
		public float Matrix21
		{
			get
			{
				return this.matrix21;
			}
			set
			{
				this.matrix21 = value;
			}
		}

		/// <summary>Gets or sets the element at the second row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the second row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00022790 File Offset: 0x00020990
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00022798 File Offset: 0x00020998
		public float Matrix22
		{
			get
			{
				return this.matrix22;
			}
			set
			{
				this.matrix22 = value;
			}
		}

		/// <summary>Gets or sets the element at the second row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the second row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x000227A1 File Offset: 0x000209A1
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x000227A9 File Offset: 0x000209A9
		public float Matrix23
		{
			get
			{
				return this.matrix23;
			}
			set
			{
				this.matrix23 = value;
			}
		}

		/// <summary>Gets or sets the element at the second row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the second row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000227B2 File Offset: 0x000209B2
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x000227BA File Offset: 0x000209BA
		public float Matrix24
		{
			get
			{
				return this.matrix24;
			}
			set
			{
				this.matrix24 = value;
			}
		}

		/// <summary>Gets or sets the element at the third row and 0 (zero) column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the third row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x000227C3 File Offset: 0x000209C3
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x000227CB File Offset: 0x000209CB
		public float Matrix30
		{
			get
			{
				return this.matrix30;
			}
			set
			{
				this.matrix30 = value;
			}
		}

		/// <summary>Gets or sets the element at the third row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the third row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x000227D4 File Offset: 0x000209D4
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x000227DC File Offset: 0x000209DC
		public float Matrix31
		{
			get
			{
				return this.matrix31;
			}
			set
			{
				this.matrix31 = value;
			}
		}

		/// <summary>Gets or sets the element at the third row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the third row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x000227E5 File Offset: 0x000209E5
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x000227ED File Offset: 0x000209ED
		public float Matrix32
		{
			get
			{
				return this.matrix32;
			}
			set
			{
				this.matrix32 = value;
			}
		}

		/// <summary>Gets or sets the element at the third row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />. Represents the alpha component.</summary>
		/// <returns>The element at the third row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x000227F6 File Offset: 0x000209F6
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x000227FE File Offset: 0x000209FE
		public float Matrix33
		{
			get
			{
				return this.matrix33;
			}
			set
			{
				this.matrix33 = value;
			}
		}

		/// <summary>Gets or sets the element at the third row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the third row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00022807 File Offset: 0x00020A07
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0002280F File Offset: 0x00020A0F
		public float Matrix34
		{
			get
			{
				return this.matrix34;
			}
			set
			{
				this.matrix34 = value;
			}
		}

		/// <summary>Gets or sets the element at the fourth row and 0 (zero) column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the fourth row and 0 column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00022818 File Offset: 0x00020A18
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00022820 File Offset: 0x00020A20
		public float Matrix40
		{
			get
			{
				return this.matrix40;
			}
			set
			{
				this.matrix40 = value;
			}
		}

		/// <summary>Gets or sets the element at the fourth row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the fourth row and first column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00022829 File Offset: 0x00020A29
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00022831 File Offset: 0x00020A31
		public float Matrix41
		{
			get
			{
				return this.matrix41;
			}
			set
			{
				this.matrix41 = value;
			}
		}

		/// <summary>Gets or sets the element at the fourth row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the fourth row and second column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0002283A File Offset: 0x00020A3A
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00022842 File Offset: 0x00020A42
		public float Matrix42
		{
			get
			{
				return this.matrix42;
			}
			set
			{
				this.matrix42 = value;
			}
		}

		/// <summary>Gets or sets the element at the fourth row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />. Represents the alpha component.</summary>
		/// <returns>The element at the fourth row and third column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0002284B File Offset: 0x00020A4B
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00022853 File Offset: 0x00020A53
		public float Matrix43
		{
			get
			{
				return this.matrix43;
			}
			set
			{
				this.matrix43 = value;
			}
		}

		/// <summary>Gets or sets the element at the fourth row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <returns>The element at the fourth row and fourth column of this <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</returns>
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002285C File Offset: 0x00020A5C
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00022864 File Offset: 0x00020A64
		public float Matrix44
		{
			get
			{
				return this.matrix44;
			}
			set
			{
				this.matrix44 = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ColorMatrix" /> class using the elements in the specified matrix <paramref name="newColorMatrix" />.</summary>
		/// <param name="newColorMatrix">The values of the elements for the new <see cref="T:System.Drawing.Imaging.ColorMatrix" />. </param>
		// Token: 0x0600091C RID: 2332 RVA: 0x0002286D File Offset: 0x00020A6D
		[CLSCompliant(false)]
		public ColorMatrix(float[][] newColorMatrix)
		{
			this.SetMatrix(newColorMatrix);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002287C File Offset: 0x00020A7C
		internal void SetMatrix(float[][] newColorMatrix)
		{
			this.matrix00 = newColorMatrix[0][0];
			this.matrix01 = newColorMatrix[0][1];
			this.matrix02 = newColorMatrix[0][2];
			this.matrix03 = newColorMatrix[0][3];
			this.matrix04 = newColorMatrix[0][4];
			this.matrix10 = newColorMatrix[1][0];
			this.matrix11 = newColorMatrix[1][1];
			this.matrix12 = newColorMatrix[1][2];
			this.matrix13 = newColorMatrix[1][3];
			this.matrix14 = newColorMatrix[1][4];
			this.matrix20 = newColorMatrix[2][0];
			this.matrix21 = newColorMatrix[2][1];
			this.matrix22 = newColorMatrix[2][2];
			this.matrix23 = newColorMatrix[2][3];
			this.matrix24 = newColorMatrix[2][4];
			this.matrix30 = newColorMatrix[3][0];
			this.matrix31 = newColorMatrix[3][1];
			this.matrix32 = newColorMatrix[3][2];
			this.matrix33 = newColorMatrix[3][3];
			this.matrix34 = newColorMatrix[3][4];
			this.matrix40 = newColorMatrix[4][0];
			this.matrix41 = newColorMatrix[4][1];
			this.matrix42 = newColorMatrix[4][2];
			this.matrix43 = newColorMatrix[4][3];
			this.matrix44 = newColorMatrix[4][4];
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002299C File Offset: 0x00020B9C
		internal float[][] GetMatrix()
		{
			float[][] array = new float[5][];
			for (int i = 0; i < 5; i++)
			{
				array[i] = new float[5];
			}
			array[0][0] = this.matrix00;
			array[0][1] = this.matrix01;
			array[0][2] = this.matrix02;
			array[0][3] = this.matrix03;
			array[0][4] = this.matrix04;
			array[1][0] = this.matrix10;
			array[1][1] = this.matrix11;
			array[1][2] = this.matrix12;
			array[1][3] = this.matrix13;
			array[1][4] = this.matrix14;
			array[2][0] = this.matrix20;
			array[2][1] = this.matrix21;
			array[2][2] = this.matrix22;
			array[2][3] = this.matrix23;
			array[2][4] = this.matrix24;
			array[3][0] = this.matrix30;
			array[3][1] = this.matrix31;
			array[3][2] = this.matrix32;
			array[3][3] = this.matrix33;
			array[3][4] = this.matrix34;
			array[4][0] = this.matrix40;
			array[4][1] = this.matrix41;
			array[4][2] = this.matrix42;
			array[4][3] = this.matrix43;
			array[4][4] = this.matrix44;
			return array;
		}

		/// <summary>Gets or sets the element at the specified row and column in the <see cref="T:System.Drawing.Imaging.ColorMatrix" />.</summary>
		/// <param name="row">The row of the element.</param>
		/// <param name="column">The column of the element.</param>
		/// <returns>The element at the specified row and column.</returns>
		// Token: 0x1700035A RID: 858
		public float this[int row, int column]
		{
			get
			{
				return this.GetMatrix()[row][column];
			}
			set
			{
				float[][] matrix = this.GetMatrix();
				matrix[row][column] = value;
				this.SetMatrix(matrix);
			}
		}

		// Token: 0x0400074C RID: 1868
		private float matrix00;

		// Token: 0x0400074D RID: 1869
		private float matrix01;

		// Token: 0x0400074E RID: 1870
		private float matrix02;

		// Token: 0x0400074F RID: 1871
		private float matrix03;

		// Token: 0x04000750 RID: 1872
		private float matrix04;

		// Token: 0x04000751 RID: 1873
		private float matrix10;

		// Token: 0x04000752 RID: 1874
		private float matrix11;

		// Token: 0x04000753 RID: 1875
		private float matrix12;

		// Token: 0x04000754 RID: 1876
		private float matrix13;

		// Token: 0x04000755 RID: 1877
		private float matrix14;

		// Token: 0x04000756 RID: 1878
		private float matrix20;

		// Token: 0x04000757 RID: 1879
		private float matrix21;

		// Token: 0x04000758 RID: 1880
		private float matrix22;

		// Token: 0x04000759 RID: 1881
		private float matrix23;

		// Token: 0x0400075A RID: 1882
		private float matrix24;

		// Token: 0x0400075B RID: 1883
		private float matrix30;

		// Token: 0x0400075C RID: 1884
		private float matrix31;

		// Token: 0x0400075D RID: 1885
		private float matrix32;

		// Token: 0x0400075E RID: 1886
		private float matrix33;

		// Token: 0x0400075F RID: 1887
		private float matrix34;

		// Token: 0x04000760 RID: 1888
		private float matrix40;

		// Token: 0x04000761 RID: 1889
		private float matrix41;

		// Token: 0x04000762 RID: 1890
		private float matrix42;

		// Token: 0x04000763 RID: 1891
		private float matrix43;

		// Token: 0x04000764 RID: 1892
		private float matrix44;
	}
}
