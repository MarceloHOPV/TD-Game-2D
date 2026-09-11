using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private enum EstadoTorre
    {
        Parado,
        Atacando
    }

    [SerializeField] private Transform canhao;
    [SerializeField] private Transform pontoDeTiro;
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private float velocidadeRotacao = 200f;
    [SerializeField] private float taxaDeTiro = 1f;
    [SerializeField] private float toleranciaMira = 2f;

    private readonly List<Entity> inimigosNoAlcance = new List<Entity>();
    private EstadoTorre estadoAtual = EstadoTorre.Parado;
    private Entity alvoAtual;
    private float tempoUltimoTiro;

    void Update()
    {
        switch (estadoAtual)
        {
            case EstadoTorre.Parado:
                break;
            case EstadoTorre.Atacando:
                Atacar();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Entity inimigo = other.GetComponent<Entity>();
        if (inimigo == null)
            return;

        inimigosNoAlcance.Add(inimigo);
        AtualizarEstado();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Entity inimigo = other.GetComponent<Entity>();
        if (inimigo == null)
            return;

        inimigosNoAlcance.Remove(inimigo);
        AtualizarEstado();
    }

    private void AtualizarEstado()
    {
        inimigosNoAlcance.RemoveAll(inimigo => inimigo == null);
        estadoAtual = inimigosNoAlcance.Count > 0 ? EstadoTorre.Atacando : EstadoTorre.Parado;
    }

    private void Atacar()
    {
        inimigosNoAlcance.RemoveAll(inimigo => inimigo == null);

        if (inimigosNoAlcance.Count == 0)
        {
            estadoAtual = EstadoTorre.Parado;
            return;
        }

        alvoAtual = inimigosNoAlcance[0];

        bool mirando = GirarPara(alvoAtual.transform.position);

        if (mirando && Time.time >= tempoUltimoTiro + 1f / taxaDeTiro)
        {
            Atirar();
            tempoUltimoTiro = Time.time;
        }
    }

    private void Atirar()
    {
        GameObject bala = Instantiate(prefabBala, pontoDeTiro.position, Quaternion.identity);
        bala.GetComponent<Bullet>().DefinirAlvo(alvoAtual.transform);
    }

    private bool GirarPara(Vector3 posicaoAlvo)
    {
        Vector2 direcao = posicaoAlvo - canhao.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotacaoAlvo = Quaternion.Euler(0f, 0f, angulo);
        canhao.rotation = Quaternion.RotateTowards(canhao.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);

        return Quaternion.Angle(canhao.rotation, rotacaoAlvo) <= toleranciaMira;
    }
}
