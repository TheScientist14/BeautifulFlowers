using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	[SerializeField] MeshRenderer m_Mesh;
	[SerializeField] Transform m_JointTransform;
	[SerializeField] float m_DistProportionJoint = 0.95f;

	public void SetLength(float iLength)
	{
		Vector3 scale = m_Mesh.transform.localScale;
		scale.x = iLength * 0.1f; // plane mesh is 10 units long
		m_Mesh.transform.localScale = scale;
		Vector3 localPosMesh = m_Mesh.transform.localPosition;
		localPosMesh.x = iLength * 0.5f;
		m_Mesh.transform.localPosition = localPosMesh;

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
		scale.z = iWidth;
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
