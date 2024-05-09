using System.Collections.Generic;
using UnityEngine;

public class MagicCircleBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>, IGameEventListener<MaxLayer>, IGameEventListener<ResetCastingInfo>
{
    public SpellSelectEvent spellSelectEvent;
    public MagicCircleBuiltEvent magicCircleBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    public ResetCastingEvent resetCastingEvent;

    public ColorListSO listOfColor;
    public GameObject magicCirclePrefab;
    GameObject magicCircle;
    
    [ColorUsage(true, true)]
    Color magicCircleColor;

    Transform magicCircleTransform;
    List<SpriteRenderer> magicCircleSprites = new List<SpriteRenderer>();

    public void OnEnable()
    {
        spellSelectEvent.RegisterListener(this);
        maxLayerEvent.RegisterListener(this);
        resetCastingEvent.RegisterListener(this);
    }

    public void OnDisable()
    {
        spellSelectEvent.UnregisterListener(this);
        maxLayerEvent.UnregisterListener(this);
        resetCastingEvent.UnregisterListener(this);
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        if (arg.layer == 0)
        {
            this.magicCircle = Instantiate(magicCirclePrefab, new Vector3(0, 0, 0), this.transform.rotation);
            this.magicCircleTransform = this.magicCircle.transform;
            this.magicCircleTransform.parent = this.transform;
            this.magicCircleTransform.localScale = Vector3.one * 2;
            this.magicCircleTransform.localPosition = new Vector3(0, 1.5f, 1.5f);
            for (int i = 0; i < magicCircleTransform.childCount; i++)
            {
                magicCircleSprites.Add(magicCircleTransform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }
        else
        {
            if(arg.layer == 1)
            {
                magicCircleColor = listOfColor.colors.Find(o => o.colorName.ToLower() == arg.buttonInfo.choice.ToLower()).color;
                magicCircleSprites[0].color = magicCircleColor;
            }
            magicCircleSprites[arg.layer - 1].sprite = arg.buttonInfo.shape;
            magicCircleSprites[arg.layer - 1].color = magicCircleColor;
        }
    }

    public void OnEventRaised(MaxLayer arg)
    {
        magicCircleBuiltEvent.Raise(new MagicCircleBuiltInfo(this.magicCircle));
    }

    public void OnEventRaised(ResetCastingInfo arg)
    {
        Destroy(this.magicCircle);
        magicCircleSprites = new List<SpriteRenderer>();
    }
}