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
        // 포톤 네트워크에서 플레이어가 생성될 당시 저장된 랜덤한 색의 정보를 가져와 적용시킨다.
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

        // 초기 움직임 속도는 기본의 절반
        curMoveSpeed = model.moveSpeed / 2;
        // 플레이어 움직임 함수 실행
        Move();

        // 달리기 시프트 : 좌측 시프트
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (photonView.Owner.IsLocal == false) return;

            // 플레이어의 속도가 정해진 수치가 됨
            curMoveSpeed = model.moveSpeed;
        }

        // 발포 기능 : 마우스 우클릭
        if (Input.GetMouseButtonDown(0))
        {
            if (photonView.Owner.IsLocal == false) return;

            // 탄환이 남아 있을 때, 발포가 가능하다.
            if (model.curBullet > 0)
            {
                Fire();
                // 한 번 발포마다 총알이 하나식 차감된다.
                model.curBullet--;
            }
            // 탄환이 하나도 남아있지 않을 경우, 발포가 불가능하다.
            else return;
        }

        // 재장전 기능 : 키보드 R 키
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (photonView.Owner.IsLocal == false) return;

            Debug.Log("재장전 진행");
            model.curBullet = model.maxBullet;
        }
    }

    // === === === 

    void Move()
    {
        PlayerDir.x = Input.GetAxisRaw("Horizontal");
        PlayerDir.z = Input.GetAxisRaw("Vertical");

        if (PlayerDir == Vector3.zero) return;

        // 카메라 앞 방향을 기준으로 하여 움직인다.
        transform.forward = followCam.transform.forward;
        PlayerDir = new Vector3(PlayerDir.x, 0, PlayerDir.z).normalized;
        transform.Translate(PlayerDir * curMoveSpeed * Time.deltaTime);
    }

    void Fire()
    {
        // 발사한 총알의 Shot 컴포넌트를 참조하고,
        GameObject MyBullet = Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation).GetComponent<GameObject>();
        Shot Bullet = MyBullet.GetComponent<Shot>();

        // 참조한 컴포넌트에 public으로 구현된 각 값에 플레이어 모델의 값 정보를 입력해준다.
        Bullet.Speed = model.bulletSpeed;
        Bullet.Damage = model.MyDamage;

        // 총알의 색상은 플레이어의 색상과 동일하다.
        Renderer BulletRenderer = MyBullet.GetComponent<Renderer>();
        BulletRenderer.material.color = renderer.material.color;
    }

    public void TakeHit(Shot otherBullet, int otherDamage)
    {
        // 총알에 피격당했을 경우, 나는 상대 플레이어의 데미지값 만큼 체력에 피해를 입는다.
        if (photonView.Owner.IsLocal == false) return;


        // 상대 플레이어가 쏜 총알의 Shot 컴포넌트를 참조한다.
        otherBullet = GetComponent<Shot>();
        // 해당 컴포넌트에 저장되어 있는 데미지 값을 가져와, 나의 체력을 차감한다.
        otherDamage = otherBullet.Damage;
        model.hp -= otherDamage;

        // 나의 체력이 0보다 작을 경우, 나는 사망하므로 오브젝트를 삭제한다.
        // 추후 오브젝트 삭제 외 다른 게임오버 방법을 구현할 것.
        if (model.hp < 0)
        {
            if (photonView.Owner.IsLocal == false) return;
            Destroy(gameObject);
        }
    }
}
