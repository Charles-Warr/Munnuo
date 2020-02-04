using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        applyGravity();
    }

    private void applyGravity()
    {
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + (Physics.gravity.y * Time.deltaTime), player.transform.position.z);
    }
}
