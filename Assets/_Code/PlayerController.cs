using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] PlayerModel model;
    [SerializeField] Renderer renderer;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] GameObject bullet;

    private CinemachineVirtualCamera followCam;
    private Vector3 PlayerDir;
    private float curMoveSpeed;

    private Color color;

    // === === === 

    private void Awake()
    {
        if (photonView.Owner.IsLocal == false)
        {
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.Follow = transform;
        }
        model.hp = model.maxHp;
        curMoveSpeed = model.moveSpeed;
        model.curBullet = model.maxBullet;
    }

    private void Start()
    {
        // ���� ��Ʈ��ũ���� �÷��̾ ������ ��� ����� ������ ���� ������ ������ �����Ų��.
        object[] data = photonView.InstantiationData;

        color.r = (float)data[0];
        color.g = (float)data[1];
        color.b = (float)data[2];

        renderer.material.color = color;
    }

    // === === === 

    private void Update()
    {
        if (photonView.Owner.IsLocal == false) return;

        // �ʱ� ������ �ӵ��� �⺻�� ����
        curMoveSpeed = model.moveSpeed / 2;
        // �÷��̾� ������ �Լ� ����
        Move();

        // �޸��� ����Ʈ : ���� ����Ʈ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (photonView.Owner.IsLocal == false) return;

            // �÷��̾��� �ӵ��� ������ ��ġ�� ��
            curMoveSpeed = model.moveSpeed;
        }

        // ���� ��� : ���콺 ��Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            if (photonView.Owner.IsLocal == false) return;

            // źȯ�� ���� ���� ��, ������ �����ϴ�.
            if (model.curBullet > 0)
            {
                Fire();
                // �� �� �������� �Ѿ��� �ϳ��� �����ȴ�.
                model.curBullet--;
            }
            // źȯ�� �ϳ��� �������� ���� ���, ������ �Ұ����ϴ�.
            else return;
        }

        // ������ ��� : Ű���� R Ű
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (photonView.Owner.IsLocal == false) return;

            Debug.Log("������ ����");
            model.curBullet = model.maxBullet;
        }
    }

    // === === === 

    void Move()
    {
        PlayerDir.x = Input.GetAxisRaw("Horizontal");
        PlayerDir.z = Input.GetAxisRaw("Vertical");

        if (PlayerDir == Vector3.zero) return;

        // ī�޶� �� ������ �������� �Ͽ� �����δ�.
        transform.forward = followCam.transform.forward;
        PlayerDir = new Vector3(PlayerDir.x, 0, PlayerDir.z).normalized;
        transform.Translate(PlayerDir * curMoveSpeed * Time.deltaTime);
    }

    void Fire()
    {
        // �߻��� �Ѿ��� Shot ������Ʈ�� �����ϰ�,
        GameObject MyBullet = Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation).GetComponent<GameObject>();
        Shot Bullet = MyBullet.GetComponent<Shot>();

        // ������ ������Ʈ�� public���� ������ �� ���� �÷��̾� ���� �� ������ �Է����ش�.
        Bullet.Speed = model.bulletSpeed;
        Bullet.Damage = model.MyDamage;

        // �Ѿ��� ������ �÷��̾��� ����� �����ϴ�.
        Renderer BulletRenderer = MyBullet.GetComponent<Renderer>();
        BulletRenderer.material.color = renderer.material.color;
    }

    public void TakeHit(Shot otherBullet, int otherDamage)
    {
        // �Ѿ˿� �ǰݴ����� ���, ���� ��� �÷��̾��� �������� ��ŭ ü�¿� ���ظ� �Դ´�.
        if (photonView.Owner.IsLocal == false) return;


        // ��� �÷��̾ �� �Ѿ��� Shot ������Ʈ�� �����Ѵ�.
        otherBullet = GetComponent<Shot>();
        // �ش� ������Ʈ�� ����Ǿ� �ִ� ������ ���� ������, ���� ü���� �����Ѵ�.
        otherDamage = otherBullet.Damage;
        model.hp -= otherDamage;

        // ���� ü���� 0���� ���� ���, ���� ����ϹǷ� ������Ʈ�� �����Ѵ�.
        // ���� ������Ʈ ���� �� �ٸ� ���ӿ��� ����� ������ ��.
        if (model.hp < 0)
        {
            if (photonView.Owner.IsLocal == false) return;
            Destroy(gameObject);
        }
    }
}
