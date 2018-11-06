using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class UnitController : PhysicalEntity
    {
        //public Vector3 TargetVector;
        //public Vector3 TargetPosition;
        
        //public Vector2 CurrentVelocity
        //{
        //    get { return _rigidBody.velocity; }
        //    set { _rigidBody.velocity = value; }
        //}

        //private Vector2 _previousVelocity;

        //private Vector2 _targetDir;

        //private Vector3 _lastTarget;

        //private Vector3 _startPosition;
        //private Vector3 _endPosition;

        //private float _lastAngle = 0;

        public Camera playerCamera;
        public NavMeshAgent agent;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            playerCamera = Camera.main;

            //TargetPosition = Vector3.zero;
            //TargetVector = Vector3.zero;
        }

        // Update is called once per frame
       void Update()
       {
           if (!isLocalPlayer)
               return;

           if (Input.GetAxis("Action") == 1)
           {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }

               //TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               //TargetVector = TargetPosition - transform.position;
               //TargetVector.z = 0;

               //_previousVelocity = CurrentVelocity;

               //_startPosition = transform.position;
               //_endPosition = transform.position + TargetVector;
           }

           //_targetDir = Heading(TargetVector);
       }

        private void FixedUpdate()
        {
            //if(TargetVector != Vector3.zero)
            //{ 
            //    Accelerate(_targetDir, 2, 2);
            //    Rotate();
            //}
        }

        public override void OnStartLocalPlayer()
        {
        }

        //public Vector2 Heading(Vector2 target)
        //{
        //    var heading = target - new Vector2(transform.position.x, transform.position.y);
        //    var distance = heading.magnitude;

        //    var direction = distance != 0 ? heading / distance : new Vector2();

        //    return direction;
        //}

        //public Vector2 Accelerate(Vector2 direction, float currentAcceleration, float maxAcceleration, float t = 1.0F)
        //{

        //    var forceIncrement = direction * Mathf.Lerp(currentAcceleration, maxAcceleration, t);
        //    _rigidBody.AddForce(forceIncrement);
        //    return forceIncrement;
        //}

        //Vector2 ShipRotation()
        //{
        //    var angle = GetAngleInDegrees(CurrentVelocity.x, CurrentVelocity.y);
        //    if (CurrentVelocity == Vector2.zero)
        //        angle = _lastAngle;

        //    return Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;
        //}

        //void Rotate()
        //{
        //    var angle = GetAngleInDegrees(CurrentVelocity.x, CurrentVelocity.y);
        //    if (CurrentVelocity == Vector2.zero)
        //        angle = _lastAngle;

        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}

        //float GetAngleInDegrees(float x, float y)
        //{
        //    return Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90.0F;
        //}
    }
}