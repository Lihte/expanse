using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public abstract class PhysicalEntity : NetworkBehaviour
    {
        public Rigidbody _rigidBody;
        public BoxCollider _collider;

        protected virtual void Awake()
        {
            if(_rigidBody == null)
                _rigidBody = gameObject.AddComponent<Rigidbody>();

            if(_collider == null)
                _collider = gameObject.AddComponent<BoxCollider>();
        }

        protected virtual void Start()
        {
            _rigidBody.useGravity = false;
            _rigidBody.isKinematic = false;
        }
    }
}
