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
        // �Ѿ˿� ���� ��ü�� �÷��̾� �±׸� �ް� ���� ���� ���, �������� �ʴ´�.
        if (collision.gameObject.tag != "player") return;

        // �Ѿ˿� ���� ��� �÷��̾� ������Ʈ�� �����Ѵ�.
        GameObject otherPL = collision.gameObject;
        // ������ ������Ʈ�� ���� ��� �÷��̾��� �÷��̾� ��Ʈ�ѷ� ������Ʈ�� �����Ѵ�.
        PlayerController otherCon = otherPL.GetComponent<PlayerController>();
        // �ش� ������Ʈ�� �����, �ǰ� �� �������� �Դ� �Լ��� �����Ͽ� ��� �÷��̾��� ü���� �����Ѵ�.
        otherCon.TakeHit(gameObject.GetComponent<Shot>(), damage);

        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // �� �Ѿ� �� �ӵ��� ���̸� �����ϱ� ���� ���� ���� �ڵ� �ۼ�

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
