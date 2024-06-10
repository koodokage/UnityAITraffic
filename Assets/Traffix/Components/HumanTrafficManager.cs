using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Traffix
{    
     #if UNITY_EDITOR
    [CustomEditor(typeof(HumanTrafficManager))]
    public class TrafficManagerHelper : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            HumanTrafficManager TM =(HumanTrafficManager)target; 
            if (GUILayout.Button("Create Waypoint"))
            {
                TM.CreateWaypoint();
            }
        }
    }
    #endif

    public class HumanTrafficManager : MonoBehaviour
    {
        [SerializeField] GameObject prefab_AI;
        [SerializeField] Transform Root;
        [SerializeField] int AIAmount = 10;

        IEnumerator Start()
        {
            while (AIAmount > 0)
            {
                HumanWayNode node = Root.GetChild(Random.Range(0, Root.childCount - 1)).GetComponent<HumanWayNode>();
                AIMovement movementSubject = Instantiate(prefab_AI).GetComponent<AIMovement>();
                movementSubject.Set(node);
                movementSubject.Teleport(node.GetPointLocation);
                
                yield return new WaitForEndOfFrame();
                AIAmount--;
            }

        }

        #if UNITY_EDITOR
        public void CreateWaypoint()
        {
            Transform WayRoot = transform;
            GameObject point = new GameObject("Start_HumanWNODE:" + WayRoot.childCount, typeof(HumanWayNode));
            point.transform.SetParent(transform);

            HumanWayNode wayPoint = point.GetComponent<HumanWayNode>();
            if (WayRoot.childCount > 1)
            {
                wayPoint.SetPrev = WayRoot.GetChild(WayRoot.childCount - 2).GetComponent<HumanWayNode>();
                wayPoint.Prev.SetNext = wayPoint;

                wayPoint.transform.position = wayPoint.Prev.transform.position;
                wayPoint.transform.forward = wayPoint.Prev.transform.forward;
            }

            Selection.activeGameObject = wayPoint.gameObject;
        }

        #endif

    }

}