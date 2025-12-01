using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = GameObject.Find("Main Camera");
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 
        int BallIndex = 1;
        int rows = 5;

        for (int r = 0; r < rows; r++)
        {
            int count = r + 1;
            float offset = (count - 1) * (BallRadius + RowSpacing) * 2 * 0.5f;
            
            for (int i = 0; i < count; i++) {
                Vector3 pos = StartPosition +
                    new Vector3(
                        (i * (BallRadius * 2 + RowSpacing)) - offset,
                        0,
                        r * (BallRadius * 1.8f + RowSpacing)
                    );
                
                GameObject obj = Instantiate(BallPrefab, pos, StartRotation);
                obj.name = BallIndex.ToString();

                obj.GetComponent<MeshRenderer>().material = 
                    Resources.Load<Material>($"Materials/ball_" + BallIndex.ToString());
                
                BallIndex++;
            }

        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        if (PlayerBall == null || CamObj == null)   return;
        
        
        Vector3 offset = new Vector3(0, 9f, -1f);

        Vector3 targetPos = PlayerBall.transform.position + offset;
        Vector3 curPos = CamObj.transform.position;

        CamObj.transform.position = Vector3.Lerp(curPos, targetPos, Time.deltaTime * CamSpeed);
        
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        if (PlayerBall == null)   return;

        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
        if(rb == null)   return;

        Vector3 dir = targetPos - PlayerBall.transform.position;
        dir.y = 0; // y축 방향 힘은 0으로 한다.

        float power = CalcPower(dir);

        rb.AddForce(dir.normalized * power, ForceMode.Impulse);
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        StartCoroutine(FallRoutine(ballName));
        // -------------------- 
    }

    IEnumerator FallRoutine(string ballName)
    {
        MyUIManager.DisplayText($"{ballName} falls", 1f);
        yield return new WaitForSeconds(1f);
    }

    
}
