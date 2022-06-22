using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellFrom", menuName = "ScriptableObjects/SpellProperties/SpellFrom")]
public class SpellFrom : ModifySpell
{
    public override void modifySpell(GameObject spell, string from)
    {
        Spell spellLogic = spell.gameObject.GetComponentInChildren<Spell>();
        spellLogic.stats.from = from;
    }

    public static SpellsMagicCircle castSpellFrom(List<GameObject> spellObjects, Spell spell, GameObject magicCircle, Ray ray, GameObject magicCircleFrom)
    {
        SpellsMagicCircle returnInfo = new SpellsMagicCircle();
        returnInfo.magicCircle = magicCircleFrom;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (spell.stats.from.ToLower().Contains("sky/ground"))
            {
                if (hit.collider.gameObject.tag.ToLower() != "ground" && hit.collider.gameObject.tag.ToLower() != "sky")
                {
                    return returnInfo;
                }
            }

            Vector3 hitPos = hit.point;
            Vector3 normalDir = hit.normal;

            Vector3 right = Vector3.Cross(normalDir, hitPos.normalized);
            Vector3 forward = Vector3.Cross(right.normalized, normalDir);

            if (!magicCircleFrom && !spell.stats.from.ToLower().Contains("caster"))
            {
                magicCircleFrom = Instantiate(magicCircle, hit.point + normalDir / 10, Quaternion.LookRotation(normalDir, forward));
                Destroy(magicCircleFrom.GetComponent<FixedJoint>());
                Transform magicCircleFromTransform = magicCircleFrom.transform;
                returnInfo.magicCircle = magicCircleFrom;
                spellObjects.ForEach(spellObject => Destroy(spellObject));
                spellObjects = new List<GameObject>();

                for (int i = 0; i < magicCircleFromTransform.childCount; i++)
                {
                    GameObject spellChild = magicCircleFromTransform.GetChild(i).gameObject;
                    Destroy(spellChild.GetComponent<FixedJoint>());
                    if (spellChild.TryGetComponent(out Spell spellScript))
                    {
                        spellObjects.Add(spellChild);
                    }
                }
            }
            else
            {
                if (spell.stats.from.ToLower().Contains("sky/ground"))
                {
                    GameObject magicCircleAim = Instantiate(magicCircle, hit.point + normalDir / 10, Quaternion.LookRotation(normalDir, forward));
                    Destroy(magicCircleAim.GetComponent<FixedJoint>());
                    Destroy(magicCircleAim, 30);
                }
                spellObjects[spellObjects.Count - 1].GetComponent<Spell>().castSpell(hit.point);
                spellObjects.RemoveAt(spellObjects.Count - 1);
            }
        }
        else
        {
            if (spell.stats.from.ToLower().Contains("caster"))
            {
                spellObjects[spellObjects.Count - 1].GetComponent<Spell>().castSpell(ray.direction * 10000);
                spellObjects.RemoveAt(spellObjects.Count - 1);
            }
        }
        returnInfo.spells = spellObjects;
        return returnInfo;
    }
}

public struct SpellsMagicCircle
{
    public List<GameObject> spells;
    public GameObject magicCircle;
}