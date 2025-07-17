public static class Evaluador
{
    public static ValorVerdad Evaluar(ValorVerdad izquierda, Operador operador, ValorVerdad derecha)
    {
        switch (operador)
        {
            case Operador.Y:
                return (izquierda == ValorVerdad.Verdadero && derecha == ValorVerdad.Verdadero)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;

            case Operador.O:
                return (izquierda == ValorVerdad.Verdadero || derecha == ValorVerdad.Verdadero)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;
            /*
            case Operador.XOR:
                return (izquierda != derecha)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;

            case Operador.NAND:
                return (izquierda == ValorVerdad.Verdadero && derecha == ValorVerdad.Verdadero)
                    ? ValorVerdad.Falso : ValorVerdad.Verdadero;

            case Operador.NOR:
                return (izquierda == ValorVerdad.Falso && derecha == ValorVerdad.Falso)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;

            case Operador.IMPLICA:
                return (izquierda == ValorVerdad.Verdadero && derecha == ValorVerdad.Falso)
                    ? ValorVerdad.Falso : ValorVerdad.Verdadero;

            case Operador.SIYSOLOSI:
                return (izquierda == derecha)
                    ? ValorVerdad.Verdadero : ValorVerdad.Falso;
            */
            default:
                throw new System.Exception("Operador lógico no soportado.");
        }
    }
}
