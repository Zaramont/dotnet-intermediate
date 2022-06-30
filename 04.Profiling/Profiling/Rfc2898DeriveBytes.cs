using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Security.Cryptography
{
	/// <summary>Implements password-based key derivation functionality, PBKDF2, by using a pseudo-random number generator based on <see cref="T:System.Security.Cryptography.HMACSHA1" />.</summary>
	[ComVisible(true)]
	public class Rfc2898DeriveBytes : DeriveBytes
	{
		private byte[] m_buffer;

		private byte[] m_salt;

		private HMAC m_hmac;

		private byte[] m_password;

		private CspParameters m_cspParams = new CspParameters();

		private uint m_iterations;

		private uint m_block;

		private int m_startIndex;

		private int m_endIndex;

		private int m_blockSize;

		[SecurityCritical]
		private SafeProvHandle _safeProvHandle;

		/// <summary>Gets or sets the number of iterations for the operation.</summary>
		/// <returns>The number of iterations for the operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of iterations is less than 1.</exception>
		public int IterationCount
		{
			get
			{
				return (int)m_iterations;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
				}
				m_iterations = (uint)value;
				Initialize();
			}
		}

		/// <summary>Gets or sets the key salt value for the operation.</summary>
		/// <returns>The key salt value for the operation.</returns>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes.</exception>
		/// <exception cref="T:System.ArgumentNullException">The salt is <see langword="null" />.</exception>
		public byte[] Salt
		{
			get
			{
				return (byte[])m_salt.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length < 8)
				{
					throw new ArgumentException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_FewBytesSalt"));
				}
				m_salt = (byte[])value.Clone();
				Initialize();
			}
		}

		private SafeProvHandle ProvHandle
		{
			[SecurityCritical]
			get
			{
				if (_safeProvHandle == null)
				{
					lock (this)
					{
						if (_safeProvHandle == null)
						{
							SafeProvHandle safeProvHandle = Utils.AcquireProvHandle(m_cspParams);
							Thread.MemoryBarrier();
							_safeProvHandle = safeProvHandle;
						}
					}
				}
				return _safeProvHandle;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using the password and salt size to derive the key.</summary>
		/// <param name="password">The password used to derive the key.</param>
		/// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes.</exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is <see langword="null" />.</exception>
		public Rfc2898DeriveBytes(string password, int saltSize)
			: this(password, saltSize, 1000)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt size, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key.</param>
		/// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="iterations" /> is out of range. This parameter requires a non-negative number.</exception>
		public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
			: this(password, saltSize, iterations, HashAlgorithmName.SHA1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using the specified password, salt size, number of iterations and the hash algorithm name to derive the key.</summary>
		/// <param name="password">The password to use to derive the key.</param>
		/// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <param name="hashAlgorithm">The hash algorithm to use to derive the key.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="saltSize" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> property of <paramref name="hashAlgorithm" /> is either <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">Hash algorithm name is invalid.</exception>
		[SecuritySafeCritical]
		public Rfc2898DeriveBytes(string password, int saltSize, int iterations, HashAlgorithmName hashAlgorithm)
		{
			if (saltSize < 0)
			{
				throw new ArgumentOutOfRangeException("saltSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (string.IsNullOrEmpty(hashAlgorithm.Name))
			{
				throw new ArgumentException(Environment.GetResourceString("Cryptography_HashAlgorithmNameNullOrEmpty"), "hashAlgorithm");
			}
			HMAC hMAC = HMAC.Create("HMAC" + hashAlgorithm.Name);
			if (hMAC == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_UnknownHashAlgorithm", hashAlgorithm.Name));
			}
			byte[] array = new byte[saltSize];
			Utils.StaticRandomNumberGenerator.GetBytes(array);
			Salt = array;
			IterationCount = iterations;
			m_password = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(password);
			hMAC.Key = m_password;
			m_hmac = hMAC;
			m_blockSize = hMAC.HashSize >> 3;
			Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password and salt to derive the key.</summary>
		/// <param name="password">The password used to derive the key.</param>
		/// <param name="salt">The key salt used to derive the key.</param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is <see langword="null" />.</exception>
		public Rfc2898DeriveBytes(string password, byte[] salt)
			: this(password, salt, 1000)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key.</param>
		/// <param name="salt">The key salt used to derive the key.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is <see langword="null" />.</exception>
		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
			: this(password, salt, iterations, HashAlgorithmName.SHA1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using the specified password, salt, number of iterations and the hash algorithm name to derive the key.</summary>
		/// <param name="password">The password to use to derive the key.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <param name="hashAlgorithm">The hash algorithm to use to derive the key.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> property of <paramref name="hashAlgorithm" /> is either <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">Hash algorithm name is invalid.</exception>
		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
			: this(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(password), salt, iterations, hashAlgorithm)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key.</param>
		/// <param name="salt">The key salt used to derive the key.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is <see langword="null" />.</exception>
		public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
			: this(password, salt, iterations, HashAlgorithmName.SHA1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using the specified password, salt, number of iterations and the hash algorithm name to derive the key.</summary>
		/// <param name="password">The password to use to derive the key.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		/// <param name="iterations">The number of iterations for the operation.</param>
		/// <param name="hashAlgorithm">The hash algorithm to use to derive the key.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="saltSize" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> property of <paramref name="hashAlgorithm" /> is either <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">Hash algorithm name is invalid.</exception>
		[SecuritySafeCritical]
		public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
		{
			if (string.IsNullOrEmpty(hashAlgorithm.Name))
			{
				throw new ArgumentException(Environment.GetResourceString("Cryptography_HashAlgorithmNameNullOrEmpty"), "hashAlgorithm");
			}
			HMAC hMAC = HMAC.Create("HMAC" + hashAlgorithm.Name);
			if (hMAC == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_UnknownHashAlgorithm", hashAlgorithm.Name));
			}
			Salt = salt;
			IterationCount = iterations;
			m_password = password;
			hMAC.Key = password;
			m_hmac = hMAC;
			m_blockSize = hMAC.HashSize >> 3;
			Initialize();
		}

		/// <summary>Returns the pseudo-random key for this object.</summary>
		/// <param name="cb">The number of pseudo-random key bytes to generate.</param>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="cb" /> is out of range. This parameter requires a non-negative number.</exception>
		public override byte[] GetBytes(int cb)
		{
			if (cb <= 0)
			{
				throw new ArgumentOutOfRangeException("cb", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			byte[] array = new byte[cb];
			int i = 0;
			int num = m_endIndex - m_startIndex;
			if (num > 0)
			{
				if (cb < num)
				{
					Buffer.InternalBlockCopy(m_buffer, m_startIndex, array, 0, cb);
					m_startIndex += cb;
					return array;
				}
				Buffer.InternalBlockCopy(m_buffer, m_startIndex, array, 0, num);
				m_startIndex = (m_endIndex = 0);
				i += num;
			}
			for (; i < cb; i += m_blockSize)
			{
				byte[] src = Func();
				int num2 = cb - i;
				if (num2 > m_blockSize)
				{
					Buffer.InternalBlockCopy(src, 0, array, i, m_blockSize);
					continue;
				}
				Buffer.InternalBlockCopy(src, 0, array, i, num2);
				i += num2;
				Buffer.InternalBlockCopy(src, num2, m_buffer, m_startIndex, m_blockSize - num2);
				m_endIndex += m_blockSize - num2;
				return array;
			}
			return array;
		}

		/// <summary>Resets the state of the operation.</summary>
		public override void Reset()
		{
			Initialize();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (m_hmac != null)
				{
					((IDisposable)m_hmac).Dispose();
				}
				if (m_buffer != null)
				{
					Array.Clear(m_buffer, 0, m_buffer.Length);
				}
				if (m_salt != null)
				{
					Array.Clear(m_salt, 0, m_salt.Length);
				}
			}
		}

		private void Initialize()
		{
			if (m_buffer != null)
			{
				Array.Clear(m_buffer, 0, m_buffer.Length);
			}
			m_buffer = new byte[m_blockSize];
			m_block = 1u;
			m_startIndex = (m_endIndex = 0);
		}

		private byte[] Func()
		{
			byte[] array = Utils.Int(m_block);
			m_hmac.TransformBlock(m_salt, 0, m_salt.Length, null, 0);
			m_hmac.TransformBlock(array, 0, array.Length, null, 0);
			m_hmac.TransformFinalBlock(EmptyArray<byte>.Value, 0, 0);
			byte[] hashValue = m_hmac.HashValue;
			m_hmac.Initialize();
			byte[] array2 = hashValue;
			for (int i = 2; i <= m_iterations; i++)
			{
				m_hmac.TransformBlock(hashValue, 0, hashValue.Length, null, 0);
				m_hmac.TransformFinalBlock(EmptyArray<byte>.Value, 0, 0);
				hashValue = m_hmac.HashValue;
				for (int j = 0; j < m_blockSize; j++)
				{
					array2[j] ^= hashValue[j];
				}
				m_hmac.Initialize();
			}
			m_block++;
			return array2;
		}

		/// <summary>Derives a cryptographic key from the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> object.</summary>
		/// <param name="algname">The algorithm name for which to derive the key.</param>
		/// <param name="alghashname">The hash algorithm name to use to derive the key.</param>
		/// <param name="keySize">The size of the key, in bits, to derive.</param>
		/// <param name="rgbIV">The initialization vector (IV) to use to derive the key.</param>
		/// <returns>The derived key.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The <paramref name="keySize" /> parameter is incorrect.  
		///  -or-  
		///  The cryptographic service provider (CSP) cannot be acquired.  
		///  -or-  
		///  The <paramref name="algname" /> parameter is not a valid algorithm name.  
		///  -or-  
		///  The <paramref name="alghashname" /> parameter is not a valid hash algorithm name.</exception>
		[SecuritySafeCritical]
		public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
		{
			if (keySize < 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
			}
			int num = X509Utils.NameOrOidToAlgId(alghashname, OidGroup.HashAlgorithm);
			if (num == 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidAlgorithm"));
			}
			int num2 = X509Utils.NameOrOidToAlgId(algname, OidGroup.AllGroups);
			if (num2 == 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidAlgorithm"));
			}
			if (rgbIV == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidIV"));
			}
			byte[] o = null;
			DeriveKey(ProvHandle, num2, num, m_password, m_password.Length, keySize << 16, rgbIV, rgbIV.Length, JitHelpers.GetObjectHandleOnStack(ref o));
			return o;
		}

		[DllImport("QCall", CharSet = CharSet.Unicode)]
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		private static extern void DeriveKey(SafeProvHandle hProv, int algid, int algidHash, byte[] password, int cbPassword, int dwFlags, byte[] IV, int cbIV, ObjectHandleOnStack retKey);
	}
}
