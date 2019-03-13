using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;

    public float speed = 70f;

    public int damage = 50;

    public float explosionRadius = 0f;
    public string impactEffect_tag;
    GameObject effectIns;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            setEffect();
            ReUse();
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    void HitTarget()
    {
        //GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        setEffect();
        Debug.Log("hit");

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        ReUse();
        //Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {

        Enemy _enemy = enemy.GetComponent<Enemy>();

        if(_enemy !=null)
        _enemy.TakkeDamager(damage);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void ReUse()
    {
         this.gameObject.SetActive(false);
    }

    private void EffectIns()
    {
        effectIns.SetActive(false);
    }

    private void setEffect()
    {
        effectIns = ObjectPool.Instance.SpawnFromPool(impactEffect_tag, transform.position, transform.rotation);
        Invoke("EffectIns", 2.5f);
    }

}
