using Assets.Scripts;
using Assets.Scripts.Interfaces;
using UnityEngine;

[AddComponentMenu("Camera-Control/Scroll")]
public class CameraScrollComponent : MonoBehaviour
{
    IInputManager _input;

    [SerializeField]
    private int _playerId;

    public Transform target;

    public float scrollSpeed = 3000f;
    public float minY = 100f;
    public float maxY = 1000f;

    private float maxX = 10000f;
    private float maxZ = 10000f;

    // Use this for initialization
    void Start()
    {
        _input = InputManager.instance;

        _playerId = GetComponentInParent<PlayerController>().PlayerID;
    }

    void LateUpdate()
    {

        var scroll = _input.GetAxis(_playerId, InputAction.Scroll);

        if (scroll != 0 && target)
        {
            Vector3 camTarget = target.position;
            Vector3 camPos = transform.position;

            camPos = Vector3.MoveTowards(camPos, camTarget, (scroll * scrollSpeed));

            //camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
            //camPos.x = Mathf.Clamp(camPos.x, -maxX, maxX);
            //camPos.z = Mathf.Clamp(camPos.z, -maxZ, maxZ);

            transform.position = camPos;
        }
    }
}
