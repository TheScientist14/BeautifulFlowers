using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "FlowerSpecies", menuName = "ScriptableObjects/FlowerSpecies")]
public class Flower : ScriptableObject
{
	public string SpeciesName;
	// Lengths are in cm

	[Header("Stem")]
	public Range<float> StemHeightRange;
	public Range<float> StemRadiusRange;
	public Color StemAverageColor;
	public Range<float> LeavesPositionRange;

	[Header("Leaves")]
	public Range<int> LeafCountRange;
	public Color LeavesAverageColor;
	public Material LeavesMaterial;
	public Range<float> LeavesLengthRange;
	public Range<float> LeavesWidthRange;
	public float LeavesAverageUpRotation;

	[Header("Petals")]
	public Range<int> PetalCountRange;
	public Color PetalsAverageColor;
	public Material PetalsMaterial;
	public Range<float> PetalsLengthRange;
	public Range<float> PetalsWidthRange;
	public Range<int> PetalsLevelCountRange;
	public float PetalsAverageUpRotation;
	public float PetalsMaxDispersionUpRotation;

	[Header("Capitulum")]
	public Range<float> CapitulumRadiusRange;
	public Color CapitulumAverageColor;
}
