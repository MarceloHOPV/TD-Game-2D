using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] pontosDeSpawn;
    [SerializeField] private float tempoEntreOndas = 10f;

    private Transform pathContainer;
    private FaseSO faseAtual;
    private MultiplicadorDificuldade multiplicadorAtual;

    void Start()
    {
        pathContainer = GameObject.Find("Enemy_Path").transform;

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
        foreach (Onda onda in faseAtual.ordas)
        {
            float intervalo = faseAtual.intervaloBaseSpawn * Mathf.Pow(1f - multiplicadorAtual.aceleracaoPorOnda, onda.numero - 1);

            foreach (MobOnda mob in onda.mobs)
            {
                int quantidade = Mathf.RoundToInt(mob.quantidade * multiplicadorAtual.multiplicadorQuantidade);

                for (int i = 0; i < quantidade; i++)
                {
                    Transform pontoSorteado = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
                    GameObject mobInstanciado = Instantiate(mob.prefab, pontoSorteado.position, Quaternion.identity);
                    mobInstanciado.GetComponent<Enemy_01>().DefinirCaminho(pathContainer);
                    yield return new WaitForSeconds(intervalo);
                }
            }

            yield return new WaitForSeconds(tempoEntreOndas);
        }
    }
}
