using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public ControlStuff playerMovement;
    private bool ropeAttached;
    private bool ropeFired;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();


    public float ropeFireForce = 1.0f;
    private Rigidbody2D _playerrb;

    void Awake()
    {
        _playerrb = playerMovement.GetComponent<Rigidbody2D>();
        // 2
        ropeJoint.enabled = false;
        ropeHingeAnchorRb = GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = GetComponent<SpriteRenderer>();
        ResetRope();
    }

    void Update()
    {
        // 3
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        // 4
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        // // 6
        if (!ropeFired)
        {
            SetCrosshairPosition(aimAngle);
            transform.position = playerMovement.transform.position;
        }
        else 
        {
            crosshairSprite.enabled = false;
            ropeRenderer.SetPosition(0, playerMovement.transform.position);
            ropeRenderer.SetPosition(1, transform.position);
        }
        
        HandleInput(aimDirection);
    }
    
    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }
        
        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ropeAttached = true;
        ropeHingeAnchorRb.constraints = RigidbodyConstraints2D.FreezeAll;
        ropeHingeAnchorRb.velocity = Vector2.zero;
        ropeHingeAnchorRb.freezeRotation = true;
        ropeJoint.distance = (playerMovement.transform.position - transform.position).magnitude;
        ropeJoint.enabled = true;
    }

    // 1
    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            // 2
            if (ropeAttached) return;
            
            ropeFired = true;
            ropeRenderer.enabled = true;
            ropeHingeAnchorSprite.enabled = true;
            ropeHingeAnchorRb.simulated = true;
            ropeHingeAnchorRb.constraints = RigidbodyConstraints2D.None;
            ropeHingeAnchorRb.velocity = _playerrb.velocity;
            ropeHingeAnchorRb.freezeRotation = false;
            ropeHingeAnchorRb.AddForce(aimDirection * ropeFireForce, ForceMode2D.Impulse);
            ropeHingeAnchorRb.SetRotation(Quaternion.LookRotation(ropeHingeAnchorRb.velocity));
            // var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
            //
            // // 3
            // if (hit.collider != null)
            // {
            //     ropeAttached = true;
            //     if (!ropePositions.Contains(hit.point))
            //     {
            //         // 4
            //         // Jump slightly to distance the player a little from the ground after grappling to something.
            //         transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
            //         ropePositions.Add(hit.point);
            //         ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
            //         ropeJoint.enabled = true;
            //         ropeHingeAnchorSprite.enabled = true;
            //     }
            // }
            // // 5
            // else
            // {
            //     ropeRenderer.enabled = false;
            //     ropeAttached = false;
            //     ropeJoint.enabled = false;
            // }
        }

        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }
    }

// 6
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        ropeFired = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
    }

    
}
