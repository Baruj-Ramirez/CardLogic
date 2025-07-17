using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TruthHolder : MonoBehaviour
{
    private ValorVerdad valor;
    private Image image;

    [SerializeField]
    private Sprite trueSprite;

    [SerializeField]
    private Sprite falseSprite;
    void Start()
    {
        if (GetComponent<Image>() != null)
        {
            image = GetComponent<Image>();
        }
    }

    public void SetTValue(ValorVerdad valorVerdad)
    {
        if (image != null)
        {
            valor = valorVerdad;
            switch (valor)
            {
                case ValorVerdad.Verdadero:
                    image.sprite = trueSprite;
                    break;
                case ValorVerdad.Falso:
                    image.sprite = falseSprite;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Image Component NUll");
        }
    }

}
