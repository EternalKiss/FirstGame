namespace FirstGame.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        bool IsAlive { get; }
    }
}
