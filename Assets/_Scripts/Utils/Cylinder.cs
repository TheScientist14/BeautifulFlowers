using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
	[SerializeField] MeshRenderer m_Mesh;
	[SerializeField] Transform m_JointTransform;
	[SerializeField] float m_DistProportionJoint = 0.95f;

	public void SetHeight(float iHeight)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.y = iHeight * 0.5f; // cylinder mesh is 2 units tall
		m_Mesh.transform.localScale = scale;
		Vector3 localPosMesh = m_Mesh.transform.localPosition;
		localPosMesh.y = iHeight * 0.5f;
		m_Mesh.transform.localPosition = localPosMesh;

		Vector3 localPos = m_JointTransform.localPosition;
		localPos.y = iHeight * m_DistProportionJoint;
		m_JointTransform.localPosition = localPos;
	}

	public void SetRadius(float iRadius)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.x = iRadius;
		scale.z = iRadius;
		m_Mesh.transform.localScale = scale;
	}

	public MeshRenderer GetMesh()
	{
		return m_Mesh;
	}

	public Transform GetJoint()
	{
		return m_JointTransform;
	}
}
