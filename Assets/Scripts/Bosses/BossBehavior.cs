using System.Collections;
using UnityEngine;
using Bullets;

[System.Serializable]
public class BossPhase
{
    public float healthThreshold;
    public GameObject[] attacks;
}

public class BossBehavior : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 1000f;

    [Header("Phases")]
    [SerializeField] private BossPhase[] phases;

    public float currentHealth;
    private int currentPhase;

    private GameObject currentAttack;
    private int lastAttackIndex = -1;

    private Coroutine attackCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentPhase = 0;

        attackCoroutine = StartCoroutine(AttackLoop());
    }

    private void Update()
    {
        CheckPhaseTransition();
    }

    private IEnumerator AttackLoop()
    {
        while (currentHealth > 0)
        {
            BossPhase phase = phases[currentPhase];

            if (phase.attacks == null || phase.attacks.Length == 0)
            {
                Debug.LogWarning(
                    $"La fase {currentPhase + 1} no tiene ataques asignados."
                );

                yield return null;
                continue;
            }

            int attackIndex = GetRandomAttackIndex(phase.attacks.Length);

            currentAttack = phase.attacks[attackIndex];

            if (currentAttack == null)
            {
                Debug.LogWarning(
                    $"El ataque {attackIndex} de la fase {currentPhase + 1} está vacío."
                );

                currentAttack = null;

                yield return null;
                continue;
            }

            lastAttackIndex = attackIndex;

            RadialShotWeapon weapon =
                currentAttack.GetComponentInChildren<RadialShotWeapon>(true);

            if (weapon == null)
            {
                Debug.LogWarning(
                    $"El GameObject '{currentAttack.name}' no tiene un " +
                    $"RadialShotWeapon en él ni en sus hijos."
                );

                currentAttack = null;

                yield return null;
                continue;
            }

            float attackDuration = weapon.GetPatternDuration();

            Debug.Log(
                $"Activando ataque: {currentAttack.name} " +
                $"durante {attackDuration} segundos."
            );

            currentAttack.SetActive(true);

            yield return new WaitForSeconds(attackDuration);

            if (currentAttack != null)
            {
                currentAttack.SetActive(false);
            }

            currentAttack = null;
        }
    }

    private int GetRandomAttackIndex(int attackCount)
    {
        if (attackCount <= 1)
            return 0;

        int index;

        do
        {
            index = Random.Range(0, attackCount);
        }
        while (index == lastAttackIndex);

        return index;
    }

    private void CheckPhaseTransition()
    {
        if (currentPhase >= phases.Length - 1)
            return;

        if (currentHealth <= phases[currentPhase + 1].healthThreshold)
        {
            ChangePhase(currentPhase + 1);
        }
    }

    private void ChangePhase(int newPhase)
    {
        if (currentAttack != null)
        {
            currentAttack.SetActive(false);
            currentAttack = null;
        }

        currentPhase = newPhase;
        lastAttackIndex = -1;

        Debug.Log($"Boss ha entrado en la fase {currentPhase + 1}");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        if (currentAttack != null)
        {
            currentAttack.SetActive(false);
        }

        Debug.Log("Boss derrotado");
    }
}