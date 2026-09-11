using UnityEngine;

public class Objective : Entity
{
    [SerializeField] private Transform barraVerde;

    public event System.Action OnDerrota;

    void Update()
    {
        float proporcao = Mathf.Clamp01(vidaAtual / vidaMaxima);
        barraVerde.localScale = new Vector3(proporcao, barraVerde.localScale.y, barraVerde.localScale.z);
    }

    protected override void Morrer()
    {
        OnDerrota?.Invoke();
    }
}
