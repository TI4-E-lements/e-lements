using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;

    private Towers parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Towers parent){
        this.target = parent.Target;
        this.parent = parent;
    }

    private void MoveToTarget(){
        if(target!=null && target.gameObject.activeSelf){

            if(transform.position == target.transform.position){
                Destroy(this);
            }
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            Vector2 dir = target.transform.position - transform.position;

            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);

        }else if(!target.gameObject.activeSelf){
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Enemy"){

            if(target.gameObject == other.gameObject){
                GameManager.Instance.Pool.ReleaseObject(gameObject);

                other.GetComponent<Enemy>().Harm(parent.Damage);
            }
            
        }
    }
}
