using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlowerInstance
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
	public List<Leaf> Leaves = new List<Leaf>();
	public Texture LeavesTexture;

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
	public List<Petal> Petals = new List<Petal>();
	public Texture PetalsTexture;
	public List<float> PetalLevelAngles = new List<float>();
	public float CapitulumRadius;
	public Color CapitulumColor;
}
