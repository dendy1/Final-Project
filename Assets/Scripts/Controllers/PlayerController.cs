using UnityEngine;

public class PlayerController : BaseCharacterController
{
    public enum CameraType
    {
        FPS,
        TOP
    }
    
    [SerializeField] private bool autoClick;
    [SerializeField] private float visionRadius;

    private float _repairCD;

    private PlayerStats _stats;
    private CameraType _cameraType = CameraType.TOP;

    private void Update()
    {
        bool leftButton, rightButton;
        leftButton = autoClick ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        rightButton = autoClick ? Input.GetMouseButton(1) : Input.GetMouseButtonDown(1);

        if (GameManager.Instance.InputMenuOpened || GameManager.Instance.ShopMenuOpened)
            return;
        
        if (leftButton)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Interactable newTarget = hit.collider.GetComponent<Interactable>();

                if (newTarget)
                {
                    SetFocus(newTarget);
                }
                else
                {
                    if (_cameraType == CameraType.TOP)
                    {
                        MoveTo(hit.point);
                        RemoveFocus();
                    }
                }
            }
        }

        _repairCD -= Time.deltaTime;
    }
}