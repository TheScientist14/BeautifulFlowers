//  A simple Unity C# script for orbital movement around a target gameobject
//  Original from https://gist.github.com/3dln/c16d000b174f7ccf6df9a1cb0cef7f80

using System;
using UnityEngine;

public class OrbitCameraBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject m_Target;
	[SerializeField] private float m_Distance = 10.0f;

	[SerializeField] private float m_XSpeed = 250.0f;
	[SerializeField] private float m_YSpeed = 120.0f;

	[SerializeField] private float m_MinY = -20;
	[SerializeField] private float m_MaxY = 80;

	private float m_CurAngleX = 0.0f;
	private float m_CurAngleY = 0.0f;

	private float m_PrevDistance;

	void Awake()
	{
		Vector3 angles = transform.eulerAngles;
		m_CurAngleX = angles.y;
		m_CurAngleY = angles.x;

		_UpdateTransform();
	}

	void Update()
	{
		if(!m_Target)
			return;

		m_Distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
		m_Distance = Mathf.Clamp(m_Distance, 2, 40);

		if(Input.GetMouseButton(1))
		{
			if(Input.GetMouseButtonDown(1))
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}

			m_CurAngleX += Input.GetAxis("Mouse X") * m_XSpeed * 0.02f;
			m_CurAngleY -= Input.GetAxis("Mouse Y") * m_YSpeed * 0.02f;

			m_CurAngleY = ClampAngle(m_CurAngleY, m_MinY, m_MaxY);

			_UpdateTransform();
		}
		else
		{
			if(Input.GetMouseButtonUp(1))
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		if(Math.Abs(m_PrevDistance - m_Distance) > 0.001f)
		{
			m_PrevDistance = m_Distance;
			_UpdateTransform();
		}
	}

	private void _UpdateTransform()
	{
		Quaternion rotation = Quaternion.Euler(m_CurAngleY, m_CurAngleX, 0);
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -m_Distance) + m_Target.transform.position;
		transform.rotation = rotation;
		transform.position = position;
	}

	static float ClampAngle(float iAngle, float iMin, float iMax)
	{
		if(iAngle < -360)
			iAngle += 360;
		if(iAngle > 360)
			iAngle -= 360;
		return Mathf.Clamp(iAngle, iMin, iMax);
	}
}