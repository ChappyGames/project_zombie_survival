using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

    private void OnSceneGUI() {
        FieldOfView lFov = (FieldOfView)target;
        Handles.color = Color.white;
        
        Vector3 lViewAngleA = lFov.GetVectorFromAngle(-lFov.viewAngle / 2, false);
        Vector3 lViewAngleB = lFov.GetVectorFromAngle(lFov.viewAngle / 2, false);

        Handles.DrawWireArc(lFov.transform.position, Vector3.up, lViewAngleA, lFov.viewAngle, lFov.viewRadius);

        Handles.DrawLine(lFov.transform.position, lFov.transform.position + lViewAngleA * lFov.viewRadius);
        Handles.DrawLine(lFov.transform.position, lFov.transform.position + lViewAngleB * lFov.viewRadius);

        Handles.color = Color.red;
        foreach(Transform aVisibleTarget in lFov.visibleTargets) {
            Handles.DrawLine(lFov.transform.position, aVisibleTarget.position);
        }
    }

}
