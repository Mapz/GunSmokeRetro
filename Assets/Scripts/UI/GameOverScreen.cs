using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverScreen : MonoBehaviour {
    public Image Cursor;
    public List<Text> Selections;
    private int Selection;
    private RectTransform CursorTransform;
    private bool IsSelected = false;

    void Awake () {
        CursorTransform = Cursor.GetComponent<RectTransform> ();
        ChangeSelection (0);
        Utility.Fade (true);
    }

    void ChangeSelection (int selection) {
        if (selection >= Selections.Count) {
            selection = 0;
        }
        CursorTransform.anchoredPosition = new Vector2 (CursorTransform.anchoredPosition.x, Selections[selection].GetComponent<RectTransform> ().anchoredPosition.y);
        Selection = selection;
    }

    void Update () {
        if (IsSelected) return;
        if (Input.GetKeyDown ("v")) {
            ChangeSelection (++Selection);
        } else if (Input.GetKeyDown ("b")) {
            Select ();
        }
    }

    void Select () {
        IsSelected = true;
        if (Selections[Selection].gameObject.name == "ContinueText") {
            GameVars.ContinueSetVars ();
            Utility.Fade (false, () => {
                GameVars.Game.SetGameState (GameState.WantedScreen);
                Destroy (this.gameObject);
            });
        } else if (Selections[Selection].gameObject.name == "RetryText") {
            GameVars.ResetVars ();
            Utility.Fade (false, () => {
                GameVars.Game.SetGameState (GameState.TitleScreen);
                Destroy (this.gameObject);
            });
        }
    }
}