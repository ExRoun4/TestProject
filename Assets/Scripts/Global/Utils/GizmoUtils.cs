using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoUtils : MonoBehaviour
{
    public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowLength = 1.0f, float arrowAngle = 20.0f)
	{
		Gizmos.DrawRay(pos, direction);
		
        Vector3 forward = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowAngle, 180, 0) * new Vector3(0,0,1);
        Vector3 back = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowAngle, 180, 0) * new Vector3(0,0,1);
		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowAngle, 0) * new Vector3(0,0,1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowAngle, 0) * new Vector3(0,0,1);

        Gizmos.DrawRay(pos, direction * arrowLength);
        Gizmos.DrawRay(pos + direction * arrowLength, forward);
        Gizmos.DrawRay(pos + direction * arrowLength, back);
		Gizmos.DrawRay(pos + direction * arrowLength, right);
		Gizmos.DrawRay(pos + direction * arrowLength, left);
	}
}
