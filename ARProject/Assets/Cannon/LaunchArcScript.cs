using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcScript : MonoBehaviour
{
	LineRenderer lr;

	public float velocity;
	float radianAngle;
	float g; // force of gravity on y axis
	float maxTime = 10.0f;
	float timeRes = 0.02f;
	
	void Awake()
	{
		lr = GetComponent<LineRenderer>();
		g = Mathf.Abs(Physics.gravity.y);
	}

	public void DeleteArc()
	{
		lr.positionCount = 0;
		lr.SetPositions(new Vector3[]{});
	}
	
	public void RenderArc(float ang)
	{	
		
		radianAngle = Mathf.Deg2Rad * ang;
		lr.positionCount = (int)(maxTime/timeRes);
		setAccordingToTime();
	}

	public void setAccordingToTime()
	{	
		float vz = velocity * Mathf.Cos(radianAngle);
		float vy = velocity * Mathf.Sin(radianAngle);
		int index = 1;
		Vector3 currentPosition = new Vector3(0,0,0);
		lr.SetPosition(0,currentPosition);
		for (float i = timeRes	; i < maxTime; i += timeRes)
		{
			float tempz = vz * i;
			float tempy = ((-0.5f) * g * i * i) + (vy * i);	
			currentPosition = new Vector3(0,tempy,tempz);
			lr.SetPosition(index,currentPosition);
			index++;
		}  
	}
}
