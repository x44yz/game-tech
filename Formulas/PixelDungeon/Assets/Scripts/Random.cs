using System;
using UnityEngine;

public static class Random
{
    // [0, 1)
	public static float Float() {
		return UnityEngine.Random.Range(0f, 1f);
	}

    // [0, max)
    public static float Float(float max)
    {
        return UnityEngine.Random.Range(0f, max);
    }

    // [0, max)
    public static int Int(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    // [min, max)
    public static int Int(int min, int max)
    {
        return min + Int(max - min);
    }

    //returns a uniformly distributed int in the range [min, max]
    public static int IntRange(int min, int max)
    {
        return min + Int(max - min + 1);
    }

	//returns a triangularly distributed int in the range [min, max]
	public static int NormalIntRange( int min, int max ) {
		return min + (int)((Float() + Float()) * (max - min + 1) / 2f);
	}
}