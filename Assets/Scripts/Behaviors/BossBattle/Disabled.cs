using Behaviors;
namespace BossBattle{
public class Disabled: State
{
    public Disabled():base("Disabled"){}
        // Start is called before the first frame update
        public override void Enter()
        {
            base.Enter();
            GameManager.Instance.boss.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            GameManager.Instance.boss.SetActive(true);
        }
    
}

}