using UnityEngine;

public enum Dificuldade
{
    Easy,
    Medium,
    Hard,
    Nightmare
}

[CreateAssetMenu(fileName = "NovaFase", menuName = "TD/Fase")]
public class FaseSO : ScriptableObject
{
    public int numeroFase = 1;
    public Dificuldade dificuldadeEscolhida;
    public float intervaloBaseSpawn = 4f;

    public MultiplicadorDificuldade[] multiplicadores = new MultiplicadorDificuldade[]
    {
        new MultiplicadorDificuldade { dificuldade = Dificuldade.Easy, multiplicadorVida = 1f, multiplicadorVelocidade = 1f, multiplicadorQuantidade = 1f, aceleracaoPorOnda = 0.15f },
        new MultiplicadorDificuldade { dificuldade = Dificuldade.Medium, multiplicadorVida = 1.3f, multiplicadorVelocidade = 1.1f, multiplicadorQuantidade = 1.2f, aceleracaoPorOnda = 0.20f },
        new MultiplicadorDificuldade { dificuldade = Dificuldade.Hard, multiplicadorVida = 1.6f, multiplicadorVelocidade = 1.2f, multiplicadorQuantidade = 1.5f, aceleracaoPorOnda = 0.30f },
        new MultiplicadorDificuldade { dificuldade = Dificuldade.Nightmare, multiplicadorVida = 2f, multiplicadorVelocidade = 1.3f, multiplicadorQuantidade = 1.8f, aceleracaoPorOnda = 0.35f }
    };

    public Onda[] ordas = new Onda[]
    {
        new Onda
        {
            numero = 1,
            mobs = new MobOnda[]
            {
                new MobOnda { nome = "inimigo 1", quantidade = 5 },
                new MobOnda { nome = "inimigo 2", quantidade = 2 }
            }
        },
        new Onda
        {
            numero = 2,
            mobs = new MobOnda[]
            {
                new MobOnda { nome = "inimigo 1", quantidade = 3 },
                new MobOnda { nome = "inimigo 3", quantidade = 1 }
            }
        }
    };
}

[System.Serializable]
public class MultiplicadorDificuldade
{
    public Dificuldade dificuldade;
    public float multiplicadorVida = 1f;
    public float multiplicadorVelocidade = 1f;
    public float multiplicadorQuantidade = 1f;
    public float aceleracaoPorOnda = 0.15f;
}

[System.Serializable]
public class Onda
{
    public int numero;
    public MobOnda[] mobs;
}

[System.Serializable]
public class MobOnda
{
    public string nome;
    public GameObject prefab;
    public int quantidade;
}
