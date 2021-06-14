using System;

namespace ComponentAce.Encryption
{
	// Token: 0x0200000E RID: 14
	internal class DecConst
	{
		// Token: 0x0400004D RID: 77
		public const string sProtectionCircular = "Circular Protection detected, Protection Object is invalid.";

		// Token: 0x0400004E RID: 78
		public const string sStringFormatExists = "String Format \"%d\" not exists.";

		// Token: 0x0400004F RID: 79
		public const string sInvalidStringFormat = "Input is not an valid %s Format.";

		// Token: 0x04000050 RID: 80
		public const string sInvalidFormatString = "Input can not be convert to %s Format.";

		// Token: 0x04000051 RID: 81
		public const string sFMT_COPY = "copy Input to Output";

		// Token: 0x04000052 RID: 82
		public const string sFMT_HEX = "Hexadecimal";

		// Token: 0x04000053 RID: 83
		public const string sFMT_HEXL = "Hexadecimal lowercase";

		// Token: 0x04000054 RID: 84
		public const string sFMT_MIME64 = "MIME Base 64";

		// Token: 0x04000055 RID: 85
		public const string sFMT_UU = "UU Coding";

		// Token: 0x04000056 RID: 86
		public const string sFMT_XX = "XX Coding";

		// Token: 0x04000057 RID: 87
		public const string sInvalidKey = "Encryptionkey is invalid";

		// Token: 0x04000058 RID: 88
		public const string sInvalidCRC = "Encrypted Data is corrupt, invalid Checksum";

		// Token: 0x04000059 RID: 89
		public const string sInvalidKeySize = "Length from Encryptionkey is invalid.\r\nKeysize for %s must be to %d-%d bytes";

		// Token: 0x0400005A RID: 90
		public const string sNotInitialized = "%s is not initialized call Init() or InitKey() before.";

		// Token: 0x0400005B RID: 91
		public const string sInvalidMACMode = "Invalid Ciphermode selected to produce a MAC.\r\nPlease use Modes CBC, CTS, CFB, CBCMAC, CFBMAC or CTSMAC for CalcMAC.";

		// Token: 0x0400005C RID: 92
		public const string sCantCalc = "Invalid Ciphermode selected.";

		// Token: 0x0400005D RID: 93
		public const string sInvalidRandomStream = "Invalid Random Data detected.";

		// Token: 0x0400005E RID: 94
		public const string sRandomDataProtected = "Random Data are protected.";

		// Token: 0x0400005F RID: 95
		public const string sBBSnotSeekable = "BBS Generator is not seekable.";

		// Token: 0x04000060 RID: 96
		public const string sBigNumDestroy = "Used Bignums in a BBS Generator can be not destroy.";

		// Token: 0x04000061 RID: 97
		public const string sIndexOutOfRange = "BBS Error: Index out of Range.";

		// Token: 0x04000062 RID: 98
		public const string sBigNumAborted = "BigNum aborted by User.";

		// Token: 0x04000063 RID: 99
		public const string sErrGeneric = "Bignum Generic Error.";

		// Token: 0x04000064 RID: 100
		public const string sErrAsInteger = "BigNum overflow in AsInteger.";

		// Token: 0x04000065 RID: 101
		public const string sErrAsComp = "BigNum overflow in AsComp.";

		// Token: 0x04000066 RID: 102
		public const string sErrAsFloat = "BigNum overflow in AsFloat.";

		// Token: 0x04000067 RID: 103
		public const string sNumberFormat = "BigNum invalid Numberformat for Base %d.\r\nValue: %s";

		// Token: 0x04000068 RID: 104
		public const string sDivByZero = "BigNum division by Zero.";

		// Token: 0x04000069 RID: 105
		public const string sStackIndex = "BigNum Stackindex out of range.";

		// Token: 0x0400006A RID: 106
		public const string sLoadFail = "BigNum invalid data format.";

		// Token: 0x0400006B RID: 107
		public const string sParams = "BigNum parameter error.\r\n%s.";

		// Token: 0x0400006C RID: 108
		public const string sJacobi = "BigNum Jacobi(A, B), B must be >= 3, Odd and B < A";

		// Token: 0x0400006D RID: 109
		public const string sSPPrime = "BigNum IsSPPrime(A, Base), |Base| must be > 1, |A| > |Base| and |A| >= 3";

		// Token: 0x0400006E RID: 110
		public const string sSetPrime = "BigNum SetPrime(Base, Residue, Modulus), Invalid Parameter.\r\n%s.";

		// Token: 0x0400006F RID: 111
		public const string sSetPrimeSize = "Value must be greater 32767";

		// Token: 0x04000070 RID: 112
		public const string sSetPrimeParam = "GCD(Residue, Modulus) must be 1 and Residue < Modulus";

		// Token: 0x04000071 RID: 113
		public const string sSqrt = "BigNum Sqrt(A) A must be position";

		// Token: 0x04000072 RID: 114
		public const string sExpMod = "BigNum ExpMod(E, M) M must be positive";

		// Token: 0x04000073 RID: 115
		public const string sCalcName = "Calculation No %d";

		// Token: 0x04000074 RID: 116
		public const string sInvalidState = "Invalid Protector State detected.";

		// Token: 0x04000075 RID: 117
		public const string sIDOutOfRange = "Protector Error: ID is out of Range.";

		// Token: 0x04000076 RID: 118
		public const string sInvalidZIPData = "Invalid compressed Data detected.";

		// Token: 0x04000077 RID: 119
		public const string sInvalidChallenge = "Challenge is not an RFC2289 Format.";

		// Token: 0x04000078 RID: 120
		public const string sInvalidPassword = "Invalid Passphraselength, must be more than 9 Chars.";

		// Token: 0x04000079 RID: 121
		public const string sInvalidSeed = "Invalid Seed Value in OTPCalc.";

		// Token: 0x0400007A RID: 122
		public const string sInvalidCalc = "Invalid Parameters in OTPCalc.";

		// Token: 0x0400007B RID: 123
		public const string sInvalidDictionary = "Used Dictionary in %s is invalid.";

		// Token: 0x0400007C RID: 124
		public const string sOTPIdent = "otp-";

		// Token: 0x0400007D RID: 125
		public const string sOTPExt = "ext";

		// Token: 0x0400007E RID: 126
		public const string sOTPWord = "word:";

		// Token: 0x0400007F RID: 127
		public const string sOTPHex = "hex:";

		// Token: 0x04000080 RID: 128
		public const string sSKeyIdent = "s/key";

		// Token: 0x04000081 RID: 129
		public const int maxBufSize = 4096;
	}
}
