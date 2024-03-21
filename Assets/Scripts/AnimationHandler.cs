using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private TMP_Text repeatCountTxt;
    [SerializeField] private TMP_Text animationNameTxt;
    public int repeatCount = 3;

    private int currentGestureIndex = 0;
    private string[] gestureNames = { "Walk", "Jump", "Crouch", "Crawl", "LeftHandRaise", "RightHandRaise" };

    private void Start()
    {
        StartCoroutine(PlayGestureAnimation());
    }

    public void NextGesture()
    {
        currentGestureIndex = (currentGestureIndex + 1) % gestureNames.Length;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        StopAllCoroutines();
        StartCoroutine(PlayGestureAnimation());
    }

    public void PreviousGesture()
    {
        if (currentGestureIndex == 0) currentGestureIndex = gestureNames.Length - 1;
        else currentGestureIndex--;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        StopAllCoroutines();
        StartCoroutine(PlayGestureAnimation());
    }

    private IEnumerator PlayGestureAnimation()
    {
        animationNameTxt.text = gestureNames[currentGestureIndex];

        for (int i = 1; i <= repeatCount; i++)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            characterAnimator.Play(gestureNames[currentGestureIndex], -1, 0f);

            AnimationClip[] clips = characterAnimator.runtimeAnimatorController.animationClips;
            float length = 0;
            foreach (AnimationClip clip in clips)
            {
                if (gestureNames[currentGestureIndex] == clip.name)
                    length = clip.averageDuration;
            }
            Debug.Log(i + " " + gestureNames[currentGestureIndex] + " " + length);

            yield return new WaitForSeconds(length);
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        characterAnimator.Play("Idle", 0, 0f);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetRepeatCount(float value)
    {
        repeatCount = Mathf.RoundToInt(value);
        repeatCountTxt.text = repeatCount.ToString();
    }
}
