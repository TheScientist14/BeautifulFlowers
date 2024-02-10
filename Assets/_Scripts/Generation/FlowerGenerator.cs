using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FlowerGenerator : MonoBehaviour
{
	[SerializeField] private Cylinder m_UpCylinderPrefab;
	[SerializeField] private Plane m_SidePlanePrefab;
	[SerializeField] private Plane m_CenterPlanePrefab;

	[SerializeField] private Material m_StemMat;
	[SerializeField] private Material m_CapitulumMat;

	[SerializeField] private Vector3 colTol = new Vector3(0.05f, 0.05f, 0.1f);

	[SerializeField] private Flower m_Flower;
	[SerializeField] private FlowerInstance m_FlowerInstance;

	private List<Cylinder> m_Stems = new List<Cylinder>();
	private List<Plane> m_Leaves = new List<Plane>();
	private Plane m_Capitulum = null;
	private List<Plane> m_Petals = new List<Plane>();

	void Start()
	{
		if(m_Flower == null)
			m_Flower = new Flower();

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
		Random.InitState((int)(Time.time * 1000));
		m_Flower.Seed = Random.Range(int.MinValue, int.MaxValue);
		_GenerateSubject();
	}

	[Button]
	private void _GenerateSubject()
	{
		Random.InitState(m_Flower.Seed);
		m_FlowerInstance = new FlowerInstance();

		m_FlowerInstance.StemHeight = m_Flower.StemHeightRange.RandVal();
		m_FlowerInstance.StemRadius = m_Flower.StemRadiusRange.RandVal();
		m_FlowerInstance.StemColor = GetRandomColorVariant(m_Flower.StemAverageColor);

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

			m_FlowerInstance.Leaves.Add(leaf);
		}

		m_FlowerInstance.CapitulumColor = GetRandomColorVariant(m_Flower.CapitulumAverageColor);
		m_FlowerInstance.CapitulumRadius = m_Flower.CapitulumRadiusRange.RandVal();

		m_FlowerInstance.PetalsMaterial = m_Flower.PetalsMaterial;
		int nbPetals = m_Flower.PetalCountRange.RandVal();
		int nbPetalLevels = m_Flower.PetalsLevelCountRange.RandVal();
		int nbPetalsPerLevel = (nbPetalLevels != 0) ? (nbPetals - 1) / nbPetalLevels + 1 : 0;
		int totPetals = 0;

		// magic values
		const float petalLevelAngleAverage = 5f;
		const float petalLevelAngleDifference = 10f;
		float minLevelAngle = petalLevelAngleAverage - petalLevelAngleDifference * (nbPetalLevels - 1) * 0.5f;
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

				m_FlowerInstance.Petals.Add(petal);
			}
		}

		Render();
	}

	public void Render(FlowerInstance iFlowerInstance)
	{
		m_FlowerInstance = iFlowerInstance;
		Render();
	}

	const float s_LeavesOffsetTol = 0.6f;
	const float s_PetalsOffsetTol = 0.8f;

	[Button]
	public void Render()
	{
		Clear();

		Cylinder stemBase = Instantiate(m_UpCylinderPrefab, transform);
		stemBase.SetHeight(m_FlowerInstance.StemHeight);
		stemBase.SetRadius(m_FlowerInstance.StemRadius);
		MeshRenderer stemRenderer = stemBase.GetMesh();
		stemRenderer.material = m_StemMat;
		stemRenderer.material.color = m_FlowerInstance.StemColor;

		Transform stemJoint = stemBase.GetJoint();
		foreach(FlowerInstance.Leaf leafData in m_FlowerInstance.Leaves)
		{
			Plane leaf = Instantiate(m_SidePlanePrefab, stemJoint);
			leaf.transform.localPosition = (leafData.Position - 1) * m_FlowerInstance.StemHeight * Vector3.up;
			leaf.transform.localRotation = Quaternion.AngleAxis(leafData.RotationAroundStem, Vector3.up);
			leaf.SetOffset(m_FlowerInstance.StemRadius * s_LeavesOffsetTol);
			leaf.SetLength(leafData.Length);
			leaf.SetWidth(leafData.Width);
			leaf.SetMaterial(m_FlowerInstance.LeavesMaterial);
			leaf.SetColor(leafData.Color);

			m_Leaves.Add(leaf);
		}

		m_Capitulum = Instantiate(m_CenterPlanePrefab, stemJoint);
		m_Capitulum.SetLength(m_FlowerInstance.CapitulumRadius * 2);
		m_Capitulum.SetWidth(m_FlowerInstance.CapitulumRadius * 2);
		m_Capitulum.SetMaterial(m_CapitulumMat);
		m_Capitulum.SetColor(m_FlowerInstance.CapitulumColor);

		foreach(FlowerInstance.Petal petalData in m_FlowerInstance.Petals)
		{
			Plane petal = Instantiate(m_SidePlanePrefab, stemJoint);
			petal.transform.localRotation = Quaternion.AngleAxis(petalData.RotationAroundCapitulum, Vector3.up);
			petal.SetOffset(m_FlowerInstance.CapitulumRadius * s_PetalsOffsetTol);
			petal.SetRotation(Quaternion.Euler(0, 0, m_FlowerInstance.PetalLevelAngles[petalData.LevelIndex]));
			petal.SetLength(petalData.Length);
			petal.SetWidth(petalData.Width);
			petal.SetMaterial(m_FlowerInstance.PetalsMaterial);
			petal.SetColor(petalData.Color);

			m_Petals.Add(petal);
		}
	}

	private void Clear()
	{
		m_Stems.Clear();
		m_Leaves.Clear();
		m_Capitulum = null;
		m_Petals.Clear();

		foreach(Transform child in transform)
			Destroy(child.gameObject);
	}

	private Color GetRandomColorVariant(Color iBaseColor)
	{
		float h, s, v;
		Color.RGBToHSV(iBaseColor, out h, out s, out v);

		return Random.ColorHSV(h - colTol.x, h + colTol.x, s - colTol.y, s + colTol.y, v - colTol.z, v + colTol.z, 1, 1);
	}
}
