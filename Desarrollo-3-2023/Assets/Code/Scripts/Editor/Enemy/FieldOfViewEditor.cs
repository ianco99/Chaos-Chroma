using UnityEditor;
using UnityEngine;

namespace Code.FOV
{
    /// <summary>
    /// Editor class for the FieldOfView component.
    /// </summary>
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        /// <summary>
        /// Draws the field of view visualization in the Scene view for the selected FieldOfView component.
        /// </summary>
        /// <remarks>
        /// This method uses Unity's Handles to draw the view radius as a wire arc and the view angles as lines.
        /// It also draws lines to each visible target within the field of view.
        /// </remarks>
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, -Vector3.forward, Vector3.right, 360, fov.viewRadius);
            Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2.0f, false);
            Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2.0f, false);

            Vector3 rotatedAngleA = Quaternion.Euler(new Vector3(0.0f, 90.0f, 90.0f)) * viewAngleA;
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