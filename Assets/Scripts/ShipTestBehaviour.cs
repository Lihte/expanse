using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Components;

namespace Assets.Scripts
{
    public class ShipTestBehaviour : NetworkBehaviour
    {
        public int _playerId;

        private IInputManager _input;

        public NavMeshAgent agent;

        public GameObject railgunProjectilePrefab;
        public Transform projectileSpawn;
        private const float _projectileSpeed = 3000.0F;

        [SerializeField]
        private Vector3 _endPosition;
        [SerializeField]
        private Vector3 _startPosition;

        public float Velocity;

        [SerializeField]
        private float _desiredVelocity = 0.0f;
        public const float MaxAcceleration = 200F;

        private float _slowingSpeed = 0.175f;

        [SerializeField]
        private Vector2 _heading = Vector2.zero;


        [SerializeField]
        private float _initialDistance = 0.0f;
        [SerializeField]
        private float _distanceFromStart = 0.0f;
        [SerializeField]
        private float _distanceFromEnd = 0.0f;
        [SerializeField]
        private float _breakingDistance = 0.0f;
        [SerializeField]
        private float _steeringTargetDistance = 0.0f;

        private const float _navMeshSampleDistance = 5F;
        private const float _stopDistanceProportion = 0.1F;

        [SerializeField]
        private bool _isMoving;

        public bool handleInput;

        private void Awake()
        {
            //agent = GetComponent<NavMeshAgent>();
        }

        // Use this for initialization
        private void Start()
        {
            _input = InputManager.instance;
            //_startPosition = transform.position;
            //_endPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            //if (!hasAuthority)
            //{
            //    return;
            //}



            //CheckForInput();

            //if (agent.pathPending)
            //{
            //    SetInitialDistance();
            //    return;
            //}

            //Debug.Log("Agent is stopped: " + agent.isStopped.ToString());
            //Debug.Log("Agent has path: " + agent.hasPath.ToString());

            //if (agent.hasPath)
            //{
            //    _desiredVelocity = agent.desiredVelocity.magnitude;
            //    _distanceFromStart = Distance(_startPosition, transform.position);
            //    _distanceFromEnd = Distance(_endPosition, transform.position);

            //    _breakingDistance = Mathf.Pow(agent.velocity.magnitude, 2) / (2 * MaxAcceleration);
            //    _breakingDistance = Mathf.Round(_breakingDistance);

            //    _steeringTargetDistance = (agent.steeringTarget - agent.nextPosition).magnitude;
            //    _steeringTargetDistance = Mathf.Round(_steeringTargetDistance);


            //    // Find out if the unit is very close to the path's starting position
            //    if (Mathf.Approximately((_startPosition - transform.position).sqrMagnitude, 0f))
            //    {
            //        SetDestinationParameters(_steeringTargetDistance);
            //        //agent.isStopped = false;
            //    }

            //    // If the unit is not headed for the endPathPosition, it should be headed for a corner
            //    if (agent.steeringTarget != agent.pathEndPosition)
            //    {
            //        var localHeading = transform.InverseTransformDirection(agent.velocity).normalized;
            //        var targetDirection = (agent.steeringTarget - agent.nextPosition).normalized;

            //        // If the unit passes the corner and moves in a direction away from the corner, it should stop and recalculate its destination
            //        if (targetDirection.magnitude - localHeading.magnitude > 5 * 5 && localHeading.z > 0 && !agent.isStopped)
            //        {
            //            agent.isStopped = true;
            //            ClearDestination();
            //            agent.SetDestination(_endPosition);
            //            SetDestinationParameters(_steeringTargetDistance);
            //            return;
            //        }

            //        // Make sure the unit is not told to stop
            //        agent.isStopped = false;
            //    }

            //    // Start deaccelerating when at half the distance
            //    if (_breakingDistance >= _steeringTargetDistance)
            //    {
            //        agent.isStopped = true;
            //    }

            //    // If unit is not close to the start or end position of its path, yet a low velocity, make sure the unit is not stopped and recalculate the max speed allowed based on the steering target 
            //    if (agent.velocity.magnitude < 10f && _distanceFromEnd > 100.0f && _distanceFromStart > 100.0f)
            //    {
            //        agent.isStopped = false;
            //        float topSpeed = SetTopSpeed(_steeringTargetDistance, MaxAcceleration);
            //        bool result = Utils.IsValidFloat(topSpeed);

            //        topSpeed = result ? topSpeed : Mathf.Pow(MaxAcceleration, 2) * Time.deltaTime;

            //        agent.speed = topSpeed;
            //    }

            //    // If the distance to a target further along the path is closer then the steering target, recalculate the path
            //    for (int i = 0; i < agent.path.corners.Length - 1; i++)
            //    {
            //        if (agent.path.corners[i + 1].sqrMagnitude - agent.nextPosition.sqrMagnitude < agent.steeringTarget.sqrMagnitude - agent.nextPosition.sqrMagnitude)
            //        {
            //            var courseCorrected = agent.CalculatePath(_endPosition, agent.path);
            //            if (courseCorrected)
            //                break; Debug.Log("Course corrected.");
            //        }
            //    }

            //    // Clear variables when at end of path
            //    if (transform.position == agent.pathEndPosition)
            //    {
            //        ClearDestination();
            //        Debug.Log("Agent arrived at destination");
            //    }
            //}

            //Debug.Log("Agent is on nav mesh: " + agent.isOnNavMesh);
        }

        private void OnDrawGizmos()
        {
            //if (agent.hasPath)
            //{
            //    for (int i = 0; i < agent.path.corners.Length - 1; i++)
            //    {
            //        Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
            //    }

            //    Debug.DrawLine(agent.nextPosition, agent.steeringTarget, Color.blue);
            //}

            //if (_breakingDistance > 0)
            //{
            //    Debug.DrawRay(transform.position, agent.velocity.normalized * _breakingDistance, Color.yellow);
            //}
        }

        void SetDestinationParameters(float distance, float speed = MaxAcceleration)
        {
            //_endPosition = agent.destination;
            //agent.acceleration = MaxAcceleration;
            //agent.speed = SetTopSpeed(distance, speed);
        }

        void ClearDestination()
        {
            //_startPosition = transform.position;
            //_endPosition = transform.position;

            //_initialDistance = 0.0f;

            //agent.acceleration = 0.0f;
            //agent.speed = 0.0f;
            //agent.ResetPath();
        }

        void SetTopSpeed(float distance, float acceleration)
        {
            //if (!Utils.IsValidFloat(distance))
            //{
            //    distance = Distance(agent.steeringTarget, transform.position);
            //}

            //if(agent.velocity.magnitude >= 1.0f)
            //{
            //    var time = (distance / 2) / agent.velocity.magnitude;
            //    var speed = (time != float.NaN && !float.IsInfinity(time) ? time : 1.0f) * acceleration;
            //    return speed;
            //}

            //return distance * (Mathf.Pow(MaxAcceleration, 2) * Time.deltaTime);
        }

        float Distance(Vector3 start, Vector3 end)
        {
            return (end - start).magnitude;
        }

        public void CheckForInput()
        {
            //Vector3 debugPos = new Vector3();

            //if (_input.GetButtonDown(_playerId, InputAction.Action))
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;

            //    if (Physics.Raycast(ray, out hit))
            //    {
            //        ClearDestination();
            //        agent.isStopped = !agent.SetDestination(hit.point);
            //        SetDestinationParameters(0, 0);
            //        debugPos = hit.point;
            //        _startPosition = transform.position;
            //    }
            //}

            //if(_input.GetButtonDown(_playerId, InputAction.Select))
            //{
            //    CmdFireRailgun();
            //}
            //Debug.DrawRay(debugPos, (Vector3.up + Vector3.forward) * 100, Color.red, 5f, false);
            //Debug.DrawRay(agent.destination, (Vector3.up + Vector3.forward) * 100, Color.green, Time.deltaTime, false);
        }

        private void SetInitialDistance()
        {
            //if(!Utils.IsValidFloat(_initialDistance))
            //{
            //    _initialDistance = 0.0f;
            //}

            //for (int i = 0; i < agent.path.corners.Length - 1; i++)
            //{
            //    _initialDistance += (agent.path.corners[i] - agent.path.corners[i + 1]).magnitude;
            //}

            //return Utils.IsValidFloat(_initialDistance);
        }

        //[Command]
        //private void CmdFireRailgun()
        //{
        //    var projectile = (GameObject)Instantiate (
        //        railgunProjectilePrefab, 
        //        projectileSpawn.position, 
        //        projectileSpawn.rotation);

        //    projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _projectileSpeed;

        //    NetworkServer.Spawn(projectile);

        //    Destroy(projectile, 10.0f);
        //}
    }
}