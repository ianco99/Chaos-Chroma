using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Code.FOV
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, -Vector3.forward, Vector3.right, 360, fov.viewRadius);
            Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2.0f, false);
            Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2.0f, false);

            Vector3 rotatedAngleA = Quaternion.Euler(new Vector3(0.0f, 90.0f, 90.0f)) * viewAngleA ;
            Vector3 rotatedAngleB = Quaternion.Euler(new Vector3(0.0f, 90.0f, 90.0f)) * viewAngleB;

            Handles.DrawLine(fov.transform.position, fov.transform.position + rotatedAngleA * fov.viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + rotatedAngleB * fov.viewRadius);

            Handles.color = Color.red;
            foreach (Transform visibleTarget in fov.visibleTargets)
            {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }
        }
    }
}

