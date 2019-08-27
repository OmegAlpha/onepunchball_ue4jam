using System.Collections;
using SRF;
using UnityEngine;

public class OPB_SoundsManager : MonoSingleton<OPB_SoundsManager>
{
    [SerializeField]
    private AudioClip[] clipsMusic;
    
    [SerializeField]
    private AudioClip clipVoice_MainTitle;
    
    [SerializeField]
    private AudioClip clipVoice_NewBall;
    
    [SerializeField]
    private AudioClip clipVoice_MakeItCount;
    
    [SerializeField]
    private AudioClip clipVoice_Winner;
    
    [SerializeField]
    private AudioClip clipVoice_Death;
    
    [SerializeField]
    private AudioClip clipVoice_ShotWoosh;
    
    [SerializeField]
    private AudioClip clipVoice_Point;

    private IEnumerator coroutineBGMusic;

    private AudioSource as_BGMMusic;
    
    private AudioSource as_Voices;
    
    private void Awake()
    {
        as_BGMMusic = gameObject.AddComponent<AudioSource>();
        as_BGMMusic.spatialize = false;
        as_BGMMusic.volume = 0.5f;
        
        as_Voices = gameObject.AddComponent<AudioSource>();
        as_Voices.spatialize = false;
        
        PlayRandomMusic();
    }

    public void PlaySFX_NewBall()
    {
        as_Voices.PlayOneShot(clipVoice_NewBall);
    }
    
    public void PlaySFX_MakeItcount()
    {
        as_Voices.PlayOneShot(clipVoice_MakeItCount);
    }
    
    public void PlaySFX_Death()
    {
        as_Voices.PlayOneShot(clipVoice_Death);
    }
    
    public void PlaySFX_Winner()
    {
        as_Voices.PlayOneShot(clipVoice_Winner);
    }
    
    public void PlaySFX_Woosh()
    {
        as_Voices.PlayOneShot(clipVoice_ShotWoosh);
    }
    
    public void PlaySFX_Point()
    {
        as_Voices.PlayOneShot(clipVoice_Point);
    }
    
    public void PlaySFX_MainTitle()
    {
        as_Voices.PlayOneShot(clipVoice_MainTitle);
    }

    public void PlayRandomMusic()
    {
        return;
        if (coroutineBGMusic != null)
        {
            StopCoroutine(coroutineBGMusic);
            coroutineBGMusic = null;
        }

        coroutineBGMusic = BackGroundMusic_Routine();
        StartCoroutine(coroutineBGMusic);
    }

    public IEnumerator BackGroundMusic_Routine()
    {
        while (true)
        {
            if(as_BGMMusic.isPlaying)
                as_BGMMusic.Stop();
            
            AudioClip getRandomClip = clipsMusic.Random();

            as_BGMMusic.PlayOneShot(getRandomClip);
            
            yield return new WaitForSeconds(getRandomClip.length);
        }
    }
}
