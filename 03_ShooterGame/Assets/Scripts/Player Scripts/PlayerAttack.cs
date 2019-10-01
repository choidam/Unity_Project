using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;

    public float fireRate = 15f; // time to allow to shoot
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCamerAnim;
    private bool zoomed;

    private Camera mainCam;
    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab; 

    [SerializeField]
    private Transform arrow_Bow_StartPosition; 

    private void Awake() {
        weapon_Manager = GetComponent<WeaponManager>();

        zoomCamerAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);  

        mainCam = Camera.main;
    } 
    
    void Start()
    {
        is_Aiming = false;
    }

    
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot(){


        if(Input.GetKeyDown(KeyCode.X)){
            // axe 1
            if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG){
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

            // axe 2,3,4  - handle shoot  
            } else if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET){
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                BulletFired();

            // axe 5,6
            } else {
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                // axe 5
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW){
                    // throw arrow 
                    ThrowArrowOrSpear(true);

                }


                // axe 6
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR){
                    // throw spear 
                    ThrowArrowOrSpear(false);


                }


            }


        }  

        /* 

        // assault rifle
        if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE){

           if(Input.GetMouseButton(0) && Time.time > nextTimeToFire) {
                nextTimeToFire = Time.time + 1f / fireRate;
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                // BulletFired();
            }

        } else { // shoot once 

            if(Input.GetMouseButtonDown(0)){
                
                // handle axe
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG){
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                // handle shoot 
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET){
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                    // BulletFired();
                } else {

                    // arrow of spear

                }

            }

        } // else 
        */
        

    } // weapon shoot 

    void ZoomInAndOut(){

        // we are goint to aim with our camera on the weapon
        // handle weapon 2,3,4
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM){

            if(Input.GetMouseButtonDown(1)){ // right mouse button
                zoomCamerAnim.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false); 
            }

            if(Input.GetMouseButtonUp(1)){ // release right mouse button
                zoomCamerAnim.Play(AnimationTags.ZOOM_OUT_AIM);
                crosshair.SetActive(true);
            }

        } // zoom the weapon


        // handle weapon 5,6
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM){

            Debug.Log("is_Aiming : " + is_Aiming);

            // if(Input.GetMouseButtonDown(1)){

            //     if(!is_Aiming){
            //         weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
            //         is_Aiming = true;
            //     } else {
            //         weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
            //         is_Aiming = false;
            //     }
          
            //  }


            if(Input.GetMouseButtonDown(1)){
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                //is_Aiming = true;
            }

            if(Input.GetMouseButtonUp(1)){
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                //is_Aiming = false;
            }


        } // weapon self aim - bow or spear 

    } // zoom in and out 

    void ThrowArrowOrSpear(bool throwArrow){

        if(throwArrow){
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_StartPosition.position;

            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);
        } else {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_StartPosition.position;

            spear.GetComponent<ArrowBowScript>().Launch(mainCam);
        }


    } // throw arrow or spear


    // shooting
    void BulletFired(){

        RaycastHit hit; // contain data hitting

        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)){

            print("We hit : " + hit.transform.gameObject.name);

            if(hit.transform.tag == Tags.ENEMY_TAG){
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }


        }




    } // bullet fired

}
