using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Azure.KeyVault.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Newtonsoft.Json;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x02000037 RID: 55
	public sealed class QueueEncryptionPolicy
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002618F File Offset: 0x0002438F
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00026197 File Offset: 0x00024397
		public IKey Key { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000261A0 File Offset: 0x000243A0
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x000261A8 File Offset: 0x000243A8
		public IKeyResolver KeyResolver { get; private set; }

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000261B1 File Offset: 0x000243B1
		public QueueEncryptionPolicy(IKey key, IKeyResolver keyResolver)
		{
			this.Key = key;
			this.KeyResolver = keyResolver;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000261C8 File Offset: 0x000243C8
		internal string EncryptMessage(byte[] inputMessage)
		{
			CommonUtility.AssertNotNull("inputMessage", inputMessage);
			if (this.Key == null)
			{
				throw new InvalidOperationException("Key is not initialized. Encryption requires it to be initialized.", null);
			}
			CloudQueueEncryptedMessage cloudQueueEncryptedMessage = new CloudQueueEncryptedMessage();
			EncryptionData encryptionData = new EncryptionData();
			encryptionData.EncryptionAgent = new EncryptionAgent("1.0", EncryptionAlgorithm.AES_CBC_256);
			encryptionData.KeyWrappingMetadata = new Dictionary<string, string>();
			string result2;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				encryptionData.ContentEncryptionIV = aesCryptoServiceProvider.IV;
				Tuple<byte[], string> result = this.Key.WrapKeyAsync(aesCryptoServiceProvider.Key, null, CancellationToken.None).Result;
				encryptionData.WrappedContentKey = new WrappedKey(this.Key.Kid, result.Item1, result.Item2);
				using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateEncryptor())
				{
					cloudQueueEncryptedMessage.EncryptedMessageContents = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(inputMessage, 0, inputMessage.Length));
				}
				cloudQueueEncryptedMessage.EncryptionData = encryptionData;
				result2 = JsonConvert.SerializeObject(cloudQueueEncryptedMessage);
			}
			return result2;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000262D4 File Offset: 0x000244D4
		internal byte[] DecryptMessage(string inputMessage, bool? requireEncryption)
		{
			CommonUtility.AssertNotNull("inputMessage", inputMessage);
			byte[] result3;
			try
			{
				CloudQueueEncryptedMessage cloudQueueEncryptedMessage = JsonConvert.DeserializeObject<CloudQueueEncryptedMessage>(inputMessage);
				if (requireEncryption != null && requireEncryption.Value && cloudQueueEncryptedMessage.EncryptionData == null)
				{
					throw new StorageException("Encryption data does not exist. If you do not want to decrypt the data, please do not set the RequireEncryption flag on request options.", null)
					{
						IsRetryable = false
					};
				}
				if (cloudQueueEncryptedMessage.EncryptionData != null)
				{
					EncryptionData encryptionData = cloudQueueEncryptedMessage.EncryptionData;
					CommonUtility.AssertNotNull("ContentEncryptionIV", encryptionData.ContentEncryptionIV);
					CommonUtility.AssertNotNull("EncryptedKey", encryptionData.WrappedContentKey.EncryptedKey);
					if (encryptionData.EncryptionAgent.Protocol != "1.0")
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
						IKey result = this.KeyResolver.ResolveKeyAsync(encryptionData.WrappedContentKey.KeyId, CancellationToken.None).Result;
						CommonUtility.AssertNotNull("keyEncryptionKey", result);
						result2 = result.UnwrapKeyAsync(encryptionData.WrappedContentKey.EncryptedKey, encryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
					}
					else
					{
						if (!(this.Key.Kid == encryptionData.WrappedContentKey.KeyId))
						{
							throw new StorageException("Key mismatch. The key id stored on the service does not match the specified key.", null)
							{
								IsRetryable = false
							};
						}
						result2 = this.Key.UnwrapKeyAsync(encryptionData.WrappedContentKey.EncryptedKey, encryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
					}
					EncryptionAlgorithm encryptionAlgorithm = encryptionData.EncryptionAgent.EncryptionAlgorithm;
					if (encryptionAlgorithm == EncryptionAlgorithm.AES_CBC_256)
					{
						using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
						{
							aesCryptoServiceProvider.Key = result2;
							aesCryptoServiceProvider.IV = encryptionData.ContentEncryptionIV;
							byte[] array = Convert.FromBase64String(cloudQueueEncryptedMessage.EncryptedMessageContents);
							using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor())
							{
								return cryptoTransform.TransformFinalBlock(array, 0, array.Length);
							}
						}
					}
					throw new StorageException("Invalid Encryption Algorithm found on the resource. This version of the client library does not support the specified encryption algorithm.", null)
					{
						IsRetryable = false
					};
				}
				else
				{
					result3 = Convert.FromBase64String(cloudQueueEncryptedMessage.EncryptedMessageContents);
				}
			}
			catch (JsonException innerException)
			{
				throw new StorageException("Error while de-serializing the encrypted queue message string from the wire. Please check inner exception for more details.", innerException)
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
	}
}
