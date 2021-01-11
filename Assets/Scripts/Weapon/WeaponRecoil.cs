using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public Animator RigController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRecoil(string weaponName)
    {
        RigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
    }
}
