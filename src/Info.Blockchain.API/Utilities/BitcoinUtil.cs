using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Info.Blockchain.API.Utilities
{
	public static class BitcoinUtil
	{
		public static double ToBtc(long satoshis)
		{
			return (double)satoshis / 100000000;
		}
	}
}
