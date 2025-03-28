using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.Jobs; // NameSpace : 소속 - 비슷한 이름을 구분해주기위해
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum WeaponMode
{ 
    Pistol,
    Rifle,
    Shotgun
}


// 비슷한 변수끼리 나눠서 정리해보자
// 전역으로 할지 지역으로 할지 : 전역은 메모리를 많이 차지한다는 것을 알기.

public class PlayerManager : MonoBehaviour
{
    // 싱글톤
    public static PlayerManager Instance { get; private set; }

    private float moveSpeed = 3.0f; // 플레이어 이동속도
    public float mouseSensitivity = 100.0f; // 마우스 감도
    public Transform cameraTransform; // 카메라 트랜스폼
    public CharacterController characterController;
    public Transform playerHead; // 플레이어 머리 위치(1인칭 모드)
    public float thridPersonDistansce = 3.0f; // 3인칭 모드에서 플레이어와 카메라의 거리
    public Vector3 thridPersonOffset = new Vector3(0f, 1.5f, 0f); // 3인칭 모드에서 카메라 오프셋
    public Transform playerLookObj; // 플레이어 시야 위치

    public float zoomDistance = 1.0f; // 카메라가 확대될 때의 거리(3인칭 모드에서 사용)
    public float zoomSpeed = 5.0f; // 확대축소가 되는 속도
    public float defaultFov = 60.0f; // 기본 카메라 시야각
    public float zooomFov = 30.0f; // 확대 시 카메라 시야각 (1인칭 모드에서 사용)

    private float currentDistance; // 현재 카메라와의 거리(3인칭 모드)
    private float targetDistance; // 목표 카메라 거리
    private float targetFov; // 목표 FOV
    private bool isZoomed = false; // 확대 여부 확인
    private Coroutine zoomCoroutine; // 코루틴을 사용하여 확대 축소 처리
    private Camera mainCamera; // 카메라 컴포넌트

    private float pitch = 0.0f; // 위아래 회전 값
    private float yaw = 0.0f; // 좌우 회전 값
    private bool isFirstPerson = false; // 1인칭 모드 여부
    private bool isrotaterAroundPlayer = true; // 카메라가 플레이어 주위를 회전하는지 여부
        
    public float gravity = -9.81f; // 중력 관련 변수
    public float jumpHeight = 2.0f;
    private Vector3 velocity;
    private bool isGround; // 지면에 닿고있는지

    private Animator animator;
    private float horizontal;
    private float vertical;
    private bool isRunning = false;
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    private bool isAim = false;
    private bool isFire = false;
    private bool isMove = false;
    private bool isItem = false;
<<<<<<< HEAD
    private bool isReload = false;
    private bool isStop = false;
=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
        
    public GameObject rifleFamas;

    public Transform startPoint;

    private int animationSpeed = 1;  // 애니메이션 스피드 조절 animator.speed = animationSpeed;
    public Transform aimTarget;
    private float weaponMaxDistance = 100.0f;
    public LayerMask TargetLayerMask;
    public LayerMask TargetLayerMask2;
    public LayerMask TargetLayerMask3;

    public MultiAimConstraint multiAimConstraint;

    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f);
    public float castDistance = 0.5f;
    public LayerMask itemLayer;
    public Transform itemGetPos;

    public GameObject crossHairObj;
    public GameObject GunIcon;

    private bool isGetGun = false;
    private bool isUseGun = false;

    public Text bulletText;
    private int fireBulletCount = 0;
    private int saveBulletCount = 0;

    private float rifleFireDelay = 0.1f;

    public float playerHp = 5.0f;

    public GameObject flashLightObj;
    private bool isFlashLightON = false;
    public AudioClip audioFlasgLihgt;

    public GameObject pauseObj;
    private bool isPause = false;

    // 무기별 변수
    private WeaponMode currentWeaponMode = WeaponMode.Rifle;
    // 샷건에 대한 변수
    private int shotgunRayCount = 5;
    private float shotgunSpreafAngle = 10.0f;
<<<<<<< HEAD
    public float recoilStrength = 0.2f;
    public float maxRecoilAngle = 15.0f;
    public float currentRecoil = 0.0f;
    // 카메라 제어를 통한 반동
    public float shakeDuration = 0.01f;
    public float shakeMagnitude = 0.01f;
=======
    private float recoilStrength = 2.0f;
    private float maxRecoilAngle = 10.0f;
    private float currentRecoil = 0.0f;
    // 카메라 제어를 통한 반동
    private float shakeDuration = 0.1f; 
    private float shakeMagnitude = 0.1f;
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    private Vector3 originalCameraPosition;
    private Coroutine cameraShakeCoroutine;

    public Transform doorCheck;
    public float doorCheckDistance = 3.0f;
<<<<<<< HEAD

    public GameObject gameOver;

    public LayerMask cameraCollision;
    public Transform front;

    public Transform effectPos;

    public GameObject playerPrefab;
=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            { 
                Destroy(gameObject);
            }
        }

<<<<<<< HEAD
=======
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            { 
                Destroy(gameObject);
            }
        }

>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    }

    void Start()
    {   
        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = startPoint.position;
        currentDistance = thridPersonDistansce;
        targetDistance = thridPersonDistansce;
        targetFov = defaultFov;
        mainCamera = cameraTransform.GetComponent<Camera>();
        mainCamera.fieldOfView = defaultFov;
        animator = GetComponent<Animator>();
        rifleFamas.SetActive(false);
        characterController = GetComponent<CharacterController>();
        crossHairObj.SetActive(false);
        GunIcon.SetActive(false);
        bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
        bulletText.gameObject.SetActive(false);
        flashLightObj.SetActive(false);
        pauseObj.SetActive(false);
<<<<<<< HEAD
        gameOver.SetActive(false);

=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    }

    void Update()
    {
        PlayerMove();
        PlayerRun();
        CameraSet();
        Aim();
        WeaponChange();
        Reload();
        Fire();
        ItemGet();
        Die();
        ActionFlashLight();
        PauseController();
        CameraRecoil();
<<<<<<< HEAD
        DoorOpen();
=======
        Door();
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053

        // animation.layer에 대한 정보 넣기
        // AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);



    }

    public void PlayerMove()
    {        
        // 마우스 입력을 받아 카메라와 플레이어 회전 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        yaw += mouseX;
        pitch -= mouseY;
        // 마우스 각도가 휙 돌아가지않도록 제한
        pitch = Mathf.Clamp(pitch, -45, 45);
    }

    void CameraSet()
    {
        isGround = characterController.isGrounded;
        isrotaterAroundPlayer = false; // 게임 시작시 3인칭으로 시작함
        
        // 플레이어가 문제없이 땅에 붙게하도록 설정
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            Debug.Log(isFirstPerson ? "1인칭 모드" : "3인칭 모드");
        }

        if (Input.GetKey(KeyCode.C)) // C키를 꾹누르고 있을때만 주변 시야 확인 가능
        {   // 3인칭일때만
            isrotaterAroundPlayer = !isrotaterAroundPlayer;
            Debug.Log(isrotaterAroundPlayer ? "카메라가 주위를 회전합니다." : "플레이어의 시야 확인가능.");

        }

        if (isFirstPerson)
        {
            FirstPersonMovement();
        }
        else
        {
            ThirdPersonMovement();
        }

    }

    void CameraRecoil()         // 총쏘고 반동 표현, 크로스헤어 커졌다가 줄어들었다가를 표현
<<<<<<< HEAD
=======
    {

        if (currentRecoil > 0)
        {
            currentRecoil -= recoilStrength * Time.deltaTime;
            currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle);
            Quaternion currentRotation = Camera.main.transform.rotation;
            Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0);
            Camera.main.transform.rotation = currentRotation * recoilRotation; // 카메라를 제어하는 코드를 꺼야한다. -> 이걸 끄지않으면 카메라는 올라가지 않는다.
        }
    }

    void ApplyRecoil()
    {
        Quaternion currentRotation = Camera.main.transform.rotation; // 현재 카메라 월드 회전값 가져오기
        Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0); // 반동을 계산해서 x축 상하 회전에 추가
        Camera.main.transform.rotation = currentRotation * recoilRotation; // 현재 회전 값에 반동을 곱하여 새로운 회전값에 넣기
        currentRecoil += recoilStrength; // 반동 값을 증가
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); // 반동값을 제한
    }

    void StartCameraShake()
    {
        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
        }
        cameraShakeCoroutine = StartCoroutine(CameraShake(shakeDuration, shakeMagnitude));
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        Vector3 orginalPosition = Camera.main.transform.position;

        while(elapsed < duration)
        {
            float offsetX = Random.Range(-1.0f, 1.0f) * magnitude;
            float offsetY = Random.Range(-1.0f, 1.0f) * magnitude;

            Camera.main.transform.position = orginalPosition + new Vector3(offsetX, offsetY, 0);
            
            elapsed += Time.deltaTime;

            yield return null;  
        }
        Camera.main.transform.position = orginalPosition;
    }

    public void Aim()
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    {

        if (currentRecoil > 0)
        {
            currentRecoil -= recoilStrength * Time.deltaTime;
            currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle);
            Quaternion currentRotation = Camera.main.transform.rotation;
            Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0);
            Camera.main.transform.rotation = currentRotation * recoilRotation; // 카메라를 제어하는 코드를 꺼야한다. -> 이걸 끄지않으면 카메라는 올라가지 않는다.
        }
    }

    void ApplyRecoil()
    {
        Quaternion currentRotation = Camera.main.transform.rotation; // 현재 카메라 월드 회전값 가져오기
        Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0); // 반동을 계산해서 x축 상하 회전에 추가
        Camera.main.transform.rotation = currentRotation * recoilRotation; // 현재 회전 값에 반동을 곱하여 새로운 회전값에 넣기
        currentRecoil += recoilStrength; // 반동 값을 증가
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); // 반동값을 제한
    }

    void StartCameraShake()
    {
        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
        }
        cameraShakeCoroutine = StartCoroutine(CameraShake(shakeDuration, shakeMagnitude));
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        Vector3 orginalPosition = Camera.main.transform.position;

        while(elapsed < duration)
        {
            float offsetX = Random.Range(-1.0f, 1.0f) * magnitude;
            float offsetY = Random.Range(-1.0f, 1.0f) * magnitude;

            Camera.main.transform.position = orginalPosition + new Vector3(offsetX, offsetY, 0);
            
            elapsed += Time.deltaTime;

            yield return null;  
        }
        Camera.main.transform.position = orginalPosition;
    }

    public void Aim()
    {
        // 서서히 가기위해 코루틴 사용 / 마우스를 눌렀을때
        if (Input.GetMouseButton(1) && isGetGun && isUseGun)
        {
            isAim = true;            
            multiAimConstraint.data.offset = new Vector3(-30, 0, 0);
            crossHairObj.SetActive(true);          
            //animator.SetBool("IsAim", isAim);
            animator.SetLayerWeight(1, 1); // 레이어를 1로 설정


            //확대 축소 거리가 값이 있을때 - 코루틴을 하고있는지 아닌지, 중복방지
            if (zoomCoroutine != null)
            {
                // 코루틴 멈춰라
                StopCoroutine(zoomCoroutine);
            }
            // 1인칭 모드일때
            if (isFirstPerson)
            {
                // 타켓의 위치를 zooomFov로 하고 1인칭모드의 코루틴으로 설정
                SetTargetFov(zooomFov);
                // 스코프 오브젝트 넣기

                playerPrefab.SetActive(false);
                rifleFamas.SetActive(false);
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else
            {   // 그게 아니라면 타켓의 위치를 zoomDistance하고 3인칭 모드의 코루틴으로 설정
                SetTargetDistance(zoomDistance);
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));
            }


        }

        // 마우스를 뗐을때
        if (Input.GetMouseButtonUp(1) && isGetGun && isUseGun)
        {
            isAim = false;
            crossHairObj.SetActive(false);
            multiAimConstraint.data.offset = new Vector3(0, 0, 0);
            //animator.SetBool("IsAim", isAim);
            animator.SetLayerWeight(1, 0);
            currentRecoil = 0;

            //확대 축소 거리가 값이 있을때 - 코루틴을 하고있는지 아닌지, 중복방지
            if (zoomCoroutine != null)
            {
                // 코루틴 멈춰라
                StopCoroutine(zoomCoroutine);
            }
            // 1인칭 모드일때
            if (isFirstPerson)
            {
                // 타켓의 위치를 defaultFov 하고 1인칭모드의 코루틴으로 설정
                SetTargetFov(defaultFov);
                //스코프 삭제
                
                playerPrefab.SetActive(true);
                rifleFamas.SetActive(true);
                zoomCoroutine = StartCoroutine(ZoomFieldOfView(targetFov));
            }
            else
            {   // 그게 아니라면 타켓의 위치를 thridPersonDistansce하고 3인칭 모드의 코루틴으로 설정
                SetTargetDistance(thridPersonDistansce);
                zoomCoroutine = StartCoroutine(ZoomCamera(targetDistance));

            }

        }
    }

    public void PlayerRun()
    {

        // bool을 통해 쉬프트키를 누르면 변화되는 것을 구현
        if (Input.GetKey(KeyCode.LeftShift) && (horizontal !=0 || vertical !=0)) // 움직임이 0이 아니라면으로 설정
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }



        // 상황별 애니메이션 연결
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("IsRunning", isRunning);


        moveSpeed = isRunning ? runSpeed : walkSpeed;
    }

    public void Fire()
    {
<<<<<<< HEAD
        if (Input.GetMouseButton(0) && !isReload)
        {
            if (isAim && !isFire)
            { /*
=======
        if (Input.GetMouseButtonDown(0))
        {
            if (isAim && !isFire)
            {
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
                if (currentWeaponMode == WeaponMode.Shotgun)
                {
                    // 총마다 반동 크기 다르게
                    
                    if (fireBulletCount > 0)
                    {
                        fireBulletCount--;
                        bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                        bulletText.gameObject.SetActive(true);
                    }

                    FireShotgun();

                }   
<<<<<<< HEAD
                */
=======

>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
                isFire = true;
                //Weapon Type에 따라 MaxDistance Set / damage 변경
                weaponMaxDistance = 1000.0f;
                float damage = 1.0f;



                if (fireBulletCount > 0)
                {                    
                    fireBulletCount--;
                    bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                    bulletText.gameObject.SetActive(true);


                    //Gun Type FireDelay Data Fix
                    StartCoroutine(FireWithDelay(rifleFireDelay));
                    animator.SetTrigger("Fire");
<<<<<<< HEAD
                    SoundManager.instance.PlaySFX("FamasShot", gameObject.transform.position);
=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053

                    ApplyRecoil();
                    StartCameraShake();
                    // orgin = 현재 위치
                    Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

                    // 레이캐스트가 여러 충돌체 감지 - 라이플 종류
                    RaycastHit[] hits = Physics.RaycastAll(ray, weaponMaxDistance, TargetLayerMask);

                    if (hits.Length > 0)
                    {
                        // 두마리 관통
                        for (int i = 0; i < hits.Length && i < 2; i++)
                        {
                            Debug.Log("Hit : " + hits[i].collider.gameObject.name);
                            Debug.DrawLine(ray.origin, hits[i].point, Color.red, 1.0f);

                            if (hits[i].collider.gameObject.CompareTag("Zombie"))
                            {
                                ParticleManager.Instance.ParticlePlay(ParticleType.Explosion, hits[i].point, Vector3.one);
                                // 파티클 데미지 소리 넣어도됨
                                hits[i].collider.gameObject.GetComponent<ZombieManager>().TakeDamage(damage);
                            }
                            else
                            {
                                Debug.DrawLine(ray.origin, ray.origin + ray.direction * weaponMaxDistance, Color.green, 1.0f);
                            }

                        }


                    }

                    RaycastHit hitWall;

                    if (Physics.Raycast(ray, out hitWall, weaponMaxDistance, TargetLayerMask2))
                    {
                        Debug.Log("Hit : " + hitWall.collider.gameObject.name);
                        Debug.DrawLine(ray.origin, hitWall.point, Color.red, 1.0f);

                        if (hitWall.collider.gameObject.layer == 8)
                        {
                            ParticleManager.Instance.ParticlePlay(ParticleType.Explosion, hitWall.point, Vector3.one);
                            SoundManager.instance.PlaySFX("WallHitSound", hitWall.point);
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + ray.direction * weaponMaxDistance, Color.green, 1.0f);
                        }

                    }

                    RaycastHit hitGround;

                    if (Physics.Raycast(ray, out hitGround, weaponMaxDistance, TargetLayerMask3))
                    {
                        Debug.Log("Hit : " + hitGround.collider.gameObject.name);
                        Debug.DrawLine(ray.origin, hitGround.point, Color.red, 1.0f);

                        if (hitGround.collider.gameObject.layer == 9)
                        {
                            ParticleManager.Instance.ParticlePlay(ParticleType.Explosion, hitGround.point, Vector3.one);
                            SoundManager.instance.PlaySFX("WallHitSound",hitGround.point);
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + ray.direction * weaponMaxDistance, Color.green, 1.0f);
                        }

                    }

                }
                else
                {
                    isFire = false;
                    SoundManager.instance.PlaySFX("EmptyBullet", gameObject.transform.position);
                    return;
                }

            }
        }
    }

    public void Reload()
    {
<<<<<<< HEAD

        if (Input.GetKeyDown(KeyCode.R) && isGetGun && saveBulletCount > 0 && !isFire)
=======
        if (Input.GetKeyDown(KeyCode.R) && isGetGun && saveBulletCount > 0)       
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
        {
            isReload = true;

            if (fireBulletCount + saveBulletCount < 30)
            {
                animator.SetTrigger("Reload");
                fireBulletCount = saveBulletCount + fireBulletCount;
                saveBulletCount = 0;
                bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                bulletText.gameObject.SetActive(true);
                StartCoroutine(FinishReload());

            }
            else if (fireBulletCount + saveBulletCount >= 30)
            {
                animator.SetTrigger("Reload");
                saveBulletCount = saveBulletCount - (30 - fireBulletCount);
                fireBulletCount = 30;

                bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                bulletText.gameObject.SetActive(true);
                StartCoroutine(FinishReload());

            }            
        }   
    }

    private IEnumerator FinishReload()
    {
        yield return new WaitForSeconds(2.5f);
        isReload = false;
    }

    public void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGetGun)
        {
            animator.SetTrigger("IsWeaponChange");
            rifleFamas.SetActive(true);
            isUseGun = true;

            walkSpeed = 2.5f;
            runSpeed = 4.0f;


        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && isGetGun)
        {
            rifleFamas.SetActive(false);
            isUseGun = false;

            walkSpeed = 3f;
            runSpeed = 7.0f;
        }
    }

    // 샷건 코드
    void FireShotgun()
    {
        for (int i = 0; i < shotgunRayCount; i++)
        {
            RaycastHit hit;

            Vector3 orgin = Camera.main.transform.position;
            Vector3 spreadDirection = GetSpreadDirection(Camera.main.transform.forward, shotgunSpreafAngle);
            Debug.DrawRay(orgin, spreadDirection, Color.yellow ,2.0f);
            if (Physics.Raycast(orgin, spreadDirection, out hit, castDistance, TargetLayerMask))
            {
                Debug.Log("Shotgun Hit : " + hit.collider.name);
            }

        }
    }

<<<<<<< HEAD
=======
    // 샷건 코드
    void FireShotgun()
    {
        for (int i = 0; i < shotgunRayCount; i++)
        {
            RaycastHit hit;

            Vector3 orgin = Camera.main.transform.position;
            Vector3 spreadDirection = GetSpreadDirection(Camera.main.transform.forward, shotgunSpreafAngle);
            Debug.DrawRay(orgin, spreadDirection, Color.yellow ,2.0f);
            if (Physics.Raycast(orgin, spreadDirection, out hit, castDistance, TargetLayerMask))
            {
                Debug.Log("Shotgun Hit : " + hit.collider.name);
            }

        }
    }

>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    Vector3 GetSpreadDirection(Vector3 forwardDirection, float spreadAngle)
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);

        Vector3 spreadDirection = Quaternion.Euler(spreadX, spreadY,0) * forwardDirection;
        return spreadDirection;
    }

<<<<<<< HEAD
    void ItemGet()
=======

    public void GetItem()
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("GetItem");
            Invoke("BoxItemGet", 1);
        }
    }
        
    void BoxItemGet()
    {
        Vector3 orgin = itemGetPos.position; // 캐릭터의 피봇이 발이기때문에 따로 설정
        Vector3 direction = itemGetPos.forward;
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(orgin, boxSize / 2, direction, Quaternion.identity, castDistance, itemLayer);
        DebugBox(orgin,direction);

        foreach (RaycastHit hit in hits)
        {            
            
            if (hit.collider.name == "WeaponFamas")
            {
                Debug.Log("Item : " + hit.collider.name);
                hit.collider.gameObject.SetActive(false);
                GunIcon.SetActive(true);
                fireBulletCount = 30;
                bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                bulletText.gameObject.SetActive(true);
                isGetGun = true;

            }

            if (hit.collider.name == "ItemBullet")
            {
                Debug.Log("Item : " + hit.collider.name);
                hit.collider.gameObject.SetActive(false);

                saveBulletCount += 30;

                if (saveBulletCount >= 120)
                {
                    saveBulletCount = 120;

                }
                bulletText.text = $"{fireBulletCount}/{saveBulletCount}";
                bulletText.gameObject.SetActive(true);


            }


        }
    }

    void ActionFlashLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
<<<<<<< HEAD
        {     
            SoundManager.instance.PlaySFX("FlashLight", flashLightObj.transform.position);
            isFlashLightON = !isFlashLightON;
            flashLightObj.SetActive(isFlashLightON);
    
=======
        {
            SoundManager.instance.PlaySFX("FlashLight",flashLightObj.transform.position);
            isFlashLightON = !isFlashLightON;
            flashLightObj.SetActive(isFlashLightON);
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
        }

    }


    void FirstPersonMovement()
    {           
        // 지역변수인 것을 지우고 전체적인 변수로 애니메이션 적용을 위해 float 삭제
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
       

        // 카메라 위치값을 통해 플레이어 움직임 설정
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;

        moveDirection.y = 0;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);


        // 카메라위치를 플레이어의 머리로 설정
        cameraTransform.position = playerHead.position;
        // 카메라 각도 설정
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0);
        // 플레이어의 각도를 카메라의 회전 각으로 설정
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0);

        UpdateAimTarget();

    }

    void ThirdPersonMovement()
    {
        // 지역변수인 것을 지우고 전체적인 변수로 애니메이션 적용을 위해 float 삭제
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * moveSpeed * Time.deltaTime);


        UpdateCameraPosition();

    }

    void UpdateCameraPosition()
    {
        if (isrotaterAroundPlayer)
        {
            // 카메라가 플레이어 오른쪽에서 회전하도록 설정
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

            // 카메라를 플레이어의 오른쪽에서 고정된 위치로 이동
            cameraTransform.position = transform.position + thridPersonOffset + rotation * direction;

            // 카메라가 플레이어의 위치를 따라가도록 설정
            cameraTransform.LookAt(transform.position + new Vector3(0, thridPersonOffset.y, 0));


            // 카메라가 벽에 충돌할시 카메라를 앞으로 이동시키는 코드
            RaycastHit hit;
            Vector3 rayDir = cameraTransform.position - front.position;
            if (Physics.Raycast(front.position, rayDir, out hit, currentDistance, cameraCollision))
            {
                cameraTransform.position = hit.point;
            }

        }
        else
        {
            if (!isStop)
            {
                // 플레이어가 직접 회전하는 모드
                transform.rotation = Quaternion.Euler(0f, yaw, 0);
                Vector3 direction = new Vector3(0, 0, -currentDistance);
                cameraTransform.position = playerLookObj.position + thridPersonOffset + Quaternion.Euler(pitch, yaw, 0) * direction;
                cameraTransform.LookAt(playerLookObj.position + new Vector3(0, thridPersonOffset.y, 0));

                // 카메라가 벽에 충돌할시 카메라를 앞으로 이동시키는 코드
                RaycastHit hit;
                Vector3 rayDir = cameraTransform.position - front.position;
                if (Physics.Raycast(front.position, rayDir, out hit, currentDistance, cameraCollision))
                {
                    cameraTransform.position = hit.point;
                }

                UpdateAimTarget();
            }
        
        }


    }

    public void SetTargetDistance(float distance)
    { 
        targetDistance = distance;
    }

    public void SetTargetFov(float fov)
    {
        targetFov = fov;
    }

    IEnumerator ZoomCamera(float targetDistance) // 3인칭
    {
        while (Mathf.Abs(currentDistance - targetDistance) > 1.0f) // 현재 거리에서 목표 거리로 부드럽게 이동, Mathf.Abs - 절댓값
        { 
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
            yield return null; 
        
        }

        currentDistance = targetDistance; // 목표 거리에 도달한 후 값을 고정
    
    }

    IEnumerator ZoomFieldOfView(float tartgetFov) // 1인칭
    {
        while (Mathf.Abs(mainCamera.fieldOfView - targetFov) > 0.01f)
        { 
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, tartgetFov, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        mainCamera.fieldOfView = targetFov;

    }

    IEnumerator FireWithDelay(float fireDelay)
    {        
        yield return new WaitForSeconds(fireDelay);
        isFire = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            animator.SetTrigger("IsWin");
            other.gameObject.transform.SetParent(transform); 
        }
        // 아이템과 충돌했을때 플레이어 안에 컴포넌트로 넣고 빼는 법 -> 아이템을 먹고 버리는거 가능
        // other.gameObject.transform.SetParent(transform); 
        // other.gameObject.transform.SetParent(null); 
<<<<<<< HEAD


    }

    void DoorOpen()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 박스캐스트로 만들기
        {
=======
           

    }

    void Door()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {         
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
            RaycastHit hit;
            Debug.DrawLine(doorCheck.position, doorCheck.forward, Color.green, 1.0f);
            if (Physics.Raycast(doorCheck.position, doorCheck.forward, out hit, doorCheckDistance))
            {

                if (hit.collider.gameObject.CompareTag("Door"))
<<<<<<< HEAD
                {                    
                    animator.SetTrigger("DoorOpen");
=======
                {
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
                    Debug.DrawLine(doorCheck.position, doorCheck.forward, Color.red, 2.0f);
                    Debug.Log("Hit : " + hit.collider.gameObject.name);

                    hit.collider.gameObject.transform.GetComponent<DoorManager>().ChangeDoorOpen();

                }

            }


        }
    }




    public void Damaged(float damage)
    {
        if (playerHp > 0)
        {             
            animator.SetTrigger("Damage");
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CharacterController>().enabled = true;
            playerHp -= damage;
            gameObject.layer = 10;
            Invoke("DamagedOff", 1.5f);
        }
    }

    void DamagedOff()
    {
        gameObject.layer = 6;
    }

    public void Die()
    {
        if (playerHp <= 0)
        {            
            isStop = true;
            animator.SetTrigger("Die");
            // 죽었을때 못움직이도록 설정해봄 - 하지만 마우스가 돌아가면 허리가 움직이는현상있음
            walkSpeed = 0;
            runSpeed = 0;
            gameOverMenu();

        }
    }

<<<<<<< HEAD
=======

>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    void PauseController()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
<<<<<<< HEAD
            isPause = !isPause;            
=======
            isPause = !isPause;
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053

            if (isPause)
            {
                Pause();
            }
            else
            {
                ReGame();
            }
        }
    }
    void ReGame()
    {
<<<<<<< HEAD
        // 소리 넣기        
        pauseObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1; // 게임시간 재개
        isStop = false;
=======
        // 소리 넣기
        pauseObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1; // 게임시간 재개
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053

    }

    public void Regame() // 버튼에 쓰일 함수
    {
        // 소리 넣기
        pauseObj.SetActive(false);
        SoundManager.instance.PlaySFX("ButtonSound");
        Cursor.lockState = CursorLockMode.Locked;
<<<<<<< HEAD
        isStop = false;
=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
        Time.timeScale = 1; // 게임시간 재개
        
    }

    void Pause()
<<<<<<< HEAD
    {
        isStop = true;
=======
    { 
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
        pauseObj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0; // 게임시간 정지
    }

<<<<<<< HEAD
    void gameOverMenu()
    {
        Invoke("GameoverPause", 4.0f);
    }

    void GameoverPause()
    {
        isPause = true;
        gameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Map1");
        Time.timeScale = 1;
    }

=======
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053

    public void Exit()
    {
        pauseObj.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

        
    }

    void UpdateAimTarget()
    {
        // 카메라를 중심으로 마우스 포인트에 광선 발사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        aimTarget.position = ray.GetPoint(10.0f);
<<<<<<< HEAD
        
=======
 
>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    }

    public void WeaponChangeSoundOn()
    {
        SoundManager.instance.PlaySFX("ChangeGun",gameObject.transform.position);
    }

    public void FireSound()
    {
        SoundManager.instance.PlaySFX("FamasShot",rifleFamas.transform.position);
<<<<<<< HEAD
=======
        famasEffect.Play();

>>>>>>> 12e6b7ba1cd363f98fb9b7c354681284cd35a053
    }

    public void DamagedSound()
    {
        SoundManager.instance.PlaySFX("PlayerDamagedSound", gameObject.transform.position);
    }

    public void GetItemSound()
    {
        SoundManager.instance.PlaySFX("GetItem", gameObject.transform.position);
    }

    public void ReloadSound()
    {
        SoundManager.instance.PlaySFX("ReLoadSound", gameObject.transform.position);
    }

    public void FootSound()
    {
        SoundManager.instance.PlaySFX("StepSound",gameObject.transform.position);
    }

    //BoxCast 그리기
    void DebugBox(Vector3 origin, Vector3 direction)
    {
        Vector3 endPoint = origin + direction * castDistance;

        Vector3[] corners = new Vector3[8];
        corners[0] = origin + new Vector3(-boxSize.x, -boxSize.y, -boxSize.z) / 2;
        corners[1] = origin + new Vector3(boxSize.x, -boxSize.y, -boxSize.z) / 2;
        corners[2] = origin + new Vector3(-boxSize.x, boxSize.y, -boxSize.z) / 2;
        corners[3] = origin + new Vector3(boxSize.x, boxSize.y, -boxSize.z) / 2;
        corners[4] = origin + new Vector3(-boxSize.x, -boxSize.y, boxSize.z) / 2;
        corners[5] = origin + new Vector3(boxSize.x, -boxSize.y, boxSize.z) / 2;
        corners[6] = origin + new Vector3(-boxSize.x, boxSize.y, boxSize.z) / 2;
        corners[7] = origin + new Vector3(boxSize.x, boxSize.y, boxSize.z) / 2;

        Debug.DrawLine(corners[0], corners[1], Color.green, 3.0f);
        Debug.DrawLine(corners[1], corners[3], Color.green, 3.0f);
        Debug.DrawLine(corners[3], corners[2], Color.green, 3.0f);
        Debug.DrawLine(corners[2], corners[0], Color.green, 3.0f);
        Debug.DrawLine(corners[4], corners[5], Color.green, 3.0f);
        Debug.DrawLine(corners[5], corners[7], Color.green, 3.0f);
        Debug.DrawLine(corners[7], corners[6], Color.green, 3.0f);
        Debug.DrawLine(corners[6], corners[4], Color.green, 3.0f);
        Debug.DrawLine(corners[0], corners[4], Color.green, 3.0f);
        Debug.DrawLine(corners[1], corners[5], Color.green, 3.0f);
        Debug.DrawLine(corners[2], corners[6], Color.green, 3.0f);
        Debug.DrawLine(corners[3], corners[7], Color.green, 3.0f);
        Debug.DrawRay(origin, direction * castDistance, Color.green);


    }

}
