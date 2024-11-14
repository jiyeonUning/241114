using Photon.Pun;
using UnityEngine;

public class Shot : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Rigidbody rigid;


    [SerializeField] float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField] int damage;
    public int Damage { get { return Damage; } set { Damage = value; } }


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * speed;
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 총알에 닿은 물체가 플레이어 태그를 달고 있지 않을 경우, 실행하지 않는다.
        if (collision.gameObject.tag != "player") return;

        // 총알에 닿은 상대 플레이어 오브젝트를 저장한다.
        GameObject otherPL = collision.gameObject;
        // 저장한 오브젝트를 토대로 상대 플레이어의 플레이어 컨트롤러 컴포넌트를 참조한다.
        PlayerController otherCon = otherPL.GetComponent<PlayerController>();
        // 해당 컴포넌트에 저장된, 피격 시 데미지를 입는 함수를 실행하여 상대 플레이어의 체력을 차감한다.
        otherCon.TakeHit(gameObject.GetComponent<Shot>(), damage);

        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 각 총알 간 속도의 차이를 보완하기 위한 지연 보상 코드 작성

        if (stream.IsWriting)
        {
            stream.SendNext(rigid.position);
            stream.SendNext(rigid.rotation);
            stream.SendNext(rigid.velocity);
        }
        else if (stream.IsReading)
        {
            rigid.position = (Vector3) stream.ReceiveNext();
            rigid.rotation = (Quaternion) stream.ReceiveNext();
            rigid.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            rigid.position += rigid.velocity * lag;
        }
    }
}
