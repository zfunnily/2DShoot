using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : 
MonoBehaviour, 
IPointerEnterHandler, 
IPointerDownHandler,
ISelectHandler, 
ISubmitHandler
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayerSFX(selectSFX);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayerSFX(submitSFX);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlayerSFX(selectSFX);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlayerSFX(submitSFX);
    }
}
