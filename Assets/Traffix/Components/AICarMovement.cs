using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Traffix
{
    public class AICarMovement : MonoBehaviour
    {
        VehicleWayNode _currentNode;

        private void Start() {
            Assert.IsNotNull(_currentNode);
        }


        public void Set(VehicleWayNode node){
            _currentNode = node;
        }


        
    }

}