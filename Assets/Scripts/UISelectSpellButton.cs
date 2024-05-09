using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct SpellSelectButtonInfo
{
    public string choice;
    public Sprite shape;

    public SpellSelectButtonInfo(string choice, Sprite shape) : this()
    {
        this.choice = choice;
        this.shape = shape;
    }
}

public class UISelectSpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private bool _state;

    private SpellSelectButtonInfo _buttonInfo;
    public bool State { get=>_state; internal set=>_state=value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        State = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        State = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        _buttonInfo.choice = this.gameObject.GetComponentInChildren<TMP_Text>().text;
    }
    
    public void updateButtonText(string choice)
    {
        _buttonInfo.choice = choice;
        this.gameObject.GetComponentInChildren<TMP_Text>().text = choice;
    }

    public SpellSelectButtonInfo GetButtonInfo()
    {
        return _buttonInfo;
    }

    public void SetButtonInfo(SpellSelectButtonInfo buttonInfo)
    {
        this._buttonInfo = buttonInfo;
    }
}
