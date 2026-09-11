using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] pontosDeSpawn;
    [SerializeField] private float tempoEntreOndas = 10f;

    public event System.Action OnOndaIniciada;
    public event System.Action<float> OnIntervaloEntreOndasIniciado;
    public event System.Action OnVitoria;

    private Transform pathContainer;
    private Entity objetivo;
    private FaseSO faseAtual;
    private MultiplicadorDificuldade multiplicadorAtual;
    private readonly List<GameObject> mobsDaOndaAtual = new List<GameObject>();
    private bool pularEspera;

    public void PularEspera()
    {
        pularEspera = true;
    }

    void Start()
    {
        pathContainer = GameObject.Find("Enemy_Path").transform;
        objetivo = GameObject.Find("Objective").GetComponent<Entity>();

        FaseSO[] fasesDisponiveis = Resources.LoadAll<FaseSO>("");
        string nomeCena = SceneManager.GetActiveScene().name;
        faseAtual = System.Array.Find(fasesDisponiveis, f => f.name == nomeCena);

        if (faseAtual == null)
        {
            Debug.LogError($"Nenhuma FaseSO encontrada para a cena '{nomeCena}'.");
            return;
        }

        multiplicadorAtual = System.Array.Find(faseAtual.multiplicadores, m => m.dificuldade == faseAtual.dificuldadeEscolhida);

        StartCoroutine(SpawnarOndas());
    }

    private IEnumerator SpawnarOndas()
    {
        for (int indiceOnda = 0; indiceOnda < faseAtual.ordas.Length; indiceOnda++)
        {
            Onda onda = faseAtual.ordas[indiceOnda];

            OnOndaIniciada?.Invoke();
            mobsDaOndaAtual.Clear();

            float intervalo = faseAtual.intervaloBaseSpawn * Mathf.Pow(1f - multiplicadorAtual.aceleracaoPorOnda, onda.numero - 1);

            foreach (MobOnda mob in onda.mobs)
            {
                int quantidade = Mathf.RoundToInt(mob.quantidade * multiplicadorAtual.multiplicadorQuantidade);

                for (int i = 0; i < quantidade; i++)
                {
                    Transform pontoSorteado = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
                    GameObject mobInstanciado = Instantiate(mob.prefab, pontoSorteado.position, Quaternion.identity);
                    mobInstanciado.GetComponent<Enemy_01>().DefinirCaminho(pathContainer, objetivo);
                    mobsDaOndaAtual.Add(mobInstanciado);
                    yield return new WaitForSeconds(intervalo);
                }
            }

            while (mobsDaOndaAtual.Exists(mob => mob != null))
            {
                yield return null;
            }

            bool ultimaOnda = indiceOnda == faseAtual.ordas.Length - 1;

            if (ultimaOnda)
            {
                OnVitoria?.Invoke();
            }
            else
            {
                pularEspera = false;
                OnIntervaloEntreOndasIniciado?.Invoke(tempoEntreOndas);

                float tempoRestante = tempoEntreOndas;

                while (tempoRestante > 0f && !pularEspera)
                {
                    tempoRestante -= Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
