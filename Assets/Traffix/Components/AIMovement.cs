
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace Traffix
{
    public class AIMovement : MonoBehaviour
    {
        enum MoveState { MOVE, WAIT }
        [SerializeField] CharacterController controllerComponent;
        [SerializeField, Range(1, 5)] float movementSpeedMax = 3.5f;
        [SerializeField, Range(1, 5)] float movementSpeedMin = 1.5f;
        [SerializeField] float rotationSpeed = .15f;
        float _movementSpeed = 3;
        enum NodeIterationDirection { TOEND, TOSTART }
        int _nodeDirection = 0;
        float _velocityMag = 0;
        Vector3 _destination;
        float _distanceToTarget = 0;
        HumanWayNode _currentNode;
        Vector3 _rotationVelocity;
        MoveState _currentMoveState;

        private void Start()
        {
            _currentMoveState = MoveState.MOVE;
            _movementSpeed = Random.Range(movementSpeedMin, movementSpeedMax);
            _nodeDirection = Random.Range((int)NodeIterationDirection.TOEND, (int)NodeIterationDirection.TOSTART + 1);

            Assert.IsNotNull(_currentNode);
        }

        public void Set(HumanWayNode startNode)
        {
            _currentNode = startNode;
            _destination = _currentNode.GetPointLocation;
        }

        public void Teleport(Vector3 targetLocation)
        {
            controllerComponent.enabled = false;
            controllerComponent.transform.position = targetLocation;
            controllerComponent.enabled = true;
        }

        public bool AnyBlockedWay()
        {
            if (_nodeDirection == (int)NodeIterationDirection.TOEND)
            {
                if (_currentNode.Next == null)
                {

                    var prevNode = _currentNode.Prev;
                    _nodeDirection = (int)NodeIterationDirection.TOSTART;
                    Set(prevNode);
                    return true;
                }
            }

            if (_nodeDirection == (int)NodeIterationDirection.TOSTART)
            {
                if (_currentNode.Prev == null)
                {

                    var nexttNode = _currentNode.Next;
                    _nodeDirection = (int)NodeIterationDirection.TOEND;
                    Set(nexttNode);
                    return true;
                }
            }

            return false;
        }



        private void Update()
        {
            if (_currentMoveState == MoveState.WAIT)
                return;

            _velocityMag = controllerComponent.velocity.sqrMagnitude;

            // check bridge condidition
            if (_currentNode.TryUsingBridgeNode(out HumanWayNode node))
            {
                Set(node);
            }

            RotateBody();
            if (TryExecuteMovementCommand() == false)
            {
                if (AnyBlockedWay())
                    return;

                if (_nodeDirection == 0)
                {
                    Set(_currentNode.Next);
                }
                else
                {
                    Set(_currentNode.Prev);
                }

            }

    
        }

        private bool TryExecuteMovementCommand()
        {
            _distanceToTarget = (_destination - transform.position).sqrMagnitude;
            bool _destinationReached = _distanceToTarget < 1.5f;
            if (_destinationReached == false)
            {
                // heading calc
                Vector3 heading = _destination - transform.position;
                var distance = heading.magnitude;
                var direction = heading / distance;

                // maneuvre
                if (_velocityMag < 1f)
                {
                    direction.x += _movementSpeed;
                }

                direction.y = 0;
                controllerComponent.Move(direction * Time.deltaTime * _movementSpeed);

                return true;
            }

            return false;
        }

        void RotateBody()
        {
            // rotation
            Vector3 bodyVelocity = controllerComponent.velocity;
            bodyVelocity.y = 0;
            transform.forward = Vector3.SmoothDamp(transform.forward, bodyVelocity.normalized, ref _rotationVelocity, rotationSpeed);
        }
    }
}
