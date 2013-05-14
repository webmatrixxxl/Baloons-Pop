using System;

namespace BaloonsPop
{
	public static class RandomGenerator
	{
        static readonly Random random = new Random();

		public static string GetRandomInt()
		{
			string legalChars = "1234";
			string builder = null;
			builder = legalChars[random.Next(0, legalChars.Length)].ToString();

			return builder;
		}
	}
}