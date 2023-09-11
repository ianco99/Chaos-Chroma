using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Code.FieldOfView
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, -Vector3.forward, Vector3.right, 360, fov.viewRadius);
        }
    }
}

