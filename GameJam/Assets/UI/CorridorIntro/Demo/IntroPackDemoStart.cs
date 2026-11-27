using UnityEngine;

// Only used in the standalone preview scene.
public sealed class IntroPackDemoStart : MonoBehaviour
{
    private void Awake()
    {
        CorridorIntroController.RequestForScene(gameObject.scene.path);
    }
}
