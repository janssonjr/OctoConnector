using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{

	public Shape shape;
	public GameObject blastParticles;

	public void MovedToTarget()
	{
		//shape.moveComplete();
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = new Vector3(0.08f, 0.08f, 1f);
		blastParticles.SetActive(false);
	}

	public void MoveToTargetDirty()
	{

	}

	public void StartFly()
	{
		shape.StartFly();
	}

}
