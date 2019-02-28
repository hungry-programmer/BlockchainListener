using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddHook
{
	class IOStream
	{
		public static string Read(string fileName)
		{
			String line;
			StringBuilder sb = new StringBuilder();
			try
			{
				StreamReader sr = new StreamReader("C:\\Users\\David\\Documents\\" + fileName);
				line = sr.ReadLine();
				while (line != null)
				{
					sb.AppendLine(line);
					line = sr.ReadLine();
				}
				sr.Close();

			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}

			return sb.ToString();
		}

		public static void Write(string fileName, string data)
		{
			data = Read(fileName) + data;
			try
			{
				StreamWriter sw = new StreamWriter("C:\\Users\\David\\Documents\\" + fileName);

				sw.Write(data);

				sw.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
		}
	}
}
