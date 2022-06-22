using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    public SpellStats stats = new SpellStats();
    public Rigidbody rigidBody;
    public Transform spellTransform;
    public bool casted = false;

    private Vector3 castedFromPoint;

    private void OnEnable()
    {
        this.castedFromPoint = this.spellTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If spell reaches it's range it gets destroyed
        if (this.casted && this.rangeCheck())
        {
            Destroy(this.gameObject);
        }
    }

    public void castSpell(Vector3 At)
    {
        this.spellTransform.SetParent(null);
        Destroy(GetComponent<FixedJoint>());
        this.spellTransform.LookAt(At);

        this.casted = true;
        if (stats.spellType.ToLower() == "offensive")
        {
            this.castedFromPoint = this.spellTransform.position;
            this.rigidBody.velocity = this.transform.forward * stats.speed;
        }
    }

    private bool rangeCheck()
    {
        return Vector3.Distance(castedFromPoint, this.spellTransform.position) > stats.range;
    }

    private void explosion()
    {

    }
}
