using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // 貪食蛇的身體部件預製件
    public GameObject segmentPrefab;
    // 食物的預製件
    public GameObject foodPrefab; 
    // 移動速度
    public float moveSpeed = 0.1f;
    // 用來存儲蛇身的每一個部分
    private List<Transform> segments = new List<Transform>();    
    // 初始移動方向設置為右
    private Vector2 direction = Vector2.right;

    private void Start()
    {
        // 將蛇頭添加到 segments 列表中
        segments.Add(this.transform);
        // 每隔一定時間移動一次
        InvokeRepeating("Move", 0.1f, moveSpeed);
        Spawnfood(); // 初始生成食物
    }

    private void Update()
    {
        // 根據鍵盤輸入改變方向
        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2.up;      // 上
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2.down; // 下
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2.left; // 左
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2.right; // 右
    }

    private void Move()
    {
        // 獲取當前蛇頭的位置
        Vector3 headPosition = segments[0].position;
        // 計算新的蛇頭位置
        Vector3 newHeadPosition = headPosition + (Vector3)direction;

        transform.position = newHeadPosition;

        // 創建新的蛇身部件
        // Transform newSegment = Instantiate(segmentPrefab).transform;
        // newSegment.position = newHeadPosition;  // 設置新部件的位置
        // segments.Insert(0, newSegment);          // 將新部件添加到列表的開頭

        // 檢查是否吃到了食物
        if (Physics2D.OverlapCircle(newHeadPosition, 0.1f, LayerMask.GetMask("food")))
        {
            // 吃到食物，隨機生成新的食物
            Spawnfood();
        }

        //if (segments.Count > 1)
        //{
        //    Destroy(segments[segments.Count - 1].gameObject);
        //    segments.RemoveAt(segments.Count - 1);
        //}
    }
    private void Spawnfood()
    {
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        Vector2 foodPosition = new Vector2(x, y);
        // 確保新食物不生成在蛇身上
        foreach (Transform segment in segments)
        {
            if (Vector2.Distance(segment.position, foodPosition) < 0.5f)
            {
                Spawnfood(); // 如果重疊，重新生成
                return;
            }
            Instantiate(foodPrefab, foodPosition, Quaternion.identity); // 生成食物
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("boundary"))
        {
            // 遊戲結束或重啟
            Debug.Log("碰到邊界，遊戲結束！");           
            // Time.timeScale = 0; // 停止遊戲            
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); 
            // 重新加載場景

        }
    }
}
