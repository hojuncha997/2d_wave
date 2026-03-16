using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 단일 웨이브에 대한 적 소환 정보를 담는 데이터 형식입니다.
/// </summary>
[System.Serializable]
public class EnemyBatch
{
    [Tooltip("소환할 적의 프리팹")]
    public GameObject EnemyPrefab;
    
    [Tooltip("이 배치에서 소환할 적의 총 마리수")]
    public int Count = 5;
    
    [Tooltip("이 배치 내에서의 적 생성 간격 (초)")]
    public float SpawnInterval = 1.0f;
}

/// <summary>
/// 유니티 에디터에서 웨이브 데이터를 에셋으로 생성하고 관리할 수 있게 해주는 ScriptableObject입니다.
/// </summary>
[CreateAssetMenu(fileName = "New WaveData", menuName = "Wave System/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Info")]
    public int WaveNumber;
    
    [Header("Spawn Config")]
    [Tooltip("이 웨이브에서 순차적으로 소환할 적 배치 리스트")]
    public List<EnemyBatch> Batches = new List<EnemyBatch>();
    
    [Header("Intermission")]
    [Tooltip("모든 적 소환 후 다음 웨이브로 넘어가기 전 대기 시간 (초)")]
    public float WaitTimeAfterWave = 5f;

    /// <summary>
    /// 이 웨이브에서 소환될 총 적의 마리수를 반환합니다.
    /// </summary>
    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var batch in Batches)
        {
            total += batch.Count;
        }
        return total;
    }
}
