using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;


public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;
        

    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeAmplitude = 2f;
    [SerializeField] float shakeFrequency = 10f;

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;
    bool isShaking;


    // SINGLETON
    void Awake() {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }


    public static void Shake() {
        if (instance == null) return;
        instance._Shake();
    }

    

    

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
    }

    void _Shake() {
        Debug.Log("shake");
        // Set the camera shake properties
        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;

        // Start a coroutine to stop the camera shake after the specified duration
        StartCoroutine(StopShakingAfterDelay());
    }

    private IEnumerator StopShakingAfterDelay()
    {
        yield return new WaitForSeconds(shakeDuration);

        // Reset the camera shake properties
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

    
    
}
