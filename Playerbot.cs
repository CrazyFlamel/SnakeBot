using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBehaviours/Playerbot")]
public class Playerbot : AIBehaviour
{
    GameObject[] orbs;
    GameObject[] bots;
    GameObject[] bodies;

    GameObject orb, bot;
    bool danger = false;
    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
    }

    public override void Execute()
    {
        AI();
    }

    //ia basica, move, muda de direcao e move
    void AI()
    {
        CheckDanger();

        if (danger)
        {
            orb = null;

            Debug.Log("Danger");

            owner.transform.position = Vector2.MoveTowards(owner.transform.position, bot.transform.position, -ownerMovement.speed * Time.deltaTime);
            float distance = Vector2.Distance(owner.transform.position, bot.transform.position);

            Vector3 dir;
            dir = bot.transform.position - owner.transform.position;
            dir.z = 0.0f;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rotation, ownerMovement.speed * Time.deltaTime);

            if (distance > 3 || !bot)
            {
                CheckDanger();
                danger = false;
            }
        }
        else
        {
            if (orb)
            {
                CheckDanger();

                owner.transform.position = Vector2.MoveTowards(owner.transform.position, orb.transform.position, ownerMovement.speed * Time.deltaTime);

                direction = orb.transform.position - owner.transform.position;
                direction.z = 0.0f;

                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rotation, ownerMovement.speed * Time.deltaTime);
            }
            else
            {
                CheckDanger();
                orbs = GameObject.FindGameObjectsWithTag("Orb");
                GetClosestOrb();
            }
        }
    }

    void CheckDanger()
    {
        bodies = GameObject.FindGameObjectsWithTag("Body");
        bots = GameObject.FindGameObjectsWithTag("Bot");

        foreach (GameObject t in bots)
        {
            float dist = Vector2.Distance(t.transform.position, owner.transform.position);

            if (!t.transform.IsChildOf(owner.transform.parent) || !t.transform.IsChildOf(ownerMovement.transform.parent))
            {
                if (dist < 4)
                {
                    danger = true;
                    bot = t;
                }
            }
            else
            {
                bot = null;
                danger = false;
            }
        }

        foreach (GameObject t in bodies)
        {
            float dist = Vector2.Distance(t.transform.position, owner.transform.position);

            if (!t.transform.IsChildOf(owner.transform.parent) || !t.transform.IsChildOf(ownerMovement.transform.parent))
            {
                if (dist < 4)
                {
                    danger = true;
                    bot = t;
                }
            }
            else
            {
                bot = null;
                danger = false;
            }
        }
    }

    //void CheckDanger()
    //{
    //    Collider2D hit = Physics2D.OverlapCircle(owner.transform.position, 10);

    //    if (hit.CompareTag("Body") && (!hit.transform.IsChildOf(owner.transform.parent) || !hit.transform.IsChildOf(ownerMovement.transform.parent)))
    //    {
    //        danger = true;
    //        bot = hit.transform.gameObject;

    //    }

    //    if(hit)
    //    {
    //        Debug.Log(hit.name);
    //    }
    //}

    void GetClosestOrb()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject t in orbs)
        {
            float dist = Vector2.Distance(t.transform.position, owner.transform.position);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        orb = closest;
    }
}
