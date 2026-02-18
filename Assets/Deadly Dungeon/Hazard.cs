using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hazard : MonoBehaviour
{
    public List<SpriteRenderer> hazardSRs;
    public bool isInHazard = false;
    public UnityEvent OnEnterHazard;
    public UnityEvent OnExitHazard;
    bool wasInHazardLastFrame;

    void Update()
    {
        wasInHazardLastFrame = isInHazard;
        bool isInHazardThisFrame = false; //this is how we tell if the in hazard never got triggered this frame
        foreach(SpriteRenderer sr in hazardSRs)
        {
            if (sr.bounds.Contains(transform.position))
            {
                if (isInHazard)
                {
                    //still in the hazard
                    isInHazardThisFrame = true;
                }
                else
                {
                    //just entered hazard
                    OnEnterHazard.Invoke();
                    isInHazard = true;
                    //as soon as we're in one we need to skip out of the loop so we don't say
                    //oh, no we're not in this water and ignore the previous one we are in...
                    return;
                }
            }
        }

        //have to wait to the end of the loop to see if we've left the hazard though ;-)
        //we have to check all the waters, not just stop at the first false because
        //there's plenty we won't be in even if we are still swimming...
        if (wasInHazardLastFrame && !isInHazardThisFrame)
        {
            //just left the hazard
            OnExitHazard.Invoke();
            isInHazard = false;
        }
    }
}
