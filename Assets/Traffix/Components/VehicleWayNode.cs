using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Traffix
{

#if UNITY_EDITOR
    [CustomEditor(typeof(VehicleWayNode))]
    public class VehicleWayNodelper : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VehicleWayNode WN = (VehicleWayNode)target;
            if (GUILayout.Button("Create Next Point"))
            {
                WN.CreateNextPoint();
            }

        }
    }
#endif

    public class VehicleWayNode : MonoBehaviour
    {
       [SerializeField] private VehicleWayNode next;
        public List<VehicleWayNode> branches;

        public VehicleWayNode Next { get => next; private set => next = value; }

        public void GetNext(out VehicleWayNode node){
            int brancheCount = branches.Count;
            if(brancheCount >0){
                
                node = branches[UnityEngine.Random.Range(0,brancheCount)];
                return;
            }

            node = Next;
        }


#if UNITY_EDITOR

        internal void CreateNextPoint()
        {
            Transform WayRoot = transform.parent;
            GameObject point = new GameObject("VehicleWNode" + WayRoot.childCount, typeof(VehicleWayNode));
            point.transform.SetParent(WayRoot);
            VehicleWayNode waypoint = point.GetComponent<VehicleWayNode>();
            VehicleWayNode selectedWayPoint = Selection.activeGameObject.GetComponent<VehicleWayNode>();
            point.transform.position = selectedWayPoint.transform.position;
            point.transform.forward = selectedWayPoint.transform.forward;

            selectedWayPoint.Next = waypoint;

            waypoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());
            Selection.activeGameObject = point;
        }
#endif
    }
}
