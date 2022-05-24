using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] private InputController _inputController = default;
    
    [SerializeField]
    public StringListSO MagicTypes;

    public MaxLayerEvent maxLayerEvent;
    public SpellSelectEvent spellSelectionEvent;
    public GameObject selection;

    private List<Transform> selectionButtons = new List<Transform>();
    private List<UISelectSpellButton> buttonScripts = new List<UISelectSpellButton>();

    int selectLayer = 0;
    // Default value
    int maxLayer = 5;
    
    private void OnEnable()
    {
        _inputController.selectSpellInputEvent += OnSelectInput;
        _inputController.cancelSpellInputEvent += OnCancelSpell;
    }

    private void OnDisable()
    {
        _inputController.selectSpellInputEvent -= OnSelectInput;
        _inputController.cancelSpellInputEvent += OnCancelSpell;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < selection.transform.childCount; i++)
        {
            selectionButtons.Add(selection.transform.GetChild(i));
            buttonScripts.Add(selectionButtons[i].GetComponentInChildren<UISelectSpellButton>());
        }
    }

    public void OnSelectInput(float select)
    {
        if (selectLayer < maxLayer)
        {
            if (select > 0)
            {
                switchCursorState(true, CursorLockMode.Confined);
                selection.SetActive(true);
            }
            else
            {
                foreach (UISelectSpellButton script in buttonScripts)
                {
                    if (script.state && script.buttonInfo.choice != "")
                    {
                        spellSelectionEvent.Raise(new SpellSelectEventInfo(script.buttonInfo, selectLayer));
                        script.state = false;
                        selectLayer += 1;
                    }
                }
                selection.SetActive(false);
                switchCursorState(false, CursorLockMode.Locked);
            }

            if (selectLayer == maxLayer - 1)
            {
                maxLayerEvent.Raise(new MaxLayer());
            }
        }
    }

    public void OnCancelSpell()
    {
        resetSelectLayer();
    }

    public void updateSelection(List<string> choices)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            buttonScripts[i].updateButtonText(choices[i]);
        }
    }

    public void setMaxSelectLayer(int max)
    {
        this.maxLayer = max;
    }

    public void resetSelectLayer()
    {
        selectLayer = 0;

        for (int i = 0; i < buttonScripts.Count; i+=2)
        {
            buttonScripts[i].updateButtonText("");
            buttonScripts[i+1].updateButtonText(MagicTypes.strings[i/2]);
        }
    }

    private void switchCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }
}
