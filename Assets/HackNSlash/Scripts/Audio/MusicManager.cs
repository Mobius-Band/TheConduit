using System;
using System.Linq;
using Eflatun.SceneReference;
using HackNSlash.ScriptableObjects;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Audio
{
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private AudioClip[] soundtrack;
        [SerializeField] private SceneReference[] correspondentScenes;
        [SerializeField] private bool simulateSceneLoading;
        private float _defaultVolume;
        private AccessData _accessData;
        private AudioSource _audioSource;

        private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        private bool _canDecrementIndex;

        public AudioSource AudioSource => _audioSource;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
            _defaultVolume = _audioSource.volume;

            if (correspondentScenes.Length != soundtrack.Length)
            {
                Debug.LogWarning("There should be the same amount of scenes and audioclips");
            }
        }

        private void Start()
        {
            _accessData = GameManager.Instance.AccessData;
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (simulateSceneLoading)
            {
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
                return;
            }
            if (GameManager.Instance.SceneManager.IsOnMainMenu())
            {
                PlayTrack(0);
                return;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (correspondentScenes.All(cs => cs.BuildIndex != CurrentSceneIndex))
            {
                return;
            }
            
            if (CurrentSceneIndex == correspondentScenes[3].BuildIndex)
            {
                PlayTrack(!_accessData.canAccessPart2 ? 1 : 3);
                return;
            }

            if (CurrentSceneIndex == correspondentScenes[5].BuildIndex)
            {
                PlayTrack(!_accessData.canAccessPart3 ? 3 : 5);
                return;
            }
            
            if (CurrentSceneIndex == correspondentScenes[7].BuildIndex)
            {
                PlayTrack(!_accessData.canAccessPart4 ? 5 : 7);
                return;
            }
            
            SceneReference correctScene = correspondentScenes.First(cs => cs.BuildIndex == CurrentSceneIndex);
            int correctIndex = Array.FindIndex(correspondentScenes, cs => correctScene.BuildIndex == cs.BuildIndex);
            PlayTrack(correctIndex);
        }

        private void PlayTrack(int index)
        {
            if (_audioSource.clip == soundtrack[index])
            {
                return;
            }
            
            _audioSource.Stop();
            _audioSource.clip = soundtrack[index];
            _audioSource.Play();
        }

        public void ResetVolume()
        {
            _audioSource.volume = _defaultVolume;
        }
    }
}
