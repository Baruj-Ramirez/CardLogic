using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public Carta cartaColocada;

    public void OnDrop(PointerEventData eventData)
    {
        if (cartaColocada != null)
            return; // Solo una carta por slot

        Carta carta = eventData.pointerDrag.GetComponent<Carta>();
        if (carta != null)
        {
            carta.transform.SetParent(this.transform);
            carta.transform.position = transform.position;
            cartaColocada = carta;
        }
    }

    public void Vaciar()
    {
        if (cartaColocada != null)
        {
            Destroy(cartaColocada.gameObject);
            cartaColocada = null;
        }
    }
}

