/*using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShipController : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler
    {

        public Texture2D Texture;

        private Rigidbody2D _rigidBody;
        private Sprite _sprite;
        private SpriteRenderer _sr;

        public Vector3 TargetVector;
        public Vector3 DeltaPosition;
        public float RotationSpeed;
        public float MaxVelocity;

        public float MovementSpeed;

        public Vector2 Velocity;
        public float Acceleration = 9.81F;

        public bool IsSelected;
        public bool IsMoving
        {
            get
            {
                return _rigidBody.velocity.sqrMagnitude > 0.0F;
            }
        }

        [SerializeField]
        private bool _isRotating;
        [SerializeField]
        private Quaternion _lookRotation;
        public Vector2 _targetDir;

        private BoxCollider2D _collider;

        void Awake()
        {
            _sr = gameObject.AddComponent<SpriteRenderer>();
            _rigidBody = gameObject.AddComponent<Rigidbody2D>();
            _collider = gameObject.AddComponent<BoxCollider2D>();
        }

        // Use this for initialization
        void Start()
        {

            _sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, 125.0f, 311.0f), new Vector2(0.5f, 0.5f), 100.0f);
            _rigidBody.gravityScale = 0.0f;

            _sr.color = Color.yellow;
            _sr.sprite = _sprite;

            _collider.size = _sprite.bounds.size;

            IsSelected = false;
            RotationSpeed = 2.0F;
            MovementSpeed = 1.0F;
            Velocity = _rigidBody.velocity;
        }

        Vector2 start;
        Vector2 end;
        bool move = false;

        void FixedUpdate()
        {
            if (move)
            {
                Move(Vector2.Distance(start, end));
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position);
                TargetVector.z = 0;

                SetLookRotation();

                start = transform.position;
                end = transform.position + TargetVector;
                move = true;
            }
        }

        void Rotate()
        {
            _lookRotation = Quaternion.LookRotation(TargetVector, Camera.main.transform.position);
            _lookRotation.x = 0;
            _lookRotation.y = 0;

            StartCoroutine(ShipRotation());
        }

        void SetLookRotation()
        {
            //var distanceDelta = Vector2.Distance(transform.position, TargetVector);
            //_lookRotation = Quaternion.LookRotation(TargetVector, Camera.main.transform.position);
            //_travelDir = Vector2.zero;
            //_travelDir = Vector2.MoveTowards(transform.position, TargetVector, distanceDelta).normalized;

            _lookRotation.x = 0;
            _lookRotation.y = 0;

        }

        protected IEnumerator ShipRotation()
        {
            float angle = Quaternion.Angle(transform.rotation, _lookRotation);

            if (angle > 90.05F)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
                _isRotating = true;
                angle = Quaternion.Angle(transform.rotation, _lookRotation);
                yield return null;
            }

            transform.rotation = _lookRotation;
            _isRotating = false;
            print("Rotation completed!");
        }

        float _vel = 0;

        protected void Move(float originalDistance)
        {
            var currentDistance = Vector2.Distance(transform.position, end);

            if (_rigidBody.velocity.normalized != _targetDir && move == true)
            {
                Decelerate(Acceleration);
            }
            else
            {
                if (currentDistance > originalDistance / 2)
                {
                    Accelerate(Acceleration);
                }
                else if (currentDistance < originalDistance / 2 && currentDistance > 1F)
                {
                    Decelerate(Acceleration);
                }
                else if (currentDistance < 1F && currentDistance > 0.5F)
                {
                    Decelerate(Acceleration / 2);
                }
                if (currentDistance < 0.1F)
                {
                    _rigidBody.velocity = Vector2.zero;
                    _targetDir = Vector2.zero;
                    move = false;
                }
            }

            //transform.position = Vector2.MoveTowards(transform.position, end, Acceleration * Time.fixedDeltaTime);

            Velocity = _rigidBody.velocity;
        }

        public Vector2 Heading()
        {
            _targetDir = Vector2.zero;
            var heading = TargetVector - gameObject.transform.position;
            var distance = heading.magnitude;
            _targetDir = heading / distance;

            return _targetDir;
        }

        public Vector2 Heading(Vector3 target)
        {
            var distanceDelta = Vector2.Distance(transform.position, target);
            _targetDir = Vector2.MoveTowards(transform.position, target, distanceDelta).normalized;
            return _targetDir;
        }

        protected void Accelerate(float acceleration)
        {
            print("Accelerate!");
            var force = Heading() * acceleration;
            _rigidBody.AddForce(force);
        }

        protected void Decelerate(float acceleration)
        {
            print("Decelerate!");
            var force = _rigidBody.velocity.normalized * -acceleration;
            _rigidBody.AddForce(force);
        }

        protected IEnumerator ShipMovement(Vector2 start, Vector2 end)
        {
            if (Vector2.Distance(transform.position, end) < Vector2.Distance(start, end) / 2)
            {
                //if(!_isRotating)
                //{

                //Vector3 dir = _lookRotation * Vector2.up;

                _rigidBody.AddForce((_targetDir * Acceleration));

                //Vector3 newPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y), end, MovementSpeed * Time.deltaTime);
                //DeltaPosition = transform.position - newPosition;
                //transform.position = newPosition;
                //}

            }
            else if (Vector2.Distance(transform.position, end) < Vector2.Distance(start, end) / 2)
            {
                //_rigidBody.AddForce(-_travelDir);
            }

            yield return null;

            transform.position = transform.position;

            print("Target reached!");
        }

        public void OnSelect(BaseEventData eventData)
        {
            _sr.color = Color.green;
            IsSelected = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _sr.color = Color.white;
            IsSelected = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsSelected)
                OnSelect(eventData);

            if (eventData.pointerCurrentRaycast.gameObject != this.gameObject)
            {
                OnDeselect(eventData);
            }
        }
    }
}
*/