using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float dano = 1f;
    [SerializeField] private float velocidade = 10f;

    private Transform alvo;

    public void DefinirAlvo(Transform alvoBala)
    {
        alvo = alvoBala;
    }

    void Update()
    {
        if (alvo == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Entity inimigo = other.GetComponent<Entity>();
        if (inimigo != null)
        {
            inimigo.TomarDano(dano);
        }

        Destroy(gameObject);
    }
}
