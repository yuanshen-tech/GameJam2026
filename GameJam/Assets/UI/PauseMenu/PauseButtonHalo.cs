using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PauseButtonHalo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup glow;
    private bool hovered;

    public void OnPointerEnter(PointerEventData eventData) => hovered = true;

    public void OnPointerExit(PointerEventData eventData) => hovered = false;

    private void Update()
    {
        glow.alpha = Mathf.MoveTowards(glow.alpha, hovered ? 1f : 0f, Time.unscaledDeltaTime * 10f);
    }

    private void OnDisable()
    {
        hovered = false;
        glow.alpha = 0f;
    }
}
