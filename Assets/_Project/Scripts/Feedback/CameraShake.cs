using System.Collections;
using UnityEngine;

namespace Haxolot.Feedback
{
    /// <summary>
    /// Gestionnaire de secousses d'écran (Screen Shake).
    /// Secoue la caméra principale lors des impacts lourds et des déviations Sekiro.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        private static CameraShake instance;
        public static CameraShake Instance
        {
            get
            {
                if (instance == null)
                {
                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        instance = cam.gameObject.GetComponent<CameraShake>();
                        if (instance == null)
                        {
                            instance = cam.gameObject.AddComponent<CameraShake>();
                        }
                    }
                }
                return instance;
            }
        }

        private Transform cameraTransform;
        private Vector3 originalLocalPos;
        private Coroutine shakeRoutine;

        private void Awake()
        {
            instance = this;
            cameraTransform = transform;
            originalLocalPos = cameraTransform.localPosition;
        }

        /// <summary>
        /// Déclenche une secousse de caméra paramétrable.
        /// </summary>
        /// <param name="intensity">Amplitude du tremblement (ex: 0.15f pour un coup standard, 0.35f pour un coup lourd)</param>
        /// <param name="duration">Durée de la secousse en secondes</param>
        public void Shake(float intensity = 0.15f, float duration = 0.12f)
        {
            if (cameraTransform == null)
            {
                if (Camera.main != null) cameraTransform = Camera.main.transform;
                else return;
            }

            if (shakeRoutine != null)
            {
                StopCoroutine(shakeRoutine);
            }

            shakeRoutine = StartCoroutine(ShakeCoroutine(intensity, duration));
        }

        private IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Calcul d'un décalage aléatoire fluide
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;

                // Application sur l'axe local (en ignorant l'axe Z de la caméra 2D)
                cameraTransform.localPosition = new Vector3(originalLocalPos.x + x, originalLocalPos.y + y, originalLocalPos.z);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            cameraTransform.localPosition = originalLocalPos;
            shakeRoutine = null;
        }
    }
}
