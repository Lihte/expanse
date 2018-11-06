using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Components;
using Assets.Scripts;
using UnityEngine.Networking;

public class UnitSelectionManager : MonoBehaviour, ISelectionManager
{
    private int playerID;

    IInputManager _input;

    public List<GameObject> selectedObjects;
    public List<GameObject> allObjects;

    public Rect selectionBox;
    Vector3 initialClick;

    public bool isSelecting;

    private void Start()
    {
        Initialize();
        selectionBox = new Rect();

        _input = InputManager.instance;



        //GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        //foreach (GameObject unit in units)
        //{
        //    GameUnit gameUnit = unit.GetComponent<GameUnit>();
        //    if (gameUnit != null && !gameUnit.hasAuthority)
        //        continue;

        //    allObjects.Add(unit);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // If we press the left mouse button, save mouse location and begin selection
        if (_input.GetButtonDown(playerID, InputAction.Select))
        {
            isSelecting = true;
            initialClick = Input.mousePosition;

            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (selectableObject.selectionRing)
                {
                    selectableObject.IsSelected = false;
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if (_input.GetButtonUp(playerID, InputAction.Select))
        {
            var selectedObjects = new List<SelectableUnitComponent>();
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectedObjects.Add(selectableObject);
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(initialClick);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                var selectable = hit.collider.GetComponent<SelectableUnitComponent>();

                // ShipTestBehaviour should be changed to something like a GameEntityComponent
                if(selectable && selectable.GetComponent<ShipTestBehaviour>().hasAuthority)
                {
                    selectable.IsSelected = true;
                    selectedObjects.Add(selectable);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
            foreach (var selectedObject in selectedObjects)
                sb.AppendLine("-> " + selectedObject.gameObject.name);
            Debug.Log(sb.ToString());

            isSelecting = false;
            initialClick = -Vector3.one;
        }

        // Highlight all objects within the selection box
        if (isSelecting && _input.GetButton(playerID, InputAction.Select))
        {
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    // ShipTestBehaviour should be changed to something like a GameEntityComponent
                    if (selectableObject.GetComponent<ShipTestBehaviour>().hasAuthority)
                    {
                        selectableObject.IsSelected = true;
                    }
                }
                else
                {
                    selectableObject.IsSelected = false;
                }
            }
        }
    }

    private void OnGUI()
    {
        //if (isSelecting)
        //{
        //    // Create a rect from both mouse positions
        //    var rect = selectionBox;
        //    Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
        //    Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        //}
    }

    private bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        selectionBox.Set(initialClick.x, Screen.height - initialClick.y, Input.mousePosition.x - initialClick.x, (Screen.height - Input.mousePosition.y) - (Screen.height - initialClick.y));

        if (selectionBox.width < 0)
        {
            selectionBox.x += selectionBox.width;
            selectionBox.width *= -1f;
        }
        if (selectionBox.height < 0)
        {
            selectionBox.y += selectionBox.height;
            selectionBox.height *= -1f;
        }

        Vector3 position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        position.y = Screen.height - position.y;

        if (selectionBox.Contains(position))
            return true;
        else
            return false;
    }

    private void Initialize()
    {
        playerID = GetComponentInParent<PlayerController>().PlayerID;

        if (selectedObjects == null)
            selectedObjects = new List<GameObject>();

        if (allObjects == null)
            allObjects = new List<GameObject>();
    }
}
