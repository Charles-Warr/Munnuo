using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private Transform startPoint;
    private GameObject player;

    [SerializeField] float distX;
    [SerializeField] float distY;
    [SerializeField] float distZ;

    [SerializeField] float boostDist;
    private float curDist;

    [SerializeField] float smoothingSpeed;

    [SerializeField] float cameraSensitivity = 1f;
    [SerializeField] bool invertY;

    private float invertModifier;

    private float Yrotation;
    private float XZrotation;



    private bool reachMaximumDist;
    private bool reachMinimumDist;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");

        startPoint.position = new Vector3(player.transform.position.x - distX, player.transform.position.y - distY, player.transform.position.z - distZ);
        oldPosition = player.transform.position;

        curDist = 0;

        //startPoint.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x - distX, GameObject.FindGameObjectWithTag("Player").transform.position.y - distY, GameObject.FindGameObjectWithTag("Player").transform.position.z- distZ);
    }

    void Update()
    {
        Yrotation = Input.GetAxis("CameraY");
        XZrotation = Input.GetAxis("CameraXZ");

        if (invertY)
        {
            invertModifier = 1f;
        }
        else
            invertModifier = -1f;

        float oldX = oldPosition.x;
        float oldY = oldPosition.y;
        float oldZ = oldPosition.z;

        float newX = player.transform.position.x;
        float newY = player.transform.position.y;
        float newZ = player.transform.position.z;

        float difX = newX - oldX;
        float difY = newY - oldY;
        float difZ = newZ - oldZ;

        

        newPosition = new Vector3(difX,difY,difZ);

        if (player.GetComponent<PlayerController>().boosting && player.GetComponent<PlayerController>().bsettings.boostEnergy > 0f)
        {
            
            if((curDist < boostDist) && !reachMaximumDist)
            {

                smoothingSpeed = 0.01f;
                curDist += smoothingSpeed;
                reachMinimumDist = false;
            }
            else
            {
                smoothingSpeed = 0;
                reachMaximumDist = true;
            }
        }
        else
        {
            if (curDist > 0f)
            {

                smoothingSpeed = -0.1f;
                curDist += smoothingSpeed; 
                reachMaximumDist = false;
            }
            else
            {
                smoothingSpeed = 0;
                curDist = 0;
                reachMinimumDist = true;
            }
        }

            newPosition = newPosition - (transform.forward * smoothingSpeed);
        



        oldPosition = player.transform.position;


    }

    void FixedUpdate()
    {
        
    }


 
    void LateUpdate()
    {

        cameraMove();

    }

    private void cameraMove()
    {

            transform.position += newPosition;

            transform.RotateAround(player.transform.position, Vector3.up, cameraSensitivity * Yrotation * Time.deltaTime);

            startPoint.LookAt(player.GetComponent<Transform>());

            transform.RotateAround(player.transform.position, transform.forward + transform.right, cameraSensitivity * invertModifier * XZrotation * Time.deltaTime);

            startPoint.LookAt(player.GetComponent<Transform>());
      
    }
}
