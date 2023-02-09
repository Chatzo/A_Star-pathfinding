using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Controller : MonoBehaviour
{
    private PlayerInput playerControls;
    private LayerMask tileMask;
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit myHit;

    private void Awake()
    {
        mainCamera = Camera.main;
        tileMask = LayerMask.NameToLayer("Tiles");

        playerControls = new PlayerInput();
        playerControls.Mouse.LeftClick.performed += context =>
        {
            if (context.interaction is MultiTapInteraction)
                DoubleLeftClick();
            else
                LeftClick();
        };
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void LeftClick()
    {
        Debug.Log("Leftclicked");
        if(IsTileHit())
        {
            myHit.collider.gameObject.GetComponent<Tile>().SetColor(Color.magenta);
        }
        
    }

    private void DoubleLeftClick()
    {
        Debug.Log("Double clicked");
        if(IsTileHit())
        {
            myHit.collider.gameObject.GetComponent<Tile>().SetColor(Color.blue);
        }
    }

    private bool IsTileHit()
    {
        myRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(myRay, out myHit) && myHit.collider.gameObject.layer.CompareTo(tileMask) == 0)
            return true;
        return false;
    }
}
