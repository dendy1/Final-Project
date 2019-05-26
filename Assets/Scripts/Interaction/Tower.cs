using UnityEngine;

public class Tower : Interactable
{
    private TowerStats _stats;

    private float _repairCD;

    public override void Interact(Transform target)
    {
        base.Interact(target);

        if (target.CompareTag("Player") && _repairCD <= 0)
        {
            Repair(target);
            _repairCD = _stats.RepairCD;
        }
    }
    
    public override void Initialize()
    {
        _stats = GetComponent<TowerStats>();
    }

    public void Repair(Transform player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        _stats.Repair(playerStats.Repair);
    }

    private void FixedUpdate()
    {
        _repairCD -= Time.deltaTime;
    }
}
