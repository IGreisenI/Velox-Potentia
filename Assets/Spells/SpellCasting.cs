using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpellCasting : MonoBehaviour, IGameEventListener<SpellBuiltEventInfo>, IGameEventListener<MagicCircleBuiltInfo>
{
    [SerializeField] private InputController _inputController = default;

    public SpellBuiltEvent spellBuiltEvent;
    public GameObject player;
    public Camera playerCamera;
    private bool spellBuilt;
    private GameObject spell;
    private GameObject magicCircle;
    private SpellStatsSO spellStats;

    private void OnEnable()
    {
        _inputController.castSpellInputEvent += OnCast;
        _inputController.cancelSpellInputEvent += OnCancelSpell;
        spellBuiltEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _inputController.castSpellInputEvent -= OnCast;
        _inputController.cancelSpellInputEvent -= OnCancelSpell;
        spellBuiltEvent.UnregisterListener(this);
    }

    public void OnCast()
    {
        // If spell/magic circle is not built don't execute past this point
        if (!spellBuilt || !magicCircle)
            return;

        Vector3 cameraPos = playerCamera.transform.position;
        Vector3 lookDir = playerCamera.transform.forward;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (spellStats.from.ToLower().Contains("sky/ground"))
            {
                Vector3 hitPos = hit.point;
                Vector3 normalDir = hit.normal;

                Vector3 right = Vector3.Cross(normalDir, hitPos.normalized);
                Vector3 forward = Vector3.Cross(right.normalized, normalDir);

                GameObject magicCircleCopy = Instantiate(magicCircle, hit.point, Quaternion.LookRotation(normalDir, forward));
            }
            else if (spellStats.from.ToLower().Contains("caster"))
            {
                spell.transform.position += player.transform.forward * spellStats.speed * Time.deltaTime;
            }
            else if (spellStats.from.ToLower().Contains("at"))
            {

            }
        }
    }

    public void OnCancelSpell()
    {
        spellBuilt = false;
    }

    public void OnEventRaised(SpellBuiltEventInfo args)
    {
        spellBuilt = true;

        this.spell = args.spellObject;
        spellStats = args.spellObject.GetComponentInChildren<SpellStats>().stats;
    }

    public void OnEventRaised(MagicCircleBuiltInfo args)
    {
        this.magicCircle = args.magicCircleObject;
    }
}