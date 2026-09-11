using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float vidaMaxima = 10f;
    [SerializeField] protected float velocidade = 2f;

    [SerializeField] protected float vidaAtual;

    protected virtual void Awake()
    {
        vidaAtual = vidaMaxima;
    }

    public virtual void TomarDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    protected virtual void Morrer()
    {
        Destroy(gameObject);
    }
}
