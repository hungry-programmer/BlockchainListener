using Blockchein_Transaction_Hook.API_Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blockchein_Transaction_Hook
{
	class TransactionHook
	{
		public string coin { get; set; }
		public string address { get; set; }
		public string url { get; set; }

	}
}
