using UnityEngine;

[CreateAssetMenu(fileName = "Achivement", menuName = "ScriptableObjects/Achivement", order = 1)]
public class AchivementScriptableObject : ScriptableObject 
{
    public int achivementId;
    [Space]
    public string achivementName;
    public string achivementDescription;
    [Space]
    public Sprite achivementSprite;
}
