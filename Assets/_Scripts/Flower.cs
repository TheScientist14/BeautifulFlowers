using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Flower
{
	[Header("Stem")]
	public Range<float> StemHeightRange;
	public Range<float> StemRadiusRange;
	public Color StemAverageColor;
	public Range<float> LeafPositionRange;


}
