using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("배경음악")]
    public AudioSource bgmSource;

    [Header("효과음")]
    public AudioSource sfxSource;

    [Header("씬별 배경음악 목록")]
    public List<SceneBGM> sceneBGMs;

    [Header("효과음 목록")]
    public List<SFXEntry> sfxClips;

    private Dictionary<string, AudioClip> sfxDict;

    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [System.Serializable]
    public class SFXEntry
    {
        public string name;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitializeSFXDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForCurrentScene();
    }

    private void PlayBGMForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        foreach (SceneBGM sceneBGM in sceneBGMs)
        {
            if (sceneBGM.sceneName == currentScene)
            {
                if (bgmSource.clip != sceneBGM.bgmClip)
                {
                    bgmSource.clip = sceneBGM.bgmClip;
                    bgmSource.loop = true;
                    bgmSource.Play();
                }
                return;
            }
        }

        bgmSource.Stop(); // 해당 씬에 BGM 없으면 정지
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] 효과음 '{name}'을 찾을 수 없습니다.");
        }
    }

    private void InitializeSFXDict()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sfxClips)
        {
            if (!sfxDict.ContainsKey(entry.name))
            {
                sfxDict.Add(entry.name, entry.clip);
            }
        }
    }
}
