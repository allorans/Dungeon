using Behaviors;
using UnityEngine;


namespace BossBattle{
public class BossDefeated: State
{
    
    public BossDefeated():base("BossDefeated"){}
        // Start is called before the first frame update
        public override void Enter()
        {
            base.Enter();
 
    
            var gameManager=GameManager.Instance;
            var boss=gameManager.boss;
            var sequencePrefab=gameManager.bossDeathSequence;
            Object.Instantiate(sequencePrefab,boss.transform.position,sequencePrefab.transform.rotation);
           
            
        }

        public override void Exit()
        {
            base.Exit();
        
        }
    
}

}