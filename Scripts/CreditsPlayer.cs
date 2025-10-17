using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Generalisk.Credits
{
    internal class CreditsPlayer : MonoBehaviour
    {
        public RectTransform credits;
        public AudioSource music;

        private bool running = false;

        void Start() => StartCoroutine(WaitForStart());

        void Update()
        {
            if (!running) { return; }

            float t = (1f / music.clip.length) * music.time;
            float position = Mathf.Lerp(0, credits.sizeDelta.y + 640, t);
            credits.anchoredPosition = Vector2.up * position;

            if (!music.isPlaying) { SceneManager.LoadScene(0); }
        }

        IEnumerator WaitForStart()
        {
            while (!music.isPlaying)
            { yield return null; }

            running = true;
        }
    }
}
