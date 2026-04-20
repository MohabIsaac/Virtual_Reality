using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bowling
{
    /// <summary>
    /// Manages the total score of the bowling game and updates the UI.
    /// Follows a Singleton pattern for easy access and robust registration.
    /// </summary>
    public class BowlingScoreManager : MonoBehaviour
    {
        public static BowlingScoreManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField, Tooltip("TextMeshPro component to display the current score.")]
        private TextMeshProUGUI scoreText;

        [Header("Settings")]
        [SerializeField, Tooltip("Prefix for the score display.")]
        private string scorePrefix = "Score: ";

        private int _currentScore;
        private List<BowlingPin> _registeredPins = new List<BowlingPin>();

        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            UpdateScoreUI();
        }

        /// <summary>
        /// Registers a pin so it can be reset later.
        /// </summary>
        public void RegisterPin(BowlingPin pin)
        {
            if (!_registeredPins.Contains(pin))
            {
                _registeredPins.Add(pin);
            }
        }

        /// <summary>
        /// Increments the score and updates the UI.
        /// </summary>
        public void IncrementScore()
        {
            _currentScore++;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = $"{scorePrefix}{_currentScore}";
            }
        }

        /// <summary>
        /// Resets the total score and physically resets all registered pins.
        /// </summary>
        public void ResetGame()
        {
            _currentScore = 0;
            UpdateScoreUI();

            foreach (var pin in _registeredPins)
            {
                pin.ResetToInitialPose();
            }
        }
    }
}
