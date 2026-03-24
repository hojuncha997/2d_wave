using UnityEngine;

/// <summary>
/// 타워의 속성(이름, 가격, 프리팹, 아이콘 등)을 정의하는 데이터 에셋입니다.
/// </summary>
[CreateAssetMenu(fileName = "NewTowerData", menuName = "WaveShooter/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int price;
    public GameObject towerPrefab;
    public Sprite icon;
    [TextArea] public string description;
}
