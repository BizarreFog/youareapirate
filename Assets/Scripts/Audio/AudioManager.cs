using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    //[HideInInspector]
    public AudioMixerGroup midPriorityGroup;
    //[HideInInspector]
    public AudioMixerGroup highPriorityGroup;


    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //[HideInInspector]
    public MixerControl mixerControl;



    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        mixerControl = GetComponent<MixerControl>();

        foreach (Sound s in sounds)
        {
            
            switch(s.SoundType)
            {
                case Sound.soundType.midPriority:
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.outputAudioMixerGroup = midPriorityGroup;
                    break;
                case Sound.soundType.highPriority:
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.outputAudioMixerGroup = highPriorityGroup;
                    break;
                case Sound.soundType.UI:
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.outputAudioMixerGroup = midPriorityGroup;
                    s.source.ignoreListenerPause = true;
                    s.source.spatialBlend = 0.0f;
                    break;
            }

            if(s.clip == null)
            {
                s.source.clip = Resources.Load<AudioClip>(s.fileName);
            }
            else
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }


        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.name, objectPool);
        }
        
    }

    public static AudioManager Get()
    {
        return instance;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = UnityEngine.Random.Range(0.5f, 1.0f);
        s.source.Play();
    }

    public AudioClip GetClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.clip;
    }

    public void PauseSound()
    {
        foreach (Sound s in sounds)
            s.source.Pause();
        AudioListener.pause = true;
    }

    public void ResumeSound()
    {
        foreach (Sound s in sounds)
            s.source.UnPause();
        AudioListener.pause = false;
    }



    public void PlayFromPool(string name)
    {
        string tag = "Sound";

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary doesn't contain correct tag. Use 'Sound'.");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        if(s.clip == null || s == null || s.source.clip == null)
        {
            obj.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(name);
        }
        else
        obj.GetComponent<AudioSource>().clip = s.clip;
        obj.GetComponent<AudioSource>().volume = s.volume;
        //obj.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.5f, 1.0f);
         
        switch(s.SoundType)
        {
            case Sound.soundType.midPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
            case Sound.soundType.highPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = highPriorityGroup;
                break;
            case Sound.soundType.UI:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
        }

        obj.GetComponent<AudioSource>().Play();

        poolDictionary[tag].Enqueue(obj);
        
        
    }


    public void PlayFromPoolRandomized(string name)
    {
        string tag = "Sound";

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary doesn't contain correct tag. Use 'Sound'.");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        if (s.clip == null || s == null)
        {
            obj.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(name);
        }
        else
            obj.GetComponent<AudioSource>().clip = s.clip;
        obj.GetComponent<AudioSource>().volume = s.volume;
        obj.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.5f, 1.0f);

        switch (s.SoundType)
        {
            case Sound.soundType.midPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
            case Sound.soundType.highPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = highPriorityGroup;
                break;
            case Sound.soundType.UI:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
        }

        obj.GetComponent<AudioSource>().Play();

        poolDictionary[tag].Enqueue(obj);


    }

    public void PlayFromPool(string name, Vector3 location)
    {
        string tag = "Sound";

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary doesn't contain correct tag. Use 'Sound'.");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = location;
        obj.GetComponent<AudioSource>().clip = s.clip;
        obj.GetComponent<AudioSource>().volume = s.volume;
        obj.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.5f, 1.0f);

        switch (s.SoundType)
        {
            case Sound.soundType.midPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                obj.GetComponent<AudioSource>().spatialBlend = 1.00f;
                break;
            case Sound.soundType.highPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = highPriorityGroup;
                obj.GetComponent<AudioSource>().spatialBlend = 1.00f;
                break;
            case Sound.soundType.UI:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
        }

        obj.GetComponent<AudioSource>().Play();

        poolDictionary[tag].Enqueue(obj);


    }

    public void PlayFromPool(string name, double time)
    {
        string tag = "Sound";

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary doesn't contain correct tag. Use 'Sound'.");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.GetComponent<AudioSource>().clip = s.clip;
        obj.GetComponent<AudioSource>().volume = s.volume;

        switch (s.SoundType)
        {
            case Sound.soundType.midPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
            case Sound.soundType.highPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = highPriorityGroup;
                break;
            case Sound.soundType.UI:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
        }

        obj.GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + time);
        Debug.Log("Played " + s.name);

        poolDictionary[tag].Enqueue(obj);


    }

    public void PlayFromPool(string name, double time, Vector3 location)
    {
        string tag = "Sound";

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary doesn't contain correct tag. Use 'Sound'.");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = location;
        obj.GetComponent<AudioSource>().clip = s.clip;
        obj.GetComponent<AudioSource>().volume = s.volume;

        switch (s.SoundType)
        {
            case Sound.soundType.midPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                obj.GetComponent<AudioSource>().spatialBlend = 1.0f;
                break;
            case Sound.soundType.highPriority:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = highPriorityGroup;
                obj.GetComponent<AudioSource>().spatialBlend = 1.0f;
                break;
            case Sound.soundType.UI:
                obj.GetComponent<AudioSource>().outputAudioMixerGroup = midPriorityGroup;
                break;
        }

        obj.GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + time);

        poolDictionary[tag].Enqueue(obj);
    }

}
