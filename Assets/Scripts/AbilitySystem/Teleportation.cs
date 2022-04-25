using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public delegate bool TeleportationInUse();

[CreateAssetMenu(menuName = "Abilities/Teleportation")]
public class Teleportation : AbilityBase
{
    [SerializeField] GameObject teleportationCam;
    [SerializeField] LayerMask aimColliderMask;
    [SerializeField] GameObject vfx;
    GameObject debugSphere;
    ThirdPersonController tpc;
    Transform hitTransform = null;
    Player player;
    public float teleportationRadius;
    CinemachineVirtualCamera vcam;
    Image crossHair;
    Volume volume;
    Bloom bloom;
    Vignette vignette;

    Vector3 mousePosition = Vector3.zero;
    
    public override void ApplySkillEffect()
    {

        base.ApplySkillEffect();
        player = GameObject.Find("Player").GetComponent<Player>();
        debugSphere = GameObject.Find("aimsphere");
        Globals.teleportationInUse = true;
        SetTeleportationCamera();
        SetCrossHair();
        

    }
    public override void UnapplySkillEffect()
    {
        
        player.playerInstance.StartCoroutine(Teleport());

    }

    public override void UpdatableEffects()
    {
        base.UpdatableEffects();
        RayCastScreenCenter();
        
    }

    void SetCrossHair()
    {
        crossHair = GameObject.Find("TeleportationCrosshair").GetComponent<Image>();
        crossHair.enabled = true;

      

    }


    void SetTeleportationCamera()
    {
        teleportationCam = GameObject.Find("TeleportationCam");
        vcam = teleportationCam.GetComponent<CinemachineVirtualCamera>();
        vcam.Priority = 30;
        volume = Camera.main.GetComponent<Volume>();

        if (volume.profile.TryGet(out bloom) && volume.profile.TryGet(out vignette))
        {
            Color32 tintColor = new Color32(0, 93, 255, 255);
            //bloom.tint.overrideState = true;
            bloom.tint.Override(tintColor);
            bloom.intensity.Override(7.5f);
            vignette.active = true;
            Time.timeScale = 0.3f;
        }

    }

    void ResetTeleportationCamera()
    {
        vcam.Priority = 0;
        crossHair.enabled = false;
        Time.timeScale = 1f;
        Color32 tintColor = new Color32(148, 209, 255, 255);
        bloom.tint.Override(tintColor);
        bloom.intensity.Override(2f);
        vignette.active = false;
    }

    void RayCastScreenCenter()
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(centerOfScreen);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, teleportationRadius, aimColliderMask))
        {
            hitTransform = raycastHit.transform;
            debugSphere.transform.position = raycastHit.point;
            mousePosition = raycastHit.point;
            Debug.Log("Ray is active, we're looking at: " + hitTransform.name);
            Vector3 aimTarget = mousePosition;
            aimTarget.y = player.transform.position.y;
            Vector3 aimdDirection = (aimTarget - player.transform.position).normalized;

            player.transform.forward = Vector3.Lerp(player.transform.forward, aimdDirection, Time.deltaTime * 20f);

        }
        else
        {
            debugSphere.transform.position = player.transform.position;
            hitTransform = null;
        }

        if (hitTransform != null)
        {
            if (hitTransform.GetComponent<Teleportable>())
            {
                crossHair.color = Color.green;
            }
            else
            {
                crossHair.color = Color.magenta;
            }

        }
        else
        {
            crossHair.color = Color.gray;
        }
    }


    IEnumerator Teleport()
    {
        if (hitTransform != null)
        {
            if (hitTransform.GetComponent<Teleportable>())
            {//clean this up lol
                Globals.teleportationInUse = false;
                tpc = player.gameObject.GetComponent<ThirdPersonController>();
                tpc.isDisabled = true;
                Vector3 fasd = new Vector3(0, 0.2f, 0);
                GameObject spawnedVFX = Instantiate(vfx, player.transform.position + fasd, player.transform.rotation);
                VisualEffect efx = spawnedVFX.GetComponent<VisualEffect>();
                efx.Play();
                yield return new WaitForSeconds(0.3f);
                player.transform.position = mousePosition;
                efx.Stop();
                Destroy(spawnedVFX, 5f);
                yield return new WaitForSeconds(0.1f);
                tpc.isDisabled = false;
                yield return new WaitForSeconds(0.2f);
                GameObject spawnedVFX2 = Instantiate(vfx, player.transform.position + fasd, player.transform.rotation);
                VisualEffect efx2 = spawnedVFX2.GetComponent<VisualEffect>();
                efx2.Play();
                ResetTeleportationCamera();
                yield return new WaitForSeconds(0.3f);
                efx2.Stop();
                Destroy(spawnedVFX2, 5f);

            }

            else
            {
                Debug.Log("Was not teleportable, cancelling action");
                Globals.teleportationInUse = false;
                ResetTeleportationCamera();
            }

        }
        Globals.teleportationInUse = false;
        ResetTeleportationCamera();
        
    }

}
