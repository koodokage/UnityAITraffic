using System.Collections;
using System.Collections.Generic;
using Traffix;
using UnityEditor;
using UnityEngine;

namespace Traffix.Editor
{

    [InitializeOnLoad()]
    public class EditorWayNode
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(HumanWayNode node, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(node.transform.position, Vector3.one * .3f);
            }
            else
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(node.transform.position, .3f);

            }

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(node.transform.position + (node.transform.right * node.transform.localScale.x  / 2f),
            node.transform.position - (node.transform.right * node.transform.localScale.x  / 2f));

            if (node.Prev != null)
            {
                Gizmos.color = Color.white;
                Vector3 prevOffset = node.transform.right * node.transform.localScale.x  / 2f;
                Vector3 prevOffsetRight = node.Prev.transform.right * node.transform.localScale.x  / 2f;
                Gizmos.DrawLine(node.transform.position + prevOffset, node.Prev.transform.position + prevOffsetRight);
         
            if(node.Prev.bridges != null)
                if (node.Prev.bridges.Count > 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.transform.position, node.Prev.transform.position);
                }
            }

            if (node.Next != null)
            {
                Gizmos.color = Color.white;
                Vector3 nextOffset = node.transform.right * -node.transform.localScale.x  / 2f;
                Vector3 offsetTo = node.Next.transform.right * -node.Next.transform.localScale.x  / 2f;
                Gizmos.DrawLine(node.transform.position + nextOffset, node.Next.transform.position + offsetTo);
            }

            if (node.bridges != null)
            {
                foreach (var bridge in node.bridges)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(node.transform.position, bridge.transform.position);
                }
            }

        }


             [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(VehicleWayNode node, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(node.transform.position, Vector3.one * .3f);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node.transform.position, .3f);

            }

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(node.transform.position + (node.transform.right * node.transform.localScale.x  / 2f),
            node.transform.position - (node.transform.right * node.transform.localScale.x  / 2f));


            if (node.Next != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(node.transform.position, node.Next.transform.position);
            }
        }
            
        
    }
}