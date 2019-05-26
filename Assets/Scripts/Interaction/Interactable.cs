using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 3f;

    private List<Transform> _focused;
    private HashSet<Transform> _interacted;
    private bool _isFocused;

    public float InteractionRadius => interactionRadius;

    protected void Awake()
    {
        _focused = new List<Transform>();
        _interacted = new HashSet<Transform>();
        Initialize();
    }

    private void Update()
    {
        if (_isFocused)
        {
            foreach (var target in _focused)
            {
                if (_interacted.Contains(target) || !target)
                    continue;
                
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance <= interactionRadius)
                {
                    Interact(target);
                    _interacted.Add(target);
                }
            }
        }
    }

    public virtual void Interact(Transform target)
    {
        //Debug.Log(this + " interacted with " + target, this);
    }
    
    public void OnFocused(Transform focus)
    {
        _isFocused = true;
        _interacted.Clear();
        
        if (!_focused.Contains(focus))
            _focused.Add(focus);
    }

    public void OnDefocused(Transform focus)
    {
        _isFocused = false;
        _interacted.Clear();
        
        _focused.Remove(focus);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    public void Refresh()
    {
        _focused.Clear();
        _interacted.Clear();
    }
    
    public abstract void Initialize();
}
