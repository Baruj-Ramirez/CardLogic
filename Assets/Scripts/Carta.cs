using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Carta : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ValorVerdad valor;
    public Image imagenCarta;

    public Sprite spriteVerdadero;
    public Sprite spriteFalso;

    private Transform padreOriginal;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ActualizarSprite();
    }

    public void AsignarValor(ValorVerdad nuevoValor)
    {
        valor = nuevoValor;
        ActualizarSprite();
    }

    void ActualizarSprite()
    {
        if (imagenCarta == null) imagenCarta = GetComponent<Image>();

        if (valor == ValorVerdad.Verdadero)
            imagenCarta.sprite = spriteVerdadero;
        else
            imagenCarta.sprite = spriteFalso;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        padreOriginal = transform.parent;
        transform.SetParent(transform.root); // Sacarla del layout temporalmente
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(padreOriginal);
        canvasGroup.blocksRaycasts = true;
    }
}

