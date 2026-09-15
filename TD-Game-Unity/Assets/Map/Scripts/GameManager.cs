using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Spawn))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeCounterText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScreenText;
    [SerializeField] private GameObject botaoSkip;

    private Spawn spawn;
    private Objective objetivo;
    private Player player;
    private Coroutine contagemAtual;
    private bool autoAtivo;
    private bool jogoAcabou;

    public void DefinirAuto(bool ativo)
    {
        autoAtivo = ativo;

        if (autoAtivo && contagemAtual != null)
        {
            spawn.PularEspera();
        }
    }

    void Awake()
    {
        spawn = GetComponent<Spawn>();
        objetivo = GameObject.Find("Objective").GetComponent<Objective>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        moneyText.text = $"Money\n{Mathf.RoundToInt(player.GetMoney())}";
    }

    void OnEnable()
    {
        spawn.OnOndaIniciada += LimparContador;
        spawn.OnIntervaloEntreOndasIniciado += IniciarContagem;
        spawn.OnVitoria += MostrarVitoria;
        objetivo.OnDerrota += MostrarDerrota;
    }

    void OnDisable()
    {
        spawn.OnOndaIniciada -= LimparContador;
        spawn.OnIntervaloEntreOndasIniciado -= IniciarContagem;
        spawn.OnVitoria -= MostrarVitoria;
        objetivo.OnDerrota -= MostrarDerrota;
    }

    private void MostrarVitoria()
    {
        if (jogoAcabou)
            return;

        jogoAcabou = true;
        LimparContador();
        endScreenText.text = "Victory";
        endScreen.SetActive(true);
    }

    private void MostrarDerrota()
    {
        if (jogoAcabou)
            return;

        jogoAcabou = true;
        LimparContador();
        endScreenText.text = "You Lose";
        endScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void LimparContador()
    {
        if (contagemAtual != null)
        {
            StopCoroutine(contagemAtual);
        }

        timeCounterText.text = "";
        botaoSkip.SetActive(false);
    }

    private void IniciarContagem(float duracao)
    {
        if (autoAtivo)
        {
            spawn.PularEspera();
            return;
        }

        botaoSkip.SetActive(true);
        contagemAtual = StartCoroutine(Contar(duracao));
    }

    private IEnumerator Contar(float duracao)
    {
        float tempoRestante = duracao;

        while (tempoRestante > 0f)
        {
            timeCounterText.text = $"Next Wave\n{Mathf.CeilToInt(tempoRestante)}";
            yield return null;
            tempoRestante -= Time.deltaTime;
        }

        timeCounterText.text = "";
    }
}
