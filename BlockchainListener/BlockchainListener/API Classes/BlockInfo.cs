using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchein_Transaction_Hook.API_Classes
{
	public class ReceivedFrom
	{
		public string txid { get; set; }
		public int output_no { get; set; }
	}

	public class Input
	{
		public int input_no { get; set; }
		public string address { get; set; }
		public string value { get; set; }
		public ReceivedFrom received_from { get; set; }
	}

	public class Spent
	{
		public string txid { get; set; }
		public int input_no { get; set; }
	}

	public class Output
	{
		public int output_no { get; set; }
		public string address { get; set; }
		public string value { get; set; }
		public Spent spent { get; set; }
	}

	public class Tx
	{
		public string txid { get; set; }
		public string fee { get; set; }
		public List<Input> inputs { get; set; }
		public List<Output> outputs { get; set; }
	}

	public class BlockData
	{
		public string network { get; set; }
		public int block_no { get; set; }
		public int confirmations { get; set; }
		public string miner { get; set; }
		public object miner_url { get; set; }
		public int version { get; set; }
		public int time { get; set; }
		public string sent_value { get; set; }
		public string fee { get; set; }
		public string mining_difficulty { get; set; }
		public long nonce { get; set; }
		public string bits { get; set; }
		public int size { get; set; }
		public string blockhash { get; set; }
		public string merkleroot { get; set; }
		public string previous_blockhash { get; set; }
		public string next_blockhash { get; set; }
		public List<Tx> txs { get; set; }
	}

	public class BlockInfo
	{
		public string status { get; set; }
		public BlockData data { get; set; }
		public int code { get; set; }
		public string message { get; set; }
	}
}
