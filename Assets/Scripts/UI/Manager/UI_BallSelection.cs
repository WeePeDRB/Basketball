using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_BallSelection : MonoBehaviour
{
    // UI Components
    [SerializeField] private Button openMenuBtn;
    [SerializeField] private GameObject ballSelectWheel;
    [SerializeField] private CanvasGroup ballSelectWheelContainer;

    // Button animation values
    private float hiddenValue = -100f;
    private float appearValue = 100f;
    private float animateDuration = 0.4f;

    // Ball select wheel animation values
    private float hiddenAlphaValue = 0f;
    private float appearAlphaValue = 1f;
    private float wheelAnimateDuration = 0.5f;

    // Implement animation
    private void OnOpenMenu()
    {
        // Animate button
        openMenuBtn.transform.DOMoveX(hiddenValue, animateDuration);

        // Animate select wheel
        //ballSelectWheel.SetActive(true);
        ballSelectWheelContainer.DOFade(appearAlphaValue, wheelAnimateDuration);
    }
    private void OnCloseMenu()
    {
        // Animate button
        openMenuBtn.transform.DOMoveX(appearValue, animateDuration);

        // Animate select wheel
        ballSelectWheelContainer.DOFade(hiddenAlphaValue, wheelAnimateDuration);
        //ballSelectWheel.SetActive(false);
    }

    //
    private void Start()
    {
        BallSelection.instance.onOpenMenu += OnOpenMenu;
        BallSelection.instance.onCloseMenu += OnCloseMenu;
    }
}
