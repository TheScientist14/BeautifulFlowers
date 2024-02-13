using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlowerInstance
{
	public float WindOffset;

	// Lengths are in cm

	[Serializable]
	public struct StemPart
	{
		public Quaternion Rotation;
	}

	[Header("Stem")]
	public float TotalStemHeight;
	public float StemRadius;
	public Color StemColor;
	public List<StemPart> StemParts = new List<StemPart>();

	[Serializable]
	public struct Leaf
	{
		public float Position;
		public float RotationAroundStem;
		public Quaternion Rotation;
		public Color Color;
		public float Length;
		public float Width;
	}

	[Header("Leaves")]
	public List<Leaf> Leaves = new List<Leaf>();
	public Material LeavesMaterial;

	[Serializable]
	public struct Petal
	{
		public int LevelIndex;
		public float RotationAroundCapitulum;
		public Quaternion Rotation;
		public Color Color;
		public float Length;
		public float Width;
	}

	[Header("Petals")]
	public List<Petal> Petals = new List<Petal>();
	public Material PetalsMaterial;
	public List<float> PetalLevelAngles = new List<float>();
	public float CapitulumRadius;
	public Color CapitulumColor;
}
