using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ShipEntity : PhysicalEntity
    {

        public Vector3 TargetVector;
        public Vector3 TargetPosition;

        private Vector3 _lastTarget;

        private Vector2 _targetDir;

        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private Vector2 _previousVelocity;
        public Vector2 CurrentVelocity
        {
            get { return _rigidBody.velocity; }
            set { _rigidBody.velocity = value; }
        }

        [SerializeField]
        private Vector2 _moveDir;
        [SerializeField]
        private Quaternion _lookRotation;

        public bool IsRotating;
        public bool IsSelected;

        public float CurrentAcceleration = 0;

        // Use this for initialization
        public new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetVector = Vector3.zero;
                _startPosition = Vector3.zero;
                _endPosition = Vector3.zero;
                _targetDir = Vector2.zero;

                TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TargetVector = TargetPosition - transform.position;
                TargetVector.z = 0;

                _previousVelocity = CurrentVelocity;

                //if (IsMoving)
                //{

                //}

                _startPosition = transform.position;
                _endPosition = transform.position + TargetVector;
                //move = true;
            }

            _targetDir = Heading(TargetVector);
            _moveDir = CurrentVelocity.normalized;
        }

        void FixedUpdate()
        {
            //if (move)
            //{
            //    Move2Vector();
            //    Rotate();
            //}
        }



        public Vector2 Heading(Vector2 target)
        {
            var heading = target - new Vector2(transform.position.x, transform.position.y);
            var distance = heading.magnitude;

            var direction = distance != 0 ? heading / distance : new Vector2();

            return direction;
        }

        //public void OnSelect(BaseEventData eventData)
        //{
        //    _sr.color = Color.green;
        //    IsSelected = true;
        //}

        //public void OnDeselect(BaseEventData eventData)
        //{
        //    _sr.color = Color.white;
        //    IsSelected = false;
        //}

    }
}