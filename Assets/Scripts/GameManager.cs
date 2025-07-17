using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Proposicion proposicionActual;

    private Operador operador;

    private int indiceProposicionActual = 0;
    private ValorVerdad? resultadoAnterior = null;

    [SerializeField]
    private TruthHolder truthHolder;

    [SerializeField]
    private ConnectorHolder connectorHolder1;
    [SerializeField]
    private ConnectorHolder connectorHolder2;
    [SerializeField]
    private ConnectorHolder connectorHolder3;

    [SerializeField]
    private GameObject WinCanvas;

    public List<Carta> manoJugador = new List<Carta>(); // Reemplazar luego con sistema de UI
    public Carta prefabCarta; // Prefab para instanciar cartas
    public Transform zonaMano; // Zona donde se colocan las cartas en la UI

    private Queue<Operador> colaConectores = new Queue<Operador>();
    private List<NodoProposicion> listaProposiciones = new List<NodoProposicion>();

    public int contadorVerdaderos = 0;
    public int contadorFalsos = 0;

    public int totalProposiciones = 5; // ajustable

    private int score = 0;

    void Start()
    {
        PrepararNivel();
        //GenerarManoInicial();
        //GenerarProposicionAleatoria();
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

    public void UpdateHolders()
    {
        NodoProposicion nodoActual = listaProposiciones[indiceProposicionActual];
        truthHolder.SetTValue(nodoActual.resultado);
        if (colaConectores.Count == totalProposiciones)
        {
            connectorHolder1.SetConnector(colaConectores.Dequeue());
            connectorHolder2.SetConnector(colaConectores.Dequeue());
            connectorHolder3.SetConnector(colaConectores.Dequeue());
        }
        else
        {
            if (connectorHolder2.IsComplete())
            {
                connectorHolder1.Complete();
            }
            else
            {
                connectorHolder1.SetConnector(connectorHolder2.GetConnector());
            }
            if (connectorHolder3.IsComplete())
            {
                connectorHolder2.Complete();
            }
            else
            {
                connectorHolder2.SetConnector(connectorHolder3.GetConnector());
            }

            try
            {
                connectorHolder3.SetConnector(colaConectores.Dequeue());
            }
            catch (InvalidOperationException)
            {
                connectorHolder3.Complete();
            }
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

        if (!resultado.HasValue)
        {
            Debug.Log("Proposición incompleta.");
            if (GameObject.FindObjectsOfType<Carta>().Length <= 1)
            {
                FinalizarJuego();
            }
            return;
        }

        ValorVerdad esperado = listaProposiciones[indiceProposicionActual].resultado;

        if (resultado.Value == esperado)
        {
            Debug.Log("¡Correcto! Resultado esperado: " + esperado);
            resultadoAnterior = resultado;
            CrearCarta(resultado.Value); // Se le da al jugador el resultado como carta
            score += 1;
        }
        else
        {
            Debug.LogWarning($"Resultado incorrecto. Esperado: {esperado}, pero se obtuvo: {resultado.Value}");
            // Aquí puedes penalizar o dejar que siga
        }

        indiceProposicionActual++;
        EjecutarSiguienteProposicion(); // Cargar la siguiente
    }


    public void PrepararNivel()
    {

        listaProposiciones.Clear();
        colaConectores.Clear();
        contadorVerdaderos = 0;
        contadorFalsos = 0;

        ValorVerdad valorActual = Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso;
        ValorVerdad izquierdaInicial = Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso;
        ValorVerdad derechaInicial = Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso;
        Operador operadorInicial = Random.value > 0.5f ? Operador.Y : Operador.O;

        ValorVerdad resultadoInicial = Evaluador.Evaluar(izquierdaInicial, operadorInicial, derechaInicial);
        listaProposiciones.Add(new NodoProposicion(izquierdaInicial, derechaInicial, operadorInicial, resultadoInicial));
        colaConectores.Enqueue(operadorInicial);

        // Contadores
        AumentarContador(izquierdaInicial);
        AumentarContador(derechaInicial);

        ValorVerdad resultadoAnterior = resultadoInicial;

        for (int i = 1; i < totalProposiciones; i++)
        {
            Operador nuevoOperador = Random.value > 0.5f ? Operador.Y : Operador.O;
            ValorVerdad nuevoValor = Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso;

            ValorVerdad nuevoResultado = Evaluador.Evaluar(resultadoAnterior, nuevoOperador, nuevoValor);

            listaProposiciones.Add(new NodoProposicion(resultadoAnterior, nuevoValor, nuevoOperador, nuevoResultado));
            colaConectores.Enqueue(nuevoOperador);

            AumentarContador(nuevoValor);

            resultadoAnterior = nuevoResultado;
        }

        // Dar cartas necesarias
        DarCartasRequeridas();

        // Bonus de 2 cartas aleatorias
        CrearCarta(Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso);
        CrearCarta(Random.value > 0.5f ? ValorVerdad.Verdadero : ValorVerdad.Falso);
        proposicionActual.MostrarOperador();

        UpdateHolders();
        UpdateHolders();


        Debug.Log($"esperado: {listaProposiciones[indiceProposicionActual].resultado}");
    }
    void AumentarContador(ValorVerdad valor)
    {
        if (valor == ValorVerdad.Verdadero)
            contadorVerdaderos++;
        else
            contadorFalsos++;
    }

    void DarCartasRequeridas()
    {
        for (int i = 0; i < contadorVerdaderos; i++)
            CrearCarta(ValorVerdad.Verdadero);

        for (int i = 0; i < contadorFalsos; i++)
            CrearCarta(ValorVerdad.Falso);
    }

    public void EjecutarSiguienteProposicion()
    {
        if (indiceProposicionActual >= listaProposiciones.Count)
        {
            FinalizarJuego();
            return;
        }

        NodoProposicion nodo = listaProposiciones[indiceProposicionActual];

        proposicionActual.Limpiar();

        // Cargar el operador desde el nodo (también está en la cola)
        proposicionActual.operador = nodo.operador;
        proposicionActual.MostrarOperador();

        /*
        // Determinar la entrada izquierda
        if (nodo.izquierda.HasValue)
        {
            // Crear una carta fija en el slot izquierdo (jugador no la pone)
            Carta cartaIzquierda = CrearCartaInternamente(nodo.izquierda.Value);
            proposicionActual.slotIzquierda.ColocarCarta(cartaIzquierda);
        }
        else if (resultadoAnterior.HasValue)
        {
            Carta cartaIzquierda = CrearCartaInternamente(resultadoAnterior.Value);
            proposicionActual.slotIzquierda.ColocarCarta(cartaIzquierda);
        }

        // El slot derecho queda vacío esperando al jugador
        */
        Debug.Log($"Proposición {indiceProposicionActual + 1}/{listaProposiciones.Count} preparada.");
        Debug.Log($"esperado: {listaProposiciones[indiceProposicionActual].resultado}");
        UpdateHolders();
    }

    private Carta CrearCartaInternamente(ValorVerdad valor)
    {
        Carta carta = Instantiate(prefabCarta);
        carta.AsignarValor(valor);
        carta.GetComponent<CanvasGroup>().blocksRaycasts = false; // no se puede arrastrar
        return carta;
    }

    private void FinalizarJuego()
    {
        Debug.Log("¡Nivel completado!");
        WinCanvas.SetActive(true);
        if (WinCanvas.GetComponent<PuntajeUI>() != null)
        {
            WinCanvas.GetComponent<PuntajeUI>().ActualizarPuntaje(score, totalProposiciones);
        }
    }
}

public class NodoProposicion
{
    public ValorVerdad? izquierda; // Puede ser el resultado anterior
    public ValorVerdad derecha;    // Generado aleatoriamente
    public Operador operador;
    public ValorVerdad resultado;  // Valor esperado al resolverla

    public NodoProposicion(ValorVerdad? izquierda, ValorVerdad derecha, Operador operador, ValorVerdad resultado)
    {
        this.izquierda = izquierda;
        this.derecha = derecha;
        this.operador = operador;
        this.resultado = resultado;
    }
}

