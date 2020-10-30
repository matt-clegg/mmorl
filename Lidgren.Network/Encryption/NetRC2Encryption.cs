using System;
using System.IO;
using System.Security.Cryptography;

namespace Lidgren.Network
{
	public class NetRC2Encryption : NetCryptoProviderBase
	{
		public NetRC2Encryption(NetPeer peer)
			: base(peer, new RC2CryptoServiceProvider())
		{
		}

		public NetRC2Encryption(NetPeer peer, string key)
			: base(peer, new RC2CryptoServiceProvider())
		{
			SetKey(key);
		}

		public NetRC2Encryption(NetPeer peer, byte[] data, int offset, int count)
			: this(peer, data.AsSpan(offset, count))
		{
		}

		public NetRC2Encryption(NetPeer peer, ReadOnlySpan<byte> data)
			: base(peer, new RC2CryptoServiceProvider())
		{
			SetKey(data);
		}
	}
}
