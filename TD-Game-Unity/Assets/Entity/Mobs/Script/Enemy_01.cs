using UnityEngine;

public class Enemy_01 : Entity
{
    private enum EstadoInimigo
    {
        Andar
    }

    [SerializeField] private Transform pathContainer;

    private Transform[] waypoints;
    private Entity objetivo;
    private EstadoInimigo estadoAtual = EstadoInimigo.Andar;
    private int waypointAtual = 0;

    void Start()
    {
        if (pathContainer != null)
        {
            DefinirCaminho(pathContainer, objetivo);
        }
    }

    public void DefinirCaminho(Transform container, Entity objetivoAtual)
    {
        pathContainer = container;
        objetivo = objetivoAtual;
        waypoints = new Transform[pathContainer.childCount];
        for (int i = 0; i < pathContainer.childCount; i++)
        {
            waypoints[i] = pathContainer.GetChild(i);
        }
    }

    void Update()
    {
        switch (estadoAtual)
        {
            case EstadoInimigo.Andar:
                Andar();
                break;
        }
    }

    private void Andar()
    {
        if (waypointAtual >= waypoints.Length)
            return;

        Transform destino = waypoints[waypointAtual];
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidade * Time.deltaTime);

        if (transform.position == destino.position)
        {
            waypointAtual++;

            if (waypointAtual >= waypoints.Length)
            {
                objetivo.TomarDano(dano);
                Morrer();
            }
        }
    }
}
