using Assets.Scripts;
using Assets.Scripts.Interfaces;
using UnityEngine;

[AddComponentMenu("Camera-Control/Orbit")]
public class CameraOrbitComponent : MonoBehaviour
{
    IInputManager _input;
    [SerializeField]
    private int _playerId;

    public Transform target;
    public float distance = 0.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        _input = InputManager.instance;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        _playerId = GetComponentInParent<PlayerController>().PlayerID;
    }

    void LateUpdate()
    {
        if (_input.GetAxis(_playerId, InputAction.Rotate) != 0 && target)
        {
            x += _input.GetAxis(_playerId, InputAction.MouseX) * xSpeed * 0.02f;
            y -= _input.GetAxis(_playerId, InputAction.MouseY) * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = (target.position - transform.position).magnitude;

            //RaycastHit hit;
            //if (Physics.Linecast(target.position, transform.position, out hit))
            //{
            //    distance -= hit.distance;
            //}

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}