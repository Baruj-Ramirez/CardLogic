using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuntajeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text puntajeTexto;

    public void ActualizarPuntaje(int puntajeActual, int total)
    {
        puntajeTexto.text = $"Aciertos: {puntajeActual}/{total}";
    }

    public void Recargar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}