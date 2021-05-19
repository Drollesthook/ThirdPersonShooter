namespace Project.Interfaces {
    public interface IHittable {
        void OnHit(int shooterId, int shootersFractionId, int weaponId, float damage);
    }
}