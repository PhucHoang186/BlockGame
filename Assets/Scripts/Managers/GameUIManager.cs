using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameUIManager : MonoBehaviour
{
    public static Action<bool> ON_UI_BLOCK_INPUT;
    public GameObject blockInputCanvas;
    void Start()
    {
        ON_UI_BLOCK_INPUT += SetBlockInput;
    }

    void OnDestroy()
    {
        ON_UI_BLOCK_INPUT -= SetBlockInput;
    }

    public void SetBlockInput(bool isBlock)
    {
        blockInputCanvas.SetActive(isBlock);
    }
}
