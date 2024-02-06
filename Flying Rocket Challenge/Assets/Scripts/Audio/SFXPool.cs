using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour
{
    private static SFXPool _instance;

    public static SFXPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SFXPool>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SFXPool");
                    _instance = singletonObject.AddComponent<SFXPool>();
                }
            }
            return _instance;
        }
    }

    private List<AudioSource> _audioSourceList;
    private int _index = 0;

    public int poolSize = 10;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        CreatePool();
    }

    private void CreatePool()
    {
        _audioSourceList = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateAudioSourceItem();
        }
    }

    private void CreateAudioSourceItem()
    {
        GameObject go = new GameObject("SFX_Pool");
        go.transform.SetParent(gameObject.transform);
        _audioSourceList.Add(go.AddComponent<AudioSource>());
    }

    public void Play(SFXType sfxType)
    {
        var sfx = SoundManager.Instance.GetSFXByType(sfxType);

        _audioSourceList[_index].clip = sfx.audioClip;
        _audioSourceList[_index].Play();

        _index++;
        if (_index >= _audioSourceList.Count) _index = 0;
    }

    public void Stop()
    {
        foreach (var audioSource in _audioSourceList)
        {
            audioSource.Stop();
        }
    }
}