using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// 사운드를 관리하는 SoundManager
public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public static SoundManager instance;


    // 배경음 리스트 
    public AudioClip[] bgmList;

    private void Awake()
    {
        // Singleton 패턴으로 SoundManager 인스턴스 유지
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // 재생 메소드 
    public void PlaySound(int clipIndex) {
        audioSource.clip = bgmList[clipIndex];
        audioSource.loop = true;
        audioSource.Play();
    }

  
}
