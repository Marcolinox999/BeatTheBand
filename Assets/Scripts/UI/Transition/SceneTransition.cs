using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private string stateName = "Transition"; // Name of the state in the Animator
    [SerializeField] private float fallbackDuration = 1f;     // Used if the clip length can't be read

    private float clipDuration;
    private bool isTransitioning;
    private Coroutine currentRoutine;

    private void Awake()
    {
        // Singleton: keep only one transition object alive
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (transitionAnimator == null)
        {
            transitionAnimator = GetComponentInChildren<Animator>();
        }

        clipDuration = GetClipDuration();

        // Start with the screen uncovered (first frame of the clip)
        transitionAnimator.Play(stateName, 0, 0f);
        transitionAnimator.speed = 0f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (isTransitioning) return;

        currentRoutine = StartCoroutine(ExitRoutine(sceneIndex));
    }

    private IEnumerator ExitRoutine(int sceneIndex)
    {
        isTransitioning = true;

        // Play the clip forward: screen gets covered
        transitionAnimator.Play(stateName, 0, 0f);
        transitionAnimator.speed = 1f;

        yield return new WaitForSecondsRealtime(clipDuration);

        // Freeze on the last frame (screen fully covered) while the scene loads
        transitionAnimator.speed = 0f;

        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only the first scene (no previous exit) starts uncovered, so skip it
        if (!isTransitioning) return;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(EnterRoutine());
    }

    private IEnumerator EnterRoutine()
    {
        // Play the same clip backwards: screen gets uncovered
        transitionAnimator.Play(stateName, 0, 1f);
        transitionAnimator.speed = -1f;

        yield return new WaitForSecondsRealtime(clipDuration);

        transitionAnimator.speed = 0f;
        isTransitioning = false;
    }

    private float GetClipDuration()
    {
        if (transitionAnimator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = transitionAnimator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0) return clips[0].length;
        }

        return fallbackDuration;
    }
}