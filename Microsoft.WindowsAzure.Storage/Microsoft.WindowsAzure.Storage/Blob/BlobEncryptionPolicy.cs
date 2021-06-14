using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Azure.KeyVault.Core;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Newtonsoft.Json;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000010 RID: 16
	public sealed class BlobEncryptionPolicy
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005CAB File Offset: 0x00003EAB
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00005CB3 File Offset: 0x00003EB3
		internal BlobEncryptionMode EncryptionMode { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005CBC File Offset: 0x00003EBC
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public IKey Key { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005CCD File Offset: 0x00003ECD
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00005CD5 File Offset: 0x00003ED5
		public IKeyResolver KeyResolver { get; private set; }

		// Token: 0x0600013F RID: 319 RVA: 0x00005CDE File Offset: 0x00003EDE
		public BlobEncryptionPolicy(IKey key, IKeyResolver keyResolver)
		{
			this.Key = key;
			this.KeyResolver = keyResolver;
			this.EncryptionMode = BlobEncryptionMode.FullBlob;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005CFC File Offset: 0x00003EFC
		internal Stream DecryptBlob(Stream userProvidedStream, IDictionary<string, string> metadata, out ICryptoTransform transform, bool? requireEncryption, byte[] iv = null, bool noPadding = false)
		{
			CommonUtility.AssertNotNull("metadata", metadata);
			string text = null;
			bool flag = metadata.TryGetValue("encryptiondata", out text);
			if (requireEncryption != null && requireEncryption.Value && !flag)
			{
				throw new StorageException("Encryption data does not exist. If you do not want to decrypt the data, please do not set the RequireEncryption flag on request options.", null)
				{
					IsRetryable = false
				};
			}
			Stream result3;
			try
			{
				if (text != null)
				{
					BlobEncryptionData blobEncryptionData = JsonConvert.DeserializeObject<BlobEncryptionData>(text);
					CommonUtility.AssertNotNull("ContentEncryptionIV", blobEncryptionData.ContentEncryptionIV);
					CommonUtility.AssertNotNull("EncryptedKey", blobEncryptionData.WrappedContentKey.EncryptedKey);
					if (blobEncryptionData.EncryptionAgent.Protocol != "1.0")
					{
						throw new StorageException("Invalid Encryption Agent. This version of the client library does not understand the Encryption Agent set on the blob.", null)
						{
							IsRetryable = false
						};
					}
					if (this.Key == null && this.KeyResolver == null)
					{
						throw new StorageException("Key and Resolver are not initialized. Decryption requires either of them to be initialized.", null)
						{
							IsRetryable = false
						};
					}
					byte[] result2;
					if (this.KeyResolver != null)
					{
						IKey result = this.KeyResolver.ResolveKeyAsync(blobEncryptionData.WrappedContentKey.KeyId, CancellationToken.None).Result;
						CommonUtility.AssertNotNull("KeyEncryptionKey", result);
						result2 = result.UnwrapKeyAsync(blobEncryptionData.WrappedContentKey.EncryptedKey, blobEncryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
					}
					else
					{
						if (!(this.Key.Kid == blobEncryptionData.WrappedContentKey.KeyId))
						{
							throw new StorageException("Key mismatch. The key id stored on the service does not match the specified key.", null)
							{
								IsRetryable = false
							};
						}
						result2 = this.Key.UnwrapKeyAsync(blobEncryptionData.WrappedContentKey.EncryptedKey, blobEncryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
					}
					EncryptionAlgorithm encryptionAlgorithm = blobEncryptionData.EncryptionAgent.EncryptionAlgorithm;
					if (encryptionAlgorithm == EncryptionAlgorithm.AES_CBC_256)
					{
						using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
						{
							aesCryptoServiceProvider.IV = ((iv != null) ? iv : blobEncryptionData.ContentEncryptionIV);
							aesCryptoServiceProvider.Key = result2;
							if (noPadding)
							{
								aesCryptoServiceProvider.Padding = PaddingMode.None;
							}
							transform = aesCryptoServiceProvider.CreateDecryptor();
							return new CryptoStream(userProvidedStream, transform, CryptoStreamMode.Write);
						}
					}
					throw new StorageException("Invalid Encryption Algorithm found on the resource. This version of the client library does not support the specified encryption algorithm.", null)
					{
						IsRetryable = false
					};
				}
				else
				{
					transform = null;
					result3 = userProvidedStream;
				}
			}
			catch (JsonException innerException)
			{
				throw new StorageException("Error while de-serializing the encryption metadata string from the wire.", innerException)
				{
					IsRetryable = false
				};
			}
			catch (StorageException)
			{
				throw;
			}
			catch (Exception innerException2)
			{
				throw new StorageException("Decryption logic threw error. Please check the inner exception for more details.", innerException2)
				{
					IsRetryable = false
				};
			}
			return result3;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005FC4 File Offset: 0x000041C4
		internal static Stream WrapUserStreamWithDecryptStream(CloudBlob blob, Stream userProvidedStream, BlobRequestOptions options, BlobAttributes attributes, bool rangeRead, out ICryptoTransform transform, long? endOffset = null, long? userSpecifiedLength = null, int discardFirst = 0, bool bufferIV = false)
		{
			if (!rangeRead)
			{
				return options.EncryptionPolicy.DecryptBlob(new NonCloseableStream(userProvidedStream), attributes.Metadata, out transform, options.RequireEncryption, null, blob.BlobType == BlobType.PageBlob);
			}
			bool noPadding = blob.BlobType == BlobType.PageBlob || (endOffset != null && endOffset.Value < attributes.Properties.Length - 16L);
			transform = null;
			return new BlobDecryptStream(userProvidedStream, attributes.Metadata, userSpecifiedLength, discardFirst, bufferIV, noPadding, options.EncryptionPolicy, options.RequireEncryption);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006058 File Offset: 0x00004258
		internal ICryptoTransform CreateAndSetEncryptionContext(IDictionary<string, string> metadata, bool noPadding)
		{
			CommonUtility.AssertNotNull("metadata", metadata);
			if (this.Key == null)
			{
				throw new InvalidOperationException("Key is not initialized. Encryption requires it to be initialized.", null);
			}
			ICryptoTransform result2;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				if (noPadding)
				{
					aesCryptoServiceProvider.Padding = PaddingMode.None;
				}
				BlobEncryptionData blobEncryptionData = new BlobEncryptionData();
				blobEncryptionData.EncryptionAgent = new EncryptionAgent("1.0", EncryptionAlgorithm.AES_CBC_256);
				Tuple<byte[], string> result = this.Key.WrapKeyAsync(aesCryptoServiceProvider.Key, null, CancellationToken.None).Result;
				blobEncryptionData.WrappedContentKey = new WrappedKey(this.Key.Kid, result.Item1, result.Item2);
				blobEncryptionData.EncryptionMode = this.EncryptionMode.ToString();
				blobEncryptionData.KeyWrappingMetadata = new Dictionary<string, string>();
				blobEncryptionData.ContentEncryptionIV = aesCryptoServiceProvider.IV;
				metadata["encryptiondata"] = JsonConvert.SerializeObject(blobEncryptionData);
				result2 = aesCryptoServiceProvider.CreateEncryptor();
			}
			return result2;
		}
	}
}
