using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spell : MonoBehaviour
{
    [SerializeField]
    public SpellStats stats = new SpellStats();
    public Rigidbody rigidBody;
    public Transform spellTransform;
    public LayerSO elementLayer;

    public GameObject model;
    public Transform modelTransform;
    public MeshCollider meshCollider;
    public Unit caster;
    public bool casted = false;

    private float figureSpeed = 1.5f;
    private Vector3 castedFromPoint;
    private List<float> damageMultipliers = new List<float> { 1f, 0.75f, 0.5f, 0.25f, 0f };

    public void Initialize(Transform parent, Vector3 spellPosOffset, string choice, Unit casterUnit)
    {
        transform.parent = parent;
        transform.localPosition = spellPosOffset;
        stats.spellType = choice;
        caster = casterUnit;
    }

    private void OnEnable()
    {
        this.castedFromPoint = this.spellTransform.position;
    }

    void FixedUpdate()
    {
        // If spell reaches it's range it gets destroyed
        if (this.casted)
        {
            if (this.rangeCheck() && this.stats.spellType.ToLower().Contains("offensive"))
            {
                Destroy(this.gameObject);
            }   

            if (this.stats.shape.ToLower().Contains("figure"))
            {
                transform.position += new Vector3(0, figureSpeed) * Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        if (!casted && this.stats.spellType == "defensive" && this.stats.position != "")
        {
            modelTransform.rotation = stats.defensiveRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!casted) return;

        if (collision.gameObject.TryGetComponent<Unit>(out Unit unit) && this.stats.spellType.ToLower() != "defensive")
        {
            this.unitCollide(unit, this);
        }
        else if (collision.gameObject.TryGetComponent<Spell>(out Spell spell) && this.stats.spellType.ToLower() == "defensive")
        {
            spellCollide(spell);
        }
        else if (this.stats.spellType.ToLower() != "defensive")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!casted) return;

        if (other.TryGetComponent<Unit>(out Unit unit) && this.stats.spellType.ToLower() != "defensive")
        {
            unit.changeHealth(stats.damage);
            Destroy(this.gameObject);
        }
        else if (other.TryGetComponent<Spell>(out Spell spell) && this.stats.spellType.ToLower() == "defensive")
        {
            spellCollide(spell);
        }
    }

    public void castSpell(Vector3 At)
    {
       
        rigidBody = this.gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        this.spellTransform.SetParent(null);

        this.castedFromPoint = this.spellTransform.position;
        this.casted = true;
        modelTransform.localScale = stats.scaleOnCast;

        if (stats.shape.Contains("projectile"))
        {
            this.spellTransform.LookAt(At);
            this.castedFromPoint = this.spellTransform.position;
            this.rigidBody.velocity = this.transform.forward * stats.speed;
        }
        else if (stats.shape.Contains("burst"))
        {
            StartCoroutine(explosion(At));
        }
        else if (stats.shape.Contains("figure"))
        {
            figure(At);
        }
        else if (stats.shape.Contains("flow"))
        {
            transform.position = At;
            this.stats.damage /= 1 * 4;

            StartCoroutine(flowEnergy(At, 1f, 4f));
        }
        else if (stats.spellType.ToLower().Contains("defensive"))
        {
            if (stats.shape.ToLower().Contains("bubble"))
            {
                transform.position += At;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

                foreach (Collider nearbyObject in colliders)
                {
                    // Check if any of targets 
                    if (nearbyObject.TryGetComponent<Unit>(out Unit unit))
                    {
                        meshCollider.isTrigger = true;
                        transform.parent = unit.gameObject.transform;
                        this.transform.localPosition = new Vector3(0, 0.9f, 0);
                    }
                }

                if (colliders.Length <= 0)
                {
                    Destroy(this);
                }
            }
            else
            {
                transform.position = At;
                this.rigidBody.isKinematic = true;
            }


            Destroy(gameObject, 20);
        }
    }

    private bool rangeCheck()
    {
        return Vector3.Distance(castedFromPoint, this.spellTransform.position) > stats.range;
    }

    private IEnumerator explosion(Vector3 At)
    {
        transform.position = At;

        GetComponentInChildren<VisualEffect>().visualEffectAsset = Resources.Load<VisualEffectAsset>("Particles/burst of energyParticles");
        yield return new WaitForSeconds(4f);

        Collider[] colliders = Physics.OverlapSphere(At, 10);

        foreach (Collider nearbyObject in colliders)
        {
            if(nearbyObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(500, At + new Vector3(0, 0.5f), 5);
            }
            if(nearbyObject.TryGetComponent<Unit>(out Unit unit))
            {
                unitCollide(unit, this);
            }
        }

        Destroy(gameObject, 10);
    }

    private void figure(Vector3 At)
    {
        this.rigidBody.isKinematic = false;
        this.transform.position = At;
    }

    public IEnumerator flowEnergy(Vector3 At, float tick, float duration)
    {
        float passedTime = 0;
        GetComponentInChildren<VisualEffect>().visualEffectAsset = Resources.Load<VisualEffectAsset>("Particles/flow of energyParticles");

        while (passedTime < duration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

            foreach (Collider nearbyObject in colliders)
            {
                if (nearbyObject.TryGetComponent<Unit>(out Unit unit))
                {
                    unitCollide(unit, this);
                }
            }

            passedTime += tick;
            yield return new WaitForSeconds(tick);
        }

        Destroy(gameObject);
    }

    public void spellCollide(Spell spell)
    {
        int defIndex = elementLayer.choices().IndexOf(this.stats.element);
        int offIndex = elementLayer.choices().IndexOf(spell.stats.element);
        int x = offIndex - defIndex;
        if (x < 0)
        {
            x += 4;
        }

        spell.stats.damage *= damageMultipliers[x];
        if (spell.stats.damage <= 0)
        {
            Destroy(spell.gameObject);
        }
    }

    public void unitCollide(Unit unit, Spell spell)
    {
        unit.changeHealth(-stats.damage);
        if (unit.health <= 0)
        {
            caster.killReward();
        }
        else
        {
            switch (spell.stats.element.ToLower())
            {
                case "Fire":
                    //burn damage
                    StartCoroutine(unit.changeHealthOverTime(-spell.stats.damage / 10f, 1, 5));
                    break;
                case "Air":
                    //Reduce speed
                    StartCoroutine(unit.reduceSpeed(50.0f));
                    break;
                case "Water":
                    //stop mana regen
                    StartCoroutine(unit.stopBasicManaRegen());
                    break;
                case "Grass":
                    //stop health reward from kills
                    StartCoroutine(unit.stopKillHealthRegen());
                    break;
                case "Earth":
                    //weakness
                    break;
            }
        }
        Destroy(this.gameObject);
    }
}
