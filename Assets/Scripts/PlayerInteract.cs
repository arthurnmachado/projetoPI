using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField]
    Weapon currentWeapon;

    bool hasWeapon = false;

    private DecalController decalController;

    // Start is called before the first frame update
    void Start()
    {
        decalController = GetComponent<DecalController>();

        hasWeapon = currentWeapon != null;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasWeapon)
        {   
            if (Input.GetButton("Fire1") && currentWeapon.CanFire())
            {
                RaycastHit hit = currentWeapon.Fire(); // Dispare a arma

                if(hit.distance > 0)
                    decalController.SpawnDecal(hit);
            }
            else if (Input.GetButtonUp("Fire2") || Input.GetButtonDown("Fire2") || Input.GetButton("Fire2"))
            {
                currentWeapon.SecondaryFire(Input.GetButtonDown("Fire2") || Input.GetButton("Fire2"));
            }
        }
    }
}
