using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]private EnemyManager enemyManager;
        [SerializeField]private GameObject gameOverText;
        [SerializeField]private GameObject victoryText;
        [SerializeField]private GameObject homeButton;
        [SerializeField]private GameObject continueButton;
        [SerializeField]private GameObject retryButton;
        [SerializeField] private GameObject settingPopUp;
        [SerializeField] private GameObject settingButton;
        [SerializeField] private GameObject pauseButton;
        private GameObject joystick;
        private void Awake()
        {
            joystick = FindFirstObjectByType<PlayerCtrl>().Joystick.gameObject;
            // set active false
            victoryText.SetActive(false);
            gameOverText.SetActive(false);
            homeButton.SetActive(false);
            retryButton.SetActive(false);
            continueButton.SetActive(false);
            settingButton.SetActive(false);
            // Đặt Time.timeScale = 1 khi bắt đầu game
            Time.timeScale = 1;
        }
        private void Update()
        {
            if (enemyManager.countEnemy <= 0)
            {
                StartCoroutine(DelayVictory());
            }
        }
        private void Victory()
        {
            Time.timeScale = 0;
            joystick.SetActive(false);
            victoryText.SetActive(true);
            homeButton.SetActive(true);
            retryButton.SetActive(true);
            settingButton.SetActive(true);
            pauseButton.SetActive(false);
            int nextLevel = SceneManager.GetActiveScene().buildIndex  + 1;
            if(PlayerPrefs.GetInt("level", 1) < nextLevel )
            {
                PlayerPrefs.SetInt("level", nextLevel  );
                Debug.Log("Next level unlocked: " + nextLevel);
            }
            // scene 4 là scene cuối nên k được hiêjn continue
            if( SceneManager.GetActiveScene().buildIndex != 4 )
                continueButton.SetActive(true);
            else continueButton.SetActive(false);
            FindFirstObjectByType<AudioManager>().PlaySFX("victory");
            // timescale =0
            // set active true
        }
        public void GameOver()
        {
            Time.timeScale = 0;
            FindFirstObjectByType<AudioManager>().PlaySFX("youlose");
            // 
            joystick.SetActive(false);
            gameOverText.SetActive(true);
            homeButton.SetActive(true);
            retryButton.SetActive(true);
            settingButton.SetActive(true);
            continueButton.SetActive(false);
            pauseButton.SetActive(false);
            // set active true
            // time.timescale = 0 de dung thoi gian
        }

        public void Continue()
        {
            
            Time.timeScale = 1;
            // tiep tuc timescale= 1
            // load next level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        }

        public void Retry()
        {
            // timescale = 1 tiep tuc thoi gian
            // load lai scene nay
            SceneManager.LoadScene(1);
        }

        public void EnemyDefeated()
        {
            FindFirstObjectByType<AudioManager>().PlaySFX("enemyDeath");
            enemyManager.countEnemy--;
            enemyManager.enemyAlive--;
        }

        public void Pause()
        {
            // unpause
            if (Time.timeScale == 0)
            {
                FindFirstObjectByType<AudioManager>().PlaySFX("unpause");
                Time.timeScale = 1;
                joystick.SetActive(true);
                homeButton.SetActive(false);
                retryButton.SetActive(false);
                continueButton.SetActive(false);
                settingButton.SetActive(false);
            }
            else // pause
            {
                FindFirstObjectByType<AudioManager>().PlaySFX("pause");
                Time.timeScale = 0;
                joystick.SetActive(false);
                homeButton.SetActive(true);
                retryButton.SetActive(true);
                continueButton.SetActive(false);
                settingButton.SetActive(true);
            }
        }

        public void Home()
        {
            SceneManager.LoadScene(0);
        }

        public void Setting()
        {
            settingPopUp.SetActive(true);
            joystick.SetActive(false);
            // homeButton.SetActive(false);
            // retryButton.SetActive(false);
            // continueButton.SetActive(false);
            // settingButton.SetActive(false);
        }
        public void QuitSetting()
        {
            settingPopUp.SetActive(false);
        }
        IEnumerator DelayVictory()
        {
            yield return new WaitForSeconds(2);
            Victory();
        }
    }
}
