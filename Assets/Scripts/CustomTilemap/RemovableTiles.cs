using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RemovableTiles : MonoBehaviour {
    // Start is called before the first frame update
    public Vector2 initialVelocity = new Vector2 (1.0f, 10.0f);
    public GameObject tilemapGameObject;

    private GridInformation gridInfo;
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
        // Debug.Log ("OnTriggerEnter2D");
        if (other.gameObject.tag == "Bullet") {
            BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
            if (tilemap != null) {
                Vector3 point = other.gameObject.transform.position;
                Vector3Int positionInGrid = tilemap.WorldToCell (point);

                TileBase tile = tilemap.GetTile (positionInGrid);
                if (tile) {
                    // Debug.Log ("positionInGrid:" + positionInGrid);
                    float HP = gridInfo.GetPositionProperty (positionInGrid, "HP", 10f);
                    HP -= bbh.damage;
                    gridInfo.SetPositionProperty (positionInGrid, "HP", HP);
                    // Debug.Log ("HP:" + HP);
                    Destroy (other.gameObject);
                    if (HP <= 0) {
                        tilemap.SetTile (positionInGrid, null);
                    }
                };

            }
        }
    }

    private void OnCollisionEnter2D (Collision2D other) {
        Debug.Log ("OnCollisionEnter2D");
    }
}