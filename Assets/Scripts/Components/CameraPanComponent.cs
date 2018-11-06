using Assets.Scripts;
using Assets.Scripts.Interfaces;
using UnityEngine;

[AddComponentMenu("Camera-Control/Panning")]
public class CameraPanComponent : MonoBehaviour
{
    IInputManager _input;
    [SerializeField]
    private int _playerId;

    public float panSpeed = 500f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public Transform target;

    // Use this for initialization
    void Start()
    {
        _input = InputManager.instance;

        _playerId = GetComponentInParent<PlayerController>().PlayerID;
    }

    void LateUpdate()
    {
        float hAxis = _input.GetAxis(_playerId, InputAction.Horizontal);
        float vAxis = _input.GetAxis(_playerId, InputAction.Vertical);

        if (hAxis != 0 || vAxis != 0 && target)
        {
            Vector3 camTarget = target.position;

            camTarget.z += (vAxis * panSpeed) * 0.02f;
            camTarget.x += (hAxis * panSpeed) * 0.02f;

            camTarget.x = Mathf.Clamp(camTarget.x, -panLimit.x, panLimit.x);
            camTarget.z = Mathf.Clamp(camTarget.z, -panLimit.y, panLimit.y);

            target.position = camTarget;
        }
    }
}