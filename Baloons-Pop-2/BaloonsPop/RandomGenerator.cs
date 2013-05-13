using System;

namespace BaloonsPop
{
	public static class RandomGenerator
	{

		static Random r = new Random();
		public static string GetRandomInt()
		{
			string legalChars = "1234";
			string builder = null;
			builder = legalChars[r.Next(0, legalChars.Length)].ToString();
			return builder;
		}
	}
}
