using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public bool isMouseControlled;

    // Update is called once per frame
    protected override void Update()
    {
        // TODO: Anything a human controller does every frame

        // Do what every controller does: Make Decisions
        base.Update();
    }

    protected override void Start()
    {
        // Connect the player to the UIManager
        pawn.uiManager = FindAnyObjectByType<PlayerUIManager>();

        base.Start();
    }

    protected override void MakeDecisions()
    {
        // Move based on Input axes
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Tell the pawn to move
        pawn.Move(moveVector);

        // IF mouse controlled
        if (isMouseControlled)
        {
            // Create the Ray from the mouse position in the direction the camera is facing
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a plane at our feet, and a normal world up
            Plane footPlane = new Plane(Vector3.up, pawn.transform.position);

            // Find the distance down that ray that the plane and ray intersect
            float distanceToIntersect;

            // Find where they intersect
            footPlane.Raycast(mouseRay, out distanceToIntersect);

            // Find the intersection point
            Vector3 intersectionPoint = mouseRay.GetPoint(distanceToIntersect);

            // Look at the intersection point
            pawn.RotateToLookAt(intersectionPoint);
        }
        else
        {
            // Tell the pawn to rotate based on the CameraRotation axis
            pawn.Rotate(Input.GetAxis("CameraRotation"));
        }
        
        // If the player is "sprinting"
        if (Input.GetButton("Sprint"))
        {
            pawn.isSprinting = true;
        }
        else
        {
            pawn.isSprinting = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            pawn.shooter.OnTriggerPull.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pawn.shooter.OnTriggerRelease.Invoke();
        }

        if (pawn.healthComp.currentHealth <= 0)
        {
            UnpossessPawn();

            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);

            Destroy(gameObject);
        }
    }
}
