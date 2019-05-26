using UnityEngine;

[System.Serializable]
public struct GameSettings
{
    [SerializeField] private int startGold;
    [SerializeField] private float startHealth;
    [SerializeField] private float waveBreak;

    public int StartGold => startGold;
    public float StartHealth => startHealth;
    public float WaveBreak => waveBreak;
}
