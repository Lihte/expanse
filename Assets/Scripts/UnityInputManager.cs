using System.Collections.Generic;
using UnityEngine;

public class UnityInputManager : InputManager
{
    [SerializeField]
    private string _playerAxisPrefix = string.Empty;
    [SerializeField]
    private int _maxNumberOfPlayers = 2;

    [Header("Unity Axis Mappings")]
    [SerializeField]
    private string _mouseX = "Mouse X";
    [SerializeField]
    private string _mouseY = "Mouse Y";
    [SerializeField]
    private string _mouseScrollWheel = "Mouse ScrollWheel";
    [SerializeField]
    private string _horizontal = "Horizontal";
    [SerializeField]
    private string _vertical = "Vertical";
    [SerializeField]
    private string _actionAxis = "Mouse LeftButton";
    [SerializeField]
    private string _selectAxis = "Mouse RightButton";
    [SerializeField]
    private string _rotate = "Mouse MiddleButton";

    private Dictionary<int, string>[] _actions;

    protected override void Awake()
    {
        base.Awake();

        if(instance != null)
        {
            isEnabled = false;
            return;
        }

        SetInstance(this);

        _actions = new Dictionary<int, string>[_maxNumberOfPlayers];

        for (int i = 0; i < _maxNumberOfPlayers; i++)
        {
            Dictionary<int, string> playerActions = new Dictionary<int, string>();
            _actions[i] = playerActions;
            string prefix = !string.IsNullOrEmpty(_playerAxisPrefix) ? _playerAxisPrefix + i : string.Empty;

            AddAction(InputAction.Select, prefix + _selectAxis, playerActions);
            AddAction(InputAction.Action, prefix + _actionAxis, playerActions);
            AddAction(InputAction.Horizontal, prefix + _horizontal, playerActions);
            AddAction(InputAction.Vertical, prefix + _vertical, playerActions);
            AddAction(InputAction.Scroll, prefix + _mouseScrollWheel, playerActions);
            AddAction(InputAction.Rotate, prefix + _rotate, playerActions);
            AddAction(InputAction.MouseX, prefix + _mouseX, playerActions);
            AddAction(InputAction.MouseY, prefix + _mouseY, playerActions);
        }
    }

    private void AddAction(InputAction action, string actionName, Dictionary<int, string> actions)
    {
        if (string.IsNullOrEmpty(actionName))
            return;

        actions.Add((int)action, actionName);
    }

    public override bool GetButton(int playerId, InputAction action)
    {
        bool value = Input.GetButton(_actions[playerId][(int)action]);
        return value;
    }

    public override bool GetButtonDown(int playerId, InputAction action)
    {
        bool value = Input.GetButtonDown(_actions[playerId][(int)action]);
        return value;
    }

    public override bool GetButtonUp(int playerId, InputAction action)
    {
        bool value = Input.GetButtonUp(_actions[playerId][(int)action]);
        return value;
    }

    public override float GetAxis(int playerId, InputAction action)
    {
        float value = Input.GetAxisRaw(_actions[playerId][(int)action]);
        return value;
    }
}
