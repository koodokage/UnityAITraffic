using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Traffix
{
#if UNITY_EDITOR
    [CustomEditor(typeof(VehicleTrafficManager))]
    public class VehicleTrafficManagerHelper : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VehicleTrafficManager TM = (VehicleTrafficManager)target;
            if (GUILayout.Button("Create Waypoint"))
            {
                TM.CreateWaypoint();
            }
        }
    }
#endif

    public class VehicleTrafficManager : MonoBehaviour
    {
        [SerializeField] GameObject prefab_AI;
        [SerializeField] Transform Root;
        [SerializeField] int AIAmount = 10;

        IEnumerator Start()
        {
            while (AIAmount > 0)
            {
                VehicleWayNode node = Root.GetChild(Random.Range(0, Root.childCount - 1)).GetComponent<VehicleWayNode>();
                AICarMovement movementSubject = Instantiate(prefab_AI).GetComponent<AICarMovement>();
                movementSubject.transform.position = node.transform.position;

                yield return new WaitForEndOfFrame();
                AIAmount--;
            }

        }

#if UNITY_EDITOR
        public void CreateWaypoint()
        {
            Transform WayRoot = transform;
            GameObject point = new GameObject("Start_VehicleWNODE:" + WayRoot.childCount, typeof(VehicleWayNode));
            point.transform.SetParent(transform);

            VehicleWayNode wayPoint = point.GetComponent<VehicleWayNode>();
          
            Selection.activeGameObject = wayPoint.gameObject;
        }

#endif

    }

}

