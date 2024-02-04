using System.Collections.Generic;
using UnityEngine;

public class FlowerGenerator : MonoBehaviour
{
	[SerializeField] private Cylinder m_UpCylinderPrefab;
	[SerializeField] private Plane m_SidePlanePrefab;
	[SerializeField] private Plane m_CenterPlanePrefab;

	[SerializeField] private Material m_StemMat;
	[SerializeField] private Material m_LeafMat;
	[SerializeField] private Material m_PetalMat;
	[SerializeField] private Material m_CapitulumMat;

	[SerializeField] private Vector3 colTol = new Vector3(0.05f, 0.05f, 0.1f);

	private FlowerInstance m_FlowerInstance = null;
	private List<Cylinder> m_Stems;
	private List<Plane> m_Leaves;
	private Plane m_Capitulum = null;
	private List<Plane> m_Petals;

	public void GenerateSubject(Flower iFlower)
	{
		Random.InitState(iFlower.Seed);
		m_FlowerInstance = new FlowerInstance();

		m_FlowerInstance.StemHeight = iFlower.StemHeightRange.RandVal();
		m_FlowerInstance.StemRadius = iFlower.StemRadiusRange.RandVal();
		m_FlowerInstance.StemColor = GetRandomColorVariant(iFlower.StemAverageColor);



		Render();
	}

	public void Render(FlowerInstance iFlowerInstance)
	{
		m_FlowerInstance = iFlowerInstance;
		Render();
	}

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
			leaf.transform.localPosition = new Vector3(-m_FlowerInstance.StemRadius, (1 - leafData.Position) * m_FlowerInstance.StemHeight, 0);
			leaf.transform.localRotation = Quaternion.AngleAxis(leafData.RotationAroundStem, Vector3.up);
			leaf.SetLength(leafData.Length);
			leaf.SetWidth(leafData.Width);
			leaf.SetMaterial(m_LeafMat);
			leaf.SetTexture(m_FlowerInstance.LeavesTexture.texture);
			leaf.SetColor(leafData.Color);

			m_Leaves.Add(leaf);
		}

		m_Capitulum = Instantiate(m_CenterPlanePrefab, stemJoint);
		m_Capitulum.SetLength(m_FlowerInstance.CapitulumRadius);
		m_Capitulum.SetWidth(m_FlowerInstance.CapitulumRadius);
		m_Capitulum.SetMaterial(m_CapitulumMat);
		m_Capitulum.SetColor(m_FlowerInstance.CapitulumColor);

		foreach(FlowerInstance.Petal petalData in m_FlowerInstance.Petals)
		{
			Plane petal = Instantiate(m_SidePlanePrefab, stemJoint);
			petal.transform.localPosition = new Vector3(-m_FlowerInstance.CapitulumRadius, 0, 0);
			petal.transform.localRotation = Quaternion.Euler(0, petalData.RotationAroundCapitulum, m_FlowerInstance.PetalLevelAngles[petalData.LevelIndex]);
			petal.SetLength(petalData.Length);
			petal.SetWidth(petalData.Width);
			petal.SetMaterial(m_PetalMat);
			petal.SetTexture(m_FlowerInstance.PetalsTexture.texture);
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
