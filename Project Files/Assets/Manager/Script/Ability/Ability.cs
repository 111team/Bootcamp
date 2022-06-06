using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptible Objects/Ability")]

public class Ability : ScriptableObject
{
    public string name;
    public float manaAmount;
}
