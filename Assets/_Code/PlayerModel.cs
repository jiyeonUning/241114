using Photon.Pun;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviourPun
{
    public UnityAction<int> OnChangedBulletEvent;
    public void OnChangedBullet() => OnChangedBulletEvent?.Invoke(curBullet);
    public int curBullet { get; set; }
    public int maxBullet;


    public UnityAction<int> OnChangedHpEvent;
    public void OnChangedHP() => OnChangedHpEvent?.Invoke(hp);
    public int hp { get; set; }
    public int maxHp;

    // === === ===

    public float moveSpeed;
    public float bulletSpeed;
    public int MyDamage;
}
