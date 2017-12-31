using UnityEngine;

public class SpawnPrefabOnKeyDown : MonoBehaviour
{
    public GameObject m_Prefab;
    public KeyCode m_KeyCode;

    void Update()
	{
		if (Input.GetKeyDown (m_KeyCode) && m_Prefab != null) {
			Vector3 pos = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 3f);
			Instantiate (m_Prefab, pos, transform.rotation);
		}
	}
}
