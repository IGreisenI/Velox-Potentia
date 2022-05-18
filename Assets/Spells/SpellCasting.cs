using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] private InputController _inputController = default;

    public GameObject spell;
    public GameObject magicCirclePrefab;
    public GameObject player;
    public SpellStatsSO spellStats;
    public Camera playerCamera;

    private void OnEnable()
    {
        _inputController.castSpellInputEvent += OnCast;
    }

    private void OnDisable()
    {
        _inputController.castSpellInputEvent -= OnCast;
    }


    public void saveFinishedSpell(GameObject spell)
    {
        this.spell = spell;
        spellStats = spell.GetComponentInChildren<SpellStats>().stats;
    }

    public void OnCast()
    {
        saveFinishedSpell(this.spell);

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

                GameObject magicCircle = Instantiate(magicCirclePrefab, hit.point, Quaternion.LookRotation(normalDir, forward));
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
}