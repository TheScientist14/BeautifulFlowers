using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FlowerInstance
{
	// Lengths are in mm

	[Header("Stem")]
	public float StemHeight;
	public float StemRadius;
	public Color StemColor;

	[Serializable]
	public struct Leaf
	{
		public float Position;
		public float RotationAroundStem;
		public Color Color;
		public float Length;
		public float Width;
	}

	[Header("Leaves")]
	public List<Leaf> Leaves;
	public Sprite LeavesTexture;

	[Serializable]
	public struct Petal
	{
		public float RotationAroundCapitulum;
		public int LevelIndex;
		public Color Color;
		public float Length;
		public float Width;
	}

	[Header("Petals")]
	public List<Petal> Petals;
	public Sprite PetalsTexture;
	public List<float> PetalLevelAngles;
	public float CapitulumRadius;
	public Color CapitulumColor;
}
