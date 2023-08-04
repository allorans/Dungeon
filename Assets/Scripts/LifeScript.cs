using System;
using UnityEngine;
using EventArgs;

public class LifeScript : MonoBehaviour
{

public event EventHandler<DamageEventArgs> OnDamage;

public event EventHandler<HealEventArgs> OnHeal;

    public int maxHealth;

   [HideInInspector] public bool isVulnerable=true;
    public int health;

  public delegate bool CanInflictDamageDelegate(GameObject attacker,int damage);
  public CanInflictDamageDelegate canInflictDamageDelegate;

    public GameObject healingPrefab;
    // Start is called before the first frame update
    void Start()
    {
      health=maxHealth;  
    }

public void InflictDamage(GameObject attacker, int damage){
  if(isVulnerable){ 
  
  bool? canInflictDamage=canInflictDamageDelegate?.Invoke(attacker,damage);
  if(canInflictDamage.HasValue&&canInflictDamage.Value==false)return;
  
  health-=damage;
  OnDamage?.Invoke(this,new DamageEventArgs(){
    damage=damage,attacker=attacker
  });
  }
}

public void Heal(){
  health=maxHealth;
  if(healingPrefab!=null){
    var effect=Instantiate(healingPrefab,transform.position,healingPrefab.transform.rotation);
    effect.transform.SetParent(transform);
    Destroy(effect,5);
  }

  OnHeal?.Invoke(this,new HealEventArgs());

}
public bool IsDead(){
  return health<=0f;
}

}
