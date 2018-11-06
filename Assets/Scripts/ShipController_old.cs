//using System.Collections;

//using UnityEngine.EventSystems;
//using UnityEngine;
//using Assets.Scripts.Interfaces;
//using System;

//namespace Assets.Scripts
//{
//    public class ShipController_old : MonoBehaviour, IKinetic, ISelectHandler, IDeselectHandler, IPointerDownHandler
//    {

//        public Texture2D Texture;

//        private Rigidbody2D _rigidBody;
//        private Sprite _sprite;
//        private SpriteRenderer _sr;

//        public Vector3 TargetVector;
//        public Vector3 TargetPosition
//        {
//            get
//            {
//                try
//                {
//                    return TargetPosition;
//                }
//                catch(StackOverflowException soex)
//                {
//                    throw new StackOverflowException(soex.Message, soex);
//                }
//            }
//            set
//            {
//                _lastTarget = TargetPosition;
//                TargetPosition = value;
//            }
//        }


//        private Vector3 _lastTarget;

//        private Vector3 _startPosition;
//        private Vector3 _endPosition;

//        public float RotationSpeed;

//        [SerializeField]
//        private Vector2 _velocity;

//        public Vector2 CurrentVelocity
//        {
//            get { return _rigidBody.velocity; }
//            set { _rigidBody.velocity = value; }
//        }

//        [SerializeField]
//        private Vector2 _moveDir;

//        public Vector2 MovingDirection
//        {
//            get { return _rigidBody.velocity.normalized; }
//        }

//        public Vector2 TargetDirection
//        {
//            get { return TargetDirection; }
//            set { TargetDirection = value; }
//        }

//        private float _lastAngle = 0;

//        public float TargetDistance;

//        public const float MaxAcceleration = 9.81F;
//        public float CurrentAcceleration = 0;


//        public bool IsSelected;

//        public bool IsMoving
//        {
//            get
//            {
//                return CurrentVelocity.magnitude > 0.01F;
//            }
//        }

//        public bool IsMovingMore
//        {
//            get
//            {
//                return CurrentVelocity.magnitude > 1.0F;
//            }
//        }

//        private Vector2 _previousVelocity;

//        [SerializeField]
//        private bool _isRotating;
//        [SerializeField]
//        private Quaternion _lookRotation;
//        public Vector2 _targetDir;

//        private BoxCollider2D _collider;

//        void Awake()
//        {
//            _sr = gameObject.AddComponent<SpriteRenderer>();
//            _rigidBody = gameObject.AddComponent<Rigidbody2D>();
//            _collider = gameObject.AddComponent<BoxCollider2D>();
//        }

//        // Use this for initialization
//        void Start()
//        {

//            _sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, 125.0f, 311.0f), new Vector2(0.5f, 0.5f), 100.0f);
//            _rigidBody.gravityScale = 0.0f;

//            _sr.color = Color.yellow;
//            _sr.sprite = _sprite;

//            _collider.size = _sprite.bounds.size;

//            _startPosition = transform.position;
//            TargetPosition = Vector3.zero;

//            IsSelected = false;
//        }

//        bool move = false;

//        void FixedUpdate()
//        {
//            if (move)
//            {
//                Move2Vector();
//                Rotate();
//            }
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                TargetVector = Vector3.zero;
//                _startPosition = Vector3.zero;
//                _endPosition = Vector3.zero;
//                _targetDir = Vector2.zero;

//                TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                TargetVector = TargetPosition - transform.position;
//                TargetVector.z = 0;

//                _previousVelocity = CurrentVelocity;

//                if(IsMoving)
//                {

//                }

//                _startPosition = transform.position;
//                _endPosition = transform.position + TargetVector;
//                move = true;
//            }

//            _velocity = CurrentVelocity;
//            _targetDir = Heading(TargetVector);
//            _moveDir = CurrentVelocity.normalized;
//        }

//        Vector2 ShipRotation()
//        {
//            var angle = GetAngleInDegrees(CurrentVelocity.x, CurrentVelocity.y);
//            if (CurrentVelocity == Vector2.zero)
//                angle = _lastAngle;

//            var temp = Quaternion.AngleAxis(angle, Vector3.forward);

//            return temp.eulerAngles;
//        }

//        void Rotate()
//        {
//            var angle = GetAngleInDegrees(CurrentVelocity.x, CurrentVelocity.y);
//            if (CurrentVelocity == Vector2.zero)
//                angle = _lastAngle;

//            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
//        }

//        float GetAngleInDegrees(float x, float y)
//        {
//            return Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90.0F;
//        }

//        //protected IEnumerator ShipRotation()
//        //{
//        //    float angle = Quaternion.Angle(transform.rotation, _lookRotation);

//        //    if (angle > 90.05F)
//        //    {
//        //        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
//        //        _isRotating = true;
        
//        //        yield return null;
//        //    }

//        //    transform.rotation = _lookRotation;
//        //    _isRotating = false;
//        //    print("Rotation completed!");
//        //}

//        public Vector2 Accelerate(Vector2 direction, float currentAcceleration, float maxAcceleration, float t = 1.0F)
//        {
            
//            var forceIncrement = direction * Mathf.Lerp(currentAcceleration, maxAcceleration, t);
//            _rigidBody.AddForce(forceIncrement);
//            return forceIncrement;
//        }

//        public void Move2Vector()
//        {
//            var startDist = (_endPosition - _startPosition).magnitude;
//            var currentDist = (_endPosition - transform.position).magnitude;

//            var normalizedDist = currentDist / startDist;
//            normalizedDist = Mathf.Round(normalizedDist * 1000.0F) / 1000.0F;

//            var dot = Vector2.Dot(ShipRotation(), Heading(TargetPosition));

//            if (TargetPosition == _lastTarget)
//            {
//                if (dot == 0 && normalizedDist > 0)
//                {
//                    if (normalizedDist <= 0.5F)
//                    {
//                        if (IsMoving)
//                        {
//                            Accelerate(-MovingDirection, CurrentAcceleration, MaxAcceleration);
//                        }
//                    }
//                    else
//                    {
//                        Accelerate(Heading(TargetPosition), 0, MaxAcceleration);
//                    }
//                }
//            }
//            else
//            {
//                if (IsMoving)
//                {
//                    Accelerate(-MovingDirection, CurrentAcceleration, MaxAcceleration);
//                }
//                else
//                {
//                    TargetPosition = TargetPosition;
//                }
//            }
//        }

//        public void MoveToVector()
//        {
//            var startDist = (_endPosition - _startPosition).magnitude;
//            var currentDist = (_endPosition - transform.position).magnitude;

//            var normalizedDist = currentDist / startDist;
//            normalizedDist = Mathf.Round(normalizedDist * 1000.0F) / 1000.0F;
//            TargetDistance = normalizedDist;

//            var angle = Vector2.Angle(ShipRotation(), Heading(TargetPosition));
//            angle = Mathf.Round(angle * 10.0F) / 10.0F;
//            var dot = Vector2.Dot(ShipRotation(), Heading(TargetPosition));

//            Debug.Log(angle);

//            if ( 45F < angle && angle < 135F)
//            {
//                if (normalizedDist <= 0.01F)
//                {
//                    _lastAngle = GetAngleInDegrees(CurrentVelocity.x, CurrentVelocity.y);
//                    TargetVector = Vector2.zero;
//                    CurrentVelocity = Vector2.zero;
//                    move = false;
//                    print("Stopping!");
//                }
//                else if (normalizedDist <= 0.02F)
//                {
//                    if(IsMoving)
//                    {
//                        Accelerate(-MovingDirection, CurrentAcceleration, CurrentVelocity.sqrMagnitude);
//                        print("Breaking hard!");
//                    }
//                    else
//                    {
//                        Accelerate(Heading(TargetPosition), CurrentAcceleration, (normalizedDist * 0.1F) * MaxAcceleration);
//                    }
//                }
//                else if (normalizedDist <= 0.5F)
//                {
//                    if(IsMovingMore)
//                    {
//                        Accelerate(-MovingDirection, CurrentAcceleration, MaxAcceleration, 2.0F);
//                        print("Breaking.");
//                    }
//                    else
//                    {
//                        Accelerate(Heading(TargetPosition), CurrentAcceleration, normalizedDist * MaxAcceleration);
//                    }
//                }
//                else 
//                {
//                    //if(_previousVelocity.magnitude > 0)
//                    //{
//                    //    CurrentVelocity = CurrentVelocity - _previousVelocity;
//                    //    Accelerate(Heading(TargetPosition), CurrentVelocity.magnitude, MaxAcceleration);
//                    //}
//                    print("Accelerating in correct angle");
                    
//                    Accelerate(Heading(TargetPosition), CurrentAcceleration, MaxAcceleration);
//                }
//            }
//            else
//            {
//                if(IsMoving)
//                {
//                    //TargetVector = -TargetVector;
//                    Accelerate(-Heading(TargetPosition), CurrentAcceleration, MaxAcceleration);
//                    Accelerate(-Heading(_previousVelocity), CurrentAcceleration, MaxAcceleration);
//                    print("Breaking for u-turn");
//                }
//                else
//                {
//                    Accelerate(Heading(TargetPosition), CurrentAcceleration, 1 - normalizedDist * MaxAcceleration);
//                    print("Wrong angle acceleration from 0");
//                }
//            }

            
//            // 2as = v^2 - u^2
//            // from u meter per second to v (0), decelerating at a meter per second^2
//            //   2 x 9.8 x S , 19.6s = 470,596 s = 24,010
//        }

//        public Vector2 Heading(Vector2 target)
//        {
//            var heading = target - new Vector2(transform.position.x, transform.position.y);
//            var distance = heading.magnitude;

//            var direction = distance != 0 ? heading / distance : new Vector2();

//            return direction;
//        }

//        //public Vector2 Heading(Vector3 target)
//        //{
//        //    var distanceDelta = Vector2.Distance(transform.position, target);
//        //    _targetDir = Vector2.MoveTowards(transform.position, target, distanceDelta).normalized;
//        //    return _targetDir;
//        //}

//        //protected IEnumerator ShipMovement(Vector2 start, Vector2 end)
//        //{
//        //    if (Vector2.Distance(transform.position, end) < Vector2.Distance(start, end) / 2)
//        //    {
//        //        //if(!_isRotating)
//        //        //{

//        //        //Vector3 dir = _lookRotation * Vector2.up;

//        //        _rigidBody.AddForce((_targetDir * Acceleration));

//        //        //Vector3 newPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y), end, MovementSpeed * Time.deltaTime);
//        //        //DeltaPosition = transform.position - newPosition;
//        //        //transform.position = newPosition;
//        //        //}

//        //    }
//        //    else if (Vector2.Distance(transform.position, end) < Vector2.Distance(start, end) / 2)
//        //    {
//        //        //_rigidBody.AddForce(-_travelDir);
//        //    }

//        //    yield return null;

//        //    transform.position = transform.position;

//        //    print("Target reached!");
//        //}

//        public void OnSelect(BaseEventData eventData)
//        {
//            _sr.color = Color.green;
//            IsSelected = true;
//        }

//        public void OnDeselect(BaseEventData eventData)
//        {
//            _sr.color = Color.white;
//            IsSelected = false;
//        }

//        public void OnPointerDown(PointerEventData eventData)
//        {
//            if (!IsSelected)
//                OnSelect(eventData);

//            if (eventData.pointerCurrentRaycast.gameObject != this.gameObject)
//            {
//                OnDeselect(eventData);
//            }
//        }
//    }
//}