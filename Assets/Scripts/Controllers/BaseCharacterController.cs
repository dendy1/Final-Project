using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{
    protected Interactable CurrentFocus;
    protected Transform Target;
    protected Unit Unit;
    
    private void Awake()
    {
        Unit = GetComponent<Unit>();
    }
    
    private void FollowTarget(Interactable newTarget)
    {
        Unit.stoppingDistance = newTarget.InteractionRadius * 0.5f;
        Target = newTarget.transform;
    }

    protected void StopFollowingTarget()
    {
        Unit.stoppingDistance = 0;
        Target = null;
    }
    
    protected void SetFocus(Interactable newFocus)
    {
        if (CurrentFocus != newFocus)
        {
            if (CurrentFocus)
            {
                CurrentFocus.OnDefocused(CurrentFocus.transform);
            }
            
            CurrentFocus = newFocus;
            FollowTarget(newFocus);
        }
        
        newFocus.OnFocused(transform);
    }

    protected void RemoveFocus()
    {
        if (CurrentFocus)
        {
            CurrentFocus.OnDefocused(CurrentFocus.transform);
        }

        CurrentFocus = null;
        StopFollowingTarget();
    }

    protected void MoveTo(Vector3 point)
    {
        Unit.SetTarget(point);
    }
}
