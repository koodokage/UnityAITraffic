
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Traffix
{

#if UNITY_EDITOR
    [CustomEditor(typeof(HumanWayNode))]
    public class WayNodeHelper : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HumanWayNode WN = (HumanWayNode)target;
            if (GUILayout.Button("Create Before"))
            {
                WN.CreateWaypointBeforeSelected();
            }
            if (GUILayout.Button("Create After"))
            {
                WN.CreateWaypointAfterSelected();
            }
            if (GUILayout.Button("Create Bridge"))
            {
                // WN.CreateBridge();
            }
            if (GUILayout.Button("Create Wait"))
            {
                // WN.CreateWait();
            }
            if (GUILayout.Button("Remove"))
            {
                WN.RemoveWaypoint();
            }
        }
    }
#endif

    public class HumanWayNode : MonoBehaviour
    {
        [SerializeField] private HumanWayNode prev;
        [SerializeField] private HumanWayNode next;
        public List<HumanWayNode> bridges;
        [Range(0, 1)] public float BridgeUsageChance = .5f;

        public Vector3 GetPointLocation
        {
            get
            {
                Vector3 RightLine = transform.position + (transform.right * transform.localScale.x * .5f);
                Vector3 LeftLine = transform.position + (-transform.right * transform.localScale.x * .5f);
                float interpolationValue = Random.Range(0.0f, 1.0f);
                Vector3 location = Vector3.Lerp(LeftLine, RightLine, interpolationValue);
                return location;

            }
        }

        public HumanWayNode Prev { get => prev; private set => prev = value; }
        public HumanWayNode Next { get => next; private set => next = value; }
        public HumanWayNode SetPrev {  set => prev = value; }
        public HumanWayNode SetNext {  set => next = value; }

        public bool TryUsingBridgeNode(out HumanWayNode bridge)
        {
            bridge = null;
            bool result = false;

            if (bridges.Count > 0)
            {
                float ratio = Random.Range(0, 1);
                result = ratio <= BridgeUsageChance; // simple chance
                if (result)
                {
                    bridge = bridges[Random.Range(0, bridges.Count - 1)];
                }
            }

            return result;
        }



        public void CreateBridge()
        {
            if (Selection.activeGameObject == null)
                return;


            Transform WayRoot = transform.parent;
            HumanWayNode selectedWayPoint = Selection.activeGameObject.GetComponent<HumanWayNode>();

            if (selectedWayPoint == null)
            {
                Debug.LogWarning("Select A Way Point");
                return;
            }

            GameObject point = new GameObject("HumanWBridge-" + selectedWayPoint.transform.name, typeof(HumanWayNode));
            HumanWayNode bridge = point.GetComponent<HumanWayNode>();
            point.transform.SetParent(WayRoot);

            selectedWayPoint.bridges.Add(bridge);

            bridge.transform.position = selectedWayPoint.transform.position;
            bridge.transform.forward = selectedWayPoint.transform.forward;

            Selection.activeGameObject = bridge.gameObject;
        }




        public void CreateWaypointBeforeSelected()
        {
            Transform WayRoot = transform.parent;
            GameObject point = new GameObject("HumanWNode" + WayRoot.childCount, typeof(HumanWayNode));
            point.transform.SetParent(WayRoot);
            HumanWayNode waypoint = point.GetComponent<HumanWayNode>();
            HumanWayNode selectedWayPoint = Selection.activeGameObject.GetComponent<HumanWayNode>();
            point.transform.position = selectedWayPoint.transform.position;
            point.transform.forward = selectedWayPoint.transform.forward;

            if (selectedWayPoint.Prev != null)
            {
                waypoint.Prev = selectedWayPoint.Prev;
                selectedWayPoint.Prev.Next = waypoint;
            }

            waypoint.Next = selectedWayPoint;
            selectedWayPoint.Prev = waypoint;

            waypoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());
            Selection.activeGameObject = point;
        }

        public void CreateWaypointAfterSelected()
        {
            Transform WayRoot = transform.parent;
            GameObject point = new GameObject("HumanWNode" + WayRoot.childCount, typeof(HumanWayNode));
            point.transform.SetParent(WayRoot);
            HumanWayNode waypoint = point.GetComponent<HumanWayNode>();
            HumanWayNode selectedWayPoint = Selection.activeGameObject.GetComponent<HumanWayNode>();
            point.transform.position = selectedWayPoint.transform.position;
            point.transform.forward = selectedWayPoint.transform.forward;

            if (selectedWayPoint.Next != null)
            {
                selectedWayPoint.Next.Prev = waypoint;
                waypoint.Next = selectedWayPoint.Next;
            }

            selectedWayPoint.Next = waypoint;
            waypoint.Prev = selectedWayPoint;

            waypoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());
            Selection.activeGameObject = point;

        }

        public void RemoveWaypoint()
        {
            HumanWayNode selected = Selection.activeGameObject.GetComponent<HumanWayNode>();

            if (selected.Next != null)
            {
                selected.Next.Prev = selected.Prev;
            }
            if (selected.Prev != null)
            {
                selected.Prev.Next = selected.Next;
                Selection.activeGameObject = selected.Prev.gameObject;
            }

            DestroyImmediate(selected.gameObject);
        }

    }

}