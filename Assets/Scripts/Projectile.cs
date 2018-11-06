using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        #region Debugging

        [SerializeField]
        private float _velocity = 0.0f;

        #endregion


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _velocity = GetComponent<Rigidbody>().velocity.magnitude;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Plane")
                return;

            var hit = collision.gameObject;
            Debug.Log("RailgunProjectile collided with: " + hit.name);

            if (hit.name == "pf_capitalShip")
                Destroy(hit.gameObject);

            Destroy(gameObject);
            
        }

    }
}