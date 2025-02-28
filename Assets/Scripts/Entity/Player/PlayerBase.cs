using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;


public class PlayerBase : MonoBehaviour
{
    public static PlayerBase instance;

    public PlayerController playerController;
    public PlayerHUD playerHUD;
    public Camera mainCamera;
    public TextMeshProUGUI fpsText;

    private float updateFPSAccumulator = 1.0f;


    private void Awake()
    {
        instance = this;

        // TEST
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        updateFPSAccumulator += Time.deltaTime;
        if(updateFPSAccumulator >= 1.0f){
            int fps = (int)(1.0f / Time.unscaledDeltaTime);
            fpsText.text = $"FPS: {fps}";
            updateFPSAccumulator = 0.0f;
        }
    }
}

#if UNITY_EDITOR

public class PlayerGizmos {
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    public static void DrawGizmos(PlayerBase playerBase, GizmoType gizmoType){
        Gizmos.color = Color.blue;
        GizmoUtils.DrawArrow(playerBase.mainCamera.transform.position, playerBase.mainCamera.transform.forward, 1.5f);
    }
}

#endif
