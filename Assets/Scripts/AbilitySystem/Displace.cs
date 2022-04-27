using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
//[CreateAssetMenu(menuName = "Abilities/Displace")]
public class Displace : AbilityBase
{
    [SerializeField] GameObject storedItem = null;
    Transform hitTransform;
    Player player;
    float grabRange = 2f;
    public float range = 12f;
    Vector3 mousePosition;
    bool itemIsStored = false;

    private void Start()
    {
        
    }

    public override void Init()
    {
        base.Init();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public override void ApplySkillEffect()
    {
        base.ApplySkillEffect();
        
        if (itemIsStored)
        {
            storedItem.SetActive(true);
            ClonedItem();
        }
        else
        {
            GrabItem();
        }
    }

    GameObject ClonedItem()
    {
        return storedItem;
    }

    public override void UpdatableEffects()
    {
        base.UpdatableEffects();
        if (storedItem && itemIsStored)
        { 
            RayCastScreenCenter(ClonedItem());
        }
    }

    public override void UnapplySkillEffect()
    {
        if (storedItem)
        {
            itemIsStored = true;
        }
        else
        {
            itemIsStored = false;
        }
    }

    void GrabItem()
    {
        player.FindCloestStorableItem();
        if (grabRange > GrabDistance())
        {
            storedItem = player.FindCloestStorableItem();
            player.FindCloestStorableItem().SetActive(false);
            Debug.Log("Stored item is: " + storedItem.name);
        }
    }

    float GrabDistance()
    {
        float distance = Vector3.Distance(player.gameObject.transform.position, player.FindCloestStorableItem().transform.position);
        return distance;
    }

    void RayCastScreenCenter(GameObject storedItem)
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(centerOfScreen);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, range))
        {
            hitTransform = raycastHit.transform;
            
            mousePosition = raycastHit.point;
            Vector3 aimTarget = mousePosition;
            aimTarget.y = player.transform.position.y;
            Vector3 aimdDirection = (aimTarget - player.transform.position).normalized;
            storedItem.transform.position = ray.GetPoint(range);

            player.transform.forward = Vector3.Lerp(player.transform.forward, aimdDirection, Time.deltaTime * 20f);

        }

    }

    //void SetTeleportationCamera()
    //{
    //    GameObject teleportationCam;
    //    CinemachineVirtualCamera vcam;
    //    teleportationCam = GameObject.Find("TeleportationCam");
    //    vcam = teleportationCam.GetComponent<CinemachineVirtualCamera>();
    //    vcam.Priority = 30;
    //    volume = Camera.main.GetComponent<Volume>();

    //    if (volume.profile.TryGet(out bloom) && volume.profile.TryGet(out vignette))
    //    {
    //        Color32 tintColor = new Color32(0, 93, 255, 255);
    //        //bloom.tint.overrideState = true;
    //        bloom.tint.Override(tintColor);
    //        bloom.intensity.Override(7.5f);
    //        vignette.active = true;
    //        Time.timeScale = 0.3f;
    //    }

    //}





}
