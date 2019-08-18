﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIScreenHUD : UIScreen
{
    public Text txt_TimeLeft;
    public Slider pickupSlider;

    private float totalTime;
    private float attackedNoticeTime;
    private Camera mainCam;

    protected override void InitComponent()
    {
        mainCam = Camera.main;
    }

    protected override void InitData()
    {
        totalTime = ParseDataByIndex<float>(0);
    }

    protected override void InitView()
    {

    }
    
    public override void OnClose()
    {
    }


    private void UpdateTimeLeft(object[] data)
    {
        float timeLeft = (float) data[0];
        int t = (int) timeLeft;
        txt_TimeLeft.text = (t / 60).ToString() + ":" + (t % 60).ToString();
        txt_TimeLeft.color = Color.Lerp(Color.red, Color.green, timeLeft / totalTime);
    }

    //通用的进度条
    public void CallPickProcess(object[] data)
    {
        float duration = (float)data[0];
        Action callBack = (Action)data[1];
        print(duration);
        StartCoroutine(PickProcess(duration, callBack));
    }

    private bool isPicking;
    private IEnumerator PickProcess(float duration, Action callBack)
    {
        float t = 0;
        //初始化进度条
        UIUtilities.DoFadeUI(pickupSlider.gameObject, 1, 0f, Ease.InOutBack);
        pickupSlider.value = 0;
        isPicking = true;
        while (t < duration)
        {
            yield return null;
            t += Time.deltaTime;
            pickupSlider.value = t / duration;
            if (!isPicking)
            {
                UIUtilities.DoFadeUI(pickupSlider.gameObject, 0, 0.2f, Ease.InOutBack);
                yield break;
            }
        }

        if (callBack != null)
            callBack.Invoke();
        UIUtilities.DoFadeUI(pickupSlider.gameObject, 0, 0.2f, Ease.InOutBack);
    }
}