using System;

namespace Diablo
{
	public static class Utils
	{
		public static Random rnd = new Random();

		public static int Rand(int min, int max)
		{
			return rnd.Next(min, max);
		}

		public static int Rand(int max)
		{
			return Rand(0, max);
		}
	}
}
