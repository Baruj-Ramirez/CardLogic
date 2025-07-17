using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ConnectorHolder : MonoBehaviour
{
    private Operador operador;
    private Image image;

    [SerializeField]
    private Sprite orSprite;

    [SerializeField]
    private Sprite andSprite;

    [SerializeField]
    private Sprite completeSprite;

    private bool clear = false;
    void Start()
    {
        if (GetComponent<Image>() != null)
        {
            image = GetComponent<Image>();
        }
    }

    public Operador GetConnector()
    {
        return operador;
    }

    public void SetConnector(Operador connector)
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            operador = connector;
            switch (operador)
            {
                case Operador.Y:
                    image.sprite = andSprite;
                    break;
                case Operador.O:
                    image.sprite = orSprite;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Image Component NUll");
        }
    }

    public bool IsComplete()
    {
        return clear;
    }

    public void Complete()
    {
        if (image != null)
        {
            image.sprite = completeSprite;
        }
        else
        {
            Debug.LogWarning("Image Component NUll");
        }
        clear = true;
    }

}
