using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Proposicion proposicionActual;

    private Operador operador;

    public List<Carta> manoJugador = new List<Carta>(); // Reemplazar luego con sistema de UI
    public Carta prefabCarta; // Prefab para instanciar cartas
    public Transform zonaMano; // Zona donde se colocan las cartas en la UI

    void Start()
    {
        GenerarManoInicial();
        GenerarProposicionAleatoria();
    }

    public void GenerarManoInicial()
    {
        // Ejemplo: 2 cartas "Verdadero", 2 "Falso"
        for (int i = 0; i < 2; i++)
        {
            CrearCarta(ValorVerdad.Verdadero);
            CrearCarta(ValorVerdad.Falso);
        }
    }

    void CrearCarta(ValorVerdad valor)
    {
        Carta nuevaCarta = Instantiate(prefabCarta, zonaMano);
        nuevaCarta.AsignarValor(valor);
        manoJugador.Add(nuevaCarta);
    }

    public void GenerarProposicionAleatoria()
    {
        proposicionActual.Limpiar();

        operador = (Random.value > 0.5f) ? Operador.Y : Operador.O; // aleatorio
        proposicionActual.operador = operador;

        proposicionActual.MostrarOperador();
    }

    public void EvaluarProposicion()
    {
        ValorVerdad? resultado = proposicionActual.Evaluar();

        if (resultado.HasValue)
        {
            Debug.Log("Resultado de la proposición: " + resultado.Value);
            CrearCarta(resultado.Value); // Dar al jugador una carta con ese valor
            proposicionActual.Limpiar(); // Limpia los slots después
            GenerarProposicionAleatoria();
        }
        else
        {
            Debug.Log("La proposición no está completa.");
        }
    }
}
