using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : SingletonMonoBehaviour<GameUI>
{
    public TextMeshProUGUI status;

    void Start()
    {
        ClearStatus();
    }

    public void UpdateStatus(string _text)
    {
        status.text = _text;
    }

    public void ClearStatus()
    {
        status.text = "";
    }
}
