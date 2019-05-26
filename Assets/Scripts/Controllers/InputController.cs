using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private InputField _inputField;
    private Text _wordText;
    private float _time;
    
    private void Start()
    {
        _wordText = transform.GetChild(0).GetComponent<Text>();
        _inputField = transform.GetChild(1).GetComponent<InputField>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GameManager.Instance.InputMenuOpened = true;
        
        if (!_inputField.isFocused)
        {
            _inputField.Select();
            _inputField.ActivateInputField();
        }

        if (_inputField.text == _wordText.text)
        {
            Deactivate();
            EventManager.Instance.Invoke("TypoSucceed", this, new TypoEventArgs(_time));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuildManager.Instance.CurrentTower = null;
            Deactivate();
        }
    }

    private void OnEnable()
    {
        _time = 0f;
    }

    private void Deactivate()
    {
        GameManager.Instance.InputMenuOpened = false;
        _inputField.text = "";
        Destroy(_inputField.transform.GetChild(0).gameObject);
        gameObject.SetActive(false);
    }
}
