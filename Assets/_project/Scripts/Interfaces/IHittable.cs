namespace Project.Interfaces {
    public interface IHittable {
        void OnHit(int shooterId, int weaponId, float damage);
    }
}