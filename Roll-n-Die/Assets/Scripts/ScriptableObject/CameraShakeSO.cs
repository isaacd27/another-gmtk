using UnityEngine;


[CreateAssetMenu(fileName = "CameraShakeSO", menuName = "ScriptableObjects/CameShakeScriptableObject", order = 1)]
public class CameraShakeSO : ScriptableObject
{
    public float AmplitudeGain;
    public float FrequencyGain;
    public float Duration;
    public Vector2 ControllerIntensity;
}
