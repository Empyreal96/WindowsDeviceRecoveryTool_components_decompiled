using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Azure.KeyVault.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Newtonsoft.Json;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200003E RID: 62
	public class TableEncryptionPolicy
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x000294A2 File Offset: 0x000276A2
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x000294AA File Offset: 0x000276AA
		public IKey Key { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x000294B3 File Offset: 0x000276B3
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x000294BB File Offset: 0x000276BB
		public IKeyResolver KeyResolver { get; private set; }

		// Token: 0x06000BE4 RID: 3044 RVA: 0x000294C4 File Offset: 0x000276C4
		public TableEncryptionPolicy(IKey key, IKeyResolver keyResolver)
		{
			this.Key = key;
			this.KeyResolver = keyResolver;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000294DC File Offset: 0x000276DC
		internal Dictionary<string, EntityProperty> EncryptEntity(IDictionary<string, EntityProperty> properties, string partitionKey, string rowKey, Func<string, string, string, bool> encryptionResolver)
		{
			CommonUtility.AssertNotNull("properties", properties);
			if (this.Key == null)
			{
				throw new InvalidOperationException("Key is not initialized. Encryption requires it to be initialized.", null);
			}
			EncryptionData encryptionData = new EncryptionData();
			encryptionData.EncryptionAgent = new EncryptionAgent("1.0", EncryptionAlgorithm.AES_CBC_256);
			encryptionData.KeyWrappingMetadata = new Dictionary<string, string>();
			Dictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();
			HashSet<string> hashSet = new HashSet<string>();
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
				{
					encryptionData.ContentEncryptionIV = aesCryptoServiceProvider.IV;
					Tuple<byte[], string> result = this.Key.WrapKeyAsync(aesCryptoServiceProvider.Key, null, CancellationToken.None).Result;
					encryptionData.WrappedContentKey = new WrappedKey(this.Key.Kid, result.Item1, result.Item2);
					foreach (KeyValuePair<string, EntityProperty> keyValuePair in properties)
					{
						if (encryptionResolver != null && encryptionResolver(partitionKey, rowKey, keyValuePair.Key))
						{
							if (keyValuePair.Value == null)
							{
								throw new InvalidOperationException("Null properties cannot be encrypted. Please assign a default value to the property if you wish to encrypt it.");
							}
							keyValuePair.Value.IsEncrypted = true;
						}
						if (keyValuePair.Value != null && keyValuePair.Value.IsEncrypted)
						{
							if (keyValuePair.Value.PropertyType != EdmType.String)
							{
								throw new InvalidOperationException(string.Format("Unsupported type : {0} encountered during encryption. Only string properties can be encrypted on the client side.", keyValuePair.Value.PropertyType));
							}
							byte[] iv = sha256CryptoServiceProvider.ComputeHash(CommonUtility.BinaryAppend(encryptionData.ContentEncryptionIV, Encoding.UTF8.GetBytes(string.Join(partitionKey, new string[]
							{
								rowKey,
								keyValuePair.Key
							}))));
							Array.Resize<byte>(ref iv, 16);
							aesCryptoServiceProvider.IV = iv;
							using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateEncryptor())
							{
								if (keyValuePair.Value.IsNull)
								{
									throw new InvalidOperationException("Null properties cannot be encrypted. Please assign a default value to the property if you wish to encrypt it.");
								}
								byte[] bytes = Encoding.UTF8.GetBytes(keyValuePair.Value.StringValue);
								byte[] input = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
								dictionary.Add(keyValuePair.Key, new EntityProperty(input));
								hashSet.Add(keyValuePair.Key);
								continue;
							}
						}
						dictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
					byte[] iv2 = sha256CryptoServiceProvider.ComputeHash(CommonUtility.BinaryAppend(encryptionData.ContentEncryptionIV, Encoding.UTF8.GetBytes(string.Join(partitionKey, new string[]
					{
						rowKey,
						"_ClientEncryptionMetadata2"
					}))));
					Array.Resize<byte>(ref iv2, 16);
					aesCryptoServiceProvider.IV = iv2;
					using (ICryptoTransform cryptoTransform2 = aesCryptoServiceProvider.CreateEncryptor())
					{
						byte[] bytes2 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(hashSet));
						byte[] input2 = cryptoTransform2.TransformFinalBlock(bytes2, 0, bytes2.Length);
						dictionary.Add("_ClientEncryptionMetadata2", new EntityProperty(input2));
					}
				}
			}
			dictionary.Add("_ClientEncryptionMetadata1", new EntityProperty(JsonConvert.SerializeObject(encryptionData)));
			return dictionary;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00029864 File Offset: 0x00027A64
		internal byte[] DecryptMetadataAndReturnCEK(string partitionKey, string rowKey, EntityProperty encryptionKeyProperty, EntityProperty propertyDetailsProperty, out EncryptionData encryptionData)
		{
			if (this.Key == null && this.KeyResolver == null)
			{
				throw new StorageException("Key and Resolver are not initialized. Decryption requires either of them to be initialized.", null)
				{
					IsRetryable = false
				};
			}
			byte[] result2;
			try
			{
				encryptionData = JsonConvert.DeserializeObject<EncryptionData>(encryptionKeyProperty.StringValue);
				CommonUtility.AssertNotNull("ContentEncryptionIV", encryptionData.ContentEncryptionIV);
				CommonUtility.AssertNotNull("EncryptedKey", encryptionData.WrappedContentKey.EncryptedKey);
				if (encryptionData.EncryptionAgent.Protocol != "1.0")
				{
					throw new StorageException("Invalid Encryption Agent. This version of the client library does not understand the Encryption Agent set on the blob.", null)
					{
						IsRetryable = false
					};
				}
				byte[] array = null;
				if (this.KeyResolver != null)
				{
					IKey result = this.KeyResolver.ResolveKeyAsync(encryptionData.WrappedContentKey.KeyId, CancellationToken.None).Result;
					CommonUtility.AssertNotNull("keyEncryptionKey", result);
					array = result.UnwrapKeyAsync(encryptionData.WrappedContentKey.EncryptedKey, encryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
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
					array = this.Key.UnwrapKeyAsync(encryptionData.WrappedContentKey.EncryptedKey, encryptionData.WrappedContentKey.Algorithm, CancellationToken.None).Result;
				}
				using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
				{
					using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
					{
						byte[] iv = sha256CryptoServiceProvider.ComputeHash(CommonUtility.BinaryAppend(encryptionData.ContentEncryptionIV, Encoding.UTF8.GetBytes(string.Join(partitionKey, new string[]
						{
							rowKey,
							"_ClientEncryptionMetadata2"
						}))));
						Array.Resize<byte>(ref iv, 16);
						aesCryptoServiceProvider.IV = iv;
						aesCryptoServiceProvider.Key = array;
						using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor())
						{
							byte[] binaryValue = propertyDetailsProperty.BinaryValue;
							propertyDetailsProperty.BinaryValue = cryptoTransform.TransformFinalBlock(binaryValue, 0, binaryValue.Length);
						}
					}
				}
				result2 = array;
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
			return result2;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00029B54 File Offset: 0x00027D54
		internal Dictionary<string, EntityProperty> DecryptEntity(IDictionary<string, EntityProperty> properties, HashSet<string> encryptedPropertyDetailsSet, string partitionKey, string rowKey, byte[] contentEncryptionKey, EncryptionData encryptionData)
		{
			Dictionary<string, EntityProperty> result;
			try
			{
				Dictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();
				EncryptionAlgorithm encryptionAlgorithm = encryptionData.EncryptionAgent.EncryptionAlgorithm;
				if (encryptionAlgorithm != EncryptionAlgorithm.AES_CBC_256)
				{
					throw new StorageException("Invalid Encryption Algorithm found on the resource. This version of the client library does not support the specified encryption algorithm.", null)
					{
						IsRetryable = false
					};
				}
				using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
				{
					using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
					{
						aesCryptoServiceProvider.Key = contentEncryptionKey;
						foreach (KeyValuePair<string, EntityProperty> keyValuePair in properties)
						{
							if (!(keyValuePair.Key == "_ClientEncryptionMetadata1") && !(keyValuePair.Key == "_ClientEncryptionMetadata2"))
							{
								if (encryptedPropertyDetailsSet.Contains(keyValuePair.Key))
								{
									byte[] iv = sha256CryptoServiceProvider.ComputeHash(CommonUtility.BinaryAppend(encryptionData.ContentEncryptionIV, Encoding.UTF8.GetBytes(string.Join(partitionKey, new string[]
									{
										rowKey,
										keyValuePair.Key
									}))));
									Array.Resize<byte>(ref iv, 16);
									aesCryptoServiceProvider.IV = iv;
									byte[] binaryValue = keyValuePair.Value.BinaryValue;
									using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor())
									{
										byte[] array = cryptoTransform.TransformFinalBlock(binaryValue, 0, binaryValue.Length);
										string @string = Encoding.UTF8.GetString(array, 0, array.Length);
										dictionary.Add(keyValuePair.Key, new EntityProperty(@string));
										continue;
									}
								}
								dictionary.Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
				result = dictionary;
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
			return result;
		}
	}
}
