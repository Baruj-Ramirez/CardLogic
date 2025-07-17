using UnityEngine;
using UnityEngine.UI;

public class Proposicion : MonoBehaviour
{
    public Slot slotIzquierda;
    public Slot slotDerecha;
    public Operador operador;

    public Image operadorImage;
    public Sprite spriteY;
    public Sprite spriteO;


    public ValorVerdad? Evaluar()
    {
        if (slotIzquierda.cartaColocada == null || slotDerecha.cartaColocada == null)
            return null;

        ValorVerdad a = slotIzquierda.cartaColocada.valor;
        ValorVerdad b = slotDerecha.cartaColocada.valor;

        switch (operador)
        {
            case Operador.Y:
                return (a == ValorVerdad.Verdadero && b == ValorVerdad.Verdadero)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;
            case Operador.O:
                return (a == ValorVerdad.Verdadero || b == ValorVerdad.Verdadero)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;
        }

        return null;
    }

    public void Limpiar()
    {
        slotIzquierda.Vaciar();
        slotDerecha.Vaciar();
    }

    public void MostrarOperador()
    {
        if (operadorImage == null) return;

        switch (operador)
        {
            case Operador.Y:
                operadorImage.sprite = spriteY;
                break;
            case Operador.O:
                operadorImage.sprite = spriteO;
                break;
        }
    }
}
