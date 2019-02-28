using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchein_Transaction_Hook.API_Classes
{
	public class NetworkInfo
	{
		public string status { get; set; }
		public NetworkData data { get; set; }
	}

	public class NetworkData
	{
		public string name { get; set; }
		public string acronym { get; set; }
		public string network { get; set; }
		public string symbol_htmlcode { get; set; }
		public string url { get; set; }
		public string mining_difficulty { get; set; }
		public int unconfirmed_txs { get; set; }
		public int blocks { get; set; }
		public string price { get; set; }
		public string price_base { get; set; }
		public int price_update_time { get; set; }
		public string hashrate { get; set; }
	}
}
