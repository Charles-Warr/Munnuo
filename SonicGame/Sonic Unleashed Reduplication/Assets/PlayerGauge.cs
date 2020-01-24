using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine;

public class PlayerGauge : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Player;
    private UnityEngine.UI.Slider Meter;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Meter = this.GetComponent<UnityEngine.UI.Slider>();

        Meter.maxValue = Player.GetComponent<PlayerController>().bsettings.boostMeterCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        Meter.value = Player.GetComponent<PlayerController>().bsettings.boostEnergy;
    }
}
