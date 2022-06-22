using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicCircleBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>, IGameEventListener<MaxLayer>, IGameEventListener<ResetCastingInfo>
{
    public SpellSelectEvent spellSelectEvent;
    public MagicCircleBuiltEvent magicCircleBuiltEvent;
    public MaxLayerEvent maxLayerEvent;
    public ResetCastingEvent resetCastingEvent;

    public GameObject magicCirclePrefab;
    GameObject magicCircle;

    public Transform playerCamera;
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

    public void Start()
    {
        
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        if (arg.layer == 0)
        {
            this.magicCircle = Instantiate(magicCirclePrefab, new Vector3(0, 0, 0), this.transform.rotation);
            this.magicCircleTransform = this.magicCircle.transform;
            this.magicCircleTransform.parent = this.transform;
            this.magicCircleTransform.localScale = new Vector3(2, 2, 2);
            this.magicCircleTransform.localPosition = new Vector3(0, 1.5f, 1.5f);
            this.magicCircle.AddComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
            for (int i = 0; i < magicCircleTransform.childCount; i++)
            {
                magicCircleSprites.Add(magicCircleTransform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }
        else
        {
            magicCircleSprites[arg.layer - 1].sprite = arg.buttonInfo.shape;
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