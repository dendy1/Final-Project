using UnityEngine;

[System.Serializable]
public class ObjectForSave : ScriptableObject
{
    public string objectName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 localSize;

    public ObjectForSave(string name, Vector3 position, Vector3 rotation, Vector3 size)
    {
        objectName = name;
        this.position = position;
        this.rotation = rotation;
        localSize = size;
    }
}
