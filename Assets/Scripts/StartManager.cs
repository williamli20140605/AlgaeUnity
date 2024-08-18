using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject enemySpawnerGameObject;
    public GameObject player;
    public Canvas phase1;
    public Canvas phase2;
    public Canvas phase3;
    private CheatManager cheatManager;

    public enum Phases
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3
    }

    public Phases phase;
    public Phases lastPhase;
    public bool isShowingPhases;

    private PlayerController playerController;
    private EnemySpawner enemySpawner;
    private BossMovement bossMovement;
    private BossHealth bossHealth;
    
    public bool isStarted = false;
    public Canvas Play;
    public Canvas HomePage;
    public Canvas Score;
    private bool HomePageEnabled = true;
    public Canvas GameMenu;
    public Canvas GameOverLose;
    public Canvas GameOverWin;

    public bool infiniteMode = false;

    public enum GameOverType
    {
        Win,
        Lose
    }
    public GameOverType gameOverType;

    void Start()
    {
        cheatManager = GameObject.Find("CheatManager").GetComponent<CheatManager>();
        Score.enabled = false;
        GameMenu.enabled = false;
        Play.enabled = false;
        HomePage.enabled = true;
        playerController = player.GetComponent<PlayerController>();
        enemySpawner = enemySpawnerGameObject.GetComponent<EnemySpawner>();
        bossHealth = boss.GetComponent<BossHealth>();
        bossMovement = boss.GetComponent<BossMovement>();
        GameOverLose.enabled = false;
        GameOverWin.enabled = false;

        // Make sure the phase canvases are initially disabled
        phase1.enabled = false;
        phase2.enabled = false;
        phase3.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleGameMenu();
        }

        if (cheatManager.isCheating)
        {
            DestroyTaggedObjects("Enemy");
        }

        if (infiniteMode == true)
        {
            bossMovement.speed = 4.5f + (ScoreManager.instance.score / 25);
            if (bossMovement.speed >= 8.5f)
            {
                bossMovement.speed = 8.5f;
            }
            enemySpawner.spawnInterval = 5f - ScoreManager.instance.score / 100;
            if (enemySpawner.spawnInterval <= 0.15f)
            {
                enemySpawner.spawnInterval = 0.15f;
            }
            playerController.speed = 10f + (ScoreManager.instance.score / 100);
            if (playerController.speed >= 20f)
            {
                playerController.speed = 20f;
            }
            playerController.shootCooldown = 0.2f - (ScoreManager.instance.score / 50);
            if (playerController.shootCooldown <= 0.03f)
            {
                playerController.shootCooldown = 0.03f;
            }
        }
        else
        {
            if (isStarted == true)
            {
                if (phase == Phases.Phase0 && bossHealth.currentHealth <= 250)
                {
                    phase = Phases.Phase1;
                    StartCoroutine(ShowPhase(phase1));
                    lastPhase = Phases.Phase1;
                    enemySpawner.spawnInterval = 5f;
                    bossMovement.speed = 4.5f;
                }
                else if (phase == Phases.Phase1 && bossHealth.currentHealth <= 150)
                {
                    phase = Phases.Phase2;
                    bossHealth.currentHealth = 200;
                    bossHealth.UpdateHealthUI();
                    StartCoroutine(ShowPhase(phase2));
                    lastPhase = Phases.Phase2;
                    enemySpawner.spawnInterval = 2.5f;
                    bossMovement.speed = 7.5f;
                }
                else if (phase == Phases.Phase2 && bossHealth.currentHealth <= 50)
                {
                    phase = Phases.Phase3;
                    bossHealth.UpdateHealthUI();
                    bossHealth.currentHealth = 100;
                    StartCoroutine(ShowPhase(phase3));
                    lastPhase = Phases.Phase3;
                    enemySpawner.spawnInterval = 1.5f;
                    bossMovement.speed = 15f;
                }
            }
        }
    }

    private void ToggleGameMenu()
    {
        if (HomePageEnabled)
        {
            return;
        }
        if (isStarted && GameOverLose.enabled == false && GameOverWin.enabled == false)
        {
            isStarted = false;
            GameMenu.enabled = true;
        }
        else if (isStarted == false && GameOverLose.enabled == false && GameOverWin.enabled == false)
        {
            isStarted = true;
            GameMenu.enabled = false;
        }
    }

    public void OnStartButtonClick()
    {
        Score.enabled = true;
        infiniteMode = false;
        lastPhase = Phases.Phase0;
        Play.enabled = true;
        isStarted = true;
        HomePage.enabled = false;
        HomePageEnabled = false;
        GameOverLose.enabled = false;
        GameOverWin.enabled = false;
    }

    public void OnEscButtonClick()
    {
        ToggleGameMenu();
    }

    public void ResetGame()
    {
        Vector3 bossPosition = new Vector3(0, 3.2f, 5);
        Vector3 playerPosition = new Vector3(0, -3, 5);
        bossMovement.transform.position = bossPosition;
        bossMovement.SetNewTargetPosition();
        boss.SetActive(true);
        bossHealth.isBossAlive = true;
        bossHealth.currentHealth = bossHealth.maxHealth;
        bossHealth.UpdateHealthUI();
        playerController.transform.position = playerPosition;
        enemySpawner.spawnInterval = 5f;
        bossMovement.speed = 4.5f;
        enemySpawner.spawnTimer = enemySpawner.spawnInterval;
        ScoreManager.instance.ResetScore();
        GameOverLose.enabled = false;
        GameOverWin.enabled = false;
        phase1.enabled = false;
        phase2.enabled = false;
        phase3.enabled = false;
        phase = Phases.Phase0;
        
        // Destroy existing enemies and bullets
        DestroyTaggedObjects("Enemy");
        DestroyTaggedObjects("Bullet");
    }

    private void DestroyTaggedObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    public void OnQuitButtonClick()
    {
        Score.enabled = false;
        ResetGame();
        lastPhase = Phases.Phase0;
        HomePage.enabled = true;
        Play.enabled = false;
        GameMenu.enabled = false;
        isStarted = false;
        HomePageEnabled = true;
    }

    public void OnRestartButtonClick()
    {
        Score.enabled = true;
        ResetGame();
        phase = lastPhase;
        isStarted = true;
        GameMenu.enabled = false;
        HomePageEnabled = false;

        if (phase == Phases.Phase1)
        {
            phase = Phases.Phase1;
            StartCoroutine(ShowPhase(phase1));
            lastPhase = Phases.Phase1;
            enemySpawner.spawnInterval = 5f;
            bossMovement.speed = 4.5f;
        }
        else if (phase == Phases.Phase2)
        {
            phase = Phases.Phase2;
            bossHealth.currentHealth = 200;
            bossHealth.UpdateHealthUI();
            StartCoroutine(ShowPhase(phase2));
            lastPhase = Phases.Phase2;
            enemySpawner.spawnInterval = 2.5f;
            bossMovement.speed = 7.5f;
        }
        else if (phase == Phases.Phase3)
        {
            phase = Phases.Phase3;
            bossHealth.UpdateHealthUI();
            bossHealth.currentHealth = 100;
            StartCoroutine(ShowPhase(phase3));
            lastPhase = Phases.Phase3;
            enemySpawner.spawnInterval = 1.5f;
            bossMovement.speed = 15f;
        }
    }

    public void OnInfiniteButtonClick()
    {
        Score.enabled = true;
        infiniteMode = true;
        Play.enabled = true;
        isStarted = true;
        HomePage.enabled = false;
        HomePageEnabled = false;
        GameOverLose.enabled = false;
        GameOverWin.enabled = false;
    }

    public void OnGameOver()
    {
        isStarted = false;
        if (gameOverType == GameOverType.Lose)
        {
            GameOverLose.enabled = true;
        }
        else if (gameOverType == GameOverType.Win)
        {
            lastPhase = Phases.Phase0;
            phase = Phases.Phase0;
            GameOverWin.enabled = true;
        }
        GameMenu.enabled = true;
    }

    private IEnumerator ShowPhase(Canvas phaseCanvas)
    {
        DestroyTaggedObjects("Enemy");
        isStarted = false;
        isShowingPhases = true;
        phaseCanvas.enabled = true;
        yield return new WaitForSeconds(2f);
        isStarted = true;
        phaseCanvas.enabled = false;
        isShowingPhases = false;
        DestroyTaggedObjects("Enemy");
    }
}
