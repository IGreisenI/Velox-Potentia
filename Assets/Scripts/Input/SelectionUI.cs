using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionUI : MonoBehaviour, IGameEventListener<ResetCastingInfo>
{
    [SerializeField] private InputController _inputController = default;
    
    [SerializeField]
    private StringListSO MagicTypes;

    public MaxLayerEvent maxLayerEvent;
    public SpellSelectEvent spellSelectionEvent;
    public ResetCastingEvent resetCastingEvent;

    public GameObject selection;

    private List<Transform> selectionButtons = new List<Transform>();
    private List<UISelectSpellButton> buttonScripts = new List<UISelectSpellButton>();

    public int selectLayer = 0;
    // Default value
    public int maxLayer = 5;
    
    private void OnEnable()
    {
        _inputController.selectSpellInputEvent += OnSelectInput;
        _inputController.cancelSpellInputEvent += OnCancelSpell;
        resetCastingEvent.RegisterListener(this);

    }

    private void OnDisable()
    {
        _inputController.selectSpellInputEvent -= OnSelectInput;
        _inputController.cancelSpellInputEvent -= OnCancelSpell;
        resetCastingEvent.UnregisterListener(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonScripts.AddRange(selection.GetComponentsInChildren<UISelectSpellButton>());
    }

    public void OnSelectInput(float select)
    {
        if (selectLayer >= maxLayer) return;

        if (select > 0)
        {
            SetUI(true, CursorLockMode.Confined);
        }
        else
        {
            foreach (UISelectSpellButton script in buttonScripts)
            {
                if (script.State && script.GetButtonInfo().choice != "")
                {
                    spellSelectionEvent.Raise(new SpellSelectEventInfo(script.GetButtonInfo(), selectLayer));
                    script.State = false;
                    selectLayer += 1;
                }
            }
            SetUI(false, CursorLockMode.Locked);
        }

        if (selectLayer == maxLayer)
        {
            maxLayerEvent.Raise(new MaxLayer());
        }
    }

    #region HelperMethods
    private void SetUI(bool active, CursorLockMode cursorState)
    {
        SwitchCursorState(active, cursorState);
        selection.SetActive(active);
    }
    #endregion HelperMethods

    public void OnCancelSpell()
    {
        resetCastingEvent.Raise(new ResetCastingInfo());
    }

    public void UpdateSelection(List<string> choices)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            buttonScripts[i].updateButtonText(choices[i]);
        }
    }

    public void SetMaxSelectLayer(int max)
    {
        this.maxLayer = max;
    }

    public void ResetSelectLayer()
    {
        selectLayer = 0;

        for (int i = 0; i < buttonScripts.Count; i+=2)
        {
            buttonScripts[i].updateButtonText("");
            buttonScripts[i+1].updateButtonText(MagicTypes.strings[i/2]);
        }
    }

    private void SwitchCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }

    public void OnEventRaised(ResetCastingInfo arg)
    {
        ResetSelectLayer();
    }
}
