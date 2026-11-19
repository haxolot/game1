using System.Collections;
using UnityEngine;

namespace Haxolot.Feedback
{
    /// <summary>
    /// Gestionnaire de "Hit Stop" (micro-gel du temps à l'impact).
    /// Auto-instancié automatiquement au démarrage du jeu : aucun setup manuel requis dans la scène !
    /// </summary>
    public class HitStopManager : MonoBehaviour
    {
        private static HitStopManager instance;
        public static HitStopManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("[HitStopManager]");
                    instance = go.AddComponent<HitStopManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private Coroutine hitStopRoutine;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Déclenche un gel du temps de courte durée (ex: 0.04s pour un coup normal, 0.12s pour une parade parfaite).
        /// </summary>
        public void TriggerHitStop(float duration = 0.05f)
        {
            if (duration <= 0f) return;

            if (hitStopRoutine != null)
            {
                StopCoroutine(hitStopRoutine);
            }

            hitStopRoutine = StartCoroutine(HitStopCoroutine(duration));
        }

        private IEnumerator HitStopCoroutine(float duration)
        {
            Time.timeScale = 0f;

            // WaitForSecondsRealtime continue de tourner même si Time.timeScale = 0
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
            hitStopRoutine = null;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
