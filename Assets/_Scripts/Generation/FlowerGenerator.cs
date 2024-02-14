using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowerGenerator : MonoBehaviour
{
	[SerializeField] private Cylinder m_UpCylinderPrefab;
	[SerializeField] private Plane m_SidePlanePrefab;
	[SerializeField] private Plane m_CenterPlanePrefab;

	[SerializeField] private Material m_StemMat;
	[SerializeField] private Material m_CapitulumMat;

	[SerializeField] private Vector3 s_ColTol = new Vector3(0.05f, 0.05f, 0.1f); // hsv
	[Range(0, 1)]
	[SerializeField] private float s_StemsOffsetTol = 0.5f;
	[Range(0, 1)]
	[SerializeField] private float s_LeavesOffsetTol = 0.6f;
	[Range(0, 1)]
	[SerializeField] private float s_PetalsOffsetTol = 0.8f;
	[SerializeField] private Vector3 s_RotationTol = new Vector3(5, 5, 5);

	[SerializeField] private Flower m_Flower = null;
	[SerializeField] private FlowerInstance m_FlowerInstance;
	private int m_Seed;

	private List<Cylinder> m_Stems = new List<Cylinder>();
	private List<Plane> m_Leaves = new List<Plane>();
	private Plane m_Capitulum = null;
	private List<Plane> m_Petals = new List<Plane>();

	[Range(0, 1)]
	[SerializeField] public float m_WindStrength = 0.3f;
	[SerializeField] private Range<float> m_WindSpeedRange;
	[SerializeField] private float m_WindMaxAmplitudeModification = 70;
	private float m_WindMaxAmplitudeModificationPerStem;

	[Range(0, 1)]
	[SerializeField] private float m_BlossomingState = 1; // 0: not blossomed, 1: fully blossomed
	[SerializeField] private Range<float> m_BlossomingRotationRange;

	[Range(0, 1)]
	[SerializeField] private float m_HydrationState = 1; // 0: dehydrated, 1: hydrated
	[SerializeField] private Color m_DehydratedColor = Color.white;
	[SerializeField] private Range<float> m_DehydrationRotationRange;

	private bool m_IsBlossomingDirty = true;
	private bool m_IsHydrationDirty = true;

	// summary parameters:
	// species
	// small -> tall
	// thin -> thick
	// hydration
	// blossoming
	// wind state
	[SerializeField] private List<string> m_HeightAdjectives = new List<string>();
	[SerializeField] private List<string> m_ThicknessAdjectives = new List<string>();
	[SerializeField] private List<string> m_HydrationStartSentence = new List<string>();
	[SerializeField] private List<string> m_BlossomEndSentence = new List<string>();
	[SerializeField] private List<string> m_WindRemarks = new List<string>();
	[SerializeField] private List<string> m_Names = new List<string>();
	[SerializeField] private List<string> m_SummaryTemplates = new List<string>();
	private string m_FlowerName;
	private string m_SummaryTemplate;

	void Start()
	{
		if(m_Flower != null)
			GenerateSubject();
	}

	public void GenerateSubject(Flower iFlower)
	{
		m_Flower = iFlower;
		GenerateSubject();
	}

	[Button]
	public void GenerateSubject()
	{
		Random.InitState(DateTime.Now.Millisecond);
		m_Seed = Random.Range(int.MinValue, int.MaxValue);
		_GenerateSubject();
	}

	[Button]
	private void _GenerateSubject()
	{
		Random.InitState(m_Seed);
		m_FlowerInstance = new FlowerInstance();

		m_FlowerInstance.WindOffset = Random.Range(-1000f, 1000f);

		m_FlowerInstance.TotalStemHeight = m_Flower.StemHeightRange.RandVal();
		m_FlowerInstance.StemRadius = m_Flower.StemRadiusRange.RandVal();
		m_FlowerInstance.StemColor = GetRandomColorVariant(m_Flower.StemAverageColor);
		int stemPartNumber = Mathf.FloorToInt(m_FlowerInstance.TotalStemHeight);
		for(int stemPartIdx = 0; stemPartIdx < stemPartNumber; stemPartIdx++)
		{
			FlowerInstance.StemPart stemPart = new FlowerInstance.StemPart();
			stemPart.Rotation = GetRandomRotation();

			m_FlowerInstance.StemParts.Add(stemPart);
		}
		m_WindMaxAmplitudeModificationPerStem = m_WindMaxAmplitudeModification / stemPartNumber;

		m_FlowerInstance.LeavesMaterial = m_Flower.LeavesMaterial;
		int nbLeaves = m_Flower.LeafCountRange.RandVal();
		for(int leafIdx = 0; leafIdx < nbLeaves; leafIdx++)
		{
			FlowerInstance.Leaf leaf = new FlowerInstance.Leaf();
			leaf.Length = m_Flower.LeavesLengthRange.RandVal();
			leaf.Width = m_Flower.LeavesWidthRange.RandVal();
			leaf.Color = GetRandomColorVariant(m_Flower.LeavesAverageColor);
			leaf.Position = m_Flower.LeavesPositionRange.RandVal();
			leaf.RotationAroundStem = Random.Range(0f, 360f);
			leaf.Rotation = Quaternion.Euler(0, 0, m_Flower.LeavesAverageUpRotation) * GetRandomRotation();

			m_FlowerInstance.Leaves.Add(leaf);
		}

		m_FlowerInstance.CapitulumColor = GetRandomColorVariant(m_Flower.CapitulumAverageColor);
		m_FlowerInstance.CapitulumRadius = m_Flower.CapitulumRadiusRange.RandVal();

		m_FlowerInstance.PetalsMaterial = m_Flower.PetalsMaterial;
		int nbPetals = m_Flower.PetalCountRange.RandVal();
		int nbPetalLevels = m_Flower.PetalsLevelCountRange.RandVal();
		int nbPetalsPerLevel = (nbPetalLevels != 0) ? (nbPetals - 1) / nbPetalLevels + 1 : 0;
		int totPetals = 0;

		float petalLevelAngleDifference = m_Flower.PetalsMaxDispersionUpRotation * 0.5f;
		float minLevelAngle = m_Flower.PetalsAverageUpRotation - petalLevelAngleDifference * (nbPetalLevels - 1) * 0.5f;
		float rotLevelOffset = 360f / nbPetals;

		for(int petalLevelIdx = 0; petalLevelIdx < nbPetalLevels; petalLevelIdx++)
		{
			m_FlowerInstance.PetalLevelAngles.Add(minLevelAngle + petalLevelAngleDifference * petalLevelIdx + Random.Range(-1f, 1f));

			int nbPetalsLeft = Mathf.Min(nbPetals - totPetals, nbPetalsPerLevel);
			float rotationIncr = (nbPetalsLeft != 0) ? 360f / nbPetalsLeft : 0;
			for(int petalIdx = 0; petalIdx < nbPetalsLeft; petalIdx++, totPetals++)
			{
				FlowerInstance.Petal petal = new FlowerInstance.Petal();
				petal.Length = m_Flower.PetalsLengthRange.RandVal();
				petal.Width = m_Flower.PetalsWidthRange.RandVal();
				petal.Color = GetRandomColorVariant(m_Flower.PetalsAverageColor);
				petal.LevelIndex = petalLevelIdx;
				petal.RotationAroundCapitulum = petalIdx * rotationIncr + rotLevelOffset * petalLevelIdx + Random.Range(-2f, 2f);
				petal.Rotation = GetRandomRotation();

				m_FlowerInstance.Petals.Add(petal);
			}
		}

		m_FlowerName = m_Names[Random.Range(0, m_Names.Count - 1)];
		m_SummaryTemplate = m_SummaryTemplates[Random.Range(0, m_SummaryTemplates.Count - 1)];

		Render();
	}

	[Button]
	private void Render()
	{
		Clear();

		Transform topStemJoint = transform;
		int stemPartNumber = m_FlowerInstance.StemParts.Count;
		float stemPartsHeight = m_FlowerInstance.TotalStemHeight / stemPartNumber;
		foreach(FlowerInstance.StemPart stemPartData in m_FlowerInstance.StemParts)
		{
			Cylinder stemPart = Instantiate(m_UpCylinderPrefab, topStemJoint);
			float upOffset = m_FlowerInstance.StemRadius * s_StemsOffsetTol;
			stemPart.SetHeight(stemPartsHeight + upOffset);
			stemPart.SetRadius(m_FlowerInstance.StemRadius);
			stemPart.transform.localRotation = stemPartData.Rotation;
			stemPart.transform.localPosition -= Vector3.up * upOffset;
			MeshRenderer stemRenderer = stemPart.GetMesh();
			stemRenderer.material = m_StemMat;
			stemRenderer.material.color = m_FlowerInstance.StemColor;

			m_Stems.Add(stemPart);
			topStemJoint = stemPart.GetJoint();
		}

		float posPerStemPart = 1f / stemPartNumber;
		foreach(FlowerInstance.Leaf leafData in m_FlowerInstance.Leaves)
		{
			float leafHeight = leafData.Position * m_FlowerInstance.TotalStemHeight;
			int stemPartIdx = Mathf.FloorToInt(leafHeight / stemPartsHeight);
			float posRelativeToStemPart = leafData.Position - stemPartIdx * posPerStemPart;
			Transform joint = m_Stems[stemPartIdx].GetJoint();

			Plane leaf = Instantiate(m_SidePlanePrefab, joint);
			leaf.transform.localPosition = (posRelativeToStemPart - 1) * stemPartsHeight * Vector3.up;
			leaf.transform.localRotation = Quaternion.AngleAxis(leafData.RotationAroundStem, Vector3.up);
			leaf.SetOffset(m_FlowerInstance.StemRadius * s_LeavesOffsetTol);
			leaf.SetRotation(leafData.Rotation);
			leaf.SetLength(leafData.Length);
			leaf.SetWidth(leafData.Width);
			leaf.SetMaterial(m_FlowerInstance.LeavesMaterial);
			leaf.SetColor(leafData.Color);

			m_Leaves.Add(leaf);
		}

		m_Capitulum = Instantiate(m_CenterPlanePrefab, topStemJoint);
		m_Capitulum.SetLength(m_FlowerInstance.CapitulumRadius * 2);
		m_Capitulum.SetWidth(m_FlowerInstance.CapitulumRadius * 2);
		m_Capitulum.SetMaterial(m_CapitulumMat);
		m_Capitulum.SetColor(m_FlowerInstance.CapitulumColor);

		foreach(FlowerInstance.Petal petalData in m_FlowerInstance.Petals)
		{
			Plane petal = Instantiate(m_SidePlanePrefab, topStemJoint);
			petal.transform.localRotation = Quaternion.AngleAxis(petalData.RotationAroundCapitulum, Vector3.up);
			petal.SetOffset(m_FlowerInstance.CapitulumRadius * s_PetalsOffsetTol);
			petal.SetRotation(Quaternion.Euler(0, 0, m_FlowerInstance.PetalLevelAngles[petalData.LevelIndex]) * petalData.Rotation);
			petal.SetLength(petalData.Length);
			petal.SetWidth(petalData.Width);
			petal.SetMaterial(m_FlowerInstance.PetalsMaterial);
			petal.SetColor(petalData.Color);

			m_Petals.Add(petal);
		}
	}

	void Update()
	{
		if(m_FlowerInstance == null || m_Flower == null)
			return;

		// apply modifications
		// wind -> stem animation
		_WindUpdate();
		// blossoming -> petals rotation
		_UpdateBlossoming();
		// hydration -> colors + leaves rotation
		_UpdateHydration();
	}

	private void _WindUpdate()
	{
		float windSpeed = Mathf.Lerp(m_WindSpeedRange.Min, m_WindSpeedRange.Max, m_WindStrength);
		int stemPartIdx = 0;
		foreach((var stemPartData, var stemPart) in m_FlowerInstance.StemParts.Zip(m_Stems, (x, y) => (x, y)))
		{
			float xRotation = Mathf.PerlinNoise(m_FlowerInstance.WindOffset + Time.time * windSpeed * stemPartIdx, stemPartIdx) * m_WindStrength * m_WindMaxAmplitudeModificationPerStem;
			Quaternion windRotation = Quaternion.Euler(xRotation, 0, 0);
			stemPart.transform.localRotation = Quaternion.identity;
			stemPart.transform.rotation *= windRotation;
			stemPart.transform.localRotation *= stemPartData.Rotation;

			stemPartIdx++;
		}
	}

	private void _UpdateBlossoming()
	{
		if(!m_IsBlossomingDirty)
			return;

		m_IsBlossomingDirty = false;

		Quaternion blossomingRotation = Quaternion.Euler(0, 0, Mathf.Lerp(m_BlossomingRotationRange.Max, m_BlossomingRotationRange.Min, m_BlossomingState));
		foreach((var petalData, var petal) in m_FlowerInstance.Petals.Zip(m_Petals, (x, y) => (x, y)))
			petal.SetRotation(blossomingRotation * Quaternion.Euler(0, 0, m_FlowerInstance.PetalLevelAngles[petalData.LevelIndex]) * petalData.Rotation);
	}

	private void _UpdateHydration()
	{
		if(!m_IsHydrationDirty)
			return;

		m_IsHydrationDirty = false;

		Color stemColor = Color.Lerp(m_DehydratedColor, m_FlowerInstance.StemColor, m_HydrationState);
		foreach(Cylinder stemPart in m_Stems)
			stemPart.GetMesh().material.color = stemColor;

		Quaternion hydrationRotation = Quaternion.Euler(0, 0, -Mathf.Lerp(m_DehydrationRotationRange.Max, m_DehydrationRotationRange.Min, m_HydrationState));
		foreach((var leafData, var leaf) in m_FlowerInstance.Leaves.Zip(m_Leaves, (x, y) => (x, y)))
		{
			leaf.SetColor(Color.Lerp(m_DehydratedColor, leafData.Color, m_HydrationState));
			leaf.SetRotation(hydrationRotation * leafData.Rotation);
		}
	}

	private void Clear()
	{
		m_Stems.Clear();
		m_Leaves.Clear();
		m_Capitulum = null;
		m_Petals.Clear();
		m_IsBlossomingDirty = true;
		m_IsHydrationDirty = true;

		foreach(Transform child in transform)
			Destroy(child.gameObject);
	}

	public void SetBlossomingState(float iBlossomingState)
	{
		m_BlossomingState = iBlossomingState;
		m_IsBlossomingDirty = true;
	}

	public float GetBlossomingState()
	{
		return m_BlossomingState;
	}

	public void SetHydrationState(float iHydrationState)
	{
		m_HydrationState = iHydrationState;
		m_IsHydrationDirty = true;
	}

	public float GetHydrationState()
	{
		return m_HydrationState;
	}

	public string GetSummary()
	{
		// named parameters ?

		return string.Format(m_SummaryTemplate,
			m_FlowerName,
			GetClosestString(m_HeightAdjectives, ComputeParam(m_Flower.StemHeightRange, m_FlowerInstance.TotalStemHeight)),
			GetClosestString(m_ThicknessAdjectives, ComputeParam(m_Flower.StemRadiusRange, m_FlowerInstance.StemRadius)),
			m_Flower.SpeciesName,
			GetClosestString(m_WindRemarks, m_WindStrength),
			GetClosestString(m_HydrationStartSentence, m_HydrationState),
			GetClosestString(m_BlossomEndSentence, m_BlossomingState)
		);
	}

	private string GetClosestString(List<string> iStrings, float iParam)
	{
		if(iStrings == null || iStrings.Count == 0)
			return "";

		return iStrings[Mathf.Clamp(0, iStrings.Count, Mathf.FloorToInt(Mathf.Clamp01(iParam) * iStrings.Count))];
	}

	private float ComputeParam(Range<float> iRange, float iValue)
	{
		return Mathf.InverseLerp(iRange.Min, iRange.Max, iValue);
	}

	private Color GetRandomColorVariant(Color iBaseColor)
	{
		float h, s, v;
		Color.RGBToHSV(iBaseColor, out h, out s, out v);

		return Random.ColorHSV(h - s_ColTol.x, h + s_ColTol.x, s - s_ColTol.y, s + s_ColTol.y, v - s_ColTol.z, v + s_ColTol.z, 1, 1);
	}

	private Quaternion GetRandomRotation()
	{
		return Quaternion.Euler(
				Random.Range(-s_RotationTol.x, s_RotationTol.x),
				Random.Range(-s_RotationTol.y, s_RotationTol.y),
				Random.Range(-s_RotationTol.z, s_RotationTol.z));
	}
}
