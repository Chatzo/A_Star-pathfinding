using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Pathfinding))]
internal class Controller : MonoBehaviour
{
    private PlayerInput playerControls;
    private Camera mainCamera;
    private Pathfinding pathfinding;
    private int groundLayer;

    private void Awake()
    {

        pathfinding = GetComponent<Pathfinding>();
        playerControls = new PlayerInput();
        playerControls.Mouse.LeftClick.performed += context => LeftClick();
        playerControls.Mouse.RightClick.performed += context => RightClick();
        //{
        //    if (context.interaction is MultiTapInteraction)
        //        RightClick();
        //    else
        //        LeftClick();
        //};
    }
    private void Start()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
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
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = GetClickPostion();
        if (targetPosition != null)
            pathfinding.GetPath(currentPosition, targetPosition);
    }

    private void RightClick()
    {
        Debug.Log("Rightclicked");
    }
    private Vector3 GetClickPostion()
    {
        Ray myRay;
        RaycastHit myHit;
        myRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(myRay, out myHit) && myHit.collider)
            if (myHit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
                return myHit.point;
        return Vector3.zero;
    }
}
