using System;
using UnityEngine;

// NOTE:
// dynamic Unity dont support
// http://www.yoda.arachsys.com/csharp/miscutil/usage/genericoperators.html
// 使用范型比较繁琐，需要考虑到 type convert, 而且存在性能问题
// 并且实际使用中主要是 int, float
namespace Diablo
{
	public class RangeInt
	{
		int min;
		int max;
		int current;

		public RangeInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public void Change(int delta)
		{
			int next = current + delta;
			current = Mathf.Clamp(next, min, max);
		}
	}
}
