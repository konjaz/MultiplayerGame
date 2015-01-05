using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{
    Rigidbody charRigidbody;
    public float maxSpeed = 1;
    public float basaJumpForce = 2;

    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float gravity = 10.0f;
    public float jumpHeight = 2.0f;
    public Transform CharacterSprite;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    void Awake() {
        charRigidbody = rigidbody;
    }
	
	void Update () 
    {
        OnGroundTest();
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));
	}
    #region OnGroundTest
    private bool onGround = false;
    public bool IsOnGround()
    { return onGround; }
    Ray onGroundRayTest1, onGroundRayTest2;
    //RaycastHit onGroundRayHitTest1, onGroundRayHitTest2;
    
    public void OnGroundTest()
    {
        float x_offset = collider.bounds.size.x / 2; // mozna wstawic jako parametr;
        float rayLenght = collider.bounds.size.y/2; // mozna wstawic jako parametr;
        onGroundRayTest1 = new Ray(new Vector3(charRigidbody.position.x + x_offset, charRigidbody.position.y, charRigidbody.position.z), Vector3.down);
        onGroundRayTest2 = new Ray(new Vector3(charRigidbody.position.x - x_offset, charRigidbody.position.y, charRigidbody.position.z), Vector3.down);
        onGround = false;
        //if (Physics.Raycast(onGroundRayTest1, out onGroundRayHitTest1, rayLenght) || Physics.Raycast(onGroundRayTest2, out onGroundRayHitTest2, rayLenght))
        if (Physics.Raycast(onGroundRayTest1, rayLenght) || Physics.Raycast(onGroundRayTest2, rayLenght))
        {
            onGround = true;
        }
    }
    #endregion
    #region Movement and jumping
    public float GetCharMaxSpeed() 
    {
        return maxSpeed;
    }
    public void Move(float vertical, float horizontal) 
    {
        Vector3 targetVelocity = new Vector3(horizontal, 0, 0);//Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= GetCharMaxSpeed();
        if (horizontal != 0)
        {
            CharacterSprite.localScale = new Vector3(Mathf.Abs(CharacterSprite.localScale.x) * Mathf.Sign(horizontal), CharacterSprite.localScale.y, CharacterSprite.localScale.z);
        }
        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        charRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        //charRigidbody.AddForce(new Vector3(horizontal* GetCharMaxSpeed(), 0, 0) , ForceMode.VelocityChange);
        //if (vertical < 0) 
        //{
        //    charRigidbody.AddForce(Vector3.down * basaJumpForce, ForceMode.Impulse);
        //}
    }

    public void Jump()
    {
        if (IsOnGround() && canJump)
        {
            charRigidbody.velocity = new Vector3(charRigidbody.velocity.x, CalculateJumpVerticalSpeed(), charRigidbody.velocity.z);
            //charRigidbody.AddForce(Vector3.up * basaJumpForce, ForceMode.Impulse);
        }
    }
    #endregion
    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

}
