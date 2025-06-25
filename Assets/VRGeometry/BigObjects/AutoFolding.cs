using System.Collections.Generic;
using UnityEngine;

public class AutoFolding : MonoBehaviour
{
	private int velocityAdjust = 10;

	void LateUpdate()
	{
		List<HingeJoint> hingeJoints = new List<HingeJoint>();

		foreach (Transform child in this.transform)
		{
			if (child.GetComponent<HingeJoint>() != null)
			{
				HingeJoint hingeJoint = child.GetComponent<HingeJoint>();

				if (hingeJoint.motor.targetVelocity > 300)
				{
					velocityAdjust = -10;
				}
				else if (hingeJoint.motor.targetVelocity < -100)
				{
					velocityAdjust = 10;
				}

				JointMotor motor = new JointMotor()
				{
					force = hingeJoint.motor.force,
					targetVelocity = hingeJoint.motor.targetVelocity + (velocityAdjust*Time.deltaTime),
				};
				hingeJoint.motor = motor;
			}

		}
	}
}
