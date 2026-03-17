using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordCardButton : MonoBehaviour
{
    [SerializeField] private TMP_Text wordLabel;
    [SerializeField] private Button button;

    public string Word { get; private set; }

    private RattlinBogGameManager gameManager;

    public void Initialize(string word, RattlinBogGameManager manager)
    {
        Word = word;
        gameManager = manager;

        wordLabel.text = word;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(HandleClick);
        button.interactable = true;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    private void HandleClick()
    {
        gameManager.OnCardClicked(this);
    }
}
