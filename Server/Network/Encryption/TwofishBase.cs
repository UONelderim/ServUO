#region References
using System.Security.Cryptography;
#endregion

namespace Server.Network.Encryption
{
	public abstract class TwofishBase
	{
		private const int BLOCK_SIZE = 128;
		private const int MAX_ROUNDS = 16;
		private const int ROUNDS_128 = 16;
		private const int ROUNDS_192 = 16;
		private const int ROUNDS_256 = 16;
		private const int KEY_BITS = 256;

		private const int INPUT_WHITEN = 0;
		private const int OUTPUT_WHITEN = INPUT_WHITEN + BLOCK_SIZE / 32;
		private const int ROUND_SUBKEYS = OUTPUT_WHITEN + BLOCK_SIZE / 32;
		private const int TOTAL_SUBKEYS = ROUND_SUBKEYS + 2 * MAX_ROUNDS;

		private const uint SK_STEP = 0x02020202u;
		private const uint SK_BUMP = 0x01010101u;
		private const int SK_ROTL = 9;

		private const uint RS_GF_FDBK = 0x14D;

		private const int MDS_GF_FDBK = 0x169; /* primitive polynomial for GF(256)*/

		private static readonly byte[,] m_Table =
		{
			{
				0xA9, 0x67, 0xB3, 0xE8, 0x04, 0xFD, 0xA3, 0x76, 0x9A, 0x92, 0x80, 0x78, 0xE4, 0xDD, 0xD1, 0x38, 0x0D, 0xC6, 0x35,
				0x98, 0x18, 0xF7, 0xEC, 0x6C, 0x43, 0x75, 0x37, 0x26, 0xFA, 0x13, 0x94, 0x48, 0xF2, 0xD0, 0x8B, 0x30, 0x84, 0x54,
				0xDF, 0x23, 0x19, 0x5B, 0x3D, 0x59, 0xF3, 0xAE, 0xA2, 0x82, 0x63, 0x01, 0x83, 0x2E, 0xD9, 0x51, 0x9B, 0x7C, 0xA6,
				0xEB, 0xA5, 0xBE, 0x16, 0x0C, 0xE3, 0x61, 0xC0, 0x8C, 0x3A, 0xF5, 0x73, 0x2C, 0x25, 0x0B, 0xBB, 0x4E, 0x89, 0x6B,
				0x53, 0x6A, 0xB4, 0xF1, 0xE1, 0xE6, 0xBD, 0x45, 0xE2, 0xF4, 0xB6, 0x66, 0xCC, 0x95, 0x03, 0x56, 0xD4, 0x1C, 0x1E,
				0xD7, 0xFB, 0xC3, 0x8E, 0xB5, 0xE9, 0xCF, 0xBF, 0xBA, 0xEA, 0x77, 0x39, 0xAF, 0x33, 0xC9, 0x62, 0x71, 0x81, 0x79,
				0x09, 0xAD, 0x24, 0xCD, 0xF9, 0xD8, 0xE5, 0xC5, 0xB9, 0x4D, 0x44, 0x08, 0x86, 0xE7, 0xA1, 0x1D, 0xAA, 0xED, 0x06,
				0x70, 0xB2, 0xD2, 0x41, 0x7B, 0xA0, 0x11, 0x31, 0xC2, 0x27, 0x90, 0x20, 0xF6, 0x60, 0xFF, 0x96, 0x5C, 0xB1, 0xAB,
				0x9E, 0x9C, 0x52, 0x1B, 0x5F, 0x93, 0x0A, 0xEF, 0x91, 0x85, 0x49, 0xEE, 0x2D, 0x4F, 0x8F, 0x3B, 0x47, 0x87, 0x6D,
				0x46, 0xD6, 0x3E, 0x69, 0x64, 0x2A, 0xCE, 0xCB, 0x2F, 0xFC, 0x97, 0x05, 0x7A, 0xAC, 0x7F, 0xD5, 0x1A, 0x4B, 0x0E,
				0xA7, 0x5A, 0x28, 0x14, 0x3F, 0x29, 0x88, 0x3C, 0x4C, 0x02, 0xB8, 0xDA, 0xB0, 0x17, 0x55, 0x1F, 0x8A, 0x7D, 0x57,
				0xC7, 0x8D, 0x74, 0xB7, 0xC4, 0x9F, 0x72, 0x7E, 0x15, 0x22, 0x12, 0x58, 0x07, 0x99, 0x34, 0x6E, 0x50, 0xDE, 0x68,
				0x65, 0xBC, 0xDB, 0xF8, 0xC8, 0xA8, 0x2B, 0x40, 0xDC, 0xFE, 0x32, 0xA4, 0xCA, 0x10, 0x21, 0xF0, 0xD3, 0x5D, 0x0F,
				0x00, 0x6F, 0x9D, 0x36, 0x42, 0x4A, 0x5E, 0xC1, 0xE0
			},
			{
				0x75, 0xF3, 0xC6, 0xF4, 0xDB, 0x7B, 0xFB, 0xC8, 0x4A, 0xD3, 0xE6, 0x6B, 0x45, 0x7D, 0xE8, 0x4B, 0xD6, 0x32, 0xD8,
				0xFD, 0x37, 0x71, 0xF1, 0xE1, 0x30, 0x0F, 0xF8, 0x1B, 0x87, 0xFA, 0x06, 0x3F, 0x5E, 0xBA, 0xAE, 0x5B, 0x8A, 0x00,
				0xBC, 0x9D, 0x6D, 0xC1, 0xB1, 0x0E, 0x80, 0x5D, 0xD2, 0xD5, 0xA0, 0x84, 0x07, 0x14, 0xB5, 0x90, 0x2C, 0xA3, 0xB2,
				0x73, 0x4C, 0x54, 0x92, 0x74, 0x36, 0x51, 0x38, 0xB0, 0xBD, 0x5A, 0xFC, 0x60, 0x62, 0x96, 0x6C, 0x42, 0xF7, 0x10,
				0x7C, 0x28, 0x27, 0x8C, 0x13, 0x95, 0x9C, 0xC7, 0x24, 0x46, 0x3B, 0x70, 0xCA, 0xE3, 0x85, 0xCB, 0x11, 0xD0, 0x93,
				0xB8, 0xA6, 0x83, 0x20, 0xFF, 0x9F, 0x77, 0xC3, 0xCC, 0x03, 0x6F, 0x08, 0xBF, 0x40, 0xE7, 0x2B, 0xE2, 0x79, 0x0C,
				0xAA, 0x82, 0x41, 0x3A, 0xEA, 0xB9, 0xE4, 0x9A, 0xA4, 0x97, 0x7E, 0xDA, 0x7A, 0x17, 0x66, 0x94, 0xA1, 0x1D, 0x3D,
				0xF0, 0xDE, 0xB3, 0x0B, 0x72, 0xA7, 0x1C, 0xEF, 0xD1, 0x53, 0x3E, 0x8F, 0x33, 0x26, 0x5F, 0xEC, 0x76, 0x2A, 0x49,
				0x81, 0x88, 0xEE, 0x21, 0xC4, 0x1A, 0xEB, 0xD9, 0xC5, 0x39, 0x99, 0xCD, 0xAD, 0x31, 0x8B, 0x01, 0x18, 0x23, 0xDD,
				0x1F, 0x4E, 0x2D, 0xF9, 0x48, 0x4F, 0xF2, 0x65, 0x8E, 0x78, 0x5C, 0x58, 0x19, 0x8D, 0xE5, 0x98, 0x57, 0x67, 0x7F,
				0x05, 0x64, 0xAF, 0x63, 0xB6, 0xFE, 0xF5, 0xB7, 0x3C, 0xA5, 0xCE, 0xE9, 0x68, 0x44, 0xE0, 0x4D, 0x43, 0x69, 0x29,
				0x2E, 0xAC, 0x15, 0x59, 0xA8, 0x0A, 0x9E, 0x6E, 0x47, 0xDF, 0x34, 0x35, 0x6A, 0xCF, 0xDC, 0x22, 0xC9, 0xC0, 0x9B,
				0x89, 0xD4, 0xED, 0xAB, 0x12, 0xA2, 0x0D, 0x52, 0xBB, 0x02, 0x2F, 0xA9, 0xD7, 0x61, 0x1E, 0xB4, 0x50, 0x04, 0xF6,
				0xC2, 0x16, 0x25, 0x86, 0x56, 0x55, 0x09, 0xBE, 0x91
			}
		};

		public const int InputBlockSize = BLOCK_SIZE / 8;
		public const int OutputBlockSize = BLOCK_SIZE / 8;

		protected readonly uint[] SboxKeys = new uint[KEY_BITS / 64];
		protected readonly uint[] SubKeys = new uint[TOTAL_SUBKEYS];
		protected readonly uint[] Key = {0, 0, 0, 0, 0, 0, 0, 0};
		protected readonly uint[] IV = {0, 0, 0, 0};

		protected CipherMode CipherMode = CipherMode.ECB;

		private readonly int[] m_NumRounds = {0, ROUNDS_128, ROUNDS_192, ROUNDS_256};

		private int m_KeyLength, m_Rounds;

		private static uint F32(uint x, uint[] k32, int keyLen)
		{
			var buffer = new[] { RS(x, 0), RS(x, 1), RS(x, 2), RS(x, 3) };

			switch (((keyLen + 63) / 64) & 0x03)
			{
				case 0:
				{
					buffer[0] = (byte)(m_Table[1, buffer[0]] ^ RS(k32[3], 0));
					buffer[1] = (byte)(m_Table[0, buffer[1]] ^ RS(k32[3], 1));
					buffer[2] = (byte)(m_Table[0, buffer[2]] ^ RS(k32[3], 2));
					buffer[3] = (byte)(m_Table[1, buffer[3]] ^ RS(k32[3], 3));
				}
					goto case 3;
				case 3:
				{
					buffer[0] = (byte)(m_Table[1, buffer[0]] ^ RS(k32[2], 0));
					buffer[1] = (byte)(m_Table[1, buffer[1]] ^ RS(k32[2], 1));
					buffer[2] = (byte)(m_Table[0, buffer[2]] ^ RS(k32[2], 2));
					buffer[3] = (byte)(m_Table[0, buffer[3]] ^ RS(k32[2], 3));
				}
					goto case 2;
				case 2:
				{
					buffer[0] = m_Table[1, m_Table[0, m_Table[0, buffer[0]] ^ RS(k32[1], 0)] ^ RS(k32[0], 0)];
					buffer[1] = m_Table[0, m_Table[0, m_Table[1, buffer[1]] ^ RS(k32[1], 1)] ^ RS(k32[0], 1)];
					buffer[2] = m_Table[1, m_Table[1, m_Table[0, buffer[2]] ^ RS(k32[1], 2)] ^ RS(k32[0], 2)];
					buffer[3] = m_Table[0, m_Table[1, m_Table[1, buffer[3]] ^ RS(k32[1], 3)] ^ RS(k32[0], 3)];
				}
					break;
			}

			return (uint)((buffer[0] ^ LFSR2(buffer[1]) ^ LFSR1(buffer[2]) ^ LFSR1(buffer[3])) << 0)
				 ^ (uint)((LFSR1(buffer[0]) ^ LFSR2(buffer[1]) ^ LFSR2(buffer[2]) ^ buffer[3]) << 8) 
				 ^ (uint)((LFSR2(buffer[0]) ^ LFSR1(buffer[1]) ^ buffer[2] ^ LFSR2(buffer[3])) << 16) 
				 ^ (uint)((LFSR2(buffer[0]) ^ buffer[1] ^ LFSR2(buffer[2]) ^ LFSR1(buffer[3])) << 24);
		}

		protected bool Rekey(int length, uint[] buffer)
		{
			int i;

			m_KeyLength = length;
			m_Rounds = m_NumRounds[(length - 1) / 64];

			var subkeyCnt = ROUND_SUBKEYS + 2 * m_Rounds;

			uint A, B;

			var key1 = new uint[KEY_BITS / 64];
			var key2 = new uint[KEY_BITS / 64];

			var k64Cnt = (length + 63) / 64;

			for (i = 0; i < k64Cnt; i++)
			{
				key1[i] = buffer[2 * i];
				key2[i] = buffer[2 * i + 1];

				SboxKeys[k64Cnt - 1 - i] = RS_MDS_Encode(key1[i], key2[i]);
			}

			for (i = 0; i < subkeyCnt / 2; i++)
			{
				A = F32((uint)(i * SK_STEP), key1, length);
				B = F32((uint)(i * SK_STEP + SK_BUMP), key2, length);
				B = ROL(B, 8);

				SubKeys[2 * i] = A + B;
				SubKeys[2 * i + 1] = ROL(A + 2 * B, SK_ROTL);
			}

			return true;
		}

		public void BlockDecrypt(uint[] buffer)
		{
			uint t0, t1;

			var iv = new uint[4];

			if (CipherMode == CipherMode.CBC)
			{
				buffer.CopyTo(iv, 0);
			}

			for (var i = 0; i < BLOCK_SIZE / 32; i++)
			{
				buffer[i] ^= SubKeys[OUTPUT_WHITEN + i];
			}

			for (var r = m_Rounds - 1; r >= 0; r--)
			{
				t0 = F32(buffer[0], SboxKeys, m_KeyLength);
				t1 = F32(ROL(buffer[1], 8), SboxKeys, m_KeyLength);

				buffer[2] = ROL(buffer[2], 1);
				buffer[2] ^= t0 + t1 + SubKeys[ROUND_SUBKEYS + 2 * r];
				buffer[3] ^= t0 + 2 * t1 + SubKeys[ROUND_SUBKEYS + 2 * r + 1];
				buffer[3] = ROR(buffer[3], 1);

				if (r > 0)
				{
					t0 = buffer[0];
					buffer[0] = buffer[2];
					buffer[2] = t0;
					t1 = buffer[1];
					buffer[1] = buffer[3];
					buffer[3] = t1;
				}
			}

			for (var i = 0; i < BLOCK_SIZE / 32; i++)
			{
				buffer[i] ^= SubKeys[INPUT_WHITEN + i];

				if (CipherMode == CipherMode.CBC)
				{
					buffer[i] ^= IV[i];
					IV[i] = iv[i];
				}
			}
		}

		public void BlockEncrypt(uint[] buffer)
		{
			uint t0, t1, tmp;

			for (var i = 0; i < BLOCK_SIZE / 32; i++)
			{
				buffer[i] ^= SubKeys[INPUT_WHITEN + i];

				if (CipherMode == CipherMode.CBC)
				{
					buffer[i] ^= IV[i];
				}
			}

			for (var r = 0; r < m_Rounds; r++)
			{
				t0 = F32(buffer[0], SboxKeys, m_KeyLength);
				t1 = F32(ROL(buffer[1], 8), SboxKeys, m_KeyLength);

				buffer[3] = ROL(buffer[3], 1);
				buffer[2] ^= t0 + t1 + SubKeys[ROUND_SUBKEYS + 2 * r];
				buffer[3] ^= t0 + 2 * t1 + SubKeys[ROUND_SUBKEYS + 2 * r + 1];
				buffer[2] = ROR(buffer[2], 1);

				if (r < m_Rounds - 1)
				{
					tmp = buffer[0];
					buffer[0] = buffer[2];
					buffer[2] = tmp;
					tmp = buffer[1];
					buffer[1] = buffer[3];
					buffer[3] = tmp;
				}
			}

			for (var i = 0; i < BLOCK_SIZE / 32; i++)
			{
				buffer[i] ^= SubKeys[OUTPUT_WHITEN + i];

				if (CipherMode == CipherMode.CBC)
				{
					IV[i] = buffer[i];
				}
			}
		}

		private static uint RS_MDS_Encode(uint key1, uint key2)
		{
			uint i, j, r;

			for (i = r = 0; i < 2; i++)
			{
				r ^= (i > 0) ? key1 : key2;

				for (j = 0; j < 4; j++)
				{
					var v1 = (byte)(r >> 24);
					var v2 = (uint)(((v1 << 1) ^ (((v1 & 0x80) == 0x80) ? RS_GF_FDBK : 0)) & 0xFF);
					var v3 = (uint)(((v1 >> 1) & 0x7F) ^ (((v1 & 1) == 1) ? RS_GF_FDBK >> 1 : 0) ^ v2);

					r = (r << 8) ^ (v3 << 24) ^ (v2 << 16) ^ (v3 << 8) ^ v1;
				}
			}

			return r;
		}

		private static int LFSR1(int val)
		{
			return val ^ LFSR4(val);
		}

		private static int LFSR2(int val)
		{
			return val ^ LFSR3(val) ^ LFSR4(val);
		}

		private static int LFSR3(int val)
		{
			return (val >> 1) ^ (((val & 0x01) == 0x01) ? MDS_GF_FDBK / 2 : 0);
		}

		private static int LFSR4(int val)
		{
			return (val >> 2) ^ (((val & 0x02) == 0x02) ? MDS_GF_FDBK / 2 : 0) ^ (((val & 0x01) == 0x01) ? MDS_GF_FDBK / 4 : 0);
		}

		private static uint ROL(uint val, int flag)
		{
			return (val << (flag & 0x1F)) | val >> (32 - (flag & 0x1F));
		}

		private static uint ROR(uint val, int flag)
		{
			return (val >> (flag & 0x1F)) | (val << (32 - (flag & 0x1F)));
		}

		protected static uint LSU(uint val, int shift)
		{
			return val << (shift * 8);
		}

		protected static uint RSU(uint val, int shift)
		{
			return val >> (shift * 8);
		}

		protected static byte LS(uint val, int shift)
		{
			return (byte)LSU(val, shift);
		}

		protected static byte RS(uint val, int shift)
		{
			return (byte)RSU(val, shift);
		}
	}
}
