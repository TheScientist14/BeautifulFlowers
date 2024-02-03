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

	public GameObject CreateSubject(FlowerInstance iFlowerType)
	{
		GameObject flower = new GameObject();

		Cylinder stemBase = Instantiate(m_UpCylinderPrefab, flower.transform);
		MeshRenderer stemRenderer = stemBase.GetMesh();
		stemRenderer.material = m_StemMat;
		stemRenderer.material.color = iFlowerType.StemColor;
		stemBase.SetHeight(iFlowerType.StemHeight);
		stemBase.SetRadius(iFlowerType.StemRadius);

		Transform stemJoint = stemBase.GetJoint();
		foreach(FlowerInstance.Leaf leafData in iFlowerType.Leaves)
		{
			Plane leaf = Instantiate(m_SidePlanePrefab, stemJoint);
			leaf.transform.localPosition = new Vector3(-iFlowerType.StemRadius, (1 - leafData.Position) * iFlowerType.StemHeight, 0);
			leaf.transform.localRotation = Quaternion.AngleAxis(leafData.RotationAroundStem, Vector3.up);
			leaf.SetLength(leafData.Length);
			leaf.SetWidth(leafData.Width);
			MeshRenderer leafRenderer = leaf.GetMesh();
			leafRenderer.material = m_LeafMat;
			leafRenderer.material.mainTexture = iFlowerType.LeavesTexture.texture;
			leafRenderer.material.color = leafData.Color;
		}

		Plane capitulum = Instantiate(m_CenterPlanePrefab, stemJoint);
		capitulum.SetLength(iFlowerType.CapitulumRadius);
		capitulum.SetWidth(iFlowerType.CapitulumRadius);
		capitulum.GetMesh().material = m_CapitulumMat;
		capitulum.GetMesh().material.color = iFlowerType.CapitulumColor;

		foreach(FlowerInstance.Petal petalData in iFlowerType.Petals)
		{
			Plane petal = Instantiate(m_SidePlanePrefab, stemJoint);
			petal.transform.localPosition = new Vector3(-iFlowerType.CapitulumRadius, 0, 0);
			petal.transform.localRotation = Quaternion.Euler(0, petalData.RotationAroundCapitulum, iFlowerType.PetalLevelAngles[petalData.LevelIndex]);
			petal.SetLength(petalData.Length);
			petal.SetWidth(petalData.Width);
			MeshRenderer petalRenderer = petal.GetMesh();
			petalRenderer.material = m_PetalMat;
			petalRenderer.material.mainTexture = iFlowerType.PetalsTexture.texture;
			petalRenderer.material.color = petalData.Color;
		}

		return flower;
	}

	private Color GetRandomColorVariant(Color iBaseColor)
	{
		float h, s, v;
		Color.RGBToHSV(iBaseColor, out h, out s, out v);

		return Random.ColorHSV(h - colTol.x, h + colTol.x, s - colTol.y, s + colTol.y, v - colTol.z, v + colTol.z, 1, 1);
	}
}
