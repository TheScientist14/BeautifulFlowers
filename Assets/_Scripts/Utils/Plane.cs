using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	[SerializeField] MeshRenderer m_Mesh;
	[SerializeField] MeshRenderer m_BackFaceMesh;
	[SerializeField] Transform m_JointTransform;
	[SerializeField] float m_DistProportionJoint = 0.95f;

	public void SetLength(float iLength)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.y = iLength; // quads are aligned with x/y plane
		m_Mesh.transform.localScale = scale;
		m_BackFaceMesh.transform.localScale = scale;
		Vector3 localPosMesh = m_Mesh.transform.localPosition;
		localPosMesh.x = iLength * 0.5f;
		m_Mesh.transform.localPosition = localPosMesh;
		m_BackFaceMesh.transform.localPosition = localPosMesh;

		if(m_JointTransform != null)
		{
			Vector3 localPos = m_JointTransform.localPosition;
			localPos.x = iLength * m_DistProportionJoint;
			m_JointTransform.localPosition = localPos;
		}
	}

	public void SetWidth(float iWidth)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.x = iWidth;
		m_Mesh.transform.localScale = scale;
		m_BackFaceMesh.transform.localScale = scale;
	}

	public void SetMaterial(Material iMaterial)
	{
		m_Mesh.material = iMaterial;
		m_BackFaceMesh.material = iMaterial;
	}

	public void SetTexture(Texture iTexture)
	{
		m_Mesh.material.mainTexture = iTexture;
		m_BackFaceMesh.material.mainTexture = iTexture;
	}

	public void SetColor(Color iColor)
	{
		m_Mesh.material.color = iColor;
		m_BackFaceMesh.material.color = iColor;
	}

	public Transform GetJoint()
	{
		return m_JointTransform;
	}
}
