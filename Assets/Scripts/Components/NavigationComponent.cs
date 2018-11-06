using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NavigationComponent : NetworkBehaviour
{
    public int _playerID;

    private NavMeshAgent _agent;
    private IInputManager _input;
    private SelectableUnitComponent _selectable;

    #region Navigation Variables

    [Header("Navigation Variables")]
    [SerializeField]
    private Vector3 _endPosition;
    [SerializeField]
    private Vector3 _startPosition;
    [SerializeField]
    private float _accuracy = 5.0f;
    [SerializeField]
    private float _desiredVelocity = 0.0f;
    [SerializeField]
    private float _currentVelocity = 0.0f;
    [SerializeField]
    private float _maxAcceleration = 98.1F;
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
    [SerializeField]
    private Vector3 _heading;
    [SerializeField]
    private Vector3 _targetDirection;

    #endregion

    public float BreakingDistance
    {
        get { return _breakingDistance; }
        set { _breakingDistance = Mathf.Round(value); }
    }

    public float SteeringTargetDistance
    {
        get { return _steeringTargetDistance; }
        set { _steeringTargetDistance = Mathf.Round(value); }
    }

    [Header("State Properties")]
    [SerializeField]
    private bool _isMoving;

    [SyncVar]
    private bool _isSelected;

    private Vector3 _oldTarget;

    // Use this for initialization
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (!_agent)
            throw new MissingComponentException("NavCom missing NavMeshAgent.");

        _selectable = GetComponent<SelectableUnitComponent>();
        if (!_selectable)
            throw new MissingComponentException("NavCom missing SelectableUnitComponent.");
    }

    void Start()
    {
        _input = InputManager.instance;

        _startPosition = transform.position;
        _endPosition = transform.position;
        _heading = Vector3.zero;
        _targetDirection = Vector3.zero;

        _oldTarget = Vector3.one * -1f;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();


    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        _isMoving = _agent.isStopped && _agent.velocity.magnitude > 0;
        _currentVelocity = _agent.velocity.magnitude;
        _isSelected = _selectable.IsSelected;

        if (_isSelected)
        {
            if (_input.GetButtonDown(_playerID, InputAction.Action))
            {
                CastRay();
            }
        }

        if (_agent.pathPending)
        {
            return;
        }

        if (_agent.hasPath)
        {
            if (!Utils.IsValidFloat(_initialDistance) || _initialDistance == 0)
            {
                InitializeNavigationData();
                return;
            }

            Navigate();
        }
    }

    [Command]
    private void CmdSetTarget(Vector3 target)
    {
        RpcSetTarget(target);
    }

    [ClientRpc]
    private void RpcSetTarget(Vector3 target)
    {
        if (_oldTarget != target)
        {
            _agent.SetDestination(target);
            _initialDistance = Utils.IsValidFloat(_agent.remainingDistance) ? _agent.remainingDistance : 0.0f;
            _agent.speed = 0.0f;
            _oldTarget = target;
        }
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag.Equals("NavMeshPlane"))
            {
                CmdSetTarget(hit.point);
                break;
            }
        }
    }

    private void InitializeNavigationData()
    {
        if (Utils.IsValidFloat(_initialDistance) == false)
            _initialDistance = 0.0f;

        if (_initialDistance == 0.0f)
        {
            for (int i = 0; i < _agent.path.corners.Length - 1; i++)
            {
                _initialDistance += Vector3.Distance(_agent.path.corners[i], _agent.path.corners[i + 1]);
            }
        }

        _startPosition = transform.position;
        _endPosition = _agent.destination;

        SteeringTargetDistance = Vector3.Distance(_agent.steeringTarget, _agent.nextPosition);

        _agent.speed = SetTopSpeed(SteeringTargetDistance, _maxAcceleration);
        _agent.isStopped = false;
    }

    private void Navigate()
    {
        _distanceFromStart = Vector3.Distance(_startPosition, transform.position);
        _distanceFromEnd = Vector3.Distance(_endPosition, transform.position);
        _desiredVelocity = _agent.desiredVelocity.magnitude;

        SteeringTargetDistance = Vector3.Distance(_agent.steeringTarget, _agent.nextPosition);
        BreakingDistance = Mathf.Pow(_agent.velocity.magnitude, 2) / (2 * _maxAcceleration);

        _heading = _agent.velocity.normalized;
        _targetDirection = (_agent.steeringTarget - _agent.nextPosition).normalized;

        var angle = Vector3.Angle(_targetDirection, transform.forward);

        if (angle > _accuracy)
        {
            _agent.speed = _accuracy;
        }
        else
        {
            if (_agent.speed <= _accuracy)
            {
                _agent.speed = SetTopSpeed(SteeringTargetDistance, _maxAcceleration);
            }
        }

        if (_agent.steeringTarget != _agent.pathEndPosition)
        {
            if (angle > 175f && _heading.z > 0 && !_agent.isStopped)
            {
                _agent.isStopped = true;
                var courseCorrected = _agent.CalculatePath(_oldTarget, _agent.path);
                if (courseCorrected)
                    Debug.Log("Course corrected.");
                else
                    Debug.Log("Course failed to correct.");
                return;
            }
        }

        if (BreakingDistance >= SteeringTargetDistance)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }

        for (int i = 0; i < _agent.path.corners.Length - 1; i++)
        {
            if ((_agent.path.corners[i + 1] - _agent.nextPosition).sqrMagnitude < (_agent.steeringTarget - _agent.nextPosition).sqrMagnitude)
            {
                var courseCorrected = _agent.CalculatePath(_oldTarget, _agent.path);
                if (courseCorrected)
                    Debug.Log("Course updated."); break;
            }
        }

        if (Vector3.Distance(transform.position, _agent.pathEndPosition) < _accuracy)
        {
            _agent.isStopped = true;
            _agent.speed = 0;
            _endPosition = _agent.pathEndPosition;
            _agent.Warp(_agent.pathEndPosition);
            _agent.ResetPath();

            transform.position = _endPosition;
            _startPosition = transform.position;

            Debug.Log("Agent arrived at destination");
        }
    }

    private float SetTopSpeed(float distance, float acceleration)
    {
        if (!Utils.IsValidFloat(distance))
            distance = Vector3.Distance(_agent.steeringTarget, transform.position);

        var time = 0.0f;
        var speed = 0.0f;

        if (_agent.velocity.magnitude < 1.0f)
        {
            time = distance / (Mathf.Pow(acceleration, 2) * Time.deltaTime);
            speed = time * acceleration;
        }
        else
        {
            time = (distance / 2) / _agent.velocity.magnitude;
            speed = time * acceleration;
        }

        return speed;
    }

    void OnDrawGizmos()
    {
        if (_agent.hasPath)
        {
            for (int i = 0; i < _agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1], Color.red);
                Debug.DrawRay(_agent.path.corners[i + 1], (Vector3.up + Vector3.forward) * 300, Color.red);
            }

            Debug.DrawLine(_agent.nextPosition, _agent.steeringTarget, Color.blue);
            Debug.DrawRay(_agent.steeringTarget, (Vector3.up + Vector3.forward) * 300, Color.blue);
            Debug.DrawRay(_agent.nextPosition, (Vector3.up + Vector3.forward) * 500, Color.magenta);
        }

        Debug.DrawRay(_agent.destination, (Vector3.up + Vector3.forward) * 300, Color.green);

        if (BreakingDistance > 0)
        {
            Debug.DrawRay(transform.position, _agent.velocity.normalized * BreakingDistance, Color.yellow);
        }
    }
}
