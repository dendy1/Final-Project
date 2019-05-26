using UnityEditorInternal;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Restrictions")] 
    [SerializeField] private float zoomMinimumFOV;
    [SerializeField] private float zoomMaximumFOV;
    
    [Header("Camera GameSettings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float zoomSensetivity;
    [SerializeField] private float borderOffset;
    
    [SerializeField] private bool freeze;

    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;
    private Vector3 _newCameraPosition;
    private Vector3 _lastMousePosition;

    private Camera _camera;
    private float _zoom;
    
    private void Awake()
    {
        _defaultPosition = transform.position;
        _defaultRotation = transform.rotation;
        _camera = GetComponent<Camera>();
        _zoom = _camera.fieldOfView;
    }

    void Update()
    {
        if (freeze || GameManager.Instance.ShopMenuOpened || GameManager.Instance.InputMenuOpened)
            return;
        
        var mouse = Input.mousePosition;
        
        _zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensetivity;
        _zoom = Mathf.Clamp(_zoom, zoomMinimumFOV, zoomMaximumFOV);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _zoom, Time.deltaTime * zoomSensetivity);

        Vector3 desiredPosition = transform.position;
        
        float cameraSpeed = movementSpeed * Time.deltaTime;

        if (Input.GetKey("w") || mouse.y > Screen.height - borderOffset)
        {
            desiredPosition += new Vector3(0f, 0f, cameraSpeed);
        }
        
        if (Input.GetKey("a") || mouse.x < borderOffset)
        {
            desiredPosition += new Vector3(-cameraSpeed, 0f, 0f);
        }
    
        if (Input.GetKey("s") || mouse.y < borderOffset)       
        {
            desiredPosition += new Vector3(0f, 0f, -cameraSpeed);
        }
    
        if (Input.GetKey("d") || mouse.x > Screen.width - borderOffset)
        {
            desiredPosition += new Vector3(cameraSpeed, 0f, 0f);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        
        if (Input.GetKey("space"))
        {
            transform.position = _defaultPosition;
            transform.rotation = _defaultRotation;
        }

        _lastMousePosition = mouse;
    }
}
