using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RemovableTiles : MonoBehaviour {
    // Start is called before the first frame update
    public Vector2 initialVelocity = new Vector2 (1.0f, 10.0f);
    public GameObject tilemapGameObject;

    private GridInformation gridInfo;

    private RaycastHit2D[] hitBuff = new RaycastHit2D[16];
    Tilemap tilemap;
    void Start () {

        // var rb = GetComponent<Rigidbody2D> ();
        // rb.velocity = initialVelocity.x * UnityEngine.Random.Range (-1f, 1f) * Vector3.right + initialVelocity.y * Vector3.down;
        if (tilemapGameObject != null) {
            tilemap = tilemapGameObject.GetComponent<Tilemap> ();
        }
        gridInfo = GetComponent<GridInformation> ();
    }

    // Update is called once per frame
    void Update () {

    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D (Collider2D other) {
        Debug.Log ("OnTriggerEnter2D");
        if (other.gameObject.tag == "Bullet") {
            if (tilemap != null) {
                BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
                if (bbh.team != Team.Team1) return;
                Rigidbody2D bbhRigd = bbh.GetComponent<Rigidbody2D> ();
                // 需要cast一下射线，找到接触点，否则找不到 tile cell
                int hitCount = bbh.GetComponent<Rigidbody2D> ().Cast (bbhRigd.velocity, hitBuff);
                if (hitCount > 0) {
                    for (int i = 0; i < hitCount; i++) {
                        RaycastHit2D hit = hitBuff[i];
                        Vector3Int positionInGrid = tilemap.WorldToCell (hit.point);
                        TileBase tile = tilemap.GetTile (positionInGrid);
                        if (tile) {
                            float HP = gridInfo.GetPositionProperty (positionInGrid, "HP", 10f);
                            HP -= bbh.damage;
                            gridInfo.SetPositionProperty (positionInGrid, "HP", HP);
                            // Debug.Log ("HP:" + HP);
                            ObjectMgr<BulletBehavior>.Instance.Destroy (bbh);
                            if (HP <= 0) {
                                tilemap.SetTile (positionInGrid, null);
                            }
                        };
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D (Collision2D other) {
        Debug.Log ("OnCollisionEnter2D");
    }
}