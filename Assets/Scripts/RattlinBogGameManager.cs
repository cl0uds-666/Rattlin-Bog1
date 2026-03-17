using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RattlinBogGameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text selectionPreviewText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private WordCardButton cardPrefab;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button restartButton;

    [Header("Optional Settings")]
    [SerializeField] private bool shuffleCardsOnNewGame = true;

    private readonly List<string> chain = new()
    {
        "bog", "hole", "seed", "tree", "branch",
        "twig", "leaf", "nest", "egg", "bird",
        "feather", "flea", "leg", "boot", "lace"
    };

    private readonly List<WordCardButton> cardInstances = new();
    private readonly List<string> selectedSequence = new();

    private int currentRound;
    private int highestSuccessfulRound;
    private bool gameOver;

    private void Awake()
    {
        submitButton.onClick.AddListener(SubmitRound);
        clearButton.onClick.AddListener(ClearSelection);
        restartButton.onClick.AddListener(StartNewGame);

        BuildCards();
    }

    private void Start()
    {
        StartNewGame();
    }

    public void OnCardClicked(WordCardButton card)
    {
        if (gameOver)
        {
            return;
        }

        if (selectedSequence.Count >= currentRound)
        {
            instructionText.text = $"Round {currentRound}: You already picked {currentRound} card(s). Tap Submit or Clear.";
            return;
        }

        selectedSequence.Add(card.Word);
        card.SetInteractable(false);
        UpdateSelectionPreview();
    }

    private void BuildCards()
    {
        List<string> wordsToBuild = new(chain);

        if (shuffleCardsOnNewGame)
        {
            Shuffle(wordsToBuild);
        }

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        cardInstances.Clear();

        foreach (string word in wordsToBuild)
        {
            WordCardButton newCard = Instantiate(cardPrefab, cardContainer);
            newCard.Initialize(word, this);
            cardInstances.Add(newCard);
        }
    }

    private void StartNewGame()
    {
        if (shuffleCardsOnNewGame)
        {
            BuildCards();
        }
        else
        {
            foreach (WordCardButton card in cardInstances)
            {
                card.SetInteractable(true);
            }
        }

        currentRound = 1;
        highestSuccessfulRound = 0;
        gameOver = false;
        selectedSequence.Clear();

        restartButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(true);
        clearButton.gameObject.SetActive(true);

        resultText.text = string.Empty;
        UpdateRoundUI();
        UpdateSelectionPreview();
    }

    private void SubmitRound()
    {
        if (gameOver)
        {
            return;
        }

        if (selectedSequence.Count != currentRound)
        {
            resultText.text = $"Pick {currentRound} card(s) before submitting.";
            return;
        }

        bool isCorrect = true;
        for (int i = 0; i < currentRound; i++)
        {
            if (selectedSequence[i] != chain[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (!isCorrect)
        {
            gameOver = true;
            instructionText.text = "Round failed.";
            resultText.text = $"You fucked the bog. Highest successful round: {highestSuccessfulRound}";
            restartButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
            clearButton.gameObject.SetActive(false);
            SetAllCardsInteractable(false);
            return;
        }

        highestSuccessfulRound = currentRound;

        if (currentRound >= chain.Count)
        {
            gameOver = true;
            instructionText.text = "You completed the full bog chain!";
            resultText.text = "Legend. You rattled the whole bog.";
            restartButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
            clearButton.gameObject.SetActive(false);
            SetAllCardsInteractable(false);
            return;
        }

        currentRound++;
        selectedSequence.Clear();
        SetAllCardsInteractable(true);
        resultText.text = "Correct! Next round.";

        UpdateRoundUI();
        UpdateSelectionPreview();
    }

    private void ClearSelection()
    {
        if (gameOver)
        {
            return;
        }

        selectedSequence.Clear();
        SetAllCardsInteractable(true);
        resultText.text = "Selection cleared.";
        UpdateSelectionPreview();
    }

    private void UpdateRoundUI()
    {
        roundText.text = $"Round: {currentRound}/{chain.Count}";
        instructionText.text = $"Tap cards in order from bog. This round needs {currentRound} card(s).";
    }

    private void UpdateSelectionPreview()
    {
        if (selectedSequence.Count == 0)
        {
            selectionPreviewText.text = "Selected: (none)";
            return;
        }

        selectionPreviewText.text = "Selected: " + string.Join(" → ", selectedSequence);
    }

    private void SetAllCardsInteractable(bool interactable)
    {
        foreach (WordCardButton card in cardInstances)
        {
            card.SetInteractable(interactable);
        }
    }

    private static void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
