using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
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

    private bool inAir;

    [SerializeField] float jumpHeight;

    private float Xaxis;
    private float Zaxis;

    public bool boosting;

    private bool jumping;

    private Rigidbody PlayerBody;
    private GameObject Camera;
    
    private Vector3 moveDirection;

    private bool grounded;


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

        inAir = !grounded;

        moveDirection = (Camera.transform.right * Xaxis) + (Camera.transform.forward * Zaxis);

        Debug.Log(PlayerBody.velocity);

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


        if(grounded && Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
    }

    private void groundStatus()
    {
        grounded = feet.GetComponent<GroundStatus>().ground;
    }

    private void motion()
    {
        if(Input.GetButtonDown("Boost") && bsettings.boostEnergy > 0f && (!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.maximumOverallVelocity|| Mathf.Abs(PlayerBody.velocity.z) > bsettings.maximumOverallVelocity)))
            PlayerBody.AddForce(new Vector3(moveDirection.x * bsettings.boostInitialBurst,0f, moveDirection.z*bsettings.boostInitialBurst), ForceMode.Impulse);

        if(boosting && bsettings.boostEnergy > 0f)
        {
            if(!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.boostMaximumVelocity || Mathf.Abs(PlayerBody.velocity.z) > bsettings.boostMaximumVelocity))
            {
                PlayerBody.AddForce(new Vector3(acceleration * moveDirection.x, 0f, acceleration * moveDirection.z), ForceMode.Acceleration);
            }
            //PlayerBody.velocity = new Vector3((speed + boostMaximumVelocity) * Xaxis, PlayerBody.velocity.y, (speed + boostMaximumVelocity) * Zaxis);
        }    
        else
        {
            if(!(Mathf.Abs(PlayerBody.velocity.x) > bsettings.maximumVelocity || Mathf.Abs(PlayerBody.velocity.z) > bsettings.maximumVelocity))
            {
                PlayerBody.AddForce(new Vector3(acceleration * moveDirection.x, 0f,acceleration * moveDirection.z), ForceMode.Acceleration);
            }

              //  PlayerBody.velocity = new Vector3(speed * Xaxis, PlayerBody.velocity.y, speed * Zaxis);
        }
            
    }

    

    void FixedUpdate()
    {
        motion();
        jump();
    }

    private void jump()
    {
        if (jumping)
            PlayerBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}
