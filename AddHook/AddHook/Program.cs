using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddHook
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hey! input fields to add hook");

			Console.WriteLine("coin: ");
			string coin = Console.ReadLine();

			Console.WriteLine("address: ");
			string address = Console.ReadLine();

			Console.WriteLine("(only url. path will be added)url: ");
			string url = Console.ReadLine();

			try
			{
				AddHook(coin, address, url);
			}
			catch (Exception ex)
			{
				Console.Out.Write(ex.Message);
			}
			finally
			{
				Console.Out.WriteLine("new hook added successfully");
			}

			Console.ReadKey();
		}

		private static void AddHook(string coin, string address, string url)
		{
			TransactionHook hook = new TransactionHook();
			hook.coin = coin;
			hook.address = address;
			hook.url = url;

			IOStream.Write("hooks.txt", JsonConvert.SerializeObject(hook));
		}
	}
}
