using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed = 8f;
    [SerializeField] private float JumpPower = 18f;
    [SerializeField] private float DashingPower = 32f;
    [SerializeField] private float MaxJump = 2;
    public float JumpRemain;

    [SerializeField] private Rigidbody2D MyRigidbody;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private TrailRenderer tr;



    private float horizontal;

    private bool isFacingRight = true;
    
    private bool canDash = true;
    private bool isDashing;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private bool isJumping;
   
  


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = SimpleInput.GetAxis("Horizontal");
        
        if (isDashing)
        {
            return;
        }
        //Jump
        if (!Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            isJumping = false;
            JumpRemain = MaxJump;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpM();
        }
      

       
        //Dash

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine( Dash() );
        }
       
        Flip();
            
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        MyRigidbody.velocity = new Vector2(horizontal * PlayerSpeed, MyRigidbody.velocity.y);  
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = MyRigidbody.gravityScale;
        MyRigidbody.gravityScale = 0f;
        MyRigidbody.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        MyRigidbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void Jump()
    {
        if (IsGrounded() || (isJumping && (JumpRemain > 0)))
        {
            isJumping = true;
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, JumpPower);
            JumpRemain--;
        }
    }
    public void JumpM()
    {
        if (MyRigidbody.velocity.x > 0f)
        {
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, MyRigidbody.velocity.y * 0.5f);
        }
    }
    
}
 

