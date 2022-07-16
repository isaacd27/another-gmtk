using UnityEngine;
using System.Collections;
using Cinemachine;
#if (UNITY_EDITOR || UNITY_STANDALONE)
using XInputDotNetPure;
#endif

public class BasicCameraFollow : MonoBehaviour 
{
    [SerializeField]
    CinemachineVirtualCamera cam;

#if (UNITY_EDITOR || UNITY_STANDALONE)    
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
#endif

    private void Start()
    {
        EndShake();
    }

    void Update()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex) i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
#endif
    }

    public void StartShake(CameraShakeSO Intensity)
    {
        StopAllCoroutines();
        EndShake();

        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Intensity.AmplitudeGain;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Intensity.FrequencyGain;
#if (UNITY_EDITOR || UNITY_STANDALONE)
        GamePad.SetVibration(playerIndex, Intensity.ControllerIntensity.x, Intensity.ControllerIntensity.y);
#endif
        if (Intensity.Duration > 0.0f)
        {
            StartCoroutine(WaitEndShake(Intensity.Duration));
        }
    }

    public void EndShake()
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.0f;
#if (UNITY_EDITOR || UNITY_STANDALONE)
        GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
#endif
    }

    IEnumerator WaitEndShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndShake();
    }

}

