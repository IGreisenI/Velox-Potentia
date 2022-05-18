using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct SpellSelectButtonInfo
{
    public string choice;
    public Sprite shape;
}

public class UISelectSpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool state = false;
    public SpellSelectButtonInfo buttonInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        state = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        buttonInfo.choice = this.gameObject.GetComponentInChildren<TMP_Text>().text;
    }
    
    public void updateButtonText(string choice)
    {
        buttonInfo.choice = choice;
        this.gameObject.GetComponentInChildren<TMP_Text>().text = choice;
    }
}
