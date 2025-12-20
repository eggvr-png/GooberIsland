using UnityEngine;
using UnityEngine.Audio;

public class CoolAssAudioMuffler : MonoBehaviour
{
    [Header("References")]
    public Transform listenerTransform;
    
    [Header("Muffling Stuff")]
    [Tooltip("Angle in degrees - audio is clear within this cone")]
    [Range(0f, 180f)]
    public float angle = 90f;
    
    [Tooltip("How quickly the muffle effect transitions")]
    [Range(0.1f, 10f)]
    public float transitionSpeed = 5f;
    
    [Header("Low Pass Filter Thingy Settings")]
    [Range(10f, 22000f)]
    public float clearCutoffFreq = 22000f;
    
    [Range(10f, 22000f)]
    public float muffledCutoffFreq = 800f;
    
    float resonanceQ = 1f;
    
    private AudioSource audioSrc;
    private AudioLowPassFilter lowPassFilter;
    private float targetCutoff;
    private float currentCutoff;
    
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        
        if (audioSrc == null)
        {
            Debug.LogError("No AudioSource found on " + gameObject.name);
            enabled = false;
            return;
        }

        lowPassFilter = audioSrc.GetComponent<AudioLowPassFilter>();
        
        currentCutoff = clearCutoffFreq;
        lowPassFilter.cutoffFrequency = currentCutoff;
    }
    
    void Update()
    {
        if (listenerTransform == null || audioSrc == null) return;
        
        bool isFacing = IsListenerFacingSource();
        targetCutoff = isFacing ? clearCutoffFreq : muffledCutoffFreq;
        
        currentCutoff = Mathf.Lerp(currentCutoff, targetCutoff, Time.deltaTime * transitionSpeed);
        lowPassFilter.cutoffFrequency = currentCutoff;
        lowPassFilter.lowpassResonanceQ = resonanceQ;
    }
    
    bool IsListenerFacingSource()
    {
        Vector3 dirToSource = (transform.position - listenerTransform.position).normalized;
        Vector3 listenerForward = listenerTransform.forward;
        
        float dot = Vector3.Dot(listenerForward, dirToSource);
        float dotThreshold = Mathf.Cos(angle * Mathf.Deg2Rad);
        
        return dot >= dotThreshold;
    }
    
    void OnDrawGizmosSelected()
    {
        if (listenerTransform == null) return;
        
        Gizmos.color = IsListenerFacingSource() ? Color.green : Color.red;
        Gizmos.DrawLine(listenerTransform.position, transform.position);
    
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}