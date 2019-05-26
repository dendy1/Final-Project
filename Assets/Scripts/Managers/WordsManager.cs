using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WordsManager : MonoBehaviour
{
    private struct LanguageSettings
    {
        public enum Language
        {
            Russian, English
        }

        public static string Name(Language type)
        {
            switch (type)
            {
                case Language.English:
                    return "en";
                case Language.Russian:
                    return "ru";
                default:
                    throw new Exception("Invalid language");
            }
        }
    }
    
    [SerializeField] private LanguageSettings.Language language;
    [Header("Input panel settings")]
    [SerializeField] private GameObject inputContainer;

    public static WordsManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("WordsManger is existing!");
            return;
        }
        
        Instance = this;
    }

    private string GetRandomWord()
    {
        string lang = LanguageSettings.Name(language) + ".txt";
        string path = "Assets/Resources/Words/" + lang;
        
        int currentLine = 1;
        string pick = null;
        foreach (string line in File.ReadLines(path)) 
        {
            if (Random.Range(0, currentLine) == 0) {
                pick = line;
            }
            ++currentLine;
        }   
        return pick;
    }

    public void ShowInputPanel()
    {
        string word = GetRandomWord();
        
        Text wordText = inputContainer.transform.GetChild(0).GetComponent<Text>();
        wordText.text = word;
        
        Text placeholder = inputContainer.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        placeholder.text = "Type " + word + "...";

        inputContainer.SetActive(true);
    }
}

