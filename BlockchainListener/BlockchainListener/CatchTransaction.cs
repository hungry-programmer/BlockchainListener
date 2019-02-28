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
	class CatchTransaction
	{
		private static int GetLastBlockNumber(string coin)
		{
			Thread.Sleep(1000);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://chain.so/api/v2/get_info/" + coin);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			var result = JsonConvert.DeserializeObject<NetworkInfo>(responseString);

			return result.data.blocks;
		}

		private static long GetLastCheckedBlockNumber(string coin)
		{
			long blocknumber = long.Parse(IOStream.Read("last_checked_block_" + coin + ".txt"));
			return blocknumber;
		}

		private static void SaveLastCheckedBlockNumber(long blockNumber, string coin)
		{
			IOStream.WriteBlockNumber("last_checked_block_" + coin + ".txt", blockNumber.ToString());
		}

		private static List<Tx> GetBlockTransactions(long blockNumber, string coin)
		{
			Thread.Sleep(3000);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://chain.so/api/v2/block/{0}/{1}", coin, blockNumber));
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			var result = JsonConvert.DeserializeObject<BlockInfo>(responseString);

			return result.data.txs;
		}

		private static string TransactionCatched(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			return responseString;
		}

		public static void CatchTtransaction()
		{
			//ხდება ფაილიდან ტექსტის წაკითხვა
			string responce = IOStream.Read("hooks.txt");
			string[] splittedResponce;
			splittedResponce = responce.Split('\n');
			splittedResponce = splittedResponce.Take(splittedResponce.Count() - 1).ToArray();

			//ლაიტკოინის ჰუკებს ვაგროვებთ ერთად
			List<TransactionHook> hooksLTC = new List<TransactionHook>();
			foreach (var i in splittedResponce)
			{
				TransactionHook hook = JsonConvert.DeserializeObject<TransactionHook>(i);
				if (hook.coin == "LTC")
					hooksLTC.Add(hook);
			}

			//ბიტკოინის ჰუკებს ვაგროვებთ ერთად
			List<TransactionHook> hooksBTC = new List<TransactionHook>();
			foreach (var i in splittedResponce)
			{
				TransactionHook hook = JsonConvert.DeserializeObject<TransactionHook>(i);
				if (hook.coin == "BTC")
					hooksBTC.Add(hook);
			}



			// ბლოკის ტრანზაქციებეის სანახი ლაიტკოინისთვის
			List<Tx> blockTransactionsLTC = new List<Tx>();
			// ბლოკის ტრანზაქციებეის სანახი ბიტკოინისთვის
			List<Tx> blockTransactionsBTC = new List<Tx>();



			// წამოვიღოთ ლითკოინის ბლოკჩეინიდან ბოლო ბლოკის ნომერი
			long latestBlockLTC = GetLastBlockNumber("LTC");
			// წამოვიღოთ ბიტკოინის ბლოკჩეინიდან ბოლო ბლოკის ნომერი
			long latestBlockBTC = GetLastBlockNumber("BTC");



			// წამოვიღოთ ჩვენს ბაზაში შენახული ბოლო შემოწმებული ბლოკის ნომერი LTC
			long lastBlockLTC = GetLastCheckedBlockNumber("LTC");
			// წამოვიღოთ ჩვენს ბაზაში შენახული ბოლო შემოწმებული ბლოკის ნომერი BTC
			long lastBlockBTC = GetLastCheckedBlockNumber("BTC");



			//ციკლი ლაიტკოინის მისამართების ამოსაცნობად
			while (lastBlockLTC < latestBlockLTC)
			{
				Thread.Sleep(500);
				blockTransactionsLTC = GetBlockTransactions(lastBlockLTC + 1, "LTC");

				//გადავყვეთ თითოეულ ტრანზაქციას ბლოკში
				foreach (Tx tx in blockTransactionsLTC)
				{
					//ვამოწმებთ ინფუთებიდან რომელიმე ხომ არ იყო ჩვენი მისამართი
					foreach (Input input in tx.inputs)
					{
						foreach (TransactionHook hook in hooksLTC)
						{
							if (hook.address == input.address)
							{
								string fullURL = string.Format("{0}?address={1}&amp;transactionType=CREDIT&amp;amount={2}&amp;commission={3}", hook.url, input.value, tx.fee);
								TransactionCatched(fullURL);
							}
						}
					}

					//ვამოწმებთ აუთფუთებიდან რომელიმე ხომ არ იყო ჩვენი მისამართი
					foreach (Output output in tx.outputs)
					{
						foreach (TransactionHook hook in hooksLTC)
						{
							if (hook.address == output.address)
							{
								string fullURL = string.Format("{0}?address={1}&amp;transactionType=DEBIT&amp;amount={2}&amp;commission={3}", hook.url, output.value, tx.fee);
								TransactionCatched(fullURL);
							}
						}
					}
				}
				lastBlockLTC++;
				SaveLastCheckedBlockNumber(lastBlockLTC, "LTC");
			}
			//ციკლი ბიტკოინის მისამართების ამოსაცნობად
			while (lastBlockLTC < latestBlockLTC)
			{
				Thread.Sleep(500);
				blockTransactionsBTC = GetBlockTransactions(lastBlockBTC + 1, "BTC");

				//გადავყვეთ თითოეულ ტრანზაქციას ბლოკში
				foreach (Tx tx in blockTransactionsBTC)
				{
					//ვამოწმებთ ინფუთებიდან რომელიმე ხომ არ იყო ჩვენი მისამართი
					foreach (Input input in tx.inputs)
					{
						foreach (TransactionHook hook in hooksBTC)
						{
							if (hook.address == input.address)
							{
								string fullURL = string.Format("{0}?address={1}&amp;transactionType=CREDIT&amp;amount={2}&amp;commission={3}", hook.url, input.value, tx.fee);
								TransactionCatched(fullURL);
							}
						}
					}

					//ვამოწმებთ აუთფუთებიდან რომელიმე ხომ არ იყო ჩვენი მისამართი
					foreach (Output output in tx.outputs)
					{
						foreach (TransactionHook hook in hooksBTC)
						{
							if (hook.address == output.address)
							{
								string fullURL = string.Format("{0}?address={1}&amp;transactionType=DEBIT&amp;amount={2}&amp;commission={3}", hook.url, output.value, tx.fee);
								TransactionCatched(fullURL);
							}
						}
					}
				}
				lastBlockBTC++;
				SaveLastCheckedBlockNumber(lastBlockBTC, "BTC");
			}



			//ფუნქცია ჩერდება 6 წუთით და ელოდება ახალი ბლოკების გამოსვლას. შემდეგ ეშვება თავიდან რეკურსიულად
			Thread.Sleep(1000 * 60 * 6);
			CatchTtransaction();
		}
	}
}
