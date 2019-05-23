using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private InputField _inputField;
    private Text _wordText;
    private Text _timeText;
    private float _time;

    private void Start()
    {
        _timeText = transform.GetChild(0).GetComponent<Text>();
        _wordText = transform.GetChild(1).GetComponent<Text>();
        _inputField = transform.GetChild(2).GetComponent<InputField>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        Utils.SetText((int)_time, _timeText, "TIME: ");
        
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
        _inputField.text = "";
        Destroy(_inputField.transform.GetChild(0).gameObject);
        gameObject.SetActive(false);   
    }
}
