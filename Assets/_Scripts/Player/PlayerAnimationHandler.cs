using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour {
    private Animator animator;
    private PlayerMovement movement;
    private Rigidbody2D rb;

    private void Awake() {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", movement.IsOnGround());
        animator.SetBool("isWallSliding", movement.IsSlidingOnWall());
    }

    public void TriggerDoubleJump() {
        animator.SetTrigger("isDoubleJump");
    }

    public void TriggerWallJump() {
        animator.SetTrigger("isWallJump");
    }

    public void TriggerHit() {
        animator.SetTrigger("isHit");
    }

}
