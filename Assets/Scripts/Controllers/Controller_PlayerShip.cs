using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_PlayerShip : MonoBehaviour
{
    public Model_Player playerModel;

    private void Start()
    {
        Debug.Assert(playerModel != null, "Controller_PlayerShip is looking for a reference to Model_Player, but none has been added in the Inspector!");
    }

    private void Update()
    {
        _ApplySmoothingToMotion();
    }

    public void ShipUpdate()
    {
        _TakeInputs();
        _LimitToScreen();
    }

    public void ForceShipPos(Vector3 where)
    {
        playerModel.positionCurrent = playerModel.positionTarget = where;
    }

    private void _TakeInputs()
    {
        if (Input.GetKey(KeyCode.W))
            playerModel.positionTarget += Vector3.forward * Time.deltaTime * playerModel.shipSpeed;
        if (Input.GetKey(KeyCode.S))
            playerModel.positionTarget -= Vector3.forward * Time.deltaTime * playerModel.shipSpeed;

        if (Input.GetKey(KeyCode.A))
            playerModel.positionTarget -= Vector3.right * Time.deltaTime * playerModel.shipSpeed;
        if (Input.GetKey(KeyCode.D))
            playerModel.positionTarget += Vector3.right * Time.deltaTime * playerModel.shipSpeed;
    }
    private void _LimitToScreen()
    {
        if (playerModel.positionTarget.x < -playerModel.limitHorz)
            playerModel.positionTarget.x = -playerModel.limitHorz;
        if (playerModel.positionTarget.x > playerModel.limitHorz)
            playerModel.positionTarget.x = playerModel.limitHorz;

        if (playerModel.positionTarget.z < -playerModel.limitVert)
            playerModel.positionTarget.z = -playerModel.limitVert;
        if (playerModel.positionTarget.z > playerModel.limitVert)
            playerModel.positionTarget.z = playerModel.limitVert;
    }
    private void _ApplySmoothingToMotion()
    {
        playerModel.positionCurrent = Vector3.Lerp(
            playerModel.positionCurrent, 
            playerModel.positionTarget, 
            Time.deltaTime * playerModel.smoothingFactor);

        playerModel.ship.transform.position = playerModel.positionCurrent;
        playerModel.shield.transform.position = playerModel.ship.transform.position;
    }
}
