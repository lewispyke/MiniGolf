using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAnimator : MonoBehaviour
{
    public float waitBetween = 0.15f;
    public float waitEnd = 0.22f;

    List<Animator> animators; 
    void Start()
    {
        animators = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        while(true)
        {
            foreach (var anim in animators)
            {
                anim.SetTrigger("DoPop");
                yield return new WaitForSeconds(waitBetween);
            }
            yield return new WaitForSeconds(waitEnd);
        }
    }
}
