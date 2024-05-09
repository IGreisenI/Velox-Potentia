using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "SpellShape", menuName = "ScriptableObjects/SpellProperties/SpellShape")]
public class SpellShape : ModifySpell
{
    public ColorListSO listOfColor;

    public override void modifySpell(GameObject spell, string shape)
    {
        Spell spellScript = spell.gameObject.GetComponentInChildren<Spell>();
        spellScript.stats.addStats(this.choiceStats.getModifier(shape));

        Mesh mesh = Resources.Load<Mesh>("Mesh/" + spellScript.stats.shape);
        spell.gameObject.GetComponentInChildren<MeshFilter>().mesh = mesh;
        spell.gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = mesh;

        Material resourceMat = Resources.Load<Material>("Shaders/Spell/" + spellScript.stats.shape + "_Mat");

        Color color = listOfColor.colors.Find(o => o.colorName.ToLower() == spellScript.stats.element.ToLower()).color;
        if (resourceMat != null)
        {
            Material mat = new Material(resourceMat);
            mat.color = color;
            mat.SetColor("Color", color);
            MeshRenderer _renderer = spell.GetComponentInChildren<MeshRenderer>();
            _renderer.material = mat;
        }

        VisualEffectAsset vfxAsset = Resources.Load<VisualEffectAsset>("Particles/" + spellScript.stats.shape + "Particles");

        if (vfxAsset != null && !shape.ToLower().Contains("burst") && !shape.ToLower().Contains("flow"))
        {
            spell.GetComponentInChildren<VisualEffect>().visualEffectAsset = vfxAsset;
            spell.GetComponentInChildren<VisualEffect>().SetVector4("Color", color);
        }
    }
}