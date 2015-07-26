namespace Info.Blockchain.API.BlockExplorer
{
	public struct BitcoinValue
	{
		public const int SatoshisPerBitcoin = 100000000;
		public const int BitsPerBitcoin = 1000000;
		public const int MilliBitsPerBitcoin = 1000;

		private decimal btc { get; }
		public BitcoinValue(decimal btc)
		{
			this.btc = btc;
		}

		public decimal Btc => this.btc;

		public decimal MilliBits => this.btc * MilliBitsPerBitcoin;

		public decimal Bits => this.btc * BitsPerBitcoin;

		public long Satoshis => (long)(this.btc * SatoshisPerBitcoin);

		public static BitcoinValue Zero => new BitcoinValue();

		public static BitcoinValue FromSatoshis(long satoshis) => new BitcoinValue((decimal)satoshis / SatoshisPerBitcoin);

		public static BitcoinValue FromBits(decimal bits) => new BitcoinValue(bits / BitsPerBitcoin);

		public static BitcoinValue FromMilliBits(decimal mBtc) => new BitcoinValue(mBtc / MilliBitsPerBitcoin);

		public static BitcoinValue FromBtc(decimal btc) => new BitcoinValue(btc);

		public static BitcoinValue operator +(BitcoinValue x, BitcoinValue y)
		{
			decimal btc = x.Btc + y.Btc;
			return new BitcoinValue(btc);
		}

		public static BitcoinValue operator -(BitcoinValue x, BitcoinValue y)
		{
			decimal btc = x.Btc - y.Btc;
			return new BitcoinValue(btc);
		}

		public override string ToString() => this.Btc.ToString();

	}
}
