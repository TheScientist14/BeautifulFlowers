using System;

[Serializable]
public class Range<T>
{
	public T Min;
	public T Max;
}

static class NumericalRangeSpecialization
{
	public static int RandVal(this Range<int> iRange)
	{
		return UnityEngine.Random.Range(iRange.Min, iRange.Max);
	}

	public static float RandVal(this Range<float> iRange)
	{
		return UnityEngine.Random.Range(iRange.Min, iRange.Max);
	}
}