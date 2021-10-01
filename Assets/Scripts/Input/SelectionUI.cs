using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SelectionEvent : UnityEvent<string, string, int> { }

public class SelectionUI : MonoBehaviour
{
    #region StringsForButtons
    [SerializeField]
    public StringListSO MagicTypes;
    [SerializeField]
    public StringListSO Element;
    [SerializeField]
    public StringListSO Shape;
    [SerializeField]
    public StringListSO From;
    [SerializeField]
    public StringListSO Range;
    [SerializeField]
    public StringListSO Position;
    [SerializeField]
    public StringListSO Type;
    [SerializeField]
    public StringListSO Duration;
    [SerializeField]
    public StringListSO Target;
    #endregion

    private List<Transform> selectionButtons = new List<Transform>();

    public SelectionEvent selectionEvent;

    int selectLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            selectionButtons.Add(this.gameObject.transform.GetChild(i));
        }
    }

    public void OnSelectInput(float select)
    {
        if (select == 1)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform button in selectionButtons)
            {
                UISelectSpellButton buttonScript = button.GetComponentInChildren<UISelectSpellButton>();
                if (buttonScript.state && buttonScript.choice != "")
                {
                    selectionEvent.Invoke(buttonScript.choice, buttonScript.shape, selectLayer);
                    button.GetComponentInChildren<UISelectSpellButton>().state = false;
                    selectLayer += 1;
                }
            }
            this.gameObject.SetActive(false);
        }
        
    }

    public void updateSelection(string layer)
    {
        for (int i = 0; i < selectionButtons.Count; i++)
        {
            selectionButtons[i].transform.GetComponentInChildren<UISelectSpellButton>().updateButtonText(((StringListSO)GetType().GetField(layer).GetValue(this)).strings[i]);
        }
    }

    public void resetSelectLayer()
    {
        selectLayer = 0;

        for (int i = 0; i < selectionButtons.Count; i+=2)
        {
            selectionButtons[i].transform.GetComponentInChildren<UISelectSpellButton>().updateButtonText("");
            selectionButtons[i+1].transform.GetComponentInChildren<UISelectSpellButton>().updateButtonText(MagicTypes.strings[i/2]);
        }
    }
}
