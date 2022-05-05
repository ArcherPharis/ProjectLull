using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBase : MonoBehaviour
{
    [SerializeField] Sprite skillIcon;
    [SerializeField] AudioClip useAudioClip;
    [SerializeField] AudioClip stopUseAudioClip;
    [SerializeField] AudioClip finaleAudioClip;
    [SerializeField] AudioSource audioSource;
    Image image;
    public float coolDown;
    public string name;
    public int ID;


    public Sprite GetSkillIcon()
    {
        return skillIcon;
        
        
    }

    public virtual void Init()
    {

    }

    public virtual void ApplySkillEffect()
    {
        image = GameObject.Find("Cooldown").GetComponent<Image>();
        image.fillAmount = 0;
        audioSource.PlayOneShot(useAudioClip);
        Debug.Log("Base Skill in effect");

    }

    public virtual void UnapplySkillEffect()
    {
        audioSource.PlayOneShot(useAudioClip);
    }

    public virtual void UpdatableEffects()
    {

    }

    public void PlayCustomAudio()
    {
        audioSource.PlayOneShot(finaleAudioClip);
    }
}
