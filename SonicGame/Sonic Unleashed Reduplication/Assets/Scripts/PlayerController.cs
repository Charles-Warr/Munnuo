using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float deceleration;
    public GameObject feet;

    [System.Serializable]
    public class BoostSettings
    {
        public float boostInitialBurst = 20f;
        public float boostMaximumVelocity = 50f;
        public float maximumOverallVelocity = 75f;
        public float maximumVelocity = 10f;

        public float boostMeterCapacity = 100f;
        public float boostEnergy = 100f;

        public float boostRefillRate = 0.1f;
        public float boostDepleteRate = 0.5f;
    }

    public BoostSettings bsettings = new BoostSettings();

    [SerializeField] float jumpHeight;
    [SerializeField] float jumpForce;

    private float Xaxis;
    private float Zaxis;

    public bool boosting;

    private bool jumping;

    private Rigidbody PlayerBody;
    private GameObject Camera;
    
    private Vector3 moveDirection;
    private Vector3 jumpStartPosition;

    private bool grounded;
    private bool wasGrounded;


    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = this.GetComponent<Rigidbody>();
        Camera = GameObject.FindGameObjectWithTag("PlayerViewCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Xaxis = Input.GetAxis("Horizontal");
        Zaxis = Input.GetAxis("Vertical");

        boosting = Input.GetButton("Boost");

        groundStatus();

        setJumpingStatus();

        moveDirection = (Camera.transform.right * Xaxis) + (Camera.transform.forward * Zaxis);

        if(!boosting && (bsettings.boostEnergy<bsettings.boostMeterCapacity))
        {
            bsettings.boostEnergy += bsettings.boostRefillRate;
        }

        if(boosting)
        {
            if (bsettings.boostEnergy > 0f)
                bsettings.boostEnergy -= bsettings.boostDepleteRate;
            else
                bsettings.boostEnergy = 0f;
        }

        wasGrounded = grounded;

    }

    private void groundStatus()
    {
        grounded = feet.GetComponent<GroundStatus>().ground;
    }

    private void motion()
    {
        if (grounded)
        {


            if (Input.GetButtonDown("Boost") && bsettings.boostEnergy > 0f && (!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.maximumOverallVelocity || Mathf.Abs(PlayerBody.velocity.z) > bsettings.maximumOverallVelocity)))
                PlayerBody.AddForce(new Vector3(moveDirection.x * bsettings.boostInitialBurst, 0f, moveDirection.z * bsettings.boostInitialBurst), ForceMode.Impulse);

            if (boosting && bsettings.boostEnergy > 0f)
            {
                if (!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.boostMaximumVelocity || Mathf.Abs(PlayerBody.velocity.z) > bsettings.boostMaximumVelocity))
                {
                    PlayerBody.AddForce(new Vector3(acceleration * moveDirection.x, 0f, acceleration * moveDirection.z), ForceMode.Acceleration);
                }
                //PlayerBody.velocity = new Vector3((speed + boostMaximumVelocity) * Xaxis, PlayerBody.velocity.y, (speed + boostMaximumVelocity) * Zaxis);
            }
            else
            {
                if (!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.maximumVelocity || Mathf.Abs(PlayerBody.velocity.z) > bsettings.maximumVelocity))
                {
                    PlayerBody.AddForce(new Vector3(acceleration * moveDirection.x, 0f, acceleration * moveDirection.z), ForceMode.Acceleration);
                }

                //  PlayerBody.velocity = new Vector3(speed * Xaxis, PlayerBody.velocity.y, speed * Zaxis);
            }

            if(Xaxis == 0 && Zaxis == 0)
            {
                if (Mathf.Abs(PlayerBody.velocity.x) >= Mathf.Epsilon || Mathf.Abs(PlayerBody.velocity.z) >= Mathf.Epsilon)
                    PlayerBody.velocity = new Vector3(PlayerBody.velocity.x - (PlayerBody.velocity.x*deceleration * Time.deltaTime), PlayerBody.velocity.y, PlayerBody.velocity.z - (PlayerBody.velocity.z * deceleration * Time.deltaTime));
                else
                    PlayerBody.velocity = new Vector3(0,PlayerBody.velocity.y,0);
            }

        }
    }

    

    void FixedUpdate()
    {
        
        jump();
        motion();
        applyGravity();
    }

    private void applyGravity()
    {
        if(!grounded)
        {
            PlayerBody.velocity += Physics.gravity*1.5f*Time.deltaTime;
        }

        if(PlayerBody.velocity.y <= 0 && !grounded)
        {
            PlayerBody.velocity += Physics.gravity * Time.deltaTime;
        }
    }

    private void setJumpingStatus()
    {
        if (Input.GetButtonDown("Jump") && !jumping && grounded)
        {
            jumping = true;
            jumpStartPosition = PlayerBody.position;
        }
            

        if(Input.GetButtonUp("Jump") || (!wasGrounded && grounded) || (PlayerBody.position.y - jumpStartPosition.y > jumpHeight))
        {
            jumping = false;
        }
    }

    private void jump()
    {
        if (jumping)
        {
            PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }
            
    }
}
