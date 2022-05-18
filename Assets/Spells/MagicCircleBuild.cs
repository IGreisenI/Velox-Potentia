using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicCircleBuild : MonoBehaviour, IGameEventListener<SpellSelectEventInfo>
{
    public SpellSelectEvent spellSelectEvent;

    public GameObject magicCirclePrefab;
    GameObject magicCircle;

    public Transform playerCamera;
    Transform magicCircleTransform;
    List<SpriteRenderer> magicCircleSprites = new List<SpriteRenderer>();

    public void OnEnable()
    {
        spellSelectEvent.RegisterListener(this);
    }

    public void OnDisable()
    {
        spellSelectEvent.RegisterListener(this);
    }

    public void Start()
    {
        
    }

    public void OnEventRaised(SpellSelectEventInfo arg)
    {
        if (arg.layer == 0)
        {
            magicCircle = Instantiate(magicCirclePrefab, new Vector3(0, 0, 0), this.transform.rotation);
            magicCircleTransform = magicCircle.transform;
            magicCircleTransform.parent = this.transform;
            magicCircleTransform.localScale = new Vector3(2,2,2);
            magicCircleTransform.localPosition = new Vector3(0, 1.5f, 1.5f);
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
}