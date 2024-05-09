using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellFrom", menuName = "ScriptableObjects/SpellProperties/SpellFrom")]
public class SpellFrom : ModifySpell
{
    public override void modifySpell(GameObject spell, string from)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(from));
    }

    public static SpellsMagicCircle castSpellFrom(List<Spell> spellObjects, Spell spell, GameObject magicCircle, Ray ray, GameObject magicCircleFrom)
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

            //Distance check
            if (Vector3.Distance(magicCircle.transform.position, hitPos) > spell.stats.range)
                hitPos = (hitPos - magicCircle.transform.position).normalized * spell.stats.range;

            if (!magicCircleFrom && !spell.stats.from.ToLower().Contains("caster"))
            {
                magicCircleFrom = Instantiate(magicCircle, hitPos + normalDir / 10, Quaternion.LookRotation(normalDir, forward));
                Destroy(magicCircleFrom.GetComponent<FixedJoint>());
                Transform magicCircleFromTransform = magicCircleFrom.transform;
                returnInfo.magicCircle = magicCircleFrom;
                spellObjects.ForEach(spellObject => Destroy(spellObject));
                spellObjects.Clear();

                for (int i = 0; i < magicCircleFromTransform.childCount; i++)
                {
                    Spell spellChild = magicCircleFromTransform.GetChild(i).GetComponent<Spell>();
                    Destroy(spellChild.GetComponent<FixedJoint>());
                    if (spellChild.TryGetComponent(out Spell spellScript))
                    {
                        spellObjects.Add(spellChild);
                    }
                }
            }
            else
            {
                if (!spell.stats.from.ToLower().Contains("caster"))
                {
                    //Distance check
                    if (Vector3.Distance(magicCircleFrom.transform.position, hitPos) > spell.stats.range)
                        hitPos = (hitPos - magicCircleFrom.transform.position).normalized * spell.stats.range;
            
                    magicCircleFrom.transform.LookAt(hit.point, Vector3.up);
                    returnInfo.magicCircle = magicCircleFrom;

                    GameObject magicCircleAim = Instantiate(magicCircle, hitPos + normalDir / 10, Quaternion.LookRotation(normalDir, forward));
                    Destroy(magicCircleAim.GetComponent<FixedJoint>());
                    Destroy(magicCircleAim, 30);
                }

                spellObjects[spellObjects.Count - 1].GetComponent<Spell>().castSpell(hitPos);
            }
        }
        else
        {
            if (spell.stats.from.ToLower().Contains("caster"))
            {
                spellObjects[spellObjects.Count - 1].GetComponent<Spell>().castSpell(ray.direction * spell.stats.range);
            }
        }
        returnInfo.spells = spellObjects;
        return returnInfo;
    }
}

public struct SpellsMagicCircle
{
    public List<Spell> spells;
    public GameObject magicCircle;
}