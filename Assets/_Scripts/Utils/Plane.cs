using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	[SerializeField] MeshRenderer m_Mesh;
	[SerializeField] MeshRenderer m_BackFaceMesh;
	[SerializeField] Transform m_JointTransform;
	[SerializeField] Transform m_OffsetTransform;
	[SerializeField] float m_DistProportionJoint = 1f;

	public void SetLength(float iLength)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.y = iLength; // quads are aligned with x/y plane
		m_Mesh.transform.localScale = scale;
		scale.z *= -1;
		m_BackFaceMesh.transform.localScale = scale;

		// if no joint, assuming plane is centered, no position adjustement done
		if(m_JointTransform != null)
		{
			Vector3 localPosMesh = m_Mesh.transform.localPosition;
			localPosMesh.x = iLength * 0.5f;
			m_Mesh.transform.localPosition = localPosMesh;
			m_BackFaceMesh.transform.localPosition = localPosMesh;

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
		scale.z *= -1;
		m_BackFaceMesh.transform.localScale = scale;
	}

	public void SetOffset(float iOffset)
	{
		if(m_OffsetTransform != null)
			m_OffsetTransform.transform.localPosition = Vector3.right * iOffset;
	}

	public void SetRotation(Quaternion iRotation)
	{
		if(m_OffsetTransform != null)
			m_OffsetTransform.transform.localRotation = iRotation;
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
