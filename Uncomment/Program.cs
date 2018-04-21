using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uncomment
{
	class Program
	{
		static string DeleteComment(string data)
		{
			bool commentLineSign = false;
			bool commentBlockSign = false;
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == '/' && data[i + 1] == '/')
				{
					commentLineSign = true;
				}
				else if (data[i] == '\n')
				{
					commentLineSign = false;
				}
				else if (data[i] == '/' && data[i + 1] == '*')
				{
					commentBlockSign = true;
				}
				else if (i > 0 && data[i - 1] == '*' && data[i] == '/')
				{
					commentBlockSign = false;
				}
				else if (!commentLineSign && !commentBlockSign)
				{
					output.Append(data[i]);
				}
			}
			return output.ToString();
		}

		static string DeleteExtraEmptyLine(string data)
		{
			StringBuilder outputStringBuilder = new StringBuilder();
			StringReader inputStringReader = new StringReader(data);
			bool emptyLineSign = false;
			while (true)
			{
				string singleLine = inputStringReader.ReadLine();
				if (singleLine == null)
				{
					break;
				}
				if (String.IsNullOrWhiteSpace(singleLine))
				{
					if (!emptyLineSign)
					{
						emptyLineSign = true;
						//outputStringBuilder.AppendLine(string.Empty);
					}
				}
				else
				{
					emptyLineSign = false;
					outputStringBuilder.AppendLine(singleLine);
				}
			}
			return outputStringBuilder.ToString();
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("No input file, press any key to exit");
				Console.ReadKey();
				return;
			}
			foreach (string path in args)
			{
				try
				{
					File.Move(path, path + ".bak");
				}
				catch (Exception e)
				{
					Console.WriteLine("Error: " + e.Message);
					continue;
				}
				
				FileStream inputFileStream;
				inputFileStream = new FileStream(path + ".bak", FileMode.Open);
				FileStream outputFileStream = new FileStream(path,FileMode.Create);
				StreamReader inputStreamReader = new StreamReader(inputFileStream);
				StreamWriter outputStreamWriter = new StreamWriter(outputFileStream);
				string allData = inputStreamReader.ReadToEnd();

				string uncommentData = DeleteComment(allData);
				string outputData = DeleteExtraEmptyLine(uncommentData);

				outputStreamWriter.Write(outputData);

				outputStreamWriter.Flush();
				outputStreamWriter.Close();
				inputStreamReader.Close();

				outputFileStream.Close();
				inputFileStream.Close();

				Console.WriteLine(" Completed: " + path);
			}
			Console.WriteLine("Finished, press any key to exit");
			Console.ReadKey();
		}
	}
}
