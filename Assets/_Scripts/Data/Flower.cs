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
	public Range<float> LeavesPositionRange;

	[Header("Leaves")]
	public Range<int> LeafCountRange;
	public Color LeavesAverageColor;
	public Sprite LeavesTexture;
	public Range<float> LeavesCurvatureRange;
	public Range<float> LeavesLengthRange;
	public Range<float> LeavesWidthRange;

	[Header("Petals")]
	public Range<int> PetalCountRange;
	public Color PetalsAverageColor;
	public Sprite PetalsTexture;
	public Range<float> PetalsLengthRange;
	public Range<float> PetalsWidthRange;
	public Range<int> PetalsLevelCountRange;

	[Header("Pistils")]
	public Range<int> PistilCountRange;
	public Range<float> PistilsLengthRange;
	public Range<float> CapitulumRadiusRange;
	public Range<float> PistilsAverageColor;
}
