using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;
using Assets.Scripts.Interfaces;

public class PlayerController : NetworkBehaviour
{

    public int PlayerID = 0;

    IInputManager _input;

    public GameObject pf_capitalShipPrefab;
    public GameObject playerCamera;

    public void Start()
    {
        if (isLocalPlayer)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            playerCamera.SetActive(false);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        _input = InputManager.instance;

        CmdSpawnShip();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

    }

    [Command]
    void CmdSpawnShip()
    {
        var shipUnit = (GameObject)Instantiate(pf_capitalShipPrefab, transform.position, Quaternion.identity);
        shipUnit.GetComponent<ShipTestBehaviour>()._playerId = PlayerID;
        shipUnit.GetComponent<NavigationComponent>()._playerID = PlayerID;
        NetworkServer.SpawnWithClientAuthority(shipUnit, gameObject);
    }
}
