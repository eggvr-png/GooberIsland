using UnityEngine;
[CreateAssetMenu(fileName = "Grabbable Preset", menuName = "ScriptableObjects/Grabbable Preset", order = 1)]
public class PresetMaker : ScriptableObject
{
    [Header("Mass")]
    public float mass;
    [Tooltip("The amount of force to move the object to the grab point")] public float force = 100f;
    [Tooltip("How long it takes to slow down")] public float damping = 0.98f;
}
